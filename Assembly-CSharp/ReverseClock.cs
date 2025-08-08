using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000469 RID: 1129
public class ReverseClock : CreatureBase
{
	// Token: 0x0600287B RID: 10363 RVA: 0x0002800A File Offset: 0x0002620A
	public ReverseClock()
	{
	}

	// Token: 0x0600287C RID: 10364 RVA: 0x0011AD78 File Offset: 0x00118F78
	public override void ParamInit()
	{
		base.ParamInit();
		if (!this.model.Unit.gameObject.activeSelf)
		{
			this.model.Unit.gameObject.SetActive(true);
		}
		this._workTime = 5f;
		this._maxEnergy = 0f;
		this._specialAgentList.Clear();
		this._allActivated = false;
		this._tempAgent = null;
		this._successSpecialSkill = false;
		this._specialWorkTimer.StopTimer();
		this._rouletteLastingTimer.StopTimer();
		this._rouleteStartTimer.StopTimer();
		this._animScript.ResetLamp();
	}

	// Token: 0x0600287D RID: 10365 RVA: 0x0011AE20 File Offset: 0x00119020
	public override void RoomSpriteInit()
	{
		base.RoomSpriteInit();
		this._specialFilter = base.Unit.room.AddFilter(base.Unit.room.StateFilter);
		this._specialFilter.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
		this._specialFilter.renderSprite = Resources.Load<Sprite>("Sprites/CreatureSprite/Isolate/" + this.model.metadataId);
		this._specialFilter.specialAnimKey = "Play";
		this._specialFilter.hasSpecialAnimKey = true;
		this._specialFilter.renderAnim.enabled = true;
	}

