using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200048C RID: 1164
public class SmallBird : BirdCreatureBase, IObserver, IBirdControl
{
	// Token: 0x06002A87 RID: 10887 RVA: 0x00029B2A File Offset: 0x00027D2A
	public SmallBird()
	{
	}

	// Token: 0x170003F5 RID: 1013
	// (get) Token: 0x06002A88 RID: 10888 RVA: 0x0001FDF6 File Offset: 0x0001DFF6
	private static float mentalHeal
	{
		get
		{
			return UnityEngine.Random.Range(3f, 5f);
		}
	}

	// Token: 0x170003F6 RID: 1014
	// (get) Token: 0x06002A89 RID: 10889 RVA: 0x00029B61 File Offset: 0x00027D61
	private static float redDmg
	{
		get
		{
			return UnityEngine.Random.Range(1f, 1f);
		}
	}

	// Token: 0x170003F7 RID: 1015
	// (get) Token: 0x06002A8A RID: 10890 RVA: 0x00029B72 File Offset: 0x00027D72
	private static float madDmg
	{
		get
		{
			return UnityEngine.Random.Range(800f, 1200f);
		}
	}

	// Token: 0x170003F8 RID: 1016
	// (get) Token: 0x06002A8B RID: 10891 RVA: 0x00029B83 File Offset: 0x00027D83
	private static int totalDmg
	{
		get
		{
			return UnityEngine.Random.Range(8, 13);
		}
	}

	// Token: 0x170003F9 RID: 1017
	// (get) Token: 0x06002A8C RID: 10892 RVA: 0x00029B8D File Offset: 0x00027D8D
	public bool Boss_Activated
	{
		get
		{
			return this.boss_activated;
		}
	}

	// Token: 0x170003FA RID: 1018
	// (get) Token: 0x06002A8D RID: 10893 RVA: 0x00029B95 File Offset: 0x00027D95
	public BossBird Boss
	{
		get
		{
			return this.boss;
		}
	}

	// Token: 0x170003FB RID: 1019
	// (get) Token: 0x06002A8E RID: 10894 RVA: 0x00029B9D File Offset: 0x00027D9D
	public BossBird.OtherBirdState OtherBirdState
	{
		get
		{
			return this._otherBirdState;
		}
	}

	// Token: 0x170003FC RID: 1020
	// (get) Token: 0x06002A8F RID: 10895 RVA: 0x00029BA5 File Offset: 0x00027DA5
	public SmallBirdAnim animScript
	{
		get
		{
			if (this._animScript == null)
			{
				this._animScript = (base.Unit.animTarget as SmallBirdAnim);
			}
			return this._animScript;
		}
	}

	// Token: 0x06002A90 RID: 10896 RVA: 0x000140D5 File Offset: 0x000122D5
	public override bool HasUniqueFaction()
	{
		return true;
	}

	// Token: 0x06002A91 RID: 10897 RVA: 0x00029BD4 File Offset: 0x00027DD4
	public override bool IsAttackTargetable()
	{
		return this._otherBirdState == BossBird.OtherBirdState.NORMAL && base.IsAttackTargetable() && !this.returnCommandActivated;
	}

	// Token: 0x06002A92 RID: 10898 RVA: 0x0000450B File Offset: 0x0000270B
	public override bool IsAutoSuppressable()
	{
		return false;
	}

	// Token: 0x06002A93 RID: 10899 RVA: 0x00029BD4 File Offset: 0x00027DD4
	public override bool IsSuppressable()
	{
		return this._otherBirdState == BossBird.OtherBirdState.NORMAL && base.IsAttackTargetable() && !this.returnCommandActivated;
	}

	// Token: 0x06002A94 RID: 10900 RVA: 0x001262A4 File Offset: 0x001244A4
	public override bool CanTakeDamage(UnitModel attacker, DamageInfo dmg)
	{
		if (!base.IsAttackTargetable())
		{
			return false;
		}
		if (this._otherBirdState != BossBird.OtherBirdState.NORMAL)
		{
			return false;
		}
		if (this.returnCommandActivated)
		{
			return false;
		}
		if (!this.madMode && attacker is AgentModel)
		{
			AgentModel agentModel = attacker as AgentModel;
			if (!agentModel.IsPanic())
			{
				this.StartMadMode(agentModel);
				this.animScript.MadEffect();
			}
		}
		return true;
	}

