using System;
using UnityEngine;

// Token: 0x020002CF RID: 719
public class YesodCoreAnim : CreatureAnimEventCalled
{
	// Token: 0x060014C0 RID: 5312 RVA: 0x00013984 File Offset: 0x00011B84
	public YesodCoreAnim()
	{
	}

	// Token: 0x060014C1 RID: 5313 RVA: 0x00019A10 File Offset: 0x00017C10
	public void SetScript(YesodCoreScript script)
	{
		this.script = script;
	}

	// Token: 0x17000165 RID: 357
	// (get) Token: 0x060014C2 RID: 5314 RVA: 0x00019A19 File Offset: 0x00017C19
	private UnscaledTimer closeTimer
	{
		get
		{
			return this.script.bossBase._closeTimer;
		}
	}

	// Token: 0x060014C3 RID: 5315 RVA: 0x0001440D File Offset: 0x0001260D
	public void OnChange()
	{
		this.animator.SetTrigger("Change");
	}

	// Token: 0x060014C4 RID: 5316 RVA: 0x0001441F File Offset: 0x0001261F
	public void OnClear()
	{
		this.animator.SetTrigger("Dead");
	}

	// Token: 0x060014C5 RID: 5317 RVA: 0x000CC93C File Offset: 0x000CAB3C
	public void Update()
	{
		if (this.closeTimer.started)
		{
			float rate = this.closeTimer.Rate;
			this.middlePivot.transform.localScale = Vector3.one * this.destroyCurve.Evaluate(rate);
		}
	}

	// Token: 0x040017BF RID: 6079
	private YesodCoreScript script;

	// Token: 0x040017C0 RID: 6080
	public Transform middlePivot;

	// Token: 0x040017C1 RID: 6081
	public AnimationCurve destroyCurve;
}
