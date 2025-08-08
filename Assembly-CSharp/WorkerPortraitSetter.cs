using System;
using System.Collections.Generic;
using Customizing;
using UnityEngine;
using UnityEngine.UI;
using WorkerSprite;

// Token: 0x02000A51 RID: 2641
[RequireComponent(typeof(CanvasGroup))]
public class WorkerPortraitSetter : MonoBehaviour
{
	// Token: 0x1700077C RID: 1916
	// (get) Token: 0x0600501E RID: 20510 RVA: 0x00041E3A File Offset: 0x0004003A
	public CanvasGroup CanvasGroup
	{
		get
		{
			return base.GetComponent<CanvasGroup>();
		}
	}

	// Token: 0x0600501F RID: 20511 RVA: 0x001D912C File Offset: 0x001D732C
	private void Start()
	{ // <Patch>
        if (this.attachmentsMod == null)
        {
            this.attachmentsMod = new Dictionary<LobotomyBaseMod.LcId, WorkerPortraitAttachment>();
        }
		this.Head.material = null;
		this.Eye.material = null;
		this.Mouth.material = null;
		this.Eyebrow.material = null;
		this.FrontHair.material = null;
		this.RearHair.material = null;
		this.CoatBack.material = null;
		this.CoatRight.material = null;
		this.CoatLeft.material = null;
		this.LeftUpLeg.material = null;
		this.LeftDownLeg.material = null;
		this.RightUpLeg.material = null;
		this.RightDownLeg.material = null;
		this.LeftUpArm.material = null;
		this.LeftDownArm.material = null;
		this.LeftHand.material = null;
		this.Body.material = null;
		this.RightUpArm.material = null;
		this.RightDownArm.material = null;
		this.RightHand.material = null;
		this.Symbol.material = null;
		this.SetWeapon(null);
		if (this._armor == null && this.model == null)
		{
			this.SetAgentArmor();
			this.Eye.gameObject.SetActive(true);
			this.Eyebrow.gameObject.SetActive(true);
			this.Mouth.gameObject.SetActive(true);
			this.FrontHair.gameObject.SetActive(true);
			this.RearHair.gameObject.SetActive(true);
			this.Symbol.gameObject.SetActive(true);
		}
	}

	// Token: 0x06005020 RID: 20512 RVA: 0x0003BD7D File Offset: 0x00039F7D
	public void SetActive(bool state)
	{
		base.gameObject.SetActive(state);
	}

	// Token: 0x06005021 RID: 20513 RVA: 0x001D92C0 File Offset: 0x001D74C0
	public void SetArmor(EquipmentTypeInfo armor)
	{ // <Patch>
		LobotomyBaseMod.LcId lcId = EquipmentTypeInfo.GetLcId(armor);
		int armorId = armor.armorId;
		if (this._armor != null && armor.id == this._armor.id)
		{
			return;
		}
		this._armor = armor;
		WorkerSprite.WorkerSprite workerSprite = new WorkerSprite.WorkerSprite();
		WorkerSpriteManager.instance.GetArmorData_Mod(new LobotomyBaseMod.LcId(lcId.packageId, armorId), ref workerSprite);
		this.Eye.gameObject.SetActive(false);
		this.Eyebrow.gameObject.SetActive(false);
		this.Mouth.gameObject.SetActive(false);
		this.FrontHair.gameObject.SetActive(false);
		this.RearHair.gameObject.SetActive(false);
		this.Symbol.gameObject.SetActive(false);
		this.SetSprite(this.Body, workerSprite.Armor.Body);
		this.SetSprite(this.LeftUpArm, workerSprite.Armor.Arm_Left_Up);
		this.SetSprite(this.LeftDownArm, workerSprite.Armor.Arm_Left_Down);
		this.SetSprite(this.RightUpArm, workerSprite.Armor.Arm_Right_Up);
		this.SetSprite(this.RightDownArm, workerSprite.Armor.Arm_Right_Down);
		this.SetSprite(this.LeftUpLeg, workerSprite.Armor.Leg_Left_Up);
		this.SetSprite(this.LeftDownLeg, workerSprite.Armor.Leg_Left_Down);
		this.SetSprite(this.RightUpLeg, workerSprite.Armor.Leg_Right_Up);
		this.SetSprite(this.RightDownLeg, workerSprite.Armor.Leg_Right_Down);
		this.SetSprite(this.LeftHand, workerSprite.Armor.Left_Hand);
		this.SetSprite(this.RightHand, workerSprite.Armor.Right_Hand);
		this.SetSprite(this.CoatBack, workerSprite.Armor.Coat_Back);
		this.SetSprite(this.CoatLeft, workerSprite.Armor.Coat_Right);
		this.SetSprite(this.CoatRight, workerSprite.Armor.Coat_Left);
		this.SetWeapon(null);
		this.OnSetAmror(armor);
	}

