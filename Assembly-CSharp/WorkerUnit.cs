using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using WorkerSpine;
using WorkerSprite;

// Token: 0x02000B3D RID: 2877
public class WorkerUnit : MonoBehaviour
{
	// Token: 0x06005785 RID: 22405 RVA: 0x00046570 File Offset: 0x00044770
	public void SetDefaultZValue(float value)
	{
		this.zValueDefault = value;
		this.zValue = value;
	}

	// Token: 0x06005786 RID: 22406 RVA: 0x00046580 File Offset: 0x00044780
	public void ResetZValue()
	{
		this.zValue = this.zValueDefault;
	}

	// Token: 0x06005787 RID: 22407 RVA: 0x001FA15C File Offset: 0x001F835C
	protected void UpdateDirection()
	{
		if (this.blockRotation)
		{
			return;
		}
		MovableObjectNode movableNode = this.workerModel.GetMovableNode();
		UnitDirection direction = movableNode.GetDirection();
		Vector3 localScale = this.animRoot.localScale;
		if (direction == UnitDirection.RIGHT)
		{
			if (localScale.x > 0f)
			{
				localScale.x = -localScale.x;
			}
		}
		else if (localScale.x < 0f)
		{
			localScale.x = -localScale.x;
		}
		this.animRoot.localScale = localScale;
	}

	// Token: 0x06005788 RID: 22408 RVA: 0x001FA1EC File Offset: 0x001F83EC
	protected void UpdateViewPosition()
	{
		if (this.blockMoving)
		{
			return;
		}
		base.transform.localScale = new Vector3(this.workerModel.GetMovableNode().currentScale, this.workerModel.GetMovableNode().currentScale, base.transform.localScale.z);
		MapEdge currentEdge = this.workerModel.GetCurrentEdge();
		Vector3 vector = this.workerModel.GetCurrentViewPosition() + this.recoilPosition;
		if (currentEdge != null && currentEdge.type == "door")
		{
			vector.z = 100000f;
			base.transform.localPosition = vector;
		}
		else
		{
			if (!this.workerModel.GetMovableNode().isIgnoreZValue)
			{
				vector.z += this.zValue;
			}
			base.transform.localPosition = vector;
		}
		this.UpdateCheckInCamera(vector);
	}

	// Token: 0x06005789 RID: 22409 RVA: 0x001FA2E0 File Offset: 0x001F84E0
	private void UpdateCheckInCamera(Vector3 newPosition)
	{
		this._inCamera = false;
		Camera main = Camera.main;
		float orthographicSize = main.orthographicSize;
		float aspect = main.aspect;
		Vector3 position = main.transform.position;
		Vector3 vector = newPosition - position;
		float num = orthographicSize * aspect;
		float num2 = orthographicSize;
		float num3 = 1.5f;
		float num4 = 1.5f;
		if (num > vector.x - num3 && -num < vector.x + num3 && num2 > vector.y - num4 && -num2 < vector.y + num4)
		{
			this._inCamera = true;
		}
	}

	// Token: 0x0600578A RID: 22410 RVA: 0x000043CD File Offset: 0x000025CD
	protected virtual void UpdateAnimationQuality()
	{
	}

	// Token: 0x0600578B RID: 22411 RVA: 0x001FA380 File Offset: 0x001F8580
	protected void UpdateAnimatorChange()
	{ // <Patch>
		this._animChangeTimer.StopTimer();
		if (this._animChangeReady && !this._animChanged)
		{
			this._animChanged = true;
			if (this.workerModel.Equipment.weapon != null && this.workerModel.Equipment.weapon.metaInfo.weaponClassType == WeaponClassType.SPECIAL)
			{
				LobotomyBaseMod.KeyValuePairSS name = new LobotomyBaseMod.KeyValuePairSS(EquipmentTypeInfo.GetLcId(this.workerModel.Equipment.weapon.metaInfo).packageId, this.workerModel.Equipment.weapon.metaInfo.specialWeaponAnim);
				this.animChanger.ChangeAnimator_Mod(name, true);
			}
			if (!this.workerModel.IsPanic())
			{
				this.SetWorkerFaceType(WorkerFaceType.BATTLE);
			}
		}
		if (!this._animChangeReady && this._animChanged)
		{
			this._animChanged = false;
			if (this.workerModel.Equipment.weapon != null && this.workerModel.Equipment.weapon.metaInfo.weaponClassType == WeaponClassType.SPECIAL)
			{
				this.animChanger.ChangeAnimator();
			}
			if (!this.workerModel.IsPanic())
			{
				this.SetWorkerFaceType(WorkerFaceType.DEFAULT);
			}
		}
	}

