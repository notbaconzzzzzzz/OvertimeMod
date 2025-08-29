/*
public void Init() // 
public void OncePerSecond() // 
*/
using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009C4 RID: 2500
public class MissionUI : MonoBehaviour, IObserver
{
	// Token: 0x06004BAC RID: 19372 RVA: 0x0003ED9E File Offset: 0x0003CF9E
	public MissionUI()
	{
	}

	// Token: 0x170006FA RID: 1786
	// (get) Token: 0x06004BAD RID: 19373 RVA: 0x0003EDBC File Offset: 0x0003CFBC
	public static MissionUI instance
	{
		get
		{
			return MissionUI._instance;
		}
	}

	// Token: 0x06004BAE RID: 19374 RVA: 0x0003EDC3 File Offset: 0x0003CFC3
	public void Awake()
	{
		MissionUI._instance = this;
	}

	// Token: 0x06004BAF RID: 19375 RVA: 0x0003EDCB File Offset: 0x0003CFCB
	public void OnEnable()
	{
		this.anim.gameObject.SetActive(true);
		Notice.instance.Observe(NoticeName.OnMissionProgressed, this);
	}

	// Token: 0x06004BB0 RID: 19376 RVA: 0x0003EDEE File Offset: 0x0003CFEE
	public void OnDisable()
	{
		this.perSecondTimer.StopTimer();
		Notice.instance.Remove(NoticeName.OnMissionProgressed, this);
	}

	// Token: 0x06004BB1 RID: 19377 RVA: 0x001BDD3C File Offset: 0x001BBF3C
	public void Init()
	{ // <Mod> Multiline Missions
		this.perSecondTimer.Init();
		this.perSecondTimer.StartTimer(1f, new AutoTimer.TargetMethod(this.OncePerSecond), false, AutoTimer.UpdateMode.FIXEDUPDATE);
		foreach (MissionSlot missionSlot in this.missions)
		{
			UnityEngine.Object.Destroy(missionSlot.gameObject);
		}
		this.missions.Clear();
		List<Mission> missionsInProgress = MissionManager.instance.GetMissionsInProgress();
		if (SefiraBossManager.Instance.IsAnyBossSessionActivated())
		{
			Mission bossMission = MissionManager.instance.GetBossMission(SefiraBossManager.Instance.CurrentActivatedSefira);
			if (bossMission != null)
			{
				GameObject gameObject = Prefab.LoadPrefab("UIComponent/MissionSlot_List");
				CanvasGroup canvasGroup = gameObject.AddComponent<CanvasGroup>();
				canvasGroup.interactable = false;
				canvasGroup.blocksRaycasts = false;
				MissionSlot component = gameObject.GetComponent<MissionSlot>();
				component.Init(bossMission);
				this.missions.Add(component);
			}
		}
		if (!SefiraBossManager.Instance.IsAnyBossSessionActivated() || SpecialModeConfig.instance.GetValue<bool>("ReverseResearch"))
		{
			for (int i = 0; i < missionsInProgress.Count; i++)
			{
				Mission mission = missionsInProgress[i];
				if (mission.successCondition.condition_Type != ConditionType.DESTROY_CORE)
				{
					if (mission.missionScript != null && mission.missionScript.GetLineNum() > 1)
					{
						for (int j = 0; j < mission.missionScript.GetLineNum(); j++)
						{
							GameObject gameObject2 = Prefab.LoadPrefab("UIComponent/MissionSlot_List");
							CanvasGroup canvasGroup2 = gameObject2.AddComponent<CanvasGroup>();
							canvasGroup2.interactable = false;
							canvasGroup2.blocksRaycasts = false;
							MissionSlot component2 = gameObject2.GetComponent<MissionSlot>();
							component2.Init(mission, j);
							if (j == 0)
							{
								component2.rect.sizeDelta = new Vector2(component2.rect.sizeDelta.x, component2.rect.sizeDelta.y * 0.75f);
							}
							else if (j < mission.missionScript.GetLineNum() - 1)
							{
								component2.rect.sizeDelta = new Vector2(component2.rect.sizeDelta.x, component2.rect.sizeDelta.y * 0.5f);
							}
							missions.Add(component2);
						}
					}
					else
					{
						GameObject gameObject2 = Prefab.LoadPrefab("UIComponent/MissionSlot_List");
						CanvasGroup canvasGroup2 = gameObject2.AddComponent<CanvasGroup>();
						canvasGroup2.interactable = false;
						canvasGroup2.blocksRaycasts = false;
						MissionSlot component2 = gameObject2.GetComponent<MissionSlot>();
						component2.Init(mission);
						this.missions.Add(component2);
					}
				}
			}
		}
		this.SetList();
	}

