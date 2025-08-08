using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200043A RID: 1082
public class OutterGodDawn : OutterGodOrdealCreature
{
	// Token: 0x06002649 RID: 9801 RVA: 0x00026A54 File Offset: 0x00024C54
	public OutterGodDawn()
	{
	}

	// Token: 0x17000384 RID: 900
	// (get) Token: 0x0600264A RID: 9802 RVA: 0x0001F287 File Offset: 0x0001D487
	private static float aggroTime
	{
		get
		{
			return UnityEngine.Random.Range(10f, 15f);
		}
	}

	// Token: 0x17000385 RID: 901
	// (get) Token: 0x0600264B RID: 9803 RVA: 0x00026A80 File Offset: 0x00024C80
	private static float boomTime
	{
		get
		{
			return UnityEngine.Random.Range(60f, 70f);
		}
	}

	// Token: 0x17000386 RID: 902
	// (get) Token: 0x0600264C RID: 9804 RVA: 0x00026A91 File Offset: 0x00024C91
	private static int attackDmg
	{
		get
		{
			return UnityEngine.Random.Range(1, 4);
		}
	}

	// Token: 0x17000387 RID: 903
	// (get) Token: 0x0600264D RID: 9805 RVA: 0x00026A9A File Offset: 0x00024C9A
	private static int boomDmg
	{
		get
		{
			return UnityEngine.Random.Range(10, 15);
		}
	}

	// Token: 0x17000388 RID: 904
	// (get) Token: 0x0600264E RID: 9806 RVA: 0x00026AA5 File Offset: 0x00024CA5
	public OutterGodDawnAnim animScript
	{
		get
		{
			if (this._animScript == null)
			{
				this._animScript = (base.Unit.animTarget as OutterGodDawnAnim);
			}
			return this._animScript;
		}
	}