	// Token: 0x06005022 RID: 20514 RVA: 0x001D94C8 File Offset: 0x001D76C8
	private void CheckGifts()
	{ // <Patch>
		List<EGOgiftModel> addedGifts = this.model.Equipment.gifts.addedGifts;
		List<EGOgiftModel> replacedGifts = this.model.Equipment.gifts.replacedGifts;
		List<EGOgiftModel> list = new List<EGOgiftModel>();
		list.AddRange(addedGifts);
		list.AddRange(replacedGifts);
		List<EGOgiftModel> list2 = new List<EGOgiftModel>();
		List<EGOgiftModel> list3 = new List<EGOgiftModel>();
        foreach (KeyValuePair<LobotomyBaseMod.LcId, WorkerPortraitAttachment> keyValuePair in this.attachmentsMod)
        {
			EGOgiftModel egogiftModel = keyValuePair.Value.gift as EGOgiftModel;
			if (egogiftModel != null)
			{
				if (!list.Contains(egogiftModel))
				{
					list3.Add(egogiftModel);
				}
				else if (!this.model.Equipment.gifts.GetDisplayState(egogiftModel))
				{
					this.model.Equipment.gifts.GetDisplayState(egogiftModel);
					list3.Add(egogiftModel);
				}
			}
		}
		foreach (EGOgiftModel egogiftModel2 in list)
		{
			bool flag = this.ContainsGift(egogiftModel2);
			if (!flag && this.model.Equipment.gifts.GetDisplayState(egogiftModel2))
			{
				list2.Add(egogiftModel2);
			}
			if (flag && egogiftModel2.metaInfo.attachType == EGOgiftAttachType.REPLACE)
			{
				if (egogiftModel2.metaInfo.AttachRegion == EGOgiftAttachRegion.MOUTH)
				{
					this._mouthReplace = true;
				}
				if (egogiftModel2.metaInfo.AttachRegion == EGOgiftAttachRegion.RIGHTHAND)
				{
					this._handReplace = true;
				}
			}
		}
		foreach (EGOgiftModel egogiftModel3 in list3)
		{
			this.RemoveDisabled(egogiftModel3);
			if (egogiftModel3.metaInfo.attachType == EGOgiftAttachType.REPLACE)
			{
				if (egogiftModel3.metaInfo.AttachRegion == EGOgiftAttachRegion.MOUTH)
				{
					this._mouthReplace = false;
				}
				if (egogiftModel3.metaInfo.AttachRegion == EGOgiftAttachRegion.RIGHTHAND)
				{
					this._handReplace = false;
				}
			}
		}
		foreach (EGOgiftModel egogiftModel4 in list2)
		{
			this.AddNewAttach(egogiftModel4);
		}
		if (this._mouthReplace)
		{
			this.Mouth.color = WorkerPortraitSetter.transparentColor;
		}
		else
		{
			this.Mouth.color = Color.white;
		}
		if (this._handReplace)
		{
			this.RightHand.color = WorkerPortraitSetter.transparentColor;
		}
		else
		{
			this.RightHand.color = Color.white;
		}
	}

	// Token: 0x06005023 RID: 20515 RVA: 0x00041E42 File Offset: 0x00040042
	private bool ContainsGift(EGOgiftModel model)
	{ // <Patch>
        LobotomyBaseMod.LcId lcId = EquipmentTypeInfo.GetLcId(model.metaInfo);
        return this.attachmentsMod.ContainsKey(lcId);
		// return this.attachments.ContainsKey((long)model.metaInfo.id);
	}

