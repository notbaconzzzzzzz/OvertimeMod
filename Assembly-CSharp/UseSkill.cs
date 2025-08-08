using System;
using UnityEngine;

// Token: 0x02000B82 RID: 2946
public class UseSkill
{
	// Token: 0x060058D8 RID: 22744 RVA: 0x000471A0 File Offset: 0x000453A0
	public UseSkill()
	{
	}

	// Token: 0x17000812 RID: 2066
	// (get) Token: 0x060058D9 RID: 22745 RVA: 0x000471C1 File Offset: 0x000453C1
	public bool IsWorkPlaying
	{
		get
		{
			return this.workPlaying;
		}
	}

	// Token: 0x17000813 RID: 2067
	// (get) Token: 0x060058DA RID: 22746 RVA: 0x000471C9 File Offset: 0x000453C9
	public bool creatureFaced
	{
		get
		{
			return this._faceCreature;
		}
	}

	// Token: 0x17000814 RID: 2068
	// (get) Token: 0x060058DB RID: 22747 RVA: 0x000471D1 File Offset: 0x000453D1
	public float elapsedTime
	{
		get
		{
			return this._elapsedTime;
		}
	}

	// Token: 0x060058DC RID: 22748 RVA: 0x001F8900 File Offset: 0x001F6B00
	public void OnFixedUpdate()
	{
		if (this.closed)
		{
			return;
		}
		if (!this._faceCreature)
		{
			this._faceCreature = true;
			this.OnEnterRoom();
			return;
		}
		if (this.targetCreature.script.HasUniqueProcessWorkNarration())
		{
			this.targetCreature.script.UniqueProcessWorkNarration(this);
		}
		else
		{
			this.ProcessWorkNarration();
		}
		this.targetCreature.script.OnFixedUpdateInSkill(this);
		this.CheckLive();
		if (this.closed)
		{
			return;
		}
		this.ProgressWork();
		this.CheckLive();
		if (this.closed)
		{
			return;
		}
		if (this.skillTypeInfo.id != 5L && this.workCount >= this.maxCubeCount && !this._readyToFinish)
		{
			this.targetCreature.ShowNarrationText("finish", true, new string[]
			{
				this.agent.name
			});
			this.targetCreature.script.OnSkillGoalComplete(this);
			this._readyToFinish = true;
			return;
		}
		if (this.workPlaying && this._readyToFinish)
		{
			this.FinishWorkSuccessfully();
		}
		if (this.workPlaying)
		{
			this._elapsedTime += Time.deltaTime;
			this.workProgress += Time.deltaTime * this.workSpeed;
		}
	}

