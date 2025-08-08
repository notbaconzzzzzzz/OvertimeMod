using System;

// Token: 0x020004A8 RID: 1192
public class YggdrasilBlessBuf : UnitBuf
{
	// Token: 0x06002B9A RID: 11162 RVA: 0x0002A563 File Offset: 0x00028763
	public YggdrasilBlessBuf(WorkerModel worker, Yggdrasil script)
	{ // <Mod>
		this.worker = worker;
		this.script = script;
		this.type = UnitBufType.YGGDRASIL_BLESS;
		this.duplicateType = BufDuplicateType.ONLY_ONE;
		this.remainTime = float.PositiveInfinity;
        this.statBuf = new UnitStatBuf(float.PositiveInfinity, UnitBufType.YGGDRASIL_BLESS_STAT);
		this.statBuf.maxMental = 10;
		this.statBuf.cubeSpeed = 20;
		this.worker.AddUnitBuf(statBuf);
	}

	// Token: 0x06002B9B RID: 11163 RVA: 0x0002A593 File Offset: 0x00028793
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.worker != null && this.worker.HasUnitBuf(UnitBufType.DEATH_ANGEL_BETRAYER))
		{
			this.Destroy();
		}
	}

	// Token: 0x06002B9C RID: 11164 RVA: 0x0002357A File Offset: 0x0002177A
	public override void OnUnitDie()
	{
		base.OnUnitDie();
		this.Destroy();
	}

	// Token: 0x06002B9D RID: 11165 RVA: 0x0002A5BD File Offset: 0x000287BD
	public override void OnDestroy()
	{ // <Mod>
        this.statBuf.Destroy();
		base.OnDestroy();
		if (this.worker != null)
		{
			this.worker.GetWorkerUnit().RemoveUnitBuf(this);
			this.script.RemoveBlessedWorker(this.worker);
		}
	}

	// Token: 0x04002970 RID: 10608
	private WorkerModel worker;

	// Token: 0x04002971 RID: 10609
	private Yggdrasil script;

    private UnitStatBuf statBuf; // <Mod>
}
