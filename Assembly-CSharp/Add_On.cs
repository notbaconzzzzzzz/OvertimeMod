using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Spine.Unity;
using UnityEngine;

// Token: 0x02000CD2 RID: 3282
public class Add_On
{
	// Token: 0x0600634B RID: 25419 RVA: 0x0004DAE9 File Offset: 0x0004BCE9
	public Add_On()
	{
		this.AssemList = new List<Assembly>();
	}

	// Token: 0x0600634C RID: 25420 RVA: 0x0022E348 File Offset: 0x0022C548
	public void init()
	{
		try
		{
			Add_On.SaveBackUp();
		}
		catch (Exception ex)
		{
			File.WriteAllText(Application.dataPath + "/BaseMods/BackUperror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
		}
		this.AssemList = new List<Assembly>();
		this.EffectList = new Dictionary<string, SkeletonDataAsset>();
		DirectoryInfo[] directories = new DirectoryInfo(Application.dataPath + "/BaseMods").GetDirectories();
		this.DirList = new List<DirectoryInfo>();
		for (int i = 0; i < directories.Length; i++)
		{
			this.DirList.Add(directories[i]);
			if (Directory.Exists(directories[i].FullName + "/CustomEffect"))
			{
				foreach (DirectoryInfo directoryInfo in new DirectoryInfo(directories[i].FullName + "/CustomEffect").GetDirectories())
				{
					try
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
						SkeletonDataAsset value = SkeletonDataAsset.CreateRuntimeInstance(File.ReadAllText(directoryInfo.FullName + "/json.txt"), atlasAsset, true, 0.01f);
						this.EffectList.Add(directoryInfo.Name, value);
					}
					catch (Exception ex2)
					{
						File.WriteAllText(Application.dataPath + "/BaseMods/Initerror.txt", ex2.Message + Environment.NewLine + ex2.StackTrace);
					}
				}
			}
			foreach (FileInfo fileInfo2 in directories[i].GetFiles())
			{
				if (fileInfo2.Name.Contains(".dll"))
				{
					foreach (Type type in Assembly.LoadFile(fileInfo2.FullName).GetTypes())
					{
						if (type.Name == "Harmony_Patch")
						{
							try
							{
								Activator.CreateInstance(type);
							}
							catch (Exception ex3)
							{
								File.WriteAllText(Application.dataPath + "/BaseMods/Herror.txt", ex3.Message + Environment.NewLine + ex3.StackTrace);
							}
						}
					}
					this.AssemList.Add(Assembly.LoadFile(fileInfo2.FullName));
				}
			}
		}
		Add_On.instance = this;
	}

	// Token: 0x0600634D RID: 25421 RVA: 0x0004DB07 File Offset: 0x0004BD07
	public static bool Loading(string Class, string Method, object[] values)
	{
		if (Add_On.instance != null)
		{
			return false;
		}
		Add_On.instance = new Add_On();
		Add_On.instance.init();
		return false;
	}

	// Token: 0x0600634E RID: 25422 RVA: 0x0004DB27 File Offset: 0x0004BD27
	public static object[] Loading_Return(string Class, string Method, object[] values)
	{
		if (Add_On.instance != null)
		{
			return null;
		}
		Add_On.instance = new Add_On();
		Add_On.instance.init();
		return null;
	}

	// Token: 0x0600634F RID: 25423 RVA: 0x0013B568 File Offset: 0x00139768
	public static DirectoryInfo CheckNamedDir(DirectoryInfo dir, string name)
	{
		foreach (DirectoryInfo directoryInfo in dir.GetDirectories())
		{
			if (directoryInfo.Name == name)
			{
				return directoryInfo;
			}
		}
		return null;
	}

	// Token: 0x06006350 RID: 25424 RVA: 0x0022E64C File Offset: 0x0022C84C
	static Add_On()
	{
		Add_On.version = "5.2";
		Add_On.instance = new Add_On();
		Add_On.instance.init();
		Add_On.Portraits = new Dictionary<string, Sprite>();
		Add_On.Error_Report = Application.dataPath + "/BaseMods/";
	}

