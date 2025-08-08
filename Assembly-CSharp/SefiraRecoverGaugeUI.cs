/*
public void OnDisplayEffectInfo() // 
*/
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020009C6 RID: 2502
public class SefiraRecoverGaugeUI : MonoBehaviour, IObserver
{
	// Token: 0x06004BC1 RID: 19393 RVA: 0x001BE334 File Offset: 0x001BC534
	public SefiraRecoverGaugeUI()
	{
	}

	// Token: 0x170006FB RID: 1787
	// (get) Token: 0x06004BC2 RID: 19394 RVA: 0x0003EE80 File Offset: 0x0003D080
	private Sefira Sefira
	{
		get
		{
			return this._sefira;
		}
	}

	// Token: 0x06004BC3 RID: 19395 RVA: 0x001BE39C File Offset: 0x001BC59C
	private void Update()
	{
		Sefira sefira = SefiraManager.instance.GetSefira(this.sefiraEnum);
		if (sefira == null)
		{
			return;
		}
		if (this._recoverActivated != sefira.isRecoverActivated)
		{
			this.SetActivate(sefira.isRecoverActivated);
		}
		this.gauge.fillAmount = sefira.recoverElapsedTime / 10f;
		int officerAliveLevel = sefira.GetOfficerAliveLevel();
		if (this._oldOfficerAliveLevel != officerAliveLevel)
		{
			for (int i = 0; i < this.officerAliveLevels.Length; i++)
			{
				if (i == officerAliveLevel)
				{
					this.officerAliveLevels[i].gameObject.SetActive(true);
				}
				else
				{
					this.officerAliveLevels[i].gameObject.SetActive(false);
				}
			}
			this._oldOfficerAliveLevel = officerAliveLevel;
		}
	}

