using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003C6 RID: 966
public class BugDawn : BugOrdealCreature
{
	// Token: 0x06001EDC RID: 7900 RVA: 0x00020E5B File Offset: 0x0001F05B
	public BugDawn()
	{
	}

	// Token: 0x17000290 RID: 656
	// (get) Token: 0x06001EDD RID: 7901 RVA: 0x00020E9A File Offset: 0x0001F09A
	private static float beforeteleportTime
	{
		get
		{
			return UnityEngine.Random.Range(0.1f, 0.3f);
		}
	}

	// Token: 0x17000291 RID: 657
	// (get) Token: 0x06001EDE RID: 7902 RVA: 0x00013DB6 File Offset: 0x00011FB6
	private static float teleportDelay
	{
		get
		{
			return UnityEngine.Random.Range(0.5f, 1f);
		}
	}

	// Token: 0x17000292 RID: 658
	// (get) Token: 0x06001EDF RID: 7903 RVA: 0x00020EAB File Offset: 0x0001F0AB
	private static float attackDelay
	{
		get
		{
			return UnityEngine.Random.Range(1.5f, 2f);
		}
	}

	// Token: 0x17000293 RID: 659
	// (get) Token: 0x06001EE0 RID: 7904 RVA: 0x00013DB6 File Offset: 0x00011FB6
	private static float appearanceDelay
	{
		get
		{
			return UnityEngine.Random.Range(0.5f, 1f);
		}
	}

	// Token: 0x17000294 RID: 660
	// (get) Token: 0x06001EE1 RID: 7905 RVA: 0x00020EBC File Offset: 0x0001F0BC
	private static int attackDmg
	{
		get
		{
			return UnityEngine.Random.Range(1, 3);
		}
	}

	// Token: 0x17000295 RID: 661
	// (get) Token: 0x06001EE2 RID: 7906 RVA: 0x00020EBC File Offset: 0x0001F0BC
	private static int appearDmg
	{
		get
		{
			return UnityEngine.Random.Range(1, 3);
		}
	}

	// Token: 0x17000296 RID: 662
	// (get) Token: 0x06001EE3 RID: 7907 RVA: 0x00020EC5 File Offset: 0x0001F0C5
	private static float speed
	{
		get
		{
			return UnityEngine.Random.Range(2.8f, 3.2f);
		}
	}

	// Token: 0x17000297 RID: 663
	// (get) Token: 0x06001EE4 RID: 7908 RVA: 0x00020ED6 File Offset: 0x0001F0D6
	public BugDawnAnim animScript
	{
		get
		{
			if (this._animScript == null)
			{
				this._animScript = (base.Unit.animTarget as BugDawnAnim);
			}
			return this._animScript;
		}
	}

	// Token: 0x06001EE5 RID: 7909 RVA: 0x00020F05 File Offset: 0x0001F105
	public virtual void Init()
	{
		this.animScript.SetScript(this);
		this.model.movementScale = BugDawn.speed;
	}

