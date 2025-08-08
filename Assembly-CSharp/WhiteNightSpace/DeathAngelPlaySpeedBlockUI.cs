using System;
using System.Collections.Generic;
using BinahBoss;
using GameStatusUI;
using UnityEngine;
using UnityEngine.UI;

namespace WhiteNightSpace
{
	// Token: 0x02000AF4 RID: 2804
	public class DeathAngelPlaySpeedBlockUI : PlaySpeedSettingBlockedUI
	{
		// Token: 0x06005463 RID: 21603 RVA: 0x0004492A File Offset: 0x00042B2A
		public DeathAngelPlaySpeedBlockUI()
		{
		}

		// Token: 0x06005464 RID: 21604 RVA: 0x001E5054 File Offset: 0x001E3254
		public void SetAngel(DeathAngel angel)
		{
			this._deathAngel = angel;
			this.pauseTexts.Add(angel.GetFunctionDesc("escape").Trim());
			this.multiplyTexts.Add(angel.GetFunctionDesc("time").Trim());
			this.manualTexts.Add(angel.GetFunctionDesc("manual0").Trim());
			this.manualTexts.Add(angel.GetFunctionDesc("manual1").Trim());
			this.AddAction(PlaySpeedSettingBlockFunction.TIMESTOP_PAUSE, new PlaySpeedSettingBlockedUI.voidAction(this.OnTryPause));
			this.AddAction(PlaySpeedSettingBlockFunction.SPEEDMULTIPLIER, new PlaySpeedSettingBlockedUI.voidAction(this.OnTryTimeMultiplier));
			this.AddAction(PlaySpeedSettingBlockFunction.MANUAL, new PlaySpeedSettingBlockedUI.voidAction(this.OnTryOpenManual));
			this.AddAction(PlaySpeedSettingBlockFunction.ESCAPE, new PlaySpeedSettingBlockedUI.voidAction(this.OnTryEscape));
		}

		// Token: 0x06005465 RID: 21605 RVA: 0x00044953 File Offset: 0x00042B53
		private void Start()
		{
			this.group.alpha = 0f;
		}

		// Token: 0x06005466 RID: 21606 RVA: 0x001E5120 File Offset: 0x001E3320
		public override bool IsFunctionEnabled(PlaySpeedSettingBlockFunction function)
		{
			if (!this._deathAngel.model.IsEscapedOnlyEscape())
			{
				return false;
			}
			if (function == PlaySpeedSettingBlockFunction.ESCAPE || function == PlaySpeedSettingBlockFunction.MANUAL)
			{
				return !GameStatusUI.GameStatusUI.Window.sceneController.IsAgentAlldead;
			}
			if (SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.BINAH, false))
			{
				return (SefiraBossManager.Instance.CurrentBossBase as BinahBossBase).Script.Phase != BinahPhase.P3;
			}
			return !SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.CHOKHMAH, false);
		}

		// Token: 0x06005467 RID: 21607 RVA: 0x001E51AC File Offset: 0x001E33AC
		public override void Update()
		{
			if (this.Timer.started)
			{
				this.group.alpha = this.curve.Evaluate(this.Timer.Rate);
				if (this.Timer.RunTimer())
				{
					this.group.alpha = 0f;
					this.OnClose();
				}
			}
		}

		// Token: 0x06005468 RID: 21608 RVA: 0x001E5210 File Offset: 0x001E3410
		public void OnTryEscape()
		{
			if (!this.IsFunctionEnabled(PlaySpeedSettingBlockFunction.ESCAPE))
			{
				return;
			}
			this.textDisplayed.text = this.pauseTexts[UnityEngine.Random.Range(0, this.pauseTexts.Count)];
			this.OnShow();
			this.MakeSound("creature/deathangel/Lucifer_Bell0");
		}

		// Token: 0x06005469 RID: 21609 RVA: 0x001E5264 File Offset: 0x001E3464
		public void OnTryPause()
		{
			if (!this.IsFunctionEnabled(PlaySpeedSettingBlockFunction.TIMESTOP_PAUSE))
			{
				return;
			}
			if (SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.BINAH, false))
			{
				if ((SefiraBossManager.Instance.CurrentBossBase as BinahBossBase).Script.Phase == BinahPhase.P3)
				{
					return;
				}
			}
			else if (SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.CHOKHMAH, false))
			{
				return;
			}
			this.textDisplayed.text = this.multiplyTexts[UnityEngine.Random.Range(0, this.multiplyTexts.Count)];
			this.OnShow();
			this.MakeSound("creature/deathangel/Lucifer_Bell0");
		}

		// Token: 0x0600546A RID: 21610 RVA: 0x00044965 File Offset: 0x00042B65
		public void OnTryTimeMultiplier()
		{
			if (!this.IsFunctionEnabled(PlaySpeedSettingBlockFunction.SPEEDMULTIPLIER))
			{
				return;
			}
			this.textDisplayed.text = this.multiplyTexts[UnityEngine.Random.Range(0, this.multiplyTexts.Count)];
			this.OnShow();
		}

		// Token: 0x0600546B RID: 21611 RVA: 0x001E5300 File Offset: 0x001E3500
		public void OnTryOpenManual()
		{
			if (!this.IsFunctionEnabled(PlaySpeedSettingBlockFunction.MANUAL))
			{
				return;
			}
			this.textDisplayed.text = this.manualTexts[UnityEngine.Random.Range(0, this.manualTexts.Count)];
			this.OnShow();
			this.MakeSound("creature/deathangel/Lucifer_Bell0");
		}

		// Token: 0x0600546C RID: 21612 RVA: 0x0012D224 File Offset: 0x0012B424
		public void MakeSound(string src)
		{
			SoundEffectPlayer soundEffectPlayer = SoundEffectPlayer.Play(src, CameraMover.instance.transform);
			if (soundEffectPlayer != null)
			{
				soundEffectPlayer.AttachToCamera();
				soundEffectPlayer.src.loop = false;
			}
		}

		// Token: 0x0600546D RID: 21613 RVA: 0x000449A1 File Offset: 0x00042BA1
		public override void OnShow()
		{
			base.OnShow();
		}

		// Token: 0x04004DE5 RID: 19941
		private List<string> pauseTexts = new List<string>();

		// Token: 0x04004DE6 RID: 19942
		private List<string> multiplyTexts = new List<string>();

		// Token: 0x04004DE7 RID: 19943
		private List<string> manualTexts = new List<string>();

		// Token: 0x04004DE8 RID: 19944
		public AnimationCurve curve;

		// Token: 0x04004DE9 RID: 19945
		public CanvasGroup group;

		// Token: 0x04004DEA RID: 19946
		public AudioClip clip;

		// Token: 0x04004DEB RID: 19947
		public Text textDisplayed;

		// Token: 0x04004DEC RID: 19948
		private DeathAngel _deathAngel;
	}
}