	// Token: 0x0600578C RID: 22412 RVA: 0x0004658E File Offset: 0x0004478E
	public void ChangeAnimatorForcely(string name, bool uniqueFace, bool useSep = false)
	{ // <Patch>
        ChangeAnimatorForcely_Mod(new LobotomyBaseMod.KeyValuePairSS(string.Empty, name), uniqueFace, useSep);
        /*
		this._animChangeTimer.StopTimer();
		this._animChangeReady = false;
		this._animChanged = false;
		if (uniqueFace)
		{
			this.animChanger.ChangeAnimatorWithUniqueFace(name, useSep);
		}
		else
		{
			this.animChanger.ChangeAnimator(name);
		}*/
	}

	// Token: 0x0600578D RID: 22413 RVA: 0x000465CD File Offset: 0x000447CD
	public void ChangeAnimatorDefault()
	{
		this.animChanger.ChangeAnimator();
	}

	// Token: 0x0600578E RID: 22414 RVA: 0x000465DA File Offset: 0x000447DA
	public virtual void LateUpdate()
	{
		if (this.workerModel.GetMovableNode().IsMoving())
		{
			this._animController.SetMove(true);
		}
		else
		{
			this._animController.SetMove(false);
		}
	}

	// Token: 0x0600578F RID: 22415 RVA: 0x000043CD File Offset: 0x000025CD
	public virtual void FixedUpdate()
	{
	}

	// Token: 0x06005790 RID: 22416 RVA: 0x0004660E File Offset: 0x0004480E
	public void Attack(int attackType, float attackSpeed)
	{
		if (this._animChanged != this._animChangeReady)
		{
			this.UpdateAnimatorChange();
		}
		if (attackType >= 0)
		{
			this._animController.SetAttackType(attackType);
		}
		this._animController.SetBattle(true);
		this._animController.SetAttackSpeed(attackSpeed);
	}

	// Token: 0x06005791 RID: 22417 RVA: 0x0004664C File Offset: 0x0004484C
	public void EndAttack()
	{
		this._animController.SetBattle(false);
		this.workerModel.OnEndAttackCycle();
		Notice.instance.Send(NoticeName.WorkerAttackEnd, new object[]
		{
			this.workerModel
		});
	}

	// Token: 0x06005792 RID: 22418 RVA: 0x00046683 File Offset: 0x00044883
	public void SetWorkerFaceType(WorkerFaceType type)
	{
		this.spriteSetter.SetWorkerFaceType(type);
	}

	// Token: 0x06005793 RID: 22419 RVA: 0x00046691 File Offset: 0x00044891
	public void SetDeadType(DeadType type)
	{
		this._animController.SetDeadType(type);
	}

	// Token: 0x06005794 RID: 22420 RVA: 0x0004669F File Offset: 0x0004489F
	public virtual void SetPanic(bool b)
	{
		this._animController.SetPanic(b);
		if (this.bufUI != null)
		{
			this.bufUI.gameObject.SetActive(!b);
		}
	}

	// Token: 0x06005795 RID: 22421 RVA: 0x001FA4A0 File Offset: 0x001F86A0
	public void AttachUI(GameObject attachItem)
	{
		attachItem.transform.SetParent(this.uiRoot.transform);
		attachItem.transform.localPosition = Vector3.zero;
		attachItem.transform.localScale = Vector3.one;
		attachItem.transform.localRotation = Quaternion.identity;
	}

	// Token: 0x06005796 RID: 22422 RVA: 0x001FA4F4 File Offset: 0x001F86F4
	public void DisableUI()
	{
		this.uiActivated = false;
		IEnumerator enumerator = this.uiRoot.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				transform.gameObject.SetActive(false);
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
	}

	// Token: 0x06005797 RID: 22423 RVA: 0x000466D2 File Offset: 0x000448D2
	public void DisableShadow()
	{
		if (this.shadow != null)
		{
			this.shadow.SetActive(false);
		}
	}

	// Token: 0x06005798 RID: 22424 RVA: 0x000466F1 File Offset: 0x000448F1
	public void SetClickArea(bool state)
	{
		if (this.clickArea != null)
		{
			this.clickArea.SetActive(state);
		}
	}

	// Token: 0x06005799 RID: 22425 RVA: 0x000043CD File Offset: 0x000025CD
	public virtual void ShutUp()
	{
	}

	// Token: 0x0600579A RID: 22426 RVA: 0x000043CD File Offset: 0x000025CD
	public virtual void RemoveShadow()
	{
	}

