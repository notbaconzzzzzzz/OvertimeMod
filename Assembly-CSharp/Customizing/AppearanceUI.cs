using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WorkerSprite;

namespace Customizing
{
	// Token: 0x0200097A RID: 2426
	public class AppearanceUI : MonoBehaviour
	{
		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x060049EE RID: 18926 RVA: 0x0003DCA9 File Offset: 0x0003BEA9
		// (set) Token: 0x060049EF RID: 18927 RVA: 0x0003DCB1 File Offset: 0x0003BEB1
		public AppearanceUI.HairRegion CurrentRegion
		{
			get
			{
				return this._currentRegion;
			}
			set
			{
				this._currentRegion = value;
				this.SetHairRegion();
			}
		}

		// Token: 0x060049F0 RID: 18928 RVA: 0x001BFF40 File Offset: 0x001BE140
		private void Awake()
		{
			this.frontHair.region = SpriteSelectRegion.FRONTHAIR;
			this.rearHair.region = SpriteSelectRegion.REARHAIR;
			this.eyebrow_Def.region = SpriteSelectRegion.EYEBROW_DEF;
			this.eyebrow_Battle.region = SpriteSelectRegion.EYEBROW_BATTLE;
			this.eyebrow_Panic.region = SpriteSelectRegion.EYEBROW_PANIC;
			this.eye_Def.region = SpriteSelectRegion.EYE_DEF;
			this.eye_Panic.region = SpriteSelectRegion.EYE_PANIC;
			this.eye_Dead.region = SpriteSelectRegion.EYE_DEAD;
			this.mouth_Def.region = SpriteSelectRegion.MOUTH_DEF;
			this.mouth_Battle.region = SpriteSelectRegion.MOUTH_BATTLE;
			this.palette.SetSelector(this.hairColor);
		}

		// Token: 0x060049F1 RID: 18929 RVA: 0x0003DCC0 File Offset: 0x0003BEC0
		private void Start()
		{
			this.SetHairRegion();
		}

		// Token: 0x060049F2 RID: 18930 RVA: 0x0003DCC8 File Offset: 0x0003BEC8
		public void OnClickHairMove()
		{
			if (this.CurrentRegion == AppearanceUI.HairRegion.FRONT)
			{
				this.CurrentRegion = AppearanceUI.HairRegion.REAR;
			}
			else if (this.CurrentRegion == AppearanceUI.HairRegion.REAR)
			{
				this.CurrentRegion = AppearanceUI.HairRegion.FRONT;
			}
		}

		// Token: 0x060049F3 RID: 18931 RVA: 0x001BFFD8 File Offset: 0x001BE1D8
		private void SetHairRegion()
		{
			this.hairColor.colorSetted = this.frontHair.Current;
			this.hairColor.OnExitEdit(this.hairColor.CurrentColor);
			this.HairTitle.text = LocalizeTextDataModel.instance.GetText("Customizing_Hair");
		}

		// Token: 0x060049F4 RID: 18932 RVA: 0x001C002C File Offset: 0x001BE22C
		public void OpenWindow(AgentData data)
		{
			this.original = data;
			if (!this.init)
			{
				this.InitialDataLoad();
				this.init = true;
			}
			this.SetCreditControl(true);
			this.SetCopy();
			this.DelegateParamInit(this.copied);
			this.UpdatePortrait();
			this.hairColor.CurrentColor = data.appearance.HairColor;
			this.palette.OnSetColor(this.hairColor.CurrentColor);
			this.CurrentRegion = AppearanceUI.HairRegion.FRONT;
			this.faceType = WorkerFaceType.DEFAULT;
			this.SetFaceText();
			this.SetAppearanceSprite(data);
			this.NameInput.placeholder.GetComponent<Text>().text = data.agentName.GetName();
			this.NameInput.text = string.Empty;
			this.rootObject.SetActive(true);
		}

