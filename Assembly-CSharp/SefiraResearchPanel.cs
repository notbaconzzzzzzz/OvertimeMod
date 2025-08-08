using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200088E RID: 2190
public class SefiraResearchPanel : MonoBehaviour
{
	// Token: 0x060043DE RID: 17374 RVA: 0x00039B85 File Offset: 0x00037D85
	public SefiraResearchPanel()
	{
	}

	// Token: 0x17000640 RID: 1600
	// (get) Token: 0x060043DF RID: 17375 RVA: 0x00039BA2 File Offset: 0x00037DA2
	public Sefira CurrentSefira
	{
		get
		{
			return this._currentSefira;
		}
	}

	// Token: 0x060043E0 RID: 17376 RVA: 0x001A3BBC File Offset: 0x001A1DBC
	public void Init(Sefira sefira)
	{
		this._currentSefira = sefira;
		this.DescController.gameObject.SetActive(false);
		List<ResearchItemModel> modelBySefira = ResearchDataModel.instance.GetModelBySefira(this.CurrentSefira.indexString);
		List<ResearchItemModel> remainResearchListBySefira = ResearchDataModel.instance.GetRemainResearchListBySefira(this.CurrentSefira.indexString);
		SefiraUIColor sefiraColor = UIColorManager.instance.GetSefiraColor(sefira.sefiraEnum);
		this.currentColor = sefiraColor.imageColor;
		this.blackColor = Color.black;
		int count = modelBySefira.Count;
		this._currentResearchMax = count;
		for (int i = 0; i < 4; i++)
		{
			SefiraResearchSlot sefiraResearchSlot = this.slots[i];
			Transform child = sefiraResearchSlot.RootObject.transform.GetChild(1);
			if (child.GetComponent<Image>() != null)
			{
				sefiraResearchSlot.Frame = child.GetComponent<Image>();
			}
			if (i < count)
			{
				sefiraResearchSlot.RootObject.SetActive(true);
				bool flag = true;
				ResearchItemModel researchItemModel = modelBySefira[i];
				if (remainResearchListBySefira.Contains(researchItemModel))
				{
					flag = false;
				}
				if (flag)
				{
					sefiraResearchSlot.RootObject.SetActive(true);
					sefiraResearchSlot.Icon.sprite = researchItemModel.info.GetIcon();
					sefiraResearchSlot.name = researchItemModel.info.GetDesc().name;
					sefiraResearchSlot.desc = researchItemModel.info.GetDesc().desc;
					sefiraResearchSlot.Icon.color = sefiraColor.imageColor;
					sefiraResearchSlot.Texture.color = sefiraColor.imageColor;
				}
				else
				{
					sefiraResearchSlot.RootObject.SetActive(false);
				}
			}
			else
			{
				sefiraResearchSlot.RootObject.SetActive(false);
			}
		}
	}

	// Token: 0x060043E1 RID: 17377 RVA: 0x001A3D7C File Offset: 0x001A1F7C
	public void DisplayData(int index)
	{
		if (index < 0 || index >= this.slots.Count)
		{
			return;
		}
		if (this._current >= 0 && this._current < this.slots.Count)
		{
			this.slots[this._current].Frame.color = this.blackColor;
			this.slots[this._current].Icon.color = this.currentColor;
		}
		this._current = index;
		this.CurrentResearchName.text = this.slots[index].name;
		this.CurrentResearchDesc.text = this.slots[index].desc;
		this.slots[index].Frame.color = this.currentColor;
		this.slots[index].Icon.color = this.blackColor;
		this.DescController.gameObject.SetActive(true);
	}

	// Token: 0x060043E2 RID: 17378 RVA: 0x001A3E90 File Offset: 0x001A2090
	public void ResetAll()
	{
		if (this._picked != -1)
		{
			this.slots[this._picked].OnDePicked();
		}
		if (this._current >= 0 && this._current < this.slots.Count)
		{
			this.slots[this._current].Frame.color = this.blackColor;
			this.slots[this._current].Icon.color = this.currentColor;
			this._current = -1;
		}
		this._picked = -1;
		this._current = -1;
		this.CurrentResearchDesc.text = string.Empty;
		this.CurrentResearchName.text = string.Empty;
		this.DescController.gameObject.SetActive(false);
	}

	// Token: 0x060043E3 RID: 17379 RVA: 0x00039BAA File Offset: 0x00037DAA
	public void OnPointerEnter(int index)
	{
		if (index < 0 || index >= this._currentResearchMax)
		{
			return;
		}
		this.DisplayData(index);
		this.SefiraSound.OnPlayInList(0);
	}

	// Token: 0x060043E4 RID: 17380 RVA: 0x001A3F6C File Offset: 0x001A216C
	public void OnPointerExit(int index)
	{
		if (index < 0 || index >= this._currentResearchMax)
		{
			return;
		}
		if (this._current >= 0 && this._current < this.slots.Count)
		{
			this.slots[this._current].Frame.color = this.blackColor;
			this.slots[this._current].Icon.color = this.currentColor;
			this._current = -1;
		}
		if (this._picked != -1)
		{
			this.DisplayData(this._picked);
			return;
		}
		this.ResetAll();
	}

	// Token: 0x060043E5 RID: 17381 RVA: 0x001A4018 File Offset: 0x001A2218
	public void OnPointerClick(int index)
	{
		if (index < 0 || index >= this._currentResearchMax)
		{
			return;
		}
		if (this._picked != -1)
		{
			this.slots[this._picked].OnDePicked();
		}
		if (this._picked == index)
		{
			this._picked = -1;
			return;
		}
		this._picked = index;
		this.slots[this._picked].OnPicked();
		this._current = this._picked;
		this.SefiraSound.OnPlayInList(1);
	}

	// Token: 0x04003E75 RID: 15989
	public const int ResearchMax = 4;

	// Token: 0x04003E76 RID: 15990
	public List<SefiraResearchSlot> slots;

	// Token: 0x04003E77 RID: 15991
	private Sefira _currentSefira;

	// Token: 0x04003E78 RID: 15992
	public GameObject DescController;

	// Token: 0x04003E79 RID: 15993
	public Text CurrentResearchName;

	// Token: 0x04003E7A RID: 15994
	public Text CurrentResearchDesc;

	// Token: 0x04003E7B RID: 15995
	public AudioClipPlayer SefiraSound;

	// Token: 0x04003E7C RID: 15996
	private int _picked = -1;

	// Token: 0x04003E7D RID: 15997
	private int _currentResearchMax = 4;

	// Token: 0x04003E7E RID: 15998
	private int _current = -1;

	// Token: 0x04003E7F RID: 15999
	private Color currentColor;

	// Token: 0x04003E80 RID: 16000
	private Color blackColor;
}
