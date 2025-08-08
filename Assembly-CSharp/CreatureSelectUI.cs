/*
+private bool DoubleAbno // 
private void Start() // 
private void CheckKitGeneration() // 
public void OnCalled() // 
-private bool _tiperethRunned; // (!?) 
+private int _tiperethRunned; // (!?) 
*/
using System;
using System.Collections.Generic;
using CreatureGenerate;
using CreatureSelect;
using LobotomyBaseMod; // 
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200096C RID: 2412
public class CreatureSelectUI : MonoBehaviour, IAnimatorEventCalled
{
	// Token: 0x060048B0 RID: 18608 RVA: 0x0003D02B File Offset: 0x0003B22B
	public CreatureSelectUI()
	{
	}

	// Token: 0x170006A7 RID: 1703
	// (get) Token: 0x060048B1 RID: 18609 RVA: 0x0003D05F File Offset: 0x0003B25F
	public static CreatureSelectUI instance
	{
		get
		{
			return CreatureSelectUI._instance;
		}
	}

	// Token: 0x170006A8 RID: 1704
	// (get) Token: 0x060048B2 RID: 18610 RVA: 0x0003D066 File Offset: 0x0003B266
	private int Day
	{
		get
		{
			return PlayerModel.instance.GetDay();
		}
	}

	// Token: 0x170006A9 RID: 1705
	// (get) Token: 0x060048B3 RID: 18611 RVA: 0x0003D072 File Offset: 0x0003B272
	private bool ReExtractResearchCompleted
	{
		get
		{
			return ResearchDataModel.instance.IsUpgradedAbility("reextract_creature");
		}
	}

    // <Mod>
	private bool DoubleAbno
	{
		get
		{
			return SpecialModeConfig.instance.GetValue<bool>("DoubleAbno");
		}
	}

	// Token: 0x170006AA RID: 1706
	// (get) Token: 0x060048B4 RID: 18612 RVA: 0x0003D083 File Offset: 0x0003B283
	// (set) Token: 0x060048B5 RID: 18613 RVA: 0x0003D095 File Offset: 0x0003B295
	public bool IsEnabled
	{
		get
		{
			return this.RootObject.gameObject.activeInHierarchy;
		}
		private set
		{
			this.RootObject.gameObject.SetActive(value);
		}
	}

	// Token: 0x060048B6 RID: 18614 RVA: 0x001B3C08 File Offset: 0x001B1E08
	private void Awake()
	{
		if (CreatureSelectUI.instance != null && CreatureSelectUI.instance.gameObject != null)
		{
			UnityEngine.Object.Destroy(CreatureSelectUI.instance.gameObject);
		}
		CreatureSelectUI._instance = this;
		this.Block.gameObject.SetActive(false);
		this.TextBoxController.gameObject.SetActive(false);
	}

	// Token: 0x060048B7 RID: 18615 RVA: 0x0003D0A8 File Offset: 0x0003B2A8
	private void Start()
	{ // <Mod>
		this._tiperethRunned = 0; 
		this._reExtracted = false;
		this.threshold = 0;
		if (GlobalGameManager.instance.ExistEtcData())
		{
			GlobalGameManager.instance.LoadEtcFile();
		}
		this.Init();
	}

	// Token: 0x060048B8 RID: 18616 RVA: 0x001B3C74 File Offset: 0x001B1E74
	private void FixedUpdate()
	{
		if (this.FadeoutEffectTimer.started)
		{
			float volume = Mathf.Clamp(Mathf.Lerp(this.startVolume, 0f, this.FadeoutEffectTimer.Rate), 0f, 1f);
			StoryBgm.instance.SetVolume(volume);
			if (this.FadeoutEffectTimer.RunTimer())
			{
				StoryBgm.instance.SetVolume(0f);
			}
		}
	}

	// Token: 0x060048B9 RID: 18617 RVA: 0x0003D0DF File Offset: 0x0003B2DF
	public bool IsInteractable()
	{
		return !this.effectRunned;
	}

	// Token: 0x060048BA RID: 18618 RVA: 0x001B3CE8 File Offset: 0x001B1EE8
	public void OnClickUnit(CreatureSelectUnit unit)
	{ // <Patch>
		if (!this.effectRunned)
		{
			this.effectRunned = true;
			if (unit._creatureIdMod == 100015L)
			{
				PlayerModel.instance.AddWaitingCreature(100014L);
			}
			else
			{
				PlayerModel.instance.AddWaitingCreature_Mod(unit._creatureIdMod);
			}
			CreatureGenerateInfoManager.Instance.OnUsed_Mod(unit._creatureIdMod);
			this.GlobalControlAnim.SetTrigger("Close");
			this.FadeoutEffect(3f);
		}
		this.TextBoxController.Hide();
		/*
		if (!this.effectRunned)
		{
			this.effectRunned = true;
			if (unit.CreatureID == 100015L)
			{
				PlayerModel.instance.AddWaitingCreature(100014L);
			}
			else
			{
				PlayerModel.instance.AddWaitingCreature(unit.CreatureID);
			}
			CreatureGenerateInfoManager.Instance.OnUsed(unit.CreatureID);
			this.GlobalControlAnim.SetTrigger("Close");
			this.FadeoutEffect(3f);
		}
		this.TextBoxController.Hide();*/
	}

