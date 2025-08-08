using System;
using System.Collections.Generic;
using System.IO;
using LobotomyBaseModLib;
using UnityEngine;

namespace LobotomyBaseMod
{
	public class ModAssetBundleManager : Singleton<ModAssetBundleManager>
	{
		public void Init()
		{
			this.bundles = new Dictionary<string, List<AssetBundle>>();
			this.GObjList = new Dictionary<string, List<ModAssetBundleManager.GameObjectBundleCache>>();
			this.GetBundles();
		}

		public void GetBundles()
		{
			this.bundles = new Dictionary<string, List<AssetBundle>>();
			foreach (ModInfo modInfo in Add_On.instance.ModList)
			{
				DirectoryInfo directoryInfo = modInfo.modpath.CheckNamedDir(ModAssetBundleManager.PathDir);
				bool flag = directoryInfo != null;
				if (flag)
				{
					bool flag2 = !this.bundles.ContainsKey(modInfo.modid);
					if (flag2)
					{
						this.bundles[modInfo.modid] = new List<AssetBundle>();
						this.GObjList[modInfo.modid] = new List<ModAssetBundleManager.GameObjectBundleCache>();
					}
					this.GetBundles(modInfo.modid, directoryInfo);
				}
			}
		}

		public void GetBundles(string modid, DirectoryInfo curdic)
		{
			foreach (DirectoryInfo curdic2 in curdic.GetDirectories())
			{
				this.GetBundles(modid, curdic2);
			}
			foreach (FileInfo fileInfo in curdic.GetFiles())
			{
				AssetBundle assetBundle = AssetBundle.LoadFromFile(fileInfo.FullName);
				bool flag = assetBundle != null;
				if (flag)
				{
					assetBundle.name = Path.GetFileNameWithoutExtension(fileInfo.FullName);
					this.bundles[modid].Add(assetBundle);
				}
				else
				{
					ModDebug.Log("Can't load asset - " + Path.GetFileNameWithoutExtension(fileInfo.FullName));
				}
			}
		}

		public GameObject LoadAssetEachScale(Transform parent, Vector3 scale, Vector3 position, KeyValuePairSS name, string bundlename = "")
		{
			GameObject asset = Singleton<ModAssetBundleManager>.Instance.GetAsset(name, bundlename);
			asset.LocalEachScalingAll(scale.x, scale.y, scale.z);
			asset.transform.parent = parent;
			asset.transform.localPosition = position;
			asset.SetActive(true);
			return asset;
		}

		public GameObject LoadAssetEachScale(Transform parent, Vector3 scale, Vector3 position, string name, string bundlename = "")
		{
			return this.LoadAssetEachScale(parent, scale, position, new KeyValuePairSS(string.Empty, name), bundlename);
		}

		public GameObject GetAsset(string name, string bundlename = "")
		{
			return this.GetAsset(new KeyValuePairSS(string.Empty, name), bundlename);
		}

		public GameObject GetAsset(KeyValuePairSS name, string bundlename = "")
		{
			bool flag = this.GObjList.ContainsKey(name.key);
			GameObject result;
			if (flag)
			{
				ModAssetBundleManager.GameObjectBundleCache gameObjectBundleCache = this.GObjList[name.key].Find((ModAssetBundleManager.GameObjectBundleCache x) => x.objname == name.value && (bundlename == "" || x.BundleName == bundlename));
				bool flag2 = gameObjectBundleCache != null;
				if (flag2)
				{
					result = UnityEngine.Object.Instantiate<GameObject>(gameObjectBundleCache.obj);
				}
				else
				{
					foreach (AssetBundle assetBundle in this.bundles[name.key])
					{
						GameObject gameObject = assetBundle.LoadAsset<GameObject>(name.value);
						bool flag3 = gameObject != null;
						if (flag3)
						{
							ModAssetBundleManager.GameObjectBundleCache item = new ModAssetBundleManager.GameObjectBundleCache
							{
								BundleName = assetBundle.name,
								objname = name.value,
								obj = gameObject
							};
							this.GObjList[name.key].Add(item);
							return UnityEngine.Object.Instantiate<GameObject>(gameObject);
						}
					}
					result = null;
				}
			}
			else
			{
				ModDebug.Log("AssetBundleLoader - Wrong modid : " + name.key);
				result = null;
			}
			return result;
		}

		public Dictionary<string, List<AssetBundle>> bundles;

		public Dictionary<string, List<ModAssetBundleManager.GameObjectBundleCache>> GObjList;

		public static string PathDir = "BaseModAssetBundle";

		public class GameObjectBundleCache
		{
			public string objname;

			public string BundleName;

			public GameObject obj;
		}
	}
}