	// Token: 0x060058DD RID: 22749 RVA: 0x001F8A60 File Offset: 0x001F6C60
	private void OnEnterRoom()
	{
		this.targetCreature.ShowNarrationText("move", new string[]
		{
			this.agent.name
		});
		this.targetCreature.ShowNarrationText("start", true, new string[]
		{
			this.agent.name
		});
		if (this.agent.HasUnitBuf(UnitBufType.BARRIER_R))
		{
			this.agent.RemoveUnitBuf(this.agent.GetUnitBufByType(UnitBufType.BARRIER_R));
		}
		if (this.agent.HasUnitBuf(UnitBufType.BARRIER_W))
		{
			this.agent.RemoveUnitBuf(this.agent.GetUnitBufByType(UnitBufType.BARRIER_W));
		}
		if (this.agent.HasUnitBuf(UnitBufType.BARRIER_B))
		{
			this.agent.RemoveUnitBuf(this.agent.GetUnitBufByType(UnitBufType.BARRIER_B));
		}
		if (this.agent.HasUnitBuf(UnitBufType.BARRIER_P))
		{
			this.agent.RemoveUnitBuf(this.agent.GetUnitBufByType(UnitBufType.BARRIER_P));
		}
		if (this.agent.HasUnitBuf(UnitBufType.BARRIER_ALL))
		{
			this.agent.RemoveUnitBuf(this.agent.GetUnitBufByType(UnitBufType.BARRIER_ALL));
		}
		Notice.instance.Send(NoticeName.OnWorkStart, new object[]
		{
			this.targetCreature
		});
		this.targetCreatureView.PlaySound("enter");
		if (this.targetCreature.isOverloaded)
		{
			Notice.instance.Send(NoticeName.WorkToOverloaded, new object[]
			{
				this.targetCreature
			});
			this._isOverloadedCreature = true;
			this.targetCreature.CancelOverload();
		}
		else if (this.targetCreature.metaInfo.creatureWorkType == CreatureWorkType.NORMAL)
		{
			CreatureOverloadManager.instance.AddOverloadGague();
		}
		this.targetCreature.AddWorkCount();
		if (this.skillTypeInfo.id == 5L)
		{
			this.targetCreature.Unit.room.OnEnterRoom(this.agent, this);
			this.targetCreature.script.OnEnterRoom(this);
			if (this.agent.Equipment.kitCreature != null)
			{
				this.agent.Equipment.kitCreature.script.kitEvent.OnEnterRoom(this.agent, this);
			}
			if (this.targetCreature.metaInfo.creatureKitType == CreatureKitType.EQUIP)
			{
				if (this.targetCreature.kitEquipOwner != null)
				{
					this.CancelWork();
					return;
				}
				if (this.agent.Equipment.kitCreature != null)
				{
					this.CancelWork();
					return;
				}
				SoundEffectPlayer.PlayOnce("Bullet/Kit_Equip", this.targetCreature.GetCurrentViewPosition());
				return;
			}
			else if (this.targetCreature.metaInfo.creatureKitType == CreatureKitType.ONESHOT)
			{
				SoundEffectPlayer.PlayOnce("Bullet/Bullet_Heal", this.targetCreature.GetCurrentViewPosition());
				return;
			}
		}
		else
		{
			AgentModel.GetWorkIconId(this.skillTypeInfo);
			this.agent.eventHandler.CallEvent(AgentEventEnum.OnEnterRoom, new object[]
			{
				this
			});
			this.targetCreature.Unit.room.OnEnterRoom(this.agent, this);
			this.targetCreature.script.OnEnterRoom(this);
			this.agent.OnEnterRoom(this);
			if (this.agent.Equipment.kitCreature != null)
			{
				this.agent.Equipment.kitCreature.script.kitEvent.OnEnterRoom(this.agent, this);
			}
			if (this.closed)
			{
				this.room.OnExitRoom();
				return;
			}
			this.HorrorDamage();
			if (!this.closed && this.skillTypeInfo.id != 6L && this.skillTypeInfo.id != 7L)
			{
				this.agent.GetUnit().StartWork();
			}
			if (this.skillTypeInfo.id == 6L || this.skillTypeInfo.id == 7L)
			{
				this.room.StopWorkDesc();
			}
		}
	}

	// Token: 0x060058DE RID: 22750 RVA: 0x000471D9 File Offset: 0x000453D9
	private void HorrorDamage()
	{
		if (this.skillTypeInfo.id != 5L)
		{
			this.agent.HorrorDamage(this.targetCreature);
		}
	}

	// Token: 0x060058DF RID: 22751 RVA: 0x001F8E28 File Offset: 0x001F7028
	private void ProgressWork()
	{
		if (this.skillTypeInfo.id == 5L)
		{
			this.ProcessKitCreatureWork();
		}
		else
		{
			if (this.workCount >= this.maxCubeCount)
			{
				return;
			}
			if (this.workProgress >= this.tickInterval * (float)(this.workCount + 1))
			{
				this.workCount++;
				bool isSuccess = true;
				this.ProcessWorkTick(out isSuccess);
				this.room.ProgressBar.AddBar(isSuccess);
				Notice.instance.Send(NoticeName.OnProcessWorkTick, new object[]
				{
					this.targetCreature
				});
			}
		}
	}

