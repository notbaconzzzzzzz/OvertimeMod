using System;
using UnityEngine;

// Token: 0x0200026C RID: 620
public class HodCoreAnim : CreatureAnimEventCalled
{
	// Token: 0x060010F3 RID: 4339 RVA: 0x000139AC File Offset: 0x00011BAC
	public HodCoreAnim()
	{
	}

	// Token: 0x060010F4 RID: 4340 RVA: 0x000168B7 File Offset: 0x00014AB7
	public void SetScript(HodCoreScript script)
	{ // <Mod>
		if (script is OvertimeHodCoreScript)
		{
			overtimeScript = script as OvertimeHodCoreScript;
		}
		this.script = script;
	}

	// Token: 0x1700014B RID: 331
	// (get) Token: 0x060010F5 RID: 4341 RVA: 0x000168C0 File Offset: 0x00014AC0
	private UnscaledTimer closeTimer
	{ // <Mod>
		get
		{
            if (overtimeScript != null)
            {
                return overtimeScript.bossBase._closeTimer;
            }
			return this.script.bossBase._closeTimer;
		}
	}

	// Token: 0x060010F6 RID: 4342 RVA: 0x00014435 File Offset: 0x00012635
	public void OnChange()
	{
		this.animator.SetTrigger("Change");
	}

	// Token: 0x060010F7 RID: 4343 RVA: 0x00014447 File Offset: 0x00012647
	public void OnClear()
	{
		this.animator.SetTrigger("Dead");
	}

	// Token: 0x060010F8 RID: 4344 RVA: 0x000C6800 File Offset: 0x000C4A00
	public void Update()
	{
		if (this.closeTimer.started)
		{
			float rate = this.closeTimer.Rate;
			this.middlePivot.transform.localScale = Vector3.one * this.destroyCurve.Evaluate(rate);
		}
	}

	// Token: 0x04001566 RID: 5478
	private HodCoreScript script;

	// <Mod>
	private OvertimeHodCoreScript overtimeScript;

	// Token: 0x04001567 RID: 5479
	public Transform middlePivot;

	// Token: 0x04001568 RID: 5480
	public AnimationCurve destroyCurve;
}
