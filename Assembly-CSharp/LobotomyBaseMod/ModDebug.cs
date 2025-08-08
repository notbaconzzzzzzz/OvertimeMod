using System;
using System.IO;
using System.Reflection;
using Harmony;
using UnityEngine;

namespace LobotomyBaseMod
{
	public class ModDebug
	{
		public static bool CheckLogFileExist()
		{
			return File.Exists(ModDebug.LogFilePath);
		}

		public static void FileInit()
		{
			bool flag = !ModDebug.Inited;
			if (flag)
			{
				HarmonyInstance harmonyInstance = HarmonyInstance.Create("Lobotomy.abcdcode.MidnightEGO");
				MethodInfo method = typeof(ModDebug).GetMethod("Debug_Log", AccessTools.all);
				harmonyInstance.Patch(typeof(Debug).GetMethod("Log", AccessTools.all, null, new Type[]
				{
					typeof(object)
				}, null), null, new HarmonyMethod(method), null);
				method = typeof(ModDebug).GetMethod("Debug_LogError", AccessTools.all);
				harmonyInstance.Patch(typeof(Debug).GetMethod("LogError", AccessTools.all, null, new Type[]
				{
					typeof(object)
				}, null), null, new HarmonyMethod(method), null);
				ModDebug.Inited = true;
			}
			bool flag2 = !Directory.Exists(ModDebug.BaseModFolderPath);
			if (flag2)
			{
				Directory.CreateDirectory(ModDebug.BaseModFolderPath);
			}
			File.WriteAllText(ModDebug.LogFilePath, "");
		}

		public static void Debug_LogError(object message)
		{
			string str = message.ToString();
			ModDebug.Log(str + Environment.NewLine + Environment.StackTrace);
		}

		public static void Debug_Log(object message)
		{
			bool flag = message == null;
			if (flag)
			{
				ModDebug.Log("NULL OBJ" + Environment.NewLine + Environment.StackTrace);
			}
			else
			{
				string str = message.ToString();
				ModDebug.Log(str + Environment.NewLine + Environment.StackTrace);
			}
		}

		public static void Log(string msg)
		{
			bool flag = !ModDebug.CheckLogFileExist();
			if (flag)
			{
				ModDebug.FileInit();
			}
			using (StreamWriter streamWriter = new StreamWriter(ModDebug.LogFilePath, true))
			{
				streamWriter.WriteLine(msg);
			}
		}

		public static string LogFilePath
		{
			get
			{
				return ModDebug.BaseModFolderPath + "/Log.txt";
			}
		}

		public static string BaseModFolderPath
		{
			get
			{
				return Application.persistentDataPath + "/LobotomyBaseMod";
			}
		}

		public static bool Inited;
	}
}
