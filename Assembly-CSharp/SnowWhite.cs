using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000495 RID: 1173
public class SnowWhite : CreatureBase
{
	// Token: 0x06002AD7 RID: 10967 RVA: 0x00029EC1 File Offset: 0x000280C1
	public SnowWhite()
	{
	}

	// Token: 0x170003FD RID: 1021
	// (get) Token: 0x06002AD8 RID: 10968 RVA: 0x00029EFD File Offset: 0x000280FD
	private static int GetDmg
	{
		get
		{
			return UnityEngine.Random.Range(20, 26);
		}
	}

	// Token: 0x170003FE RID: 1022
	// (get) Token: 0x06002AD9 RID: 10969 RVA: 0x00029F08 File Offset: 0x00028108
	private SnowWhiteAnim animScript
	{
		get
		{
			if (this._animScript == null)
			{
				this._animScript = (base.Unit.animTarget as SnowWhiteAnim);
			}
			return this._animScript;
		}
	}

	// Token: 0x06002ADA RID: 10970 RVA: 0x00029F37 File Offset: 0x00028137
	public override void ParamInit()
	{
		this.teleportCasting = false;
		this.teleportDone = false;
		this.currentInfectInfo = null;
		this.teleportTimer.StopTimer();
	}

	// Token: 0x06002ADB RID: 10971 RVA: 0x00029F59 File Offset: 0x00028159
	public override void OnStageStart()
	{
		this.ParamInit();
		if (this.animScript != null)
		{
			this.animScript.ChangeDressToDefault();
		}
		this.attackDelays = new Dictionary<long, SnowWhite.AttackDelay>();
		this.infectInfos = new Dictionary<PassageObjectModel, SnowWhite.PassageInfectInfo>();
	}

	// Token: 0x06002ADC RID: 10972 RVA: 0x0012881C File Offset: 0x00126A1C
	public override void OnStageRelease()
	{
		this.ParamInit();
		this.infectInfos = new Dictionary<PassageObjectModel, SnowWhite.PassageInfectInfo>();
		for (int i = 0; i < this.vineList.Count; i++)
		{
			Notice.instance.Send(NoticeName.RemoveEtcUnit, new object[]
			{
				this.vineList[i]
			});
		}
		this.vineList = new List<SnowWhite.VineArea>();
		this.attackDelays = new Dictionary<long, SnowWhite.AttackDelay>();
	}

	// Token: 0x06002ADD RID: 10973 RVA: 0x0001A90D File Offset: 0x00018B0D
	public override void OnSuppressed()
	{
		this.ParamInit();
	}

	// Token: 0x06002ADE RID: 10974 RVA: 0x00029F93 File Offset: 0x00028193
	public override void ActivateQliphothCounter()
	{
		base.ActivateQliphothCounter();
		this.model.Escape();
	}

	// Token: 0x06002ADF RID: 10975 RVA: 0x00128890 File Offset: 0x00126A90
	public override void OnReturn()
	{
		if (this.animScript != null)
		{
			this.animScript.ChangeDressToDefault();
		}
		this.ParamInit();
		this.model.ResetQliphothCounter();
		if (this.animScript != null)
		{
			this.animScript.OnReturned();
		}
	}

	// Token: 0x06002AE0 RID: 10976 RVA: 0x00029FA6 File Offset: 0x000281A6
	public override void OnFixedUpdate(CreatureModel creature)
	{
		base.OnFixedUpdate(creature);
		this.ProcessVineBuf();
	}

