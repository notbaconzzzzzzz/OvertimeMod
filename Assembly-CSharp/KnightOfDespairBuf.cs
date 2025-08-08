/*
public override float OnTakeDamage(UnitModel attacker, DamageInfo damageInfo) // 
*/
using System;
using System.Runtime.CompilerServices;

// Token: 0x02000413 RID: 1043
public class KnightOfDespairBuf : UnitBuf
{
	// Token: 0x060023D6 RID: 9174 RVA: 0x00024557 File Offset: 0x00022757
	public KnightOfDespairBuf(KnightOfDespair knight)
	{
		this.knight = knight;
		this.type = UnitBufType.KNIGHTOFDESPAIR_BLESS;
		this.duplicateType = BufDuplicateType.ONLY_ONE;
	}

	// Token: 0x060023D7 RID: 9175 RVA: 0x00024575 File Offset: 0x00022775
	public override void Init(UnitModel model)
	{
		this.model = model;
		if (model is AgentModel)
		{
			(model as AgentModel).SetWorkCommandCheckEvnet(new AgentModel.CheckCommandState(this.Check));
		}
	}

	// Token: 0x060023D8 RID: 9176 RVA: 0x000245A0 File Offset: 0x000227A0
	public override void OnUnitDie()
	{
		this.knight.OnBlessedWorkerEndangered();
	}

	// Token: 0x060023D9 RID: 9177 RVA: 0x000245AD File Offset: 0x000227AD
	public override void OnUnitPanic()
	{
		this.knight.OnBlessedWorkerEndangered();
		this.Destroy();
	}

	// Token: 0x060023DA RID: 9178 RVA: 0x00003FDD File Offset: 0x000021DD
	public override void FixedUpdate()
	{
	}

	// Token: 0x060023DB RID: 9179 RVA: 0x001072C0 File Offset: 0x001054C0
	public override void OnStageRelease()
	{
		base.OnStageRelease();
		if (this.model is AgentModel)
		{
			AgentModel agentModel = this.model as AgentModel;
			if (KnightOfDespairBuf.cache0 == null)
			{
				KnightOfDespairBuf.cache0 = new AgentModel.CheckCommandState(AgentModel.DummyCheckCommand);
			}
			agentModel.SetWorkCommandCheckEvnet(KnightOfDespairBuf.cache0);
		}
	}

	// Token: 0x060023DC RID: 9180 RVA: 0x000040E7 File Offset: 0x000022E7
	private bool Check()
	{
		return false;
	}

	// Token: 0x060023DD RID: 9181 RVA: 0x00107310 File Offset: 0x00105510
	public override float OnTakeDamage(UnitModel attacker, DamageInfo damageInfo)
	{ // <Mod> changed Pale damage multiplier from 1.5
		float result = 1f;
		switch (damageInfo.type)
		{
		case RwbpType.R:
		case RwbpType.W:
		case RwbpType.B:
			result = 0.5f;
			break;
		case RwbpType.P:
			result = 2f;
			break;
		}
		return result;
	}

	// Token: 0x0400230A RID: 8970
	public KnightOfDespair knight;

	// Token: 0x0400230B RID: 8971
	[CompilerGenerated]
	private static AgentModel.CheckCommandState cache0;
}
