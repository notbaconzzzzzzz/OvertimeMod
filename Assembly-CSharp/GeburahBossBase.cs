using System;
using GeburahBoss;
using UnityEngine;

// Token: 0x0200080D RID: 2061
public class GeburahBossBase : SefiraBossBase
{
	// Token: 0x06004025 RID: 16421 RVA: 0x00192784 File Offset: 0x00190984
	public GeburahBossBase()
	{
		this.sefiraEnum = SefiraEnum.GEBURAH;
		ColorUtility.TryParseHtmlString("#350000FF", out this.hexa_black);
		ColorUtility.TryParseHtmlString("#FF0000FF", out this.hexa_red);
	}

	// Token: 0x170005EE RID: 1518
	// (get) Token: 0x06004026 RID: 16422 RVA: 0x00037799 File Offset: 0x00035999
	private GeburahCoreScript Script
	{
		get
		{
			return this.model.script as GeburahCoreScript;
		}
	}

	// Token: 0x06004027 RID: 16423 RVA: 0x001927E8 File Offset: 0x001909E8
	public override SefiraBossDescType GetDescType(float defaultProb = 0.5f)
	{
		SefiraBossDescType result = SefiraBossDescType.DEFAULT;
		if (UnityEngine.Random.value <= defaultProb)
		{
			return result;
		}
		switch (this.Script.Phase)
		{
		case GeburahPhase.START:
		case GeburahPhase.P1:
			result = SefiraBossDescType.OVERLOAD1;
			break;
		case GeburahPhase.P2:
			result = SefiraBossDescType.OVERLOAD2;
			break;
		case GeburahPhase.P3:
			result = SefiraBossDescType.OVERLOAD3;
			break;
		default:
			result = SefiraBossDescType.OVERLOAD4;
			break;
		}
		return result;
	}

	// Token: 0x06004028 RID: 16424 RVA: 0x00192848 File Offset: 0x00190A48
	public override void OnStageStart()
	{
		MapNode nodeById = MapGraph.instance.GetNodeById("dept-geburah-4");
		this.model = SefiraBossManager.Instance.AddCreature(nodeById, this, "GeburahCoreScript", "GeburahCoreAnim", 400001L);
		this.model.GetMovableNode().SetDirection(UnitDirection.RIGHT);
		this.vignetting = Camera.main.gameObject.AddComponent<CameraFilterPack_TV_Vignetting>();
		this.vignetting.Vignetting = 1f;
		this.vignetting.VignettingFull = 0.245f;
		this.vignetting.VignettingDirt = 0.476f;
		this.vignetting.VignettingColor = Color.black;
		this.superComputer = Camera.main.gameObject.AddComponent<CameraFilterPack_AAA_SuperComputer>();
		this.superComputer._AlphaHexa = 1f;
		this.superComputer.ShapeFormula = 9f;
		this.superComputer.Shape = 0.77f;
		this.superComputer._BorderSize = 0.28f;
		this.superComputer._BorderColor = Color.red;
		this.superComputer.Radius = 0.92f;
		this.hexagon = Camera.main.gameObject.AddComponent<CameraFilterPack_AAA_SuperHexagon>();
		this.hexagon._HexaColor = this.hexa_black;
		this.hexagon._AlphaHexa = 1f;
		this.hexagon.HexaSize = 2.93f;
		this.hexagon._SpotSize = 2.5f;
		this.hexagon.Radius = 0.25f;
		this.hexagon.enabled = false;
		this.Script.SetBossBase(this);
		this.Script.OnStageStart();
		this.StartHexagonEffect();
		SefiraBossManager.Instance.AddBossBgm(new string[]
		{
			"Sounds/BGM/Boss/Geburah/1_Tilarids - Distorted Night",
			"Sounds/BGM/Boss/Geburah/2_Tilarids - Insignia Decay"
		});
		this._cameraDescTimer.StartTimer(5f * UnityEngine.Random.value);
	}

	// Token: 0x06004029 RID: 16425 RVA: 0x000043E5 File Offset: 0x000025E5
	public override void OnKetherStart()
	{
	}

	// Token: 0x0600402A RID: 16426 RVA: 0x00192A28 File Offset: 0x00190C28
	public void InitModel()
	{
		MapNode nodeById = MapGraph.instance.GetNodeById("dept-geburah-4");
		this.model = SefiraBossManager.Instance.AddCreature(nodeById, this, "GeburahCoreScript", "GeburahCoreAnim", 400001L);
		this.model.GetMovableNode().SetDirection(UnitDirection.RIGHT);
		this.Script.SetBossBase(this);
		this.Script.OnStageStart();
	}