	// Token: 0x06002AE1 RID: 10977 RVA: 0x001288E8 File Offset: 0x00126AE8
	public override void UniqueEscape()
	{
		if (this.teleportCasting)
		{
			this.remainTeleportCasting += Time.deltaTime;
			if (this.remainTeleportCasting >= 2f)
			{
				this.animScript.animator.gameObject.SetActive(false);
			}
			if (this.remainTeleportCasting >= 3f && !this.teleportDone)
			{
				this.TeleportTo(this.teleportTargetPassage);
				this.animScript.ChangeDressToRooted();
				this.animScript.teleportEffect.SetInteger("state", 2);
			}
			if (this.remainTeleportCasting >= 5f)
			{
				this.animScript.animator.gameObject.SetActive(true);
				this.animScript.animator.SetInteger("state", 2);
			}
			if (this.remainTeleportCasting >= 6f)
			{
				this.teleportCasting = false;
				this.teleportTargetPassage = null;
				this.animScript.animator.updateMode = AnimatorUpdateMode.Normal;
				this.animScript.animator.SetInteger("state", 0);
				this.animScript.teleportEffect.updateMode = AnimatorUpdateMode.Normal;
				this.animScript.teleportEffect.gameObject.SetActive(false);
			}
		}
		if (this.remainSpreadDelay > 0f)
		{
			this.remainSpreadDelay -= Time.deltaTime;
		}
		if (this.teleportTimer.RunTimer())
		{
			this.CastTeleport();
			return;
		}
		if (this.remainSpreadDelay <= 0f && !this.teleportCasting)
		{
			if (this.currentInfectInfo != null)
			{
				if (this.currentInfectInfo.completeInfection)
				{
					if (!this.infectedPassage.Contains(this.currentInfectInfo.currentPassage))
					{
						this.infectedPassage.Add(this.currentInfectInfo.currentPassage);
					}
					if (!this.teleportTimer.started)
					{
						this.CastTeleport();
					}
				}
				else
				{
					this.currentInfectInfo.Spread();
				}
				this.remainSpreadDelay = 2f;
			}
			else
			{
				this.CastTeleport();
			}
		}
		this.ProcessVineAttack();
	}

