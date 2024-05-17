/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueLobart : MonoBehaviour
{

	private StatsSystem _self;
	public StatsSystem self
    {
		get { return GetComponent<StatsSystem>(); }
    }
	private StatsSystem _other;
	public StatsSystem other
	{
		get { return GOManager.playerSS; }
	}

	public PhraseSystem phraseSystem;
    private void Start()
    {

        phraseSystem = GOManager.phraseSystem;
    }
    public void UpdateDialoge()
    {

		


	}
	
	Dialoge DIA_Lobart_WorkNOW = new Dialoge("DIA_Lobart_WorkNOW_Condition", "DIA_Lobart_WorkNOW_Info", false, "Я ищу работу.") ;
	public class Dialoge
    {
		public GameObject instance;
		string condition;
		string information;
		bool permanent;
		bool important;
		string description;
		
		public Dialoge(string condition, string information, bool permanent, bool important,string description)
        {
			this.condition = condition;
			this.information = information;
			this.permanent = permanent;
			this.important = important;
			this.description = description;

			GameObject dialogue = Instantiate(Resources.Load("Dialogues/DialogueEmpty") as GameObject);
			dialogue.transform.SetParent(GOManager.dialoguePanel.transform);
			Dialogue dia = dialogue.GetComponent<Dialogue>();
			dia.text.text = description;
			GOManager.dialogueSystem.dialogueList.Add(dia);
			instance = dialogue;
		}
		public Dialoge(string condition, string information, bool permanent, bool important)
		{
			this.condition = condition;
			this.information = information;
			this.permanent = permanent;
			this.important = important;

			GameObject dialogue = Instantiate(Resources.Load("Dialogues/DialogueEmpty") as GameObject);
			dialogue.transform.SetParent(GOManager.dialoguePanel.transform);
			Dialogue dia = dialogue.GetComponent<Dialogue>();
			GOManager.dialogueSystem.dialogueList.Add(dia);
			instance = dialogue;
		}
		public Dialoge(string condition, string information, bool permanent, string description)
		{
			
			this.condition = condition;
			this.information = information;
			this.permanent = permanent;
			this.description = description;

			GameObject dialogue = Instantiate(Resources.Load("Dialogues/DialogueEmpty") as GameObject);
			dialogue.transform.SetParent(GOManager.dialoguePanel.transform);
			Dialogue dia = dialogue.GetComponent<Dialogue>();
			dia.text.text = description;
			GOManager.dialogueSystem.dialogueList.Add(dia);
			instance = dialogue;
		}

	}
	object DIA_Lobart_STOLENCLOTHS()
	{
		return new Dialoge("DIA_Lobart_STOLENCLOTHS_Condition", "DIA_Lobart_STOLENCLOTHS_Info", false, true).instance;
	}
	int DIA_Lobart_STOLENCLOTHS_Condition()
	{
		//if ((Mob_HasItems("CHEST_LOBART", ITAR_Bau_L) == FALSE) && (Lobart_Kleidung_Verkauft == FALSE) && (hero.guild == GIL_NONE))
		if (other.inventorySystem.EquipedItemArmor._name == "Простая одежда фермера")
		{
			return 1;
		}
		return 0;
	}

	void DIA_Lobart_STOLENCLOTHS_Info()
	{
		if (other.inventorySystem.EquipedItemArmor._name == "Простая одежда фермера")
		{
			AI_Output(self, other, "DIA_Lobart_STOLENCLOTHS_05_00", "Я не могу поверить своим глазам! Этот ублюдок расхаживает в МОЕЙ одежде!");
		}
		else
		{
			AI_Output(self, other, "DIA_Lobart_STOLENCLOTHS_05_01", "Эй, ты!");    
			AI_Output(self, other, "DIA_Lobart_STOLENCLOTHS_05_02", "Когда ты тут был последний раз, из моего сундука пропали вещи!");   
		}
	//	if ( DIA_Lobart_WorkNOW)
	if(false)
		{
			AI_Output(self, other, "DIA_Lobart_STOLENCLOTHS_05_03", "Ты мог бы поработать здесь вместо того, чтобы шарить в моем доме, ленивый бездельник!");   
		}
		AI_Output(self, other, "DIA_Lobart_STOLENCLOTHS_05_04", "Отдай немедленно мою одежду!"); 
		Info_ClearChoices(DIA_Lobart_STOLENCLOTHS);
		Info_AddChoice(DIA_Lobart_STOLENCLOTHS, "Забудь об этом!", DIA_Lobart_STOLENCLOTHS_ForgetIt);
		if (Npc_HasItems(other, ITAR_Bau_L) > 0)
		{
			Info_AddChoice(DIA_Lobart_STOLENCLOTHS, "Ладно, можешь забрать ее назад.", DIA_Lobart_STOLENCLOTHS_HereYouGo);
		}
		else
		{
			Info_AddChoice(DIA_Lobart_STOLENCLOTHS, "У меня ее нет.", DIA_Lobart_STOLENCLOTHS_DontHaveIt);
		}
	}

	void DIA_Lobart_STOLENCLOTHS_HereYouGo()
	{
		AI_Output(other, self, "DIA_Lobart_STOLENCLOTHS_HereYouGo_15_00");  //Ладно, можешь забрать ее назад.
		AI_Output(self, other, "DIA_Lobart_STOLENCLOTHS_HereYouGo_05_01");  //Если она тебе нужна, ты можешь ЗАПЛАТИТЬ за нее!
		AI_Output(self, other, "DIA_Lobart_STOLENCLOTHS_HereYouGo_05_02");  //(отрывисто) А теперь иди работай!
		B_GiveInvItems(other, self, ITAR_Bau_L, 1);
		Info_ClearChoices(DIA_Lobart_STOLENCLOTHS);
	};

	void DIA_Lobart_STOLENCLOTHS_DontHaveIt()
	{
		AI_Output(other, self, "DIA_Lobart_STOLENCLOTHS_DontHaveIt_15_00"); //У меня ее нет.
		AI_Output(self, other, "DIA_Lobart_STOLENCLOTHS_DontHaveIt_05_01"); //Ты уже продал ее, да? (зло) Я проучу тебя, парень!
		Lobart_Kleidung_gestohlen = TRUE;
		AI_StopProcessInfos(self);
		B_Attack(self, other, AR_Theft, 1);
	};

	void DIA_Lobart_STOLENCLOTHS_ForgetIt()
	{
		AI_Output(other, self, "DIA_Lobart_STOLENCLOTHS_ForgetIt_15_00");   //Забудь об этом!
		AI_Output(self, other, "DIA_Lobart_STOLENCLOTHS_ForgetIt_05_01");   //(зло) Я проучу тебя, парень!
		Lobart_Kleidung_gestohlen = TRUE;
		AI_StopProcessInfos(self);
		B_Attack(self, other, AR_Theft, 1);
	};

	void AI_Output(StatsSystem from, StatsSystem to, string audioClip, string text)
    {
        phraseSystem.AI_Output(from,to,audioClip,text);
    }
}
*/