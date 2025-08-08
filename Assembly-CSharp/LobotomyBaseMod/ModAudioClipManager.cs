using System;
using System.Collections.Generic;
using System.IO;
using LobotomyBaseModLib;
using NAudio.Wave;
using UnityEngine;

namespace LobotomyBaseMod
{
	public class ModAudioClipManager : Singleton<ModAudioClipManager>
	{
		public void Init()
		{
			this.AudioClipPathCaching();
			this.AudioClipDic = new CacheDic<KeyValuePairSS, AudioClip>(new CacheDic<KeyValuePairSS, AudioClip>.getdele(this.AudioClipFinding));
		}

		public AudioClip GetAudioClip(string modid, string spritename)
		{
			return this.GetAudioClip(new KeyValuePairSS(modid, spritename));
		}

		public AudioClip GetAudioClip(KeyValuePairSS SS)
		{
			return this.AudioClipDic[SS];
		}

		private AudioClip AudioClipFinding(KeyValuePairSS id)
		{
			bool flag = this.AudioClipPathCache.ContainsKey(id);
			if (flag)
			{
				try
				{
					string path = this.AudioClipPathCache[id];
					string extension = Path.GetExtension(path);
					bool flag2 = extension == ".wav";
					AudioClip result;
					if (flag2)
					{
						result = ModAudioClipManager.WavtoAudioClip(path);
					}
					else
					{
						bool flag3 = extension == ".mp3";
						if (!flag3)
						{
							ModDebug.Log("GetAudioClip error path : " + this.AudioClipPathCache[id]);
							ModDebug.Log("None Support Extension");
							this.AudioClipPathCache.Remove(id);
							return null;
						}
						result = ModAudioClipManager.mp3toAudioClip(path);
					}
					this.AudioClipPathCache.Remove(id);
					return result;
				}
				catch (Exception ex)
				{
					ModDebug.Log("GetAudioClip error path : " + this.AudioClipPathCache[id]);
					ModDebug.Log("GetAudioClip error - " + ex.Message + Environment.NewLine + ex.StackTrace);
					this.AudioClipPathCache.Remove(id);
					return null;
				}
			}
			return null;
		}

		private void AudioClipPathCaching()
		{
			this.AudioClipPathCache = new Dictionary<KeyValuePairSS, string>();
			foreach (ModInfo modInfo in Add_On.instance.ModList)
			{
				DirectoryInfo directoryInfo = modInfo.modpath.CheckNamedDir(ModAudioClipManager.PathDir);
				bool flag = directoryInfo != null;
				if (flag)
				{
					this.AudioClipPathCaching(modInfo.modid, directoryInfo);
				}
			}
		}

		private void AudioClipPathCaching(string modid, DirectoryInfo curdic)
		{
			foreach (DirectoryInfo curdic2 in curdic.GetDirectories())
			{
				this.AudioClipPathCaching(modid, curdic2);
			}
			foreach (FileInfo fileInfo in curdic.GetFiles())
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.FullName);
				this.AudioClipPathCache[new KeyValuePairSS(modid, fileNameWithoutExtension)] = fileInfo.FullName;
			}
		}

		public static AudioClip mp3toAudioClip(string path)
		{
			Mp3FileReader sourceProvider = new Mp3FileReader(path);
			WaveFileWriter.CreateWaveFile(path + ".wav", sourceProvider);
			AudioClip result = ModAudioClipManager.WavtoAudioClip(path + ".wav");
			File.Delete(path + ".wav");
			return result;
		}

		public static AudioClip WavtoAudioClip(string path)
		{
			byte[] wav = File.ReadAllBytes(path);
			WAV wav2 = new WAV(wav);
			AudioClip audioClip = AudioClip.Create("Default", wav2.SampleCount, 1, wav2.Frequency, false);
			audioClip.SetData(wav2.LeftChannel, 0);
			return audioClip;
		}

		public Dictionary<KeyValuePairSS, string> AudioClipPathCache;

		public CacheDic<KeyValuePairSS, AudioClip> AudioClipDic;

		public static string PathDir = "BaseModAudioClip";
	}
}