	// Token: 0x060058E0 RID: 22752 RVA: 0x001F8EC8 File Offset: 0x001F70C8
	private void ProcessWorkNarration()
	{
		if (this.skillTypeInfo.id == 5L)
		{
			return;
		}
		if (!this.narrationPart1 && (float)this.workCount >= (float)this.maxCubeCount / 5f)
		{
			this.targetCreature.ShowNarrationText("mid1", true, new string[]
			{
				this.agent.name
			});
			this.narrationPart1 = true;
		}
		else if (!this.narrationPart2 && (float)this.workCount >= (float)(2 * this.maxCubeCount) / 5f)
		{
			this.targetCreature.ShowNarrationText("mid2", true, new string[]
			{
				this.agent.name
			});
			this.narrationPart2 = true;
		}
		else if (!this.narrationPart3 && (float)this.workCount >= (float)(3 * this.maxCubeCount) / 5f)
		{
			this.targetCreature.ShowNarrationText("mid3", true, new string[]
			{
				this.agent.name
			});
			this.narrationPart3 = true;
		}
		else if (!this.narrationPart4 && (float)this.workCount >= (float)(4 * this.maxCubeCount) / 5f)
		{
			this.targetCreature.ShowNarrationText("mid4", true, new string[]
			{
				this.agent.name
			});
			this.narrationPart4 = true;
		}
	}

	// Token: 0x060058E1 RID: 22753 RVA: 0x000471FE File Offset: 0x000453FE
	public void PauseWorking()
	{
		this.workPlaying = false;
	}

	// Token: 0x060058E2 RID: 22754 RVA: 0x00047207 File Offset: 0x00045407
	public void ResumeWorking()
	{
		this.workPlaying = true;
	}

	// Token: 0x060058E3 RID: 22755 RVA: 0x000471C1 File Offset: 0x000453C1
	public bool IsWorking()
	{
		return this.workPlaying;
	}

	// Token: 0x060058E4 RID: 22756 RVA: 0x00047210 File Offset: 0x00045410
	public CreatureFeelingState GetFeelingState(int successCount)
	{
		return this.targetCreature.metaInfo.feelingStateCubeBounds.CalculateFeelingState(successCount);
	}

	// Token: 0x060058E5 RID: 22757 RVA: 0x00047228 File Offset: 0x00045428
	public CreatureFeelingState GetCurrentFeelingState()
	{
		return this.GetFeelingState(this.successCount);
	}

	// Token: 0x060058E6 RID: 22758 RVA: 0x001F9044 File Offset: 0x001F7244
	private void CloseWork(bool success)
	{
		if (this.closed)
		{
			return;
		}
		this.closed = true;
		if (this.skillTypeInfo.id == 5L || this.skillTypeInfo.id == 6L || this.skillTypeInfo.id == 7L)
		{
			this.agentView.ChangeAnimatorDefault();
		}
		int num = this.successCount;
		if (this.skillTypeInfo.id != 5L)
		{
			this.targetCreature.AddCreatureSuccessCube(this.successCount);
		}
		this.targetCreature.script.OnWorkClosed(this, this.successCount);
		this.targetCreature.ClearProbReduction();
		CreatureFeelingState currentFeelingState = this.GetCurrentFeelingState();
		if (this.skillTypeInfo.id != 5L)
		{
			if (num > 0)
			{
				EnergyModel.instance.AddEnergy((float)num);
			}
			else if (num < 0)
			{
				EnergyModel.instance.SubEnergy((float)(-(float)num));
			}
			this.targetCreature.SetFeelingStateInWork(currentFeelingState);
		}
		if (!this._isOverloadedCreature)
		{
			this.targetCreature.AddProbReductionCounter();
		}
		Notice.instance.Send(NoticeName.OnReleaseWork, new object[]
		{
			this.targetCreature
		});
		this.agent.FinishManage();
		if (this.targetCreature.state == CreatureState.WORKING)
		{
			this.targetCreature.state = CreatureState.WAIT;
		}
		this.targetCreature.currentSkill = null;
		this.agent.currentSkill = null;
		this.agent.counterAttackEnabled = true;
		if (this.targetCreature.script.isWorkAllocated)
		{
			this.targetCreature.script.isWorkAllocated = false;
		}
		this.room.OnWorkEnd();
		if (this.animRemoveOnDestroy)
		{
			this.agent.animationMessageRecevied = null;
		}
		this.agent.eventHandler.CallEvent(AgentEventEnum.OnExitRoom, new object[]
		{
			this
		});
		this.targetCreature.script.OnReleaseWork(this);
		this.room.OnExitRoom();
		if (this.skillTypeInfo.id != 5L)
		{
			this.room.OnFeelingStateDisplayStart(currentFeelingState);
		}
		else
		{
			this.room.OnFeelingStateDisplayEnd();
		}
		this.room.OnDamageAnimArrived();
	}