	// Token: 0x0600287E RID: 10366 RVA: 0x0002803E File Offset: 0x0002623E
	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
		this._animScript = (unit.animTarget as ReverseClockAnim);
		this._animScript.SetModel(this.model);
	}

	// Token: 0x0600287F RID: 10367 RVA: 0x00020796 File Offset: 0x0001E996
	public override void OnStageStart()
	{
		base.OnStageStart();
		this.ParamInit();
	}

	// Token: 0x06002880 RID: 10368 RVA: 0x0011AED4 File Offset: 0x001190D4
	public override void OnStageEnd()
	{
		base.OnStageEnd();
		foreach (AgentModel agentModel in this._specialAgentList)
		{
			agentModel.SetInvincible(false);
			agentModel.Die();
		}
		this.ParamInit();
	}

	// Token: 0x06002881 RID: 10369 RVA: 0x00028069 File Offset: 0x00026269
	public override bool OnOpenWorkWindow()
	{
		return base.OnOpenWorkWindow() && this.model.Unit.gameObject.activeSelf;
	}

	// Token: 0x06002882 RID: 10370 RVA: 0x0011AF44 File Offset: 0x00119144
	public override void OnEnterRoom(UseSkill skill)
	{
		base.OnEnterRoom(skill);
		if (!this.model.Unit.gameObject.activeSelf)
		{
			return;
		}
		this._animScript.animator.SetTrigger("Start");
		if (this._specialFilter.Activated)
		{
			this._tempAgent = skill.agent;
			if (skill.agent.level >= 5)
			{
				this._successSpecialSkill = true;
			}
			else
			{
				this._successSpecialSkill = false;
			}
			this._specialWorkTimer.StartTimer(this._workTime);
			this._workTime = 50f;
			this._specialFilter.Activated = false;
		}
	}

	// Token: 0x06002883 RID: 10371 RVA: 0x0011AFF0 File Offset: 0x001191F0
	public override void OnReleaseWork(UseSkill skill)
	{
		base.OnReleaseWork(skill);
		if (!this.model.Unit.gameObject.activeSelf)
		{
			return;
		}
		if (this._allActivated)
		{
			this._allActivated = false;
		}
		else if (skill.agent.level >= 3)
		{
			this._allActivated = this._animScript.TurnOnLamp();
			this._specialFilter.Activated = this._allActivated;
		}
		this._animScript.animator.SetTrigger("Default");
		this._animScript.animEventHandler.StopWorkLoopSound();
	}

	// Token: 0x06002884 RID: 10372 RVA: 0x0002808E File Offset: 0x0002628E
	public override float GetKitCreatureProcessTime()
	{
		return this._workTime;
	}

	// Token: 0x06002885 RID: 10373 RVA: 0x0011B090 File Offset: 0x00119290
	public override void OnFixedUpdateInSkill(UseSkill skill)
	{
		base.OnFixedUpdateInSkill(skill);
		if (this._specialWorkTimer.RunTimer())
		{
			skill.agent.workerAnimator.SetTrigger("Put");
			this._rouleteStartTimer.StartTimer(1.5f);
			this._animScript.animator.SetTrigger("Put");
		}
		if (this._rouleteStartTimer.RunTimer())
		{
			this.ViewRouletteEvent();
		}
		if (this._rouletteLastingTimer.RunTimer())
		{
			this._animScript.rouletteUI.anim.SetTrigger("Display");
			this._animScript.rouletteUI.SetHidingEvent(new ReverseClockUI.HideUIEvent(this.HideRouletteEvent));
		}
	}

	// Token: 0x06002886 RID: 10374 RVA: 0x0011B14C File Offset: 0x0011934C
	public override void OnFixedUpdate(CreatureModel creature)
	{
		base.OnFixedUpdate(creature);
		float energy = EnergyModel.instance.GetEnergy();
		if (energy >= this._maxEnergy)
		{
			this._maxEnergy = energy;
		}
	}

	// Token: 0x06002887 RID: 10375 RVA: 0x00028096 File Offset: 0x00026296
	private void HideRouletteEvent()
	{
		this._animScript.HideRoulette();
		this._workTime = 5f;
		this.Retrograde(this._successSpecialSkill);
		this._animScript.ResetLamp();
	}

	// Token: 0x06002888 RID: 10376 RVA: 0x000280C5 File Offset: 0x000262C5
	private void ViewRouletteEvent()
	{
		this._animScript.ViewRoulette();
		this._rouletteLastingTimer.StartTimer(5f);
	}

	// Token: 0x06002889 RID: 10377 RVA: 0x0011B180 File Offset: 0x00119380
	private void Retrograde(bool b)
	{
		if (b)
		{
			float energy = EnergyModel.instance.GetEnergy();
			if (energy < this._maxEnergy)
			{
				EnergyModel.instance.AddEnergy(this._maxEnergy - energy);
			}
			if (!this._tempAgent.IsDead())
			{
				if (this._tempAgent.HasUnitBuf(UnitBufType.OTHER_WORLD_PORTRAIT_VICTIM))
				{
					CreatureModel creatureModel = CreatureManager.instance.FindCreature(300104L);
					if (creatureModel != null)
					{
						(creatureModel.script as OtherWorldPortrait).ReleaseVictim(this._tempAgent);
					}
				}
				this._tempAgent.Die();
				this._tempAgent.GetUnit().gameObject.SetActive(false);
				this._specialAgentList.Add(this._tempAgent);
			}
			List<OrdealBase> activatedOrdeals = OrdealManager.instance.GetActivatedOrdeals();
			foreach (OrdealBase ordealBase in activatedOrdeals)
			{
				ordealBase.canTakeRewards = false;
			}
			foreach (OrdealCreatureModel ordealCreatureModel in OrdealManager.instance.GetOrdealCreatureList())
			{
				if (ordealCreatureModel.OrdealBase.level != OrdealLevel.MIDNIGHT && ordealCreatureModel.OrdealBase.level != OrdealLevel.OVERTIME_MIDNIGHT)
				{
					ordealCreatureModel.Suppressed();
				}
			}
			foreach (CreatureModel creatureModel2 in CreatureManager.instance.GetCreatureList())
			{
				if (this.CheckCreatureSkillTargetable(creatureModel2))
				{
					List<ChildCreatureModel> aliveChilds = creatureModel2.GetAliveChilds();
					foreach (ChildCreatureModel childCreatureModel in aliveChilds)
					{
						if (childCreatureModel.metadataId != 100038L)
						{
							if (childCreatureModel.script.IsSuppressable())
							{
								childCreatureModel.Suppressed();
							}
						}
					}
					if (creatureModel2.metadataId != 100003L && creatureModel2.metadataId != 100045L && creatureModel2.IsEscaped())
					{
						creatureModel2.Suppressed();
					}
					if (creatureModel2.isOverloaded)
					{
						creatureModel2.CancelOverload();
						creatureModel2.ClearProbReduction();
					}
					creatureModel2.ResetProbReductionCounter();
				}
			}
			this.model.Unit.gameObject.SetActive(false);
			this._workTime = 0f;
		}
		else
		{
			foreach (AgentModel agentModel in AgentManager.instance.GetAgentList())
			{
				if (agentModel != this._tempAgent)
				{
					if (base.Prob(50))
					{
						agentModel.Die();
					}
					else
					{
						agentModel.Panic();
					}
				}
			}
			foreach (OfficerModel officerModel in OfficerManager.instance.GetOfficerList())
			{
				if (base.Prob(50))
				{
					officerModel.Die();
				}
				else
				{
					officerModel.Panic();
				}
			}
		}
	}

	// Token: 0x0600288A RID: 10378 RVA: 0x0011B4F8 File Offset: 0x001196F8
	private bool CheckCreatureSkillTargetable(CreatureModel creature)
	{
		return creature.metadataId != 100015L && creature.metadataId != 100064L && creature.metadataId != 400001L && creature.metadataId != 400002L;
	}

	// Token: 0x040026DD RID: 9949
	private const int _SPECIAL_AGENT_LEVEL = 5;

	// Token: 0x040026DE RID: 9950
	private const int _NORMAL_AGENT_LEVEL = 3;

	// Token: 0x040026DF RID: 9951
	private const float _DEFAULT_WORK_TIME = 5f;

	// Token: 0x040026E0 RID: 9952
	private const float _ROULETTE_LASTING_TIME = 5f;

	// Token: 0x040026E1 RID: 9953
	private const float _ROULETTE_START_DELAY_TIME = 1.5f;

	// Token: 0x040026E2 RID: 9954
	private float _maxEnergy;

	// Token: 0x040026E3 RID: 9955
	private List<AgentModel> _specialAgentList = new List<AgentModel>();

	// Token: 0x040026E4 RID: 9956
	private IsolateFilter _specialFilter;

	// Token: 0x040026E5 RID: 9957
	private float _workTime;

	// Token: 0x040026E6 RID: 9958
	private AgentModel _tempAgent;

	// Token: 0x040026E7 RID: 9959
	private ReverseClockAnim _animScript;

	// Token: 0x040026E8 RID: 9960
	private bool _allActivated;

	// Token: 0x040026E9 RID: 9961
	private bool _successSpecialSkill;

	// Token: 0x040026EA RID: 9962
	private Timer _specialWorkTimer = new Timer();

	// Token: 0x040026EB RID: 9963
	private Timer _rouletteLastingTimer = new Timer();

	// Token: 0x040026EC RID: 9964
	private Timer _rouleteStartTimer = new Timer();
}
