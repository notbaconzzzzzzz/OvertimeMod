/*
public void Update() // Unstable TT2
public void SetOverloadLevel(int level) // 
private string GetOrdealName(OrdealLevel level) // 
public void SetOverloadGauge(int num, int max) // 
+public void SetSecondaryOverloadGauge(int num, int max) // 
+public void SetSecondaryOverload(int gaugePos, int gaugeMax, OrdealBase ordeal, int overloadNum) // 
+public void ClearSecondaryOverload() // 
+private int _defaultfontsize // 
+things // Secondary Qliphoth Overload
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameStatusUI
{
	// Token: 0x02000AE1 RID: 2785
	public class EnergyController : MonoBehaviour
	{
		// Token: 0x060053AD RID: 21421 RVA: 0x00043A2D File Offset: 0x00041C2D
		public EnergyController()
		{
		}

		// Token: 0x060053AE RID: 21422 RVA: 0x001E146C File Offset: 0x001DF66C
		public void Update()
		{ // <Mod>
			this.current = EnergyModel.instance.GetEnergy();
			float fillAmount = this.current / this.max;
            if (UnstableTT2Manager.instance.isActive) {
				fillAmount = UnstableTT2Manager.instance.curPauseCharge / UnstableTT2Manager.instance.maxPauseCharge;
			}
			this.EnergyFill.fillAmount = fillAmount;
			this.InnerText.text = (int)this.current + " / " + (int)this.max;
			this.OrdealUpdate();
			this.CheckGameStatus();
			if (this.stageRewardInfo != null)
			{
				this.SetStageRank(this.stageRewardInfo.GetStageRank(GameManager.currentGameManager.PlayTime));
			}
			if (this._overloadUITimer.started && this._overloadUITimer.RunTimer())
			{
				this.OverloadUIController.Hide();
			}
			OvertimeIsolateGradientTimer += Time.unscaledDeltaTime / 3f;
			if (OvertimeIsolateGradientTimer >= 4f)
			{
				OvertimeIsolateGradientTimer -= 4f;
			}
			Color leftTween;
			Color rightTween;
			float tweenAmount;
			if (OvertimeIsolateGradientTimer < 1f)
			{
				leftTween = UIColorManager.instance.GetOverloadColor(OverloadType.PAIN);
				rightTween = UIColorManager.instance.GetOverloadColor(OverloadType.GRIEF);
				tweenAmount = OvertimeIsolateGradientTimer;
			}
			else if (OvertimeIsolateGradientTimer < 2f)
			{
				leftTween = UIColorManager.instance.GetOverloadColor(OverloadType.GRIEF);
				rightTween = UIColorManager.instance.GetOverloadColor(OverloadType.RUIN);
				tweenAmount = OvertimeIsolateGradientTimer - 1f;
			}
			else if (OvertimeIsolateGradientTimer < 3f)
			{
				leftTween = UIColorManager.instance.GetOverloadColor(OverloadType.RUIN);
				rightTween = UIColorManager.instance.GetOverloadColor(OverloadType.OBLIVION);
				tweenAmount = OvertimeIsolateGradientTimer - 2f;
			}
			else
			{
				leftTween = UIColorManager.instance.GetOverloadColor(OverloadType.OBLIVION);
				rightTween = UIColorManager.instance.GetOverloadColor(OverloadType.PAIN);
				tweenAmount = OvertimeIsolateGradientTimer - 3f;
			}
			tweenAmount = 0.5f - Mathf.Cos(tweenAmount * 180f * Mathf.Deg2Rad) / 2f;
			Color color = leftTween * (1f - tweenAmount) + rightTween * tweenAmount;
			if (OvertimeIsolateNumText != null)
			{
				OvertimeIsolateNumText.color = color;
			}
			if (SecondaryIsolateNumText != null)
			{
				SecondaryIsolateNumText.color = color;
			}
		}

		// Token: 0x060053AF RID: 21423 RVA: 0x00043A47 File Offset: 0x00041C47
		private void Start()
		{
			this.OrdealSlotInit();
			this.stageRewardInfo = StageRewardTypeList.instance.GetData(PlayerModel.instance.GetDay() + 1);
			this.OverloadUIController.gameObject.SetActive(false);
		}

		// Token: 0x060053B0 RID: 21424 RVA: 0x001E1530 File Offset: 0x001DF730
		public void OnStageStart()
		{
			this.max = StageTypeInfo.instnace.GetEnergyNeed(PlayerModel.instance.GetDay());
			this.current = 0f;
			if (this.max == 0f)
			{
				this.max = 100f;
			}
		}

		// Token: 0x060053B1 RID: 21425 RVA: 0x001E1580 File Offset: 0x001DF780
		private void OrdealSlotInit()
		{
			this.OrdealSlots[0].OrdealNameText.text = "여명";
			this.OrdealSlots[1].OrdealNameText.text = "정오";
			this.OrdealSlots[2].OrdealNameText.text = "어스름";
		}

		// Token: 0x060053B2 RID: 21426 RVA: 0x00003FDD File Offset: 0x000021DD
		private void OrdealUpdate()
		{
		}

		// Token: 0x060053B3 RID: 21427 RVA: 0x001E15D4 File Offset: 0x001DF7D4
		public void SetDawnOrdeal(bool activated, int current, int max)
		{
			EnergyController.OrdealSlot ordealSlot = this.OrdealSlots[0];
			ordealSlot.SetStatusText(activated, current, max);
		}

		// Token: 0x060053B4 RID: 21428 RVA: 0x001E15F4 File Offset: 0x001DF7F4
		public void SetDawnOrdeal(bool activated, int timeSec)
		{
			EnergyController.OrdealSlot ordealSlot = this.OrdealSlots[0];
			ordealSlot.SetStatusTimeText(activated, timeSec);
		}

		// Token: 0x060053B5 RID: 21429 RVA: 0x001E1614 File Offset: 0x001DF814
		public void SetDawnOrdealVisible(bool visible)
		{
			EnergyController.OrdealSlot ordealSlot = this.OrdealSlots[0];
			ordealSlot.rootObject.SetActive(visible);
		}

		// Token: 0x060053B6 RID: 21430 RVA: 0x001E1638 File Offset: 0x001DF838
		public void SetNoonOrdeal(bool activated, int current, int max)
		{
			EnergyController.OrdealSlot ordealSlot = this.OrdealSlots[1];
			ordealSlot.SetStatusText(activated, current, max);
		}

		// Token: 0x060053B7 RID: 21431 RVA: 0x001E1658 File Offset: 0x001DF858
		public void SetNoonOrdeal(bool activated, int timeSec)
		{
			EnergyController.OrdealSlot ordealSlot = this.OrdealSlots[1];
			ordealSlot.SetStatusTimeText(activated, timeSec);
		}

		// Token: 0x060053B8 RID: 21432 RVA: 0x001E1678 File Offset: 0x001DF878
		public void SetNoonOrdealVisible(bool visible)
		{
			EnergyController.OrdealSlot ordealSlot = this.OrdealSlots[1];
			ordealSlot.rootObject.SetActive(visible);
		}

		// Token: 0x060053B9 RID: 21433 RVA: 0x001E169C File Offset: 0x001DF89C
		public void SetDuskOrdeal(bool activated, int current, int max)
		{
			EnergyController.OrdealSlot ordealSlot = this.OrdealSlots[2];
			ordealSlot.SetStatusText(activated, current, max);
		}

		// Token: 0x060053BA RID: 21434 RVA: 0x001E16BC File Offset: 0x001DF8BC
		public void SetDuskOrdeal(bool activated, int timeSec)
		{
			EnergyController.OrdealSlot ordealSlot = this.OrdealSlots[2];
			ordealSlot.SetStatusTimeText(activated, timeSec);
		}

		// Token: 0x060053BB RID: 21435 RVA: 0x001E16DC File Offset: 0x001DF8DC
		public void SetDuskOrdealVisible(bool visible)
		{
			EnergyController.OrdealSlot ordealSlot = this.OrdealSlots[2];
			ordealSlot.rootObject.SetActive(visible);
		}

		// Token: 0x060053BC RID: 21436 RVA: 0x001E1700 File Offset: 0x001DF900
		public void CheckGameStatus()
		{
			float playTime = GameManager.currentGameManager.PlayTime;
			if (this._currentEntered == -1)
			{
				TimeSpan timeSpan = TimeSpan.FromSeconds((double)playTime);
				string text = string.Empty;
				if (timeSpan.Hours == 0)
				{
					text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
				}
				else
				{
					text = "99:00";
				}
				this.PlayTime.text = text;
			}
			int rewardMoney = this.stageRewardInfo.GetRewardMoney(playTime);
			this.LobPoint.text = string.Format("LOB {0}", rewardMoney);
			Color color = (rewardMoney != 0) ? UIColorManager.instance.UIBlueColor : UIColorManager.instance.UIRedColor;
			Graphic lobPoint = this.LobPoint;
			Color color2 = color;
			this.LobPointTexture.color = color2;
			lobPoint.color = color2;
		}

		// Token: 0x060053BD RID: 21437 RVA: 0x00003FDD File Offset: 0x000021DD
		public void SetStageRank(StageRank rank)
		{
		}

		// Token: 0x060053BE RID: 21438 RVA: 0x00003FDD File Offset: 0x000021DD
		public void OnRankSlotEnter(int i)
		{
		}

		// Token: 0x060053BF RID: 21439 RVA: 0x00043A7C File Offset: 0x00041C7C
		public void OnRankSlotExit(int i)
		{
			this._currentEntered = -1;
			this.PlayTime.color = UIColorManager.Orange;
		}

		// Token: 0x060053C0 RID: 21440 RVA: 0x001E17E0 File Offset: 0x001DF9E0
		public float GetRankTime(StageRank rank)
		{
			StageRewardTypeInfo data = StageRewardTypeList.instance.GetData(PlayerModel.instance.GetDay() + 1);
			return data.GetLimitTime(rank);
		}

		// Token: 0x060053C1 RID: 21441 RVA: 0x001E180C File Offset: 0x001DFA0C
		public void SetOverloadLevel(int level)
		{ // <Mod>
			if (_defaultfontsize == -1)
			{
				_defaultfontsize = OverloadLevelText.fontSize;
			}
			switch (level)
			{
			case 1:
				this.OverloadLevelText.text = "I";
				OverloadLevelText.fontSize = _defaultfontsize;
				break;
			case 2:
				this.OverloadLevelText.text = "II";
				break;
			case 3:
				this.OverloadLevelText.text = "III";
				break;
			case 4:
				this.OverloadLevelText.text = "IV";
				break;
			case 5:
				this.OverloadLevelText.text = "V";
				break;
			case 6:
				this.OverloadLevelText.text = "VI";
				break;
			case 7:
				this.OverloadLevelText.text = "VII";
				break;
			case 8:
				this.OverloadLevelText.text = "VIII";
				break;
			case 9:
				this.OverloadLevelText.text = "IX";
				break;
			case 10:
				this.OverloadLevelText.text = "X";
				break;
			case 11:
				this.OverloadLevelText.text = "XI";
				break;
			case 12:
				this.OverloadLevelText.text = "XII";
				break;
			case 13:
				this.OverloadLevelText.text = "XIII";
				break;
			case 14:
				this.OverloadLevelText.text = "XIV";
				OverloadLevelText.fontSize = _defaultfontsize * 6 / 7;
				break;
			case 15:
				this.OverloadLevelText.text = "XV";
				OverloadLevelText.fontSize = _defaultfontsize;
				break;
			case 16:
				this.OverloadLevelText.text = "XVI";
				OverloadLevelText.fontSize = _defaultfontsize * 6 / 7;
				break;
			case 17:
				this.OverloadLevelText.text = "XVII";
				OverloadLevelText.fontSize = _defaultfontsize * 6 / 8;
				break;
			case 18:
				this.OverloadLevelText.text = "XVIII";
				OverloadLevelText.fontSize = _defaultfontsize * 6 / 9;
				break;
			case 19:
				this.OverloadLevelText.text = "IXX";
				OverloadLevelText.fontSize = _defaultfontsize * 6 / 7;
				break;
			case 20:
				this.OverloadLevelText.text = "XX";
				OverloadLevelText.fontSize = _defaultfontsize * 9 / 10;
				break;
			}
		}

		// Token: 0x060053C2 RID: 21442 RVA: 0x001E1920 File Offset: 0x001DFB20
		public void SetOverLoadOrdeal(OrdealBase ordeal)
		{
			if (ordeal == null)
			{
				Graphic overloadImage = this.OverloadImage;
				Color color = this.Orange;
				this.OverloadLevelText.color = color;
				overloadImage.color = color;
				this.OverLoadOrdealRoot.SetActive(false);
				this.OverLoadIsolateNumText.gameObject.SetActive(true);
				foreach (MaskableGraphic maskableGraphic in this.OverloadColored)
				{
					maskableGraphic.color = this.Orange;
				}
			}
			else
			{
				Graphic overloadImage2 = this.OverloadImage;
				Color color = this.Red;
				this.OverloadLevelText.color = color;
				overloadImage2.color = color;
				this.OverLoadOrdealRoot.SetActive(true);
				this.OverLoadIsolateNumText.gameObject.SetActive(false);
				foreach (MaskableGraphic maskableGraphic2 in this.OverloadColored)
				{
					maskableGraphic2.color = ordeal.OrdealColor;
				}
				this.OverLoadOrdealImage.sprite = IconManager.instance.GetOrdealIcon(ordeal.level);
				this.OverLoadOrdealName.text = this.GetOrdealName(ordeal.level);
			}
		}

		// Token: 0x060053C3 RID: 21443 RVA: 0x001E1A88 File Offset: 0x001DFC88
		private string GetOrdealName(OrdealLevel level)
		{ // <Mod>
			string text = "Ordeal_";
			switch (level)
			{
			case OrdealLevel.DAWN:
				text += "Dawn";
				break;
			case OrdealLevel.NOON:
				text += "Noon";
				break;
			case OrdealLevel.DUSK:
				text += "Dusk";
				break;
			case OrdealLevel.MIDNIGHT:
				text += "Midnight";
				break;
			case OrdealLevel.OVERTIME_DAWN:
				text += "Dawn";
				break;
			case OrdealLevel.OVERTIME_NOON:
				text += "Noon";
				break;
			case OrdealLevel.OVERTIME_DUSK:
				text += "Dusk";
				break;
			case OrdealLevel.OVERTIME_MIDNIGHT:
				text += "Midnight";
				break;
			}
			return LocalizeTextDataModel.instance.GetText(text);
		}

		// Token: 0x060053C4 RID: 21444 RVA: 0x00043A95 File Offset: 0x00041C95
		public void SetOverloadIsolateNum(int num)
		{
			this.OverLoadIsolateNumText.text = num.ToString();
		}

		// <Mod>
		public void SetOvertimeOverloadIsolateNum(int num)
		{
			if (OvertimeIsolateNumText == null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.OverLoadIsolateNumText.gameObject);
				gameObject.transform.SetParent(OverLoadIsolateNumText.gameObject.transform.parent);
				Vector3 scale = gameObject.transform.localScale;
				scale.y *= 1f / 3f;
				scale.x *= 1f / 3f;
				gameObject.transform.localScale = scale;
				gameObject.transform.localPosition = OverLoadIsolateNumText.gameObject.transform.localPosition;
				gameObject.layer = OverLoadIsolateNumText.gameObject.layer;
				gameObject.transform.Translate(-30f, 0f, 0f);
				OvertimeIsolateNumText = gameObject.GetComponent<Text>();
			}
			if (num == 0)
			{
				OvertimeIsolateNumText.gameObject.SetActive(false);
			}
			else
			{
				OvertimeIsolateNumText.text = num.ToString();
				OvertimeIsolateNumText.gameObject.SetActive(true);
			}
		}

		// Token: 0x060053C5 RID: 21445 RVA: 0x001E1B08 File Offset: 0x001DFD08
		public void SetOverloadGauge(int num, int max)
		{ // <Mod> Work Compression
			IEnumerator enumerator = this.OverLoadGaugeLayout.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					UnityEngine.Object.Destroy(transform.gameObject);
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
			bool isCompressed = CreatureOverloadManager.instance.GetQliphothOverloadLevel() <= CreatureOverloadManager.instance.workCompressionLimit;
			bool isEvery5th = false;
			if (!isCompressed)
			{
				isEvery5th = CreatureOverloadManager.instance.GetQliphothOverloadLevel() <= 10 && MissionManager.instance.ExistsFinishedOvertimeBossMission(SefiraEnum.TIPERERTH1);
			}
			int Nth = (CreatureOverloadManager.instance.GetQliphothOverloadLevel() - CreatureOverloadManager.instance.workCompressionLimit - 1) * max;
			for (int i = 0; i < max; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.OverloadGaugeUnitPrefab);
				gameObject.transform.SetParent(this.OverLoadGaugeLayout.transform);
				gameObject.transform.localScale = Vector3.one;
				if (i >= num)
				{
					Image component = gameObject.GetComponent<Image>();
					if (isCompressed || (isEvery5th && Nth % 6 >= 4))
					{
						component.color = new Color(0.5f, 0f, 0.75f, 0.5f);
					}
					else
					{
						component.color = new Color(0f, 0f, 0f, 0f);
					}
				}
				Nth++;
			}
		}

		// <Mod>
		public void SetSecondaryOverloadGauge(int num, int max)
		{
			if (SecondaryOverLoadGaugeLayout == null)
			{
				SecondaryOverLoadGaugeLayout = UnityEngine.Object.Instantiate<GameObject>(this.OverLoadGaugeLayout);
				SecondaryOverLoadGaugeLayout.transform.SetParent(OverLoadGaugeLayout.transform.parent);
				Vector3 scale = OverLoadGaugeLayout.transform.localScale;
				scale.y *= 0.25f;
				scale.x *= 0.98f;
				SecondaryOverLoadGaugeLayout.transform.localScale = scale;
				SecondaryOverLoadGaugeLayout.transform.localPosition = OverLoadGaugeLayout.transform.localPosition;
				SecondaryOverLoadGaugeLayout.layer = OverLoadGaugeLayout.layer;
				SecondaryOverLoadGaugeLayout.transform.Translate(0f, -5.5f, 0f);
				SecondaryOverLoadGaugeLayout.SetActive(true);
			}
			if (SecondaryOverLoadGaugeLayout == null) return;
			IEnumerator enumerator = SecondaryOverLoadGaugeLayout.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					UnityEngine.Object.Destroy(transform.gameObject);
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
			for (int i = 0; i < max; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.OverloadGaugeUnitPrefab);
				gameObject.transform.SetParent(this.SecondaryOverLoadGaugeLayout.transform);
				gameObject.transform.localScale = Vector3.one;
				Image component = gameObject.GetComponent<Image>();
				if (i < num)
				{
					component.color = new Color(0f, 0.5f, 0.75f, 0.75f);
				}
				else
				{
					component.color = new Color(0f, 0f, 0f, 0f);
				}
			}
		}

		// <Mod>
		public void SetSecondaryOverload(int gaugePos, int gaugeMax, OrdealBase ordeal, int overloadNum)
		{
			if (SecondaryOrdealRoot == null)
			{
				float scaleFactor = 0.3f;
				if (gaugeMax > 12)
				{
					scaleFactor = 0.3f * 12f / (float)gaugeMax;
				}
				SecondaryOrdealRoot = UnityEngine.Object.Instantiate<GameObject>(this.OverLoadOrdealRoot);
				SecondaryOrdealRoot.transform.SetParent(OverLoadOrdealRoot.transform.parent);
				Vector3 scale = SecondaryOrdealRoot.transform.localScale;
				scale.y *= scaleFactor;
				scale.x *= scaleFactor;
				SecondaryOrdealRoot.transform.localScale = scale;
				SecondaryOrdealRoot.transform.localPosition = OverLoadOrdealRoot.transform.localPosition;
				SecondaryOrdealRoot.layer = OverLoadOrdealRoot.layer;
				SecondaryOrdealRoot.SetActive(true);
				SecondaryOrdealName = SecondaryOrdealRoot.GetComponentInChildren<Text>();
				SecondaryOrdealImage = SecondaryOrdealRoot.GetComponentsInChildren<Image>()[1];
				SecondaryOrdealName.text = "";

				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.OverLoadIsolateNumText.gameObject);
				gameObject.transform.SetParent(OverLoadIsolateNumText.gameObject.transform.parent);
				scale = gameObject.transform.localScale;
				scale.y *= scaleFactor;
				scale.x *= scaleFactor;
				gameObject.transform.localScale = scale;
				gameObject.transform.localPosition = OverLoadIsolateNumText.gameObject.transform.localPosition;
				gameObject.layer = OverLoadIsolateNumText.gameObject.layer;
				gameObject.SetActive(true);
				SecondaryIsolateNumText = gameObject.GetComponent<Text>();

				SecondaryOverloadColored = new List<MaskableGraphic>();
				foreach (MaskableGraphic graphic in SecondaryOrdealRoot.GetComponentsInChildren<MaskableGraphic>())
				{
					if (graphic.name == "IconBase(Clone)" || graphic.name == "OrdealIcon" || graphic.name == "OrdealName" || graphic.name == "IconBase")
					{
						SecondaryOverloadColored.Add(graphic);
					}
				}/*
				foreach (Component component in OverLoadOrdealRoot.GetComponentsInChildren<Component>())
				{
					Notice.instance.Send(NoticeName.AddSystemLog, new object[]
					{
						component.gameObject.name + " : " + component.name + " : " + component.GetType().FullName
					});
				}
				Notice.instance.Send(NoticeName.AddSystemLog, new object[]
				{
					" --- "
				});
				foreach (Component component in SecondaryOrdealRoot.GetComponentsInChildren<Component>())
				{
					Notice.instance.Send(NoticeName.AddSystemLog, new object[]
					{
						component.gameObject.name + " : " + component.name + " : " + component.GetType().FullName
					});
				}*/
				//SecondaryOverloadColored.AddRange(SecondaryOrdealRoot.GetComponentsInChildren<MaskableGraphic>());
				//SecondaryOverloadColored.AddRange(gameObject.GetComponentsInChildren<MaskableGraphic>());
				/*foreach (MaskableGraphic graphic in OverloadColored)
				{
					Notice.instance.Send(NoticeName.AddSystemLog, new object[]
					{
						graphic.name + " : " + graphic.gameObject.name
					});
				}
				Notice.instance.Send(NoticeName.AddSystemLog, new object[]
				{
					" --- "
				});
				foreach (MaskableGraphic graphic in SecondaryOverloadColored)
				{
					Notice.instance.Send(NoticeName.AddSystemLog, new object[]
					{
						graphic.name + " : " + graphic.gameObject.name
					});
				}*/
			}
			Transform gauge = OverLoadGaugeLayout.transform.GetChild(gaugePos - 1);
			SecondaryOrdealRoot.transform.Translate(gauge.position.x - SecondaryOrdealRoot.transform.position.x, 0f, 0f);
			SecondaryIsolateNumText.gameObject.transform.Translate(gauge.position.x - SecondaryIsolateNumText.gameObject.transform.position.x, 0f, 0f);
			if (ordeal == null)
			{
				SecondaryOrdealRoot.SetActive(false);
				SecondaryIsolateNumText.gameObject.SetActive(true);
				foreach (MaskableGraphic maskableGraphic in this.SecondaryOverloadColored)
				{
					maskableGraphic.color = this.Orange;
				}
				SecondaryIsolateNumText.text = overloadNum.ToString();
			}
			else
			{
				SecondaryOrdealRoot.SetActive(true);
				SecondaryIsolateNumText.gameObject.SetActive(false);
				foreach (MaskableGraphic maskableGraphic2 in this.SecondaryOverloadColored)
				{
					maskableGraphic2.color = ordeal.OrdealColor;
				}
				SecondaryOrdealImage.sprite = IconManager.instance.GetOrdealIcon(ordeal.level);
				//SecondaryOrdealName.text = this.GetOrdealName(ordeal.level);
				SecondaryOrdealName.text = "";
			}
		}

		// <Mod>
		public void ClearSecondaryOverload()
		{
			SecondaryOrdealRoot.SetActive(false);
			SecondaryIsolateNumText.gameObject.SetActive(false);
		}

		// Token: 0x060053C6 RID: 21446 RVA: 0x001E1BF0 File Offset: 0x001DFDF0
		public void SetOverLoadUI(string text)
		{
			if (SefiraBossManager.Instance.IsKetherBoss(KetherBossType.E4))
			{
				return;
			}
			this.OverloadUIController.gameObject.SetActive(true);
			this.OverloadUIText.text = string.Format("{0} - {1}{2}", LocalizeTextDataModel.instance.GetText("Qliphoth_Overload"), text, LocalizeTextDataModel.instance.GetText("Qliphoth_Level"));
			this.OverloadUIController.Show();
			this._overloadUITimer.StartTimer(5f);
			if (SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.CHOKHMAH, false) && CreatureOverloadManager.instance.GetQliphothOverloadLevel() >= 6)
			{
				return;
			}
			GlobalAudioManager.instance.PlayLocalClip(this.OverloadClip);
		}

		// Token: 0x04004D42 RID: 19778
		private const float _overloadUITime = 5f;

		// Token: 0x04004D43 RID: 19779
		public GameObject ActiveControl;

		// Token: 0x04004D44 RID: 19780
		public Image EnergyFill;

		// Token: 0x04004D45 RID: 19781
		public Text InnerText;

		// Token: 0x04004D46 RID: 19782
		[Header("Ordeal")]
		public EnergyController.OrdealSlot[] OrdealSlots;

		// Token: 0x04004D47 RID: 19783
		[Header("StageInfo")]
		public Text PlayTime;

		// Token: 0x04004D48 RID: 19784
		public Text LobPoint;

		// Token: 0x04004D49 RID: 19785
		public Image LobPointTexture;

		// Token: 0x04004D4A RID: 19786
		public EnergyController.RankSlot[] RankSlots;

		// Token: 0x04004D4B RID: 19787
		[Header("Overload")]
		public Text OverLoadIsolateNumText;

		// Token: 0x04004D4C RID: 19788
		public Text OverloadLevelText;

		// Token: 0x04004D4D RID: 19789
		public Image OverloadImage;

		// Token: 0x04004D4E RID: 19790
		public GameObject OverLoadGaugeLayout;

		// Token: 0x04004D4F RID: 19791
		public GameObject OverLoadOrdealRoot;

		// Token: 0x04004D50 RID: 19792
		public Text OverLoadOrdealName;

		// Token: 0x04004D51 RID: 19793
		public Image OverLoadOrdealImage;

		// Token: 0x04004D52 RID: 19794
		public List<MaskableGraphic> OverloadColored;

		// Token: 0x04004D53 RID: 19795
		public GameObject OverloadGaugeUnitPrefab;

		// Token: 0x04004D54 RID: 19796
		public UIController OverloadUIController;

		// Token: 0x04004D55 RID: 19797
		public Text OverloadUIText;

		// Token: 0x04004D56 RID: 19798
		public Color Orange;

		// Token: 0x04004D57 RID: 19799
		public Color Red;

		// Token: 0x04004D58 RID: 19800
		private UnscaledTimer _overloadUITimer = new UnscaledTimer();

		// Token: 0x04004D59 RID: 19801
		public AudioClip OverloadClip;

		// Token: 0x04004D5A RID: 19802
		private float max;

		// Token: 0x04004D5B RID: 19803
		private float current;

		// Token: 0x04004D5C RID: 19804
		private int _currentEntered = -1;

		// Token: 0x04004D5D RID: 19805
		private StageRewardTypeInfo stageRewardInfo;

		//> <Mod>
		private int _defaultfontsize = -1;

		public Text OvertimeIsolateNumText;

		public float OvertimeIsolateGradientTimer;

		public GameObject SecondaryOverLoadGaugeLayout;

		public Text SecondaryIsolateNumText;

		public GameObject SecondaryOrdealRoot;

		public Text SecondaryOrdealName;

		public Image SecondaryOrdealImage;
		
		public List<MaskableGraphic> SecondaryOverloadColored;
		//<

		// Token: 0x02000AE2 RID: 2786
		[Serializable]
		public class OrdealSlot
		{
			// Token: 0x060053C7 RID: 21447 RVA: 0x00003FB0 File Offset: 0x000021B0
			public OrdealSlot()
			{
			}

			// Token: 0x060053C8 RID: 21448 RVA: 0x00043AAF File Offset: 0x00041CAF
			public void SetOrdealName(string name)
			{
				this.OrdealNameText.text = name;
			}

			// Token: 0x060053C9 RID: 21449 RVA: 0x001E1CA0 File Offset: 0x001DFEA0
			public void SetStatusText(bool activated, int current, int max)
			{
				if (activated)
				{
					this.StatusText.text = LocalizeTextDataModel.instance.GetText("Activated");
					this.SetColorBlue();
				}
				else
				{
					this.StatusText.text = string.Format("{0}/{1}", current, max);
					this.SetColorNormal();
				}
			}

			// Token: 0x060053CA RID: 21450 RVA: 0x001E1D00 File Offset: 0x001DFF00
			public void SetStatusTimeText(bool activated, int timeSec)
			{
				if (activated)
				{
					this.StatusText.text = LocalizeTextDataModel.instance.GetText("Activated");
					this.SetColorBlue();
				}
				else
				{
					this.StatusText.text = string.Format("{0:D1}:{1:D2}", timeSec / 60, timeSec % 60);
					this.SetColorNormal();
				}
			}

			// Token: 0x060053CB RID: 21451 RVA: 0x001E1D68 File Offset: 0x001DFF68
			public void SetColorBlue()
			{
				Color uiblueColor = UIColorManager.instance.UIBlueColor;
				foreach (MaskableGraphic maskableGraphic in this.ColoredGraphic)
				{
					maskableGraphic.color = uiblueColor;
				}
			}

			// Token: 0x060053CC RID: 21452 RVA: 0x001E1DD0 File Offset: 0x001DFFD0
			public void SetColorNormal()
			{
				Color orange = UIColorManager.Orange;
				foreach (MaskableGraphic maskableGraphic in this.ColoredGraphic)
				{
					maskableGraphic.color = orange;
				}
			}

			// Token: 0x04004D5E RID: 19806
			public GameObject rootObject;

			// Token: 0x04004D5F RID: 19807
			public List<MaskableGraphic> ColoredGraphic;

			// Token: 0x04004D60 RID: 19808
			public Text StatusText;

			// Token: 0x04004D61 RID: 19809
			public Text OrdealNameText;
		}

		// Token: 0x02000AE3 RID: 2787
		[Serializable]
		public class RankSlot
		{
			// Token: 0x060053CD RID: 21453 RVA: 0x00003FB0 File Offset: 0x000021B0
			public RankSlot()
			{
			}

			// Token: 0x060053CE RID: 21454 RVA: 0x001E1E34 File Offset: 0x001E0034
			public void SetState(EnergyController.RankSlot.RankState state)
			{
				Color color = Color.white;
				if (state != EnergyController.RankSlot.RankState.CURRENT)
				{
					if (state != EnergyController.RankSlot.RankState.DISABLED)
					{
						if (state == EnergyController.RankSlot.RankState.NORMAL)
						{
							color = UIColorManager.Orange;
						}
					}
					else
					{
						color = UIColorManager.instance.UIDisabledColor;
					}
				}
				else
				{
					color = UIColorManager.instance.UIBlueColor;
				}
				Graphic rankText = this.RankText;
				Color color2 = color;
				this.Texture.color = color2;
				rankText.color = color2;
				if (state == EnergyController.RankSlot.RankState.DISABLED)
				{
					this.CrossImage.gameObject.SetActive(true);
				}
				else
				{
					this.CrossImage.gameObject.SetActive(false);
				}
			}

			// Token: 0x04004D62 RID: 19810
			public Image Texture;

			// Token: 0x04004D63 RID: 19811
			public Text RankText;

			// Token: 0x04004D64 RID: 19812
			public Image CrossImage;

			// Token: 0x02000AE4 RID: 2788
			public enum RankState
			{
				// Token: 0x04004D66 RID: 19814
				DISABLED,
				// Token: 0x04004D67 RID: 19815
				CURRENT,
				// Token: 0x04004D68 RID: 19816
				NORMAL
			}
		}
	}
}
