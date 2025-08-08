/*
public Sprite GetSefiraSymbol(SefiraEnum sefira, int level = 1) // 
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Customizing;
using UnityEngine;
using WorkerSpine;
using WorkerSprite;

// Token: 0x02000BE6 RID: 3046
public class WorkerSpriteManager : MonoBehaviour
{
	// Token: 0x06005C24 RID: 23588 RVA: 0x000497DA File Offset: 0x000479DA
	public WorkerSpriteManager()
	{
		this.workerColor = new List<WorkerColorPreset>();
		this.Modifys = new List<int>();
	}

	// Token: 0x17000856 RID: 2134
	// (get) Token: 0x06005C25 RID: 23589 RVA: 0x000497F8 File Offset: 0x000479F8
	public static WorkerSpriteManager instance
	{
		get
		{
			return WorkerSpriteManager._instance;
		}
	}

	// Token: 0x17000857 RID: 2135
	// (get) Token: 0x06005C26 RID: 23590 RVA: 0x000497FF File Offset: 0x000479FF
	public string CustomFolderSrc
	{
		get
		{
			return Application.dataPath + "/CustomData";
		}
	}

	// Token: 0x17000858 RID: 2136
	// (get) Token: 0x06005C27 RID: 23591 RVA: 0x00049810 File Offset: 0x00047A10
	public string CustomHairSrc
	{
		get
		{
			return this.CustomFolderSrc + "/Hair";
		}
	}

	// Token: 0x17000859 RID: 2137
	// (get) Token: 0x06005C28 RID: 23592 RVA: 0x00049822 File Offset: 0x00047A22
	public string CustomFaceSrc
	{
		get
		{
			return this.CustomFolderSrc + "/Face";
		}
	}

	// Token: 0x1700085A RID: 2138
	// (get) Token: 0x06005C29 RID: 23593 RVA: 0x00049834 File Offset: 0x00047A34
	public string CustomFrontHairSrc
	{
		get
		{
			return this.CustomHairSrc + "/Front";
		}
	}

	// Token: 0x1700085B RID: 2139
	// (get) Token: 0x06005C2A RID: 23594 RVA: 0x00049846 File Offset: 0x00047A46
	public string CustomRearHairSrc
	{
		get
		{
			return this.CustomHairSrc + "/Rear";
		}
	}

	// Token: 0x1700085C RID: 2140
	// (get) Token: 0x06005C2B RID: 23595 RVA: 0x00049858 File Offset: 0x00047A58
	public string CustomEyeSrc
	{
		get
		{
			return this.CustomFaceSrc + "/Eye_Default";
		}
	}

	// Token: 0x1700085D RID: 2141
	// (get) Token: 0x06005C2C RID: 23596 RVA: 0x0004986A File Offset: 0x00047A6A
	public string CustomPanicEyeSrc
	{
		get
		{
			return this.CustomFaceSrc + "/Eye_Panic";
		}
	}

	// Token: 0x1700085E RID: 2142
	// (get) Token: 0x06005C2D RID: 23597 RVA: 0x0004987C File Offset: 0x00047A7C
	public string CustomDeadEyeSrc
	{
		get
		{
			return this.CustomFaceSrc + "/Eye_Dead";
		}
	}

	// Token: 0x1700085F RID: 2143
	// (get) Token: 0x06005C2E RID: 23598 RVA: 0x0004988E File Offset: 0x00047A8E
	public string CustomMouthSrc
	{
		get
		{
			return this.CustomFaceSrc + "/Mouth_Default";
		}
	}

	// Token: 0x17000860 RID: 2144
	// (get) Token: 0x06005C2F RID: 23599 RVA: 0x000498A0 File Offset: 0x00047AA0
	public string CustomBattleMouthSrc
	{
		get
		{
			return this.CustomFaceSrc + "/Mouth_Battle";
		}
	}

	// Token: 0x17000861 RID: 2145
	// (get) Token: 0x06005C30 RID: 23600 RVA: 0x000498B2 File Offset: 0x00047AB2
	public string CustomEyebrowSrc
	{
		get
		{
			return this.CustomFaceSrc + "/Eyebrow_Default";
		}
	}

	// Token: 0x17000862 RID: 2146
	// (get) Token: 0x06005C31 RID: 23601 RVA: 0x000498C4 File Offset: 0x00047AC4
	public string CustomBattleEyebrowSrc
	{
		get
		{
			return this.CustomFaceSrc + "/Eyebrow_Battle";
		}
	}

	// Token: 0x17000863 RID: 2147
	// (get) Token: 0x06005C32 RID: 23602 RVA: 0x000498D6 File Offset: 0x00047AD6
	public string CustomPanicEyebrowSrc
	{
		get
		{
			return this.CustomFaceSrc + "/Eyebrow_Panic";
		}
	}

	// Token: 0x06005C33 RID: 23603 RVA: 0x002091CC File Offset: 0x002073CC
	public void Awake()
	{
		if (WorkerSpriteManager._instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		WorkerSpriteManager._instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.LoadSpineData();
		this._uniqueWeaponDic = new Dictionary<string, UniqueWeaponSpriteUnit>();
		foreach (UniqueWeaponSpriteUnit uniqueWeaponSpriteUnit in this.uniqueWeaponSprites)
		{
			this._uniqueWeaponDic.Add(uniqueWeaponSpriteUnit.indexer, uniqueWeaponSpriteUnit);
		}
	}

	// Token: 0x06005C34 RID: 23604 RVA: 0x00209274 File Offset: 0x00207474
	private int GetOfficerArmor(SefiraEnum sefira)
	{
		switch (sefira)
		{
		case SefiraEnum.MALKUT:
			return 1001;
		case SefiraEnum.YESOD:
			return 1002;
		case SefiraEnum.HOD:
			return 1004;
		case SefiraEnum.NETZACH:
			return 1003;
		case SefiraEnum.TIPERERTH1:
			return 1005;
		case SefiraEnum.TIPERERTH2:
			return 1006;
		case SefiraEnum.GEBURAH:
			return 1007;
		case SefiraEnum.CHESED:
			return 1008;
		case SefiraEnum.BINAH:
			return 1009;
		case SefiraEnum.CHOKHMAH:
			return 1010;
		default:
			return 1;
		}
	}

	// Token: 0x06005C35 RID: 23605 RVA: 0x002092F4 File Offset: 0x002074F4
	public Sprite GetPanicShadow(RwbpType type)
	{
		Sprite result;
		try
		{
			result = this.PanicShadow[type - RwbpType.R];
		}
		catch (Exception ex)
		{
			result = null;
		}
		return result;
	}

	// Token: 0x06005C36 RID: 23606 RVA: 0x00209330 File Offset: 0x00207530
	private void LoadSpineData()
	{
		this.basicData = WorkerSpriteDataLoader.Loader.basic;
		this.equipData = WorkerSpriteDataLoader.Loader.equip;
		this.workerColor = WorkerSpriteDataLoader.Loader.colorLoaded;
		WorkerEquipmentSprite workerEquipmentSprite = null;
		if (this.equipData.GetData(EquipmentSpriteRegion.WEAPON, out workerEquipmentSprite))
		{
			WorkerWeaponSprite workerWeaponSprite = new WorkerWeaponSprite
			{
				subRegion = workerEquipmentSprite.subRegion,
				loadedData = workerEquipmentSprite.loadedData
			};
			workerWeaponSprite.Init();
			this.equipData.lib.Remove(EquipmentSpriteRegion.WEAPON);
			this.equipData.lib.Add(EquipmentSpriteRegion.WEAPON, workerWeaponSprite);
		}
		if (this.equipData.GetData(EquipmentSpriteRegion.CLOTHES, out workerEquipmentSprite))
		{
			WorkerClothesSprite workerClothesSprite = new WorkerClothesSprite
			{
				subRegion = workerEquipmentSprite.subRegion,
				loadedData = workerEquipmentSprite.loadedData
			};
			workerClothesSprite.Init();
			this.equipData.lib.Remove(EquipmentSpriteRegion.CLOTHES);
			this.equipData.lib.Add(EquipmentSpriteRegion.CLOTHES, workerClothesSprite);
		}
		if (this.equipData.GetData(EquipmentSpriteRegion.ATTACHMENTS, out workerEquipmentSprite))
		{
			WorkerAttachmentSprite workerAttachmentSprite = new WorkerAttachmentSprite
			{
				subRegion = EquipmentSpriteRegion.ATTACHMENTS,
				loadedData = workerEquipmentSprite.loadedData
			};
			workerAttachmentSprite.Init();
			this.equipData.lib.Remove(EquipmentSpriteRegion.ATTACHMENTS);
			this.equipData.lib.Add(EquipmentSpriteRegion.ATTACHMENTS, workerAttachmentSprite);
		}
		this.LoadCustomSprites();
	}

	// Token: 0x06005C37 RID: 23607 RVA: 0x00209494 File Offset: 0x00207694
	public SpriteResourceLoadData GetClothesSet(int id)
	{ // <Patch??>
		SpriteResourceLoadData result = null;
		WorkerEquipmentSprite workerEquipmentSprite = null;
		if (this.equipData.GetData(EquipmentSpriteRegion.CLOTHES, out workerEquipmentSprite))
		{
			WorkerClothesSprite workerClothesSprite = workerEquipmentSprite as WorkerClothesSprite;
			if ((result = workerClothesSprite.GetData(id)) == null)
			{
				Debug.Log("Finding clothes set " + id);
				result = workerClothesSprite.lib[0];
			}
		}
		return result;
	}

	// Token: 0x06005C38 RID: 23608 RVA: 0x002094EC File Offset: 0x002076EC
	public void SetAgentBasicData(WorkerSprite.WorkerSprite set, Appearance appear)
	{
		WorkerSpriteSaveData saveData = set.saveData;
		try
		{
			WorkerSpriteSaveData.Pair frontHair = new WorkerSpriteSaveData.Pair
			{
				a = (long)appear.lib.FrontHair.id,
				b = appear.lib.FrontHair.innerid
			};
			saveData.FrontHair = frontHair;
			WorkerSpriteSaveData.Pair rearHair = new WorkerSpriteSaveData.Pair
			{
				a = (long)appear.lib.RearHair.id,
				b = appear.lib.RearHair.innerid
			};
			saveData.RearHair = rearHair;
			WorkerSpriteSaveData.Pair eyeBrow = new WorkerSpriteSaveData.Pair
			{
				a = (long)appear.lib.Eyebrow_Def.id,
				b = appear.lib.Eyebrow_Def.innerid
			};
			saveData.EyeBrow = eyeBrow;
			WorkerSpriteSaveData.Pair battleEyeBrow = new WorkerSpriteSaveData.Pair
			{
				a = (long)appear.lib.Eyebrow_Battle.id,
				b = appear.lib.Eyebrow_Battle.innerid
			};
			saveData.BattleEyeBrow = battleEyeBrow;
			WorkerSpriteSaveData.Pair panicEyeBrow = new WorkerSpriteSaveData.Pair
			{
				a = (long)appear.lib.Eyebrow_Panic.id,
				b = appear.lib.Eyebrow_Panic.innerid
			};
			saveData.PanicEyeBrow = panicEyeBrow;
			WorkerSpriteSaveData.Pair eye = new WorkerSpriteSaveData.Pair
			{
				a = (long)appear.lib.Eye_Def.id,
				b = appear.lib.Eye_Def.innerid
			};
			saveData.Eye = eye;
			WorkerSpriteSaveData.Pair eyePanic = new WorkerSpriteSaveData.Pair
			{
				a = (long)appear.lib.Eye_Panic.id,
				b = appear.lib.Eye_Panic.innerid
			};
			saveData.EyePanic = eyePanic;
			WorkerSpriteSaveData.Pair eyeDead = new WorkerSpriteSaveData.Pair
			{
				a = (long)appear.lib.Eye_Dead.id,
				b = appear.lib.Eye_Dead.innerid
			};
			saveData.EyeDead = eyeDead;
			WorkerSpriteSaveData.Pair mouth = new WorkerSpriteSaveData.Pair
			{
				a = (long)appear.lib.Mouth_Def.id,
				b = appear.lib.Mouth_Def.innerid
			};
			saveData.Mouth = mouth;
			WorkerSpriteSaveData.Pair battleMouth = new WorkerSpriteSaveData.Pair
			{
				a = (long)appear.lib.Mouth_Battle.id,
				b = appear.lib.Mouth_Battle.innerid
			};
			saveData.BattleMouth = battleMouth;
			saveData.HairColor = new WorkerSpriteSaveData.ColorData(appear.HairColor);
			saveData.EyeColor = new WorkerSpriteSaveData.ColorData(Color.white);
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
		this.LoadWorkerSpriteSetBySaveData(set);
	}

	// Token: 0x06005C39 RID: 23609 RVA: 0x002097C0 File Offset: 0x002079C0
	public void GetRandomBasicData(WorkerSprite.WorkerSprite set, bool containCustoms = true)
	{
		WorkerSpriteSaveData saveData = set.saveData;
		bool flag = false;
		WorkerBasicSprite workerBasicSprite = null;
		try
		{
			int b = 0;
			long id = 0L;
			if (this.basicData.GetData(BasicSpriteRegion.EYE_DEFAULT, out workerBasicSprite))
			{
				WorkerSpriteSaveData.Pair pair = new WorkerSpriteSaveData.Pair();
				SpriteResourceLoadData randomData = workerBasicSprite.GetRandomData(false);
				pair.a = (long)randomData.id;
				pair.b = UnityEngine.Random.Range(0, randomData.count);
				saveData.Eye = pair;
			}
			if (this.basicData.GetData(BasicSpriteRegion.EYE_DEAD, out workerBasicSprite))
			{
				WorkerSpriteSaveData.Pair pair2 = new WorkerSpriteSaveData.Pair();
				SpriteResourceLoadData randomData2 = workerBasicSprite.GetRandomData(false);
				pair2.a = (long)randomData2.id;
				pair2.b = UnityEngine.Random.Range(0, randomData2.count);
				saveData.EyeDead = pair2;
			}
			if (this.basicData.GetData(BasicSpriteRegion.EYE_CLOSED, out workerBasicSprite))
			{
				WorkerSpriteSaveData.Pair pair3 = new WorkerSpriteSaveData.Pair();
				SpriteResourceLoadData randomData3 = workerBasicSprite.GetRandomData(false);
				pair3.a = (long)randomData3.id;
				pair3.b = UnityEngine.Random.Range(0, randomData3.count);
				saveData.EyeClose = pair3;
			}
			if (this.basicData.GetData(BasicSpriteRegion.EYEBROW, out workerBasicSprite))
			{
				WorkerSpriteSaveData.Pair pair4 = new WorkerSpriteSaveData.Pair();
				SpriteResourceLoadData randomData4 = workerBasicSprite.GetRandomData(false);
				id = (pair4.a = (long)randomData4.id);
				b = (pair4.b = UnityEngine.Random.Range(0, randomData4.count));
				saveData.EyeBrow = pair4;
			}
			if (this.basicData.GetData(BasicSpriteRegion.EYE_PANIC, out workerBasicSprite))
			{
				WorkerSpriteSaveData.Pair pair5 = new WorkerSpriteSaveData.Pair();
				SpriteResourceLoadData randomData5 = workerBasicSprite.GetRandomData(false);
				pair5.a = (long)randomData5.id;
				pair5.b = UnityEngine.Random.Range(0, randomData5.count);
				saveData.EyePanic = pair5;
			}
			if (this.basicData.GetData(BasicSpriteRegion.MOUTH, out workerBasicSprite))
			{
				WorkerSpriteSaveData.Pair pair6 = new WorkerSpriteSaveData.Pair();
				SpriteResourceLoadData randomData6 = workerBasicSprite.GetRandomData(false);
				pair6.a = (long)randomData6.id;
				pair6.b = UnityEngine.Random.Range(0, randomData6.count);
				saveData.Mouth = pair6;
			}
			if (this.basicData.GetData(BasicSpriteRegion.HAIR_FRONT, out workerBasicSprite))
			{
				WorkerSpriteSaveData.Pair pair7 = new WorkerSpriteSaveData.Pair();
				SpriteResourceLoadData spriteResourceLoadData;
				if (UnityEngine.Random.value <= 0.02f)
				{
					spriteResourceLoadData = workerBasicSprite.GetData(0L);
					flag = true;
				}
				else
				{
					spriteResourceLoadData = workerBasicSprite.GetRandomDataWithExclude(new List<int>(0), false);
				}
				pair7.a = (long)spriteResourceLoadData.id;
				pair7.b = UnityEngine.Random.Range(0, spriteResourceLoadData.count);
				saveData.FrontHair = pair7;
			}
			if (this.basicData.GetData(BasicSpriteRegion.HAIR_REAR, out workerBasicSprite))
			{
				WorkerSpriteSaveData.Pair pair8 = new WorkerSpriteSaveData.Pair();
				SpriteResourceLoadData spriteResourceLoadData2;
				if (UnityEngine.Random.value <= 0.5f || flag)
				{
					spriteResourceLoadData2 = workerBasicSprite.GetData(0L);
				}
				else
				{
					spriteResourceLoadData2 = workerBasicSprite.GetRandomDataWithExclude(new List<int>(0), false);
				}
				pair8.a = (long)spriteResourceLoadData2.id;
				pair8.b = UnityEngine.Random.Range(0, spriteResourceLoadData2.count);
				saveData.RearHair = pair8;
			}
			if (this.basicData.GetData(BasicSpriteRegion.MOUTH_BATTLE, out workerBasicSprite))
			{
				WorkerSpriteSaveData.Pair pair9 = new WorkerSpriteSaveData.Pair();
				SpriteResourceLoadData randomData7 = workerBasicSprite.GetRandomData(false);
				pair9.a = (long)randomData7.id;
				pair9.b = UnityEngine.Random.Range(0, randomData7.count);
				saveData.BattleMouth = pair9;
			}
			if (this.basicData.GetData(BasicSpriteRegion.EYEBROW_BATTLE, out workerBasicSprite))
			{
				WorkerSpriteSaveData.Pair pair10 = new WorkerSpriteSaveData.Pair();
				SpriteResourceLoadData data = workerBasicSprite.GetData(id);
				pair10.a = (long)data.id;
				pair10.b = b;
				saveData.BattleEyeBrow = pair10;
			}
			if (this.basicData.GetData(BasicSpriteRegion.MOUTH_PANIC, out workerBasicSprite))
			{
				WorkerSpriteSaveData.Pair pair11 = new WorkerSpriteSaveData.Pair();
				SpriteResourceLoadData randomData8 = workerBasicSprite.GetRandomData(false);
				pair11.a = (long)randomData8.id;
				pair11.b = UnityEngine.Random.Range(0, randomData8.count);
				saveData.PanicMouth = pair11;
			}
			if (this.basicData.GetData(BasicSpriteRegion.EYEBROW_PANIC, out workerBasicSprite))
			{
				WorkerSpriteSaveData.Pair pair12 = new WorkerSpriteSaveData.Pair();
				SpriteResourceLoadData randomData9 = workerBasicSprite.GetRandomData(false);
				pair12.a = (long)randomData9.id;
				pair12.b = UnityEngine.Random.Range(0, randomData9.count);
				saveData.PanicEyeBrow = pair12;
			}
			saveData.HairColor = new WorkerSpriteSaveData.ColorData(this.GetRandomColor());
			saveData.EyeColor = new WorkerSpriteSaveData.ColorData(Color.white);
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
		this.LoadWorkerSpriteSetBySaveData(set);
	}

	// Token: 0x06005C3A RID: 23610 RVA: 0x00209C34 File Offset: 0x00207E34
	public void GetUniqueBasicData(WorkerSprite.WorkerSprite set, UniqueCreditAgentInfo info)
	{
		WorkerSpriteSaveData saveData = set.saveData;
		WorkerBasicSprite workerBasicSprite = null;
		try
		{
			if (this.basicData.GetData(BasicSpriteRegion.EYE_DEFAULT, out workerBasicSprite))
			{
				WorkerSpriteSaveData.Pair pair = new WorkerSpriteSaveData.Pair();
				SpriteResourceLoadData uniqueCustomData = workerBasicSprite.GetUniqueCustomData();
				pair.a = (long)uniqueCustomData.id;
				pair.b = info.appearanceId;
				saveData.Eye = pair;
			}
			if (this.basicData.GetData(BasicSpriteRegion.EYE_DEAD, out workerBasicSprite))
			{
				WorkerSpriteSaveData.Pair pair2 = new WorkerSpriteSaveData.Pair();
				SpriteResourceLoadData randomData = workerBasicSprite.GetRandomData(false);
				pair2.a = (long)randomData.id;
				pair2.b = UnityEngine.Random.Range(0, randomData.count);
				saveData.EyeDead = pair2;
			}
			if (this.basicData.GetData(BasicSpriteRegion.EYE_CLOSED, out workerBasicSprite))
			{
				WorkerSpriteSaveData.Pair pair3 = new WorkerSpriteSaveData.Pair();
				SpriteResourceLoadData randomData2 = workerBasicSprite.GetRandomData(false);
				pair3.a = (long)randomData2.id;
				pair3.b = UnityEngine.Random.Range(0, randomData2.count);
				saveData.EyeClose = pair3;
			}
			if (this.basicData.GetData(BasicSpriteRegion.EYEBROW, out workerBasicSprite))
			{
				WorkerSpriteSaveData.Pair pair4 = new WorkerSpriteSaveData.Pair();
				SpriteResourceLoadData uniqueCustomData2 = workerBasicSprite.GetUniqueCustomData();
				pair4.a = (long)uniqueCustomData2.id;
				Debug.Log(pair4.a);
				Debug.Log(uniqueCustomData2.count);
				pair4.b = info.appearanceId;
				saveData.EyeBrow = pair4;
			}
			if (this.basicData.GetData(BasicSpriteRegion.EYEBROW_BATTLE, out workerBasicSprite))
			{
				WorkerSpriteSaveData.Pair pair5 = new WorkerSpriteSaveData.Pair();
				SpriteResourceLoadData randomData3 = workerBasicSprite.GetRandomData(false);
				pair5.a = (long)randomData3.id;
				pair5.b = UnityEngine.Random.Range(0, randomData3.count);
				saveData.BattleEyeBrow = pair5;
			}
			if (this.basicData.GetData(BasicSpriteRegion.MOUTH, out workerBasicSprite))
			{
				WorkerSpriteSaveData.Pair pair6 = new WorkerSpriteSaveData.Pair();
				SpriteResourceLoadData uniqueCustomData3 = workerBasicSprite.GetUniqueCustomData();
				pair6.a = (long)uniqueCustomData3.id;
				pair6.b = info.appearanceId;
				saveData.Mouth = pair6;
			}
			if (this.basicData.GetData(BasicSpriteRegion.MOUTH_PANIC, out workerBasicSprite))
			{
				WorkerSpriteSaveData.Pair pair7 = new WorkerSpriteSaveData.Pair();
				SpriteResourceLoadData randomData4 = workerBasicSprite.GetRandomData(false);
				pair7.a = (long)randomData4.id;
				pair7.b = UnityEngine.Random.Range(0, randomData4.count);
				saveData.PanicMouth = pair7;
			}
			if (this.basicData.GetData(BasicSpriteRegion.EYEBROW_PANIC, out workerBasicSprite))
			{
				WorkerSpriteSaveData.Pair pair8 = new WorkerSpriteSaveData.Pair();
				SpriteResourceLoadData randomData5 = workerBasicSprite.GetRandomData(false);
				pair8.a = (long)randomData5.id;
				pair8.b = UnityEngine.Random.Range(0, randomData5.count);
				saveData.PanicEyeBrow = pair8;
			}
			if (this.basicData.GetData(BasicSpriteRegion.HAIR_FRONT, out workerBasicSprite))
			{
				WorkerSpriteSaveData.Pair pair9 = new WorkerSpriteSaveData.Pair();
				SpriteResourceLoadData uniqueCustomData4 = workerBasicSprite.GetUniqueCustomData();
				pair9.a = (long)uniqueCustomData4.id;
				pair9.b = info.appearanceId;
				saveData.FrontHair = pair9;
			}
			if (this.basicData.GetData(BasicSpriteRegion.HAIR_REAR, out workerBasicSprite))
			{
				WorkerSpriteSaveData.Pair pair10 = new WorkerSpriteSaveData.Pair();
				if (info.rearHairId != -1)
				{
					SpriteResourceLoadData spriteResourceLoadData = workerBasicSprite.GetUniqueCustomData();
					pair10.a = (long)spriteResourceLoadData.id;
					pair10.b = info.rearHairId;
				}
				else
				{
					SpriteResourceLoadData spriteResourceLoadData = workerBasicSprite.GetData(0L);
					pair10.a = (long)spriteResourceLoadData.id;
					pair10.b = UnityEngine.Random.Range(0, spriteResourceLoadData.count);
				}
				saveData.RearHair = pair10;
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
		saveData.HairColor = new WorkerSpriteSaveData.ColorData(Color.white);
		this.LoadWorkerSpriteSetBySaveData(set);
	}

	// Token: 0x06005C3B RID: 23611 RVA: 0x00209FD4 File Offset: 0x002081D4
	public void LoadWorkerSpriteSetBySaveData(WorkerSprite.WorkerSprite set)
	{
		WorkerSpriteSaveData saveData = set.saveData;
		Sprite eye = null;
		Sprite eyePanic = null;
		Sprite eyeDead = null;
		Sprite eyeClose = null;
		Sprite eyeBrow = null;
		Sprite battleEyeBrow = null;
		Sprite mouth = null;
		Sprite frontHair = null;
		Sprite rearHair = null;
		Sprite battleMouth = null;
		Sprite panicMouth = null;
		Sprite panicEyeBrow = null;
		WorkerBasicSprite workerBasicSprite = null;
		if (this.basicData.GetData(BasicSpriteRegion.EYE_DEFAULT, out workerBasicSprite))
		{
			SpriteResourceLoadData data = workerBasicSprite.GetData(saveData.Eye.a);
			eye = data.GetSprite(saveData.Eye.b);
		}
		if (this.basicData.GetData(BasicSpriteRegion.EYE_DEAD, out workerBasicSprite))
		{
			SpriteResourceLoadData data2 = workerBasicSprite.GetData(saveData.EyeDead.a);
			eyeDead = data2.GetSprite(saveData.EyeDead.b);
		}
		if (this.basicData.GetData(BasicSpriteRegion.EYE_CLOSED, out workerBasicSprite))
		{
			SpriteResourceLoadData data3 = workerBasicSprite.GetData(saveData.EyeClose.a);
			eyeClose = data3.GetSprite(saveData.EyeClose.b);
		}
		if (this.basicData.GetData(BasicSpriteRegion.EYEBROW, out workerBasicSprite))
		{
			SpriteResourceLoadData data4 = workerBasicSprite.GetData(saveData.EyeBrow.a);
			eyeBrow = data4.GetSprite(saveData.EyeBrow.b);
		}
		if (this.basicData.GetData(BasicSpriteRegion.EYE_PANIC, out workerBasicSprite))
		{
			SpriteResourceLoadData data5 = workerBasicSprite.GetData(saveData.EyePanic.a);
			eyePanic = data5.GetSprite(saveData.EyePanic.b);
		}
		if (this.basicData.GetData(BasicSpriteRegion.MOUTH, out workerBasicSprite))
		{
			SpriteResourceLoadData data6 = workerBasicSprite.GetData(saveData.Mouth.a);
			mouth = data6.GetSprite(saveData.Mouth.b);
		}
		if (this.basicData.GetData(BasicSpriteRegion.HAIR_FRONT, out workerBasicSprite))
		{
			SpriteResourceLoadData data7 = workerBasicSprite.GetData(saveData.FrontHair.a);
			frontHair = data7.GetSprite(saveData.FrontHair.b);
		}
		if (this.basicData.GetData(BasicSpriteRegion.HAIR_REAR, out workerBasicSprite))
		{
			SpriteResourceLoadData data8 = workerBasicSprite.GetData(saveData.RearHair.a);
			rearHair = data8.GetSprite(saveData.RearHair.b);
		}
		if (this.basicData.GetData(BasicSpriteRegion.MOUTH_BATTLE, out workerBasicSprite))
		{
			SpriteResourceLoadData data9 = workerBasicSprite.GetData(saveData.BattleMouth.a);
			battleMouth = data9.GetSprite(saveData.BattleMouth.b);
		}
		if (this.basicData.GetData(BasicSpriteRegion.EYEBROW_BATTLE, out workerBasicSprite))
		{
			SpriteResourceLoadData data10 = workerBasicSprite.GetData(saveData.BattleEyeBrow.a);
			battleEyeBrow = data10.GetSprite(saveData.BattleEyeBrow.b);
		}
		if (this.basicData.GetData(BasicSpriteRegion.MOUTH_PANIC, out workerBasicSprite))
		{
			SpriteResourceLoadData data11 = workerBasicSprite.GetData(saveData.PanicMouth.a);
			panicMouth = data11.GetSprite(saveData.PanicMouth.b);
		}
		if (this.basicData.GetData(BasicSpriteRegion.EYEBROW_PANIC, out workerBasicSprite))
		{
			SpriteResourceLoadData data12 = workerBasicSprite.GetData(saveData.PanicEyeBrow.a);
			panicEyeBrow = data12.GetSprite(saveData.PanicEyeBrow.b);
		}
		set.Eye = eye;
		set.EyeBrow = eyeBrow;
		set.EyePanic = eyePanic;
		set.EyeDead = eyeDead;
		set.EyeClose = eyeClose;
		set.Mouth = mouth;
		set.FrontHair = frontHair;
		set.RearHair = rearHair;
		set.BattleMouth = battleMouth;
		set.PanicMouth = panicMouth;
		set.PanicEyeBrow = panicEyeBrow;
		set.BattleEyeBrow = battleEyeBrow;
		set.HairColor = saveData.HairColor.GetColor();
		set.EyeColor = saveData.EyeColor.GetColor();
	}

	// Token: 0x06005C3C RID: 23612 RVA: 0x0020A350 File Offset: 0x00208550
	public Sprite LoadBasicData(BasicSpriteRegion region, WorkerSpriteSaveData.Pair pair)
	{
		Sprite result;
		try
		{
			WorkerBasicSprite workerBasicSprite = null;
			if (this.basicData.GetData(region, out workerBasicSprite))
			{
				SpriteResourceLoadData data = workerBasicSprite.GetData(pair.a);
				Sprite sprite = data.GetSprite(pair.b);
				result = sprite;
			}
			else
			{
				result = null;
			}
		}
		catch (Exception ex)
		{
			result = null;
		}
		return result;
	}

	// Token: 0x06005C3D RID: 23613 RVA: 0x0020A3B8 File Offset: 0x002085B8
	public void GetArmorData(int id, ref WorkerSprite.WorkerSprite set)
	{ // <Patch??>
		WorkerEquipmentSprite workerEquipmentSprite = null;
		this.equipData.GetData(EquipmentSpriteRegion.CLOTHES, out workerEquipmentSprite);
		if (id < 100000000)
		{
			id += 100000000;
		}
		if (!(workerEquipmentSprite as WorkerClothesSprite).lib.ContainsKey(id))
		{
			SpriteResourceLoadData clothesSet = this.GetClothesSet(0);
			AtlasLoadData atlasLoadData = new AtlasLoadData();
			atlasLoadData.sprites.Clear();
			bool flag = false;
			for (int i = 0; i < (clothesSet as AtlasLoadData).sprites.Count; i++)
			{
				Texture2D texture2D = new Texture2D(2, 2);
				foreach (DirectoryInfo dir in Add_On.instance.DirList)
				{
					flag = false;
					DirectoryInfo directoryInfo = EquipmentDataLoader.CheckNamedDir(dir, "Equipment");
					if (directoryInfo != null)
					{
						if (!Directory.Exists(directoryInfo.FullName + "/Sprite/Armor"))
						{
							continue;
						}
						DirectoryInfo directoryInfo2 = new DirectoryInfo(directoryInfo.FullName + "/Sprite/Armor");
						if (directoryInfo2.GetFiles().Length != 0)
						{
							FileInfo[] files = directoryInfo2.GetFiles();
							for (int j = 0; j < files.Length; j++)
							{
								if (files[j].Name == (id - 100000000).ToString() + ".png")
								{
									texture2D.LoadImage(File.ReadAllBytes(files[j].FullName));
									flag = true;
									break;
								}
							}
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (flag)
				{
					Sprite sprite = (clothesSet as AtlasLoadData).sprites[i];
					Vector2 vector = new Vector2(sprite.pivot.x / sprite.rect.width, sprite.pivot.y / sprite.rect.height);
					Sprite sprite2 = Sprite.Create(texture2D, sprite.rect, vector, sprite.pixelsPerUnit, 0U, SpriteMeshType.FullRect, sprite.border, true);
					sprite2.name = sprite.name;
					atlasLoadData.sprites.Add(sprite2);
					atlasLoadData.count++;
				}
			}
			if (flag)
			{
				(workerEquipmentSprite as WorkerClothesSprite).lib.Add(id, atlasLoadData);
			}
			else
			{
				(workerEquipmentSprite as WorkerClothesSprite).lib.Add(id, (workerEquipmentSprite as WorkerClothesSprite).lib[id - 100000000]);
			}
		}
		SpriteResourceLoadData clothesSet2 = this.GetClothesSet(id);
		if (clothesSet2 != null)
		{
			set.Armor.SetSprite(clothesSet2);
		}
	}

	// Token: 0x06005C3E RID: 23614 RVA: 0x000498E8 File Offset: 0x00047AE8
	public Color GetRandomColor()
	{
		if (this.workerColor.Count == 0)
		{
			return Color.white;
		}
		return this.workerColor[UnityEngine.Random.Range(0, this.workerColor.Count)].color;
	}

	// Token: 0x06005C3F RID: 23615 RVA: 0x0020A64C File Offset: 0x0020884C
	public Sprite GetSefiraSymbol(SefiraEnum sefira, int level = 1)
	{ // <Mod>
		switch (level)
		{
		case 2:
			return this.SefiraSymbol2[(int)sefira];
		case 3:
			return this.SefiraSymbol3[(int)sefira];
		case 4:
		case 5:
		case 6:
			return this.SefiraSymbol4[(int)sefira];
		default:
			return this.SefiraSymbol[(int)sefira];
		}
	}

	// Token: 0x06005C40 RID: 23616 RVA: 0x0020A6A8 File Offset: 0x002088A8
	public Sprite GetSefiraSymbol(AgentModel agent)
	{
		Sefira currentSefira = agent.GetCurrentSefira();
		if (currentSefira == null)
		{
			return null;
		}
		return this.GetSefiraSymbol(currentSefira.sefiraEnum, agent.GetContinuousServiceLevel());
	}

	// Token: 0x06005C41 RID: 23617 RVA: 0x0020A6D8 File Offset: 0x002088D8
	public void GetAgentArmor(ref WorkerSprite.WorkerSprite set)
	{
		SpriteResourceLoadData clothesSet = this.GetClothesSet(0);
		if (clothesSet != null)
		{
			set.Armor.SetSprite(clothesSet);
			set.SetArmorColor = false;
		}
	}

	// Token: 0x06005C42 RID: 23618 RVA: 0x0020A708 File Offset: 0x00208908
	public void GetOfficerArmror(ref WorkerSprite.WorkerSprite set, SefiraEnum sefira)
	{
		SpriteResourceLoadData clothesSet = this.GetClothesSet(this.GetOfficerArmor(sefira));
		if (clothesSet != null)
		{
			set.Armor.SetSprite(clothesSet);
			set.SetArmorColor = false;
		}
	}

	// Token: 0x06005C43 RID: 23619 RVA: 0x0020A740 File Offset: 0x00208940
	public Sprite[] GetFistSprite(int id)
	{
		WorkerEquipmentSprite workerEquipmentSprite = null;
		Sprite[] array = new Sprite[2];
		array[0] = (array[1] = null);
		if (this.equipData.GetData(EquipmentSpriteRegion.WEAPON, out workerEquipmentSprite))
		{
			WorkerWeaponSprite workerWeaponSprite = workerEquipmentSprite as WorkerWeaponSprite;
			List<SpriteResourceLoadData> data;
			if ((data = workerWeaponSprite.GetData(7)) != null)
			{
				try
				{
					SpriteResourceLoadData spriteResourceLoadData = data[0];
					if (spriteResourceLoadData.type == SpriteResourceType.ATLAS)
					{
						AtlasLoadData atlasLoadData = spriteResourceLoadData as AtlasLoadData;
						int num = atlasLoadData.count / 2;
						array[0] = atlasLoadData.sprites[id * 2];
						array[1] = atlasLoadData.sprites[id * 2 + 1];
						Debug.Log("Successfully selected");
					}
				}
				catch (IndexOutOfRangeException ex)
				{
					Debug.Log("Couldn't find fist sprite about ::" + id);
					return null;
				}
				return array;
			}
		}
		return array;
	}

	// <Mod>
	public Sprite[] GetFistSprite(LobotomyBaseMod.KeyValuePairSS SS)
	{
		if (this.FistWeaponSpriteMod == null)
		{
			this.FistWeaponSpriteMod = new Dictionary<LobotomyBaseMod.KeyValuePairSS, Sprite[]>();
		}
		if (this.FistWeaponSpriteMod.ContainsKey(SS))
		{
			return this.FistWeaponSpriteMod[SS];
		}
		WeaponClassType type = WeaponClassType.FIST;
		WorkerEquipmentSprite workerEquipmentSprite2 = null;
		if (this.equipData.GetData(EquipmentSpriteRegion.WEAPON, out workerEquipmentSprite2))
		{
			WorkerWeaponSprite workerWeaponSprite2 = workerEquipmentSprite2 as WorkerWeaponSprite;
			FieldInfo field2 = typeof(WorkerWeaponSprite.WeaponDatabase).GetField("data", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			Dictionary<string, Sprite> dictionary2 = (Dictionary<string, Sprite>)field2.GetValue(workerWeaponSprite2.GetDb((int)type));
			Sprite sprite4 = null;
			Sprite sprite5 = null;
			if (!dictionary2.ContainsKey(SS.value))
			{
				workerWeaponSprite2.GetDb((int)type).GetSprite("Fist_Set_01_0", out sprite4);
				workerWeaponSprite2.GetDb((int)type).GetSprite("Fist_Set_01_1", out sprite5);
				Texture2D texture2D = new Texture2D(sprite4.texture.width, sprite4.texture.height);
				ModInfo modInfo_patch = Add_On.instance.ModList.Find((ModInfo x) => x.modid == SS.key);
				if (modInfo_patch != null)
				{
					DirectoryInfo directoryInfo3 = EquipmentDataLoader.CheckNamedDir(modInfo_patch.modpath, "Equipment");
					if (directoryInfo3 != null)
					{
						if (Directory.Exists(directoryInfo3.FullName + "/Sprite/Weapon"))
						{
							DirectoryInfo directoryInfo4 = new DirectoryInfo(directoryInfo3.FullName + "/Sprite/Weapon");
							if (directoryInfo4.GetFiles().Length != 0)
							{
								FileInfo[] files2 = directoryInfo4.GetFiles();
								for (int j = 0; j < files2.Length; j++)
								{
									if (files2[j].Name == SS.value + ".png")
									{
										texture2D.LoadImage(File.ReadAllBytes(files2[j].FullName));
										break;
									}
								}
							}
						}
					}
					Vector2 vector = new Vector2(sprite4.pivot.x / sprite4.rect.width, sprite4.pivot.y / sprite4.rect.height);
					Vector2 vector2 = new Vector2(sprite5.pivot.x / sprite5.rect.width, sprite5.pivot.y / sprite5.rect.height);
					Sprite sprite6 = Sprite.Create(texture2D, sprite4.rect, vector, sprite4.pixelsPerUnit, 0U, SpriteMeshType.FullRect, sprite4.border);
					sprite6.name = SS.value + "B";
					Sprite sprite7 = Sprite.Create(texture2D, sprite5.rect, vector2, sprite5.pixelsPerUnit, 0U, SpriteMeshType.FullRect, sprite5.border);
					sprite7.name = SS.value + "F";
					Sprite[] array = new Sprite[2];
					array[0] = sprite6;
					array[1] = sprite7;
					this.FistWeaponSpriteMod[SS] = array;
					return this.FistWeaponSpriteMod[SS];
				}
			}
		}
		return null;
	}

	// Token: 0x06005C44 RID: 23620 RVA: 0x00049921 File Offset: 0x00047B21
	public bool GetUniqueWeaponSpriteInfo(string name, out UniqueWeaponSpriteUnit unit)
	{ // <Patch>
		return GetUniqueWeaponSpriteInfo_Mod(new LobotomyBaseMod.KeyValuePairSS(string.Empty, name), out unit);
		/*
		this.GetWeaponSprite(WeaponClassType.SPECIAL, name);
		return this._uniqueWeaponDic.TryGetValue(name + "_AbcdcodeMade", out unit);*/
	}

	// Token: 0x06005C45 RID: 23621 RVA: 0x0020A824 File Offset: 0x00208A24
	public Sprite GetWeaponSprite(WeaponClassType type, string name)
	{ // <Patch>
		return this.GetWeaponSprite_Mod(type, new LobotomyBaseMod.KeyValuePairSS(string.Empty, name));
		/*
		Sprite result = null;
		try
		{
			WorkerEquipmentSprite workerEquipmentSprite = null;
			if (type == WeaponClassType.SPECIAL)
			{
				if (this._uniqueWeaponDic.ContainsKey(name))
				{
					UniqueWeaponSpriteUnit uniqueWeaponSpriteUnit = null;
					if (this._uniqueWeaponDic.TryGetValue(name + "_AbcdcodeMade", out uniqueWeaponSpriteUnit))
					{
						result = uniqueWeaponSpriteUnit.CommonSprite;
					}
					else
					{
						Texture2D texture = new Texture2D(256, 256);
						DirectoryInfo dir = null;
						bool flag = this.SpecialFindDir_Texture(name, ref dir, ref texture);
						this._uniqueWeaponDic.Add(name + "_AbcdcodeMade", this._uniqueWeaponDic[name].GetCopy());
						if (!flag)
						{
							result = this._uniqueWeaponDic[name + "_AbcdcodeMade"].CommonSprite;
							return result;
						}
						this.MakeNewUniqueWeaponSpriteUnit(name, this._uniqueWeaponDic[name + "_AbcdcodeMade"], dir, texture, out result);
					}
				}
				else
				{
					char[] separator = new char[]
					{
						'_'
					};
					string key = name.Split(separator)[0];
					if (this._uniqueWeaponDic.ContainsKey(key))
					{
						Texture2D texture2 = new Texture2D(256, 256);
						DirectoryInfo dir2 = null;
						bool flag2 = this.SpecialFindDir_Texture(name, ref dir2, ref texture2);
						this._uniqueWeaponDic.Add(name, this._uniqueWeaponDic[key].GetCopy());
						this._uniqueWeaponDic.Add(name + "_AbcdcodeMade", this._uniqueWeaponDic[key].GetCopy());
						if (!flag2)
						{
							result = this._uniqueWeaponDic[name + "_AbcdcodeMade"].CommonSprite;
							return result;
						}
						this.MakeNewUniqueWeaponSpriteUnit(name, this._uniqueWeaponDic[name + "_AbcdcodeMade"], dir2, texture2, out result);
					}
				}
			}
			else if (type != WeaponClassType.FIST)
			{
				if (this.equipData.GetData(EquipmentSpriteRegion.WEAPON, out workerEquipmentSprite))
				{
					WorkerWeaponSprite workerWeaponSprite = workerEquipmentSprite as WorkerWeaponSprite;
					FieldInfo field = typeof(WorkerWeaponSprite.WeaponDatabase).GetField("data", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					Dictionary<string, Sprite> dictionary = (Dictionary<string, Sprite>)field.GetValue(workerWeaponSprite.GetDb((int)type));
					Sprite sprite = null;
					try
					{
						if (!dictionary.ContainsKey(name))
						{
							if (type == WeaponClassType.SPEAR)
							{
								workerWeaponSprite.GetDb((int)type).GetSprite("Spear_Set_01_0", out sprite);
							}
							if (type == WeaponClassType.PISTOL)
							{
								workerWeaponSprite.GetDb((int)type).GetSprite("Pistol_Set_01_0", out sprite);
							}
							if (type == WeaponClassType.HAMMER)
							{
								workerWeaponSprite.GetDb((int)type).GetSprite("Hammer_Set_01_0", out sprite);
							}
							if (type == WeaponClassType.BOWGUN)
							{
								workerWeaponSprite.GetDb((int)type).GetSprite("BowGun_Set_01_0", out sprite);
							}
							if (type == WeaponClassType.CANNON)
							{
								workerWeaponSprite.GetDb((int)type).GetSprite("Cannon_Set_01_0", out sprite);
							}
							if (type == WeaponClassType.AXE)
							{
								workerWeaponSprite.GetDb((int)type).GetSprite("Axe_Set_01_0", out sprite);
							}
							if (type == WeaponClassType.KNIFE)
							{
								workerWeaponSprite.GetDb((int)type).GetSprite("Knife_Set_01_0", out sprite);
							}
							if (type == WeaponClassType.MACE)
							{
								workerWeaponSprite.GetDb((int)type).GetSprite("Mace_Set_01_0", out sprite);
							}
							if (type == WeaponClassType.RIFLE)
							{
								workerWeaponSprite.GetDb((int)type).GetSprite("Rifle_Set_01_0", out sprite);
							}
							Texture2D texture2D = new Texture2D(sprite.texture.width, sprite.texture.height);
							foreach (DirectoryInfo dir3 in Add_On.instance.DirList)
							{
								bool flag3 = false;
								DirectoryInfo directoryInfo = EquipmentDataLoader.CheckNamedDir(dir3, "Equipment");
								if (directoryInfo != null)
								{
									if (!Directory.Exists(directoryInfo.FullName + "/Sprite/Weapon"))
									{
										continue;
									}
									DirectoryInfo directoryInfo2 = new DirectoryInfo(directoryInfo.FullName + "/Sprite/Weapon");
									if (directoryInfo2.GetFiles().Length != 0)
									{
										FileInfo[] files = directoryInfo2.GetFiles();
										for (int i = 0; i < files.Length; i++)
										{
											if (files[i].Name == name + ".png")
											{
												texture2D.LoadImage(File.ReadAllBytes(files[i].FullName));
												flag3 = true;
												break;
											}
										}
									}
								}
								if (flag3)
								{
									break;
								}
							}
							Vector2 vector = new Vector2(sprite.pivot.x / sprite.rect.width, sprite.pivot.y / sprite.rect.height);
							Sprite sprite2 = Sprite.Create(texture2D, sprite.rect, vector, sprite.pixelsPerUnit, 0U, SpriteMeshType.FullRect, sprite.border);
							sprite2.name = name;
							dictionary.Add(name, sprite2);
							field.SetValue(workerWeaponSprite.GetDb((int)type), dictionary);
						}
						if (workerWeaponSprite.GetDb((int)type).GetSprite(name, out sprite))
						{
							return sprite;
						}
					}
					catch (Exception message)
					{
						Debug.LogError(message);
						return null;
					}
				}
				result = null;
			}
			else
			{
				Sprite[] fistSprite = this.GetFistSprite((int)float.Parse(name));
				if (fistSprite.Length <= 1)
				{
					result = null;
				}
				else
				{
					result = fistSprite[1];
				}
			}
		}
		catch (Exception ex)
		{
			File.WriteAllText(Application.dataPath + "/BaseMods/error.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			result = null;
		}
		return result;*/
	}

	// Token: 0x06005C46 RID: 23622 RVA: 0x0020AD78 File Offset: 0x00208F78
	public Sprite GetAttachmentSprite(EGOgiftAttachRegion region, string name)
	{ // <Mod> Fix Mouth 1 gifts
		WorkerEquipmentSprite workerEquipmentSprite = null;
		if (this.equipData.GetData(EquipmentSpriteRegion.ATTACHMENTS, out workerEquipmentSprite))
		{
			WorkerAttachmentSprite workerAttachmentSprite = workerEquipmentSprite as WorkerAttachmentSprite;
			FieldInfo field = typeof(WorkerAttachmentSprite.AttachDatabase).GetField("data", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			Dictionary<string, Sprite> dictionary = (Dictionary<string, Sprite>)field.GetValue(workerAttachmentSprite.GetDb((int)region));
			Sprite sprite = null;
			if (!dictionary.ContainsKey(name))
			{
				if (region == EGOgiftAttachRegion.BACK)
				{
					workerAttachmentSprite.GetDb((int)region).GetSprite("BossBirdWing", out sprite);
				}
				if (region == EGOgiftAttachRegion.BACK2)
				{
					workerAttachmentSprite.GetDb((int)region).GetSprite("DeathAngel_Wing", out sprite);
				}
				if (region == EGOgiftAttachRegion.BODY_UP)
				{
					workerAttachmentSprite.GetDb((int)region).GetSprite("Brooch_0_0", out sprite);
				}
				if (region == EGOgiftAttachRegion.EYE)
				{
					workerAttachmentSprite.GetDb((int)region).GetSprite("Glass_0", out sprite);
				}
				if (region == EGOgiftAttachRegion.FACE)
				{
					workerAttachmentSprite.GetDb((int)region).GetSprite("BigBadWolf_Scar", out sprite);
				}
				if (region == EGOgiftAttachRegion.HAIR)
				{
					workerAttachmentSprite.GetDb((int)region).GetSprite("Helmet_0_0", out sprite);
				}
				if (region == EGOgiftAttachRegion.HEAD)
				{
					workerAttachmentSprite.GetDb((int)region).GetSprite("Hat_0", out sprite);
				}
				if (region == EGOgiftAttachRegion.HEADBACK)
				{
					workerAttachmentSprite.GetDb((int)region).GetSprite("Halo_Glow", out sprite);
				}
				if (region == EGOgiftAttachRegion.MASK)
				{
					workerAttachmentSprite.GetDb((int)region).GetSprite("Mask_0", out sprite);
				}
				if (region == EGOgiftAttachRegion.MOUTH)
				{
					if (name.StartsWith("AltMouth."))
					{
						workerAttachmentSprite.GetDb((int)region).GetSprite("RedHood_2", out sprite);
					}
					else
					{
						workerAttachmentSprite.GetDb((int)region).GetSprite("Mouth_0_0", out sprite);
					}
				}
				if (region == EGOgiftAttachRegion.RIBBORN)
				{
					workerAttachmentSprite.GetDb((int)region).GetSprite("Ribborn_0_1", out sprite);
				}
				if (region == EGOgiftAttachRegion.RIGHTCHEEK)
				{
					workerAttachmentSprite.GetDb((int)region).GetSprite("SnowQueen_Gift", out sprite);
				}
				if (region == EGOgiftAttachRegion.RIGHTHAND)
				{
					workerAttachmentSprite.GetDb((int)region).GetSprite("Glove_0_0", out sprite);
				}
				Texture2D texture2D = new Texture2D(2, 2);
				bool flag = false;
				string fileName = name + ".png";
				if (name.StartsWith("AltMouth."))
				{
					fileName = name.Substring(9) + ".png";
				}
				foreach (DirectoryInfo dir in Add_On.instance.DirList)
				{
					DirectoryInfo directoryInfo = EquipmentDataLoader.CheckNamedDir(dir, "Equipment");
					if (directoryInfo != null)
					{
						if (!Directory.Exists(directoryInfo.FullName + "/Sprite/Attachment"))
						{
							continue;
						}
						DirectoryInfo directoryInfo2 = new DirectoryInfo(directoryInfo.FullName + "/Sprite/Attachment");
						if (directoryInfo2.GetFiles().Length != 0)
						{
							FileInfo[] files = directoryInfo2.GetFiles();
							for (int i = 0; i < files.Length; i++)
							{
								if (files[i].Name == fileName)
								{
									texture2D.LoadImage(File.ReadAllBytes(files[i].FullName));
									flag = true;
									break;
								}
							}
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (!flag)
				{
					return null;
				}
				Vector2 vector = new Vector2(sprite.pivot.x / sprite.rect.width, sprite.pivot.y / sprite.rect.height);
				Sprite sprite2 = Sprite.Create(texture2D, sprite.rect, vector, sprite.pixelsPerUnit, 0U, SpriteMeshType.FullRect, sprite.border, true);
				sprite2.name = sprite.name;
				dictionary.Add(name, sprite2);
				field.SetValue(workerAttachmentSprite.GetDb((int)region), dictionary);
			}
			if (workerAttachmentSprite.GetDb((int)region).GetSprite(name, out sprite))
			{
				return sprite;
			}
		}
		return null;
	}

	// Token: 0x06005C47 RID: 23623 RVA: 0x00049944 File Offset: 0x00047B44
	public Sprite GetWorkNoteSprite(int id)
	{
		if (id < 0 || id > 4)
		{
			return this.WorkNote[0];
		}
		return this.WorkNote[id - 1];
	}

	// Token: 0x06005C48 RID: 23624 RVA: 0x0020B0B8 File Offset: 0x002092B8
	public void LoadCustomSprites()
	{ // <Patch>
		DirectoryInfo directoryInfo = new DirectoryInfo(this.CustomFolderSrc);
		DirectoryInfo directoryInfo2 = new DirectoryInfo(this.CustomFaceSrc);
		DirectoryInfo directoryInfo3 = new DirectoryInfo(this.CustomHairSrc);
		DirectoryInfo directoryInfo4 = new DirectoryInfo(this.CustomEyeSrc);
		DirectoryInfo directoryInfo5 = new DirectoryInfo(this.CustomPanicEyeSrc);
		DirectoryInfo directoryInfo6 = new DirectoryInfo(this.CustomDeadEyeSrc);
		DirectoryInfo directoryInfo7 = new DirectoryInfo(this.CustomEyebrowSrc);
		DirectoryInfo directoryInfo8 = new DirectoryInfo(this.CustomBattleEyebrowSrc);
		DirectoryInfo directoryInfo9 = new DirectoryInfo(this.CustomPanicEyebrowSrc);
		DirectoryInfo directoryInfo10 = new DirectoryInfo(this.CustomMouthSrc);
		DirectoryInfo directoryInfo11 = new DirectoryInfo(this.CustomBattleMouthSrc);
		DirectoryInfo directoryInfo12 = new DirectoryInfo(this.CustomFrontHairSrc);
		DirectoryInfo directoryInfo13 = new DirectoryInfo(this.CustomRearHairSrc);
		if (!directoryInfo.Exists)
		{
			directoryInfo.Create();
			Debug.Log("Not Exist CustomFoldoer So Make Directory : " + this.CustomFolderSrc);
			directoryInfo2.Create();
			directoryInfo3.Create();
			directoryInfo4.Create();
			directoryInfo5.Create();
			directoryInfo6.Create();
			directoryInfo7.Create();
			directoryInfo8.Create();
			directoryInfo9.Create();
			directoryInfo10.Create();
			directoryInfo11.Create();
			directoryInfo12.Create();
			directoryInfo13.Create();
		}
		if (!directoryInfo2.Exists)
		{
			directoryInfo2.Create();
			Debug.Log("Not Exist CustomFaceFolder So Make Directory : " + this.CustomFaceSrc);
		}
		if (!directoryInfo3.Exists)
		{
			directoryInfo3.Create();
			if (!directoryInfo12.Exists)
			{
				directoryInfo12.Create();
			}
			if (!directoryInfo13.Exists)
			{
				directoryInfo13.Create();
			}
			Debug.Log("Not Exist CustomHairFolder So Make Directory : " + this.CustomHairSrc);
		}
		if (!directoryInfo4.Exists)
		{
			directoryInfo4.Create();
		}
		if (!directoryInfo5.Exists)
		{
			directoryInfo5.Create();
		}
		if (!directoryInfo6.Exists)
		{
			directoryInfo6.Create();
		}
		if (!directoryInfo7.Exists)
		{
			directoryInfo7.Create();
		}
		if (!directoryInfo8.Exists)
		{
			directoryInfo8.Create();
		}
		if (!directoryInfo9.Exists)
		{
			directoryInfo9.Create();
		}
		if (!directoryInfo10.Exists)
		{
			directoryInfo10.Create();
		}
		if (!directoryInfo11.Exists)
		{
			directoryInfo11.Create();
		}
		if (!directoryInfo12.Exists)
		{
			directoryInfo12.Create();
		}
		if (!directoryInfo13.Exists)
		{
			directoryInfo13.Create();
		}
		this.LoadCustomSprite(directoryInfo4, BasicSpriteRegion.EYE_DEFAULT, WorkerSpriteManager.SizeRef.Eye());
		this.LoadCustomSprite(directoryInfo5, BasicSpriteRegion.EYE_PANIC, WorkerSpriteManager.SizeRef.Eye());
		this.LoadCustomSprite(directoryInfo6, BasicSpriteRegion.EYE_DEAD, WorkerSpriteManager.SizeRef.Eye());
		this.LoadCustomSprite(directoryInfo7, BasicSpriteRegion.EYEBROW, WorkerSpriteManager.SizeRef.Eyebrow());
		this.LoadCustomSprite(directoryInfo8, BasicSpriteRegion.EYEBROW_BATTLE, WorkerSpriteManager.SizeRef.Eyebrow());
		this.LoadCustomSprite(directoryInfo9, BasicSpriteRegion.EYEBROW_PANIC, WorkerSpriteManager.SizeRef.Eyebrow());
		this.LoadCustomSprite(directoryInfo10, BasicSpriteRegion.MOUTH, WorkerSpriteManager.SizeRef.Mouth());
		this.LoadCustomSprite(directoryInfo11, BasicSpriteRegion.MOUTH_BATTLE, WorkerSpriteManager.SizeRef.Mouth());
		this.LoadCustomSprite(directoryInfo12, BasicSpriteRegion.HAIR_FRONT, WorkerSpriteManager.SizeRef.FrontHair());
		this.LoadCustomSprite(directoryInfo13, BasicSpriteRegion.HAIR_REAR, WorkerSpriteManager.SizeRef.RearHair());
		/*>*/
		foreach (DirectoryInfo dir in Add_On.instance.DirList)
		{
			this.LoadModSprites(dir);
		}
	}

	// Token: 0x06005C49 RID: 23625 RVA: 0x0020B39C File Offset: 0x0020959C
	public void LoadCustomSprite(DirectoryInfo di, BasicSpriteRegion region, WorkerSpriteManager.SizeRef size)
	{
		List<SpriteInfo> list = new List<SpriteInfo>();
		FileInfo[] files = di.GetFiles();
		WorkerBasicSprite workerBasicSprite = null;
		int num = 0;
		if (this.basicData.GetData(region, out workerBasicSprite))
		{
			num = workerBasicSprite.GetLastId() + 1;
			foreach (FileInfo fileInfo in files)
			{
				if (!fileInfo.FullName.Contains(".meta"))
				{
					byte[] data;
					try
					{
						data = File.ReadAllBytes(fileInfo.FullName);
					}
					catch (FileNotFoundException ex)
					{
						Debug.Log("Couldn't find " + fileInfo.FullName);
						goto IL_188;
					}
					Texture2D texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, true);
					texture2D.LoadImage(data);
					int targetWidth = (int)size.x;
					int targetHeight = (int)size.y;
					Texture2D texture2D2 = WorkerSpriteManager.ScaleTexture(texture2D, targetWidth, targetHeight, true);
					Sprite sprite = Sprite.Create(texture2D2, new Rect(0f, 0f, (float)texture2D2.width, (float)texture2D2.height), WorkerSpriteManager.pivot, 100f, 0U, SpriteMeshType.FullRect);
					sprite.name = fileInfo.Name.Substring(0, fileInfo.Name.LastIndexOf('.'));
					list.Add(new SpriteInfo(fileInfo.FullName, sprite)
					{
						isCustom = true
					});
					SpriteLoadData spriteLoadData = new SpriteLoadData
					{
						type = SpriteResourceType.SPRITE,
						src = fileInfo.FullName,
						id = num,
						sprite = sprite,
						count = 1
					};
					spriteLoadData.isCustom = true;
					workerBasicSprite.loadedData.Add(spriteLoadData);
					num++;
				}
				IL_188:;
			}
		}
	}

	// Token: 0x06005C4A RID: 23626 RVA: 0x0020B554 File Offset: 0x00209754
	public static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight, bool mipmap = true)
	{
		Texture2D texture2D = new Texture2D(targetWidth, targetHeight, source.format, mipmap);
		Color[] pixels = texture2D.GetPixels(0);
		float num = 1f / (float)targetWidth;
		float num2 = 1f / (float)targetHeight;
		for (int i = 0; i < pixels.Length; i++)
		{
			pixels[i] = source.GetPixelBilinear(num * ((float)i % (float)targetWidth), num2 * Mathf.Floor((float)(i / targetWidth)));
		}
		texture2D.SetPixels(pixels, 0);
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x06005C4B RID: 23627 RVA: 0x0004996F File Offset: 0x00047B6F
	static WorkerSpriteManager()
	{
	}

	// Token: 0x06005C4C RID: 23628 RVA: 0x0020B5DC File Offset: 0x002097DC
	public void MakeNewUniqueWeaponSpriteUnit(string name, UniqueWeaponSpriteUnit unit, DirectoryInfo dir, Texture2D texture, out Sprite result)
	{
		Sprite commonSprite = unit.CommonSprite;
		Vector2 vector = new Vector2(commonSprite.pivot.x / commonSprite.rect.width, commonSprite.pivot.y / commonSprite.rect.height);
		Sprite sprite = Sprite.Create(texture, commonSprite.rect, vector, commonSprite.pixelsPerUnit, 0U, SpriteMeshType.FullRect, commonSprite.border);
		sprite.name = name;
		result = sprite;
		if (unit.sprites != null && unit.sprites.Count > 0)
		{
			int num = 0;
			foreach (UniqueWeaponSprite uniqueWeaponSprite in unit.sprites)
			{
				Sprite sprite2 = uniqueWeaponSprite.sprite;
				Texture2D texture2D = new Texture2D(2, 2);
				byte[] data = File.ReadAllBytes(string.Concat(new object[]
				{
					dir.FullName,
					"/",
					name,
					"_",
					num,
					".png"
				}));
				texture2D.LoadImage(data);
				Vector2 vector2 = new Vector2(sprite2.pivot.x / sprite2.rect.width, sprite2.pivot.y / sprite2.rect.height);
				Sprite sprite3 = Sprite.Create(texture2D, sprite2.rect, vector2, sprite2.pixelsPerUnit, 0U, SpriteMeshType.FullRect, sprite2.border);
				unit.sprites[num].sprite = sprite3;
				num++;
			}
		}
		unit.GetType().GetField("_commonSprite", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).SetValue(unit, sprite);
	}

	// Token: 0x06005C4D RID: 23629 RVA: 0x0020B7A8 File Offset: 0x002099A8
	public bool SpecialFindDir_Texture(string name, ref DirectoryInfo info, ref Texture2D tex)
	{ // <Patch>
		return this.SpecialFindDir_Texture_Mod(string.Empty, name, ref info, ref tex);
		/*
		bool flag = false;
		foreach (DirectoryInfo dir in Add_On.instance.DirList)
		{
			DirectoryInfo directoryInfo = EquipmentDataLoader.CheckNamedDir(dir, "Equipment");
			if (directoryInfo != null)
			{
				if (!Directory.Exists(directoryInfo.FullName + "/Sprite/Weapon"))
				{
					continue;
				}
				DirectoryInfo directoryInfo2 = new DirectoryInfo(directoryInfo.FullName + "/Sprite/Weapon");
				if (directoryInfo2.GetFiles().Length != 0)
				{
					FileInfo[] files = directoryInfo2.GetFiles();
					for (int i = 0; i < files.Length; i++)
					{
						if (files[i].Name == name + ".png")
						{
							info = directoryInfo2;
							tex.LoadImage(File.ReadAllBytes(files[i].FullName));
							flag = true;
							break;
						}
					}
				}
			}
			if (flag)
			{
				break;
			}
		}
		return flag;*/
	}

	// <Patch>
	public void LoadModSprites(DirectoryInfo dir)
	{
		this.CurModPath = dir.FullName;
		DirectoryInfo directoryInfo = new DirectoryInfo(this.ModFolderSrc);
		DirectoryInfo directoryInfo2 = new DirectoryInfo(this.ModEyeSrc);
		DirectoryInfo directoryInfo3 = new DirectoryInfo(this.ModPanicEyeSrc);
		DirectoryInfo directoryInfo4 = new DirectoryInfo(this.ModDeadEyeSrc);
		DirectoryInfo directoryInfo5 = new DirectoryInfo(this.ModEyebrowSrc);
		DirectoryInfo directoryInfo6 = new DirectoryInfo(this.ModBattleEyebrowSrc);
		DirectoryInfo directoryInfo7 = new DirectoryInfo(this.ModPanicEyebrowSrc);
		DirectoryInfo directoryInfo8 = new DirectoryInfo(this.ModMouthSrc);
		DirectoryInfo directoryInfo9 = new DirectoryInfo(this.ModBattleMouthSrc);
		DirectoryInfo directoryInfo10 = new DirectoryInfo(this.ModFrontHairSrc);
		DirectoryInfo directoryInfo11 = new DirectoryInfo(this.ModRearHairSrc);
		if (!directoryInfo.Exists)
		{
			return;
		}
		if (directoryInfo2.Exists)
		{
			this.LoadCustomSprite(directoryInfo2, BasicSpriteRegion.EYE_DEFAULT, WorkerSpriteManager.SizeRef.Eye());
		}
		if (directoryInfo3.Exists)
		{
			this.LoadCustomSprite(directoryInfo3, BasicSpriteRegion.EYE_PANIC, WorkerSpriteManager.SizeRef.Eye());
		}
		if (directoryInfo4.Exists)
		{
			this.LoadCustomSprite(directoryInfo4, BasicSpriteRegion.EYE_DEAD, WorkerSpriteManager.SizeRef.Eye());
		}
		if (directoryInfo5.Exists)
		{
			this.LoadCustomSprite(directoryInfo5, BasicSpriteRegion.EYEBROW, WorkerSpriteManager.SizeRef.Eyebrow());
		}
		if (directoryInfo6.Exists)
		{
			this.LoadCustomSprite(directoryInfo6, BasicSpriteRegion.EYEBROW_BATTLE, WorkerSpriteManager.SizeRef.Eyebrow());
		}
		if (directoryInfo7.Exists)
		{
			this.LoadCustomSprite(directoryInfo7, BasicSpriteRegion.EYEBROW_PANIC, WorkerSpriteManager.SizeRef.Eyebrow());
		}
		if (directoryInfo8.Exists)
		{
			this.LoadCustomSprite(directoryInfo8, BasicSpriteRegion.MOUTH, WorkerSpriteManager.SizeRef.Mouth());
		}
		if (directoryInfo9.Exists)
		{
			this.LoadCustomSprite(directoryInfo9, BasicSpriteRegion.MOUTH_BATTLE, WorkerSpriteManager.SizeRef.Mouth());
		}
		if (directoryInfo10.Exists)
		{
			this.LoadCustomSprite(directoryInfo10, BasicSpriteRegion.HAIR_FRONT, WorkerSpriteManager.SizeRef.FrontHair());
		}
		if (directoryInfo11.Exists)
		{
			this.LoadCustomSprite(directoryInfo11, BasicSpriteRegion.HAIR_REAR, WorkerSpriteManager.SizeRef.RearHair());
		}
	}

	// <Patch>
	public string ModFolderSrc
	{
		get
		{
			return this.CurModPath + "/BaseModCustomData";
		}
	}

	// <Patch>
	public string ModHairSrc
	{
		get
		{
			return this.ModFolderSrc + "/Hair";
		}
	}

	// <Patch>
	public string ModFaceSrc
	{
		get
		{
			return this.ModFolderSrc + "/Face";
		}
	}

	// <Patch>
	public string ModFrontHairSrc
	{
		get
		{
			return this.ModHairSrc + "/Front";
		}
	}

	// <Patch>
	public string ModRearHairSrc
	{
		get
		{
			return this.ModHairSrc + "/Rear";
		}
	}

	// <Patch>
	public string ModEyeSrc
	{
		get
		{
			return this.ModFaceSrc + "/Eye_Default";
		}
	}

	// <Patch>
	public string ModPanicEyeSrc
	{
		get
		{
			return this.ModFaceSrc + "/Eye_Panic";
		}
	}

	// <Patch>
	public string ModDeadEyeSrc
	{
		get
		{
			return this.ModFaceSrc + "/Eye_Dead";
		}
	}

	// <Patch>
	public string ModMouthSrc
	{
		get
		{
			return this.ModFaceSrc + "/Mouth_Default";
		}
	}

	// <Patch>
	public string ModBattleMouthSrc
	{
		get
		{
			return this.ModFaceSrc + "/Mouth_Battle";
		}
	}

	// <Patch>
	public string ModEyebrowSrc
	{
		get
		{
			return this.ModFaceSrc + "/Eyebrow_Default";
		}
	}

	// <Patch>
	public string ModBattleEyebrowSrc
	{
		get
		{
			return this.ModFaceSrc + "/Eyebrow_Battle";
		}
	}

	// <Patch>
	public string ModPanicEyebrowSrc
	{
		get
		{
			return this.ModFaceSrc + "/Eyebrow_Panic";
		}
	}

	// <Patch>
	public bool GetUniqueWeaponSpriteInfo_Mod(LobotomyBaseMod.KeyValuePairSS SS, out UniqueWeaponSpriteUnit unit)
	{
		if (SS.key == string.Empty)
		{
			this.GetWeaponSprite(WeaponClassType.SPECIAL, SS.value);
			return this._uniqueWeaponDic.TryGetValue(SS.value + "_AbcdcodeMade", out unit);
		}
		this.GetWeaponSprite_Mod(WeaponClassType.SPECIAL, SS);
		return this.uniqueWeaponDicMod.TryGetValue(SS, out unit);
	}

	// <Patch>
	public bool SpecialFindDir_Texture_Mod(string modid, string name, ref DirectoryInfo info, ref Texture2D tex)
	{
		if (modid == string.Empty)
		{
			foreach (DirectoryInfo dir in Add_On.instance.DirList)
			{
				DirectoryInfo directoryInfo = EquipmentDataLoader.CheckNamedDir(dir, "Equipment");
				if (directoryInfo != null)
				{
					if (!Directory.Exists(directoryInfo.FullName + "/Sprite/Weapon"))
					{
						continue;
					}
					DirectoryInfo directoryInfo2 = new DirectoryInfo(directoryInfo.FullName + "/Sprite/Weapon");
					if (directoryInfo2.GetFiles().Length != 0)
					{
						FileInfo[] files = directoryInfo2.GetFiles();
						for (int i = 0; i < files.Length; i++)
						{
							if (files[i].Name == name + ".png")
							{
								info = directoryInfo2;
								tex.LoadImage(File.ReadAllBytes(files[i].FullName));
								return true;
							}
						}
					}
				}
			}
		}
		else
		{
			ModInfo modInfo_patch = Add_On.instance.ModList.Find((ModInfo x) => x.modid == modid);
			if (modInfo_patch != null)
			{
				DirectoryInfo directoryInfo3 = EquipmentDataLoader.CheckNamedDir(modInfo_patch.modpath, "Equipment");
				if (Directory.Exists(directoryInfo3.FullName + "/Sprite/Weapon"))
				{
					DirectoryInfo directoryInfo4 = new DirectoryInfo(directoryInfo3.FullName + "/Sprite/Weapon");
					if (directoryInfo4.GetFiles().Length != 0)
					{
						FileInfo[] files2 = directoryInfo4.GetFiles();
						for (int j = 0; j < files2.Length; j++)
						{
							if (files2[j].Name == name + ".png")
							{
								info = directoryInfo4;
								tex.LoadImage(File.ReadAllBytes(files2[j].FullName));
								return true;
							}
						}
					}
				}
			}
		}
		return false;
	}

	// <Patch>
	public Sprite GetWeaponSprite_Mod(WeaponClassType type, LobotomyBaseMod.KeyValuePairSS SS)
	{ // <Mod> Modded Fists
		Sprite sprite = null;
		if (SS.key == string.Empty)
		{
			try
			{
				WorkerEquipmentSprite workerEquipmentSprite = null;
				if (type == WeaponClassType.SPECIAL)
				{
					if (this._uniqueWeaponDic.ContainsKey(SS.value))
					{
						UniqueWeaponSpriteUnit uniqueWeaponSpriteUnit = null;
						if (this._uniqueWeaponDic.TryGetValue(SS.value + "_AbcdcodeMade", out uniqueWeaponSpriteUnit))
						{
							sprite = uniqueWeaponSpriteUnit.CommonSprite;
						}
						else
						{
							Texture2D texture = new Texture2D(256, 256);
							DirectoryInfo dir = null;
							bool flag5 = this.SpecialFindDir_Texture(SS.value, ref dir, ref texture);
							this._uniqueWeaponDic.Add(SS.value + "_AbcdcodeMade", this._uniqueWeaponDic[SS.value].GetCopy());
							if (!flag5)
							{
								sprite = this._uniqueWeaponDic[SS.value + "_AbcdcodeMade"].CommonSprite;
								return sprite;
							}
							this.MakeNewUniqueWeaponSpriteUnit(SS.value, this._uniqueWeaponDic[SS.value + "_AbcdcodeMade"], dir, texture, out sprite);
						}
					}
					else
					{
						char[] separator = new char[]
						{
							'_'
						};
						string key = SS.value.Split(separator)[0];
						if (this._uniqueWeaponDic.ContainsKey(key))
						{
							Texture2D texture2 = new Texture2D(256, 256);
							DirectoryInfo dir2 = null;
							bool flag8 = this.SpecialFindDir_Texture(SS.value, ref dir2, ref texture2);
							this._uniqueWeaponDic.Add(SS.value, this._uniqueWeaponDic[key].GetCopy());
							this._uniqueWeaponDic.Add(SS.value + "_AbcdcodeMade", this._uniqueWeaponDic[key].GetCopy());
							if (!flag8)
							{
								sprite = this._uniqueWeaponDic[SS.value + "_AbcdcodeMade"].CommonSprite;
								return sprite;
							}
							this.MakeNewUniqueWeaponSpriteUnit(SS.value, this._uniqueWeaponDic[SS.value + "_AbcdcodeMade"], dir2, texture2, out sprite);
						}
					}
				}
				else
				{
					if (type != WeaponClassType.FIST)
					{
						if (this.equipData.GetData(EquipmentSpriteRegion.WEAPON, out workerEquipmentSprite))
						{
							WorkerWeaponSprite workerWeaponSprite = workerEquipmentSprite as WorkerWeaponSprite;
							FieldInfo field = typeof(WorkerWeaponSprite.WeaponDatabase).GetField("data", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
							Dictionary<string, Sprite> dictionary = (Dictionary<string, Sprite>)field.GetValue(workerWeaponSprite.GetDb((int)type));
							Sprite sprite2 = null;
							try
							{
								if (!dictionary.ContainsKey(SS.value))
								{
									if (type == WeaponClassType.SPEAR)
									{
										workerWeaponSprite.GetDb((int)type).GetSprite("Spear_Set_01_0", out sprite2);
									}
									if (type == WeaponClassType.PISTOL)
									{
										workerWeaponSprite.GetDb((int)type).GetSprite("Pistol_Set_01_0", out sprite2);
									}
									if (type == WeaponClassType.HAMMER)
									{
										workerWeaponSprite.GetDb((int)type).GetSprite("Hammer_Set_01_0", out sprite2);
									}
									if (type == WeaponClassType.BOWGUN)
									{
										workerWeaponSprite.GetDb((int)type).GetSprite("BowGun_Set_01_0", out sprite2);
									}
									if (type == WeaponClassType.CANNON)
									{
										workerWeaponSprite.GetDb((int)type).GetSprite("Cannon_Set_01_0", out sprite2);
									}
									if (type == WeaponClassType.AXE)
									{
										workerWeaponSprite.GetDb((int)type).GetSprite("Axe_Set_01_0", out sprite2);
									}
									if (type == WeaponClassType.KNIFE)
									{
										workerWeaponSprite.GetDb((int)type).GetSprite("Knife_Set_01_0", out sprite2);
									}
									if (type == WeaponClassType.MACE)
									{
										workerWeaponSprite.GetDb((int)type).GetSprite("Mace_Set_01_0", out sprite2);
									}
									if (type == WeaponClassType.RIFLE)
									{
										workerWeaponSprite.GetDb((int)type).GetSprite("Rifle_Set_01_0", out sprite2);
									}
									Texture2D texture2D = new Texture2D(sprite2.texture.width, sprite2.texture.height);
									foreach (DirectoryInfo dir3 in Add_On.instance.DirList)
									{
										bool flag21 = false;
										DirectoryInfo directoryInfo = EquipmentDataLoader.CheckNamedDir(dir3, "Equipment");
										if (directoryInfo != null)
										{
											if (!Directory.Exists(directoryInfo.FullName + "/Sprite/Weapon"))
											{
												continue;
											}
											DirectoryInfo directoryInfo2 = new DirectoryInfo(directoryInfo.FullName + "/Sprite/Weapon");
											if (directoryInfo2.GetFiles().Length != 0)
											{
												FileInfo[] files = directoryInfo2.GetFiles();
												for (int i = 0; i < files.Length; i++)
												{
													if (files[i].Name == SS.value + ".png")
													{
														texture2D.LoadImage(File.ReadAllBytes(files[i].FullName));
														flag21 = true;
														break;
													}
												}
											}
										}
										if (flag21)
										{
											break;
										}
									}
									Vector2 vector = new Vector2(sprite2.pivot.x / sprite2.rect.width, sprite2.pivot.y / sprite2.rect.height);
									Sprite sprite3 = Sprite.Create(texture2D, sprite2.rect, vector, sprite2.pixelsPerUnit, 0U, SpriteMeshType.FullRect, sprite2.border);
									sprite3.name = SS.value;
									dictionary.Add(SS.value, sprite3);
									field.SetValue(workerWeaponSprite.GetDb((int)type), dictionary);
								}
								if (workerWeaponSprite.GetDb((int)type).GetSprite(SS.value, out sprite2))
								{
									return sprite2;
								}
							}
							catch (Exception message)
							{
								Debug.LogError(message);
								return null;
							}
						}
						sprite = null;
					}
					else
					{
						Sprite[] fistSprite = this.GetFistSprite((int)float.Parse(SS.value));
						if (fistSprite.Length <= 1)
						{
							sprite = null;
						}
						else
						{
							sprite = fistSprite[1];
						}
					}
				}
			}
			catch (Exception ex)
			{
				LobotomyBaseMod.ModDebug.Log(" GetWeaponSprite error - " + ex.Message + Environment.NewLine + ex.StackTrace);
				File.WriteAllText(Application.dataPath + "/BaseMods/error.txt", ex.Message + Environment.NewLine + ex.StackTrace);
				sprite = null;
			}
			return sprite;
		}
		else
		{
			try
			{
				WorkerEquipmentSprite workerEquipmentSprite2 = null;
				if (this.uniqueWeaponDicMod == null)
				{
					this.uniqueWeaponDicMod = new Dictionary<LobotomyBaseMod.KeyValuePairSS, UniqueWeaponSpriteUnit>();
				}
				if (this.CommonWeaponSpriteMod == null)
				{
					this.CommonWeaponSpriteMod = new Dictionary<LobotomyBaseMod.KeyValuePairSS, Sprite>();
				}
				if (type == WeaponClassType.SPECIAL)
				{
					if (this.uniqueWeaponDicMod.ContainsKey(SS))
					{
						return this.uniqueWeaponDicMod[SS].CommonSprite;
					}
					if (this._uniqueWeaponDic.ContainsKey(SS.value))
					{
						Texture2D texture3 = new Texture2D(256, 256);
						DirectoryInfo dir4 = null;
						bool flag33 = this.SpecialFindDir_Texture_Mod(SS.key, SS.value, ref dir4, ref texture3);
						this.uniqueWeaponDicMod[SS] = this._uniqueWeaponDic[SS.value].GetCopy();
						if (!flag33)
						{
							sprite = this.uniqueWeaponDicMod[SS].CommonSprite;
							return sprite;
						}
						this.MakeNewUniqueWeaponSpriteUnit(SS.value, this.uniqueWeaponDicMod[SS], dir4, texture3, out sprite);
					}
					else
					{
						char[] separator2 = new char[]
						{
							'_'
						};
						string key2 = SS.value.Split(separator2)[0];
						if (this._uniqueWeaponDic.ContainsKey(key2))
						{
							Texture2D texture4 = new Texture2D(256, 256);
							DirectoryInfo dir5 = null;
							bool flag36 = this.SpecialFindDir_Texture_Mod(SS.key, SS.value, ref dir5, ref texture4);
							this.uniqueWeaponDicMod[SS] = this._uniqueWeaponDic[key2].GetCopy();
							if (!flag36)
							{
								sprite = this.uniqueWeaponDicMod[SS].CommonSprite;
								return sprite;
							}
							this.MakeNewUniqueWeaponSpriteUnit(SS.value, this.uniqueWeaponDicMod[SS], dir5, texture4, out sprite);
						}
					}
				}
				else
				{
					if (type != WeaponClassType.FIST)
					{
						if (this.CommonWeaponSpriteMod.ContainsKey(SS))
						{
							return this.CommonWeaponSpriteMod[SS];
						}
						if (this.equipData.GetData(EquipmentSpriteRegion.WEAPON, out workerEquipmentSprite2))
						{
							WorkerWeaponSprite workerWeaponSprite2 = workerEquipmentSprite2 as WorkerWeaponSprite;
							FieldInfo field2 = typeof(WorkerWeaponSprite.WeaponDatabase).GetField("data", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
							Dictionary<string, Sprite> dictionary2 = (Dictionary<string, Sprite>)field2.GetValue(workerWeaponSprite2.GetDb((int)type));
							Sprite sprite5 = null;
							if (!dictionary2.ContainsKey(SS.value))
							{
								if (type == WeaponClassType.SPEAR)
								{
									workerWeaponSprite2.GetDb((int)type).GetSprite("Spear_Set_01_0", out sprite5);
								}
								if (type == WeaponClassType.PISTOL)
								{
									workerWeaponSprite2.GetDb((int)type).GetSprite("Pistol_Set_01_0", out sprite5);
								}
								if (type == WeaponClassType.HAMMER)
								{
									workerWeaponSprite2.GetDb((int)type).GetSprite("Hammer_Set_01_0", out sprite5);
								}
								if (type == WeaponClassType.BOWGUN)
								{
									workerWeaponSprite2.GetDb((int)type).GetSprite("BowGun_Set_01_0", out sprite5);
								}
								if (type == WeaponClassType.CANNON)
								{
									workerWeaponSprite2.GetDb((int)type).GetSprite("Cannon_Set_01_0", out sprite5);
								}
								if (type == WeaponClassType.AXE)
								{
									workerWeaponSprite2.GetDb((int)type).GetSprite("Axe_Set_01_0", out sprite5);
								}
								if (type == WeaponClassType.KNIFE)
								{
									workerWeaponSprite2.GetDb((int)type).GetSprite("Knife_Set_01_0", out sprite5);
								}
								if (type == WeaponClassType.MACE)
								{
									workerWeaponSprite2.GetDb((int)type).GetSprite("Mace_Set_01_0", out sprite5);
								}
								if (type == WeaponClassType.RIFLE)
								{
									workerWeaponSprite2.GetDb((int)type).GetSprite("Rifle_Set_01_0", out sprite5);
								}
								Texture2D texture2D2 = new Texture2D(sprite5.texture.width, sprite5.texture.height);
								ModInfo modInfo_patch = Add_On.instance.ModList.Find((ModInfo x) => x.modid == SS.key);
								if (modInfo_patch != null)
								{
									DirectoryInfo directoryInfo3 = EquipmentDataLoader.CheckNamedDir(modInfo_patch.modpath, "Equipment");
									if (directoryInfo3 != null)
									{
										if (Directory.Exists(directoryInfo3.FullName + "/Sprite/Weapon"))
										{
											DirectoryInfo directoryInfo4 = new DirectoryInfo(directoryInfo3.FullName + "/Sprite/Weapon");
											if (directoryInfo4.GetFiles().Length != 0)
											{
												FileInfo[] files2 = directoryInfo4.GetFiles();
												for (int j = 0; j < files2.Length; j++)
												{
													if (files2[j].Name == SS.value + ".png")
													{
														texture2D2.LoadImage(File.ReadAllBytes(files2[j].FullName));
														break;
													}
												}
											}
										}
									}
									Vector2 vector2 = new Vector2(sprite5.pivot.x / sprite5.rect.width, sprite5.pivot.y / sprite5.rect.height);
									Sprite sprite6 = Sprite.Create(texture2D2, sprite5.rect, vector2, sprite5.pixelsPerUnit, 0U, SpriteMeshType.FullRect, sprite5.border);
									sprite6.name = SS.value;
									this.CommonWeaponSpriteMod[SS] = sprite6;
									return this.CommonWeaponSpriteMod[SS];
								}
							}
						}
						sprite = null;
					}
					else
					{
						Sprite[] fistSprite2 = this.GetFistSprite(SS);
						if (fistSprite2.Length <= 1)
						{
							sprite = null;
						}
						else
						{
							sprite = fistSprite2[1];
						}
					}
				}
				return sprite;
			}
			catch (Exception ex2)
			{
				LobotomyBaseMod.ModDebug.Log("GetWeaponSpriteMod error - " + ex2.Message + Environment.NewLine + ex2.StackTrace);
			}
		}
		return null;
	}

	// <Patch>
	public void GetArmorData_Mod(LobotomyBaseMod.LcId id, ref WorkerSprite.WorkerSprite set)
	{
		if (id.packageId == string.Empty)
		{
			this.GetArmorData(id.id, ref set);
		}
		else
		{
			if (this.ClothesSetMod == null)
			{
				this.ClothesSetMod = new Dictionary<LobotomyBaseMod.LcId, SpriteResourceLoadData>();
			}
			if (this.ClothesSetMod.ContainsKey(id))
			{
				set.Armor.SetSprite(this.ClothesSetMod[id]);
			}
			foreach (ModInfo modInfo in Add_On.instance.ModList)
			{
				if (modInfo.modid == id.packageId)
				{
					Texture2D texture2D = new Texture2D(2, 2);
					bool flag5 = false;
					DirectoryInfo directoryInfo = EquipmentDataLoader.CheckNamedDir(modInfo.modpath, "Equipment");
					if (directoryInfo != null)
					{
						if (!Directory.Exists(directoryInfo.FullName + "/Sprite/Armor"))
						{
							continue;
						}
						DirectoryInfo directoryInfo2 = new DirectoryInfo(directoryInfo.FullName + "/Sprite/Armor");
						if (directoryInfo2.GetFiles().Length != 0)
						{
							FileInfo[] files = directoryInfo2.GetFiles();
							for (int i = 0; i < files.Length; i++)
							{
								if (files[i].Name == id.id.ToString() + ".png")
								{
									texture2D.LoadImage(File.ReadAllBytes(files[i].FullName));
									flag5 = true;
									break;
								}
							}
						}
					}
					if (flag5)
					{
						SpriteResourceLoadData clothesSet = this.GetClothesSet(0);
						AtlasLoadData atlasLoadData = new AtlasLoadData();
						atlasLoadData.sprites.Clear();
						for (int j = 0; j < (clothesSet as AtlasLoadData).sprites.Count; j++)
						{
							Sprite sprite = (clothesSet as AtlasLoadData).sprites[j];
							Vector2 vector = new Vector2(sprite.pivot.x / sprite.rect.width, sprite.pivot.y / sprite.rect.height);
							Sprite sprite2 = Sprite.Create(Add_On.duplicateTexture(texture2D), sprite.rect, vector, sprite.pixelsPerUnit, 0U, SpriteMeshType.FullRect, sprite.border, true);
							sprite2.name = sprite.name;
							atlasLoadData.sprites.Add(sprite2);
							atlasLoadData.count++;
						}
						this.ClothesSetMod[id] = atlasLoadData;
						set.Armor.SetSprite(this.ClothesSetMod[id]);
						return;
					}
				}
			}
			LobotomyBaseMod.ModDebug.Log("GetArmorData_Mod No Match! Id : " + id.ToString());
		}
	}

	// Token: 0x04005413 RID: 21523
	private static WorkerSpriteManager _instance = null;

	// Token: 0x04005414 RID: 21524
	private static float custom_128 = 128f;

	// Token: 0x04005415 RID: 21525
	private static float custom_256 = 256f;

	// Token: 0x04005416 RID: 21526
	private static float custom_512 = 512f;

	// Token: 0x04005417 RID: 21527
	private static Vector2 pivot = new Vector2(0.5f, 0.5f);

	// Token: 0x04005418 RID: 21528
	private const float custom_x_std_hair = 256f;

	// Token: 0x04005419 RID: 21529
	private const float custom_x_std_face = 256f;

	// Token: 0x0400541A RID: 21530
	public const string facePath = "Sprites/Agent/Face";

	// Token: 0x0400541B RID: 21531
	public const string bothHairPath = "Sprites/Agent/Hair/Both";

	// Token: 0x0400541C RID: 21532
	public const string femaleHairPath = "Sprites/Agent/Hair/Female";

	// Token: 0x0400541D RID: 21533
	public const string maleHairPath = "Sprites/Agent/Hair/Male";

	// Token: 0x0400541E RID: 21534
	public Sprite righthand;

	// Token: 0x0400541F RID: 21535
	[Header("Spine")]
	public WorkerBasicSpriteController basicData;

	// Token: 0x04005420 RID: 21536
	public WorkerEquipmentSpriteController equipData;

	// Token: 0x04005421 RID: 21537
	public List<WorkerColorPreset> workerColor;

	// Token: 0x04005422 RID: 21538
	public List<Sprite> SefiraSymbol;

	// Token: 0x04005423 RID: 21539
	public List<Sprite> SefiraSymbol2;

	// Token: 0x04005424 RID: 21540
	public List<Sprite> SefiraSymbol3;

	// Token: 0x04005425 RID: 21541
	public List<Sprite> SefiraSymbol4;

	// Token: 0x04005426 RID: 21542
	public List<Sprite> WorkNote;

	// Token: 0x04005427 RID: 21543
	public List<Sprite> UniqueWeapon;

	// Token: 0x04005428 RID: 21544
	public List<Sprite> PanicShadow;

	// Token: 0x04005429 RID: 21545
	[Header("UniqueWeapon")]
	public List<UniqueWeaponSpriteUnit> uniqueWeaponSprites;

	// Token: 0x0400542A RID: 21546
	private Dictionary<string, UniqueWeaponSpriteUnit> _uniqueWeaponDic;

	// Token: 0x0400542B RID: 21547
	private static string ModifyBases;

	// Token: 0x0400542C RID: 21548
	public List<int> Modifys;

	// Token: 0x0400542D RID: 21549
	public static Sprite Special_Basic;

	// <Patch>
	public string CurModPath;

	// <Patch>
	[NonSerialized]
	public Dictionary<LobotomyBaseMod.KeyValuePairSS, Sprite> CommonWeaponSpriteMod = new Dictionary<LobotomyBaseMod.KeyValuePairSS, Sprite>();

	// <Patch>
	[NonSerialized]
	public Dictionary<LobotomyBaseMod.KeyValuePairSS, UniqueWeaponSpriteUnit> uniqueWeaponDicMod = new Dictionary<LobotomyBaseMod.KeyValuePairSS, UniqueWeaponSpriteUnit>();

	// <Patch>
	[NonSerialized]
	public Dictionary<LobotomyBaseMod.LcId, SpriteResourceLoadData> ClothesSetMod = new Dictionary<LobotomyBaseMod.LcId, SpriteResourceLoadData>();

	// <Mod>
	[NonSerialized]
	public Dictionary<LobotomyBaseMod.KeyValuePairSS, Sprite[]> FistWeaponSpriteMod = new Dictionary<LobotomyBaseMod.KeyValuePairSS, Sprite[]>();

	// Token: 0x02000BE7 RID: 3047
	public class SizeRef
	{
		// Token: 0x06005C4E RID: 23630 RVA: 0x000499A9 File Offset: 0x00047BA9
		public SizeRef()
		{
		}

		// Token: 0x06005C4F RID: 23631 RVA: 0x000499C7 File Offset: 0x00047BC7
		public static WorkerSpriteManager.SizeRef Mouth()
		{
			return new WorkerSpriteManager.SizeRef();
		}

		// Token: 0x06005C50 RID: 23632 RVA: 0x0020B8A4 File Offset: 0x00209AA4
		public static WorkerSpriteManager.SizeRef Eye()
		{
			return new WorkerSpriteManager.SizeRef
			{
				x = WorkerSpriteManager.custom_256
			};
		}

		// Token: 0x06005C51 RID: 23633 RVA: 0x0020B8A4 File Offset: 0x00209AA4
		public static WorkerSpriteManager.SizeRef Eyebrow()
		{
			return new WorkerSpriteManager.SizeRef
			{
				x = WorkerSpriteManager.custom_256
			};
		}

		// Token: 0x06005C52 RID: 23634 RVA: 0x0020B8C4 File Offset: 0x00209AC4
		public static WorkerSpriteManager.SizeRef FrontHair()
		{
			return new WorkerSpriteManager.SizeRef
			{
				x = WorkerSpriteManager.custom_256,
				y = WorkerSpriteManager.custom_256
			};
		}

		// Token: 0x06005C53 RID: 23635 RVA: 0x0020B8F0 File Offset: 0x00209AF0
		public static WorkerSpriteManager.SizeRef RearHair()
		{
			return new WorkerSpriteManager.SizeRef
			{
				x = WorkerSpriteManager.custom_256,
				y = WorkerSpriteManager.custom_512
			};
		}

		// Token: 0x0400542E RID: 21550
		public float x = WorkerSpriteManager.custom_128;

		// Token: 0x0400542F RID: 21551
		public float y = WorkerSpriteManager.custom_128;
	}
}
