using System;
using System.Collections.Generic;
using WorkerSpine;

// Token: 0x02000395 RID: 917
public class ArmorCreature : CreatureBase, IObserver
{
	// Token: 0x06001C7C RID: 7292 RVA: 0x0001EFBB File Offset: 0x0001D1BB
	public ArmorCreature()
	{
	}

	// Token: 0x06001C7D RID: 7293 RVA: 0x000ECF94 File Offset: 0x000EB194
	public override void OnViewInit(CreatureUnit unit)
	{
		this._anim = (unit.animTarget as ArmorCreatureAnim);
		if (this._anim != null)
		{
			this._anim.SetModel(this.model);
		}
		IList<AgentModel> agentList = AgentManager.instance.GetAgentList();
		foreach (AgentModel agentModel in agentList)
		{
			if (agentModel.HasEquipment(4000371))
			{
				ArmorCreature.StackAgent stackAgent = new ArmorCreature.StackAgent();
				stackAgent.agent = agentModel;
				stackAgent.SetFire(EGOgiftModel.MakeGift(EquipmentTypeList.instance.GetData(4000371)), ArmorCreature.GiftState.BLUE_ONE);
				this._specialAgentList.Add(stackAgent);
			}
			else if (agentModel.HasEquipment(4000372))
			{
				ArmorCreature.StackAgent stackAgent2 = new ArmorCreature.StackAgent();
				stackAgent2.agent = agentModel;
				stackAgent2.SetFire(EGOgiftModel.MakeGift(EquipmentTypeList.instance.GetData(4000372)), ArmorCreature.GiftState.BLUE_TWO);
				this._specialAgentList.Add(stackAgent2);
			}
			else if (agentModel.HasEquipment(4000373))
			{
				ArmorCreature.StackAgent stackAgent3 = new ArmorCreature.StackAgent();
				stackAgent3.agent = agentModel;
				stackAgent3.SetFire(EGOgiftModel.MakeGift(EquipmentTypeList.instance.GetData(4000373)), ArmorCreature.GiftState.ORANGE);
				this._specialAgentList.Add(stackAgent3);
			}
			else if (agentModel.HasEquipment(4000374))
			{
				ArmorCreature.StackAgent stackAgent4 = new ArmorCreature.StackAgent();
				stackAgent4.agent = agentModel;
				stackAgent4.SetFire(EGOgiftModel.MakeGift(EquipmentTypeList.instance.GetData(4000374)), ArmorCreature.GiftState.RED);
				this._specialAgentList.Add(stackAgent4);
			}
		}
	}

	// Token: 0x06001C7E RID: 7294 RVA: 0x000ED158 File Offset: 0x000EB358
	public override void OnStageStart()
	{ // <Mod>
		foreach (ArmorCreature.StackAgent stackAgent in this._specialAgentList)
		{
            stackAgent.attachmentCount = 0;
			switch (stackAgent.state)
			{
			case ArmorCreature.GiftState.BLUE_ONE:
				stackAgent.SetFire(EGOgiftModel.MakeGift(EquipmentTypeList.instance.GetData(4000371)), ArmorCreature.GiftState.BLUE_ONE);
				break;
			case ArmorCreature.GiftState.BLUE_TWO:
				stackAgent.SetFire(EGOgiftModel.MakeGift(EquipmentTypeList.instance.GetData(4000372)), ArmorCreature.GiftState.BLUE_TWO);
				break;
			case ArmorCreature.GiftState.ORANGE:
				stackAgent.SetFire(EGOgiftModel.MakeGift(EquipmentTypeList.instance.GetData(4000373)), ArmorCreature.GiftState.ORANGE);
				break;
			case ArmorCreature.GiftState.RED:
				stackAgent.SetFire(EGOgiftModel.MakeGift(EquipmentTypeList.instance.GetData(4000374)), ArmorCreature.GiftState.RED);
				break;
			}
		}
		this.SetObserver(true);
	}

