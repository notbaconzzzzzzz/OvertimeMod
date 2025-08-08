using System;
using System.Collections.Generic;
using DeadEffect;
using GeburahBoss;
using UnityEngine;

// Token: 0x02000870 RID: 2160
public class GeburahCoreScript : CreatureBase
{
	// Token: 0x0600431E RID: 17182 RVA: 0x000043E5 File Offset: 0x000025E5
	public static void Log(string log)
	{
	}

	// Token: 0x17000635 RID: 1589
	// (get) Token: 0x0600431F RID: 17183 RVA: 0x0003963E File Offset: 0x0003783E
	public GeburahPhase Phase
	{
		get
		{
			return this._phase;
		}
	}

	// Token: 0x17000636 RID: 1590
	// (get) Token: 0x06004320 RID: 17184 RVA: 0x00039646 File Offset: 0x00037846
	public GeburahBossBase BossBase
	{
		get
		{
			return this.bossBase;
		}
	}

	// <Mod>
	public virtual OvertimeGeburahBossBase OvertimeBossBase
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000637 RID: 1591
	// (get) Token: 0x06004321 RID: 17185 RVA: 0x0003964E File Offset: 0x0003784E
	public GeburahCoreAnim AnimScript
	{
		get
		{
			return base.Unit.animTarget as GeburahCoreAnim;
		}
	}

	// Token: 0x17000638 RID: 1592
	// (get) Token: 0x06004322 RID: 17186 RVA: 0x00039660 File Offset: 0x00037860
	private Sefira GeburahSefira
	{
		get
		{
			return SefiraManager.instance.GetSefira(SefiraEnum.GEBURAH);
		}
	}

	// Token: 0x17000639 RID: 1593
	// (get) Token: 0x06004323 RID: 17187 RVA: 0x0003966D File Offset: 0x0003786D
	// (set) Token: 0x06004324 RID: 17188 RVA: 0x00039675 File Offset: 0x00037875
	public bool IsInvincible
	{
		get
		{
			return this._isInvincible;
		}
		set
		{
			this._isInvincible = value;
		}
	}

	// Token: 0x06004325 RID: 17189 RVA: 0x0003967E File Offset: 0x0003787E
	public void SetBossBase(GeburahBossBase bossBase)
	{
		this.bossBase = bossBase;
		this._phase = GeburahPhase.START;
	}

	// Token: 0x06004326 RID: 17190 RVA: 0x0003968E File Offset: 0x0003788E
	public void SetDamageInvoked(GeburahActionMethod actionMethod)
	{
		this._damageInvoked = actionMethod;
	}

	// Token: 0x06004327 RID: 17191 RVA: 0x00039697 File Offset: 0x00037897
	public void SetAttackEndInvoked(GeburahActionMethod actionMethod)
	{
		this._attackEndInvoked = actionMethod;
	}

	// Token: 0x06004328 RID: 17192 RVA: 0x000396A0 File Offset: 0x000378A0
	public void SetEventCalledInvoked(GeburahEventCalled actionMethod)
	{
		this._eventCalledInvoked = actionMethod;
	}

	// Token: 0x06004329 RID: 17193 RVA: 0x000396A9 File Offset: 0x000378A9
	public void ClearDamageInvoked()
	{
		this.SetDamageInvoked(null);
	}

	// Token: 0x0600432A RID: 17194 RVA: 0x000396B2 File Offset: 0x000378B2
	public void ClearAttackEndInvoked()
	{
		this.SetAttackEndInvoked(null);
	}

	// Token: 0x0600432B RID: 17195 RVA: 0x000396BB File Offset: 0x000378BB
	public void ClearEventInvoked()
	{
		this.SetEventCalledInvoked(null);
	}

	// Token: 0x0600432C RID: 17196 RVA: 0x000396C4 File Offset: 0x000378C4
	public override void OnViewInit(CreatureUnit unit)
	{
		this.AnimScript.SetScript(this);
	}