	// Token: 0x0600402B RID: 16427 RVA: 0x000374C3 File Offset: 0x000356C3
	private void StartCameraMoveEndFirst()
	{
		CameraMover.instance.ReleaseMove();
	}

	// Token: 0x0600402C RID: 16428 RVA: 0x000377AB File Offset: 0x000359AB
	public override bool IsCleared()
	{
		return this.Script.Phase == GeburahPhase.END;
	}

	// Token: 0x0600402D RID: 16429 RVA: 0x000377BB File Offset: 0x000359BB
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.model != null)
		{
			this.model.OnFixedUpdate();
		}
	}

	// Token: 0x0600402E RID: 16430 RVA: 0x00192A90 File Offset: 0x00190C90
	public override void Update()
	{
		base.Update();
		if (this._hexaEffect.started)
		{
			Color hexaColor = Color.Lerp(this.hexa_red, this.hexa_black, this._hexaEffect.Rate);
			this.hexagon._HexaColor = hexaColor;
			if (this._hexaEffect.RunTimer())
			{
				this.hexagon.enabled = false;
				if (!this._isInit)
				{
					this._isInit = true;
					Vector3 currentViewPosition = this.model.GetCurrentViewPosition();
					currentViewPosition.y += 2f * this.model.GetMovableNode().currentScale;
					CameraMover.instance.SetEndCall(new CameraMover.OnCameraMoveEndEvent(this.StartCameraMoveEndFirst));
					CameraMover.instance.StopMove();
					CameraMover.instance.CameraMoveEvent(currentViewPosition, 8f);
				}
			}
		}
	}

	// Token: 0x0600402F RID: 16431 RVA: 0x000377D9 File Offset: 0x000359D9
	public void StartHexagonEffect()
	{
		if (this.hexagon != null)
		{
			this.hexagon._HexaColor = this.hexa_red;
			this._hexaEffect.StartTimer(4f);
			this.hexagon.enabled = true;
		}
	}

	// Token: 0x06004030 RID: 16432 RVA: 0x00037819 File Offset: 0x00035A19
	public override void OnChangePhase()
	{
		this._phase++;
		if (!SefiraBossManager.Instance.IsKetherBoss())
		{
			SefiraBossManager.Instance.PlayBossBgm(this._phase);
		}
	}

	// Token: 0x06004031 RID: 16433 RVA: 0x00192B6C File Offset: 0x00190D6C
	public override void OnCleared()
	{
		base.OnCleared();
		Vector3 currentViewPosition = this.Script.model.GetCurrentViewPosition();
		currentViewPosition.y += 2.5f;
		CameraMover.instance.CameraMoveEvent(currentViewPosition, 6f);
		CameraMover.instance.StopMove();
	}

	// Token: 0x04003AA2 RID: 15010
	private const string blackColor = "#350000FF";

	// Token: 0x04003AA3 RID: 15011
	private const string redColor = "#FF0000FF";

	// Token: 0x04003AA4 RID: 15012
	private const string animSrc = "GeburahCoreAnim";

	// Token: 0x04003AA5 RID: 15013
	private const string geburahBase = "GeburahCoreScript";

	// Token: 0x04003AA6 RID: 15014
	private const string startNode = "dept-geburah-4";

	// Token: 0x04003AA7 RID: 15015
	private const float _hexagonTime = 4f;

	// Token: 0x04003AA8 RID: 15016
	private const string bgm1 = "Geburah/1_Tilarids - Distorted Night";

	// Token: 0x04003AA9 RID: 15017
	private const string bgm2 = "Geburah/2_Tilarids - Insignia Decay";

	// Token: 0x04003AAA RID: 15018
	public SefiraBossCreatureModel model;

	// Token: 0x04003AAB RID: 15019
	private CameraFilterPack_TV_Vignetting vignetting;

	// Token: 0x04003AAC RID: 15020
	private CameraFilterPack_AAA_SuperComputer superComputer;

	// Token: 0x04003AAD RID: 15021
	private CameraFilterPack_AAA_SuperHexagon hexagon;

	// Token: 0x04003AAE RID: 15022
	private Color hexa_black = Color.black;

	// Token: 0x04003AAF RID: 15023
	private Color hexa_red = Color.red;

	// Token: 0x04003AB0 RID: 15024
	private UnscaledTimer _hexaEffect = new UnscaledTimer();

	// Token: 0x04003AB1 RID: 15025
	private bool _isInit;

	// Token: 0x04003AB2 RID: 15026
	private int _phase = -1;
}
