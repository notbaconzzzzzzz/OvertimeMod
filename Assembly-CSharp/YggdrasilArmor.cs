using System;

// Token: 0x0200062D RID: 1581
public class YggdrasilArmor : EquipmentScriptBase
{
	// Token: 0x06003577 RID: 13687 RVA: 0x000305B3 File Offset: 0x0002E7B3
	public YggdrasilArmor()
	{
	}

	// Token: 0x06003578 RID: 13688 RVA: 0x000305C6 File Offset: 0x0002E7C6
	public override void OnStageStart()
	{
		base.OnStageStart();
		this.worker = (base.model.owner as WorkerModel);
	}

	// Token: 0x06003579 RID: 13689 RVA: 0x0015EF44 File Offset: 0x0015D144
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

	// Token: 0x0600357A RID: 13690 RVA: 0x0015EFB0 File Offset: 0x0015D1B0
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

	// Token: 0x0600357B RID: 13691 RVA: 0x0015F04C File Offset: 0x0015D24C
	private bool CheckCondition()
	{
		return this.worker != null && !this.worker.IsDead() && this.worker.GetMovableNode().currentPassage != null && !this.worker.IsPanic() && this.worker.unconAction == null && !this.worker.CannotControll();
	}

	// Token: 0x040031A9 RID: 12713
	private Timer mpHealTimer = new Timer();

	// Token: 0x040031AA RID: 12714
	private const float _mpHealFreq = 2.5f;

	// Token: 0x040031AB RID: 12715
	private const float _mpHealValue = 2.5f;

	// Token: 0x040031AC RID: 12716
	private WorkerModel worker;
}
