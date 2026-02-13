using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A04 RID: 2564
public class SefiraConversationController : MonoBehaviour, IObserver
{
	// Token: 0x06004E3E RID: 20030 RVA: 0x00040761 File Offset: 0x0003E961
	public SefiraConversationController()
	{
	}

	// Token: 0x1700073F RID: 1855
	// (get) Token: 0x06004E3F RID: 20031 RVA: 0x00040774 File Offset: 0x0003E974
	public static SefiraConversationController Instance
	{
		get
		{
			return SefiraConversationController._instance;
		}
	}

	// Token: 0x06004E40 RID: 20032 RVA: 0x001D3C6C File Offset: 0x001D1E6C
	private void Awake()
	{
		SefiraConversationController._instance = this;
		Notice.instance.Observe(NoticeName.OnAgentDead, this);
		Notice.instance.Observe(NoticeName.OnAgentPanic, this);
		Notice.instance.Observe(NoticeName.OnEscape, this);
		Notice.instance.Observe(NoticeName.RabbitCaptainConversation, this);
		Notice.instance.Observe(NoticeName.RabbitProtocolActivated, this);
		Notice.instance.Observe(NoticeName.UnmuteSefiraConversation, this);
		Notice.instance.Observe(NoticeName.KetherConversation, this);
		this._conversationPrefab = Resources.Load("Prefabs/UIComponent/Window/ConversationUI");
		this.mutedSefira.Clear();
	}

	// Token: 0x06004E41 RID: 20033 RVA: 0x0004077B File Offset: 0x0003E97B
	private void Start()
	{
		this.count = 0;
	}

	// Token: 0x06004E42 RID: 20034 RVA: 0x001D3D0C File Offset: 0x001D1F0C
	private void OnDestroy()
	{
		Notice.instance.Remove(NoticeName.OnAgentDead, this);
		Notice.instance.Remove(NoticeName.OnAgentPanic, this);
		Notice.instance.Remove(NoticeName.OnEscape, this);
		Notice.instance.Remove(NoticeName.RabbitCaptainConversation, this);
		Notice.instance.Remove(NoticeName.RabbitProtocolActivated, this);
		Notice.instance.Remove(NoticeName.UnmuteSefiraConversation, this);
		Notice.instance.Remove(NoticeName.KetherConversation, this);
		this.mutedSefira.Clear();
	}

