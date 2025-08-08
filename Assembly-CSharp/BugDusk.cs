/*
private PassageObjectModel GetPassage() // 
*/
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020003C8 RID: 968
public class BugDusk : BugOrdealCreature
{
	// Token: 0x06001EF9 RID: 7929 RVA: 0x000F6B80 File Offset: 0x000F4D80
	public BugDusk()
	{
	}

	// Token: 0x17000298 RID: 664
	// (get) Token: 0x06001EFA RID: 7930 RVA: 0x00020E9A File Offset: 0x0001F09A
	private static float beforeteleportTime
	{
		get
		{
			return UnityEngine.Random.Range(0.1f, 0.3f);
		}
	}

	// Token: 0x17000299 RID: 665
	// (get) Token: 0x06001EFB RID: 7931 RVA: 0x00013DB6 File Offset: 0x00011FB6
	private static float teleportDelay
	{
		get
		{
			return UnityEngine.Random.Range(0.5f, 1f);
		}
	}

	// Token: 0x1700029A RID: 666
	// (get) Token: 0x06001EFC RID: 7932 RVA: 0x00013DB6 File Offset: 0x00011FB6
	private static float appearanceDelay
	{
		get
		{
			return UnityEngine.Random.Range(0.5f, 1f);
		}
	}

	// Token: 0x1700029B RID: 667
	// (get) Token: 0x06001EFD RID: 7933 RVA: 0x00020FE8 File Offset: 0x0001F1E8
	private static float slowDownTime
	{
		get
		{
			return UnityEngine.Random.Range(1f, 2f);
		}
	}

	// Token: 0x1700029C RID: 668
	// (get) Token: 0x06001EFE RID: 7934 RVA: 0x00020EAB File Offset: 0x0001F0AB
	private static float attackDelay
	{
		get
		{
			return UnityEngine.Random.Range(1.5f, 2f);
		}
	}

	// Token: 0x1700029D RID: 669
	// (get) Token: 0x06001EFF RID: 7935 RVA: 0x0001FBFC File Offset: 0x0001DDFC
	private static float spawnDelay
	{
		get
		{
			return UnityEngine.Random.Range(3f, 5f);
		}
	}

	// Token: 0x1700029E RID: 670
	// (get) Token: 0x06001F00 RID: 7936 RVA: 0x00020FF9 File Offset: 0x0001F1F9
	private static int attackDmg
	{
		get
		{
			return UnityEngine.Random.Range(50, 71);
		}
	}

	// Token: 0x1700029F RID: 671
	// (get) Token: 0x06001F01 RID: 7937 RVA: 0x00021004 File Offset: 0x0001F204
	private static int appearDmg
	{
		get
		{
			return UnityEngine.Random.Range(30, 41);
		}
	}

	// Token: 0x170002A0 RID: 672
	// (get) Token: 0x06001F02 RID: 7938 RVA: 0x0002100F File Offset: 0x0001F20F
	public BugDuskAnim animScript
	{
		get
		{
			if (this._animScript == null)
			{
				this._animScript = (base.Unit.animTarget as BugDuskAnim);
			}
			return this._animScript;
		}
	}

	// Token: 0x06001F03 RID: 7939 RVA: 0x0002103E File Offset: 0x0001F23E
	public void Init()
	{
		this.animScript.SetScript(this);
	}