	// Token: 0x06005024 RID: 20516 RVA: 0x001D97E0 File Offset: 0x001D79E0
	private void InitGifts(WorkerModel worker)
	{
		List<EGOgiftModel> addedGifts = worker.Equipment.gifts.addedGifts;
		List<EGOgiftModel> replacedGifts = worker.Equipment.gifts.replacedGifts;
		this.attachedGifts.Clear();
		this.attachedGifts.AddRange(addedGifts);
		this.attachedGifts.AddRange(replacedGifts);
		this.DestroyGifts();
		foreach (EGOgiftModel egogiftModel in this.attachedGifts)
		{
			if (worker.Equipment.gifts.GetDisplayState(egogiftModel))
			{
				if (egogiftModel.metaInfo.attachType == EGOgiftAttachType.REPLACE)
				{
					if (egogiftModel.metaInfo.attachPos == "mouth")
					{
						this._mouthReplace = true;
					}
					if (egogiftModel.metaInfo.attachPos == "right_hand")
					{
						this._handReplace = true;
					}
				}
				this.AddNewAttach(egogiftModel);
			}
		}
		if (this._mouthReplace)
		{
			if (this.log)
			{
				Debug.Log("SetTransparent");
			}
			this.Mouth.color = WorkerPortraitSetter.transparentColor;
		}
		else
		{
			this.Mouth.color = Color.white;
		}
		if (this._handReplace)
		{
			this.RightHand.color = WorkerPortraitSetter.transparentColor;
		}
		else
		{
			this.RightHand.color = Color.white;
		}
	}

	// Token: 0x06005025 RID: 20517 RVA: 0x001D996C File Offset: 0x001D7B6C
	private void DestroyGifts()
	{ // <Patch>
        if (this.attachmentsMod == null)
        {
            this.attachmentsMod = new Dictionary<LobotomyBaseMod.LcId, WorkerPortraitAttachment>();
            return;
        }
        foreach (KeyValuePair<LobotomyBaseMod.LcId, WorkerPortraitAttachment> keyValuePair in this.attachmentsMod)
        {
            UnityEngine.Object.Destroy(keyValuePair.Value.gameObject);
        }
        this.attachmentsMod.Clear();
	}

	// Token: 0x06005026 RID: 20518 RVA: 0x001D99E0 File Offset: 0x001D7BE0
	private void OnSetAmror(EquipmentTypeInfo model)
	{ // <Patch>
        if (this.attachmentsMod == null)
        {
            this.attachmentsMod = new Dictionary<LobotomyBaseMod.LcId, WorkerPortraitAttachment>();
        }
		LobotomyBaseMod.LcId lcId = EquipmentTypeInfo.GetLcId(model);
		if (model.script == "MagicalGirlArmor")
		{
            foreach (KeyValuePair<LobotomyBaseMod.LcId, WorkerPortraitAttachment> keyValuePair in this.attachmentsMod)
            {
				if (keyValuePair.Value.isUnique)
				{
					return;
				}
			}
			GameObject gameObject = Prefab.LoadPrefab("UIComponent/WorkerPortraitAttachment");
			WorkerPortraitAttachment component = gameObject.GetComponent<WorkerPortraitAttachment>();
			component.SetMagicalGirlArmor();
			this.attachmentsMod.Add(lcId, component);
			component.RectTransform.SetParent(base.transform);
			component.RectTransform.localScale = Vector3.one;
			component.RectTransform.localPosition = Vector3.zero;
			component.Image.SetNativeSize();
			component.RectTransform.anchoredPosition = new Vector2(2.7f, -29.3f);
		}
		else
		{
            foreach (KeyValuePair<LobotomyBaseMod.LcId, WorkerPortraitAttachment> keyValuePair2 in this.attachmentsMod)
            {
				if (keyValuePair2.Value.isUnique)
				{
					UnityEngine.Object.Destroy(keyValuePair2.Value.gameObject);
					this.attachmentsMod.Remove(keyValuePair2.Key);
					break;
				}
			}
		}
	}

	// Token: 0x06005027 RID: 20519 RVA: 0x001D9B68 File Offset: 0x001D7D68
	private void RemoveDisabled(EGOgiftModel model)
	{ // <Patch>
        LobotomyBaseMod.LcId lcId = EquipmentTypeInfo.GetLcId(model.metaInfo);
        if (this.attachmentsMod.ContainsKey(lcId))
        {
            WorkerPortraitAttachment workerPortraitAttachment = this.attachmentsMod[lcId];
            UnityEngine.Object.Destroy(workerPortraitAttachment.gameObject);
            this.attachmentsMod.Remove(lcId);
        }
        /*
		if (this.attachments.ContainsKey((long)model.metaInfo.id))
		{
			WorkerPortraitAttachment workerPortraitAttachment = this.attachments[(long)model.metaInfo.id];
			UnityEngine.Object.Destroy(workerPortraitAttachment.gameObject);
			this.attachments.Remove((long)model.metaInfo.id);
		}*/
	}