		// Token: 0x060049F5 RID: 18933 RVA: 0x001C00FC File Offset: 0x001BE2FC
		private void DelegateParamInit(AgentData data)
		{
			this.frontHair.SetData(data);
			this.rearHair.SetData(data);
			this.eyebrow_Def.SetData(data);
			this.eyebrow_Battle.SetData(data);
			this.eyebrow_Panic.SetData(data);
			this.eye_Def.SetData(data);
			this.eye_Panic.SetData(data);
			this.eye_Dead.SetData(data);
			this.mouth_Def.SetData(data);
			this.mouth_Battle.SetData(data);
		}

		// Token: 0x060049F6 RID: 18934 RVA: 0x0003DCF4 File Offset: 0x0003BEF4
		public void OnSetSpriteAcion(ref Sprite target, Sprite sprite)
		{
			target = sprite;
			this.UpdatePortrait();
		}

		// Token: 0x060049F7 RID: 18935 RVA: 0x001C0184 File Offset: 0x001BE384
		public void SetAppearanceSprite(AgentData data)
		{
			this.frontHair.SetInitialIndex(this.frontHair.GetSpriteIndex(data.appearance.FrontHair));
			this.rearHair.SetInitialIndex(this.rearHair.GetSpriteIndex(data.appearance.RearHair));
			this.eyebrow_Def.SetInitialIndex(this.eyebrow_Def.GetSpriteIndex(data.appearance.Eyebrow_Def));
			this.eyebrow_Battle.SetInitialIndex(this.eyebrow_Battle.GetSpriteIndex(data.appearance.Eyebrow_Battle));
			this.eyebrow_Panic.SetInitialIndex(this.eyebrow_Panic.GetSpriteIndex(data.appearance.Eyebrow_Panic));
			this.eye_Def.SetInitialIndex(this.eye_Def.GetSpriteIndex(data.appearance.Eye_Def));
			this.eye_Panic.SetInitialIndex(this.eye_Panic.GetSpriteIndex(data.appearance.Eye_Panic));
			this.eye_Dead.SetInitialIndex(this.eye_Dead.GetSpriteIndex(data.appearance.Eye_Dead));
			this.mouth_Def.SetInitialIndex(this.mouth_Def.GetSpriteIndex(data.appearance.Mouth_Def));
			this.mouth_Battle.SetInitialIndex(this.mouth_Battle.GetSpriteIndex(data.appearance.Mouth_Battle));
		}

		// Token: 0x060049F8 RID: 18936 RVA: 0x0003DCFF File Offset: 0x0003BEFF
		public void GenRandomAppearance()
		{
			CustomizingWindow.CurrentWindow.GenRandomSpriteSet(ref this.copied);
			this.SetAppearanceSprite(this.copied);
			this.palette.OnSetColor(this.copied.appearance.HairColor);
		}

		// Token: 0x060049F9 RID: 18937 RVA: 0x0003DD38 File Offset: 0x0003BF38
		public void GenRandomFace()
		{
			CustomizingWindow.CurrentWindow.GenRandomFaceSpriteSet(ref this.copied);
			this.SetAppearanceSprite(this.copied);
		}

		// Token: 0x060049FA RID: 18938 RVA: 0x0003DD56 File Offset: 0x0003BF56
		public void GenRandomHair()
		{
			CustomizingWindow.CurrentWindow.GenRandomHairSpriteSet(ref this.copied);
			this.SetAppearanceSprite(this.copied);
			this.palette.OnSetColor(this.copied.appearance.HairColor);
		}

		// Token: 0x060049FB RID: 18939 RVA: 0x0003DD8F File Offset: 0x0003BF8F
		public void UpdatePortrait()
		{
			if (this.copied != null)
			{
				this.portrait.SetCustomizing(this.copied, this.faceType);
			}
		}

		// Token: 0x060049FC RID: 18940 RVA: 0x001C02DC File Offset: 0x001BE4DC
		public void ChangeFace(int val)
		{
			int num = (int)(this.faceType + val);
			int num2 = 3;
			if (num < 0)
			{
				num = num + num2 + 1;
			}
			else if (num > 3)
			{
				num -= num2 + 1;
			}
			try
			{
				AgentInfoWindow.currentWindow.GetComponent<AudioClipPlayer>().OnPlayInList(1);
			}
			catch (Exception ex)
			{
			}
			this.faceType = (WorkerFaceType)num;
			this.portrait.SetCustomizing(this.copied, this.faceType);
			this.SetFaceText();
		}

