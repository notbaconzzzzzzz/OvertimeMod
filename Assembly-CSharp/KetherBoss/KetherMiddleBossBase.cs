/*
+public override void AdjustOrdealSpawnTime(int[] _ordealSpawnTime) // 
*/
using System;
using UnityEngine;

namespace KetherBoss
{
	// Token: 0x0200081F RID: 2079
	public class KetherMiddleBossBase : KetherBossBase
	{
		// Token: 0x060040A5 RID: 16549 RVA: 0x00195004 File Offset: 0x00193204
		public KetherMiddleBossBase()
		{
			this.type = KetherBossType.E2;
			this.sefiraEnum = SefiraEnum.KETHER;
			this._tiphereth = new TipherethBossBase();
			this._chesed = new ChesedBossBase();
			this._geburah = new GeburahBossBase();
			this.bossBaseList.Add(this._tiphereth);
			this.bossBaseList.Add(this._chesed);
			this.bossBaseList.Add(this._geburah);
		}

		// Token: 0x060040A6 RID: 16550 RVA: 0x001950FC File Offset: 0x001932FC
		public override void OnStageStart()
		{
			base.OnStageStart();
			this._clearEnergyValue = StageTypeInfo.instnace.GetEnergyNeed(PlayerModel.instance.GetDay());
			this.vignetting = Camera.main.gameObject.AddComponent<CameraFilterPack_TV_Vignetting>();
			this.vignetting.Vignetting = 1f;
			this.vignetting.VignettingDirt = 0f;
			this.vignetting.VignettingFull = 0f;
			this.vignetting.VignettingColor = Color.black;
			this.artefact = Camera.main.gameObject.AddComponent<CameraFilterPack_TV_Artefact>();
			this.artefact.Fade = 0f;
			this.artefact.Colorisation = 1f;
			this.artefact.Parasite = 1f;
			this.artefact.Noise = 1f;
			this.movie = Camera.main.gameObject.AddComponent<CameraFilterPack_TV_Old_Movie_2>();
			this.movie.FramePerSecond = 1.1f;
			this.movie.Contrast = 0.68f;
			this.movie.Burn = 1.19f;
			this.movie.SceneCut = 1.29f;
			this.movie.Fade = 0.2f;
			this.movie.enabled = false;
			SefiraBossManager.Instance.AddBossBgm(new string[]
			{
				"Sounds/BGM/Boss/Event/48/1_Dark Fantasy Scene",
				"Sounds/BGM/Boss/Event/48/2_Tilarids - Insignia Decay",
				"Sounds/BGM/Boss/Event/48/3_Battle_-_Urgent_Encounter"
			});
			this._chesed.SetDamageMultiplied(2);
		}

		// Token: 0x060040A7 RID: 16551 RVA: 0x00195278 File Offset: 0x00193478
		public override void OnOverloadActivated(int currentLevel)
		{
			this._currentQliphothLevel = currentLevel;
			this._tiphereth.OnOverloadActivated(currentLevel);
			if (currentLevel == 4)
			{
				this._geburah.InitModel();
				this._geburah.model.baseMaxHp = (int)((float)this._geburah.model.baseMaxHp * 0.6f);
				this._geburah.model.hp = (float)this._geburah.model.maxHp;
				(this._geburah.model.script as GeburahCoreScript).AnimScript.KetherAttach();
				SefiraBossManager.Instance.PlayBossBgm(1);
			}
			if (currentLevel >= 4 && currentLevel <= 6)
			{
				this._chesed.SetDamageMultiplied(0);
			}
			else
			{
				if (currentLevel < 4)
				{
					this._chesed.SetDamageMultiplied(2);
				}
				else
				{
					this._chesed.SetDamageMultiplied(3);
				}
				if (currentLevel == 7)
				{
					SefiraBossManager.Instance.PlayBossBgm(2);
				}
			}
			this._artefactEffectTimer.StartTimer(this._artefactEffectTime);
			if (currentLevel >= 7)
			{
				if (!this.movie.enabled)
				{
					this.movie.enabled = true;
				}
				else
				{
					if (currentLevel == 8)
					{
						this._movieEffectTimer.StartTimer(this._movieEffectTime);
						this.fpsEffect.min = 1.5f;
						this.fpsEffect.max = 1.75f;
						this.fadeEffect.min = 0.2f;
						this.fadeEffect.max = 0.4f;
					}
					if (currentLevel == 9)
					{
						this._movieEffectTimer.StartTimer(this._movieEffectTime);
						this.fpsEffect.min = 1.75f;
						this.fpsEffect.max = 2f;
						this.fadeEffect.min = 0.4f;
						this.fadeEffect.max = 0.6f;
					}
				}
			}
		}

