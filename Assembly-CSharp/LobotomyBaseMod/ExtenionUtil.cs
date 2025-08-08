using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace LobotomyBaseMod
{
	public static class ExtenionUtil
	{
		public static Sprite CreateSpriteByPng(string filepath)
		{
			bool flag = File.Exists(filepath);
			Sprite result;
			if (flag)
			{
				Texture2D texture2D = new Texture2D(2, 2);
				texture2D.LoadImage(File.ReadAllBytes(filepath));
				Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0f, 0f), 100f, 0U, SpriteMeshType.FullRect);
				result = sprite;
			}
			else
			{
				result = null;
			}
			return result;
		}

		public static T GetTypeInstance<T>(string typename)
		{
			object obj = null;
			foreach (Assembly assembly in Add_On.instance.AssemList)
			{
				foreach (Type type in assembly.GetTypes())
				{
					bool flag = type.Name == typename;
					if (flag)
					{
						obj = Activator.CreateInstance(type);
						return (T)((object)obj);
					}
				}
			}
			bool flag2 = obj == null;
			if (flag2)
			{
				obj = Activator.CreateInstance(Type.GetType(typename));
			}
			return (T)((object)obj);
		}

		public static Type GetType(string typename)
		{
			foreach (Assembly assembly in Add_On.instance.AssemList)
			{
				foreach (Type type in assembly.GetTypes())
				{
					bool flag = type.Name == typename;
					if (flag)
					{
						return type;
					}
				}
			}
			return Type.GetType(typename);
		}

		public static bool TryGetValue<T>(Dictionary<string, object> dic, string name, ref T field)
		{
			object obj;
			bool flag = dic.TryGetValue(name, out obj) && obj is T;
			bool result;
			if (flag)
			{
				field = (T)((object)obj);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}
	}
}