	// Token: 0x06005028 RID: 20520 RVA: 0x001D9BCC File Offset: 0x001D7DCC
	private void AddNewAttach(EGOgiftModel model)
	{ // <Patch> <Mod> Fixed Modded Mouth 1 gifts
		LobotomyBaseMod.LcId lcId = EquipmentTypeInfo.GetLcId(model.metaInfo);
		GameObject gameObject = Prefab.LoadPrefab("UIComponent/WorkerPortraitAttachment");
		WorkerPortraitAttachment component = gameObject.GetComponent<WorkerPortraitAttachment>();
		component.SetGift(model);
		this.attachmentsMod.Add(lcId, component);
		component.RectTransform.SetParent(base.transform);
		component.RectTransform.localScale = Vector3.one;
		component.RectTransform.localPosition = Vector3.zero;
		component.Image.SetNativeSize();
		EGOgiftAttachRegion region = component.region;
		bool flag = model.metaInfo.attachType == EGOgiftAttachType.REPLACE;
		switch (region)
		{
		case EGOgiftAttachRegion.HEAD:
			component.RectTransform.anchoredPosition = WorkerPortraitSetter.PositionFix_Head;
			break;
		case EGOgiftAttachRegion.EYE:
			component.RectTransform.anchoredPosition = WorkerPortraitSetter.PositionFix_Eye;
			component.RectTransform.SetSiblingIndex(this.Mouth.transform.GetSiblingIndex() + 1);
			if (component.Image.sprite.name.Contains("Mask"))
			{
				float x = -15.71f;
				if (lcId == 400052)
				{
					x = -28.4f;
				}
				component.RectTransform.anchoredPosition = new Vector2(x, WorkerPortraitSetter.PositionFix_Eye.y);
			}
			break;
		case EGOgiftAttachRegion.MOUTH:
			component.RectTransform.anchoredPosition = this.Mouth.rectTransform.anchoredPosition;
			if (lcId == 400032 || lcId == 400018 || !flag)
			{
				component.RectTransform.anchoredPosition = new Vector2(-13f, 9f);
				component.RectTransform.SetSiblingIndex(this.FrontHair.transform.GetSiblingIndex() + 2);
			}
			else
			{
				component.RectTransform.anchoredPosition = new Vector2(-34f, 0f);
				component.RectTransform.SetSiblingIndex(this.FrontHair.transform.GetSiblingIndex() + 1);
			}
			break;
		case EGOgiftAttachRegion.HAIR:
			component.RectTransform.SetAsLastSibling();
			component.RectTransform.anchoredPosition = new Vector2(16f, 108f);
			component.RectTransform.localScale = Vector3.one * 1.2f;
			break;
		case EGOgiftAttachRegion.RIGHTHAND:
			component.RectTransform.SetParent(this.RightHand.transform.parent);
			component.RectTransform.SetSiblingIndex(this.RightHand.transform.GetSiblingIndex() + 1);
			component.RectTransform.anchoredPosition = new Vector2(16.5f, -91.1f);
			break;
		case EGOgiftAttachRegion.BODY_UP:
			component.RectTransform.anchoredPosition = new Vector2(-6.5f, -49.7f);
			break;
		case EGOgiftAttachRegion.RIBBORN:
			component.RectTransform.anchoredPosition = new Vector2(-6.3f, -27.8f);
			component.RectTransform.localScale = Vector3.one;
			component.RectTransform.SetAsLastSibling();
			break;
		case EGOgiftAttachRegion.RIGHTCHEEK:
		{
			Vector2 anchoredPosition = new Vector2(36.6f, 24.3f);
			component.RectTransform.anchoredPosition = anchoredPosition;
			break;
		}
		case EGOgiftAttachRegion.FACE:
			component.RectTransform.anchoredPosition = this.Head.rectTransform.anchoredPosition;
			component.RectTransform.SetSiblingIndex(this.Mouth.rectTransform.GetSiblingIndex() + 1);
			break;
		case EGOgiftAttachRegion.BACK:
			component.RectTransform.SetSiblingIndex(this.RearHair.rectTransform.GetSiblingIndex() + 1);
			if (lcId == 400043)
			{
				component.RectTransform.anchoredPosition = new Vector2(17f, -94.8f);
			}
			else
			{
				component.RectTransform.anchoredPosition = new Vector2(104.2f, -94.8f);
			}
			break;
		case EGOgiftAttachRegion.HEADBACK:
			component.RectTransform.anchoredPosition = WorkerPortraitSetter.PositionFix_HeadBack;
			component.RectTransform.localScale = Vector3.one * 0.65f;
			component.RectTransform.SetAsFirstSibling();
			break;
		case EGOgiftAttachRegion.BACK2:
			component.RectTransform.SetSiblingIndex(this.RearHair.rectTransform.GetSiblingIndex() + 1);
			component.RectTransform.anchoredPosition = new Vector2(-74.5f, -94.8f);
			break;
		case EGOgiftAttachRegion.LEFTHAND:
			component.RectTransform.anchoredPosition = this.LeftHand.rectTransform.anchoredPosition;
			break;
		}
	}

