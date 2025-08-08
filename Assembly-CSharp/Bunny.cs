using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020003CD RID: 973
public class Bunny : CreatureBase
{
	// Token: 0x06001F52 RID: 8018 RVA: 0x000215B8 File Offset: 0x0001F7B8
	public Bunny()
	{
	}

	// Token: 0x170002A7 RID: 679
	// (get) Token: 0x06001F53 RID: 8019 RVA: 0x000213D8 File Offset: 0x0001F5D8
	private static int _damage
	{
		get
		{
			return UnityEngine.Random.Range(500, 501);
		}
	}

	// Token: 0x170002A8 RID: 680
	// (get) Token: 0x06001F54 RID: 8020 RVA: 0x000215CB File Offset: 0x0001F7CB
	public BunnyAnim animScript
	{
		get
		{
			if (this._animScript == null)
			{
				this._animScript = (base.Unit.animTarget as BunnyAnim);
			}
			return this._animScript;
		}
	}

	// Token: 0x06001F55 RID: 8021 RVA: 0x000215FA File Offset: 0x0001F7FA
	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
		this.animScript.SetScript(this);
		this.ParamInit();
	}

	// Token: 0x06001F56 RID: 8022 RVA: 0x00021615 File Offset: 0x0001F815
	public override void ParamInit()
	{
		base.ParamInit();
		this.attackDelayTimer.StopTimer();
		this.animScript.Init();
		this.model.ResetQliphothCounter();
	}

	// Token: 0x06001F57 RID: 8023 RVA: 0x00020BF7 File Offset: 0x0001EDF7
	public override void OnStageStart()
	{
		base.OnStageStart();
		this.ParamInit();
	}

	// Token: 0x06001F58 RID: 8024 RVA: 0x0002163E File Offset: 0x0001F83E
	public override void OnReleaseWork(UseSkill skill)
	{
		base.OnReleaseWork(skill);
		if (this.model.GetFeelingState() == CreatureFeelingState.BAD)
		{
			this.model.SubQliphothCounter();
		}
		if (skill.elapsedTime <= 40f)
		{
			this.model.SubQliphothCounter();
		}
	}

	// Token: 0x06001F59 RID: 8025 RVA: 0x0001F4D9 File Offset: 0x0001D6D9
	public override void OnReturn()
	{
		base.OnReturn();
		this.ParamInit();
	}

	// Token: 0x06001F5A RID: 8026 RVA: 0x000FAA48 File Offset: 0x000F8C48
	public override void UniqueEscape()
	{
		base.UniqueEscape();
		if (this.attackDelayTimer.started)
		{
			this.attackDelayTimer.RunTimer();
			return;
		}
		if (this.animScript.IsAttacking())
		{
			return;
		}
		List<UnitModel> targets = this.GetTargets(3.1f);
		if (targets.Count > 0)
		{
			this.AttackStart();
		}
	}

	// Token: 0x06001F5B RID: 8027 RVA: 0x000044EF File Offset: 0x000026EF
	public override bool IsAutoSuppressable()
	{
		return false;
	}

	// Token: 0x06001F5C RID: 8028 RVA: 0x000044EF File Offset: 0x000026EF
	public override bool IsSensoredInPassage()
	{
		return false;
	}

	// Token: 0x06001F5D RID: 8029 RVA: 0x000044EF File Offset: 0x000026EF
	public override bool IsIndirectSuppressable()
	{
		return false;
	}

	// Token: 0x06001F5E RID: 8030 RVA: 0x000044EF File Offset: 0x000026EF
	public override bool HasEscapeUI()
	{
		return false;
	}

	// Token: 0x06001F5F RID: 8031 RVA: 0x0001E7A5 File Offset: 0x0001C9A5
	public override void ActivateQliphothCounter()
	{
		base.ActivateQliphothCounter();
		this.Escape();
	}

	// Token: 0x06001F60 RID: 8032 RVA: 0x0002167E File Offset: 0x0001F87E
	public override void Escape()
	{
		base.Escape();
		this.animScript.OnEscape();
		this.model.Escape();
	}

	// Token: 0x06001F61 RID: 8033 RVA: 0x0002169C File Offset: 0x0001F89C
	public void OnEscapeSuccess()
	{
		this.Teleport();
	}

	// Token: 0x06001F62 RID: 8034 RVA: 0x000FAAA8 File Offset: 0x000F8CA8
	private void Teleport()
	{
		MapNode mapNode = this.NodeSelection();
		if (mapNode == null)
		{
			Debug.Log("teleport failed");
			return;
		}
		this.model.SetCurrentNode(mapNode);
		Sefira sefira = SefiraManager.instance.GetSefira(mapNode.GetAttachedPassage().GetSefiraName());
		this.model.sefira = sefira;
		this.model.sefiraNum = sefira.indexString;
	}

	// Token: 0x06001F63 RID: 8035 RVA: 0x000FAB0C File Offset: 0x000F8D0C
	private MapNode NodeSelection()
	{
		List<MapNode> nodes = this.GetNodes();
		Debug.Log("target node count : " + nodes.Count);
		return nodes[UnityEngine.Random.Range(0, nodes.Count)];
	}

	// Token: 0x06001F64 RID: 8036 RVA: 0x000FAB4C File Offset: 0x000F8D4C
	private List<MapNode> GetNodes()
	{
		List<MapNode> list = new List<MapNode>();
		foreach (PassageObjectModel passageObjectModel in this.GetPassages())
		{
			List<MapNode> list2 = passageObjectModel.GetNodeList().ToList<MapNode>();
			float num = 0f;
			float num2 = 0f;
			float scaleFactor = passageObjectModel.scaleFactor;
			passageObjectModel.GetVerticalRange(ref num, ref num2);
			foreach (MapNode mapNode in list2)
			{
				if (mapNode.GetPosition().x - num >= 4f * scaleFactor)
				{
					if (num2 - mapNode.GetPosition().x >= 4f * scaleFactor)
					{
						list.Add(mapNode);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06001F65 RID: 8037 RVA: 0x000FAC68 File Offset: 0x000F8E68
	private List<PassageObjectModel> GetPassages()
	{
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
		return list;
	}

	// Token: 0x06001F66 RID: 8038 RVA: 0x000216A4 File Offset: 0x0001F8A4
	private void AttackStart()
	{
		this.animScript.OnAttackStart();
	}

	// Token: 0x06001F67 RID: 8039 RVA: 0x000216B1 File Offset: 0x0001F8B1
	public void OnAttackEnd()
	{
		this.attackDelayTimer.StartTimer(2f);
	}

	// Token: 0x06001F68 RID: 8040 RVA: 0x000FAD58 File Offset: 0x000F8F58
	public void OnAttackDamageTimeCalled()
	{
		List<UnitModel> targets = this.GetTargets(4f);
		foreach (UnitModel unitModel in targets)
		{
			unitModel.TakeDamage(this.model, new DamageInfo(RwbpType.R, (float)Bunny._damage));
			DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(unitModel, RwbpType.R, this.model);
			WorkerModel workerModel = unitModel as WorkerModel;
			if (workerModel != null)
			{
				if (workerModel.IsDead())
				{
					float num = 1f;
					if (this.currentPassage != null)
					{
						num = this.currentPassage.scaleFactor;
					}
					this.MakeExplodeEffect(workerModel, 1f * num);
				}
			}
		}
	}

	// Token: 0x06001F69 RID: 8041 RVA: 0x000FAE2C File Offset: 0x000F902C
	private List<UnitModel> GetTargets(float range)
	{
		List<UnitModel> list = new List<UnitModel>();
		if (this.currentPassage != null)
		{
			foreach (MovableObjectNode movableObjectNode in this.currentPassage.GetEnteredTargets(this.movable))
			{
				UnitModel unit = movableObjectNode.GetUnit();
				if (unit.hp > 0f)
				{
					if (this.IsInRange(unit, range))
					{
						list.Add(unit);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06001F6A RID: 8042 RVA: 0x000216C3 File Offset: 0x0001F8C3
	private bool IsInRange(UnitModel target, float range)
	{
		return this.GetDist(target) <= range;
	}

	// Token: 0x06001F6B RID: 8043 RVA: 0x000FAEB0 File Offset: 0x000F90B0
	private float GetDist(UnitModel target)
	{
		float x = target.GetCurrentViewPosition().x;
		float x2 = this.model.GetCurrentViewPosition().x;
		float num = Math.Abs(x - x2);
		float num2 = 1f;
		if (this.currentPassage != null)
		{
			num2 = this.currentPassage.scaleFactor;
		}
		return num / num2;
	}

	// Token: 0x06001F6C RID: 8044 RVA: 0x000FAF10 File Offset: 0x000F9110
	public void MakeExplodeEffect(WorkerModel target, float size)
	{
		ExplodeGutEffect explodeGutEffect = null;
		WorkerUnit workerUnit = target.GetWorkerUnit();
		if (workerUnit == null)
		{
			return;
		}
		if (ExplodeGutManager.instance.MakeEffects(workerUnit.gameObject.transform.position, ref explodeGutEffect))
		{
			explodeGutEffect.particleCount = UnityEngine.Random.Range(3, 9);
			explodeGutEffect.ground = target.GetMovableNode().GetCurrentViewPosition().y;
			float num = 0f;
			float num2 = 0f;
			explodeGutEffect.SetEffectSize(size);
			explodeGutEffect.Shoot(ExplodeGutEffect.Directional.CENTRAL, null);
			if (target.GetMovableNode().GetPassage() != null)
			{
				target.GetMovableNode().GetPassage().GetVerticalRange(ref num, ref num2);
				explodeGutEffect.SetCurrentPassage(target.GetMovableNode().GetPassage());
			}
		}
		workerUnit.gameObject.SetActive(false);
	}

	// Token: 0x04001FC8 RID: 8136
	private Timer attackDelayTimer = new Timer();

	// Token: 0x04001FC9 RID: 8137
	private const float _attackDelayTime = 2f;

	// Token: 0x04001FCA RID: 8138
	private const int _qliphothMax = 1;

	// Token: 0x04001FCB RID: 8139
	private const float _subQliphothTimeCondition = 40f;

	// Token: 0x04001FCC RID: 8140
	private const int _damageMin = 500;

	// Token: 0x04001FCD RID: 8141
	private const int _damageMax = 500;

	// Token: 0x04001FCE RID: 8142
	private const float _attackRange = 3.1f;

	// Token: 0x04001FCF RID: 8143
	private const float _damageRange = 4f;

	// Token: 0x04001FD0 RID: 8144
	private const float _killEffecSize = 1f;

	// Token: 0x04001FD1 RID: 8145
	private const float _sideNodeRemoveRange = 4f;

	// Token: 0x04001FD2 RID: 8146
	private BunnyAnim _animScript;
}