	// Token: 0x060048BB RID: 18619 RVA: 0x0003D0EA File Offset: 0x0003B2EA
	public void FadeoutEffect(float time = 3f)
	{
		this.FadeoutEffectTimer.StartTimer(time);
		this.startVolume = StoryBgm.instance.GetVolume();
	}

	// Token: 0x060048BC RID: 18620 RVA: 0x001B3D6C File Offset: 0x001B1F6C
	public void Init()
	{ // <Patch>
		try
		{
			if (!ReExtractResearchCompleted)
			{
				this.reExtractController.gameObject.SetActive(false);
			}
			else
			{
				if (!this.reExtractController.gameObject.activeInHierarchy && !this._reExtracted)
				{
					this.reExtractController.gameObject.SetActive(true);
				}
			}
			if (!this.CheckUIActivateCondition())
			{
				this.OnUIActionEnd();
			}
			else
			{
				this.effectRunned = false;
				this.filter.enabled = true;
				foreach (CreatureSelectUnit creatureSelectUnit in this.Units)
				{
					creatureSelectUnit.gameObject.SetActive(true);
					creatureSelectUnit.TransAnim.SetTrigger("Exit");
				}
				this.GlobalControlAnim.SetTrigger("Open");
				this.GetCreatureList(true);
				LobotomyBaseMod.ModDebug.Log("CurrentCreatures_Mod Count : " + this.CurrentCreatures_Mod.Count.ToString());
				if (this.CurrentCreatures_Mod.Count == 1)
				{
					this.Units[0].SetDisabled();
					this.Units[2].SetDisabled();
					this.Units[1].Init_Mod(this.CurrentCreatures_Mod[0]);
					this.Units[1].transform.SetParent(this.Index_Normal);
				}
				else
				{
					if (this.CurrentCreatures_Mod.Count == 0)
					{
						List<LobotomyBaseMod.LcIdLong> list = new List<LobotomyBaseMod.LcIdLong>();
						foreach (long id in new List<long>(CreatureGenerateInfo.GetAll(true)))
						{
							list.Add(new LobotomyBaseMod.LcIdLong(id));
						}
						foreach (CreatureModel creatureModel in CreatureManager.instance.GetCreatureList())
						{
							LobotomyBaseMod.LcIdLong item = new LobotomyBaseMod.LcIdLong(CreatureTypeList.instance.GetModId(creatureModel.metaInfo), creatureModel.metadataId);
							list.Remove(item);
						}
						for (int k = 0; k < 3; k++)
						{
							LobotomyBaseMod.LcIdLong lcIdLong = list[UnityEngine.Random.Range(0, list.Count)];
							this.Units[k].Init_Mod(lcIdLong);
							list.Remove(lcIdLong);
						}
					}
					else
					{
						List<int> list2 = new List<int>(new int[]
						{
							0,
							1,
							2
						});
						for (int l = 0; l < this.Units.Length; l++)
						{
							int num = list2[UnityEngine.Random.Range(0, list2.Count)];
							list2.Remove(num);
							CreatureSelectUnit creatureSelectUnit2 = this.Units[num];
							LobotomyBaseMod.LcIdLong lcIdLong2 = new LobotomyBaseMod.LcIdLong(-1L);
							if (l < this.CurrentCreatures_Mod.Count)
							{
								lcIdLong2 = this.CurrentCreatures_Mod[l];
							}
							creatureSelectUnit2.transform.SetParent(this.Index_Normal);
							if (lcIdLong2 == null)
							{
								LobotomyBaseMod.ModDebug.Log(" creatureid NULL!");
							}
							else
							{
								if (lcIdLong2.packageId == null)
								{
									LobotomyBaseMod.ModDebug.Log(" creatureid - pid NULL!");
								}
								else
								{
									LobotomyBaseMod.ModDebug.Log("Init creatureid : " + lcIdLong2.ToString());
								}
							}
							creatureSelectUnit2.Init_Mod(lcIdLong2);
						}
					}
				}
				StoryBgm.instance.PlayClip(this.clip, 55f);
				for (int m = this.Units.Length - 1; m >= 0; m--)
				{
					CreatureSelectUnit creatureSelectUnit = this.Units[m];
					for (int n = 0; n < m; n++)
					{
						CreatureSelectUnit creatureSelectUnit2 = this.Units[n];
						if (m != n && creatureSelectUnit2._creatureIdMod == creatureSelectUnit._creatureIdMod)
						{
							break;
						}
					}
				}
				if (ReExtractResearchCompleted && !this._reExtracted)
				{
					this.reExtractController.Show();
				}
				else
				{
					this.reExtractController.Hide();
				}
			}
		}
		catch (Exception ex)
		{
			LobotomyBaseMod.ModDebug.Log("CSUI.Initerror - " + ex.Message + Environment.NewLine + ex.StackTrace);
		}
		/*
		if (!this.ReExtractResearchCompleted)
		{
			this.reExtractController.gameObject.SetActive(false);
		}
		else if (!this.reExtractController.gameObject.activeInHierarchy && !this._reExtracted)
		{
			this.reExtractController.gameObject.SetActive(true);
		}
		if (!this.CheckUIActivateCondition())
		{
			this.OnUIActionEnd();
			return;
		}
		this.effectRunned = false;
		this.filter.enabled = true;
		foreach (CreatureSelectUnit creatureSelectUnit in this.Units)
		{
			creatureSelectUnit.gameObject.SetActive(true);
			creatureSelectUnit.TransAnim.SetTrigger("Exit");
		}
		this.GlobalControlAnim.SetTrigger("Open");
		this.GetCreatureList(true);
		if (this.CurrentCreatures.Count == 1)
		{
			this.Units[0].SetDisabled();
			this.Units[2].SetDisabled();
			this.Units[1].Init(this.CurrentCreatures[0]);
			this.Units[1].transform.SetParent(this.Index_Normal);
		}
		else if (this.CurrentCreatures.Count == 0)
		{
			List<long> list = new List<long>(CreatureGenerateInfo.GetAll(true));
			foreach (CreatureModel creatureModel in CreatureManager.instance.GetCreatureList())
			{
				list.Remove(creatureModel.metadataId);
			}
			for (int j = 0; j < 3; j++)
			{
				long num = list[UnityEngine.Random.Range(0, list.Count)];
				this.Units[j].Init(num);
				list.Remove(num);
			}
		}
		else
		{
			List<int> list2 = new List<int>(new int[]
			{
				0,
				1,
				2
			});
			for (int k = 0; k < this.Units.Length; k++)
			{
				int num2 = list2[UnityEngine.Random.Range(0, list2.Count)];
				list2.Remove(num2);
				CreatureSelectUnit creatureSelectUnit2 = this.Units[num2];
				long creatureId = -1L;
				if (k < this.CurrentCreatures.Count)
				{
					creatureId = this.CurrentCreatures[k];
				}
				creatureSelectUnit2.transform.SetParent(this.Index_Normal);
				creatureSelectUnit2.Init(creatureId);
			}
		}
		StoryBgm.instance.PlayClip(this.clip, 55f);
		for (int l = this.Units.Length - 1; l >= 0; l--)
		{
			CreatureSelectUnit creatureSelectUnit3 = this.Units[l];
			for (int m = 0; m < l; m++)
			{
				CreatureSelectUnit creatureSelectUnit4 = this.Units[m];
				if (l != m && creatureSelectUnit4.CreatureID == creatureSelectUnit3.CreatureID)
				{
					creatureSelectUnit3.gameObject.SetActive(false);
					break;
				}
			}
		}
		if (this.ReExtractResearchCompleted && !this._reExtracted)
		{
			this.reExtractController.Show();
			return;
		}
		this.reExtractController.Hide();*/
	}

