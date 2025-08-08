/*
public void Init(Mission m, MissionPopupUI.MissionPopupCallbackFunc callback) // Fix text being cut off
*/
using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A6C RID: 2668
public class MissionPopupUI : MonoBehaviour, IAnimatorEventCalled
{
	// Token: 0x06004FFB RID: 20475 RVA: 0x00041A35 File Offset: 0x0003FC35
	public MissionPopupUI()
	{
	}

	// Token: 0x17000778 RID: 1912
	// (get) Token: 0x06004FFC RID: 20476 RVA: 0x00041A5A File Offset: 0x0003FC5A
	public static MissionPopupUI instance
	{
		get
		{
			return MissionPopupUI._instance;
		}
	}

	// Token: 0x17000779 RID: 1913
	// (get) Token: 0x06004FFD RID: 20477 RVA: 0x00041A61 File Offset: 0x0003FC61
	public bool canClick
	{
		get
		{
			return this._canClick;
		}
	}

	// Token: 0x06004FFE RID: 20478 RVA: 0x00041A69 File Offset: 0x0003FC69
	private void Awake()
	{
		MissionPopupUI._instance = this;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06004FFF RID: 20479 RVA: 0x001D3B10 File Offset: 0x001D1D10
	public void Init(Mission m, MissionPopupUI.MissionPopupCallbackFunc callback)
	{ // <Mod>
		try
		{
			this.missionTeamNameText.text = CharacterResourceDataModel.instance.GetName(SefiraName.GetSefiraByEnum(m.metaInfo.sefira));
		}
		catch (Exception ex)
		{
			this.missionTeamNameText.text = string.Empty;
		}
		this.missionNameText.text = LocalizeTextDataModel.instance.GetTextAppend(new string[]
		{
			"Mission",
			"Mission"
		}) + LocalizeTextDataModel.instance.GetText(m.metaInfo.title);
		this.missionDescText.text = LocalizeTextDataModel.instance.GetTextAppend(new string[]
		{
			"Mission",
			"Context"
		}) + LocalizeTextDataModel.instance.GetText(m.metaInfo.desc);
		this.missionDiagText.text = LocalizeTextDataModel.instance.GetText(m.metaInfo.diag);
		missionDescText.resizeTextForBestFit = true;
		missionDescText.resizeTextMaxSize = missionDescText.fontSize;
		missionDescText.resizeTextMinSize = missionDescText.fontSize / 2;
		// TextGenerator textGenerator = missionDescText.cachedTextGeneratorForLayout;
		// int missingCharactors = textGenerator.characterCount - textGenerator.characterCountVisible;
		// int containerHeight = 3;
        // missionDescText.verticalOverflow = 
		this._callback = callback;
		SefiraUIColor sefiraColor = UIColorManager.instance.GetSefiraColor(m.metaInfo.sefira);
		foreach (ColorMultiplier colorMultiplier in this.frameColorMultipliers)
		{
			colorMultiplier.Init(sefiraColor.imageColor);
		}
		base.gameObject.SetActive(true);
		this._canClick = false;
		this.Anim.Show();
		this.PlayAudio();
		this.clickTimer.StartTimer(this.clickBlockTime);
	}

	// Token: 0x06005000 RID: 20480 RVA: 0x00041A7D File Offset: 0x0003FC7D
	public void PlayAudio()
	{
		LocalAudioManager.instance.PlayClip("MissionPopUp");
	}

	// Token: 0x06005001 RID: 20481 RVA: 0x00041A8E File Offset: 0x0003FC8E
	public void OnClick()
	{
		if (!this._canClick)
		{
			return;
		}
		this._callback();
		this._canClick = false;
		this.Anim.Hide();
	}

	// Token: 0x06005002 RID: 20482 RVA: 0x0002B111 File Offset: 0x00029311
	public void OnCalled()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06005003 RID: 20483 RVA: 0x000184BE File Offset: 0x000166BE
	public void OnCalled(int i)
	{
		if (i != 0)
		{
		}
	}

	// Token: 0x06005004 RID: 20484 RVA: 0x00013D64 File Offset: 0x00011F64
	public void AgentReset()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06005005 RID: 20485 RVA: 0x00013D64 File Offset: 0x00011F64
	public void SimpleReset()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06005006 RID: 20486 RVA: 0x00013D64 File Offset: 0x00011F64
	public void AnimatorEventInit()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06005007 RID: 20487 RVA: 0x00013D64 File Offset: 0x00011F64
	public void CreatureAnimCall(int i, CreatureBase script)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06005008 RID: 20488 RVA: 0x00013D64 File Offset: 0x00011F64
	public void AttackCalled(int i)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06005009 RID: 20489 RVA: 0x00013D64 File Offset: 0x00011F64
	public void AttackDamageTimeCalled()
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600500A RID: 20490 RVA: 0x00013D64 File Offset: 0x00011F64
	public void SoundMake(string src)
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600500B RID: 20491 RVA: 0x00041AB9 File Offset: 0x0003FCB9
	private void Update()
	{
		if (this.clickTimer.RunTimer())
		{
			this._canClick = true;
		}
	}

	// Token: 0x04004A4A RID: 19018
	private static MissionPopupUI _instance;

	// Token: 0x04004A4B RID: 19019
	public Text missionNameText;

	// Token: 0x04004A4C RID: 19020
	public Text missionDescText;

	// Token: 0x04004A4D RID: 19021
	public Text missionTeamNameText;

	// Token: 0x04004A4E RID: 19022
	public Text missionDiagText;

	// Token: 0x04004A4F RID: 19023
	public ColorMultiplier[] frameColorMultipliers;

	// Token: 0x04004A50 RID: 19024
	private Mission _mission;

	// Token: 0x04004A51 RID: 19025
	private MissionPopupUI.MissionPopupCallbackFunc _callback;

	// Token: 0x04004A52 RID: 19026
	public UIController Anim;

	// Token: 0x04004A53 RID: 19027
	private bool _canClick = true;

	// Token: 0x04004A54 RID: 19028
	public Timer clickTimer = new Timer();

	// Token: 0x04004A55 RID: 19029
	public float clickBlockTime = 2f;

	// Token: 0x02000A6D RID: 2669
	// (Invoke) Token: 0x0600500D RID: 20493
	public delegate void MissionPopupCallbackFunc();
}