	// Token: 0x06004BB2 RID: 19378 RVA: 0x001BDED4 File Offset: 0x001BC0D4
	public void SetList()
	{
		float x = this.DefaultPos.x;
		float num = this.DefaultPos.y;
		float num2 = 0f;
		for (int i = 0; i < this.missions.Count; i++)
		{
			MissionSlot missionSlot = this.missions[i];
			missionSlot.txt.color = new Color(255f, 255f, 255f, 1f);
			missionSlot.rect.SetParent(this.listParent);
			missionSlot.rect.localRotation = Quaternion.identity;
			missionSlot.rect.localScale = new Vector3(1f, 1f, 1f);
			num2 = this.Spacing.y + missionSlot.rect.rect.height * missionSlot.rect.localScale.y;
			num -= num2;
		}
		this.listParent.sizeDelta = new Vector2(this.listParent.sizeDelta.x, num2);
	}

	// Token: 0x06004BB3 RID: 19379 RVA: 0x001BDFF4 File Offset: 0x001BC1F4
	public void OnNotice(string notice, params object[] param)
	{
		if (notice == NoticeName.OnMissionProgressed)
		{
			foreach (MissionSlot missionSlot in this.missions)
			{
				missionSlot.Refresh();
			}
		}
	}

	// Token: 0x06004BB4 RID: 19380 RVA: 0x001BE060 File Offset: 0x001BC260
	public void OnClickButton()
	{
		this.anim.SetBool("Appear", !this.anim.GetBool("Appear"));
		Debug.Log("Mouse click " + this.anim.GetBool("Appear"));
	}

	// Token: 0x06004BB5 RID: 19381 RVA: 0x0000431D File Offset: 0x0000251D
	public void OnEnterButton()
	{
	}

	// Token: 0x06004BB6 RID: 19382 RVA: 0x0000431D File Offset: 0x0000251D
	public void OnExitButton()
	{
	}

	// Token: 0x06004BB7 RID: 19383 RVA: 0x001BE0B4 File Offset: 0x001BC2B4
	public void OncePerSecond()
	{ // <Mod>
		this.perSecondTimer.StartTimer(1f, new AutoTimer.TargetMethod(this.OncePerSecond), false, AutoTimer.UpdateMode.FIXEDUPDATE);
		foreach (MissionSlot missionSlot in this.missions)
		{
			if (missionSlot.hasTimerCondition)
			{
				missionSlot.Refresh();
			}
		}
	}

	// Token: 0x06004BB8 RID: 19384 RVA: 0x0000431D File Offset: 0x0000251D
	// Note: this type is marked as 'beforefieldinit'.
	static MissionUI()
	{
	}

	// Token: 0x04004630 RID: 17968
	private static MissionUI _instance;

	// Token: 0x04004631 RID: 17969
	private const string MIssionSlotSrc = "UIComponent/MissionSlot_List";

	// Token: 0x04004632 RID: 17970
	public RectTransform listParent;

	// Token: 0x04004633 RID: 17971
	public Vector2 Spacing;

	// Token: 0x04004634 RID: 17972
	public Vector2 DefaultPos;

	// Token: 0x04004635 RID: 17973
	public Animator anim;

	// Token: 0x04004636 RID: 17974
	private AutoTimer perSecondTimer = new AutoTimer();

	// Token: 0x04004637 RID: 17975
	private const float oneSecond = 1f;

	// Token: 0x04004638 RID: 17976
	private List<MissionSlot> missions = new List<MissionSlot>();
}
