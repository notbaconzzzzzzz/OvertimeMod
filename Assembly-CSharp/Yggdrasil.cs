using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WorkerSpine;

// Token: 0x020004A6 RID: 1190
public class Yggdrasil : CreatureBase, IObserver
{
	// Token: 0x06002B6F RID: 11119 RVA: 0x00129550 File Offset: 0x00127750
	public Yggdrasil()
	{
	}

	// Token: 0x17000408 RID: 1032
	// (get) Token: 0x06002B70 RID: 11120 RVA: 0x0002A888 File Offset: 0x00028A88
	public static DamageInfo PassageDmg
	{
		get
		{
			return new DamageInfo(RwbpType.W, 9, 12);
		}
	}

	// Token: 0x17000409 RID: 1033
	// (get) Token: 0x06002B71 RID: 11121 RVA: 0x00024F43 File Offset: 0x00023143
	private static float DEFAULT_SOUND_FREQ
	{
		get
		{
			return UnityEngine.Random.Range(5f, 10f);
		}
	}

	// Token: 0x1700040A RID: 1034
	// (get) Token: 0x06002B72 RID: 11122 RVA: 0x001295BC File Offset: 0x001277BC
	private bool ChildAlive
	{
		get
		{
			bool flag = false;
			List<ChildCreatureModel> list = new List<ChildCreatureModel>(this.model.GetAliveChilds());
			List<WorkerModel> list2 = new List<WorkerModel>(WorkerManager.instance.GetWorkerList());
			foreach (WorkerModel workerModel in list2)
			{
				if (workerModel.HasUnitBuf(UnitBufType.YGGDRASIL_SUICIDE))
				{
					flag = true;
					break;
				}
			}
			if (!flag && list.Count > 0)
			{
				foreach (ChildCreatureModel childCreatureModel in list)
				{
					if (childCreatureModel.state == CreatureState.ESCAPE)
					{
						flag = true;
						break;
					}
				}
			}
			return flag;
		}
	}

	// Token: 0x1700040B RID: 1035
	// (get) Token: 0x06002B73 RID: 11123 RVA: 0x0002A894 File Offset: 0x00028A94
	private int BlessedCnt
	{
		get
		{
			return this.blessedList.Count;
		}
	}

	// Token: 0x1700040C RID: 1036
	// (get) Token: 0x06002B74 RID: 11124 RVA: 0x0002A8A1 File Offset: 0x00028AA1
	public YggdrasilAnim animScript
	{
		get
		{
			if (this._animScript == null)
			{
				this._animScript = (base.Unit.animTarget as YggdrasilAnim);
			}
			return this._animScript;
		}
	}

