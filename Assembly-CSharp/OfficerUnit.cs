using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorkerSprite;

// Token: 0x02000790 RID: 1936
public class OfficerUnit : WorkerUnit, IMouseOnSelectListener, IMouseCommandTarget
{
	// Token: 0x06003BDC RID: 15324 RVA: 0x00034F20 File Offset: 0x00033120
	public OfficerUnit()
	{
	}

	// Token: 0x06003BDD RID: 15325 RVA: 0x00034F33 File Offset: 0x00033133
	private void Awake()
	{
		this.model = null;
		this.workerModel = null;
		this._animController = base.GetComponent<UnitAnimatorController>();
	}

	// Token: 0x06003BDE RID: 15326 RVA: 0x00177D64 File Offset: 0x00175F64
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
		if (SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.YESOD))
		{
			int layer = LayerMask.NameToLayer("Front1");
			IEnumerator enumerator = this.showSpeech.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					transform.gameObject.layer = layer;
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

	// Token: 0x06003BDF RID: 15327 RVA: 0x00034F4F File Offset: 0x0003314F
	public void OnSuicide()
	{
		SoundEffectPlayer.PlayOnce("Weapons/officerGun", this.model.GetCurrentViewPosition());
	}

	// Token: 0x06003BE0 RID: 15328 RVA: 0x00034F6C File Offset: 0x0003316C
	public void SetModel(OfficerModel model)
	{
		this.model = model;
		this.workerModel = model;
		this.model.SetUnit(this);
		this.showSpeech.Init(this.model);
	}

	// Token: 0x06003BE1 RID: 15329 RVA: 0x00034F99 File Offset: 0x00033199
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.model == null)
		{
			return;
		}
	}

	// Token: 0x06003BE2 RID: 15330 RVA: 0x00177EBC File Offset: 0x001760BC
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

	// Token: 0x06003BE3 RID: 15331 RVA: 0x00177F08 File Offset: 0x00176108
	protected override void UpdateAnimationQuality()
	{
		base.UpdateAnimationQuality();
		if (GameManager.currentGameManager != null && GameManager.currentGameManager.state == GameState.STOP)
		{
			this.spineRenderer.optimizeLevel = 5;
			return;
		}
		if (this._inCamera)
		{
			if (this.model != null)
			{
				float currentScale = this.workerModel.GetMovableNode().currentScale;
			}
			float num = Camera.main.orthographicSize / this.workerModel.GetMovableNode().currentScale;
			if (num > 70f)
			{
				this.spineRenderer.optimizeLevel = 5;
			}
			else if (num > 45f)
			{
				this.spineRenderer.optimizeLevel = 4;
			}
			else if (num > 35f)
			{
				this.spineRenderer.optimizeLevel = 3;
			}
			else if (num > 30f)
			{
				this.spineRenderer.optimizeLevel = 2;
			}
			else if (num > 22f)
			{
				this.spineRenderer.optimizeLevel = 1;
			}
			else
			{
				this.spineRenderer.optimizeLevel = 0;
			}
		}
		else
		{
			this.spineRenderer.optimizeLevel = 5;
		}
	}

	// Token: 0x06003BE4 RID: 15332 RVA: 0x000043A5 File Offset: 0x000025A5
	public void OnClick()
	{
	}

	// Token: 0x06003BE5 RID: 15333 RVA: 0x0017803C File Offset: 0x0017623C
	public bool IsSelectable()
	{
		return !this.model.IsDead() && (this.model.unconAction is Uncontrollable_RedShoesAttract || this.model.unconAction is Uncontrollable_RedShoes || this.model.unconAction is Uncontrollable_YoungPrince || this.model.unconAction is Uncontrollable_Sakura || this.model.unconAction is Uncontrollable_Baku || this.model.unconAction is Uncontrollable_SingingMachine_attacker || this.model.unconAction is Uncontrollable_Yggdrasil || this.model.unconAction is Uncontrollable_Yggdrasil_Fake_PanicReady || this.model.unconAction is Uncontrollable_Yggdrasil_Fake_Panic);
	}

	// Token: 0x06003BE6 RID: 15334 RVA: 0x00034FAD File Offset: 0x000331AD
	public void OnSelect()
	{
		this.model.OnClick();
	}

	// Token: 0x06003BE7 RID: 15335 RVA: 0x000043A5 File Offset: 0x000025A5
	public void OnUnselect()
	{
	}

	// Token: 0x06003BE8 RID: 15336 RVA: 0x00034FBA File Offset: 0x000331BA
	public IMouseCommandTargetModel GetCommandTargetModel()
	{
		return this.model;
	}

	// Token: 0x06003BE9 RID: 15337 RVA: 0x00034FC2 File Offset: 0x000331C2
	public override void SetPanic(bool b)
	{
		base.SetPanic(b);
		if (b)
		{
			this._animController.SetOfficerPanic(b);
		}
	}

	// Token: 0x06003BEA RID: 15338 RVA: 0x00034FDD File Offset: 0x000331DD
	public override void RemoveShadow()
	{
		if (this.shadow != null)
		{
			this.shadow.SetActive(false);
		}
	}

	// Token: 0x06003BEB RID: 15339 RVA: 0x0017812C File Offset: 0x0017632C
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

	// Token: 0x06003BEC RID: 15340 RVA: 0x001781A0 File Offset: 0x001763A0
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

	// Token: 0x06003BED RID: 15341 RVA: 0x00034FFC File Offset: 0x000331FC
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

	// Token: 0x06003BEE RID: 15342 RVA: 0x0017822C File Offset: 0x0017642C
	private IEnumerator MannualMoving(Vector3 pos, bool block, bool moveZ, bool moveAnim, bool scaling, bool small, float unitWaitTime)
	{
		Transform target = base.gameObject.transform;
		Vector3 initialPos = new Vector3(target.position.x, target.position.y, target.position.z);
		Vector3 reference = new Vector3(pos.x - target.position.x, pos.y - target.position.y, pos.z - target.position.z);
		int cntMax = 20;
		int cnt = cntMax;
		this.blockMoving = block;
		float toScale = 1f;
		if (scaling)
		{
			if (small)
			{
				toScale = -0.1f;
			}
			else
			{
				toScale = 0.1f;
			}
		}
		float unitScale = toScale / (float)cntMax;
		while (cnt > 0)
		{
			if (moveAnim)
			{
			}
			yield return new WaitForSeconds(unitWaitTime);
			float zValue = this.zValue;
			if (moveZ)
			{
				zValue = initialPos.z + reference.z / (float)cntMax * (float)(cntMax - 1 - cnt);
			}
			target.position = new Vector3(initialPos.x + reference.x / (float)cntMax * (float)(cntMax - 1 - cnt), initialPos.y + reference.y / (float)cntMax * (float)(cntMax - 1 - cnt), zValue);
			if (!scaling || cnt % 2 == 0)
			{
			}
			cnt--;
		}
		if (moveAnim)
		{
		}
		this.isMovingByMannually = true;
		yield break;
	}

	// Token: 0x06003BEF RID: 15343 RVA: 0x0017827C File Offset: 0x0017647C
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

	// Token: 0x06003BF0 RID: 15344 RVA: 0x001782D4 File Offset: 0x001764D4
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

	// Token: 0x06003BF1 RID: 15345 RVA: 0x00178304 File Offset: 0x00176504
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

	// Token: 0x06003BF2 RID: 15346 RVA: 0x0003503B File Offset: 0x0003323B
	public bool CheckMannualMovingEnd()
	{
		if (this.isMovingByMannually)
		{
			this.isMovingStarted = false;
			return true;
		}
		return false;
	}

	// Token: 0x06003BF3 RID: 15347 RVA: 0x00035052 File Offset: 0x00033252
	public void ReleaseUpdatePosition()
	{
		this.blockMoving = false;
	}

	// Token: 0x06003BF4 RID: 15348 RVA: 0x00178358 File Offset: 0x00176558
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

	// Token: 0x06003BF5 RID: 15349 RVA: 0x001783B4 File Offset: 0x001765B4
	public void StopSound(string key)
	{
		SoundEffectPlayer soundEffectPlayer = null;
		if (this.sounds.TryGetValue(key, out soundEffectPlayer))
		{
			soundEffectPlayer.Stop();
		}
	}

	// Token: 0x06003BF6 RID: 15350 RVA: 0x0003505B File Offset: 0x0003325B
	public void RevealShadow()
	{
		if (this.shadow != null)
		{
			this.shadow.gameObject.SetActive(true);
		}
	}

	// Token: 0x06003BF7 RID: 15351 RVA: 0x001783DC File Offset: 0x001765DC
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

	// Token: 0x06003BF8 RID: 15352 RVA: 0x0003507F File Offset: 0x0003327F
	public void OnResurrect()
	{
		this.RevealShadow();
		this.ui.Name.gameObject.SetActive(true);
	}

	// Token: 0x06003BF9 RID: 15353 RVA: 0x0017847C File Offset: 0x0017667C
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

	// Token: 0x06003BFA RID: 15354 RVA: 0x00178530 File Offset: 0x00176730
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

	// Token: 0x06003BFB RID: 15355 RVA: 0x001785E8 File Offset: 0x001767E8
	public void MakeEffectAttach(string src, Transform t)
	{
		GameObject gameObject = Prefab.LoadPrefab(src);
		gameObject.transform.SetParent(t);
		gameObject.transform.position = t.transform.position;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localRotation = Quaternion.identity;
	}

	// Token: 0x06003BFC RID: 15356 RVA: 0x00178640 File Offset: 0x00176840
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

	// Token: 0x06003BFD RID: 15357 RVA: 0x001786DC File Offset: 0x001768DC
	public void ClearEffect()
	{
		foreach (GameObject obj in this.effectAttached)
		{
			UnityEngine.Object.Destroy(obj);
		}
		this.effectAttached.Clear();
	}

	// Token: 0x06003BFE RID: 15358 RVA: 0x0003509D File Offset: 0x0003329D
	public override void ShutUp()
	{
		this.showSpeech.DisableText();
	}

	// Token: 0x040036EB RID: 14059
	[NonSerialized]
	public OfficerModel model;

	// Token: 0x040036EC RID: 14060
	public GameObject officerAttackedAnimator;

	// Token: 0x040036ED RID: 14061
	public OfficerUnitUI ui;

	// Token: 0x040036EE RID: 14062
	public float tempZval;

	// Token: 0x040036EF RID: 14063
	private bool changeState;

	// Token: 0x040036F0 RID: 14064
	private string currentBool = string.Empty;

	// Token: 0x040036F1 RID: 14065
	private bool uiOpened;

	// Token: 0x040036F2 RID: 14066
	public const float backZVal = 0f;

	// Token: 0x040036F3 RID: 14067
	public Dictionary<string, SoundEffectPlayer> sounds;

	// Token: 0x040036F4 RID: 14068
	public GameObject memoObject;

	// Token: 0x040036F5 RID: 14069
	private float waitTimer;

	// Token: 0x040036F6 RID: 14070
	private bool puppetAnimHasMoveCheck;

	// Token: 0x040036F7 RID: 14071
	private RuntimeAnimatorController oldPuppetAnimController;

	// Token: 0x040036F8 RID: 14072
	public bool isMovingByMannually;

	// Token: 0x040036F9 RID: 14073
	private bool isMovingStarted;
}
