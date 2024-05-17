using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueSystem : MonoBehaviour
{
    public int select;
    public bool open;
    public List<Dialogue> dialogueList = new List<Dialogue>();
    public GameObject lastSel;
    public DialogueTarget dialogueTarget;
    public DialogueTarget DdialogueTarget;
    public PhraseSystem phraseSystem;
    public MoveControll moveControll;
    TargetSystem targetSystem;
   // public StatsSystem self;
    public StatsSystem other;
    private void Awake()
    {
        GOManager.dialogueSystem = this;
    }
    void Start()
    {
        phraseSystem = GOManager.phrasePanel.GetComponent<PhraseSystem>();
        moveControll = GetComponent<MoveControll>();
        GOManager.dialoguePanel.SetActive(false);
        
        targetSystem = GetComponent<TargetSystem>();
      // self = dialogueTarget.GetComponent<StatsSystem>();
        other = GetComponent<StatsSystem>();
    }

    void Update()
    {
        
        if (open)
        {
            if (DdialogueTarget)
            {
                dialogueTarget = DdialogueTarget;
            }
            for (int i = 0; i < dialogueList.Count; i++)
            {
                if (!phraseSystem.Talking)
                {
                    dialogueList[i].image.color = Color.Lerp(dialogueList[i].image.color, new Color(1, 1, 1, 0.1f), 0.3f);
                    dialogueList[i].text.color = Color.Lerp(dialogueList[i].text.color, new Color(1, 1, 1, 1f), 0.3f);
                }
                else
                {
                    dialogueList[i].image.color = Color.Lerp(dialogueList[i].image.color, new Color(1, 1, 1, 0), 0.3f);
                    dialogueList[i].text.color = Color.Lerp(dialogueList[i].text.color, new Color(1, 1, 1, 0), 0.3f);
                }

            }
            if (!phraseSystem.Talking)
                GOManager.dialoguePanel.GetComponent<Image>().color = Color.Lerp(GOManager.dialoguePanel.GetComponent<Image>().color, new Color(0, 0, 0, 1f), 0.3f);
            else
                GOManager.dialoguePanel.GetComponent<Image>().color = Color.Lerp(GOManager.dialoguePanel.GetComponent<Image>().color, new Color(0, 0, 0, 0), 0.3f);
            if (Input.GetKeyDown(KeyCode.W) | Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                select--;
                if (select < 0) select = dialogueList.Count - 1;
                if (lastSel)
                    lastSel.SetActive(false);
                lastSel = dialogueList[select].transform.GetChild(0).gameObject;
                lastSel.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.S) | Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                select++;
                if (select > dialogueList.Count - 1) select = 0;
                if (lastSel)
                    lastSel.SetActive(false);
                lastSel = dialogueList[select].transform.GetChild(0).gameObject;
                lastSel.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)&&dialogueTarget && !phraseSystem.Talking&&!InventorySystem.UIActive)
        {
            AI ai = dialogueTarget.GetComponent<AI>();
            if (!ai||(ai&&!ai.target))
            {
                if (open)
                {
                    if (dialogueList[select].id == -100)
                    {
                        ExitBed();
                    }
                    else
                        if (dialogueList[select].id == -101)
                    {
                        ExitBed();
                    }
                    else
                        if (dialogueList[select].id == -102)
                    {
                        ExitBed();
                    }
                    else
                        if (dialogueList[select].id == -103)
                    {
                        ExitBed();
                    }
                    else
                    {
                        if (dialogueList[select].id == 0)
                        {

                            if (dialogueTarget.ID == 2)
                            {
                                StartCoroutine(efw());
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                for(int i=0;i< dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                }
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, true, dialogueTarget.GetComponent<StatsSystem>()));
                            }
                            else
                            if (dialogueTarget.ID == 1)
                            {
                                Exit();
                            }
                            else
                            if (dialogueTarget.ID >= 3)
                            {
                                Exit();
                            }
                            else
                            if (dialogueTarget.ID == -100)
                            {
                                ExitBed();
                            }
                            else
                                Exit();


                        }
                        else
                        {
                            if (dialogueList[select].id == 1)
                            {
                                QuestSystem._DialogueKsardas1 = true;
                                QuestSystem._DialogueKsardas2 = true;
                                QuestSystem._DialogueKsardas3 = true;
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));

                            }
                            else
                            if (dialogueList[select].id == 2)
                            {
                                GameObject go =  Instantiate(dialogueList[select].trigObject, GOManager.inventoryPanel.transform.parent);
                                Destroy(go, 80);
                                QuestSystem._DialogueKsardas2 = false;
                                QuestSystem._DialogueKsardas4 = true;
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));

                            }
                            else
                            if (dialogueList[select].id == 3)
                            {
                                QuestSystem._DialogueKsardas3 = false;
                                QuestSystem._DialogueKsardas4 = true;
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));

                            }
                            else
                            if (dialogueList[select].id == 4)
                            {
                                QuestSystem._DialogueKsardas4 = false;
                                QuestSystem._DialogueKsardas5 = true;
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));

                            }
                            else
                            if (dialogueList[select].id == 5)
                            {
                                
                                QuestSystem._DialogueKsardas5 = false;
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                StartCoroutine(efwAct1(dialogueList[select].trigObject));
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, true, dialogueTarget.GetComponent<StatsSystem>(), audioClips));

                            }
                            else
                            if (dialogueList[select].id == 100)
                            {
                                dialogueTarget.GetComponent<AI>().companion = transform;
                                StartCoroutine(efw());
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(new string[] { "Идем к твоим падальщикам.", "Пошли!", }, new int[] { 0, 1 }, true, dialogueTarget.GetComponent<StatsSystem>()));



                            }
                            else
                            if (dialogueList[select].id == 101)
                            {
                                dialogueTarget.GetComponent<AI>().companion = null;
                                StartCoroutine(efw());
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(new string[] { "Жди здесь.", "Хорошо!" }, new int[] { 0, 1 }, true, dialogueTarget.GetComponent<StatsSystem>()));

                            }
                            else
                            if (dialogueList[select].id == 105)
                            {
                                QuestSystem.Bdt2 = true;
                                QuestSystem.BdtFollowe = true;
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(new string[] { "Проблемы?", "(Раздражительно)Да! Эти чертовы падальщики не дают покоя.", "Черт! Я не смогу убить их в одиночку!!!" }, new int[] { 0, 1, 1 }, false, dialogueTarget.GetComponent<StatsSystem>()));
                            }
                            else
                            if (dialogueList[select].id == 200)
                            {

                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                StartCoroutine(efwTrade());
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, true, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                            }
                            else
                            if (dialogueList[select].id == 201)
                            {
                                QuestSystem.guard2 = true;
                                QuestSystem.guard3 = true;
                                StartCoroutine(efw());
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(new string[] { "Я принес тебе травы.", "Дай посмотрю!", "ХОРОШАЯ ТРАВА!ПРОХОДИ!" }, new int[] { 0, 1, 1 }, true, dialogueTarget.GetComponent<StatsSystem>()));
                                dialogueTarget.AgroDialog = false;
                                dialogueTarget.guard = false;
                            }
                            else
                            if (dialogueList[select].id == 3000)
                            {
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                                GOManager.player.GetComponent<StatsSystem>().CmdAddStr(5);
                            }
                            else
                            if (dialogueList[select].id == 3001)
                            {
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                                GOManager.player.GetComponent<StatsSystem>().CmdAddDex(5);
                            }
                            else
                            if (dialogueList[select].id == 3002)
                            {
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                                GOManager.player.GetComponent<StatsSystem>().CmdAddSkill1h(5);
                            }
                            else
                            if (dialogueList[select].id == 3003)
                            {
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                                GOManager.player.GetComponent<StatsSystem>().CmdAddSkill2h(5);
                            }
                            else
                            if (dialogueList[select].id == 100000)
                            {
                                QuestSystem._meme1 = true;
                                QuestSystem._meme2 = true;
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                InteractiveChair chair = (Instantiate(Resources.Load("Кресло"),dialogueTarget.transform.position,Quaternion.identity) as GameObject).GetComponent<InteractiveChair>();
                                
                                ai.chair = chair;
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                                
                            }
                            else
                            if (dialogueList[select].id == 100001)
                            {
                                QuestSystem._meme2 = false;
                                QuestSystem._meme3 = true;
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                            }
                            else
                            if (dialogueList[select].id == 100002)
                            {
                                QuestSystem._meme3 = false;
                                QuestSystem._meme4 = true;
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                            }
                            else
                            if (dialogueList[select].id == 100003)
                            {
                                QuestSystem._meme4 = false;
                                QuestSystem._meme5 = true;
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                            }
                            else
                            if (dialogueList[select].id == 100004)
                            {
                                QuestSystem._meme5 = false;
                                QuestSystem._meme6 = true;
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                            }
                            else
                            if (dialogueList[select].id == 100005)
                            {
                                QuestSystem._meme6 = false;
                                QuestSystem._meme7 = true;
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                            }
                            else
                            if (dialogueList[select].id == 100006)
                            {
                                QuestSystem._meme7 = false;
                                QuestSystem._meme8 = true;
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                            }
                            else
                            if (dialogueList[select].id == 100007)
                            {
                                QuestSystem._meme8 = false;
                                QuestSystem._meme9 = true;
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                            }
                            else
                            if (dialogueList[select].id == 100008)
                            {
                                QuestSystem._meme9 = false;
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                Destroy(ai.chair.gameObject);
                                ai.chair = null;
                                StartCoroutine(efwAct1(dialogueList[select].trigObject));
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, true, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                                
                            }
                            else
                            if (dialogueList[select].id == 9541)
                            {
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                QuestSystem._DialogueMaleth1 = true;
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                            }
                            else
                            if (dialogueList[select].id == 9543)
                            {
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                QuestSystem._DialogueMaleth2 = true;
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));

                            }
                            else
                            if (dialogueList[select].id == 9544)
                            {
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                GOManager.playerSS.TakeExp(150);
                                QuestSystem._DialogueMaleth3 = true;
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                            }
                            else
                            if (dialogueList[select].id == 9545)
                            {
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                QuestSystem._DialogueMaleth4 = true;
                                QuestSystem.MalethBanditsLOG += "Если я уничтожу бандитов, поселившихся на полпути от башни Ксардаса к ферме Лобарта, все обитатели фермы будут очень благодарны мне. /n";
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                            }
                            else
                            if (dialogueList[select].id == 9546)
                            {
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                QuestSystem._DialogueMaleth5 = true;
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                            }
                            else
                            if (dialogueList[select].id == 9547)
                            {
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                QuestSystem._DialogueMaleth5 = true;
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                            }
                            else
                            if (dialogueList[select].id == 9548)
                            {
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                QuestSystem._DialogueMaleth6 = true;
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                            }
                            else
                            if (dialogueList[select].id == 9549)
                            {
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                QuestSystem._DialogueMaleth7 = true;
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                            }
                            else
                            if (dialogueList[select].id == 43001)
                            {
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                QuestSystem._DialogueCavalorn1 = true;
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                            }
                            else
                            if (dialogueList[select].id == 43002)
                            {
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                QuestSystem._DialogueCavalorn2 = true;
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                            }
                            else
                            if (dialogueList[select].id == 43003)
                            {
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                QuestSystem._DialogueCavalorn3 = true;
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                            }
                            else
                            if (dialogueList[select].id == 43004)
                            {
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                QuestSystem._DialogueCavalorn4 = true;
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                            }
                            else
                            if (dialogueList[select].id == 43005)
                            {
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                QuestSystem._DialogueCavalorn4 = true;
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                            }
                            else
                            if (dialogueList[select].id == 43006)
                            {
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                QuestSystem._DialogueCavalorn4 = true;
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                            }
                            else
                            if (dialogueList[select].id == 43007)
                            {
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                QuestSystem._DialogueCavalorn5 = true;
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                            }
                            else
                            if (dialogueList[select].id == 43008)
                            {
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                QuestSystem._DialogueCavalorn6 = true;
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                            }
                            else
                            if (dialogueList[select].id == 43009)
                            {
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                QuestSystem._DialogueCavalorn7 = true;
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                            }
                            else
                            if (dialogueList[select].id == 43010)
                            {
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                QuestSystem._DialogueCavalorn8 = true;
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                            }
                            else
                            if (dialogueList[select].id == 43011)
                            {
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                QuestSystem._DialogueCavalorn9 = true;
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                            }
                            else
                            if (dialogueList[select].id == 43012)
                            {
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                QuestSystem._DialogueCavalorn10 = true;
                                dialogueTarget.ai.startPosRnd = new Vector3(-899.35f, -12.15924f, 94.5f);
                                dialogueTarget.ai.startRot = new Vector3(0, -28.272f,0);
                                StartCoroutine(efw());
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, true, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                            }
                            else
                            if (dialogueList[select].id == 43013)
                            {
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                QuestSystem._DialogueCavalorn11 = true;
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                            }
                            else
                            if (dialogueList[select].id == 43014)
                            {
                                string[] str = new string[dialogueList[select].diaInfos.Length];
                                int[] ids = new int[dialogueList[select].diaInfos.Length];
                                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                                {
                                    str[i] = dialogueList[select].diaInfos[i].dias;
                                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                                }
                                QuestSystem._DialogueCavalorn12 = true;
                                dialogueTarget.ai.startPosRnd = new Vector3(-974.53f, -12.177f, 95.29f);
                                StartCoroutine(efw());
                                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, true, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                            }
                            GenerageDialogue(dialogueTarget, true);
                        }
                    }
                }
                else
                {
                    if (!moveControll.stats.wpnrdy)
                    {
                        GenerageDialogue(dialogueTarget, true);
                    }
                }
            }
            

        }


    }
    public void GenerageDialogue(DialogueTarget dialogueTarget,bool self)
    {
        phraseSystem.cameraController.dialogueOffset = new Vector3(2f, -0.25f, -0.9f);
        GOManager.dialoguePanel.SetActive(true);
        phraseSystem.panel.SetActive(true);
        select = 0;
        DdialogueTarget = dialogueTarget;
        if (!open)
        {
            open = true;
            GOManager.dialoguePanel.SetActive(open);
            select = 0;
            moveControll.lookDialogTarget = dialogueTarget.transform.position;
            AI ai = dialogueTarget.GetComponent<AI>();
            if (ai)
                ai.dialogueTarget = transform;
        }
        for (int i = 0; i < dialogueList.Count; i++)
                    {
                        DestroyImmediate(dialogueList[i].gameObject);
                    }
        dialogueList = new List<Dialogue>();
        if (dialogueTarget.ID == 10001)
        {
            if (!QuestSystem._meme1) {
                QuestSystem._meme1 = true;
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/DialogueMeme/DialogueMeme 9") as GameObject);
            dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
            dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            string[] str = new string[dialogueList[select].diaInfos.Length];
            int[] ids = new int[dialogueList[select].diaInfos.Length];
            AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
            for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
            {
                str[i] = dialogueList[select].diaInfos[i].dias;
                ids[i] = dialogueList[select].diaInfos[i].talkerId;
                audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
            }
            StartCoroutine(efwAct1(dialogueList[select].trigObject));
            StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, true, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
            GenerageDialogue(dialogueTarget, true);
            return;
        }
        }else
            if (dialogueTarget.ID == 10000)
        {
            if (!QuestSystem._meme1)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/DialogueMeme/DialogueMeme") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());

                QuestSystem._meme1 = true;
                QuestSystem._meme2 = true;
                string[] str = new string[dialogueList[select].diaInfos.Length];
                int[] ids = new int[dialogueList[select].diaInfos.Length];
                AudioClip[] audioClips = new AudioClip[dialogueList[select].diaInfos.Length];
                for (int i = 0; i < dialogueList[select].diaInfos.Length; i++)
                {
                    str[i] = dialogueList[select].diaInfos[i].dias;
                    ids[i] = dialogueList[select].diaInfos[i].talkerId;
                    audioClips[i] = dialogueList[select].diaInfos[i].audioClip;
                }
                InteractiveChair chair = (Instantiate(Resources.Load("Кресло"), dialogueTarget.transform.position, Quaternion.identity) as GameObject).GetComponent<InteractiveChair>();

                dialogueTarget.ai.chair = chair;
                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(str, ids, false, dialogueTarget.GetComponent<StatsSystem>(), audioClips));
                GenerageDialogue(dialogueTarget, true);
                return;
            }
            if (QuestSystem._meme2)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/DialogueMeme/DialogueMeme 1") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
            if (QuestSystem._meme3)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/DialogueMeme/DialogueMeme 2") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
            if (QuestSystem._meme4)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/DialogueMeme/DialogueMeme 3") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
            if (QuestSystem._meme5)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/DialogueMeme/DialogueMeme 4") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
            if (QuestSystem._meme6)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/DialogueMeme/DialogueMeme 5") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
            if (QuestSystem._meme7)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/DialogueMeme/DialogueMeme 6") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
            if (QuestSystem._meme8)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/DialogueMeme/DialogueMeme 7") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
            if (QuestSystem._meme9)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/DialogueMeme/DialogueMeme 8") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
        }
        else
            if (dialogueTarget.ID == 222222)
        {
            if (!QuestSystem._DialogueKsardas1)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/DialogueKsardas1") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
            if (QuestSystem._DialogueKsardas2)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/DialogueKsardas2") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
            if (QuestSystem._DialogueKsardas3)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/DialogueKsardas3") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
            if (QuestSystem._DialogueKsardas4 && !QuestSystem._DialogueKsardas2 && !QuestSystem._DialogueKsardas3)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/DialogueKsardas4") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
            if (QuestSystem._DialogueKsardas5)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/DialogueKsardas5") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/DialogueGainStr") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/DialogueGainDex") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/DialogueGainSkill1h") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/DialogueGainSkill2h") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
        }

        if (dialogueTarget.ID == 2)
        {

            if (!QuestSystem.Bdt1)
            {
                // StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(new string[] { "Эй парень, а ты откуда нарисовался?", "Я спустился с гор.", "Аааа!", }, new int[] { 1, 0, 1 }, false, dialogueTarget.GetComponent<StatsSystem>()));
                QuestSystem.Bdt1 = true;
                select = 0;
                GenerageDialogue(dialogueTarget, true);
                return;
            }
            if (!QuestSystem.BdtScavengersQuestSuffD && QuestSystem.BdtScavengersQuestSuff)
            {
                StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(new string[] { "Ха! Так им! Спасибо за помощь", "А как же награда?.", "А, да, вот, возьми.", }, new int[] { 1, 0, 1 }, false, dialogueTarget.GetComponent<StatsSystem>()));
                GOManager.player.GetComponent<StatsSystem>().TakeExp(400);
                select = 0;
                QuestSystem.BdtScavengersQuestSuffD = true;
                QuestSystem.BdtFollowe = false;
                dialogueTarget.GetComponent<AI>().companion = null;
                GenerageDialogue(dialogueTarget, true);
                return;
            }

            if (QuestSystem.BdtFollowe)
            {
                if (dialogueTarget.GetComponent<AI>().companion == null)
                {
                    GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/DialogueFollow") as GameObject);
                    dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                    dialogueList.Add(dialogue1.GetComponent<Dialogue>());
                }
                else
                {
                    GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/DialogueFollow 1") as GameObject);
                    dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                    dialogueList.Add(dialogue1.GetComponent<Dialogue>());
                }
            }
            if (!QuestSystem.Bdt2)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/DialogueProblems") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }

        }
        else
    if (dialogueTarget.ID == 1)
        {
            if (!self && !QuestSystem.guard3)
            {
                if (!QuestSystem.guard1)
                {
                    QuestSystem.guard1 = true;
                    StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(new string[] { "Стоять!", "Что?", "Ты не пройдешь.", }, new int[] { 1, 0, 1 }, false, dialogueTarget.GetComponent<StatsSystem>()));
                    select = 0;
                }
                else
                if (!QuestSystem.guard10)
                {
                    QuestSystem.guard10 = true;
                    StartCoroutine(GOManager.phrasePanel.GetComponent<PhraseSystem>().SetPhrase(new string[] { "Ты... Дурачок или как?", "Что?", }, new int[] { 1, 0 }, false, dialogueTarget.GetComponent<StatsSystem>()));
                    select = 0;
                }
                else
                {

                    dialogueTarget.ai.target = GetComponent<StatsSystem>();
                    Exit();

                }
            }
            if (!QuestSystem.guard2)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/DialogueGuard1") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
            if (!QuestSystem.guard3)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/DialogueGuard2") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
        }
        else
        if (dialogueTarget.ID == -100)
        {
            GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/DialogueBed1") as GameObject);
            dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
            dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            GameObject dialogue2 = Instantiate(Resources.Load("Dialogues/DialogueBed2") as GameObject);
            dialogue2.transform.SetParent(GOManager.dialoguePanel.transform);
            dialogueList.Add(dialogue2.GetComponent<Dialogue>());
            GameObject dialogue3 = Instantiate(Resources.Load("Dialogues/DialogueBed3") as GameObject);
            dialogue3.transform.SetParent(GOManager.dialoguePanel.transform);
            dialogueList.Add(dialogue3.GetComponent<Dialogue>());
            GameObject dialogue4 = Instantiate(Resources.Load("Dialogues/DialogueBed4") as GameObject);
            dialogue4.transform.SetParent(GOManager.dialoguePanel.transform);
            dialogueList.Add(dialogue4.GetComponent<Dialogue>());
        }
        else
        if (dialogueTarget.ID == 100023)
        {
            GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/DialogueTrade") as GameObject);
            dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
            dialogueList.Add(dialogue1.GetComponent<Dialogue>());
        }
        else
        if (dialogueTarget.ID == 954)
        {
            if (!QuestSystem._DialogueMaleth1)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/Малет/DiaMaleth1") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
            if (QuestSystem._DialogueMaleth1 && !QuestSystem._DialogueMaleth2)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/Малет/DiaMaleth3") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
            if (QuestSystem._DialogueMaleth2)
            {
                if (QuestSystem.Npc_IsDead("Браго"))
                {
                    if (!QuestSystem._DialogueMaleth3)
                    {
                        GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/Малет/DiaMaleth4") as GameObject);
                        dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                        dialogueList.Add(dialogue1.GetComponent<Dialogue>());
                    }
                }
                else
                {
                    if (!QuestSystem._DialogueMaleth4 && !QuestSystem._DialogueMalethKnowWhereBandits)
                    {
                        GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/Малет/DiaMaleth5") as GameObject);
                        dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                        dialogueList.Add(dialogue1.GetComponent<Dialogue>());
                    }
                }
            }

            if (GOManager.playerSS.inventorySystem.EquipedArmor)
            {
                if (QuestSystem._DialogueMaleth1 && !QuestSystem._DialogueMaleth5)
                {
                    GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/Малет/DiaMaleth6") as GameObject);
                    dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                    dialogueList.Add(dialogue1.GetComponent<Dialogue>());
                }
            }
            else
            {
                if (QuestSystem._DialogueMaleth1 && !QuestSystem._DialogueMaleth5)
                {
                    GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/Малет/DiaMaleth7") as GameObject);
                    dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                    dialogueList.Add(dialogue1.GetComponent<Dialogue>());
                }
            }
            if (QuestSystem._DialogueMaleth1 && !QuestSystem._DialogueMaleth6)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/Малет/DiaMaleth8") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
            if (QuestSystem._DialogueMaleth6 && !QuestSystem._DialogueMaleth7)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/Малет/DiaMaleth9") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
        }
        else
        if (dialogueTarget.ID == 4300)
        {

            if (!QuestSystem._DialogueCavalorn1)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/Кавалорн/DiaCavalorn1") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }

            if (QuestSystem._DialogueCavalorn1&& !QuestSystem._DialogueCavalorn3)
            {
                if (!QuestSystem._DialogueCavalorn2)
                {
                    GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/Кавалорн/DiaCavalorn2") as GameObject);
                    dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                    dialogueList.Add(dialogue1.GetComponent<Dialogue>());
                }
                    GameObject dialogue2 = Instantiate(Resources.Load("Dialogues/Кавалорн/DiaCavalorn3") as GameObject);
                dialogue2.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue2.GetComponent<Dialogue>());
                
            }
            if (QuestSystem._DialogueCavalorn3&&!QuestSystem._DialogueCavalorn4)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/Кавалорн/DiaCavalorn4") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
            if (QuestSystem._DialogueCavalorn3 && !QuestSystem._DialogueCavalorn4)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/Кавалорн/DiaCavalorn5") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
            if (QuestSystem._DialogueCavalorn3 && !QuestSystem._DialogueCavalorn4)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/Кавалорн/DiaCavalorn6") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }

            if (QuestSystem._DialogueCavalorn4 && !QuestSystem._DialogueCavalorn5)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/Кавалорн/DiaCavalorn7") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
            if (QuestSystem._DialogueCavalorn5 && !QuestSystem._DialogueCavalorn6)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/Кавалорн/DiaCavalorn8") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
            if (QuestSystem._DialogueCavalorn6 && !QuestSystem._DialogueCavalorn7)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/Кавалорн/DiaCavalorn9") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
            if (QuestSystem._DialogueCavalorn7 && !QuestSystem._DialogueCavalorn8)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/Кавалорн/DiaCavalorn10") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
            if (QuestSystem._DialogueCavalorn8 && !QuestSystem._DialogueCavalorn9)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/Кавалорн/DiaCavalorn11") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
            if (QuestSystem._DialogueCavalorn8 && !QuestSystem._DialogueCavalorn10)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/Кавалорн/DiaCavalorn12") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
            if (QuestSystem.Npc_IsDead("Браго")&&QuestSystem._DialogueCavalorn10 && !QuestSystem._DialogueCavalorn11)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/Кавалорн/DiaCavalorn13") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
            if (QuestSystem._DialogueCavalorn11 && !QuestSystem._DialogueCavalorn12)
            {
                GameObject dialogue1 = Instantiate(Resources.Load("Dialogues/Кавалорн/DiaCavalorn14") as GameObject);
                dialogue1.transform.SetParent(GOManager.dialoguePanel.transform);
                dialogueList.Add(dialogue1.GetComponent<Dialogue>());
            }
        }

            GameObject dialogueExit = Instantiate(Resources.Load("Dialogues/DialogueExit") as GameObject);
        dialogueExit.transform.SetParent (GOManager.dialoguePanel.transform);
        dialogueList.Add(dialogueExit.GetComponent<Dialogue>());
        for (int i = 0; i < dialogueList.Count; i++)
        {

            dialogueList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(10, -10 - (i) * 30);
        }
        if (lastSel)
            lastSel.SetActive(false);
        lastSel = dialogueList[select].transform.GetChild(0).gameObject;
        lastSel.SetActive(true);
        
    }
    public IEnumerator efw()
    {
        for (int i = 0; i < 1000; i++)
        {
            yield return new WaitForSeconds(0.01f);
            if (phraseSystem.off)
            {
                DdialogueTarget = null;
                open = false;
                moveControll.lookDialogTarget = new Vector3(0,0,0);
                dialogueTarget.trig2 = false;
                if (dialogueTarget.ai)
                    dialogueTarget.ai.dialogueTarget = null;
                GOManager.dialoguePanel.SetActive(false);
                phraseSystem.panel.SetActive(false);
                GOManager.dialoguePanel.SetActive(open);
                phraseSystem.cameraController.dialogueOffset = new Vector3(0, 0, 0);
                break;
            }
        }
    }
    public IEnumerator efwTrade()
    {
        for (int i = 0; i < 1000; i++)
        {
            yield return new WaitForSeconds(0.01f);
            if (phraseSystem.off)
            {
                DdialogueTarget = null;
                open = false;
                moveControll.lookDialogTarget = new Vector3(0, 0, 0);
                dialogueTarget.trig2 = false;
                if (dialogueTarget.ai)
                    dialogueTarget.ai.dialogueTarget = null;
                GOManager.dialoguePanel.SetActive(false);
                phraseSystem.panel.SetActive(false);
                GOManager.dialoguePanel.SetActive(open);
                phraseSystem.cameraController.dialogueOffset = new Vector3(0, 0, 0);
                targetSystem.Invoke("OpenTargetInventoryTrade", 0.1f);
                break;
            }
        }
    }
    public IEnumerator efwAct1(GameObject go1)
    {
        for (int i = 0; i < 1000; i++)
        {
            yield return new WaitForSeconds(0.01f);
            if (phraseSystem.off)
            {
                GameObject go = Instantiate(go1, GOManager.inventoryPanel.transform.parent);
                Destroy(go, 13);
                DdialogueTarget = null;
                open = false;
                moveControll.lookDialogTarget = new Vector3(0, 0, 0);
                dialogueTarget.trig2 = false;
                if (dialogueTarget.ai)
                    dialogueTarget.ai.dialogueTarget = null;
                GOManager.dialoguePanel.SetActive(false);
                phraseSystem.panel.SetActive(false);
                GOManager.dialoguePanel.SetActive(open);
                phraseSystem.cameraController.dialogueOffset = new Vector3(0, 0, 0);
                break;
            }
        }
    }
    public void Exit()
    {
        DdialogueTarget = null;
        open = false;
        moveControll.lookDialogTarget = new Vector3(0, 0, 0);
        dialogueTarget.trig2 = false;
        if(dialogueTarget.ai)
        dialogueTarget.ai.dialogueTarget = null;
        GOManager.dialoguePanel.SetActive(false);
        phraseSystem.panel.SetActive(false);
        GOManager.dialoguePanel.SetActive(open);
        phraseSystem.cameraController.dialogueOffset = new Vector3(0, 0, 0);
    }
    public void ExitBed()
    {
        DdialogueTarget = null;
        open = false;
        moveControll.lookDialogTarget = new Vector3(0, 0, 0);
        dialogueTarget.trig2 = false;
        GOManager.dialoguePanel.SetActive(false);
        phraseSystem.panel.SetActive(false);
        GOManager.dialoguePanel.SetActive(false);
        targetSystem.animator.SetBool("Bed", false);
        targetSystem.Invoke("UnBed", 1.3f);
        phraseSystem.cameraController.dialogueOffset = new Vector3(0, 0, 0);
    }

}
