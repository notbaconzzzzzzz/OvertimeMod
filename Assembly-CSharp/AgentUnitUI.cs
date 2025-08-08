using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B07 RID: 2823
[Serializable]
public class AgentUnitUI
{
	// Token: 0x060054E8 RID: 21736 RVA: 0x00044DC8 File Offset: 0x00042FC8
	public void Initial(AgentModel model)
	{
		this.kitCreatureIcon.enabled = false;
	}

	// Token: 0x060054E9 RID: 21737 RVA: 0x000043A5 File Offset: 0x000025A5
	public void initUI()
	{
	}

	// Token: 0x060054EA RID: 21738 RVA: 0x001E7CAC File Offset: 0x001E5EAC
	public void activateUI(AgentModel model)
	{
		if (SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.YESOD, false))
		{
			this.DeactivateAllUI();
			return;
		}
		this.Activated = true;
		this.Name.text = model.GetUnitName();
		string empty = string.Empty;
		string empty2 = string.Empty;
		string text = model.GetTitle(out empty, out empty2);
		this.title.text = model.GetTitle();
		if (this.title.preferredWidth >= 510f)
		{
			text = string.Format("{0}{1}{2}", empty, Environment.NewLine, empty2);
			this.title.text = text;
		}
		this.title.gameObject.SetActive(true);
		Vector2 anchoredPosition = this.kitCreatureIcon.rectTransform.anchoredPosition;
		if (model.isAce)
		{
			anchoredPosition.x = 419f;
		}
		else
		{
			anchoredPosition.x = 362f;
		}
		this.kitCreatureIcon.rectTransform.anchoredPosition = anchoredPosition;
	}

	// Token: 0x060054EB RID: 21739 RVA: 0x001E7DA0 File Offset: 0x001E5FA0
	public void DeactivateAllUI()
	{
		if (this.Name != null)
		{
			this.Name.gameObject.SetActive(false);
		}
		if (this.title != null)
		{
			this.title.gameObject.SetActive(false);
		}
	}

	// Token: 0x060054EC RID: 21740 RVA: 0x00044DD6 File Offset: 0x00042FD6
	public void setUIValue(AgentModel model)
	{
		if (!this.Activated)
		{
			return;
		}
		if (!model.uiActivated)
		{
			return;
		}
	}

	// Token: 0x04004E6E RID: 20078
	public bool Activated;

	// Token: 0x04004E6F RID: 20079
	public Text Name;

	// Token: 0x04004E70 RID: 20080
	public Image kitCreatureIcon;

	// Token: 0x04004E71 RID: 20081
	public Text title;
}