	// Token: 0x060048BD RID: 18621 RVA: 0x001B4040 File Offset: 0x001B2240
	public void OnClickReExtract()
	{ // <Patch>
		try
		{
			if (ReExtractResearchCompleted)
			{
				this._reExtracted = true;
				this.GetCreatureList(true);
				CreatureSelectUnit[] units = this.Units;
				for (int i = 0; i < units.Length; i++)
				{
					units[i].gameObject.SetActive(true);
					units[i].OnChange();
				}
				LobotomyBaseMod.ModDebug.Log("CurrentCreatures_Mod Count : " + this.CurrentCreatures_Mod.Count.ToString());
				if (this.CurrentCreatures_Mod.Count == 1)
				{
					this.Units[0].SetDisabled();
					this.Units[2].SetDisabled();
					this.Units[1].Init_Mod(this.CurrentCreatures_Mod[0]);
					this.Units[1].transform.SetParent(this.Index_Normal);
				}
				else
				{
					if (this.CurrentCreatures_Mod.Count == 0)
					{
						List<LobotomyBaseMod.LcIdLong> list = new List<LobotomyBaseMod.LcIdLong>();
						foreach (long id in new List<long>(CreatureGenerateInfo.GetAll(true)))
						{
							list.Add(new LobotomyBaseMod.LcIdLong(id));
						}
						foreach (CreatureModel creatureModel in CreatureManager.instance.GetCreatureList())
						{
							LobotomyBaseMod.LcIdLong item = new LobotomyBaseMod.LcIdLong(CreatureTypeList.instance.GetModId(creatureModel.metaInfo), creatureModel.metadataId);
							list.Remove(item);
						}
						for (int k = 0; k < 3; k++)
						{
							LobotomyBaseMod.LcIdLong lcIdLong = list[UnityEngine.Random.Range(0, list.Count)];
							this.Units[k].Init_Mod(lcIdLong);
							list.Remove(lcIdLong);
						}
					}
					else
					{
						List<int> list2 = new List<int>(new int[]
						{
							0,
							1,
							2
						});
						for (int l = 0; l < this.Units.Length; l++)
						{
							int num = list2[UnityEngine.Random.Range(0, list2.Count)];
							list2.Remove(num);
							CreatureSelectUnit creatureSelectUnit = this.Units[num];
							LobotomyBaseMod.LcIdLong lcIdLong2 = new LobotomyBaseMod.LcIdLong(-1L);
							if (l < this.CurrentCreatures_Mod.Count)
							{
								lcIdLong2 = this.CurrentCreatures_Mod[l];
							}
							creatureSelectUnit.transform.SetParent(this.Index_Normal);
							if (lcIdLong2 == null)
							{
								LobotomyBaseMod.ModDebug.Log(" creatureid NULL!");
							}
							else
							{
								if (lcIdLong2.packageId == null)
								{
									LobotomyBaseMod.ModDebug.Log(" creatureid - pid NULL!");
								}
								else
								{
									LobotomyBaseMod.ModDebug.Log("Init creatureid : " + lcIdLong2.ToString());
								}
							}
							creatureSelectUnit.Init_Mod(lcIdLong2);
						}
					}
				}
				for (int m = this.Units.Length - 1; m >= 0; m--)
				{
					CreatureSelectUnit creatureSelectUnit = this.Units[m];
					for (int n = 0; n < m; n++)
					{
						CreatureSelectUnit creatureSelectUnit2 = this.Units[n];
						if (m != n && creatureSelectUnit2._creatureIdMod == creatureSelectUnit._creatureIdMod)
						{
							break;
						}
					}
				}
				if (!this._reExtracted)
				{
					this.reExtractController.Show();
				}
				else
				{
					this.reExtractController.Hide();
				}
			}
		}
		catch (Exception ex)
		{
			LobotomyBaseMod.ModDebug.Log("CSUI.OnClickReExtracterror - " + ex.Message + Environment.NewLine + ex.StackTrace);
		}
		/*
		if (!this.ReExtractResearchCompleted)
		{
			return;
		}
		this._reExtracted = true;
		this.GetCreatureList(false);
		CreatureSelectUnit[] units = this.Units;
		for (int i = 0; i < units.Length; i++)
		{
			units[i].OnChange();
		}
		if (this.CurrentCreatures.Count == 1)
		{
			this.Units[0].SetDisabled();
			this.Units[2].SetDisabled();
			this.Units[1].Init(this.CurrentCreatures[0]);
			this.Units[1].transform.SetParent(this.Index_Normal);
		}
		else if (this.CurrentCreatures.Count == 0)
		{
			List<long> list = new List<long>(CreatureGenerateInfo.GetAll(true));
			foreach (CreatureModel creatureModel in CreatureManager.instance.GetCreatureList())
			{
				list.Remove(creatureModel.metadataId);
			}
			for (int k = 0; k < 3; k++)
			{
				long num = list[UnityEngine.Random.Range(0, list.Count)];
				this.Units[k].Init(num);
				list.Remove(num);
			}
		}
		else
		{
			List<int> list2 = new List<int>(new int[]
			{
				0,
				1,
				2
			});
			for (int l = 0; l < this.Units.Length; l++)
			{
				int num2 = list2[UnityEngine.Random.Range(0, list2.Count)];
				list2.Remove(num2);
				CreatureSelectUnit creatureSelectUnit = this.Units[num2];
				long creatureId = -1L;
				if (l < this.CurrentCreatures.Count)
				{
					creatureId = this.CurrentCreatures[l];
				}
				creatureSelectUnit.transform.SetParent(this.Index_Normal);
				creatureSelectUnit.Init(creatureId);
			}
		}
		for (int m = this.Units.Length - 1; m >= 0; m--)
		{
			CreatureSelectUnit creatureSelectUnit2 = this.Units[m];
			for (int n = 0; n < m; n++)
			{
				CreatureSelectUnit creatureSelectUnit3 = this.Units[n];
				if (m != n && creatureSelectUnit3.CreatureID == creatureSelectUnit2.CreatureID)
				{
					creatureSelectUnit2.gameObject.SetActive(false);
					break;
				}
			}
		}
		if (!this._reExtracted)
		{
			this.reExtractController.Show();
			return;
		}
		this.reExtractController.Hide();*/
	}

