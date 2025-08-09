/*
public static int CalculateStatLevelForCustomizing(int stat) // 
public int temperanceStat // Take the averge of SR and WS istead of just SR
public int justiceStat // Take the averge of AS and MS istead of just AS
public override DefenseInfo defense // Change the way Chesed/Binah/Kether service bonuses are handled
public void Init() // 
public static WorkerPrimaryStat GetDefaultStat() // Hod research fix
public override void OnStageStart() // Overtime Research
public override void OnStageEnd() // Clear Work Order Queue
public override void OnFixedUpdate() // Overtime Yesod Suppression
public float GetMovementValue() // Fixed Malkuth's suppression reward
public void UpdateTitle(int oldLevel) // Squash an issue that the new title bonuses created; change to pseudo-random
public int GetContinuousServiceLevel() // Overtime Research
public int GetFortitudeStatBySefiraAbility() // Overtime Research
public int GetPrudenceStatBySefiraAbility() // Overtime Research
public int GetTemperanceStatBySefiraAbility() // Overtime Research
public int GetJusticeStatBySefiraAbility() // Overtime Research
public int GetAttackSpeedBufBySefiraAbility() // Overtime Research
public int GetWorkProbBufBySefiraAbility() // Overtime Research
public int GetMovementBufBySefiraAbility() // Overtime Research
public override float GetDamageFactorBySefiraAbility() // Kether service bonuses
-public override void RecoverHP(float amount) // 
+public override float RecoverHPv2(float amount) // 
-public override void RecoverMental(float amount) // 
+public override float RecoverMentalv2(float amount) // 
+public void EnqueueWorkOrder(QueuedWorkOrder order) // 
+public void DequeueWorkOrder() // 
+public void DequeueWorkOrder(QueuedWorkOrder order) // 
+public bool CanQueueWorkOrder() // 
+public void ClearWorkOrderQueue() // 
+public List<QueuedWorkOrder> GetWorkOrderQueue() // 
+private List<QueuedWorkOrder> _workOrderQueue // 
+public void ForcelyChangePrefix(int title) // 
+public void ForcelyChangeSuffix(int title) // 
+public bool ForceHideUI // 
+private bool forceHideUI // 
*/
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CommandWindow;
using UnityEngine;
using UnityEngine.UI;
using WorkerSprite;

[Serializable]
public class AgentModel : WorkerModel
{
	public AgentModel(long id)
	{
		this.workerClass = WorkerClass.AGENT;
		this.movableNode = new MovableObjectNode(this);
		this.commandQueue = new WorkerCommandQueue(this);
		this.skillInfos = new List<AgentModel.SkillInfo>();
		this.instanceId = id;
		base.currentSefira = "0";
		this.activated = false;
		this.movableNode.SetCurrentNode(MapGraph.instance.GetSepiraNodeByRandom("Malkut"));
		this.history = new AgentHistory();
		this.SetFaction(FactionTypeList.StandardFaction.Worker);
		this._eventHandler = new AgentModelEventHandler();
		this.spriteData = new WorkerSprite.WorkerSprite();
		this.Init();
	}

	// <Mod>
	public AgentModel(long id, int[] customTitles)
	{
		for (int i = 0; i < 4; i++)
		{
			forceTitles[i] = Customizing.CustomizingWindow.GetTitleData(i, customTitles[i]);
		}
		this.workerClass = WorkerClass.AGENT;
		this.movableNode = new MovableObjectNode(this);
		this.commandQueue = new WorkerCommandQueue(this);
		this.skillInfos = new List<AgentModel.SkillInfo>();
		this.instanceId = id;
		base.currentSefira = "0";
		this.activated = false;
		this.movableNode.SetCurrentNode(MapGraph.instance.GetSepiraNodeByRandom("Malkut"));
		this.history = new AgentHistory();
		this.SetFaction(FactionTypeList.StandardFaction.Worker);
		this._eventHandler = new AgentModelEventHandler();
		this.spriteData = new WorkerSprite.WorkerSprite();
		this.Init();
	}

