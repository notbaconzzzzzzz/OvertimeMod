using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace CreatureInfo
{
	// Token: 0x02000ACC RID: 2764
	public class CreatureInfoCodex : MonoBehaviour
	{
		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x06005369 RID: 21353 RVA: 0x00043E8C File Offset: 0x0004208C
		public static CreatureInfoCodex Codex
		{
			get
			{
				return CreatureInfoWindow.CurrentWindow.codex;
			}
		}

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x0600536A RID: 21354 RVA: 0x00043E98 File Offset: 0x00042098
		private bool Scrollable
		{
			get
			{
				return this._scrollElap <= 0f;
			}
		}

		// Token: 0x0600536B RID: 21355 RVA: 0x00043EAA File Offset: 0x000420AA
		private void Start()
		{
			this.Observation_Title.text = LocalizeTextDataModel.instance.GetText("Codex_Title");
		}

		// Token: 0x0600536C RID: 21356 RVA: 0x001E5FE4 File Offset: 0x001E41E4
		private void Update()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				this.MoveDown();
			}
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				this.MoveUp();
			}
			if (this.Scrollable)
			{
				if (Input.mouseScrollDelta.y < 0f)
				{
					this.MoveDown();
					this._scrollElap += this.ScrollDelay;
				}
				else if (Input.mouseScrollDelta.y > 0f)
				{
					this.MoveUp();
					this._scrollElap += this.ScrollDelay;
				}
			}
			if (this._scrollElap > 0f)
			{
				this._scrollElap -= Time.unscaledDeltaTime;
			}
			else if (this._scrollElap < 0f)
			{
				this._scrollElap = 0f;
			}
		}

		// Token: 0x0600536D RID: 21357 RVA: 0x00043EC6 File Offset: 0x000420C6
		public void MoveUp()
		{
			if (this.currentDisplayIndex != 0)
			{
				CreatureInfoWindow.CurrentWindow.audioClipPlayer.OnPlayInList(3);
			}
			this.SetList(this.currentDisplayIndex - 1);
		}

		// Token: 0x0600536E RID: 21358 RVA: 0x00043EF1 File Offset: 0x000420F1
		public void MoveDown()
		{
			if (this.currentDisplayIndex != this.maxDisplayIndex)
			{
				CreatureInfoWindow.CurrentWindow.audioClipPlayer.OnPlayInList(3);
			}
			this.SetList(this.currentDisplayIndex + 1);
		}

		// Token: 0x0600536F RID: 21359 RVA: 0x00043F22 File Offset: 0x00042122
		public Color GetRiskLevelColor(RiskLevel risk)
		{
			return this.riskColor[(int)risk];
		}

		// Token: 0x06005370 RID: 21360 RVA: 0x001E60E0 File Offset: 0x001E42E0
		public void Init()
		{ // <Patch>
			this.Clear();
			List<CreatureObserveInfoModel> observeInfoList = CreatureManager.instance.GetObserveInfoList();
			List<CreatureObserveInfoModel> list = new List<CreatureObserveInfoModel>();
			foreach (CreatureObserveInfoModel creatureObserveInfoModel in observeInfoList)
			{
				if (creatureObserveInfoModel.creatureTypeId <= 200000L || creatureObserveInfoModel.creatureTypeId >= 300000L)
				{
					if (creatureObserveInfoModel.creatureTypeId <= 400000L)
					{
						if (creatureObserveInfoModel.IsMaxObserved())
						{
							list.Add(creatureObserveInfoModel);
						}
					}
				}
			}
			List<LobotomyBaseMod.CreatureInfoCodex_SortData_Mod> list2 = new List<LobotomyBaseMod.CreatureInfoCodex_SortData_Mod>();
			foreach (CreatureObserveInfoModel creatureObserveInfoModel2 in list)
			{
				CreatureTypeInfo data = CreatureTypeList.instance.GetData_Mod(creatureObserveInfoModel2.lcid);
				string codeId = data.codeId;
				int num = -1;
				if (!this.TryParse(codeId, out num))
				{
					if (creatureObserveInfoModel2.lcid.packageId != string.Empty || creatureObserveInfoModel2.creatureTypeId != CreatureInfoCodex.uniqueId[1])
					{
						continue;
					}
					num = 1000;
				}
				int num2 = num;
				if (creatureObserveInfoModel2.lcid.packageId != string.Empty)
				{
					this.GenerateSlot_Mod(list2, creatureObserveInfoModel2, data, num);
				}
				else if (this.CheckUniqueGeneration_Mod(creatureObserveInfoModel2.lcid, list2, num, out num2))
				{
					num = num2;
					this.GenerateSlot_Mod(list2, creatureObserveInfoModel2, data, num);
				}
			}
			List<LobotomyBaseMod.CreatureInfoCodex_SortData_Mod> list3 = list2;
			if (CreatureInfoCodex.cache1 == null)
			{
				CreatureInfoCodex.cache1 = new Comparison<LobotomyBaseMod.CreatureInfoCodex_SortData_Mod>(LobotomyBaseMod.CreatureInfoCodex_SortData_Mod.Compare);
			}
			list3.Sort(CreatureInfoCodex.cache1);
			this.displayList_Mod.Clear();
			foreach (LobotomyBaseMod.CreatureInfoCodex_SortData_Mod sortData in list2)
			{
				this.displayList_Mod.Add(sortData.id);
			}
			int num3 = this.displayList_Mod.Count % 15;
			this.maxDisplayIndex = this.displayList_Mod.Count / 15;
			if (num3 == 0)
			{
				this.maxDisplayIndex--;
			}
			this.SetList(0);
			this.allocateFilters.Clear();
			this._dontouchmeCount = 0;
			this.SetPercentage();
		}

		// Token: 0x06005371 RID: 21361 RVA: 0x001E6344 File Offset: 0x001E4544
		private void GenerateSlot(List<CreatureInfoCodex.SortData> sort, CreatureObserveInfoModel info, CreatureTypeInfo typeInfo, int index)
		{
			CreatureInfoCodex.SortData item = new CreatureInfoCodex.SortData
			{
				index = index,
				id = typeInfo.id
			};
			sort.Add(item);
			GameObject gameObject;
			if (CreatureGenerateInfo.IsCreditCreature(typeInfo.id))
			{
				gameObject = UnityEngine.Object.Instantiate<GameObject>(this._creditSlot);
			}
			else
			{
				gameObject = UnityEngine.Object.Instantiate<GameObject>(this._slot);
			}
			gameObject.transform.SetParent(this._disabledLayout);
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = Vector3.zero;
			this.slotDic.Add(info.creatureTypeId, gameObject);
			CreatureInfoCodexSlot component = gameObject.GetComponent<CreatureInfoCodexSlot>();
			component.Init(typeInfo, info);
		}

		// Token: 0x06005372 RID: 21362 RVA: 0x00043F35 File Offset: 0x00042135
		public void OnMetaClose()
		{
			this.SetList(this.GetCurrentDisplayedIndex() / 15);
		}

		// Token: 0x06005373 RID: 21363 RVA: 0x00043F46 File Offset: 0x00042146
		public void OnOpen()
		{
			this.SimpleStat.SetActive(false);
		}

		// Token: 0x06005374 RID: 21364 RVA: 0x001E63F8 File Offset: 0x001E45F8
		private bool TryParse(string code, out int index)
		{
			string[] array = code.Split(new char[]
			{
				'-'
			});
			string s = array[array.Length - 1];
			return int.TryParse(s, out index);
		}

		// Token: 0x06005375 RID: 21365 RVA: 0x001E6428 File Offset: 0x001E4628
		private void Clear()
		{ // <Patch>
			foreach (GameObject gameObject in this.slotDic_Mod.Values)
			{
				UnityEngine.Object.Destroy(gameObject.gameObject);
			}
			this.slotDic_Mod.Clear();
		}

		// Token: 0x06005376 RID: 21366 RVA: 0x001E6498 File Offset: 0x001E4698
		public void SetList(int index)
		{ // <Patch>
			if (index < 0)
			{
				return;
			}
			if (index > this.maxDisplayIndex)
			{
				return;
			}
			this.MoveEnabledToDisable();
			this.currentDisplayIndex = index;
			int num = this.currentDisplayIndex * 15;
			int num2 = num + 15;
			for (int i = num; i < num2; i++)
			{
				if (i < this.displayList_Mod.Count)
				{
					GameObject gameObject = this.slotDic_Mod[this.displayList_Mod[i]];
					gameObject.transform.SetParent(this._layout);
				}
			}
			this.UpperArrow.interactable = true;
			this.LowerArrow.interactable = true;
			if (index == 0)
			{
				this.UpperArrow.interactable = false;
			}
			if (index == this.maxDisplayIndex)
			{
				this.LowerArrow.interactable = false;
			}
		}

		// Token: 0x06005377 RID: 21367 RVA: 0x001E656C File Offset: 0x001E476C
		private void MoveEnabledToDisable()
		{
			List<Transform> list = new List<Transform>();
			IEnumerator enumerator = this._layout.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					if (!(transform == this._layout))
					{
						list.Add(transform);
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			foreach (Transform transform2 in list)
			{
				transform2.SetParent(this._disabledLayout);
			}
		}

		// Token: 0x06005378 RID: 21368 RVA: 0x001E663C File Offset: 0x001E483C
		public void OnOeverlayEnter(CreatureTypeInfo typeInfo)
		{
			this.SimpleStat.SetActive(true);
			this.Name.text = typeInfo.collectionName;
			this.Code.text = typeInfo.codeId;
			this.Risk.text = typeInfo.GetRiskLevel().ToString();
			this.Risk.color = UIColorManager.instance.GetRiskColor(typeInfo.GetRiskLevel());
			this.Portrait.sprite = Add_On.GetPortrait(typeInfo.portraitSrc);
		}

		// Token: 0x06005379 RID: 21369 RVA: 0x00043F46 File Offset: 0x00042146
		public void OnOverlayExit()
		{
			this.SimpleStat.SetActive(false);
		}

		// Token: 0x0600537A RID: 21370 RVA: 0x00043F54 File Offset: 0x00042154
		public void OnClickCodexUnit(CreatureTypeInfo metaInfo)
		{
			this.CheckUniqueAction(metaInfo.id);
			if (!this.CheckIdValidation(metaInfo.id))
			{
				return;
			}
			CreatureInfoWindow.CurrentWindow.OpenCodexCreatureInfo(metaInfo);
			this.UpdateArrow(this.GetCurrentDisplayedIndex());
		}

		// Token: 0x0600537B RID: 21371 RVA: 0x001E66C8 File Offset: 0x001E48C8
		private int GetCurrentDisplayedIndex()
		{ // <Patch>
			LobotomyBaseMod.LcIdLong currentMetaId = CreatureInfoWindow.CurrentWindow.CurrentMetaIdMod;
			if (currentMetaId == -1L)
			{
				return -1;
			}
			if (!this.displayList_Mod.Contains(currentMetaId))
			{
				return -1;
			}
			return this.displayList_Mod.IndexOf(currentMetaId);
		}

		// Token: 0x0600537C RID: 21372 RVA: 0x001E670C File Offset: 0x001E490C
		public void MoveNext()
		{ // <Patch>
			int num = this.GetCurrentDisplayedIndex();
			if (num == this.displayList_Mod.Count - 1)
			{
				return;
			}
			LobotomyBaseMod.LcIdLong metaId = this.displayList_Mod[++num];
			while (!this.CheckIdValidation_Mod(metaId))
			{
				try
				{
					metaId = this.displayList_Mod[++num];
				}
				catch (Exception ex)
				{
					return;
				}
			}
			CreatureInfoWindow.CurrentWindow.OpenCodexCreatureInfo_Mod(metaId);
			CreatureInfoWindow.CurrentWindow.audioClipPlayer.OnPlayInList(3);
			this.UpdateArrow(num);
		}

		// Token: 0x0600537D RID: 21373 RVA: 0x001E67A4 File Offset: 0x001E49A4
		public void MovePrev()
		{ // <Patch>
			int num = this.GetCurrentDisplayedIndex();
			if (num == 0)
			{
				return;
			}
			LobotomyBaseMod.LcIdLong metaId = this.displayList_Mod[--num];
			while (!this.CheckIdValidation_Mod(metaId))
			{
				try
				{
					metaId = this.displayList_Mod[--num];
				}
				catch (Exception ex)
				{
					return;
				}
			}
			CreatureInfoWindow.CurrentWindow.OpenCodexCreatureInfo_Mod(metaId);
			CreatureInfoWindow.CurrentWindow.audioClipPlayer.OnPlayInList(3);
			this.UpdateArrow(num);
		}

		// Token: 0x0600537E RID: 21374 RVA: 0x001E6830 File Offset: 0x001E4A30
		private void UpdateArrow(int current)
		{ // <Patch>
			CreatureInfoWindow.CurrentWindow.PrevCodex.interactable = true;
			CreatureInfoWindow.CurrentWindow.NextCodex.interactable = true;
			if (current == 0)
			{
				CreatureInfoWindow.CurrentWindow.PrevCodex.interactable = false;
			}
			else if (current == this.displayList_Mod.Count - 1)
			{
				CreatureInfoWindow.CurrentWindow.NextCodex.interactable = false;
			}
		}

		// Token: 0x0600537F RID: 21375 RVA: 0x00043F8B File Offset: 0x0004218B
		public bool CheckIdValidation(long metaId)
		{
			return metaId != CreatureInfoCodex.uniqueId[2];
		}

		// Token: 0x06005380 RID: 21376 RVA: 0x00043F9D File Offset: 0x0004219D
		private void CheckUniqueAction(long id)
		{
			if (id == CreatureInfoCodex.uniqueId[2])
			{
				this.DontTouchMe();
			}
		}

		// Token: 0x06005381 RID: 21377 RVA: 0x001E689C File Offset: 0x001E4A9C
		private bool CheckUniqueGeneration(long id, List<CreatureInfoCodex.SortData> list, int currentIndex, out int changedIndex)
		{
			changedIndex = currentIndex;
			if (id == CreatureInfoCodex.uniqueId[0])
			{
				return false;
			}
			if (id == CreatureInfoCodex.uniqueId[3])
			{
				return false;
			}
			if (id == CreatureInfoCodex.uniqueId[4])
			{
				CreatureTypeInfo data = CreatureTypeList.instance.GetData(CreatureInfoCodex.uniqueId[3]);
				CreatureObserveInfoModel observeInfo = CreatureManager.instance.GetObserveInfo(CreatureInfoCodex.uniqueId[3]);
				if (!observeInfo.IsMaxObserved())
				{
					observeInfo.ObserveAll(new string[0]);
				}
				this.GenerateSlot(list, observeInfo, data, currentIndex);
				changedIndex = currentIndex + 1;
			}
			return true;
		}

		// Token: 0x06005382 RID: 21378 RVA: 0x001E6924 File Offset: 0x001E4B24
		private void DontTouchMe()
		{
			if (GameManager.currentGameManager != null)
			{
				CreatureModel creatureModel;
				if ((creatureModel = CreatureManager.instance.FindCreature(CreatureInfoCodex.uniqueId[2])) != null)
				{
					creatureModel.script.OnOpenCollectionWindow();
				}
				return;
			}
			if (this._dontouchmeCount >= 3)
			{
				CreatureInfoWindow.CurrentWindow.CloseWindow();
				return;
			}
			Camera camera = CreatureInfoWindow.CurrentWindow.RootCanvas.worldCamera;
			if (camera == null)
			{
				camera = Camera.main;
			}
			CameraFilterPack_TV_BrokenGlass cameraFilterPack_TV_BrokenGlass = camera.gameObject.AddComponent<CameraFilterPack_TV_BrokenGlass>();
			int count = this.allocateFilters.Count;
			if (count != 0)
			{
				if (count != 1)
				{
					if (count == 2)
					{
						cameraFilterPack_TV_BrokenGlass.Broken_Big = 128f;
					}
				}
				else
				{
					cameraFilterPack_TV_BrokenGlass.Broken_High = 128f;
				}
			}
			else
			{
				cameraFilterPack_TV_BrokenGlass.Broken_High = 36f;
			}
			this.allocateFilters.Add(cameraFilterPack_TV_BrokenGlass);
			this._dontouchmeCount++;
		}

		// Token: 0x06005383 RID: 21379 RVA: 0x001E6A20 File Offset: 0x001E4C20
		public void SetPercentage()
		{
			int maxHiddenProgressByObserveLevel = CreatureManager.instance.GetMaxHiddenProgressByObserveLevel();
			int hiddenProgressByObserveLevel = CreatureManager.instance.GetHiddenProgressByObserveLevel();
			int num = 100 * hiddenProgressByObserveLevel / maxHiddenProgressByObserveLevel;
			this.Observation_Percent.text = string.Format("{0}%", num);
		}

		// <Patch>
		public bool CheckIdValidation_Mod(LobotomyBaseMod.LcIdLong metaId)
		{
			return metaId != CreatureInfoCodex.uniqueId[2];
		}

		// <Patch>
		private bool CheckUniqueGeneration_Mod(LobotomyBaseMod.LcIdLong id, List<LobotomyBaseMod.CreatureInfoCodex_SortData_Mod> list, int currentIndex, out int changedIndex)
		{
			changedIndex = currentIndex;
			if (id == CreatureInfoCodex.uniqueId[0])
			{
				return false;
			}
			if (id == CreatureInfoCodex.uniqueId[3])
			{
				return false;
			}
			if (id == CreatureInfoCodex.uniqueId[4])
			{
				CreatureTypeInfo data = CreatureTypeList.instance.GetData(CreatureInfoCodex.uniqueId[3]);
				CreatureObserveInfoModel observeInfo = CreatureManager.instance.GetObserveInfo(CreatureInfoCodex.uniqueId[3]);
				if (!observeInfo.IsMaxObserved())
				{
					observeInfo.ObserveAll(new string[0]);
				}
				this.GenerateSlot_Mod(list, observeInfo, data, currentIndex);
				changedIndex = currentIndex + 1;
			}
			return true;
		}

		// <Patch>
		private void GenerateSlot_Mod(List<LobotomyBaseMod.CreatureInfoCodex_SortData_Mod> sort, CreatureObserveInfoModel info, CreatureTypeInfo typeInfo, int index)
		{
			LobotomyBaseMod.LcIdLong lcIdLong = new LobotomyBaseMod.LcIdLong(CreatureTypeList.instance.GetModId(typeInfo), typeInfo.id);
			LobotomyBaseMod.CreatureInfoCodex_SortData_Mod item = new LobotomyBaseMod.CreatureInfoCodex_SortData_Mod
			{
				index = index,
				id = lcIdLong
			};
			sort.Add(item);
			GameObject gameObject;
			if (lcIdLong.packageId == string.Empty && CreatureGenerateInfo.IsCreditCreature(typeInfo.id))
			{
				gameObject = UnityEngine.Object.Instantiate<GameObject>(this._creditSlot);
			}
			else
			{
				gameObject = UnityEngine.Object.Instantiate<GameObject>(this._slot);
			}
			gameObject.transform.SetParent(this._disabledLayout);
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = Vector3.zero;
			this.slotDic_Mod.Add(info.lcid, gameObject);
			CreatureInfoCodexSlot component = gameObject.GetComponent<CreatureInfoCodexSlot>();
			component.Init(typeInfo, info);
		}

		// Token: 0x04004CC9 RID: 19657
		public GameObject _activeControl;

		// Token: 0x04004CCA RID: 19658
		private const int displayCount = 15;

		// Token: 0x04004CCB RID: 19659
		public Color[] riskColor;

		// Token: 0x04004CCC RID: 19660
		public GameObject _slot;

		// Token: 0x04004CCD RID: 19661
		public GameObject _creditSlot;

		// Token: 0x04004CCE RID: 19662
		public RectTransform _layout;

		// Token: 0x04004CCF RID: 19663
		public RectTransform _disabledLayout;

		// Token: 0x04004CD0 RID: 19664
		public Button UpperArrow;

		// Token: 0x04004CD1 RID: 19665
		public Button LowerArrow;

		// Token: 0x04004CD2 RID: 19666
		public float ScrollDelay = 0.5f;

		// Token: 0x04004CD3 RID: 19667
		[Header("Simple Stat")]
		public GameObject SimpleStat;

		// Token: 0x04004CD4 RID: 19668
		public Text Name;

		// Token: 0x04004CD5 RID: 19669
		public Text Code;

		// Token: 0x04004CD6 RID: 19670
		public Text Risk;

		// Token: 0x04004CD7 RID: 19671
		public Image Portrait;

		// Token: 0x04004CD8 RID: 19672
		public Text Observation_Title;

		// Token: 0x04004CD9 RID: 19673
		public Text Observation_Percent;

		// Token: 0x04004CDA RID: 19674
		private static long[] uniqueId = new long[]
		{
			100000L,
			100034L,
			100024L,
			100014L,
			100015L
		};

		// Token: 0x04004CDB RID: 19675
		private const char div = '-';

		// Token: 0x04004CDC RID: 19676
		private int currentDisplayIndex;

		// Token: 0x04004CDD RID: 19677
		private int maxDisplayIndex;

		// Token: 0x04004CDE RID: 19678
		private Dictionary<long, GameObject> slotDic = new Dictionary<long, GameObject>();

		// Token: 0x04004CDF RID: 19679
		private float _scrollElap;

		// Token: 0x04004CE0 RID: 19680
		private List<long> displayList = new List<long>();

		// Token: 0x04004CE1 RID: 19681
		private int _dontouchmeCount;

		// Token: 0x04004CE2 RID: 19682
		private List<MonoBehaviour> allocateFilters = new List<MonoBehaviour>();

		// Token: 0x04004CE3 RID: 19683
		[CompilerGenerated]
		private static Comparison<CreatureInfoCodex.SortData> cache0;

		// Token: 0x04004CE4 RID: 19684
		[CompilerGenerated]
		private static Comparison<CreatureInfoCodex.SortData> f__mg_cache0;

		// <Patch>
		[NonSerialized]
		private Dictionary<LobotomyBaseMod.LcIdLong, GameObject> slotDic_Mod = new Dictionary<LobotomyBaseMod.LcIdLong, GameObject>();

		// <Patch>
		[NonSerialized]
		private List<LobotomyBaseMod.LcIdLong> displayList_Mod = new List<LobotomyBaseMod.LcIdLong>();

		// <Patch>
		[NonSerialized]
		private static Comparison<LobotomyBaseMod.CreatureInfoCodex_SortData_Mod> cache1;

		// Token: 0x02000ACD RID: 2765
		public class SortData
		{
			// Token: 0x06005386 RID: 21382 RVA: 0x00043FD9 File Offset: 0x000421D9
			public static int Compare(CreatureInfoCodex.SortData a, CreatureInfoCodex.SortData b)
			{
				if (a.index == b.index)
				{
					return a.id.CompareTo(b.id);
				}
				return a.index.CompareTo(b.index);
			}

			// Token: 0x04004CE5 RID: 19685
			public int index = -1;

			// Token: 0x04004CE6 RID: 19686
			public long id;
		}
	}
}
