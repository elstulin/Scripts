using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterChanger : MonoBehaviour
{
    public Slider weightSlider;
    public Slider scaleSlider;
    public Slider bodySlider;
    public Slider headSlider;
    public TMP_InputField nickNameInputField;
    public delegate void ChangeWeight();
    public static event ChangeWeight OnChangeWeight;
    public static bool UIActive = false;
    public GameObject CharacterChangerMenu;
    public Material[] skins;
    public Item equipedItemArmor;
    public static CharacterChanger instance;
    public delegate void OnMenuOpened();
    public static event OnMenuOpened OnMenuOpenedEvent;
    public delegate void OnMenuClosed();
    public static event OnMenuClosed OnMenuClosedEvent;
    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        weightSlider.onValueChanged.AddListener(delegate { OnWeightSliderValueChange(); });
        scaleSlider.onValueChanged.AddListener(delegate { OnScaleValueChange(); });
        bodySlider.onValueChanged.AddListener(delegate { OnBodyValueChange(); });
        headSlider.onValueChanged.AddListener(delegate { OnHeadValueChange(); });

    }
    void OnBodyValueChange()
    {
        GOManager.playerSS.currentBody = (byte)bodySlider.value;
        GOManager.playerSS.CmdBodySync(GOManager.playerSS.currentBody);
        GOManager.playerSS.CmdSaveCustomization(GOManager.AccountName);

    }
    void OnHeadValueChange()
    {
        GOManager.playerSS.currentHead = (byte)headSlider.value;
        GOManager.playerSS.CmdHeadSync(GOManager.playerSS.currentHead);
        GOManager.playerSS.CmdSaveCustomization(GOManager.AccountName);

    }
    void OnScaleValueChange()
    {
        GOManager.playerSS.size = scaleSlider.value;
        GOManager.playerSS.CmdSizeSync(GOManager.playerSS.size);
        GOManager.playerSS.CmdSaveCustomization(GOManager.AccountName);

    }
    void OnWeightSliderValueChange()
    {
        GOManager.playerSS.weight = weightSlider.value;
        GOManager.playerSS.CmdFatnessSync(GOManager.playerSS.weight);
        GOManager.playerSS.CmdSaveCustomization(GOManager.AccountName);
        OnChangeWeight?.Invoke();
    }
    public void SwitchActive()
    {
        UIActive = !UIActive;
        if(UIActive)
        {
            OnMenuOpenedEvent?.Invoke();
            CharacterChangerMenu.SetActive(true);
            if (GOManager.playerSS.inventorySystem.EquipedItemArmor)
            {
                equipedItemArmor = GOManager.playerSS.inventorySystem.EquipedItemArmor;
                GOManager.playerSS.inventorySystem.EquipedItemArmor.Use(false);
                GOManager.playerSS.inventorySystem.EquipedItemArmor = null;
                GOManager.playerSS.inventorySystem.CmdEquipArmor(string.Empty);
                nickNameInputField.text = GOManager.playerSS._name;
                bodySlider.value = GOManager.playerSS.currentBody;
                headSlider.value = GOManager.playerSS.currentHead;
                scaleSlider.value = GOManager.playerSS.size;
                weightSlider.value = GOManager.playerSS.weight;
               
            }
        }
        else
        {
            OnMenuClosedEvent?.Invoke();
            CharacterChangerMenu.SetActive(false);
            if (equipedItemArmor)
            {
                GOManager.playerSS.inventorySystem.EquipedItemArmor = equipedItemArmor;
                GOManager.playerSS.inventorySystem.EquipedItemArmor.Use(true, 2);
                GOManager.playerSS.inventorySystem.CmdEquipArmor(equipedItemArmor._name);
            }
        }
    }
    public void SetCustomization()
    {
        GOManager.playerSS.CmdSaveCustomization(GOManager.AccountName);
        GOManager.playerSS._name = nickNameInputField.text;
        GOManager.playerSS.CmdSetNickName(GOManager.playerSS._name);
        
        SwitchActive();
    }
}