		// Token: 0x060049FD RID: 18941 RVA: 0x001C0364 File Offset: 0x001BE564
		private void SetFaceText()
		{
			string id = string.Empty;
			switch (this.faceType)
			{
			case WorkerFaceType.DEFAULT:
				id = "AgentState_DefaultState";
				break;
			case WorkerFaceType.BATTLE:
				id = "AgentState_BattleState";
				break;
			case WorkerFaceType.PANIC:
				id = "AgentState_Panic";
				break;
			case WorkerFaceType.DEAD:
				id = "AgentState_Dead";
				break;
			}
			string text = string.Format("{0} {1}", LocalizeTextDataModel.instance.GetText(id), LocalizeTextDataModel.instance.GetText("Customizing_Face"));
			this.currentFaceTypeText.text = text;
		}

		// Token: 0x060049FE RID: 18942 RVA: 0x0003DDB3 File Offset: 0x0003BFB3
		public void UpdateColor()
		{
			if (this.copied != null)
			{
				this.copied.appearance.HairColor = this.hairColor.CurrentColor;
			}
			this.UpdatePortrait();
		}

		// Token: 0x060049FF RID: 18943 RVA: 0x001C03F8 File Offset: 0x001BE5F8
		public void InitialDataLoad()
		{
			WorkerBasicSpriteController basicData = WorkerSpriteManager.instance.basicData;
			for (int i = 0; i < 12; i++)
			{
				WorkerBasicSprite workerBasicSprite = null;
				if (basicData.GetData((BasicSpriteRegion)i, out workerBasicSprite))
				{
					List<Sprite> allSprites = workerBasicSprite.GetAllSprites();
					switch (i)
					{
					case 0:
						this.eye_Def.Init(allSprites);
						break;
					case 2:
						this.mouth_Def.Init(allSprites);
						break;
					case 3:
						this.eyebrow_Def.Init(allSprites);
						break;
					case 4:
						this.frontHair.Init(allSprites);
						break;
					case 5:
						this.rearHair.Init(allSprites);
						break;
					case 6:
						this.eye_Panic.Init(allSprites);
						break;
					case 7:
						this.eye_Dead.Init(allSprites);
						break;
					case 8:
						this.mouth_Battle.Init(allSprites);
						break;
					case 9:
						this.eyebrow_Battle.Init(allSprites);
						break;
					case 11:
						this.eyebrow_Panic.Init(allSprites);
						break;
					}
				}
			}
		}

		// Token: 0x06004A00 RID: 18944 RVA: 0x0003DDE1 File Offset: 0x0003BFE1
		private void SetCopy()
		{
			this.copied = new AgentData();
			this.copied.AppearCopy(this.original);
		}

		// Token: 0x06004A01 RID: 18945 RVA: 0x0003DDFF File Offset: 0x0003BFFF
		public void CloseWindow()
		{
			this.rootObject.SetActive(false);
			this.SetCreditControl(true);
		}

		// Token: 0x06004A02 RID: 18946 RVA: 0x0003DE14 File Offset: 0x0003C014
		public void OnConfirm()
		{
			if (this.closeAction != null)
			{
				this.closeAction(this.copied);
			}
			this.CloseWindow();
		}

		// Token: 0x06004A03 RID: 18947 RVA: 0x0003DE38 File Offset: 0x0003C038
		public void OnRevert()
		{
			if (this.closeAction != null)
			{
				this.closeAction(this.original);
			}
			this.CloseWindow();
		}

