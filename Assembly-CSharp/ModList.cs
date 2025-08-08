using System;
using System.Collections.Generic;
using System.IO;
using LobotomyBaseMod;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CEB RID: 3307
public class ModList : MonoBehaviour
{
	// Token: 0x060063F0 RID: 25584 RVA: 0x00235F14 File Offset: 0x00234114
	private void Awake()
	{ // <Patch>
        try
        {
            this.Modlist = Add_On.instance.ModList;
            this.Page = 0;
            bool flag = !this.init;
            if (flag)
            {
                this.PanelList = new List<GameObject>();
                this.PanelTextList = new List<Text>();
                for (int i = 0; i < 5; i++)
                {
                    bool flag2 = this.Modlist.Count > i;
                    GameObject gameObject;
                    if (flag2)
                    {
                        gameObject = MakeModInfo(this.Modlist[i], i);
                        MakeModInfo2(this.Modlist[i], gameObject);
                    }
                    else
                    {
                        gameObject = MakeModInfo(null, i);
                        MakeModInfo2(null, gameObject);
                    }
                    this.PanelList.Add(gameObject);
                    gameObject.transform.localPosition = new Vector2(-800f, (float)(255 - i * 150));
                }
                this.DescPanel = MakeModDesc(null);
                MakeModDesc2(null);
                this.DescPanel.transform.localPosition = new Vector2(160f, -75f);
                this.init = true;
                this.Down = MakeDownButton();
                this.Down.transform.localPosition = new Vector2(-795f, -445f);
                this.Up = MakeUpButton();
                this.Up.transform.localPosition = new Vector2(-795f, 355f);
                UpdatePage();
            }
        }
        catch (Exception ex)
        {
            ModDebug.Log("AWKerror - " + ex.Message + Environment.NewLine + ex.StackTrace);
        }/*
		try
		{
			this.Modlist = new List<ModInfo>();
			this.Page = 0;
			foreach (DirectoryInfo dir in Add_On.instance.DirList)
			{
				ModInfo item = new ModInfo(dir);
				this.Modlist.Add(item);
			}
			if (!this.init)
			{
				this.PanelList = new List<GameObject>();
				this.PanelTextList = new List<Text>();
				for (int i = 0; i < 5; i++)
				{
					GameObject gameObject;
					if (this.Modlist.Count > i)
					{
						gameObject = this.MakeModInfo(this.Modlist[i], i);
						this.MakeModInfo2(this.Modlist[i], gameObject);
					}
					else
					{
						gameObject = this.MakeModInfo(null, i);
						this.MakeModInfo2(null, gameObject);
					}
					this.PanelList.Add(gameObject);
					gameObject.transform.localPosition = new Vector2(-800f, (float)(255 - i * 150));
				}
				this.DescPanel = this.MakeModDesc(null);
				this.MakeModDesc2(null);
				this.DescPanel.transform.localPosition = new Vector2(160f, -75f);
				this.init = true;
				this.Down = this.MakeDownButton();
				this.Down.transform.localPosition = new Vector2(-795f, -445f);
				this.Up = this.MakeUpButton();
				this.Up.transform.localPosition = new Vector2(-795f, 355f);
				this.UpdatePage();
			}
		}
		catch (Exception ex)
		{
			File.WriteAllText(Add_On.Error_Report + "AWKerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
		}*/
	}

	// Token: 0x060063F2 RID: 25586 RVA: 0x00236134 File Offset: 0x00234334
	private void Update()
	{
		if (!base.gameObject.active)
		{
			float x = this.Up.transform.localPosition.x;
			float y = this.Up.transform.localPosition.y;
			if (Input.GetKey(KeyCode.UpArrow))
			{
				this.Up.transform.localPosition = new Vector2(x, y + 1f);
				File.WriteAllText(Add_On.Error_Report + "position.txt", x.ToString() + " / " + y.ToString());
			}
			if (Input.GetKey(KeyCode.DownArrow))
			{
				this.Up.transform.localPosition = new Vector2(x, y - 1f);
				File.WriteAllText(Add_On.Error_Report + "position.txt", x.ToString() + " / " + y.ToString());
			}
			if (Input.GetKey(KeyCode.LeftArrow))
			{
				this.Up.transform.localPosition = new Vector2(x - 1f, y);
				File.WriteAllText(Add_On.Error_Report + "position.txt", x.ToString() + " / " + y.ToString());
			}
			if (Input.GetKey(KeyCode.RightArrow))
			{
				this.Up.transform.localPosition = new Vector2(x + 1f, y);
				File.WriteAllText(Add_On.Error_Report + "position.txt", x.ToString() + " / " + y.ToString());
			}
		}
	}

