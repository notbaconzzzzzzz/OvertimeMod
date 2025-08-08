using System;
using UnityEngine;

namespace KetherBoss
{
	// Token: 0x0200081E RID: 2078
	public class KetherLowerBossBase : KetherBossBase
	{
		// Token: 0x0600409A RID: 16538 RVA: 0x00194A6C File Offset: 0x00192C6C
		public KetherLowerBossBase()
		{
			this.type = KetherBossType.E3;
			this.sefiraEnum = SefiraEnum.KETHER;
			this._chokhmah = new ChokhmahBossBase();
			this._binah = new BinahBossBase();
			this.bossBaseList.Add(this._chokhmah);
			this.bossBaseList.Add(this._binah);
			this._levelReached = false;
			this._clearConfirm = false;
		}

		// Token: 0x0600409B RID: 16539 RVA: 0x00194B00 File Offset: 0x00192D00
		public override void OnStageStart()
		{
			base.OnStageStart();
			this._clearEnergyValue = StageTypeInfo.instnace.GetEnergyNeed(PlayerModel.instance.GetDay());
			this.vignetting = Camera.main.gameObject.AddComponent<CameraFilterPack_TV_Vignetting>();
			this.vignetting.Vignetting = 1f;
			this.vignetting.VignettingDirt = 0f;
			this.vignetting.VignettingFull = 0f;
			this.vignetting.VignettingColor = Color.black;
			this.SetCameraRotation(0f);
			this.superComputer = Camera.main.gameObject.AddComponent<CameraFilterPack_AAA_SuperComputer>();
			this.superComputer._AlphaHexa = 0.74f;
			this.superComputer.ShapeFormula = 3.9f;
			this.superComputer.Shape = 0.3f;
			this.superComputer._BorderSize = 1.26f;
			this.superComputer._SpotSize = 2.5f;
			this.superComputer.Radius = 0.77f;
			Color white = Color.white;
			if (ColorUtility.TryParseHtmlString("#F1FF00FF", out white))
			{
				this.superComputer._BorderColor = white;
			}
			this.earthQuake = Camera.main.gameObject.AddComponent<CameraFilterPack_FX_EarthQuake>();
			this.earthQuake.enabled = false;
			this.curve = SefiraBossUI.Instance.ketherLowerRotationCurve;
			SefiraBossManager.Instance.AddBossBgm(new string[]
			{
				"Sounds/BGM/Boss/Event/49/1_Tilarids - 090909090",
				"Sounds/BGM/Boss/Event/49/2_Haunted Streets_1",
				"Sounds/BGM/Boss/Event/49/3_Tilarids - circle-rombed oxygen"
			});
			this.EffectInit();
		}

		// Token: 0x0600409C RID: 16540 RVA: 0x00194C84 File Offset: 0x00192E84
		public override void OnOverloadActivated(int currentLevel)
		{
			this._currentQliphothLevel = currentLevel;
			if (currentLevel == 4)
			{
				this._binah.InitModel();
				this._binah.model.baseMaxHp = (int)((float)this._binah.model.baseMaxHp * 0.6f);
				this._binah.model.hp = (float)this._binah.model.maxHp;
				this._binah.model.Unit.animTarget.animator.gameObject.AddComponent<_FX_CompressionFX_Spine>();
				SefiraBossManager.Instance.PlayBossBgm(1);
			}
			if (currentLevel >= 10)
			{
				this._clearConfirm = true;
			}
			if (currentLevel >= 8)
			{
				if (currentLevel == 8)
				{
					PlaySpeedSettingUI.instance.SetNormalSpeedForcely();
					SefiraBossManager.Instance.PlayBossBgm(2);
					this._chokhmah.Script.AnimScript.OnChangePhase();
					this._binah.Script.HaltExecution();
				}
				this._chokhmah.OnKehterOverloadActivated(currentLevel);
			}
		}

		// Token: 0x0600409D RID: 16541 RVA: 0x00194D88 File Offset: 0x00192F88
		public override bool IsCleared()
		{
			return (this._binah.model != null && this._binah.IsCleared() && this._binah.IsReadyToClose()) || (this._clearConfirm && EnergyModel.instance.GetEnergy() >= this._clearEnergyValue);
		}

		// Token: 0x0600409E RID: 16542 RVA: 0x00037CE7 File Offset: 0x00035EE7
		public override void Update()
		{
			base.Update();
			this.RotationUpdate();
		}

		// Token: 0x0600409F RID: 16543 RVA: 0x00194DEC File Offset: 0x00192FEC
		public void StartRotation(int level)
		{
			this._rotationValue.min = (float)(level - 1) * 18f;
			this._rotationValue.max = (float)level * 18f;
			this._rotationTimer.StartTimer(this._rotationCurve);
			this._effect.SetActive(true);
			this.curve = SefiraBossUI.Instance.ketherLowerRotationCurve;
		}