		// Token: 0x06004A04 RID: 18948 RVA: 0x001C0524 File Offset: 0x001BE724
		public void OnSetNametext(string str)
		{ // <Mod>
			string text = this.NameInput.text;
			if (text.Equals(this.original.agentName.GetName()) || text == string.Empty)
			{
                if (original.isCustomName)
                {
                    copied.isCustomName = true;
                    copied.CustomName = original.CustomName;
                }
                else
                {
				    this.copied.isCustomName = false;
                }
			}
			else
			{
				this.copied.isCustomName = true;
				this.copied.CustomName = text;
				if (AgentNameList.instance.IsUniqueName(text))
				{
					UniqueCreditAgentInfo uniqueCreditInfo = AgentNameList.instance.GetUniqueCreditInfo(text);
					CustomizingWindow.CurrentWindow.GenUniqueSpriteSet(uniqueCreditInfo, ref this.copied);
					this.copied.isUniqueCredit = true;
					this.copied.uniqueScriptIndex = uniqueCreditInfo.scriptId;
					this.palette.OnSetColor(this.copied.appearance.HairColor);
					this.SetCreditControl(false);
				}
			}
		}

		// Token: 0x06004A05 RID: 18949 RVA: 0x001C0600 File Offset: 0x001BE800
		public void SetCreditControl(bool isInteractable)
		{
			float alpha = (!isInteractable) ? 0.5f : 1f;
			foreach (CanvasGroup canvasGroup in this.inputControlGroup)
			{
				canvasGroup.interactable = isInteractable;
				canvasGroup.alpha = alpha;
			}
		}

		// Token: 0x04004444 RID: 17476
		public GameObject rootObject;

		// Token: 0x04004445 RID: 17477
		public ColorPalette palette;

		// Token: 0x04004446 RID: 17478
		public Text currentFaceTypeText;

		// Token: 0x04004447 RID: 17479
		[Space(10f)]
		public InputField NameInput;

		// Token: 0x04004448 RID: 17480
		public SpriteSelector frontHair;

		// Token: 0x04004449 RID: 17481
		public SpriteSelector rearHair;

		// Token: 0x0400444A RID: 17482
		public ColorSelector hairColor;

		// Token: 0x0400444B RID: 17483
		public SpriteSelector eyebrow_Def;

		// Token: 0x0400444C RID: 17484
		public SpriteSelector eyebrow_Battle;

		// Token: 0x0400444D RID: 17485
		public SpriteSelector eyebrow_Panic;

		// Token: 0x0400444E RID: 17486
		public SpriteSelector eye_Def;

		// Token: 0x0400444F RID: 17487
		public SpriteSelector eye_Battle;

		// Token: 0x04004450 RID: 17488
		public SpriteSelector eye_Panic;

		// Token: 0x04004451 RID: 17489
		public SpriteSelector eye_Dead;

		// Token: 0x04004452 RID: 17490
		public ColorSelector eyeColor;

		// Token: 0x04004453 RID: 17491
		public SpriteSelector mouth_Def;

		// Token: 0x04004454 RID: 17492
		public SpriteSelector mouth_Battle;

		// Token: 0x04004455 RID: 17493
		public SpriteSelector mouth_Panic;

		// Token: 0x04004456 RID: 17494
		public CanvasGroup[] inputControlGroup;

		// Token: 0x04004457 RID: 17495
		public AgentData original;

		// Token: 0x04004458 RID: 17496
		public AgentData copied;

		// Token: 0x04004459 RID: 17497
		[NonSerialized]
		public WorkerPortraitSetter portrait;

		// Token: 0x0400445A RID: 17498
		public AppearanceUI.OnCloseAction closeAction;

		// Token: 0x0400445B RID: 17499
		private bool init;

		// Token: 0x0400445C RID: 17500
		private AppearanceUI.HairRegion _currentRegion;

		// Token: 0x0400445D RID: 17501
		public Text HairTitle;

		// Token: 0x0400445E RID: 17502
		private WorkerFaceType faceType;

		// Token: 0x0200097B RID: 2427
		public enum HairRegion
		{
			// Token: 0x04004460 RID: 17504
			FRONT,
			// Token: 0x04004461 RID: 17505
			REAR
		}

		// Token: 0x0200097C RID: 2428
		// (Invoke) Token: 0x06004A07 RID: 18951
		public delegate void OnCloseAction(AgentData data);
	}
}
