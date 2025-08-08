using System;

// Token: 0x02000B6A RID: 2922
public class Uncontrollable_Baku : UncontrollableAction
{
	// Token: 0x0600583D RID: 22589 RVA: 0x001F9944 File Offset: 0x001F7B44
	public Uncontrollable_Baku(WorkerModel model, Baku script)
	{
		this.model = model;
		this.script = script;
		this.remainClicked = 5;
		model.specialDeadScene = true;
		model.blockRecover = true;
		this.hp = model.hp;
		this.mp = model.mental;
		if (model.GetMovableNode() != null)
		{
			model.GetMovableNode().StopMoving();
		}
	}

	// Token: 0x0600583E RID: 22590 RVA: 0x001F99C8 File Offset: 0x001F7BC8
	public override void OnStageEnd()
	{
		base.OnStageEnd();
		try
		{
			this.model.GetWorkerUnit().animChanger.ChangeAnimator();
			this.model.invincible = false;
			this.model.Die();
		}
		catch (Exception ex)
		{
		}
	}

	// Token: 0x0600583F RID: 22591 RVA: 0x00046FB9 File Offset: 0x000451B9
	public override void Execute()
	{
		base.Execute();
		if (this.model == null)
		{
			return;
		}
		this.model.StopAction();
		this.model.GetMovableNode().StopMoving();
		this.CheckAnnoyed();
	}

	// Token: 0x06005840 RID: 22592 RVA: 0x001F9A24 File Offset: 0x001F7C24
	public override void OnClick()
	{
		if (this.waked)
		{
			return;
		}
		if (GameManager.currentGameManager.state != GameState.PLAYING)
		{
			return;
		}
		this.remainClicked--;
		if (this.remainClicked <= 0)
		{
			this.WakeUp();
		}
		else if (this.model is AgentModel)
		{
			AgentUnit agent = AgentLayer.currentLayer.GetAgent(this.model.instanceId);
			agent.CharRecoilInput(1);
		}
	}

	// Token: 0x06005841 RID: 22593 RVA: 0x00046FEE File Offset: 0x000451EE
	public override void OnDie()
	{
		base.OnDie();
		this.model.workerAnimator.SetTrigger("Die");
	}

	// Token: 0x06005842 RID: 22594 RVA: 0x0004700B File Offset: 0x0004520B
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.model.specialDeadScene = false;
		this.model.GetWorkerUnit().animChanger.ChangeAnimator();
		this.model.blockRecover = false;
	}

	// Token: 0x06005843 RID: 22595 RVA: 0x001F9AA0 File Offset: 0x001F7CA0
	public void CheckAnnoyed()
	{
		if (this.model.IsDead())
		{
			return;
		}
		if (this.model.IsPanic())
		{
			this.Annoyed();
		}
		else if (this.model.hp / this.hp < 0.2f)
		{
			this.Annoyed();
		}
		else if (this.model.mental / this.mp < 0.2f)
		{
			this.Annoyed();
		}
		else if (this.model.mental <= 0f || this.model.IsPanic())
		{
			this.Annoyed();
		}
	}

	// Token: 0x06005844 RID: 22596 RVA: 0x00047040 File Offset: 0x00045240
	private void Annoyed()
	{
		this.script.AnnoyWorker(this.model);
	}

	// Token: 0x06005845 RID: 22597 RVA: 0x00047053 File Offset: 0x00045253
	public void OnAnnoyed()
	{
		this.waked = true;
	}

	// Token: 0x06005846 RID: 22598 RVA: 0x0004705C File Offset: 0x0004525C
	private void WakeUp()
	{
		this.waked = true;
		this.script.WakeWorker(this.model);
	}

	// Token: 0x04005120 RID: 20768
	private const int _neededClicked = 5;

	// Token: 0x04005121 RID: 20769
	private const float _woundToBeAnnoyed = 0.2f;

	// Token: 0x04005122 RID: 20770
	private WorkerModel model;

	// Token: 0x04005123 RID: 20771
	private Baku script;

	// Token: 0x04005124 RID: 20772
	private bool waked;

	// Token: 0x04005125 RID: 20773
	private float hp = float.MaxValue;

	// Token: 0x04005126 RID: 20774
	private float mp = float.MaxValue;

	// Token: 0x04005127 RID: 20775
	private int remainClicked = 5;
}
