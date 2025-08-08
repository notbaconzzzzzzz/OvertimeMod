using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;

public class SpecialModeConfig
{
    public static SpecialModeConfig instance
    {
        get
        {
			if (_instance == null)
			{
				_instance = new SpecialModeConfig();
				_instance.Load();
			}
			return _instance;
        }
    }

	private static SpecialModeConfig _instance;

    public string persistantData
    {
        get
        {
            string dataPath = Application.persistentDataPath;
		    dataPath = dataPath.Substring(0, dataPath.Length - 9);
            return dataPath + "/SpecialModes_Persistant.dat";
        }
    }

    public string transientData
    {
        get
        {
            return Application.persistentDataPath + "/SpecialModes_Transient.dat";
        }
    }

    public string plainTextFile
    {
        get
        {
            string dataPath = Application.persistentDataPath;
		    dataPath = dataPath.Substring(0, dataPath.Length - 9);
            return dataPath + "/SpecialModes.txt";
        }
    }

    public void Load()
    {
        if (!File.Exists(persistantData) || !File.Exists(transientData))
        {
			if (File.Exists(Application.dataPath + "/Managed/BaseMod/SpecialModes.txt"))
			{
				LoadOld();
				ConsoleScript.instance.ConsoleWnd.gameObject.SetActive(true);
				ConsoleScript.instance.ConsoleWnd.GetComponent<InputField>().text = "SpecialModes.txt migrated to AppData";
			}
			Save();
        }
		else
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream fileStream = File.Open(persistantData, FileMode.Open);
			Dictionary<string, object> dic = (Dictionary<string, object>)binaryFormatter.Deserialize(fileStream);
			fileStream.Close();
			GameUtil.TryGetValue<Dictionary<string, object>>(dic, "BasicModes", ref basicModeDictionaryPersistant);
			GameUtil.TryGetValue<Dictionary<string, object>>(dic, "SpecialModes", ref specialModeDictionaryPersistant);
			GameUtil.TryGetValue<bool>(dic, "OvertimeMode", ref overtimeModePersistant);
			GameUtil.TryGetValue<Dictionary<string, object>>(dic, "OvertimeModes", ref overtimeModeDictionaryPersistant);
			GameUtil.TryGetValue<int>(dic, "ReworkTier", ref reworkTierPersistant);
			GameUtil.TryGetValue<Dictionary<string, object>>(dic, "ReworkModes", ref reworkModeDictionaryPersistant);
			fileStream = File.Open(transientData, FileMode.Open);
			Dictionary<string, object> dic2 = (Dictionary<string, object>)binaryFormatter.Deserialize(fileStream);
			fileStream.Close();
			GameUtil.TryGetValue<Dictionary<string, object>>(dic2, "BasicModes", ref basicModeDictionaryTransient);
			GameUtil.TryGetValue<Dictionary<string, object>>(dic2, "SpecialModes", ref specialModeDictionaryTransient);
			GameUtil.TryGetValue<int>(dic2, "OvertimeMode", ref overtimeModeTransient);
			GameUtil.TryGetValue<Dictionary<string, object>>(dic2, "OvertimeModes", ref overtimeModeDictionaryTransient);
			GameUtil.TryGetValue<int>(dic2, "ReworkTier", ref reworkTierTransient);
			GameUtil.TryGetValue<Dictionary<string, object>>(dic2, "ReworkModes", ref reworkModeDictionaryTransient);
			if (overtimeModeTransient == -1)
			{
				overtimeMode = overtimeModePersistant;
			}
			else
			{
				overtimeMode = overtimeModeTransient == 1;
			}
			if (reworkTierTransient == -1)
			{
				reworkTier = reworkTierPersistant;
			}
			else
			{
				reworkTier = reworkTierTransient;
			}
			bool val = false;
			if (GameUtil.TryGetValue<bool>(overtimeModeDictionaryPersistant, "GreenMidnightRework", ref val))
			{
				reworkModeDictionaryPersistant.Add("GreenMidnightRework", val);
				overtimeModeDictionaryPersistant.Remove("GreenMidnightRework");
			}
			if (GameUtil.TryGetValue<bool>(overtimeModeDictionaryTransient, "GreenMidnightRework", ref val))
			{
				reworkModeDictionaryTransient.Add("GreenMidnightRework", val);
				overtimeModeDictionaryTransient.Remove("GreenMidnightRework");
			}
		}
		ModeDictionary.Clear();
		EnumerateDictionary(basicModeDictionaryTransient);
		EnumerateDictionary(specialModeDictionaryTransient);
		EnumerateDictionary(overtimeModeDictionaryTransient);
		EnumerateDictionary(reworkModeDictionaryTransient);
		EnumerateDictionary(basicModeDictionaryPersistant);
		EnumerateDictionary(specialModeDictionaryPersistant);
		EnumerateDictionary(overtimeModeDictionaryPersistant);
		EnumerateDictionary(reworkModeDictionaryPersistant);
		EnumerateDictionary(basicModeDictionaryDefault);
		EnumerateDictionary(specialModeDictionaryDefault);
		EnumerateDictionary(overtimeMode ? overtimeModeDictionaryOnDefault : overtimeModeDictionaryOffDefault);
		EnumerateDictionaryTiered(reworkModeTierDictionary);
		File.WriteAllText(plainTextFile, GetPlainTextRepresentation());
    }

    public void EnumerateDictionary(Dictionary<string, object> dic)
    {
		if (dic.Count <= 0) return;
		IEnumerator<KeyValuePair<string, object>> enumerator = dic.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
                KeyValuePair<string, object> obj = enumerator.Current;
				if (!ModeDictionary.ContainsKey(obj.Key))
				{
					ModeDictionary.Add(obj.Key, obj.Value);
				}
			}
		}
		catch
		{
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
    }

    public void EnumerateDictionaryTiered(Dictionary<string, object> dic)
    {
		if (dic.Count <= 0) return;
		IEnumerator<KeyValuePair<string, object>> enumerator = dic.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
                KeyValuePair<string, object> obj = enumerator.Current;
				if (!ModeDictionary.ContainsKey(obj.Key))
				{
					ModeDictionary.Add(obj.Key, reworkTier >= (int)obj.Value);
				}
			}
		}
		catch
		{
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
    }

    public void Save()
    {
		Dictionary<string, object> dic = new Dictionary<string, object>();
		dic.Add("BasicModes", basicModeDictionaryPersistant);
		dic.Add("SpecialModes", specialModeDictionaryPersistant);
		dic.Add("OvertimeMode", overtimeModePersistant);
		dic.Add("OvertimeModes", overtimeModeDictionaryPersistant);
		dic.Add("ReworkTier", reworkTierPersistant);
		dic.Add("ReworkModes", reworkModeDictionaryPersistant);
		SaveUtil.WriteSerializableFile(persistantData, dic);
		Dictionary<string, object> dic2 = new Dictionary<string, object>();
		dic2.Add("BasicModes", basicModeDictionaryTransient);
		dic2.Add("SpecialModes", specialModeDictionaryTransient);
		dic2.Add("OvertimeMode", overtimeModeTransient);
		dic2.Add("OvertimeModes", overtimeModeDictionaryTransient);
		dic2.Add("ReworkTier", reworkTierTransient);
		dic2.Add("ReworkModes", reworkModeDictionaryTransient);
		SaveUtil.WriteSerializableFile(transientData, dic2);
		File.WriteAllText(plainTextFile, GetPlainTextRepresentation());
    }

	public string GetPlainTextRepresentation()
	{
		string txt = "";
		txt += "DO NOT MODIFY VALUES ON THIS FILE";
		txt += Environment.NewLine;
		txt += "They will not be updated ingame";
		txt += Environment.NewLine;
		txt += Environment.NewLine;
		txt += Environment.NewLine;
		txt += "        Basic Modes:";
		IEnumerator<KeyValuePair<string, object>> enumerator = basicModeDictionaryDefault.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				txt += Environment.NewLine;
				string line = "";
                KeyValuePair<string, object> obj = enumerator.Current;
				string mode = obj.Key;
				object defaultValue = obj.Value;
				object persistantValue = null;
				basicModeDictionaryPersistant.TryGetValue(mode, out persistantValue);
				object transientValue = null;
				basicModeDictionaryTransient.TryGetValue(mode, out transientValue);
				object actualValue = null;
				ModeDictionary.TryGetValue(mode, out actualValue);
				line += mode;
				if (actualValue != null)
				{
					line += " = " + actualValue.ToString();
				}
				line += "   (";
				if (defaultValue != null)
				{
					line += defaultValue.ToString();
				}
				if (persistantValue != null)
				{
					line += " : " + persistantValue.ToString();
				}
				if (transientValue != null)
				{
					line += " | " + transientValue.ToString();
				}
				line += ")";
				txt += line;
			}
		}
		catch
		{
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		txt += Environment.NewLine;
		txt += Environment.NewLine;
		txt += Environment.NewLine;
		txt += "        Special Modes:";
		IEnumerator<KeyValuePair<string, object>> enumerator2 = specialModeDictionaryDefault.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				txt += Environment.NewLine;
				string line = "";
                KeyValuePair<string, object> obj = enumerator2.Current;
				string mode = obj.Key;
				object defaultValue = obj.Value;
				object persistantValue = null;
				specialModeDictionaryPersistant.TryGetValue(mode, out persistantValue);
				object transientValue = null;
				specialModeDictionaryTransient.TryGetValue(mode, out transientValue);
				object actualValue = null;
				ModeDictionary.TryGetValue(mode, out actualValue);
				line += mode;
				if (actualValue != null)
				{
					line += " = " + actualValue.ToString();
				}
				line += "   (";
				if (defaultValue != null)
				{
					line += defaultValue.ToString();
				}
				if (persistantValue != null)
				{
					line += " : " + persistantValue.ToString();
				}
				if (transientValue != null)
				{
					line += " | " + transientValue.ToString();
				}
				line += ")";
				txt += line;
			}
		}
		catch
		{
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator2 as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		txt += Environment.NewLine;
		txt += Environment.NewLine;
		txt += Environment.NewLine;
		if (overtimeModeTransient == -1)
		{
			txt += "OvertimeMode = " + overtimeMode.ToString() + "   (" + overtimeModePersistant.ToString() + ")";
		}
		else
		{
			txt += "OvertimeMode = " + overtimeMode.ToString() + "   (" + overtimeModePersistant.ToString() + " | " + (overtimeModeTransient == 1).ToString() + ")";
		}
		txt += Environment.NewLine;
		txt += "        Overtime Modes:";
		Dictionary<string, object> dic = null;
		if (overtimeMode)
		{
			dic = overtimeModeDictionaryOnDefault;
		}
		else
		{
			dic = overtimeModeDictionaryOffDefault;
		}
		IEnumerator<KeyValuePair<string, object>> enumerator3 = dic.GetEnumerator();
		try
		{
			while (enumerator3.MoveNext())
			{
				txt += Environment.NewLine;
				string line = "";
                KeyValuePair<string, object> obj = enumerator3.Current;
				string mode = obj.Key;
				object defaultValue = obj.Value;
				object persistantValue = null;
				overtimeModeDictionaryPersistant.TryGetValue(mode, out persistantValue);
				object transientValue = null;
				overtimeModeDictionaryTransient.TryGetValue(mode, out transientValue);
				object actualValue = null;
				ModeDictionary.TryGetValue(mode, out actualValue);
				line += mode;
				if (actualValue != null)
				{
					line += " = " + actualValue.ToString();
				}
				line += "   (";
				if (defaultValue != null)
				{
					line += defaultValue.ToString();
				}
				if (persistantValue != null)
				{
					line += " : " + persistantValue.ToString();
				}
				if (transientValue != null)
				{
					line += " | " + transientValue.ToString();
				}
				line += ")";
				txt += line;
			}
		}
		catch
		{
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator3 as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		txt += Environment.NewLine;
		txt += Environment.NewLine;
		txt += Environment.NewLine;
		if (reworkTierTransient == -1)
		{
			txt += "ReworkTier = " + reworkTier.ToString() + "   (" + reworkTierPersistant.ToString() + ")";
		}
		else
		{
			txt += "ReworkTier = " + reworkTier.ToString() + "   (" + reworkTierPersistant.ToString() + " | " + reworkTierTransient.ToString() + ")";
		}
		txt += Environment.NewLine;
		txt += "        Rework Modes:";
		IEnumerator<KeyValuePair<string, object>> enumerator4 = reworkModeTierDictionary.GetEnumerator();
		try
		{
			while (enumerator4.MoveNext())
			{
				txt += Environment.NewLine;
				string line = "";
				KeyValuePair<string, object> obj = enumerator4.Current;
				string mode = obj.Key;
				object defaultValue = reworkTier >= (int)obj.Value;
				object persistantValue = null;
				reworkModeDictionaryPersistant.TryGetValue(mode, out persistantValue);
				object transientValue = null;
				reworkModeDictionaryTransient.TryGetValue(mode, out transientValue);
				object actualValue = null;
				ModeDictionary.TryGetValue(mode, out actualValue);
				line += "(T" + obj.Value.ToString() + ") ";
				line += mode;
				if (actualValue != null)
				{
					line += " = " + actualValue.ToString();
				}
				line += "   (";
				if (defaultValue != null)
				{
					line += defaultValue.ToString();
				}
				if (persistantValue != null)
				{
					line += " : " + persistantValue.ToString();
				}
				if (transientValue != null)
				{
					line += " | " + transientValue.ToString();
				}
				line += ")";
				txt += line;
			}
		}
		catch
		{
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator4 as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		return txt;
	}

    public T GetValue<T>(string mode)
    {
		object obj;
		if (ModeDictionary.TryGetValue(mode, out obj) && obj is T)
		{
			return (T)obj;
		}
		return default(T);
    }

	public void ChangeValue(string mode, object value, ModeType modeType, SaveType saveType = SaveType.PERSISTANT)
	{
		Dictionary<string, object> defaults;
		Dictionary<string, object> transients;
		Dictionary<string, object> persistants;
		switch (modeType)
		{
			case ModeType.BASIC:
				defaults = basicModeDictionaryDefault;
				transients = basicModeDictionaryTransient;
				persistants = basicModeDictionaryPersistant;
				break;
			case ModeType.SPECIAL:
				defaults = specialModeDictionaryDefault;
				transients = specialModeDictionaryTransient;
				persistants = specialModeDictionaryPersistant;
				break;
			case ModeType.OVERTIME:
				defaults = overtimeMode ? overtimeModeDictionaryOnDefault : overtimeModeDictionaryOffDefault;
				transients = overtimeModeDictionaryTransient;
				persistants = overtimeModeDictionaryPersistant;
				break;
			case ModeType.REWORK:
				defaults = reworkModeTierDictionary;
				transients = reworkModeDictionaryTransient;
				persistants = reworkModeDictionaryPersistant;
				break;
			default:
				return;
		}
		object obj;
        if (!defaults.TryGetValue(mode, out obj))
        {
            return;
        }
		if (modeType == ModeType.REWORK)
		{
			if (value.GetType() != typeof(bool))
			{
				return;
			}
		}
        else if (value.GetType() != obj.GetType())
        {
            return;
        }
		switch (saveType)
		{
			case SaveType.NONE:
				ModeDictionary[mode] = value;
				break;
			case SaveType.TRANSIENT:
				ModeDictionary[mode] = value;
				if (transients.ContainsKey(mode))
				{
					transients[mode] = value;
				}
				else
				{
					transients.Add(mode, value);
				}
				break;
			case SaveType.PERSISTANT:
				ModeDictionary[mode] = value;
				if (transients.ContainsKey(mode))
				{
					transients.Remove(mode);
				}
				if (persistants.ContainsKey(mode))
				{
					persistants[mode] = value;
				}
				else
				{
					persistants.Add(mode, value);
				}
				break;
			case SaveType.QUIET:
				if (!transients.ContainsKey(mode))
				{
					ModeDictionary[mode] = value;
				}
				if (persistants.ContainsKey(mode))
				{
					persistants[mode] = value;
				}
				else
				{
					persistants.Add(mode, value);
				}
				break;
		}
        if (saveType != SaveType.NONE)
        {
            Save();
        }
		Notice.instance.Send(NoticeName.AddSystemLog, new object[]
		{
			mode + " : " + modeType.ToString() + " : " + saveType.ToString() + " = " + value.ToString()
		});
	}

	public void ResetValue(string mode, ModeType modeType, SaveType saveType = SaveType.PERSISTANT)
	{
		Dictionary<string, object> defaults;
		Dictionary<string, object> transients;
		Dictionary<string, object> persistants;
		switch (modeType)
		{
			case ModeType.BASIC:
				defaults = basicModeDictionaryDefault;
				transients = basicModeDictionaryTransient;
				persistants = basicModeDictionaryPersistant;
				break;
			case ModeType.SPECIAL:
				defaults = specialModeDictionaryDefault;
				transients = specialModeDictionaryTransient;
				persistants = specialModeDictionaryPersistant;
				break;
			case ModeType.OVERTIME:
				defaults = overtimeMode ? overtimeModeDictionaryOnDefault : overtimeModeDictionaryOffDefault;
				transients = overtimeModeDictionaryTransient;
				persistants = overtimeModeDictionaryPersistant;
				break;
			case ModeType.REWORK:
				defaults = reworkModeTierDictionary;
				transients = reworkModeDictionaryTransient;
				persistants = reworkModeDictionaryPersistant;
				break;
			default:
				return;
		}
		object obj;
        if (!defaults.TryGetValue(mode, out obj))
        {
            return;
        }
		switch (saveType)
		{
			case SaveType.NONE:
				if (transients.ContainsKey(mode))
				{
					ModeDictionary[mode] = transients[mode];
				}
				else if (persistants.ContainsKey(mode))
				{
					ModeDictionary[mode] = persistants[mode];
				}
				else
				{
					ModeDictionary[mode] = modeType == ModeType.REWORK ? reworkTier >= (int)obj : obj;
				}
				break;
			case SaveType.TRANSIENT:
				if (transients.ContainsKey(mode))
				{
					transients.Remove(mode);
					if (persistants.ContainsKey(mode))
					{
						ModeDictionary[mode] = persistants[mode];
					}
					else
					{
						ModeDictionary[mode] = modeType == ModeType.REWORK ? reworkTier >= (int)obj : obj;
					}
				}
				break;
			case SaveType.PERSISTANT:
				if (transients.ContainsKey(mode))
				{
					transients.Remove(mode);
				}
				if (persistants.ContainsKey(mode))
				{
					persistants.Remove(mode);
				}
				ModeDictionary[mode] = modeType == ModeType.REWORK ? reworkTier >= (int)obj : obj;
				break;
			case SaveType.QUIET:
				if (persistants.ContainsKey(mode))
				{
					persistants.Remove(mode);
					if (transients.ContainsKey(mode))
					{
						ModeDictionary[mode] = transients[mode];
					}
					else
					{
						ModeDictionary[mode] = modeType == ModeType.REWORK ? reworkTier >= (int)obj : obj;
					}
				}
				break;
		}
        if (saveType != SaveType.NONE)
        {
            Save();
        }
		Notice.instance.Send(NoticeName.AddSystemLog, new object[]
		{
			mode + " : " + modeType.ToString() + " : " + saveType.ToString() + " : reset"
		});
	}

	public void ChangeOvertimeMode(bool value, SaveType saveType = SaveType.PERSISTANT)
	{
        bool changed = overtimeMode != value;
		switch (saveType)
		{
			case SaveType.NONE:
				overtimeMode = value;
				break;
			case SaveType.TRANSIENT:
				overtimeMode = value;
				overtimeModeTransient = value ? 1 : 0;
				break;
			case SaveType.PERSISTANT:
				overtimeMode = value;
				overtimeModeTransient = -1;
				overtimeModePersistant = value;
				break;
			case SaveType.QUIET:
				if (overtimeModeTransient == -1)
				{
					overtimeMode = value;
				}
				else
				{
					changed = false;
				}
				overtimeModePersistant = value;
				break;
		}
		if (saveType != SaveType.NONE)
		{
			Save();
		}
		Notice.instance.Send(NoticeName.AddSystemLog, new object[]
		{
			"OvertimeMode : " + saveType.ToString() + " = " + value.ToString()
		});
        if (!changed) return;
        Dictionary<string, object> dic = overtimeMode ? overtimeModeDictionaryOnDefault : overtimeModeDictionaryOffDefault;
		IEnumerator<KeyValuePair<string, object>> enumerator = dic.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
                KeyValuePair<string, object> obj = enumerator.Current;
                if (overtimeModeDictionaryPersistant.ContainsKey(obj.Key) || overtimeModeDictionaryTransient.ContainsKey(obj.Key))
                {
                    continue;
                }
				else if (!ModeDictionary.ContainsKey(obj.Key))
				{
					ModeDictionary.Add(obj.Key, obj.Value);
				}
                else
                {
                    ModeDictionary[obj.Key] = obj.Value;
                }
			}
		}
		catch
		{
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	public void ChangeReworkTier(int value, SaveType saveType = SaveType.PERSISTANT)
	{
        bool changed = reworkTier != value;
		switch (saveType)
		{
			case SaveType.NONE:
				reworkTier = value;
				break;
			case SaveType.TRANSIENT:
				reworkTier = value;
				reworkTierTransient = value;
				break;
			case SaveType.PERSISTANT:
				reworkTier = value;
				reworkTierTransient = -1;
				reworkTierPersistant = value;
				break;
			case SaveType.QUIET:
				if (reworkTierTransient == -1)
				{
					reworkTier = value;
				}
				else
				{
					changed = false;
				}
				reworkTierPersistant = value;
				break;
		}
		if (saveType != SaveType.NONE)
		{
			Save();
		}
		Notice.instance.Send(NoticeName.AddSystemLog, new object[]
		{
			"ReworkTier : " + saveType.ToString() + " = " + value.ToString()
		});
        if (!changed) return;
		IEnumerator<KeyValuePair<string, object>> enumerator = reworkModeTierDictionary.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
                KeyValuePair<string, object> obj = enumerator.Current;
                if (reworkModeDictionaryPersistant.ContainsKey(obj.Key) || reworkModeDictionaryTransient.ContainsKey(obj.Key))
                {
                    continue;
                }
				else if (!ModeDictionary.ContainsKey(obj.Key))
				{
					ModeDictionary.Add(obj.Key, reworkTier >= (int)obj.Value);
				}
                else
                {
                    ModeDictionary[obj.Key] = reworkTier >= (int)obj.Value;
                }
			}
		}
		catch
		{
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	public void ResetTransients()
	{
		basicModeDictionaryTransient.Clear();
		specialModeDictionaryTransient.Clear();
		overtimeModeTransient = -1;
		overtimeMode = overtimeModePersistant;
		overtimeModeDictionaryTransient.Clear();
		reworkTierTransient = -1;
		reworkTier = reworkTierPersistant;
		ModeDictionary.Clear();
		EnumerateDictionary(basicModeDictionaryPersistant);
		EnumerateDictionary(specialModeDictionaryPersistant);
		EnumerateDictionary(overtimeModeDictionaryPersistant);
		EnumerateDictionary(reworkModeDictionaryPersistant);
		EnumerateDictionary(basicModeDictionaryDefault);
		EnumerateDictionary(specialModeDictionaryDefault);
		EnumerateDictionary(overtimeMode ? overtimeModeDictionaryOnDefault : overtimeModeDictionaryOffDefault);
		EnumerateDictionaryTiered(reworkModeTierDictionary);
		Save();
		Notice.instance.Send(NoticeName.AddSystemLog, new object[]
		{
			"Reset Transients"
		});
	}

	public void ResetAll()
	{
		basicModeDictionaryTransient.Clear();
		specialModeDictionaryTransient.Clear();
		overtimeModeDictionaryTransient.Clear();
		basicModeDictionaryPersistant.Clear();
		specialModeDictionaryPersistant.Clear();
		overtimeModeDictionaryPersistant.Clear();
		overtimeModePersistant = false;
		overtimeModeTransient = -1;
		overtimeMode = false;
		reworkTierPersistant = 2;
		reworkTierTransient = -1;
		reworkTier = 2;
		ModeDictionary.Clear();
		EnumerateDictionary(basicModeDictionaryDefault);
		EnumerateDictionary(specialModeDictionaryDefault);
		EnumerateDictionary(overtimeMode ? overtimeModeDictionaryOnDefault : overtimeModeDictionaryOffDefault);
		EnumerateDictionaryTiered(reworkModeTierDictionary);
		Save();
		Notice.instance.Send(NoticeName.AddSystemLog, new object[]
		{
			"Reset All"
		});
	}

    public Type GetModeType(string mode, ModeType modeType)
    {
		Dictionary<string, object> defaults;
		switch (modeType)
		{
			case ModeType.BASIC:
				defaults = basicModeDictionaryDefault;
				break;
			case ModeType.SPECIAL:
				defaults = specialModeDictionaryDefault;
				break;
			case ModeType.OVERTIME:
				defaults = overtimeMode ? overtimeModeDictionaryOnDefault : overtimeModeDictionaryOffDefault;
				break;
			case ModeType.REWORK:
				return typeof(bool);
			default:
				return null;
		}
		object obj;
		if (defaults.TryGetValue(mode, out obj))
		{
			return obj.GetType();
		}
		return null;
    }

	public Dictionary<string, object> ModeDictionary = new Dictionary<string, object>();

	public Dictionary<string, object> basicModeDictionaryPersistant = new Dictionary<string, object>();
	public Dictionary<string, object> basicModeDictionaryTransient = new Dictionary<string, object>();
	public Dictionary<string, object> basicModeDictionaryDefault = new Dictionary<string, object>
	{
		{"ControlGroups", true},
		{"WorkOrderQueue", true},
		{"AddSubtractSelection", true},
		{"FreeCustomAppearance", true},
		{"RealTimeSuccessRate", SuccessRateDisplayMode.PERCENTAGE},
		{"InfoWindowSuccessRate", SuccessRateDisplayMode.PERCENTAGE},
		{"RevealEXP", true},
		{"RevealOrdeals", true},
		{"HpBarStackingAgent", true},
		{"HpBarStackingAbnormality", true},
		{"HpBarStackingRabbit", true},
		{"PseudoRandomTitles", true},
		{"WorkCompression", 2},
		{"WorkCompressionOvertime", 0},
		{"WorkCompressionAlways", 0},
		{"SpendPEForEgoGiftChance", true},
		{"StackableEgoGifts", true},
		{"EgoGiftHelper", false},
		{"WhiteNightImmobilize", true},
		{"PLKeepAbilityWhenWNAbsent", false},
		{"DisplayAbnoHp", AbnoHpDisplayMode.NAME_AND_HP},
		{"MapGraphFix", true},
		{"TipherethElevators", true},
		{"GrungeLimit", 50f},
		{"TwoTipherethCaptains", true},
		{"DynamicNeutralMusic", false},
		{"DontTouchMeFix", true},
		{"AltReductionText", false},
		{"WindowScaleAgent", 1f},
		{"WindowScaleWork", 1f},
		{"WindowScaleSuppress", 1f}
	};

	public Dictionary<string, object> specialModeDictionaryPersistant = new Dictionary<string, object>();
	public Dictionary<string, object> specialModeDictionaryTransient = new Dictionary<string, object>();
	public Dictionary<string, object> specialModeDictionaryDefault = new Dictionary<string, object>
	{
		{"NoEXP", false},
		{"UnstableTT2", false},
		{"EarlyOvertimeOverloads", false},
		{"EarlyOvertimeOrdeals", false},
		{"JailbreakOvertimeMissions", false},
		{"DoubleAbno", false},
		{"AutoQliphoth", false},
		{"ControlableClerks", false},
		{"UnknownAbnos", false},
		{"Blind", false}
	};

	public bool overtimeModePersistant;
	public int overtimeModeTransient = -1;
	public bool overtimeMode;
	public Dictionary<string, object> overtimeModeDictionaryPersistant = new Dictionary<string, object>();
	public Dictionary<string, object> overtimeModeDictionaryTransient = new Dictionary<string, object>();
	public Dictionary<string, object> overtimeModeDictionaryOffDefault = new Dictionary<string, object>
	{
		{"OvertimeMissions", false},
		{"OvertimeOverloads", false},
		{"OvertimeOrdeals", false},
		{"SecondaryQliphothOverload", false},
		{"NewAbnormalities", false}
	};
	public Dictionary<string, object> overtimeModeDictionaryOnDefault = new Dictionary<string, object>
	{
		{"OvertimeMissions", true},
		{"OvertimeOverloads", true},
		{"OvertimeOrdeals", true},
		{"SecondaryQliphothOverload", true},
		{"NewAbnormalities", true}
	};

	public int reworkTierPersistant = 2;
	public int reworkTierTransient = -1;
	public int reworkTier = 2;
	public Dictionary<string, object> reworkModeDictionaryPersistant = new Dictionary<string, object>();
	public Dictionary<string, object> reworkModeDictionaryTransient = new Dictionary<string, object>();
	public Dictionary<string, object> reworkModeTierDictionary = new Dictionary<string, object>
	{
		{"FragmentErosion", 1},
		{"LuminousAndYangForgiveness", 1},
		{"ShyLookFreeze", 1},
		{"ParasiteTreeStall", 1},
		{"SpiderBudAndBloodbathEnergy", 2},
		{"CrumblingArmorGift", 2},
		{"GreenMidnightRework", 3},
		{"FuneralRework", 3}
	};

	public enum ModeType
	{
		BASIC,
		SPECIAL,
		OVERTIME,
		REWORK
	}

	public enum SaveType
	{
		NONE,
		TRANSIENT,
		PERSISTANT,
		QUIET
	}

    public void LoadOld()
    {
        try
        {
            if (!File.Exists(Application.dataPath + "/Managed/BaseMod/SpecialModes.txt"))
            {
                return;
                // string xml2 = File.ReadAllText(Application.dataPath + "/Managed/BaseMod/SpecialModesReference_DoNotModify.txt");
                // File.WriteAllText(Application.dataPath + "/Managed/BaseMod/SpecialModes.txt", xml2);
            }
            string xml = File.ReadAllText(Application.dataPath + "/Managed/BaseMod/SpecialModes.txt");
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            IEnumerator enumeratorToDispose = null;
            try
            {
                IEnumerator enumerator = xmlDocument.SelectSingleNode("Modes").SelectSingleNode("DefaultModesList").SelectNodes("DefaultMode").GetEnumerator();
                enumeratorToDispose = enumerator;
                while (enumerator.MoveNext())
                {
                    object obj = enumerator.Current;
                    XmlNode xmlNode = (XmlNode)obj;
                    string type = xmlNode.Attributes.GetNamedItem("type").InnerText;
                    string value = xmlNode.InnerText.Trim();
                    string[] values;
                    switch (type)
                    {
                        case "FreeCustomAppearance":
                        case "SpendPEForEgoGiftChance":
                        case "StackableEgoGifts":
                        case "WhiteNightImmobilize":
                        case "PseudoRandomTitles":
                        case "EgoGiftHelper":
                        case "RevealEXP":
                        case "RevealOrdeals":
                        case "HpBarStackingAgent":
                        case "HpBarStackingAbnormality":
                        case "HpBarStackingRabbit":
                            basicModeDictionaryPersistant.Add(type, bool.Parse(value));
                            break;
                        case "WorkCompression":
                            values = value.Split(',');
                            basicModeDictionaryPersistant.Add("WorkCompression", int.Parse(values[0]));
                            basicModeDictionaryPersistant.Add("WorkCompressionAlways", int.Parse(values[1]));
                            break;
                        case "RealTimeSuccessRate":
                        case "InfoWindowSuccessRate":
                            basicModeDictionaryPersistant.Add(type, (SuccessRateDisplayMode)int.Parse(value));
                            break;
                    }
                }
            }
			catch
			{
				// ConsoleScript.instance.ConsoleWnd.gameObject.SetActive(true);
				// ConsoleScript.instance.ConsoleWnd.GetComponent<InputField>().text = "Error in SpecialModes.txt in DefaultModesList";
			}
            finally
            {
                IDisposable disposable;
                if ((disposable = (enumeratorToDispose as IDisposable)) != null)
                {
                    disposable.Dispose();
                }
                enumeratorToDispose = null;
            }
            try
            {
                IEnumerator enumerator2 = xmlDocument.SelectSingleNode("Modes").SelectSingleNode("SpecialModesList").SelectNodes("SpecialMode").GetEnumerator();
                enumeratorToDispose = enumerator2;
                while (enumerator2.MoveNext())
                {
                    object obj = enumerator2.Current;
                    XmlNode xmlNode = (XmlNode)obj;
                    string type = xmlNode.Attributes.GetNamedItem("type").InnerText;
                    string value = xmlNode.InnerText.Trim();
                    switch (type)
                    {
                        case "NoEXP":
                        case "UnstableTT2":
                        case "EarlyOvertimeOverloads":
                            specialModeDictionaryPersistant.Add(type, bool.Parse(value));
                            break;
                    }
                }
            }
			catch
			{
				// ConsoleScript.instance.ConsoleWnd.gameObject.SetActive(true);
				// ConsoleScript.instance.ConsoleWnd.GetComponent<InputField>().text = "Error in SpecialModes.txt in SpecialModesList";
			}
            finally
            {
                IDisposable disposable;
                if ((disposable = (enumeratorToDispose as IDisposable)) != null)
                {
                    disposable.Dispose();
                }
                enumeratorToDispose = null;
            }
            try
            {
                overtimeModePersistant = bool.Parse(xmlDocument.SelectSingleNode("Modes").SelectSingleNode("OvertimeModesListReference").Attributes.GetNamedItem("enabled").InnerText);
                IEnumerator enumerator3 = xmlDocument.SelectSingleNode("Modes").SelectSingleNode("OvertimeModesList").SelectNodes("OvertimeMode").GetEnumerator();
                enumeratorToDispose = enumerator3;
                while (enumerator3.MoveNext())
                {
                    object obj = enumerator3.Current;
                    XmlNode xmlNode = (XmlNode)obj;
                    string type = xmlNode.Attributes.GetNamedItem("type").InnerText;
                    string value = xmlNode.InnerText.Trim();
                    switch (type)
                    {
                        case "OvertimeOverloads":
                            overtimeModeDictionaryPersistant.Add(type, bool.Parse(value));
                            break;
                        case "GreenMidnightRework":
                            reworkModeDictionaryPersistant.Add(type, bool.Parse(value));
                            break;
                    }
                }
            }
			catch
			{
				// ConsoleScript.instance.ConsoleWnd.gameObject.SetActive(true);
				// ConsoleScript.instance.ConsoleWnd.GetComponent<InputField>().text = "Error in SpecialModes.txt in OvertimeModesList";
			}
            finally
            {
                IDisposable disposable;
                if ((disposable = (enumeratorToDispose as IDisposable)) != null)
                {
                    disposable.Dispose();
                }
                enumeratorToDispose = null;
            }
        }
        catch
        {
            // ConsoleScript.instance.ConsoleWnd.gameObject.SetActive(true);
            // ConsoleScript.instance.ConsoleWnd.GetComponent<InputField>().text = "Error in SpecialModes.txt";
            return;
        }
    }
}