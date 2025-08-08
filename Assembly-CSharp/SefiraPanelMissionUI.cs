using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200088C RID: 2188
public class SefiraPanelMissionUI : MonoBehaviour
{
	// Token: 0x06004357 RID: 17239 RVA: 0x00004604 File Offset: 0x00002804
	public SefiraPanelMissionUI()
	{
	}

	// Token: 0x06004358 RID: 17240 RVA: 0x0019CAD0 File Offset: 0x0019ACD0
	public void InitProgressMission(Mission mission)
	{ // <Mod> Overtime Dawn Mission
		this.MissionPrefix.text = LocalizeTextDataModel.instance.GetTextAppend(new string[]
		{
			"Mission",
			"Mission"
		});
		this.MissionTitle.text = LocalizeTextDataModel.instance.GetText(mission.metaInfo.title);
		string str = string.Empty;
		if (mission.metaInfo.id == 104 || mission.metaInfo.id == 109)
		{
			int num = 20;
			if (PlayerModel.instance.GetDay() < num)
			{
				str = string.Format(LocalizeTextDataModel.instance.GetText("SefiraBossCondition_day"), num, num) + "\n";
			}
		}
		this.MissionContext.text = str + LocalizeTextDataModel.instance.GetText(mission.metaInfo.desc);
	}

	// Token: 0x06004359 RID: 17241 RVA: 0x0019CBA8 File Offset: 0x0019ADA8
	public void InitRequireMission(List<string> requireText)
	{
		this.MissionPrefix.text = LocalizeTextDataModel.instance.GetText("MissionCondition");
		this.MissionTitle.text = string.Empty;
		this.MissionPrefix.alignment = TextAnchor.MiddleLeft;
		this.MissionPrefix.horizontalOverflow = HorizontalWrapMode.Overflow;
		string text = string.Join("\n", requireText.ToArray());
		this.MissionContext.text = text;
	}

	// Token: 0x0600435A RID: 17242 RVA: 0x0019CC14 File Offset: 0x0019AE14
	public void InitRequireBossStarting(List<string> requireText)
	{
		this.MissionPrefix.text = LocalizeTextDataModel.instance.GetText("SefiraBossCondition");
		this.MissionTitle.text = string.Empty;
		this.MissionPrefix.alignment = TextAnchor.MiddleLeft;
		this.MissionPrefix.horizontalOverflow = HorizontalWrapMode.Overflow;
		string text = string.Join("\n", requireText.ToArray());
		this.MissionContext.text = text;
	}

	// Token: 0x0600435B RID: 17243 RVA: 0x0019CC80 File Offset: 0x0019AE80
	public void UniqueDisable()
	{
		if (this.NoMission != null)
		{
			this.NoMission.gameObject.SetActive(true);
			this.NoMission.text = LocalizeTextDataModel.instance.GetTextAppend(new string[]
			{
				"Mission",
				"NoMission"
			});
		}
		this.MissionPrefix.gameObject.SetActive(false);
		this.MissionTitle.gameObject.SetActive(false);
		this.MissionContext.gameObject.SetActive(false);
	}

	// Token: 0x04003E1A RID: 15898
	public Text MissionPrefix;

	// Token: 0x04003E1B RID: 15899
	public Text MissionTitle;

	// Token: 0x04003E1C RID: 15900
	public Text MissionContext;

	// Token: 0x04003E1D RID: 15901
	public Text NoMission;

	// Token: 0x04003E1E RID: 15902
	public bool hasUniqueDisable;

	// Token: 0x04003E1F RID: 15903
	public bool notDisabled;
}