	// Token: 0x06002A95 RID: 10901 RVA: 0x00029BF8 File Offset: 0x00027DF8
	public void SetBoss(BossBird boss)
	{
		this.boss = boss;
	}

	// Token: 0x06002A96 RID: 10902 RVA: 0x00126314 File Offset: 0x00124514
	public override void ParamInit()
	{
		this.boss_activated = false;
		this.returnCommandActivated = false;
		this.madMode = false;
		this.isAttacking = false;
		this.targetAgent = null;
		this.oldTargetNode = null;
		this.currentAttackedDamage = 0f;
		this.escapePattern = SmallBird.EscapePattern.NORMAL;
		this.changeTargetTimer.StopTimer();
		this.elevatorEscapeTimer.StopTimer();
		this.encountered.Clear();
		try
		{
			this.animScript.gameObject.SetActive(true);
			this.model.GetMovableNode().SetActive(true);
		}
		catch (Exception ex)
		{
		}
	}

	// Token: 0x06002A97 RID: 10903 RVA: 0x00029C01 File Offset: 0x00027E01
	public override void OnInit()
	{
		this.ParamInit();
		this.model.SetFactionForcely("11");
	}

	// Token: 0x06002A98 RID: 10904 RVA: 0x00029C19 File Offset: 0x00027E19
	public override void OnViewInit(CreatureUnit unit)
	{
		this.animScript.SetScript(this);
		this.animScript.Init();
	}

	// Token: 0x06002A99 RID: 10905 RVA: 0x00029C32 File Offset: 0x00027E32
	public override void OnStageStart()
	{
		this.ParamInit();
		this.animScript.OnStageStart();
		this.SetObserver(true);
		this._otherBirdState = BossBird.OtherBirdState.NORMAL;
	}

	// Token: 0x06002A9A RID: 10906 RVA: 0x00029C53 File Offset: 0x00027E53
	public override void OnStageRelease()
	{
		this.ParamInit();
		this.model.ClearCommand();
		this.movable.StopMoving();
		this.SetObserver(false);
	}

	// Token: 0x06002A9B RID: 10907 RVA: 0x00029C78 File Offset: 0x00027E78
	public override void OnEnterRoom(UseSkill skill)
	{
		this.SetObserver(false);
	}

	// Token: 0x06002A9C RID: 10908 RVA: 0x00029C81 File Offset: 0x00027E81
	public override void OnWorkCoolTimeEnd(CreatureFeelingState oldState)
	{
		this.SetObserver(true);
	}

	// Token: 0x06002A9D RID: 10909 RVA: 0x00029C8A File Offset: 0x00027E8A
	public override void Escape()
	{
		this.SetObserver(false);
		if (this.boss != null)
		{
			this.boss.OnBirdEscape(null, this, null);
		}
		this.animScript.OnEscape();
		this.model.Escape();
		this.MakeMovement();
	}

	// Token: 0x06002A9E RID: 10910 RVA: 0x00029CC8 File Offset: 0x00027EC8
	public override void OnReturn()
	{
		this.ParamInit();
		this.animScript.Init();
		this.SetObserver(true);
		this.model.ResetQliphothCounter();
	}

	// Token: 0x06002A9F RID: 10911 RVA: 0x00029CED File Offset: 0x00027EED
	private void SetPattern(SmallBird.EscapePattern pattern)
	{
		this.escapePattern = pattern;
	}

