using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
public class StatsPanel : MonoBehaviour
{
    public Text textLvl;
    public Text textExp;
    public Text textLvlupExp;
    public Text textLvlPoints;
    public Text textStr;
    public Text textDex;
    public Text textMana;
    public Text textHp;
    public Text textDef1;
    public Text textDef2;
    public Text textDef3;
    public Text textDef4;
    public Text textSkill1h;
    public Text textSkill2h;
    public Text textSkillbow;
    public Text textSkillcrossbow;

    StatsSystem statsSystem;
    public bool open;
    public GameObject go;
    void Start()
    {
        GOManager.statsPanel = this;
        go.SetActive(open);
    }

    void Update()
    {
        if (!NetworkClient.ready) return;
        statsSystem = GOManager.playerSS;
        if (!ChatOmegaLul.active && Input.GetKeyDown(KeyCode.C))
        {
            
            open = !open;
            go.SetActive(open);
            if (open)
            {
                TextUpdate();
            }
        }
    }
    public void TextUpdate()
    {
        textLvl.text = "Уровень. "+ statsSystem.lvl;
        textExp.text = "" + statsSystem.exp;
        textLvlupExp.text = "" + statsSystem.expToLvl;
        textLvlPoints.text = "" + statsSystem.lvlPoints;
        textStr.text = "" + statsSystem.strength;
        textDex.text = "" + statsSystem.dex;
        textHp.text = statsSystem.minHealth + "/" + statsSystem.maxHealth;
        textMana.text = statsSystem.minMana + "/" + statsSystem.maxMana;
        textDef1.text = ""+statsSystem.armor;
        textDef2.text = "" + statsSystem.arrowdef;
        textDef3.text = "" + statsSystem.firedef;
        textDef4.text = "" + statsSystem.magicdef;
        string skill1 = "Новичок";
        string skill2 = "Новичок";
        string skill3 = "Новичок";
        string skill4 = "Новичок";
        if (statsSystem.skill1h >= 30)
            skill1 = "Боец";
        if (statsSystem.skill2h >= 30)
            skill2 = "Боец";
        if (statsSystem.skillbow >= 30)
            skill3 = "Боец";
        if (statsSystem.skillcrossbow >= 30)
            skill4 = "Боец";

        if (statsSystem.skill1h >= 60)
            skill1 = "Мастер";
        if (statsSystem.skill2h >= 60)
            skill2 = "Мастер";
        if (statsSystem.skillbow >= 60)
            skill3 = "Мастер";
        if (statsSystem.skillcrossbow >= 60)
            skill4 = "Мастер";

        textSkill1h.text = skill1+ "     " + statsSystem.skill1h + "%";
        textSkill2h.text = skill2+ "     " + statsSystem.skill2h + "%";
        textSkillbow.text = skill3+ "     " + statsSystem.skillbow + "%";
        textSkillcrossbow.text = skill4+ "     " + statsSystem.skillcrossbow + "%";
    }

    public void AddStr(int value)
    {

        GOManager.playerSS.CmdAddStr(value);
        if (GOManager.playerSS.lvlPoints >= value)
        {
            GOManager.playerSS.lvlPoints -= value;
            GOManager.playerSS.strength += value;
            TextUpdate();
        }
    }
    public void AddDex(int value)
    {
        GOManager.playerSS.CmdAddDex(value);
        if (GOManager.playerSS.lvlPoints >= value)
        {
            GOManager.playerSS.lvlPoints -= value;
            GOManager.playerSS.dex += value;
            TextUpdate();
        }
    }
    public void AddSkill1h(int value)
    {
        GOManager.playerSS.CmdAddSkill1h(value);
        if (GOManager.playerSS.lvlPoints >= value)
        {
            GOManager.playerSS.lvlPoints -= value;
            GOManager.playerSS.skill1h += value;
            TextUpdate();
        }
    }
    public void AddSkill2h(int value)
    {
        GOManager.playerSS.CmdAddSkill2h(value);
        if (GOManager.playerSS.lvlPoints >= value)
        {
            GOManager.playerSS.lvlPoints -= value;
            GOManager.playerSS.skill2h += value;
            TextUpdate();
        }
    }

}