	// Token: 0x06002B75 RID: 11125 RVA: 0x0002A8D0 File Offset: 0x00028AD0
	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
		this.animScript.SetScript(this);
		this.ParamInit();
	}

	// Token: 0x06002B76 RID: 11126 RVA: 0x001296AC File Offset: 0x001278AC
	public override void ParamInit()
	{
		base.ParamInit();
		this.ResetQliphothCounter();
		this.animScript.Init();
		foreach (Yggdrasil.InfestedPassage infestedPassage in this.infestedPassages.Values)
		{
			infestedPassage.Destroy();
		}
		this.infestedPassages = new Dictionary<PassageObjectModel, Yggdrasil.InfestedPassage>();
		this.blessedList = new List<WorkerModel>();
		this.blessTarget = null;
		this.isInSkill = false;
		this.gonnaSubQliphoth = false;
		this.ResetWorkCount();
		this.SetFilters(0f);
		this.filterIncreaseTimer.StopTimer();
		this.filterDecreaseTimer.StopTimer();
		this.defaultSoundTimer.StopTimer();
		this.RemoveLoopSound();
	}

	// Token: 0x06002B77 RID: 11127 RVA: 0x0002A8EB File Offset: 0x00028AEB
	public override void OnStageStart()
	{
		base.OnStageStart();
		this.ParamInit();
		this.defaultSoundTimer.StartTimer(Yggdrasil.DEFAULT_SOUND_FREQ);
		Notice.instance.Observe(NoticeName.OnReleaseWork, this);
		Notice.instance.Observe(NoticeName.OnEscape, this);
	}

	// Token: 0x06002B78 RID: 11128 RVA: 0x0002A929 File Offset: 0x00028B29
	public override void OnStageRelease()
	{
		base.OnStageRelease();
		this.ParamInit();
		Notice.instance.Remove(NoticeName.OnReleaseWork, this);
		Notice.instance.Remove(NoticeName.OnEscape, this);
	}

	// Token: 0x06002B79 RID: 11129 RVA: 0x00129788 File Offset: 0x00127988
	public override void ActivateQliphothCounter()
	{
		base.ActivateQliphothCounter();
		this.gonnaSubQliphoth = false;
		this.ResetWorkCount();
		if (!this.isInSkill)
		{
			if (!this.ChildAlive && !this.IsTransformed())
			{
				this.ActivateSkill();
			}
			else
			{
				this.Reset();
			}
		}
	}

	// Token: 0x06002B7A RID: 11130 RVA: 0x001297DC File Offset: 0x001279DC
	public override void OnFixedUpdate(CreatureModel creature)
	{
		base.OnFixedUpdate(creature);
		if (GameManager.currentGameManager.state != GameState.PLAYING)
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		foreach (Yggdrasil.InfestedPassage infestedPassage in this.infestedPassages.Values)
		{
			infestedPassage.Process();
		}
		if (this.filterIncreaseTimer.started)
		{
			this.AddFilters(deltaTime * 1.2f);
			if (this.filterIncreaseTimer.RunTimer() && this.blessTarget == null)
			{
				this.filterDecreaseTimer.StartTimer(1f);
			}
		}
		else if (this.filterDecreaseTimer.started)
		{
			this.ReduceFilters(deltaTime * 1.2f);
			if (this.filterDecreaseTimer.RunTimer() && this.isInSkill)
			{
				this.filterIncreaseTimer.StartTimer(1f);
			}
		}
		if (this.defaultSoundTimer.RunTimer())
		{
			this.defaultSoundTimer.StartTimer(Yggdrasil.DEFAULT_SOUND_FREQ);
			this.MakeSound(Yggdrasil.SoundKey.DEFAULT, base.Unit.gameObject.transform.position, 1f);
		}
	}

	// Token: 0x06002B7B RID: 11131 RVA: 0x0002A957 File Offset: 0x00028B57
	public override void OnEnterRoom(UseSkill skill)
	{
		base.OnEnterRoom(skill);
		this.ResetWorkCount();
	}

	// Token: 0x06002B7C RID: 11132 RVA: 0x0002A966 File Offset: 0x00028B66
	public override void OnFinishWork(UseSkill skill)
	{
		base.OnFinishWork(skill);
		if (skill.skillTypeInfo.id == (long)this.TARGET_SKILL_ID)
		{
			this.gonnaSubQliphoth = true;
		}
	}

	// Token: 0x06002B7D RID: 11133 RVA: 0x0002A98D File Offset: 0x00028B8D
	public override void OnWorkCoolTimeEnd(CreatureFeelingState oldState)
	{
		base.OnWorkCoolTimeEnd(oldState);
		if (this.gonnaSubQliphoth)
		{
			this.SubQliphothCounter();
		}
	}

	// Token: 0x06002B7E RID: 11134 RVA: 0x00129934 File Offset: 0x00127B34
	private void ActivateSkill()
	{
		if (this.isInSkill)
		{
			return;
		}
		this.isInSkill = true;
		List<WorkerModel> list = new List<WorkerModel>();
		list = this.GetAttractTargets().ToList<WorkerModel>();
		if (list.Count <= 0)
		{
			this.Reset();
			return;
		}
		WorkerModel workerModel = list[UnityEngine.Random.Range(0, list.Count)];
		if (workerModel == null)
		{
			this.Reset();
			return;
		}
		if (this.model.IsWorkingState())
		{
			this.model.currentSkill.CancelWork();
		}
		this.Attract(workerModel);
	}

	// Token: 0x06002B7F RID: 11135 RVA: 0x001299C4 File Offset: 0x00127BC4
	private void Attract(WorkerModel worker)
	{
		this.blessTarget = null;
		this.filterDecreaseTimer.StopTimer();
		this.filterIncreaseTimer.StartTimer(1f);
		if (this.model.currentSkill != null)
		{
			this.model.currentSkill.CancelWork();
		}
		worker.StopAction();
		worker.LoseControl();
		worker.SetUncontrollableAction(new Uncontrollable_Yggdrasil(worker, this));
	}

	// Token: 0x06002B80 RID: 11136 RVA: 0x00129A2C File Offset: 0x00127C2C
	public void CancelAttract(WorkerModel worker)
	{
		worker.ClearUnconCommand();
		worker.StopAction();
		worker.GetControl();
		AgentModel agentModel = worker as AgentModel;
		if (agentModel != null)
		{
			agentModel.GetUnit().OnUnselect();
		}
		if (this.blessTarget == null)
		{
			this.Reset();
		}
	}

	// Token: 0x06002B81 RID: 11137 RVA: 0x0002A9A7 File Offset: 0x00028BA7
	private void Reset()
	{
		this.isInSkill = false;
		this.filterIncreaseTimer.StopTimer();
		this.filterDecreaseTimer.StopTimer();
		this.ResetWorkCount();
		this.ResetQliphothCounter();
	}

	// Token: 0x06002B82 RID: 11138 RVA: 0x00129A74 File Offset: 0x00127C74
	private bool CheckAttractCondition(WorkerModel worker)
	{
		return worker.unconAction == null && !worker.CannotControll() && (!(worker is AgentModel) || (worker as AgentModel).currentSkill == null) && !worker.invincible && !worker.HasUnitBuf(UnitBufType.DEATH_ANGEL_BETRAYER) && this.CheckBlessCondition(worker);
	}

	// Token: 0x06002B83 RID: 11139 RVA: 0x0002A9D2 File Offset: 0x00028BD2
	private bool CheckBlessCondition(WorkerModel worker)
	{
		return !worker.IsDead() && !worker.IsPanic() && !worker.HasUnitBuf(UnitBufType.YGGDRASIL_BLESS);
	}

	// Token: 0x06002B84 RID: 11140 RVA: 0x00129AE4 File Offset: 0x00127CE4
	private WorkerModel[] GetAttractTargets()
	{
		List<WorkerModel> list = new List<WorkerModel>();
		Sefira sefiraOrigin = this.model.sefiraOrigin;
		WorkerModel[] attractTargets = this.GetAttractTargets(sefiraOrigin);
		if (attractTargets.Length <= 0)
		{
			foreach (Sefira sefira in SefiraManager.instance.GetOpendSefiraList())
			{
				WorkerModel[] attractTargets2 = this.GetAttractTargets(sefira);
				if (attractTargets2.Length > 0)
				{
					list.AddRange(attractTargets2);
				}
			}
		}
		else
		{
			list.AddRange(attractTargets);
		}
		return list.ToArray();
	}

	// Token: 0x06002B85 RID: 11141 RVA: 0x00129B6C File Offset: 0x00127D6C
	private WorkerModel[] GetAttractTargets(Sefira sefira)
	{
		List<WorkerModel> list = new List<WorkerModel>();
		foreach (AgentModel agentModel in sefira.agentList)
		{
			if (this.CheckAttractCondition(agentModel))
			{
				list.Add(agentModel);
			}
		}
		foreach (OfficerModel officerModel in sefira.officerList)
		{
			if (this.CheckAttractCondition(officerModel))
			{
				list.Add(officerModel);
			}
		}
		return list.ToArray();
	}

	// Token: 0x06002B86 RID: 11142 RVA: 0x00129C38 File Offset: 0x00127E38
	public void OnArrive(WorkerModel arrived)
	{
		this.filterDecreaseTimer.StopTimer();
		this.filterIncreaseTimer.StartTimer(1f * (1f - this.animScript.GetCurrentAlpha()));
		if (this.blessTarget != null)
		{
			this.Reset();
			return;
		}
		this.Bless(arrived);
	}

	// Token: 0x06002B87 RID: 11143 RVA: 0x00129C8C File Offset: 0x00127E8C
	private void Bless(WorkerModel worker)
	{
		this.blessTarget = worker;
		worker.GetControl();
		if (this.model.currentSkill == null)
		{
			worker.LoseControl();
		}
		this.loopSound = this.MakeSoundLoop(Yggdrasil.SoundKey.BLESS, base.Unit.gameObject.transform, 1f);
		this.MakeSound(Yggdrasil.SoundKey.BLESS_AGENT, worker.GetWorkerUnit().gameObject.transform.position, 1f);
		worker.GetWorkerUnit().animChanger.ChangeAnimatorWithUniqueFace(WorkerSpine.AnimatorName.YggdrasilAgentCTRL, false);
		worker.GetWorkerUnit().animEventHandler.SetAnimEvent(new AnimatorEventHandler.AnimEventDelegate(this.OnWorkerAnimCalled));
		this.BlessStart();
	}

	// Token: 0x06002B88 RID: 11144 RVA: 0x00129D40 File Offset: 0x00127F40
	public void RemoveBlessedWorker(WorkerModel worker)
	{
		if (worker != null)
		{
			if (this.blessedList.Contains(worker))
			{
				this.ReduceFlower();
				this.blessedList.Remove(worker);
			}
			if (this.blessedList.Count <= 0 && !this.ChildAlive && this.IsTransformed())
			{
				this.BlessOverloadEnd();
			}
		}
	}

	// Token: 0x06002B89 RID: 11145 RVA: 0x00129DA4 File Offset: 0x00127FA4
	public override void OnSkillGoalComplete(UseSkill skill)
	{
		base.OnSkillGoalComplete(skill);
		WorkerModel agent = skill.agent;
		if (this.CheckBlessCondition(agent))
		{
			skill.PauseWorking();
			this.model.Unit.room.StopWorkDesc();
			this.filterIncreaseTimer.StartTimer(1f);
			this.Bless(skill.agent);
		}
	}

	// Token: 0x06002B8A RID: 11146 RVA: 0x0002A9FE File Offset: 0x00028BFE
	public void AddBlessedList(WorkerModel target)
	{
		this.AddFlower();
		this.blessedList.Add(target);
	}

	// Token: 0x06002B8B RID: 11147 RVA: 0x00129E04 File Offset: 0x00128004
	public void OnWorkerAnimCalled(int i)
	{
		if (i == 1000620)
		{
			if (this.model.currentSkill != null)
			{
				this.model.currentSkill.ResumeWorking();
			}
			if (this.blessTarget != null && !this.blessTarget.IsDead())
			{
				this.AddBlessedList(this.blessTarget);
				YggdrasilBlessBuf buf = new YggdrasilBlessBuf(this.blessTarget, this);
				this.blessTarget.AddUnitBuf(buf);
				this.blessTarget.GetWorkerUnit().AddUnitBuf(buf, this.animScript.blessObject, true);
				this.blessTarget.GetControl();
				this.blessTarget.GetWorkerUnit().animChanger.ChangeAnimator();
				if (this.BlessedCnt >= this.BLESS_OVERLOAD_CNT)
				{
					this.BlessOverload();
				}
			}
			this.RemoveLoopSound();
			this.Reset();
		}
	}

	// Token: 0x06002B8C RID: 11148 RVA: 0x00129EE0 File Offset: 0x001280E0
	private void BlessOverload()
	{
		this.SetTransform(true);
		this.defaultSoundTimer.StopTimer();
		this.loopSound = this.MakeSoundLoop(Yggdrasil.SoundKey.DEVIL_DEFAULT, base.Unit.gameObject.transform, 1f);
		foreach (WorkerModel workerModel in this.blessedList)
		{
			if (!workerModel.IsDead())
			{
				if (!workerModel.CannotControll())
				{
					if (workerModel.unconAction == null)
					{
						if (!workerModel.invincible)
						{
							if (!workerModel.HasUnitBuf(UnitBufType.DEATH_ANGEL_BETRAYER))
							{
								workerModel.GetMovableNode().StopMoving();
								workerModel.StopAction();
								workerModel.LoseControl();
								workerModel.SetUncontrollableAction(new Uncontrollable_Yggdrasil_Fake_PanicReady(workerModel, this));
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06002B8D RID: 11149 RVA: 0x0002AA12 File Offset: 0x00028C12
	private void SetTransform(bool isTransform)
	{
		this.animScript.SetTransform(isTransform);
		this.InitFlower();
	}

	// Token: 0x06002B8E RID: 11150 RVA: 0x0002AA26 File Offset: 0x00028C26
	private void BlessStart()
	{
		this.animScript.OnBless();
	}

	// Token: 0x06002B8F RID: 11151 RVA: 0x0002AA33 File Offset: 0x00028C33
	private void BlessOverloadEnd()
	{
		this.SetTransform(false);
		this.RemoveLoopSound();
		this.defaultSoundTimer.StartTimer(Yggdrasil.DEFAULT_SOUND_FREQ);
		this.ParamInit();
	}

	// Token: 0x06002B90 RID: 11152 RVA: 0x0002AA58 File Offset: 0x00028C58
	private void AddWorkCount()
	{
		this.workOtherCnt++;
		this.AddFilters(1f / (float)this.SUB_QLIPHOTH_WORK_OHTER_CNT);
	}

	// Token: 0x06002B91 RID: 11153 RVA: 0x0002AA7B File Offset: 0x00028C7B
	private void ResetWorkCount()
	{
		this.workOtherCnt = 0;
		this.filterIncreaseTimer.StopTimer();
		this.filterDecreaseTimer.StartTimer(1f);
	}

	// Token: 0x06002B92 RID: 11154 RVA: 0x00129FE4 File Offset: 0x001281E4
	public void OnNotice(string notice, params object[] param)
	{
		if (notice == NoticeName.OnReleaseWork)
		{
			CreatureModel creatureModel = param[0] as CreatureModel;
			if (creatureModel != null)
			{
				if (creatureModel.script is Yggdrasil)
				{
					return;
				}
				if (this.model.IsWorkingState())
				{
					return;
				}
				if (!this.isInSkill && !this.ChildAlive && !this.IsTransformed())
				{
					this.AddWorkCount();
					if (this.workOtherCnt >= this.SUB_QLIPHOTH_WORK_OHTER_CNT)
					{
						this.ResetWorkCount();
						this.SubQliphothCounter();
					}
				}
			}
		}
		else if (notice == NoticeName.OnEscape)
		{
			CreatureModel creatureModel2 = param[0] as CreatureModel;
			if (creatureModel2 != null)
			{
				if (creatureModel2 is ChildCreatureModel)
				{
					return;
				}
				if (creatureModel2 is OrdealCreatureModel)
				{
					return;
				}
				if (creatureModel2 is EventCreatureModel)
				{
					return;
				}
				if (creatureModel2.script is SmallBird)
				{
					return;
				}
				if (creatureModel2.script is BossBird)
				{
					return;
				}
				if (creatureModel2.script is GeburahCoreScript)
				{
					return;
				}
				if (creatureModel2.script is BinahCoreScript)
				{
					return;
				}
				if (!this.isInSkill && !this.ChildAlive && !this.IsTransformed())
				{
					float value = UnityEngine.Random.value;
					if (value <= this.SUB_QLIPHOTH_ESCAPE_PROB)
					{
						this.SubQliphothCounter();
					}
				}
			}
		}
	}

	// Token: 0x06002B93 RID: 11155 RVA: 0x00014081 File Offset: 0x00012281
	public override bool HasRoomCounter()
	{
		return true;
	}

	// Token: 0x06002B94 RID: 11156 RVA: 0x0002AA9F File Offset: 0x00028C9F
	public override bool OnOpenWorkWindow()
	{
		return base.OnOpenWorkWindow() && !this.isInSkill && !this.ChildAlive && !this.IsTransformed();
	}

	// Token: 0x06002B95 RID: 11157 RVA: 0x0002AACE File Offset: 0x00028CCE
	public override bool IsWorkable()
	{
		return base.IsWorkable() && !this.isInSkill && !this.ChildAlive && !this.IsTransformed();
	}

	// Token: 0x06002B96 RID: 11158 RVA: 0x00029400 File Offset: 0x00027600
	private void SubQliphothCounter()
	{
		if (this.model.qliphothCounter > 0)
		{
			this.model.SubQliphothCounter();
		}
	}

	// Token: 0x06002B97 RID: 11159 RVA: 0x0012A13C File Offset: 0x0012833C
	public override void OnChildSuppressed(ChildCreatureModel child)
	{
		base.OnChildSuppressed(child);
		PassageObjectModel currentPassage = child.GetMovableNode().currentPassage;
		Yggdrasil.InfestedPassage infestedPassage = null;
		if (this.infestedPassages.TryGetValue(currentPassage, out infestedPassage) && !infestedPassage.IsAvailable())
		{
			infestedPassage.Destroy();
			this.infestedPassages.Remove(currentPassage);
		}
		if (!this.ChildAlive && this.IsTransformed())
		{
			this.BlessOverloadEnd();
		}
	}

	// Token: 0x06002B98 RID: 11160 RVA: 0x0012A1AC File Offset: 0x001283AC
	public override ChildCreatureModel MakeChildCreature(UnitModel origin)
	{
		if (!(origin is WorkerModel))
		{
			return null;
		}
		MovableObjectNode movableNode = origin.GetMovableNode();
		try
		{
			long instID = this.model.AddChildCreatureModel();
			ChildCreatureModel childCreatureModel = this.model.GetChildCreatureModel(instID);
			childCreatureModel.GetMovableNode().Assign(movableNode);
			childCreatureModel.Unit.init = true;
			childCreatureModel.GetMovableNode().StopMoving();
			childCreatureModel.Escape();
			childCreatureModel.GetMovableNode().SetDirection(origin.GetMovableNode().GetDirection());
			PassageObjectModel currentPassage = movableNode.currentPassage;
			Yggdrasil.InfestedPassage value = null;
			if (!this.infestedPassages.TryGetValue(currentPassage, out value))
			{
				Texture2D sp = Resources.Load("Sprites/CreatureSprite/Yggdrasil/filter_hall") as Texture2D;
				PassageObject passageObject = SefiraMapLayer.instance.GetPassageObject(currentPassage);
				if (passageObject != null)
				{
					GameObject filter = passageObject.SetPassageFilter(sp, "YggdrasilSporeFilter", "Normal", 0);
					value = new Yggdrasil.InfestedPassage(currentPassage, filter, this);
					this.infestedPassages.Add(currentPassage, value);
				}
			}
			WorkerModel workerModel = origin as WorkerModel;
			if (workerModel != null)
			{
				workerModel.GetWorkerUnit().gameObject.SetActive(false);
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
		return base.MakeChildCreature(origin);
	}

	// Token: 0x06002B99 RID: 11161 RVA: 0x0012A2F0 File Offset: 0x001284F0
	public UnitModel[] GetTargets(PassageObjectModel passage)
	{
		List<UnitModel> list = new List<UnitModel>();
		if (passage == null)
		{
			return list.ToArray();
		}
		foreach (MovableObjectNode movableObjectNode in passage.GetEnteredTargets(this.movable))
		{
			UnitModel unit = movableObjectNode.GetUnit();
			if (this.IsHostile(movableObjectNode))
			{
				list.Add(unit);
			}
		}
		return list.ToArray();
	}

	// Token: 0x06002B9A RID: 11162 RVA: 0x0012A35C File Offset: 0x0012855C
	private bool IsHostile(MovableObjectNode mov)
	{
		UnitModel unit = mov.GetUnit();
		CreatureModel creatureModel = unit as CreatureModel;
		return unit.hp > 0f && unit.IsAttackTargetable() && unit != this.model && (creatureModel == null || !(creatureModel.script is Yggdrasil_Sapling));
	}

	// Token: 0x06002B9B RID: 11163 RVA: 0x0002AAFD File Offset: 0x00028CFD
	private bool IsTransformed()
	{
		return this.animScript.IsTransformed();
	}

	// Token: 0x06002B9C RID: 11164 RVA: 0x0002AB0A File Offset: 0x00028D0A
	private void AddFilters(float alpha)
	{
		this.animScript.AddFilters(alpha);
	}

	// Token: 0x06002B9D RID: 11165 RVA: 0x0002AB18 File Offset: 0x00028D18
	private void ReduceFilters(float alpha)
	{
		this.animScript.ReduceFilters(alpha);
	}

	// Token: 0x06002B9E RID: 11166 RVA: 0x0002AB26 File Offset: 0x00028D26
	private void SetFilters(float alpha)
	{
		this.animScript.SetFilters(alpha);
	}

	// Token: 0x06002B9F RID: 11167 RVA: 0x0010CC2C File Offset: 0x0010AE2C
	private string GetSoundSrc(string key)
	{
		string result;
		if (this.model.metaInfo.soundTable.TryGetValue(key, out result))
		{
			return result;
		}
		return string.Empty;
	}

	// Token: 0x06002BA0 RID: 11168 RVA: 0x0012A3C0 File Offset: 0x001285C0
	public SoundEffectPlayer MakeSound(string src, Vector3 pos, float vol = 1f)
	{
		string soundSrc = this.GetSoundSrc(src);
		if (soundSrc == string.Empty)
		{
			return null;
		}
		return SoundEffectPlayer.PlayOnce(soundSrc, pos);
	}

	// Token: 0x06002BA1 RID: 11169 RVA: 0x0012A3F4 File Offset: 0x001285F4
	public SoundEffectPlayer MakeSoundLoop(string src, Transform trans, float vol = 1f)
	{
		string soundSrc = this.GetSoundSrc(src);
		if (soundSrc == string.Empty)
		{
			return null;
		}
		this.RemoveLoopSound();
		return SoundEffectPlayer.Play(soundSrc, trans);
	}

	// Token: 0x06002BA2 RID: 11170 RVA: 0x0002AB34 File Offset: 0x00028D34
	private void RemoveLoopSound()
	{
		if (this.loopSound != null)
		{
			this.loopSound.Stop();
			this.loopSound = null;
		}
	}

	// Token: 0x06002BA3 RID: 11171 RVA: 0x0002AB59 File Offset: 0x00028D59
	private void AddFlower()
	{
		this.animScript.AddFlower();
	}

	// Token: 0x06002BA4 RID: 11172 RVA: 0x0002AB66 File Offset: 0x00028D66
	private void ReduceFlower()
	{
		this.animScript.ReduceFlower();
	}

	// Token: 0x06002BA5 RID: 11173 RVA: 0x0002AB73 File Offset: 0x00028D73
	private void InitFlower()
	{
		this.animScript.InitFlower();
	}

	// Token: 0x04002961 RID: 10593
	private float SUB_QLIPHOTH_ESCAPE_PROB = 0.3f;

	// Token: 0x04002962 RID: 10594
	private int SUB_QLIPHOTH_WORK_OHTER_CNT = 5;

	// Token: 0x04002963 RID: 10595
	private int BLESS_OVERLOAD_CNT = 5;

	// Token: 0x04002964 RID: 10596
	private int TARGET_SKILL_ID = 4;

	// Token: 0x04002965 RID: 10597
	private const int _PASSAGE_DMG_MIN = 9;

	// Token: 0x04002966 RID: 10598
	private const int _PASSAGE_DMG_MAX = 12;

	// Token: 0x04002967 RID: 10599
	private const RwbpType _PASSAGE_DMG_TYPE = RwbpType.W;

	// Token: 0x04002968 RID: 10600
	private Timer filterIncreaseTimer = new Timer();

	// Token: 0x04002969 RID: 10601
	private const float FILTER_INCREASE_TIME = 1f;

	// Token: 0x0400296A RID: 10602
	private Timer filterDecreaseTimer = new Timer();

	// Token: 0x0400296B RID: 10603
	private const float FILTER_DECREASE_TIME = 1f;

	// Token: 0x0400296C RID: 10604
	private Timer defaultSoundTimer = new Timer();

	// Token: 0x0400296D RID: 10605
	private const float _defaultSoundFreqMin = 5f;

	// Token: 0x0400296E RID: 10606
	private const float _defaultSoundFreqMax = 10f;

	// Token: 0x0400296F RID: 10607
	private const string _FILTER_SRC = "Sprites/CreatureSprite/Yggdrasil/filter_hall";

	// Token: 0x04002970 RID: 10608
	private const string _FILTER_NAME = "YggdrasilSporeFilter";

	// Token: 0x04002971 RID: 10609
	private Dictionary<PassageObjectModel, Yggdrasil.InfestedPassage> infestedPassages = new Dictionary<PassageObjectModel, Yggdrasil.InfestedPassage>();

	// Token: 0x04002972 RID: 10610
	private List<WorkerModel> blessedList = new List<WorkerModel>();

	// Token: 0x04002973 RID: 10611
	private WorkerModel blessTarget;

	// Token: 0x04002974 RID: 10612
	private SoundEffectPlayer loopSound;

	// Token: 0x04002975 RID: 10613
	private bool isInSkill;

	// Token: 0x04002976 RID: 10614
	private bool gonnaSubQliphoth;

	// Token: 0x04002977 RID: 10615
	private int workOtherCnt;

	// Token: 0x04002978 RID: 10616
	private YggdrasilAnim _animScript;

	// Token: 0x020004A7 RID: 1191
	public class SoundKey
	{
		// Token: 0x06002BA6 RID: 11174 RVA: 0x00004380 File Offset: 0x00002580
		public SoundKey()
		{
		}

		// Token: 0x06002BA7 RID: 11175 RVA: 0x0012A428 File Offset: 0x00128628
		// Note: this type is marked as 'beforefieldinit'.
		static SoundKey()
		{
		}

		// Token: 0x04002979 RID: 10617
		public static string BLESS = "bless";

		// Token: 0x0400297A RID: 10618
		public static string BLESS_AGENT = "bless_agent";

		// Token: 0x0400297B RID: 10619
		public static string CHANGE = "change";

		// Token: 0x0400297C RID: 10620
		public static string CHANGE_FIN = "change_fin";

		// Token: 0x0400297D RID: 10621
		public static string DEFAULT = "default";

		// Token: 0x0400297E RID: 10622
		public static string DEVIL_DEFAULT = "devil_default";

		// Token: 0x0400297F RID: 10623
		public static string DUST = "dust";
	}

	// Token: 0x020004A8 RID: 1192
	public class InfestedPassage
	{
		// Token: 0x06002BA8 RID: 11176 RVA: 0x0002AB80 File Offset: 0x00028D80
		public InfestedPassage(PassageObjectModel passage, GameObject filter, Yggdrasil script)
		{
			this.passage = passage;
			this.filter = filter;
			this.script = script;
			this.dmgTimer.StartTimer(5f);
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06002BA9 RID: 11177 RVA: 0x0002ABB8 File Offset: 0x00028DB8
		private CreatureModel model
		{
			get
			{
				if (this.script == null)
				{
					return null;
				}
				return this.script.model;
			}
		}

		// Token: 0x06002BAA RID: 11178 RVA: 0x0002ABD2 File Offset: 0x00028DD2
		public void Process()
		{
			if (this.script == null)
			{
				return;
			}
			if (this.dmgTimer.RunTimer())
			{
				this.dmgTimer.StartTimer(5f);
				this.Attack();
			}
		}

		// Token: 0x06002BAB RID: 11179 RVA: 0x0012A47C File Offset: 0x0012867C
		private void Attack()
		{
			this.AttackEffect();
			List<UnitModel> list = new List<UnitModel>(this.script.GetTargets(this.passage));
			foreach (UnitModel unitModel in list)
			{
				WorkerModel workerModel = unitModel as WorkerModel;
				bool flag = workerModel != null && !workerModel.IsPanic();
				int sapNum = this.GetSapNum();
				if (sapNum > 0)
				{
					for (int i = 0; i < sapNum; i++)
					{
						unitModel.TakeDamage(this.model, Yggdrasil.PassageDmg);
					}
					DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(unitModel, Yggdrasil.PassageDmg.type, this.model);
					if (flag && workerModel.unconAction == null && !workerModel.IsDead() && !workerModel.invincible && (workerModel.mental <= 0f || workerModel.IsPanic()))
					{
						this.script.AddBlessedList(workerModel);
						workerModel.GetMovableNode().StopMoving();
						workerModel.StopAction();
						workerModel.LoseControl();
						workerModel.StopPanic();
						workerModel.SetUncontrollableAction(new Uncontrollable_Yggdrasil_Fake_PanicReady(workerModel, this.script));
					}
				}
			}
		}

		// Token: 0x06002BAC RID: 11180 RVA: 0x0012A5E0 File Offset: 0x001287E0
		private void AttackEffect()
		{
			foreach (CreatureModel creatureModel in this.GetSaps())
			{
				this.script.MakeSound(Yggdrasil.SoundKey.DUST, creatureModel.Unit.gameObject.transform.position, 1f);
				Yggdrasil_Sapling yggdrasil_Sapling = creatureModel.script as Yggdrasil_Sapling;
				if (yggdrasil_Sapling != null)
				{
					yggdrasil_Sapling.animScript.SetEffect();
				}
			}
		}

		// Token: 0x06002BAD RID: 11181 RVA: 0x0012A654 File Offset: 0x00128854
		public bool IsAvailable()
		{
			return this.GetSapNum() > 0;
		}

		// Token: 0x06002BAE RID: 11182 RVA: 0x0012A670 File Offset: 0x00128870
		private CreatureModel[] GetSaps()
		{
			List<CreatureModel> list = new List<CreatureModel>();
			List<MovableObjectNode> list2 = new List<MovableObjectNode>(this.passage.GetEnteredTargets());
			foreach (MovableObjectNode movableObjectNode in list2)
			{
				CreatureModel creatureModel = movableObjectNode.GetUnit() as CreatureModel;
				if (creatureModel != null && creatureModel.script is Yggdrasil_Sapling && creatureModel.state == CreatureState.ESCAPE)
				{
					list.Add(creatureModel);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06002BAF RID: 11183 RVA: 0x0012A718 File Offset: 0x00128918
		private int GetSapNum()
		{
			return this.GetSaps().Length;
		}

		// Token: 0x06002BB0 RID: 11184 RVA: 0x0002AC06 File Offset: 0x00028E06
		public void Destroy()
		{
			if (this.filter != null)
			{
				UnityEngine.Object.Destroy(this.filter);
			}
		}

		// Token: 0x04002980 RID: 10624
		private Timer dmgTimer = new Timer();

		// Token: 0x04002981 RID: 10625
		private const float dmgFreq = 5f;

		// Token: 0x04002982 RID: 10626
		private Yggdrasil script;

		// Token: 0x04002983 RID: 10627
		private PassageObjectModel passage;

		// Token: 0x04002984 RID: 10628
		private GameObject filter;
	}
}