	// Token: 0x06005029 RID: 20521 RVA: 0x001DA074 File Offset: 0x001D8274
	public void SetWorker(WorkerModel worker)
	{
		if (worker == null)
		{
			this._mouthReplace = false;
			this._handReplace = false;
			this.SetActive(false);
			return;
		}
		this.SetActive(true);
		if (this.model != worker)
		{
			this._mouthReplace = false;
			this._handReplace = false;
			this.InitGifts(worker);
		}
		else
		{
			this.CheckGifts();
		}
		this.Eye.gameObject.SetActive(true);
		this.Eyebrow.gameObject.SetActive(true);
		this.Mouth.gameObject.SetActive(true);
		this.FrontHair.gameObject.SetActive(true);
		this.RearHair.gameObject.SetActive(true);
		this.Symbol.gameObject.SetActive(true);
		this.model = worker;
		WorkerSprite.WorkerSprite spriteData = worker.spriteData;
		this.Eye.sprite = spriteData.Eye;
		this.SetSprite(this.Eyebrow, spriteData.EyeBrow);
		this.SetSprite(this.Eye, spriteData.Eye);
		this.SetSprite(this.Mouth, spriteData.Mouth);
		this.SetSprite(this.FrontHair, spriteData.FrontHair);
		this.SetSprite(this.RearHair, spriteData.RearHair);
		this.Eye.color = spriteData.EyeColor;
		Graphic frontHair = this.FrontHair;
		Color hairColor = spriteData.HairColor;
		this.RearHair.color = hairColor;
		frontHair.color = hairColor;
		this.SetSprite(this.Body, spriteData.Armor.Body);
		this.SetSprite(this.LeftUpArm, spriteData.Armor.Arm_Left_Up);
		this.SetSprite(this.LeftDownArm, spriteData.Armor.Arm_Left_Down);
		this.SetSprite(this.RightUpArm, spriteData.Armor.Arm_Right_Up);
		this.SetSprite(this.RightDownArm, spriteData.Armor.Arm_Right_Down);
		this.SetSprite(this.LeftUpLeg, spriteData.Armor.Leg_Left_Up);
		this.SetSprite(this.LeftDownLeg, spriteData.Armor.Leg_Left_Down);
		this.SetSprite(this.RightUpLeg, spriteData.Armor.Leg_Right_Up);
		this.SetSprite(this.RightDownLeg, spriteData.Armor.Leg_Right_Down);
		this.SetSprite(this.LeftHand, spriteData.Armor.Left_Hand);
		this.SetSprite(this.RightHand, spriteData.Armor.Right_Hand);
		this.SetSprite(this.CoatBack, spriteData.Armor.Coat_Back);
		this.SetSprite(this.CoatLeft, spriteData.Armor.Coat_Right);
		this.SetSprite(this.CoatRight, spriteData.Armor.Coat_Left);
		if (this.WeaponSet && this.model is AgentModel)
		{
			this.SetWeapon(this.model.Equipment.weapon);
		}
		else
		{
			this.SetWeapon(null);
		}
		try
		{
			this.OnSetAmror(this.model.Equipment.armor.metaInfo);
		}
		catch (Exception ex)
		{
		}
		if (worker is AgentModel)
		{
			Sefira currentSefira = (worker as AgentModel).GetCurrentSefira();
			if (currentSefira == null)
			{
				spriteData.Symbol = null;
				this.Symbol.gameObject.SetActive(false);
				return;
			}
			spriteData.Symbol = WorkerSpriteManager.instance.GetSefiraSymbol(currentSefira.sefiraEnum, (worker as AgentModel).GetContinuousServiceLevel());
			this.SetSprite(this.Symbol, spriteData.Symbol);
			this.Symbol.sprite = spriteData.Symbol;
			this.Symbol.gameObject.SetActive(true);
			this.Symbol.enabled = true;
		}
	}

	// Token: 0x0600502A RID: 20522 RVA: 0x00041E5B File Offset: 0x0004005B
	public void SetSprite(Image region, Sprite sprite)
	{
		if (sprite == null)
		{
			region.enabled = false;
		}
		else
		{
			region.enabled = true;
			region.sprite = sprite;
		}
	}

