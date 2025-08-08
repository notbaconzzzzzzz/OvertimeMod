/*
IsolateRoomUI.public void GetFeelingStateData(CreatureFeelingState state, out Sprite sprite, out Color color) // 
+IsolateRoomUI.public static Sprite[] extraFeelingStateIcon; // 
+IsolateRoomUI.public static Color[] extraFeelingStateColor; // 
*/
using System;
using System.Collections.Generic;
using System.IO; // 
using System.Reflection;
using Spine.Unity;
using UnityEngine;

// Token: 0x02000B18 RID: 2840
public class CreatureLayer : MonoBehaviour, IObserver
{
	// Token: 0x060055A8 RID: 21928 RVA: 0x000455D3 File Offset: 0x000437D3
	public CreatureLayer()
	{
	}

	// Token: 0x170007FF RID: 2047
	// (get) Token: 0x060055AA RID: 21930 RVA: 0x00045604 File Offset: 0x00043804
	// (set) Token: 0x060055A9 RID: 21929 RVA: 0x000455FC File Offset: 0x000437FC
	public static CreatureLayer currentLayer { get; private set; }

	// Token: 0x17000800 RID: 2048
	// (get) Token: 0x060055AB RID: 21931 RVA: 0x0004560B File Offset: 0x0004380B
	public static CreatureLayer.IsolateRoomUI IsolateRoomUIData
	{
		get
		{
			return UIColorManager.instance.isolateRoomUI;
		}
	}

	// Token: 0x060055AC RID: 21932 RVA: 0x001EB90C File Offset: 0x001E9B0C
	private void Awake()
	{
		CreatureLayer.currentLayer = this;
		this.creatureList = new List<CreatureUnit>();
		this.ordealCreatureList = new List<CreatureUnit>();
		this.eventCreatureList = new List<CreatureUnit>();
		this.sefiraBossList = new List<CreatureUnit>();
		this.creatureDic = new Dictionary<long, CreatureUnit>();
		this.etcList = new List<EtcUnit>();
		this.Init();
	}

	// Token: 0x060055AD RID: 21933 RVA: 0x001EB968 File Offset: 0x001E9B68
	private void OnEnable()
	{
		Notice.instance.Observe(NoticeName.RemoveCreature, this);
		Notice.instance.Observe(NoticeName.AddCreature, this);
		Notice.instance.Observe(NoticeName.ClearCreature, this);
		Notice.instance.Observe(NoticeName.RemoveOrdealCreature, this);
		Notice.instance.Observe(NoticeName.AddOrdealCreature, this);
		Notice.instance.Observe(NoticeName.ClearOrdealCreature, this);
		Notice.instance.Observe(NoticeName.RemoveEventCreature, this);
		Notice.instance.Observe(NoticeName.AddEventCreature, this);
		Notice.instance.Observe(NoticeName.ClearEventCreature, this);
		Notice.instance.Observe(NoticeName.AddEtcUnit, this);
		Notice.instance.Observe(NoticeName.RemoveEtcUnit, this);
		Notice.instance.Observe(NoticeName.ClearEtcUnit, this);
		Notice.instance.Observe(NoticeName.AddSefiraBossCreature, this);
		Notice.instance.Observe(NoticeName.RemoveSefiraBossCreature, this);
	}

	// Token: 0x060055AE RID: 21934 RVA: 0x001EBA58 File Offset: 0x001E9C58
	private void OnDisable()
	{
		Notice.instance.Remove(NoticeName.RemoveCreature, this);
		Notice.instance.Remove(NoticeName.AddCreature, this);
		Notice.instance.Remove(NoticeName.ClearCreature, this);
		Notice.instance.Remove(NoticeName.RemoveEventCreature, this);
		Notice.instance.Remove(NoticeName.AddEventCreature, this);
		Notice.instance.Remove(NoticeName.ClearEventCreature, this);
		Notice.instance.Remove(NoticeName.RemoveOrdealCreature, this);
		Notice.instance.Remove(NoticeName.AddOrdealCreature, this);
		Notice.instance.Remove(NoticeName.ClearOrdealCreature, this);
		Notice.instance.Remove(NoticeName.AddEtcUnit, this);
		Notice.instance.Remove(NoticeName.RemoveEtcUnit, this);
		Notice.instance.Remove(NoticeName.ClearEtcUnit, this);
		Notice.instance.Remove(NoticeName.AddSefiraBossCreature, this);
		Notice.instance.Remove(NoticeName.RemoveSefiraBossCreature, this);
	}