	// Token: 0x06002AA0 RID: 10912 RVA: 0x001263BC File Offset: 0x001245BC
	public override void UniqueEscape()
	{
		if (this._otherBirdState != BossBird.OtherBirdState.NORMAL)
		{
			return;
		}
		PassageObjectModel currentPassage = this.movable.currentPassage;
		if (currentPassage != null)
		{
			if (currentPassage.type == PassageType.VERTICAL)
			{
				if (!this.elevatorEscapeTimer.started)
				{
					this.elevatorEscapeTimer.StartTimer(10f);
				}
				else if (this.elevatorEscapeTimer.RunTimer())
				{
					this.ReturnCommand();
				}
			}
			else if (this.elevatorEscapeTimer.started)
			{
				this.elevatorEscapeTimer.StopTimer();
			}
		}
		if (this.returnCommandActivated && !this.isAttacking)
		{
			this.ReturnExecute();
		}
		else if (this.madMode)
		{
			if (this.CheckDistance())
			{
				this.StopMovement();
				this.StartAttackProcess(SmallBird.AttackPattern.MAD);
			}
			else
			{
				this.MakeMovement();
			}
		}
		else if (this.escapePattern == SmallBird.EscapePattern.PANIC)
		{
			if (this.targetAgent != null)
			{
				if (!this.targetAgent.IsPanic() || this.targetAgent.IsDead())
				{
					this.targetAgent = null;
					this.ReturnCommand();
					return;
				}
				if (this.CheckDistance())
				{
					this.StopMovement();
					this.StartAttackProcess(SmallBird.AttackPattern.MENTAL_HEAL);
				}
				else
				{
					this.MakeMovement();
				}
			}
			else
			{
				this.ReturnCommand();
			}
		}
		else if ((float)SmallBird.totalDmg <= this.currentAttackedDamage)
		{
			if (this.changeTargetTimer.started)
			{
				this.changeTargetTimer.StopTimer();
			}
			this.StopMovement();
			this.ReturnCommand();
		}
		else
		{
			WorkerModel workerModel = null;
			if (this.changeTargetTimer.RunTimer())
			{
				workerModel = this.targetAgent;
				this.targetAgent = null;
			}
			if (this.targetAgent != null)
			{
				if (this.targetAgent.IsDead())
				{
					workerModel = this.targetAgent;
					this.targetAgent = null;
					this.changeTargetTimer.StopTimer();
				}
				else if (this.targetAgent.GetState() == AgentAIState.MANAGE)
				{
					workerModel = this.targetAgent;
					this.targetAgent = null;
					this.changeTargetTimer.StopTimer();
				}
			}
			if (this.targetAgent == null)
			{
				List<WorkerModel> list = null;
				List<AgentModel> list2 = this.CheckNearWorkerEncounting(out list);
				List<AgentModel> list3 = new List<AgentModel>();
				foreach (AgentModel agentModel in list2)
				{
					if (agentModel.GetState() == AgentAIState.MANAGE)
					{
						list3.Add(agentModel);
					}
				}
				foreach (AgentModel item in list3)
				{
					list2.Remove(item);
				}
				if (list2.Count == 0)
				{
					this.MakeMovement();
				}
				else
				{
					if (list2.Count == 1)
					{
						this.targetAgent = list2[0];
					}
					else
					{
						while (list2.Count > 0)
						{
							this.targetAgent = list2[UnityEngine.Random.Range(0, list2.Count)];
							if (this.targetAgent != workerModel)
							{
								break;
							}
							list2.Remove(this.targetAgent);
						}
					}
					this.MakeMovement();
					if (!this.changeTargetTimer.started)
					{
						this.changeTargetTimer.StartTimer(3f);
					}
				}
			}
			else if (this.CheckDistance())
			{
				this.StopMovement();
				this.StartAttackProcess(SmallBird.AttackPattern.NORMAL);
			}
			else
			{
				this.MakeMovement();
			}
		}
	}

	// Token: 0x06002AA1 RID: 10913 RVA: 0x00126770 File Offset: 0x00124970
	private bool CheckDistance()
	{
		return this.targetAgent != null && this.targetAgent.GetMovableNode().currentPassage != null && this.targetAgent.GetMovableNode().currentPassage == this.movable.currentPassage && Math.Abs(this.targetAgent.GetCurrentViewPosition().x - this.movable.GetCurrentViewPosition().x) <= 1.7f;
	}

	// Token: 0x06002AA2 RID: 10914 RVA: 0x001267FC File Offset: 0x001249FC
	private void StartAttackProcess(SmallBird.AttackPattern pattern)
	{
		if (this.isAttacking)
		{
			return;
		}
		this.isAttacking = true;
		this.attackPattern = pattern;
		this.animScript.Attack();
		if (this.targetAgent != null)
		{
			this.SetDirection(this.targetAgent.GetMovableNode());
		}
	}