	// Token: 0x0600579B RID: 22427 RVA: 0x001FA56C File Offset: 0x001F876C
	public virtual Transform GetHairTransform()
	{
		Transform transform = this.spriteSetter.GiftPos[1];
		transform.localPosition = new Vector3(transform.localPosition.x, 0.4f, transform.localPosition.z);
		return transform;
	}

	// Token: 0x0600579C RID: 22428 RVA: 0x00046710 File Offset: 0x00044910
	public virtual Transform GetBodyTransform()
	{
		return base.transform;
	}

	// Token: 0x0600579D RID: 22429 RVA: 0x0000440A File Offset: 0x0000260A
	public virtual GameObject MakeCreatureEffectToHead(long creatureId)
	{
		return null;
	}

	// Token: 0x0600579E RID: 22430 RVA: 0x00046718 File Offset: 0x00044918
	public virtual BufStateUI.BufData AddUnitBuf(UnitBuf buf, Sprite sprite)
	{
		if (this.bufUI == null)
		{
			return null;
		}
		return this.bufUI.AddBuf(buf, sprite);
	}

	// Token: 0x0600579F RID: 22431 RVA: 0x0004673A File Offset: 0x0004493A
	public virtual BufStateUI.BufData AddUnitBuf(UnitBuf buf, BufRenderer bufObject, bool copy = false)
	{
		if (this.bufUI == null)
		{
			return null;
		}
		return this.bufUI.AddBuf(buf, bufObject, copy);
	}

	// Token: 0x060057A0 RID: 22432 RVA: 0x0004675D File Offset: 0x0004495D
	public void RemoveUnitBuf(UnitBuf buf)
	{
		if (this.bufUI == null)
		{
			return;
		}
		this.bufUI.RemoveBuf(buf.type);
	}

    // <Patch>
    public void ChangeAnimatorForcely_Mod(LobotomyBaseMod.KeyValuePairSS name, bool uniqueFace, bool useSep = false)
    {
        this._animChangeTimer.StopTimer();
        this._animChangeReady = false;
        this._animChanged = false;
        if (uniqueFace)
        {
            this.animChanger.ChangeAnimatorWithUniqueFace_Mod(name, useSep);
        }
        else
        {
            this.animChanger.ChangeAnimator_Mod(name);
        }
    }

	// Token: 0x04005098 RID: 20632
	public WorkerModel workerModel;

	// Token: 0x04005099 RID: 20633
	public SkeletonRenderer spineRenderer;

	// Token: 0x0400509A RID: 20634
	protected bool _inCamera;

	// Token: 0x0400509B RID: 20635
	public Canvas uiRoot;

	// Token: 0x0400509C RID: 20636
	public Transform animRoot;

	// Token: 0x0400509D RID: 20637
	public GameObject clickArea;

	// Token: 0x0400509E RID: 20638
	public GameObject shadow;

	// Token: 0x0400509F RID: 20639
	public AnimatorEventHandler animEventHandler;

	// Token: 0x040050A0 RID: 20640
	public WorkerSpriteSetter spriteSetter;

	// Token: 0x040050A1 RID: 20641
	public WeaponSetter weaponSetter;

	// Token: 0x040050A2 RID: 20642
	protected UnitAnimatorController _animController;

	// Token: 0x040050A3 RID: 20643
	public AgentSpeech showSpeech;

	// Token: 0x040050A4 RID: 20644
	public Transform barrierParent;

	// Token: 0x040050A5 RID: 20645
	[NonSerialized]
	public bool blockRotation;

	// Token: 0x040050A6 RID: 20646
	[NonSerialized]
	public bool blockMoving;

	// Token: 0x040050A7 RID: 20647
	private float zValueDefault;

	// Token: 0x040050A8 RID: 20648
	[NonSerialized]
	public float zValue;

	// Token: 0x040050A9 RID: 20649
	protected Vector3 recoilPosition = new Vector3(0f, 0f, 0f);

	// Token: 0x040050AA RID: 20650
	protected bool uiActivated = true;

	// Token: 0x040050AB RID: 20651
	protected List<GameObject> effectAttached = new List<GameObject>();

	// Token: 0x040050AC RID: 20652
	public WorkerAnimatorChanger animChanger;

	// Token: 0x040050AD RID: 20653
	public BufStateUI bufUI;

	// Token: 0x040050AE RID: 20654
	protected bool _animChangeReady;

	// Token: 0x040050AF RID: 20655
	protected bool _animChanged;

	// Token: 0x040050B0 RID: 20656
	protected Timer _animChangeTimer = new Timer();
}
