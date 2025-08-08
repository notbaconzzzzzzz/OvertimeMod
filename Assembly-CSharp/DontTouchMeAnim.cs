using System;
using UnityEngine;

// Token: 0x02000250 RID: 592
public class DontTouchMeAnim : CreatureAnimScript
{
	// Token: 0x06000FA7 RID: 4007 RVA: 0x0001391C File Offset: 0x00011B1C
	public DontTouchMeAnim()
	{
	}

	// Token: 0x06000FA8 RID: 4008 RVA: 0x0001582C File Offset: 0x00013A2C
	public void SetCreatureScript(DontTouchMe srt)
	{
		this.creatureScript = srt;
	}

	// Token: 0x06000FA9 RID: 4009 RVA: 0x000043A5 File Offset: 0x000025A5
	public void OnClickCreature()
	{
        /*
		foreach (CreatureModel creatureModel in CreatureManager.instance.GetCreatureList())
		{
			creatureModel.SetQliphothCounter(0);
		}*/
	}

	// Token: 0x06000FAA RID: 4010 RVA: 0x00015835 File Offset: 0x00013A35
	public void PlayAnimation()
	{
		Debug.Log("PlayAnimation");
		this.animator.SetBool("Touch", true);
	}

	// Token: 0x040014AC RID: 5292
	private DontTouchMe creatureScript;
}