	// Token: 0x060055AF RID: 21935 RVA: 0x001EBB48 File Offset: 0x001E9D48
	public void Init()
	{
		this.Clear();
		foreach (CreatureModel model in CreatureManager.instance.GetCreatureList())
		{
			this.AddCreature(model);
		}
		this.ClearOrdealCreature();
		foreach (OrdealCreatureModel model2 in OrdealManager.instance.GetOrdealCreatureList())
		{
			this.AddOrdealCreature(model2);
		}
		this.ClearEventCreature();
		foreach (EventCreatureModel model3 in SpecialEventManager.instance.GetEventCreatureList())
		{
			this.AddEventCreature(model3);
		}
		this.ClearSefiraBossCreature();
	}

	// Token: 0x060055B0 RID: 21936 RVA: 0x001EBC00 File Offset: 0x001E9E00
	public void Clear()
	{
		foreach (CreatureUnit creatureUnit in this.creatureList)
		{
			if (creatureUnit != null && creatureUnit.gameObject != null)
			{
				UnityEngine.Object.Destroy(creatureUnit.gameObject);
			}
		}
		this.creatureList.Clear();
		this.creatureDic.Clear();
	}

	// Token: 0x060055B1 RID: 21937 RVA: 0x001EBC94 File Offset: 0x001E9E94
	public void ClearOrdealCreature()
	{
		foreach (CreatureUnit creatureUnit in this.ordealCreatureList)
		{
			if (creatureUnit != null && creatureUnit.gameObject != null)
			{
				UnityEngine.Object.Destroy(creatureUnit.gameObject);
			}
		}
		this.ordealCreatureList.Clear();
	}

	// Token: 0x060055B2 RID: 21938 RVA: 0x001EBD1C File Offset: 0x001E9F1C
	public void ClearEventCreature()
	{
		foreach (CreatureUnit creatureUnit in this.eventCreatureList)
		{
			if (creatureUnit != null && creatureUnit.gameObject != null)
			{
				UnityEngine.Object.Destroy(creatureUnit.gameObject);
			}
		}
		this.eventCreatureList.Clear();
	}

	// Token: 0x060055B3 RID: 21939 RVA: 0x001EBDA4 File Offset: 0x001E9FA4
	public void ClearSefiraBossCreature()
	{
		foreach (CreatureUnit creatureUnit in this.sefiraBossList)
		{
			if (creatureUnit != null && creatureUnit.gameObject != null)
			{
				UnityEngine.Object.Destroy(creatureUnit.gameObject);
			}
		}
		this.sefiraBossList.Clear();
	}

