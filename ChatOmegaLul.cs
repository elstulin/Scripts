
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.Xml;
using System.IO;
using TMPro;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.Mail;
public class ChatOmegaLul : NetworkBehaviour
{

    public ChatUIController chatUiController;
    public Dropdown ChannelDropdown;
    public GameObject chatPanel;
    public InputField inputField;
    public Text inputFieldPlaceholder;
    public TMP_InputField chatText;
    public List<ChatUIController.ChatText> chatTextList = new List<ChatUIController.ChatText>();
    public static bool active = false;
    public MegaEventController megaEvent;
    bool trig;
    int fileCount;
    string channel = "";
    string predictChannel = "";
    public static List<string> blockList;
    public static List<string> lastMessageArray = new List<string>();
    int currentLastMessageIndex;
    public static ChatOmegaLul instance;
    public struct MailRequestMessage : NetworkMessage
    {
        public string email;
    }
    private void OnEnable()
    {
        InstPlayer.OnPlayerInstanceEvent += OnPlayerInstance;
        NetworkManager.OnServerStart += OnServerStart;
        NetworkManager.OnServerStop += OnServerStop;
    }
    private void OnDisable()
    {
        InstPlayer.OnPlayerInstanceEvent -= OnPlayerInstance;
    }
    void Start()
    {
        Resources.UnloadUnusedAssets();
        inputFieldPlaceholder = inputField.placeholder.GetComponent<Text>();
        blockList = new List<string>(InterfaceSettings.blockList);
        if (!instance)
            instance = this;

    }
    public static string UseRegularExpression(string input)
    {
        var result = Regex.Replace(input, "<.*?>", string.Empty);
        return result;
    }
    public static string UseHtmlDecode(string input)
    {
        var result = UseRegularExpression(input);
        result = WebUtility.HtmlDecode(result);
        return result;
    }
    void OnServerStart()
    {
        NetworkServer.RegisterHandler<MailRequestMessage>(OnMailRequestMessage, false);
    }
    void OnServerStop()
    {
        NetworkServer.UnregisterHandler<MailRequestMessage>();
    }
    void OnPlayerInstance()
    {
        trig = true;
    }
    [ClientCallback]
    void Update()
    {
        if (trig)
        {

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (inputField.text == "")
                {
                    active = !active;
                    if (active)
                    {

                    }
                    else
                    {
                        // GOManager.playerSS.moveControll.CmdVoiceOff();
                        // GOManager.playerSS.moveControll.animator.SetBool("Dialogue", false);
                        StopCoroutine(GOManager.playerSS.moveControll.DialogueRnd());
                    }
                }
                if (active)
                {
                    if (inputField.text.Length > 0)
                    {

                        if (ChannelDropdown.value == 0)
                            channel = "/рядом ";
                        if (ChannelDropdown.value == 1)
                            channel = "/крик ";
                        if (ChannelDropdown.value == 2)
                            channel = "/действие ";
                        if (ChannelDropdown.value == 3)
                            channel = "/общий ";
                        if (ChannelDropdown.value == 4)
                            channel = "/лс ";
                        if (ChannelDropdown.value == 5)
                            channel = "/группа ";
                        if (ChannelDropdown.value == 6)
                            channel = "/гильдия ";

                        string value1 = "";
                        string value2 = "";
                        string value3 = "";
                        SplitText(inputField.text, ref value1, ref value2, ref value3);
                        if (inputField.text[0].ToString() == "/") channel = "";
                        if (value1 == "/dnd")
                        {
                            ClickChannel1(4);
                            bool DND = chatUiController.channelsActive[4];
                            if (!DND)
                            {
                                chatUiController.buttonsChannelsSelect[4].SetActive(false);
                                chatUiController.buttonsChannelsUnselect[4].SetActive(true);
                                TakeSystemMessage("<color=#ff0008>[Система]: Режим не беспокоить включен</color>");
                            }
                            else
                            {
                                chatUiController.buttonsChannelsSelect[4].SetActive(true);
                                chatUiController.buttonsChannelsUnselect[4].SetActive(false);
                                TakeSystemMessage("<color=#ff0008>[Система]: Режим не беспокоить выключен</color>");
                            }
                        }
                        else
                            if (value1 == "/заблокировать")
                        {
                            if (value2.Length > 0)
                            {
                                if (blockList.IndexOf(value2) == -1)
                                {
                                    blockList.Add(value2);
                                    InterfaceSettings.SaveChatSettings((int)chatUiController.rectTransform.anchoredPosition.x, (int)chatUiController.rectTransform.anchoredPosition.y, (int)chatUiController.rectTransform.sizeDelta.x, (int)chatUiController.rectTransform.sizeDelta.y, chatUiController.channelsActive, blockList.ToArray());
                                    TakeSystemMessage("<color=#ff0008>[Система]: Вы добавили игрока: " + value2 + " в чёрный список</color>");
                                }
                                else
                                {
                                    TakeSystemMessage("<color=#ff0008>[Система]: Игрок: " + value2 + " уже находится в чёрном списке</color>");
                                }
                            }
                        }
                        else
                            if (value1 == "/разблокировать")
                        {
                            if (value2.Length > 0)
                            {
                                if (blockList.IndexOf(value2) > -1)
                                {
                                    blockList.Remove(value2);
                                    InterfaceSettings.SaveChatSettings((int)chatUiController.rectTransform.anchoredPosition.x, (int)chatUiController.rectTransform.anchoredPosition.y, (int)chatUiController.rectTransform.sizeDelta.x, (int)chatUiController.rectTransform.sizeDelta.y, chatUiController.channelsActive, blockList.ToArray());
                                    TakeSystemMessage("<color=#ff0008>[Система]: Вы убрали игрока: " + value2 + " из чёрного списка из</color>");
                                }
                                else
                                {
                                    TakeSystemMessage("<color=#ff0008>[Система]: Игрок: " + value2 + " не находится в чёрном списке</color>");
                                }
                            }
                        }
                        else
                            if (value1 == "/создание")
                        {
                            CharacterChanger.instance.SwitchActive();
                            active = false;
                            chatPanel.SetActive(false);
                        }
                        else
                            if (value1 == "/телепорт")
                        {
                            if (value2 == "хоринис")
                            {
                                GOManager.playerSS.Teleportation(new Vector3(-720.37f, -43.83f, -98.47f), -90f);
                            }
                            else if (value2 == "лендлорд")
                            {
                                GOManager.playerSS.Teleportation(new Vector3(-1469.679f, -8.579733f, 36.47685f), 180);
                            }
                            else if (value2 == "ксардас")
                            {
                                GOManager.playerSS.Teleportation(new Vector3(-1030.521f, 1.027277f, 77.16867f), 0);
                            }
                            else if (value2 == "монастырь")
                            {
                                GOManager.playerSS.Teleportation(new Vector3(-1186.844f, 5.127534f, -299.0494f), -120);
                            }
                            else if (value2 == "долина")
                            {
                                GOManager.playerSS.Teleportation(new Vector3(-1117.844f, -15.49133f, 125.3682f), 30);
                            }
                        }
                        else
                            if (value1 == "/танец")
                        {
                            int danceNum;
                            if (int.TryParse(value2, out danceNum) && danceNum > 0 && danceNum < 10)
                            {
                                if (!GOManager.playerSS.moveControll.dance)
                                {
                                    GOManager.playerSS.moveControll.dance = true;
                                    GOManager.playerSS.animator.SetInteger("DanceNum", danceNum);
                                    GOManager.playerSS.animator.SetBool("Dance", true);
                                    active = false;
                                    chatPanel.SetActive(false);
                                    CmdSendMessageInGlobalChat("/действие танцует");
                                }

                            }
                            else
                            {
                                TakeSystemMessage("<color=#ff0008>[Система]: Введите число от 1 до 9</color>");
                            }

                        }
                        else
                            CmdSendMessageInGlobalChat(channel + inputField.text);

                        lastMessageArray.Add(inputField.text);
                        currentLastMessageIndex = lastMessageArray.Count - 1;
                        //  CmdCreateWorldChatText(inputField.text, ChannelDropdown.value,GOManager.playerSS.netIdentity.netId);
                        inputField.text = "";
                    }
                }
                else
                {
                    if (inputField.text.Length > 0)
                    {
                        //GOManager.playerSS.moveControll.animator.SetBool("Dialogue", false);
                        StopCoroutine(GOManager.playerSS.moveControll.DialogueRnd());
                        //GOManager.playerSS.moveControll.CmdVoiceOffDelay();

                    }
                }
                chatPanel.SetActive(active);
                if (active)
                {
                    inputField.ActivateInputField();
                }

            }
            else
            {
                if (active)
                {
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {

                        inputField.text = lastMessageArray[currentLastMessageIndex];
                        currentLastMessageIndex++;
                        if (currentLastMessageIndex > lastMessageArray.Count - 1) currentLastMessageIndex = 0;
                    }
                    else
                        if (Input.GetKeyDown(KeyCode.DownArrow))
                    {

                        inputField.text = lastMessageArray[currentLastMessageIndex];
                        currentLastMessageIndex--;
                        if (currentLastMessageIndex < 0) currentLastMessageIndex = lastMessageArray.Count - 1;
                    }
                    inputFieldPlaceholder.enabled = true;
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        string text = inputField.text.ToLowerInvariant();
                        if (predictChannel.Length > 0)
                        {
                            inputField.text = predictChannel;
                            text = predictChannel;
                        }
                        if (text == "/рядом")
                        {
                            inputField.text = "";
                            ChannelDropdown.value = 0;
                        }
                        if (text == "/крик")
                        {
                            inputField.text = "";
                            ChannelDropdown.value = 1;
                        }
                        if (text == "/действие")
                        {
                            inputField.text = "";
                            ChannelDropdown.value = 2;
                        }
                        if (text == "/общий")
                        {
                            inputField.text = "";
                            ChannelDropdown.value = 3;
                        }
                        if (text == "/лс")
                        {
                            inputField.text = "";
                            ChannelDropdown.value = 4;
                        }
                        if (text == "/группа")
                        {
                            inputField.text = "";
                            ChannelDropdown.value = 5;
                        }
                        if (text == "/гильдия")
                        {
                            inputField.text = "";
                            ChannelDropdown.value = 6;
                        }
                    }
                    if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKeyDown(KeyCode.Mouse1) && !Input.GetKeyDown(KeyCode.Mouse2))
                    {
                        GOManager.playerSS.moveControll.stats.SetBoolAsTrigger("Dialogue");
                        StartCoroutine(GOManager.playerSS.moveControll.DialogueRnd());
                        GOManager.playerSS.moveControll.CmdVoiceOn();
                        GOManager.playerSS.moveControll.CmdVoiceOffDelay();
                        inputField.text = UseHtmlDecode(inputField.text);
                        string text = inputField.text.ToLowerInvariant();
                        if (text == "/т" || text == "/те" || text == "/тел" || text == "/теле" || text == "/телеп" || text == "/телепо" || text == "/телепор")
                        {
                            predictChannel = "/телепорт";
                            inputFieldPlaceholder.text = predictChannel;

                        }
                        else
                        if (text == "/телепорт ")
                        {
                            predictChannel = "/телепорт хоринис";
                            inputFieldPlaceholder.text = predictChannel;

                        }
                        else
                        if (text == "/телепорт х" || text == "/телепорт хо" || text == "/телепорт хор" || text == "/телепорт хори" || text == "/телепорт хорин" || text == "/телепорт хорини")
                        {
                            predictChannel = "/телепорт хоринис";
                            inputFieldPlaceholder.text = predictChannel;

                        }
                        else
                        if (text == "/телепорт л" || text == "/телепорт ле" || text == "/телепорт ленд" || text == "/телепорт лендл" || text == "/телепорт лендло" || text == "/телепорт лендлор")
                        {
                            predictChannel = "/телепорт лендлорд";
                            inputFieldPlaceholder.text = predictChannel;

                        }
                        else
                        if (text == "/телепорт к" || text == "/телепорт кс" || text == "/телепорт кса" || text == "/телепорт ксар" || text == "/телепорт ксард" || text == "/телепорт ксарда")
                        {
                            predictChannel = "/телепорт ксардас";
                            inputFieldPlaceholder.text = predictChannel;

                        }
                        else
                        if (text == "/телепорт м" || text == "/телепорт мо" || text == "/телепорт мон" || text == "/телепорт мона" || text == "/телепорт монас" || text == "/телепорт монаст" || text == "/телепорт монасты" || text == "/телепорт монастыр")
                        {
                            predictChannel = "/телепорт монастырь";
                            inputFieldPlaceholder.text = predictChannel;

                        }
                        else
                        if (text == "/телепорт д" || text == "/телепорт до" || text == "/телепорт дол" || text == "/телепорт доли" || text == "/телепорт долин")
                        {
                            predictChannel = "/телепорт долина";
                            inputFieldPlaceholder.text = predictChannel;

                        }
                        else if (text == "/р" || text == "/ря" || text == "/ряд" || text == "/рядо")
                        {
                            predictChannel = "/рядом";
                            inputFieldPlaceholder.text = predictChannel;

                        }
                        else if (text == "/к" || text == "/кр" || text == "/кри")
                        {
                            predictChannel = "/крик";
                            inputFieldPlaceholder.text = predictChannel;

                        }
                        else if (text == "/д" || text == "/де" || text == "/дей" || text == "/дейс" || text == "/действ" || text == "/действи")
                        {
                            predictChannel = "/действие";
                            inputFieldPlaceholder.text = predictChannel;

                        }
                        else if (text == "/о" || text == "/об" || text == "/общ" || text == "/общи")
                        {
                            predictChannel = "/общий";
                            inputFieldPlaceholder.text = predictChannel;

                        }
                        else if (text == "/л")
                        {
                            predictChannel = "/лс";
                            inputFieldPlaceholder.text = predictChannel;

                        }
                        else if (text == "/г" || text == "/гр" || text == "/гру" || text == "/груп" || text == "/групп")
                        {
                            predictChannel = "/группа";
                            inputFieldPlaceholder.text = predictChannel;

                        }
                        else if (text == "/г" || text == "/ги" || text == "/гил" || text == "/гиль" || text == "/гильд" || text == "/гильди")
                        {
                            predictChannel = "/гильдия";
                            inputFieldPlaceholder.text = predictChannel;

                        }
                        else if (text == "/з" || text == "/за" || text == "/заб" || text == "/забл" || text == "/заблок" || text == "/заблоки" || text == "/заблокир" || text == "/заблокиро" || text == "/заблокиров" || text == "/заблокирова" || text == "/заблокироват")
                        {
                            predictChannel = "/заблокировать";
                            inputFieldPlaceholder.text = predictChannel;

                        }
                        else if (text == "/d" || text == "/dn")
                        {
                            predictChannel = "/dnd";
                            inputFieldPlaceholder.text = predictChannel;

                        }
                        else
                        {
                            predictChannel = "";
                            inputFieldPlaceholder.text = "";
                        }


                    }

                }
            }

        }

    }


    public void CreateWorldChatText(uint id, string text)
    {
        if (id == uint.MaxValue) return;
        NetworkIdentity[] go = GameObject.FindObjectsOfType<NetworkIdentity>();
        for (int i = 0; i < go.Length; i++)
        {
            NetworkIdentity networkIdentity = go[i];
            if (networkIdentity.netId == id && Vector3.Distance(GOManager.playerTransform.position, networkIdentity.transform.position) < 20)
            {
                GameObject go1 = Instantiate(Resources.Load("ChatTextWorld"), GOManager.Canvas) as GameObject;
                WorldChatTextController worldChatTextController = go1.GetComponent<WorldChatTextController>();
                worldChatTextController.text.text = text;
                worldChatTextController.targetTransform = go[i].transform;
                break;
            }
        }


    }
    [Command(requiresAuthority = false)]
    public void CmdSendMessageInGlobalChat(string text, NetworkConnectionToClient conn = null)
    {
        StatsSystem ss = conn.identity.GetComponent<StatsSystem>();
        string login = ss._name;
        Vector3 pos = ss._transform.position;
        if (text[0].ToString() == "/")
        {
            string value1 = "";
            string value2 = "";
            string value3 = "";
            SplitText(text, ref value1, ref value2, ref value3);
            string resultText = value2;
            if (value3.Length > 0)
                resultText += " " + value3;
            //Debug.Log(value1 + "+" + value2 + "+" + value3);
            value1 = value1.ToLowerInvariant();
            if (value1 == "/spawn")
            {
                GameObject go = Resources.Load("Items/WorldItems/" + resultText) as GameObject;
                if (!go)
                    go = Resources.Load("NPC/" + resultText) as GameObject;
                if (go)
                {
                    
                    NetworkServer.Spawn(Instantiate(go, pos + ss._transform.forward, Quaternion.Euler(0, 0, 0)));
                }
                

            }
            else
            if (value1 == "/itemlist")
            {
                string preparedItemList = "chicken leg piece\r\nElegantSword\r\nItFo_Potion_110\r\nOW_MUSHROOM_V2\r\nSword\r\nSword 1\r\nАлебарда\r\nБоевой топор\r\nБулава\r\nВоенная дубина\r\nВоенный меч орков\r\nГрубый длинный меч\r\nДвойной топор\r\nДвуручный рудный меч\r\nДоспехи Стража\r\nДоспехи из пластин ползунов\r\nДоспехи кольца Воды\r\nДоспехи наемника\r\nДоспехи паладина\r\nДоспехи пирата\r\nДоспехи послушника\r\nДоспехи призрака\r\nДоспехи рыцаря\r\nДоспехи стражника\r\nДубина с шипами\r\nЗаряженный Уризель\r\nЗолотая монета\r\nИзысканный длинный меч\r\nИзысканный меч\r\nИзысканный полуторный меч\r\nКинжал\r\nКирка\r\nКоготь Белиара\r\nКожаные доспехи\r\nКольцо некроманта\r\nКороткий лук\r\nЛегкие доспехи наемника\r\nЛегкие доспехи послушника\r\nЛегкие доспехи стража\r\nЛегкие доспехи стражника\r\nЛегкое одеяние вора\r\nЛук мастера\r\nЛёгкие доспехи наёмника\r\nЛёгкие доспехи ополчения\r\nЛёгкие доспехи охотника на драконов\r\nМалая коса\r\nМантия мага Огня\r\nМеч\r\nМеч стражника\r\nМеч ящера\r\nНабедренная повязка послушника\r\nНезаряженный Уризель\r\nОблачение Гуру\r\nОблачение высших Гуру\r\nОблачение высших магов воды\r\nОблачение высших магов огня\r\nОблачение магов воды\r\nОблачение магов огня\r\nОдеяние вора\r\nОдеяние призрака\r\nОдеяние рудокопа\r\nПиратская абордажная сабля\r\nПростая одежда фермера\r\nПростые штаны для рудокопа\r\nРапира\r\nРжавый меч\r\nРоба послушника\r\nРуна огненная стрела\r\nРуна превращения в падальщика\r\nРуна превращения в снеппера\r\nРуна превращения в ящера\r\nРуна призыва демона\r\nРуна призыва малого скелета\r\nРуна призыва скелета\r\nРуна призыва скелета война\r\nРунный меч\r\nСвященный молот\r\nСерп\r\nСредние доспехи наёмника\r\nСредние доспехи охотника на драконов\r\nСтрела\r\nТопор\r\nТопор берсеркера\r\nТяжелая огненная мантия\r\nТяжелое одеяние вора\r\nТяжелые доспехи Барона\r\nТяжелые доспехи Стража\r\nТяжелые доспехи наемника\r\nТяжелые доспехи стражника\r\nТяжелый рудный боевой клинок орков\r\nТяжелый сук\r\nТяжёлые доспехи наёмника\r\nТяжёлые доспехи ополчения\r\nТяжёлые доспехи охотника на драконов\r\nУбийца орков\r\nУризель без камня\r\nШпага\r\nШтаны рудокопа\r\nЭссенция маны";
                if (value2 == "debug")
                {
                    string itemlist = "";
                    Object[] itemlistGo = Resources.LoadAll("Items/WorldItems", typeof(GameObject));
                    for (int i = 0; i < itemlistGo.Length; i++)
                    {
                        GameObject itemGo = (GameObject)itemlistGo[i];
                        itemlist += "\n" + itemGo.name;
                    }
                    TargetRpcSendMessageInGlobalChat(conn, "<color=#ff0008>[Система]: Список предметов:\n" +
                        itemlist + "</color>");
                    Resources.UnloadUnusedAssets();
                    Debug.Log(itemlist);
                }
                else
                {
                    TargetRpcSendMessageInGlobalChat(conn, "<color=#ff0008>[Система]: Список предметов:\n" +
                        preparedItemList + "</color>");
                }
            }
            else
                if (value1 == "/marvin")
            {

                if (!ss.god)
                {
                    ss.god = true;
                    ss.type = StatsSystem.Type.GodMode;
                    TargetRpcSendMessageInGlobalChat(conn, "<color=#ff0008>[Система]: Режим Бога включен</color>");
                }
                else
                {
                    ss.god = false;
                    ss.type = 0;
                    TargetRpcSendMessageInGlobalChat(conn, "<color=#ff0008>[Система]: Режим Бога выключен</color>");
                }

            }
            else
            if (value1 == "/death")
            {
                bool isGod = ss.god;
                if (isGod) ss.god = false;
                ss.TakeDamage(int.MaxValue, 0, 0, 0, false, ss.gameObject);
                if (isGod) ss.god = true;
            }
            else
            if (value1 == "/startevent")
            {
                megaEvent.StartEvent();
            }
            else
            if (value1 == "/лс")
            {
                TargetRpcSendMessageInGlobalChat(conn, "<color=#c539a3>вы пишите [" + value2 + "]: " + value3 + "</color>");

                RpcSendMessageInWhisperChat(conn.identity.netId, login, value3, value2);

            }
            else if (value1 == "/группа")
            {
                //Debug.Log(wName);
                RpcSendMessageInPartyChat(conn.identity.netId, login, resultText);
            }
            else if (value1 == "/гильдия")
            {
                //Debug.Log(wName);
                RpcSendMessageInGuildChat(conn.identity.netId, login, resultText);
            }
            else if (value1 == "/setguild")
            {
                //Debug.Log(wName);
                RpcSetGuild(uint.Parse(value2));
                TargetRpcSendMessageInGlobalChat(conn, "<color=#ff0008>[Система]: гильдия задана " + value2 + "</color>");
            }
            else if (value1 == "/setparty")
            {
                //Debug.Log(wName);
                RpcSetParty(uint.Parse(value2));
                TargetRpcSendMessageInGlobalChat(conn, "<color=#ff0008>[Система]: группа задана " + value2 + "</color>");
            }
            else if (value1 == "/рядом")
            {
                //Debug.Log(wName);
                RpcSendMessageInNearChat(conn.identity.netId, login, resultText, pos);
            }
            else if (value1 == "/крик")
            {
                //Debug.Log(wName);
                RpcSendMessageInCallChat(conn.identity.netId, login, resultText, pos);
            }
            else if (value1 == "/общий")
            {
                //Debug.Log(wName);
                RpcSendMessageInGeneralChat(conn.identity.netId, login, resultText);
            }
            else if (value1 == "/действие")
            {
                //Debug.Log(wName);
                RpcSendMessageInActionChat(conn.identity.netId, login, resultText, pos);
            }
            else if (value1 == "/мир")
            {
                //Debug.Log(wName);
                RpcSendMessageInGlobalChat(conn.identity.netId, login, resultText);

            }
            else
            {
                TargetRpcSendMessageInGlobalChat(conn, "<color=#ff0008>[Система]: Команды не существует</color>");
            }
        }

        //Debug.Log(login + text);
    }
    void SplitText(string text, ref string value1, ref string value2)
    {
        value1 = "";
        value2 = "";
        int space = 0;
        for (int i = 0; i < text.Length; i++)
        {

            string chr = text[i].ToString();
            if (chr == " ")
            {
                space++;
            }
            if (space >= 1)
            {
                value2 += chr;
            }
            else
            if (space >= 0)
            {
                value1 += chr;
            }
            value2 = value2.TrimStart();
        }
    }
    void SplitText(string text, ref string value1, ref string value2, ref string value3)
    {
        value1 = "";
        value2 = "";
        value3 = "";
        int space = 0;
        for (int i = 0; i < text.Length; i++)
        {

            string chr = text[i].ToString();
            if (chr == " ")
            {
                space++;
            }
            if (space >= 2)
            {
                value3 += chr;
            }
            else
            if (space >= 1)
            {
                value2 += chr;
            }
            else
            if (space >= 0)
            {
                value1 += chr;
            }
            value2 = value2.TrimStart();
            value3 = value3.TrimStart();
        }
    }
    public void TakeSystemMessage(string text)
    {

        UpdateTextInChat(text);
    }
    [TargetRpc]
    public void TargetRpcSendMessageInGlobalChat(NetworkConnection conn, string text)
    {

        UpdateTextInChat(uint.MaxValue, "", text, 8);
    }
    [ClientRpc]
    public void RpcSendMessageInNearChat(uint netid, string login, string text, Vector3 pos)
    {
        if (Vector3.Distance(GOManager.playerSS.moveControll.transform.position, pos) <= 15)
        {
            UpdateTextInChat(netid, login, text, 0);
        }
    }
    [ClientRpc]
    public void RpcSendMessageInCallChat(uint netid, string login, string text, Vector3 pos)
    {
        if (Vector3.Distance(GOManager.playerSS.moveControll.transform.position, pos) <= 30)
        {
            UpdateTextInChat(netid, login, text, 1);
        }
    }
    [ClientRpc]
    public void RpcSendMessageInActionChat(uint netid, string login, string text, Vector3 pos)
    {
        if (Vector3.Distance(GOManager.playerSS.moveControll.transform.position, pos) <= 15)
        {
            UpdateTextInChat(netid, login, text, 2);
        }
    }

    [ClientRpc]
    public void RpcSendMessageInGeneralChat(uint netid, string login, string text)
    {
        UpdateTextInChat(netid, login, text, 3);
    }

    [ClientRpc]
    public void RpcSendMessageInWhisperChat(uint netid, string login, string text, string name)
    {

        if (name == GOManager.playerSS._name)
        {

            if (!chatUiController.channelsActive[4])
            {

                CmdSendWhisperBanMessageToPLayer(login);
            }
            for (int i = 0; i < blockList.Count; i++)
            {
                if (blockList[i] == login)
                {
                    CmdSendWhisperBanMessageToPLayer(login);
                    return;
                }
            }
            UpdateTextInChat(netid, login, text, 4);

        }
    }
    [Command(requiresAuthority = false)]
    public void CmdSendWhisperBanMessageToPLayer(string login)
    {
        foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
        {
            if (conn.identity.GetComponent<StatsSystem>()._name == login)
            {
                TargetRpcSendWhisperBanMessageToPLayer(conn);
                return;
            }
        }

    }
    [TargetRpc]
    public void TargetRpcSendWhisperBanMessageToPLayer(NetworkConnection conn)
    {
        TakeSystemMessage("<color=#ff0008>[Система]: Этот игрок не видит ваших сообщений</color>");
    }
    [ClientRpc]
    public void RpcSendMessageInPartyChat(uint netid, string login, string text)
    {
        NetworkIdentity[] go = GameObject.FindObjectsOfType<NetworkIdentity>();
        uint partyId = 0;
        for (int i = 0; i < go.Length; i++)
        {
            NetworkIdentity networkIdentity = go[i];
            if (networkIdentity.netId == netId)
            {
                partyId = networkIdentity.GetComponent<StatsSystem>().partyId;
            }
        }
        if (partyId == GOManager.playerSS.partyId)
        {
            UpdateTextInChat(netid, login, text, 5);
        }
    }
    [ClientRpc]
    public void RpcSendMessageInGuildChat(uint netid, string login, string text)
    {
        NetworkIdentity[] go = GameObject.FindObjectsOfType<NetworkIdentity>();
        uint guildId = 0;
        for (int i = 0; i < go.Length; i++)
        {
            NetworkIdentity networkIdentity = go[i];
            if (networkIdentity.netId == netId)
            {
                guildId = networkIdentity.GetComponent<StatsSystem>().guildId;
            }
        }
        if (guildId == GOManager.playerSS.guildId)
        {
            UpdateTextInChat(netid, login, text, 6);
        }
    }
    [ClientRpc]
    public void RpcSendMessageInGlobalChat(uint netid, string login, string text)
    {
        UpdateTextInChat(netid, login, text, 7);

    }
    void UpdateTextInChat(string text)
    {
        text = "\n" + text;
        chatTextList.Add(new ChatUIController.ChatText(text, 8));
        chatText.text = "";
        for (int i = 0; i < chatTextList.Count; i++)
        {
            if (chatUiController.channelsActive[chatTextList[i].channel])
            {
                chatText.text += chatTextList[i].text;

            }
        }
    }
    void UpdateTextInChat(uint netid, string login, string text, byte channel)
    {
        //Debug.Log(text);
        var SystemTime = System.DateTime.Now;

        StringBuilder TextStringBuilder = new StringBuilder();
        StringBuilder TextStringBuilder2 = new StringBuilder();
        TextStringBuilder.Append("\n");
        TextStringBuilder.Append("<color=#d82500>[");
        TextStringBuilder.Append(SystemTime.Hour.ToString("00"));
        TextStringBuilder.Append(":");
        TextStringBuilder.Append(SystemTime.Minute.ToString("00"));
        TextStringBuilder.Append("]</color> ");
        switch (channel)
        {
            case 0:
                TextStringBuilder.Append("<color=#fdf5e6>[<b>" + login + "</b>][Рядом]: " + text + "</color>");
                TextStringBuilder2.Append("<color=#fdf5e6>" + text + "</color>");
                break;
            case 1:
                TextStringBuilder.Append("<color=#ff0000>[<b>" + login + "</b>][Крик]: " + text + "</color>");
                TextStringBuilder2.Append("<color=#ff0000>" + text + "</color>");
                break;
            case 2:
                TextStringBuilder.Append("<color=#d73d27>[<b>" + login + "</b>][Действие]: " + text + "</color>");
                TextStringBuilder2.Append("<color=#d73d27>" + text + "</color>");
                break;
            case 3:
                TextStringBuilder.Append("<color=#008000>[<b>" + login + "</b>][Общий]: " + text + "</color>");
                TextStringBuilder2.Append("<color=#008000>" + text + "</color>");
                break;
            case 4:
                TextStringBuilder.Append("<color=#c539a3>[<b>" + login + "</b>] пишет вам: " + text + "</color>");
                TextStringBuilder2.Append("<color=#c539a3>" + text + "</color>");
                break;
            case 5:
                TextStringBuilder.Append("<color=#00bfff>[<b>" + login + "</b>][Группа]: " + text + "</color>");
                TextStringBuilder2.Append("<color=#00bfff>" + text + "</color>");
                break;
            case 6:
                TextStringBuilder.Append("<color=#0000ff>[<b>" + login + "</b>][Гильдия]: " + text + "</color>");
                TextStringBuilder2.Append("<color=#0000ff>" + text + "</color>");
                break;
            case 7:
                TextStringBuilder.Append("<color=#00ffff>[<b>" + login + "</b>][Мир]: " + text + "</color>");
                TextStringBuilder2.Append("<color=#00ffff>" + text + "</color>");
                break;
            case 8:
                TextStringBuilder.Append(text);
                break;
        }

        chatTextList.Add(new ChatUIController.ChatText(TextStringBuilder.ToString(), channel));

        CreateWorldChatText(netid, TextStringBuilder2.ToString());
        chatText.text = "";
        for (int i = 0; i < chatTextList.Count; i++)
        {
            if (chatUiController.channelsActive[chatTextList[i].channel])
            {
                chatText.text += chatTextList[i].text;

            }
        }

    }
    void UpdateTextInChat()
    {
        chatText.text = "";
        for (int i = 0; i < chatTextList.Count; i++)
        {
            if (chatUiController.channelsActive[chatTextList[i].channel])
                chatText.text += chatTextList[i].text;
        }
    }
    [ClientRpc]
    public void RpcSetGuild(uint id)
    {
        GOManager.playerSS.guildId = id;
    }
    [ClientRpc]
    public void RpcSetParty(uint id)
    {
        GOManager.playerSS.partyId = id;
    }
    public void ClickChannel1(int i)
    {
        chatUiController.channelsActive[i] = !chatUiController.channelsActive[i];
        InterfaceSettings.SaveChatSettings((int)chatUiController.rectTransform.anchoredPosition.x, (int)chatUiController.rectTransform.anchoredPosition.y, (int)chatUiController.rectTransform.sizeDelta.x, (int)chatUiController.rectTransform.sizeDelta.y, chatUiController.channelsActive, blockList.ToArray());
        UpdateTextInChat();
    }
    public void OnMailRequestMessage(NetworkConnectionToClient conn, MailRequestMessage d)
    {
        conn.Disconnect();
        string login = d.email;
        string _password = string.Empty;
        if (File.Exists("DataBase/Accounts/" + login + "/Account.xml"))
        {
            XmlTextReader reader = new XmlTextReader("DataBase/Accounts/" + login + "/Account.xml");
            while (reader.Read())
            {

                if (reader.IsStartElement("Account"))
                {
                    _password = reader.GetAttribute("Password");

                    // отправитель - устанавливаем адрес и отображаемое в письме имя
                    MailAddress from = new MailAddress("ovchinnikov3228@yandex.ru", "Gothic Online Support");
                    // кому отправляем
                    MailAddress to = new MailAddress(login);
                    // создаем объект сообщения
                    MailMessage m = new MailMessage(from, to);
                    // тема письма
                    m.Subject = "Восстановление пароля";
                    // текст письма
                    m.Body = "<h2> Ваш пароль: " + _password + "</h2>";
                    // письмо представляет код html
                    m.IsBodyHtml = true;
                    // адрес smtp-сервера и порт, с которого будем отправлять письмо
                    SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 587);
                    // логин и пароль
                    smtp.Credentials = new NetworkCredential("ovchinnikov3228@yandex.ru", "yoirwhlpqfvoctmq");
                    smtp.EnableSsl = true;
                    smtp.Send(m);
                    reader.Close();
                    return;
                }
            }

            reader.Close();
        }
    }

}