		// Token: 0x060040A0 RID: 16544 RVA: 0x00194E50 File Offset: 0x00193050
		private void RotationUpdate()
		{
			if (this._rotationTimer.started)
			{
				float cameraRotation = this._rotationValue.GetLerp(this.curve.Evaluate(this._rotationTimer.Rate));
				if (this._rotationTimer.Rate >= 0.4f && !this.earthQuake.enabled)
				{
					this.earthQuake.enabled = true;
				}
				if (this._rotationTimer.RunTimer())
				{
					cameraRotation = this._rotationValue.max;
					this.earthQuake.enabled = false;
				}
				this.SetCameraRotation(cameraRotation);
			}
		}

		// Token: 0x060040A1 RID: 16545 RVA: 0x00194EF0 File Offset: 0x001930F0
		public void SetCameraRotation(float value)
		{
			Transform transform = Camera.main.transform;
			transform.localRotation = Quaternion.Euler(0f, 0f, value);
		}

		// Token: 0x060040A2 RID: 16546 RVA: 0x00194F20 File Offset: 0x00193120
		public void EffectInit()
		{
			GameObject gameObject = Prefab.LoadPrefab("Effect/SefiraBoss/DustCameraAttachedEffect");
			gameObject.transform.SetParent(Camera.main.transform);
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = new Vector3(0f, 0f, 10f);
			gameObject.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
			gameObject.gameObject.SetActive(false);
			this._effect = gameObject;
		}

		// Token: 0x060040A3 RID: 16547 RVA: 0x00194FB0 File Offset: 0x001931B0
		public override SefiraBossDescType GetDescType(float defaultProb = 0.5f)
		{
			switch (this.QliphothOverloadLevel)
			{
			case 0:
			case 1:
			case 2:
			case 3:
				return SefiraBossDescType.OVERLOAD1;
			case 4:
			case 5:
			case 6:
				return SefiraBossDescType.OVERLOAD2;
			case 7:
			case 8:
				return SefiraBossDescType.OVERLOAD3;
			case 9:
				return SefiraBossDescType.OVERLOAD4;
			}
			return SefiraBossDescType.OVERLOAD5;
		}

		// Token: 0x060040A4 RID: 16548 RVA: 0x00037A0B File Offset: 0x00035C0B
		public override bool IsStartEmergencyBgm()
		{
			SefiraBossManager.Instance.PlayBossBgm(0);
			return false;
		}

		// Token: 0x04003B37 RID: 15159
		private const float rotationValue = 18f;

		// Token: 0x04003B38 RID: 15160
		private ChokhmahBossBase _chokhmah;

		// Token: 0x04003B39 RID: 15161
		private BinahBossBase _binah;

		// Token: 0x04003B3A RID: 15162
		private float _clearEnergyValue;

		// Token: 0x04003B3B RID: 15163
		private const int ClearQliphothLevel = 10;

		// Token: 0x04003B3C RID: 15164
		private bool _levelReached;

		// Token: 0x04003B3D RID: 15165
		private bool _clearConfirm;

		// Token: 0x04003B3E RID: 15166
		private CameraFilterPack_TV_Vignetting vignetting;

		// Token: 0x04003B3F RID: 15167
		private CameraFilterPack_AAA_SuperComputer superComputer;

		// Token: 0x04003B40 RID: 15168
		private const string bgm0 = "Sounds/BGM/Boss/Event/49/1_Tilarids - 090909090";

		// Token: 0x04003B41 RID: 15169
		private const string bgm1 = "Sounds/BGM/Boss/Event/49/2_Haunted Streets_1";

		// Token: 0x04003B42 RID: 15170
		private const string bgm2 = "Sounds/BGM/Boss/Event/49/3_Tilarids - circle-rombed oxygen";

		// Token: 0x04003B43 RID: 15171
		private const string effectSrc = "Effect/SefiraBoss/DustCameraAttachedEffect";

		// Token: 0x04003B44 RID: 15172
		private CameraFilterPack_FX_EarthQuake earthQuake;

		// Token: 0x04003B45 RID: 15173
		private AnimationCurve curve;

		// Token: 0x04003B46 RID: 15174
		private UnscaledTimer _rotationTimer = new UnscaledTimer();

		// Token: 0x04003B47 RID: 15175
		private float _rotationCurve = 2f;

		// Token: 0x04003B48 RID: 15176
		private MinMax _rotationValue = new MinMax(0f, 1f);

		// Token: 0x04003B49 RID: 15177
		private GameObject _effect;
	}
}