	// Token: 0x06002AE2 RID: 10978 RVA: 0x00128B0C File Offset: 0x00126D0C
	private void ProcessTryToAttack(WorkerModel target)
	{
		if (target.IsDead())
		{
			return;
		}
		if (target.invincible)
		{
			return;
		}
		SnowWhite.AttackDelay attackDelay = null;
		if (this.attackDelays.TryGetValue(target.instanceId, out attackDelay))
		{
			attackDelay.remainDelay -= Time.deltaTime;
			if (attackDelay.remainDelay <= 0f)
			{
				attackDelay.remainDelay = 4f;
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.animScript.attackEffectPrefab, target.GetCurrentViewPosition() + new Vector3(0f, 0f, -1f), Quaternion.identity);
				target.TakeDamage(this.model, new DamageInfo(this.dmgType, (float)SnowWhite.GetDmg));
				DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(target, this.dmgType, this.model);
			}
		}
		else
		{
			attackDelay = new SnowWhite.AttackDelay();
			attackDelay.target = target;
			attackDelay.remainDelay = 4f;
			this.attackDelays.Add(target.instanceId, attackDelay);
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.animScript.attackEffectPrefab, target.GetCurrentViewPosition() + new Vector3(0f, 0f, -1f), Quaternion.identity);
			target.TakeDamage(this.model, new DamageInfo(this.dmgType, (float)SnowWhite.GetDmg));
			DamageParticleEffect damageParticleEffect2 = DamageParticleEffect.Invoker(target, this.dmgType, this.model);
		}
	}

	// Token: 0x06002AE3 RID: 10979 RVA: 0x00128C74 File Offset: 0x00126E74
	private void ProcessVineBuf()
	{
		for (int i = 0; i < this.vineList.Count; i++)
		{
			PassageObjectModel passage = this.vineList[i].GetMovableNode().GetPassage();
			if (passage != null)
			{
				foreach (MovableObjectNode movableObjectNode in passage.GetEnteredTargets())
				{
					UnitModel unit = movableObjectNode.GetUnit();
					if (unit is WorkerModel)
					{
						if (MovableObjectNode.GetDistance(movableObjectNode, this.vineList[i].GetMovableNode()) < 2f)
						{
							unit.AddUnitBuf(new SnowWhiteVineBuf());
						}
					}
				}
			}
		}
	}

	// Token: 0x06002AE4 RID: 10980 RVA: 0x00128D4C File Offset: 0x00126F4C
	private void ProcessVineAttack()
	{
		if (this.teleportCasting)
		{
			return;
		}
		foreach (WorkerModel workerModel in WorkerManager.instance.GetWorkerList())
		{
			if (workerModel.GetMovableNode().currentPassage == this.model.GetMovableNode().currentPassage)
			{
				bool flag = false;
				for (int i = 0; i < this.vineList.Count; i++)
				{
					if (MovableObjectNode.GetDistance(workerModel.GetMovableNode(), this.vineList[i].GetMovableNode()) < 1f)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					this.ProcessTryToAttack(workerModel);
				}
				else if (this.attackDelays.ContainsKey(workerModel.instanceId))
				{
					this.attackDelays.Remove(workerModel.instanceId);
				}
			}
		}
	}

	// Token: 0x06002AE5 RID: 10981 RVA: 0x00128E5C File Offset: 0x0012705C
	private void CastTeleport()
	{
		if (this.teleportCasting)
		{
			return;
		}
		List<PassageObjectModel> list = new List<PassageObjectModel>(this.GetTeleportTargets());
		if (list.Count == 0)
		{
			list.AddRange(this.infectedPassage);
			this.teleportTimer.StartTimer(20f);
		}
		if (list.Count > 0)
		{
			this.teleportTargetPassage = list[UnityEngine.Random.Range(0, list.Count)];
			this.animScript.animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
			this.animScript.animator.SetInteger("state", 1);
			this.animScript.teleportEffect.gameObject.SetActive(true);
			this.animScript.teleportEffect.updateMode = AnimatorUpdateMode.AnimatePhysics;
			this.animScript.teleportEffect.SetInteger("state", 1);
			this.remainTeleportCasting = 0f;
			this.teleportCasting = true;
			this.teleportDone = false;
			return;
		}
	}

	// Token: 0x06002AE6 RID: 10982 RVA: 0x00128F50 File Offset: 0x00127150
	private List<PassageObjectModel> GetTeleportTargets()
	{
		List<Sefira> list = new List<Sefira>(PlayerModel.instance.GetOpenedAreaList());
		List<PassageObjectModel> list2 = new List<PassageObjectModel>();
		foreach (Sefira sefira in list)
		{
			List<PassageObjectModel> list3 = new List<PassageObjectModel>(this.GetPassages(sefira));
			if (list3.Count > 0)
			{
				list2.AddRange(list3);
			}
		}
		return list2;
	}

	// Token: 0x06002AE7 RID: 10983 RVA: 0x00128FDC File Offset: 0x001271DC
	private List<PassageObjectModel> GetPassages(Sefira sefira)
	{
		List<PassageObjectModel> list = new List<PassageObjectModel>();
		foreach (PassageObjectModel passageObjectModel in sefira.passageList)
		{
			if (passageObjectModel.isActivate)
			{
				if (passageObjectModel.type != PassageType.VERTICAL)
				{
					if (passageObjectModel.type != PassageType.SEFIRA)
					{
						if (passageObjectModel.type != PassageType.DEPARTMENT)
						{
							if (passageObjectModel != this.movable.currentPassage)
							{
								if (!this.infectedPassage.Contains(passageObjectModel))
								{
									if (!(SefiraMapLayer.instance.GetPassageObject(passageObjectModel) == null))
									{
										if (SefiraMapLayer.instance.GetPassageObject(passageObjectModel).type != SpaceObjectType.HUBSHORT)
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
		return list;
	}

	// Token: 0x06002AE8 RID: 10984 RVA: 0x001290E4 File Offset: 0x001272E4
	private void TeleportTo(PassageObjectModel passage)
	{
		if (this.infectInfos.ContainsKey(passage))
		{
			this.currentInfectInfo = this.infectInfos[passage];
			this.model.SetCurrentNode(this.currentInfectInfo.startNode);
			Sefira sefira = SefiraManager.instance.GetSefira(this.currentInfectInfo.startNode.GetAttachedPassage().GetSefiraName());
			this.model.sefira = sefira;
			this.model.sefiraNum = sefira.indexString;
		}
		else
		{
			MapNode mapNode = null;
			if (passage.centerNode != null)
			{
				mapNode = passage.centerNode;
			}
			else
			{
				List<MapNode> list = new List<MapNode>();
				foreach (MapNode mapNode2 in passage.GetNodeList())
				{
					if (mapNode2.GetDoor() == null)
					{
						list.Add(mapNode2);
					}
				}
				if (list.Count > 0)
				{
					mapNode = list[UnityEngine.Random.Range(0, list.Count)];
				}
			}
			if (mapNode != null)
			{
				this.currentInfectInfo = new SnowWhite.PassageInfectInfo(this);
				this.currentInfectInfo.currentPassage = passage;
				this.currentInfectInfo.Init();
				this.currentInfectInfo.startNode = mapNode;
				this.model.SetCurrentNode(this.currentInfectInfo.startNode);
				Sefira sefira2 = SefiraManager.instance.GetSefira(mapNode.GetAttachedPassage().GetSefiraName());
				this.model.sefira = sefira2;
				this.model.sefiraNum = sefira2.indexString;
				this.infectInfos.Add(passage, this.currentInfectInfo);
			}
		}
		this.teleportDone = true;
	}

	// Token: 0x06002AE9 RID: 10985 RVA: 0x001127CC File Offset: 0x001109CC
	public override void OnWorkCoolTimeEnd(CreatureFeelingState oldState)
	{
		int probability = 0;
		if (oldState != CreatureFeelingState.BAD)
		{
			if (oldState == CreatureFeelingState.NORM)
			{
				probability = 50;
			}
		}
		else
		{
			probability = 70;
		}
		if (this.Prob(probability))
		{
			this.model.SubQliphothCounter();
		}
	}

	// Token: 0x040028D6 RID: 10454
	private const string animSrc = "Agent/Dead/SnowWhiteAgent";

	// Token: 0x040028D7 RID: 10455
	private const float spreadDelay = 2f;

	// Token: 0x040028D8 RID: 10456
	private const float teleportDelay = 20f;

	// Token: 0x040028D9 RID: 10457
	private const int dmgMin = 20;

	// Token: 0x040028DA RID: 10458
	private const int dmgMax = 25;

	// Token: 0x040028DB RID: 10459
	private RwbpType dmgType = RwbpType.B;

	// Token: 0x040028DC RID: 10460
	public long nextVineId = 10000L;

	// Token: 0x040028DD RID: 10461
	private SnowWhite.PassageInfectInfo currentInfectInfo;

	// Token: 0x040028DE RID: 10462
	public List<PassageObjectModel> infectedPassage = new List<PassageObjectModel>();

	// Token: 0x040028DF RID: 10463
	public List<SnowWhite.VineArea> vineList = new List<SnowWhite.VineArea>();

	// Token: 0x040028E0 RID: 10464
	public Dictionary<PassageObjectModel, SnowWhite.PassageInfectInfo> infectInfos;

	// Token: 0x040028E1 RID: 10465
	private SnowWhiteAnim _animScript;

	// Token: 0x040028E2 RID: 10466
	private float remainSpreadDelay;

	// Token: 0x040028E3 RID: 10467
	private float remainTeleportCasting;

	// Token: 0x040028E4 RID: 10468
	private bool teleportCasting;

	// Token: 0x040028E5 RID: 10469
	private bool teleportDone;

	// Token: 0x040028E6 RID: 10470
	private PassageObjectModel teleportTargetPassage;

	// Token: 0x040028E7 RID: 10471
	private Timer teleportTimer = new Timer();

	// Token: 0x040028E8 RID: 10472
	private Dictionary<long, SnowWhite.AttackDelay> attackDelays;

	// Token: 0x02000496 RID: 1174
	public class PassageInfectInfo
	{
		// Token: 0x06002AEA RID: 10986 RVA: 0x00029FB5 File Offset: 0x000281B5
		public PassageInfectInfo(SnowWhite s)
		{
			this.snowWhite = s;
		}

		// Token: 0x06002AEB RID: 10987 RVA: 0x00029FC4 File Offset: 0x000281C4
		public void Init()
		{
			this.leftVine = null;
			this.rightVine = null;
			this.completeInfection = false;
		}

		// Token: 0x06002AEC RID: 10988 RVA: 0x00129280 File Offset: 0x00127480
		public void Spread()
		{
			if (this.leftVine == null)
			{
				SnowWhite snowWhite = this.snowWhite;
				long nextVineId;
				snowWhite.nextVineId = (nextVineId = snowWhite.nextVineId) + 1L;
				SnowWhite.VineArea vineArea = new SnowWhite.VineArea(nextVineId, this.snowWhite.model.GetMovableNode());
				this.leftVine = vineArea;
				this.rightVine = vineArea;
				this.snowWhite.vineList.Add(vineArea);
				Notice.instance.Send(NoticeName.AddEtcUnit, new object[]
				{
					vineArea
				});
			}
			else
			{
				MovableObjectNode sideMovableNode = this.leftVine.GetMovableNode().GetSideMovableNode(UnitDirection.LEFT, 0.5f);
				MovableObjectNode sideMovableNode2 = this.rightVine.GetMovableNode().GetSideMovableNode(UnitDirection.RIGHT, 0.5f);
				this.completeInfection = true;
				if (sideMovableNode.currentPassage == this.currentPassage && MovableObjectNode.GetDistance(sideMovableNode, this.leftVine.GetMovableNode()) > 0.5f)
				{
					SnowWhite snowWhite2 = this.snowWhite;
					long nextVineId;
					snowWhite2.nextVineId = (nextVineId = snowWhite2.nextVineId) + 1L;
					SnowWhite.VineArea vineArea2 = new SnowWhite.VineArea(nextVineId, sideMovableNode);
					this.leftVine = vineArea2;
					this.snowWhite.vineList.Add(vineArea2);
					Notice.instance.Send(NoticeName.AddEtcUnit, new object[]
					{
						vineArea2
					});
					this.completeInfection = false;
				}
				if (sideMovableNode2.currentPassage == this.currentPassage && MovableObjectNode.GetDistance(sideMovableNode2, this.rightVine.GetMovableNode()) > 0.5f)
				{
					SnowWhite snowWhite3 = this.snowWhite;
					long nextVineId;
					snowWhite3.nextVineId = (nextVineId = snowWhite3.nextVineId) + 1L;
					SnowWhite.VineArea vineArea3 = new SnowWhite.VineArea(nextVineId, sideMovableNode2);
					this.rightVine = vineArea3;
					this.snowWhite.vineList.Add(vineArea3);
					Notice.instance.Send(NoticeName.AddEtcUnit, new object[]
					{
						vineArea3
					});
					this.completeInfection = false;
				}
			}
		}

		// Token: 0x040028E9 RID: 10473
		private const float vineDistance = 0.5f;

		// Token: 0x040028EA RID: 10474
		public SnowWhite snowWhite;

		// Token: 0x040028EB RID: 10475
		public PassageObjectModel currentPassage;

		// Token: 0x040028EC RID: 10476
		public MapNode startNode;

		// Token: 0x040028ED RID: 10477
		public SnowWhite.VineArea leftVine;

		// Token: 0x040028EE RID: 10478
		public SnowWhite.VineArea rightVine;

		// Token: 0x040028EF RID: 10479
		public bool completeInfection;
	}

	// Token: 0x02000497 RID: 1175
	public class VineArea : UnitModel
	{
		// Token: 0x06002AED RID: 10989 RVA: 0x00029FDB File Offset: 0x000281DB
		public VineArea(long instanceId, MovableObjectNode m)
		{
			this.instanceId = instanceId;
			this.movableNode = new MovableObjectNode(false);
			this.movableNode.Assign(m);
			this.movableNode.StopMoving();
		}
	}

	// Token: 0x02000498 RID: 1176
	public class AttackDelay
	{
		// Token: 0x06002AEE RID: 10990 RVA: 0x000043D4 File Offset: 0x000025D4
		public AttackDelay()
		{
		}

		// Token: 0x040028F0 RID: 10480
		public WorkerModel target;

		// Token: 0x040028F1 RID: 10481
		public float remainDelay;
	}
}