	// Token: 0x060063F3 RID: 25587 RVA: 0x002362E8 File Offset: 0x002344E8
	public GameObject MakeModDesc(ModInfo info)
	{
		GameObject gameObject = new GameObject("BackGround");
		Image image = gameObject.AddComponent<Image>();
		image.transform.SetParent(base.gameObject.transform);
		Texture2D texture2D = new Texture2D(2, 2);
		texture2D.LoadImage(File.ReadAllBytes(Application.dataPath + "/Managed/BaseMod/Image/Desc.png"));
		Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0f, 0f));
		image.sprite = sprite;
		image.rectTransform.sizeDelta = new Vector2((float)texture2D.width, (float)texture2D.height);
		gameObject.transform.localScale = new Vector3(1f, 1f);
		gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x060063F4 RID: 25588 RVA: 0x002363D8 File Offset: 0x002345D8
	public GameObject MakeModDesc2(ModInfo info)
	{
		GameObject gameObject = new GameObject("BackGround");
		Text text = gameObject.AddComponent<Text>();
		gameObject.transform.SetParent(this.DescPanel.transform);
		text.rectTransform.sizeDelta = Vector2.zero;
		text.rectTransform.anchorMin = new Vector2(0.05f, 0.05f);
		text.rectTransform.anchorMax = new Vector2(0.95f, 0.95f);
		text.rectTransform.anchoredPosition = new Vector2(0f, 0f);
		text.text = " ";
		text.font = OptionUI.Instance.CreditTitle.font;
		text.fontSize = 45;
		text.color = new Color(0.2509804f, 1f, 0.654902f);
		text.alignment = TextAnchor.UpperLeft;
		gameObject.transform.localScale = new Vector3(1f, 1f);
		gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		gameObject.SetActive(true);
		this.Desc = text;
		return gameObject;
	}

	// Token: 0x060063F5 RID: 25589 RVA: 0x002364FC File Offset: 0x002346FC
	public GameObject MakeModInfo2(ModInfo info, GameObject Button)
	{
		GameObject gameObject = new GameObject("InfoText");
		Text text = gameObject.AddComponent<Text>();
		gameObject.transform.SetParent(Button.transform);
		text.rectTransform.sizeDelta = Vector2.zero;
		text.rectTransform.anchorMin = new Vector2(0.02f, 0f);
		text.rectTransform.anchorMax = new Vector2(0.98f, 1f);
		text.rectTransform.anchoredPosition = new Vector2(0f, 0f);
		if (info != null)
		{
			text.text = info.modname;
		}
		text.font = OptionUI.Instance.CreditTitle.font;
		text.fontSize = 30;
		text.color = new Color(0.2509804f, 1f, 0.654902f);
		text.alignment = TextAnchor.MiddleCenter;
		gameObject.transform.localScale = new Vector3(1f, 1f);
		gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		gameObject.SetActive(true);
		this.PanelTextList.Add(text);
		return gameObject;
	}

	// Token: 0x060063F6 RID: 25590 RVA: 0x0004E113 File Offset: 0x0004C313
	public void OnClickModInfo(int i)
	{
		if (this.Modlist.Count >= i + this.Page * 5)
		{
			this.Desc.text = this.Modlist[this.Page * 5 + i].modinfo;
		}
	}

	// Token: 0x060063F7 RID: 25591 RVA: 0x00236624 File Offset: 0x00234824
	public GameObject MakeModInfo(ModInfo info, int i)
	{
		GameObject gameObject = new GameObject("BackGround1");
		Image image = gameObject.AddComponent<Image>();
		gameObject.transform.SetParent(base.gameObject.transform);
		Texture2D texture2D = new Texture2D(2, 2);
		texture2D.LoadImage(File.ReadAllBytes(Application.dataPath + "/Managed/BaseMod/Image/Mod.png"));
		Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0f, 0f));
		image.sprite = sprite;
		image.rectTransform.sizeDelta = new Vector2((float)texture2D.width, (float)texture2D.height);
		gameObject.transform.localScale = new Vector3(1f, 1f);
		Button button = gameObject.AddComponent<Button>();
		button.targetGraphic = image;
		button.onClick.AddListener(delegate()
		{
			this.OnClickModInfo(i);
		});
		return gameObject;
	}

	// Token: 0x060063F8 RID: 25592 RVA: 0x00236724 File Offset: 0x00234924
	public void UpdatePage()
	{
		if (this.Page == 0)
		{
			this.Up.SetActive(false);
		}
		else
		{
			this.Up.SetActive(true);
		}
		for (int i = 0; i < 5; i++)
		{
			if (this.Page * 5 + i >= this.Modlist.Count)
			{
				this.PanelTextList[i].text = "";
			}
			else
			{
				this.PanelTextList[i].text = this.Modlist[this.Page * 5 + i].modname;
			}
		}
		if (this.Page * 5 + 5 >= this.Modlist.Count)
		{
			this.Down.SetActive(false);
			return;
		}
		this.Down.SetActive(true);
	}

	// Token: 0x060063F9 RID: 25593 RVA: 0x0004E151 File Offset: 0x0004C351
	public void OnClickDownButton()
	{
		if ((this.Page + 1) * 5 >= this.Modlist.Count)
		{
			return;
		}
		this.Page++;
		this.UpdatePage();
	}

	// Token: 0x060063FA RID: 25594 RVA: 0x002367EC File Offset: 0x002349EC
	public GameObject MakeDownButton()
	{
		GameObject gameObject = new GameObject("Down");
		Image image = gameObject.AddComponent<Image>();
		gameObject.transform.SetParent(base.gameObject.transform);
		Texture2D texture2D = new Texture2D(2, 2);
		texture2D.LoadImage(File.ReadAllBytes(Application.dataPath + "/Managed/BaseMod/Image/Down.png"));
		Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0f, 0f));
		image.sprite = sprite;
		image.rectTransform.sizeDelta = new Vector2((float)texture2D.width, (float)texture2D.height);
		gameObject.transform.localScale = new Vector3(1f, 1f);
		Button button = gameObject.AddComponent<Button>();
		button.targetGraphic = image;
		button.onClick.AddListener(delegate()
		{
			this.OnClickDownButton();
		});
		return gameObject;
	}

	// Token: 0x060063FB RID: 25595 RVA: 0x0004E17F File Offset: 0x0004C37F
	public void OnClickUpButton()
	{
		if (this.Page == 0)
		{
			return;
		}
		this.Page--;
		this.UpdatePage();
	}

	// Token: 0x060063FC RID: 25596 RVA: 0x002368D8 File Offset: 0x00234AD8
	public GameObject MakeUpButton()
	{
		GameObject gameObject = new GameObject("Down");
		Image image = gameObject.AddComponent<Image>();
		gameObject.transform.SetParent(base.gameObject.transform);
		Texture2D texture2D = new Texture2D(2, 2);
		texture2D.LoadImage(File.ReadAllBytes(Application.dataPath + "/Managed/BaseMod/Image/Down.png"));
		Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0f, 0f));
		image.sprite = sprite;
		image.rectTransform.sizeDelta = new Vector2((float)texture2D.width, (float)texture2D.height);
		gameObject.transform.localScale = new Vector3(1f, -1f);
		Button button = gameObject.AddComponent<Button>();
		button.targetGraphic = image;
		button.onClick.AddListener(delegate()
		{
			this.OnClickUpButton();
		});
		return gameObject;
	}

	// Token: 0x040059D9 RID: 23001
	public List<ModInfo> Modlist;

	// Token: 0x040059DA RID: 23002
	public List<GameObject> PanelList;

	// Token: 0x040059DB RID: 23003
	public bool init;

	// Token: 0x040059DC RID: 23004
	public GameObject DescPanel;

	// Token: 0x040059DD RID: 23005
	public int Page;

	// Token: 0x040059DE RID: 23006
	public Text Desc;

	// Token: 0x040059DF RID: 23007
	public GameObject Down;

	// Token: 0x040059E0 RID: 23008
	public List<Text> PanelTextList;

	// Token: 0x040059E1 RID: 23009
	public GameObject Up;
}
