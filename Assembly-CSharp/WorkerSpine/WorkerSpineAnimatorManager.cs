using System;
using System.Collections.Generic;
using System.IO;
using System.Linq; // 
using Spine;
using Spine.Unity;
using UnityEngine;

namespace WorkerSpine
{
	// Token: 0x020008B7 RID: 2231
	public class WorkerSpineAnimatorManager
	{
		// Token: 0x060044A9 RID: 17577 RVA: 0x0003A41C File Offset: 0x0003861C
		public WorkerSpineAnimatorManager()
		{
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x060044AA RID: 17578 RVA: 0x0003A43A File Offset: 0x0003863A
		public static WorkerSpineAnimatorManager instance
		{
			get
			{
				if (WorkerSpineAnimatorManager._instance == null)
				{
					WorkerSpineAnimatorManager._instance = new WorkerSpineAnimatorManager();
				}
				return WorkerSpineAnimatorManager._instance;
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x060044AB RID: 17579 RVA: 0x0003A452 File Offset: 0x00038652
		public bool IsLoaded
		{
			get
			{
				return this._isLoaded;
			}
		}

		// Token: 0x060044AC RID: 17580 RVA: 0x001A62B4 File Offset: 0x001A44B4
		public void Init(List<WorkerSpineAnimatorData> data)
		{ // <Patch>
			try
			{
				this.nameDicMod = new Dictionary<LobotomyBaseMod.KeyValuePairSS, WorkerSpineAnimatorData>();
				Dictionary<LobotomyBaseMod.KeyValuePairSS, object> dictionary = new Dictionary<LobotomyBaseMod.KeyValuePairSS, object>();
				Dictionary<LobotomyBaseMod.KeyValuePairSS, string> dictionary2 = new Dictionary<LobotomyBaseMod.KeyValuePairSS, string>();
				new Dictionary<string, string>();
				if (Add_On.instance.DirList.Count > 0)
				{
					foreach (ModInfo modInfo_patch in Add_On.instance.ModList)
					{
						DirectoryInfo directoryInfo = Add_On.CheckNamedDir(modInfo_patch.modpath, "AgentAnimation");
						string modid = modInfo_patch.modid;
						if (directoryInfo != null && directoryInfo.GetDirectories().Length != 0)
						{
							foreach (DirectoryInfo directoryInfo2 in directoryInfo.GetDirectories())
							{
								LobotomyBaseMod.KeyValuePairSS key = new LobotomyBaseMod.KeyValuePairSS(modid, directoryInfo2.Name);
								if (File.Exists(directoryInfo2.FullName + "/json.txt"))
								{
									dictionary.Add(key, File.ReadAllText(directoryInfo2.FullName + "/json.txt"));
								}
								else if (File.Exists(directoryInfo2.FullName + "/skeleton.skel"))
								{
									dictionary.Add(key, File.ReadAllBytes(directoryInfo2.FullName + "/skeleton.skel"));
								}
								dictionary2.Add(key, directoryInfo2.FullName);
							}
						}
					}
				}
				Dictionary<LobotomyBaseMod.KeyValuePairSS, object> dictionary3 = new Dictionary<LobotomyBaseMod.KeyValuePairSS, object>(dictionary);
				Dictionary<LobotomyBaseMod.KeyValuePairSS, string> dictionary4 = new Dictionary<LobotomyBaseMod.KeyValuePairSS, string>(dictionary2);
				foreach (WorkerSpineAnimatorData workerSpineAnimatorData in data)
				{
					try
					{
						this.FindNewSkinandSkel_Mod(workerSpineAnimatorData, dictionary, dictionary2, dictionary3, dictionary4);
						this.nameDic.Add(workerSpineAnimatorData.name, workerSpineAnimatorData);
						this.nameDicMod.Add(new LobotomyBaseMod.KeyValuePairSS(string.Empty, workerSpineAnimatorData.name), workerSpineAnimatorData);
						this.idDic.Add(workerSpineAnimatorData.id, workerSpineAnimatorData);
					}
					catch (Exception arg)
					{
						Debug.LogError(workerSpineAnimatorData.name + Environment.NewLine + arg);
					}
					this.GetClipInfo(workerSpineAnimatorData);
				}
				if (dictionary3.Count > 0)
				{
					foreach (KeyValuePair<LobotomyBaseMod.KeyValuePairSS, object> keyValuePair in dictionary3)
					{
						char[] separator = new char[]
						{
							'_'
						};
						string text = keyValuePair.Key.value.Split(separator)[0];
						if (this.nameDic.ContainsKey(text))
						{
							WorkerSpineAnimatorData workerSpineAnimatorData2 = this.nameDic[text];
							WorkerSpineAnimatorData workerSpineAnimatorData3 = new WorkerSpineAnimatorData(workerSpineAnimatorData2.id + 10000000, keyValuePair.Key.value, workerSpineAnimatorData2.animatorSrc, workerSpineAnimatorData2.skeletonSrc);
							workerSpineAnimatorData3.LoadData();
							DirectoryInfo directoryInfo3 = new DirectoryInfo(dictionary4[keyValuePair.Key]);
							List<Texture2D> list = new List<Texture2D>();
							foreach (FileInfo fileInfo in directoryInfo3.GetFiles())
							{
								if (fileInfo.Name.Contains(".png"))
								{
									byte[] data2 = File.ReadAllBytes(directoryInfo3.FullName + "/" + fileInfo.Name);
									Texture2D texture2D = new Texture2D(2, 2);
									texture2D.LoadImage(data2);
									texture2D.name = Path.GetFileNameWithoutExtension(fileInfo.Name);
									list.Add(texture2D);
								}
							}
							string atlasText = File.ReadAllText(directoryInfo3.FullName + "/atlas.txt");
							Shader shader = null;
							AtlasAsset atlasAsset = AtlasAsset.CreateRuntimeInstance(atlasText, list.ToArray(), shader, true);
							new AtlasAttachmentLoader(new Atlas[]
							{
								atlasAsset.GetAtlas()
							});
							SkeletonDataAsset skeletonData;
							if (keyValuePair.Value is string)
							{
								skeletonData = SkeletonDataAsset.CreateRuntimeInstance((string)keyValuePair.Value, atlasAsset, true, workerSpineAnimatorData3.skeletonData.scale);
							}
							else
							{
								skeletonData = SkeletonDataAsset.CreateRuntimeInstance((byte[])keyValuePair.Value, atlasAsset, true, workerSpineAnimatorData3.skeletonData.scale);
							}
							workerSpineAnimatorData3.skeletonData = skeletonData;
							this.nameDicMod.Add(keyValuePair.Key, workerSpineAnimatorData3);
						}
						else if (text == "Custom")
						{
							WorkerSpineAnimatorData workerSpineAnimatorData4 = new WorkerSpineAnimatorData(keyValuePair.Key.GetHashCode(), keyValuePair.Key.value);
							DirectoryInfo directoryInfo4 = new DirectoryInfo(dictionary4[keyValuePair.Key]);
							List<Texture2D> list2 = new List<Texture2D>();
							foreach (FileInfo fileInfo2 in directoryInfo4.GetFiles())
							{
								if (fileInfo2.Name.Contains(".png"))
								{
									byte[] data3 = File.ReadAllBytes(directoryInfo4.FullName + "/" + fileInfo2.Name);
									Texture2D texture2D2 = new Texture2D(2, 2);
									texture2D2.LoadImage(data3);
									texture2D2.name = Path.GetFileNameWithoutExtension(fileInfo2.Name);
									list2.Add(texture2D2);
								}
							}
							string atlasText2 = File.ReadAllText(directoryInfo4.FullName + "/atlas.txt");
							Shader shader2 = null;
							AtlasAsset atlasAsset2 = AtlasAsset.CreateRuntimeInstance(atlasText2, list2.ToArray(), shader2, true);
							new AtlasAttachmentLoader(new Atlas[]
							{
								atlasAsset2.GetAtlas()
							});
							SkeletonDataAsset skeletonDataAsset;
							if (keyValuePair.Value is string)
							{
								skeletonDataAsset = SkeletonDataAsset.CreateRuntimeInstance((string)keyValuePair.Value, atlasAsset2, true, 0.01f);
							}
							else
							{
								skeletonDataAsset = SkeletonDataAsset.CreateRuntimeInstance((byte[])keyValuePair.Value, atlasAsset2, true, 0.01f);
							}
							workerSpineAnimatorData4.skeletonData = skeletonDataAsset;
							if (skeletonDataAsset.controller != null)
							{
								File.WriteAllText(Application.dataPath + "/BaseMods/controller.txt", "");
							}
							this.nameDicMod.Add(keyValuePair.Key, workerSpineAnimatorData4);
						}
					}
				}
				this._isLoaded = true;
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/error2.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}

		// Token: 0x060044AD RID: 17581 RVA: 0x001A68D8 File Offset: 0x001A4AD8
		public WorkerSpineAnimatorData GetData(int id)
		{
			WorkerSpineAnimatorData result = null;
			if (this.idDic.TryGetValue(id, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x060044AE RID: 17582 RVA: 0x001A68FC File Offset: 0x001A4AFC
		public WorkerSpineAnimatorData GetData(string name)
		{
			WorkerSpineAnimatorData result = null;
			if (this.nameDic.TryGetValue(name, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x060044AF RID: 17583 RVA: 0x0003A45A File Offset: 0x0003865A
		public bool GetDataWithCheck(int id, out WorkerSpineAnimatorData output)
		{
			return this.idDic.TryGetValue(id, out output);
		}

		// Token: 0x060044B0 RID: 17584 RVA: 0x0003A469 File Offset: 0x00038669
		public bool GetDataWithCheck(string name, out WorkerSpineAnimatorData output)
		{ // <Patch>
            return GetDataWithCheck_Mod(new LobotomyBaseMod.KeyValuePairSS(string.Empty, name), out output);
			// return this.nameDic.TryGetValue(name, out output);
		}

		// Token: 0x060044B1 RID: 17585 RVA: 0x001A6920 File Offset: 0x001A4B20
		public void OutPutSkeletonRenderer(CreatureTypeInfo info, SkeletonRenderer renderer)
		{
			DirectoryInfo directoryInfo = Directory.CreateDirectory(string.Concat(new object[]
			{
				Application.dataPath,
				"/CreatureAnim/",
				info.id
			}));
			File.WriteAllText(string.Concat(new object[]
			{
				directoryInfo.FullName,
				"/json.txt"
			}), renderer.skeletonDataAsset.skeletonJSON.text);
			int num = 0;
			foreach (AtlasAsset atlasAsset in renderer.skeletonDataAsset.atlasAssets)
			{
				File.WriteAllText(string.Concat(new object[]
				{
					directoryInfo.FullName,
					"/atlas_" + num.ToString() + ".txt"
				}), atlasAsset.atlasFile.text);
				int num2 = 0;
				Material[] materials = atlasAsset.materials;
				for (int j = 0; j < materials.Length; j++)
				{
					byte[] bytes = Add_On.duplicateTexture(materials[j].mainTexture as Texture2D).EncodeToPNG();
					File.WriteAllBytes(string.Concat(new object[]
					{
						directoryInfo.FullName,
						string.Concat(new string[]
						{
							"/texture_",
							num.ToString(),
							"_",
							num2.ToString(),
							".png"
						})
					}), bytes);
					num2++;
				}
				num++;
			}
		}

		// Token: 0x060044B2 RID: 17586 RVA: 0x001A6A90 File Offset: 0x001A4C90
		public void OutPutSkeletonRendererChild(CreatureTypeInfo info, SkeletonRenderer renderer)
		{
			DirectoryInfo directoryInfo = Directory.CreateDirectory(string.Concat(new object[]
			{
				Application.dataPath,
				"/CreatureAnim/",
				info.id,
				"_Child"
			}));
			File.WriteAllText(string.Concat(new object[]
			{
				directoryInfo.FullName,
				"/json.txt"
			}), renderer.skeletonDataAsset.skeletonJSON.text);
			int num = 0;
			foreach (AtlasAsset atlasAsset in renderer.skeletonDataAsset.atlasAssets)
			{
				File.WriteAllText(string.Concat(new object[]
				{
					directoryInfo.FullName,
					"/atlas_" + num.ToString() + ".txt"
				}), atlasAsset.atlasFile.text);
				int num2 = 0;
				Material[] materials = atlasAsset.materials;
				for (int j = 0; j < materials.Length; j++)
				{
					byte[] bytes = Add_On.duplicateTexture(materials[j].mainTexture as Texture2D).EncodeToPNG();
					File.WriteAllBytes(string.Concat(new object[]
					{
						directoryInfo.FullName,
						string.Concat(new string[]
						{
							"/texture_",
							num.ToString(),
							"_",
							num2.ToString(),
							".png"
						})
					}), bytes);
					num2++;
				}
				num++;
			}
		}

		// Token: 0x060044B3 RID: 17587 RVA: 0x001A6C08 File Offset: 0x001A4E08
		public void extractCreatureData()
		{
			foreach (CreatureTypeInfo creatureTypeInfo in CreatureTypeList.instance.GetList())
			{
				try
				{
					SkeletonRenderer skeletonRenderer = null;
					GameObject gameObject = null;
					if (creatureTypeInfo.animSrc != null)
					{
						gameObject = Prefab.LoadPrefab(creatureTypeInfo.animSrc);
					}
					if (gameObject != null)
					{
						gameObject.active = false;
						if (gameObject.GetComponent<CreatureAnimScript>().animator != null)
						{
							skeletonRenderer = gameObject.GetComponent<CreatureAnimScript>().animator.GetComponent<SkeletonRenderer>();
						}
					}
					if (skeletonRenderer != null)
					{
						this.OutPutSkeletonRenderer(creatureTypeInfo, skeletonRenderer);
						if (creatureTypeInfo.childTypeInfo != null)
						{
							gameObject = Prefab.LoadPrefab(creatureTypeInfo.childTypeInfo.animSrc);
							gameObject.active = false;
							skeletonRenderer = gameObject.GetComponent<CreatureAnimScript>().animator.GetComponent<SkeletonRenderer>();
							this.OutPutSkeletonRendererChild(creatureTypeInfo, skeletonRenderer);
						}
					}
					else
					{
						SpriteRenderer component = gameObject.GetComponent<CreatureAnimScript>().animator.GetComponent<SpriteRenderer>();
						if (component != null)
						{
							FileSystemInfo fileSystemInfo = Directory.CreateDirectory(string.Concat(new object[]
							{
								Application.dataPath,
								"/CreatureAnim/",
								creatureTypeInfo.id,
								"_NoSpine"
							}));
							byte[] bytes = Add_On.duplicateTexture(component.sprite.texture).EncodeToPNG();
							File.WriteAllBytes(fileSystemInfo.FullName + "/Texture.png", bytes);
						}
						else
						{
							Directory.CreateDirectory(string.Concat(new object[]
							{
								Application.dataPath,
								"/CreatureAnim/",
								creatureTypeInfo.id,
								"_none"
							}));
						}
					}
				}
				catch (Exception ex)
				{
					File.WriteAllText(string.Concat(new object[]
					{
						Application.dataPath,
						"/BaseMods/error_",
						creatureTypeInfo.id,
						".txt"
					}), ex.Message + Environment.NewLine + creatureTypeInfo.id);
				}
			}
		}

		// Token: 0x060044B4 RID: 17588 RVA: 0x001A6E10 File Offset: 0x001A5010
		public void extractAgentAnimationData(List<WorkerSpineAnimatorData> data)
		{
			int num = 0;
			foreach (WorkerSpineAnimatorData workerSpineAnimatorData in data)
			{
				foreach (AtlasAsset atlasAsset in workerSpineAnimatorData.skeletonData.atlasAssets)
				{
					int num2 = 0;
					File.WriteAllText(string.Concat(new string[]
					{
						Application.dataPath,
						"/Textures/atlas_json/",
						workerSpineAnimatorData.name,
						"/atlas_",
						num.ToString(),
						".txt"
					}), atlasAsset.atlasFile.text);
					Material[] materials = atlasAsset.materials;
					for (int j = 0; j < materials.Length; j++)
					{
						Texture2D tex = Add_On.duplicateTexture(materials[j].mainTexture as Texture2D);
						File.WriteAllBytes(string.Concat(new string[]
						{
							Application.dataPath,
							"/Textures/",
							workerSpineAnimatorData.name,
							"_",
							num.ToString(),
							"_",
							num2.ToString(),
							".png"
						}), tex.EncodeToPNG());
						num2++;
					}
					num++;
				}
			}
		}

		// Token: 0x060044B5 RID: 17589 RVA: 0x001A6F84 File Offset: 0x001A5184
		public void ApplyDataSkel(SkeletonRenderer renderer, string Path)
		{
			if (File.Exists(Path + "/json.txt"))
			{
				string text = File.ReadAllText(Path + "/json.txt");
				List<Atlas> list = new List<Atlas>();
				foreach (AtlasAsset atlasAsset in renderer.skeletonDataAsset.atlasAssets)
				{
					list.Add(atlasAsset.GetAtlas());
				}
				AtlasAttachmentLoader attachmentLoader = new AtlasAttachmentLoader(list.ToArray());
				SkeletonData sd = SkeletonDataAsset.ReadSkeletonData(text, attachmentLoader, renderer.skeletonDataAsset.scale);
				renderer.skeletonDataAsset.InitializeWithData(sd);
			}
			int num = 0;
			foreach (AtlasAsset atlasAsset2 in renderer.skeletonDataAsset.atlasAssets)
			{
				int num2 = 0;
				foreach (Material material in atlasAsset2.materials)
				{
					if (File.Exists(string.Concat(new object[]
					{
						Path,
						"/texture_",
						num.ToString(),
						"_",
						num2.ToString(),
						".png"
					})))
					{
						byte[] data = File.ReadAllBytes(string.Concat(new object[]
						{
							Path,
							"/texture_",
							num.ToString(),
							"_",
							num2.ToString(),
							".png"
						}));
						(material.mainTexture as Texture2D).LoadImage(data);
					}
					num2++;
				}
				num++;
			}
		}

		// Token: 0x060044B6 RID: 17590 RVA: 0x001A7114 File Offset: 0x001A5314
		public void FindNewSkinandSkel(WorkerSpineAnimatorData data, Dictionary<string, string> dic, Dictionary<string, string> dic2, Dictionary<string, string> dic_c, Dictionary<string, string> dic2_c)
		{
			data.LoadData();
			if (dic.ContainsKey(data.name))
			{
				string nskel = dic[data.name];
				this.FNSS_skel(data, nskel);
				dic_c.Remove(data.name);
			}
			if (dic2.ContainsKey(data.name))
			{
				this.FNSS_skin(data, dic2[data.name]);
				dic2_c.Remove(data.name);
			}
		}

		// Token: 0x060044B7 RID: 17591 RVA: 0x001A7188 File Offset: 0x001A5388
		public void FNSS_skel(WorkerSpineAnimatorData data, string nskel)
		{
			AtlasAttachmentLoader attachmentLoader = new AtlasAttachmentLoader(data.skeletonData.GetAtlasArray());
			SkeletonData sd = SkeletonDataAsset.ReadSkeletonData(nskel, attachmentLoader, data.skeletonData.scale);
			data.skeletonData.InitializeWithData(sd);
		}

		// Token: 0x060044B8 RID: 17592 RVA: 0x001A71C8 File Offset: 0x001A53C8
		public void FNSS_skin(WorkerSpineAnimatorData data, string dir)
		{
			if (data.skeletonData.atlasAssets != null && data.skeletonData.atlasAssets.Length != 0)
			{
				int num = 0;
				foreach (AtlasAsset atlasAsset in data.skeletonData.atlasAssets)
				{
					int num2 = 0;
					foreach (Material material in atlasAsset.materials)
					{
						if (File.Exists(string.Concat(new string[]
						{
							dir,
							"/texture_",
							num.ToString(),
							"_",
							num2.ToString(),
							".png"
						})))
						{
							Texture mainTexture = material.mainTexture;
							byte[] data2 = File.ReadAllBytes(string.Concat(new string[]
							{
								dir,
								"/texture_",
								num.ToString(),
								"_",
								num2.ToString(),
								".png"
							}));
							(material.mainTexture as Texture2D).LoadImage(data2);
						}
						num2++;
					}
					num++;
				}
			}
		}

		// Token: 0x060044B9 RID: 17593 RVA: 0x001A72EC File Offset: 0x001A54EC
		public void FNSS_skel(WorkerSpineAnimatorData data, byte[] nskel)
		{
			AtlasAttachmentLoader attachmentLoader = new AtlasAttachmentLoader(data.skeletonData.GetAtlasArray());
			SkeletonData sd = SkeletonDataAsset.ReadSkeletonData(nskel, attachmentLoader, data.skeletonData.scale);
			data.skeletonData.InitializeWithData(sd);
		}

		// Token: 0x060044BA RID: 17594 RVA: 0x001A732C File Offset: 0x001A552C
		public void FindNewSkinandSkel(WorkerSpineAnimatorData data, Dictionary<string, object> dic, Dictionary<string, string> dic2, Dictionary<string, object> dic_c, Dictionary<string, string> dic2_c)
		{
			data.LoadData();
			if (dic.ContainsKey(data.name))
			{
				if (dic[data.name] is string)
				{
					this.FNSS_skel(data, (string)dic[data.name]);
				}
				else
				{
					this.FNSS_skel(data, (byte[])dic[data.name]);
				}
				dic_c.Remove(data.name);
			}
			if (dic2.ContainsKey(data.name))
			{
				this.FNSS_skin(data, dic2[data.name]);
				dic2_c.Remove(data.name);
			}
		}

		// Token: 0x060044BB RID: 17595 RVA: 0x001A73D0 File Offset: 0x001A55D0
		public void GetClipInfo(WorkerSpineAnimatorData workerSpineAnimatorData)
		{
			try
			{
				string fullName = Directory.CreateDirectory(Application.dataPath + "/SpineClips/" + workerSpineAnimatorData.name).FullName;
				if (workerSpineAnimatorData.animator.animationClips.Length != 0)
				{
					foreach (AnimationClip animationClip in workerSpineAnimatorData.animator.animationClips)
					{
						string fullName2 = Directory.CreateDirectory(fullName + "/" + animationClip.name).FullName;
					}
				}
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/SSerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}

		// <Patch>
		public bool GetDataWithCheck_Mod(LobotomyBaseMod.KeyValuePairSS name, out WorkerSpineAnimatorData output)
		{
			return this.nameDicMod.TryGetValue(name, out output);
		}

		// <Patch>
		public void FindNewSkinandSkel_Mod(WorkerSpineAnimatorData data, Dictionary<LobotomyBaseMod.KeyValuePairSS, object> dic, Dictionary<LobotomyBaseMod.KeyValuePairSS, string> dic2, Dictionary<LobotomyBaseMod.KeyValuePairSS, object> dic_c, Dictionary<LobotomyBaseMod.KeyValuePairSS, string> dic2_c)
		{
			data.LoadData();
			foreach (LobotomyBaseMod.KeyValuePairSS keyValuePairSS in dic.Keys.ToList<LobotomyBaseMod.KeyValuePairSS>())
			{
				if (keyValuePairSS.value == data.name)
				{
					if (dic[keyValuePairSS] is string)
					{
						this.FNSS_skel(data, (string)dic[keyValuePairSS]);
					}
					else
					{
						this.FNSS_skel(data, (byte[])dic[keyValuePairSS]);
					}
					dic_c.Remove(keyValuePairSS);
				}
			}
			foreach (LobotomyBaseMod.KeyValuePairSS keyValuePairSS2 in dic2.Keys.ToList<LobotomyBaseMod.KeyValuePairSS>())
			{
				if (keyValuePairSS2.value == data.name)
				{
					this.FNSS_skin(data, dic2[keyValuePairSS2]);
					dic2_c.Remove(keyValuePairSS2);
				}
			}
		}

		// Token: 0x04003F52 RID: 16210
		private static WorkerSpineAnimatorManager _instance;

		// Token: 0x04003F53 RID: 16211
		private bool _isLoaded;

		// Token: 0x04003F54 RID: 16212
		private Dictionary<string, WorkerSpineAnimatorData> nameDic = new Dictionary<string, WorkerSpineAnimatorData>();

		// Token: 0x04003F55 RID: 16213
		private Dictionary<int, WorkerSpineAnimatorData> idDic = new Dictionary<int, WorkerSpineAnimatorData>();

		// Token: 0x04003F56 RID: 16214
		public static WorkerSpineAnimatorData basicspecial;

		// <Patch>
		private Dictionary<LobotomyBaseMod.KeyValuePairSS, WorkerSpineAnimatorData> nameDicMod = new Dictionary<LobotomyBaseMod.KeyValuePairSS, WorkerSpineAnimatorData>();
	}
}