	// Token: 0x0600432D RID: 17197 RVA: 0x001A30C8 File Offset: 0x001A12C8
	public void OnProjectileGiveDamage(UnitModel target, GeburahProjectile proj)
	{
		if (proj == null)
		{
			return;
		}
		if (proj.damageInfo != null)
		{
			this.GeburahGiveDamage(target, proj.damageInfo, false);
		}
		if (target is WorkerModel && proj.projectileType == ProjectileType.BLOODYTREE && (target as WorkerModel).IsDead())
		{
			this.MakeBloodyTree(target as WorkerModel);
		}
		if (proj.projectileType == ProjectileType.MAGICALGIRL)
		{
			target.AddUnitBuf(new MovementBuf(0.5f, 2f));
		}
	}

	// Token: 0x0600432E RID: 17198 RVA: 0x00038F26 File Offset: 0x00037126
	public void SetSpeed(float speed = 4.8f)
	{
		(this.model as SefiraBossCreatureModel).speed = speed;
	}

	// Token: 0x0600432F RID: 17199 RVA: 0x001A3150 File Offset: 0x001A1350
	public override void OnStageStart()
	{
		this.damageCalculator.CurDamageInit();
		this.model.EscapeWithoutIsolateRoom();
		this.SetSpeed(4.8f);
		this.MoveToNextPhase();
		GameObject gameObject = Prefab.LoadPrefab("Effect/SefiraBoss/GeburahProjectile/GeburahSpawn");
		gameObject.transform.SetParent(EffectLayer.currentLayer.transform);
		gameObject.transform.position = this.movable.GetCurrentViewPosition();
		float currentScale = this.movable.currentScale;
		gameObject.transform.localScale = Vector3.one * currentScale;
		gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
		this.movable.SetTeleportable(false);
	}

	// Token: 0x06004330 RID: 17200 RVA: 0x000396D2 File Offset: 0x000378D2
	public void OnAttackEnd()
	{
		if (this._attackEndInvoked != null)
		{
			this._attackEndInvoked();
		}
	}

	// Token: 0x06004331 RID: 17201 RVA: 0x000396EA File Offset: 0x000378EA
	public void OnDamage()
	{
		if (this._damageInvoked != null)
		{
			this._damageInvoked();
		}
	}

	// Token: 0x06004332 RID: 17202 RVA: 0x001A3200 File Offset: 0x001A1400
	public List<UnitModel> GetNearUnits()
	{
		List<UnitModel> list = new List<UnitModel>();
		if (this.currentPassage == null)
		{
			return list;
		}
		MovableObjectNode[] enteredTargets = this.movable.currentPassage.GetEnteredTargets(this.movable);
		foreach (MovableObjectNode movableObjectNode in enteredTargets)
		{
			UnitModel unit = movableObjectNode.GetUnit();
			if (!list.Contains(unit))
			{
				if (unit.IsAttackTargetable())
				{
					list.Add(unit);
				}
			}
		}
		return list;
	}

	// Token: 0x06004333 RID: 17203 RVA: 0x001A3288 File Offset: 0x001A1488
	public override void OnFixedUpdate(CreatureModel creature)
	{
		if (this._bloodyTreeTimer.started)
		{
			this._bloodyTreeTimer.RunTimer();
		}
		if (this._recoverTimer.started)
		{
			this.model.hp = Mathf.Lerp(1f, (float)this.model.maxHp, this._recoverTimer.Rate);
			if (this._recoverTimer.RunTimer())
			{
				this.model.hp = (float)this.model.maxHp;
				if (BossBase != null) BossBase.StartHexagonEffect();
				if (OvertimeBossBase != null) OvertimeBossBase.StartHexagonEffect();
			}
			return;
		}
		if (this.Phase == GeburahPhase.END)
		{
			if (this.movable.IsMoving())
			{
				this.model.ClearCommand();
			}
			return;
		}
		if (this._phaseExecution != null)
		{
			this._phaseExecution.FixedUpdate();
			if (this._currentAction == null)
			{
				if (this._actionQueue.Count == 0)
				{
					this._currentAction = this._phaseExecution.GetNextAction(this.GetNearUnits());
					GeburahCoreScript.Log("Next Action : " + this._currentAction.GetType().ToString());
				}
				else
				{
					this._currentAction = this._actionQueue.Dequeue();
					GeburahCoreScript.Log("Next Action By Queue : " + this._currentAction.GetType().ToString());
				}
			}
			else
			{
				try
				{
					if (this._currentAction.actionState == GeburahActionState.START)
					{
						this._currentAction.OnStart();
					}
					else if (this._currentAction.actionState == GeburahActionState.EXECUTE)
					{
						this._currentAction.OnExecute();
					}
					else if (this._currentAction.actionState == GeburahActionState.ENDED)
					{
						this._currentAction.OnEnd();
						this._currentAction = null;
					}
				}
				catch (Exception message)
				{
					string log = string.Format("Action Error, State : {0}, ActionType : {1}\n", this._currentAction.actionState, this._currentAction.GetType());
					GeburahCoreScript.Log(log);
					Debug.LogError(message);
				}
			}
		}
	}

