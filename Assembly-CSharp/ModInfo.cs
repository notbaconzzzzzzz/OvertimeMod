using System;
using System.IO;
using System.Xml;

// Token: 0x02000CEA RID: 3306
public class ModInfo
{
	// Token: 0x060063EE RID: 25582 RVA: 0x0004E104 File Offset: 0x0004C304
	public ModInfo(string folder)
	{
		this.foldername = folder;
	}

	// Token: 0x060063EF RID: 25583 RVA: 0x00235DB4 File Offset: 0x00233FB4
	public ModInfo(DirectoryInfo dir)
	{
		this.foldername = dir.Name;
		if (File.Exists(dir.FullName + "/Info/" + GlobalGameManager.instance.GetCurrentLanguage() + "/info.xml"))
		{
			string xml = File.ReadAllText(dir.FullName + "/Info/" + GlobalGameManager.instance.GetCurrentLanguage() + "/info.xml");
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("/info/name");
			XmlNodeList xmlNodeList = xmlDocument.SelectSingleNode("/info/descs").SelectNodes("desc");
			this.modname = xmlNode.InnerText;
			string str = string.Concat(new string[]
			{
				"Folder : ",
				this.foldername,
				Environment.NewLine,
				"Name : ",
				this.modname,
				Environment.NewLine
			});
			foreach (object obj in xmlNodeList)
			{
				XmlNode xmlNode2 = (XmlNode)obj;
				str = str + Environment.NewLine + xmlNode2.InnerText;
			}
			this.modinfo = str;
			return;
		}
		this.modinfo = "unknown";
		this.modname = this.foldername;
	}
	
	// <Patch>
	public void Init(DirectoryInfo dir)
	{
		this.foldername = dir.Name;
		this.modpath = dir;
		string str = string.Empty;
		string str2 = string.Empty;
		string text = string.Empty;
		this.modid = string.Empty;
		string str3 = GlobalGameManager.instance.GetCurrentLanguage();
		bool flag = File.Exists(dir.FullName + "/Info/" + str3 + "/info.xml");
		bool flag2 = !flag;
		if (flag2)
		{
			str3 = "en";
			flag = File.Exists(dir.FullName + "/Info/" + str3 + "/info.xml");
		}
		bool flag3 = !flag;
		if (flag3)
		{
			str3 = "kr";
			flag = File.Exists(dir.FullName + "/Info/" + str3 + "/info.xml");
		}
		bool flag4 = flag;
		if (flag4)
		{
			string xml = File.ReadAllText(dir.FullName + "/Info/" + str3 + "/info.xml");
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("/info/name");
			XmlNodeList xmlNodeList = xmlDocument.SelectSingleNode("/info/descs").SelectNodes("desc");
			this.modname = xmlNode.InnerText;
			str = string.Concat(new string[]
			{
				"Folder : ",
				this.foldername,
				Environment.NewLine,
				"Name : ",
				this.modname,
				Environment.NewLine
			});
			xmlNode = xmlDocument.SelectSingleNode("/info/ID");
			bool flag5 = xmlNode != null;
			if (flag5)
			{
				str2 = "ID : " + xmlNode.InnerText + Environment.NewLine;
				this.modid = xmlNode.InnerText;
			}
			foreach (object obj in xmlNodeList)
			{
				XmlNode xmlNode2 = (XmlNode)obj;
				text = text + Environment.NewLine + xmlNode2.InnerText;
			}
			bool flag6 = File.Exists(dir.FullName + "/Info/GlobalInfo.xml");
			if (flag6)
			{
				xml = File.ReadAllText(dir.FullName + "/Info/GlobalInfo.xml");
				xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xml);
				xmlNode = xmlDocument.SelectSingleNode("/info/ID");
				bool flag7 = xmlNode != null;
				if (flag7)
				{
					str2 = "ID : " + xmlNode.InnerText + Environment.NewLine;
					this.modid = xmlNode.InnerText;
				}
			}
			this.modinfo = str + str2 + text;
		}
		else
		{
			this.modinfo = "UnKnown";
			this.modname = this.foldername;
		}
	}

	// <Patch>
	public string modid;

	// <Patch>
	public DirectoryInfo modpath;

	// Token: 0x040059D6 RID: 22998
	public string foldername;

	// Token: 0x040059D7 RID: 22999
	public string modname;

	// Token: 0x040059D8 RID: 23000
	public string modinfo;
}
