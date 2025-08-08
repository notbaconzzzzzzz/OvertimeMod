using System;

// Token: 0x02000644 RID: 1604
public class SingingMachineGiftBuf : UnitStatBuf
{
	// Token: 0x060035C4 RID: 13764 RVA: 0x00031031 File Offset: 0x0002F231
	public SingingMachineGiftBuf() : base(SingingMachineGiftBuf.REMAIN, UnitBufType.SINGING_MACHINE_GIFT_BUF)
	{
		this.duplicateType = BufDuplicateType.ONLY_ONE;
	}

	// Token: 0x060035C5 RID: 13765 RVA: 0x00031047 File Offset: 0x0002F247
	public override void Init(UnitModel model)
	{
		base.Init(model);
		this.agent = (model as AgentModel);
		if (this.agent != null)
		{
			this.attackSpeed = 10f;
		}
		this.remainTime = SingingMachineGiftBuf.REMAIN;
	}

	// Token: 0x060035C6 RID: 13766 RVA: 0x00023B47 File Offset: 0x00021D47
	public override void OnUnitDie()
	{
		base.OnUnitDie();
		this.Destroy();
	}

	// Token: 0x060035C7 RID: 13767 RVA: 0x0003107D File Offset: 0x0002F27D
	// Note: this type is marked as 'beforefieldinit'.
	static SingingMachineGiftBuf()
	{
	}

	// Token: 0x040031D1 RID: 12753
	private static float REMAIN = 5f;

	// Token: 0x040031D2 RID: 12754
	private const int ATTACK_SPEED_BUF = 10;

	// Token: 0x040031D3 RID: 12755
	private AgentModel agent;
}