	public int level
	{
		get
		{
			int num;
			if (SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.HOD, false))
			{
				num = this.fortitudeLevel + this.prudenceLevel + this.temperanceLevel + this.justiceLevel;
			}
			else
			{
				num = this.originFortitudeLevel + this.originPrudenceLevel + this.originTemperanceLevel + this.originJusticeLevel;
			}
			if (num < 6)
			{
				return 1;
			}
			if (num < 9)
			{
				return 2;
			}
			if (num < 12)
			{
				return 3;
			}
			if (num < 16)
			{
				return 4;
			}
			return 5;
		}
	}

	public int Rstat
	{
		get
		{
			return this.fortitudeLevel;
		}
	}

	public int Wstat
	{
		get
		{
			return this.prudenceLevel;
		}
	}

	public int Bstat
	{
		get
		{
			return this.temperanceLevel;
		}
	}

	public int Pstat
	{
		get
		{
			return this.justiceLevel;
		}
	}

	public int StatLevel(RwbpType type)
	{
		switch (type)
		{
		case RwbpType.R:
			return this.Rstat;
		case RwbpType.W:
			return this.Wstat;
		case RwbpType.B:
			return this.Bstat;
		case RwbpType.P:
			return this.Pstat;
		default:
			return this.Rstat;
		}
	}

	public static int CalculateStatLevel(int stat)
	{
		if (stat < 30)
		{
			return 1;
		}
		if (stat < 45)
		{
			return 2;
		}
		if (stat < 65)
		{
			return 3;
		}
		if (stat < 85)
		{
			return 4;
		}
		return 5;
	}

	public static int CalculateStatLevelForCustomizing(int stat)
	{ // <Mod>
		if (stat < 30)
		{
			return 1;
		}
		if (stat < 45)
		{
			return 2;
		}
		if (stat < 65)
		{
			return 3;
		}
		if (stat < 85)
		{
			return 4;
		}
		if (stat < 110)
		{
			return 5;
		}
		return 6;
	}

	public override int fortitudeLevel
	{
		get
		{
			return AgentModel.CalculateStatLevel(this.fortitudeStat);
		}
	}

	public int originFortitudeLevel
	{
		get
		{
			return AgentModel.CalculateStatLevel(this.originFortitudeStat);
		}
	}

	public int fortitudeStat
	{
		get
		{
			return this.primaryStat.hp + this.titleBonus.hp + base.GetPrimaryStatBuf().hp + base.GetEGObonus().hp;
		}
	}

	public int originFortitudeStat
	{
		get
		{
			return this.primaryStat.hp + this.titleBonus.hp;
		}
	}

	public override int prudenceLevel
	{
		get
		{
			return AgentModel.CalculateStatLevel(this.prudenceStat);
		}
	}

	public int originPrudenceLevel
	{
		get
		{
			return AgentModel.CalculateStatLevel(this.originPrudenceStat);
		}
	}

	public int prudenceStat
	{
		get
		{
			return this.primaryStat.mental + this.titleBonus.mental + base.GetPrimaryStatBuf().mental + base.GetEGObonus().mental;
		}
	}

	public int originPrudenceStat
	{
		get
		{
			return this.primaryStat.mental + this.titleBonus.mental;
		}
	}

	public override int temperanceLevel
	{
		get
		{
			return AgentModel.CalculateStatLevel(this.temperanceStat);
		}
	}

	public int originTemperanceLevel
	{
		get
		{
			return AgentModel.CalculateStatLevel(this.originTemperanceStat);
		}
	}

	public int temperanceStat
	{ // <Mod> takes the average of SR and WS for EGO gifts instead of just SR
		get
		{
			return this.primaryStat.work + this.titleBonus.work + base.GetPrimaryStatBuf().work + (base.GetEGObonus().workProb + base.GetEGObonus().cubeSpeed) / 2;
		}
	}

	public int originTemperanceStat
	{
		get
		{
			return this.primaryStat.work + this.titleBonus.work;
		}
	}

	public override int justiceLevel
	{
		get
		{
			return AgentModel.CalculateStatLevel(this.justiceStat);
		}
	}

	public int originJusticeLevel
	{
		get
		{
			return AgentModel.CalculateStatLevel(this.originJusticeStat);
		}
	}

	public int justiceStat
	{ // <Mod> takes the average of AS and MS for EGO gifts instead of AS + MS
		get
		{
			return this.primaryStat.battle + this.titleBonus.battle + base.GetPrimaryStatBuf().battle + (base.GetEGObonus().attackSpeed + base.GetEGObonus().movement) / 2;
		}
	}

	public int originJusticeStat
	{
		get
		{
			return this.primaryStat.battle + this.titleBonus.battle;
		}
	}

	public override int maxHp
	{
		get
		{
			int num = this.primaryStat.maxHP;
			num += base.GetMaxHpBuf();
			num += base.GetEGObonus().hp;
			num += base.GetPrimaryStatBuf().maxHP;
			num += this.titleBonus.hp;
			num += this.GetFortitudeStatBySefiraAbility();
			if (num < 1)
			{
				num = 1;
			}
			return num;
		}
	}

	public override int maxMental
	{
		get
		{
			int num = this.primaryStat.maxMental;
			num += base.GetMaxMentalBuf();
			num += base.GetEGObonus().mental;
			num += base.GetPrimaryStatBuf().maxMental;
			num += this.titleBonus.mental;
			num += this.GetPrudenceStatBySefiraAbility();
			if (num < 1)
			{
				num = 1;
			}
			return num;
		}
	}

	public override float movement
	{
		get
		{
			float num = (float)this.primaryStat.movementSpeed;
			num += base.GetMovementBuf();
			num += (float)base.GetEGObonus().movement;
			num += (float)base.GetPrimaryStatBuf().movementSpeed;
			num += (float)this.titleBonus.movementSpeed;
			num += (float)this.GetMovementBufBySefiraAbility();
			num += (float)this.GetJusticeStatBySefiraAbility();
			if (base.CurrentPanicAction != null)
			{
				num *= base.CurrentPanicAction.GetMovementMultiplier();
			}
			return num;
		}
	}

	public override int regeneration
	{
		get
		{
			return 20 + ResearchDataModel.instance.GetAgentStatBonus().regeneration;
		}
	}

	public override float regenerationDelay
	{
		get
		{
			return 10f - ResearchDataModel.instance.GetAgentStatBonus().regenerationDelay;
		}
	}

	public int regenerationMental
	{
		get
		{
			return 20 + ResearchDataModel.instance.GetAgentStatBonus().regenerationMental;
		}
	}

	public float regenerationMentalDelay
	{
		get
		{
			return 10f - ResearchDataModel.instance.GetAgentStatBonus().regenerationMentalDelay;
		}
	}

	public override int physicalDefense
	{
		get
		{
			return ResearchDataModel.instance.GetAgentStatBonus().physicalDefense;
		}
	}

	public override int mentalDefense
	{
		get
		{
			return ResearchDataModel.instance.GetAgentStatBonus().mentalDefense;
		}
	}

	public int workSpeed
	{
		get
		{
			return this.primaryStat.cubeSpeed + base.GetCubeSpeedBuf() + base.GetEGObonus().cubeSpeed + base.GetPrimaryStatBuf().cubeSpeed + this.titleBonus.cubeSpeed + this.GetTemperanceStatBySefiraAbility();
		}
	}

	public int workProb
	{
		get
		{
			return this.primaryStat.workProb + base.GetWorkProbBuf() + base.GetEGObonus().workProb + base.GetPrimaryStatBuf().workProb + this.titleBonus.workProb + this.GetWorkProbBufBySefiraAbility() + this.GetTemperanceStatBySefiraAbility();
		}
	}

	public override float attackSpeed
	{
		get
		{
			float num = (float)this.primaryStat.attackSpeed;
			num += base.GetAttackSpeedBuf();
			num += (float)base.GetEGObonus().attackSpeed;
			num += (float)base.GetPrimaryStatBuf().attackSpeed;
			num += (float)this.titleBonus.attackSpeed;
			num += (float)this.GetAttackSpeedBufBySefiraAbility();
			num += (float)this.GetJusticeStatBySefiraAbility();
			if (base.CurrentPanicAction != null)
			{
				num *= base.CurrentPanicAction.GetAttackSpeedMultiplier();
			}
			return num;
		}
	}

	public override DefenseInfo defense
	{ // <Mod> Chesed/Binah service bonuses now act as / (1 + num) instead of * (1 - num); Kether service bonuses
		get
		{
			DefenseInfo defenseInfo = new DefenseInfo();
			if (this._equipment.armor != null)
			{
				DefenseInfo defense = this._equipment.armor.GetDefense(this);
				defenseInfo.R = defense.R;
				defenseInfo.W = defense.W;
				defenseInfo.B = defense.B;
				defenseInfo.P = defense.P;
				float num3 = EGOrealizationManager.instance.ArmorUpgrade(this._equipment.armor.metaInfo);
				defenseInfo.R -= num3;
				defenseInfo.W -= num3;
				defenseInfo.B -= num3;
				defenseInfo.P -= num3;
			}
			else
			{
				defenseInfo.R = 1f;
				defenseInfo.W = 1f;
				defenseInfo.B = 1.5f;
				defenseInfo.P = 2f;
			}
			defenseInfo.R *= this.additionalDef.R;
			defenseInfo.W *= this.additionalDef.W;
			defenseInfo.B *= this.additionalDef.B;
			defenseInfo.P *= this.additionalDef.P;
			if (this.currentSefiraEnum == SefiraEnum.CHESED)
			{
				float num = 1f - 1f / (1f + (float)SefiraAbilityValueInfo.chesedContinuousServiceValues[this.GetContinuousServiceLevel() - 1] / 100f);
				num = Mathf.Clamp01(num);
				defenseInfo.R -= Mathf.Max(0f, defenseInfo.R * num);
				defenseInfo.W -= Mathf.Max(0f, defenseInfo.W * num);
				defenseInfo.B -= Mathf.Max(0f, defenseInfo.B * num);
				defenseInfo.P -= Mathf.Max(0f, defenseInfo.P * num);
			}
			if (this.currentSefiraEnum == SefiraEnum.BINAH)
			{
				float num2 = 1f - 1f / (1f + (float)SefiraAbilityValueInfo.binahContinuousServiceValues[this.GetContinuousServiceLevel() - 1] / 100f);
				num2 = Mathf.Clamp01(num2);
				defenseInfo.R -= Mathf.Max(0f, defenseInfo.R * num2);
				defenseInfo.W -= Mathf.Max(0f, defenseInfo.W * num2);
				defenseInfo.B -= Mathf.Max(0f, defenseInfo.B * num2);
				defenseInfo.P -= Mathf.Max(0f, defenseInfo.P * num2);
			}
			if (currentSefiraEnum == SefiraEnum.KETHER)
			{
				float num2 = 1f - 1f / (1f + (float)SefiraAbilityValueInfo.ketherContinuousServiceValues2[GetContinuousServiceLevel() - 1] / 100f);
				num2 = Mathf.Clamp01(num2);
				defenseInfo.R -= Mathf.Max(0f, defenseInfo.R * num2);
				defenseInfo.W -= Mathf.Max(0f, defenseInfo.W * num2);
				defenseInfo.B -= Mathf.Max(0f, defenseInfo.B * num2);
				defenseInfo.P -= Mathf.Max(0f, defenseInfo.P * num2);
			}
			if (base.CurrentPanicAction != null)
			{
				float defenseMultiplier = base.CurrentPanicAction.GetDefenseMultiplier();
				defenseInfo.R *= defenseMultiplier;
				defenseInfo.W *= defenseMultiplier;
				defenseInfo.B *= defenseMultiplier;
				defenseInfo.P *= defenseMultiplier;
			}
			return defenseInfo;
		}
	}

	public WorkerPrimaryStatBonus titleBonus
	{
		get
		{
			WorkerPrimaryStatBonus workerPrimaryStatBonus = new WorkerPrimaryStatBonus();
			if (this.prefix != null)
			{
				workerPrimaryStatBonus.hp += this.prefix.hp;
				workerPrimaryStatBonus.mental += this.prefix.mental;
				workerPrimaryStatBonus.work += this.prefix.workProb;
				workerPrimaryStatBonus.battle += this.prefix.attackSpeed;
			}
			if (this.suffix != null)
			{
				workerPrimaryStatBonus.hp += this.suffix.hp;
				workerPrimaryStatBonus.mental += this.suffix.mental;
				workerPrimaryStatBonus.work += this.suffix.workProb;
				workerPrimaryStatBonus.battle += this.suffix.attackSpeed;
			}
			return workerPrimaryStatBonus;
		}
	}

	public UseSkill currentSkill
	{
		get
		{
			return this._currentSkill;
		}
		set
		{
			this._currentSkill = value;
			if (value != null)
			{
				this._unit.SelectIconForcelyEnable();
				return;
			}
			this._unit.SelectIconDisable();
		}
	}

	public AgentModel.CheckCommandState CheckWorkCommand
	{
		get
		{
			if (this._workCommand == null)
			{
				if (AgentModel.f__mgcache0 == null)
				{
					AgentModel.f__mgcache0 = new AgentModel.CheckCommandState(AgentModel.DummyCheckCommand);
				}
				return AgentModel.f__mgcache0;
			}
			return this._workCommand;
		}
	}

	public AgentModel.CheckCommandState CheckSuppressCommand
	{
		get
		{
			if (this._suppressCommand == null)
			{
				if (AgentModel.f__mgcache1 == null)
				{
					AgentModel.f__mgcache1 = new AgentModel.CheckCommandState(AgentModel.DummyCheckCommand);
				}
				return AgentModel.f__mgcache1;
			}
			return this._suppressCommand;
		}
	}

	public AgentModelEventHandler eventHandler
	{
		get
		{
			return this._eventHandler;
		}
	}

	private AgentAIState state
	{
		get
		{
			return this._state;
		}
		set
		{
			this._state = value;
		}
	}

	public RwbpType bestRwbp
	{
		get
		{
			if (this.forcelyPanicType != RwbpType.N)
			{
				return this.forcelyPanicType;
			}
			return this._bestRwbp;
		}
	}

	public bool canCancelCurrentWork
	{
		get
		{
			return this.currentSkill == null && this.state == AgentAIState.MANAGE;
		}
	}

	public bool IsAutoSuppressing
	{
		get
		{
			return this._isAutoSuppressing;
		}
	}

	public void Init()
	{ // <Mod>
		base.SetWeapon(WeaponModel.GetDummyWeapon());
		base.SetArmor(ArmorModel.GetDummyArmor());
		this.InitTitle();
		this.primaryStat = AgentModel.GetDefaultStat();
		this.InitSkills();
		this.panicReport = false;
		this.UpdateTitle(1, false);
		this._isAutoSuppressing = false;
	}

	public void InitTitle()
	{ // <Mod>
		if (forceTitles[0] != 0)
		{
			AgentTitleTypeInfo title = AgentTitleTypeList.instance.GetData(forceTitles[0]);
			if (title != null)
			{
				this.prefix = title;
				this.suffix = AgentTitleTypeList.instance.GetDataSuffix(this, 1, true);
				return;
			}
		}
		this.prefix = AgentTitleTypeList.instance.GetDataPrefix(this, 1, true);
		this.suffix = AgentTitleTypeList.instance.GetDataSuffix(this, 1, true);
	}

	public static int GetDefaultLevel1Stat()
	{
		int num = 15;
		int num2 = 0;
		if (ResearchDataModel.instance.IsUpgradedAbility("agent_start_stat"))
		{
			num2 += 5;
		}
		return num + num2;
	}

	public static WorkerPrimaryStat GetDefaultStat()
	{ // <Mod> fix Hod's increase new hire stat research that used to not work
	// also may nerf Hod's suppression reward from 30 to 15, but only once I have an overtime Hod suppression reward implemented to make up the difference
		WorkerPrimaryStat workerPrimaryStat = new WorkerPrimaryStat();
		int num = 0;
		if (ResearchDataModel.instance.IsUpgradedAbility("agent_start_stat"))
		{
			num += 5;
		}
		if (MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.HOD))
		{
			num += SpecialModeConfig.instance.GetValue<bool>("OvertimeMissions") ? 15 : 30;
		}
		if (MissionManager.instance.ExistsFinishedOvertimeBossMission(SefiraEnum.HOD))
		{
			num += 15;
		}
		workerPrimaryStat.hp = 15 + num;
		workerPrimaryStat.mental = 15 + num;
		workerPrimaryStat.work = 15 + num;
		workerPrimaryStat.battle = 15 + num;
		return workerPrimaryStat;
	}

	public void InitSkills()
	{
		List<int> list = new List<int>(new int[]
		{
			1,
			2,
			3,
			4
		});
		for (int i = 0; i < 4; i++)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			int num = list[index];
			list.RemoveAt(index);
			this.skillInfos.Add(new AgentModel.SkillInfo(SkillTypeList.instance.GetData((long)num)));
		}
	}

	public void InitSkills(params SkillTypeInfo[] skills)
	{
		this.skillInfos.Clear();
		foreach (SkillTypeInfo info in skills)
		{
			this.skillInfos.Add(new AgentModel.SkillInfo(info));
		}
	}

	public override Dictionary<string, object> GetSaveData()
	{
		Dictionary<string, object> saveData = base.GetSaveData();
		saveData.Add("history", this.history.GetSaveData());
		saveData.Add("iscustom", this.iscustom);
		if (this.iscustom)
		{
			saveData.Add("customName", this.name);
		}
		saveData.Add("nameId", this._agentName.id);
		saveData.Add("isUniqueCredit", this.isUniqueCredit);
		saveData.Add("uniqueScriptIndex", this.uniqueScriptIndex);
		saveData.Add("spriteSet", this.spriteData.saveData);
		saveData.Add("primaryStat", this.primaryStat);
		if (this.prefix != null)
		{
			saveData.Add("prefix", this.prefix.id);
		}
		if (this.suffix != null)
		{
			saveData.Add("suffix", this.suffix.id);
		}
		if (this._equipment.weapon != null)
		{
			saveData.Add("weaponId", this._equipment.weapon.instanceId);
		}
		if (this._equipment.armor != null)
		{
			saveData.Add("armorId", this._equipment.armor.instanceId);
		}
		saveData.Add("gifts", this._equipment.gifts.GetSaveData());
		saveData.Add("lastServiceSefira", this.lastServiceSefira);
		saveData.Add("continuousServiceDay", this.continuousServiceDay);
		saveData.Add("isAce", this.isAce);
		return saveData;
	}

	public override void LoadData(Dictionary<string, object> dic)
	{
		base.LoadData(dic);
		this.history = new AgentHistory();
		Dictionary<string, object> dic2 = null;
		if (WorkerModel.TryGetValue<Dictionary<string, object>>(dic, "history", ref dic2))
		{
			this.history.LoadData(dic2);
		}
		WorkerModel.TryGetValue<bool>(dic, "iscustom", ref this.iscustom);
		WorkerModel.TryGetValue<bool>(dic, "isUniqueCredit", ref this.isUniqueCredit);
		if (!WorkerModel.TryGetValue<int>(dic, "uniqueScriptIndex", ref this.uniqueScriptIndex))
		{
			this.uniqueScriptIndex = -1;
		}
		int num = 0;
		WorkerModel.TryGetValue<int>(dic, "nameId", ref num);
		if (AgentNameList.CheckCustomForOldSave((long)num))
		{
			this.iscustom = true;
		}
		if (this.iscustom)
		{
			AgentName customNameByInfo = AgentNameList.instance.GetCustomNameByInfo(this.name, num);
			this._agentName = customNameByInfo;
			this.name = customNameByInfo.GetName();
		}
		this.InitSkills();
		if (!this.iscustom)
		{
			if (num == 0)
			{
				num = 1;
			}
			if (num != 0)
			{
				this._agentName = AgentNameList.instance.GetNameByInfo(num);
				if (this._agentName == null)
				{
					this._agentName = AgentNameList.instance.GetRandomNameByInfo();
				}
				this.name = this._agentName.GetName();
			}
		}
		WorkerModel.TryGetValue<WorkerPrimaryStat>(dic, "primaryStat", ref this.primaryStat);
		int id = 0;
		if (WorkerModel.TryGetValue<int>(dic, "prefix", ref id))
		{
			this.prefix = AgentTitleTypeList.instance.GetData(id);
		}
		int id2 = 0;
		if (WorkerModel.TryGetValue<int>(dic, "suffix", ref id2) || WorkerModel.TryGetValue<int>(dic, "suffix ", ref id2))
		{
			this.suffix = AgentTitleTypeList.instance.GetData(id2);
		}
		if (this.level >= 4 && (this.prefix == null || this.prefix.level == 1))
		{
			this.UpdatePrefixTitle_reset(1);
		}
		if (this.level >= 2 && (this.suffix == null || this.suffix.level == 1))
		{
			this.UpdateSuffixTitle_reset(1);
		}
		if (WorkerModel.TryGetValue<WorkerSpriteSaveData>(dic, "spriteSet", ref this.spriteData.saveData))
		{
			WorkerSpriteManager.instance.LoadWorkerSpriteSetBySaveData(this.spriteData);
		}
		else
		{
			WorkerSpriteManager.instance.GetRandomBasicData(this.spriteData, true);
		}
		long instanceId = 0L;
		if (WorkerModel.TryGetValue<long>(dic, "weaponId", ref instanceId))
		{
			EquipmentModel equipment = InventoryModel.Instance.GetEquipment(instanceId);
			if (equipment != null && equipment.metaInfo.type == EquipmentTypeInfo.EquipmentType.WEAPON)
			{
				base.SetWeapon((WeaponModel)equipment);
			}
		}
		long instanceId2 = 0L;
		if (WorkerModel.TryGetValue<long>(dic, "armorId", ref instanceId2))
		{
			EquipmentModel equipment2 = InventoryModel.Instance.GetEquipment(instanceId2);
			if (equipment2 != null && equipment2.metaInfo.type == EquipmentTypeInfo.EquipmentType.ARMOR)
			{
				base.SetArmor((ArmorModel)equipment2);
			}
		}
		Dictionary<string, object> dic3 = new Dictionary<string, object>();
		if (WorkerModel.TryGetValue<Dictionary<string, object>>(dic, "gifts", ref dic3))
		{
			this._equipment.gifts.LoadDataAndAttach(this, dic3);
		}
		if (!WorkerModel.TryGetValue<string>(dic, "lastServiceSefira", ref this.lastServiceSefira))
		{
			this.lastServiceSefira = "0";
		}
		if (!WorkerModel.TryGetValue<int>(dic, "continuousServiceDay", ref this.continuousServiceDay))
		{
			this.continuousServiceDay = 1;
		}
		WorkerModel.TryGetValue<bool>(dic, "isAce", ref this.isAce);
		this.hp = (float)this.maxHp;
		this.mental = (float)this.maxMental;
	}

	public override void OnStageStart()
	{ // <Mod>
		Notice.instance.Observe(NoticeName.UnconWorkerDead, this);
		Notice.instance.Observe(NoticeName.ManageCancel, this);
		Notice.instance.Observe(NoticeName.CreatureSuppressCancel, this);
		this.primaryStatExp.Init();
		this._isAutoSuppressing = false;
		if (base.Equipment.armor != null)
		{
			base.Equipment.armor.script.OnStageStart();
		}
		if (base.Equipment.weapon != null)
		{
			base.Equipment.weapon.script.OnStageStart();
		}
		Equipment.gifts.OnStageStart();
		this.recentWork = null;
		this.oldWork = null;
		this.recentWorkedCreature = null;
		this.counterAttackEnabled = true;
		this.StopStun();
		this.currentSkill = null;
		this._revivalHp = false;
		this._revivalMental = false;
		this._revivaledHp = false;
		this._revivaledMental = false;
		Sefira currentSefira = this.GetCurrentSefira();
		if (currentSefira != null)
		{
			this._currentWaitingPassage = currentSefira.sefiraPassage;
		}
		else
		{
			this._currentWaitingPassage = null;
		}
		if (base.CurrentPanicAction != null)
		{
			base.CurrentPanicAction = null;
		}
		if (this.lastServiceSefira == base.currentSefira)
		{
			this.continuousServiceDay++;
		}
		else if (((this.lastServiceSefira == "5" || this.lastServiceSefira == "6") && (base.currentSefira == "5" || base.currentSefira == "6")) && SpecialModeConfig.instance.GetValue<bool>("TwoTipherethCaptains"))
		{
			this.isAce = false;
			this.continuousServiceDay++;
			this.lastServiceSefira = base.currentSefira;
		}
		else
		{
			this.isAce = false;
			this.continuousServiceDay = 1;
			if (ResearchDataModel.instance.IsUpgradedAbility("sooner_service_bonuses"))
			{
				continuousServiceDay += (int)((float)history.WorkDay * 0.15f);
			}
			this.lastServiceSefira = base.currentSefira;
		}
		this.hp = (float)this.maxHp;
		this.mental = (float)this.maxMental;
		this.UpdateBestRwbp();
		if (base.currentSefira != "0")
		{
			this._unit.spriteSetter.SetSefira(SefiraManager.instance.GetSefira(base.currentSefira).sefiraEnum, this.GetContinuousServiceLevel());
		}
		cannotAttackUnits.Clear();
	}

	public override void OnStageEnd()
	{ // <Mod>
		Notice.instance.Remove(NoticeName.UnconWorkerDead, this);
		Notice.instance.Remove(NoticeName.ManageCancel, this);
		Notice.instance.Remove(NoticeName.CreatureSuppressCancel, this);
		ClearWorkOrderQueue();
		if (this.IsSuppressing())
		{
			this.FinishSuppress();
			this.StopAction();
		}
		if (base.Equipment.kitCreature != null)
		{
			base.ReleaseKitCreature(true);
		}
		if (this.unconAction != null)
		{
			this.unconAction.OnStageEnd();
			if (this.IsDead())
			{
				return;
			}
		}
		if (this.IsPanic())
		{
			this.SetInvincible(false);
			this.Die();
		}
		this.StopAction();
		if (!this.IsDead())
		{
			this.hp = (float)this.maxHp;
			this.mental = (float)this.maxMental;
		}
		this.StopStun();
		this.counterAttackEnabled = true;
		this.returnPanic = false;
	}

	public override void OnStageRelease()
	{
		UnitBuf[] array = this._bufList.ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].OnStageRelease();
		}
		this.eventHandler.OnStageRelease();
		this.GetControl();
		if (this.IsDead())
		{
			base.ReleaseWeaponV2();
			base.ReleaseArmor();
		}
		if (base.Equipment.weapon != null)
		{
			base.Equipment.weapon.script.OnStageRelease();
			if (!base.Equipment.weapon.CheckRequire(this))
			{
				base.ReleaseWeaponV2();
			}
		}
		if (base.Equipment.armor != null)
		{
			base.Equipment.armor.script.OnStageRelease();
			if (!base.Equipment.armor.CheckRequire(this))
			{
				base.ReleaseArmor();
			}
		}
	}

	public void SetUnit(AgentUnit unit)
	{
		this._unit = unit;
		unit.spriteSetter.Init(this);
	}

	public void ResetWaitingPassage()
	{
		MapNode sepiraNodeByRandom = MapGraph.instance.GetSepiraNodeByRandom(base.currentSefira);
		if (sepiraNodeByRandom != null)
		{
			PassageObjectModel attachedPassage = sepiraNodeByRandom.GetAttachedPassage();
			if (attachedPassage != null)
			{
				this.SetWaitingPassage(attachedPassage);
				this.StopAction();
				this.counterAttackEnabled = false;
				PassageObject passageObject = SefiraMapLayer.instance.GetPassageObject(attachedPassage);
				if (passageObject != null)
				{
					passageObject.OnPointEnter();
					passageObject.OnPointerClick();
				}
			}
		}
	}

	public void SetWaitingPassage(PassageObjectModel passage)
	{
		this._currentWaitingPassage = passage;
	}

	public override void ChangeHairSprite(SpriteInfo spriteInfo)
	{
		this.tempHairSpriteInfo = spriteInfo;
		this.hairSprite = this.tempHairSpriteInfo.sprite;
		if (this._unit != null)
		{
			this._unit.UpdateHair();
		}
	}
	public override void OnFixedUpdate()
	{ // <Mod>
		if (this.IsDead())
		{
			return;
		}
		if (ForceHideUI)
		{
			if (currentSkill == null && hp >= maxHp && mental >= maxMental)
			{
				ForceHideUI = false;
			}
		}
		this.UpdateBufState();
		PassageObjectModel passage = this.movableNode.GetPassage();
		if (!this.willDead && passage != null)
		{
			UnitModel unitModel = null;
			foreach (MovableObjectNode movableObjectNode in passage.GetEnteredTargets())
			{
				UnitModel unit = movableObjectNode.GetUnit();
				if (unit.IsAttackTargetable() && this.IsHostile(unit))
				{
					unitModel = unit;
					break;
				}
			}
			if (this.state == AgentAIState.IDLE && unitModel != null && this.counterAttackEnabled && this.factionTypeInfo.Check(unitModel) == FactionActionType.HOSTILE && (!(unitModel is CreatureModel) || (unitModel as CreatureModel).script.IsAutoSuppressable()))
			{
				this.Suppress(unitModel, true);
			}
		}
		if (this.remainAttackDelay > 0f)
		{
			this.remainAttackDelay -= Time.deltaTime;
		}
		if (this.remainMoveDelay > 0f)
		{
			this.remainMoveDelay -= Time.deltaTime;
		}
		if (this._equipment.weapon != null)
		{
			this._equipment.weapon.OnFixedUpdate();
		}
		if (this._equipment.armor != null)
		{
			this._equipment.armor.OnFixedUpdate();
		}
		this._equipment.gifts.OnFixedUpdate();
		if (this._equipment.kitCreature != null)
		{
			this._equipment.kitCreature.OnFixedUpdateInKitEquip(this);
		}
		this.tempAnim.OnFixedUpdate();
		if (this.stunTime > 0f)
		{
			this.stunTime -= Time.deltaTime;
			return;
		}
		if (this.haltUpdate)
		{
			return;
		}
		this.ProcessAction();
		if (this.IsDead())
		{
			return;
		}
		if (this._equipment.weapon != null && this._equipment.weapon.remainDelay > 0f)
		{
			this.movableNode.StopMoving();
		}
		if (this.remainMoveDelay > 0f)
		{
			this.movableNode.ProcessMoveNode(0f);
		}
		else
		{
			this.movableNode.ProcessMoveNode(this.GetMovementValue() * this.movementMul * base.GetMovementScaleByBuf());
		}
		if (this.panicReport && this.mental / (float)this.maxMental * 100f > 30f)
		{
			this.panicReport = false;
		}
	}

	public float GetMovementValue()
	{ // <Mod> Fixed Malkuth's Core Suppression reward; also changed it so that it's applied before the movement speed stat instead of after
		float num = 4f;
		if (MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.MALKUT))
		{
			num += 0.25f;
		}
		if (MissionManager.instance.ExistsFinishedOvertimeBossMission(SefiraEnum.MALKUT))
		{
			num += 0.25f;
		}
		return num * (1f + this.movement / 100f);
	}

	public override void ProcessAction()
	{
		this.commandQueue.Execute(this);
		if (base.CurrentPanicAction != null)
		{
			base.CurrentPanicAction.Execute();
		}
		else if (this.state == AgentAIState.IDLE)
		{
			PassageObjectModel passage = this.movableNode.GetPassage();
			if (passage != null && passage == this._currentWaitingPassage && !this.counterAttackEnabled)
			{
				this.counterAttackEnabled = true;
			}
			if (this.waitTimer <= 0f)
			{
				if (this._currentWaitingPassage != null)
				{
					base.MoveToMovable(this._currentWaitingPassage.GetRandomMovableNode());
				}
				this.waitTimer = 3.5f + UnityEngine.Random.value;
			}
		}
		else if (this.state == AgentAIState.MANAGE || this.state == AgentAIState.OBSERVE)
		{
			if (this.target.state == CreatureState.ESCAPE)
			{
				this.StopAction();
			}
		}
		else if (this.state == AgentAIState.CANNOT_CONTROLL && this.unconAction != null)
		{
			this.unconAction.Execute();
		}
		if (this._isAutoSuppressing)
		{
			if (this.IsSuppressing())
			{
				this.CheckAutoSuppressing();
			}
			else
			{
				this._isAutoSuppressing = false;
			}
		}
		this.waitTimer -= Time.deltaTime;
	}

	private void CheckAutoSuppressing()
	{
		if (this.target != null)
		{
			PassageObjectModel currentPassage = this.target.GetMovableNode().currentPassage;
			if (currentPassage == null)
			{
				return;
			}
			if (this.movableNode.currentPassage != currentPassage)
			{
				this.ForcelyCancelSuppress();
				return;
			}
		}
		else if (this.targetWorker != null)
		{
			PassageObjectModel currentPassage2 = this.targetWorker.GetMovableNode().currentPassage;
			if (currentPassage2 == null)
			{
				return;
			}
			if (this.movableNode.currentPassage != currentPassage2)
			{
				this.ForcelyCancelSuppress();
			}
		}
	}

	public WorkerPrimaryStatBonus UpdatePrimaryStat()
	{
		int level = this.level;
		WorkerPrimaryStatBonus workerPrimaryStatBonus = new WorkerPrimaryStatBonus();
		workerPrimaryStatBonus.hp = this.primaryStat.hp;
		workerPrimaryStatBonus.mental = this.primaryStat.mental;
		workerPrimaryStatBonus.work = this.primaryStat.work;
		workerPrimaryStatBonus.battle = this.primaryStat.battle;
		this.primaryStat.UpdateStat(this.primaryStatExp);
		this.UpdateTitle(level);
		return workerPrimaryStatBonus;
	}

	private void UpdateBestRwbp()
	{
		List<int> stats = new List<int>(new int[]
		{
			this.fortitudeStat,
			this.prudenceStat,
			this.temperanceStat,
			this.justiceStat
		});
		List<int> indicesOfBestStats = new List<int>(new int[]
		{
			0,
			1,
			2,
			3
		});
		indicesOfBestStats.Sort((int x, int y) => stats[y] - stats[x]);
		indicesOfBestStats.RemoveAll((int x) => stats[x] < stats[indicesOfBestStats[0]]);
		int index = UnityEngine.Random.Range(0, indicesOfBestStats.Count);
		switch (indicesOfBestStats[index])
		{
		case 0:
			this._bestRwbp = RwbpType.R;
			return;
		case 1:
			this._bestRwbp = RwbpType.W;
			return;
		case 2:
			this._bestRwbp = RwbpType.B;
			return;
		case 3:
			this._bestRwbp = RwbpType.P;
			return;
		default:
			return;
		}
	}

	public void UpdateTitle(int oldLevel)
	{ // <Mod>
		UpdateTitle(oldLevel, true);
	}

	// <Mod>
	public void UpdateTitle(int oldLevel, bool pseudoRandom = true)
	{ // <Mod> The new title bonuses make it possible for title bonueses to lower after leveling up,
	// thereby openning up the possiblity of a stat levels being reduced and then the employee not longer qualifying for that overall level
		int newLevel = this.level;
		int[] oldStatLevel = new int[4];
		int[] newStatLevel = new int[4];
		oldStatLevel[0] = this.originFortitudeLevel;
		oldStatLevel[1] = this.originPrudenceLevel;
		oldStatLevel[2] = this.originTemperanceLevel;
		oldStatLevel[3] = this.originJusticeLevel;
		for (int i = oldLevel + 1; i <= newLevel; i++)
		{
			switch (i)
			{
				case 1:
					if (forceTitles[0] != 0)
					{
						AgentTitleTypeInfo title = AgentTitleTypeList.instance.GetData(forceTitles[0]);
						if (title != null)
						{
							this.prefix = title;
							AgentTitleTypeInfo dataSuffix2 = AgentTitleTypeList.instance.GetDataSuffix(this, i, !pseudoRandom);
							if (dataSuffix2 != null)
							{
								this.suffix = dataSuffix2;
							}
							continue;
						}
					}
					break;
				case 3:
					if (forceTitles[1] != 0)
					{
						AgentTitleTypeInfo title = AgentTitleTypeList.instance.GetData(forceTitles[1]);
						if (title != null)
						{
							this.suffix = title;
							continue;
						}
					}
					break;
				case 4:
					if (forceTitles[2] != 0)
					{
						AgentTitleTypeInfo title = AgentTitleTypeList.instance.GetData(forceTitles[2]);
						if (title != null)
						{
							this.prefix = title;
							continue;
						}
					}
					break;
				case 5:
					if (forceTitles[3] != 0)
					{
						AgentTitleTypeInfo title = AgentTitleTypeList.instance.GetData(forceTitles[3]);
						if (title != null)
						{
							this.suffix = title;
							continue;
						}
					}
					break;
			}
			AgentTitleTypeInfo dataPrefix = AgentTitleTypeList.instance.GetDataPrefix(this, i, !pseudoRandom);
			if (dataPrefix != null)
			{
				this.prefix = dataPrefix;
			}
			AgentTitleTypeInfo dataSuffix = AgentTitleTypeList.instance.GetDataSuffix(this, i, !pseudoRandom);
			if (dataSuffix != null)
			{
				this.suffix = dataSuffix;
			}
		}
		newStatLevel[0] = this.originFortitudeLevel;
		newStatLevel[1] = this.originPrudenceLevel;
		newStatLevel[2] = this.originTemperanceLevel;
		newStatLevel[3] = this.originJusticeLevel;
        int[] levelThresholds = {0, 30, 45, 65, 85};
		for (int i = 0; i < 4; i++)
		{
			if (newStatLevel[i] < oldStatLevel[i])
			{
                int stat = 0;
                switch (i) { case 0: stat = this.originFortitudeStat; break; case 1: stat = this.originPrudenceStat; break; case 2: stat = this.originTemperanceStat; break; case 3: stat = this.originJusticeStat; break; }
				int adjustment = levelThresholds[oldStatLevel[i] - 1] - stat;
                switch (i) { case 0: this.primaryStat.hp += adjustment; break; case 1: this.primaryStat.mental += adjustment; break; case 2: this.primaryStat.work += adjustment; break; case 3: this.primaryStat.battle += adjustment; break; }
			}
		}
	}

	public void UpdatePrefixTitle_reset(int oldLevel)
	{
		for (int i = oldLevel + 1; i <= this.level; i++)
		{
			AgentTitleTypeInfo dataPrefix = AgentTitleTypeList.instance.GetDataPrefix(this, i, true);
			if (dataPrefix != null)
			{
				this.prefix = dataPrefix;
			}
		}
	}

	public void UpdateSuffixTitle_reset(int oldLevel)
	{
		for (int i = oldLevel + 1; i <= this.level; i++)
		{
			AgentTitleTypeInfo dataSuffix = AgentTitleTypeList.instance.GetDataSuffix(this, i, true);
			if (dataSuffix != null)
			{
				this.suffix = dataSuffix;
			}
		}
	}

	protected override void OnSetWeapon()
	{
		base.OnSetWeapon();
		if (this._unit != null)
		{
			this._unit.OnChangeWeapon();
		}
	}

	protected override void OnChangeGift()
	{
		base.OnChangeGift();
		if (this._unit != null)
		{
			this._unit.UpdateGiftModel();
		}
	}

	protected override void OnReleaseWeapon()
	{
		base.OnReleaseWeapon();
	}

	protected override void OnSetArmor()
	{
		base.OnSetArmor();
		if (this._unit != null)
		{
			this._unit.OnChangeArmor();
		}
	}

	protected override void OnReleaseArmor()
	{
		base.OnReleaseArmor();
		if (this._unit != null)
		{
			this._unit.OnChangeArmor();
		}
	}

	protected override void OnSetKitCreature()
	{
		base.OnSetKitCreature();
		if (this._unit != null)
		{
			this._unit.OnChangeKitCreature();
		}
	}

	protected override void OnReleaseKitCreature()
	{
		base.OnReleaseKitCreature();
		if (this._unit != null)
		{
			this._unit.OnChangeKitCreature();
		}
	}

	public override void PrepareWeapon()
	{
		base.PrepareWeapon();
		WeaponSetter.SetWeaponAnimParam(this);
		this._unit.PrepareWeapon();
	}

	public override void CancelWeapon()
	{
		base.CancelWeapon();
		this._unit.CancelWeapon();
	}

	public override float RecoverHPv2(float amount, bool canBeModified = true)
	{
		if (this.IsDead())
		{
			return 0f;
		}
		if (SefiraBossManager.Instance.IsRecoverBlocked)
		{
			return 0f;
		}
		if (this.blockRecover)
		{
			return 0f;
		}
		float num = base.RecoverHPv2(amount, canBeModified);
		if (num > 0)
		{
			this.MakeRecoverEffect(true);
		}
		return num;
	}

	public override float RecoverMentalv2(float amount, bool canBeModified = true)
	{
		if (this.IsDead())
		{
			return 0f;
		}
		if (SefiraBossManager.Instance.IsRecoverBlocked)
		{
			return 0f;
		}
		float num = base.RecoverMentalv2(amount, canBeModified);
		if (this.mental >= (float)this.maxMental && this.IsPanic())
		{
			this.StopPanic();
		}
		if (num > 0)
		{
			this.MakeRecoverEffect(false);
		}
		return num;
	}

	public void UpdateWeaponLevel()
	{
	}

	public AgentModel.SkillInfo[] GetSkillInfos()
	{
		return this.skillInfos.ToArray();
	}

	public bool HasSkill(SkillTypeInfo skill)
	{
		using (List<AgentModel.SkillInfo>.Enumerator enumerator = this.skillInfos.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.skill == skill)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool HasSkill(long id)
	{
		using (List<AgentModel.SkillInfo>.Enumerator enumerator = this.skillInfos.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.skill.id == id)
				{
					return true;
				}
			}
		}
		return false;
	}

	public AgentAIState GetState()
	{
		return this.state;
	}

	public override void StopAction()
	{
		if (this.state != AgentAIState.CANNOT_CONTROLL)
		{
			this.state = AgentAIState.IDLE;
		}
		this.waitTimer = 0f;
		this.commandQueue.Clear();
		this.target = null;
	}

	public override void ClearUnconCommand()
	{
		if (this.state == AgentAIState.CANNOT_CONTROLL)
		{
			this.commandQueue.Clear();
		}
	}

	public new void FollowMovable(MovableObjectNode node)
	{
		this.commandQueue.SetAgentCommand(WorkerCommand.MakeFollowAgent(node));
	}

	public void PursueAgent(UnitModel target)
	{
		this.state = AgentAIState.PANIC_VIOLENCE;
		this.commandQueue.SetAgentCommand(WorkerCommand.MakePanicPursueAgent(target));
	}

	public override void PursueUnconAgent(UnitModel target)
	{
		this.commandQueue.SetAgentCommand(WorkerCommand.MakeUnconPursueAgent(target));
	}

	public void ManageCreature(CreatureModel target, SkillTypeInfo skill, Sprite skillSprite)
	{
		this.recentWork = skill;
		this.state = AgentAIState.MANAGE;
		this.target = target;
		this.commandQueue.SetAgentCommand(WorkerCommand.MakeManageCreature(target, this, skill, skillSprite));
		object[] param = new object[]
		{
			this,
			target,
			skill
		};
		Notice.instance.Send(NoticeName.ReportAgentSuccess, param);
		this.IconAllocated();
		this._unit.SetWorkNote((int)skill.id);
	}

	public void ManageKitCreature(CreatureModel target)
	{
		SkillTypeInfo data = SkillTypeList.instance.GetData(5L);
		this.recentWork = data;
		this.state = AgentAIState.MANAGE;
		this.target = target;
		this.commandQueue.SetAgentCommand(WorkerCommand.MakeManageCreature(target, this, data, null));
		this.IconAllocated();
		this._unit.SetWorkNote((int)data.id);
	}

	public void ReturnKitCreature()
	{
		if (this.IsDead() || this.IsCrazy())
		{
			return;
		}
		if (base.Equipment.kitCreature == null)
		{
			return;
		}
		if (this.state == AgentAIState.MANAGE)
		{
			return;
		}
		this.state = AgentAIState.MANAGE;
		this.target = base.Equipment.kitCreature;
		this.commandQueue.SetAgentCommand(new ReturnKitCreatureCommand());
	}

	public void ReturnCancelKitCreature()
	{
		if (SefiraBossManager.Instance.IsWorkCancelable)
		{
			Notice.instance.Send(NoticeName.ManageCancel, new object[]
			{
				this.target
			});
		}
	}

	public void ObserveCreature(CreatureModel target, Sprite skillSprite)
	{
	}

	public void ReturnCreature(CreatureModel target)
	{
		if (target.state != CreatureState.SUPPRESSED)
		{
			Debug.Log("ReturnCreature >> Invalid state");
			return;
		}
		this.state = AgentAIState.RETURN_CREATURE;
		target.state = CreatureState.SUPPRESSED_RETURN;
		this.commandQueue.SetAgentCommand(WorkerCommand.MakeReturnCreature(target));
	}

	public void FinishManage()
	{
		try
		{
			this._unit.DisappearNote();
		}
		catch (Exception)
		{
		}
		this._unit.EndWork();
		if (this.state != AgentAIState.MANAGE)
		{
			return;
		}
		Notice.instance.Send(NoticeName.WorkEndReport, new object[]
		{
			this
		});
		this.state = AgentAIState.IDLE;
		this.target = null;
	}

	public void FinishObserve()
	{
		if (this.state != AgentAIState.OBSERVE)
		{
			return;
		}
		Notice.instance.Send(NoticeName.WorkEndReport, new object[]
		{
			this
		});
		this.state = AgentAIState.IDLE;
		this.target = null;
	}

	public void FinishReturnKitCreature()
	{
		if (this.state != AgentAIState.MANAGE)
		{
			return;
		}
		this.state = AgentAIState.IDLE;
		this.target = null;
	}

	public void FinishSuppress()
	{
		if (this.IsSuppressing())
		{
			this._unit.SelectIconDisable();
			this.state = AgentAIState.IDLE;
			this.target = null;
			this.targetWorker = null;
		}
	}

	public void Suppress(UnitModel target, bool isAuto = false)
	{
		if (this.IsCrazy() || this.unconAction != null)
		{
			return;
		}
		if (this.state == AgentAIState.SUPPRESS_WORKER && base.GetCurrentCommand() is SuppressWorkerCommand && this.targetWorker == target)
		{
			return;
		}
		if (this.state == AgentAIState.SUPPRESS_CREATURE && base.GetCurrentCommand() is SuppressWorkerCommand && this.target == target)
		{
			return;
		}
		if (cannotAttackUnits.Contains(target))
		{
			return;
		}
		this.commandQueue.SetAgentCommand(WorkerCommand.MakeSuppressCommand(target));
		if (target is WorkerModel)
		{
			this.state = AgentAIState.SUPPRESS_WORKER;
			this.targetWorker = (target as WorkerModel);
		}
		else
		{
			this.state = AgentAIState.SUPPRESS_CREATURE;
			this.target = (target as CreatureModel);
		}
		this._unit.SelectIconForcelyEnable();
		this._isAutoSuppressing = isAuto;
		Notice.instance.Send(NoticeName.OnCommandSuppress, new object[]
		{
			target
		});
	}

	public void SuppressStandingObject(StandingItemModel standing)
	{
		this.state = AgentAIState.SUPPRESS_OBJECT;
		this.commandQueue.SetAgentCommand(WorkerCommand.MakeSuppressCommand(standing));
		this.targetObject = standing;
		this._unit.SelectIconForcelyEnable();
	}

	public bool IsSuppressing()
	{
		return this.state == AgentAIState.SUPPRESS_CREATURE || this.state == AgentAIState.SUPPRESS_WORKER || this.state == AgentAIState.SUPPRESS_OBJECT;
	}

	public void PanicSuppressed()
	{
		this.movableNode.StopMoving();
	}

	public override void UnderAttack(UnitModel attacker)
	{
		base.UnderAttack(attacker);
		if (this.state == AgentAIState.MANAGE || this.state == AgentAIState.OBSERVE)
		{
			if (!this.counterAttackEnabled)
			{
				return;
			}
			this.StopAction();
		}
		else if (this.state == AgentAIState.IDLE && !this.counterAttackEnabled)
		{
			return;
		}
		if (!this.IsPanic())
		{
			this.Suppress(attacker, true);
			return;
		}
		if (base.CurrentPanicAction is PanicViolence)
		{
			this.Suppress(attacker, true);
		}
	}

	public override void LoseControl()
	{
		if (this.IsPanic())
		{
			this.StopPanic();
		}
		this.state = AgentAIState.CANNOT_CONTROLL;
		this.commandQueue.Clear();
	}

	public override void GetControl()
	{
		if (this.state == AgentAIState.CANNOT_CONTROLL)
		{
			this.state = AgentAIState.IDLE;
			this.commandQueue.Clear();
			if (this.unconAction != null)
			{
				this.unconAction.OnDestroy();
				this.unconAction = null;
			}
			this.GetCurrentSefira().OnAgentReturnControll();
		}
	}

	public override bool CannotControll()
	{
		return this.state == AgentAIState.CANNOT_CONTROLL;
	}

	public override void SetUncontrollableAction(UncontrollableAction uncon)
	{
		if (base.CurrentPanicAction != null)
		{
			base.CurrentPanicAction = null;
			base.specialDeadScene = false;
		}
		if (this.unconAction != null)
		{
			this.unconAction.OnDestroy();
		}
		this.unconAction = uncon;
		if (this.unconAction != null)
		{
			this.unconAction.Init();
		}
		SefiraManager.instance.GetSefira(base.currentSefira).OnAgentCannotControll(this);
	}

	public void OnClick()
	{
		if (this.unconAction != null)
		{
			this.unconAction.OnClick();
		}
		if (this.IsPanic())
		{
			CommandWindow.CommandWindow.CreateWindow(CommandType.Suppress, this);
		}
	}

	public void IconAllocated()
	{
		this._unit.ManagingCreature();
	}

	public override void OnDie()
	{
		if (this.invincible)
		{
			return;
		}
		base.OnDie();
		UnitMouseEventManager.instance.Unselect(this);
		if (this.stunTime > 0f)
		{
			this.stunTime = 0f;
		}
		if (this.unconAction != null)
		{
			this.unconAction.OnPrevDie();
		}
		PassageObjectModel passage = this.movableNode.GetPassage();
		if (!this.IsPanic() && passage != null)
		{
			List<WorkerModel> list = new List<WorkerModel>();
			foreach (MovableObjectNode movableObjectNode in passage.GetEnteredTargets())
			{
				WorkerModel workerModel = movableObjectNode.GetUnit() as WorkerModel;
				if (workerModel != null && workerModel != this)
				{
					list.Add(workerModel);
				}
			}
			foreach (WorkerModel workerModel2 in list)
			{
				if (workerModel2 is AgentModel)
				{
					(workerModel2 as AgentModel).HorrorDamageByDead(this);
				}
				else
				{
					workerModel2.InitialEncounteredCreature(this);
				}
			}
		}
		if (base.CurrentPanicAction != null)
		{
			base.CurrentPanicAction.OnDie();
		}
		float num = 1f;
		if (ResearchDataModel.instance.IsUpgradedAbility("regist_ego_destroy"))
		{
			num = 0.75f;
		}
		if (MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.BINAH))
		{
			num = 0f;
		}
		if (this._equipment.weapon != null && UnityEngine.Random.value <= num)
		{
			InventoryModel.Instance.RemoveEquipment(this._equipment.weapon);
		}
		if (this._equipment.armor != null && UnityEngine.Random.value <= num)
		{
			InventoryModel.Instance.RemoveEquipment(this._equipment.armor);
		}
		if (this._equipment.kitCreature != null)
		{
			base.ReleaseKitCreature(false);
		}
		if (this.unconAction != null)
		{
			this.unconAction.OnDie();
		}
		if (base.specialDeadScene)
		{
			base.SetDeadType(DeadType.SPECIAL);
			try
			{
				if (this.hasUniqueFace)
				{
					this.GetUnit().animChanger.ChangeAnimatorWithUniqueFace(this.deadSceneName, this.seperator);
				}
				else
				{
					this.GetUnit().SetWorkerFaceType(WorkerFaceType.PANIC);
					this.GetUnit().animChanger.ChangeAnimator(this.deadSceneName, this.seperator);
				}
			}
			catch (Exception)
			{
			}
		}
		if (this.isRealWorker)
		{
			if (this.IsPanic())
			{
				Notice instance = Notice.instance;
				string onAgentPanicReturn = NoticeName.OnAgentPanicReturn;
				object[] array = new object[2];
				array[0] = this;
				instance.Send(onAgentPanicReturn, array);
			}
			if (!this.deadInit)
			{
				this.deadInit = true;
				Notice.instance.Send(NoticeName.OnAgentDead, new object[]
				{
					this
				});
			}
			PlayerModel.emergencyController.AddScore(4f);
			SefiraManager.instance.GetSefira(base.currentSefira).OnAgentCannotControll(this);
			AngelaConversationUI.instance.StartAgentDeadTimer();
			this._unit.OnDie();
			Debug.Log("Make Angela Log");
			AngelaConversation.instance.MakeDefaultFormatMessage(AngelaMessageState.AGENT_DEAD_HEALTH, new object[]
			{
				this
			});
			BgmManager.instance.SubAgent();
			return;
		}
		this._unit.OnDie();
	}

	// Token: 0x06003364 RID: 13156 RVA: 0x0002ED24 File Offset: 0x0002CF24
	public void FinishOpenIolateRoom()
	{
		if (this.state == AgentAIState.OPEN_ISOLATE)
		{
			this.state = AgentAIState.IDLE;
		}
	}

	// Token: 0x06003365 RID: 13157 RVA: 0x0002ED36 File Offset: 0x0002CF36
	public void FinishPursueAgent()
	{
		if (this.state == AgentAIState.PANIC_VIOLENCE)
		{
			this.state = AgentAIState.IDLE;
		}
	}

	// Token: 0x06003366 RID: 13158 RVA: 0x0002ED49 File Offset: 0x0002CF49
	public void FinishReturnCreature()
	{
		if (this.state == AgentAIState.RETURN_CREATURE)
		{
			this.state = AgentAIState.IDLE;
		}
	}

	// Token: 0x06003367 RID: 13159 RVA: 0x00157A04 File Offset: 0x00155C04
	public void SetCurrentSefira(string sefira)
	{
		string currentSefira = base.currentSefira;
		base.currentSefira = sefira;
		this.waitTimer = 0f;
		if (this.instanceId < 0L)
		{
			return;
		}
		Notice.instance.Send(NoticeName.ChangeAgentSefira, new object[]
		{
			this,
			currentSefira
		});
	}

	// Token: 0x06003368 RID: 13160 RVA: 0x00157A54 File Offset: 0x00155C54
	public override void Panic()
	{
		if (this.IsDead())
		{
			return;
		}
		if (this.state == AgentAIState.CANNOT_CONTROLL)
		{
			return;
		}
		if (this.IsPanic())
		{
			return;
		}
		if (this.returnPanic)
		{
			return;
		}
		if (this.invincible)
		{
			return;
		}
		PlayerModel.emergencyController.AddScore(2f);
		this.StopStun();
		this.StopAction();
		if (this.IsDead())
		{
			return;
		}
		if (this.state == AgentAIState.CANNOT_CONTROLL)
		{
			return;
		}
		if (this.IsPanic())
		{
			return;
		}
		if (this.returnPanic)
		{
			return;
		}
		if (this.invincible)
		{
			return;
		}
		base.CurrentPanicAction = new PanicReady(this);
		SefiraManager.instance.GetSefira(base.currentSefira).OnAgentCannotControll(this);
		this.GetCurrentSefira().OnAgentGetPanic(this);
		Notice.instance.Send(NoticeName.OnAgentPanic, new object[]
		{
			this
		});
		UnitBuf[] array = this._bufList.ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].OnUnitPanic();
		}
		this.SetFaction(FactionTypeList.StandardFaction.PanicWorker);
		UnitMouseEventManager.instance.Unselect(this);
		this._unit.SpeechDefaultLyric();
		PassageObjectModel passage = this.movableNode.GetPassage();
		if (passage != null)
		{
			List<WorkerModel> list = new List<WorkerModel>();
			foreach (MovableObjectNode movableObjectNode in passage.GetEnteredTargets())
			{
				WorkerModel workerModel = movableObjectNode.GetUnit() as WorkerModel;
				if (workerModel != null && workerModel != this && !workerModel.IsPanic())
				{
					list.Add(workerModel);
				}
			}
			foreach (WorkerModel workerModel2 in list)
			{
				if (workerModel2 is AgentModel)
				{
					(workerModel2 as AgentModel).HorrorDamageByPanic(this);
				}
				else
				{
					workerModel2.InitialEncounteredCreature(this);
				}
			}
		}
	}

	// Token: 0x06003369 RID: 13161 RVA: 0x00157C38 File Offset: 0x00155E38
	public void PanicByCreature(CreatureModel creature, SkillTypeInfo skill)
	{
		if (this.IsDead())
		{
			return;
		}
		if (this.state == AgentAIState.CANNOT_CONTROLL)
		{
			return;
		}
		if (this.IsPanic())
		{
			return;
		}
		if (this.returnPanic)
		{
			return;
		}
		if (this.invincible)
		{
			return;
		}
		base.Panic();
		Debug.Log("패닉");
		PlayerModel.emergencyController.AddScore(2f);
		switch (this.level)
		{
		case 1:
			this.panicValue = 10;
			break;
		case 2:
			this.panicValue = 15;
			break;
		case 3:
			this.panicValue = 25;
			break;
		case 4:
			this.panicValue = 35;
			break;
		case 5:
			this.panicValue = 50;
			break;
		}
		base.CurrentPanicAction = this.panicData.BuildDefaultPanicAction(this);
		this.StopAction();
		this._unit.showSpeech.ShowSpeech_old(AgentLyrics.instance.GetLyricByType(LyricType.MENTALBAD));
		AngelaConversation.instance.MakeMessage(AngelaMessageState.AGENT_DEAD_MENTAL, new object[]
		{
			this,
			creature,
			skill
		});
		SefiraManager.instance.GetSefira(base.currentSefira).OnAgentCannotControll(this);
		this.SetFaction(FactionTypeList.StandardFaction.PanicWorker);
		UnitMouseEventManager.instance.Unselect(this);
	}

	// Token: 0x0600336A RID: 13162 RVA: 0x00157D68 File Offset: 0x00155F68
	public override void PanicReadyComplete()
	{
		if (base.CurrentPanicAction != null && !(base.CurrentPanicAction is PanicReady))
		{
			return;
		}
		switch (this.bestRwbp)
		{
		case RwbpType.R:
			base.CurrentPanicAction = new PanicViolence(this);
			return;
		case RwbpType.W:
			base.CurrentPanicAction = new PanicSuicideExecutor(this);
			return;
		case RwbpType.B:
			base.CurrentPanicAction = new PanicRoaming(this);
			return;
		case RwbpType.P:
			base.CurrentPanicAction = new PanicOpenIsolate(this);
			return;
		default:
			return;
		}
	}

	// Token: 0x0600336B RID: 13163 RVA: 0x00157DE0 File Offset: 0x00155FE0
	public void ForcelyPanic(RwbpType type)
	{
		switch (type)
		{
		case RwbpType.R:
			base.CurrentPanicAction = new PanicViolence(this);
			return;
		case RwbpType.W:
			base.CurrentPanicAction = new PanicSuicideExecutor(this);
			return;
		case RwbpType.B:
			base.CurrentPanicAction = new PanicRoaming(this);
			return;
		case RwbpType.P:
			base.CurrentPanicAction = new PanicOpenIsolate(this);
			return;
		default:
			return;
		}
	}

	// Token: 0x0600336C RID: 13164 RVA: 0x00157E3C File Offset: 0x0015603C
	public override void StopPanic()
	{
		if (!this.IsPanic())
		{
			return;
		}
		Notice.instance.Send(NoticeName.OnAgentPanicReturn, new object[]
		{
			this
		});
		this.state = AgentAIState.IDLE;
		this.forcelyPanicType = RwbpType.N;
		if (base.CurrentPanicAction != null)
		{
			base.CurrentPanicAction.PanicEnd();
			base.CurrentPanicAction = null;
		}
		base.ClearWorkerEncounting();
		base.SetWorkerFaceType(WorkerFaceType.DEFAULT);
		base.SetPanicAnim(false);
		this.Stun(1f);
		base.specialDeadScene = false;
		this.factionTypeInfo = FactionTypeList.instance.GetFaction(FactionTypeList.StandardFaction.Worker);
		this.GetCurrentSefira().OnAgentReturnControll();
		this._unit.spriteSetter.SetPanicShadow(false, RwbpType.N);
	}

	// Token: 0x0600336D RID: 13165 RVA: 0x0002ED5B File Offset: 0x0002CF5B
	public override void StopPanicWithoutStun()
	{
		if (!this.IsPanic())
		{
			return;
		}
		this.state = AgentAIState.IDLE;
		base.CurrentPanicAction = null;
		base.specialDeadScene = false;
		this.factionTypeInfo = FactionTypeList.instance.GetFaction(FactionTypeList.StandardFaction.Worker);
		this.GetCurrentSefira().OnAgentReturnControll();
	}

	// Token: 0x0600336E RID: 13166 RVA: 0x0002ED9B File Offset: 0x0002CF9B
	public override bool IsPanic()
	{
		return base.CurrentPanicAction != null;
	}

	// Token: 0x0600336F RID: 13167 RVA: 0x0002EDA6 File Offset: 0x0002CFA6
	public override bool IsAttackTargetable()
	{
		return !this.cannotBeAttackTargetable && base.IsAttackTargetable() && this.activated;
	}

	// Token: 0x06003370 RID: 13168 RVA: 0x0002EDC0 File Offset: 0x0002CFC0
	public override int GetRiskLevel()
	{
		return this.level;
	}

	// Token: 0x06003371 RID: 13169 RVA: 0x00157EEC File Offset: 0x001560EC
	public override int GetAttackLevel()
	{
		if (this._equipment.weapon != null)
		{
			int result;
			if (int.TryParse(this._equipment.weapon.metaInfo.grade, out result))
			{
				return result;
			}
			Debug.LogError("이게 뭐야");
		}
		return 1;
	}

	// Token: 0x06003372 RID: 13170 RVA: 0x00157F34 File Offset: 0x00156134
	public override int GetDefenseLevel()
	{
		if (this._equipment.weapon != null)
		{
			int result;
			if (int.TryParse(this._equipment.armor.metaInfo.grade, out result))
			{
				return result;
			}
			Debug.LogError("이게 뭐야");
		}
		return 1;
	}

	// Token: 0x06003373 RID: 13171 RVA: 0x00157F7C File Offset: 0x0015617C
	public override void EncounterCreature(UnitModel encounteredCreature)
	{
		if (this.IsCrazy() || this.IsPanic() || this.IsDead())
		{
			return;
		}
		if (this.state != AgentAIState.SUPPRESS_CREATURE && this.state != AgentAIState.SUPPRESS_OBJECT && this.counterAttackEnabled && this.factionTypeInfo.Check(encounteredCreature) == FactionActionType.HOSTILE)
		{
			this.Suppress(encounteredCreature, true);
		}
	}

	// Token: 0x06003374 RID: 13172 RVA: 0x0002EDC8 File Offset: 0x0002CFC8
	public override void EncounterStandingItem(StandingItemModel standing)
	{
		if (this.IsCrazy() || this.IsPanic() || this.IsDead())
		{
			return;
		}
		if (this.state != AgentAIState.SUPPRESS_CREATURE && this.state != AgentAIState.SUPPRESS_OBJECT && this.counterAttackEnabled)
		{
			this.SuppressStandingObject(standing);
		}
	}

	// Token: 0x06003375 RID: 13173 RVA: 0x0002EE04 File Offset: 0x0002D004
	public void EncounterCreature(CreatureModel encounteredCreature, AgentModel infected)
	{
		if (this.IsCrazy() || this.IsPanic() || this.IsDead())
		{
			return;
		}
		if (this.state != AgentAIState.SUPPRESS_CREATURE && this.factionTypeInfo.Check(encounteredCreature) == FactionActionType.HOSTILE)
		{
			this.Suppress(infected, true);
		}
	}

	// Token: 0x06003376 RID: 13174 RVA: 0x0002EE3E File Offset: 0x0002D03E
	public override void ResetAnimator()
	{
		if (this.currentSkill != null)
		{
			this.currentSkill.targetCreature.script.OnAgentAnimatorReseted();
		}
	}

	// Token: 0x06003377 RID: 13175 RVA: 0x0002EE5D File Offset: 0x0002D05D
	public void WorkEndReaction()
	{
		this.workEndReaction = true;
	}

	// Token: 0x06003378 RID: 13176 RVA: 0x0002EE66 File Offset: 0x0002D066
	public void WorkAnimPlayed()
	{
		if (this.currentSkill != null)
		{
			this.currentSkill.OnWorkEndAnimPlayed();
		}
	}

	// Token: 0x06003379 RID: 13177 RVA: 0x0002EE7B File Offset: 0x0002D07B
	public static PersonalityType GetPersonalityType(int index)
	{
		switch (index)
		{
		case 0:
			return PersonalityType.D;
		case 1:
			return PersonalityType.I;
		case 2:
			return PersonalityType.S;
		case 3:
			return PersonalityType.C;
		default:
			return PersonalityType.D;
		}
	}

	// Token: 0x0600337A RID: 13178 RVA: 0x0002EE9E File Offset: 0x0002D09E
	public static string GetLifeStyleEnumToString(PersonalityType type)
	{
		switch (type)
		{
		case PersonalityType.D:
			return "D";
		case PersonalityType.I:
			return "I";
		case PersonalityType.S:
			return "S";
		case PersonalityType.C:
			return "C";
		default:
			return "D";
		}
	}

	// Token: 0x0600337B RID: 13179 RVA: 0x0002EED5 File Offset: 0x0002D0D5
	public void CreatureActionSpeechByAnim(string key)
	{
		if (this.currentSkill == null)
		{
			return;
		}
		this.ShowCreatureActionSpeech(this.currentSkill.targetCreature.metadataId, key);
	}

	// Token: 0x0600337C RID: 13180 RVA: 0x00157FD4 File Offset: 0x001561D4
	public override void ShowCreatureActionSpeech(long creatureId, string key)
	{
		AgentLyrics.CreatureReactionList creatureReaction = AgentLyrics.instance.GetCreatureReaction(creatureId);
		if (creatureReaction != null)
		{
			AgentLyrics.CreatureAction action = creatureReaction.action;
			if (action != null)
			{
				this._unit.showSpeech.ShowCreatureActionLyric(action, key);
			}
		}
	}

	// Token: 0x0600337D RID: 13181 RVA: 0x0002EEF7 File Offset: 0x0002D0F7
	public int calc(int value, int standard)
	{
		if (value < standard)
		{
			return 0;
		}
		if (value >= standard && value < 2 * standard)
		{
			return 1;
		}
		return 2;
	}

	// Token: 0x0600337E RID: 13182 RVA: 0x0002EF0C File Offset: 0x0002D10C
	public override string GetUnitName()
	{
		return this.name;
	}

	// Token: 0x0600337F RID: 13183 RVA: 0x0015800C File Offset: 0x0015620C
	public string GetTitle()
	{
		string text = string.Empty;
		if (this.isAce && this.lastServiceSefira == base.currentSefira)
		{
			SefiraEnum sefiraEnum = this.currentSefiraEnum;
			if (sefiraEnum == SefiraEnum.TIPERERTH2)
			{
				sefiraEnum = SefiraEnum.TIPERERTH1;
			}
			if (sefiraEnum == SefiraEnum.DUMMY)
			{
				text = string.Empty;
			}
			else
			{
				text += string.Format(LocalizeTextDataModel.instance.GetText("continous_service_ability_ace"), LocalizeTextDataModel.instance.GetTextAppend(new string[]
				{
					SefiraName.GetSefiraByEnum(sefiraEnum),
					"Name"
				}));
			}
		}
		else
		{
			if (this.prefix != null)
			{
				text = text + this.prefix.name + " ";
			}
			if (this.suffix != null)
			{
				text += this.suffix.name;
			}
		}
		return text;
	}

	// Token: 0x06003380 RID: 13184 RVA: 0x001580CC File Offset: 0x001562CC
	public string GetTitle(out string pre, out string post)
	{
		string text = string.Empty;
		string empty;
		post = (empty = string.Empty);
		pre = empty;
		if (this.prefix != null)
		{
			text = text + this.prefix.name + " ";
			pre = this.prefix.name;
		}
		if (this.suffix != null)
		{
			text += this.suffix.name;
			post = this.suffix.name;
		}
		return text;
	}

	// Token: 0x06003381 RID: 13185 RVA: 0x00003E0D File Offset: 0x0000200D
	public void SetSprite()
	{
	}

	// Token: 0x06003382 RID: 13186 RVA: 0x0002EF14 File Offset: 0x0002D114
	public static int CompareByID(AgentModel x, AgentModel y)
	{
		if (x == null || y == null)
		{
			Debug.Log("Errror in comparison by sefira");
			return 0;
		}
		return x.instanceId.CompareTo(y.instanceId);
	}

	// Token: 0x06003383 RID: 13187 RVA: 0x00158140 File Offset: 0x00156340
	public static int CompareBySefira(AgentModel x, AgentModel y)
	{
		if (x == null || y == null)
		{
			Debug.Log("Errror in comparison by sefira");
			return 0;
		}
		int num = int.Parse(x.currentSefira);
		int value = int.Parse(y.currentSefira);
		return num.CompareTo(value);
	}

	// Token: 0x06003384 RID: 13188 RVA: 0x0002EF39 File Offset: 0x0002D139
	public static int CompareByBattle(AgentModel x, AgentModel y)
	{
		return AgentModel.CompareByLevel(x, y);
	}

	// Token: 0x06003385 RID: 13189 RVA: 0x00158180 File Offset: 0x00156380
	public static int CompareByLevel(AgentModel x, AgentModel y)
	{
		if (x == null || y == null)
		{
			Debug.Log("Errror in comparison by level");
			return 0;
		}
		int level = x.level;
		int level2 = y.level;
		return level.CompareTo(level2);
	}

	// Token: 0x06003386 RID: 13190 RVA: 0x0002EF42 File Offset: 0x0002D142
	public override Sefira GetCurrentSefira()
	{
		if (base.currentSefira == "0")
		{
			return null;
		}
		return SefiraManager.instance.GetSefira(base.currentSefira);
	}

	// Token: 0x06003387 RID: 13191 RVA: 0x0002EF68 File Offset: 0x0002D168
	public static void SetPortraitSprite(AgentModel target, Image face, Image hair)
	{
		face.sprite = target.faceSprite;
		hair.sprite = target.hairSprite;
	}

	// Token: 0x06003388 RID: 13192 RVA: 0x001581B8 File Offset: 0x001563B8
	public static string GetLevelGradeText(AgentModel target)
	{
		switch (target.level)
		{
		case 1:
			return "I";
		case 2:
			return "II";
		case 3:
			return "III";
		case 4:
			return "IV";
		case 5:
			return "V";
		case 6:
			return "VI";
		case 7:
			return "VII";
		case 8:
			return "VIII";
		case 9:
			return "IX";
		case 10:
			return "X";
		default:
			return "I";
		}
	}

	// Token: 0x06003389 RID: 13193 RVA: 0x00158240 File Offset: 0x00156440
	public static string GetLevelGradeText(int level)
	{
		switch (level)
		{
		case 1:
			return "I";
		case 2:
			return "II";
		case 3:
			return "III";
		case 4:
			return "IV";
		case 5:
			return "V";
		case 6:
			return "VI";
		case 7:
			return "VII";
		case 8:
			return "VIII";
		case 9:
			return "IX";
		case 10:
			return "X";
		default:
			return "I";
		}
	}

	// Token: 0x0600338A RID: 13194 RVA: 0x001582C0 File Offset: 0x001564C0
	public void DebugDamage(RwbpType type, int damage)
	{
		DamageInfo dmg = new DamageInfo(type, (float)damage);
		base.TakeDamage(dmg);
	}

	// Token: 0x0600338B RID: 13195 RVA: 0x001582E0 File Offset: 0x001564E0
	private void TakeMentalCalculate(float damage)
	{
		AgentUnit unit = this._unit;
		float num = (float)this.maxMental / 5f;
		int num2 = (int)(damage / num);
		if (num2 < 1)
		{
			num2 = 1;
		}
		this.history.MentalDamage(damage);
	}

	// Token: 0x0600338C RID: 13196 RVA: 0x0002EF82 File Offset: 0x0002D182
	public void AddSpecialSkill(SkillTypeInfo skill)
	{
		if (this.HasSkill(skill.id))
		{
			return;
		}
		this.skillInfos.Add(new AgentModel.SkillInfo(skill));
	}

	// Token: 0x0600338D RID: 13197 RVA: 0x0002EFA4 File Offset: 0x0002D1A4
	public void AddSkill(long id)
	{
		if (this.HasSkill(id))
		{
			return;
		}
		this.skillInfos.Add(new AgentModel.SkillInfo(SkillTypeList.instance.GetData(id)));
	}

	// Token: 0x0600338E RID: 13198 RVA: 0x0002EFCB File Offset: 0x0002D1CB
	public void AddSpecialSkill(long id)
	{
		this.AddSkill(id);
	}

	// Token: 0x0600338F RID: 13199 RVA: 0x00158320 File Offset: 0x00156520
	public void RemoveSkill(long id)
	{
		int num = 0;
		using (List<AgentModel.SkillInfo>.Enumerator enumerator = this.skillInfos.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.skill.id == id)
				{
					this.skillInfos.RemoveAt(num);
					break;
				}
				num++;
			}
		}
	}

	// Token: 0x06003390 RID: 13200 RVA: 0x0015838C File Offset: 0x0015658C
	public void RemoveSkill(SkillTypeInfo skill)
	{
		int num = 0;
		using (List<AgentModel.SkillInfo>.Enumerator enumerator = this.skillInfos.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.skill == skill)
				{
					this.skillInfos.RemoveAt(num);
					break;
				}
				num++;
			}
		}
	}

	// Token: 0x06003391 RID: 13201 RVA: 0x0002EFD4 File Offset: 0x0002D1D4
	public void RemoveSkillAll()
	{
		this.skillInfos.Clear();
	}

	// Token: 0x06003392 RID: 13202 RVA: 0x001583F4 File Offset: 0x001565F4
	public AgentModel.SkillInfo[] GetNormalSKill()
	{
		List<AgentModel.SkillInfo> list = new List<AgentModel.SkillInfo>();
		try
		{
			for (long num = 1L; num <= 5L; num += 1L)
			{
				foreach (AgentModel.SkillInfo skillInfo in this.skillInfos)
				{
					if (skillInfo.skill.id == num)
					{
						list.Add(skillInfo);
						break;
					}
				}
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
		return list.ToArray();
	}

	// Token: 0x06003393 RID: 13203 RVA: 0x00158488 File Offset: 0x00156688
	public static Sprite[] GetAgentSkillSprite(AgentModel model)
	{
		List<Sprite> list = new List<Sprite>();
		for (long num = 1L; num <= 5L; num += 1L)
		{
			foreach (AgentModel.SkillInfo skillInfo in model.skillInfos)
			{
				if (skillInfo.skill.id == num)
				{
					int workIconId = AgentModel.GetWorkIconId(skillInfo.skill);
					Sprite icon = IconManager.instance.GetWorkIcon(workIconId).GetIcon(ManageWorkIconState.DEFAULT).icon;
					list.Add(icon);
					break;
				}
			}
		}
		return list.ToArray();
	}

	// Token: 0x06003394 RID: 13204 RVA: 0x00158530 File Offset: 0x00156730
	public static int GetWorkIconId(SkillTypeInfo skill)
	{
		if (skill.id > 5L)
		{
			return IconId.Special1;
		}
		long id = skill.id;
		if (id >= 1L && id <= 5L)
		{
			switch ((int)(id - 1L))
			{
			case 0:
				return IconId.Meal1;
			case 1:
				return IconId.Clean1;
			case 2:
				return IconId.Communion1;
			case 3:
				return IconId.Play1;
			case 4:
				return IconId.Violent1;
			}
		}
		return IconId.Meal1;
	}

	// Token: 0x06003395 RID: 13205 RVA: 0x0002EFE1 File Offset: 0x0002D1E1
	public static Sprite GetPanicIcon()
	{
		return IconManager.instance.GetIcon(IconId.Panic).icon;
	}

	// Token: 0x06003396 RID: 13206 RVA: 0x0002EFF7 File Offset: 0x0002D1F7
	public override bool IsCrazy()
	{
		return this.state == AgentAIState.CANNOT_CONTROLL || this.IsPanic();
	}

	// Token: 0x06003397 RID: 13207 RVA: 0x0002F00A File Offset: 0x0002D20A
	public override void OnResurrect()
	{
		Debug.Log("다시 태어난다");
		this._unit.OnResurrect();
	}

	// Token: 0x06003398 RID: 13208 RVA: 0x001585A4 File Offset: 0x001567A4
	public void HorrorDamage(UnitModel target)
	{ // <Patch>
		if (this.IsCrazy())
		{
			return;
		}
		int num = target.GetRiskLevel() - this.level;
		int level;
		if (target is CreatureModel && (target as CreatureModel).metaInfo.LcId == 100015L)
		{
			if (this.level <= 3)
			{
				level = 6;
			}
			else if (this.level <= 4)
			{
				level = 5;
			}
			else
			{
				level = 2;
			}
		}
		else if (target is CreatureModel && (target as CreatureModel).script is CensoredCreatureBase)
		{
			if (((target as CreatureModel).script as CensoredCreatureBase) is Censored)
			{
				if (this.level <= 4)
				{
					level = 5;
				}
				else
				{
					level = 4;
				}
			}
			else if (this.level <= 3)
			{
				level = 5;
			}
			else if (this.level <= 4)
			{
				level = 4;
			}
			else
			{
				level = 3;
			}
		}
		else if (num <= -1)
		{
			level = 0;
		}
		else if (num <= 0)
		{
			level = 1;
		}
		else if (num <= 1)
		{
			level = 2;
		}
		else if (num <= 2)
		{
			level = 3;
		}
		else if (num <= 3)
		{
			level = 4;
		}
		else
		{
			level = 5;
		}
		if (!this.IsCrazy())
		{
			this._unit.agentUI.EncounterActivate(level);
		}
		float num2 = 0f;
		switch (level)
		{
		case 0:
		case 1:
			num2 = 0f;
			break;
		case 2:
			num2 = 10f;
			break;
		case 3:
			num2 = 30f;
			break;
		case 4:
			num2 = 60f;
			break;
		case 5:
		case 6:
			num2 = 100f;
			break;
		}
		int num3 = (int)((float)this.maxMental * num2 / 100f);
		if (num3 > 0)
		{
			this.TakeDamageWithoutEffect(target, new DamageInfo(RwbpType.W, (float)num3));
		}
		this._unit.SpeechHorrorLyric(level);
		if (this.IsPanic())
		{
			this._unit.SpeechDefaultLyricForce();
		}
		Notice.instance.Send(NoticeName.HorrorDamage, new object[]
		{
			this
		});
	}

	// Token: 0x06003399 RID: 13209 RVA: 0x00158760 File Offset: 0x00156960
	public void HorrorDamageByDead(AgentModel target)
	{
		if (this.IsCrazy())
		{
			return;
		}
		int num = target.level - this.level;
		float num2 = 0f;
		int level;
		if (num <= -2)
		{
			level = 0;
		}
		else if (num <= -1)
		{
			level = 1;
		}
		else if (num <= 0)
		{
			level = 2;
		}
		else if (num <= 1)
		{
			level = 3;
		}
		else if (num <= 2)
		{
			level = 4;
		}
		else
		{
			level = 5;
		}
		if (!this.IsCrazy())
		{
			this._unit.agentUI.EncounterActivate(level);
		}
		switch (level)
		{
		case 0:
		case 1:
			num2 = 0f;
			break;
		case 2:
			num2 = 10f;
			break;
		case 3:
			num2 = 30f;
			break;
		case 4:
			num2 = 60f;
			break;
		case 5:
		case 6:
			num2 = 100f;
			break;
		}
		int num3 = (int)((float)this.maxMental * num2 / 100f);
		if (num3 > 0)
		{
			this.TakeDamageWithoutEffect(target, new DamageInfo(RwbpType.W, (float)num3));
		}
		this._unit.SpeechOtherdeadLyric(level, target.name);
		if (this.IsPanic())
		{
			this._unit.SpeechDefaultLyricForce();
		}
	}

	// Token: 0x0600339A RID: 13210 RVA: 0x00158864 File Offset: 0x00156A64
	public void HorrorDamageByPanic(AgentModel target)
	{
		if (this.IsCrazy())
		{
			return;
		}
		int num = target.level - this.level;
		float num2 = 0f;
		int level;
		if (num <= -2)
		{
			level = 0;
		}
		else if (num <= -1)
		{
			level = 1;
		}
		else if (num <= 0)
		{
			level = 2;
		}
		else if (num <= 1)
		{
			level = 3;
		}
		else if (num <= 2)
		{
			level = 4;
		}
		else
		{
			level = 5;
		}
		if (!this.IsCrazy())
		{
			this._unit.agentUI.EncounterActivate(level);
		}
		switch (level)
		{
		case 0:
		case 1:
			num2 = 0f;
			break;
		case 2:
			num2 = 10f;
			break;
		case 3:
			num2 = 30f;
			break;
		case 4:
			num2 = 60f;
			break;
		case 5:
		case 6:
			num2 = 100f;
			break;
		}
		int num3 = (int)((float)this.maxMental * num2 / 100f);
		if (num3 > 0)
		{
			this.TakeDamageWithoutEffect(target, new DamageInfo(RwbpType.W, (float)num3));
		}
		this._unit.SpeechOtherpanicLyric(level, target.name);
		if (this.IsPanic())
		{
			this._unit.SpeechDefaultLyricForce();
		}
	}

	// Token: 0x0600339B RID: 13211 RVA: 0x0002F021 File Offset: 0x0002D221
	public override void InitialEncounteredCreature(UnitModel encountered)
	{
		if (this.IsDead())
		{
			return;
		}
		if (this.invincible)
		{
			return;
		}
		base.InitialEncounteredCreature(encountered);
		this.HorrorDamage(encountered);
	}

	// Token: 0x0600339C RID: 13212 RVA: 0x00158968 File Offset: 0x00156B68
	public override void InitialEncounteredCreature(RiskLevel level)
	{
		int num = level + 1 - (RiskLevel)this.level;
		if (num < 0)
		{
			num = 0;
		}
		this._unit.agentUI.EncounterActivate(num);
	}

	// Token: 0x0600339D RID: 13213 RVA: 0x0002F043 File Offset: 0x0002D243
	public bool IsIdle()
	{
		return this.state == AgentAIState.IDLE;
	}

	// Token: 0x0600339E RID: 13214 RVA: 0x00003F17 File Offset: 0x00002117
	public bool IsShieldBlock()
	{
		return false;
	}

	// Token: 0x0600339F RID: 13215 RVA: 0x0002F04E File Offset: 0x0002D24E
	public AgentUnit GetUnit()
	{
		return this._unit;
	}

	// Token: 0x060033A0 RID: 13216 RVA: 0x0002F056 File Offset: 0x0002D256
	public override GameObject MakeCreatureEffectHead(CreatureModel model)
	{
		return this._unit.MakeCreatureEffect(model);
	}

	// Token: 0x060033A1 RID: 13217 RVA: 0x0002F064 File Offset: 0x0002D264
	public override GameObject MakeCreatureEffectHead(CreatureModel model, bool addlist)
	{
		return this._unit.MakeCreatureEffect(model, addlist);
	}

	// Token: 0x060033A2 RID: 13218 RVA: 0x0002F073 File Offset: 0x0002D273
	public override GameObject MakeCreatureEffect(long id)
	{
		return this._unit.MakeCreatureEffect(id);
	}

	// Token: 0x060033A3 RID: 13219 RVA: 0x00158998 File Offset: 0x00156B98
	public void MakeRecoverEffect(bool isPhyiscal)
	{
		if (this._unit == null)
		{
			return;
		}
		if (isPhyiscal)
		{
			this._unit.MakeEffectAttach("Effect/RecoverHP", this._unit.animRoot.transform, false);
			return;
		}
		this._unit.MakeEffectAttach("Effect/RecoverMental", this._unit.animRoot.transform, false);
	}

	// Token: 0x060033A4 RID: 13220 RVA: 0x0002F081 File Offset: 0x0002D281
	public override void ClearEffect()
	{
		if (this._unit != null)
		{
			this._unit.ClearEffect();
		}
	}

	// Token: 0x060033A5 RID: 13221 RVA: 0x0002F09C File Offset: 0x0002D29C
	public Sprite GetRecentWorkIcon()
	{
		if (this.recentWork == null)
		{
			return null;
		}
		return IconManager.instance.GetWorkIcon(AgentModel.GetWorkIconId(this.recentWork)).GetIcon(ManageWorkIconState.DEFAULT).icon;
	}

	// Token: 0x060033A6 RID: 13222 RVA: 0x0002F0C8 File Offset: 0x0002D2C8
	public SkillTypeInfo GetRecentWork()
	{
		return this.recentWork;
	}

	// Token: 0x060033A7 RID: 13223 RVA: 0x0002F0D0 File Offset: 0x0002D2D0
	public override void RecentlyAttackedCreature(CreatureModel creatureModel)
	{
		base.RecentlyAttackedCreature(creatureModel);
		if (this.counterAttackEnabled && this.state == AgentAIState.IDLE)
		{
			this.Suppress(creatureModel, true);
		}
	}

	// Token: 0x060033A8 RID: 13224 RVA: 0x0002F0F1 File Offset: 0x0002D2F1
	public int GetContinuousServiceLevel()
	{ // <Mod>
		if (this.isAce)
		{
			if (!ResearchDataModel.instance.IsUpgradedAbility("upgrade_service_bonuses") || continuousServiceDay <= 13)
			{
				return 4;
			}
			return 6;
		}
		if (this.continuousServiceDay <= 2)
		{
			return 1;
		}
		if (this.continuousServiceDay <= 6)
		{
			return 2;
		}
		if (!ResearchDataModel.instance.IsUpgradedAbility("upgrade_service_bonuses") || continuousServiceDay <= 13)
		{
			return 3;
		}
		return 5;
	}

	// Token: 0x060033A9 RID: 13225 RVA: 0x001589FC File Offset: 0x00156BFC
	public int GetFortitudeStatBySefiraAbility()
	{ // <Mod>
		int num = 0;
		num += SefiraAbilityValueInfo.hokmaOfficerAliveValues[SefiraManager.instance.GetOfficerAliveLevel(SefiraEnum.CHOKHMAH)];
		if (ResearchDataModel.instance.IsUpgradedAbility("upgrade_officer_bonuses"))
		{
			num *= 2;
		}
		if (this.currentSefiraEnum == SefiraEnum.NETZACH)
		{
			num += SefiraAbilityValueInfo.netzachContinuousServiceValues[this.GetContinuousServiceLevel() - 1];
		}/*
		if (this.currentSefiraEnum == SefiraEnum.HOD && this.GetContinuousServiceLevel() == 4)
		{
			num += SefiraAbilityValueInfo.hodContinuousServiceValues[3];
		}*/
		if (this.currentSefiraEnum == SefiraEnum.TIPERERTH1)
		{
			num += SefiraAbilityValueInfo.tipherethContinuousServiceValues[this.GetContinuousServiceLevel() - 1];
		}
		if (this.currentSefiraEnum == SefiraEnum.CHOKHMAH)
		{
			num += SefiraAbilityValueInfo.hokmaContinuousServiceValues[this.GetContinuousServiceLevel() - 1];
		}
		if (this.currentSefiraEnum == SefiraEnum.KETHER)
		{
			num += SefiraAbilityValueInfo.ketherContinuousServiceValues[this.GetContinuousServiceLevel() - 1];
		}
		return num;
	}

	// Token: 0x060033AA RID: 13226 RVA: 0x001589FC File Offset: 0x00156BFC
	public int GetPrudenceStatBySefiraAbility()
	{ // <Mod>
		int num = 0;
		num += SefiraAbilityValueInfo.hokmaOfficerAliveValues[SefiraManager.instance.GetOfficerAliveLevel(SefiraEnum.CHOKHMAH)];
		if (ResearchDataModel.instance.IsUpgradedAbility("upgrade_officer_bonuses"))
		{
			num *= 2;
		}
		if (this.currentSefiraEnum == SefiraEnum.NETZACH)
		{
			num += SefiraAbilityValueInfo.netzachContinuousServiceValues[this.GetContinuousServiceLevel() - 1];
		}/*
		if (this.currentSefiraEnum == SefiraEnum.HOD && this.GetContinuousServiceLevel() == 4)
		{
			num += SefiraAbilityValueInfo.hodContinuousServiceValues[3];
		}*/
		if (this.currentSefiraEnum == SefiraEnum.TIPERERTH1)
		{
			num += SefiraAbilityValueInfo.tipherethContinuousServiceValues[this.GetContinuousServiceLevel() - 1];
		}
		if (this.currentSefiraEnum == SefiraEnum.CHOKHMAH)
		{
			num += SefiraAbilityValueInfo.hokmaContinuousServiceValues[this.GetContinuousServiceLevel() - 1];
		}
		if (this.currentSefiraEnum == SefiraEnum.KETHER)
		{
			num += SefiraAbilityValueInfo.ketherContinuousServiceValues[this.GetContinuousServiceLevel() - 1];
		}
		return num;
	}

	// Token: 0x060033AB RID: 13227 RVA: 0x00158AA8 File Offset: 0x00156CA8
	public int GetTemperanceStatBySefiraAbility()
	{ // <Mod>
		int num = 0;
		num += SefiraAbilityValueInfo.hokmaOfficerAliveValues[SefiraManager.instance.GetOfficerAliveLevel(SefiraEnum.CHOKHMAH)];
		if (ResearchDataModel.instance.IsUpgradedAbility("upgrade_officer_bonuses"))
		{
			num *= 2;
		}
		if (this.currentSefiraEnum == SefiraEnum.YESOD)
		{
			num += SefiraAbilityValueInfo.yesodContinuousServiceValues[this.GetContinuousServiceLevel() - 1];
		}/*
		if (this.currentSefiraEnum == SefiraEnum.HOD && this.GetContinuousServiceLevel() == 4)
		{
			num += SefiraAbilityValueInfo.hodContinuousServiceValues[3];
		}*/
		if (this.currentSefiraEnum == SefiraEnum.TIPERERTH1)
		{
			num += SefiraAbilityValueInfo.tipherethContinuousServiceValues[this.GetContinuousServiceLevel() - 1];
		}
		if (this.currentSefiraEnum == SefiraEnum.CHOKHMAH)
		{
			num += SefiraAbilityValueInfo.hokmaContinuousServiceValues[this.GetContinuousServiceLevel() - 1];
		}
		if (this.currentSefiraEnum == SefiraEnum.KETHER)
		{
			num += SefiraAbilityValueInfo.ketherContinuousServiceValues[this.GetContinuousServiceLevel() - 1];
		}
		return num;
	}

	// Token: 0x060033AC RID: 13228 RVA: 0x00158B54 File Offset: 0x00156D54
	public int GetJusticeStatBySefiraAbility()
	{ // <Mod>
		int num = 0;
		int num2 = 0;
		if (num2 != 1)
		{
			if (num2 != 2)
			{
				if (num2 == 3)
				{
					num += 5;
				}
			}
			else
			{
				num += 3;
			}
		}
		else
		{
			num++;
		}
		num += SefiraAbilityValueInfo.hokmaOfficerAliveValues[SefiraManager.instance.GetOfficerAliveLevel(SefiraEnum.CHOKHMAH)];
		if (ResearchDataModel.instance.IsUpgradedAbility("upgrade_officer_bonuses"))
		{
			num *= 2;
		}
		/*
		if (this.currentSefiraEnum == SefiraEnum.HOD && this.GetContinuousServiceLevel() == 4)
		{
			num += SefiraAbilityValueInfo.hodContinuousServiceValues[3];
		}*/
		if (this.currentSefiraEnum == SefiraEnum.TIPERERTH1)
		{
			num += SefiraAbilityValueInfo.tipherethContinuousServiceValues[this.GetContinuousServiceLevel() - 1];
		}
		if (this.currentSefiraEnum == SefiraEnum.CHOKHMAH)
		{
			num += SefiraAbilityValueInfo.hokmaContinuousServiceValues[this.GetContinuousServiceLevel() - 1];
		}
		if (this.currentSefiraEnum == SefiraEnum.KETHER)
		{
			num += SefiraAbilityValueInfo.ketherContinuousServiceValues[this.GetContinuousServiceLevel() - 1];
		}
		return num;
	}

	// Token: 0x060033AD RID: 13229 RVA: 0x0002F114 File Offset: 0x0002D314
	public int GetAttackSpeedBufBySefiraAbility()
	{ // <Mod>
		int num = SefiraAbilityValueInfo.geburahOfficerAliveValues[SefiraManager.instance.GetOfficerAliveLevel(SefiraEnum.GEBURAH)];
		if (ResearchDataModel.instance.IsUpgradedAbility("upgrade_officer_bonuses"))
		{
			num *= 2;
		}
		return num;
	}

	// Token: 0x060033AE RID: 13230 RVA: 0x0002F129 File Offset: 0x0002D329
	public int GetWorkProbBufBySefiraAbility()
	{ // <Mod>
		int num = SefiraAbilityValueInfo.yesodOfficerAliveValues[SefiraManager.instance.GetOfficerAliveLevel(SefiraEnum.YESOD)];
		if (ResearchDataModel.instance.IsUpgradedAbility("upgrade_officer_bonuses"))
		{
			num *= 2;
		}
		return num;
	}

	// Token: 0x060033AF RID: 13231 RVA: 0x00158C04 File Offset: 0x00156E04
	public int GetMovementBufBySefiraAbility()
	{ // <Mod>
		int num = 0;
		num += SefiraAbilityValueInfo.malkuthOfficerAliveValues[SefiraManager.instance.GetOfficerAliveLevel(SefiraEnum.MALKUT)];
		if (ResearchDataModel.instance.IsUpgradedAbility("upgrade_officer_bonuses"))
		{
			num *= 2;
		}
		if (this.currentSefiraEnum == SefiraEnum.MALKUT)
		{
			num += SefiraAbilityValueInfo.malkuthContinuousServiceValues[this.GetContinuousServiceLevel() - 1];
		}
		return num;
	}

	// Token: 0x060033B0 RID: 13232 RVA: 0x00158C44 File Offset: 0x00156E44
	public override float GetDamageFactorBySefiraAbility()
	{ // <Mod>
		if (this.currentSefiraEnum == SefiraEnum.GEBURAH)
		{
			return 1f + (float)SefiraAbilityValueInfo.geburahContinuousServiceValues[this.GetContinuousServiceLevel() - 1] / 100f;
		}
		if (this.currentSefiraEnum == SefiraEnum.BINAH)
		{
			return 1f + (float)SefiraAbilityValueInfo.binahContinuousServiceValues[this.GetContinuousServiceLevel() - 1] / 100f;
		}
		if (currentSefiraEnum == SefiraEnum.KETHER)
		{
			return 1f + (float)SefiraAbilityValueInfo.ketherContinuousServiceValues2[GetContinuousServiceLevel() - 1] / 100f;
		}
		return 1f;
	}

	// Token: 0x060033B1 RID: 13233 RVA: 0x00158CA0 File Offset: 0x00156EA0
	public void SetToAce()
	{
		this.isAce = true;
		if (base.currentSefira != "0")
		{
			this._unit.spriteSetter.SetSefira(SefiraManager.instance.GetSefira(base.currentSefira).sefiraEnum, this.GetContinuousServiceLevel());
		}
		this._unit.ui.activateUI(this);
	}

	// Token: 0x060033B2 RID: 13234 RVA: 0x0002F13E File Offset: 0x0002D33E
	public override WorkerDeadScript ChangePuppetAnimToDie(string src)
	{
		if (this.puppetChanged)
		{
			return null;
		}
		this.puppetChanged = true;
		return null;
	}

	// Token: 0x060033B3 RID: 13235 RVA: 0x0002F152 File Offset: 0x0002D352
	public override void ChangePuppetAnimToUncon(string src)
	{
		if (this.puppetChanged)
		{
			return;
		}
		this.puppetChanged = true;
	}

	// Token: 0x060033B4 RID: 13236 RVA: 0x00158D04 File Offset: 0x00156F04
	public override void OnNotice(string notice, params object[] param)
	{
		if (notice == NoticeName.UnconWorkerDead)
		{
			if (param[0] is WorkerModel && (param[0] as WorkerModel).Equals(this.targetWorker))
			{
				Debug.Log("dead");
				this.history.SuccessWorkerSuppress();
				this.FinishSuppress();
				this.commandQueue.Clear();
				return;
			}
		}
		else if (notice == NoticeName.CreatureSuppressCancel)
		{
			if (param[0] is CreatureModel && (param[0] as CreatureModel).Equals(this.target))
			{
				this.history.SuccessCreatureSuppress();
				this.FinishSuppress();
				this.commandQueue.Clear();
				return;
			}
		}
		else if (notice == NoticeName.ManageCancel && param[0] is CreatureModel)
		{
			if (this.target == null)
			{
				return;
			}
			if ((param[0] as CreatureModel).instanceId.Equals(this.target.instanceId))
			{
				this.ForcelyCancelWork();
			}
		}
	}

	// Token: 0x060033B5 RID: 13237 RVA: 0x0002F04E File Offset: 0x0002D24E
	public override WorkerUnit GetWorkerUnit()
	{
		return this._unit;
	}

	// Token: 0x060033B6 RID: 13238 RVA: 0x0002F164 File Offset: 0x0002D364
	public bool CanObserveCreature(CreatureModel model)
	{
		return model.script == null || model.script.CanObservedByAgent(this);
	}

	// Token: 0x060033B7 RID: 13239 RVA: 0x0002F17C File Offset: 0x0002D37C
	public void ForcelyCancelSuppress()
	{
		this.counterAttackEnabled = false;
		this._isAutoSuppressing = false;
		this.FinishSuppress();
		this.StopAction();
		base.MoveToMovable(MapGraph.instance.GetSefiraMovableNodeByRandom(base.currentSefira));
	}

	// Token: 0x060033B8 RID: 13240 RVA: 0x00158DF8 File Offset: 0x00156FF8
	public void ForcelyCancelWork()
	{
		if (!this.canCancelCurrentWork)
		{
			return;
		}
		if (this.target != null && this.target.Unit != null)
		{
			this.target.Unit.room.OnCancelWork();
		}
		this.counterAttackEnabled = false;
		this.StopAction();
		base.MoveToMovable(MapGraph.instance.GetSefiraMovableNodeByRandom(base.currentSefira));
	}

	// Token: 0x060033B9 RID: 13241 RVA: 0x0002F1AE File Offset: 0x0002D3AE
	public void OnEnterRoom(UseSkill skill)
	{
		this.oldWork = skill.skillTypeInfo;
		this.recentWorkedCreature = skill.targetCreature;
	}

	// Token: 0x060033BA RID: 13242 RVA: 0x0002F1C8 File Offset: 0x0002D3C8
	public SkillTypeInfo GetOldWork()
	{
		return this.oldWork;
	}

	// Token: 0x060033BB RID: 13243 RVA: 0x0002F1D0 File Offset: 0x0002D3D0
	public CreatureModel GetRecentWorkedCreature()
	{
		return this.recentWorkedCreature;
	}

	// Token: 0x060033BC RID: 13244 RVA: 0x0002F1D8 File Offset: 0x0002D3D8
	public List<EGOgiftModel> GetAllGifts()
	{
		List<EGOgiftModel> list = new List<EGOgiftModel>();
		list.AddRange(base.Equipment.gifts.replacedGifts);
		list.AddRange(base.Equipment.gifts.addedGifts);
		return list;
	}

	// Token: 0x060033BD RID: 13245 RVA: 0x00013AE1 File Offset: 0x00011CE1
	public static bool DummyCheckCommand()
	{
		return true;
	}

	// Token: 0x060033BE RID: 13246 RVA: 0x0002F20B File Offset: 0x0002D40B
	public void SetWorkCommandCheckEvnet(AgentModel.CheckCommandState check)
	{
		this._workCommand = check;
	}

	// Token: 0x060033BF RID: 13247 RVA: 0x0002F214 File Offset: 0x0002D414
	public void SetSuppressCommandCheckEvent(AgentModel.CheckCommandState check)
	{
		this._suppressCommand = check;
	}

	//> <Mod>
	public void EnqueueWorkOrder(QueuedWorkOrder order)
	{
		_workOrderQueue.Add(order);
		if (_workOrderQueue.Count == 1)
		{
			order.isAgentFront = true;
		}
	}
	
	public void DequeueWorkOrder()
	{
		if (_workOrderQueue.Count > 0)
		{
			_workOrderQueue.RemoveAt(0);
			if (_workOrderQueue.Count > 0)
			{
				_workOrderQueue[0].isAgentFront = true;
			}
		}
	}
	
	public void DequeueWorkOrder(QueuedWorkOrder order)
	{
		_workOrderQueue.Remove(order);
		if (_workOrderQueue.Count > 0)
		{
			_workOrderQueue[0].isAgentFront = true;
		}
	}
	
	public bool CanQueueWorkOrder()
	{
		int maxWorkCount = 3;
		if (!SpecialModeConfig.instance.GetValue<bool>("OvertimeMissions") || MissionManager.instance.ExistsFinishedOvertimeBossMission(SefiraEnum.MALKUT))
		{
			maxWorkCount = 5;
		}
		if (maxWorkCount == -1)
		{
			return true;
		}
		int num = _workOrderQueue.Count;
		if (state == AgentAIState.MANAGE)
		{
			num++;
		}
		return num < maxWorkCount;
	}
	
	public void ClearWorkOrderQueue()
	{
		foreach (QueuedWorkOrder order in _workOrderQueue)
		{
			order.room.DequeueWorkOrder(order);
		}
		_workOrderQueue.Clear();
	}

	public List<QueuedWorkOrder> GetWorkOrderQueue()
	{
		return _workOrderQueue;
	}

	private List<QueuedWorkOrder> _workOrderQueue = new List<QueuedWorkOrder>();

	public void ForcelyChangePrefix(int title)
	{
		prefix = AgentTitleTypeList.instance.GetData(title);
	}

	public void ForcelyChangeSuffix(int title)
	{
		suffix = AgentTitleTypeList.instance.GetData(title);
	}

	public void FeignDeathScene(string specialDeadSceneName)
	{
		try
		{
			if (this.hasUniqueFace)
			{
				this.GetUnit().animChanger.ChangeAnimatorWithUniqueFace(specialDeadSceneName, this.seperator);
			}
			else
			{
				this.GetUnit().SetWorkerFaceType(WorkerFaceType.PANIC);
				this.GetUnit().animChanger.ChangeAnimator(specialDeadSceneName, this.seperator);
			}
		}
		catch (Exception)
		{
		}
	}

	[NonSerialized]
	public List<UnitModel> cannotAttackUnits = new List<UnitModel>();

	public bool ForceHideUI
	{
		get
		{
			return _forceHideUI;
		}
		set
		{
			_forceHideUI = value;
		}
	}

	private bool _forceHideUI = false;

	[NonSerialized]
	public int[] forceTitles = new int[4];
	//<

	// Token: 0x04003061 RID: 12385
	private const string reloadSound = "Agent/Weapon/Reload_Pistol";

	// Token: 0x04003062 RID: 12386
	public const int MinLevel = 1;

	// Token: 0x04003063 RID: 12387
	public const int MaxLevel = 5;

	// Token: 0x04003064 RID: 12388
	public WorkerPrimaryStat primaryStat = new WorkerPrimaryStat();

	// Token: 0x04003065 RID: 12389
	public WorkerPrimaryStatExp primaryStatExp = new WorkerPrimaryStatExp();

	// Token: 0x04003066 RID: 12390
	public AgentHistory history;

	// Token: 0x04003067 RID: 12391
	private List<AgentModel.SkillInfo> skillInfos;

	// Token: 0x04003068 RID: 12392
	public Sprite[] StatusSprites = new Sprite[4];

	// Token: 0x04003069 RID: 12393
	public SpriteInfo tempHairSpriteInfo;

	// Token: 0x0400306A RID: 12394
	public SpriteInfo tempFaceSpriteInfo;

	// Token: 0x0400306B RID: 12395
	private AgentTitleTypeInfo prefix;

	// Token: 0x0400306C RID: 12396
	private AgentTitleTypeInfo suffix;

	// Token: 0x0400306D RID: 12397
	public bool iscustom;

	// Token: 0x0400306E RID: 12398
	public bool isUniqueCredit;

	// Token: 0x0400306F RID: 12399
	public int uniqueScriptIndex = -1;

	// Token: 0x04003070 RID: 12400
	public AgentName _agentName;

	// Token: 0x04003071 RID: 12401
	public string lastServiceSefira = "0";

	// Token: 0x04003072 RID: 12402
	public int continuousServiceDay = 1;

	// Token: 0x04003073 RID: 12403
	public bool isAce;

	// Token: 0x04003074 RID: 12404
	private PassageObjectModel _currentWaitingPassage;

	// Token: 0x04003075 RID: 12405
	private bool panicReport;

	// Token: 0x04003076 RID: 12406
	private bool deadInit;

	// Token: 0x04003077 RID: 12407
	private UseSkill _currentSkill;

	// Token: 0x04003078 RID: 12408
	private AgentModelEventHandler _eventHandler;

	// Token: 0x04003079 RID: 12409
	private AgentModel.CheckCommandState _workCommand;

	// Token: 0x0400307A RID: 12410
	private AgentModel.CheckCommandState _suppressCommand;

	// Token: 0x0400307B RID: 12411
	public bool activated;

	// Token: 0x0400307C RID: 12412
	[NonSerialized]
	public bool cannotBeAttackTargetable;

	// Token: 0x0400307D RID: 12413
	public bool workEndReaction;

	// Token: 0x0400307E RID: 12414
	private AgentUnit _unit;

	// Token: 0x0400307F RID: 12415
	public ValueInfo levelSetting;

	// Token: 0x04003080 RID: 12416
	private AgentAIState _state;

	// Token: 0x04003081 RID: 12417
	public string[] randomOverlay = new string[4];

	// Token: 0x04003082 RID: 12418
	public string[] randomLevel = new string[4];

	// Token: 0x04003083 RID: 12419
	private SkillTypeInfo recentWork;

	// Token: 0x04003084 RID: 12420
	private SkillTypeInfo oldWork;

	// Token: 0x04003085 RID: 12421
	private RwbpType _bestRwbp = RwbpType.R;

	// Token: 0x04003086 RID: 12422
	public RwbpType forcelyPanicType;

	// Token: 0x04003087 RID: 12423
	[NonSerialized]
	private CreatureModel recentWorkedCreature;

	// Token: 0x04003088 RID: 12424
	public bool uiActivated = true;

	// Token: 0x04003089 RID: 12425
	public bool counterAttackEnabled = true;

	// Token: 0x0400308A RID: 12426
	private bool _isAutoSuppressing;

	// Token: 0x0400308B RID: 12427
	[CompilerGenerated]
	private static AgentModel.CheckCommandState f__mgcache0;

	// Token: 0x0400308C RID: 12428
	[CompilerGenerated]
	private static AgentModel.CheckCommandState f__mgcache1;

	// Token: 0x020005E6 RID: 1510
	public class SkillInfo : ISerializablePlayData
	{
		// Token: 0x060033C0 RID: 13248 RVA: 0x00003DE0 File Offset: 0x00001FE0
		public SkillInfo()
		{
		}

		// Token: 0x060033C1 RID: 13249 RVA: 0x0002F21D File Offset: 0x0002D41D
		public SkillInfo(SkillTypeInfo info)
		{
			this.skill = info;
		}

		// Token: 0x060033C2 RID: 13250 RVA: 0x0002F22C File Offset: 0x0002D42C
		public Dictionary<string, object> GetSaveData()
		{
			return new Dictionary<string, object>
			{
				{
					"skillId",
					this.skill.id
				}
			};
		}

		// Token: 0x060033C3 RID: 13251 RVA: 0x00158E64 File Offset: 0x00157064
		public void LoadData(Dictionary<string, object> dic)
		{
			long num = 0L;
			GameUtil.TryGetValue<long>(dic, "skillId", ref num);
			this.skill = SkillTypeList.instance.GetData(num);
			if (this.skill == null)
			{
				Debug.Log("skill not found >> " + num);
			}
		}

		// Token: 0x0400308D RID: 12429
		public SkillTypeInfo skill;
	}

	// Token: 0x020005E7 RID: 1511
	// (Invoke) Token: 0x060033C5 RID: 13253
	public delegate bool CheckCommandState();
}