	// Token: 0x0600264F RID: 9807 RVA: 0x00026AD4 File Offset: 0x00024CD4
	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
		this.animScript.SetScript(this);
		this.boomTimer.StartTimer(OutterGodDawn.boomTime);
	}

	// Token: 0x06002650 RID: 9808 RVA: 0x001145A0 File Offset: 0x001127A0
	public override void OnTakeDamage(UnitModel actor, DamageInfo dmg, float value)
	{
		base.OnTakeDamage(actor, dmg, value);
		if (this.GonnaBoom())
		{
			return;
		}
		if (!this.boomTimer.started)
		{
			return;
		}
		if (this.model.hp <= 0f)
		{
			this.Suppressed();
		}
		WorkerModel workerModel = actor as WorkerModel;
		if (workerModel != null)
		{
			if (this._currentTarget == null)
			{
				this._currentTarget = workerModel;
				this.aggroTimer.StartTimer(OutterGodDawn.aggroTime);
			}
			else if (this._currentTarget == workerModel)
			{
				this.aggroTimer.StartTimer(OutterGodDawn.aggroTime);
			}
		}
	}

	// Token: 0x06002651 RID: 9809 RVA: 0x00114640 File Offset: 0x00112840
	public override void OnFixedUpdate(CreatureModel creature)
	{
		base.OnFixedUpdate(creature);
		if (this.model.hp <= 0f)
		{
			return;
		}
		if (this.boomTimer.RunTimer())
		{
			this.BoomStart();
			return;
		}
		if (this.aggroTimer.RunTimer())
		{
			this.InitAggro();
		}
		this.MakeMovement();
	}

	// Token: 0x06002652 RID: 9810 RVA: 0x00026AF9 File Offset: 0x00024CF9
	private void InitAggro()
	{
		this._currentTarget = null;
		this.StopMovement();
	}

	// Token: 0x06002653 RID: 9811 RVA: 0x001146A0 File Offset: 0x001128A0
	private void MakeMovement()
	{
		if (this._currentTarget != null)
		{
			if (this.IsHostile(this._currentTarget))
			{
				if (this.IsInRange(this._currentTarget, 0.2f))
				{
					this.StopMovement();
				}
				else
				{
					this.MoveToTarget(this._currentTarget);
				}
				return;
			}
			this.InitAggro();
		}
		WorkerModel nearest = this.GetNearest(10f);
		if (nearest != null)
		{
			this.MoveToTarget(nearest);
		}
		else if (!this.movable.IsMoving())
		{
			this.Wander();
		}
	}

	// Token: 0x06002654 RID: 9812 RVA: 0x000210BE File Offset: 0x0001F2BE
	private void StopMovement()
	{
		this.movable.StopMoving();
	}

	// Token: 0x06002655 RID: 9813 RVA: 0x000FA9F0 File Offset: 0x000F8BF0
	private float GetDistance(WorkerModel target)
	{
		float currentScale = this.movable.currentScale;
		float x = this.movable.GetCurrentViewPosition().x;
		float x2 = target.GetCurrentViewPosition().x;
		float num = Math.Abs(x - x2);
		return num / currentScale;
	}

	// Token: 0x06002656 RID: 9814 RVA: 0x00114734 File Offset: 0x00112934
	private bool IsHostile(WorkerModel worker)
	{
		AgentModel agentModel = worker as AgentModel;
		return !worker.IsDead() && (agentModel == null || agentModel.currentSkill == null) && worker.IsAttackTargetable() && this.IsInPassage(worker);
	}

	// Token: 0x06002657 RID: 9815 RVA: 0x00114784 File Offset: 0x00112984
	private WorkerModel GetNearest(float range)
	{
		List<WorkerModel> targets = this.GetTargets(range);
		WorkerModel result = null;
		float num = 10000f;
		foreach (WorkerModel workerModel in targets)
		{
			float distance = this.GetDistance(workerModel);
			if (distance < num)
			{
				result = workerModel;
				num = distance;
			}
		}
		return result;
	}

	// Token: 0x06002658 RID: 9816 RVA: 0x001147FC File Offset: 0x001129FC
	private List<WorkerModel> GetTargets(float range)
	{
		List<WorkerModel> list = new List<WorkerModel>();
		if (this.currentPassage == null)
		{
			return list;
		}
		foreach (MovableObjectNode movableObjectNode in this.currentPassage.GetEnteredTargets())
		{
			WorkerModel workerModel = movableObjectNode.GetUnit() as WorkerModel;
			if (workerModel != null)
			{
				if (this.IsHostile(workerModel))
				{
					if (this.IsInRange(workerModel, range))
					{
						if (workerModel is AgentModel)
						{
							AgentModel agentModel = workerModel as AgentModel;
							if (agentModel.GetState() == AgentAIState.MANAGE)
							{
								continue;
							}
						}
						list.Add(workerModel);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06002659 RID: 9817 RVA: 0x001148D0 File Offset: 0x00112AD0
	private bool IsInRange(WorkerModel target, float range)
	{
		float distance = this.GetDistance(target);
		return range >= distance;
	}

	// Token: 0x0600265A RID: 9818 RVA: 0x001148F0 File Offset: 0x00112AF0
	private bool IsInPassage(WorkerModel target)
	{
		return !target.IsDead() && target.GetMovableNode().currentPassage != null && target.GetMovableNode().currentPassage == this.currentPassage && !target.GetMovableNode().currentPassage.IsIsolate();
	}

	// Token: 0x0600265B RID: 9819 RVA: 0x00026B08 File Offset: 0x00024D08
	private void MoveToTarget(WorkerModel target)
	{
		this.model.MoveToMovable(target.GetMovableNode());
	}

	// Token: 0x0600265C RID: 9820 RVA: 0x0011494C File Offset: 0x00112B4C
	private void Wander()
	{
		if (this.currentPassage == null)
		{
			Debug.Log("null passage");
			return;
		}
		List<MapNode> list = new List<MapNode>(this.currentPassage.GetNodeList());
		MapNode mapNode = list[UnityEngine.Random.Range(0, list.Count)];
		this.model.MoveToNode(mapNode);
	}

	// Token: 0x0600265D RID: 9821 RVA: 0x001149A0 File Offset: 0x00112BA0
	public void OnAttackDamageTimeCalled()
	{
		List<WorkerModel> targets = this.GetTargets(2f);
		foreach (WorkerModel workerModel in targets)
		{
			workerModel.TakeDamage(this.model, new DamageInfo(this.attackType, (float)OutterGodDawn.attackDmg));
			DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(workerModel, this.attackType, this.model);
		}
	}

	// Token: 0x0600265E RID: 9822 RVA: 0x00026B1B File Offset: 0x00024D1B
	private void BoomStart()
	{
		this.animScript.OnBoomStart();
		this.model.hp = 0f;
		this.model.Suppressed();
	}

	// Token: 0x0600265F RID: 9823 RVA: 0x00026B43 File Offset: 0x00024D43
	public void OnBoom()
	{
		this.MakeEffect("OutterGodDawnSkillEffect");
	}

	// Token: 0x06002660 RID: 9824 RVA: 0x00114A2C File Offset: 0x00112C2C
	public void OnBoomEnd()
	{
		Sefira sefira = this.model.sefira;
		List<WorkerModel> list = new List<WorkerModel>();
		List<CreatureModel> list2 = new List<CreatureModel>(sefira.creatureList);
		foreach (AgentModel item in sefira.agentList)
		{
			list.Add(item);
		}
		foreach (OfficerModel item2 in sefira.officerList)
		{
			list.Add(item2);
		}
		foreach (WorkerModel workerModel in list)
		{
			if (!workerModel.IsDead())
			{
				workerModel.TakeDamage(this.model, new DamageInfo(this.boomType, (float)OutterGodDawn.boomDmg));
			}
		}
		foreach (CreatureModel creatureModel in list2)
		{
			if (!creatureModel.IsEscaped())
			{
				creatureModel.SetQliphothCounter(0);
			}
		}
	}

	// Token: 0x06002661 RID: 9825 RVA: 0x00026B51 File Offset: 0x00024D51
	public override bool OnAfterSuppressed()
	{
		this.OnDie();
		return true;
	}

	// Token: 0x06002662 RID: 9826 RVA: 0x00026B5A File Offset: 0x00024D5A
	private void Suppressed()
	{
		this.boomTimer.StopTimer();
		this.MakeEffect("OutterGodDawnDeadEffect");
		this.animScript.OnSuppressed();
	}

	// Token: 0x06002663 RID: 9827 RVA: 0x00026B7E File Offset: 0x00024D7E
	private bool GonnaBoom()
	{
		return this.animScript.GonnaBoom();
	}

	// Token: 0x06002664 RID: 9828 RVA: 0x00114BC0 File Offset: 0x00112DC0
	private GameObject MakeEffect(string src)
	{
		GameObject gameObject = Prefab.LoadPrefab("Effect/Creature/OutterGodDawn/" + src);
		Vector3 one = Vector3.one;
		if (gameObject == null || !this.animScript.gameObject.activeInHierarchy)
		{
			return null;
		}
		if (this.model.GetDirection() == UnitDirection.LEFT)
		{
			one.x *= -1f;
		}
		gameObject.transform.position = this.animScript.gameObject.transform.position;
		gameObject.transform.localScale = one;
		gameObject.transform.localRotation = Quaternion.identity;
		return gameObject;
	}

	// Token: 0x04002574 RID: 9588
	private Timer aggroTimer = new Timer();

	// Token: 0x04002575 RID: 9589
	private const float _aggroTimeMax = 15f;

	// Token: 0x04002576 RID: 9590
	private const float _aggroTimeMin = 10f;

	// Token: 0x04002577 RID: 9591
	private Timer boomTimer = new Timer();

	// Token: 0x04002578 RID: 9592
	private const float _boomTimeMax = 70f;

	// Token: 0x04002579 RID: 9593
	private const float _boomTimeMin = 60f;

	// Token: 0x0400257A RID: 9594
	private const int _attackDmgMax = 3;

	// Token: 0x0400257B RID: 9595
	private const int _attackDmgMin = 1;

	// Token: 0x0400257C RID: 9596
	private RwbpType attackType = RwbpType.B;

	// Token: 0x0400257D RID: 9597
	private const int _boomDmgMax = 15;

	// Token: 0x0400257E RID: 9598
	private const int _boomDmgMin = 10;

	// Token: 0x0400257F RID: 9599
	private RwbpType boomType = RwbpType.W;

	// Token: 0x04002580 RID: 9600
	private const float _attackDmgRange = 2f;

	// Token: 0x04002581 RID: 9601
	private const float _recognizeRange = 10f;

	// Token: 0x04002582 RID: 9602
	private const string _effect_src = "Effect/Creature/OutterGodDawn/";

	// Token: 0x04002583 RID: 9603
	private const string _effect_dead = "OutterGodDawnDeadEffect";

	// Token: 0x04002584 RID: 9604
	private const string _effect_skill = "OutterGodDawnSkillEffect";

	// Token: 0x04002585 RID: 9605
	private WorkerModel _currentTarget;

	// Token: 0x04002586 RID: 9606
	private OutterGodDawnAnim _animScript;
}
