/*
Fuck if I know // 
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorkerSprite;

// Token: 0x02000790 RID: 1936
public class OfficerUnit : WorkerUnit, IMouseOnSelectListener, IMouseCommandTarget, IMouseOnDragListener
{
	// Token: 0x06003C06 RID: 15366 RVA: 0x00035051 File Offset: 0x00033251
	public OfficerUnit()
	{
	}

	// Token: 0x06003C07 RID: 15367 RVA: 0x00035064 File Offset: 0x00033264
	private void Awake()
	{
		this.model = null;
		this.workerModel = null;
		this._animController = base.GetComponent<UnitAnimatorController>();
	}

	// Token: 0x06003C08 RID: 15368 RVA: 0x0017A5D0 File Offset: 0x001787D0
	public void Start()
	{
		if (this.memoObject != null && this.memoObject.activeInHierarchy)
		{
			this.memoObject.SetActive(false);
		}
		if (this.model == null)
		{
			return;
		}
		this.spriteSetter.SetWorkerType(this.model);
		this.ui.Name.text = this.model.name;
		this.effectAttached.Clear();
		this.animEventHandler.SetDamageEvent(new AnimatorEventHandler.EventDelegate(this.model.OnGiveDamageByWeapon));
		this.animEventHandler.SetAttackEndEvent(new AnimatorEventHandler.EventDelegate(base.EndAttack));
		this.animEventHandler.SetSuicideEvent(new AnimatorEventHandler.EventDelegate(this.OnSuicide));
		this.spriteSetter.InitBasicSet();
		this.OnChangeWeapon();
		if (SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.YESOD, false))
		{
			int layer = LayerMask.NameToLayer("Front1");
			IEnumerator enumerator = this.showSpeech.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					((Transform)obj).gameObject.layer = layer;
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
	}

	// Token: 0x06003C09 RID: 15369 RVA: 0x00035080 File Offset: 0x00033280
	public void OnSuicide()
	{
		SoundEffectPlayer.PlayOnce("Weapons/officerGun", this.model.GetCurrentViewPosition());
	}

	// Token: 0x06003C0A RID: 15370 RVA: 0x0003509D File Offset: 0x0003329D
	public void SetModel(OfficerModel model)
	{
		this.model = model;
		this.workerModel = model;
		this.model.SetUnit(this);
		this.showSpeech.Init(this.model);
	}

	// Token: 0x06003C0B RID: 15371 RVA: 0x000350CA File Offset: 0x000332CA
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		OfficerModel officerModel = this.model;
	}

	// Token: 0x06003C0C RID: 15372 RVA: 0x000350D9 File Offset: 0x000332D9
	private void Update()
	{
		this._animChangeTimer.RunTimer();
		if (this.model != null)
		{
			base.UpdateViewPosition();
			base.UpdateDirection();
			this.UpdateAnimationQuality();
		}
		if (!this._animChangeTimer.started)
		{
			base.UpdateAnimatorChange();
		}
	}

	// Token: 0x06003C0D RID: 15373 RVA: 0x0017A70C File Offset: 0x0017890C
	protected override void UpdateAnimationQuality()
	{
		base.UpdateAnimationQuality();
		if (GameManager.currentGameManager != null && GameManager.currentGameManager.state == GameState.STOP)
		{
			this.spineRenderer.optimizeLevel = 5;
			return;
		}
		if (!this._inCamera)
		{
			this.spineRenderer.optimizeLevel = 5;
			return;
		}
		if (this.model != null)
		{
			float currentScale = this.workerModel.GetMovableNode().currentScale;
		}
		float num = Camera.main.orthographicSize / this.workerModel.GetMovableNode().currentScale;
		if (num > 70f)
		{
			this.spineRenderer.optimizeLevel = 5;
			return;
		}
		if (num > 45f)
		{
			this.spineRenderer.optimizeLevel = 4;
			return;
		}
		if (num > 35f)
		{
			this.spineRenderer.optimizeLevel = 3;
			return;
		}
		if (num > 30f)
		{
			this.spineRenderer.optimizeLevel = 2;
			return;
		}
		if (num > 22f)
		{
			this.spineRenderer.optimizeLevel = 1;
			return;
		}
		this.spineRenderer.optimizeLevel = 0;
	}

	// Token: 0x06003C0E RID: 15374 RVA: 0x000043CD File Offset: 0x000025CD
	public void OnClick()
	{
	}

	// Token: 0x06003C0F RID: 15375 RVA: 0x0017A808 File Offset: 0x00178A08
	public bool IsSelectable()
	{
		return !this.model.IsDead() && (this.model.unconAction is Uncontrollable_RedShoesAttract || this.model.unconAction is Uncontrollable_RedShoes || this.model.unconAction is Uncontrollable_YoungPrince || this.model.unconAction is Uncontrollable_Sakura || this.model.unconAction is Uncontrollable_Baku || this.model.unconAction is Uncontrollable_SingingMachine_attacker || this.model.unconAction is Uncontrollable_Yggdrasil || this.model.unconAction is Uncontrollable_Yggdrasil_Fake_PanicReady || this.model.unconAction is Uncontrollable_Yggdrasil_Fake_Panic);
	}

	// Token: 0x06003C10 RID: 15376 RVA: 0x00035114 File Offset: 0x00033314
	public void OnSelect()
	{
		this.model.OnClick();
	}

	// Token: 0x06003C11 RID: 15377 RVA: 0x000043CD File Offset: 0x000025CD
	public void OnUnselect()
	{
	}

	// Token: 0x06003C12 RID: 15378 RVA: 0x00035121 File Offset: 0x00033321
	public IMouseCommandTargetModel GetCommandTargetModel()
	{
		return this.model;
	}

	// Token: 0x06003C13 RID: 15379 RVA: 0x00035129 File Offset: 0x00033329
	public override void SetPanic(bool b)
	{
		base.SetPanic(b);
		if (b)
		{
			this._animController.SetOfficerPanic(b);
		}
	}

	// Token: 0x06003C14 RID: 15380 RVA: 0x00035141 File Offset: 0x00033341
	public override void RemoveShadow()
	{
		if (this.shadow != null)
		{
			this.shadow.SetActive(false);
		}
	}

	// Token: 0x06003C15 RID: 15381 RVA: 0x0017A8D4 File Offset: 0x00178AD4
	public void PrepareWeapon()
	{
		if (this.model.Equipment.weapon != null && this.model.Equipment.weapon.metaInfo.weaponClassType == WeaponClassType.SPECIAL)
		{
			this._animChangeReady = true;
			this._animChangeTimer.StartTimer(0.5f);
		}
		if (!this.model.IsPanic())
		{
			base.SetWorkerFaceType(WorkerFaceType.BATTLE);
		}
	}

	// Token: 0x06003C16 RID: 15382 RVA: 0x0017A93C File Offset: 0x00178B3C
	public void CancelWeapon()
	{
		if (this.model.Equipment.weapon != null && this.model.Equipment.weapon.metaInfo.weaponClassType == WeaponClassType.SPECIAL)
		{
			this._animChangeReady = false;
			this._animChangeTimer.StartTimer(0.5f);
		}
		this._animController.SetBattleReady(false);
		this._animController.SetBattle(false);
		if (!this.model.IsPanic())
		{
			base.SetWorkerFaceType(WorkerFaceType.DEFAULT);
		}
	}

	// Token: 0x06003C17 RID: 15383 RVA: 0x0003515D File Offset: 0x0003335D
	public void OnChangeWeapon()
	{
		if (this.model == null)
		{
			return;
		}
		if (this.model.Equipment.weapon == null)
		{
			return;
		}
		this.weaponSetter.SetWeapon(this.model.Equipment.weapon);
	}

	// Token: 0x06003C18 RID: 15384 RVA: 0x0017A9BC File Offset: 0x00178BBC
	private IEnumerator MannualMoving(Vector3 pos, bool block, bool moveZ, bool moveAnim, bool scaling, bool small, float unitWaitTime)
	{
		Transform target = base.gameObject.transform;
		Vector3 initialPos = new Vector3(target.position.x, target.position.y, target.position.z);
		Vector3 reference = new Vector3(pos.x - target.position.x, pos.y - target.position.y, pos.z - target.position.z);
		int cntMax = 20;
		int cnt = cntMax;
		this.blockMoving = block;
		float num = 1f;
		if (scaling)
		{
			if (small)
			{
				num = -0.1f;
			}
			else
			{
				num = 0.1f;
			}
		}
		float num2 = num / (float)cntMax;
		while (cnt > 0)
		{
			yield return new WaitForSeconds(unitWaitTime);
			float z = this.zValue;
			if (moveZ)
			{
				z = initialPos.z + reference.z / (float)cntMax * (float)(cntMax - 1 - cnt);
			}
			target.position = new Vector3(initialPos.x + reference.x / (float)cntMax * (float)(cntMax - 1 - cnt), initialPos.y + reference.y / (float)cntMax * (float)(cntMax - 1 - cnt), z);
			if (scaling)
			{
				int num3 = cnt % 2;
			}
			int num4 = cnt;
			cnt = num4 - 1;
		}
		this.isMovingByMannually = true;
		yield break;
	}

	// Token: 0x06003C19 RID: 15385 RVA: 0x0017AA0C File Offset: 0x00178C0C
	public bool MannualMovingCall(Vector3 pos, bool blockMov, bool moveZ, bool moveAnim, bool scailing, bool small, float unitWaitTime)
	{
		if (!this.isMovingStarted)
		{
			this.isMovingStarted = true;
			this.isMovingByMannually = false;
			base.StartCoroutine(this.MannualMoving(pos, blockMov, moveZ, moveAnim, scailing, small, unitWaitTime));
			return false;
		}
		if (this.isMovingByMannually)
		{
			this.isMovingStarted = false;
			return true;
		}
		return false;
	}

	// Token: 0x06003C1A RID: 15386 RVA: 0x00035196 File Offset: 0x00033396
	private IEnumerator MannualMovingWithTime(Vector3 pos, bool blockMoving, float time)
	{
		Transform target = base.gameObject.transform;
		Vector3 initial = new Vector3(target.position.x, target.position.y, target.position.z);
		Vector3 reference = new Vector3(pos.x - target.position.x, pos.y - target.position.y, 0f);
		float elapsedTime = 0f;
		this.blockMoving = blockMoving;
		while (elapsedTime < time)
		{
			yield return new WaitForFixedUpdate();
			target.localPosition = new Vector3(initial.x + reference.x * (elapsedTime / time), initial.y + reference.y * (elapsedTime / time), initial.z);
			elapsedTime += Time.deltaTime;
		}
		this.isMovingByMannually = true;
		yield break;
	}

	// Token: 0x06003C1B RID: 15387 RVA: 0x0017AA5C File Offset: 0x00178C5C
	public bool MannualMovingCallWithTime(Vector3 pos, float time)
	{
		if (!this.isMovingStarted)
		{
			this.isMovingStarted = true;
			this.isMovingByMannually = false;
			base.StartCoroutine(this.MannualMovingWithTime(pos, true, time));
			return false;
		}
		if (this.isMovingByMannually)
		{
			this.isMovingByMannually = false;
			this.isMovingStarted = false;
			return true;
		}
		return false;
	}

	// Token: 0x06003C1C RID: 15388 RVA: 0x000351BA File Offset: 0x000333BA
	public bool CheckMannualMovingEnd()
	{
		if (this.isMovingByMannually)
		{
			this.isMovingStarted = false;
			return true;
		}
		return false;
	}

	// Token: 0x06003C1D RID: 15389 RVA: 0x000351CE File Offset: 0x000333CE
	public void ReleaseUpdatePosition()
	{
		this.blockMoving = false;
	}

	// Token: 0x06003C1E RID: 15390 RVA: 0x0017AAAC File Offset: 0x00178CAC
	public SoundEffectPlayer PlaySound(string src, string key, bool isLoop)
	{
		SoundEffectPlayer soundEffectPlayer;
		if (isLoop)
		{
			soundEffectPlayer = SoundEffectPlayer.Play(src, base.gameObject.transform);
		}
		else
		{
			soundEffectPlayer = SoundEffectPlayer.PlayOnce(src, base.gameObject.transform.position);
		}
		if (key != null)
		{
			this.sounds.Add(key, soundEffectPlayer);
		}
		return soundEffectPlayer;
	}

	// Token: 0x06003C1F RID: 15391 RVA: 0x0017AB00 File Offset: 0x00178D00
	public void StopSound(string key)
	{
		SoundEffectPlayer soundEffectPlayer = null;
		if (this.sounds.TryGetValue(key, out soundEffectPlayer))
		{
			soundEffectPlayer.Stop();
		}
	}

	// Token: 0x06003C20 RID: 15392 RVA: 0x000351D7 File Offset: 0x000333D7
	public void RevealShadow()
	{
		if (this.shadow != null)
		{
			this.shadow.gameObject.SetActive(true);
		}
	}

	// Token: 0x06003C21 RID: 15393 RVA: 0x0017AB28 File Offset: 0x00178D28
	public void OnDie()
	{
		this._animChangeReady = false;
		if (!this.model.specialDeadScene)
		{
			if (this._animChanged)
			{
				base.UpdateAnimatorChange();
			}
		}
		else
		{
			this._animChangeTimer.StopTimer();
			this._animChanged = false;
		}
		if (!this.model.IsPanic() && this.model.DeadType != DeadType.EXECUTION)
		{
			this.spriteSetter.SetWorkerFaceType(WorkerFaceType.DEAD);
		}
		this._animController.OnDie();
		this.RemoveShadow();
		this.ui.Name.gameObject.SetActive(false);
	}

	// Token: 0x06003C22 RID: 15394 RVA: 0x000351F8 File Offset: 0x000333F8
	public void OnResurrect()
	{
		this.RevealShadow();
		this.ui.Name.gameObject.SetActive(true);
	}

	// Token: 0x06003C23 RID: 15395 RVA: 0x0017ABBC File Offset: 0x00178DBC
	public GameObject MakeCreatureEffect(CreatureModel model)
	{
		Transform hairTransform = this.GetHairTransform();
		GameObject gameObject = Prefab.LoadPrefab("Effect/Creature/AgentAttached/" + model.metadataId + "_Effect");
		if (gameObject == null)
		{
			Debug.Log("Prefabs/Effect/Creature/AgentAttached/" + model.metadataId + "_Effect");
			return null;
		}
		GameObject gameObject2 = gameObject;
		gameObject2.transform.SetParent(hairTransform);
		gameObject2.transform.position = hairTransform.transform.position;
		gameObject2.transform.localScale = Vector3.one;
		gameObject2.transform.localRotation = Quaternion.identity;
		this.effectAttached.Add(gameObject2);
		return gameObject;
	}

	// Token: 0x06003C24 RID: 15396 RVA: 0x0017AC6C File Offset: 0x00178E6C
	public GameObject MakeCreatureEffect(CreatureModel model, bool attach)
	{
		Transform hairTransform = this.GetHairTransform();
		GameObject gameObject = Prefab.LoadPrefab("Effect/Creature/AgentAttached/" + model.metadataId + "_Effect");
		if (gameObject == null)
		{
			Debug.Log("Prefabs/Effect/Creature/AgentAttached/" + model.metadataId + "_Effect");
			return null;
		}
		GameObject gameObject2 = gameObject;
		gameObject2.transform.SetParent(hairTransform);
		gameObject2.transform.position = hairTransform.transform.position;
		gameObject2.transform.localScale = Vector3.one;
		gameObject2.transform.localRotation = Quaternion.identity;
		if (attach)
		{
			this.effectAttached.Add(gameObject2);
		}
		return gameObject;
	}

	// Token: 0x06003C25 RID: 15397 RVA: 0x0017AD20 File Offset: 0x00178F20
	public void MakeEffectAttach(string src, Transform t)
	{
		GameObject gameObject = Prefab.LoadPrefab(src);
		gameObject.transform.SetParent(t);
		gameObject.transform.position = t.transform.position;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localRotation = Quaternion.identity;
	}

	// Token: 0x06003C26 RID: 15398 RVA: 0x0017AD74 File Offset: 0x00178F74
	public GameObject MakeCreatureEffect(long id)
	{
		Transform hairTransform = this.GetHairTransform();
		GameObject gameObject = Prefab.LoadPrefab("Effect/Creature/AgentAttached/" + id + "_Effect");
		if (gameObject == null)
		{
			Debug.Log("Prefabs/Effect/Creature/AgentAttached/" + id + "_Effect");
			return null;
		}
		GameObject gameObject2 = gameObject;
		gameObject2.transform.SetParent(hairTransform);
		gameObject2.transform.position = hairTransform.transform.position;
		gameObject2.transform.localScale = Vector3.one;
		gameObject2.transform.localRotation = Quaternion.identity;
		return gameObject2;
	}

	// Token: 0x06003C27 RID: 15399 RVA: 0x0017AE0C File Offset: 0x0017900C
	public void ClearEffect()
	{
		foreach (GameObject obj in this.effectAttached)
		{
			UnityEngine.Object.Destroy(obj);
		}
		this.effectAttached.Clear();
	}

	// Token: 0x06003C28 RID: 15400 RVA: 0x00035216 File Offset: 0x00033416
	public override void ShutUp()
	{
		this.showSpeech.DisableText();
	}

	// Token: 0x06003C29 RID: 15401 RVA: 0x00035223 File Offset: 0x00033423
	public bool IsDragSelectable()
	{
		return !this.model.IsDead() && !this.model.IsCrazy();
	}

	// Token: 0x06003C2A RID: 15402 RVA: 0x000043CD File Offset: 0x000025CD
	public void OnEnterDragArea()
	{
	}

	// Token: 0x06003C2B RID: 15403 RVA: 0x000043CD File Offset: 0x000025CD
	public void OnExitDragArea()
	{
	}

	// Token: 0x04003704 RID: 14084
	[NonSerialized]
	public OfficerModel model;

	// Token: 0x04003705 RID: 14085
	public GameObject officerAttackedAnimator;

	// Token: 0x04003706 RID: 14086
	public OfficerUnitUI ui;

	// Token: 0x04003707 RID: 14087
	public float tempZval;

	// Token: 0x04003708 RID: 14088
	private bool changeState;

	// Token: 0x04003709 RID: 14089
	private string currentBool = string.Empty;

	// Token: 0x0400370A RID: 14090
	private bool uiOpened;

	// Token: 0x0400370B RID: 14091
	public const float backZVal = 0f;

	// Token: 0x0400370C RID: 14092
	public Dictionary<string, SoundEffectPlayer> sounds;

	// Token: 0x0400370D RID: 14093
	public GameObject memoObject;

	// Token: 0x0400370E RID: 14094
	private float waitTimer;

	// Token: 0x0400370F RID: 14095
	private bool puppetAnimHasMoveCheck;

	// Token: 0x04003710 RID: 14096
	private RuntimeAnimatorController oldPuppetAnimController;

	// Token: 0x04003711 RID: 14097
	public bool isMovingByMannually;

	// Token: 0x04003712 RID: 14098
	private bool isMovingStarted;
}