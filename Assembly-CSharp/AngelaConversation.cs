using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005F4 RID: 1524
public class AngelaConversation
{
	// Token: 0x060033FB RID: 13307 RVA: 0x0002FB6C File Offset: 0x0002DD6C
	private AngelaConversation()
	{
		AngelaConversation._instance = this;
	}

	// Token: 0x170004ED RID: 1261
	// (get) Token: 0x060033FC RID: 13308 RVA: 0x0002FB85 File Offset: 0x0002DD85
	public static AngelaConversation instance
	{
		get
		{
			if (AngelaConversation._instance == null)
			{
				AngelaConversation._instance = new AngelaConversation();
			}
			return AngelaConversation._instance;
		}
	}

	// Token: 0x170004EE RID: 1262
	// (get) Token: 0x060033FD RID: 13309 RVA: 0x0002FBA0 File Offset: 0x0002DDA0
	public bool loaded
	{
		get
		{
			return this.isLoaded;
		}
	}

	// Token: 0x060033FE RID: 13310 RVA: 0x0015B240 File Offset: 0x00159440
	public static AngelaMessageState GetAngelaMessageState(int index)
	{
		switch (index)
		{
		case 0:
			return AngelaMessageState.GREETING;
		case 1:
			return AngelaMessageState.ENERGY_HALF;
		case 2:
			return AngelaMessageState.ENERGY_FULL;
		case 3:
			return AngelaMessageState.MANAGE_START;
		case 4:
			return AngelaMessageState.MANAGE_END;
		case 5:
			return AngelaMessageState.WORK_SUCCESS;
		case 6:
			return AngelaMessageState.GAMEOVER_RESTARTED;
		case 7:
			return AngelaMessageState.GAMEOVER;
		case 8:
			return AngelaMessageState.CREATURE_STATE_BAD;
		case 9:
			return AngelaMessageState.CREATURE_ESCAPE;
		case 10:
			return AngelaMessageState.AGENT_PANIC;
		case 11:
			return AngelaMessageState.AGENT_BAD;
		case 12:
			return AngelaMessageState.AGENT_DEAD_HEALTH;
		case 13:
			return AngelaMessageState.AGENT_DEAD_MENTAL;
		case 14:
			return AngelaMessageState.OBSERVE_START;
		case 15:
			return AngelaMessageState.OBSERVE_SUCCSS;
		case 16:
			return AngelaMessageState.OBSERVE_FAIL;
		case 17:
			return AngelaMessageState.AGENT_DEAD_NORMAL;
		case 18:
			return AngelaMessageState.MATCHGIRL;
		case 19:
			return AngelaMessageState.OVERENERGY;
		case 20:
			return AngelaMessageState.REDSHOES;
		case 21:
			return AngelaMessageState.D;
		case 22:
			return AngelaMessageState.S;
		case 23:
			return AngelaMessageState.C;
		case 24:
			return AngelaMessageState.I;
		default:
			return AngelaMessageState.GREETING;
		}
	}

	// Token: 0x060033FF RID: 13311 RVA: 0x0002FBA8 File Offset: 0x0002DDA8
	public void Init(AngelaMessage[] ary)
	{
		this.isLoaded = true;
		this.messageLib = new AngelaMessageLib(ary);
	}

	// Token: 0x06003400 RID: 13312 RVA: 0x0002FBBD File Offset: 0x0002DDBD
	public void SendSystemLogMessage(CreatureModel cm, string desc)
	{
		Notice.instance.Send(NoticeName.AddSystemLog, new object[]
		{
			cm,
			desc
		});
	}

	// Token: 0x06003401 RID: 13313 RVA: 0x0002FBDC File Offset: 0x0002DDDC
	public void SendNarrationLogMessage(CreatureModel model, string desc)
	{
		Notice.instance.Send(NoticeName.AddNarrationLog, new object[]
		{
			desc,
			model
		});
		model.narrationList.Add(desc);
	}