	// Token: 0x06004334 RID: 17204 RVA: 0x00039702 File Offset: 0x00037902
	public bool CanStartBloodyTree()
	{
		return !this._bloodyTreeTimer.started;
	}

	// Token: 0x06004335 RID: 17205 RVA: 0x00039712 File Offset: 0x00037912
	public void OnStartBloodyTreeTimer()
	{
		this._bloodyTreeTimer.StartTimer(this._bloodyTreeFreq.GetRandomFloat());
	}

	// Token: 0x06004336 RID: 17206 RVA: 0x0003972A File Offset: 0x0003792A
	public void InterruptCurrentAction()
	{
		this._currentAction = null;
	}

	// Token: 0x06004337 RID: 17207 RVA: 0x00039733 File Offset: 0x00037933
	public void ClearActionQueue()
	{
		this._actionQueue.Clear();
	}

	// Token: 0x06004338 RID: 17208 RVA: 0x00039740 File Offset: 0x00037940
	public void EnqeueAction(GeburahAction action)
	{
		this._actionQueue.Enqueue(action);
	}

	// Token: 0x06004339 RID: 17209 RVA: 0x001A349C File Offset: 0x001A169C
	public Sefira GetRandomMoveSefira(Sefira removeSefira)
	{
		List<Sefira> list = new List<Sefira>(PlayerModel.instance.GetOpenedAreaList());
		if (list.Contains(this.GeburahSefira))
		{
			list.Remove(this.GeburahSefira);
		}
		if (list.Contains(removeSefira))
		{
			list.Remove(removeSefira);
		}
		if (list.Count == 0)
		{
			return null;
		}
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	// Token: 0x0600433A RID: 17210 RVA: 0x001A350C File Offset: 0x001A170C
	public Sefira GetCurrentStandingSefira()
	{
		PassageObjectModel passageObjectModel = this.currentPassage;
		Sefira result;
		try
		{
			if (passageObjectModel == null)
			{
				Sefira sefira = null;
				MapEdge currentEdge = this.movable.GetCurrentEdge();
				if (currentEdge == null)
				{
					result = null;
				}
				else
				{
					MapNode mapNode = currentEdge.node1;
					passageObjectModel = mapNode.GetAttachedPassage();
					if (passageObjectModel != null)
					{
						sefira = SefiraManager.instance.GetSefira(passageObjectModel.GetSefiraName());
					}
					else
					{
						mapNode = currentEdge.node2;
						passageObjectModel = mapNode.GetAttachedPassage();
						if (passageObjectModel != null)
						{
							sefira = SefiraManager.instance.GetSefira(passageObjectModel.GetSefiraName());
						}
					}
					result = sefira;
				}
			}
			else
			{
				result = SefiraManager.instance.GetSefira(passageObjectModel.GetSefiraName());
			}
		}
		catch (Exception ex)
		{
			GeburahCoreScript.Log(ex.ToString());
			result = null;
		}
		return result;
	}

	// Token: 0x0600433B RID: 17211 RVA: 0x0003974E File Offset: 0x0003794E
	public void OnAnimEventCalled(int i)
	{
		if (this._eventCalledInvoked != null)
		{
			this._eventCalledInvoked(i);
		}
	}

	// Token: 0x0600433C RID: 17212 RVA: 0x001A35D8 File Offset: 0x001A17D8
	public bool GenProjectile(Transform pivot, ProjectileType type, out GeburahProjectile output)
	{
		GameObject gameObject = Prefab.LoadPrefab("Effect/SefiraBoss/GeburahProjectile/GeburahProjectile");
		GeburahProjectile component = gameObject.GetComponent<GeburahProjectile>();
		gameObject.transform.SetParent(EffectLayer.currentLayer.transform);
		gameObject.transform.position = pivot.transform.position;
		gameObject.transform.localScale = pivot.transform.lossyScale;
		gameObject.transform.localRotation = pivot.transform.rotation;
		component.projectileType = type;
		string projectileName = this.GetProjectileName(type);
		bool result = this.AttachProjectile(component, projectileName);
		component.geburah = this;
		output = component;
		return result;
	}

	// Token: 0x0600433D RID: 17213 RVA: 0x001A3674 File Offset: 0x001A1874
	private string GetProjectileName(ProjectileType type)
	{
		string result = string.Empty;
		switch (type)
		{
		case ProjectileType.ORCHESTRA:
			result = "DacapoProjectile";
			break;
		case ProjectileType.NULLTHING:
			result = "MimicriProjectile";
			break;
		case ProjectileType.BLOODYTREE:
			result = "BloodyTreeProjectile";
			break;
		case ProjectileType.MAGICALGIRL:
			result = "GreedyProjectile";
			break;
		case ProjectileType.LONGBIRD:
			result = "LongBirdProjectile";
			break;
		}
		return result;
	}

	// Token: 0x0600433E RID: 17214 RVA: 0x001A36E0 File Offset: 0x001A18E0
	public bool AttachProjectile(GeburahProjectile proj, string projectileName)
	{
		try
		{
			string name = "Effect/SefiraBoss/GeburahProjectile/" + projectileName;
			GameObject gameObject = Prefab.LoadPrefab(name);
			gameObject.transform.SetParent(proj.transform);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
			Animator component = gameObject.GetComponent<Animator>();
			proj.animator = component;
		}
		catch (Exception ex)
		{
			return false;
		}
		return true;
	}

	// Token: 0x0600433F RID: 17215 RVA: 0x001A3778 File Offset: 0x001A1978
	public void MakeBloodyTree(WorkerModel target)
	{
		int num = UnityEngine.Random.Range(0, 3);
		GameObject gameObject = Prefab.LoadPrefab("Agent/Dead/BloodyTree/BloodyTreeDeadEffect_" + num);
		BloodyTreeDeadScript component = gameObject.GetComponent<BloodyTreeDeadScript>();
		component.GenerateParts(this.AnimScript, target, (num != 0) ? 0.5f : 1f);
		gameObject.transform.SetParent(EffectLayer.currentLayer.transform);
		Vector3 vector;
		// target.GetCurrentViewPosition().x = vector.x + UnityEngine.Random.Range(-1f, 1f) * target.GetMovableNode().currentScale;
		gameObject.transform.position = target.GetCurrentViewPosition();
		gameObject.transform.localScale = target.GetMovableNode().currentScale * Vector3.one;
		target.GetWorkerUnit().gameObject.SetActive(false);
	}

	// Token: 0x06004340 RID: 17216 RVA: 0x001A3854 File Offset: 0x001A1A54
	public GeburahTeleport MakeTeleprot()
	{
		GameObject gameObject = Prefab.LoadPrefab("Effect/SefiraBoss/GeburahProjectile/GeburahTeleport");
		GeburahTeleport component = gameObject.GetComponent<GeburahTeleport>();
		gameObject.SetActive(false);
		return component;
	}

	// Token: 0x06004341 RID: 17217 RVA: 0x001A3880 File Offset: 0x001A1A80
	public List<UnitModel> GetRangedTargets(List<UnitModel> near, float front, float rear)
	{
		List<UnitModel> list = new List<UnitModel>();
		float x = this.movable.GetCurrentViewPosition().x;
		UnitDirection direction = this.movable.GetDirection();
		foreach (UnitModel unitModel in near)
		{
			float x2 = unitModel.GetCurrentViewPosition().x;
			if (direction == UnitDirection.RIGHT)
			{
				if (x2 >= x - rear && x2 <= x + front)
				{
					list.Add(unitModel);
				}
			}
			else if (x2 >= x - front && x2 <= x + rear)
			{
				list.Add(unitModel);
			}
		}
		return list;
	}

	// Token: 0x06004342 RID: 17218 RVA: 0x00004422 File Offset: 0x00002622
	public GeburahAction GetNextAction()
	{
		return null;
	}

	// Token: 0x06004343 RID: 17219 RVA: 0x001A392C File Offset: 0x001A1B2C
	public bool IsInRange(UnitModel target, float range)
	{
		if (this.movable.currentPassage == null)
		{
			return false;
		}
		if (target.GetMovableNode().currentPassage != this.movable.currentPassage)
		{
			return false;
		}
		float x = this.movable.GetCurrentViewPosition().x;
		float x2 = target.GetCurrentViewPosition().x;
		return Mathf.Abs(x - x2) <= range;
	}

	// Token: 0x06004344 RID: 17220 RVA: 0x00039767 File Offset: 0x00037967
	public void LookTarget(UnitModel target)
	{
		this.movable.SetDirection(this.GetDirectionWithTarget(target));
	}

	// Token: 0x06004345 RID: 17221 RVA: 0x0019CFC4 File Offset: 0x0019B1C4
	public UnitDirection GetDirectionWithTarget(UnitModel target)
	{
		UnitDirection result = UnitDirection.RIGHT;
		float x = this.movable.GetCurrentViewPosition().x;
		float x2 = target.GetCurrentViewPosition().x;
		if (x > x2)
		{
			result = UnitDirection.LEFT;
		}
		return result;
	}

	// Token: 0x06004346 RID: 17222 RVA: 0x001A399C File Offset: 0x001A1B9C
	public override void OnTakeDamage(UnitModel actor, DamageInfo dmg, float value)
	{
		this.damageCalculator.OnTakeDamage(value);
		if (this.Phase != GeburahPhase.P4 && this.model.hp <= 0f)
		{
			this.model.hp = 1f;
			this._recoverTimer.StartTimer(1f);
			this._phaseExecution.OnPrevSuppressed();
			if (this.Phase == GeburahPhase.P1 || this.Phase == GeburahPhase.P3)
			{
				if (BossBase != null) BossBase.OnChangePhase();
				if (OvertimeBossBase != null) OvertimeBossBase.OnGeburaSuppress(Phase);
			}
		}
	}

	// Token: 0x06004347 RID: 17223 RVA: 0x0003977B File Offset: 0x0003797B
	public void SetDamageFillEvent(GeburahActionMethod actionMethod)
	{
		if (actionMethod == null)
		{
			this.damageCalculator.damageFilled = null;
			return;
		}
		this.damageCalculator.damageFilled = new GeburahCoreScript.DamageCalculator.OnDamageFilled(actionMethod.Invoke);
	}

	// Token: 0x06004348 RID: 17224 RVA: 0x000397A7 File Offset: 0x000379A7
	public void ClearDamageFillEvent()
	{
		this.SetDamageFillEvent(null);
	}

	// Token: 0x06004349 RID: 17225 RVA: 0x001A3A24 File Offset: 0x001A1C24
	public void SetPhase(GeburahPhase phase)
	{ // <Mod>
		this._phase = phase;
		GeburahCoreScript.Log("Phase : " + phase);
		if (phase != GeburahPhase.START && phase != GeburahPhase.END)
		{
			CreatureModel model = this.model;
			int num = (int)phase;
			model.SetDefenseId(num.ToString());
		}
		this.AnimScript.SetPhase(phase);
		if (BossBase != null && phase == GeburahPhase.END && SefiraBossManager.Instance.IsKetherBoss(KetherBossType.E2))
		{
			this.BossBase.OnCleared();
		}
	}

	// Token: 0x0600434A RID: 17226 RVA: 0x000397B0 File Offset: 0x000379B0
	public override bool CanTakeDamage(UnitModel attacker, DamageInfo dmg)
	{
		return !this.IsInvincible && !this._recoverTimer.started && (this._currentAction == null || this._currentAction.CanTakeDamage());
	}

	// Token: 0x0600434B RID: 17227 RVA: 0x001A3AA4 File Offset: 0x001A1CA4
	public void MoveToNextPhase()
	{
		switch (this.Phase)
		{
		case GeburahPhase.START:
			this._phaseExecution = new FirstPhase(this);
			break;
		case GeburahPhase.P1:
			this._phaseExecution = new SecondPhase(this);
			break;
		case GeburahPhase.P2:
			this._phaseExecution = new ThirdPhase(this);
			break;
		case GeburahPhase.P3:
			this._phaseExecution = new FourthPhase(this);
			break;
		}
	}

	// Token: 0x0600434C RID: 17228 RVA: 0x001A3B20 File Offset: 0x001A1D20
	public List<DamageInfo> GetDamageInfo(int attackType, int indexer)
	{
		List<DamageInfo> list = new List<DamageInfo>();
		switch (this.Phase)
		{
		case GeburahPhase.P1:
			if (attackType != 0)
			{
				if (attackType != 1)
				{
					if (attackType == 2)
					{
						list.Add(GeburahStaticInfo.P1_SpiderMom);
						list.Add(GeburahStaticInfo.P1_OneBad);
					}
				}
				else
				{
					list.Add(GeburahStaticInfo.P1_OneBad);
				}
			}
			else
			{
				list.Add(GeburahStaticInfo.P1_SpiderMom);
			}
			break;
		case GeburahPhase.P2:
			if (attackType != 0)
			{
				if (attackType != 1)
				{
					if (attackType == 2)
					{
						list.Add(GeburahStaticInfo.P2_Orchestra);
						list.Add(GeburahStaticInfo.P2_Nullthing);
					}
				}
				else
				{
					list.Add(GeburahStaticInfo.P2_Orchestra);
				}
			}
			else
			{
				list.Add(GeburahStaticInfo.P2_Nullthing);
			}
			break;
		case GeburahPhase.P3:
			if (attackType == 0)
			{
				list.Add(GeburahStaticInfo.P3_LongBird);
			}
			break;
		case GeburahPhase.P4:
			if (attackType != 0)
			{
				if (attackType == 1)
				{
					list.Add(GeburahStaticInfo.GetP4RandomDamage());
				}
			}
			else
			{
				list.Add(GeburahStaticInfo.GetP4RandomDamage());
			}
			break;
		}
		return list;
	}

	// Token: 0x0600434D RID: 17229 RVA: 0x0019DFE8 File Offset: 0x0019C1E8
	public void OnMakeGut(WorkerModel target, UnitDirection dir)
	{
		WorkerUnit workerUnit = target.GetWorkerUnit();
		ExplodeGutEffect explodeGutEffect = null;
		if (ExplodeGutManager.instance.MakeEffects(workerUnit.gameObject.transform.position, ref explodeGutEffect))
		{
			explodeGutEffect.particleCount = UnityEngine.Random.Range(3, 9);
			explodeGutEffect.ground = target.GetMovableNode().GetCurrentViewPosition().y;
			float num = 0f;
			float num2 = 0f;
			explodeGutEffect.SetEffectSize(1f);
			explodeGutEffect.Shoot(ExplodeGutEffect.Directional.DIRECTION, dir);
			if (target.GetMovableNode().GetPassage() != null)
			{
				target.GetMovableNode().GetPassage().GetVerticalRange(ref num, ref num2);
				explodeGutEffect.SetCurrentPassage(target.GetMovableNode().GetPassage());
			}
		}
		workerUnit.gameObject.SetActive(false);
	}

	// Token: 0x0600434E RID: 17230 RVA: 0x001A3C64 File Offset: 0x001A1E64
	public void OnKillWokrer(WorkerModel target)
	{
		Vector3 currentViewPosition = target.GetCurrentViewPosition();
		float currentScale = target.GetMovableNode().currentScale;
		currentViewPosition.y += 2f * currentScale;
		Vector3 localScale = currentScale * Vector3.one;
		GameObject gameObject = Prefab.LoadPrefab("Effect/SefiraBoss/GeburahProjectile/GeburahKill");
		gameObject.transform.SetParent(EffectLayer.currentLayer.transform);
		gameObject.transform.localPosition = currentViewPosition;
		gameObject.transform.localScale = localScale;
		gameObject.transform.localRotation = Quaternion.Euler(Vector3.one);
	}

	// Token: 0x0600434F RID: 17231 RVA: 0x001A3CF4 File Offset: 0x001A1EF4
	public void MimicriNearDamage(Vector3 end)
	{
		GameObject gameObject = Prefab.LoadPrefab("Effect/SefiraBoss/GeburahProjectile/MimicriArrived");
		gameObject.transform.SetParent(EffectLayer.currentLayer.transform);
		gameObject.transform.position = end;
		gameObject.transform.localScale = Vector3.one * this.movable.currentScale * 2f;
		gameObject.transform.localRotation = Quaternion.Euler(new Vector3(-90f, 0f, 0f));
		PassageObjectModel currentPassage = this.currentPassage;
		if (currentPassage != null)
		{
			List<UnitModel> rangedTargets = this.GetRangedTargets(this.GetNearUnits(), GeburahStaticInfo.MimicriArriveRange.front, GeburahStaticInfo.MimicriArriveRange.rear);
			foreach (UnitModel unitModel in rangedTargets)
			{
				unitModel.TakeDamage(this.model, GeburahStaticInfo.MimicriArriveDamage);
			}
		}
	}

	// Token: 0x06004350 RID: 17232 RVA: 0x001A3E00 File Offset: 0x001A2000
	public void MakeSlash(UnitModel target)
	{
		float currentScale = target.GetMovableNode().currentScale;
		Vector3 currentViewPosition = target.GetCurrentViewPosition();
		GameObject gameObject = Prefab.LoadPrefab("Effect/SefiraBoss/GeburahProjectile/GeburahSlash" + UnityEngine.Random.Range(0, 2));
		gameObject.transform.SetParent(EffectLayer.currentLayer.transform);
		gameObject.transform.position = new Vector3(currentViewPosition.x, currentViewPosition.y + UnityEngine.Random.Range(1f, 2f) * currentScale, currentViewPosition.z - 1f);
		Vector3 localScale = Vector3.one * 2f * currentScale;
		if (UnityEngine.Random.value <= 0.5f)
		{
			localScale.x = -localScale.x;
		}
		gameObject.transform.localScale = localScale;
		gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
	}

	// Token: 0x06004351 RID: 17233 RVA: 0x001A3EE4 File Offset: 0x001A20E4
	public void GeburahGiveDamage(UnitModel target, DamageInfo damage, bool maunualgutMake = false)
	{
		target.TakeDamage(this.model, damage);
		if (target is WorkerModel)
		{
			WorkerModel workerModel = target as WorkerModel;
			if (workerModel.IsDead())
			{
				Vector3 currentViewPosition = target.GetCurrentViewPosition();
				float currentScale = target.GetMovableNode().currentScale;
				Vector3 localScale = Vector3.one * currentScale;
				currentViewPosition.y += 2f * currentScale;
				currentViewPosition.z -= 5f;
				GameObject gameObject = Prefab.LoadPrefab("Effect/SefiraBoss/GeburahProjectile/GeburahKilled");
				gameObject.transform.SetParent(EffectLayer.currentLayer.transform);
				gameObject.transform.position = currentViewPosition;
				gameObject.transform.localScale = localScale;
				gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
				if (maunualgutMake || this.Phase >= GeburahPhase.P2)
				{
					this.OnMakeGut(workerModel, this.GetDirectionWithTarget(target));
				}
			}
		}
	}

	// Token: 0x06004352 RID: 17234 RVA: 0x000397E9 File Offset: 0x000379E9
	public override string GetName()
	{
		return LocalizeTextDataModel.instance.GetText("RedMist");
	}

	// Token: 0x06004353 RID: 17235 RVA: 0x000397FA File Offset: 0x000379FA
	public void MakeBattleDesc(params int[] index)
	{
		this.MakeBattleDesc(GeburahStaticInfo.SelectRandomID(index));
	}

	// Token: 0x06004354 RID: 17236 RVA: 0x001A3FD4 File Offset: 0x001A21D4
	public void MakeBattleDesc(int index)
	{
		string empty = string.Empty;
		if (SefiraBossManager.Instance.TryGetBossDesc(SefiraEnum.GEBURAH, SefiraBossDescType.BATTLE, index, out empty))
		{
			this.AnimScript.MakeGeburahText(empty);
		}
	}

	// Token: 0x06004355 RID: 17237 RVA: 0x00039808 File Offset: 0x00037A08
	public override void OnSuppressed()
	{
		this.MakeBattleDesc(GeburahStaticInfo.Id_Dead);
		this.InterruptCurrentAction();
		this.ClearActionQueue();
		if (RabbitManager.instance.IsAnyRabbitEnabled())
		{
			RabbitManager.instance.SendBossClear();
		}
		this.SetPhase(GeburahPhase.END);
	}

	// Token: 0x06004356 RID: 17238 RVA: 0x0019D15C File Offset: 0x0019B35C
	public override SoundEffectPlayer MakeSound(string soundSrc)
	{
		return SoundEffectPlayer.PlayOnce(soundSrc, this.movable.GetCurrentViewPosition());
	}

	// Token: 0x06004357 RID: 17239 RVA: 0x00039841 File Offset: 0x00037A41
	public override float GetDamageFactor(UnitModel target, DamageInfo damage)
	{
		if (target is WorkerModel)
		{
			return 1f;
		}
		return 1f;
	}

	// Token: 0x04003DDB RID: 15835
	private const float defaultSpeed = 4.8f;

	// Token: 0x04003DDC RID: 15836
	public const string GeburahProjectileFolder = "Effect/SefiraBoss/GeburahProjectile/";

	// Token: 0x04003DDD RID: 15837
	public const string GeburahProjectileSrc = "Effect/SefiraBoss/GeburahProjectile/GeburahProjectile";

	// Token: 0x04003DDE RID: 15838
	public const string GeburahKillEffect = "Effect/SefiraBoss/GeburahProjectile/GeburahKill";

	// Token: 0x04003DDF RID: 15839
	public const string SpawnSrc = "GeburahSpawn";

	// Token: 0x04003DE0 RID: 15840
	public GeburahPhase _phase; // <Mod> changed to public

	// Token: 0x04003DE1 RID: 15841
	private GeburahBossBase bossBase;

	// Token: 0x04003DE2 RID: 15842
	public GeburahCoreScript.DamageCalculator damageCalculator = new GeburahCoreScript.DamageCalculator();

	// Token: 0x04003DE3 RID: 15843
	public Timer _recoverTimer = new Timer(); // <Mod> changed to public

	// Token: 0x04003DE4 RID: 15844
	private GeburahActionMethod _damageInvoked;

	// Token: 0x04003DE5 RID: 15845
	private GeburahActionMethod _attackEndInvoked;

	// Token: 0x04003DE6 RID: 15846
	private GeburahEventCalled _eventCalledInvoked;

	// Token: 0x04003DE7 RID: 15847
	private GeburahAction _currentAction;

	// Token: 0x04003DE8 RID: 15848
	private Queue<GeburahAction> _actionQueue = new Queue<GeburahAction>();

	// Token: 0x04003DE9 RID: 15849
	private GeburahPhaseExectuion _phaseExecution;

	// Token: 0x04003DEA RID: 15850
	private Timer _bloodyTreeTimer = new Timer();

	// Token: 0x04003DEB RID: 15851
	private MinMax _bloodyTreeFreq = new MinMax(8f, 12f);

	// Token: 0x04003DEC RID: 15852
	private bool _isInvincible;

	// Token: 0x02000871 RID: 2161
	public class DamageCalculator
	{
		// Token: 0x06004359 RID: 17241 RVA: 0x00039859 File Offset: 0x00037A59
		public void SetMaxDamage(float maxDamage)
		{
			this.maxDamage = maxDamage;
		}

		// Token: 0x0600435A RID: 17242 RVA: 0x00039862 File Offset: 0x00037A62
		public void CurDamageInit()
		{
			this.curCumlatiedDamage = 0f;
		}

		// Token: 0x0600435B RID: 17243 RVA: 0x0003986F File Offset: 0x00037A6F
		public void OnTakeDamage(float damageValue)
		{
			this.curCumlatiedDamage += damageValue;
			if (this.curCumlatiedDamage >= this.maxDamage)
			{
				if (this.damageFilled != null)
				{
					this.damageFilled();
				}
				this.CurDamageInit();
			}
		}

		// Token: 0x04003DED RID: 15853
		public float curCumlatiedDamage;

		// Token: 0x04003DEE RID: 15854
		public float maxDamage;

		// Token: 0x04003DEF RID: 15855
		public GeburahCoreScript.DamageCalculator.OnDamageFilled damageFilled;

		// Token: 0x02000872 RID: 2162
		// (Invoke) Token: 0x0600435D RID: 17245
		public delegate void OnDamageFilled();
	}
}