	// Token: 0x06002AA3 RID: 10915 RVA: 0x0012684C File Offset: 0x00124A4C
	private void SetDirection(MovableObjectNode targetNode)
	{
		UnitDirection direction = this.movable.GetDirection();
		float x = this.movable.GetCurrentViewPosition().x;
		float x2 = targetNode.GetCurrentViewPosition().x;
		if (direction == UnitDirection.LEFT)
		{
			if (x < x2)
			{
				this.movable.SetDirection(UnitDirection.RIGHT);
			}
		}
		else if (x > x2)
		{
			this.movable.SetDirection(UnitDirection.LEFT);
		}
	}

	// Token: 0x06002AA4 RID: 10916 RVA: 0x001268BC File Offset: 0x00124ABC
	public void GiveDamage()
	{ // <Mod>
		if (this.targetAgent == null)
		{
			return;
		}
		if (this.CheckDistance())
		{
			this.SetDirection(this.targetAgent.GetMovableNode());
			switch (this.attackPattern)
			{
			case SmallBird.AttackPattern.MENTAL_HEAL:
				this.targetAgent.RecoverMental(SmallBird.mentalHeal);
				if (this.targetAgent.mental >= (float)this.targetAgent.maxMental)
				{
					this.targetAgent.StopPanic();
				}
				break;
			case SmallBird.AttackPattern.NORMAL:
			{
				// float hp = this.targetAgent.hp;
                DamageInfo dmg = new DamageInfo(RwbpType.R, SmallBird.redDmg);
				this.targetAgent.TakeDamage(this.model, dmg);
				DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(this.targetAgent, RwbpType.R, this.model);
				// float num = hp - this.targetAgent.hp;
				this.currentAttackedDamage += dmg.result.resultDamage; // num
				break;
			}
			case SmallBird.AttackPattern.MAD:
			{
				this.targetAgent.TakeDamage(this.model, new DamageInfo(RwbpType.R, SmallBird.madDmg));
				DamageParticleEffect damageParticleEffect2 = DamageParticleEffect.Invoker(this.targetAgent, RwbpType.R, this.model);
				if (this.targetAgent.IsDead())
				{
					UnitDirection dir = UnitDirection.LEFT;
					if (this.targetAgent.GetCurrentViewPosition().x >= this.movable.GetCurrentViewPosition().x)
					{
						dir = UnitDirection.RIGHT;
					}
					this.MakeExplodeEffect(dir, this.targetAgent, 1f);
					AngelaConversation.instance.MakeMessage(AngelaMessageState.AGENT_DEAD_NORMAL, new object[]
					{
						this.targetAgent,
						this.model
					});
				}
				this.ReturnCommand();
				break;
			}
			default:
				Debug.Log("Invalid attack pattern");
				break;
			}
		}
	}

	// Token: 0x06002AA5 RID: 10917 RVA: 0x00029CF6 File Offset: 0x00027EF6
	public void EndAttack()
	{
		this.isAttacking = false;
	}

	// Token: 0x06002AA6 RID: 10918 RVA: 0x00029CFF File Offset: 0x00027EFF
	private void ReturnCommand()
	{
		if (this.returnCommandActivated)
		{
			return;
		}
		this.returnCommandActivated = true;
	}

	// Token: 0x06002AA7 RID: 10919 RVA: 0x00126A6C File Offset: 0x00124C6C
	private void ReturnExecute()
	{
		if (this.boss_activated)
		{
			return;
		}
		if (this.movable.GetCurrentNode() == this.model.GetRoomNode())
		{
			this.model.Suppressed();
			this.model.suppressReturnTimer = 9f;
		}
		else
		{
			this.model.MoveToNode(this.model.GetRoomNode());
		}
	}