		// Token: 0x060040A8 RID: 16552 RVA: 0x0019545C File Offset: 0x0019365C
		public override void Update()
		{
			base.Update();
			if (this._movieEffectTimer.started)
			{
				float framePerSecond = this.fpsEffect.GetLerp(this._movieEffectTimer.Rate);
				float fade = this.fadeEffect.GetLerp(this._movieEffectTimer.Rate);
				if (this._movieEffectTimer.RunTimer())
				{
					framePerSecond = this.fpsEffect.max;
					fade = this.fadeEffect.max;
				}
				this.movie.FramePerSecond = framePerSecond;
				this.movie.Fade = fade;
			}
			if (this._artefactEffectTimer.started)
			{
				float rate = this._artefactEffectTimer.Rate;
				float fade2;
				if (rate >= 0.5f)
				{
					fade2 = Mathf.Lerp(1f, 0f, (rate - 0.5f) * 2f);
				}
				else
				{
					fade2 = Mathf.Lerp(0f, 1f, rate * 2f);
				}
				if (this._artefactEffectTimer.RunTimer())
				{
					fade2 = 0f;
				}
				this.artefact.Fade = fade2;
			}
		}

		// Token: 0x060040A9 RID: 16553 RVA: 0x00195578 File Offset: 0x00193778
		public override bool IsCleared()
		{
			return (this._geburah.model != null && this._geburah.IsCleared() && this._geburah.IsReadyToClose()) || (this.QliphothOverloadLevel >= 10 && EnergyModel.instance.GetEnergy() >= this._clearEnergyValue);
		}

		// Token: 0x060040AA RID: 16554 RVA: 0x001955E0 File Offset: 0x001937E0
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
				return SefiraBossDescType.OVERLOAD3;
			case 8:
				return SefiraBossDescType.OVERLOAD4;
			}
			return SefiraBossDescType.OVERLOAD5;
		}

		// Token: 0x060040AB RID: 16555 RVA: 0x00037A0B File Offset: 0x00035C0B
		public override bool IsStartEmergencyBgm()
		{
			SefiraBossManager.Instance.PlayBossBgm(0);
			return false;
		}

        // <Mod>
        public override void AdjustOrdealSpawnTime(int[] _ordealSpawnTime)
        {
            _ordealSpawnTime[1] = 3;
            _ordealSpawnTime[2] = 7;
        }

		// Token: 0x04003B4A RID: 15178
		private const int ClearQliphothLevel = 10;

		// Token: 0x04003B4B RID: 15179
		private float _clearEnergyValue;

		// Token: 0x04003B4C RID: 15180
		private TipherethBossBase _tiphereth;

		// Token: 0x04003B4D RID: 15181
		private ChesedBossBase _chesed;

		// Token: 0x04003B4E RID: 15182
		private GeburahBossBase _geburah;

		// Token: 0x04003B4F RID: 15183
		private CameraFilterPack_TV_Vignetting vignetting;

		// Token: 0x04003B50 RID: 15184
		private CameraFilterPack_TV_Artefact artefact;

		// Token: 0x04003B51 RID: 15185
		private CameraFilterPack_TV_Old_Movie_2 movie;

		// Token: 0x04003B52 RID: 15186
		private MinMax _fps = new MinMax(1.5f, 2f);

		// Token: 0x04003B53 RID: 15187
		private MinMax _fade = new MinMax(0.2f, 0.6f);

		// Token: 0x04003B54 RID: 15188
		private const int tvOldEnableLevel = 7;

		// Token: 0x04003B55 RID: 15189
		private const int tvOldEndLevel = 10;

		// Token: 0x04003B56 RID: 15190
		private UnscaledTimer _movieEffectTimer = new UnscaledTimer();

		// Token: 0x04003B57 RID: 15191
		private float _movieEffectTime = 2f;

		// Token: 0x04003B58 RID: 15192
		private UnscaledTimer _artefactEffectTimer = new UnscaledTimer();

		// Token: 0x04003B59 RID: 15193
		private float _artefactEffectTime = 2f;

		// Token: 0x04003B5A RID: 15194
		private MinMax fpsEffect = new MinMax(0f, 1f);

		// Token: 0x04003B5B RID: 15195
		private MinMax fadeEffect = new MinMax(0f, 1f);

		// Token: 0x04003B5C RID: 15196
		private const string bgm0 = "Sounds/BGM/Boss/Event/48/1_Dark Fantasy Scene";

		// Token: 0x04003B5D RID: 15197
		private const string bgm1 = "Sounds/BGM/Boss/Event/48/2_Tilarids - Insignia Decay";

		// Token: 0x04003B5E RID: 15198
		private const string bgm2 = "Sounds/BGM/Boss/Event/48/3_Battle_-_Urgent_Encounter";

		// Token: 0x02000820 RID: 2080
		public enum KetherMiddlePhase
		{
			// Token: 0x04003B60 RID: 15200
			FIRST,
			// Token: 0x04003B61 RID: 15201
			SECOND
		}
	}
}
