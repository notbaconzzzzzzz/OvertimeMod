using System;
using UnityEngine.UI;

// Token: 0x02000B2C RID: 2860
[Serializable]
public class RabbitUnitUI
{
	// Token: 0x06005674 RID: 22132 RVA: 0x00004378 File Offset: 0x00002578
	public RabbitUnitUI()
	{
	}

	// Token: 0x06005675 RID: 22133 RVA: 0x00045D7A File Offset: 0x00043F7A
	public void activateUI(RabbitModel model)
	{
		if (SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.YESOD))
		{
			this.DeactivateAllUI();
			return;
		}
		this.Activated = true;
		this.Name.text = model.name;
	}

	// Token: 0x06005676 RID: 22134 RVA: 0x00045DAB File Offset: 0x00043FAB
	public void DeactivateAllUI()
	{
		if (this.Name != null)
		{
			this.Name.gameObject.SetActive(false);
		}
	}

	// Token: 0x04004FF1 RID: 20465
	public bool Activated;

	// Token: 0x04004FF2 RID: 20466
	public Text Name;
}
