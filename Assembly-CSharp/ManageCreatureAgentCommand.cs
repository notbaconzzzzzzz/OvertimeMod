using System;
using UnityEngine;

// Token: 0x02000B8A RID: 2954
public class ManageCreatureAgentCommand : WorkerCommand
{
	// Token: 0x0600592A RID: 22826 RVA: 0x000479EE File Offset: 0x00045BEE
	public ManageCreatureAgentCommand(CreatureModel targetCreature, AgentModel self, SkillTypeInfo skill, Sprite skillSprite)
	{
		this.targetCreature = targetCreature;
		this.skill = skill;
		this.skillSprite = skillSprite;
		this.coopAgents = new AgentModel[]
		{
			self
		};
	}

	// Token: 0x0600592B RID: 22827 RVA: 0x00047A23 File Offset: 0x00045C23
	public ManageCreatureAgentCommand(CreatureModel targetCreature, AgentModel[] coopAgents, SkillTypeInfo skill)
	{
		this.targetCreature = targetCreature;
		this.skill = skill;
		this.coopAgents = coopAgents;
	}

	// Token: 0x0600592C RID: 22828 RVA: 0x001FA040 File Offset: 0x001F8240
	public override void OnInit(WorkerModel agent)
	{
		base.OnInit(agent);
		this.targetCreature.script.OnAgentAllocateWork((AgentModel)agent);
		this.targetCreature.script.OnAllocatedWork(agent as AgentModel);
		this.targetCreature.Unit.room.SetWorkIcon(this.skillSprite);
	}

	// Token: 0x0600592D RID: 22829 RVA: 0x001FA09C File Offset: 0x001F829C
	public override void Execute()
	{
		base.Execute();
		if (this.targetCreature.IsEscaped())
		{
			base.Finish();
			return;
		}
		if (!this.targetCreature.script.IsWorkable())
		{
			base.Finish();
			return;
		}
		if (this.useSkill != null)
		{
			this.useSkill.OnFixedUpdate();
		}
		else
		{
			if (!this.targetCreature.script.CanEnterRoom())
			{
				base.Finish();
				return;
			}
			MovableObjectNode movableNode = this.actor.GetMovableNode();
			if (movableNode.GetCurrentNode() == this.targetCreature.GetWorkspaceNode())
			{
				this.CheckStarting((AgentModel)this.actor);
			}
			else if (!movableNode.IsMoving())
			{
				movableNode.MoveToNode(this.targetCreature.GetWorkspaceNode(), false);
			}
		}
	}

	// Token: 0x0600592E RID: 22830 RVA: 0x001FA170 File Offset: 0x001F8370
	public override void OnDestroy()
	{
		if (this.useSkill != null && !this.useSkill.IsFinished())
		{
			this.useSkill.CancelWork();
		}
		AgentModel agentModel = (AgentModel)this.actor;
		agentModel.FinishManage();
		this.targetCreature.WorkParamInit();
		Debug.Log("OnDestroy");
	}

	// Token: 0x0600592F RID: 22831 RVA: 0x0000403D File Offset: 0x0000223D
	public void Cancle()
	{
	}

	// Token: 0x06005930 RID: 22832 RVA: 0x001FA1CC File Offset: 0x001F83CC
	private void CheckStarting(AgentModel agent)
	{
		if (!this.waiting)
		{
			return;
		}
		if (!false)
		{
			this.useSkill = UseSkill.InitUseSkillAction(this.skill, agent, this.targetCreature);
			if (this.useSkill == null)
			{
				base.Finish();
			}
			this.waiting = false;
		}
	}

	// Token: 0x04005229 RID: 21033
	private AgentModel[] coopAgents;

	// Token: 0x0400522A RID: 21034
	private SkillTypeInfo skill;

	// Token: 0x0400522B RID: 21035
	private UseSkill useSkill;

	// Token: 0x0400522C RID: 21036
	private Sprite skillSprite;

	// Token: 0x0400522D RID: 21037
	private bool waiting = true;

	// Token: 0x0400522E RID: 21038
	private CreatureModel targetCreature;
}