	// Token: 0x06001C7F RID: 7295 RVA: 0x0001EFCE File Offset: 0x0001D1CE
	public override void OnFixedUpdate(CreatureModel creature)
	{
		base.OnFixedUpdate(creature);
	}

	// Token: 0x06001C80 RID: 7296 RVA: 0x0001EFD7 File Offset: 0x0001D1D7
	public override void OnEnterRoom(UseSkill skill)
	{ // <Mod>
		base.OnEnterRoom(skill);
		this._isGiveGift = false;
		if (skill.skillTypeInfo.id == 4L)
		{
			this._isGiveGift = true;
		}
		_isReduceGift = false;
		if (skill.skillTypeInfo.id == 3L && SpecialModeConfig.instance.GetValue<bool>("CrumblingArmorGift"))
		{
			_isReduceGift = true;
            AgentModel agent = skill.agent;
			ArmorCreature.StackAgent stackAgent = this._specialAgentList.Find((ArmorCreature.StackAgent x) => x.agent == agent);
			if (stackAgent != null)
			{
                float amt = 30f;
                for (int i = 0; i < stackAgent.attachmentCount; i++)
                {
                    amt *= 2f;
                }
                agent.SetSpecialDeadScene(WorkerSpine.AnimatorName.ArmorCreatureDead);
                agent.TakeDamage(new DamageInfo(RwbpType.P, amt));
                if (!agent.IsDead())
                {
                    agent.ResetSpecialDeadScene();
                }
                stackAgent.attachmentCount++;
            }
		}
	}

	// Token: 0x06001C81 RID: 7297 RVA: 0x000ED258 File Offset: 0x000EB458
	public override void OnFinishWork(UseSkill skill)
	{ // <Mod>
		base.OnFinishWork(skill);
		if (skill.agent.fortitudeLevel < 2 && !skill.agent.invincible)
		{
			AgentUnit unit = skill.agent.GetUnit();
			unit.animChanger.ChangeAnimator(WorkerSpine.AnimatorName.ArmorCreatureDead);
			unit.animEventHandler.SetAnimEvent(new AnimatorEventHandler.AnimEventDelegate(this.OnAgentDeadAnimEvent));
			skill.agent.Die();
			if (this._anim != null)
			{
				this._anim.KillMotion();
			}
			return;
		}
		if (this._isGiveGift)
		{
			ArmorCreature.StackAgent stackAgent = this._specialAgentList.Find((ArmorCreature.StackAgent x) => x.agent == skill.agent);
			if (stackAgent == null)
			{
				ArmorCreature.StackAgent stackAgent2 = new ArmorCreature.StackAgent();
				stackAgent2.agent = skill.agent;
				stackAgent2.SetFire(EGOgiftModel.MakeGift(EquipmentTypeList.instance.GetData(4000371)), ArmorCreature.GiftState.BLUE_ONE);
				this._specialAgentList.Add(stackAgent2);
			}
			else
			{
				this.ChangeGift(stackAgent);
			}
			this._isGiveGift = false;
		}
		if (_isReduceGift)
		{
			ArmorCreature.StackAgent stackAgent = _specialAgentList.Find((ArmorCreature.StackAgent x) => x.agent == skill.agent);
			if (stackAgent != null)
			{
				ReduceGift(stackAgent);
			}
			_isReduceGift = false;
		}
	}

	// Token: 0x06001C82 RID: 7298 RVA: 0x0001EFF9 File Offset: 0x0001D1F9
	public override void OnStageRelease()
	{
		this.SetObserver(false);
	}

	// Token: 0x06001C83 RID: 7299 RVA: 0x0001F002 File Offset: 0x0001D202
	public void OnAgentDeadAnimEvent(int i)
	{
		if (i == 0)
		{
		}
	}

