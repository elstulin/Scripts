using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Dissonance;
public class NpcVoiceArea
{
    public float timer;
    public Vector3 position;
    public NpcVoiceArea(Vector3 position)
    {
        timer = 0;
        this.position = position;
    }
}
public static class GOManager
{
    public static DissonanceComms dissonanceComms;
    public static Transform Canvas;
    public static Camera MainCamera;
    public static List<GameObject> iconsList = new List<GameObject>();
    public static GameObject inventoryPanel;
    public static GameObject inventoryViewPanel;
    public static GameObject chestPanel;
    public static GameObject itemCams;
    public static GameObject dialoguePanel;
    public static Camera waterRippleCamera;
    public static List<WorldItem> worldItems = new List<WorldItem>();
    public static List<LadderObject> LadderObjs = new List<LadderObject>();
    public static List<Chest> worldChests = new List<Chest>();
    public static List<StatsSystem> worldChars = new List<StatsSystem>();
    public static List<InteractiveBed> beds = new List<InteractiveBed>();
    public static List<InteractiveChair> chairs = new List<InteractiveChair>();
    public static List<InteractiveBookBoard> books = new List<InteractiveBookBoard>();
    public static List<TouchButton> buttons = new List<TouchButton>();
    public static GameObject player;
    public static GameObject minimap;
    public static Transform minimap2;
    public static GameObject aimImage;
    public static StatsPanel statsPanel;
    public static List<Transform> enemysPos = new List<Transform>();
    public static List<RectTransform> enemysPosUI = new List<RectTransform>();
    public static Animator playerAnim;
    public static DialogueSystem playerDS;
    public static Transform playerTransform;
    public static StatsSystem playerSS;
    public static GameObject playerHpUIGo;
    public static RectTransform playerHpUIRectTransform;
    public static GameObject playerEnduranceUIGo;
    public static RectTransform playerEnduranceUIRectTransform;
    public static RectTransform playerMpUI;
    public static GameObject MaxHPSprite;
    public static RectTransform MinHPSprite;
    public static GameObject phrasePanel;
    public static short attacked = 0;
    public static List<Transform> waters = new List<Transform>();
    public static Text itemNameText;
    public static GameObject globalight;
    public static SoundContoll HumanSoundSetup1;
    public static GameObject stepParticle;
    public static UnityStandardAssets.Water.PlanarReflection planarReflection;
    public static AudioSource globalSource;
    public static string AccountName;
    public static List<NpcVoiceArea> npcVoiceArea = new List<NpcVoiceArea>();
    public static List<MobSpawner> mobSpawners = new List<MobSpawner>();
    public static bool admin = false;
    public static int humanLanguage = 100;
    public static int orcLanguage = 0;
    public static PhraseSystem phraseSystem;
    public static DialogueSystem dialogueSystem;
    public static float waterHeight = -51.5f;
    public static bool GetVoicePermit(Vector3 position)
    {
        for (int i = 0; i < npcVoiceArea.Count; i++)
        {
            if (Vector3.Distance(npcVoiceArea[i].position, position) < 40)
            {
                return false;
            }
        }
        return true;
    }

}
public class TargetSystem : NetworkBehaviour
{
    public LayerMask layerMask;
    public Transform _transform;
    public InventorySystem inventorySystem;
    public Animator animator;
    public NetworkAnimator networkAnimator;
    public WorldItem targetWorldItem;
    public Chest targetChest;
    public StatsSystem targetCharacter;
    public LadderObject targetLadder;
    public InteractiveBed targetBed;
    public InteractiveChair targetChair;
    public InteractiveBookBoard targetBook;
    public TouchButton targetButton;
    public Text itemNameText;
    public GameObject MaxHPSprite;
    public RectTransform MinHPSprite;

    public StatsSystem stats;
    float TakeLoadTimer;
    public CombatSystem combatSystem;
    public MoveControll moveControll;
    public bool trigg1;
    public DialogueSystem dialogueSystem;
    public float weight;
    Vector3 LookAtPos;
    InteractiveChair curChair;
    InteractiveBed curBed;

