/*
public void OnExitEdit(string command) // (!) Unencrypted the console
*/
using System;
using Assets.Scripts.UI.Utils;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200037E RID: 894
public class ConsoleScript : MonoBehaviour
{
	// Token: 0x06001BA0 RID: 7072 RVA: 0x0000423C File Offset: 0x0000243C
	public ConsoleScript()
	{
	}

	// Token: 0x17000236 RID: 566
	// (get) Token: 0x06001BA1 RID: 7073 RVA: 0x0001DF21 File Offset: 0x0001C121
	public static ConsoleScript instance
	{
		get
		{
			return ConsoleScript._instance;
		}
	}

	// Token: 0x06001BA2 RID: 7074 RVA: 0x0001DF28 File Offset: 0x0001C128
	private string GetHmmCommand(string cmd)
	{
		cmd = (cmd ?? "");
		return cmd;
	}

	// Token: 0x06001BA3 RID: 7075 RVA: 0x0001DF37 File Offset: 0x0001C137
	private void Awake()
	{
		if (ConsoleScript._instance != null)
		{
			UnityEngine.Object.DestroyImmediate(base.gameObject);
			return;
		}
		ConsoleScript._instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06001BA4 RID: 7076 RVA: 0x0001DF66 File Offset: 0x0001C166
	private void Start()
	{
		this.inputField = this.ConsoleWnd.GetComponent<InputField>();
		if (this.ConsoleWnd.activeInHierarchy)
		{
			this.ConsoleWnd.gameObject.SetActive(false);
		}
	}

	// Token: 0x06001BA5 RID: 7077 RVA: 0x000E5E28 File Offset: 0x000E4028
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.BackQuote))
		{
			if (!this.consoleActivated)
			{
				this.ConsoleWnd.gameObject.SetActive(true);
				this.inputField.Select();
			}
			else
			{
				this.ConsoleWnd.gameObject.SetActive(false);
			}
			this.consoleActivated = !this.consoleActivated;
		}
	}

	// Token: 0x06001BA6 RID: 7078 RVA: 0x000E5E90 File Offset: 0x000E4090
	public void OnExitEdit(string command)
	{ // <Mod>
		if (!this.consoleActivated)
		{
			return;
		}
		try
		{
			// command = this.GetHmmCommand(command);
			this.consoleActivated = false;
			this.ConsoleWnd.gameObject.SetActive(false);
			if (this.angelaLogEnter)
			{
				ConsoleCommand.instance.StandardCommandOperation(2, new object[]
				{
					command
				});
			}
			else if (this.systemLogEnter)
			{
				ConsoleCommand.instance.StandardCommandOperation(0, new object[]
				{
					command
				});
			}
			else
			{
				char[] separator = new char[]
				{
					' '
				};
				string[] array = command.Split(separator);
				string a = array[0].ToLower();
				string text = array[1].ToLower();
				if (text.EndsWith("_mod"))
				{
					text = text.Substring(0, text.Length - 4);
				}
				if (a == ConsoleCommand.RootCommand)
				{
					int num = ConsoleCommand.instance.rootCommand.IndexOf(text);
					if (num != -1)
					{
						string[] para = new string[array.Length - 2];
						for (int i = 0; i < para.Length; i++)
						{
							para[i] = array[i + 2];
						}
						ConsoleCommand.instance.RootCommandOperation(num, para);
					}
				}
				else if (a == ConsoleCommand.StandardCommand)
				{
					int num2 = ConsoleCommand.instance.standardCommand.IndexOf(text);
					if (num2 != -1)
					{
						switch (num2)
						{
						case 0:
							this.systemLogEnter = !this.systemLogEnter;
							if (this.systemLogEnter)
							{
								Debug.Log("SystemLog Enter");
							}
							else
							{
								Debug.Log("SystemLog Exit");
							}
							break;
						case 1:
						{
							float num3 = float.Parse(array[2]);
							Debug.Log("AddEnergy + " + num3);
							ConsoleCommand.instance.StandardCommandOperation(1, new object[]
							{
								num3
							});
							break;
						}
						case 2:
							this.angelaLogEnter = !this.angelaLogEnter;
							if (this.angelaLogEnter)
							{
								Debug.Log("Angela Enter");
							}
							else
							{
								Debug.Log("Angela Exit");
							}
							break;
						case 3:
						case 4:
						case 6:
							ConsoleCommand.instance.StandardCommandOperation(num2, new object[]
							{
								array[2]
							});
							break;
						case 5:
							ConsoleCommand.instance.StandardCommandOperation(5, new object[0]);
							break;
						case 7:
							ConsoleCommand.instance.StandardCommandOperation(num2, new object[]
							{
								array[2],
								array[3],
								array[4]
							});
							break;/*
						case 8:
							int num4 = 1;
							if (array.Length > 3)
							{
								num4 = int.Parse(array[3]);
							}
							ConsoleCommand.instance.StandardCommandOperation(num2, new object[]
							{
								array[2],
								num4
							});
							break;*/
						case 9:
							if (array.Length > 3)
							{
								ConsoleCommand.instance.StandardCommandOperation(num2, new object[]
								{
									array[2],
									array[3]
								});
								break;
							}
							ConsoleCommand.instance.StandardCommandOperation(num2, new object[]
							{
								array[2]
							});
							break;
						case 10:
							ConsoleCommand.instance.StandardCommandOperation(10, new object[0]);
							break;
						case 11:
							ConsoleCommand.instance.StandardCommandOperation(num2, new object[]
							{
								array[2]
							});
							break;
						case 12:
							if (array.Length >= 4)
							{
								ConsoleCommand.instance.StandardCommandOperation(num2, new object[]
								{
									array[2],
									bool.Parse(array[3])
								});
							}
							else
							{
								ConsoleCommand.instance.StandardCommandOperation(num2, new object[]
								{
									array[2]
								});
							}
							break;
						case 13:
						{
							string text2 = string.Empty;
							if (array.Length <= 2)
							{
								ConsoleCommand.instance.StandardCommandOperation(num2, new object[0]);
							}
							else
							{
								for (int i = 2; i < array.Length; i++)
								{
									text2 = text2 + array[i] + " ";
								}
								ConsoleCommand.instance.StandardCommandOperation(num2, new object[]
								{
									text2
								});
							}
							break;
						}/*
						case 14:
							ConsoleCommand.instance.StandardCommandOperation(num2, new object[]
							{
								array[2]
							});
							break;*/
						case 15:
							ConsoleCommand.instance.StandardCommandOperation(num2, new object[]
							{
								array[2]
							});
							break;
						case 16:
						case 17:
						case 18:
						case 19:
						case 20:
						case 21:
						case 22:
							ConsoleCommand.instance.StandardCommandOperation(num2, new object[0]);
							break;
						case 23:
							ConsoleCommand.instance.StandardCommandOperation(num2, new object[]
							{
								array[2]
							});
							break;
						default:
							string[] para = new string[array.Length - 2];
							for (int i = 0; i < para.Length; i++)
							{
								para[i] = array[i + 2];
							}
							ConsoleCommand.instance.StandardCommandOperation(num2, para);
							break;
						}
					}
				}
				else if (a == ConsoleCommand.CreatureCommand)
				{
					int num4 = ConsoleCommand.instance.creatureCommand.IndexOf(text);
					Debug.Log(string.Concat(new object[]
					{
						"Creature Opeartion : ",
						num4,
						" ",
						array.Length
					}));
					if (num4 != -1)
					{
						float num5;
						if (num4 == 6)
						{
							ConsoleCommand.instance.CreatureCommandOperation(6, true, new object[0]);
						}
						else if (float.TryParse(array[2], out num5))
						{
							if (num4 >= 7)
							{
								object[] para = new object[array.Length-2];
								para[0] = (long)num5;
								for (int i = 1; i < array.Length-2; i++)
								{
									para[i] = array[i+2];
								}
								ConsoleCommand.instance.CreatureCommandOperation(num4, false, para);
							}
							else if (array.Length >= 4)
							{
								float num6 = float.Parse(array[3]);
								ConsoleCommand.instance.CreatureCommandOperation(num4, false, new object[]
								{
									(long)num5,
									num6
								});
							}
							else if (array.Length >= 5)
							{
								object[] para = new object[array.Length - 2];
								para[0] = (long)num5;
								for (int i = 1; i < para.Length; i++)
								{
									para[i] = array[i + 2];
								}
								ConsoleCommand.instance.CreatureCommandOperation(num4, false, para);
							}
							else
							{
								ConsoleCommand.instance.CreatureCommandOperation(num4, false, new object[]
								{
									(long)num5
								});
							}
						}
						else if (array.Length >= 5)
						{
							object[] para = new object[array.Length - 2];
							para[0] = (long)num5;
							for (int i = 1; i < para.Length; i++)
							{
								para[i] = array[i + 2];
							}
							ConsoleCommand.instance.CreatureCommandOperation(num4, true, para);
						}
						else if (array.Length >= 4)
						{
							float num7 = float.Parse(array[3]);
							ConsoleCommand.instance.CreatureCommandOperation(num4, true, new object[]
							{
								(long)num5,
								num7
							});
						}
						else
						{
							ConsoleCommand.instance.CreatureCommandOperation(num4, true, new object[]
							{
								(long)num5
							});
						}
					}
				}
				else if (a == ConsoleCommand.AgentCommand)
				{
					int num8 = ConsoleCommand.instance.agentCommand.IndexOf(text);
					if (num8 == 6)
					{
						RwbpType rwbpType = EnumTextConverter.GetRwbpType(array[2]);
						long num9 = long.Parse(array[3]);
						int num10 = int.Parse(array[4]);
						ConsoleCommand.instance.AgentCommandOperation(num8, new object[]
						{
							rwbpType,
							num9,
							num10
						});
					}
					else if (num8 == 4 && array.Length >= 5)
					{
						float num11 = float.Parse(array[4]);
						string str = array[3];
						long num12 = long.Parse(array[2]);
						Debug.Log(num11 + " " + num12);
						Debug.Log(text);
						if (num8 != -1)
						{
							ConsoleCommand.instance.AgentCommandOperation(num8, new object[]
							{
								num12,
								str,
								num11
							});
						}
					}
					else if (num8 < 7)
					{
						float num11 = float.Parse(array[3]);
						long num12 = long.Parse(array[2]);
						Debug.Log(num11 + " " + num12);
						Debug.Log(text);
						if (num8 != -1)
						{
							ConsoleCommand.instance.AgentCommandOperation(num8, new object[]
							{
								num12,
								num11
							});
						}
					}
					else
					{
						string[] para = new string[array.Length - 2];
						for (int i = 0; i < para.Length; i++)
						{
							para[i] = array[i + 2];
						}
						ConsoleCommand.instance.AgentCommandOperation(num8, para);
					}
				}
				else if (a == ConsoleCommand.OfficerCommand)
				{
					int num13 = ConsoleCommand.instance.officerCommand.IndexOf(text);
					if (num13 != -1)
					{
						if (num13 < 2)
						{
						ConsoleCommand.instance.OfficerCommandOperation(num13, new object[]
						{
							array[2]
						});
						}
						else
						{
							string[] para = new string[array.Length - 2];
							for (int i = 0; i < para.Length; i++)
							{
								para[i] = array[i + 2];
							}
							ConsoleCommand.instance.OfficerCommandOperation(num13, para);
						}
					}
				}
				else if (a == ConsoleCommand.BetaCommand)
				{
					ConsoleCommand.instance.BetaCommandOperation(text, array);
				}
				else if (a == ConsoleCommand.ConfigCommand)
				{
					int num2 = ConsoleCommand.instance.configCommand.IndexOf(text);
					if (num2 != -1)
					{
						switch (num2)
						{
						default:
							string[] para = new string[array.Length - 2];
							for (int i = 0; i < para.Length; i++)
							{
								para[i] = array[i + 2];
							}
							ConsoleCommand.instance.ConfigCommandOperation(num2, para);
							break;
						}
					}
				}
				else
				{
					if (this.systemLogEnter)
					{
						ConsoleCommand.instance.StandardCommandOperation(0, new object[]
						{
							command
						});
					}
					if (this.angelaLogEnter)
					{
						ConsoleCommand.instance.StandardCommandOperation(2, new object[]
						{
							command
						});
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.Log(string.Concat(new object[]
			{
				"Invalid opearation : ",
				command,
				" ",
				ex
			}));
		}
	}

	// Token: 0x04001C95 RID: 7317
	private static ConsoleScript _instance;

	// Token: 0x04001C96 RID: 7318
	public GameObject ConsoleWnd;

	// Token: 0x04001C97 RID: 7319
	private InputField inputField;

	// Token: 0x04001C98 RID: 7320
	private bool consoleActivated;

	// Token: 0x04001C99 RID: 7321
	private bool systemLogEnter;

	// Token: 0x04001C9A RID: 7322
	private bool angelaLogEnter;
}