	// Token: 0x060058E7 RID: 22759 RVA: 0x00047236 File Offset: 0x00045436
	public void CancelWork()
	{
		this.CloseWork(false);
	}

	// Token: 0x060058E8 RID: 22760 RVA: 0x001F927C File Offset: 0x001F747C
	private float CalculateDmgExp(float rate)
	{
		if (rate >= 0.9f)
		{
			return 0.4f;
		}
		if (rate >= 0.8f)
		{
			return 0.6f;
		}
		if (rate >= 0.7f)
		{
			return 0.8f;
		}
		if (rate > 0.2f)
		{
			return 1f;
		}
		if (rate > 0.1f)
		{
			return 1.3f;
		}
		return 1.5f;
	}

	// Token: 0x060058E9 RID: 22761 RVA: 0x001F92E4 File Offset: 0x001F74E4
	private float CalculateLevelExp(RwbpType rwbpType)
	{
		int num = 0;
		WorkerPrimaryStat addedStat = this.agent.primaryStat.GetAddedStat(this.agent.primaryStatExp);
		float num2 = 1f;
		switch (rwbpType)
		{
		case RwbpType.R:
			num = AgentModel.CalculateStatLevel(addedStat.hp);
			break;
		case RwbpType.W:
			num = AgentModel.CalculateStatLevel(addedStat.mental);
			break;
		case RwbpType.B:
			num = AgentModel.CalculateStatLevel(addedStat.work);
			break;
		case RwbpType.P:
			num = AgentModel.CalculateStatLevel(addedStat.battle);
			break;
		}
		int num3 = num - this.targetCreature.GetRiskLevel();
		switch (num3 + 3)
		{
		case 0:
			num2 = 1.4f;
			break;
		case 1:
			num2 = 1.2f;
			break;
		case 2:
		case 3:
			num2 = 1f;
			break;
		case 4:
			num2 = 0.8f;
			break;
		case 5:
			num2 = 0.6f;
			break;
		case 6:
			num2 = 0.4f;
			break;
		case 7:
			num2 = 0.2f;
			break;
		}
		float[] array = new float[]
		{
			0.6f,
			0.55f,
			0.5f,
			0.45f,
			0.4f
		};
		if (num <= 0 || num > array.Length)
		{
			return num2 * array[0];
		}
		num2 *= array[num - 1];
		if (rwbpType == RwbpType.P)
		{
			num2 /= 3f;
		}
		return num2;
	}