	// Token: 0x06003402 RID: 13314 RVA: 0x0015B300 File Offset: 0x00159500
	public string MakeDefaultFormatMessage(AngelaMessageState state, params object[] param)
	{
		AngelaMessage message = this.messageLib.GetMessage(state);
		if (message == null)
		{
			Debug.LogError("Error in founding Message: " + state);
			return string.Empty;
		}
		string text = message.defaultFormat;
		if (text == string.Empty)
		{
			return string.Empty;
		}
		List<AgentModel> list = new List<AgentModel>();
		List<OfficerModel> list2 = new List<OfficerModel>();
		List<SkillTypeInfo> list3 = new List<SkillTypeInfo>();
		List<CreatureModel> list4 = new List<CreatureModel>();
		List<Sefira> list5 = new List<Sefira>();
		foreach (object obj in param)
		{
			if (obj is AgentModel)
			{
				list.Add(obj as AgentModel);
			}
			else if (obj is SkillTypeInfo)
			{
				list3.Add(obj as SkillTypeInfo);
			}
			else if (obj is CreatureModel)
			{
				list4.Add(obj as CreatureModel);
			}
			else if (obj is OfficerModel)
			{
				list2.Add(obj as OfficerModel);
			}
			else if (obj is Sefira)
			{
				list5.Add(obj as Sefira);
			}
		}
		if (list.Count != 0)
		{
			text = this.RefineAgentName(text, list);
		}
		if (list3.Count != 0)
		{
			text = this.RefineSkillName(text, list3);
		}
		if (list4.Count != 0)
		{
			text = this.RefineCreatureName(text, list4);
		}
		if (list2.Count != 0)
		{
			text = this.RefineOfficerName(text, list2);
		}
		if (list5.Count != 0)
		{
			text = this.RefineSefiraName(text, list5);
		}
		if (list4.Count != 0)
		{
			Notice.instance.Send("AddSystemLog", new object[]
			{
				list4[0],
				text
			});
		}
		else
		{
			Notice.instance.Send("AddSystemLog", new object[]
			{
				text
			});
		}
		return text;
	}

	// Token: 0x06003403 RID: 13315 RVA: 0x0015B500 File Offset: 0x00159700
	public void DeplayedMessage()
	{
		AutoTimer autoTimer = new AutoTimer();
		autoTimer.Init();
		autoTimer.StartTimer(2f, new AutoTimer.TargetMethod(this.DeplayedGreeting), AutoTimer.UpdateMode.UPDATE);
	}

	// Token: 0x06003404 RID: 13316 RVA: 0x0015B534 File Offset: 0x00159734
	public void DeplayedGreeting()
	{
		AngelaMessage message = this.messageLib.GetMessage(AngelaMessageState.GREETING);
		string desc = message.GetUnit(PlayerModel.instance.GetDay() + 1).desc;
		AngelaConversationUI.instance.AddAngelaMessage(desc);
	}