	// Token: 0x0600502B RID: 20523 RVA: 0x00041E83 File Offset: 0x00040083
	private void LateUpdate()
	{
		if (this.model == null)
		{
			return;
		}
		this.SetWorker(this.model);
	}

	// Token: 0x0600502C RID: 20524 RVA: 0x001DA42C File Offset: 0x001D862C
	public void SetAgentArmor()
	{
		EquipmentTypeInfo data = EquipmentTypeList.instance.GetData(22);
		this.SetArmor(data);
	}

	// Token: 0x0600502D RID: 20525 RVA: 0x001DA450 File Offset: 0x001D8650
	public void SetCustomizing(AgentData data, WorkerFaceType face = WorkerFaceType.DEFAULT)
	{
		this.model = null;
		this.DestroyGifts();
		switch (face)
		{
		case WorkerFaceType.DEFAULT:
			this.SetSprite(this.Eyebrow, data.appearance.Eyebrow_Def);
			this.SetSprite(this.Eye, data.appearance.Eye_Def);
			this.SetSprite(this.Mouth, data.appearance.Mouth_Def);
			break;
		case WorkerFaceType.BATTLE:
			this.SetSprite(this.Eyebrow, data.appearance.Eyebrow_Battle);
			this.SetSprite(this.Eye, data.appearance.Eye_Def);
			this.SetSprite(this.Mouth, data.appearance.Mouth_Battle);
			break;
		case WorkerFaceType.PANIC:
			this.SetSprite(this.Eyebrow, data.appearance.Eyebrow_Panic);
			this.SetSprite(this.Eye, data.appearance.Eye_Panic);
			this.SetSprite(this.Mouth, data.appearance.Mouth_Battle);
			break;
		case WorkerFaceType.DEAD:
			this.SetSprite(this.Eyebrow, data.appearance.Eyebrow_Def);
			this.SetSprite(this.Eye, data.appearance.Eye_Dead);
			this.SetSprite(this.Mouth, data.appearance.Mouth_Def);
			break;
		}
		this.SetSprite(this.FrontHair, data.appearance.FrontHair);
		this.SetSprite(this.RearHair, data.appearance.RearHair);
		this.Eye.color = Color.white;
		Graphic frontHair = this.FrontHair;
		Color hairColor = data.appearance.HairColor;
		this.RearHair.color = hairColor;
		frontHair.color = hairColor;
		this.Symbol.enabled = false;
		this.SetWeapon(null);
		this.SetAgentArmor();
		this.Eye.gameObject.SetActive(true);
		this.Eyebrow.gameObject.SetActive(true);
		this.Mouth.gameObject.SetActive(true);
		this.FrontHair.gameObject.SetActive(true);
		this.RearHair.gameObject.SetActive(true);
		this.Symbol.gameObject.SetActive(true);
	}

