using System;

namespace BinahBoss
{
	// Token: 0x0200083D RID: 2109
	public class GoldenOverload : BinahOverload
	{
		// Token: 0x060040FD RID: 16637 RVA: 0x001900D8 File Offset: 0x0018E2D8
		public GoldenOverload(BinahCoreScript binah) : base(binah, OverloadType.GOLDEN)
		{
			BinahPhase phase = binah.Phase;
			if (phase != BinahPhase.P1)
			{
				if (phase != BinahPhase.P2)
				{
					if (phase == BinahPhase.P3)
					{
						this.IsolatePercent = 0.15f;
					}
				}
				else
				{
					this.IsolatePercent = 0.15f;
				}
			}
			else
			{
				this.IsolatePercent = 0.15f;
			}
			this.ProbReductionValue = 0;
			this.TimeLimit = 60f;
			UnitBuf unitBufByType = binah.model.GetUnitBufByType(UnitBufType.BINAH_RECOVER);
			if (unitBufByType != null)
			{
				binah.model.RemoveUnitBuf(unitBufByType);
			}
		}

		// Token: 0x060040FE RID: 16638 RVA: 0x00190170 File Offset: 0x0018E370
		public override void OnSuccess()
		{
			this.binah.MakeBattleDesc(BinahStaticData.ID_GoldenOverload_Success);
			base.OnSuccess();
			this.binah.InterruptCurrentAction();
			this.binah.ClearActionQueue();
			this.binah.EnqueueAction(new BinahIdle(this.binah, BinahStaticData.BinahGroggyTime.GetRandomFloat(), true));
		}

		// Token: 0x060040FF RID: 16639 RVA: 0x00037DDA File Offset: 0x00035FDA
		public override void OnFail()
		{
			this.binah.MakeBattleDesc(BinahStaticData.ID_GoldenOverload_Failure);
			this.binah.model.AddUnitBuf(new BinahRecoverBuf(this.binah));
			base.OnFail();
		}
	}
}
