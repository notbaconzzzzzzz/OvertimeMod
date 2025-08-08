using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using Spine.Unity.Modules;
using Spine.Unity.Modules.AttachmentTools;
using UnityEngine;

namespace WorkerSprite
{
	// Token: 0x020008CB RID: 2251
	[RequireComponent(typeof(SkeletonAnimator), typeof(Animator))]
	public class WorkerSpriteSetter : MonoBehaviour
	{
		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x060044D5 RID: 17621 RVA: 0x0003A58C File Offset: 0x0003878C
		public WorkerModel Model
		{
			get
			{
				return this._model;
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x060044D6 RID: 17622 RVA: 0x0003A594 File Offset: 0x00038794
		// (set) Token: 0x060044D7 RID: 17623 RVA: 0x0003A5A1 File Offset: 0x000387A1
		public WorkerSprite workerSpriteData
		{
			get
			{
				return this._model.spriteData;
			}
			set
			{
				this._model.spriteData = value;
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x060044D8 RID: 17624 RVA: 0x0003A2B5 File Offset: 0x000384B5
		private SkeletonAnimator skeletonAnimator
		{
			get
			{
				return base.GetComponent<SkeletonAnimator>();
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x060044D9 RID: 17625 RVA: 0x000047B5 File Offset: 0x000029B5
		private Animator animator
		{
			get
			{
				return base.GetComponent<Animator>();
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x060044DA RID: 17626 RVA: 0x0003A5AF File Offset: 0x000387AF
		private Skeleton skeleton
		{
			get
			{
				return this.skeletonAnimator.Skeleton;
			}
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x060044DB RID: 17627 RVA: 0x0003A5BC File Offset: 0x000387BC
		// (set) Token: 0x060044DC RID: 17628 RVA: 0x0003A5C9 File Offset: 0x000387C9
		private SkeletonDataAsset skeletonDataAsset
		{
			get
			{
				return this.skeletonAnimator.SkeletonDataAsset;
			}
			set
			{
				this.skeletonAnimator.skeletonDataAsset = value;
			}
		}

		// Token: 0x060044DD RID: 17629 RVA: 0x001A6E34 File Offset: 0x001A5034
		public Transform GetGiftPos(EGOgiftAttachRegion region)
		{
			Transform result;
			try
			{
				result = this.GiftPos[(int)region];
			}
			catch (Exception message)
			{
				Debug.LogError(message);
				result = this.GiftPos[0];
			}
			return result;
		}

		// Token: 0x060044DE RID: 17630 RVA: 0x0003A5D7 File Offset: 0x000387D7
		public void Init(WorkerModel worker)
		{
			this._model = worker;
		}

		// Token: 0x060044DF RID: 17631 RVA: 0x001A6E70 File Offset: 0x001A5070
		public void SetPanicShadow(bool state, RwbpType type = RwbpType.N)
		{
			if (this.panicRenderer == null)
			{
				return;
			}
			if (!state)
			{
				this.panicRenderer.gameObject.SetActive(false);
				return;
			}
			Sprite panicShadow = WorkerSpriteManager.instance.GetPanicShadow(type);
			if (panicShadow == null)
			{
				this.panicRenderer.gameObject.SetActive(false);
				return;
			}
			this.panicRenderer.gameObject.SetActive(true);
			this.panicRenderer.sprite = panicShadow;
		}

		// Token: 0x060044E0 RID: 17632 RVA: 0x0003A5E0 File Offset: 0x000387E0
		private void Start()
		{
			this.separator = base.GetComponent<SkeletonRenderSeparator>();
			this.ExtractWeaponRegionData();
			this.SetRegionAsTransparent("L Weapon", "Note");
			this.BaiscRendererInit();
			this.SetPanicShadow(false, RwbpType.N);
		}

		// Token: 0x060044E1 RID: 17633 RVA: 0x000043CD File Offset: 0x000025CD
		private void Update()
		{
		}

		// Token: 0x060044E2 RID: 17634 RVA: 0x001A6EE8 File Offset: 0x001A50E8
		private void LateUpdate()
		{
			if (AnimatorUtil.HasParameter(this.animator, "Move"))
			{
				if (this.animator.GetBool("Move") && ((AnimatorUtil.HasParameter(this.animator, "Battle") && this.animator.GetBool("Battle")) || (AnimatorUtil.HasParameter(this.animator, "BattleReady") && this.animator.GetBool("BattleReady")) || (AnimatorUtil.HasParameter(this.animator, "Panic") && this.animator.GetBool("Panic"))) && !this.animator.GetBool("UniqueBattleMove"))
				{
					if (!this.WeaponRenderer.gameObject.activeInHierarchy)
					{
						this.WeaponRenderer.gameObject.SetActive(true);
						return;
					}
				}
				else if (this.WeaponRenderer.gameObject.activeInHierarchy)
				{
					this.WeaponRenderer.gameObject.SetActive(false);
					return;
				}
			}
			else if (this.WeaponRenderer.gameObject.activeInHierarchy)
			{
				this.WeaponRenderer.gameObject.SetActive(false);
			}
		}

		// Token: 0x060044E3 RID: 17635 RVA: 0x0003A612 File Offset: 0x00038812
		public void Reskin()
		{
			this.BasicApply();
			this.EquipmentApply();
		}

		// Token: 0x060044E4 RID: 17636 RVA: 0x0003A620 File Offset: 0x00038820
		public void UniqueFaceReskin()
		{
			this.BaiscUniqueApply();
			this.EquipmentApply();
		}

		// Token: 0x060044E5 RID: 17637 RVA: 0x001A7008 File Offset: 0x001A5208
		public void SetSefira(SefiraEnum sefira, int level = 1)
		{
			Sprite sefiraSymbol = WorkerSpriteManager.instance.GetSefiraSymbol(sefira, level);
			if (this.SymbolRenderer != null)
			{
				this.SymbolRenderer.sprite = sefiraSymbol;
			}
		}

		// Token: 0x060044E6 RID: 17638 RVA: 0x001A703C File Offset: 0x001A523C
		public void SetWorkerType(WorkerModel worker)
		{
			if (worker is AgentModel)
			{
				WorkerSpriteManager.instance.GetAgentArmor(ref this.Model.spriteData);
			}
			else if (worker is OfficerModel)
			{
				this.SymbolRenderer.gameObject.SetActive(false);
				if (worker.GetCurrentSefira() != null)
				{
					WorkerSpriteManager.instance.GetOfficerArmror(ref worker.spriteData, worker.GetCurrentSefira().sefiraEnum);
				}
				this.WeaponRenderer.gameObject.SetActive(false);
			}
			this.UpdateArmorSpriteSet();
			this.ArmorApply();
		}

		// Token: 0x060044E7 RID: 17639 RVA: 0x0003A62E File Offset: 0x0003882E
		private void DebugArmorEquip()
		{
			WorkerSpriteManager.instance.GetArmorData(this.armorId, ref this.Model.spriteData);
			this.UpdateArmorSpriteSet();
			this.ArmorApply();
		}

		// Token: 0x060044E8 RID: 17640 RVA: 0x0003A657 File Offset: 0x00038857
		public void ArmorEquip(int armorId)
		{ // <Patch>
			this.ArmorEquip_Mod(new LobotomyBaseMod.LcId(armorId));
            /*
			this.armorId = armorId;
			WorkerSpriteManager.instance.GetArmorData(armorId, ref this.Model.spriteData);
			this.UpdateArmorSpriteSet();
			this.ArmorApply();*/
		}

		// Token: 0x060044E9 RID: 17641 RVA: 0x001A70C4 File Offset: 0x001A52C4
		public void AddGift(EGOGiftRenderData renderData)
		{
			if (renderData.renderer != null)
			{
				renderData.renderer.sprite = renderData.Sprite;
				renderData.renderer.enabled = true;
				return;
			}
			GameObject gameObject = Prefab.LoadPrefab("Slot/WorkerAttachment");
			Transform giftPos = this.GetGiftPos(renderData.region);
			if (giftPos != null)
			{
				gameObject.transform.SetParent(giftPos);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localScale = Vector3.one;
				if (renderData.Sprite.name.Contains("Mask"))
				{
					gameObject.transform.localPosition = new Vector3(-0.18f, 0f, 0f);
				}
				if (renderData.Sprite.name.Equals("ButterFly"))
				{
					gameObject.transform.localPosition = new Vector3(-0.9f, 0f, 0.1f);
				}
				SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
				component.sprite = renderData.Sprite;
				renderData.renderer = component;
				renderData.renderer.enabled = true;
				return;
			}
			Debug.LogError("Couldnt find Region Transform" + renderData.region);
		}

		// Token: 0x060044EA RID: 17642 RVA: 0x0003A682 File Offset: 0x00038882
		public void ReplaceGift(EGOGiftRenderData renderData)
		{
			this.UpdateAttachment();
		}

		// Token: 0x060044EB RID: 17643 RVA: 0x001A720C File Offset: 0x001A540C
		public void RemoveGiftModel(EGOgiftModel gift)
		{
			if (!this.currentGift.Contains(gift))
			{
				return;
			}
			EGOGiftRenderData egogiftRenderData = null;
			EGOgiftAttachRegion egogiftAttachRegion = EGOgiftAttachRegion.HEAD;
			if (!EGOGiftRegionKey.ParseRegion(gift.metaInfo.attachPos, out egogiftAttachRegion))
			{
				return;
			}
			if (gift.metaInfo.attachType == EGOgiftAttachType.ADD)
			{
				if (!this.attachGiftData.TryGetValue(UnitEGOgiftSpace.GetRegionId(gift.metaInfo), out egogiftRenderData))
				{
					return;
				}
				if (egogiftRenderData.metaId != (long)gift.metaInfo.id)
				{
					return;
				}
				egogiftRenderData.renderer.sprite = null;
				egogiftRenderData.renderer.enabled = false;
				return;
			}
			else if (gift.metaInfo.attachType == EGOgiftAttachType.SPECIAL_ADD)
			{
				if (!this.attachGiftData.TryGetValue(UnitEGOgiftSpace.GetRegionId(gift.metaInfo), out egogiftRenderData))
				{
					return;
				}
				if (egogiftRenderData.metaId != (long)gift.metaInfo.id)
				{
					return;
				}
				egogiftRenderData.renderer.sprite = null;
				egogiftRenderData.renderer.enabled = false;
				return;
			}
			else
			{
				if (!this.replaceGiftData.TryGetValue(egogiftAttachRegion, out egogiftRenderData))
				{
					return;
				}
				this.replaceGiftData.Remove(egogiftAttachRegion);
				this.InitBasicSet();
				if (egogiftAttachRegion == EGOgiftAttachRegion.RIGHTHAND)
				{
					this.UpdateArmorSpriteSet();
					this.ArmorApply();
				}
				return;
			}
		}

		// Token: 0x060044EC RID: 17644 RVA: 0x001A7324 File Offset: 0x001A5524
		public bool TryGetGift(EGOgiftModel model, out EGOGiftRenderData renderData)
		{
			renderData = null;
			if (!this.currentGift.Contains(model))
			{
				return false;
			}
			EGOGiftRenderData egogiftRenderData = null;
			EGOgiftAttachRegion key = EGOgiftAttachRegion.EYE;
			if (!EGOGiftRegionKey.ParseRegion(model.metaInfo.attachPos, out key))
			{
				return false;
			}
			if (model.metaInfo.attachType == EGOgiftAttachType.ADD || model.metaInfo.attachType == EGOgiftAttachType.SPECIAL_ADD)
			{
				if (this.attachGiftData.TryGetValue(UnitEGOgiftSpace.GetRegionId(model.metaInfo), out egogiftRenderData))
				{
					renderData = egogiftRenderData;
					return true;
				}
			}
			else if (this.replaceGiftData.TryGetValue(key, out egogiftRenderData))
			{
				renderData = egogiftRenderData;
				return true;
			}
			return false;
		}

		// Token: 0x060044ED RID: 17645 RVA: 0x001A73B0 File Offset: 0x001A55B0
		public void AddGiftModel(EGOgiftModel gift)
		{ // <Patch>
			if (!this.currentGift.Contains(gift))
			{
				this.currentGift.Add(gift);
			}
			EGOGiftRenderData egogiftRenderData = null;
			EGOgiftAttachRegion egogiftAttachRegion = EGOgiftAttachRegion.EYE;
			string sprite = gift.metaInfo.sprite;
			string empty = string.Empty;
			string empty2 = string.Empty;
			if (EGOGiftRegionKey.ParseRegion(gift.metaInfo.attachPos, out egogiftAttachRegion))
			{
				Sprite attachmentSprite = WorkerSpriteManager.instance.GetAttachmentSprite(egogiftAttachRegion, sprite);
				if (attachmentSprite == null)
				{
					Debug.LogError("Couldn't find : " + gift.metaInfo.Name + " Searched as " + sprite);
				}
				else
				{
					if (gift.metaInfo.attachType == EGOgiftAttachType.ADD || gift.metaInfo.attachType == EGOgiftAttachType.SPECIAL_ADD)
					{
						if (this.attachGiftData.TryGetValue(UnitEGOgiftSpace.GetRegionId(gift.metaInfo), out egogiftRenderData))
						{
							egogiftRenderData.Sprite = attachmentSprite;
							egogiftRenderData.DataName = gift.metaInfo.Name;
							egogiftRenderData.metaId = (long)gift.metaInfo.id;
							egogiftRenderData.modid = EquipmentTypeInfo.GetLcId(gift.metaInfo).packageId;
							this.AddGift(egogiftRenderData);
						}
						else
						{
							if (!EGOGiftRegionKey.GetRegionKey(egogiftAttachRegion, out empty, out empty2))
							{
								Debug.LogError("Error " + egogiftAttachRegion.ToString());
							}
							else
							{
								egogiftRenderData = new EGOGiftRenderData
								{
									Sprite = attachmentSprite,
									slot = empty,
									attachmentName = empty2,
									DataName = gift.metaInfo.Name,
									region = egogiftAttachRegion,
									attachType = gift.metaInfo.attachType,
									metaId = (long)gift.metaInfo.id
								};
								egogiftRenderData.modid = EquipmentTypeInfo.GetLcId(gift.metaInfo).packageId;
								this.attachGiftData.Add(UnitEGOgiftSpace.GetRegionId(gift.metaInfo), egogiftRenderData);
								this.AddGift(egogiftRenderData);
							}
						}
					}
					else
					{
						if (egogiftAttachRegion != EGOgiftAttachRegion.BACK && egogiftAttachRegion != EGOgiftAttachRegion.BACK2 && egogiftAttachRegion != EGOgiftAttachRegion.HEADBACK)
						{
                            if (this.replaceGiftData.TryGetValue(egogiftAttachRegion, out egogiftRenderData))
                            {
                                egogiftRenderData.Sprite = attachmentSprite;
                                egogiftRenderData.DataName = gift.metaInfo.Name;
                                this.ReplaceGift(egogiftRenderData);
                            }
                            else
                            {
                                if (!EGOGiftRegionKey.GetRegionKey(egogiftAttachRegion, out empty, out empty2))
                                {
                                    Debug.LogError("Error " + egogiftAttachRegion.ToString());
                                }
                                else
                                {
                                    egogiftRenderData = new EGOGiftRenderData
                                    {
                                        Sprite = attachmentSprite,
                                        slot = empty,
                                        attachmentName = empty2,
                                        DataName = gift.metaInfo.Name,
                                        region = egogiftAttachRegion,
                                        attachType = gift.metaInfo.attachType,
                                        metaId = (long)gift.metaInfo.id
                                    };
                                    egogiftRenderData.modid = EquipmentTypeInfo.GetLcId(gift.metaInfo).packageId;
                                    this.replaceGiftData.Add(egogiftAttachRegion, egogiftRenderData);
                                    this.ReplaceGift(egogiftRenderData);
                                }
                            }
						}
					}
				}
			}
            /*
			if (!this.currentGift.Contains(gift))
			{
				this.currentGift.Add(gift);
			}
			EGOGiftRenderData egogiftRenderData = null;
			EGOgiftAttachRegion egogiftAttachRegion = EGOgiftAttachRegion.EYE;
			string sprite = gift.metaInfo.sprite;
			string empty = string.Empty;
			string empty2 = string.Empty;
			if (EGOGiftRegionKey.ParseRegion(gift.metaInfo.attachPos, out egogiftAttachRegion))
			{
				Sprite attachmentSprite = WorkerSpriteManager.instance.GetAttachmentSprite(egogiftAttachRegion, sprite);
				if (attachmentSprite == null)
				{
					Debug.LogError("Couldn't find : " + gift.metaInfo.Name + " Searched as " + sprite);
					return;
				}
				if (gift.metaInfo.attachType == EGOgiftAttachType.ADD || gift.metaInfo.attachType == EGOgiftAttachType.SPECIAL_ADD)
				{
					if (this.attachGiftData.TryGetValue(UnitEGOgiftSpace.GetRegionId(gift.metaInfo), out egogiftRenderData))
					{
						egogiftRenderData.Sprite = attachmentSprite;
						egogiftRenderData.DataName = gift.metaInfo.Name;
						egogiftRenderData.metaId = (long)gift.metaInfo.id;
						this.AddGift(egogiftRenderData);
						return;
					}
					if (!EGOGiftRegionKey.GetRegionKey(egogiftAttachRegion, out empty, out empty2))
					{
						Debug.LogError("Error " + egogiftAttachRegion);
						return;
					}
					egogiftRenderData = new EGOGiftRenderData
					{
						Sprite = attachmentSprite,
						slot = empty,
						attachmentName = empty2,
						DataName = gift.metaInfo.Name,
						region = egogiftAttachRegion,
						attachType = gift.metaInfo.attachType,
						metaId = (long)gift.metaInfo.id
					};
					this.attachGiftData.Add(UnitEGOgiftSpace.GetRegionId(gift.metaInfo), egogiftRenderData);
					this.AddGift(egogiftRenderData);
					return;
				}
				else
				{
					if (egogiftAttachRegion == EGOgiftAttachRegion.BACK)
					{
						return;
					}
					if (egogiftAttachRegion == EGOgiftAttachRegion.BACK2)
					{
						return;
					}
					if (egogiftAttachRegion == EGOgiftAttachRegion.HEADBACK)
					{
						return;
					}
					if (this.replaceGiftData.TryGetValue(egogiftAttachRegion, out egogiftRenderData))
					{
						egogiftRenderData.Sprite = attachmentSprite;
						egogiftRenderData.DataName = gift.metaInfo.Name;
						this.ReplaceGift(egogiftRenderData);
						return;
					}
					if (!EGOGiftRegionKey.GetRegionKey(egogiftAttachRegion, out empty, out empty2))
					{
						Debug.LogError("Error " + egogiftAttachRegion);
						return;
					}
					egogiftRenderData = new EGOGiftRenderData
					{
						Sprite = attachmentSprite,
						slot = empty,
						attachmentName = empty2,
						DataName = gift.metaInfo.Name,
						region = egogiftAttachRegion,
						attachType = gift.metaInfo.attachType,
						metaId = (long)gift.metaInfo.id
					};
					this.replaceGiftData.Add(egogiftAttachRegion, egogiftRenderData);
					this.ReplaceGift(egogiftRenderData);
				}
			}*/
		}

		// Token: 0x060044EE RID: 17646 RVA: 0x001A761C File Offset: 0x001A581C
		public void CheckGiftModel(List<EGOgiftModel> gifts)
		{
			List<EGOgiftModel> list = new List<EGOgiftModel>();
			foreach (EGOgiftModel item in this.currentGift)
			{
				if (!gifts.Contains(item))
				{
					list.Add(item);
				}
			}
			foreach (EGOgiftModel egogiftModel in list)
			{
				this.RemoveGiftModel(egogiftModel);
				this.currentGift.Remove(egogiftModel);
			}
			this.UpdateAttachment();
		}

		// Token: 0x060044EF RID: 17647 RVA: 0x001A76D0 File Offset: 0x001A58D0
		public void UpdateBasicSpriteSet()
		{
			this.currentSpriteSet.Eye = this.workerSpriteData.Eye;
			this.currentSpriteSet.EyeClose = this.workerSpriteData.EyeClose;
			this.currentSpriteSet.Eyebrow = this.workerSpriteData.EyeBrow;
			this.currentSpriteSet.Mouth = this.workerSpriteData.Mouth;
			this.currentSpriteSet.FrontHair = this.workerSpriteData.FrontHair;
			this.currentSpriteSet.RearHair = this.workerSpriteData.RearHair;
			this.hairColor = this.workerSpriteData.HairColor;
			this.eyeColor = this.workerSpriteData.EyeColor;
		}

		// Token: 0x060044F0 RID: 17648 RVA: 0x001A7784 File Offset: 0x001A5984
		public void UpdateArmorSpriteSet()
		{
			this.currentSpriteSet.Body = this.workerSpriteData.Armor.Body;
			this.currentSpriteSet.Arm_Left_Up = this.workerSpriteData.Armor.Arm_Left_Up;
			this.currentSpriteSet.Arm_Left_Down = this.workerSpriteData.Armor.Arm_Left_Down;
			this.currentSpriteSet.Arm_Right_Up = this.workerSpriteData.Armor.Arm_Right_Up;
			this.currentSpriteSet.Arm_Right_Down = this.workerSpriteData.Armor.Arm_Right_Down;
			this.currentSpriteSet.Leg_Left_Up = this.workerSpriteData.Armor.Leg_Left_Up;
			this.currentSpriteSet.Leg_Left_Down = this.workerSpriteData.Armor.Leg_Left_Down;
			this.currentSpriteSet.Leg_Right_Up = this.workerSpriteData.Armor.Leg_Right_Up;
			this.currentSpriteSet.Leg_Right_Down = this.workerSpriteData.Armor.Leg_Right_Down;
			this.currentSpriteSet.Coat_Back = this.workerSpriteData.Armor.Coat_Back;
			this.currentSpriteSet.Coat_Left = this.workerSpriteData.Armor.Coat_Left;
			this.currentSpriteSet.Coat_Right = this.workerSpriteData.Armor.Coat_Right;
			this.currentSpriteSet.Left_Hand = this.workerSpriteData.Armor.Left_Hand;
			this.currentSpriteSet.Right_Hand = this.workerSpriteData.Armor.Right_Hand;
		}

		// Token: 0x060044F1 RID: 17649 RVA: 0x0003A68A File Offset: 0x0003888A
		public void ChangeBasicSpriteData()
		{
			this.UpdateBasicSpriteSet();
			this.BasicApply();
		}

		// Token: 0x060044F2 RID: 17650 RVA: 0x0003A698 File Offset: 0x00038898
		public void LoadBasicSpriteData()
		{
			WorkerSpriteManager.instance.GetRandomBasicData(this.Model.spriteData, true);
			this.UpdateBasicSpriteSet();
			this.BasicApply();
		}

		// Token: 0x060044F3 RID: 17651 RVA: 0x0003A6BC File Offset: 0x000388BC
		public void InitBasicSet()
		{
			this.UpdateBasicSpriteSet();
			this.BasicApply();
			this.UpdateAttachment();
		}

		// Token: 0x060044F4 RID: 17652 RVA: 0x001A790C File Offset: 0x001A5B0C
		public void UpdateAttachment()
		{ // <Patch>
			List<SpineChangeData> list = new List<SpineChangeData>();
			using (Dictionary<EGOgiftAttachRegion, EGOGiftRenderData>.ValueCollection.Enumerator enumerator = this.replaceGiftData.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					EGOGiftRenderData r = enumerator.Current;
					SpineChangeData spineChangeData = new SpineChangeData
					{
						sprite = r.Sprite,
						slot = r.slot,
						attachmentName = r.attachmentName
					};
					try
					{
						if (r.region == EGOgiftAttachRegion.RIGHTHAND && r.attachType == EGOgiftAttachType.REPLACE)
						{
							EGOgiftModel egogiftModel = this.Model.Equipment.gifts.replacedGifts.Find((EGOgiftModel x) => EquipmentTypeInfo.GetLcId(x.metaInfo) == EGOGiftRenderData.GetLcId(r));
							if (egogiftModel != null && !this.Model.Equipment.gifts.GetDisplayState(egogiftModel))
							{
								spineChangeData.sprite = WorkerSpriteManager.instance.righthand;
								list.Add(spineChangeData);
								continue;
							}
						}
					}
					catch (Exception)
					{
					}
					if (r.region == EGOgiftAttachRegion.MOUTH || r.region == EGOgiftAttachRegion.EYE)
					{
						if (r.region == EGOgiftAttachRegion.EYE)
						{
							this.workerSpriteData.replaced.Eye = r.Sprite;
							this.currentSpriteSet.Eye = r.Sprite;
							this.workerSpriteData.EyeColor = Color.white;
							this.EyeApply();
						}
						else
						{
							if (r.region == EGOgiftAttachRegion.MOUTH)
							{
								this.MouthApply();
							}
						}
					}
					else
					{
						list.Add(spineChangeData);
					}
				}
			}
			this.Apply(list);
            /*
			List<SpineChangeData> list = new List<SpineChangeData>();
			using (Dictionary<EGOgiftAttachRegion, EGOGiftRenderData>.ValueCollection.Enumerator enumerator = this.replaceGiftData.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					EGOGiftRenderData r = enumerator.Current;
					SpineChangeData spineChangeData = new SpineChangeData
					{
						sprite = r.Sprite,
						slot = r.slot,
						attachmentName = r.attachmentName
					};
					try
					{
						if (r.region == EGOgiftAttachRegion.RIGHTHAND && r.attachType == EGOgiftAttachType.REPLACE)
						{
							EGOgiftModel egogiftModel = this.Model.Equipment.gifts.replacedGifts.Find((EGOgiftModel x) => (long)x.metaInfo.id == r.metaId);
							if (egogiftModel != null && !this.Model.Equipment.gifts.GetDisplayState(egogiftModel))
							{
								spineChangeData.sprite = WorkerSpriteManager.instance.righthand;
								list.Add(spineChangeData);
								continue;
							}
						}
					}
					catch (Exception)
					{
					}
					if (r.region == EGOgiftAttachRegion.MOUTH || r.region == EGOgiftAttachRegion.EYE)
					{
						if (r.region == EGOgiftAttachRegion.EYE)
						{
							this.workerSpriteData.replaced.Eye = r.Sprite;
							this.currentSpriteSet.Eye = r.Sprite;
							this.workerSpriteData.EyeColor = Color.white;
							this.EyeApply();
						}
						else if (r.region == EGOgiftAttachRegion.MOUTH)
						{
							this.MouthApply();
						}
					}
					else
					{
						list.Add(spineChangeData);
					}
				}
			}
			this.Apply(list);*/
		}

		// Token: 0x060044F5 RID: 17653 RVA: 0x001A7AEC File Offset: 0x001A5CEC
		public void BaiscUniqueApply()
		{
			List<SpineChangeData> list = new List<SpineChangeData>();
			SpineChangeData spineChangeData = new SpineChangeData();
			SpineChangeData spineChangeData2 = new SpineChangeData();
			spineChangeData.sprite = this.currentSpriteSet.FrontHair;
			spineChangeData2.sprite = this.currentSpriteSet.RearHair;
			WorkerBasicRegionKey.GetKey(BasicSpriteRegion.HAIR_FRONT, out spineChangeData.slot, out spineChangeData.attachmentName);
			WorkerBasicRegionKey.GetKey(BasicSpriteRegion.HAIR_REAR, out spineChangeData2.slot, out spineChangeData2.attachmentName);
			spineChangeData.isSettedColor = (spineChangeData2.isSettedColor = true);
			spineChangeData.regionColor = (spineChangeData2.regionColor = this.hairColor);
			list.Add(spineChangeData);
			list.Add(spineChangeData2);
			if (this.SetHeadSprite)
			{
				SpineChangeData spineChangeData3 = new SpineChangeData
				{
					sprite = this.HeadSprite
				};
				spineChangeData3.slot = (spineChangeData3.attachmentName = "Head");
				list.Add(spineChangeData3);
			}
			this.Apply(list);
		}

		// Token: 0x060044F6 RID: 17654 RVA: 0x001A7BCC File Offset: 0x001A5DCC
		public void BasicApply()
		{
			List<SpineChangeData> list = new List<SpineChangeData>();
			SpineChangeData spineChangeData = new SpineChangeData();
			SpineChangeData spineChangeData2 = new SpineChangeData();
			spineChangeData.sprite = this.currentSpriteSet.FrontHair;
			spineChangeData2.sprite = this.currentSpriteSet.RearHair;
			WorkerBasicRegionKey.GetKey(BasicSpriteRegion.HAIR_FRONT, out spineChangeData.slot, out spineChangeData.attachmentName);
			WorkerBasicRegionKey.GetKey(BasicSpriteRegion.HAIR_REAR, out spineChangeData2.slot, out spineChangeData2.attachmentName);
			spineChangeData.isSettedColor = (spineChangeData2.isSettedColor = true);
			spineChangeData.regionColor = (spineChangeData2.regionColor = this.hairColor);
			list.Add(spineChangeData);
			list.Add(spineChangeData2);
			this.EyeApply();
			this.MouthApply();
			if (this.SetHeadSprite)
			{
				SpineChangeData spineChangeData3 = new SpineChangeData
				{
					sprite = this.HeadSprite
				};
				spineChangeData3.slot = (spineChangeData3.attachmentName = "Head");
				list.Add(spineChangeData3);
			}
			this.Apply(list);
		}

		// Token: 0x060044F7 RID: 17655 RVA: 0x001A7CB8 File Offset: 0x001A5EB8
		public void SetWeaponTransparent()
		{
			List<SpineChangeData> list = new List<SpineChangeData>();
			string empty = string.Empty;
			string empty2 = string.Empty;
			if (this.currentWeaponType != WeaponClassType.FIST)
			{
				if (WeaponRegionKey.GetKey(this.currentWeaponType, out empty, out empty2))
				{
					SpineChangeData item = new SpineChangeData
					{
						sprite = this.TransparentSprite,
						slot = empty,
						attachmentName = empty2
					};
					list.Add(item);
					this.Apply(list);
				}
				this.WeaponRenderer.sprite = this.TransparentSprite;
			}
		}

		// Token: 0x060044F8 RID: 17656 RVA: 0x001A7D30 File Offset: 0x001A5F30
		public void SetRegionAsTransparent(string slot, string attachmentName)
		{
			List<SpineChangeData> list = new List<SpineChangeData>();
			SpineChangeData item = new SpineChangeData
			{
				sprite = this.TransparentSprite,
				slot = slot,
				attachmentName = attachmentName
			};
			list.Add(item);
			this.Apply(list);
		}

		// Token: 0x060044F9 RID: 17657 RVA: 0x001A7D74 File Offset: 0x001A5F74
		public void SetRightWeapon(WeaponClassType type, Sprite sprite)
		{
			List<SpineChangeData> list = new List<SpineChangeData>();
			string empty = string.Empty;
			string empty2 = string.Empty;
			this.currentWeaponType = type;
			if (sprite == null)
			{
				this.WeaponRenderer.enabled = false;
				return;
			}
			this.WeaponRenderer.enabled = true;
			if (type == WeaponClassType.OFFICER)
			{
				Debug.Log("Officer Weapon setted");
				this.WeaponRenderer.enabled = false;
				this.WeaponRenderer.sprite = null;
			}
			if (WeaponRegionKey.GetKey(type, out empty, out empty2))
			{
				SpineChangeData item = new SpineChangeData
				{
					sprite = sprite,
					slot = empty,
					attachmentName = empty2
				};
				if (type != WeaponClassType.RIFLE)
				{
					string empty3 = string.Empty;
					string empty4 = string.Empty;
					if (WeaponRegionKey.GetKey(WeaponClassType.RIFLE, out empty3, out empty4))
					{
						SpineChangeData item2 = new SpineChangeData
						{
							attachmentName = empty4,
							slot = empty3,
							sprite = this.TransparentSprite
						};
						list.Add(item2);
					}
				}
				list.Add(item);
				this.Apply(list);
			}
			this.WeaponRenderer.sprite = sprite;
			this.ExtractWeaponRegionData();
			this.WeaponRenderer.transform.localPosition = new Vector3(this._weaponPosition.x, this._weaponPosition.y, -0.02f);
			this.WeaponRenderer.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, this._weaponRotation));
		}

		// Token: 0x060044FA RID: 17658 RVA: 0x001A7ED0 File Offset: 0x001A60D0
		public void SetLeftWeapon(WeaponClassType type, Sprite sprite)
		{
			List<SpineChangeData> list = new List<SpineChangeData>();
			string slot = string.Empty;
			string attachmentName = string.Empty;
			this.currentWeaponType = type;
			slot = WeaponRegionKey.Left;
			attachmentName = WeaponRegionKey.FistLeft;
			SpineChangeData item = new SpineChangeData
			{
				sprite = sprite,
				slot = slot,
				attachmentName = attachmentName
			};
			list.Add(item);
			this.Apply(list);
		}

		// Token: 0x060044FB RID: 17659 RVA: 0x0003A6D0 File Offset: 0x000388D0
		public void EyeApply()
		{
			this.EyeRenderer.sprite = this.currentSpriteSet.Eye;
			this.EyeRenderer.color = this.eyeColor;
			this.EyebrowRenderer.sprite = this.currentSpriteSet.Eyebrow;
		}

		// Token: 0x060044FC RID: 17660 RVA: 0x001A7F2C File Offset: 0x001A612C
		public void MouthApply()
		{
			this.MouthRenderer.sprite = this.currentSpriteSet.Mouth;
			EGOgiftModel egogiftModel = this.Model.Equipment.gifts.replacedGifts.Find((EGOgiftModel x) => x.metaInfo.attachPos == "mouth" && x.metaInfo.attachType == EGOgiftAttachType.REPLACE);
			if (egogiftModel != null && this.Model.GetGiftDisplayState(egogiftModel))
			{
				Sprite attachmentSprite = WorkerSpriteManager.instance.GetAttachmentSprite(EGOgiftAttachRegion.MOUTH, egogiftModel.metaInfo.sprite);
				if (attachmentSprite != null)
				{
					this.MouthRenderer.enabled = false;
					this.MouthReplaceGiftRender.enabled = true;
					this.MouthReplaceGiftRender.sprite = attachmentSprite;
					return;
				}
			}
			this.MouthRenderer.enabled = true;
			this.MouthReplaceGiftRender.enabled = false;
		}

		// Token: 0x060044FD RID: 17661 RVA: 0x001A7FF8 File Offset: 0x001A61F8
		public void OnDie(bool isPanic)
		{
			if (isPanic)
			{
				this.currentSpriteSet.Eye = this.workerSpriteData.EyePanic;
				this.currentSpriteSet.Eyebrow = this.workerSpriteData.EyeBrow;
				this.currentSpriteSet.Mouth = this.workerSpriteData.PanicMouth;
			}
			else
			{
				this.currentSpriteSet.Eye = this.workerSpriteData.EyeDead;
				this.currentSpriteSet.Eyebrow = this.workerSpriteData.EyeBrow;
				this.currentSpriteSet.Mouth = this.workerSpriteData.PanicMouth;
			}
			this.EyeApply();
			this.MouthApply();
		}

		// Token: 0x060044FE RID: 17662 RVA: 0x001A809C File Offset: 0x001A629C
		public void SetWorkerFaceType(WorkerFaceType type)
		{
			this.faceType = type;
			switch (type)
			{
			case WorkerFaceType.DEFAULT:
				this.currentSpriteSet.Eye = this.workerSpriteData.Eye;
				this.currentSpriteSet.Eyebrow = this.workerSpriteData.EyeBrow;
				this.currentSpriteSet.Mouth = this.workerSpriteData.Mouth;
				break;
			case WorkerFaceType.BATTLE:
				this.currentSpriteSet.Eye = this.workerSpriteData.Eye;
				this.currentSpriteSet.Eyebrow = this.workerSpriteData.BattleEyeBrow;
				this.currentSpriteSet.Mouth = this.workerSpriteData.BattleMouth;
				break;
			case WorkerFaceType.PANIC:
				this.currentSpriteSet.Eye = this.workerSpriteData.EyePanic;
				this.currentSpriteSet.Eyebrow = this.workerSpriteData.PanicEyeBrow;
				this.currentSpriteSet.Mouth = this.workerSpriteData.PanicMouth;
				break;
			case WorkerFaceType.DEAD:
				this.currentSpriteSet.Eye = this.workerSpriteData.EyeDead;
				this.currentSpriteSet.Eyebrow = this.workerSpriteData.EyeBrow;
				this.currentSpriteSet.Mouth = this.workerSpriteData.PanicMouth;
				break;
			}
			this.EyeApply();
			this.MouthApply();
		}

		// Token: 0x060044FF RID: 17663 RVA: 0x0003A70F File Offset: 0x0003890F
		public void EquipmentApply()
		{
			this.ArmorApply();
		}

		// Token: 0x06004500 RID: 17664 RVA: 0x001A81EC File Offset: 0x001A63EC
		public void ArmorApply()
		{
			List<SpineChangeData> list = new List<SpineChangeData>();
			SpineChangeData spineChangeData = new SpineChangeData();
			SpineChangeData spineChangeData2 = new SpineChangeData();
			SpineChangeData spineChangeData3 = new SpineChangeData();
			SpineChangeData spineChangeData4 = new SpineChangeData();
			SpineChangeData spineChangeData5 = new SpineChangeData();
			SpineChangeData spineChangeData6 = new SpineChangeData();
			SpineChangeData spineChangeData7 = new SpineChangeData();
			SpineChangeData spineChangeData8 = new SpineChangeData();
			SpineChangeData spineChangeData9 = new SpineChangeData();
			SpineChangeData spineChangeData10 = new SpineChangeData();
			SpineChangeData spineChangeData11 = new SpineChangeData();
			SpineChangeData spineChangeData12 = new SpineChangeData();
			SpineChangeData spineChangeData13 = new SpineChangeData();
			SpineChangeData spineChangeData14 = new SpineChangeData();
			spineChangeData.sprite = this.currentSpriteSet.Body;
			spineChangeData2.sprite = this.currentSpriteSet.Arm_Left_Up;
			spineChangeData3.sprite = this.currentSpriteSet.Arm_Left_Down;
			spineChangeData4.sprite = this.currentSpriteSet.Arm_Right_Up;
			spineChangeData5.sprite = this.currentSpriteSet.Arm_Right_Down;
			spineChangeData6.sprite = this.currentSpriteSet.Leg_Left_Up;
			spineChangeData7.sprite = this.currentSpriteSet.Leg_Left_Down;
			spineChangeData8.sprite = this.currentSpriteSet.Leg_Right_Up;
			spineChangeData9.sprite = this.currentSpriteSet.Leg_Right_Down;
			spineChangeData10.sprite = this.currentSpriteSet.Coat_Back;
			spineChangeData11.sprite = this.currentSpriteSet.Coat_Left;
			spineChangeData12.sprite = this.currentSpriteSet.Coat_Right;
			spineChangeData13.sprite = this.currentSpriteSet.Left_Hand;
			spineChangeData14.sprite = this.currentSpriteSet.Right_Hand;
			if (this.workerSpriteData.SetArmorColor)
			{
				spineChangeData4.isSettedColor = (spineChangeData3.isSettedColor = (spineChangeData2.isSettedColor = (spineChangeData5.isSettedColor = (spineChangeData.isSettedColor = true))));
				spineChangeData4.regionColor = (spineChangeData3.regionColor = (spineChangeData2.regionColor = (spineChangeData5.regionColor = (spineChangeData.regionColor = this.workerSpriteData.ArmorColor))));
			}
			list.Add(spineChangeData3);
			list.Add(spineChangeData2);
			list.Add(spineChangeData5);
			list.Add(spineChangeData4);
			list.Add(spineChangeData);
			list.Add(spineChangeData6);
			list.Add(spineChangeData7);
			list.Add(spineChangeData13);
			list.Add(spineChangeData8);
			list.Add(spineChangeData9);
			list.Add(spineChangeData14);
			list.Add(spineChangeData10);
			list.Add(spineChangeData11);
			list.Add(spineChangeData12);
			this.SetBodyRegionKey(list);
			this.Apply(list);
		}

		// Token: 0x06004501 RID: 17665 RVA: 0x001A8454 File Offset: 0x001A6654
		public void SetBodyRegionKey(List<SpineChangeData> data)
		{
			for (int i = 0; i < 14; i++)
			{
				WorkerBodyRegionKey.GetKey((BodySpriteRegion)i, out data[i].slot, out data[i].attachmentName);
			}
		}

		// Token: 0x06004502 RID: 17666 RVA: 0x001A8490 File Offset: 0x001A6690
		public void WeaponApply()
		{
			List<SpineChangeData> data = new List<SpineChangeData>();
			SpineChangeData spineChangeData = new SpineChangeData();
			SpineChangeData spineChangeData2 = new SpineChangeData();
			spineChangeData.sprite = this.currentSpriteSet.Weapon_Left;
			spineChangeData2.sprite = this.currentSpriteSet.Weapon_Right;
			spineChangeData.slot = WeaponRegionKey.Left;
			spineChangeData2.slot = WeaponRegionKey.Right;
			spineChangeData.attachmentName = WeaponRegionKey.FistLeft;
			spineChangeData2.attachmentName = WeaponRegionKey.Rifle;
			this.Apply(data);
		}

		// Token: 0x06004503 RID: 17667 RVA: 0x001A8504 File Offset: 0x001A6704
		public void Apply(List<SpineChangeData> data)
		{
			Skeleton skeleton = this.skeleton;
			Skin skin = skeleton.UnshareSkin(true, false, null);
			foreach (SpineChangeData data2 in data)
			{
				this.ChangeSpineData(skeleton, skin, data2);
			}
			bool flag = this.repack;
			this.currentSkin = skin;
			skeleton.SetSkin(skin);
			skeleton.SetToSetupPose();
		}

		// Token: 0x06004504 RID: 17668 RVA: 0x001A8580 File Offset: 0x001A6780
		private void ChangeSpineData(Skeleton skeleton, Skin newSkin, SpineChangeData data)
		{
			bool flag = false;
			int slotIndex = skeleton.FindSlotIndex(data.slot);
			if (data.sprite == null)
			{
				Debug.Log(data.slot + " sprite is null");
				return;
			}
			RegionAttachment regionAttachment = data.sprite.ToRegionAttachmentPMAClone(Shader.Find("Spine/Skeleton"), TextureFormat.RGBA32, true, null, 0f);
			Attachment attachment = skeleton.GetAttachment(slotIndex, data.attachmentName);
			if (attachment is RegionAttachment)
			{
				RegionAttachment regionAttachment2 = attachment as RegionAttachment;
				if (data.isSettedColor)
				{
					regionAttachment.SetColor(data.regionColor);
				}
				if (data.isSettedOffset)
				{
					regionAttachment.SetPositionOffset(regionAttachment2.X + data.position.x, regionAttachment2.Y + data.position.y);
					regionAttachment.rotation = regionAttachment2.rotation + data.rotation;
					regionAttachment.SetScale(new Vector2(regionAttachment2.ScaleX + data.scale.x, regionAttachment2.ScaleY + data.scale.y));
				}
				else
				{
					regionAttachment.Rotation = regionAttachment2.Rotation;
					regionAttachment.SetPositionOffset(regionAttachment2.X, regionAttachment2.Y);
					regionAttachment.SetScale(regionAttachment2.ScaleX, regionAttachment2.ScaleY);
				}
				regionAttachment.UpdateOffset();
				try
				{
					newSkin.AddAttachment(slotIndex, data.attachmentName, regionAttachment);
					goto IL_1D1;
				}
				catch (Exception message)
				{
					Debug.LogError(message);
					goto IL_1D1;
				}
			}
			Attachment clone = newSkin.GetAttachment(slotIndex, data.attachmentName).GetClone(true);
			clone.SetRegion(regionAttachment.GetRegion(), true);
			if (clone is MeshAttachment)
			{
				MeshAttachment attachment2 = clone as MeshAttachment;
				if (data.isSettedColor)
				{
					attachment2.SetColor(data.regionColor);
				}
				else
				{
					attachment2.SetColor(Color.white);
				}
			}
			try
			{
				if (clone != null)
				{
					newSkin.AddAttachment(slotIndex, data.attachmentName, clone);
				}
				else
				{
					flag = true;
				}
			}
			catch (Exception message2)
			{
				Debug.Log(message2);
				flag = true;
			}
			IL_1D1:
			try
			{
				if (!flag)
				{
					skeleton.SetAttachment(data.slot, data.attachmentName);
				}
			}
			catch (Exception message3)
			{
				Debug.Log(message3);
			}
		}

		// Token: 0x06004505 RID: 17669 RVA: 0x001A87A8 File Offset: 0x001A69A8
		private void ChangeSpineData(Skeleton skeleton, Skin newSkin, SpineChangeData data, ref Vector3 PositionFix)
		{
			int slotIndex = skeleton.FindSlotIndex(data.slot);
			RegionAttachment regionAttachment = data.sprite.ToRegionAttachmentPMAClone(Shader.Find("Spine/Skeleton"), TextureFormat.RGBA32, false, null, 0f);
			Attachment attachment = skeleton.GetAttachment(slotIndex, data.attachmentName);
			if (attachment is RegionAttachment)
			{
				RegionAttachment regionAttachment2 = attachment as RegionAttachment;
				if (data.isSettedColor)
				{
					regionAttachment.SetColor(data.regionColor);
				}
				if (data.isSettedOffset)
				{
					regionAttachment.SetPositionOffset(regionAttachment2.X + data.position.x, regionAttachment2.Y + data.position.y);
					regionAttachment.rotation = regionAttachment2.rotation + data.rotation;
					regionAttachment.SetScale(new Vector2(regionAttachment2.ScaleX + data.scale.x, regionAttachment2.ScaleY + data.scale.y));
				}
				else
				{
					regionAttachment.Rotation = regionAttachment2.Rotation;
					Vector2 v = new Vector2(regionAttachment2.X, regionAttachment2.Y);
					PositionFix = v;
				}
				regionAttachment.UpdateOffset();
				newSkin.AddAttachment(slotIndex, data.attachmentName, regionAttachment);
			}
			else
			{
				Attachment clone = newSkin.GetAttachment(slotIndex, data.attachmentName).GetClone(true);
				clone.SetRegion(regionAttachment.GetRegion(), true);
				newSkin.AddAttachment(slotIndex, data.attachmentName, clone);
			}
			skeleton.SetAttachment(data.slot, data.attachmentName);
		}

		// Token: 0x06004506 RID: 17670 RVA: 0x0003A717 File Offset: 0x00038917
		public void OnSetHair(Color c)
		{
			this.workerSpriteData.HairColor = c;
			this.hairColor = c;
			this.BasicApply();
		}

		// Token: 0x06004507 RID: 17671 RVA: 0x001A8914 File Offset: 0x001A6B14
		public void OnSetEyeColor(Color c)
		{
			this.eyeColor = c;
			this.EyeRenderer.color = (this.workerSpriteData.EyeColor = this.eyeColor);
		}

		// Token: 0x06004508 RID: 17672 RVA: 0x001A8948 File Offset: 0x001A6B48
		public void DisableSeparator()
		{
			this.separator.enabled = false;
			this.MouthRenderer.gameObject.SetActive(false);
			this.MouthReplaceGiftRender.gameObject.SetActive(false);
			this.EyeRenderer.gameObject.SetActive(false);
			this.EyebrowRenderer.gameObject.SetActive(false);
			this.MouthReplaceGiftRender.gameObject.SetActive(false);
			this.IgnoreSpriteRenderer();
		}

		// Token: 0x06004509 RID: 17673 RVA: 0x0003A732 File Offset: 0x00038932
		private void ReplaceGiftMouth(Sprite sprite)
		{
			this.MouthReplaceGiftRender.enabled = true;
			this.MouthRenderer.enabled = false;
			this.MouthReplaceGiftRender.sprite = sprite;
		}

		// Token: 0x0600450A RID: 17674 RVA: 0x001A89BC File Offset: 0x001A6BBC
		public void SetFaceEnable(bool state)
		{
			this.EyeRenderer.enabled = state;
			this.MouthRenderer.gameObject.SetActive(state);
			this.EyebrowRenderer.gameObject.SetActive(state);
			this.MouthReplaceGiftRender.gameObject.SetActive(state);
		}

		// Token: 0x0600450B RID: 17675 RVA: 0x001A8A08 File Offset: 0x001A6C08
		public void DisableSeparatorForUnique()
		{
			this.separator.enabled = false;
			this.MouthRenderer.gameObject.SetActive(false);
			this.EyeRenderer.gameObject.SetActive(false);
			this.EyebrowRenderer.gameObject.SetActive(false);
			this.MouthReplaceGiftRender.gameObject.SetActive(false);
		}

		// Token: 0x0600450C RID: 17676 RVA: 0x001A8A68 File Offset: 0x001A6C68
		public void DisableSeparartor(WorkerFaceType type)
		{
			this.separator.enabled = false;
			this.MouthRenderer.gameObject.SetActive(false);
			this.EyeRenderer.gameObject.SetActive(false);
			this.EyebrowRenderer.gameObject.SetActive(false);
			this.IgnoreSpriteRenderer(type);
			this.MouthReplaceGiftRender.gameObject.SetActive(false);
		}

		// Token: 0x0600450D RID: 17677 RVA: 0x001A8ACC File Offset: 0x001A6CCC
		public void EnableSeparator()
		{
			this.separator.enabled = true;
			this.MouthRenderer.gameObject.SetActive(true);
			this.MouthReplaceGiftRender.gameObject.SetActive(true);
			this.EyeRenderer.gameObject.SetActive(true);
			this.EyebrowRenderer.gameObject.SetActive(true);
			this.SetRegionAsTransparent("L Weapon", "Note");
			this.BaiscRendererInit();
		}

		// Token: 0x0600450E RID: 17678 RVA: 0x0003A758 File Offset: 0x00038958
		public void BaiscRendererInit()
		{
			this.SetRegionAsTransparent(WorkerBasicRegionKey.Mouth, WorkerBasicRegionKey.Mouth);
			this.SetRegionAsTransparent(WorkerBasicRegionKey.Eye, WorkerBasicRegionKey.Eye);
			this.SetRegionAsTransparent(WorkerBasicRegionKey.EyeBrow, WorkerBasicRegionKey.EyeBrow);
		}

		// Token: 0x0600450F RID: 17679 RVA: 0x001A8B40 File Offset: 0x001A6D40
		private void IgnoreSpriteRenderer(WorkerFaceType type)
		{
			List<SpineChangeData> list = new List<SpineChangeData>();
			SpineChangeData spineChangeData = new SpineChangeData();
			SpineChangeData spineChangeData2 = new SpineChangeData();
			SpineChangeData spineChangeData3 = new SpineChangeData();
			this.SetWorkerFaceType(type);
			spineChangeData.sprite = this.currentSpriteSet.Mouth;
			spineChangeData2.sprite = this.currentSpriteSet.Eye;
			spineChangeData3.sprite = this.currentSpriteSet.Eyebrow;
			spineChangeData.slot = (spineChangeData.attachmentName = WorkerBasicRegionKey.Mouth);
			spineChangeData2.slot = (spineChangeData2.attachmentName = WorkerBasicRegionKey.Eye);
			spineChangeData3.slot = (spineChangeData3.attachmentName = WorkerBasicRegionKey.EyeBrow);
			spineChangeData2.isSettedColor = true;
			spineChangeData2.regionColor = this.workerSpriteData.EyeColor;
			list.Add(spineChangeData);
			list.Add(spineChangeData2);
			list.Add(spineChangeData3);
			this.Apply(list);
		}

		// Token: 0x06004510 RID: 17680 RVA: 0x001A8C18 File Offset: 0x001A6E18
		private void IgnoreSpriteRenderer()
		{
			List<SpineChangeData> list = new List<SpineChangeData>();
			SpineChangeData spineChangeData = new SpineChangeData();
			SpineChangeData spineChangeData2 = new SpineChangeData();
			SpineChangeData spineChangeData3 = new SpineChangeData();
			spineChangeData.sprite = this.currentSpriteSet.Mouth;
			spineChangeData2.sprite = this.currentSpriteSet.Eye;
			spineChangeData3.sprite = this.currentSpriteSet.Eyebrow;
			spineChangeData.slot = (spineChangeData.attachmentName = WorkerBasicRegionKey.Mouth);
			spineChangeData2.slot = (spineChangeData2.attachmentName = WorkerBasicRegionKey.Eye);
			spineChangeData3.slot = (spineChangeData3.attachmentName = WorkerBasicRegionKey.EyeBrow);
			spineChangeData2.isSettedColor = true;
			spineChangeData2.regionColor = this.workerSpriteData.EyeColor;
			list.Add(spineChangeData);
			list.Add(spineChangeData2);
			list.Add(spineChangeData3);
			this.Apply(list);
		}

		// Token: 0x06004511 RID: 17681 RVA: 0x001A8CE8 File Offset: 0x001A6EE8
		private void ExtractWeaponRegionData()
		{
			try
			{
				WeaponClassType type = this.currentWeaponType;
				string empty = string.Empty;
				string empty2 = string.Empty;
				if (WeaponRegionKey.GetKey(type, out empty, out empty2))
				{
					int slotIndex = this.skeleton.FindSlotIndex(empty);
					RegionAttachment regionAttachment = this.skeleton.GetAttachment(slotIndex, empty2) as RegionAttachment;
					this._weaponPosition = new Vector2(regionAttachment.X, regionAttachment.Y);
					this._weaponRotation = regionAttachment.Rotation;
					this._initWeaponData = true;
				}
			}
			catch (Exception message)
			{
				Debug.LogError(message);
				this._initWeaponData = false;
			}
		}

		// Token: 0x06004512 RID: 17682 RVA: 0x0003A78A File Offset: 0x0003898A
		public void SetWorkNoteSprite(Sprite s)
		{
			if (this.NoteRenderer != null)
			{
				this.NoteRenderer.sprite = s;
			}
		}

		// Token: 0x06004513 RID: 17683 RVA: 0x001A8D7C File Offset: 0x001A6F7C
		public void StageStart()
		{
			if (this.Model is AgentModel && this.Model.currentSefira != "0")
			{
				this.SetSefira(SefiraManager.instance.GetSefira(this.Model.currentSefira).sefiraEnum, 1);
			}
		}

		// Token: 0x06004514 RID: 17684 RVA: 0x001A8DD0 File Offset: 0x001A6FD0
		public void SetHeadColor(Color c)
		{
			List<SpineChangeData> list = new List<SpineChangeData>();
			if (this.SetHeadSprite)
			{
				SpineChangeData spineChangeData = new SpineChangeData
				{
					sprite = this.HeadSprite
				};
				spineChangeData.slot = (spineChangeData.attachmentName = "Head");
				spineChangeData.isSettedColor = true;
				spineChangeData.regionColor = c;
				list.Add(spineChangeData);
			}
			this.Apply(list);
		}

		// Token: 0x06004515 RID: 17685 RVA: 0x0003A7A6 File Offset: 0x000389A6
		public void SetLayer(int layerId, int order)
		{
			this.SetLayer(base.transform, layerId, order);
		}

		// Token: 0x06004516 RID: 17686 RVA: 0x001A8E30 File Offset: 0x001A7030
		private void SetLayer(Transform tr, int layerId, int order)
		{
			IEnumerator enumerator = tr.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform tr2 = (Transform)obj;
					this.SetLayer(tr2, layerId, order);
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
			SpriteRenderer component = tr.gameObject.GetComponent<SpriteRenderer>();
			if (component != null)
			{
				component.sortingLayerID = layerId;
				component.sortingOrder = order;
				return;
			}
			SkeletonPartsRenderer component2 = tr.gameObject.GetComponent<SkeletonPartsRenderer>();
			if (component2 != null)
			{
				component2.MeshRenderer.sortingLayerID = layerId;
				component2.MeshRenderer.sortingOrder = order;
			}
		}

		// <Patch>
		public void ArmorEquip_Mod(LobotomyBaseMod.LcId armorId)
		{
			this.armorId = armorId.id;
			WorkerSpriteManager.instance.GetArmorData_Mod(armorId, ref this.Model.spriteData);
			this.UpdateArmorSpriteSet();
			this.ArmorApply();
		}

		// Token: 0x04003FCF RID: 16335
		private const string HeadRegion = "Head";

		// Token: 0x04003FD0 RID: 16336
		private const string AddObjectSrc = "Slot/WorkerAttachment";

		// Token: 0x04003FD1 RID: 16337
		private WorkerModel _model;

		// Token: 0x04003FD2 RID: 16338
		[Header("EGO_Gift")]
		public Transform[] GiftPos;

		// Token: 0x04003FD3 RID: 16339
		[Header("Data")]
		public WorkerCurrentSpriteSet currentSpriteSet;

		// Token: 0x04003FD4 RID: 16340
		private Dictionary<int, EGOGiftRenderData> attachGiftData = new Dictionary<int, EGOGiftRenderData>();

		// Token: 0x04003FD5 RID: 16341
		private Dictionary<EGOgiftAttachRegion, EGOGiftRenderData> replaceGiftData = new Dictionary<EGOgiftAttachRegion, EGOGiftRenderData>();

		// Token: 0x04003FD6 RID: 16342
		private SkeletonRenderSeparator separator;

		// Token: 0x04003FD7 RID: 16343
		private WeaponClassType currentWeaponType = WeaponClassType.AXE;

		// Token: 0x04003FD8 RID: 16344
		public WorkerFaceType faceType;

		// Token: 0x04003FD9 RID: 16345
		public Color hairColor;

		// Token: 0x04003FDA RID: 16346
		public Color eyeColor;

		// Token: 0x04003FDB RID: 16347
		[Header("Renderer")]
		public SpriteRenderer EyeRenderer;

		// Token: 0x04003FDC RID: 16348
		public SpriteRenderer EyebrowRenderer;

		// Token: 0x04003FDD RID: 16349
		public SpriteRenderer MouthRenderer;

		// Token: 0x04003FDE RID: 16350
		public SpriteRenderer SymbolRenderer;

		// Token: 0x04003FDF RID: 16351
		public SpriteRenderer MouthReplaceGiftRender;

		// Token: 0x04003FE0 RID: 16352
		[Header("Renderer")]
		public SpriteRenderer WeaponRenderer;

		// Token: 0x04003FE1 RID: 16353
		public bool SetHeadSprite;

		// Token: 0x04003FE2 RID: 16354
		public Sprite HeadSprite;

		// Token: 0x04003FE3 RID: 16355
		public Sprite TransparentSprite;

		// Token: 0x04003FE4 RID: 16356
		public bool repack = true;

		// Token: 0x04003FE5 RID: 16357
		public Shader repackedShader;

		// Token: 0x04003FE6 RID: 16358
		public SpriteRenderer NoteRenderer;

		// Token: 0x04003FE7 RID: 16359
		[Header("Not Assigned")]
		public Texture2D runtimeAtlas;

		// Token: 0x04003FE8 RID: 16360
		public Material runtimeMaterial;

		// Token: 0x04003FE9 RID: 16361
		public Skin currentSkin;

		// Token: 0x04003FEA RID: 16362
		[Header("Panic Related")]
		public SpriteRenderer panicRenderer;

		// Token: 0x04003FEB RID: 16363
		private Vector2 _weaponPosition = Vector2.zero;

		// Token: 0x04003FEC RID: 16364
		private float _weaponRotation;

		// Token: 0x04003FED RID: 16365
		private bool _initWeaponData;

		// Token: 0x04003FEE RID: 16366
		public bool debugCheck;

		// Token: 0x04003FEF RID: 16367
		public int armorId = 1;

		// Token: 0x04003FF0 RID: 16368
		private bool _armorColored;

		// Token: 0x04003FF1 RID: 16369
		private List<EGOgiftModel> currentGift = new List<EGOgiftModel>();
	}
}
