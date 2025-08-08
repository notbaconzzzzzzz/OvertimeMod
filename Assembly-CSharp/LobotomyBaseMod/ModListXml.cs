using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace LobotomyBaseMod
{
	public class ModListXml
	{
		public static void SerializeData(ModListXml data, string path)
		{
			using (StreamWriter streamWriter = new StreamWriter(path))
			{
				new XmlSerializer(typeof(ModListXml)).Serialize(streamWriter, data);
			}
		}

		public static ModListXml LoadData(string path)
		{
			ModListXml result;
			using (StringReader stringReader = new StringReader(File.ReadAllText(path)))
			{
				result = (ModListXml)new XmlSerializer(typeof(ModListXml)).Deserialize(stringReader);
			}
			return result;
		}

		public List<ModInfoXml> list = new List<ModInfoXml>();
	}
}