    InteractiveBookBoard curBook;
    TouchButton curButton;
    Mirror.NetworkIdentity networkIdentity;
    public void BreakChest()
    {
        if(targetChest)
            targetChest.animator.SetBool("Break", true);
    }
    public void UnlockChest()
    {
        _transform.position = targetChest.transform.position + targetChest.transform.forward * 1f + new Vector3(0, -0.258f, 0);
        moveControll.lookTarget = targetChest.transform.position;
        inventorySystem.itemsChest = targetChest.items;
        inventorySystem.itemsSelect = inventorySystem.items.Count;
        inventorySystem.openChest = true;
        if (targetChest.unlocked)
        {
            stats.SetBoolAsTrigger("OpenChest2", 0.7f);
        }
    }
    public void OpenChest()
    {
        if (targetChest)
        {
            targetChest.Open();
            inventorySystem.Switch(true);
                
        }





    }
    public void CloseChest()
    {
        inventorySystem.Switch(false);
        inventorySystem.openChest = false;
            
        GetComponent<MoveControll>().lookTarget = new Vector3(0, 0, 0);
        if (targetChest)
        {
            targetChest.Close();
        }
        if (targetCharacter)
            targetCharacter.ItemsChestUpdate();
        inventorySystem.itemsChest = new List<Item>();
    }
    private void Awake()
    {
        GOManager.stepParticle = Resources.Load("StepParticles") as GameObject;
    }
    void Start()
    {
        _transform = transform;
        inventorySystem = GetComponent<InventorySystem>();
        animator = GetComponent<Animator>();
        networkAnimator = GetComponent<NetworkAnimator>();
        networkAnimator.animator = animator;
        stats = GetComponent<StatsSystem>();
        combatSystem = GetComponent<CombatSystem>();
        moveControll = GetComponent<MoveControll>();
        dialogueSystem = GetComponent<DialogueSystem>();
        networkIdentity = GetComponent<Mirror.NetworkIdentity>();
        MinHPSprite = GOManager.MinHPSprite;
        MaxHPSprite = GOManager.MaxHPSprite;
        itemNameText = GOManager.itemNameText;
    }
    void OpenTargetInventory()
    {
        GetComponent<MoveControll>().lookTarget = targetCharacter.transform.position;
        inventorySystem.openChest = true;
        inventorySystem.Trade = false;
        inventorySystem.itemsChest = targetCharacter.itemsDrop;
        inventorySystem.itemsSelect = inventorySystem.items.Count;
        inventorySystem.Switch(true);

    }
    void OpenTargetInventoryTrade()
    {
        GetComponent<MoveControll>().lookTarget = targetCharacter.transform.position;
        inventorySystem.openChest = true;
        inventorySystem.Trade = true;
        inventorySystem.itemsChest = targetCharacter.itemsDrop;
        inventorySystem.itemsSelect = inventorySystem.items.Count;
        inventorySystem.ItemsChestUpdate();
        inventorySystem.openChest = false;
        inventorySystem.itemsCam.SetActive(false);
        InventorySystem.UIActive = false;
        inventorySystem.goInv.SetActive(false);
        inventorySystem.goChest.SetActive(false);
        inventorySystem.infoItm();
        inventorySystem.IconsUpdate();
    }
    void OnAnimatorIKd()
    {
        animator.SetLookAtWeight(weight);
        if (targetCharacter)
        {
            weight = Mathf.Lerp(weight, 1, 0.1f);
            LookAtPos = Vector3.Lerp(LookAtPos, targetCharacter.transform.GetChild(0).position, 0.1f);
            animator.SetLookAtPosition(LookAtPos);
        }
        else
        if (targetChest)
        {
            weight = Mathf.Lerp(weight, 1, 0.1f);
            LookAtPos = Vector3.Lerp(LookAtPos, targetChest.transform.GetChild(0).position, 0.1f);
            animator.SetLookAtPosition(LookAtPos);
        }
        else
        if (targetWorldItem)
        {
            weight = Mathf.Lerp(weight, 1, 0.1f);
            LookAtPos = Vector3.Lerp(LookAtPos, targetWorldItem.transform.GetChild(0).position, 0.1f);
            animator.SetLookAtPosition(LookAtPos);
        }
        else
        {
            weight = Mathf.Lerp(weight, 0, 0.1f);
        }


    }
    void LateUpdate()
    {
        if (!networkIdentity.isLocalPlayer) return;
        List<WorldItem> worldItems = GOManager.worldItems;
        List<Chest> worldChests = GOManager.worldChests;
        List<StatsSystem> worldChars = GOManager.worldChars;
        List<LadderObject> ladderObjs = GOManager.LadderObjs;
        List<InteractiveBed> targetBeds = GOManager.beds;
        List<InteractiveChair> targetChairs = GOManager.chairs;
        List<InteractiveBookBoard> targetBooks = GOManager.books;
        List<TouchButton> targetButtons = GOManager.buttons;
        int interactiveObj = -1;
        int indexWinner = -1;
        float mindis = 3;
        float maxDot = 0;
        float point = 0;
        float maXpoint = 0;
        float Dist = 3;
        if (!stats.wpnrdy)
        {

            for (int i = 0; i < worldItems.Count; i++)
            {
                point = 0;
                WorldItem worldItem = worldItems[i];
                Vector3 targetPos = worldItem.transform.position;
                float dis = Vector3.Distance(targetPos, _transform.position);
                if (dis <= Dist)
                {
                    float dot = Vector3.Dot(targetPos - _transform.position, _transform.forward);
                    if (dot > 0.5f)
                    {


                        if (dis < mindis)
                        {
                            mindis = dis;
                            point += (Dist - mindis);
                        }
                        if (dot > maxDot)
                        {
                            maxDot = dot;
                            point += dot * 0.5f;

                        }
                        if (point > maXpoint)
                        {
                            maXpoint = point;
                            indexWinner = i;
                            interactiveObj = 0;
                        }
                    }
                }
            }

            for (int i = 0; i < worldChests.Count; i++)
            {
                point = 0;
                Chest worldChest = worldChests[i];
                Vector3 targetPos = worldChest.transform.position;
                float dis = Vector3.Distance(targetPos, _transform.position);
                if (dis <= Dist)
                {
                    float dot = Vector3.Dot(targetPos - _transform.position, _transform.forward);
                    if (dot > 0.5f)
                    {
                        if (dis < mindis)
                        {
                            mindis = dis;
                            point += (Dist - mindis);
                        }
                        if (dot > maxDot)
                        {
                            maxDot = dot;
                            point += dot * 0.5f;

                        }
                        if (point > maXpoint)
                        {
                            maXpoint = point;
                            indexWinner = i;
                            interactiveObj = 1;
                        }
                    }
                }
            }

            for (int i = 0; i < worldChars.Count; i++)
            {
                point = 0;
                StatsSystem worldChar = worldChars[i];
                Vector3 targetPos = worldChar.transform.position;
                float dis = Vector3.Distance(targetPos, _transform.position);
                if (dis <= Dist)
                {
                    float dot = Vector3.Dot(targetPos - _transform.position, _transform.forward);
                    if (dot > 0.5f)
                    {
                        if (dis < mindis)
                        {
                            mindis = dis;
                            point += (Dist - mindis);
                        }
                        if (dot > maxDot)
                        {
                            maxDot = dot;
                            point += dot * 0.5f;

                        }
                        if (point > maXpoint)
                        {
                            maXpoint = point;
                            indexWinner = i;
                            interactiveObj = 2;
                        }
                    }
                }
            }

            for (int i = 0; i < ladderObjs.Count; i++)
            {
                point = 0;
                LadderObject ladderObj = ladderObjs[i];
                Vector3 targetPos = ladderObj.transform.position;
                float dis = Vector3.Distance(targetPos, _transform.position);
                if (dis <= Dist)
                {
                    float dot = Vector3.Dot(targetPos - _transform.position, _transform.forward);
                    if (dot > 0.5f)
                    {
                        if (dis < mindis)
                        {
                            mindis = dis;
                            point += (Dist - mindis);
                        }
                        if (dot > maxDot)
                        {
                            maxDot = dot;
                            point += dot * 0.5f;

                        }
                        if (point > maXpoint)
                        {
                            maXpoint = point;
                            indexWinner = i;
                            interactiveObj = 3;
                        }
                    }
                }
            }

            for (int i = 0; i < targetBeds.Count; i++)
            {
                point = 0;
                InteractiveBed targetBed = targetBeds[i];
                Vector3 targetPos = targetBed.transform.position;
                float dis = Vector3.Distance(targetPos, _transform.position);
                if (dis <= Dist)
                {
                    float dot = Vector3.Dot(targetPos - _transform.position, _transform.forward);
                    if (dot > 0.5f)
                    {
                        if (dis < mindis)
                        {
                            mindis = dis;
                            point += (Dist - mindis);
                        }
                        if (dot > maxDot)
                        {
                            maxDot = dot;
                            point += dot * 0.5f;

                        }
                        if (point > maXpoint)
                        {
                            maXpoint = point;
                            indexWinner = i;
                            interactiveObj = 4;
                        }
                    }
                }
            }

            for (int i = 0; i < targetChairs.Count; i++)
            {
                point = 0;
                InteractiveChair targetChair = targetChairs[i];
                Vector3 targetPos = targetChair.transform.position;
                float dis = Vector3.Distance(targetPos, _transform.position);
                if (dis <= Dist)
                {
                    float dot = Vector3.Dot(targetPos - _transform.position, _transform.forward);
                    if (dot > 0.5f)
                    {
                        if (dis < mindis)
                        {
                            mindis = dis;
                            point += (Dist - mindis);
                        }
                        if (dot > maxDot)
                        {
                            maxDot = dot;
                            point += dot * 0.5f;

                        }
                        if (point > maXpoint)
                        {
                            maXpoint = point;
                            indexWinner = i;
                            interactiveObj = 5;
                        }
                    }
                }
            }

            for (int i = 0; i < targetBooks.Count; i++)
            {
                point = 0;
                InteractiveBookBoard targetBook = targetBooks[i];
                Vector3 targetPos = targetBook.transform.position;
                float dis = Vector3.Distance(targetPos, _transform.position);
                if (dis <= Dist)
                {
                    float dot = Vector3.Dot(targetPos - _transform.position, _transform.forward);
                    if (dot > 0.5f)
                    {
                        if (dis < mindis)
                        {
                            mindis = dis;
                            point += (Dist - mindis);
                        }
                        if (dot > maxDot)
                        {
                            maxDot = dot;
                            point += dot * 0.5f;

                        }
                        if (point > maXpoint)
                        {
                            maXpoint = point;
                            indexWinner = i;
                            interactiveObj = 6;
                        }
                    }
                }
            }

            for (int i = 0; i < targetButtons.Count; i++)
            {
                point = 0;
                TouchButton targetButton = targetButtons[i];
                Vector3 targetPos = targetButton.transform.position;
                float dis = Vector3.Distance(targetPos, _transform.position);
                if (dis <= Dist)
                {
                    float dot = Vector3.Dot(targetPos - _transform.position, _transform.forward);
                    if (dot > 0.5f)
                    {
                        if (dis < mindis)
                        {
                            mindis = dis;
                            point += (Dist - mindis);
                        }
                        if (dot > maxDot)
                        {
                            maxDot = dot;
                            point += dot * 0.5f;

                        }
                        if (point > maXpoint)
                        {
                            maXpoint = point;
                            indexWinner = i;
                            interactiveObj = 7;
                        }
                    }
                }
            }
        }
        else
        {
            float d = 0;
            if (stats.wpntype >= 2 && stats.wpnrdy)
                d = 20;
            mindis = 3 + d;
            for (int i = 0; i < worldChars.Count; i++)
            {
                point = 0;
                StatsSystem worldChar = worldChars[i];
                Vector3 targetPos = worldChar.transform.position;
                float dis = Vector3.Distance(targetPos, _transform.position);

                if (dis <= Dist + d)
                {
                    float dot = Vector3.Dot(targetPos - _transform.position, _transform.forward);

                    if (dot > 0.5f && worldChar.minHealth > 0)
                    {


                        if (dis < mindis)
                        {
                            mindis = dis;
                            point += (Dist + d - mindis);
                        }
                        if (dot > maxDot)
                        {
                            maxDot = dot;
                            point += dot * 0.5f;

                        }
                        if (point > maXpoint)
                        {
                            maXpoint = point;
                            indexWinner = i;
                            interactiveObj = 2;
                        }
                    }
                }
            }
        }


        bool leftmouse = Input.GetKey(KeyCode.Mouse0);
        targetWorldItem = null;
        if (!inventorySystem.openChest)
            targetChest = null;
        // if (!leftmouse || (stats.wpnrdy&& targetCharacter && targetCharacter.minHealth<=0))
        targetCharacter = null;
        targetLadder = null;
        targetBed = null;
        targetChair = null;
        targetBook = null;
        targetButton = null;
        if (interactiveObj > -1)
        {

            if (interactiveObj == 0)
            {

                targetWorldItem = worldItems[indexWinner];
            }
            else
            if (interactiveObj == 1)
            {
                targetChest = worldChests[indexWinner];
            }
            else
            if (interactiveObj == 2)
            {
                StatsSystem statsChar = worldChars[indexWinner];
                if (!leftmouse || !targetCharacter)
                {
                    // Debug.DrawLine(_transform.position + new Vector3(0, 2, 0) + _transform.forward, worldChars[indexWinner].transform.position + new Vector3(0, 0.5f, 0), Color.red);
                    if (statsChar.minHealth <= 0)
                    {
                        targetCharacter = statsChar;
                    }
                    else
                    {
                        RaycastHit raycastHit;
                        if (Physics.Linecast(_transform.position + new Vector3(0, 2, 0) + _transform.forward, statsChar.transform.position + new Vector3(0, 0.5f, 0), out raycastHit, layerMask))
                        {
                            // Debug.Log(raycastHit.collider.name +" / "+ worldChars[indexWinner].name);
                            if (raycastHit.collider.name == statsChar.name)
                                targetCharacter = statsChar;
                        }
                    }
                }
            }
            else
            if (interactiveObj == 3)
            {
                targetLadder = ladderObjs[indexWinner];
            }
            else
            if (interactiveObj == 4)
            {
                targetBed = targetBeds[indexWinner];
            }
            else
            if (interactiveObj == 5)
            {
                targetChair = targetChairs[indexWinner];
            }
            else
            if (interactiveObj == 6)
            {
                targetBook = targetBooks[indexWinner];
            }
            else
            if (interactiveObj == 7)
            {
                targetButton = targetButtons[indexWinner];
            }
        }
        itemNameText.gameObject.SetActive(false);
        MaxHPSprite.SetActive(false);
        combatSystem.target = null;
        dialogueSystem.dialogueTarget = null;
        if (targetWorldItem)
        {

            Transform worlditemtransform = targetWorldItem.transform;
            Item worlditemitem = targetWorldItem.item;
            itemNameText.rectTransform.position = Camera.main.WorldToScreenPoint(targetWorldItem.transform.GetChild(0).position);
            itemNameText.gameObject.SetActive(true);
            if (targetWorldItem.count > 1)
                itemNameText.text = worlditemitem._name + " (" + targetWorldItem.count + ")";
            else
                itemNameText.text = worlditemitem._name;
            if (!inventorySystem.openChest && !InventorySystem.UIActive && Input.GetKeyDown(KeyCode.Mouse0))
            {
                // animator.ResetTrigger("TakeItem");
                // animator.SetTrigger("TakeItem");
                //networkAnimator.ResetTrigger("TakeItem");
                //networkAnimator.SetTrigger("TakeItem");
                stats.SetBoolAsTrigger("TakeItem");
                // CmdAnimatorSetTrigger("TakeItem");
                animator.SetInteger("TakeItemLvl", 0);
                //CmdAnimatorSetInteger("TakeItemLvl", 0);

                if (worlditemtransform.position.y - _transform.position.y >= 1f)
                    // CmdAnimatorSetInteger("TakeItemLvl", 1);
                    animator.SetInteger("TakeItemLvl", 1);
                if (worlditemtransform.position.y - _transform.position.y >= 1.8f)
                    // CmdAnimatorSetInteger("TakeItemLvl", 2);
                    animator.SetInteger("TakeItemLvl", 2);
                inventorySystem.TakedItem = worlditemitem;
                inventorySystem.TakedWorldItem = targetWorldItem;
            }
        }
        else
        {

            if (targetCharacter)
            {
                itemNameText.gameObject.SetActive(true);
                itemNameText.rectTransform.position = Camera.main.WorldToScreenPoint(targetCharacter.transform.GetChild(0).position);
                if (targetCharacter.PLAYER)
                    targetCharacter.CmdUpdateName();
                itemNameText.text = targetCharacter._name;
                MaxHPSprite.SetActive(true);
                MinHPSprite.sizeDelta = new Vector2(((float)targetCharacter.minHealth / targetCharacter.maxHealth) * 186, 16);

                combatSystem.target = targetCharacter.transform;
                combatSystem.targetss = targetCharacter;
                dialogueSystem.dialogueTarget = targetCharacter.GetComponent<DialogueTarget>();
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (!InventorySystem.UIActive && !inventorySystem.openChest)
                    {

                        if (targetCharacter.minHealth <= 0 && targetCharacter.itemsDrop.Count > 0)
                        {
                            Invoke(nameof(OpenTargetInventory), 0.1f);
                        }
                        else
                        {
                            //GetComponent<DialogueSystem>().dialogueTarget = targetCharacter.GetComponent<DialogueTarget>();
                        }
                    }
                }
            }
            else
            {



                if (targetChest)
                {
                    itemNameText.gameObject.SetActive(true);
                    itemNameText.rectTransform.position = Camera.main.WorldToScreenPoint(targetChest.transform.GetChild(0).position);
                    itemNameText.text = "Сундук";

                    if (!InventorySystem.UIActive && !inventorySystem.openChest && TakeLoadTimer <= 0 && Vector3.Dot((_transform.position - targetChest.transform.position).normalized, targetChest.transform.forward) >= 0 && Input.GetKeyDown(KeyCode.Mouse0))
                    {

                        stats.SetBoolAsTrigger("OpenChest");

                        TakeLoadTimer = 1;

                    }

                }
                else
                {

                    if (targetLadder)
                    {
                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            // _transform.position = targetLadder.transform.position + targetLadder.transform.forward * 0.2f + new Vector3(0, 0, 0);
                            moveControll.targetLadder = targetLadder;

                        }
                    }
                    else
                    {
                        if (targetBed)
                        {
                            itemNameText.gameObject.SetActive(true);
                            itemNameText.rectTransform.position = GOManager.MainCamera.WorldToScreenPoint(targetBed.transform.GetChild(0).position);
                            itemNameText.text = "Кровать";

                            if (Input.GetKeyDown(KeyCode.Mouse0) && !dialogueSystem.dialogueTarget)
                            {
                                curBed = targetBed;
                                animator.SetBool("Bed", true);
                                //  CmdAnimatorSetBool("Bed", true);
                                targetBed.transform.GetChild(1).GetComponent<Collider>().enabled = false;
                                _transform.position = targetBed.transform.GetChild(3).position;
                                moveControll.lookTarget = targetBed.transform.position;
                            }
                        }
                        else
                        {
                            if (targetChair)
                            {
                                if (targetChair._name.Length > 0)
                                {
                                    itemNameText.gameObject.SetActive(true);
                                    itemNameText.rectTransform.position = GOManager.MainCamera.WorldToScreenPoint(targetChair.transform.GetChild(0).position);
                                    itemNameText.text = targetChair._name;
                                }

                                if (Input.GetKeyDown(KeyCode.Mouse0))
                                {
                                    curChair = targetChair;
                                    animator.SetBool("Seet", true);
                                    //CmdAnimatorSetBool("Seet", true);
                                    targetChair.transform.GetChild(1).GetComponent<Collider>().enabled = false;
                                    _transform.position = targetChair.transform.GetChild(2).position;
                                    moveControll.lookTarget = targetChair.transform.position;
                                }
                            }
                            else
                            {
                                if (targetBook)
                                {
                                    if (targetBook._name.Length > 0)
                                    {
                                        itemNameText.gameObject.SetActive(true);
                                        itemNameText.rectTransform.position = GOManager.MainCamera.WorldToScreenPoint(targetBook.transform.GetChild(0).position);
                                        itemNameText.text = targetBook._name;
                                    }


                                    if (Input.GetKeyDown(KeyCode.Mouse0))
                                    {
                                        curBook = targetBook;
                                        animator.SetBool("BookRead", true);
                                        //CmdAnimatorSetBool("BookRead", true);
                                        targetBook.transform.GetChild(1).GetComponent<Collider>().enabled = false;
                                        _transform.position = targetBook.transform.GetChild(2).position;
                                        moveControll.lookTarget = targetBook.transform.position;
                                        if (targetBook.expBook)
                                        {
                                            stats.TakeExp(25);
                                            targetBook.expBook = false;
                                        }
                                    }
                                }
                                else
                                {
                                    if (targetButton)
                                    {
                                        if (Input.GetKeyDown(KeyCode.Mouse0))
                                        {
                                            if (!targetButton.touched)
                                            {
                                                curButton = targetButton;
                                                stats.SetBoolAsTrigger("TouchPlate");
                                                // animator.ResetTrigger("TouchPlate");
                                                //animator.SetTrigger("TouchPlate");
                                                // networkAnimator.ResetTrigger("TouchPlate");
                                                //networkAnimator.SetTrigger("TouchPlate");
                                                //  CmdAnimatorSetTrigger("TouchPlate");
                                                _transform.position = targetButton.transform.GetChild(0).position;
                                                moveControll.lookTarget = targetButton.transform.position;
                                                Invoke("UnTouch", 1.10f);
                                                targetButton.touched = true;
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        if (TakeLoadTimer > 0)
            TakeLoadTimer -= Time.deltaTime;
        if (targetChest && !targetChest.unlocked && inventorySystem.openChest && Input.GetKeyDown(KeyCode.A))
        {
            stats.SetBoolAsTrigger("UnlockLeft");
            // animator.ResetTrigger("UnlockLeft");
            // animator.SetTrigger("UnlockLeft");
            // networkAnimator.ResetTrigger("UnlockLeft");
            //  networkAnimator.SetTrigger("UnlockLeft");
            // CmdAnimatorSetTrigger("UnlockLeft");
            targetChest.UnlockTry(0);
            if (targetChest.unlocked)
            {
                stats.SetBoolAsTrigger("OpenChest2", 0.7f);
                // animator.ResetTrigger("OpenChest2");
                // animator.SetTrigger("OpenChest2");
                //  networkAnimator.ResetTrigger("OpenChest2");
                // networkAnimator.SetTrigger("OpenChest2");
                // CmdAnimatorSetTrigger("OpenChest2");
            }
        }
        if (targetChest && !targetChest.unlocked && Input.GetKeyDown(KeyCode.D))
        {
            stats.SetBoolAsTrigger("UnlockRight");
            //animator.ResetTrigger("UnlockRight");
            // animator.SetTrigger("UnlockRight");
            // networkAnimator.ResetTrigger("UnlockRight");
            //networkAnimator.SetTrigger("UnlockRight");
            //CmdAnimatorSetTrigger("UnlockRight");
            targetChest.UnlockTry(1);
            if (targetChest.unlocked)
            {
                stats.SetBoolAsTrigger("OpenChest2", 0.7f);
                // animator.ResetTrigger("OpenChest2");
                //  animator.SetTrigger("OpenChest2");
                //networkAnimator.ResetTrigger("OpenChest2");
                // networkAnimator.SetTrigger("OpenChest2");
                //CmdAnimatorSetTrigger("OpenChest2");
            }
        }
        if (inventorySystem.openChest)
        {
            if (targetCharacter)
            {
                if (targetCharacter.itemsDrop.Count < 1 || Input.GetKey(KeyCode.Tab))
                {
                    TakeLoadTimer = 1;
                    CloseChest();
                }
            }
            else
            if (targetChest)
            {
                if ((!targetChest.opened && Input.GetKey(KeyCode.S)) || (targetChest.opened && Input.GetKey(KeyCode.Tab)))
                {
                    TakeLoadTimer = 1;
                    stats.SetBoolAsTrigger("CloseChest", 1f);
                }
            }


        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (curBed)
            {
                animator.SetBool("Bed", false);
                UnBed();
            }
            if (curChair)
            {

                animator.SetBool("Seet", false);
                Invoke("UnSeet", 1.15f);
            }
            if (curBook)
            {

                animator.SetBool("BookRead", false);
                UnBook();
            }
        }



    }
    void UnSeet()
    {
        curChair.transform.GetChild(1).GetComponent<Collider>().enabled = true;
        moveControll.lookTarget = new Vector3(0, 0, 0);
        curChair = null;
    }
    void UnBed()
    {
        curBed.transform.GetChild(1).GetComponent<Collider>().enabled = true;
        moveControll.lookTarget = new Vector3(0, 0, 0);
        curBed = null;
    }
    void UnBook()
    {
        if (curBook)
            curBook.transform.GetChild(1).GetComponent<Collider>().enabled = true;
        moveControll.lookTarget = new Vector3(0, 0, 0);
        curBook = null;
    }
    void UnTouch()
    {
        moveControll.lookTarget = new Vector3(0, 0, 0);
        curButton = null;
    }
    public void ButtonEvent()
    {
        CmdButtonEvent(curButton.GetComponent<NetworkIdentity>());
    }
    [Command(requiresAuthority = false)]
    public void CmdButtonEvent(NetworkIdentity networkIdentity)
    {
        TouchButton touchButton = networkIdentity.GetComponent<TouchButton>();
        if (touchButton)
            touchButton.targetGate.enabled = true;
    }

}