	// Token: 0x06002AA8 RID: 10920 RVA: 0x00126AD8 File Offset: 0x00124CD8
	private void MakeMovement()
	{
		if (this.OtherBirdState == BossBird.OtherBirdState.MOVETOGATE)
		{
			return;
		}
		if (this.isAttacking)
		{
			return;
		}
		if (this.targetAgent == null)
		{
			if (this.oldTargetNode == null)
			{
				MapNode roamingNodeByRandom = MapGraph.instance.GetRoamingNodeByRandom();
				this.oldTargetNode = roamingNodeByRandom;
				this.model.MoveToNode(roamingNodeByRandom);
			}
			else if (this.movable.GetCurrentNode() == this.oldTargetNode)
			{
				MapNode roamingNodeByRandom2 = MapGraph.instance.GetRoamingNodeByRandom();
				this.oldTargetNode = roamingNodeByRandom2;
				this.model.MoveToNode(roamingNodeByRandom2);
			}
			else
			{
				this.model.MoveToNode(this.oldTargetNode);
			}
			return;
		}
		MovableObjectNode movableNode = this.targetAgent.GetMovableNode();
		if (movableNode.InElevator())
		{
			if (this.oldTargetNode != null)
			{
				this.model.MoveToNode(this.oldTargetNode);
				return;
			}
		}
		else if (movableNode.GetCurrentNode() != null)
		{
			this.oldTargetNode = movableNode.GetCurrentNode();
		}
		this.model.MoveToMovable(movableNode);
	}

	// Token: 0x06002AA9 RID: 10921 RVA: 0x00021178 File Offset: 0x0001F378
	private void StopMovement()
	{
		this.movable.StopMoving();
	}

	// Token: 0x06002AAA RID: 10922 RVA: 0x00126BE0 File Offset: 0x00124DE0
	public override SoundEffectPlayer MakeSound(string src)
	{
		SoundEffectPlayer soundEffectPlayer = null;
		string filename;
		if (this.model.metaInfo.soundTable.TryGetValue(src, out filename))
		{
			soundEffectPlayer = SoundEffectPlayer.PlayOnce(filename, this.model.Unit.transform.position);
			if (soundEffectPlayer != null)
			{
				soundEffectPlayer.transform.position = base.Unit.animTarget.gameObject.transform.position;
			}
		}
		if (soundEffectPlayer == null)
		{
			Debug.Log("Error in finidng sound " + src);
		}
		return soundEffectPlayer;
	}

	// Token: 0x06002AAB RID: 10923 RVA: 0x00126C7C File Offset: 0x00124E7C
	public List<AgentModel> CheckNearWorkerEncounting(out List<WorkerModel> nearWorkers)
	{
		List<AgentModel> list = new List<AgentModel>();
		List<WorkerModel> list2 = new List<WorkerModel>();
		if (this.movable.currentPassage == null)
		{
			nearWorkers = list2;
			return list;
		}
		foreach (MovableObjectNode movableObjectNode in this.movable.currentPassage.GetEnteredTargets())
		{
			UnitModel unit = movableObjectNode.GetUnit();
			if (unit is WorkerModel)
			{
				WorkerModel workerModel = unit as WorkerModel;
				if (!workerModel.IsDead())
				{
					if (workerModel is AgentModel)
					{
						if (!(workerModel as AgentModel).activated)
						{
							continue;
						}
						if (!list.Contains(workerModel as AgentModel))
						{
							list.Add(workerModel as AgentModel);
						}
					}
					if (!list2.Contains(workerModel))
					{
						list2.Add(workerModel);
					}
					if (!this.encountered.Contains(workerModel))
					{
						this.encountered.Add(workerModel);
						workerModel.InitialEncounteredCreature(this.model);
					}
				}
			}
		}
		nearWorkers = list2;
		return list;
	}

	// Token: 0x06002AAC RID: 10924 RVA: 0x0001E78C File Offset: 0x0001C98C
	public override void ActivateQliphothCounter()
	{
		base.ActivateQliphothCounter();
		this.Escape();
	}

	// Token: 0x06002AAD RID: 10925 RVA: 0x00126DB4 File Offset: 0x00124FB4
	private void StartMadMode(AgentModel target)
	{
		if (this.madMode)
		{
			return;
		}
		this.targetAgent = target;
		this.madMode = true;
		this.animScript.OnMadStart();
		if (!this.model.HasUnitBuf(UnitBufType.SMALLBIRD_MAD))
		{
			this.model.AddUnitBuf(new SmallBirdMadBuf(this));
		}
	}