	// Token: 0x06004E43 RID: 20035 RVA: 0x001D3D94 File Offset: 0x001D1F94
	public bool CheckMuted(int index)
	{
		Sefira sefira = SefiraManager.instance.GetSefira(index);
		if (sefira != null && sefira.sefiraEnum == SefiraEnum.KETHER)
		{
			return true;
		}
		try
		{
			if (this.mutedSefira.Contains(sefira.sefiraEnum))
			{
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
		}
		return false;
	}

	// Token: 0x06004E44 RID: 20036 RVA: 0x001D3E00 File Offset: 0x001D2000
	public void OnNotice(string notice, params object[] param)
	{ // <Mod>
		if (notice == NoticeName.RabbitProtocolActivated)
		{
			foreach (SefiraEnum sefiraEnum in (SefiraEnum[])param[0])
			{
				if (sefiraEnum != SefiraEnum.KETHER && !this.mutedSefira.Contains(sefiraEnum))
				{
					this.mutedSefira.Add(sefiraEnum);
				}
			}
			return;
		}
		if (notice == NoticeName.UnmuteSefiraConversation)
		{
			SefiraEnum item = (SefiraEnum)param[0];
			if (this.mutedSefira.Contains(item))
			{
				this.mutedSefira.Remove(item);
			}
			return;
		}
		SefiraMessage sefiraMessage = new SefiraMessage();
		int num = 1;
		string text = sefiraMessage.desc;
		if (notice == NoticeName.OnAgentDead)
		{
			AgentModel agentModel = param[0] as AgentModel;
			num = agentModel.GetCurrentSefira().index;
			if (this.CheckMuted(num))
			{
				return;
			}
			if (num == 5 || num == 6)
			{
				bool isRobot = MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.TIPERERTH1);
				List<AgentModel> list = new List<AgentModel>();
				int num2 = UnityEngine.Random.Range(0, 2);
				if (num2 == 0)
				{
					num = 5;
				}
				else
				{
					num = 6;
				}
				list.AddRange(SefiraManager.instance.GetSefira(5).agentList);
				list.AddRange(SefiraManager.instance.GetSefira(6).agentList);
				int num3 = 0;
				using (List<AgentModel>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.IsDead())
						{
							num3++;
						}
					}
				}
				if (num3 == list.Count)
				{
					sefiraMessage = Conversation.instance.GetSefiraMessage(num, 3, num2, isRobot);
				}
				else
				{
					sefiraMessage = Conversation.instance.GetSefiraMessage(num, 1, num2, isRobot);
				}
				text = sefiraMessage.desc;
				text = text.Replace("#0", "<color=#66bfcd>" + agentModel.name + "</color>");
			}
			else
			{
				bool isRobot2 = MissionManager.instance.ExistsFinishedBossMission(agentModel.GetCurrentSefira().sefiraEnum);
				List<AgentModel> agentList = SefiraManager.instance.GetSefira(num).agentList;
				int num4 = 0;
				using (List<AgentModel>.Enumerator enumerator2 = agentList.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.IsDead())
						{
							num4++;
						}
					}
				}
				if (num4 == agentList.Count)
				{
					sefiraMessage = Conversation.instance.GetSefiraMessage(num, 3, isRobot2);
				}
				else
				{
					sefiraMessage = Conversation.instance.GetSefiraMessage(num, 1, isRobot2);
				}
				text = sefiraMessage.desc;
				text = text.Replace("#0", "<color=#66bfcd>" + agentModel.name + "</color>");
			}
		}
		else if (notice == NoticeName.OnAgentPanic)
		{
			AgentModel agentModel2 = param[0] as AgentModel;
			num = agentModel2.GetCurrentSefira().index;
			if (this.CheckMuted(num))
			{
				return;
			}
			if (num == 5 || num == 6)
			{
				bool isRobot3 = MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.TIPERERTH1);
				int num5 = UnityEngine.Random.Range(0, 2);
				if (num5 == 0)
				{
					num = 5;
				}
				else
				{
					num = 6;
				}
				sefiraMessage = Conversation.instance.GetSefiraMessage(num, 0, num5, isRobot3);
				text = sefiraMessage.desc;
				text = text.Replace("#0", "<color=#66bfcd>" + agentModel2.name + "</color>");
			}
			else
			{
				bool isRobot4 = MissionManager.instance.ExistsFinishedBossMission(agentModel2.GetCurrentSefira().sefiraEnum);
				sefiraMessage = Conversation.instance.GetSefiraMessage(num, 0, isRobot4);
				text = sefiraMessage.desc;
				text = text.Replace("#0", "<color=#66bfcd>" + agentModel2.name + "</color>");
			}
		}
		else if (notice == NoticeName.OnEscape)
		{
			if (param[0] is ChildCreatureModel)
			{
				return;
			}
			CreatureModel creatureModel = param[0] as CreatureModel;
			num = creatureModel.sefira.index;
			if (this.CheckMuted(num))
			{
				return;
			}
			if (num == 5 || num == 6)
			{
				bool isRobot5 = MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.TIPERERTH1);
				int num6 = UnityEngine.Random.Range(0, 2);
				if (num6 == 0)
				{
					num = 5;
				}
				else
				{
					num = 6;
				}
				sefiraMessage = Conversation.instance.GetSefiraMessage(num, 2, num6, isRobot5);
				text = sefiraMessage.desc;
				text = text.Replace("$0", "<color=#66bfcd>" + creatureModel.GetUnitName() + "</color>");
			}
			else
			{
				bool isRobot6 = MissionManager.instance.ExistsFinishedBossMission(creatureModel.sefira.sefiraEnum);
				sefiraMessage = Conversation.instance.GetSefiraMessage(num, 2, isRobot6);
				text = sefiraMessage.desc;
				text = text.Replace("$0", "<color=#66bfcd>" + creatureModel.GetUnitName() + "</color>");
			}
		}
		else
		{
			if (notice == NoticeName.RabbitCaptainConversation)
			{
				string character = "Rabbit";
				Sprite portrait = CharacterResourceDataModel.instance.GetPortrait(character);
				Color color = CharacterResourceDataModel.instance.GetColor(character);
				string d = (string)param[0];
				this.UpdateConversation(portrait, color, d);
				return;
			}
			if (notice == NoticeName.KetherConversation)
			{
				string character2 = "Kether";
				Sprite portrait2 = CharacterResourceDataModel.instance.GetPortrait(character2);
				Color color2 = CharacterResourceDataModel.instance.GetColor(character2);
				string d2 = (string)param[0];
				this.UpdateConversation(portrait2, color2, d2);
				return;
			}
		}
		Sefira sefira = SefiraManager.instance.GetSefira(num);
		if (SefiraBossManager.Instance.IsAnyBossSessionActivated() || PlayerModel.instance.GetDay() >= 45)
		{
			return;
		}
        if (SpecialModeConfig.instance.GetValue<bool>("ReverseResearch"))
        {
            if (MissionManager.instance.GetClearedOrClosedMissions().Exists((Mission x) => x.metaInfo.sefira == sefira.sefiraEnum && x.metaInfo.sefira_Level == 6))
            {
                return;
            }
        }
		else if (MissionManager.instance.ExistsBossMission(sefira.sefiraEnum))
		{
			return;
		}
		string name = sefira.name;
		Sprite sefiraPortrait = CharacterResourceDataModel.instance.GetSefiraPortrait(sefira.sefiraEnum, false);
		Color color3 = CharacterResourceDataModel.instance.GetColor(name);
		this.UpdateConversation(sefiraPortrait, color3, text);
	}

	// Token: 0x06004E45 RID: 20037 RVA: 0x001D438C File Offset: 0x001D258C
	public void UpdateConversation(Sprite s, Color c, string d)
	{
		Vector3 localPosition = this._layout.GetChild(this._layout.childCount - 1).transform.localPosition;
		this.count++;
		base.StopCoroutine(this.MoveDown());
		base.StartCoroutine(this.MoveDown());
		this.asd = (UnityEngine.Object.Instantiate(this._conversationPrefab, this._layout) as GameObject);
		this.asd.transform.GetChild(0).GetComponent<ConversationUI>().InitUI(s, c, d);
		this.asd.transform.localPosition = new Vector3(localPosition.x, localPosition.y + 105f, localPosition.z);
		if (this._layout.childCount > 7)
		{
			UnityEngine.Object.Destroy(this._layout.GetChild(0).gameObject);
		}
	}

	// Token: 0x06004E46 RID: 20038 RVA: 0x001D4474 File Offset: 0x001D2674
	private IEnumerator MoveDown()
	{
		RectTransform rect = this._layout.GetComponent<RectTransform>();
		float unit = 15f;
		while (rect.anchoredPosition.y > (float)(-(float)this.count) * 105f)
		{
			rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y - unit);
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06004E47 RID: 20039 RVA: 0x001D4490 File Offset: 0x001D2690
	private void MoveDownUpdate()
	{
		RectTransform component = this._layout.GetComponent<RectTransform>();
		float num = 15f;
		if (component.anchoredPosition.y > (float)(-(float)this.count) * 105f)
		{
			component.anchoredPosition = new Vector2(component.anchoredPosition.x, component.anchoredPosition.y - num);
		}
		else
		{
			component.anchoredPosition = new Vector2(component.anchoredPosition.x, (float)(-(float)this.count) * 105f);
		}
	}

	// Token: 0x06004E48 RID: 20040 RVA: 0x00040784 File Offset: 0x0003E984
	private void Update()
	{
		this.MoveDownUpdate();
	}

	// Token: 0x06004E49 RID: 20041 RVA: 0x00004401 File Offset: 0x00002601
	static SefiraConversationController()
	{
	}

	// Token: 0x0400485C RID: 18524
	private static SefiraConversationController _instance;

	// Token: 0x0400485D RID: 18525
	public Transform _layout;

	// Token: 0x0400485E RID: 18526
	private UnityEngine.Object _conversationPrefab;

	// Token: 0x0400485F RID: 18527
	private List<SefiraEnum> mutedSefira = new List<SefiraEnum>();

	// Token: 0x04004860 RID: 18528
	private int count;

	// Token: 0x04004861 RID: 18529
	private GameObject asd;

	// Token: 0x02000A05 RID: 2565
	private enum ConversationType
	{
		// Token: 0x04004863 RID: 18531
		PANIC,
		// Token: 0x04004864 RID: 18532
		DEAD,
		// Token: 0x04004865 RID: 18533
		ESCAPE,
		// Token: 0x04004866 RID: 18534
		ALLDEAD
	}
}