	// Token: 0x060058EA RID: 22762 RVA: 0x001F9448 File Offset: 0x001F7648
	private void FinishWorkSuccessfully()
	{
		this.agent.eventHandler.CallEvent(AgentEventEnum.OnFinishWork, new object[]
		{
			this
		});
		this.targetCreature.script.OnFinishWork(this);
		if (this.skillTypeInfo.id == 5L)
		{
			if (this.targetCreature.metaInfo.creatureKitType == CreatureKitType.EQUIP)
			{
				this.agent.SetKitCreature(this.targetCreature);
			}
			if (this.targetCreature.metaInfo.creatureKitType == CreatureKitType.EQUIP || this.targetCreature.metaInfo.creatureKitType == CreatureKitType.ONESHOT)
			{
				bool flag = false;
				int observeCost = this.targetCreature.observeInfo.GetObserveCost(CreatureModel.regionName[0]);
				if (this.targetCreature.observeInfo.totalKitUseCount >= observeCost)
				{
					flag = true;
				}
				this.targetCreature.observeInfo.totalKitUseCount++;
				if (!flag && this.targetCreature.observeInfo.totalKitUseCount >= observeCost)
				{
					this.targetCreature.OnObservationLevelChanged();
				}
				this.room.SetKitObserveLevel();
			}
		}
		else
		{
			CreatureEquipmentMakeInfo creatureEquipmentMakeInfo = this.targetCreature.metaInfo.equipMakeInfos.Find((CreatureEquipmentMakeInfo x) => x.equipTypeInfo.type == EquipmentTypeInfo.EquipmentType.SPECIAL);
			if (creatureEquipmentMakeInfo != null && this.targetCreature.GetObservationLevel() >= creatureEquipmentMakeInfo.level && creatureEquipmentMakeInfo.GetProb() >= UnityEngine.Random.value)
			{
				EGOgiftModel egogiftModel = EGOgiftModel.MakeGift(creatureEquipmentMakeInfo.equipTypeInfo);
				this.agent.AttachEGOgift(egogiftModel);
				Notice.instance.Send(NoticeName.OnGetEGOgift, new object[]
				{
					this.agent,
					egogiftModel
				});
			}
			float num = 1f;
			float num2 = ResearchDataModel.instance.GetPromotionEasilyValue();
			if (this.agent.currentSefiraEnum == SefiraEnum.HOD && this.agent.GetContinuousServiceLevel() <= 3)
			{
				num2 += (float)SefiraAbilityValueInfo.hodContinuousServiceValues[this.agent.GetContinuousServiceLevel() - 1] / 100f;
			}
			num2 += (float)SefiraAbilityValueInfo.hodOfficerAliveValues[SefiraManager.instance.GetOfficerAliveLevel(SefiraEnum.HOD)] / 100f;
			long id = this.skillTypeInfo.id;
			if (id >= 1L && id <= 4L)
			{
				switch ((int)(id - 1L))
				{
				case 0:
					num *= this.CalculateDmgExp(this.agent.hp / this.startAgentHp);
					this.agent.primaryStatExp.hp += (float)this.successCount * this.CalculateLevelExp(RwbpType.R) * num * num2;
					break;
				case 1:
					num *= this.CalculateDmgExp(this.agent.mental / this.startAgentMental);
					this.agent.primaryStatExp.mental += (float)this.successCount * this.CalculateLevelExp(RwbpType.W) * num * num2;
					break;
				case 2:
					num *= this.CalculateDmgExp((this.agent.hp / this.startAgentHp + this.agent.mental / this.startAgentMental) / 2f);
					this.agent.primaryStatExp.work += (float)this.successCount * this.CalculateLevelExp(RwbpType.B) * num * num2;
					break;
				case 3:
					num *= 1.5f;
					this.agent.primaryStatExp.battle += (float)this.successCount * this.CalculateLevelExp(RwbpType.P) * num * num2;
					break;
				}
			}
		}
		this.CloseWork(true);
	}

	// Token: 0x060058EB RID: 22763 RVA: 0x001F97CC File Offset: 0x001F79CC
	private void ProcessKitCreatureWork()
	{
		CreatureKitType creatureKitType = this.targetCreature.metaInfo.creatureKitType;
		if (creatureKitType == CreatureKitType.EQUIP)
		{
			if (this._elapsedTime > this.targetCreature.script.GetKitCreatureProcessTime())
			{
				this._readyToFinish = true;
			}
		}
		else if (creatureKitType == CreatureKitType.ONESHOT)
		{
			if (this._elapsedTime > this.targetCreature.script.GetKitCreatureProcessTime())
			{
				this._readyToFinish = true;
			}
		}
		else if (creatureKitType == CreatureKitType.CHANNEL)
		{
			this.targetCreature.OnFixedUpdateInKitEquip(this.agent);
		}
	}