	// Token: 0x060048BE RID: 18622 RVA: 0x0003D108 File Offset: 0x0003B308
	public void OnUIActionEnd()
	{
		this.IsEnabled = false;
		this.filter.enabled = false;
		if (GlobalGameManager.instance.ExistEtcData())
		{
			GlobalGameManager.instance.SaveEtcData();
		}
		StorySceneController.instance.InitStory();
	}

	// Token: 0x060048BF RID: 18623 RVA: 0x001B4280 File Offset: 0x001B2480
	private bool CheckUIActivateCondition()
	{
		bool result = true;
		int day = this.Day;
		if (PlayerModel.instance.ketherGameOver)
		{
			result = false;
		}
		else if (day < 0)
		{
			PlayerModel.instance.AddWaitingCreature(CreatureGenerateInfo.r1[0][0]);
			result = false;
		}
		else if (day >= 51)
		{
			result = false;
		}
		else if (day < 10)
		{
			if (day % 5 == 4)
			{
				result = false;
			}
		}
		else if (day < 20)
		{
			if (day != 14)
			{
				if (day == 18)
				{
					int openLevel = SefiraManager.instance.GetSefira("Netzach").openLevel;
					int openLevel2 = SefiraManager.instance.GetSefira("Hod").openLevel;
					result = (openLevel == 3 || openLevel2 == 3);
				}
				else if (day == 19)
				{
					result = false;
				}
			}
		}
		else if (day >= 20 && day < 25)
		{
			if (day == 24)
			{
				result = false;
			}
		}
		else if (day >= 25 && day < 35)
		{
			if (day != 29)
			{
				if (day == 33)
				{
					int openLevel3 = SefiraManager.instance.GetSefira("Chesed").openLevel;
					int openLevel4 = SefiraManager.instance.GetSefira("Geburah").openLevel;
					result = (openLevel3 == 3 || openLevel4 == 3);
				}
				else if (day == 34)
				{
					result = false;
				}
			}
		}
		else if (day >= 35 && day < 45)
		{
			if (day != 39)
			{
				if (day == 33)
				{
					int openLevel5 = SefiraManager.instance.GetSefira("Binah").openLevel;
					int openLevel6 = SefiraManager.instance.GetSefira("Chokhmah").openLevel;
					result = (openLevel5 == 3 || openLevel6 == 3);
				}
				else if (day == 44)
				{
					result = false;
				}
			}
		}
		else if (day >= 45 && day < 50)
		{
			if (SpecialModeConfig.instance.GetValue<bool>("DoubleAbno") && CreatureGenerateInfo.GetAll(false).Length < 100)
			{
				result = false;
			}
			if (day == 49)
			{
				result = false;
			}
		}
		else if (day >= 50)
		{
			result = false;
		}
		if (PlayerModel.instance.IsWaitingCreatureExist())
		{
			result = false;
		}
		if (GlobalGameManager.instance.gameMode == GameMode.HIDDEN)
		{
			result = false;
		}
		return result;
	}