	// Token: 0x06003405 RID: 13317 RVA: 0x0015B574 File Offset: 0x00159774
	public string MakeMessage(AngelaMessageState state, params object[] param)
	{
		switch (state)
		{
		case AngelaMessageState.CREATURE_ESCAPE:
		case AngelaMessageState.AGENT_PANIC:
		case AngelaMessageState.AGENT_DEAD_HEALTH:
		case AngelaMessageState.AGENT_DEAD_MENTAL:
		case AngelaMessageState.AGENT_DEAD_NORMAL:
			return string.Empty;
		}
		AngelaMessage message = this.messageLib.GetMessage(state);
		if (message == null)
		{
			Debug.LogError("Error in founding Message: " + state);
			return string.Empty;
		}
		bool flag = !(message.defaultFormat == string.Empty);
		if (state == AngelaMessageState.GREETING)
		{
			string desc = message.GetUnit(PlayerModel.instance.GetDay() + 1).desc;
			this.DeplayedMessage();
			return desc;
		}
		List<AgentModel> list = new List<AgentModel>();
		List<OfficerModel> list2 = new List<OfficerModel>();
		List<SkillTypeInfo> list3 = new List<SkillTypeInfo>();
		List<CreatureModel> list4 = new List<CreatureModel>();
		List<Sefira> list5 = new List<Sefira>();
		foreach (object obj in param)
		{
			if (obj is AgentModel)
			{
				list.Add(obj as AgentModel);
			}
			else if (obj is SkillTypeInfo)
			{
				list3.Add(obj as SkillTypeInfo);
			}
			else if (obj is CreatureModel)
			{
				list4.Add(obj as CreatureModel);
			}
			else if (obj is OfficerModel)
			{
				list2.Add(obj as OfficerModel);
			}
			else if (obj is Sefira)
			{
				list5.Add(obj as Sefira);
			}
		}
		int num = (int)state / (int)AngelaMessageState.OBSERVE_START;
		string text = message.GetUnit().desc;
		string text2 = message.defaultFormat;
		string result = text;
		if (list.Count != 0)
		{
			text = this.RefineAgentName(text, list);
			if (flag)
			{
				text2 = this.RefineAgentName(text2, list);
			}
		}
		if (list3.Count != 0)
		{
			text = this.RefineSkillName(text, list3);
			if (flag)
			{
				text2 = this.RefineSkillName(text2, list3);
			}
		}
		if (list4.Count != 0)
		{
			text = this.RefineCreatureName(text, list4);
			if (flag)
			{
				text2 = this.RefineCreatureName(text2, list4);
			}
		}
		if (list2.Count != 0)
		{
			text = this.RefineOfficerName(text, list2);
			if (flag)
			{
				text2 = this.RefineOfficerName(text2, list2);
			}
		}
		if (list5.Count != 0)
		{
			text = this.RefineSefiraName(text, list5);
			if (flag)
			{
				text2 = this.RefineSefiraName(text2, list5);
			}
		}
		if (message.pos == AngelaMessagePos.UP)
		{
			AngelaConversationUI.instance.AddAngelaMessage(text);
		}
		if (state == AngelaMessageState.AGENT_DEAD_HEALTH)
		{
			AngelaConversationUI.instance.StartAgentDeadTimer();
		}
		switch (state)
		{
		case AngelaMessageState.CREATURE_STATE_BAD:
		case AngelaMessageState.AGENT_BAD:
		case AngelaMessageState.AGENT_DEAD_HEALTH:
		case AngelaMessageState.AGENT_DEAD_MENTAL:
		case AngelaMessageState.AGENT_DEAD_NORMAL:
		case AngelaMessageState.OVERENERGY:
			break;
		default:
			if (state != AngelaMessageState.ENERGY_HALF)
			{
				goto IL_2E5;
			}
			break;
		}
		AngelaConversationUI.instance.AddAngelaMessage(text);
		IL_2E5:
		if (flag)
		{
			if (list4.Count != 0)
			{
				Notice.instance.Send("AddSystemLog", new object[]
				{
					list4[0],
					text2
				});
			}
			else
			{
				Notice.instance.Send("AddSystemLog", new object[]
				{
					text2
				});
			}
		}
		return result;
	}

	// Token: 0x06003406 RID: 13318 RVA: 0x0015B8C0 File Offset: 0x00159AC0
	public string MakeDetailMessage(AngelaMessageState state, bool writeLog, bool angelaNarrate, params object[] param)
	{
		if (!writeLog && !angelaNarrate)
		{
			return string.Empty;
		}
		AngelaMessage message = this.messageLib.GetMessage(state);
		if (message == null)
		{
			Debug.LogError("Error in founding Message: " + state);
			return string.Empty;
		}
		bool flag = !(message.defaultFormat == string.Empty);
		string text = message.GetUnit().desc;
		CreatureModel creatureModel = null;
		if (flag)
		{
			text = this.RefineMessage(text, out creatureModel, param);
			if (writeLog)
			{
				if (creatureModel != null)
				{
					Notice.instance.Send("AddSystemLog", new object[]
					{
						creatureModel,
						text
					});
				}
				else
				{
					Notice.instance.Send("AddSystemLog", new object[]
					{
						text
					});
				}
			}
			if (angelaNarrate)
			{
				AngelaConversationUI.instance.AddAngelaMessage(text);
			}
			return text;
		}
		return string.Empty;
	}

