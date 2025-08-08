using System;
using System.Collections.Generic;
using System.IO;
using LobotomyBaseModLib;
using UnityEngine;

namespace LobotomyBaseMod
{
	public class ModArtWorkManager : Singleton<ModArtWorkManager>
	{
		public void Init()
		{
			this.ArtWorkPathCaching();
			this.ArtWorkDic = new CacheDic<KeyValuePairSS, Sprite>(new CacheDic<KeyValuePairSS, Sprite>.getdele(this.ArtWorkFinding));
		}

		public Sprite GetArtWork(string modid, string spritename)
		{
			return this.GetArtWork(new KeyValuePairSS(modid, spritename));
		}

		public Sprite GetArtWork(KeyValuePairSS SS)
		{
			return this.ArtWorkDic[SS];
		}

		private Sprite ArtWorkFinding(KeyValuePairSS id)
		{
			bool flag = this.ArtWorkPathCache.ContainsKey(id);
			if (flag)
			{
				try
				{
					Sprite result = ExtenionUtil.CreateSpriteByPng(this.ArtWorkPathCache[id]);
					this.ArtWorkPathCache.Remove(id);
					return result;
				}
				catch (Exception ex)
				{
					ModDebug.Log("GetArtWork error path : " + this.ArtWorkPathCache[id]);
					ModDebug.Log("GetArtWork error - " + ex.Message + Environment.NewLine + ex.StackTrace);
					this.ArtWorkPathCache.Remove(id);
					return null;
				}
			}
			return null;
		}

		private void ArtWorkPathCaching()
		{
			this.ArtWorkPathCache = new Dictionary<KeyValuePairSS, string>();
			foreach (ModInfo modInfo in Add_On.instance.ModList)
			{
				DirectoryInfo directoryInfo = modInfo.modpath.CheckNamedDir(ModArtWorkManager.PathDir);
				bool flag = directoryInfo != null;
				if (flag)
				{
					this.ArtWorkPathCaching(modInfo.modid, directoryInfo);
				}
			}
		}

		private void ArtWorkPathCaching(string modid, DirectoryInfo curdic)
		{
			foreach (DirectoryInfo curdic2 in curdic.GetDirectories())
			{
				this.ArtWorkPathCaching(modid, curdic2);
			}
			foreach (FileInfo fileInfo in curdic.GetFiles())
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.FullName);
				this.ArtWorkPathCache[new KeyValuePairSS(modid, fileNameWithoutExtension)] = fileInfo.FullName;
			}
		}

		public Dictionary<KeyValuePairSS, string> ArtWorkPathCache;

		public CacheDic<KeyValuePairSS, Sprite> ArtWorkDic;

		public static string PathDir = "BaseModArtWork";
	}
}