	// Token: 0x060048C0 RID: 18624 RVA: 0x001B44D0 File Offset: 0x001B26D0
	private void CheckKitGeneration()
	{ // <Mod>
		int genDay = CreatureGenerateInfoManager.Instance.GenDay;
        if (DoubleAbno)
        {
            if (genDay < 20)
            {
                if ((genDay % 5 == 1 || genDay % 5 == 3) && _tiperethRunned == 1)
                {
                    CreatureGenerateInfoManager.Instance.GenKit = true;
                }
                else
                {
                    CreatureGenerateInfoManager.Instance.GenKit = false;
                }
            }
            else if (genDay >= 20 && genDay < 25)
            {
                CreatureGenerateInfoManager.Instance.GenKit = false;
                if (this._tiperethRunned == 3)
                {
                    CreatureGenerateInfoManager.Instance.GenKit = true;
                }
            }
            else if (genDay >= 25 && genDay < 45)
            {
                if ((genDay % 5 == 1 || genDay % 5 == 3) && _tiperethRunned == 1)
                {
                    CreatureGenerateInfoManager.Instance.GenKit = true;
                }
                else
                {
                    CreatureGenerateInfoManager.Instance.GenKit = false;
                }
            }
            else
            {
                CreatureGenerateInfoManager.Instance.GenKit = false;
                if (this._tiperethRunned == 3)
                {
                    CreatureGenerateInfoManager.Instance.GenKit = true;
                }
            }
        }
        else
        {
            if (genDay < 20)
            {
                if (genDay % 5 == 3)
                {
                    CreatureGenerateInfoManager.Instance.GenKit = true;
                }
                else
                {
                    CreatureGenerateInfoManager.Instance.GenKit = false;
                }
            }
            else if (genDay >= 20 && genDay < 25)
            {
                CreatureGenerateInfoManager.Instance.GenKit = false;
                if (genDay == 21)
                {
                    if (this._tiperethRunned == 1)
                    {
                        CreatureGenerateInfoManager.Instance.GenKit = true;
                    }
                }
                else if (genDay == 23 && this._tiperethRunned == 1)
                {
                    CreatureGenerateInfoManager.Instance.GenKit = true;
                }
            }
            else if (genDay >= 25 && genDay < 50)
            {
                if (genDay % 5 == 3)
                {
                    CreatureGenerateInfoManager.Instance.GenKit = true;
                }
                else
                {
                    CreatureGenerateInfoManager.Instance.GenKit = false;
                }
            }
        }
		if (CreatureGenerateInfoManager.Instance.GenKit && !CreatureGenerateInfoManager.Instance.CheckKitCreatureRemains())
		{
			CreatureGenerateInfoManager.Instance.GenKit = false;
		}
	}

	// Token: 0x060048C1 RID: 18625 RVA: 0x001B45D8 File Offset: 0x001B27D8
	private void SetSlotInit(bool setEmpty = true)
	{ // <Patch>
		this.CurrentCreatures_Mod.Clear();
		if (setEmpty)
		{
			foreach (CreatureSelectUnit creatureSelectUnit in this.Units)
			{
				creatureSelectUnit.Init_Mod(new LobotomyBaseMod.LcIdLong(-1L));
			}
		}
		/*
		this.CurrentCreatures.Clear();
		if (setEmpty)
		{
			foreach (CreatureSelectUnit creatureSelectUnit in this.Units)
			{
				creatureSelectUnit.Init(-1L);
			}
		}*/
	}