	// Token: 0x06004BC4 RID: 19396 RVA: 0x001BE45C File Offset: 0x001BC65C
	private void OnStageStart()
	{
		this._oldOfficerAliveLevel = -1;
		if (this.sefiraEnum == SefiraEnum.DAAT)
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.SetActivate(true);
		this.frame.color = UIColorManager.instance.GetSefiraColor(this.sefiraEnum).imageColor;
		if (this.frameTop != null)
		{
			this.frameTop.color = UIColorManager.instance.GetSefiraColor(this.sefiraEnum).imageColor;
		}
		if (this.sefiraReturnBtnRoot != null)
		{
			if (ResearchDataModel.instance.IsUpgradedAbility("sefira_return") || (GlobalGameManager.instance.gameMode == GameMode.TUTORIAL && GlobalGameManager.instance.tutorialStep > 1))
			{
				this.sefiraReturnBtnRoot.SetActive(true);
			}
			else
			{
				this.sefiraReturnBtnRoot.SetActive(false);
			}
		}
		this._sefira = SefiraManager.instance.GetSefira(this.sefiraEnum);
		try
		{
			Transform child = base.transform.GetChild(0).GetChild(2);
			child.GetComponent<MaskableGraphic>().raycastTarget = false;
			IEnumerator enumerator = child.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					MaskableGraphic component = transform.GetComponent<MaskableGraphic>();
					if (component != null)
					{
						component.raycastTarget = false;
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			this.AttachEvent();
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	// Token: 0x06004BC5 RID: 19397 RVA: 0x001BE608 File Offset: 0x001BC808
	private void AttachEvent()
	{
		EventTrigger eventTrigger = this.frameTop.GetComponent<EventTrigger>();
		bool flag = false;
		if (eventTrigger == null)
		{
			flag = true;
			eventTrigger = this.frameTop.gameObject.AddComponent<EventTrigger>();
		}
		string b = "OnDisplayEffectInfo";
		string b2 = "OnClearEffectInfo";
		EventTrigger.Entry entry = null;
		EventTrigger.Entry entry2 = null;
		bool flag2 = false;
		bool flag3 = false;
		if (!flag)
		{
			foreach (EventTrigger.Entry entry3 in eventTrigger.triggers)
			{
				if (entry3.eventID == EventTriggerType.PointerEnter)
				{
					entry = entry3;
				}
				if (entry3.eventID == EventTriggerType.PointerExit)
				{
					entry2 = entry3;
				}
				if (entry2 != null && entry != null)
				{
					break;
				}
			}
			int persistentEventCount = entry.callback.GetPersistentEventCount();
			for (int i = 0; i < persistentEventCount; i++)
			{
				string persistentMethodName = entry.callback.GetPersistentMethodName(i);
				if (persistentMethodName == b)
				{
					flag2 = true;
					break;
				}
			}
			persistentEventCount = entry2.callback.GetPersistentEventCount();
			for (int j = 0; j < persistentEventCount; j++)
			{
				string persistentMethodName2 = entry2.callback.GetPersistentMethodName(j);
				if (persistentMethodName2 == b2)
				{
					flag3 = true;
					break;
				}
			}
		}
		else
		{
			entry = new EventTrigger.Entry
			{
				eventID = EventTriggerType.PointerEnter
			};
			entry2 = new EventTrigger.Entry
			{
				eventID = EventTriggerType.PointerExit
			};
		}
		if (!flag2)
		{
			entry.callback.AddListener(delegate(BaseEventData newEvent)
			{
				this.OnDisplayEffectInfo();
			});
		}
		if (!flag3)
		{
			entry2.callback.AddListener(delegate(BaseEventData newEvent)
			{
				this.OnClearEffectInfo();
			});
		}
		if (flag)
		{
			eventTrigger.triggers.Add(entry);
			eventTrigger.triggers.Add(entry2);
		}
	}

	// Token: 0x06004BC6 RID: 19398 RVA: 0x001BE7FC File Offset: 0x001BC9FC
	public void OnDisplayEffectInfo()
	{ // <Mod>
		int officerAliveLevel = this.Sefira.GetOfficerAliveLevel();
		if (officerAliveLevel == 0)
		{
			OverlayManager.Instance.SetText(LocalizeTextDataModel.instance.GetText("officer_alive_ability_zero"));
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		string arg = string.Format(LocalizeTextDataModel.instance.GetText(SefiraRecoverGaugeUI.f1), officerAliveLevel);
		string text = LocalizeTextDataModel.instance.GetText(SefiraRecoverGaugeUI.f2);
        int num = 1;
        if (ResearchDataModel.instance.IsUpgradedAbility("upgrade_officer_bonuses"))
        {
            num = 2;
        }
		string arg2 = string.Format(LocalizeTextDataModel.instance.GetText(SefiraRecoverGaugeUI.f3 + this.Sefira.name.ToLower()), SefiraAbilityValueInfo.GetOfficerAliveValues(this.sefiraEnum)[officerAliveLevel] * num);
		stringBuilder.AppendFormat("{0}\n\n{1}\n{2}", arg, text, arg2);
		OverlayManager.Instance.SetText(stringBuilder.ToString());
	}

	// Token: 0x06004BC7 RID: 19399 RVA: 0x0003EE88 File Offset: 0x0003D088
	public void OnClearEffectInfo()
	{
		OverlayManager.Instance.ClearOverlay();
	}

	// Token: 0x06004BC8 RID: 19400 RVA: 0x001BE8C4 File Offset: 0x001BCAC4
	private void SetActivate(bool b)
	{
		this._recoverActivated = b;
		if (b)
		{
			this.gauge.color = this.activateColor;
			this.recoverX.SetActive(true);
			this.recoverXDisable.SetActive(false);
		}
		else if (ResearchDataModel.instance.IsUpgradedAbility("regeneration_upgrade"))
		{
			this.gauge.color = this.slowActivateColor;
			this.recoverX.SetActive(true);
			this.recoverXDisable.SetActive(false);
		}
		else
		{
			this.gauge.color = this.deactivateColor;
			this.recoverX.SetActive(false);
			this.recoverXDisable.SetActive(true);
		}
	}

	// Token: 0x06004BC9 RID: 19401 RVA: 0x0003EE94 File Offset: 0x0003D094
	private void OnEnable()
	{
		Notice.instance.Observe(NoticeName.OnStageStart, this);
	}

	// Token: 0x06004BCA RID: 19402 RVA: 0x0003CA92 File Offset: 0x0003AC92
	private void OnDisable()
	{
		Notice.instance.Remove(NoticeName.OnStageStart, this);
	}

	// Token: 0x06004BCB RID: 19403 RVA: 0x001BE978 File Offset: 0x001BCB78
	public void OnClickSefiraReturn()
	{
		Sefira sefira = SefiraManager.instance.GetSefira(this.sefiraEnum);
		if (sefira != null)
		{
			sefira.ReturnAgentsToSefira();
		}
	}

	// Token: 0x06004BCC RID: 19404 RVA: 0x0003EEA6 File Offset: 0x0003D0A6
	public void OnNotice(string notice, params object[] param)
	{
		if (notice == NoticeName.OnStageStart)
		{
			this.OnStageStart();
		}
	}

	// Token: 0x06004BCD RID: 19405 RVA: 0x0003EEBE File Offset: 0x0003D0BE
	// Note: this type is marked as 'beforefieldinit'.
	static SefiraRecoverGaugeUI()
	{
	}

	// Token: 0x04004642 RID: 17986
	public SefiraEnum sefiraEnum;

	// Token: 0x04004643 RID: 17987
	public GameObject recoverX;

	// Token: 0x04004644 RID: 17988
	public GameObject recoverXDisable;

	// Token: 0x04004645 RID: 17989
	public Image frame;

	// Token: 0x04004646 RID: 17990
	public Image gauge;

	// Token: 0x04004647 RID: 17991
	public Image frameTop;

	// Token: 0x04004648 RID: 17992
	public Image[] officerAliveLevels;

	// Token: 0x04004649 RID: 17993
	public GameObject sefiraReturnBtnRoot;

	// Token: 0x0400464A RID: 17994
	public Canvas buttonCanvas;

	// Token: 0x0400464B RID: 17995
	private Color activateColor = new Color(0.14509805f, 1f, 0.34117648f);

	// Token: 0x0400464C RID: 17996
	private Color deactivateColor = new Color(0.27058825f, 0.101960786f, 0.0627451f);

	// Token: 0x0400464D RID: 17997
	private Color slowActivateColor = new Color(0.23137255f, 0.45490196f, 0.19215687f);

	// Token: 0x0400464E RID: 17998
	private bool _recoverActivated;

	// Token: 0x0400464F RID: 17999
	private int _oldOfficerAliveLevel = -1;

	// Token: 0x04004650 RID: 18000
	private Sefira _sefira;

	// Token: 0x04004651 RID: 18001
	private static string f1 = "officer_alive_ability_level_txt";

	// Token: 0x04004652 RID: 18002
	private static string f2 = "officer_alive_ability_effect";

	// Token: 0x04004653 RID: 18003
	private static string f3 = "officer_alive_ability_cur_";
}