	// Token: 0x060058EC RID: 22764 RVA: 0x001F9860 File Offset: 0x001F7A60
	private void ProcessWorkTick(out bool isSuccess)
	{
		isSuccess = true;
		bool flag = false;
		float num = this.targetCreature.GetWorkSuccessProb(this.agent, this.skillTypeInfo);
		num += (float)this.targetCreature.GetObserveBonusProb() / 100f;
		num += (float)this.targetCreature.script.OnBonusWorkProb() / 100f;
		num += (float)this.agent.workProb / 500f;
		num += this.agent.Equipment.GetWorkProbSpecialBonus(this.agent, this.skillTypeInfo) / 500f;
		if (this.agent.GetUnitBufList().Count > 0)
		{
			foreach (UnitBuf unitBuf in this.agent.GetUnitBufList())
			{
				num += unitBuf.GetWorkProbSpecialBonus(this.agent, this.skillTypeInfo) / 100f;
			}
		}
		num = this.targetCreature.script.TranformWorkProb(num);
		if (num > 0.95f)
		{
			num = 0.95f;
		}
		float num2 = (float)this.targetCreature.GetRedusedWorkProbByCounter() / 100f;
		float num3 = (float)this.targetCreature.ProbReductionValue / 100f;
		if (num3 > 0f)
		{
			num -= num3;
		}
		else
		{
			num -= num2;
		}
		if (this.targetCreature.sefira.agentDeadPenaltyActivated)
		{
			num -= 0.5f;
		}
		bool flag2 = UnityEngine.Random.value < num;
		if (this.targetCreature.script.ForcelyFail(this))
		{
			flag2 = false;
		}
		else if (this.targetCreature.script.ForcelySuccess(this))
		{
			flag2 = true;
		}
		if (this.forceSuccess)
		{
			flag2 = true;
		}
		isSuccess = flag2;
		if (flag2)
		{
			this.successCount++;
			this.targetCreature.script.OnSkillSuccessWorkTick(this);
			this.agent.history.AddWorkCubeCount(this.skillTypeInfo.rwbpType, 1);
		}
		else
		{
			this.failCount++;
			this.targetCreature.script.OnSkillFailWorkTick(this);
			if (this.workPlaying && this.targetCreature.script.isAttackInWorkProcess())
			{
				DamageInfo damageInfo = this.targetCreature.metaInfo.workDamage.Copy() * this.targetCreature.script.GetDamageMultiplierInWork(this);
				this.agent.TakeDamage(this.targetCreature, damageInfo);
				this.InvokeEffect(this.agent, damageInfo);
				this.targetCreatureView.room.OnDamageInvoked(damageInfo);
				this.targetCreature.script.OnAttackInWorkProcess(this);
			}
		}
		if (flag)
		{
			Notice.instance.Send("UpdateAgentState_" + this.agent.instanceId, new object[0]);
		}
	}

	// Token: 0x060058ED RID: 22765 RVA: 0x001F9B40 File Offset: 0x001F7D40
	private void InvokeEffect(UnitModel target, DamageInfo damageInfo)
	{
		RwbpType type = damageInfo.type;
		DefenseInfo defense = target.defense;
		UnitDirection dir = UnitDirection.LEFT;
		DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(target, type, defense, dir);
	}

	// Token: 0x060058EE RID: 22766 RVA: 0x001F9B68 File Offset: 0x001F7D68
	public void CheckLive()
	{
		if (this.closed)
		{
			return;
		}
		if (this.agent.mental <= 0f)
		{
			string text = this.targetCreature.ShowNarrationText("panic", true, new string[]
			{
				this.agent.name
			});
			if (this.agent.IsPanic())
			{
				this.agentView.showSpeech.ShowSpeech_old(AgentLyrics.instance.GetLyricByType(LyricType.MENTALBAD));
			}
			this.CancelWork();
			if (!this.targetCreature.script.OnWorkerPanic(this.agent))
			{
				this.agent.PanicByCreature(this.targetCreature, this.skillTypeInfo);
			}
			this.agent.history.AddWorkFail();
			return;
		}
		if (this.agent.IsDead())
		{
			this.CancelWork();
		}
	}

	// Token: 0x060058EF RID: 22767 RVA: 0x0004723F File Offset: 0x0004543F
	public bool IsFinished()
	{
		return this.closed;
	}

	// Token: 0x060058F0 RID: 22768 RVA: 0x00047247 File Offset: 0x00045447
	public int GetSuccessCubeCount()
	{
		return this.successCount;
	}

	// Token: 0x060058F1 RID: 22769 RVA: 0x0004724F File Offset: 0x0004544F
	public int GetFailCubeCount()
	{
		return this.failCount;
	}

	// Token: 0x060058F2 RID: 22770 RVA: 0x00047257 File Offset: 0x00045457
	public int GetMaxCubCount()
	{
		return this.maxCubeCount;
	}