	// Token: 0x060048C2 RID: 18626 RVA: 0x001B4620 File Offset: 0x001B2820
	public static bool CheckCreatureExisting(long targetId)
	{ // <Patch>
		return CreatureSelectUI.CheckCreatureExisting_Mod(new LobotomyBaseMod.LcIdLong(targetId));
		/*
		if (targetId == 100014L)
		{
			return CreatureManager.instance.FindCreature(100015L) != null || CreatureManager.instance.FindCreature(100014L) != null;
		}
		return CreatureManager.instance.FindCreature(targetId) != null;*/
	}

	// Token: 0x060048C3 RID: 18627 RVA: 0x001B467C File Offset: 0x001B287C
	private void CheckYinAndYang()
	{ // <Patch>
		this.threshold++;
		if (this.threshold >= 3)
		{
			return;
		}
		bool flag = CreatureSelectUI.CheckCreatureExisting(100104L);
		bool flag2 = CreatureSelectUI.CheckCreatureExisting(300109L);
		List<CreatureModel> list = new List<CreatureModel>(CreatureManager.instance.GetCreatureList());
		int count = list.Count;
		if (flag2 && flag)
		{
			return;
		}
		if (flag)
		{
			if (this.Day >= 48)
			{
				if (CurrentCreatures_Mod.Count <= 1)
				{
					this.GetCreatureList(false);
					return;
				}
				this.CurrentCreatures_Mod.Remove(new LobotomyBaseMod.LcIdLong(100104L));
				return;
			}
			else if (CreatureGenerateInfoManager.Instance.GenKit && !PlayerModel.instance.IsWaitingCreature(300109L))
			{
				this.CurrentCreatures_Mod.Clear();
				this.CurrentCreatures_Mod.Add(new LobotomyBaseMod.LcIdLong(300109L));
			}
		}
		/*
		this.threshold++;
		if (this.threshold >= 3)
		{
			return;
		}
		List<CreatureModel> list = new List<CreatureModel>(CreatureManager.instance.GetCreatureList());
		bool flag = CreatureSelectUI.CheckCreatureExisting(100104L);
		bool flag2 = CreatureSelectUI.CheckCreatureExisting(300109L);
		int count = list.Count;
		if (flag2 && flag)
		{
			return;
		}
		if (flag)
		{
			if (this.Day >= 48)
			{
				if (this.CurrentCreatures.Count <= 1)
				{
					this.GetCreatureList(false);
					return;
				}
				this.CurrentCreatures.Remove(100104L);
				return;
			}
			else if (CreatureGenerateInfoManager.Instance.GenKit && !PlayerModel.instance.IsWaitingCreature(300109L))
			{
				this.CurrentCreatures.Clear();
				this.CurrentCreatures.Add(300109L);
			}
		}*/
	}