	// Token: 0x06001EE6 RID: 7910 RVA: 0x000F6030 File Offset: 0x000F4230
	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
		this.Init();
		if (this.animScript.CurrentState() != BugDawn.AnimationState.MADE_BY_SECOND)
		{
			this.animScript.SetAnimation(BugDawn.AnimationState.DIG_OUT);
			this._phase = BugOrdealCreature.BugPhase.Teleporting;
		}
		unit.escapeRisk.text = this._risk.ToString().ToUpper();
		unit.escapeRisk.color = UIColorManager.instance.GetRiskColor(this._risk);
	}

	// Token: 0x06001EE7 RID: 7911 RVA: 0x000F60AC File Offset: 0x000F42AC
	public override bool OnAfterSuppressed()
	{
		this.animScript.SetAnimation(BugDawn.AnimationState.DEAD);
		this.movable.StopMoving();
		this.OnDie();
		GameObject gameObject = Prefab.LoadPrefab("Effect/RandomEvent/HordeOfBugs/Daughter_Dead");
		gameObject.transform.SetParent(this.animScript.gameObject.transform);
		gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 180f));
		return base.OnAfterSuppressed();
	}

	// Token: 0x06001EE8 RID: 7912 RVA: 0x000F6158 File Offset: 0x000F4358
	public override void OnFixedUpdate(CreatureModel creature)
	{
		base.OnFixedUpdate(creature);
		if (this.model.hp <= 0f)
		{
			return;
		}
		this.model.CheckNearWorkerEncounting();
		this.attackDelayTimer.RunTimer();
		if (this._attackNode != null && this.movable.IsMoving() && this._attackNode.GetAttachedPassage() != this.currentPassage && this.movable.GetPassageCheckPrev() != null)
		{
			this.StopMovement();
			this.GetAttackNode();
		}
		if (this.appearanceTimer.RunTimer())
		{
			this._phase = BugOrdealCreature.BugPhase.Teleporting;
			this.animScript.SetAnimation(BugDawn.AnimationState.DIG_OUT);
		}
		if (this.teleportDelayTimer.started)
		{
			this.StopMovement();
			if (this.teleportDelayTimer.RunTimer())
			{
				this.GetAttackNode();
			}
			return;
		}
		switch (this._phase)
		{
		case BugOrdealCreature.BugPhase.ReadyToTeleport:
			this.StopMovement();
			break;
		case BugOrdealCreature.BugPhase.Teleporting:
			this.StopMovement();
			break;
		case BugOrdealCreature.BugPhase.Moving:
			this.ProcessMoving();
			break;
		case BugOrdealCreature.BugPhase.Attack:
			this.ProcessAttack(1f);
			break;
		}
		if (this._phase != BugOrdealCreature.BugPhase.Attack && this.beforeteleportTimer.RunTimer())
		{
			this._phase = BugOrdealCreature.BugPhase.ReadyToTeleport;
			this.StartTeleport();
		}
	}

	// Token: 0x06001EE9 RID: 7913 RVA: 0x000F62B4 File Offset: 0x000F44B4
	public virtual void FindTarget()
	{
		if (this.currentPassage != null)
		{
			UnitModel unitModel = null;
			float num = 10000f;
			foreach (MovableObjectNode movableObjectNode in this.currentPassage.GetEnteredTargets())
			{
				UnitModel unit = movableObjectNode.GetUnit();
				if (this.IsHostile(unit))
				{
					float distance = MovableObjectNode.GetDistance(movableObjectNode, this.movable);
					if (distance < num)
					{
						unitModel = unit;
						num = distance;
					}
				}
			}
			if (unitModel == null)
			{
				return;
			}
			this._currentTarget = unitModel;
		}
	}

	// Token: 0x06001EEA RID: 7914 RVA: 0x000F6364 File Offset: 0x000F4564
	public virtual void ProcessMoving()
	{
		if (this._currentTarget != null && (!this._currentTarget.IsAttackTargetable() || this._currentTarget.GetMovableNode().GetPassage() != this.currentPassage || this._currentTarget.GetMovableNode().GetPassage() == null))
		{
			this.StopMovement();
			this._currentTarget = null;
		}
		if (this._currentTarget == null)
		{
			this.FindTarget();
		}
		if (!this.attackDelayTimer.started)
		{
			if (this._currentTarget != null)
			{
				if (MovableObjectNode.GetDistance(this._currentTarget.GetMovableNode(), this.movable) < 5f)
				{
					this.attackDelayTimer.StartTimer(BugDawn.attackDelay);
					this.StartAttack();
					return;
				}
				if (!this.movable.IsMoving())
				{
					this.movable.MoveToMovableNode(this._currentTarget.GetMovableNode(), false);
				}
			}
			else if (!this.movable.IsMoving())
			{
				this.MakeMovement();
			}
		}
	}

	// Token: 0x06001EEB RID: 7915 RVA: 0x00020F23 File Offset: 0x0001F123
	public override void ReadyToTeleport(PassageObjectModel passage)
	{
		base.ReadyToTeleport(passage);
		if (this.beforeteleportTimer.started)
		{
			return;
		}
		this.beforeteleportTimer.StartTimer(BugDawn.beforeteleportTime);
		this._targetNode = this.NodeSelection(passage);
		this.StopMovement();
	}

	// Token: 0x06001EEC RID: 7916 RVA: 0x00020F60 File Offset: 0x0001F160
	public void StartTeleport()
	{
		this._phase = BugOrdealCreature.BugPhase.Teleporting;
		this.animScript.SetAnimation(BugDawn.AnimationState.DIG_IN);
		this._currentTarget = null;
		this.StopMovement();
	}

	// Token: 0x06001EED RID: 7917 RVA: 0x000F6470 File Offset: 0x000F4670
	public MapNode NodeSelection(PassageObjectModel passage)
	{
		List<MapNode> list = new List<MapNode>(passage.GetNodeList());
		List<MapNode> list2 = new List<MapNode>();
		float num = 0f;
		float num2 = 0f;
		float scaleFactor = passage.scaleFactor;
		passage.GetVerticalRange(ref num, ref num2);
		foreach (MapNode mapNode in list)
		{
			if (mapNode.GetPosition().x - num >= 3f * scaleFactor)
			{
				if (num2 - mapNode.GetPosition().x >= 3f * scaleFactor)
				{
					list2.Add(mapNode);
				}
			}
		}
		if (list2.Count <= 0)
		{
			return null;
		}
		return list2[UnityEngine.Random.Range(0, list2.Count)];
	}

	// Token: 0x06001EEE RID: 7918 RVA: 0x000F6564 File Offset: 0x000F4764
	public void GetAttackNode()
	{
		List<MapNode> list = new List<MapNode>();
		if (this.currentPassage == null)
		{
			Debug.Log("in null passage");
			PassageObjectModel passageCheckPrev = this.movable.GetPassageCheckPrev();
			if (passageCheckPrev != null)
			{
				list.AddRange(passageCheckPrev.GetNodeList());
				if (list.Count <= 0)
				{
					return;
				}
				this._attackNode = list[UnityEngine.Random.Range(0, list.Count)];
			}
			return;
		}
		list.AddRange(this.currentPassage.GetNodeList());
		if (list.Count <= 0)
		{
			return;
		}
		this._attackNode = list[UnityEngine.Random.Range(0, list.Count)];
	}

	// Token: 0x06001EEF RID: 7919 RVA: 0x000F6608 File Offset: 0x000F4808
	public void MakeMovement()
	{
		if (this.currentPassage == null)
		{
			Debug.Log("in null passage");
			PassageObjectModel passageCheckPrev = this.movable.GetPassageCheckPrev();
			if (passageCheckPrev != null)
			{
				List<MapNode> list = new List<MapNode>(passageCheckPrev.GetNodeList());
				if (list.Count <= 0)
				{
					return;
				}
				this._attackNode = list[UnityEngine.Random.Range(0, list.Count)];
				this.model.MoveToNode(this._attackNode);
			}
			return;
		}
		this.GetAttackNode();
		this.model.MoveToNode(this._attackNode);
	}

	// Token: 0x06001EF0 RID: 7920 RVA: 0x00020F82 File Offset: 0x0001F182
	public void StopMovement()
	{
		this.movable.StopMoving();
	}

	// Token: 0x06001EF1 RID: 7921 RVA: 0x000F6698 File Offset: 0x000F4898
	public bool IsHostile(UnitModel u)
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

	// Token: 0x06001EF2 RID: 7922 RVA: 0x000F6704 File Offset: 0x000F4904
	public virtual void OnEndDigIn()
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
		this.appearanceTimer.StartTimer(BugDawn.appearanceDelay);
	}

	// Token: 0x06001EF3 RID: 7923 RVA: 0x00020F8F File Offset: 0x0001F18F
	public virtual void OnEndDigOut()
	{
		this._phase = BugOrdealCreature.BugPhase.Moving;
		this.teleportDelayTimer.StartTimer(BugDawn.teleportDelay);
		this.model.movementScale = BugDawn.speed;
		this.StopMovement();
		this.GiveAppearDmg(2f);
	}

	// Token: 0x06001EF4 RID: 7924 RVA: 0x000F6780 File Offset: 0x000F4980
	public virtual void StartAttack()
	{
		if (this._currentTarget == null)
		{
			return;
		}
		if (this.currentPassage != null && this.currentPassage == this._currentTarget.GetMovableNode().GetPassage())
		{
			this._phase = BugOrdealCreature.BugPhase.Attack;
			this.oldPosX = this.movable.GetCurrentViewPosition().x;
			if (this._currentTarget.GetCurrentViewPosition().x < this.oldPosX)
			{
				this.attackDirection = UnitDirection.LEFT;
			}
			else if (this._currentTarget.GetCurrentViewPosition().x > this.oldPosX)
			{
				this.attackDirection = UnitDirection.RIGHT;
			}
			else
			{
				this.attackDirection = this.movable.GetDirection();
			}
			float distance = MovableObjectNode.GetDistance(this._currentTarget.GetMovableNode(), this.movable);
			this.model.movementScale = 10f * distance / 5f + UnityEngine.Random.Range(0f, 2f);
			this.movable.MoveBy(this.attackDirection, 10f);
			this.animScript.SetAnimation(BugDawn.AnimationState.ATTACK);
			this.damagedUnits.Clear();
		}
	}

	// Token: 0x06001EF5 RID: 7925 RVA: 0x000F68B0 File Offset: 0x000F4AB0
	public virtual void ProcessAttack(float baseRange)
	{
		if (this._currentTarget == null || this._currentTarget.hp <= 0f)
		{
			return;
		}
		if (this.damagedUnits.Contains(this._currentTarget))
		{
			return;
		}
		float x = this.movable.GetCurrentViewPosition().x;
		float x2 = this._currentTarget.GetCurrentViewPosition().x;
		PassageObjectModel passage = this.movable.GetPassage();
		if (passage != null && passage == this._currentTarget.GetMovableNode().currentPassage)
		{
			List<UnitModel> list = new List<UnitModel>();
			float num = baseRange * this.movable.currentScale;
			if (this.oldPosX < x)
			{
				if (this.oldPosX - num < x2 && x2 < x + num)
				{
					list.Add(this._currentTarget);
				}
			}
			else if (x - num < x2 && x2 < this.oldPosX + num)
			{
				list.Add(this._currentTarget);
			}
			foreach (UnitModel unitModel in list)
			{
				unitModel.TakeDamage(this.model, new DamageInfo(RwbpType.R, (float)BugDawn.attackDmg));
				this.damagedUnits.Add(unitModel);
				DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(unitModel, RwbpType.R, this.model);
			}
		}
		this.oldPosX = x;
	}

	// Token: 0x06001EF6 RID: 7926 RVA: 0x00020FC9 File Offset: 0x0001F1C9
	public virtual void OnEndAttack()
	{
		this.StopMovement();
		this.model.movementScale = BugDawn.speed;
		this._phase = BugOrdealCreature.BugPhase.Moving;
	}

	// Token: 0x06001EF7 RID: 7927 RVA: 0x000F6A40 File Offset: 0x000F4C40
	public bool CanRangeInCamera()
	{
		Vector3 position = Camera.main.gameObject.transform.position;
		Vector3 currentViewPosition = this.movable.GetCurrentViewPosition();
		float num = position.x - currentViewPosition.x;
		float num2 = position.y - currentViewPosition.y;
		float num3 = num * num + num2 * num2;
		return num3 <= 30f;
	}

	// Token: 0x06001EF8 RID: 7928 RVA: 0x000F6AA8 File Offset: 0x000F4CA8
	public virtual void GiveAppearDmg(float baseRange)
	{
		if (this.currentPassage == null)
		{
			return;
		}
		List<MovableObjectNode> list = new List<MovableObjectNode>(this.currentPassage.GetEnteredTargets(this.movable));
		float num = baseRange * this.movable.currentScale;
		foreach (MovableObjectNode movableObjectNode in list)
		{
			UnitModel unit = movableObjectNode.GetUnit();
			if (this.IsHostile(unit))
			{
				if (MovableObjectNode.GetDistance(movableObjectNode, this.movable) <= num)
				{
					unit.TakeDamage(this.model, new DamageInfo(RwbpType.R, (float)BugDawn.appearDmg));
					DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(unit, RwbpType.R, this.model);
				}
			}
		}
	}

	// Token: 0x04001F53 RID: 8019
	public Timer beforeteleportTimer = new Timer();

	// Token: 0x04001F54 RID: 8020
	private const float _beforeteleportTimeMax = 0.3f;

	// Token: 0x04001F55 RID: 8021
	private const float _beforeteleportTimeMin = 0.1f;

	// Token: 0x04001F56 RID: 8022
	public Timer teleportDelayTimer = new Timer();

	// Token: 0x04001F57 RID: 8023
	private const float _teleportDelayMax = 1f;

	// Token: 0x04001F58 RID: 8024
	private const float _teleportDelayMin = 0.5f;

	// Token: 0x04001F59 RID: 8025
	public Timer attackDelayTimer = new Timer();

	// Token: 0x04001F5A RID: 8026
	private const float _attackDelayMax = 2f;

	// Token: 0x04001F5B RID: 8027
	private const float _attackDelayMin = 1.5f;

	// Token: 0x04001F5C RID: 8028
	public Timer appearanceTimer = new Timer();

	// Token: 0x04001F5D RID: 8029
	private const float _appearanceDelayMax = 1f;

	// Token: 0x04001F5E RID: 8030
	private const float _appearanceDelayMin = 0.5f;

	// Token: 0x04001F5F RID: 8031
	private const int _attackDmgMax = 2;

	// Token: 0x04001F60 RID: 8032
	private const int _attackDmgMin = 1;

	// Token: 0x04001F61 RID: 8033
	private const int _appearDmgMax = 2;

	// Token: 0x04001F62 RID: 8034
	private const int _appearDmgMin = 1;

	// Token: 0x04001F63 RID: 8035
	private const float _speedMin = 2.8f;

	// Token: 0x04001F64 RID: 8036
	private const float _speedMax = 3.2f;

	// Token: 0x04001F65 RID: 8037
	private const float _attackRange = 5f;

	// Token: 0x04001F66 RID: 8038
	private const float _soundDistDobule = 30f;

	// Token: 0x04001F67 RID: 8039
	private const float _sideNodeRemoveRange = 3f;

	// Token: 0x04001F68 RID: 8040
	public float oldPosX;

	// Token: 0x04001F69 RID: 8041
	public UnitDirection attackDirection;

	// Token: 0x04001F6A RID: 8042
	public List<UnitModel> damagedUnits = new List<UnitModel>();

	// Token: 0x04001F6B RID: 8043
	public UnitModel _currentTarget;

	// Token: 0x04001F6C RID: 8044
	public MapNode _targetNode;

	// Token: 0x04001F6D RID: 8045
	public MapNode _attackNode;

	// Token: 0x04001F6E RID: 8046
	private BugDawnAnim _animScript;

	// Token: 0x020003C7 RID: 967
	public enum AnimationState
	{
		// Token: 0x04001F70 RID: 8048
		MOVE,
		// Token: 0x04001F71 RID: 8049
		DIG_IN,
		// Token: 0x04001F72 RID: 8050
		DIG_OUT,
		// Token: 0x04001F73 RID: 8051
		DEAD,
		// Token: 0x04001F74 RID: 8052
		ATTACK,
		// Token: 0x04001F75 RID: 8053
		MADE_BY_SECOND
	}
}