	// Token: 0x06002AAE RID: 10926 RVA: 0x00029D14 File Offset: 0x00027F14
	public void MadRelease()
	{
		this.madMode = false;
		this.ReturnCommand();
	}

	// Token: 0x06002AAF RID: 10927 RVA: 0x00126E0C File Offset: 0x0012500C
	public void OnNotice(string notice, params object[] param)
	{
		if (notice == NoticeName.OnAgentPanic)
		{
			if (param.Length > 0 && param[0] is AgentModel)
			{
				this.targetAgent = (param[0] as AgentModel);
				this.SetPattern(SmallBird.EscapePattern.PANIC);
				this.model.SubQliphothCounter();
			}
		}
		else if (notice == NoticeName.OnWorkStart && param.Length > 0 && param[0] is CreatureModel)
		{
			if (param[0] == this.model)
			{
				return;
			}
			float value = UnityEngine.Random.value;
			if (value < 0.2f)
			{
				this.SetPattern(SmallBird.EscapePattern.NORMAL);
				this.model.SubQliphothCounter();
			}
		}
	}

	// Token: 0x06002AB0 RID: 10928 RVA: 0x00126EBC File Offset: 0x001250BC
	public void MakeMoveToGate(MapNode dest)
	{
		this._otherBirdState = BossBird.OtherBirdState.MOVETOGATE;
		if (!base.Unit.gameObject.activeInHierarchy)
		{
			return;
		}
		if (!this.model.IsEscapedOnlyEscape())
		{
			this.model.SetQliphothCounter(0);
			this.Escape();
		}
		this.model.MoveToNode(dest);
		if (this.model.GetCurrentCommand() is MoveCreatureCommand)
		{
			MoveCreatureCommand moveCreatureCommand = this.model.GetCurrentCommand() as MoveCreatureCommand;
			moveCreatureCommand.SetEndCommand(new CreatureCommand.OnCommandEnd(this.OnArrivedAndHide));
		}
		this.model.ForcelyCancelSuppress();
	}

	// Token: 0x06002AB1 RID: 10929 RVA: 0x00029D23 File Offset: 0x00027F23
	public void OnArrivedAndHide()
	{
		Debug.Log("[BossBird]SamllBird Arrived");
		this.boss.OnBirdArrived(this);
		base.Unit.gameObject.SetActive(false);
		this.model.GetMovableNode().SetActive(false);
	}

	// Token: 0x06002AB2 RID: 10930 RVA: 0x00029D5D File Offset: 0x00027F5D
	public void OnBossActivate()
	{
		this.boss_activated = true;
	}

	// Token: 0x06002AB3 RID: 10931 RVA: 0x00126F58 File Offset: 0x00125158
	public void OnBossSuppressed()
	{
		this._otherBirdState = BossBird.OtherBirdState.NORMAL;
		this.boss_activated = false;
		this.model.Suppressed();
		base.Unit.gameObject.SetActive(true);
		this.model.SetMoveAnimState(false);
		this.model.GetMovableNode().SetActive(true);
	}

	// Token: 0x06002AB4 RID: 10932 RVA: 0x00029D66 File Offset: 0x00027F66
	public override bool OnOpenCollectionWindow()
	{
		if (this.boss_activated)
		{
			this.boss.OpenBossBirdCollection();
		}
		return !this.boss_activated;
	}

	// Token: 0x06002AB5 RID: 10933 RVA: 0x00126FAC File Offset: 0x001251AC
	public void SetObserver(bool activate)
	{
		if (activate)
		{
			Notice.instance.Observe(NoticeName.OnAgentPanic, this);
			Notice.instance.Observe(NoticeName.OnWorkStart, this);
		}
		else
		{
			Notice.instance.Remove(NoticeName.OnAgentPanic, this);
			Notice.instance.Remove(NoticeName.OnWorkStart, this);
		}
	}