	// Token: 0x060048C4 RID: 18628 RVA: 0x001B4760 File Offset: 0x001B2960
	private void GetCreatureList(bool setEmpty = true)
	{ // <Patch>
		this.CurrentCreatures_Mod.Clear();
		CreatureGenerateInfoManager.Instance.CalculateDay();
		this.CheckKitGeneration();
		CreatureGenerateInfoManager.Instance.OnDayChanged();
		this.SetSlotInit(setEmpty);
		List<LobotomyBaseMod.LcIdLong> list = CreatureGenerateInfoManager.Instance.GetCreature_Mod();
		if (list == null)
		{
			list = new List<LobotomyBaseMod.LcIdLong>();
			Debug.LogError("null removed + " + (Day % 5 == 3).ToString());
			List<long> list2 = new List<long>(CreatureGenerateInfo.GetAll(true));
			foreach (long id in list2)
			{
				list.Add(new LobotomyBaseMod.LcIdLong(id));
			}
			List<LobotomyBaseMod.LcIdLong> collection = new List<LobotomyBaseMod.LcIdLong>(CreatureGenerateInfo.GetAll_Mod(true));
			list.AddRange(collection);
		}
		LobotomyBaseMod.ModDebug.Log("GetCreatureList list count : " + list.Count.ToString());
		List<LobotomyBaseMod.LcIdLong> list3 = new List<LobotomyBaseMod.LcIdLong>();
		foreach (long id2 in new List<long>(CreatureGenerateInfo.GetAll(true)))
		{
			list3.Add(new LobotomyBaseMod.LcIdLong(id2));
		}
		bool flag2 = list.Count == 0;
		if (list.Count == 0)
		{
			Debug.LogError("Could not make Creature");
			return;
		}
		foreach (CreatureModel creatureModel in CreatureManager.instance.GetCreatureList())
		{
			LobotomyBaseMod.LcIdLong lcIdLong = new LobotomyBaseMod.LcIdLong(CreatureTypeList.instance.GetModId(creatureModel.metaInfo), creatureModel.metadataId);
			list3.Remove(lcIdLong);
			if (lcIdLong == 100014L)
			{
				list3.Remove(new LobotomyBaseMod.LcIdLong(100015L));
			}
			if (!list.Remove(lcIdLong))
			{
				if (lcIdLong == 100015L)
				{
					list.Remove(new LobotomyBaseMod.LcIdLong(100014L));
				}
			}
		}
		List<LobotomyBaseMod.LcIdLong> list4 = new List<LobotomyBaseMod.LcIdLong>();
		for (int j = 0; j < 3; j++)
		{
			if (list.Count == 0)
			{
				if (list4.Count != 0)
				{
					break;
				}
				foreach (CreatureModel creatureModel2 in CreatureManager.instance.GetCreatureList())
				{
					LobotomyBaseMod.LcIdLong lcIdLong2 = new LobotomyBaseMod.LcIdLong(CreatureTypeList.instance.GetModId(creatureModel2.metaInfo), creatureModel2.metadataId);
					if (!list3.Remove(lcIdLong2))
					{
						if (lcIdLong2 == 100015L)
						{
							list.Remove(new LobotomyBaseMod.LcIdLong(100014L));
						}
					}
				}
				list = list3;
			}
			LobotomyBaseMod.LcIdLong item = list[UnityEngine.Random.Range(0, list.Count)];
			list4.Add(item);
			list.Remove(item);
		}
		this.CurrentCreatures_Mod.AddRange(list4);
		this.CheckYinAndYang();
		/*
		this.CurrentCreatures.Clear();
		CreatureGenerateInfoManager.Instance.CalculateDay();
		this.CheckKitGeneration();
		CreatureGenerateInfoManager.Instance.OnDayChanged();
		this.SetSlotInit(setEmpty);
		List<long> list = CreatureGenerateInfoManager.Instance.GetCreature();
		if (list == null)
		{
			Debug.LogError("null removed + " + (this.Day % 5 == 3).ToString());
			list = new List<long>(CreatureGenerateInfo.GetAll(true));
		}
		List<long> list2 = new List<long>(CreatureGenerateInfo.GetAll(true));
		if (list.Count == 0)
		{
			Debug.LogError("Could not make Creature");
			return;
		}
		foreach (CreatureModel creatureModel in CreatureManager.instance.GetCreatureList())
		{
			list2.Remove(creatureModel.metadataId);
			if (creatureModel.metadataId == 100014L)
			{
				list2.Remove(100015L);
			}
			if (!list.Remove(creatureModel.metadataId))
			{
				if (creatureModel.metadataId == 100015L)
				{
					list.Remove(100014L);
				}
			}
		}
		List<long> list3 = new List<long>();
		for (int j = 0; j < 3; j++)
		{
			if (list.Count == 0)
			{
				if (list3.Count != 0)
				{
					break;
				}
				foreach (CreatureModel creatureModel2 in CreatureManager.instance.GetCreatureList())
				{
					if (!list2.Remove(creatureModel2.metadataId))
					{
						if (creatureModel2.metadataId == 100015L)
						{
							list.Remove(100014L);
						}
					}
				}
				list = list2;
			}
			long item = list[UnityEngine.Random.Range(0, list.Count)];
			list3.Add(item);
			list.Remove(item);
		}
		this.CurrentCreatures.AddRange(list3);
		this.CheckYinAndYang();*/
	}

	// Token: 0x060048C5 RID: 18629 RVA: 0x001B4968 File Offset: 0x001B2B68
	public void OnExitUnit(CreatureSelectUnit unit)
	{
		this.Block.Hide();
		this.TextBoxController.Hide();
		foreach (CreatureSelectUnit creatureSelectUnit in this.Units)
		{
			if (!creatureSelectUnit.TransSelected)
			{
				creatureSelectUnit.transform.SetParent(this.Index_Normal);
			}
		}
	}

	// Token: 0x060048C6 RID: 18630 RVA: 0x001B49C8 File Offset: 0x001B2BC8
	public void OnEnterUnit(CreatureSelectUnit unit)
	{
		foreach (CreatureSelectUnit creatureSelectUnit in this.Units)
		{
			if (creatureSelectUnit.TransSelected)
			{
				return;
			}
		}
		this.Block.Show();
		this.TextBoxText.text = unit.GetText();
		this.TextBoxController.Show();
		foreach (CreatureSelectUnit creatureSelectUnit2 in this.Units)
		{
			if (unit.Equals(creatureSelectUnit2))
			{
				creatureSelectUnit2.transform.SetParent(this.Index_Select);
			}
			else
			{
				creatureSelectUnit2.transform.SetParent(this.Index_Normal);
			}
		}
	}

	// Token: 0x060048C7 RID: 18631 RVA: 0x001B4A84 File Offset: 0x001B2C84
	public void OnCalled()
	{ // <Mod>
        if (DoubleAbno)
        {
            if (((this.Day >= 20 && this.Day < 24) || (this.Day >= 45 && this.Day <= 49)) && this._tiperethRunned < 3)
            {
                this._tiperethRunned++;
                this._reExtracted = false;
                this.Init();
                return;
            }
            else if (_tiperethRunned < 1)
            {
                this._tiperethRunned++;
                this._reExtracted = false;
                this.Init();
                return;
            }
        }
		else if (((this.Day >= 20 && this.Day < 24) || (this.Day >= 45 && this.Day <= 49)) && this._tiperethRunned < 1)
		{
			this._tiperethRunned++;
			this._reExtracted = false;
			this.Init();
			return;
		}
		this.GlobalControlAnim.SetTrigger("GlobalClose");
		StoryBgm.instance.SetFadeIn(2f);
		StorySceneController.instance.InitStory();
	}