	// Token: 0x06003407 RID: 13319 RVA: 0x0015B9B4 File Offset: 0x00159BB4
	public string MakeDefaultMessage(AngelaMessageState state, bool writeLog, bool angelaNarrate, params object[] param)
	{
		if (!writeLog && !angelaNarrate)
		{
			return string.Empty;
		}
		AngelaMessage message = this.messageLib.GetMessage(state);
		if (message == null)
		{
			Debug.LogError("Error in founding Message: " + state);
			return string.Empty;
		}
		bool flag = !(message.defaultFormat == string.Empty);
		string text = message.defaultFormat;
		CreatureModel creatureModel = null;
		if (flag)
		{
			text = this.RefineMessage(text, out creatureModel, param);
			if (writeLog)
			{
				if (creatureModel != null)
				{
					Notice.instance.Send("AddSystemLog", new object[]
					{
						creatureModel,
						text
					});
				}
				else
				{
					Notice.instance.Send("AddSystemLog", new object[]
					{
						text
					});
				}
			}
			if (angelaNarrate)
			{
				AngelaConversationUI.instance.AddAngelaMessage(text);
			}
			return text;
		}
		return string.Empty;
	}

	// Token: 0x06003408 RID: 13320 RVA: 0x0015BAA0 File Offset: 0x00159CA0
	public string RefineMessage(string origin, out CreatureModel model, params object[] param)
	{
		List<AgentModel> list = new List<AgentModel>();
		List<OfficerModel> list2 = new List<OfficerModel>();
		List<SkillTypeInfo> list3 = new List<SkillTypeInfo>();
		List<CreatureModel> list4 = new List<CreatureModel>();
		List<Sefira> list5 = new List<Sefira>();
		foreach (object obj in param)
		{
			if (obj is AgentModel)
			{
				list.Add(obj as AgentModel);
			}
			else if (obj is SkillTypeInfo)
			{
				list3.Add(obj as SkillTypeInfo);
			}
			else if (obj is CreatureModel)
			{
				list4.Add(obj as CreatureModel);
			}
			else if (obj is OfficerModel)
			{
				list2.Add(obj as OfficerModel);
			}
			else if (obj is Sefira)
			{
				list5.Add(obj as Sefira);
			}
		}
		string text = this.RefineAgentName(origin, list);
		text = this.RefineSkillName(text, list3);
		text = this.RefineCreatureName(text, list4);
		text = this.RefineOfficerName(text, list2);
		text = this.RefineSefiraName(text, list5);
		if (list4.Count != 0)
		{
			model = list4[0];
		}
		else
		{
			model = null;
		}
		return text;
	}

	// Token: 0x06003409 RID: 13321 RVA: 0x0015BBD8 File Offset: 0x00159DD8
	public string RefineOfficerName(string origin, List<OfficerModel> list)
	{
		int count = list.Count;
		string text = origin;
		string str = (!(GlobalGameManager.instance.GetCurrentLanguage() == "kr")) ? "Officer" : "사무직";
		for (int i = 0; i < count; i++)
		{
			text = text.Replace("#0$" + i, "<color=#66bfcd>" + str + "</color>");
		}
		return text;
	}

	// Token: 0x0600340A RID: 13322 RVA: 0x0015BC54 File Offset: 0x00159E54
	public string RefineAgentName(string origin, List<AgentModel> list)
	{
		int count = list.Count;
		string text = origin;
		for (int i = 0; i < count; i++)
		{
			text = text.Replace("#0$" + i, "<color=#66bfcd>" + list[i].name + "</color>");
		}
		return text;
	}

	// Token: 0x0600340B RID: 13323 RVA: 0x0015BCB0 File Offset: 0x00159EB0
	public string RefineAgentName(string origin, AgentModel target)
	{
		return origin.Replace("#0$0", "<color=#66bfcd>" + target.name + "</color>");
	}

	// Token: 0x0600340C RID: 13324 RVA: 0x0015BCE4 File Offset: 0x00159EE4
	public string RefineNarrationText(string origin, UseSkill skill)
	{
		string text = origin.Replace("#0$0", skill.agent.name);
		text = text.Replace("#2$0", skill.targetCreature.metaInfo.collectionName);
		return text.Replace("#5$0", skill.skillTypeInfo.name);
	}