	// Token: 0x06001F04 RID: 7940 RVA: 0x000F6BE0 File Offset: 0x000F4DE0
	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
		this.Init();
		if (this.animScript.CurrentState() != BugDusk.AnimationState.MADE_BY_THIRD)
		{
			this.animScript.SetAnimation(BugDusk.AnimationState.APPEAR);
			this._phase = BugOrdealCreature.BugPhase.Teleporting;
		}
		this.spawnDelayTimer.StartTimer(BugDusk.spawnDelay);
	}

	// Token: 0x06001F05 RID: 7941 RVA: 0x0002104C File Offset: 0x0001F24C
	public override void OnTakeDamage(UnitModel actor, DamageInfo dmg, float value)
	{
		base.OnTakeDamage(actor, dmg, value);
		if (value >= (float)this.model.maxHp * 0.05f)
		{
			this.SlowDown();
		}
	}

	// Token: 0x06001F06 RID: 7942 RVA: 0x00021075 File Offset: 0x0001F275
	private void SlowDown()
	{
		this.slowDownTimer.StartTimer(BugDusk.slowDownTime);
		this.model.movementScale = 0.1f;
	}

	// Token: 0x06001F07 RID: 7943 RVA: 0x00021097 File Offset: 0x0001F297
	private void RecoverSpeed()
	{
		this.model.movementScale = 1f;
	}

	// Token: 0x06001F08 RID: 7944 RVA: 0x000F6A40 File Offset: 0x000F4C40
	public bool CanRangeInCamera()
	{
		Vector3 position = Camera.main.gameObject.transform.position;
		Vector3 currentViewPosition = this.movable.GetCurrentViewPosition();
		float num = position.x - currentViewPosition.x;
		float num2 = position.y - currentViewPosition.y;
		float num3 = num * num + num2 * num2;
		return num3 <= 30f;
	}

	// Token: 0x06001F09 RID: 7945 RVA: 0x000F6C30 File Offset: 0x000F4E30
	public void CreateDaughter(int count)
	{
		if (this._spawnNode == null)
		{
			return;
		}
		BugOrdealCreature bugOrdealCreature;
		if (count % 2 == 1)
		{
			bugOrdealCreature = this._ordealScript.MakeOrdealCreature(OrdealLevel.DAWN, this._spawnNode, this._midnight, new UnitDirection[]
			{
				this.movable.GetDirection()
			});
		}
		else if (this.movable.GetDirection() == UnitDirection.LEFT)
		{
			bugOrdealCreature = this._ordealScript.MakeOrdealCreature(OrdealLevel.DAWN, this._spawnNode, this._midnight, new UnitDirection[]
			{
				UnitDirection.RIGHT
			});
		}
		else
		{
			bugOrdealCreature = this._ordealScript.MakeOrdealCreature(OrdealLevel.DAWN, this._spawnNode, this._midnight, new UnitDirection[1]);
		}
		BugDawn bugDawn = bugOrdealCreature as BugDawn;
		bugDawn.Init();
		bugDawn.animScript.SetAnimation(BugDawn.AnimationState.MADE_BY_SECOND);
		this._childs.Add(bugDawn);
	}

	// Token: 0x06001F0A RID: 7946 RVA: 0x000210A9 File Offset: 0x0001F2A9
	public override bool OnAfterSuppressed()
	{
		this.OnDie();
		return base.OnAfterSuppressed();
	}

	// Token: 0x06001F0B RID: 7947 RVA: 0x000F6D00 File Offset: 0x000F4F00
	public override void OnFixedUpdate(CreatureModel creature)
	{
		base.OnFixedUpdate(creature);
		if (this.model.hp <= 0f)
		{
			return;
		}
		this.model.CheckNearWorkerEncounting();
		this.attackDelayTimer.RunTimer();
		this.spawnDelayTimer.RunTimer();
		if (this.appearanceTimer.RunTimer())
		{
			this._phase = BugOrdealCreature.BugPhase.Teleporting;
			this.animScript.SetAnimation(BugDusk.AnimationState.APPEAR);
		}
		if (this.slowDownTimer.RunTimer())
		{
			this.RecoverSpeed();
		}
		for (int i = this._childs.Count - 1; i >= 0; i--)
		{
			if (this._childs[i].model.hp <= 0f)
			{
				this._childs.RemoveAt(i);
			}
		}
		if (this.teleportDelayTimer.started)
		{
			this.teleportDelayTimer.RunTimer();
			return;
		}
		switch (this._phase)
		{
		case BugOrdealCreature.BugPhase.Teleporting:
			this.StopMovement();
			break;
		case BugOrdealCreature.BugPhase.Moving:
			this.ProcessMoving();
			break;
		case BugOrdealCreature.BugPhase.Attack:
			this.ProcessAttack();
			break;
		case BugOrdealCreature.BugPhase.Spawning:
			this.StopMovement();
			break;
		}
		if (this.beforeteleportTimer.RunTimer())
		{
			this.StartTeleport();
		}
	}

	// Token: 0x06001F0C RID: 7948 RVA: 0x000210B7 File Offset: 0x0001F2B7
	private void StartSpawn()
	{
		this.canSpawn = false;
		this._phase = BugOrdealCreature.BugPhase.Spawning;
		this.animScript.SetAnimation(BugDusk.AnimationState.SPAWN);
	}

	// Token: 0x06001F0D RID: 7949 RVA: 0x000F6E54 File Offset: 0x000F5054
	private List<UnitModel> GetTarget(float range, bool needDist = true)
	{
		List<UnitModel> list = new List<UnitModel>();
		if (this.currentPassage != null)
		{
			foreach (MovableObjectNode movableObjectNode in this.currentPassage.GetEnteredTargets())
			{
				UnitModel unit = movableObjectNode.GetUnit();
				if (this.IsHostile(unit))
				{
					float distance = this.GetDistance(movableObjectNode);
					if (distance <= range)
					{
						if (needDist)
						{
							float x = this.movable.GetCurrentViewPosition().x;
							float x2 = movableObjectNode.GetCurrentViewPosition().x;
							UnitDirection direction = this.movable.GetDirection();
							if (direction == UnitDirection.RIGHT && x > x2)
							{
								continue;
							}
							if (direction == UnitDirection.LEFT && x < x2)
							{
								continue;
							}
						}
						list.Add(unit);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06001F0E RID: 7950 RVA: 0x000F6F54 File Offset: 0x000F5154
	private void ProcessMoving()
	{
		if (this.GetTarget(3.5f, true).Count > 0)
		{
			if (!this.attackDelayTimer.started)
			{
				this.StartAttack();
				return;
			}
		}
		else if (!this.movable.IsMoving())
		{
			this.MakeMovement();
		}
		if (this._childs.Count <= 2 && !this.spawnDelayTimer.started)
		{
			this.canSpawn = true;
		}
		if (this.canSpawn && this.movable.currentNode != null && this.currentPassage.type == PassageType.HORIZONTAL)
		{
			this._spawnNode = this.movable.currentNode;
			this.StartSpawn();
		}
	}

	// Token: 0x06001F0F RID: 7951 RVA: 0x000210D3 File Offset: 0x0001F2D3
	private void ProcessAttack()
	{
		this.StopMovement();
	}

	// Token: 0x06001F10 RID: 7952 RVA: 0x000F7018 File Offset: 0x000F5218
	public override void ReadyToTeleport(PassageObjectModel passage)
	{
		base.ReadyToTeleport(passage);
		if (this.beforeteleportTimer.started)
		{
			return;
		}
		foreach (BugDawn bugDawn in this._childs)
		{
			bugDawn.ReadyToTeleport(passage);
		}
		this.beforeteleportTimer.StartTimer(BugDusk.beforeteleportTime);
		this.NodeSelection(passage);
	}

	// Token: 0x06001F11 RID: 7953 RVA: 0x000210DB File Offset: 0x0001F2DB
	public void StartTeleport()
	{
		this._phase = BugOrdealCreature.BugPhase.Teleporting;
		this.animScript.SetAnimation(BugDusk.AnimationState.DISAPPEAR);
		this.StopMovement();
	}

	// Token: 0x06001F12 RID: 7954 RVA: 0x000F70A4 File Offset: 0x000F52A4
	private void NodeSelection(PassageObjectModel passage)
	{
		List<MapNode> list = new List<MapNode>(passage.GetNodeList());
		List<MapNode> list2 = list;
		if (BugDusk.cache0 == null)
		{
			BugDusk.cache0 = new Comparison<MapNode>(MapNode.CompareByX);
		}
		list2.Sort(BugDusk.cache0);
		if (list.Count <= 0)
		{
			Debug.Log("nodeless");
			return;
		}
		if (this.Prob(50))
		{
			this.movable.SetDirection(UnitDirection.LEFT);
			this.SetAttackNode(list[0]);
			this._targetNode = list[list.Count - 1];
		}
		else
		{
			this.movable.SetDirection(UnitDirection.RIGHT);
			this.SetAttackNode(list[list.Count - 1]);
			this._targetNode = list[0];
		}
	}

	// Token: 0x06001F13 RID: 7955 RVA: 0x000210F6 File Offset: 0x0001F2F6
	public void SetAttackNode(MapNode node)
	{
		this._attackNode = node;
	}

	// Token: 0x06001F14 RID: 7956 RVA: 0x000F7164 File Offset: 0x000F5364
	private void MakeMovement()
	{
		if (this._attackNode == null)
		{
			return;
		}
		if (this._attackNode.GetAttachedPassage() != this.currentPassage)
		{
			return;
		}
		this.model.MoveToNode(this._attackNode);
		CreatureCommand creatureCurrentCmd = this.model.GetCreatureCurrentCmd();
		creatureCurrentCmd.SetEndCommand(new CreatureCommand.OnCommandEnd(this.OnArrive));
	}

	// Token: 0x06001F15 RID: 7957 RVA: 0x000F71C4 File Offset: 0x000F53C4
	public void OnArrive()
	{
		if (this.beforeteleportTimer.started)
		{
			return;
		}
		if (this.GetPassage() == null)
		{
			Debug.Log("nowhere to go");
			return;
		}
		this.ReadyToTeleport(this.GetPassage());
	}

	// Token: 0x06001F16 RID: 7958 RVA: 0x00020F82 File Offset: 0x0001F182
	private void StopMovement()
	{
		this.movable.StopMoving();
	}

	// Token: 0x06001F17 RID: 7959 RVA: 0x000F6698 File Offset: 0x000F4898
	private bool IsHostile(UnitModel u)
	{
		if (u.hp <= 0f)
		{
			return false;
		}
		if (!u.IsAttackTargetable())
		{
			return false;
		}
		if (u is WorkerModel)
		{
			return true;
		}
		if (u is CreatureModel)
		{
			return !((u as CreatureModel).script is BugOrdealCreature);
		}
		return u is RabbitModel;
	}

	// Token: 0x06001F18 RID: 7960 RVA: 0x000F7208 File Offset: 0x000F5408
	public void OnEndDigOut()
	{
		this._phase = BugOrdealCreature.BugPhase.Moving;
		this.teleportDelayTimer.StartTimer(BugDusk.teleportDelay);
		this.RecoverSpeed();
		List<UnitModel> list = new List<UnitModel>(this.GetTarget(4f, false));
		foreach (UnitModel unitModel in list)
		{
			unitModel.TakeDamage(this.model, new DamageInfo(RwbpType.R, (float)BugDusk.appearDmg));
			DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(unitModel, RwbpType.R, this.model);
		}
	}

	// Token: 0x06001F19 RID: 7961 RVA: 0x000210FF File Offset: 0x0001F2FF
	private void StartAttack()
	{
		if (this.currentPassage != null)
		{
			this._phase = BugOrdealCreature.BugPhase.Attack;
			this.StopMovement();
			this.animScript.SetAnimation(BugDusk.AnimationState.ATTACK);
		}
	}

	// Token: 0x06001F1A RID: 7962 RVA: 0x00021125 File Offset: 0x0001F325
	public void OnEndAttack()
	{
		this.attackDelayTimer.StartTimer(BugDusk.attackDelay);
		this._phase = BugOrdealCreature.BugPhase.Moving;
	}

	// Token: 0x06001F1B RID: 7963 RVA: 0x0002113E File Offset: 0x0001F33E
	public void OnEndSpawn()
	{
		this._phase = BugOrdealCreature.BugPhase.Moving;
	}

	// Token: 0x06001F1C RID: 7964 RVA: 0x000F72B0 File Offset: 0x000F54B0
	public void OnDisappear()
	{
		if (this._targetNode == null)
		{
			Debug.Log("Teleport failed");
			return;
		}
		this.movable.SetCurrentNode(this._targetNode);
		Sefira sefira = SefiraManager.instance.GetSefira(this._targetNode.GetAttachedPassage().GetSefiraName());
		this.model.sefira = sefira;
		this.model.sefiraNum = sefira.indexString;
		this.appearanceTimer.StartTimer(BugDusk.appearanceDelay);
	}

	// Token: 0x06001F1D RID: 7965 RVA: 0x000F732C File Offset: 0x000F552C
	private PassageObjectModel GetPassage()
	{ // <Mod> return the current passage if none are available
		Sefira[] openedAreaList = PlayerModel.instance.GetOpenedAreaList();
		List<PassageObjectModel> list = new List<PassageObjectModel>();
		for (int i = 0; i < openedAreaList.Length; i++)
		{
			foreach (PassageObjectModel passageObjectModel in openedAreaList[i].passageList)
			{
				if (passageObjectModel.isActivate)
				{
					if (passageObjectModel.GetPassageType() == PassageType.HORIZONTAL)
					{
						if (!(SefiraMapLayer.instance.GetPassageObject(passageObjectModel) == null))
						{
							if (SefiraMapLayer.instance.GetPassageObject(passageObjectModel).type != SpaceObjectType.HUBSHORT)
							{
								if (SefiraMapLayer.instance.GetPassageObject(passageObjectModel).type != SpaceObjectType.HUBLONG)
								{
									if (SefiraMapLayer.instance.GetPassageObject(passageObjectModel).type != SpaceObjectType.NONE)
									{
										if (passageObjectModel != this.currentPassage)
										{
											list.Add(passageObjectModel);
										}
									}
								}
							}
						}
					}
				}
			}
		}
		if (list.Count <= 0)
		{
			return currentPassage;
		}
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	// Token: 0x06001F1E RID: 7966 RVA: 0x000F7470 File Offset: 0x000F5670
	public void GiveAttackDamage()
	{
		List<UnitModel> list = new List<UnitModel>(this.GetTarget(4f, true));
		foreach (UnitModel unitModel in list)
		{
			unitModel.TakeDamage(this.model, new DamageInfo(RwbpType.R, (float)BugDusk.attackDmg));
			DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(unitModel, RwbpType.R, this.model);
		}
	}

	// Token: 0x06001F1F RID: 7967 RVA: 0x000F74F8 File Offset: 0x000F56F8
	private float GetDistance(MovableObjectNode mov)
	{
		float x = this.movable.GetCurrentViewPosition().x;
		float x2 = mov.GetCurrentViewPosition().x;
		float currentScale = this.movable.currentScale;
		float num = Math.Abs(x - x2);
		return num / currentScale;
	}

	// Token: 0x04001F76 RID: 8054
	private Timer beforeteleportTimer = new Timer();

	// Token: 0x04001F77 RID: 8055
	private const float _beforeteleportTimeMax = 0.3f;

	// Token: 0x04001F78 RID: 8056
	private const float _beforeteleportTimeMin = 0.1f;

	// Token: 0x04001F79 RID: 8057
	private Timer teleportDelayTimer = new Timer();

	// Token: 0x04001F7A RID: 8058
	private const float _teleportDelayMax = 1f;

	// Token: 0x04001F7B RID: 8059
	private const float _teleportDelayMin = 0.5f;

	// Token: 0x04001F7C RID: 8060
	private Timer appearanceTimer = new Timer();

	// Token: 0x04001F7D RID: 8061
	private const float _appearanceDelayMax = 1f;

	// Token: 0x04001F7E RID: 8062
	private const float _appearanceDelayMin = 0.5f;

	// Token: 0x04001F7F RID: 8063
	private Timer slowDownTimer = new Timer();

	// Token: 0x04001F80 RID: 8064
	private const float _slowDownTimeMax = 2f;

	// Token: 0x04001F81 RID: 8065
	private const float _slowDownTimeMin = 1f;

	// Token: 0x04001F82 RID: 8066
	private Timer attackDelayTimer = new Timer();

	// Token: 0x04001F83 RID: 8067
	private const float _attackDelayMax = 2f;

	// Token: 0x04001F84 RID: 8068
	private const float _attackDelayMin = 1.5f;

	// Token: 0x04001F85 RID: 8069
	private Timer spawnDelayTimer = new Timer();

	// Token: 0x04001F86 RID: 8070
	private const float _spawnDelayMax = 5f;

	// Token: 0x04001F87 RID: 8071
	private const float _spawnDelayMin = 3f;

	// Token: 0x04001F88 RID: 8072
	private const int _attackDmgMax = 70;

	// Token: 0x04001F89 RID: 8073
	private const int _attackDmgMin = 50;

	// Token: 0x04001F8A RID: 8074
	private const int _appearDmgMax = 40;

	// Token: 0x04001F8B RID: 8075
	private const int _appearDmgMin = 30;

	// Token: 0x04001F8C RID: 8076
	private const float _attackDmgRangeMax = 4f;

	// Token: 0x04001F8D RID: 8077
	private const float _attackRange = 3.5f;

	// Token: 0x04001F8E RID: 8078
	private const float _appearDmgRange = 4f;

	// Token: 0x04001F8F RID: 8079
	private const float _defaultSpeedMul = 1f;

	// Token: 0x04001F90 RID: 8080
	private const float _slowDownRatio = 0.1f;

	// Token: 0x04001F91 RID: 8081
	private const float _slowDownHpCondition = 0.05f;

	// Token: 0x04001F92 RID: 8082
	private const float _soundDistDobule = 30f;

	// Token: 0x04001F93 RID: 8083
	private List<BugDawn> _childs = new List<BugDawn>();

	// Token: 0x04001F94 RID: 8084
	private MapNode _targetNode;

	// Token: 0x04001F95 RID: 8085
	private MapNode _attackNode;

	// Token: 0x04001F96 RID: 8086
	private MapNode _spawnNode;

	// Token: 0x04001F97 RID: 8087
	private bool canSpawn;

	// Token: 0x04001F98 RID: 8088
	private BugDuskAnim _animScript;

	// Token: 0x04001F99 RID: 8089
	[CompilerGenerated]
	private static Comparison<MapNode> cache0;

	// Token: 0x020003C9 RID: 969
	public enum AnimationState
	{
		// Token: 0x04001F9B RID: 8091
		MOVE,
		// Token: 0x04001F9C RID: 8092
		APPEAR,
		// Token: 0x04001F9D RID: 8093
		DISAPPEAR,
		// Token: 0x04001F9E RID: 8094
		SPAWN,
		// Token: 0x04001F9F RID: 8095
		ATTACK,
		// Token: 0x04001FA0 RID: 8096
		DEAD,
		// Token: 0x04001FA1 RID: 8097
		MADE_BY_THIRD
	}
}
