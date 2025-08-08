using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using WorkerSpine;

// Token: 0x020003BB RID: 955
public class BossBird : BirdCreatureBase
{
	// Token: 0x17000280 RID: 640
	// (get) Token: 0x06001E7D RID: 7805 RVA: 0x00020ACF File Offset: 0x0001ECCF
	private static int laserDmg
	{
		get
		{
			return UnityEngine.Random.Range(8, 15);
		}
	}

	// Token: 0x17000281 RID: 641
	// (get) Token: 0x06001E7E RID: 7806 RVA: 0x00020AD9 File Offset: 0x0001ECD9
	private static int scaleDmg
	{
		get
		{
			return UnityEngine.Random.Range(10, 16);
		}
	}

	// Token: 0x17000282 RID: 642
	// (get) Token: 0x06001E7F RID: 7807 RVA: 0x00020AE4 File Offset: 0x0001ECE4
	private static int biteDmg
	{
		get
		{
			return UnityEngine.Random.Range(50, 116);
		}
	}

	// Token: 0x17000283 RID: 643
	// (get) Token: 0x06001E80 RID: 7808 RVA: 0x00020AEF File Offset: 0x0001ECEF
	private static int attackDmg
	{
		get
		{
			return UnityEngine.Random.Range(23, 44);
		}
	}

	// Token: 0x17000284 RID: 644
	// (get) Token: 0x06001E81 RID: 7809 RVA: 0x00020AFA File Offset: 0x0001ECFA
	private static int attractCnt
	{
		get
		{
			return UnityEngine.Random.Range(3, 5);
		}
	}

	// Token: 0x17000285 RID: 645
	// (get) Token: 0x06001E82 RID: 7810 RVA: 0x00020B03 File Offset: 0x0001ED03
	private float attackDelay
	{
		get
		{
			return UnityEngine.Random.Range(4f, 6f);
		}
	}

	// Token: 0x17000286 RID: 646
	// (get) Token: 0x06001E83 RID: 7811 RVA: 0x00020B14 File Offset: 0x0001ED14
	private float teleportDelay
	{
		get
		{
			return UnityEngine.Random.Range(15f, 25f);
		}
	}

	// Token: 0x17000287 RID: 647
	// (get) Token: 0x06001E84 RID: 7812 RVA: 0x0001F301 File Offset: 0x0001D501
	private float forcelyTeleportDelay
	{
		get
		{
			return UnityEngine.Random.Range(10f, 15f);
		}
	}

	// Token: 0x17000288 RID: 648
	// (get) Token: 0x06001E85 RID: 7813 RVA: 0x0001E815 File Offset: 0x0001CA15
	private ChildCreatureModel Model
	{
		get
		{
			return this.model as ChildCreatureModel;
		}
	}

	// Token: 0x17000289 RID: 649
	// (get) Token: 0x06001E86 RID: 7814 RVA: 0x00020B25 File Offset: 0x0001ED25
	private BossBirdAnim animScript
	{
		get
		{
			return this._unit.animTarget as BossBirdAnim;
		}
	}

	// Token: 0x1700028A RID: 650
	// (get) Token: 0x06001E87 RID: 7815 RVA: 0x00020B37 File Offset: 0x0001ED37
	// (set) Token: 0x06001E88 RID: 7816 RVA: 0x00020B3F File Offset: 0x0001ED3F
	public BossBird.Phase currentPhase
	{
		get
		{
			return this._currentPhase;
		}
		set
		{
			this.OnSetPhase(value, this._currentPhase != value);
			this._currentPhase = value;
		}
	}

	// Token: 0x1700028B RID: 651
	// (get) Token: 0x06001E89 RID: 7817 RVA: 0x00020B5B File Offset: 0x0001ED5B
	public EventCreatureModel GateWayModel
	{
		get
		{
			return this.gateWay.model as EventCreatureModel;
		}
	}

	// Token: 0x1700028C RID: 652
	// (get) Token: 0x06001E8A RID: 7818 RVA: 0x00020B6D File Offset: 0x0001ED6D
	public EventCreatureModel BigEggModel
	{
		get
		{
			return this.bigEgg.model as EventCreatureModel;
		}
	}

	// Token: 0x1700028D RID: 653
	// (get) Token: 0x06001E8B RID: 7819 RVA: 0x00020B7F File Offset: 0x0001ED7F
	public EventCreatureModel LongEggModel
	{
		get
		{
			return this.longEgg.model as EventCreatureModel;
		}
	}

	// Token: 0x1700028E RID: 654
	// (get) Token: 0x06001E8C RID: 7820 RVA: 0x00020B91 File Offset: 0x0001ED91
	public EventCreatureModel SmallEggModel
	{
		get
		{
			return this.smallEgg.model as EventCreatureModel;
		}
	}

	// Token: 0x06001E8D RID: 7821 RVA: 0x00020BA3 File Offset: 0x0001EDA3
	public override void SetModel(CreatureModel model)
	{
		this.model = model;
	}

	// Token: 0x06001E8E RID: 7822 RVA: 0x000F689C File Offset: 0x000F4A9C
	public override void OnViewInit(CreatureUnit unit)
	{
		this._unit = (unit as ChildCreatureUnit);
		this.animScript.SetScript(this);
		this.animScript.SetHalfEvent(new BossBirdGutScript.HalfEvent(this.EndUltShoot));
		List<string> list = new List<string>();
		list.AddRange(CreatureModel.regionName);
		list.AddRange(CreatureModel.careTakingRegion);
		foreach (string region in list)
		{
			this.model.observeInfo.OnObserveRegion(region);
		}
	}

	// Token: 0x06001E8F RID: 7823 RVA: 0x00020BAC File Offset: 0x0001EDAC
	public override void OnStageStart()
	{
		base.OnStageStart();
		this.ParamInit();
	}

	// Token: 0x06001E90 RID: 7824 RVA: 0x00020BBA File Offset: 0x0001EDBA
	public override void OnStageEnd()
	{
		base.OnStageRelease();
		this.bigBird = null;
		this.smallBird = null;
		this.longBird = null;
	}

	// Token: 0x06001E91 RID: 7825 RVA: 0x000F6948 File Offset: 0x000F4B48
	public override void ParamInit()
	{
		this.escapedCreatures.Clear();
		this.currentAttackType = BossBird.AttackType.NORMAL;
		this.currentTeleportDest = null;
		this.aliveEggs.Clear();
		this.forcelyTeleportReady = false;
		this.teleportReady = false;
		this.normalAttackReady = false;
		this.patternReady = false;
		this.forcelyTeleport = false;
		this.activateState = true;
		this.isTeleporting = false;
		this.ultShooted = false;
		this.currentForcelyTeleportTarget = null;
	}

	// Token: 0x06001E92 RID: 7826 RVA: 0x00020BD7 File Offset: 0x0001EDD7
	public void DeleteSound(SoundEffectPlayer sound)
	{
		if (sound != null)
		{
			sound.Stop();
			UnityEngine.Object.Destroy(sound.gameObject);
			sound = null;
		}
	}

	// Token: 0x06001E93 RID: 7827 RVA: 0x00020BF9 File Offset: 0x0001EDF9
	private void OnSetPhase(BossBird.Phase p, bool isDifferentWithPrevious)
	{
		if (p != BossBird.Phase.IDLE)
		{
		}
	}

	// Token: 0x06001E94 RID: 7828 RVA: 0x000F69B8 File Offset: 0x000F4BB8
	public void SetBirds(LongBird longBird, SmallBird smallBird, BigBird bigBird)
	{
		this.smallBird = smallBird;
		this.longBird = longBird;
		this.bigBird = bigBird;
		smallBird.SetBoss(this);
		longBird.SetBoss(this);
		bigBird.SetBoss(this);
		this.birdControl.Clear();
		this.birdControl.Add(smallBird);
		this.birdControl.Add(longBird);
		this.birdControl.Add(bigBird);
	}

	// Token: 0x06001E95 RID: 7829 RVA: 0x000F6A20 File Offset: 0x000F4C20
	public void OnBirdEscape(LongBird longBird, SmallBird small, BigBird big)
	{
		EventBase eventBase = null;
		if (!SpecialEventManager.instance.CheckEventContains(EventBase.EventType.BOSS_BIRD, out eventBase))
		{
			return;
		}
		if (this.isEscaped)
		{
			return;
		}
		List<CreatureBase> list = new List<CreatureBase>();
		foreach (CreatureBase creatureBase in this.escapedCreatures)
		{
			if (!creatureBase.model.IsEscaped())
			{
				list.Add(creatureBase);
			}
		}
		foreach (CreatureBase item in list)
		{
			this.escapedCreatures.Remove(item);
		}
		if (longBird != null && !this.escapedCreatures.Contains(longBird))
		{
			this.escapedCreatures.Add(longBird);
		}
		if (small != null && !this.escapedCreatures.Contains(small))
		{
			this.escapedCreatures.Add(small);
		}
		if (big != null && !this.escapedCreatures.Contains(big))
		{
			this.escapedCreatures.Add(big);
		}
		if (this.escapedCreatures.Count >= 2 && this.CheckTwilight())
		{
			this.eventScript = (eventBase as BossBirdEvent);
			this.SetCreatureActivateState(true);
			this.animScript.gameObject.SetActive(true);
			this.animScript.DisplayNarration(BossBird.NarrationState.ESCAPE, 4f, new BossBirdAnim.NarrationUI.NarrationEndEvent(this.MakeBirdConcentrationGate));
			this.notArrived.Add(this.smallBird);
			this.notArrived.Add(this.bigBird);
			this.notArrived.Add(this.longBird);
			this.isEscaped = true;
		}
	}

