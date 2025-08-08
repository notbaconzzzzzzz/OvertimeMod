/*
private void UpdateText() // 
public void OnClickCommand() // 
*/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rabbit
{
	// Token: 0x02000A3F RID: 2623
	public class RabbitProtocolWindow : MonoBehaviour, IObserver
	{
		// Token: 0x06004ED6 RID: 20182 RVA: 0x001CDA6C File Offset: 0x001CBC6C
		public RabbitProtocolWindow()
		{
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06004ED7 RID: 20183 RVA: 0x00040F27 File Offset: 0x0003F127
		public static RabbitProtocolWindow Window
		{
			get
			{
				return RabbitProtocolWindow._window;
			}
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x06004ED8 RID: 20184 RVA: 0x00040F2E File Offset: 0x0003F12E
		// (set) Token: 0x06004ED9 RID: 20185 RVA: 0x00040F36 File Offset: 0x0003F136
		public bool Activated
		{
			get
			{
				return this._activated;
			}
			private set
			{
				this._activated = value;
				this.root.SetActive(value);
				if (this._activated)
				{
					this.Calculate();
				}
				else
				{
					this._uiUpdateTimer.StopTimer();
				}
			}
		}

		// Token: 0x06004EDA RID: 20186 RVA: 0x001CDAC4 File Offset: 0x001CBCC4
		private void Awake()
		{
			if (RabbitProtocolWindow.Window != null)
			{
				try
				{
					UnityEngine.Object.Destroy(RabbitProtocolWindow.Window.gameObject);
				}
				catch (Exception ex)
				{
				}
			}
			RabbitProtocolWindow._window = this;
		}

		// Token: 0x06004EDB RID: 20187 RVA: 0x001CDB14 File Offset: 0x001CBD14
		private void Start()
		{
			Notice.instance.Observe(NoticeName.OnStageStart, this);
			Notice.instance.Observe(NoticeName.OnStageEnd, this);
			this._uiUpdateTimer.StartTimer(this.freq);
			this.Activated = false;
			this.activatedAnim.gameObject.SetActive(false);
			this.ProtocolActivationButton.gameObject.SetActive(false);
			this.ClearFilter();
		}

		// Token: 0x06004EDC RID: 20188 RVA: 0x00040F6C File Offset: 0x0003F16C
		private void OnDestroy()
		{
			this.DestroyNotice();
			this.filter.Clear();
		}

		// Token: 0x06004EDD RID: 20189 RVA: 0x00040F7F File Offset: 0x0003F17F
		private void DestroyNotice()
		{
			Notice.instance.Remove(NoticeName.OnStageStart, this);
			Notice.instance.Remove(NoticeName.OnStageEnd, this);
		}

		// Token: 0x06004EDE RID: 20190 RVA: 0x001CDB84 File Offset: 0x001CBD84
		public void OnStageStart()
		{
			if (this.enabledSefiraList == null)
			{
				this.enabledSefiraList = new List<RabbitProtocolSefiraSlot>();
			}
			else
			{
				this.enabledSefiraList.Clear();
			}
			foreach (RabbitProtocolSefiraSlot rabbitProtocolSefiraSlot in this.sefiraList)
			{
				int openLevel = rabbitProtocolSefiraSlot.Sefira.openLevel;
				SlotState state;
				if (openLevel == 0)
				{
					state = SlotState.NOTOPENED;
				}
				else if (openLevel >= 4 || rabbitProtocolSefiraSlot.Sefira.sefiraEnum == SefiraEnum.KETHER)
				{
					state = SlotState.ACTIVATED;
					this.enabledSefiraList.Add(rabbitProtocolSefiraSlot);
				}
				else
				{
					state = SlotState.NOTAVAILABLE;
				}
				rabbitProtocolSefiraSlot.Init(state);
			}
			if (ResearchDataModel.instance.IsUpgradedAbility("rabbit_protocol"))
			{
				this.ProtocolActivationButton.gameObject.SetActive(true);
				this.ProtocolActivationButton.interactable = true;
			}
			else
			{
				this.ProtocolActivationButton.gameObject.SetActive(false);
				this.ProtocolActivationButton.interactable = false;
			}
		}

		// Token: 0x06004EDF RID: 20191 RVA: 0x001CDCA4 File Offset: 0x001CBEA4
		private void Update()
		{
			if (this._uiUpdateTimer.RunTimer())
			{
				this.Calculate();
			}
			if (this.Activated)
			{
				this.ConfirmButton.interactable = (this.currentSelected.Count > 0);
			}
			if (this._filterAlphaTimer.started)
			{
				if (this.filter.Count == 0)
				{
					this._filterAlphaTimer.StopTimer();
					return;
				}
				float filterAlpha = this.filterAlphaCurve.Evaluate(this._filterAlphaTimer.Rate);
				this.SetFilterAlpha(filterAlpha);
				if (this._filterAlphaTimer.RunTimer())
				{
					this._filterAlphaTimer.StartTimer(this.filterFreq);
				}
			}
		}

		// Token: 0x06004EE0 RID: 20192 RVA: 0x001CDD58 File Offset: 0x001CBF58
		private void UpdateText()
		{ // <Mod>
			this.Count.text = string.Format("{0}/{1}", this.currentSelected.Count, 4);
			float num = (float)this.currentSelected.Count * 0.25f;
			num /= StageTypeInfo.instnace.GetPercentEnergyFactor();
			if (ResearchDataModel.instance.IsUpgradedAbility("energy_discount"))
			{
				num *= 0.9f;
			}
			float energy = EnergyModel.instance.GetEnergy();
			float num2 = energy * num;
			this.Cost.text = ((int)num2).ToString();
			this.CostText.text = string.Format(LocalizeTextDataModel.instance.GetText("Rabbit_Info_CostCalc"), Mathf.RoundToInt(num * 100f));
		}

		// Token: 0x06004EE1 RID: 20193 RVA: 0x001CDDFC File Offset: 0x001CBFFC
		private void SetFilterAlpha(float value)
		{
			foreach (List<SefiraFilterMgr> list in this.filter.Values)
			{
				foreach (SefiraFilterMgr sefiraFilterMgr in list)
				{
					sefiraFilterMgr.SetAlpha(value, this.rootFilterUpdate);
				}
			}
		}

		// Token: 0x06004EE2 RID: 20194 RVA: 0x001CDEA4 File Offset: 0x001CC0A4
		private void Calculate()
		{
			Dictionary<SefiraEnum, RabbitProtocolWindow.SefiraInfo> dictionary = this.CalculateUnits();
			foreach (RabbitProtocolSefiraSlot rabbitProtocolSefiraSlot in this.enabledSefiraList)
			{
				RabbitProtocolWindow.SefiraInfo list = null;
				if (dictionary.TryGetValue(rabbitProtocolSefiraSlot.sefiraEnum, out list))
				{
					rabbitProtocolSefiraSlot.UpdateData(list);
				}
			}
			this._uiUpdateTimer.StartTimer(this.freq);
		}

		// Token: 0x06004EE3 RID: 20195 RVA: 0x001CDF34 File Offset: 0x001CC134
		public void OnOpen()
		{
			this.Activated = true;
			this.protocolActivated = false;
			this.windowAnim.Show();
			this.currentSelected.Clear();
			foreach (RabbitProtocolSefiraSlot rabbitProtocolSefiraSlot in this.sefiraList)
			{
				rabbitProtocolSefiraSlot.OnOpenWindow();
			}
			this.UpdateText();
		}

		// Token: 0x06004EE4 RID: 20196 RVA: 0x00040FA1 File Offset: 0x0003F1A1
		public void OnClose()
		{
			this.windowAnim.Hide();
		}

		// Token: 0x06004EE5 RID: 20197 RVA: 0x00040FAE File Offset: 0x0003F1AE
		public void OnWindowClosed()
		{
			if (this.protocolActivated)
			{
				return;
			}
			this.Activated = false;
		}

		// Token: 0x06004EE6 RID: 20198 RVA: 0x00040FC3 File Offset: 0x0003F1C3
		public void CloseProtocolActivatedWindow()
		{
			this.protocolActivated = false;
			this.Activated = false;
		}

		// Token: 0x06004EE7 RID: 20199 RVA: 0x001CDFBC File Offset: 0x001CC1BC
		public bool OnTrySelectArea(SefiraEnum sefira, bool currentState)
		{
			if (currentState)
			{
				if (this.currentSelected.Contains(sefira))
				{
					this.currentSelected.Remove(sefira);
					if (sefira == SefiraEnum.TIPERERTH1)
					{
						this.currentSelected.Remove(SefiraEnum.TIPERERTH2);
					}
				}
				this.UpdateText();
				return false;
			}
			if (this.currentSelected.Count >= 4)
			{
				return false;
			}
			if (sefira == SefiraEnum.TIPERERTH1 && this.currentSelected.Count >= 3)
			{
				return false;
			}
			if (!this.currentSelected.Contains(sefira))
			{
				this.currentSelected.Add(sefira);
				if (sefira == SefiraEnum.TIPERERTH1)
				{
					this.currentSelected.Add(SefiraEnum.TIPERERTH2);
				}
			}
			this.UpdateText();
			return true;
		}

		// Token: 0x06004EE8 RID: 20200 RVA: 0x001CE070 File Offset: 0x001CC270
		public RabbitProtocolSefiraSlot GetSlot(SefiraEnum sefira)
		{
			RabbitProtocolSefiraSlot result = null;
			foreach (RabbitProtocolSefiraSlot rabbitProtocolSefiraSlot in this.sefiraList)
			{
				if (rabbitProtocolSefiraSlot.sefiraEnum == sefira)
				{
					result = rabbitProtocolSefiraSlot;
					break;
				}
			}
			return result;
		}

		// Token: 0x06004EE9 RID: 20201 RVA: 0x001CE0DC File Offset: 0x001CC2DC
		private Dictionary<SefiraEnum, RabbitProtocolWindow.SefiraInfo> CalculateUnits()
		{
			List<AgentModel> list = new List<AgentModel>(AgentManager.instance.GetAgentList());
			List<OfficerModel> list2 = new List<OfficerModel>(OfficerManager.instance.GetOfficerList());
			List<CreatureModel> escapedCreatures = SefiraManager.instance.GetEscapedCreatures();
			List<OrdealCreatureModel> list3 = new List<OrdealCreatureModel>(OrdealManager.instance.GetOrdealCreatureList());
			List<WorkerModel> list4 = new List<WorkerModel>();
			list4.AddRange(list.ToArray());
			list4.AddRange(list2.ToArray());
			Dictionary<SefiraEnum, RabbitProtocolWindow.SefiraInfo> dictionary = new Dictionary<SefiraEnum, RabbitProtocolWindow.SefiraInfo>();
			foreach (WorkerModel workerModel in list4)
			{
				if (!workerModel.IsDead())
				{
					SefiraEnum sefiraEnum = workerModel.GetMovableNode().GetCurrentStandingSefira().sefiraEnum;
					if (sefiraEnum == SefiraEnum.TIPERERTH2)
					{
						sefiraEnum = SefiraEnum.TIPERERTH1;
					}
					RabbitProtocolWindow.SefiraInfo sefiraInfo = null;
					if (!dictionary.TryGetValue(sefiraEnum, out sefiraInfo))
					{
						sefiraInfo = new RabbitProtocolWindow.SefiraInfo();
						dictionary.Add(sefiraEnum, sefiraInfo);
					}
					sefiraInfo.Add(workerModel);
				}
			}
			foreach (CreatureModel creatureModel in escapedCreatures)
			{
				if (creatureModel.IsEscapedOnlyEscape())
				{
					if (creatureModel.script.IsSuppressable())
					{
						SefiraEnum sefiraEnum2 = creatureModel.GetMovableNode().GetCurrentStandingSefira().sefiraEnum;
						if (sefiraEnum2 == SefiraEnum.TIPERERTH2)
						{
							sefiraEnum2 = SefiraEnum.TIPERERTH1;
						}
						RabbitProtocolWindow.SefiraInfo sefiraInfo2 = null;
						if (!dictionary.TryGetValue(sefiraEnum2, out sefiraInfo2))
						{
							sefiraInfo2 = new RabbitProtocolWindow.SefiraInfo();
							dictionary.Add(sefiraEnum2, sefiraInfo2);
						}
						sefiraInfo2.Add(creatureModel);
						List<ChildCreatureModel> aliveChilds = creatureModel.GetAliveChilds();
						foreach (ChildCreatureModel childCreatureModel in aliveChilds)
						{
							if (childCreatureModel.script.IsSuppressable())
							{
								if (childCreatureModel.IsEscapedOnlyEscape())
								{
									SefiraEnum sefiraEnum3 = childCreatureModel.GetMovableNode().GetCurrentStandingSefira().sefiraEnum;
									if (sefiraEnum3 == SefiraEnum.TIPERERTH2)
									{
										sefiraEnum3 = SefiraEnum.TIPERERTH1;
									}
									RabbitProtocolWindow.SefiraInfo sefiraInfo3 = null;
									if (!dictionary.TryGetValue(sefiraEnum2, out sefiraInfo3))
									{
										sefiraInfo3 = new RabbitProtocolWindow.SefiraInfo();
										dictionary.Add(sefiraEnum3, sefiraInfo3);
									}
									sefiraInfo3.Add(childCreatureModel);
								}
							}
						}
					}
				}
			}
			foreach (OrdealCreatureModel ordealCreatureModel in list3)
			{
				if (ordealCreatureModel.IsEscapedOnlyEscape())
				{
					if (ordealCreatureModel.script.IsSuppressable())
					{
						SefiraEnum sefiraEnum4 = ordealCreatureModel.GetMovableNode().GetCurrentStandingSefira().sefiraEnum;
						if (sefiraEnum4 == SefiraEnum.TIPERERTH2)
						{
							sefiraEnum4 = SefiraEnum.TIPERERTH1;
						}
						RabbitProtocolWindow.SefiraInfo sefiraInfo4 = null;
						if (!dictionary.TryGetValue(sefiraEnum4, out sefiraInfo4))
						{
							sefiraInfo4 = new RabbitProtocolWindow.SefiraInfo();
							dictionary.Add(sefiraEnum4, sefiraInfo4);
						}
						sefiraInfo4.Add(ordealCreatureModel);
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06004EEA RID: 20202 RVA: 0x001CE444 File Offset: 0x001CC644
		public void OnClickCommand()
		{ // <Mod>
			if (this.currentSelected.Count > 0)
			{
				List<SefiraEnum> list = new List<SefiraEnum>(this.currentSelected.ToArray());
				float num = (float)this.currentSelected.Count * 0.25f;
				num /= StageTypeInfo.instnace.GetPercentEnergyFactor();
				float energy = EnergyModel.instance.GetEnergy();
				float num2 = energy * num;
				if (ResearchDataModel.instance.IsUpgradedAbility("energy_discount"))
				{
					num2 *= 0.9f;
				}
				EnergyModel.instance.SubEnergy((float)((int)num2));
				Notice.instance.Send(NoticeName.RabbitProtocolActivated, new object[]
				{
					list.ToArray()
				});
				int count = this.currentSelected.Count;
				int num3 = 2;
				if (count != 1 && count != 2)
				{
					num3 = 2;
				}
				else
				{
					num3 = 4;
				}
				foreach (SefiraEnum sefiraEnum in this.currentSelected)
				{
					int num4 = num3;
					if (sefiraEnum != SefiraEnum.TIPERERTH2)
					{
						if (sefiraEnum == SefiraEnum.TIPERERTH1)
						{
							num4 *= 2;
						}
						RabbitManager.instance.CreateRabbitSquad(sefiraEnum, num4);
					}
				}
				RabbitManager.instance.OnStartSession();
				this.activatedAnim.gameObject.SetActive(true);
				this.activatedAnim.Show();
				this.protocolActivated = true;
				this.currentSelected.Clear();
				this.ProtocolActivationButton.interactable = false;
				this.ProtocolActivationButton.gameObject.SetActive(false);
				this._filterAlphaTimer.StartTimer(this.filterFreq);
			}
			this.OnClose();
		}

		// Token: 0x06004EEB RID: 20203 RVA: 0x00040FD3 File Offset: 0x0003F1D3
		public void ApplyFilter(string area, List<SefiraFilterMgr> filterMgr)
		{
			this.filter.Add(area, filterMgr);
		}

		// Token: 0x06004EEC RID: 20204 RVA: 0x001CE5D8 File Offset: 0x001CC7D8
		public void ClearFilter(string area)
		{
			List<SefiraFilterMgr> list = null;
			if (this.filter.TryGetValue(area, out list))
			{
				foreach (SefiraFilterMgr sefiraFilterMgr in list)
				{
					UnityEngine.Object.Destroy(sefiraFilterMgr.gameObject);
				}
				list.Clear();
			}
			this.filter.Remove(area);
		}

		// Token: 0x06004EED RID: 20205 RVA: 0x00040FE2 File Offset: 0x0003F1E2
		public void OnNotice(string notice, params object[] param)
		{
			if (notice == NoticeName.OnStageStart)
			{
				this.OnStageStart();
			}
			else if (notice == NoticeName.OnStageEnd)
			{
				this.DestroyNotice();
				this.ClearFilter();
			}
		}

		// Token: 0x06004EEE RID: 20206 RVA: 0x001CE65C File Offset: 0x001CC85C
		public void ClearFilter()
		{
			if (this.filter.Count > 0)
			{
				foreach (List<SefiraFilterMgr> list in this.filter.Values)
				{
					foreach (SefiraFilterMgr sefiraFilterMgr in list)
					{
						UnityEngine.Object.Destroy(sefiraFilterMgr.gameObject);
					}
				}
			}
			this.filter.Clear();
		}

		// Token: 0x06004EEF RID: 20207 RVA: 0x0000434D File Offset: 0x0000254D
		// Note: this type is marked as 'beforefieldinit'.
		static RabbitProtocolWindow()
		{
		}

		// Token: 0x04004930 RID: 18736
		private const int openLevelStd = 4;

		// Token: 0x04004931 RID: 18737
		private const int selectMax = 4;

		// Token: 0x04004932 RID: 18738
		private static RabbitProtocolWindow _window;

		// Token: 0x04004933 RID: 18739
		public GameObject root;

		// Token: 0x04004934 RID: 18740
		[Header("Sefira Slots")]
		public List<RabbitProtocolSefiraSlot> sefiraList;

		// Token: 0x04004935 RID: 18741
		private List<RabbitProtocolSefiraSlot> enabledSefiraList;

		// Token: 0x04004936 RID: 18742
		[Header("UI")]
		public Text Cost;

		// Token: 0x04004937 RID: 18743
		public Text Count;

		// Token: 0x04004938 RID: 18744
		public Text CostText;

		// Token: 0x04004939 RID: 18745
		[Header("UI Update")]
		public float freq = 1f;

		// Token: 0x0400493A RID: 18746
		public UIController windowAnim;

		// Token: 0x0400493B RID: 18747
		public CheckPointUI activatedAnim;

		// Token: 0x0400493C RID: 18748
		public Sprite RabbitConnerSprite;

		// Token: 0x0400493D RID: 18749
		public Button ConfirmButton;

		// Token: 0x0400493E RID: 18750
		public AnimationCurve filterAlphaCurve;

		// Token: 0x0400493F RID: 18751
		public float filterFreq = 1.2f;

		// Token: 0x04004940 RID: 18752
		public bool rootFilterUpdate;

		// Token: 0x04004941 RID: 18753
		private UnscaledTimer _filterAlphaTimer = new UnscaledTimer();

		// Token: 0x04004942 RID: 18754
		[Header("ActvationButton")]
		public Button ProtocolActivationButton;

		// Token: 0x04004943 RID: 18755
		private UnscaledTimer _uiUpdateTimer = new UnscaledTimer();

		// Token: 0x04004944 RID: 18756
		public Color NotAvailableColor;

		// Token: 0x04004945 RID: 18757
		private bool _activated;

		// Token: 0x04004946 RID: 18758
		private bool protocolActivated;

		// Token: 0x04004947 RID: 18759
		private List<SefiraEnum> currentSelected = new List<SefiraEnum>();

		// Token: 0x04004948 RID: 18760
		private Dictionary<string, List<SefiraFilterMgr>> filter = new Dictionary<string, List<SefiraFilterMgr>>();

		// Token: 0x02000A40 RID: 2624
		public class SefiraInfo
		{
			// Token: 0x06004EF0 RID: 20208 RVA: 0x00004320 File Offset: 0x00002520
			public SefiraInfo()
			{
			}

			// Token: 0x06004EF1 RID: 20209 RVA: 0x001CE71C File Offset: 0x001CC91C
			public void Add(UnitModel unit)
			{
				if (unit is AgentModel)
				{
					this.AgentCount++;
				}
				else if (unit is OfficerModel)
				{
					this.OfficerCount++;
				}
				else if (unit is CreatureModel)
				{
					this.EnermyCount++;
				}
				else if (unit is ChildCreatureModel)
				{
					this.EnermyCount++;
				}
				else if (unit is OrdealCreatureModel)
				{
					this.EnermyCount++;
				}
			}

			// Token: 0x04004949 RID: 18761
			public int AgentCount;

			// Token: 0x0400494A RID: 18762
			public int OfficerCount;

			// Token: 0x0400494B RID: 18763
			public int EnermyCount;
		}
	}
}