	// Token: 0x06002AB6 RID: 10934 RVA: 0x0010ACF0 File Offset: 0x00108EF0
	public void MakeExplodeEffect(UnitDirection dir, WorkerModel target, float size)
	{
		ExplodeGutEffect explodeGutEffect = null;
		WorkerUnit workerUnit = target.GetWorkerUnit();
		if (ExplodeGutManager.instance.MakeEffects(workerUnit.gameObject.transform.position, ref explodeGutEffect))
		{
			explodeGutEffect.particleCount = UnityEngine.Random.Range(3, 9);
			explodeGutEffect.ground = target.GetMovableNode().GetCurrentViewPosition().y;
			float num = 0f;
			float num2 = 0f;
			explodeGutEffect.SetEffectSize(size);
			explodeGutEffect.Shoot(ExplodeGutEffect.Directional.DIRECTION, dir);
			if (target.GetMovableNode().GetPassage() != null)
			{
				target.GetMovableNode().GetPassage().GetVerticalRange(ref num, ref num2);
				explodeGutEffect.SetCurrentPassage(target.GetMovableNode().GetPassage());
			}
		}
		workerUnit.gameObject.SetActive(false);
	}

	// Token: 0x06002AB7 RID: 10935 RVA: 0x00029D87 File Offset: 0x00027F87
	public void OnGateSuppressed()
	{
		this.OnBossSuppressed();
	}

	// Token: 0x04002894 RID: 10388
	private const string FactionCode = "11";

	// Token: 0x04002895 RID: 10389
	private const float distanceRange = 1.7f;

	// Token: 0x04002896 RID: 10390
	private const float escapeProb = 0.2f;

	// Token: 0x04002897 RID: 10391
	private const float mentalHealMin = 3f;

	// Token: 0x04002898 RID: 10392
	private const float mentalHealMax = 5f;

	// Token: 0x04002899 RID: 10393
	private const float redDmgMin = 1f;

	// Token: 0x0400289A RID: 10394
	private const float redDmgMax = 1f;

	// Token: 0x0400289B RID: 10395
	private const float madDmgMin = 800f;

	// Token: 0x0400289C RID: 10396
	private const float madDmgMax = 1200f;

	// Token: 0x0400289D RID: 10397
	private const int totalDmgMin = 8;

	// Token: 0x0400289E RID: 10398
	private const int totalDmgMax = 12;

	// Token: 0x0400289F RID: 10399
	private AgentModel targetAgent;

	// Token: 0x040028A0 RID: 10400
	private SmallBird.EscapePattern escapePattern = SmallBird.EscapePattern.NORMAL;

	// Token: 0x040028A1 RID: 10401
	private SmallBird.AttackPattern attackPattern = SmallBird.AttackPattern.NORMAL;

	// Token: 0x040028A2 RID: 10402
	private float currentAttackedDamage;

	// Token: 0x040028A3 RID: 10403
	private bool returnCommandActivated;

	// Token: 0x040028A4 RID: 10404
	private bool isAttacking;

	// Token: 0x040028A5 RID: 10405
	private bool madMode;

	// Token: 0x040028A6 RID: 10406
	private MapNode oldTargetNode;

	// Token: 0x040028A7 RID: 10407
	private List<WorkerModel> encountered = new List<WorkerModel>();

	// Token: 0x040028A8 RID: 10408
	private const float changeTargetFreq = 3f;

	// Token: 0x040028A9 RID: 10409
	private Timer changeTargetTimer = new Timer();

	// Token: 0x040028AA RID: 10410
	private const float elevatorBlockTime = 10f;

	// Token: 0x040028AB RID: 10411
	private Timer elevatorEscapeTimer = new Timer();

	// Token: 0x040028AC RID: 10412
	private bool boss_activated;

	// Token: 0x040028AD RID: 10413
	private BossBird boss;

	// Token: 0x040028AE RID: 10414
	private BossBird.OtherBirdState _otherBirdState;

	// Token: 0x040028AF RID: 10415
	private SmallBirdAnim _animScript;

	// Token: 0x0200048D RID: 1165
	private enum EscapePattern
	{
		// Token: 0x040028B1 RID: 10417
		PANIC,
		// Token: 0x040028B2 RID: 10418
		NORMAL
	}

	// Token: 0x0200048E RID: 1166
	private enum AttackPattern
	{
		// Token: 0x040028B4 RID: 10420
		MENTAL_HEAL,
		// Token: 0x040028B5 RID: 10421
		NORMAL,
		// Token: 0x040028B6 RID: 10422
		MAD
	}
}