	// Token: 0x0600502E RID: 20526 RVA: 0x001DA690 File Offset: 0x001D8890
	public void SetWeapon(WeaponModel weapon)
	{ // <Patch> <Mod>
		if (!this.WeaponSet)
		{
			return;
		}
		if (weapon == null)
		{
			this.OneHandedRenderer.enabled = false;
			this.TwoHandedRenderer.enabled = false;
			this.FistRenderer.enabled = false;
			this.ClearAddedWeapon();
			return;
		}
		LobotomyBaseMod.KeyValuePairSS ss = new LobotomyBaseMod.KeyValuePairSS(EquipmentTypeInfo.GetLcId(weapon.metaInfo).packageId, weapon.metaInfo.sprite);
		WeaponClassType weaponClassType = weapon.metaInfo.weaponClassType;
		if (weaponClassType == WeaponClassType.SPECIAL)
		{
			UniqueWeaponSpriteUnit uniqueWeaponSpriteUnit = null;
			string sprite = weapon.metaInfo.sprite;
			if (WorkerSpriteManager.instance.GetUniqueWeaponSpriteInfo_Mod(ss, out uniqueWeaponSpriteUnit) && uniqueWeaponSpriteUnit != this._currentUnit)
			{
				this._currentUnit = uniqueWeaponSpriteUnit;
				this.FistRenderer.enabled = false;
				this.OneHandedRenderer.enabled = false;
				this.TwoHandedRenderer.enabled = false;
				this.SetUniqueWeapon(uniqueWeaponSpriteUnit);
			}
			return;
		}
		if (this._currentUnit != null)
		{
			this.ClearAddedWeapon();
			this._currentUnit = null;
		}
		if (weaponClassType == WeaponClassType.FIST)
		{
			if (ss.key != "")
			{
				Sprite[] fistSprite = WorkerSpriteManager.instance.GetFistSprite(ss);
				if (fistSprite[0] == null || fistSprite[1] == null)
				{
					return;
				}
				this.FistRenderer.sprite = fistSprite[1];
				this.OneHandedRenderer.enabled = false;
				this.TwoHandedRenderer.enabled = false;
				this.FistRenderer.enabled = true;
			}
			else
			{
				int id = (int)float.Parse(weapon.metaInfo.sprite);
				Sprite[] fistSprite = WorkerSprite_WorkerSpriteManager.instance.GetFistSprite(id);
				if (fistSprite[0] == null || fistSprite[1] == null)
				{
					return;
				}
				this.FistRenderer.sprite = fistSprite[1];
				this.OneHandedRenderer.enabled = false;
				this.TwoHandedRenderer.enabled = false;
				this.FistRenderer.enabled = true;
			}
		}
		else
		{
			bool flag = WeaponSetter.IsTwohanded(weapon);
			Sprite weaponSprite = WorkerSpriteManager.instance.GetWeaponSprite_Mod(weaponClassType, ss);
			if (flag)
			{
				this.TwoHandedRenderer.sprite = weaponSprite;
				this.OneHandedRenderer.enabled = false;
				this.TwoHandedRenderer.enabled = true;
				this.FistRenderer.enabled = false;
			}
			else
			{
				this.OneHandedRenderer.sprite = weaponSprite;
				this.OneHandedRenderer.enabled = true;
				this.TwoHandedRenderer.enabled = false;
				this.FistRenderer.enabled = false;
			}
		}
	}

	// Token: 0x0600502F RID: 20527 RVA: 0x001DA870 File Offset: 0x001D8A70
	private void ClearAddedWeapon()
	{
		if (this.weaponAdded.Count == 0)
		{
			return;
		}
		foreach (GameObject obj in this.weaponAdded)
		{
			UnityEngine.Object.Destroy(obj);
		}
		this.weaponAdded.Clear();
	}

	// Token: 0x06005030 RID: 20528 RVA: 0x001DA8E8 File Offset: 0x001D8AE8
	public void SetUniqueWeapon(UniqueWeaponSpriteUnit unit)
	{
		this.ClearAddedWeapon();
		foreach (UniqueWeaponSprite uniqueWeaponSprite in unit.sprites)
		{
			GameObject gameObject = Prefab.LoadPrefab("UIComponent/PortraitWeapon");
			Image component = gameObject.GetComponent<Image>();
			component.sprite = uniqueWeaponSprite.sprite;
			component.SetNativeSize();
			RectTransform rectTransform = component.rectTransform;
			RectTransform uniqueWeaponParent = this.GetUniqueWeaponParent(uniqueWeaponSprite.pos);
			if (uniqueWeaponSprite.pos == UniqueWeaponPos.LEFTHAND)
			{
				uniqueWeaponSprite.portraitInfo.childIndex = 1;
			}
			else if (uniqueWeaponSprite.pos == UniqueWeaponPos.RIGHTHAND)
			{
				uniqueWeaponSprite.portraitInfo.childIndex = 0;
			}
			rectTransform.SetParent(uniqueWeaponParent);
			rectTransform.anchoredPosition = uniqueWeaponSprite.portraitInfo.localPosition;
			rectTransform.localRotation = Quaternion.Euler(uniqueWeaponSprite.portraitInfo.localRotation);
			rectTransform.localScale = uniqueWeaponSprite.portraitInfo.localScale;
			if (uniqueWeaponSprite.portraitInfo.childIndex != -1)
			{
				rectTransform.SetSiblingIndex(uniqueWeaponSprite.portraitInfo.childIndex);
			}
			this.weaponAdded.Add(gameObject);
		}
	}