	// Token: 0x060048C8 RID: 18632 RVA: 0x0003D140 File Offset: 0x0003B340
	public void OnCalled(int i)
	{
		this.OnUIActionEnd();
	}

	// Token: 0x060048C9 RID: 18633 RVA: 0x00013DBC File Offset: 0x00011FBC
	public void AgentReset()
	{
		throw new NotImplementedException();
	}

	// Token: 0x060048CA RID: 18634 RVA: 0x00013DBC File Offset: 0x00011FBC
	public void SimpleReset()
	{
		throw new NotImplementedException();
	}

	// Token: 0x060048CB RID: 18635 RVA: 0x00013DBC File Offset: 0x00011FBC
	public void AnimatorEventInit()
	{
		throw new NotImplementedException();
	}

	// Token: 0x060048CC RID: 18636 RVA: 0x00013DBC File Offset: 0x00011FBC
	public void CreatureAnimCall(int i, CreatureBase script)
	{
		throw new NotImplementedException();
	}

	// Token: 0x060048CD RID: 18637 RVA: 0x00013DBC File Offset: 0x00011FBC
	public void AttackCalled(int i)
	{
		throw new NotImplementedException();
	}

	// Token: 0x060048CE RID: 18638 RVA: 0x00013DBC File Offset: 0x00011FBC
	public void AttackDamageTimeCalled()
	{
		throw new NotImplementedException();
	}

	// Token: 0x060048CF RID: 18639 RVA: 0x00013DBC File Offset: 0x00011FBC
	public void SoundMake(string src)
	{
		throw new NotImplementedException();
	}

	// <Patch>
	public static bool CheckCreatureExisting_Mod(LobotomyBaseMod.LcIdLong targetId)
	{
		if (targetId.packageId == string.Empty)
		{
			if (targetId == 100014L)
			{
				return CreatureManager.instance.FindCreature(100015L) != null || CreatureManager.instance.FindCreature(100014L) != null;
			}
		}
		return CreatureManager.instance.FindCreature_Mod(targetId) != null;
	}

	// Token: 0x060048D0 RID: 18640 RVA: 0x000043A5 File Offset: 0x000025A5
	static CreatureSelectUI()
	{
	}

	// Token: 0x0400431C RID: 17180
	private static CreatureSelectUI _instance;

	// Token: 0x0400431D RID: 17181
	public const int SelectStartDay = 0;

	// Token: 0x0400431E RID: 17182
	public const int SelectEndDay = 51;

	// Token: 0x0400431F RID: 17183
	public const long yin = 100104L;

	// Token: 0x04004320 RID: 17184
	public const long yang = 300109L;

	// Token: 0x04004321 RID: 17185
	public const long plagueDoctor = 100014L;

	// Token: 0x04004322 RID: 17186
	public const long deathangel = 100015L;

	// Token: 0x04004323 RID: 17187
	private bool _skip;

	// Token: 0x04004324 RID: 17188
	public GameObject RootObject;

	// Token: 0x04004325 RID: 17189
	[Header("Effect")]
	public Animator GlobalControlAnim;

	// Token: 0x04004326 RID: 17190
	public AudioClip clip;

	// Token: 0x04004327 RID: 17191
	private AudioClip clipSaved;

	// Token: 0x04004328 RID: 17192
	[Header("UI")]
	public CreatureSelectUnit[] Units;

	// Token: 0x04004329 RID: 17193
	[Header("Index")]
	public RectTransform Index_Normal;

	// Token: 0x0400432A RID: 17194
	public RectTransform Index_Select;

	// Token: 0x0400432B RID: 17195
	public UIController Block;

	// Token: 0x0400432C RID: 17196
	[Header("TextBox")]
	public UIController TextBoxController;

	// Token: 0x0400432D RID: 17197
	public Text TextBoxText;

	// Token: 0x0400432E RID: 17198
	public CameraFilterPack_TV_80 filter;

	// Token: 0x0400432F RID: 17199
	[Header("ReExtract")]
	public UIController reExtractController;

	// Token: 0x04004330 RID: 17200
	private bool _reExtracted;

	// Token: 0x04004331 RID: 17201
	private bool effectRunned;

	// Token: 0x04004332 RID: 17202
	private Timer EffectTimer = new Timer();

	// Token: 0x04004333 RID: 17203
	private List<long> CurrentCreatures = new List<long>();

	// Token: 0x04004334 RID: 17204
	private int threshold;

	// Token: 0x04004335 RID: 17205
	private int _tiperethRunned; // <Mod> changed to int

	// Token: 0x04004336 RID: 17206
	private Timer FadeoutEffectTimer = new Timer();

	// Token: 0x04004337 RID: 17207
	private float startVolume = 1f;

	// <Patch>
	[NonSerialized]
	public List<LobotomyBaseMod.LcIdLong> CurrentCreatures_Mod = new List<LobotomyBaseMod.LcIdLong>();
}
