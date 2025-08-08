using System;
using UnityEngine;

// Token: 0x0200028C RID: 652
public class NetzachCoreAnim : CreatureAnimEventCalled
{
	// Token: 0x06001244 RID: 4676 RVA: 0x00017ACA File Offset: 0x00015CCA
	public NetzachCoreAnim()
	{
	}

	// Token: 0x17000154 RID: 340
	// (get) Token: 0x06001245 RID: 4677 RVA: 0x00017ADD File Offset: 0x00015CDD
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

	// Token: 0x06001246 RID: 4678 RVA: 0x00017AEF File Offset: 0x00015CEF
	public void SetScript(NetzachCoreScript script)
	{ // <Mod>
		if (script is OvertimeNetzachCoreScript)
		{
			overtimeScript = script as OvertimeNetzachCoreScript;
		}
		this.script = script;
	}

	// Token: 0x06001247 RID: 4679 RVA: 0x00014435 File Offset: 0x00012635
	public void OnChange()
	{
		this.animator.SetTrigger("Change");
	}

	// Token: 0x06001248 RID: 4680 RVA: 0x00014447 File Offset: 0x00012647
	public void OnClear()
	{
		this.animator.SetTrigger("Dead");
	}

	// Token: 0x06001249 RID: 4681 RVA: 0x000C94A8 File Offset: 0x000C76A8
	public void Update()
	{
		if (this.closeTimer.started)
		{
			float rate = this.closeTimer.Rate;
			this.middlePivot.transform.localScale = Vector3.one * this.destroyCurve.Evaluate(rate);
		}
	}

	// Token: 0x04001654 RID: 5716
	private NetzachCoreScript script;

	// <Mod>
	private OvertimeNetzachCoreScript overtimeScript;

	// Token: 0x04001655 RID: 5717
	public Transform middlePivot;

	// Token: 0x04001656 RID: 5718
	public float startEffectTime = 3f;

	// Token: 0x04001657 RID: 5719
	public AnimationCurve destroyCurve;
}