	// Token: 0x060058F3 RID: 22771 RVA: 0x001F9C48 File Offset: 0x001F7E48
	public static UseSkill InitUseSkillAction(SkillTypeInfo skillInfo, AgentModel agent, CreatureModel creature)
	{
		UseSkill useSkill = new UseSkill();
		AgentUnit unit = agent.GetUnit();
		CreatureUnit unit2 = creature.Unit;
		useSkill.room = unit2.room;
		useSkill.agent = agent;
		useSkill.agentView = unit;
		useSkill.targetCreature = creature;
		useSkill.targetCreatureView = unit2;
		if (skillInfo.id != 5L)
		{
			useSkill.workCount = 0;
			useSkill.maxCubeCount = creature.metaInfo.feelingStateCubeBounds.GetLastBound();
			useSkill.workSpeed = creature.GetCubeSpeed() * (1f + (float)(creature.GetObserveBonusSpeed() + agent.workSpeed) / 100f);
			if (skillInfo.id == 6L)
			{
				unit.ChangeAnimatorForcely("MeatIdolUse", false, true);
			}
		}
		else if (creature.metaInfo.workAnim != string.Empty)
		{
			unit.ChangeAnimatorForcely(creature.metaInfo.workAnim, creature.metaInfo.workAnimFace == "unique", false);
		}
		useSkill.startAgentHp = agent.hp;
		useSkill.startAgentMental = agent.mental;
		useSkill.skillTypeInfo = skillInfo;
		creature.state = CreatureState.WORKING;
		creature.currentSkill = useSkill;
		useSkill.agent.animationMessageRecevied = useSkill.targetCreature.script;
		useSkill.room.ProgressBar.SetVisible(true);
		useSkill.room.ProgressBar.InitializeState();
		float num = useSkill.targetCreatureView.transform.localPosition.y;
		num = 1f + num;
		useSkill.agent.currentSkill = useSkill;
		return useSkill;
	}

	// Token: 0x060058F4 RID: 22772 RVA: 0x0004725F File Offset: 0x0004545F
	public void OnWorkEndAnimPlayed()
	{
		this.targetCreature.script.OnAgentWorkEndAnimationPlayed(this);
	}

	// Token: 0x060058F5 RID: 22773 RVA: 0x00047272 File Offset: 0x00045472
	public void SetForceSuccess()
	{
		this.forceSuccess = true;
	}

	// Token: 0x040051D6 RID: 20950
	private const int KIT_WORK_ID = 5;

	// Token: 0x040051D7 RID: 20951
	private const int CONFESS_ID = 6;

	// Token: 0x040051D8 RID: 20952
	private const int IMPROVISE_ID = 7;

	// Token: 0x040051D9 RID: 20953
	public int maxCubeCount;

	// Token: 0x040051DA RID: 20954
	public float tickInterval = 1f;

	// Token: 0x040051DB RID: 20955
	public float workSpeed;

	// Token: 0x040051DC RID: 20956
	public float workProgress;

	// Token: 0x040051DD RID: 20957
	public int successCount;

	// Token: 0x040051DE RID: 20958
	public int failCount;

	// Token: 0x040051DF RID: 20959
	public int workCount;

	// Token: 0x040051E0 RID: 20960
	public SkillTypeInfo skillTypeInfo;

	// Token: 0x040051E1 RID: 20961
	public AgentModel agent;

	// Token: 0x040051E2 RID: 20962
	public AgentUnit agentView;

	// Token: 0x040051E3 RID: 20963
	public CreatureModel targetCreature;

	// Token: 0x040051E4 RID: 20964
	public CreatureUnit targetCreatureView;

	// Token: 0x040051E5 RID: 20965
	public IsolateRoom room;

	// Token: 0x040051E6 RID: 20966
	private bool workPlaying = true;

	// Token: 0x040051E7 RID: 20967
	private bool _readyToFinish;

	// Token: 0x040051E8 RID: 20968
	public bool narrationPart1;

	// Token: 0x040051E9 RID: 20969
	public bool narrationPart2;

	// Token: 0x040051EA RID: 20970
	public bool narrationPart3;

	// Token: 0x040051EB RID: 20971
	public bool narrationPart4;

	// Token: 0x040051EC RID: 20972
	private bool closed;

	// Token: 0x040051ED RID: 20973
	private bool forceSuccess;

	// Token: 0x040051EE RID: 20974
	private bool _faceCreature;

	// Token: 0x040051EF RID: 20975
	private bool _isOverloadedCreature;

	// Token: 0x040051F0 RID: 20976
	private float _elapsedTime;

	// Token: 0x040051F1 RID: 20977
	public bool animRemoveOnDestroy = true;

	// Token: 0x040051F2 RID: 20978
	public float startAgentHp;

	// Token: 0x040051F3 RID: 20979
	public float startAgentMental;
}
