/*
private void Update() // Stop from registering keys while ctrl is pressed
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000361 RID: 865
public class CameraMover : MonoBehaviour
{
	// Token: 0x06001AB0 RID: 6832 RVA: 0x000E16BC File Offset: 0x000DF8BC
	public CameraMover()
	{
	}

	// Token: 0x17000220 RID: 544
	// (get) Token: 0x06001AB1 RID: 6833 RVA: 0x0001D9D2 File Offset: 0x0001BBD2
	public static CameraMover instance
	{
		get
		{
			return CameraMover._instance;
		}
	}

	// Token: 0x17000221 RID: 545
	// (get) Token: 0x06001AB2 RID: 6834 RVA: 0x000E1740 File Offset: 0x000DF940
	public int currentRangeLevel
	{
		get
		{
			int num = PlayerModel.instance.GetOpenedAreaCount() - 1;
			if (this._currentRangeLevel == -1)
			{
				if (num >= this.Range.Count)
				{
					num = this.Range.Count - 1;
				}
				else if (num < 0)
				{
					num = 0;
				}
				this._currentRangeLevel = num;
			}
			return this._currentRangeLevel;
		}
	}

	// Token: 0x17000222 RID: 546
	// (get) Token: 0x06001AB3 RID: 6835 RVA: 0x0001D9D9 File Offset: 0x0001BBD9
	private CameraMover.CameraMoveRange currentRange
	{
		get
		{
			return this.Range[this.currentRangeLevel];
		}
	}

	// Token: 0x17000223 RID: 547
	// (get) Token: 0x06001AB4 RID: 6836 RVA: 0x0001D9EC File Offset: 0x0001BBEC
	private float orthoMax
	{
		get
		{
			return this.currentRange.ortho;
		}
	}

	// Token: 0x17000224 RID: 548
	// (get) Token: 0x06001AB5 RID: 6837 RVA: 0x0001D9F9 File Offset: 0x0001BBF9
	// (set) Token: 0x06001AB6 RID: 6838 RVA: 0x0001DA01 File Offset: 0x0001BC01
	private bool movable
	{
		get
		{
			return this._movable;
		}
		set
		{
			this._movable = value;
		}
	}

	// Token: 0x17000225 RID: 549
	// (get) Token: 0x06001AB7 RID: 6839 RVA: 0x0001DA0A File Offset: 0x0001BC0A
	// (set) Token: 0x06001AB8 RID: 6840 RVA: 0x000E17A0 File Offset: 0x000DF9A0
	private Vector3 CameraPos
	{
		get
		{
			return this.targetCamera.transform.position;
		}
		set
		{
			Vector3 movementClamped = this.GetMovementClamped(value);
			this.targetCamera.transform.position = movementClamped;
		}
	}

	// Token: 0x17000226 RID: 550
	// (get) Token: 0x06001AB9 RID: 6841 RVA: 0x0001DA1C File Offset: 0x0001BC1C
	// (set) Token: 0x06001ABA RID: 6842 RVA: 0x0001DA29 File Offset: 0x0001BC29
	public float CameraOrthographicSize
	{
		get
		{
			return this.targetCamera.orthographicSize;
		}
		set
		{
			this.targetCamera.orthographicSize = value;
			Notice.instance.Send(NoticeName.OnChangeCameraSize, new object[]
			{
				this.targetCamera.orthographicSize
			});
		}
	}

	// Token: 0x06001ABB RID: 6843 RVA: 0x000E17C8 File Offset: 0x000DF9C8
	public Vector3 GetMovementClamped(Vector3 input)
	{
		float min = this.currentRange.min.x;
		float max = this.currentRange.max.x;
		float min2 = this.currentRange.min.y;
		float max2 = this.currentRange.max.y;
		if (this.currentRangeLevel >= 10 && SefiraBossManager.Instance.IsKetherBoss(KetherBossType.E4))
		{
			float num = (this.CameraOrthographicSize - this._kether_ortho_min) * this._kether_ortho_inv;
			float lerp = this._kether_x.GetLerp(1f - num);
			float num2 = this._kether_y_min.GetLerp(1f - num) * -1f;
			float lerp2 = this._kether_y_max.GetLerp(1f - num);
			min = -lerp;
			max = lerp;
			min2 = num2;
			max2 = lerp2;
		}
		return new Vector3(Mathf.Clamp(input.x, min, max), Mathf.Clamp(input.y, min2, max2), this.CameraPos.z);
	}

	// Token: 0x06001ABC RID: 6844 RVA: 0x0001DA5F File Offset: 0x0001BC5F
	private void Awake()
	{
		CameraMover._instance = this;
		this._currentRangeLevel = -1;
	}

	// Token: 0x06001ABD RID: 6845 RVA: 0x0001DA6E File Offset: 0x0001BC6E
	private void Start()
	{
		this.ResetCamera = this.CameraPos;
		this.moveScript.Init(this);
	}

	// Token: 0x06001ABE RID: 6846 RVA: 0x0001DA88 File Offset: 0x0001BC88
	public void StopMove()
	{
		this.movable = false;
	}

	// Token: 0x06001ABF RID: 6847 RVA: 0x0001DA91 File Offset: 0x0001BC91
	public void ReleaseMove()
	{
		this.movable = true;
	}

	// Token: 0x06001AC0 RID: 6848 RVA: 0x0001DA9A File Offset: 0x0001BC9A
	public bool IsMovable()
	{
		return this.movable;
	}

	// Token: 0x06001AC1 RID: 6849 RVA: 0x000E18D0 File Offset: 0x000DFAD0
	private void Update()
	{ // <Mod>
		if (this.attached && this.attachTarget != null)
		{
			this.CameraPos = this.attachTarget.GetCurrentViewPosition();
		}
		this.moveScript.Update();
		Vector3 mousePosition = Input.mousePosition;
		float num = Time.unscaledDeltaTime;
		if (num > 1f)
		{
			num = 1f;
		}
		if (this.movable)
		{
            bool ctrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
			Vector3 cameraPos = this.CameraPos;
			Vector3 vector = new Vector3(cameraPos.x, cameraPos.y, cameraPos.z);
			Vector3 vector2 = Vector3.zero;
			Vector3 localPosition = this.targetCamera.transform.localPosition;
			if (Input.GetKey(KeyCode.A) && !ctrl || Input.GetKey(KeyCode.LeftArrow))
			{
				vector2.x = -num * this.CameraOrthographicSize;
			}
			if (Input.GetKey(KeyCode.D) && !ctrl || Input.GetKey(KeyCode.RightArrow))
			{
				vector2.x = num * this.CameraOrthographicSize;
			}
			if (Input.GetKey(KeyCode.S) && !ctrl || Input.GetKey(KeyCode.DownArrow))
			{
				vector2.y = -num * this.CameraOrthographicSize;
			}
			if (Input.GetKey(KeyCode.W) && !ctrl || Input.GetKey(KeyCode.UpArrow))
			{
				vector2.y = num * this.CameraOrthographicSize;
			}
			if (this.targetCamera.transform.localRotation.z != 0f)
			{
				vector2 = this.targetCamera.transform.localRotation * vector2;
			}
			this.CameraPos = localPosition + vector2;
			if (this._target == null)
			{
				if (Input.GetAxis("Mouse ScrollWheel") > 0f)
				{
					float num2 = 0.3f * (1f + (this.CameraOrthographicSize - 5f) / 35f);
					this.CameraOrthographicSize = Mathf.Clamp(this.CameraOrthographicSize - num2 * this.scrollSpeed, 5f, this.orthoMax);
				}
				if (Input.GetAxis("Mouse ScrollWheel") < 0f)
				{
					float num3 = 0.3f * (1f + (this.CameraOrthographicSize - 5f) / 35f);
					this.CameraOrthographicSize = Mathf.Clamp(this.CameraOrthographicSize + num3 * this.scrollSpeed, 5f, this.orthoMax);
				}
			}
			if (Input.GetKey(KeyCode.F))
			{
				float num4 = 0.06f * (1f + (this.CameraOrthographicSize - 5f) / 35f);
				this.CameraOrthographicSize = Mathf.Clamp(this.CameraOrthographicSize - num4 * this.scrollSpeed, 5f, this.orthoMax);
			}
			if (Input.GetKey(KeyCode.G))
			{
				float num5 = 0.06f * (1f + (this.CameraOrthographicSize - 5f) / 35f);
				this.CameraOrthographicSize = Mathf.Clamp(this.CameraOrthographicSize + num5 * this.scrollSpeed, 5f, this.orthoMax);
			}
		}
	}

	// Token: 0x06001AC2 RID: 6850 RVA: 0x000E1BCC File Offset: 0x000DFDCC
	private void LateUpdate()
	{
		if (!this.movable)
		{
			return;
		}
		if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
		{
			this.Diference = this.targetCamera.ScreenToWorldPoint(Input.mousePosition) - this.targetCamera.transform.position;
			if (!this.Drag)
			{
				this.Drag = true;
				CursorManager.instance.CursorSet(MouseCursorType.CLICK);
				this.Origin = this.targetCamera.ScreenToWorldPoint(Input.mousePosition);
			}
		}
		else
		{
			if (this.Drag)
			{
				if (UnitMouseEventManager.instance.suppressCursor)
				{
					CursorManager.instance.CursorSet(MouseCursorType.SUPPRESS);
				}
				else
				{
					CursorManager.instance.CursorSet(MouseCursorType.NORMAL);
				}
			}
			this.Drag = false;
		}
		if (this.Drag)
		{
			Vector3 cameraPos = this.CameraPos;
			this.CameraPos = this.Origin - this.Diference;
			this.dragedPos = this.CameraPos;
			Vector3 localPosition = this.targetCamera.transform.localPosition;
		}
		Notice.instance.Send(NoticeName.OnLateUpdateCamera, new object[0]);
	}

	// Token: 0x06001AC3 RID: 6851 RVA: 0x0001DAA2 File Offset: 0x0001BCA2
	public void Registration(IScrollTarget target)
	{
		this._target = target;
		CursorManager.instance.CursorSet(MouseCursorType.SCROLL);
	}

	// Token: 0x06001AC4 RID: 6852 RVA: 0x0001DAB6 File Offset: 0x0001BCB6
	public void Registration(IScrollTarget target, bool cursorSet)
	{
		this._target = target;
		if (cursorSet)
		{
			CursorManager.instance.CursorSet(MouseCursorType.SCROLL);
		}
	}

	// Token: 0x06001AC5 RID: 6853 RVA: 0x0001DAD0 File Offset: 0x0001BCD0
	public void DeRegistration()
	{
		this._target = null;
		CursorManager.instance.CursorSet(MouseCursorType.NORMAL);
	}

	// Token: 0x06001AC6 RID: 6854 RVA: 0x000E1CF8 File Offset: 0x000DFEF8
	public void Recoil(int level)
	{
		List<RecoilArrow> list = RecoilEffect.MakeRecoilArrow(level * this.recoil.recoilCount);
		Vector3 localPosition = this.recoil.targetTransform.localPosition;
		this.movable = false;
		Queue<Vector3> queue = new Queue<Vector3>();
		float scale = this.recoil.scale * (this.CameraOrthographicSize / this.DefaultOrtho);
		foreach (RecoilArrow arrow in list)
		{
			queue.Enqueue(RecoilEffect.GetVector(arrow, localPosition, scale));
		}
		queue.Enqueue(localPosition);
		base.StartCoroutine(this.PlayRecoil(queue, this.recoil.maxTime));
	}

	// Token: 0x06001AC7 RID: 6855 RVA: 0x000E1DC4 File Offset: 0x000DFFC4
	public void Recoil(int level, float maxTime)
	{
		List<RecoilArrow> list = RecoilEffect.MakeRecoilArrow((int)((float)(level * this.recoil.recoilCount) * (maxTime * 3f)));
		Vector3 localPosition = this.recoil.targetTransform.localPosition;
		this.movable = false;
		Queue<Vector3> queue = new Queue<Vector3>();
		float scale = this.recoil.scale * (this.CameraOrthographicSize / this.DefaultOrtho);
		foreach (RecoilArrow arrow in list)
		{
			queue.Enqueue(RecoilEffect.GetVector(arrow, localPosition, scale));
		}
		queue.Enqueue(localPosition);
		base.StartCoroutine(this.PlayRecoil(queue, maxTime));
	}

	// Token: 0x06001AC8 RID: 6856 RVA: 0x000E1E90 File Offset: 0x000E0090
	public void Recoil(int level, float maxTime, int recoilCount, float scale)
	{
		List<RecoilArrow> list = RecoilEffect.MakeRecoilArrow((int)((float)(level * recoilCount) * (maxTime * 3f)));
		Vector3 localPosition = this.recoil.targetTransform.localPosition;
		this.movable = false;
		Queue<Vector3> queue = new Queue<Vector3>();
		float scale2 = scale * (this.CameraOrthographicSize / this.DefaultOrtho);
		foreach (RecoilArrow arrow in list)
		{
			queue.Enqueue(RecoilEffect.GetVector(arrow, localPosition, scale2));
		}
		queue.Enqueue(localPosition);
		base.StartCoroutine(this.PlayRecoil(queue, maxTime));
	}

	// Token: 0x06001AC9 RID: 6857 RVA: 0x000E1F4C File Offset: 0x000E014C
	private IEnumerator PlayRecoil(Queue<Vector3> queue, float maxTime)
	{
		int val = queue.Count;
		float unitStep = maxTime / (float)val;
		while (queue.Count > 1)
		{
			yield return new WaitForSeconds(unitStep);
			Vector3 recoilUnit = queue.Dequeue();
			this.recoil.targetTransform.localPosition = new Vector3(recoilUnit.x, recoilUnit.y, recoilUnit.z);
		}
		if (this.Drag)
		{
			this.recoil.targetTransform.position = this.dragedPos;
		}
		else
		{
			this.recoil.targetTransform.localPosition = queue.Dequeue();
		}
		this.movable = true;
		yield break;
	}

	// Token: 0x06001ACA RID: 6858 RVA: 0x0001DAE4 File Offset: 0x0001BCE4
	public void SetSettingToDefault()
	{
		this.CameraPos = this.DefaultPos;
		this.CameraOrthographicSize = this.DefaultOrtho;
	}

	// Token: 0x06001ACB RID: 6859 RVA: 0x000E1F78 File Offset: 0x000E0178
	public void SetSettingToStart()
	{
		this.CameraPos = new Vector3(0f, -5f, this.CameraPos.z);
		this.CameraOrthographicSize = 20f;
	}

	// Token: 0x06001ACC RID: 6860 RVA: 0x000E1FB4 File Offset: 0x000E01B4
	public void SetTutorialCam()
	{
		float x = 0f;
		float y = 0f;
		float cameraOrthographicSize = 15f;
		this.CameraPos = new Vector3(x, y, this.CameraPos.z);
		this.CameraOrthographicSize = cameraOrthographicSize;
		this.movable = false;
	}

	// Token: 0x06001ACD RID: 6861 RVA: 0x0001DAFE File Offset: 0x0001BCFE
	public void AttachToMovable(MovableObjectNode mov)
	{
		this.attached = true;
		this.attachTarget = mov;
	}

	// Token: 0x06001ACE RID: 6862 RVA: 0x0001DB0E File Offset: 0x0001BD0E
	public void DecoupleToMovable()
	{
		this.attachTarget = null;
		this.attached = false;
	}

	// Token: 0x06001ACF RID: 6863 RVA: 0x000E1FFC File Offset: 0x000E01FC
	public void GenObserveMovement(CreatureModel target)
	{
		PlaySpeedSettingUI.instance.OnPause(PAUSECALL.OBSERVE);
		this.StopMove();
		Vector3 position = target.Unit.room.transform.position;
		this.saveOrtho = this.CameraOrthographicSize;
		float ortho = 4f;
		position = new Vector3(position.x + -1f + 0.4f, position.y + -1.7f + 0.9f, this.CameraPos.z);
		this.moveScript.StartMoveByTime(position, ortho, 1f);
		this.isObserving = true;
		this.observeTarget = target;
	}

	// Token: 0x06001AD0 RID: 6864 RVA: 0x000E20A0 File Offset: 0x000E02A0
	public void CameraMoveEvent(Vector3 dest, float ortho)
	{
		dest = new Vector3(dest.x, dest.y, this.CameraPos.z);
		this.moveScript.StartMoveByTime(dest, ortho, 1f);
	}

	// Token: 0x06001AD1 RID: 6865 RVA: 0x000E20E4 File Offset: 0x000E02E4
	public void CameraMoveEvent(Vector3 dest, float ortho, float time)
	{
		dest = new Vector3(dest.x, dest.y, this.CameraPos.z);
		this.moveScript.StartMoveByTime(dest, ortho, time);
	}

	// Token: 0x06001AD2 RID: 6866 RVA: 0x0001DB1E File Offset: 0x0001BD1E
	public void OnCameraMoveEnd()
	{
		if (this.isObserving)
		{
			this.OnObserveCameraMoveEnd();
			return;
		}
		if (this.endCall != null)
		{
			this.endCall();
			this.endCall = null;
		}
	}

	// Token: 0x06001AD3 RID: 6867 RVA: 0x0001DB4F File Offset: 0x0001BD4F
	public void SetEndCall(CameraMover.OnCameraMoveEndEvent call)
	{
		this.endCall = call;
	}

	// Token: 0x06001AD4 RID: 6868 RVA: 0x0001DB58 File Offset: 0x0001BD58
	private void OnObserveCameraMoveEnd()
	{
		MaxObservation.instance.OnCameraMoveEnd();
	}

	// Token: 0x06001AD5 RID: 6869 RVA: 0x0001DB64 File Offset: 0x0001BD64
	public void OnObserveEnded()
	{
		this.isObserving = false;
		this.observeTarget = null;
		PlaySpeedSettingUI.instance.OnResume(PAUSECALL.OBSERVE);
		this.ReleaseMove();
	}

	// Token: 0x06001AD6 RID: 6870 RVA: 0x000E2124 File Offset: 0x000E0324
	public void OnStageStart()
	{
		this.ReleaseMove();
		this.CameraPos = new Vector3(0f, (this.currentRange.min.y + this.currentRange.max.y) / 2f, this.CameraPos.z);
		this.CameraOrthographicSize = this.currentRange.ortho;
	}

	// Token: 0x04001B5F RID: 7007
	private static CameraMover _instance;

	// Token: 0x04001B60 RID: 7008
	public Camera targetCamera;

	// Token: 0x04001B61 RID: 7009
	private const float ObserveOrthoSize = 4f;

	// Token: 0x04001B62 RID: 7010
	private const float roomXCorrection = -1f;

	// Token: 0x04001B63 RID: 7011
	private const float roomYCorrection = -1.7f;

	// Token: 0x04001B64 RID: 7012
	public Vector3 CameraMin;

	// Token: 0x04001B65 RID: 7013
	public Vector3 CameraMax;

	// Token: 0x04001B66 RID: 7014
	public Vector3 DefaultPos;

	// Token: 0x04001B67 RID: 7015
	public float DefaultOrtho;

	// Token: 0x04001B68 RID: 7016
	public GameObject player;

	// Token: 0x04001B69 RID: 7017
	public GameObject escapeButton;

	// Token: 0x04001B6A RID: 7018
	public float scrollSpeed;

	// Token: 0x04001B6B RID: 7019
	public RecoilEffect recoil;

	// Token: 0x04001B6C RID: 7020
	private IScrollTarget _target;

	// Token: 0x04001B6D RID: 7021
	private Vector3 ResetCamera;

	// Token: 0x04001B6E RID: 7022
	private Vector3 Origin;

	// Token: 0x04001B6F RID: 7023
	private Vector3 Diference;

	// Token: 0x04001B70 RID: 7024
	private bool Drag;

	// Token: 0x04001B71 RID: 7025
	public CameraMover.OnCameraMoveEndEvent endCall;

	// Token: 0x04001B72 RID: 7026
	public List<CameraMover.CameraMoveRange> Range;

	// Token: 0x04001B73 RID: 7027
	private int _currentRangeLevel = -1;

	// Token: 0x04001B74 RID: 7028
	private float saveOrtho;

	// Token: 0x04001B75 RID: 7029
	public CameraMover.CameraForcelyMove moveScript = new CameraMover.CameraForcelyMove();

	// Token: 0x04001B76 RID: 7030
	private bool _movable = true;

	// Token: 0x04001B77 RID: 7031
	private bool attached;

	// Token: 0x04001B78 RID: 7032
	private MovableObjectNode attachTarget;

	// Token: 0x04001B79 RID: 7033
	private Vector3 dragedPos;

	// Token: 0x04001B7A RID: 7034
	public long creatureTarget;

	// Token: 0x04001B7B RID: 7035
	public long agentTarget;

	// Token: 0x04001B7C RID: 7036
	private bool isObserving;

	// Token: 0x04001B7D RID: 7037
	[NonSerialized]
	private CreatureModel observeTarget;

	// Token: 0x04001B7E RID: 7038
	private MinMax _kether_x = new MinMax(44f, 100f);

	// Token: 0x04001B7F RID: 7039
	private MinMax _kether_y_min = new MinMax(109f, 160f);

	// Token: 0x04001B80 RID: 7040
	private MinMax _kether_y_max = new MinMax(-60f, 10f);

	// Token: 0x04001B81 RID: 7041
	private float _kether_ortho_inv = 0.0117f;

	// Token: 0x04001B82 RID: 7042
	private float _kether_ortho_min = 5f;

	// Token: 0x02000362 RID: 866
	[Serializable]
	public class CameraMoveRange
	{
		// Token: 0x06001AD7 RID: 6871 RVA: 0x000042F0 File Offset: 0x000024F0
		public CameraMoveRange()
		{
		}

		// Token: 0x04001B83 RID: 7043
		public Vector2 min;

		// Token: 0x04001B84 RID: 7044
		public Vector2 max;

		// Token: 0x04001B85 RID: 7045
		public float ortho;
	}

	// Token: 0x02000363 RID: 867
	public class CameraForcelyMove
	{
		// Token: 0x06001AD8 RID: 6872 RVA: 0x0001DB85 File Offset: 0x0001BD85
		public CameraForcelyMove()
		{
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06001AD9 RID: 6873 RVA: 0x0001DB94 File Offset: 0x0001BD94
		private Transform transform
		{
			get
			{
				return this.script.transform;
			}
		}

		// Token: 0x06001ADA RID: 6874 RVA: 0x0001DBA1 File Offset: 0x0001BDA1
		public void Init(CameraMover script)
		{
			this.script = script;
		}

		// Token: 0x06001ADB RID: 6875 RVA: 0x000E2190 File Offset: 0x000E0390
		public void StartMoveByTime(Vector3 dest, float ortho, float time)
		{
			this.dest = dest;
			this.startOrtho = this.script.CameraOrthographicSize;
			this.destOrtho = ortho;
			this.startPos = this.transform.position;
			this.started = true;
			this.startTime = Time.unscaledTime;
			if (time == 0f)
			{
				time = 1f;
			}
			this.time = time;
		}

		// Token: 0x06001ADC RID: 6876 RVA: 0x000E21F8 File Offset: 0x000E03F8
		public void Update()
		{
			if (this.started)
			{
				float num = (Time.unscaledTime - this.startTime) / this.time;
				this.transform.position = Vector3.Lerp(this.startPos, this.dest, num);
				if (this.orthoCheck)
				{
					this.script.CameraOrthographicSize = Mathf.Lerp(this.startOrtho, this.destOrtho, num);
				}
				if (num >= 1f)
				{
					this.OnMoveEnd();
					this.started = false;
				}
			}
		}

		// Token: 0x06001ADD RID: 6877 RVA: 0x0001DBAA File Offset: 0x0001BDAA
		public void OnMoveEnd()
		{
			this.script.OnCameraMoveEnd();
		}

		// Token: 0x04001B86 RID: 7046
		public CameraMover script;

		// Token: 0x04001B87 RID: 7047
		public Vector3 dest;

		// Token: 0x04001B88 RID: 7048
		public float destOrtho;

		// Token: 0x04001B89 RID: 7049
		public float time;

		// Token: 0x04001B8A RID: 7050
		public float startOrtho;

		// Token: 0x04001B8B RID: 7051
		public Vector3 startPos;

		// Token: 0x04001B8C RID: 7052
		public bool started;

		// Token: 0x04001B8D RID: 7053
		private bool orthoCheck = true;

		// Token: 0x04001B8E RID: 7054
		private float startTime;
	}

	// Token: 0x02000364 RID: 868
	// (Invoke) Token: 0x06001ADF RID: 6879
	public delegate void OnCameraMoveEndEvent();
}