	// Token: 0x06005031 RID: 20529 RVA: 0x001DAA38 File Offset: 0x001D8C38
	private RectTransform GetUniqueWeaponParent(UniqueWeaponPos pos)
	{
		RectTransform result;
		try
		{
			switch (pos)
			{
			case UniqueWeaponPos.LEFTWAIST:
				result = this.OneHandedRenderer.rectTransform;
				break;
			case UniqueWeaponPos.BACK:
				result = this.TwoHandedRenderer.rectTransform;
				break;
			case UniqueWeaponPos.LEFTHAND:
				result = this.LeftHand.rectTransform.parent.GetComponent<RectTransform>();
				break;
			case UniqueWeaponPos.RIGHTHAND:
				result = this.RightHand.rectTransform.parent.GetComponent<RectTransform>();
				break;
			case UniqueWeaponPos.FIST:
				result = this.FistRenderer.rectTransform;
				break;
			default:
				result = this.TwoHandedRenderer.rectTransform;
				break;
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
			result = base.gameObject.GetComponent<RectTransform>();
		}
		return result;
	}

	// Token: 0x04004A1E RID: 18974
	private const string weaponUnit = "UIComponent/PortraitWeapon";

	// Token: 0x04004A1F RID: 18975
	private const string attachSrc = "UIComponent/WorkerPortraitAttachment";

	// Token: 0x04004A20 RID: 18976
	private static Color transparentColor = new Color(1f, 1f, 1f, 0f);

	// Token: 0x04004A21 RID: 18977
	private static Vector2 PositionFix_Head = new Vector2(5.1f, 121.4f);

	// Token: 0x04004A22 RID: 18978
	private static Vector2 PositionFix_HeadBack = new Vector2(0f, 160f);

	// Token: 0x04004A23 RID: 18979
	private static Vector2 PositionFix_Eye = new Vector2(-2.4f, 63f);

	// Token: 0x04004A24 RID: 18980
	public Image Head;

	// Token: 0x04004A25 RID: 18981
	public Image Eye;

	// Token: 0x04004A26 RID: 18982
	public Image Mouth;

	// Token: 0x04004A27 RID: 18983
	public Image Eyebrow;

	// Token: 0x04004A28 RID: 18984
	public Image FrontHair;

	// Token: 0x04004A29 RID: 18985
	public Image RearHair;

	// Token: 0x04004A2A RID: 18986
	public Image CoatBack;

	// Token: 0x04004A2B RID: 18987
	public Image CoatRight;

	// Token: 0x04004A2C RID: 18988
	public Image CoatLeft;

	// Token: 0x04004A2D RID: 18989
	public Image LeftUpLeg;

	// Token: 0x04004A2E RID: 18990
	public Image LeftDownLeg;

	// Token: 0x04004A2F RID: 18991
	public Image RightUpLeg;

	// Token: 0x04004A30 RID: 18992
	public Image RightDownLeg;

	// Token: 0x04004A31 RID: 18993
	public Image LeftUpArm;

	// Token: 0x04004A32 RID: 18994
	public Image LeftDownArm;

	// Token: 0x04004A33 RID: 18995
	public Image LeftHand;

	// Token: 0x04004A34 RID: 18996
	public Image Body;

	// Token: 0x04004A35 RID: 18997
	public Image RightUpArm;

	// Token: 0x04004A36 RID: 18998
	public Image RightDownArm;

	// Token: 0x04004A37 RID: 18999
	public Image RightHand;

	// Token: 0x04004A38 RID: 19000
	public Image Symbol;

	// Token: 0x04004A39 RID: 19001
	public GameObject[] WeaponMask;

	// Token: 0x04004A3A RID: 19002
	public Image TwoHandedRenderer;

	// Token: 0x04004A3B RID: 19003
	public Image OneHandedRenderer;

	// Token: 0x04004A3C RID: 19004
	public Image FistRenderer;

	// Token: 0x04004A3D RID: 19005
	private bool _mouthReplace;

	// Token: 0x04004A3E RID: 19006
	private bool _handReplace;

	// Token: 0x04004A3F RID: 19007
	public bool WeaponSet;

	// Token: 0x04004A40 RID: 19008
	private WorkerModel model;

	// Token: 0x04004A41 RID: 19009
	private EquipmentTypeInfo _armor;

	// Token: 0x04004A42 RID: 19010
	public bool log;

	// Token: 0x04004A43 RID: 19011
	private List<EGOgiftModel> attachedGifts = new List<EGOgiftModel>();

	// Token: 0x04004A44 RID: 19012
	private Dictionary<long, WorkerPortraitAttachment> attachments = new Dictionary<long, WorkerPortraitAttachment>();

	// Token: 0x04004A45 RID: 19013
	private List<GameObject> weaponAdded = new List<GameObject>();

	// Token: 0x04004A46 RID: 19014
	private UniqueWeaponSpriteUnit _currentUnit;

    // <Patch>
    [NonSerialized]
    public Dictionary<LobotomyBaseMod.LcId, WorkerPortraitAttachment> attachmentsMod = new Dictionary<LobotomyBaseMod.LcId, WorkerPortraitAttachment>();
}