	// Token: 0x060055B4 RID: 21940 RVA: 0x001EBE2C File Offset: 0x001EA02C
	private void AddCreature(CreatureModel model)
	{ // <Patch> <Mod>
		bool flag = model == null;
		if (!flag)
		{
			try
			{
				CreatureUnit component = ResourceCache.instance.LoadPrefab("Unit/CreatureBase").GetComponent<CreatureUnit>();
				component.transform.SetParent(this.transform, false);
				component.model = model;
				model.SetUnit(component);
				bool flag2 = model.metaInfo.animSrc != string.Empty;
				if (flag2)
				{
					string[] array = model.metaInfo.animSrc.Split(new char[]
					{
						'/'
					});
					//if (AprilFoolsManager.instance.IsEventActive("AllDTM")) array = "Unit/CreatureAnimator/DontTouchMe".Split(new char[] { '/' });
					bool flag3 = array[0] == "Custom";
					if (flag3)
					{
						DirectoryInfo directoryInfo = null;
						foreach (ModInfo modInfo in Add_On.instance.ModList)
						{
							bool flag4 = modInfo.modid == CreatureTypeInfo.GetLcId(model.metaInfo).packageId;
							if (flag4)
							{
								bool flag5 = Directory.Exists(modInfo.modpath.FullName + "/CreatureAnimation/" + array[1]);
								if (flag5)
								{
									directoryInfo = new DirectoryInfo(modInfo.modpath.FullName + "/CreatureAnimation/" + array[1]);
									break;
								}
							}
						}
						bool flag6 = directoryInfo != null;
						if (flag6)
						{
							List<Texture2D> list = new List<Texture2D>();
							foreach (FileInfo fileInfo in directoryInfo.GetFiles())
							{
								bool flag7 = fileInfo.FullName.Contains(".png");
								if (flag7)
								{
									byte[] data = File.ReadAllBytes(fileInfo.FullName);
									Texture2D texture2D = new Texture2D(2, 2);
									texture2D.LoadImage(data);
									texture2D.name = Path.GetFileNameWithoutExtension(fileInfo.Name);
									list.Add(texture2D);
								}
							}
							string atlasText = File.ReadAllText(directoryInfo.FullName + "/atlas.txt");
							Shader shader = null;
							AtlasAsset atlasAsset = AtlasAsset.CreateRuntimeInstance(atlasText, list.ToArray(), shader, true);
							bool flag8 = File.Exists(directoryInfo.FullName + "/json.txt");
							GameObject gameObject;
							if (flag8)
							{
								gameObject = SkeletonAnimation.NewSkeletonAnimationGameObject(SkeletonDataAsset.CreateRuntimeInstance(File.ReadAllText(directoryInfo.FullName + "/json.txt"), atlasAsset, true, 0.01f)).gameObject;
							}
							else
							{
								gameObject = SkeletonAnimation.NewSkeletonAnimationGameObject(SkeletonDataAsset.CreateRuntimeInstance(File.ReadAllBytes(directoryInfo.FullName + "/skeleton.skel"), atlasAsset, true, 0.01f)).gameObject;
							}
							Type type = LobotomyBaseMod.ExtenionUtil.GetType(array[1]);
							gameObject.AddComponent(type);
							component.animTarget = gameObject.GetComponent<CreatureAnimScript>();
							gameObject.transform.SetParent(component.transform, false);
						}
					}
					else
					{
						GameObject gameObject2 = Prefab.LoadPrefab(model.metaInfo.animSrc);
						component.animTarget = gameObject2.GetComponent<CreatureAnimScript>();
						gameObject2.transform.SetParent(component.transform, false);
					}
				}
				bool flag9 = model.metaInfo.roomReturnSrc != string.Empty;
				if (flag9)
				{
					component.returnObject = Prefab.LoadPrefab(model.metaInfo.roomReturnSrc);
					component.returnObject.transform.SetParent(component.transform);
					component.returnObject.transform.localScale = new Vector3(0.2f, 0.2f, 1f);
					component.returnObject.transform.localPosition = new Vector3(0f, -0.2f, 0f);
					component.returnObject.SetActive(false);
					component.returnSpriteRenderer.gameObject.SetActive(false);
				}
				else
				{
					component.returnObject = component.returnSpriteRenderer.gameObject;
					component.returnObject.SetActive(false);
				}
				if (model.isolateRoomData != null)
				{
					GameObject gameObject3 = Prefab.LoadPrefab("IsolateRoom");
					gameObject3.transform.SetParent(this.transform, false);
					IsolateRoom component2 = gameObject3.GetComponent<IsolateRoom>();
					int item = UnityEngine.Random.Range(1, 4);
					this.tempIntforSprite.Add(item);
					string name = this.directory + 1.ToString();
					component2.RoomSpriteRenderer.sprite = ResourceCache.instance.GetSprite(name);
					component2.SetCreature(component);
					component2.Init();
					gameObject3.transform.position = model.basePosition;
					component.room = component2;
				}
				this.creatureList.Add(component);
				this.creatureDic.Add(model.instanceId, component);
			}
			catch (Exception ex)
			{
				LobotomyBaseMod.ModDebug.Log("AddCreature error - " + ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}
		/*
		if (model == null)
		{
			return;
		}
		try
		{
			CreatureUnit component = ResourceCache.instance.LoadPrefab("Unit/CreatureBase").GetComponent<CreatureUnit>();
			component.transform.SetParent(base.transform, false);
			component.model = model;
			model.SetUnit(component);
			if (model.metaInfo.animSrc != string.Empty)
			{
				string[] array = model.metaInfo.animSrc.Split(new char[]
				{
					'/'
				});
				if (array[0] == "Custom")
				{
					DirectoryInfo directoryInfo = null;
					foreach (DirectoryInfo directoryInfo2 in Add_On.instance.DirList)
					{
						if (Directory.Exists(directoryInfo2.FullName + "/CreatureAnimation/" + array[1]))
						{
							directoryInfo = new DirectoryInfo(directoryInfo2.FullName + "/CreatureAnimation/" + array[1]);
							break;
						}
					}
					if (directoryInfo != null)
					{
						List<Texture2D> list = new List<Texture2D>();
						foreach (FileInfo fileInfo in directoryInfo.GetFiles())
						{
							if (fileInfo.FullName.Contains(".png"))
							{
								byte[] data = File.ReadAllBytes(fileInfo.FullName);
								Texture2D texture2D = new Texture2D(2, 2);
								texture2D.LoadImage(data);
								texture2D.name = Path.GetFileNameWithoutExtension(fileInfo.Name);
								list.Add(texture2D);
							}
						}
						string atlasText = File.ReadAllText(directoryInfo.FullName + "/atlas.txt");
						Shader shader = null;
						AtlasAsset atlasAsset = AtlasAsset.CreateRuntimeInstance(atlasText, list.ToArray(), shader, true);
						GameObject gameObject;
						if (File.Exists(directoryInfo.FullName + "/json.txt"))
						{
							gameObject = SkeletonAnimation.NewSkeletonAnimationGameObject(SkeletonDataAsset.CreateRuntimeInstance(File.ReadAllText(directoryInfo.FullName + "/json.txt"), atlasAsset, true, 0.01f)).gameObject;
						}
						else
						{
							gameObject = SkeletonAnimation.NewSkeletonAnimationGameObject(SkeletonDataAsset.CreateRuntimeInstance(File.ReadAllBytes(directoryInfo.FullName + "/skeleton.skel"), atlasAsset, true, 0.01f)).gameObject;
						}
						Type type = null;
						foreach (Assembly assembly in Add_On.instance.AssemList)
						{
							foreach (Type type2 in assembly.GetTypes())
							{
								if (type2.Name == array[1])
								{
									type = type2;
									break;
								}
							}
							if (type != null)
							{
								break;
							}
						}
						gameObject.AddComponent(type);
						component.animTarget = gameObject.GetComponent<CreatureAnimScript>();
						gameObject.transform.SetParent(component.transform, false);
					}
				}
				else
				{
					GameObject gameObject2 = Prefab.LoadPrefab(model.metaInfo.animSrc);
					component.animTarget = gameObject2.GetComponent<CreatureAnimScript>();
					gameObject2.transform.SetParent(component.transform, false);
				}
			}
			if (model.metaInfo.roomReturnSrc != string.Empty)
			{
				component.returnObject = Prefab.LoadPrefab(model.metaInfo.roomReturnSrc);
				component.returnObject.transform.SetParent(component.transform);
				component.returnObject.transform.localScale = new Vector3(0.2f, 0.2f, 1f);
				component.returnObject.transform.localPosition = new Vector3(0f, -0.2f, 0f);
				component.returnObject.SetActive(false);
				component.returnSpriteRenderer.gameObject.SetActive(false);
			}
			else
			{
				component.returnObject = component.returnSpriteRenderer.gameObject;
				component.returnObject.SetActive(false);
			}
			GameObject gameObject3 = Prefab.LoadPrefab("IsolateRoom");
			gameObject3.transform.SetParent(base.transform, false);
			IsolateRoom component2 = gameObject3.GetComponent<IsolateRoom>();
			int item = UnityEngine.Random.Range(1, 4);
			this.tempIntforSprite.Add(item);
			string name = this.directory + 1;
			component2.RoomSpriteRenderer.sprite = ResourceCache.instance.GetSprite(name);
			component2.SetCreature(component);
			component2.Init();
			gameObject3.transform.position = model.basePosition;
			component.room = component2;
			this.creatureList.Add(component);
			this.creatureDic.Add(model.instanceId, component);
		}
		catch (Exception ex)
		{
			File.WriteAllText(Application.dataPath + "/BaseMods/Cerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
		}*/
	}

	// Token: 0x060055B5 RID: 21941 RVA: 0x001EC314 File Offset: 0x001EA514
	private void AddOrdealCreature(OrdealCreatureModel model)
	{
		GameObject gameObject = ResourceCache.instance.LoadPrefab("Unit/CreatureBase");
		CreatureUnit component = gameObject.GetComponent<CreatureUnit>();
		component.transform.SetParent(base.transform, false);
		component.model = model;
		model.SetUnit(component);
		if (model.metaInfo.animSrc != string.Empty)
		{
			GameObject gameObject2 = Prefab.LoadPrefab(model.metaInfo.animSrc);
			component.animTarget = gameObject2.GetComponent<CreatureAnimScript>();
			gameObject2.transform.SetParent(component.transform, false);
		}
		component.returnObject = component.returnSpriteRenderer.gameObject;
		component.returnObject.SetActive(false);
		this.ordealCreatureList.Add(component);
	}

	// Token: 0x060055B6 RID: 21942 RVA: 0x001EC3CC File Offset: 0x001EA5CC
	private void AddEventCreature(EventCreatureModel model)
	{
		CreatureUnit component = ResourceCache.instance.LoadPrefab("Unit/CreatureBase").GetComponent<CreatureUnit>();
		component.transform.SetParent(base.transform, false);
		component.model = model;
		model.SetUnit(component);
		if (model.metaInfo.animSrc != string.Empty)
		{
			string[] array = model.metaInfo.animSrc.Split(new char[]
			{
				'/'
			});
			if (array[0] == "Custom")
			{
				DirectoryInfo directoryInfo = null;
				foreach (DirectoryInfo directoryInfo2 in Add_On.instance.DirList)
				{
					if (Directory.Exists(directoryInfo2.FullName + "/CreatureAnimation/" + array[1]))
					{
						directoryInfo = new DirectoryInfo(directoryInfo2.FullName + "/CreatureAnimation/" + array[1]);
						break;
					}
				}
				if (directoryInfo != null)
				{
					List<Texture2D> list = new List<Texture2D>();
					foreach (FileInfo fileInfo in directoryInfo.GetFiles())
					{
						if (fileInfo.FullName.Contains(".png"))
						{
							byte[] data = File.ReadAllBytes(fileInfo.FullName);
							Texture2D texture2D = new Texture2D(2, 2);
							texture2D.LoadImage(data);
							texture2D.name = Path.GetFileNameWithoutExtension(fileInfo.Name);
							list.Add(texture2D);
						}
					}
					string atlasText = File.ReadAllText(directoryInfo.FullName + "/atlas.txt");
					Shader shader = null;
					AtlasAsset atlasAsset = AtlasAsset.CreateRuntimeInstance(atlasText, list.ToArray(), shader, true);
					GameObject gameObject;
					if (File.Exists(directoryInfo.FullName + "/json.txt"))
					{
						gameObject = SkeletonAnimation.NewSkeletonAnimationGameObject(SkeletonDataAsset.CreateRuntimeInstance(File.ReadAllText(directoryInfo.FullName + "/json.txt"), atlasAsset, true, 0.01f)).gameObject;
					}
					else
					{
						gameObject = SkeletonAnimation.NewSkeletonAnimationGameObject(SkeletonDataAsset.CreateRuntimeInstance(File.ReadAllBytes(directoryInfo.FullName + "/skeleton.skel"), atlasAsset, true, 0.01f)).gameObject;
					}
					Type type = null;
					foreach (Assembly assembly in Add_On.instance.AssemList)
					{
						foreach (Type type2 in assembly.GetTypes())
						{
							if (type2.Name == array[1])
							{
								type = type2;
								break;
							}
						}
						if (type != null)
						{
							break;
						}
					}
					gameObject.AddComponent(type);
					component.animTarget = gameObject.GetComponent<CreatureAnimScript>();
					gameObject.transform.SetParent(component.transform, false);
				}
			}
			else
			{
				GameObject gameObject2 = Prefab.LoadPrefab(model.metaInfo.animSrc);
				component.animTarget = gameObject2.GetComponent<CreatureAnimScript>();
				gameObject2.transform.SetParent(component.transform, false);
			}
		}
		component.returnObject = component.returnSpriteRenderer.gameObject;
		component.returnObject.SetActive(false);
		this.eventCreatureList.Add(component);
	}

	// Token: 0x060055B7 RID: 21943 RVA: 0x001EC6F8 File Offset: 0x001EA8F8
	private void AddSefiraBossCreature(SefiraBossCreatureModel model, string animSrc)
	{
		GameObject gameObject = ResourceCache.instance.LoadPrefab("Unit/CreatureBase");
		CreatureUnit component = gameObject.GetComponent<CreatureUnit>();
		component.transform.SetParent(base.transform, false);
		component.model = model;
		model.SetUnit(component);
		if (animSrc != null && animSrc != string.Empty)
		{
			GameObject gameObject2 = Prefab.LoadPrefab("Unit/CreatureAnimator/" + animSrc);
			component.animTarget = gameObject2.GetComponent<CreatureAnimScript>();
			gameObject2.transform.SetParent(component.transform, false);
		}
		else if (model.metaInfo.animSrc != string.Empty)
		{
			GameObject gameObject3 = Prefab.LoadPrefab(model.metaInfo.animSrc);
			component.animTarget = gameObject3.GetComponent<CreatureAnimScript>();
			gameObject3.transform.SetParent(component.transform, false);
		}
		component.returnObject = component.returnSpriteRenderer.gameObject;
		component.returnObject.SetActive(false);
		this.sefiraBossList.Add(component);
	}

	// Token: 0x060055B8 RID: 21944 RVA: 0x001EC7F8 File Offset: 0x001EA9F8
	public void ChangeCreaturePos(CreatureModel caller, CreatureModel changed)
	{
		CreatureUnit unit = caller.Unit;
		CreatureUnit unit2 = changed.Unit;
		IsolateRoom room = unit.room;
		IsolateRoom room2 = unit2.room;
		room.transform.position = caller.basePosition;
		room2.transform.position = changed.basePosition;
	}

	// Token: 0x060055B9 RID: 21945 RVA: 0x001EC850 File Offset: 0x001EAA50
	public void RemoveCreature(CreatureModel model)
	{
		CreatureUnit unit = model.Unit;
		unit.gameObject.SetActive(false);
		UnityEngine.Object.Destroy(unit);
	}

	// Token: 0x060055BA RID: 21946 RVA: 0x001EC878 File Offset: 0x001EAA78
	public CreatureUnit GetCreature(long id)
	{
		CreatureUnit result = null;
		this.creatureDic.TryGetValue(id, out result);
		return result;
	}

	// Token: 0x060055BB RID: 21947 RVA: 0x001EC898 File Offset: 0x001EAA98
	public EtcUnit AddEtcUnit(UnitModel model)
	{ // <Mod>
		GameObject gameObject = null;
		if (model is SnowWhite.VineArea)
		{
			gameObject = ResourceCache.instance.LoadPrefab("Unit/EtcUnit");
		}
		else if (model is YoungPrinceFriend.Spore)
		{
			gameObject = ResourceCache.instance.LoadPrefab("Unit/Spore");
		}
		else if (model is Butterfly.ButterflyEffect)
		{
			gameObject = ResourceCache.instance.LoadPrefab("Unit/ButterflyEffect");
		}
		else
		{
			Debug.Log("Invalid EtcUnit");
			return null;
		}
		EtcUnit component = gameObject.GetComponent<EtcUnit>();
		component.transform.SetParent(base.transform, false);
		component.transform.localPosition = Vector3.zero;
		component.transform.localScale = Vector3.one;
		component.model = model;
		this.etcList.Add(component);
		return component;
	}

	// <Mod>
	public EtcUnit AddEtcUnit(UnitModel model, GameObject gameObject)
	{
		if (gameObject == null) return null;
		// gameObject = GameObject.Instantiate(gameObject);
		EtcUnit component = gameObject.GetComponent<EtcUnit>();
		if (component == null) component = gameObject.AddComponent<EtcUnit>();
		component.transform.SetParent(base.transform, false);
		component.transform.localPosition = Vector3.zero;
		component.transform.localScale = Vector3.one;
		component.model = model;
		this.etcList.Add(component);
		return component;
	}

	// Token: 0x060055BC RID: 21948 RVA: 0x001EC960 File Offset: 0x001EAB60
	public void RemoveEtcUnit(UnitModel model)
	{
		EtcUnit etcUnit = this.etcList.Find((EtcUnit x) => x.model == model);
		if (etcUnit != null)
		{
			this.etcList.Remove(etcUnit);
			UnityEngine.Object.Destroy(etcUnit.gameObject);
		}
	}

	// Token: 0x060055BD RID: 21949 RVA: 0x000043E2 File Offset: 0x000025E2
	public EtcUnit GetEtcUnit(long id)
	{
		return null;
	}

	// Token: 0x060055BE RID: 21950 RVA: 0x001EC9B8 File Offset: 0x001EABB8
	public void OnNotice(string notice, params object[] param)
	{ // <Mod>
		if (notice == NoticeName.AddCreature)
		{
			foreach (object obj in param)
			{
				this.AddCreature((CreatureModel)obj);
			}
		}
		else if (notice == NoticeName.ClearCreature)
		{
			this.Clear();
		}
		else if (notice == NoticeName.RemoveCreature)
		{
			foreach (object obj2 in param)
			{
				this.RemoveCreature((CreatureModel)obj2);
			}
		}
		else if (notice == NoticeName.AddOrdealCreature)
		{
			foreach (object obj3 in param)
			{
				this.AddOrdealCreature((OrdealCreatureModel)obj3);
			}
		}
		else if (notice == NoticeName.ClearOrdealCreature)
		{
			this.ClearOrdealCreature();
		}
		else if (notice == NoticeName.RemoveOrdealCreature)
		{
			foreach (object obj4 in param)
			{
				this.RemoveCreature((CreatureModel)obj4);
			}
		}
		else if (notice == NoticeName.AddEtcUnit)
		{
			if (param.Length > 1)
			{
				this.AddEtcUnit((UnitModel)param[0], (GameObject)param[1]);
			}
			else
			{
				this.AddEtcUnit((UnitModel)param[0]);
			}
		}
		else if (notice == NoticeName.RemoveEtcUnit)
		{
			this.RemoveEtcUnit((UnitModel)param[0]);
		}
		else if (notice == NoticeName.AddSefiraBossCreature)
		{
			this.AddSefiraBossCreature((SefiraBossCreatureModel)param[0], (string)param[1]);
		}
		else if (notice == NoticeName.RemoveSefiraBossCreature)
		{
			this.ClearSefiraBossCreature();
		}
		else if (notice == NoticeName.AddEventCreature)
		{
			foreach (object obj5 in param)
			{
				this.AddEventCreature((EventCreatureModel)obj5);
			}
		}
		else if (notice == NoticeName.ClearEventCreature)
		{
			this.ClearEventCreature();
		}
		else if (notice == NoticeName.RemoveEventCreature)
		{
			foreach (object obj6 in param)
			{
				this.RemoveCreature((CreatureModel)obj6);
			}
		}
	}

	// Token: 0x060055BF RID: 21951 RVA: 0x001ECC30 File Offset: 0x001EAE30
	public void OnSpriteButtonClick(bool state)
	{
		string name = string.Empty;
		if (state)
		{
			for (int i = 0; i < this.creatureList.Count; i++)
			{
				CreatureUnit creatureUnit = this.creatureList[i];
				name = this.directory + this.tempIntforSprite[0] + this.dark;
				creatureUnit.room.RoomSpriteRenderer.sprite = ResourceCache.instance.GetSprite(name);
				creatureUnit.room.RoomSpriteRenderer.transform.localPosition = Vector3.zero;
			}
		}
		else
		{
			for (int j = 0; j < this.creatureList.Count; j++)
			{
				CreatureUnit creatureUnit2 = this.creatureList[j];
				name = this.directory + this.tempIntforSprite[0];
				creatureUnit2.room.RoomSpriteRenderer.sprite = ResourceCache.instance.GetSprite(name);
				creatureUnit2.room.RoomSpriteRenderer.transform.localPosition = Vector3.zero;
			}
		}
	}

	// Token: 0x04004EED RID: 20205
	private List<CreatureUnit> creatureList;

	// Token: 0x04004EEE RID: 20206
	private Dictionary<long, CreatureUnit> creatureDic;

	// Token: 0x04004EEF RID: 20207
	private List<CreatureUnit> ordealCreatureList;

	// Token: 0x04004EF0 RID: 20208
	private List<CreatureUnit> sefiraBossList;

	// Token: 0x04004EF1 RID: 20209
	private List<CreatureUnit> eventCreatureList;

	// Token: 0x04004EF2 RID: 20210
	private List<int> tempIntforSprite = new List<int>();

	// Token: 0x04004EF3 RID: 20211
	private List<EtcUnit> etcList;

	// Token: 0x04004EF4 RID: 20212
	private Dictionary<long, EtcUnit> etcDic;

	// Token: 0x04004EF5 RID: 20213
	private string directory = "Sprites/IsolateRoom/isolate_";

	// Token: 0x04004EF6 RID: 20214
	private string dark = "_dark";

	// Token: 0x04004EF7 RID: 20215
	public CreatureLayer.IsolateRoomUI isolateRoomUI;

	// Token: 0x02000B19 RID: 2841
	[Serializable]
	public class IsolateRoomUI
	{
		// Token: 0x060055C0 RID: 21952 RVA: 0x00004378 File Offset: 0x00002578
		public IsolateRoomUI()
		{
		}

		// Token: 0x060055C1 RID: 21953 RVA: 0x00045617 File Offset: 0x00043817
		public Sprite GetDamageSprite(RwbpType type)
		{
			if (type == RwbpType.N)
			{
				return null;
			}
			return this.DamageSprite[type - RwbpType.R];
		}

		// Token: 0x060055C2 RID: 21954 RVA: 0x001ECD50 File Offset: 0x001EAF50
		public void GetFeelingStateData(CreatureFeelingState state, out Sprite sprite, out Color color)
		{ // <Mod>
            try
            {
                if (extraFeelingStateIcon == null)
                {
                    extraFeelingStateIcon = new Sprite[2];
                    String resultName = "Tranq";
                    byte[] data = File.ReadAllBytes(Application.dataPath + "/Managed/BaseMod/AssetDump/" + resultName + "ResultIcon.png");
                    Texture2D texture2D = new Texture2D(250, 250);
                    texture2D.LoadImage(data);
                    extraFeelingStateIcon[0] = Sprite.Create(texture2D, new Rect(0, 0, 250, 250), new Vector2(125, 125), 50, 0U, SpriteMeshType.Tight, new Vector4());
                    resultName = "Unknown";
                    data = File.ReadAllBytes(Application.dataPath + "/Managed/BaseMod/AssetDump/" + resultName + "ResultIcon.png");
                    texture2D = new Texture2D(250, 250);
                    texture2D.LoadImage(data);
                    extraFeelingStateIcon[1] = Sprite.Create(texture2D, new Rect(0, 0, 250, 250), new Vector2(125, 125), 50, 0U, SpriteMeshType.Tight, new Vector4());
                }
                if (extraFeelingStateColor == null)
                {
                    extraFeelingStateColor = new Color[2];
                    extraFeelingStateColor[0] = new Color(0.15f, 0.269f, 0.721f);
                    extraFeelingStateColor[1] = new Color(0.575f, 0f, 0.741f);
                }
            }
            catch (Exception ex)
            {
                Notice.instance.Send(NoticeName.AddSystemLog, new object[]
                {
                    ex.Message + " : " + ex.StackTrace
                });
            }
			sprite = null;
			color = this.DisabledColor;
			switch (state)
			{
			case CreatureFeelingState.GOOD:
				sprite = this.FeelingStateIcon[2];
				color = this.FeelingStateColor[2];
				break;
			case CreatureFeelingState.NORM:
				sprite = this.FeelingStateIcon[1];
				color = this.FeelingStateColor[1];
				break;
			case CreatureFeelingState.BAD:
				sprite = this.FeelingStateIcon[0];
				color = this.FeelingStateColor[0];
				break;
			case CreatureFeelingState.TRANQ:
				sprite = extraFeelingStateIcon[0];
				color = extraFeelingStateColor[0];
				break;
			case CreatureFeelingState.NONE:
				sprite = extraFeelingStateIcon[1];
				color = extraFeelingStateColor[1];
				break;
			}/*
			Notice.instance.Send(NoticeName.AddSystemLog, new object[]
			{
				(int)(color.r * 255f) + "-" + (int)(color.g * 255f) + "-" + (int)(color.b * 255f)
			});
			Notice.instance.Send(NoticeName.AddSystemLog, new object[]
			{
				sprite.rect.ToString() + " : " + sprite.pivot.x + "-" + sprite.pivot.y + " : " + sprite.pixelsPerUnit + " : " + sprite.border.ToString()
			});*/
            /*
            try
            {
                if (!File.Exists(Application.dataPath + "/Managed/BaseMod/AssetDump/" + resultName + "ResultIcon.png"))
                {
					Texture2D tex = new Texture2D(sprite.texture.width, sprite.texture.height);
					tex.SetPixels(sprite.texture.GetPixels());
                    File.WriteAllBytes(Application.dataPath + "/Managed/BaseMod/AssetDump/" + resultName + "ResultIcon.png", tex.EncodeToPNG());
                }
            }
            catch (Exception ex)
            {
                Notice.instance.Send(NoticeName.AddSystemLog, new object[]
                {
                    ex.Message + " : " + ex.StackTrace
                });
            }*/
		}

		// Token: 0x060055C3 RID: 21955 RVA: 0x001ECE00 File Offset: 0x001EB000
		public void GetGeneratedEnergyColor(int value, out Color fill, out Color text)
		{
			if (value < 0)
			{
				fill = this.MinusEnergyColor;
				text = this.MinusTextColor;
			}
			else if (value == 0)
			{
				fill = this.ZeroEnergyColor;
				text = this.ZeroColor;
			}
			else
			{
				fill = this.PlusEnergyColor;
				text = this.PlusTextColor;
			}
		}

		// Token: 0x04004EF8 RID: 20216
		public Color DisabledColor;

		// Token: 0x04004EF9 RID: 20217
		public Sprite[] FeelingStateIcon;

		// Token: 0x04004EFA RID: 20218
		public Color[] FeelingStateColor;

		// Token: 0x04004EFB RID: 20219
		public Sprite[] RiskLevel;

		// Token: 0x04004EFC RID: 20220
		public Color PlusEnergyColor;

		// Token: 0x04004EFD RID: 20221
		public Color MinusEnergyColor;

		// Token: 0x04004EFE RID: 20222
		public Color ZeroEnergyColor;

		// Token: 0x04004EFF RID: 20223
		public Color PlusTextColor;

		// Token: 0x04004F00 RID: 20224
		public Color MinusTextColor;

		// Token: 0x04004F01 RID: 20225
		public Color ZeroColor;

		// Token: 0x04004F02 RID: 20226
		public Sprite[] DamageSprite;

		// Token: 0x04004F03 RID: 20227
		public Color RoomDisabledColor;

		// Token: 0x04004F04 RID: 20228
		public Color RoomEnabledColor;

		// <Mod>
        public static Sprite[] extraFeelingStateIcon;

		// <Mod>
        public static Color[] extraFeelingStateColor;
	}
}