	// Token: 0x06001C84 RID: 7300 RVA: 0x000ED388 File Offset: 0x000EB588
	private void ChangeGift(ArmorCreature.StackAgent specialAgent)
	{
		switch (specialAgent.state)
		{
		case ArmorCreature.GiftState.BLUE_ONE:
			specialAgent.SetFire(EGOgiftModel.MakeGift(EquipmentTypeList.instance.GetData(4000372)), ArmorCreature.GiftState.BLUE_TWO);
			break;
		case ArmorCreature.GiftState.BLUE_TWO:
			specialAgent.SetFire(EGOgiftModel.MakeGift(EquipmentTypeList.instance.GetData(4000373)), ArmorCreature.GiftState.ORANGE);
			break;
		case ArmorCreature.GiftState.ORANGE:
			specialAgent.SetFire(EGOgiftModel.MakeGift(EquipmentTypeList.instance.GetData(4000374)), ArmorCreature.GiftState.RED);
			break;
		}
	}

    // <Mod>
	private void ReduceGift(ArmorCreature.StackAgent specialAgent)
	{
		switch (specialAgent.state)
		{
		case ArmorCreature.GiftState.BLUE_ONE:
			specialAgent.agent.ReleaseEGOgift(specialAgent.gift);
			_specialAgentList.Remove(specialAgent);
			break;
		case ArmorCreature.GiftState.BLUE_TWO:
			specialAgent.SetFire(EGOgiftModel.MakeGift(EquipmentTypeList.instance.GetData(4000371)), ArmorCreature.GiftState.BLUE_ONE);
			break;
		case ArmorCreature.GiftState.ORANGE:
			specialAgent.SetFire(EGOgiftModel.MakeGift(EquipmentTypeList.instance.GetData(4000372)), ArmorCreature.GiftState.BLUE_TWO);
			break;
		case ArmorCreature.GiftState.RED:
			specialAgent.SetFire(EGOgiftModel.MakeGift(EquipmentTypeList.instance.GetData(4000373)), ArmorCreature.GiftState.ORANGE);
			break;
		}
	}

