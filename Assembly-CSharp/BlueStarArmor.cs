/*
changed some static values
*/
using System;

// Token: 0x02000615 RID: 1557
public class BlueStarArmor : EquipmentScriptBase
{
	// Token: 0x0600351F RID: 13599 RVA: 0x00030252 File Offset: 0x0002E452
	public BlueStarArmor()
	{
	}

	// Token: 0x06003520 RID: 13600 RVA: 0x00030265 File Offset: 0x0002E465
	public override void OnStageStart()
	{
		base.OnStageStart();
		this.worker = (base.model.owner as WorkerModel);
	}

	// Token: 0x06003521 RID: 13601 RVA: 0x0015DE60 File Offset: 0x0015C060
	public override void OnFixedUpdate()
	{
		base.OnFixedUpdate();
		if (!this.CheckCondition())
		{
			return;
		}
		if (this.mpHealTimer.started)
		{
			if (this.mpHealTimer.RunTimer())
			{
				this.MpHeal();
				this.mpHealTimer.StartTimer(_mpHealFreq);
			}
		}
		else
		{
			this.mpHealTimer.StartTimer(_mpHealFreq);
		}
	}

	// Token: 0x06003522 RID: 13602 RVA: 0x0015DECC File Offset: 0x0015C0CC
	private void MpHeal()
	{
		PassageObjectModel currentPassage = this.worker.GetMovableNode().currentPassage;
		if (currentPassage != null)
		{
			foreach (MovableObjectNode movableObjectNode in currentPassage.GetEnteredTargets())
			{
				WorkerModel workerModel = movableObjectNode.GetUnit() as WorkerModel;
				if (workerModel != null)
				{
					if (!workerModel.IsDead())
					{
						workerModel.RecoverMental(_mpHealValue);
					}
				}
			}
		}
	}

	// Token: 0x06003523 RID: 13603 RVA: 0x0015DF68 File Offset: 0x0015C168
	private bool CheckCondition()
	{
		return this.worker != null && !this.worker.IsDead() && this.worker.GetMovableNode().currentPassage != null && !this.worker.IsPanic() && this.worker.unconAction == null && !this.worker.CannotControll();
	}

	// Token: 0x04003170 RID: 12656
	private Timer mpHealTimer = new Timer();

	// Token: 0x04003171 RID: 12657
	private const float _mpHealFreq = 2.5f; // <Mod> changed from 5 sp

	// Token: 0x04003172 RID: 12658
	private const float _mpHealValue = 2.5f; // <Mod> changed from 5 seconds

	// Token: 0x04003173 RID: 12659
	private WorkerModel worker;
}