	// Token: 0x06006351 RID: 25425 RVA: 0x0022E73C File Offset: 0x0022C93C
	public static Texture2D duplicateTexture(Texture2D source)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
		Graphics.Blit(source, temporary);
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = temporary;
		Texture2D texture2D = new Texture2D(source.width, source.height);
		texture2D.ReadPixels(new Rect(0f, 0f, (float)temporary.width, (float)temporary.height), 0, 0);
		texture2D.Apply();
		RenderTexture.active = active;
		RenderTexture.ReleaseTemporary(temporary);
		return texture2D;
	}

	// Token: 0x06006352 RID: 25426 RVA: 0x0022E7BC File Offset: 0x0022C9BC
	public static Sprite GetPortrait(string portraitSrc)
	{
		if (Add_On.Portraits.ContainsKey(portraitSrc))
		{
			return Add_On.Portraits[portraitSrc];
		}
		string[] array = portraitSrc.Split(new char[]
		{
			'/'
		});
		Sprite result = null;
		if (array[0] == "Custom")
		{
			Sprite sprite = Resources.Load<Sprite>("Sprites/Unit/creature/AuthorNote");
			foreach (DirectoryInfo directoryInfo in Add_On.instance.DirList)
			{
				string path = directoryInfo.FullName + "/Creature/Portrait/" + array[1] + ".png";
				if (File.Exists(path))
				{
					byte[] data = File.ReadAllBytes(path);
					Texture2D texture2D = new Texture2D(2, 2);
					texture2D.LoadImage(data);
					Add_On.Portraits.Add(portraitSrc, Sprite.Create(texture2D, sprite.rect, sprite.pivot, sprite.pixelsPerUnit, 0U, SpriteMeshType.Tight, sprite.border));
					return Add_On.Portraits[portraitSrc];
				}
			}
			return result;
		}
		result = Resources.Load<Sprite>(portraitSrc);
		return result;
	}

	// Token: 0x06006353 RID: 25427 RVA: 0x0022E8E4 File Offset: 0x0022CAE4
	public static Texture2D GetPortrait_OnlyTexture(string portraitSrc)
	{
		string[] array = portraitSrc.Split(new char[]
		{
			'/'
		});
		if (array[0] == "Custom")
		{
			foreach (DirectoryInfo directoryInfo in Add_On.instance.DirList)
			{
				string path = directoryInfo.FullName + "/Creature/Portrait/" + array[1] + ".png";
				if (File.Exists(path))
				{
					byte[] data = File.ReadAllBytes(path);
					Texture2D texture2D = new Texture2D(2, 2);
					texture2D.LoadImage(data);
					return texture2D;
				}
			}
		}
		return Add_On.duplicateTexture(Resources.Load<Sprite>(portraitSrc).texture);
	}

	// Token: 0x06006354 RID: 25428 RVA: 0x0022E9A4 File Offset: 0x0022CBA4
	public static DirectoryInfo GetBackDir()
	{
		if (Directory.Exists(Application.dataPath + "/Managed/BaseMod/BackUp"))
		{
			return new DirectoryInfo(Application.dataPath + "/Managed/BaseMod/BackUp");
		}
		return Directory.CreateDirectory(Application.dataPath + "/Managed/BaseMod/BackUp");
	}

	// Token: 0x06006355 RID: 25429 RVA: 0x0022E9F0 File Offset: 0x0022CBF0
	public static void UpdatingBackUps(DirectoryInfo back, DirectoryInfo save)
	{
		DirectoryInfo[] directories = back.GetDirectories();
		List<DirectoryInfo> list = new List<DirectoryInfo>();
		foreach (DirectoryInfo item in directories)
		{
			list.Add(item);
		}
		if (list.Count >= 40)
		{
			int count = list.Count;
			for (int j = 0; j < count - 39; j++)
			{
				long num = long.MaxValue;
				DirectoryInfo directoryInfo = null;
				foreach (DirectoryInfo directoryInfo2 in list)
				{
					long num2 = directoryInfo2.CreationTime.ToFileTime();
					if (num2 < num)
					{
						num = num2;
						directoryInfo = directoryInfo2;
					}
				}
				if (directoryInfo != null)
				{
					Directory.Delete(directoryInfo.FullName, true);
					list.Remove(directoryInfo);
				}
			}
		}
		DateTime now = DateTime.Now;
		Add_On.CopyingSave(Directory.CreateDirectory(string.Concat(new object[]
		{
			back.FullName,
			"/",
			now.Year,
			"_",
			now.Month,
			"_",
			now.Day,
			"_",
			now.Hour,
			"_",
			now.Minute,
			"_",
			now.Second
		})), save);
	}

	// Token: 0x06006356 RID: 25430 RVA: 0x0022EB8C File Offset: 0x0022CD8C
	public static void CopyingSave(DirectoryInfo newback, DirectoryInfo save)
	{
		if (save.GetDirectories().Length != 0)
		{
			foreach (DirectoryInfo directoryInfo in save.GetDirectories())
			{
				Add_On.CopyingSave(Directory.CreateDirectory(newback.FullName + "/" + directoryInfo.Name), directoryInfo);
			}
		}
		if (save.GetFiles().Length != 0)
		{
			foreach (FileInfo fileInfo in save.GetFiles())
			{
				File.Copy(fileInfo.FullName, newback.FullName + "/" + fileInfo.Name);
			}
		}
	}

	// Token: 0x06006357 RID: 25431 RVA: 0x0022EC24 File Offset: 0x0022CE24
	public static void SaveBackUp()
	{
		DirectoryInfo backDir = Add_On.GetBackDir();
		DirectoryInfo parent = new DirectoryInfo(Application.persistentDataPath).Parent;
		Add_On.UpdatingBackUps(backDir, parent);
	}

	// Token: 0x0400593E RID: 22846
	private string DllList;

	// Token: 0x0400593F RID: 22847
	public List<Assembly> AssemList;

	// Token: 0x04005940 RID: 22848
	public static Add_On instance;

	// Token: 0x04005941 RID: 22849
	public Dictionary<string, object> ObjectList = new Dictionary<string, object>();

	// Token: 0x04005942 RID: 22850
	public List<DirectoryInfo> DirList;

	// Token: 0x04005943 RID: 22851
	public static string version;

	// Token: 0x04005944 RID: 22852
	public static string[] NoBoostList = new string[]
	{
		"MagicalGirlWeapon",
		"DeathAngelWeapon",
		"RedHoodWeapon",
		"BigBadWolfWeapon",
		"BossBirdWeapon",
		"DangoCreatureWeapon",
		"ButterflyWeapon",
		"BlackSwanWeapon",
		"SharkWeapon",
		"KnightOfDespairWeapon",
		"CensoredWeapon",
		"PorccuWeapon",
		"BakuWeapon",
		"FireBirdWeapon",
		"PinkCorpsWeapon",
		"PianoWeapon",
		"FreischutzWeapon",
		"YinWeapon"
	};

	// Token: 0x04005945 RID: 22853
	private static Dictionary<string, Sprite> Portraits;

	// Token: 0x04005946 RID: 22854
	public Dictionary<string, SkeletonDataAsset> EffectList;

	// Token: 0x04005947 RID: 22855
	public static string Error_Report;
}