	// Token: 0x06001C85 RID: 7301 RVA: 0x000ED420 File Offset: 0x000EB620
	public void OnNotice(string notice, params object[] param)
	{ // <Mod>
		if (notice == NoticeName.OnWorkStart)
		{
			CreatureModel creatureModel = param[0] as CreatureModel;
			if (creatureModel == this.model)
			{
				return;
			}
			long id = creatureModel.currentSkill.skillTypeInfo.id;
			if (id != 3L)
			{
				return;
			}
			AgentModel agent = creatureModel.currentSkill.agent;
			if (agent.invincible)
			{
				return;
			}
			ArmorCreature.StackAgent stackAgent = this._specialAgentList.Find((ArmorCreature.StackAgent x) => x.agent == agent);
			if (stackAgent != null)
			{
				if (SpecialModeConfig.instance.GetValue<bool>("CrumblingArmorGift"))
				{
					float amt = 30f;
					for (int i = 0; i < stackAgent.attachmentCount; i++)
					{
						amt *= 2f;
					}
					agent.SetSpecialDeadScene(WorkerSpine.AnimatorName.ArmorCreatureDead);
                    agent.TakeDamage(new DamageInfo(RwbpType.P, amt));
                    if (!agent.IsDead())
                    {
                        agent.ResetSpecialDeadScene();
                    }
					stackAgent.attachmentCount++;
				}
				else
				{
					AgentUnit unit = stackAgent.agent.GetUnit();
					unit.animChanger.ChangeAnimator(WorkerSpine.AnimatorName.ArmorCreatureDead);
					unit.animEventHandler.SetAnimEvent(new AnimatorEventHandler.AnimEventDelegate(this.OnAgentDeadAnimEvent));
					stackAgent.agent.Die();
					this._specialAgentList.Remove(stackAgent);
				}
			}
		}
		else if (notice == NoticeName.OnReleaseWork)
		{
			try
			{
				CreatureModel creatureModel2 = param[0] as CreatureModel;
				if (creatureModel2 != this.model)
				{
					long id2 = creatureModel2.currentSkill.skillTypeInfo.id;
					if (id2 == 4L)
					{
						AgentModel agent = creatureModel2.currentSkill.agent;
						if (!agent.IsPanic() && !agent.IsDead())
						{
							ArmorCreature.StackAgent stackAgent2 = this._specialAgentList.Find((ArmorCreature.StackAgent x) => x.agent == agent);
							if (stackAgent2 != null)
							{
								this.ChangeGift(stackAgent2);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
			}
		}
		else if (notice == NoticeName.OnChangeGift)
		{
			AgentModel agent = param[0] as AgentModel;
			ArmorCreature.StackAgent stackAgent3 = this._specialAgentList.Find((ArmorCreature.StackAgent x) => x.agent == agent);
			if (stackAgent3 != null)
			{
				if (!agent.HasEquipment(4000371) && !agent.HasEquipment(4000372) && !agent.HasEquipment(4000373) && !agent.HasEquipment(4000374))
				{
					this._specialAgentList.Remove(stackAgent3);
				}
			}
		}
	}

	// Token: 0x06001C86 RID: 7302 RVA: 0x000ED634 File Offset: 0x000EB834
	private void SetObserver(bool activate)
	{
		if (activate)
		{
			Notice.instance.Observe(NoticeName.OnReleaseWork, this);
			Notice.instance.Observe(NoticeName.OnChangeGift, this);
			Notice.instance.Observe(NoticeName.OnWorkStart, this);
		}
		else
		{
			Notice.instance.Remove(NoticeName.OnReleaseWork, this);
			Notice.instance.Remove(NoticeName.OnChangeGift, this);
			Notice.instance.Remove(NoticeName.OnWorkStart, this);
		}
	}

	// Token: 0x04001D3E RID: 7486
	private List<ArmorCreature.StackAgent> _specialAgentList = new List<ArmorCreature.StackAgent>();

	// Token: 0x04001D3F RID: 7487
	private bool _isGiveGift;

    // <Mod>
	private bool _isReduceGift;

	// Token: 0x04001D40 RID: 7488
	private ArmorCreatureAnim _anim;

	// Token: 0x02000396 RID: 918
	private enum GiftState
	{
		// Token: 0x04001D42 RID: 7490
		NONE,
		// Token: 0x04001D43 RID: 7491
		BLUE_ONE,
		// Token: 0x04001D44 RID: 7492
		BLUE_TWO,
		// Token: 0x04001D45 RID: 7493
		ORANGE,
		// Token: 0x04001D46 RID: 7494
		RED
	}

	// Token: 0x02000397 RID: 919
	private class StackAgent
	{
		// Token: 0x06001C87 RID: 7303 RVA: 0x00004380 File Offset: 0x00002580
		public StackAgent()
		{
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06001C88 RID: 7304 RVA: 0x0001F00A File Offset: 0x0001D20A
		// (set) Token: 0x06001C89 RID: 7305 RVA: 0x0001F012 File Offset: 0x0001D212
		public AgentModel agent { get; set; }

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06001C8A RID: 7306 RVA: 0x0001F01B File Offset: 0x0001D21B
		// (set) Token: 0x06001C8B RID: 7307 RVA: 0x0001F023 File Offset: 0x0001D223
		public ArmorCreature.GiftState state { get; set; }

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06001C8C RID: 7308 RVA: 0x0001F02C File Offset: 0x0001D22C
		// (set) Token: 0x06001C8D RID: 7309 RVA: 0x0001F034 File Offset: 0x0001D234
		public EGOgiftModel gift { get; set; }

        // <Mod>
		public int attachmentCount { get; set; }

		// Token: 0x06001C8E RID: 7310 RVA: 0x0001F03D File Offset: 0x0001D23D
		public void SetFire(EGOgiftModel g, ArmorCreature.GiftState s)
		{
			if (this.gift != null)
			{
				this.agent.ReleaseEGOgift(this.gift);
			}
			this.agent.AttachEGOgift(g);
			this.gift = g;
			this.state = s;
		}
	}
}