	// Token: 0x06001E96 RID: 7830 RVA: 0x000F6C04 File Offset: 0x000F4E04
	private bool CheckTwilight()
	{ // <Patch>
		List<AgentModel> list = new List<AgentModel>(AgentManager.instance.GetAgentList());
		foreach (AgentModel agentModel in list)
		{
			if (!agentModel.IsDead())
			{
				WeaponModel weapon = agentModel.Equipment.weapon;
				ArmorModel armor = agentModel.Equipment.armor;
				if (weapon != null && EquipmentTypeInfo.GetLcId(weapon.metaInfo) == 200038)
				{
					return false;
				}
				if (armor != null && EquipmentTypeInfo.GetLcId(armor.metaInfo) == 300038)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06001E97 RID: 7831 RVA: 0x000F6CDC File Offset: 0x000F4EDC
	public void MakeBirdConcentrationGate()
	{
		SpecialEventManager.instance.ActivateEvent(this.eventScript);
		EventCreatureModel eventCreatureModel = this.eventScript.MakeGate();
		this.gateWay = (eventCreatureModel.script as BossGateWay);
		this.gateWay.SetBirds(this.bigBird, this.smallBird, this.longBird, this);
		MapNode currentNode = eventCreatureModel.GetCurrentNode();
		this.gatewayNode = currentNode;
		this.StartBirdMoveToNode(currentNode);
	}

	// Token: 0x06001E98 RID: 7832 RVA: 0x000F6D4C File Offset: 0x000F4F4C
	private void MakeBirdObjects()
	{
		List<MapNode> list = new List<MapNode>();
		List<Sefira> list2 = new List<Sefira>(PlayerModel.instance.GetOpenedAreaList());
		List<Sefira> list3 = new List<Sefira>();
		int count = list2.Count;
		for (int i = 0; i < count; i++)
		{
			Sefira item = list2[UnityEngine.Random.Range(0, list2.Count)];
			list3.Add(item);
			list2.Remove(item);
		}
		list2 = list3;
		foreach (Sefira sefira in list2)
		{
			MapNode[] nodeList = sefira.sefiraPassage.GetNodeList();
			List<MapNode> list4 = new List<MapNode>(nodeList);
			List<MapNode> list5 = list4;
			if (BossBird.cache0 == null)
			{
				BossBird.cache0 = new Comparison<MapNode>(MapNode.CompareByX);
			}
			list5.Sort(BossBird.cache0);
			MapNode item2 = list4[list4.Count / 2];
			list.Add(item2);
			if (list.Count == 3)
			{
				break;
			}
		}
		EventCreatureModel eventCreatureModel = this.eventScript.MakeBigEgg(list[0]);
		this.bigEgg = (eventCreatureModel.script as BossEggBase);
		this.bigEgg.SetType(BossEggBase.EggType.BIG);
		EventCreatureModel eventCreatureModel2 = this.eventScript.MakeSmallEgg(list[1]);
		this.smallEgg = (eventCreatureModel2.script as BossEggBase);
		this.smallEgg.SetType(BossEggBase.EggType.SMALL);
		EventCreatureModel eventCreatureModel3 = this.eventScript.MakeLongEgg(list[2]);
		this.longEgg = (eventCreatureModel3.script as BossEggBase);
		this.longEgg.SetType(BossEggBase.EggType.LONG);
		this.SetPassageAlpha(true);
	}

	// Token: 0x06001E99 RID: 7833 RVA: 0x00020C11 File Offset: 0x0001EE11
	private void StartBirdMoveToNode(MapNode dest)
	{
		this.smallBird.MakeMoveToGate(dest);
		this.bigBird.MakeMoveToGate(dest);
		this.longBird.MakeMoveToGate(dest);
	}

	// Token: 0x06001E9A RID: 7834 RVA: 0x000F6F04 File Offset: 0x000F5104
	public override void OnFixedUpdate(CreatureModel creature)
	{
		this.CheckCreatureUnitState();
		if (!this.activateState)
		{
			return;
		}
		if (this.currentPhase == BossBird.Phase.NOTACTIVATED)
		{
			return;
		}
		this.Model.CheckNearWorkerEncounting();
		if (this.forcelyTeleportTimer.started)
		{
			if (this.forcelyTeleportTimer.RunTimer())
			{
				this.forcelyTeleportReady = true;
			}
		}
		else if (this.teleportTimer.started && this.teleportTimer.RunTimer())
		{
			this.teleportReady = true;
		}
		if (this.forcelyTeleportReady && this.currentForcelyTeleportTarget != null && this.currentForcelyTeleportTarget.IsEnabled())
		{
			if (this.currentPhase == BossBird.Phase.IDLE)
			{
				this.InvokeForceTeleport();
			}
		}
		else if (this.teleportReady && this.currentPhase == BossBird.Phase.IDLE)
		{
			this.InvokeTeleport();
		}
		if (this.currentPhase != BossBird.Phase.TELEPORT)
		{
			if (this.currentAttackType == BossBird.AttackType.ULTIMATE && this.currentPhase == BossBird.Phase.SPECIAL_PATTERN && this.ultShooted)
			{
				this.TakeUltDamage();
			}
			if (this.normalAttackTimer.started && this.normalAttackTimer.RunTimer())
			{
				this.normalAttackReady = true;
			}
			if (this.specialPatternTimer.started && this.specialPatternTimer.RunTimer())
			{
				this.patternReady = true;
			}
			if (this.normalAttackReady && this.currentPhase == BossBird.Phase.IDLE)
			{
				this.currentPhase = BossBird.Phase.ATTACK;
				this.InvokeNormalAttack();
			}
			if (this.patternReady && this.currentPhase == BossBird.Phase.IDLE)
			{
				this.currentPhase = BossBird.Phase.SPECIAL_PATTERN;
				this.InvokePatternAttack();
			}
		}
	}

	// Token: 0x06001E9B RID: 7835 RVA: 0x000F70B4 File Offset: 0x000F52B4
	private void InvokeForceTeleport()
	{
		this.teleportReady = false;
		this.forcelyTeleportTimer.StopTimer();
		this.currentAttackType = BossBird.AttackType.NORMAL;
		this.currentPhase = BossBird.Phase.TELEPORT;
		this.forcelyTeleport = false;
		this.MakeMovementToEgg(this.currentForcelyTeleportTarget);
		this.currentForcelyTeleportTarget = null;
		this.forcelyTeleportTimer.StartTimer(this.forcelyTeleportDelay);
	}

	// Token: 0x06001E9C RID: 7836 RVA: 0x00020C37 File Offset: 0x0001EE37
	private void InvokeTeleport()
	{
		this.teleportTimer.StopTimer();
		this.teleportReady = false;
		this.currentAttackType = BossBird.AttackType.NORMAL;
		this.currentPhase = BossBird.Phase.TELEPORT;
		this.MakeMovementRandom();
		this.teleportTimer.StartTimer(this.teleportDelay);
	}

	// Token: 0x06001E9D RID: 7837 RVA: 0x00020C70 File Offset: 0x0001EE70
	private void InvokeNormalAttack()
	{
		this.normalAttackReady = false;
		this.currentAttackType = BossBird.AttackType.NORMAL;
		this.StartMeleeAttack();
	}

	// Token: 0x06001E9E RID: 7838 RVA: 0x000F710C File Offset: 0x000F530C
	private void InvokePatternAttack()
	{
		this.patternReady = false;
		BossBird.AttackType attackType = this.GetAttackType();
		if (attackType == BossBird.AttackType.NORMAL)
		{
			this.currentPhase = BossBird.Phase.ATTACK;
			this.specialPatternTimer.StartTimer(15f);
			this.normalAttackTimer.StopTimer();
			this.InvokeNormalAttack();
			return;
		}
		switch (attackType)
		{
		case BossBird.AttackType.RANGED:
			this.StartRangeAttack();
			break;
		case BossBird.AttackType.ATTRACT:
			this.AttractPattern();
			break;
		case BossBird.AttackType.SCALE:
			this.ScalePattern();
			break;
		case BossBird.AttackType.ULTIMATE:
			this.BitePattern();
			break;
		}
	}

	// Token: 0x06001E9F RID: 7839 RVA: 0x00020C86 File Offset: 0x0001EE86
	public void EndNormalAttack()
	{
		this.currentPhase = BossBird.Phase.IDLE;
		this.normalAttackTimer.StartTimer(this.attackDelay);
	}

	// Token: 0x06001EA0 RID: 7840 RVA: 0x00020CA0 File Offset: 0x0001EEA0
	public void EndPatternAttack()
	{
		this.currentPhase = BossBird.Phase.IDLE;
		this.specialPatternTimer.StartTimer(15f);
	}

	// Token: 0x06001EA1 RID: 7841 RVA: 0x00020CB9 File Offset: 0x0001EEB9
	public void CancelNormalAttack()
	{
		if (this.isTeleporting)
		{
			return;
		}
		this.animScript.OnCancel();
		this.EndNormalAttack();
		this.InvokeTeleport();
	}

	// Token: 0x06001EA2 RID: 7842 RVA: 0x00020CDE File Offset: 0x0001EEDE
	public void CancelPatternAttack()
	{
		if (this.isTeleporting)
		{
			return;
		}
		this.animScript.OnCancel();
		this.EndPatternAttack();
		this.InvokeTeleport();
	}

	// Token: 0x06001EA3 RID: 7843 RVA: 0x000F71A0 File Offset: 0x000F53A0
	private void CheckCreatureUnitState()
	{
		try
		{
			if (!(this._unit == null))
			{
				if (this.activateState != this._unit.animTarget.gameObject.activeInHierarchy)
				{
					this._unit.animTarget.gameObject.SetActive(this.activateState);
				}
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	// Token: 0x06001EA4 RID: 7844 RVA: 0x000F7220 File Offset: 0x000F5420
	public void SetCreatureActivateState(bool state)
	{
		if (this._unit != null && this._unit.gameObject != null)
		{
			this._unit.gameObject.SetActive(state);
			if (this.movable != null)
			{
				this.movable.SetActive(state);
			}
		}
		this.activateState = state;
		ChildCreatureModel childCreatureModel = this.model as ChildCreatureModel;
		childCreatureModel.activateState = state;
		if (state)
		{
			this.currentPhase = BossBird.Phase.IDLE;
		}
		else
		{
			this.currentPhase = BossBird.Phase.NOTACTIVATED;
		}
	}

	// Token: 0x06001EA5 RID: 7845 RVA: 0x000F72B0 File Offset: 0x000F54B0
	private void EscapeInit()
	{
		this.normalAttackReady = false;
		this.movable.SetActive(true);
		this.model.EscapeWithoutIsolateRoom();
		this.currentAttackType = BossBird.AttackType.NORMAL;
		this.StartMeleeAttack();
		PlaySpeedSettingUI.instance.SetNormalSpeedForcely();
		this.specialPatternTimer.StartTimer(15f);
		this.teleportTimer.StartTimer(this.teleportDelay);
		this.uniquePattern.StartTimer(0.1f);
		PlaySpeedSettingUI.instance.SetBlockEvent(new PlaySpeedSettingUI.BlockedUIEvent(this.OnCannotUsePlaySpeedSetting));
		PlaySpeedSettingUI.instance.BlockSetting(this);
		this.aliveEggs.Add(this.bigEgg);
		this.bigEgg.SetBoss(this);
		this.aliveEggs.Add(this.smallEgg);
		this.smallEgg.SetBoss(this);
		this.aliveEggs.Add(this.longEgg);
		this.longEgg.SetBoss(this);
		this.defaultAmbience = this.MakeSoundLoop("default");
		this.SetPassageAlpha(true);
	}

	// Token: 0x06001EA6 RID: 7846 RVA: 0x00020D03 File Offset: 0x0001EF03
	public void OnCannotUsePlaySpeedSetting(int id)
	{
		Debug.Log("Block ui " + id);
	}

	// Token: 0x06001EA7 RID: 7847 RVA: 0x00020D1A File Offset: 0x0001EF1A
	public void OnWorkerWakeUp(WorkerModel worker)
	{
		this.attracted.Remove(worker);
	}

	// Token: 0x06001EA8 RID: 7848 RVA: 0x000F73B4 File Offset: 0x000F55B4
	public override void OnSuppressed()
	{
		base.OnSuppressed();
		List<WorkerModel> list = new List<WorkerModel>();
		foreach (WorkerModel item in this.attracted)
		{
			list.Add(item);
		}
		foreach (WorkerModel workerModel in list)
		{
			if (workerModel.IsAttackTargetable())
			{
				if (workerModel.unconAction is Uncontrollable_BigBird)
				{
					this.WakeUpWorker(workerModel);
				}
			}
		}
		this.attracted.Clear();
		this.currentPhase = BossBird.Phase.NOTACTIVATED;
		this.bigBird.OnBossSuppressed();
		this.smallBird.OnBossSuppressed();
		this.longBird.OnBossSuppressed();
		this.Model.animAutoSet = false;
		this.AddAgentsTrait();
	}

	// Token: 0x06001EA9 RID: 7849 RVA: 0x000F74C8 File Offset: 0x000F56C8
	private void AddAgentsTrait()
	{
		List<AgentModel> list = new List<AgentModel>();
		foreach (AgentModel agentModel in AgentManager.instance.GetAgentList())
		{
			if (!agentModel.IsDead())
			{
				if (agentModel.unconAction == null)
				{
					if (!agentModel.HasEquipment(400038))
					{
						list.Add(agentModel);
					}
				}
			}
		}
		this.animScript.StartCoroutine(this.animScript.AttachGifts(list));
		this.TryMakeEquipments();
	}

	// Token: 0x06001EAA RID: 7850 RVA: 0x00020D29 File Offset: 0x0001EF29
	private void TryMakeEquipments()
	{
		this.TryMakeEquip(200038);
		this.TryMakeEquip(300038);
		this.Model.observeInfo.ObserveAll(new string[0]);
	}

	// Token: 0x06001EAB RID: 7851 RVA: 0x000F757C File Offset: 0x000F577C
	private void TryMakeEquip(int id)
	{
		if (InventoryModel.Instance.CheckEquipmentCount(id))
		{
			EquipmentModel equipmentModel = InventoryModel.Instance.CreateEquipment(id);
		}
	}

	// Token: 0x06001EAC RID: 7852 RVA: 0x000F75A8 File Offset: 0x000F57A8
	public void OnBirdArrived(IBirdControl bird)
	{
		if (this.notArrived.Remove(bird))
		{
			BossBird.NarrationState state = BossBird.NarrationState.BIGBIRD_ARRIVED;
			if (bird is LongBird)
			{
				state = BossBird.NarrationState.LONGBIRD_ARRIVED;
			}
			else if (bird is SmallBird)
			{
				state = BossBird.NarrationState.SMALLBIRD_ARRIVED;
			}
			else if (bird is BigBird)
			{
				state = BossBird.NarrationState.BIGBIRD_ARRIVED;
			}
			this.OnBirdArrivedStartNarration();
			BossBirdAnim.NarrationUI.NarrationEndEvent endEvent = null;
			if (this.notArrived.Count > 0)
			{
				endEvent = new BossBirdAnim.NarrationUI.NarrationEndEvent(this.OnBirdArrivedEndNarration);
			}
			this.gateWay.OnEnterBird();
			this.animScript.DisplayNarration(state, 4f, endEvent);
			if (this.notArrived.Count == 0)
			{
				this.animScript.DisplayNarration(BossBird.NarrationState.BOSS_APPEAR, 2f, new BossBirdAnim.NarrationUI.NarrationEndEvent(this.OnAllBirdArrived), new BossBirdAnim.NarrationUI.FadeOutEvent(this.FadeOutSound));
				this.MakeBirdObjects();
				this.animScript.SetCreatureAnim(true);
			}
		}
		else
		{
			Debug.Log("Cannot find Bird " + bird);
		}
	}

	// Token: 0x06001EAD RID: 7853 RVA: 0x00020D57 File Offset: 0x0001EF57
	public void OnGateWaySuppressed()
	{
		this.eventScript.EventEnd();
		this.smallBird.OnGateSuppressed();
		this.bigBird.OnGateSuppressed();
		this.longBird.OnGateSuppressed();
	}

	// Token: 0x06001EAE RID: 7854 RVA: 0x000F769C File Offset: 0x000F589C
	public void FadeOutSound()
	{
		SoundEffectPlayer soundEffectPlayer = this.MakeSound("appear");
		soundEffectPlayer.SetDestroyTime(4.5f);
	}

	// Token: 0x06001EAF RID: 7855 RVA: 0x00020D85 File Offset: 0x0001EF85
	private void OnBirdArrivedStartNarration()
	{
		this.isNarration = true;
	}

	// Token: 0x06001EB0 RID: 7856 RVA: 0x00020D8E File Offset: 0x0001EF8E
	private void OnBirdArrivedEndNarration()
	{
		this.isNarration = false;
	}

	// Token: 0x06001EB1 RID: 7857 RVA: 0x000F76C0 File Offset: 0x000F58C0
	private void OnAllBirdArrived()
	{
		foreach (IBirdControl birdControl in this.birdControl)
		{
			birdControl.OnBossActivate();
		}
		this.AfterAppearNarration();
	}

	// Token: 0x06001EB2 RID: 7858 RVA: 0x00020D97 File Offset: 0x0001EF97
	private void AfterAppearNarration()
	{
		this.movable.SetCurrentNode(this.gatewayNode);
		this.EscapeInit();
	}

	// Token: 0x06001EB3 RID: 7859 RVA: 0x00020DB0 File Offset: 0x0001EFB0
	public void MoveToNode(MapNode node)
	{
		this.model.MoveToNode(node);
	}

	// Token: 0x06001EB4 RID: 7860 RVA: 0x000F7724 File Offset: 0x000F5924
	public void MakeMovementToEgg(BossEggBase script)
	{
		PassageObjectModel currentPassage = script.currentPassage;
		MapNode mapNode = script.movable.GetCurrentNode();
		if (mapNode == null)
		{
			Debug.LogError("Egg's Node is NULL");
			MapNode[] nodeList = script.currentPassage.GetNodeList();
			mapNode = nodeList[nodeList.Length / 2];
		}
		this.PrevTeleport(mapNode);
	}

	// Token: 0x06001EB5 RID: 7861 RVA: 0x000F7770 File Offset: 0x000F5970
	public void MakeMovementRandom()
	{
		if (this.currentPassage == null)
		{
			Debug.LogError("Boss bird cannot go null passage");
			return;
		}
		Sefira[] openedAreaList = PlayerModel.instance.GetOpenedAreaList();
		Sefira sefira = null;
		string sefiraName = this.currentPassage.GetSefiraName();
		List<Sefira> list = new List<Sefira>(openedAreaList);
		List<Sefira> list2 = new List<Sefira>();
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			Sefira item = list[UnityEngine.Random.Range(0, list.Count)];
			list2.Add(item);
			list.Remove(item);
		}
		foreach (Sefira sefira2 in list2)
		{
			if (sefira2.indexString != sefiraName)
			{
				sefira = sefira2;
				break;
			}
		}
		Sefira sefira3 = sefira;
		List<MapNode> list3 = new List<MapNode>(sefira3.sefiraPassage.GetNodeList());
		List<MapNode> list4 = list3;
		if (BossBird.cache1 == null)
		{
			BossBird.cache1 = new Comparison<MapNode>(MapNode.CompareByX);
		}
		list4.Sort(BossBird.cache1);
		MapNode targetNode = list3[list3.Count / 2];
		this.currentPassage.ExitUnit(this.movable);
		this.PrevTeleport(targetNode);
	}

	// Token: 0x06001EB6 RID: 7862 RVA: 0x00020DBE File Offset: 0x0001EFBE
	public void PrevTeleport(MapNode targetNode)
	{
		this.animScript.StartTeleport();
		this.currentTeleportDest = targetNode;
		this.isTeleporting = true;
		this.MakeSound("move");
	}

	// Token: 0x06001EB7 RID: 7863 RVA: 0x000F78C4 File Offset: 0x000F5AC4
	public void Teleport()
	{
		this.model.SetCurrentNode(this.currentTeleportDest);
		this.currentTeleportDest.GetAttachedPassage().EnterUnit(this.movable);
		this.currentTeleportDest = null;
		if (this.Prob(50))
		{
			this.movable.SetDirection(UnitDirection.LEFT);
		}
		else
		{
			this.movable.SetDirection(UnitDirection.RIGHT);
		}
	}

	// Token: 0x06001EB8 RID: 7864 RVA: 0x00020DE5 File Offset: 0x0001EFE5
	public void EndTeleport()
	{
		this.isTeleporting = false;
		this.currentPhase = BossBird.Phase.IDLE;
	}

	// Token: 0x06001EB9 RID: 7865 RVA: 0x00020DF5 File Offset: 0x0001EFF5
	public void StopMovement()
	{
		this.movable.StopMoving();
		if (this.model.GetCurrentCommand() is MoveCreatureCommand)
		{
			this.model.ClearCommand();
		}
	}

	// Token: 0x06001EBA RID: 7866 RVA: 0x000F792C File Offset: 0x000F5B2C
	public void StartMeleeAttack()
	{
		if (this.currentPassage == null)
		{
			this.CancelNormalAttack();
			return;
		}
		List<UnitModel> attackTarget = this.GetAttackTarget();
		if (attackTarget.Count != 0)
		{
			UnitModel unitModel = attackTarget[UnityEngine.Random.Range(0, attackTarget.Count)];
			float x = unitModel.GetCurrentViewPosition().x;
			float x2 = this.model.GetCurrentViewPosition().x;
			if (x2 <= x)
			{
				this.movable.SetDirection(UnitDirection.RIGHT);
			}
			else
			{
				this.movable.SetDirection(UnitDirection.LEFT);
			}
		}
		this.animScript.StartMeleeAttack();
	}

	// Token: 0x06001EBB RID: 7867 RVA: 0x000F79C8 File Offset: 0x000F5BC8
	public void GiveMeleeDamage()
	{
		if (this.currentPassage == null)
		{
			return;
		}
		List<UnitModel> directionTarget = this.GetDirectionTarget(this.GetAttackTarget(), 10f, 0f);
		foreach (UnitModel unitModel in directionTarget)
		{
			unitModel.TakeDamage(this.model, new DamageInfo(this.attackType, (float)BossBird.attackDmg));
			DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(unitModel, this.attackType, this.model);
		}
	}

	// Token: 0x06001EBC RID: 7868 RVA: 0x000F7A6C File Offset: 0x000F5C6C
	public void StartRangeAttack()
	{
		this.laserArriveSoundPlayed = false;
		if (this.currentPassage == null)
		{
			this.CancelPatternAttack();
			return;
		}
		List<UnitModel> attackTarget = this.GetAttackTarget();
		if (attackTarget.Count != 0)
		{
			UnitModel unitModel = attackTarget[UnityEngine.Random.Range(0, attackTarget.Count)];
			float x = unitModel.GetCurrentViewPosition().x;
			float x2 = this.model.GetCurrentViewPosition().x;
			if (x2 <= x)
			{
				this.movable.SetDirection(UnitDirection.RIGHT);
			}
			else
			{
				this.movable.SetDirection(UnitDirection.LEFT);
			}
			List<UnitModel> directionTarget = this.GetDirectionTarget(this.GetAttackTarget(), 1000f, 0f);
			if (directionTarget.Count == 0)
			{
				return;
			}
			int count = directionTarget.Count;
			List<BossBird.LaserAttackTargetData> list = new List<BossBird.LaserAttackTargetData>();
			int num = 0;
			int num2 = 26;
			foreach (UnitModel unitModel2 in directionTarget)
			{
				list.Add(new BossBird.LaserAttackTargetData
				{
					unit = unitModel2,
					DamageCount = 1,
					savedPosition = unitModel2.GetCurrentViewPosition()
				});
				num2--;
				num++;
				if (num == 26)
				{
					break;
				}
			}
			if (num2 > 0)
			{
				for (int i = 0; i < num2; i++)
				{
					int index = UnityEngine.Random.Range(0, list.Count);
					list[index].DamageCount++;
				}
			}
			this.animScript.StartRangeAttack(list);
		}
		else
		{
			this.animScript.StartRangeAttack(null);
		}
	}

	// Token: 0x06001EBD RID: 7869 RVA: 0x000F7C34 File Offset: 0x000F5E34
	public List<UnitModel> GetDirectionTarget(List<UnitModel> targets, float max_dist, float min_dist)
	{
		List<UnitModel> list = new List<UnitModel>();
		UnitDirection direction = this.movable.GetDirection();
		float x = this.movable.GetCurrentViewPosition().x;
		foreach (UnitModel unitModel in targets)
		{
			float x2 = unitModel.GetCurrentViewPosition().x;
			if (direction == UnitDirection.LEFT)
			{
				if (x2 > x - min_dist)
				{
					continue;
				}
				if (x2 < x - max_dist)
				{
					continue;
				}
			}
			else
			{
				if (x2 < x + min_dist)
				{
					continue;
				}
				if (x2 > x + max_dist)
				{
					continue;
				}
			}
			list.Add(unitModel);
		}
		return list;
	}

	// Token: 0x06001EBE RID: 7870 RVA: 0x000F7D10 File Offset: 0x000F5F10
	public List<UnitModel> GetAttackTarget()
	{
		if (this.currentPassage == null)
		{
			return null;
		}
		MovableObjectNode[] enteredTargets = this.currentPassage.GetEnteredTargets(this.movable);
		List<UnitModel> list = new List<UnitModel>();
		foreach (MovableObjectNode movableObjectNode in enteredTargets)
		{
			UnitModel unit = movableObjectNode.GetUnit();
			CreatureModel creatureModel = unit as CreatureModel;
			if (!list.Contains(unit))
			{
				if (creatureModel == null || !(creatureModel.script is BirdCreatureBase))
				{
					if (unit.IsAttackTargetable())
					{
						list.Add(unit);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06001EBF RID: 7871 RVA: 0x000F7DB8 File Offset: 0x000F5FB8
	public List<UnitModel> GetRangeTarget(Vector3 pos, float half_Range)
	{
		List<UnitModel> attackTarget = this.GetAttackTarget();
		List<UnitModel> list = new List<UnitModel>();
		if (attackTarget.Count == 0)
		{
			return list;
		}
		foreach (UnitModel unitModel in attackTarget)
		{
			if (!(unitModel is StandingItemModel))
			{
				Vector3 currentViewPosition = unitModel.GetCurrentViewPosition();
				if (currentViewPosition.x <= pos.x + half_Range && currentViewPosition.x >= pos.x - half_Range)
				{
					list.Add(unitModel);
				}
			}
		}
		return list;
	}

	// Token: 0x06001EC0 RID: 7872 RVA: 0x000F7E6C File Offset: 0x000F606C
	public void OnLaserParticleArrived(BossBird.LaserAttackTargetData data)
	{
		Vector3 vector = data.savedPosition;
		List<UnitModel> rangeTarget = this.GetRangeTarget(vector, 2f);
		foreach (UnitModel unitModel in rangeTarget)
		{
			unitModel.TakeDamage(this.model, new DamageInfo(this.laserType, (float)BossBird.laserDmg));
			DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(unitModel, this.laserType, this.model);
		}
		GameObject gameObject = this.MakeEffectGlobalPos("Effect/Creature/BossBird/Particle/ExplodeEffect", vector);
		if (!this.laserArriveSoundPlayed)
		{
			SoundEffectPlayer soundEffectPlayer = this.MakeSound("laserExplode");
			soundEffectPlayer.transform.position = vector;
			this.laserArriveSoundPlayed = true;
		}
	}

	// Token: 0x06001EC1 RID: 7873 RVA: 0x00020E22 File Offset: 0x0001F022
	public void ScalePattern()
	{
		if (this.currentPassage == null)
		{
			this.CancelPatternAttack();
			return;
		}
		this.currentAttackType = BossBird.AttackType.SCALE;
		this.animScript.StartCasting();
	}

	// Token: 0x06001EC2 RID: 7874 RVA: 0x00020E48 File Offset: 0x0001F048
	public BossBird.AttackType GetCurrentAttackType()
	{
		return this.currentAttackType;
	}

	// Token: 0x06001EC3 RID: 7875 RVA: 0x00020E50 File Offset: 0x0001F050
	public void BitePattern()
	{
		if (this.currentPassage == null)
		{
			this.CancelPatternAttack();
			return;
		}
		this.currentAttackType = BossBird.AttackType.ULTIMATE;
		this.animScript.StartUltimate();
	}

	// Token: 0x06001EC4 RID: 7876 RVA: 0x00020E76 File Offset: 0x0001F076
	public void AttractPattern()
	{
		if (this.currentPassage == null)
		{
			this.CancelPatternAttack();
			return;
		}
		this.currentAttackType = BossBird.AttackType.ATTRACT;
		this.animScript.StartCasting();
	}

	// Token: 0x06001EC5 RID: 7877 RVA: 0x000F7F40 File Offset: 0x000F6140
	public void OnCastingEnd()
	{
		switch (this.currentAttackType)
		{
		case BossBird.AttackType.ATTRACT:
			this.AttractCastingSuccess();
			break;
		case BossBird.AttackType.SCALE:
			this.ScaleCastingSuccess();
			break;
		case BossBird.AttackType.ULTIMATE:
			this.BiteCastingSuccess();
			break;
		}
	}

	// Token: 0x06001EC6 RID: 7878 RVA: 0x000F7F9C File Offset: 0x000F619C
	private void ScaleCastingSuccess()
	{
		List<WorkerModel> list = new List<WorkerModel>();
		foreach (AgentModel item in AgentManager.instance.GetAgentList())
		{
			list.Add(item);
		}
		foreach (OfficerModel item2 in OfficerManager.instance.GetOfficerList())
		{
			list.Add(item2);
		}
		foreach (WorkerModel workerModel in list)
		{
			if (!workerModel.IsDead() || workerModel.unconAction == null)
			{
				workerModel.SetSpecialDeadScene(WorkerSpine.AnimatorName.LongBirdAgentDead, false, true);
				workerModel.TakeDamage(this.model, new DamageInfo(this.scaleType, (float)BossBird.scaleDmg));
				DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(workerModel, this.scaleType, this.model);
				if (!workerModel.IsDead())
				{
					workerModel.ResetSpecialDeadScene();
				}
			}
		}
	}

	// Token: 0x06001EC7 RID: 7879 RVA: 0x000F8104 File Offset: 0x000F6304
	private void BiteCastingSuccess()
	{
		if (this.currentPassage == null)
		{
			return;
		}
		Vector3 currentViewPosition = this.movable.GetCurrentViewPosition();
		MapNode[] nodeList = this.currentPassage.GetNodeList();
		UnitDirection direction = this.movable.GetDirection();
		List<MapNode> list = new List<MapNode>(nodeList);
		List<MapNode> list2 = list;
		if (BossBird.cache2 == null)
		{
			BossBird.cache2 = new Comparison<MapNode>(MapNode.CompareByX);
		}
		list2.Sort(BossBird.cache2);
		MapNode destNode;
		if (direction == UnitDirection.LEFT)
		{
			destNode = list[0];
		}
		else
		{
			destNode = list[list.Count - 1];
		}
		this.StartUltShoot(destNode);
	}

	// Token: 0x06001EC8 RID: 7880 RVA: 0x00020E9C File Offset: 0x0001F09C
	private void StartUltShoot(MapNode destNode)
	{
		this.ultShooted = true;
		this.animScript.ShootGuts(destNode.GetPosition());
	}

	// Token: 0x06001EC9 RID: 7881 RVA: 0x00020EB6 File Offset: 0x0001F0B6
	public void EndUltShoot()
	{
		this.ultShooted = false;
	}

	// Token: 0x06001ECA RID: 7882 RVA: 0x000F819C File Offset: 0x000F639C
	public void TakeUltDamage()
	{
		List<UnitModel> rangeTarget = this.GetRangeTarget(this.animScript.gutScript.endPos.transform.position, 1f);
		foreach (UnitModel unitModel in rangeTarget)
		{
			unitModel.TakeDamage(this.model, new DamageInfo(this.biteType, (float)BossBird.biteDmg));
			DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(unitModel, this.biteType, this.model);
			GameObject gameObject = this.MakeEffectGlobalPos("Effect/Creature/BossBird/Particle/BossBirdAgentDead", unitModel.GetCurrentViewPosition());
		}
	}

	// Token: 0x06001ECB RID: 7883 RVA: 0x000F8258 File Offset: 0x000F6458
	public List<UnitModel> GetDirectionTarget()
	{
		Vector3 currentViewPosition = this.movable.GetCurrentViewPosition();
		List<UnitModel> list = new List<UnitModel>();
		List<UnitModel> attackTarget = this.GetAttackTarget();
		UnitDirection direction = this.movable.GetDirection();
		float x = currentViewPosition.x;
		foreach (UnitModel unitModel in attackTarget)
		{
			if (!list.Contains(unitModel))
			{
				float x2 = unitModel.GetMovableNode().GetCurrentViewPosition().x;
				if (direction == UnitDirection.LEFT)
				{
					if (x2 <= x)
					{
						list.Add(unitModel);
					}
				}
				else if (x2 >= x)
				{
					list.Add(unitModel);
				}
			}
		}
		return list;
	}

	// Token: 0x06001ECC RID: 7884 RVA: 0x000F8330 File Offset: 0x000F6530
	private void AttractCastingSuccess()
	{
		if (this.currentPassage == null)
		{
			return;
		}
		List<WorkerModel> list = new List<WorkerModel>();
		int attractCnt = BossBird.attractCnt;
		List<WorkerModel> workerList = WorkerManager.instance.GetWorkerList();
		List<WorkerModel> list2 = new List<WorkerModel>();
		foreach (WorkerModel workerModel in workerList)
		{
			AgentModel agentModel = workerModel as AgentModel;
			if (this.attracted.Contains(workerModel))
			{
				list2.Add(workerModel);
			}
			else if (workerModel.IsDead())
			{
				list2.Add(workerModel);
			}
			else if (workerModel.unconAction != null)
			{
				list2.Add(workerModel);
			}
			else if (workerModel.GetMovableNode().GetPassage() == this.currentPassage)
			{
				list2.Add(workerModel);
			}
			else if (agentModel != null)
			{
				if (agentModel.currentSkill != null)
				{
					list2.Add(workerModel);
				}
				else if (agentModel.cannotBeAttackTargetable)
				{
					list2.Add(workerModel);
				}
			}
		}
		foreach (WorkerModel item in list2)
		{
			workerList.Remove(item);
		}
		if (workerList.Count > 0)
		{
			for (int i = 0; i < attractCnt; i++)
			{
				int count = workerList.Count;
				if (count == 0)
				{
					break;
				}
				WorkerModel item2 = workerList[UnityEngine.Random.Range(0, count)];
				list.Add(item2);
				workerList.Remove(item2);
			}
			foreach (WorkerModel workerModel2 in list)
			{
				Uncontrollable_BigBird uncontrollableAction = new Uncontrollable_BigBird(workerModel2, this);
				workerModel2.SetUncontrollableAction(uncontrollableAction);
				this.attracted.Add(workerModel2);
			}
		}
	}

	// Token: 0x06001ECD RID: 7885 RVA: 0x000F8564 File Offset: 0x000F6764
	private BossBird.AttackType GetAttackType()
	{
		int num = 0;
		try
		{
			List<BossBird.AttackTypeData> list = new List<BossBird.AttackTypeData>();
			if (this.smallEgg != null && this.smallEgg.IsEnabled())
			{
				list.Add(new BossBird.AttackTypeData
				{
					type = BossBird.AttackType.ULTIMATE,
					prob = 10
				});
				num += 10;
			}
			if (this.bigEgg != null && this.bigEgg.IsEnabled())
			{
				list.Add(new BossBird.AttackTypeData
				{
					type = BossBird.AttackType.ATTRACT,
					prob = 30
				});
				num += 30;
				list.Add(new BossBird.AttackTypeData
				{
					type = BossBird.AttackType.RANGED,
					prob = 35
				});
				num += 35;
			}
			if (this.longEgg != null && this.longEgg.IsEnabled())
			{
				list.Add(new BossBird.AttackTypeData
				{
					type = BossBird.AttackType.SCALE,
					prob = 25
				});
				num += 25;
			}
			if (list.Count == 0)
			{
				return BossBird.AttackType.NORMAL;
			}
			if (num == 0)
			{
				return BossBird.AttackType.NORMAL;
			}
			int num2 = UnityEngine.Random.Range(0, num);
			int num3 = 0;
			for (int i = 0; i < list.Count; i++)
			{
				BossBird.AttackTypeData attackTypeData = list[i];
				num3 += attackTypeData.prob;
				if (num2 <= num3)
				{
					return attackTypeData.type;
				}
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
		return BossBird.AttackType.NORMAL;
	}

	// Token: 0x06001ECE RID: 7886 RVA: 0x00020EBF File Offset: 0x0001F0BF
	public void WakeUpWorker(WorkerModel target)
	{
		target.ClearEffect();
		target.GetControl();
		target.GetWorkerUnit().animChanger.ChangeAnimator();
	}

	// Token: 0x06001ECF RID: 7887 RVA: 0x00020EDD File Offset: 0x0001F0DD
	public override bool OnAfterSuppressed()
	{
		this.DeleteSound(this.defaultAmbience);
		this.MakeSound("dead");
		this.animScript.OnDie();
		return true;
	}

	// Token: 0x06001ED0 RID: 7888 RVA: 0x00020F03 File Offset: 0x0001F103
	public void OnEggHalfDamage(BossEggBase eggScript)
	{
		this.currentForcelyTeleportTarget = eggScript;
	}

	// Token: 0x06001ED1 RID: 7889 RVA: 0x000F8700 File Offset: 0x000F6900
	public void OnEggBreakDown(BossEggBase eggScript)
	{
		if (this.aliveEggs.Contains(eggScript))
		{
			this.aliveEggs.Remove(eggScript);
			BossBird.NarrationState state;
			if (eggScript == this.bigEgg)
			{
				state = BossBird.NarrationState.BIGBIRD_BREAKDOWN;
				this.SetPassageAlpha(false);
			}
			else if (eggScript == this.smallEgg)
			{
				state = BossBird.NarrationState.SMALLBIRD_BREAKDOWN;
			}
			else
			{
				state = BossBird.NarrationState.LONGBIRD_BREAKDOWN;
				PlaySpeedSettingUI.instance.ReleaseSetting(this);
			}
			this.animScript.DisplayNarration(state, 2f, null);
			this.AfterEggBreakNarration();
			float num = (float)this.model.maxHp * 0.32f;
			this.model.hp -= num;
		}
	}

	// Token: 0x06001ED2 RID: 7890 RVA: 0x00020F0C File Offset: 0x0001F10C
	private void AfterEggBreakNarration()
	{
		if (this.aliveEggs.Count == 0)
		{
			this.animScript.DisplayNarration(BossBird.NarrationState.SUPPRESSED, 6f, new BossBirdAnim.NarrationUI.NarrationEndEvent(this.model.Suppressed));
		}
	}

	// Token: 0x06001ED3 RID: 7891 RVA: 0x000F87A8 File Offset: 0x000F69A8
	private void SetPassageAlpha(bool isDark)
	{
		Sefira[] openedAreaList = PlayerModel.instance.GetOpenedAreaList();
		foreach (Sefira sefira in openedAreaList)
		{
			List<PassageObjectModel> list = new List<PassageObjectModel>(sefira.passageList);
			foreach (PassageObjectModel passageObjectModel in list)
			{
				if (isDark)
				{
					Notice.instance.Send(NoticeName.PassageBlackOut, new object[]
					{
						passageObjectModel
					});
				}
				else
				{
					Notice.instance.Send(NoticeName.PassageWhitle, new object[]
					{
						passageObjectModel
					});
				}
			}
		}
	}

	// Token: 0x06001ED4 RID: 7892 RVA: 0x000F886C File Offset: 0x000F6A6C
	public override SoundEffectPlayer MakeSoundLoop(string src)
	{
		SoundEffectPlayer soundEffectPlayer = null;
		string text;
		if (this.Model.childMetaInfo.soundTable.TryGetValue(src, out text))
		{
			soundEffectPlayer = SoundEffectPlayer.Play(src, this._unit.transform);
			if (soundEffectPlayer != null)
			{
				soundEffectPlayer.AttachToCamera();
			}
		}
		if (soundEffectPlayer == null)
		{
			Debug.Log("Error in finidng sound " + src);
		}
		return soundEffectPlayer;
	}

	// Token: 0x06001ED5 RID: 7893 RVA: 0x000F88DC File Offset: 0x000F6ADC
	public override SoundEffectPlayer MakeSound(string src)
	{
		SoundEffectPlayer soundEffectPlayer = null;
		string filename;
		if (this.Model.childMetaInfo.soundTable.TryGetValue(src, out filename))
		{
			soundEffectPlayer = SoundEffectPlayer.PlayOnce(filename, this._unit.transform.position);
			if (soundEffectPlayer != null)
			{
				soundEffectPlayer.AttachToCamera();
			}
		}
		if (soundEffectPlayer == null)
		{
			Debug.Log("Error in finidng sound " + src);
		}
		return soundEffectPlayer;
	}

	// Token: 0x06001ED6 RID: 7894 RVA: 0x000F8954 File Offset: 0x000F6B54
	public override bool SetCastingSlider(Slider castingSlider)
	{
		if (!this.animScript.CastingTimer.started)
		{
			return false;
		}
		Timer castingTimer = this.animScript.CastingTimer;
		castingSlider.maxValue = castingTimer.maxTime;
		castingSlider.value = castingTimer.elapsed;
		return true;
	}

	// Token: 0x06001ED7 RID: 7895 RVA: 0x00020F41 File Offset: 0x0001F141
	public void OpenBossBirdCollection()
	{
		CreatureInfoWindow.CreateWindow(this.Model.metadataId);
	}

	// Token: 0x06001ED8 RID: 7896 RVA: 0x00020F54 File Offset: 0x0001F154
	public CreatureStaticData.ParameterData GetParam(string key)
	{
		return this.Model.GetBaseMetaInfo().creatureStaticData.GetParam(key);
	}

	// Token: 0x06001ED9 RID: 7897 RVA: 0x00020F6C File Offset: 0x0001F16C
	public string GetParamData(string key)
	{
		return this.GetParam(key).desc;
	}

	// Token: 0x06001EDA RID: 7898 RVA: 0x000F89A0 File Offset: 0x000F6BA0
	public static string GetNarrationKeyByEnum(BossBird.NarrationState nar)
	{
		switch (nar)
		{
		case BossBird.NarrationState.ESCAPE:
			return "Escape";
		case BossBird.NarrationState.BOSS_APPEAR:
			return "BossBirdAppear";
		case BossBird.NarrationState.BIGBIRD_ARRIVED:
			return "BigBirdArrive";
		case BossBird.NarrationState.BIGBIRD_BREAKDOWN:
			return "BigBirdBreak";
		case BossBird.NarrationState.LONGBIRD_ARRIVED:
			return "LongBirdArrive";
		case BossBird.NarrationState.LONGBIRD_BREAKDOWN:
			return "LongBirdBreak";
		case BossBird.NarrationState.SMALLBIRD_ARRIVED:
			return "SmallBirdArrive";
		case BossBird.NarrationState.SMALLBIRD_BREAKDOWN:
			return "SmallBirdBreak";
		case BossBird.NarrationState.SUPPRESSED:
			return "Suppressed";
		default:
			return "Escape";
		}
	}

	// Token: 0x06001EDB RID: 7899 RVA: 0x000F8A18 File Offset: 0x000F6C18
	public static BossBird.NarrationState GetNarrationState(string state)
	{
		string text = state.Trim();
		switch (text)
		{
		case "Escape":
			return BossBird.NarrationState.ESCAPE;
		case "BigBirdArrive":
			return BossBird.NarrationState.BIGBIRD_ARRIVED;
		case "LongBirdArrive":
			return BossBird.NarrationState.LONGBIRD_ARRIVED;
		case "SmallBirdArrive":
			return BossBird.NarrationState.SMALLBIRD_ARRIVED;
		case "BossBirdAppear":
			return BossBird.NarrationState.BOSS_APPEAR;
		case "BigBirdBreak":
			return BossBird.NarrationState.BIGBIRD_BREAKDOWN;
		case "LongBirdBreak":
			return BossBird.NarrationState.LONGBIRD_BREAKDOWN;
		case "SmallBirdBreak":
			return BossBird.NarrationState.SMALLBIRD_BREAKDOWN;
		case "Suppressed":
			return BossBird.NarrationState.SUPPRESSED;
		}
		return BossBird.NarrationState.ESCAPE;
	}

	// Token: 0x06001EDC RID: 7900 RVA: 0x00020F7A File Offset: 0x0001F17A
	public override string GetName()
	{
		return this.model.metaInfo.collectionName;
	}

	// Token: 0x04001EB0 RID: 7856
	public const int LaserCount = 26;

	// Token: 0x04001EB1 RID: 7857
	private const float laserUnitExplodeRad = 2f;

	// Token: 0x04001EB2 RID: 7858
	public const string meleeHit_1_EffectSrc = "Effect/Creature/BossBird/Particle/BossBirdMeleeHit";

	// Token: 0x04001EB3 RID: 7859
	public const string meleeHit_2_Claw_LeftEffectSrc = "Effect/Creature/BossBird/Particle/SlashEffectLeft";

	// Token: 0x04001EB4 RID: 7860
	public const string meleeHit_2_Claw_RightEffectSrc = "Effect/Creature/BossBird/Particle/SlashEffectRight";

	// Token: 0x04001EB5 RID: 7861
	public const string meleeHit_2_Grep_LeftEffectSrc = "Effect/Creature/BossBird/Particle/GrepEffectLeft";

	// Token: 0x04001EB6 RID: 7862
	public const string meleeHit_2_Grep_RightEffectSrc = "Effect/Creature/BossBird/Particle/GrepEffectRight";

	// Token: 0x04001EB7 RID: 7863
	public const string ultShootEffectLeftSrc = "Effect/Creature/BossBird/Particle/GutShootEffectLeft";

	// Token: 0x04001EB8 RID: 7864
	public const string ultShootEffectRightSrc = "Effect/Creature/BossBird/Particle/GutShootEffectRight";

	// Token: 0x04001EB9 RID: 7865
	public const string ultDeadEffecSrc = "Effect/Creature/BossBird/Particle/BossBirdAgentDead";

	// Token: 0x04001EBA RID: 7866
	public const string ultHandLeftSrc = "Effect/Creature/BossBird/Particle/UltShock_Left";

	// Token: 0x04001EBB RID: 7867
	public const string ultHandRightSrc = "Effect/Creature/BossBird/Particle/UltShcok_Right";

	// Token: 0x04001EBC RID: 7868
	public const string attractEffect = "Effect/Creature/BossBird/Particle/BossBirdAttractEffect";

	// Token: 0x04001EBD RID: 7869
	public const string scaleEffect = "Effect/Creature/BossBird/Particle/ScaleEffect";

	// Token: 0x04001EBE RID: 7870
	public const string laserExplode = "Effect/Creature/BossBird/Particle/ExplodeEffect";

	// Token: 0x04001EBF RID: 7871
	private const long traitId = 100038L;

	// Token: 0x04001EC0 RID: 7872
	private const int _laserDmgMin = 8;

	// Token: 0x04001EC1 RID: 7873
	private const int _laserDmgMax = 14;

	// Token: 0x04001EC2 RID: 7874
	private RwbpType laserType = RwbpType.B;

	// Token: 0x04001EC3 RID: 7875
	private const int _scaleDmgMin = 10;

	// Token: 0x04001EC4 RID: 7876
	private const int _scaleDmgMax = 15;

	// Token: 0x04001EC5 RID: 7877
	private RwbpType scaleType = RwbpType.P;

	// Token: 0x04001EC6 RID: 7878
	private const int _biteDmgMin = 50;

	// Token: 0x04001EC7 RID: 7879
	private const int _biteDmgMax = 115;

	// Token: 0x04001EC8 RID: 7880
	private RwbpType biteType = RwbpType.R;

	// Token: 0x04001EC9 RID: 7881
	private const int _attackDmgMin = 23;

	// Token: 0x04001ECA RID: 7882
	private const int _attackDmgMax = 43;

	// Token: 0x04001ECB RID: 7883
	private RwbpType attackType = RwbpType.B;

	// Token: 0x04001ECC RID: 7884
	private const int _attractMin = 3;

	// Token: 0x04001ECD RID: 7885
	private const int _attractMax = 5;

	// Token: 0x04001ECE RID: 7886
	private const float attackDelayMin = 4f;

	// Token: 0x04001ECF RID: 7887
	private const float attackDelayMax = 6f;

	// Token: 0x04001ED0 RID: 7888
	private const int attractPatternProb = 30;

	// Token: 0x04001ED1 RID: 7889
	private const int laserPatternProb = 35;

	// Token: 0x04001ED2 RID: 7890
	private const int scalePatternProb = 25;

	// Token: 0x04001ED3 RID: 7891
	private const int bitePatternProb = 10;

	// Token: 0x04001ED4 RID: 7892
	private const float patternDelay = 15f;

	// Token: 0x04001ED5 RID: 7893
	private const float teleportDelayMin = 15f;

	// Token: 0x04001ED6 RID: 7894
	private const float teleportDelayMax = 25f;

	// Token: 0x04001ED7 RID: 7895
	private const float forcelyTeleportDelayMin = 10f;

	// Token: 0x04001ED8 RID: 7896
	private const float forcelyTteleportDelayMax = 15f;

	// Token: 0x04001ED9 RID: 7897
	private const float uniquePatternCheckTime = 5f;

	// Token: 0x04001EDA RID: 7898
	private const int _boss_weapon_id = 200038;

	// Token: 0x04001EDB RID: 7899
	private const int _boss_armor_id = 300038;

	// Token: 0x04001EDC RID: 7900
	public const int _boss_gift_id = 400038;

	// Token: 0x04001EDD RID: 7901
	private Timer forcelyTeleportTimer = new Timer();

	// Token: 0x04001EDE RID: 7902
	private Timer teleportTimer = new Timer();

	// Token: 0x04001EDF RID: 7903
	private Timer normalAttackTimer = new Timer();

	// Token: 0x04001EE0 RID: 7904
	private Timer specialPatternTimer = new Timer();

	// Token: 0x04001EE1 RID: 7905
	private Timer uniquePattern = new Timer();

	// Token: 0x04001EE2 RID: 7906
	private bool forcelyTeleportReady;

	// Token: 0x04001EE3 RID: 7907
	private bool teleportReady;

	// Token: 0x04001EE4 RID: 7908
	private bool normalAttackReady;

	// Token: 0x04001EE5 RID: 7909
	private bool patternReady;

	// Token: 0x04001EE6 RID: 7910
	private bool forcelyTeleport;

	// Token: 0x04001EE7 RID: 7911
	private bool activateState = true;

	// Token: 0x04001EE8 RID: 7912
	private bool isEscaped;

	// Token: 0x04001EE9 RID: 7913
	private bool isTeleporting;

	// Token: 0x04001EEA RID: 7914
	private bool isNarration;

	// Token: 0x04001EEB RID: 7915
	private bool ultShooted;

	// Token: 0x04001EEC RID: 7916
	private bool laserArriveSoundPlayed;

	// Token: 0x04001EED RID: 7917
	private Queue<IBirdControl> waitQueue = new Queue<IBirdControl>();

	// Token: 0x04001EEE RID: 7918
	private List<IBirdControl> birdControl = new List<IBirdControl>();

	// Token: 0x04001EEF RID: 7919
	private List<IBirdControl> notArrived = new List<IBirdControl>();

	// Token: 0x04001EF0 RID: 7920
	private List<WorkerModel> attracted = new List<WorkerModel>();

	// Token: 0x04001EF1 RID: 7921
	private List<CreatureBase> escapedCreatures = new List<CreatureBase>();

	// Token: 0x04001EF2 RID: 7922
	private MapNode currentTeleportDest;

	// Token: 0x04001EF3 RID: 7923
	private SmallBird smallBird;

	// Token: 0x04001EF4 RID: 7924
	private BigBird bigBird;

	// Token: 0x04001EF5 RID: 7925
	private LongBird longBird;

	// Token: 0x04001EF6 RID: 7926
	private ChildCreatureUnit _unit;

	// Token: 0x04001EF7 RID: 7927
	private MapNode gatewayNode;

	// Token: 0x04001EF8 RID: 7928
	private BossBird.AttackType currentAttackType;

	// Token: 0x04001EF9 RID: 7929
	private BossBird.Phase _currentPhase;

	// Token: 0x04001EFA RID: 7930
	private BossBirdEvent eventScript = new BossBirdEvent();

	// Token: 0x04001EFB RID: 7931
	private BossGateWay gateWay;

	// Token: 0x04001EFC RID: 7932
	private BossEggBase bigEgg;

	// Token: 0x04001EFD RID: 7933
	private BossEggBase longEgg;

	// Token: 0x04001EFE RID: 7934
	private BossEggBase smallEgg;

	// Token: 0x04001EFF RID: 7935
	private BossEggBase currentForcelyTeleportTarget;

	// Token: 0x04001F00 RID: 7936
	private List<BossEggBase> aliveEggs = new List<BossEggBase>();

	// Token: 0x04001F01 RID: 7937
	private SoundEffectPlayer defaultAmbience;

	// Token: 0x04001F02 RID: 7938
	[CompilerGenerated]
	private static Comparison<MapNode> cache0;

	// Token: 0x04001F03 RID: 7939
	[CompilerGenerated]
	private static Comparison<MapNode> cache1;

	// Token: 0x04001F04 RID: 7940
	[CompilerGenerated]
	private static Comparison<MapNode> cache2;

	// Token: 0x020003BC RID: 956
	public class LaserAttackTargetData
	{
		// Token: 0x04001F06 RID: 7942
		public UnitModel unit;

		// Token: 0x04001F07 RID: 7943
		public int DamageCount;

		// Token: 0x04001F08 RID: 7944
		public Vector2 savedPosition;
	}

	// Token: 0x020003BD RID: 957
	private class AttackTypeData
	{
		// Token: 0x04001F09 RID: 7945
		public BossBird.AttackType type;

		// Token: 0x04001F0A RID: 7946
		public int prob;
	}

	// Token: 0x020003BE RID: 958
	private class NarrationKey
	{
		// Token: 0x04001F0B RID: 7947
		public const string Escape = "Escape";

		// Token: 0x04001F0C RID: 7948
		public const string BigBirdArrive = "BigBirdArrive";

		// Token: 0x04001F0D RID: 7949
		public const string LongBirdArrive = "LongBirdArrive";

		// Token: 0x04001F0E RID: 7950
		public const string SmallBirdArrive = "SmallBirdArrive";

		// Token: 0x04001F0F RID: 7951
		public const string BossBirdAppear = "BossBirdAppear";

		// Token: 0x04001F10 RID: 7952
		public const string BigEggBreak = "BigBirdBreak";

		// Token: 0x04001F11 RID: 7953
		public const string LongBirdBreak = "LongBirdBreak";

		// Token: 0x04001F12 RID: 7954
		public const string SmallBirdBreak = "SmallBirdBreak";

		// Token: 0x04001F13 RID: 7955
		public const string Suppressed = "Suppressed";

		// Token: 0x04001F14 RID: 7956
		public const float LongTime = 6f;

		// Token: 0x04001F15 RID: 7957
		public const float MiddleTime = 4f;

		// Token: 0x04001F16 RID: 7958
		public const float ShortTime = 2f;
	}

	// Token: 0x020003BF RID: 959
	public class SoundKey
	{
		// Token: 0x04001F17 RID: 7959
		public const string attack1Down = "attack1Down";

		// Token: 0x04001F18 RID: 7960
		public const string attack1Move = "attack1Move";

		// Token: 0x04001F19 RID: 7961
		public const string attack1Swing = "attack1Swing";

		// Token: 0x04001F1A RID: 7962
		public const string attack2Grab = "attack2Grab";

		// Token: 0x04001F1B RID: 7963
		public const string attack2Move = "attack2Move";

		// Token: 0x04001F1C RID: 7964
		public const string attack2Swing = "attack2Swing";

		// Token: 0x04001F1D RID: 7965
		public const string attract = "attract";

		// Token: 0x04001F1E RID: 7966
		public const string bigEgg = "bigEgg";

		// Token: 0x04001F1F RID: 7967
		public const string appear = "appear";

		// Token: 0x04001F20 RID: 7968
		public const string appear_origin = "appear_origin";

		// Token: 0x04001F21 RID: 7969
		public const string casting = "casting";

		// Token: 0x04001F22 RID: 7970
		public const string dead = "dead";

		// Token: 0x04001F23 RID: 7971
		public const string @default = "default";

		// Token: 0x04001F24 RID: 7972
		public const string laserExplode = "laserExplode";

		// Token: 0x04001F25 RID: 7973
		public const string laserFire = "laserFire";

		// Token: 0x04001F26 RID: 7974
		public const string laserCast = "laserCast";

		// Token: 0x04001F27 RID: 7975
		public const string laserMove = "laserMove";

		// Token: 0x04001F28 RID: 7976
		public const string longEgg = "longEgg";

		// Token: 0x04001F29 RID: 7977
		public const string move = "move";

		// Token: 0x04001F2A RID: 7978
		public const string smallEgg = "smallEgg";

		// Token: 0x04001F2B RID: 7979
		public const string ultAir = "ultAir";

		// Token: 0x04001F2C RID: 7980
		public const string ultMouth1 = "ultMouth1";

		// Token: 0x04001F2D RID: 7981
		public const string ultMouth2 = "ultMouth2";

		// Token: 0x04001F2E RID: 7982
		public const string ultMove1 = "ultMove1";

		// Token: 0x04001F2F RID: 7983
		public const string ultMove2 = "ultMove2";

		// Token: 0x04001F30 RID: 7984
		public const string ultMove3 = "ultMove3";

		// Token: 0x04001F31 RID: 7985
		public const string Attack1 = "Attack1";

		// Token: 0x04001F32 RID: 7986
		public const string Scale = "Scale";

		// Token: 0x04001F33 RID: 7987
		public const string Ult = "Ult";

		// Token: 0x04001F34 RID: 7988
		public const string Attack2 = "Attack2";
	}

	// Token: 0x020003C0 RID: 960
	public enum NarrationState
	{
		// Token: 0x04001F36 RID: 7990
		ESCAPE,
		// Token: 0x04001F37 RID: 7991
		BOSS_APPEAR,
		// Token: 0x04001F38 RID: 7992
		BIGBIRD_ARRIVED,
		// Token: 0x04001F39 RID: 7993
		BIGBIRD_BREAKDOWN,
		// Token: 0x04001F3A RID: 7994
		LONGBIRD_ARRIVED,
		// Token: 0x04001F3B RID: 7995
		LONGBIRD_BREAKDOWN,
		// Token: 0x04001F3C RID: 7996
		SMALLBIRD_ARRIVED,
		// Token: 0x04001F3D RID: 7997
		SMALLBIRD_BREAKDOWN,
		// Token: 0x04001F3E RID: 7998
		SUPPRESSED
	}

	// Token: 0x020003C1 RID: 961
	public enum LaserMode
	{
		// Token: 0x04001F40 RID: 8000
		ONLY_ONE,
		// Token: 0x04001F41 RID: 8001
		MULTIPLE
	}

	// Token: 0x020003C2 RID: 962
	public enum OtherBirdState
	{
		// Token: 0x04001F43 RID: 8003
		NORMAL,
		// Token: 0x04001F44 RID: 8004
		MOVETOGATE
	}

	// Token: 0x020003C3 RID: 963
	public enum AttackType
	{
		// Token: 0x04001F46 RID: 8006
		NORMAL,
		// Token: 0x04001F47 RID: 8007
		RANGED,
		// Token: 0x04001F48 RID: 8008
		ATTRACT,
		// Token: 0x04001F49 RID: 8009
		SCALE,
		// Token: 0x04001F4A RID: 8010
		ULTIMATE
	}

	// Token: 0x020003C4 RID: 964
	public enum Phase
	{
		// Token: 0x04001F4C RID: 8012
		NOTACTIVATED,
		// Token: 0x04001F4D RID: 8013
		IDLE,
		// Token: 0x04001F4E RID: 8014
		TELEPORT,
		// Token: 0x04001F4F RID: 8015
		ATTACK,
		// Token: 0x04001F50 RID: 8016
		SPECIAL_PATTERN
	}
}