	// Token: 0x0600340D RID: 13325 RVA: 0x0015BD40 File Offset: 0x00159F40
	public string RefineSkillName(string origin, List<SkillTypeInfo> list)
	{
		int count = list.Count;
		string text = origin;
		for (int i = 0; i < count; i++)
		{
			string text2 = list[i].calledName;
			string str = string.Empty;
			if (SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.MALKUT, false) || SefiraBossManager.Instance.IsKetherBoss(KetherBossType.E1))
			{
				text2 = "Unknown</color>";
			}
			else if (GlobalGameManager.instance.GetCurrentLanguage() == "kr")
			{
				str = text2[text2.Length - 1] + string.Empty;
				text2 = text2.Remove(text2.Length - 1);
				text2 += "</color>";
				text2 += str;
			}
			else
			{
				text2 += "</color>";
			}
			text = text.Replace("#1$" + i, "<color=#84bd36>" + text2);
		}
		return text;
	}

	// Token: 0x0600340E RID: 13326 RVA: 0x0015BE38 File Offset: 0x0015A038
	public string RefineSkillName(string origin, SkillTypeInfo target)
	{
		string text = target.calledName;
		string str = string.Empty;
		if (GlobalGameManager.instance.GetCurrentLanguage() == "kr")
		{
			str = text[text.Length - 1] + string.Empty;
			text = text.Remove(text.Length - 1);
			text += "</color>";
			text += str;
		}
		else
		{
			text += "</color>";
		}
		return origin.Replace("#1$0", "<color=#84bd36>" + text);
	}

	// Token: 0x0600340F RID: 13327 RVA: 0x0015BED8 File Offset: 0x0015A0D8
	public string RefineCreatureName(string origin, List<CreatureModel> list)
	{ // <Mod>
		bool isOvertimeYesod = SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.YESOD, true);
		int count = list.Count;
		string text = origin;
		for (int i = 0; i < count; i++)
		{
			if (isOvertimeYesod)
			{
				text = text.Replace("#2$" + i, "<color=#ef9696>????????</color>");
			}
			else
			{
				text = text.Replace("#2$" + i, "<color=#ef9696>" + list[i].GetUnitName() + "</color>");
			}
		}
		return text;
	}

	// Token: 0x06003410 RID: 13328 RVA: 0x0015BF34 File Offset: 0x0015A134
	public string RefineCreatureName(string origin, CreatureModel target)
	{ // <Mod>
		if (SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.YESOD, true))
		{
			return origin.Replace("#2$0", "<color=#ef9696>????????</color>");
		}
		return origin.Replace("#2$0", "<color=#ef9696>" + target.GetUnitName() + "</color>");
	}

	// Token: 0x06003411 RID: 13329 RVA: 0x0015BF68 File Offset: 0x0015A168
	public string RefineSkillOriginName(string origin, List<SkillTypeInfo> list)
	{
		int count = list.Count;
		string text = origin;
		for (int i = 0; i < count; i++)
		{
			text = text.Replace("#5$" + i, "<color=#84bd36>" + list[i].name + "</color>");
		}
		return text;
	}

	// Token: 0x06003412 RID: 13330 RVA: 0x0015BFC4 File Offset: 0x0015A1C4
	public string RefineSkillOriginName(string origin, SkillTypeInfo target)
	{
		return origin.Replace("#5$0", "<color=#84bd36>" + target.name + "</color>");
	}

	// Token: 0x06003413 RID: 13331 RVA: 0x0015BFF8 File Offset: 0x0015A1F8
	public string RefineSefiraName(string origin, List<Sefira> target)
	{
		int count = target.Count;
		string text = origin;
		for (int i = 0; i < count; i++)
		{
			text = text.Replace("#3$" + i, LocalizeTextDataModel.instance.GetTextAppend(new string[]
			{
				target[i].name,
				"Name"
			}));
		}
		return text;
	}

	// Token: 0x06003414 RID: 13332 RVA: 0x0015C060 File Offset: 0x0015A260
	public string RefineSefiraName(string origin, Sefira target)
	{
		return origin.Replace("#3$0", LocalizeTextDataModel.instance.GetTextAppend(new string[]
		{
			target.name,
			"Name"
		}));
	}

	// Token: 0x040030CC RID: 12492
	public AngelaMessageLib messageLib;

	// Token: 0x040030CD RID: 12493
	private static AngelaConversation _instance;

	// Token: 0x040030CE RID: 12494
	public Timer agentDeadTimer = new Timer();

	// Token: 0x040030CF RID: 12495
	private bool isLoaded;
}
