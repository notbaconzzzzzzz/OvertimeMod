/*
+public override void AdjustOrdealSpawnTime(int[] _ordealSpawnTime) // 
*/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KetherBoss
{
	// Token: 0x02000821 RID: 2081
	public class KetherUpperBossBase : KetherBossBase
	{
		// Token: 0x060040AC RID: 16556 RVA: 0x00195634 File Offset: 0x00193834
		public KetherUpperBossBase()
		{
			this.type = KetherBossType.E1;
			this.sefiraEnum = SefiraEnum.KETHER;
			this.bossBaseList.Add(new MalkutBossBase());
			this.bossBaseList.Add(new YesodBossBase());
			this.bossBaseList.Add(new NetzachBossBase());
			this.bossBaseList.Add(new HodBossBase());
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x060040AD RID: 16557 RVA: 0x00037CF5 File Offset: 0x00035EF5
		public KetherUpperBossBase.KetherUpperPhase Phase
		{
			get
			{
				return this._phase;
			}
		}

		// Token: 0x060040AE RID: 16558 RVA: 0x001956AC File Offset: 0x001938AC
		public override void OnStageStart()
		{
			base.OnStageStart();
			this.totalEnergy = StageTypeInfo.instnace.GetEnergyNeed(PlayerModel.instance.GetDay());
			SefiraBossManager.Instance.GenerateHodBuf();
			this._pixelisation = Camera.main.gameObject.AddComponent<CameraFilterPack_Pixel_Pixelisation>();
			this._pixelisation._Pixelisation = 2f;
			this._pixelisation._SizeX = 0.6f;
			this._pixelisation._SizeY = 1f;
			this._pixelisation.enabled = false;
			this._glitch = Camera.main.gameObject.AddComponent<CameraFilterPack_FX_Glitch3>();
			this._glitch._Noise = 1f;
			this._glitch._Glitch = 0.06f;
			this._glitch.enabled = false;
			this.vignetting = Camera.main.gameObject.AddComponent<CameraFilterPack_TV_Vignetting>();
			this.vignetting.Vignetting = 1f;
			this.vignetting.VignettingFull = 0f;
			this.vignetting.VignettingDirt = 0f;
			this.vignetting.VignettingColor = Color.black;
			this.superComputer = Camera.main.gameObject.AddComponent<CameraFilterPack_AAA_SuperComputer>();
			this.superComputer._AlphaHexa = 0.742f;
			this.superComputer.ShapeFormula = 3.9f;
			this.superComputer.Shape = 0.3f;
			this.superComputer._BorderSize = 1.26f;
			Color white = Color.white;
			if (ColorUtility.TryParseHtmlString("#C3D1FFFF", out white))
			{
				this.superComputer._BorderColor = white;
			}
			this.SetHodBufValue(0f);
			this._phase = KetherUpperBossBase.KetherUpperPhase.FIRST;
			SefiraBossManager.Instance.AddBossBgm(new string[]
			{
				"Sounds/BGM/Boss/Event/47/1_Tilarids - Red Dots",
				"Sounds/BGM/Boss/Event/47/2_Tilarids - Faded",
				"Sounds/BGM/Boss/Event/47/3_Theme_-_Retro_Time_ALT(Mix)",
				"Sounds/BGM/Boss/Event/47/4_Tilarids - Blue Dots",
				"Sounds/BGM/Boss/Event/47/5_Tilarids - Violation Of Black Colors"
			});
		}

		// Token: 0x060040AF RID: 16559 RVA: 0x00037CFD File Offset: 0x00035EFD
		public void SetCameraScript(YesodBossCameraScript cameraScript)
		{
			this.cameraScript = cameraScript;
		}

		// Token: 0x060040B0 RID: 16560 RVA: 0x00037D06 File Offset: 0x00035F06
		public override void OnStageEnd()
		{
			base.OnStageEnd();
		}

		// Token: 0x060040B1 RID: 16561 RVA: 0x0019588C File Offset: 0x00193A8C
		public void SetHodBufValue(float value)
		{
			foreach (HodBossBuf hodBossBuf in this.bufList)
			{
				hodBossBuf.SetReduceValue(value);
			}
		}

		// Token: 0x060040B2 RID: 16562 RVA: 0x001958E8 File Offset: 0x00193AE8
		public override bool IsCleared()
		{
			float energy = EnergyModel.instance.GetEnergy();
			return energy >= this.totalEnergy && this.QliphothOverloadLevel >= 6;
		}

		// Token: 0x060040B3 RID: 16563 RVA: 0x00037D0E File Offset: 0x00035F0E
		public override void OnCleared()
		{
			base.OnCleared();
			SefiraBossManager.Instance.ResetYesodBossSetting();
		}

		// Token: 0x060040B4 RID: 16564 RVA: 0x0019591C File Offset: 0x00193B1C
		public override void OnOverloadActivated(int currentLevel)
		{
			this._currentQliphothLevel = currentLevel;
			switch (currentLevel)
			{
			case 1:
				SefiraBossManager.Instance.PlayBossBgm(1);
				break;
			case 2:
				this._phase = KetherUpperBossBase.KetherUpperPhase.SECOND;
				this.OnPhaseShift();
				SefiraBossManager.Instance.GenYesodBossSetting();
				SefiraBossManager.Instance.PlayBossBgm(2);
				break;
			case 3:
				this.SetHodBufValue(50f);
				SefiraBossManager.Instance.PlayBossBgm(3);
				break;
			case 4:
				SefiraBossManager.Instance.SetWorkCancelableState(false);
				SefiraBossManager.Instance.SetRecoverBlockState(true);
				this._phase = KetherUpperBossBase.KetherUpperPhase.THIRD;
				this.OnPhaseShift();
				SefiraBossManager.Instance.PlayBossBgm(4);
				break;
			}
			if (currentLevel >= 4)
			{
				(this.bossBaseList[2] as NetzachBossBase).RecoverAll();
			}
		}

		// Token: 0x060040B5 RID: 16565 RVA: 0x00037D20 File Offset: 0x00035F20
		private void OnPhaseShift()
		{
			SefiraBossManager.Instance.RandomizeWorkId();
			this._phaseShiftTimer.StartTimer(this._phaseShiftEffectTime);
		}

		// Token: 0x060040B6 RID: 16566 RVA: 0x000140B9 File Offset: 0x000122B9
		public override bool IsReadyToClose()
		{
			return true;
		}

		// Token: 0x060040B7 RID: 16567 RVA: 0x001959F0 File Offset: 0x00193BF0
		public override void Update()
		{
			base.Update();
			if (this._phaseShiftTimer.started)
			{
				float rate = this._phaseShiftTimer.Rate;
				float shape;
				if (rate >= 0.5f)
				{
					shape = Mathf.Lerp(1.1f, 0.3f, (rate - 0.5f) * 2f);
				}
				else
				{
					shape = Mathf.Lerp(0.3f, 1.1f, rate * 2f);
				}
				if (this._phaseShiftTimer.RunTimer())
				{
					shape = 0.3f;
				}
				this.superComputer.Shape = shape;
			}
		}

		// Token: 0x060040B8 RID: 16568 RVA: 0x00195A8C File Offset: 0x00193C8C
		public override SefiraBossDescType GetDescType(float defaultProb = 0.5f)
		{
			switch (this.QliphothOverloadLevel)
			{
			case 0:
			case 1:
				return SefiraBossDescType.OVERLOAD1;
			case 2:
				return SefiraBossDescType.OVERLOAD2;
			case 3:
				return SefiraBossDescType.OVERLOAD3;
			case 4:
				return SefiraBossDescType.OVERLOAD4;
			}
			return SefiraBossDescType.OVERLOAD5;
		}

		// Token: 0x060040B9 RID: 16569 RVA: 0x00037A0B File Offset: 0x00035C0B
		public override bool IsStartEmergencyBgm()
		{
			SefiraBossManager.Instance.PlayBossBgm(0);
			return false;
		}

        // <Mod>
        public override void AdjustOrdealSpawnTime(int[] _ordealSpawnTime)
        {
            _ordealSpawnTime[1] = 4;
        }

		// Token: 0x04003B62 RID: 15202
		private const float _hodReduceVal = 50f;

		// Token: 0x04003B63 RID: 15203
		private YesodBossCameraScript cameraScript;

		// Token: 0x04003B64 RID: 15204
		public List<HodBossBuf> bufList;

		// Token: 0x04003B65 RID: 15205
		private CameraFilterPack_Pixel_Pixelisation _pixelisation;

		// Token: 0x04003B66 RID: 15206
		private CameraFilterPack_FX_Glitch3 _glitch;

		// Token: 0x04003B67 RID: 15207
		private CameraFilterPack_TV_Vignetting vignetting;

		// Token: 0x04003B68 RID: 15208
		private CameraFilterPack_AAA_SuperComputer superComputer;

		// Token: 0x04003B69 RID: 15209
		private KetherUpperBossBase.KetherUpperPhase _phase;

		// Token: 0x04003B6A RID: 15210
		private float totalEnergy;

		// Token: 0x04003B6B RID: 15211
		private const int clearQliphothLevel = 6;

		// Token: 0x04003B6C RID: 15212
		private UnscaledTimer _phaseShiftTimer = new UnscaledTimer();

		// Token: 0x04003B6D RID: 15213
		private float _phaseShiftEffectTime = 3f;

		// Token: 0x04003B6E RID: 15214
		private const string bgm0 = "Sounds/BGM/Boss/Event/47/1_Tilarids - Red Dots";

		// Token: 0x04003B6F RID: 15215
		private const string bgm1 = "Sounds/BGM/Boss/Event/47/2_Tilarids - Faded";

		// Token: 0x04003B70 RID: 15216
		private const string bgm2 = "Sounds/BGM/Boss/Event/47/3_Theme_-_Retro_Time_ALT(Mix)";

		// Token: 0x04003B71 RID: 15217
		private const string bgm3 = "Sounds/BGM/Boss/Event/47/4_Tilarids - Blue Dots";

		// Token: 0x04003B72 RID: 15218
		private const string bgm4 = "Sounds/BGM/Boss/Event/47/5_Tilarids - Violation Of Black Colors";

		// Token: 0x02000822 RID: 2082
		public enum KetherUpperPhase
		{
			// Token: 0x04003B74 RID: 15220
			FIRST,
			// Token: 0x04003B75 RID: 15221
			SECOND,
			// Token: 0x04003B76 RID: 15222
			THIRD
		}
	}
}
