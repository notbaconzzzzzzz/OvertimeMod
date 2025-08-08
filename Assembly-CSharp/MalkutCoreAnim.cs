using System;
using UnityEngine;

// Token: 0x02000287 RID: 647
public class MalkutCoreAnim : CreatureAnimEventCalled
{
	// Token: 0x06001219 RID: 4633 RVA: 0x0001792C File Offset: 0x00015B2C
	public MalkutCoreAnim()
	{
	}

	// Token: 0x17000153 RID: 339
	// (get) Token: 0x0600121A RID: 4634 RVA: 0x0001793F File Offset: 0x00015B3F
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

	// Token: 0x0600121B RID: 4635 RVA: 0x00017951 File Offset: 0x00015B51
	public void SetScript(MalkutCoreScript script)
	{ // <Mod>
		if (script is OvertimeMalkutCoreScript)
		{
			overtimeScript = script as OvertimeMalkutCoreScript;
		}
		this.script = script;
	}

	// Token: 0x0600121C RID: 4636 RVA: 0x0001440D File Offset: 0x0001260D
	public void OnChange()
	{
		this.animator.SetTrigger("Change");
	}

	// Token: 0x0600121D RID: 4637 RVA: 0x0001441F File Offset: 0x0001261F
	public void OnClear()
	{
		this.animator.SetTrigger("Dead");
	}

	// Token: 0x0600121E RID: 4638 RVA: 0x000C8518 File Offset: 0x000C6718
	public void Update()
	{
		if (this.closeTimer.started)
		{
			float rate = this.closeTimer.Rate;
			this.middlePivot.transform.localScale = Vector3.one * this.destroyCurve.Evaluate(rate);
		}
	}

	// Token: 0x04001642 RID: 5698
	private MalkutCoreScript script;

	// <Mod>
	private OvertimeMalkutCoreScript overtimeScript;

	// Token: 0x04001643 RID: 5699
	public Transform middlePivot;

	// Token: 0x04001644 RID: 5700
	public AnimationCurve startPsychoCurve;

	// Token: 0x04001645 RID: 5701
	public float startEffectTime = 3f;

	// Token: 0x04001646 RID: 5702
	public AnimationCurve destroyCurve;
}
