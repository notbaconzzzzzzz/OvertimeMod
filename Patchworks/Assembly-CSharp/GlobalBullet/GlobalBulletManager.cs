/*
buncha fuckin shit // 
*/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GlobalBullet
{
	// Token: 0x020006AD RID: 1709
	public class GlobalBulletManager
	{
		// Token: 0x06003758 RID: 14168 RVA: 0x00165AAC File Offset: 0x00163CAC
		private GlobalBulletManager()
		{ // <Mod>
			this._funcs = new Dictionary<GlobalBulletType, GlobalBulletManager.BulletFunc>();
			this._funcs.Add(GlobalBulletType.RECOVER_HP, new GlobalBulletManager.BulletFunc(this.RecoverHPBullet));
			this._funcs.Add(GlobalBulletType.RECOVER_MENTAL, new GlobalBulletManager.BulletFunc(this.RecoverMentalBullet));
			this._funcs.Add(GlobalBulletType.RESIST_R, new GlobalBulletManager.BulletFunc(this.ResistRBullet));
			this._funcs.Add(GlobalBulletType.RESIST_W, new GlobalBulletManager.BulletFunc(this.ResistWBullet));
			this._funcs.Add(GlobalBulletType.RESIST_B, new GlobalBulletManager.BulletFunc(this.ResistBBullet));
			this._funcs.Add(GlobalBulletType.RESIST_P, new GlobalBulletManager.BulletFunc(this.ResistPBullet));
			this._funcs.Add(GlobalBulletType.SLOW, new GlobalBulletManager.BulletFunc(this.SlowBullet));
			this._funcs.Add(GlobalBulletType.EXCUTE, new GlobalBulletManager.BulletFunc(this.ExcuteBullet));
			_funcs.Add(GlobalBulletType.STIM, new BulletFunc(StimBullet));
			_funcs.Add(GlobalBulletType.TRANQ, new BulletFunc(TranqBullet));
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06003759 RID: 14169 RVA: 0x00031822 File Offset: 0x0002FA22
		public static GlobalBulletManager instance
		{
			get
			{
				if (GlobalBulletManager._instance == null)
				{
					GlobalBulletManager._instance = new GlobalBulletManager();
				}
				return GlobalBulletManager._instance;
			}
		}

		// Token: 0x0600375A RID: 14170 RVA: 0x00165BA4 File Offset: 0x00163DA4
		public void OnStageStart()
		{ // <Mod>
			GlobalBulletWindow currentWindow = GlobalBulletWindow.CurrentWindow;
			currentWindow.SetBulletCountMax(this.maxBullet);
			this.Reload();
			refillsAvailable = 0;
			if (ResearchDataModel.instance.IsUpgradedAbility("bullet_refill"))
			{
				refillsAvailable += 1;
			}
			bool active = false;
			if (ResearchDataModel.instance.IsUpgradedBullet(GlobalBulletType.RECOVER_HP))
			{
				currentWindow.SetSlotActive(GlobalBulletType.RECOVER_HP, true);
				active = true;
			}
			else
			{
				currentWindow.SetSlotActive(GlobalBulletType.RECOVER_HP, false);
			}
			if (ResearchDataModel.instance.IsUpgradedBullet(GlobalBulletType.RECOVER_MENTAL))
			{
				currentWindow.SetSlotActive(GlobalBulletType.RECOVER_MENTAL, true);
				active = true;
			}
			else
			{
				currentWindow.SetSlotActive(GlobalBulletType.RECOVER_MENTAL, false);
			}
			if (ResearchDataModel.instance.IsUpgradedBullet(GlobalBulletType.RESIST_R))
			{
				currentWindow.SetSlotActive(GlobalBulletType.RESIST_R, true);
				active = true;
			}
			else
			{
				currentWindow.SetSlotActive(GlobalBulletType.RESIST_R, false);
			}
			if (ResearchDataModel.instance.IsUpgradedBullet(GlobalBulletType.RESIST_W))
			{
				currentWindow.SetSlotActive(GlobalBulletType.RESIST_W, true);
				active = true;
			}
			else
			{
				currentWindow.SetSlotActive(GlobalBulletType.RESIST_W, false);
			}
			if (ResearchDataModel.instance.IsUpgradedBullet(GlobalBulletType.RESIST_B))
			{
				currentWindow.SetSlotActive(GlobalBulletType.RESIST_B, true);
				active = true;
			}
			else
			{
				currentWindow.SetSlotActive(GlobalBulletType.RESIST_B, false);
			}
			if (MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.TIPERERTH1))
			{
				currentWindow.SetSlotActive(GlobalBulletType.RESIST_P, true);
				active = true;
			}
			else
			{
				currentWindow.SetSlotActive(GlobalBulletType.RESIST_P, false);
			}
			if (ResearchDataModel.instance.IsUpgradedBullet(GlobalBulletType.SLOW))
			{
				currentWindow.SetSlotActive(GlobalBulletType.SLOW, true);
				active = true;
			}
			else
			{
				currentWindow.SetSlotActive(GlobalBulletType.SLOW, false);
			}
			if (ResearchDataModel.instance.IsUpgradedBullet(GlobalBulletType.EXCUTE))
			{
				currentWindow.SetSlotActive(GlobalBulletType.EXCUTE, true);
				active = true;
			}
			else
			{
				currentWindow.SetSlotActive(GlobalBulletType.EXCUTE, false);
			}
			currentWindow.SetActive(active);
		}

		// Token: 0x0600375B RID: 14171 RVA: 0x00003E21 File Offset: 0x00002021
		public void OnStageRelease()
		{
		}

		// Token: 0x0600375C RID: 14172 RVA: 0x0003183D File Offset: 0x0002FA3D
		public void OnFixedUpdate()
		{
			if (GlobalBulletWindow.CurrentWindow == null)
			{
				return;
			}
			if (GlobalBulletWindow.CurrentWindow.CurrentSelectedBullet != GlobalBulletType.NONE)
			{
			}
		}

		// Token: 0x0600375D RID: 14173 RVA: 0x00165CDC File Offset: 0x00163EDC
		private void UpdateUI()
		{
			if (GlobalBulletWindow.CurrentWindow == null)
			{
				return;
			}
			GlobalBulletWindow.CurrentWindow.SetBulletCount(this.currentBullet);
			if (this.currentBullet > 0)
			{
				GlobalBulletWindow.CurrentWindow.SetBulletFillGauge(1f);
			}
			else
			{
				GlobalBulletWindow.CurrentWindow.SetBulletFillGauge(0f);
			}
		}

		// Token: 0x0600375E RID: 14174 RVA: 0x0003185F File Offset: 0x0002FA5F
		public void SetMaxBullet(int max)
		{
			this.initialMaxBullet = max;
			this.UpdateMaxBullet();
		}

		// Token: 0x0600375F RID: 14175 RVA: 0x00165D3C File Offset: 0x00163F3C
		public void UpdateMaxBullet()
		{ // <Mod>
			GlobalBulletWindow currentWindow = GlobalBulletWindow.CurrentWindow;
			this.maxBullet = this.initialMaxBullet;
			if (MissionManager.instance.ExistsFinishedOvertimeBossMission(SefiraEnum.TIPERERTH1))
			{
				maxBullet += (int)Mathf.Round((float)maxBullet * 0.5f);
			}
			else if (MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.TIPERERTH1))
			{
				this.maxBullet += (int)Mathf.Round((float)this.maxBullet * 0.3f);
			}
			this.maxBullet += SefiraAbilityValueInfo.chesedOfficerAliveValues[SefiraManager.instance.GetOfficerAliveLevel(SefiraEnum.CHESED)] * (ResearchDataModel.instance.IsUpgradedAbility("upgrade_officer_bonuses") ? 2 : 1);
			if (SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.NETZACH, true))
			{
				int num = 0;
				foreach (AgentModel agent in AgentManager.instance.GetAgentList())
				{
					if (!agent.IsDead()) num++;
				}
				maxBullet += num / 2;
			}
			if (this.currentBullet > this.maxBullet)
			{
				this.currentBullet = this.maxBullet;
			}
			if (currentWindow != null)
			{
				currentWindow.SetBulletCountMax(this.maxBullet);
			}
			GlobalBulletWindow.CurrentWindow.SetBulletCount(this.currentBullet);
		}

		// Token: 0x06003760 RID: 14176 RVA: 0x0003186E File Offset: 0x0002FA6E
		public void Reload()
		{
			this.currentBullet = this.maxBullet;
			this.UpdateUI();
		}

		// Token: 0x06003761 RID: 14177 RVA: 0x00165DF0 File Offset: 0x00163FF0
		public bool ActivateBullet(GlobalBulletType type, List<UnitModel> targets)
		{ // <Mod>
			if (this.currentBullet <= 0)
			{
				if (refillsAvailable > 0)
				{
					refillsAvailable--;
					Reload();
					SoundEffectPlayer soundEffectPlayer = SoundEffectPlayer.PlayOnce("Bullet/Kit_Equip", Vector2.zero, 4f);
					if (soundEffectPlayer != null)
					{
						soundEffectPlayer.AttachToCamera();
					}
				}
				else
				{
					CursorManager.instance.CannotAnim();
					SoundEffectPlayer soundEffectPlayer = SoundEffectPlayer.PlayOnce("Bullet/Bullet_Empty", Vector2.zero);
					if (soundEffectPlayer != null)
					{
						soundEffectPlayer.AttachToCamera();
					}
					return false;
				}
			}
			SoundEffectPlayer soundEffectPlayer2 = SoundEffectPlayer.PlayOnce("Bullet/Bullet_Fire", Vector2.zero);
			if (soundEffectPlayer2 != null)
			{
				soundEffectPlayer2.AttachToCamera();
			}
			switch (type)
			{
			case GlobalBulletType.RECOVER_HP:
			case GlobalBulletType.RECOVER_MENTAL:
				SoundEffectPlayer.PlayOnce("Bullet/Bullet_Heal", Vector2.zero);
				break;
			case GlobalBulletType.RESIST_R:
			case GlobalBulletType.RESIST_W:
			case GlobalBulletType.RESIST_B:
			case GlobalBulletType.RESIST_P:
				SoundEffectPlayer.PlayOnce("Bullet/Bullet_Shield", Vector2.zero);
				break;
			case GlobalBulletType.SLOW:
				SoundEffectPlayer.PlayOnce("Bullet/Bullet_Slow", Vector2.zero);
				break;
			case GlobalBulletType.EXCUTE:
				SoundEffectPlayer.PlayOnce("Bullet/Bullet_Execution", Vector2.zero);
				break;
			case GlobalBulletType.STIM:
				SoundEffectPlayer.PlayOnce("Bullet/Bullet_Heal", Vector2.zero);
				break;
			case GlobalBulletType.TRANQ:
				SoundEffectPlayer.PlayOnce("Bullet/Bullet_Slow", Vector2.zero);
				break;
			}
			int num = 0;
			if (SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.NETZACH, true) && (type == GlobalBulletType.RECOVER_HP || type == GlobalBulletType.RECOVER_MENTAL))
			{
				num = 1;
				UnitModel weakest = null;
				float weakestPercent = 999f;
				foreach (UnitModel target in targets.ToArray())
				{
					if (target is AgentModel)
					{
						float percent = type == GlobalBulletType.RECOVER_HP ? target.hp / (float)target.maxHp : target.mental / (float)target.maxMental;
						if (percent < weakestPercent)
						{
							weakest = target;
							weakestPercent = percent;
						}
						targets.Remove(target);
					}
				}
				if (weakest != null)
				{
					targets.Add(weakest);
				}
			}
			else
			{
				foreach (UnitModel target in targets)
				{
					if (target is AgentModel)
					{
						num++;
					}
				}
			}
			targetedBulletMult = 1f;
			if (num > 0)
			{
				if (MissionManager.instance.ExistsFinishedOvertimeBossMission(SefiraEnum.TIPERERTH1))
				{
					targetedBulletMult = 1f + 1f / (float)num;
				}
				else if (ResearchDataModel.instance.IsUpgradedAbility("targeted_bullets"))
				{
					targetedBulletMult = 1f + 0.5f / (float)num;
				}
			}
			foreach (UnitModel target in targets)
			{
				this._funcs[type](target);
			}
			this.currentBullet--;
			Notice.instance.Send(NoticeName.OnUseBullet, new object[]
			{
				type
			});
			this.UpdateUI();
			return true;
		}

		// Token: 0x06003762 RID: 14178 RVA: 0x00165F24 File Offset: 0x00164124
		private void RecoverHPBullet(UnitModel target)
		{ // <Mod> Made Chesed's upgrade bullet research add +10 instead of +15
			WorkerModel workerModel = target as WorkerModel;
			if (workerModel != null && !workerModel.HasUnitBuf(UnitBufType.QUEENBEE_SPORE))
			{
				OvertimeNetzachBossBuf.IsBullet = true;
				float num = 25f;
				if (ResearchDataModel.instance.IsUpgradedAbility("upgrade_recover_bullet"))
				{
					num += 10f;
				}
				num *= targetedBulletMult;
                float prevHP = workerModel.hp;
				float HPrecovered = workerModel.RecoverHPv2(num);
				prevHP = workerModel.maxHp - prevHP;
				if (HPrecovered > prevHP)
				{
					HPrecovered = prevHP;
				}
				OvertimeNetzachBossBuf.IsBullet = false;
				Notice.instance.Send(NoticeName.RecoverByBullet, new object[]
				{
					workerModel,
					HPrecovered,
					1
				});
			}
		}

		// Token: 0x06003763 RID: 14179 RVA: 0x00165F70 File Offset: 0x00164170
		private void RecoverMentalBullet(UnitModel target)
		{ // <Mod> Made Chesed's upgrade bullet research add +10 instead of +15
			WorkerModel workerModel = target as WorkerModel;
			if (workerModel != null && !workerModel.IsPanic())
			{
				OvertimeNetzachBossBuf.IsBullet = true;
				float num = 25f;
				if (ResearchDataModel.instance.IsUpgradedAbility("upgrade_recover_bullet"))
				{
					num += 10f;
				}
				num *= targetedBulletMult;
                float prevSP = workerModel.mental;
				float SPrecovered = workerModel.RecoverMentalv2(num);
				prevSP = workerModel.maxMental - prevSP;
				if (SPrecovered > prevSP)
				{
					SPrecovered = prevSP;
				}
				OvertimeNetzachBossBuf.IsBullet = false;
				Notice.instance.Send(NoticeName.RecoverByBullet, new object[]
				{
					workerModel,
					SPrecovered,
					2
				});
			}
		}

		// Token: 0x06003764 RID: 14180 RVA: 0x00165FBC File Offset: 0x001641BC
		private void ResistRBullet(UnitModel target)
		{ // <Mod>
			WorkerModel workerModel = target as WorkerModel;
			if (workerModel != null)
			{
				float num = 50f;
				if (ResearchDataModel.instance.IsUpgradedAbility("upgrade_shield_bullet"))
				{
					num += 10f;
				}
				workerModel.AddUnitBuf(new BarrierBuf(RwbpType.R, num * targetedBulletMult, 15f * targetedBulletMult));
			}
		}

		// Token: 0x06003765 RID: 14181 RVA: 0x00165FF0 File Offset: 0x001641F0
		private void ResistWBullet(UnitModel target)
		{ // <Mod>
			WorkerModel workerModel = target as WorkerModel;
			if (workerModel != null)
			{
				float num = 50f;
				if (ResearchDataModel.instance.IsUpgradedAbility("upgrade_shield_bullet"))
				{
					num += 10f;
				}
				workerModel.AddUnitBuf(new BarrierBuf(RwbpType.W, num * targetedBulletMult, 15f * targetedBulletMult));
			}
		}

		// Token: 0x06003766 RID: 14182 RVA: 0x00166024 File Offset: 0x00164224
		private void ResistBBullet(UnitModel target)
		{ // <Mod>
			WorkerModel workerModel = target as WorkerModel;
			if (workerModel != null)
			{
				float num = 50f;
				if (ResearchDataModel.instance.IsUpgradedAbility("upgrade_shield_bullet"))
				{
					num += 10f;
				}
				workerModel.AddUnitBuf(new BarrierBuf(RwbpType.B, num * targetedBulletMult, 15f * targetedBulletMult));
			}
		}

		// Token: 0x06003767 RID: 14183 RVA: 0x00166058 File Offset: 0x00164258
		private void ResistPBullet(UnitModel target)
		{ // <Mod>
			WorkerModel workerModel = target as WorkerModel;
			if (workerModel != null)
			{
				float num = 50f;
				if (ResearchDataModel.instance.IsUpgradedAbility("upgrade_shield_bullet"))
				{
					num += 10f;
				}
				workerModel.AddUnitBuf(new BarrierBuf(RwbpType.P, num * targetedBulletMult, 15f * targetedBulletMult));
			}
		}

		// Token: 0x06003768 RID: 14184 RVA: 0x00031882 File Offset: 0x0002FA82
		private void SlowBullet(UnitModel target)
		{
			target.AddUnitBuf(new SlowBulletBuf(10f));
		}

		// Token: 0x06003769 RID: 14185 RVA: 0x0016608C File Offset: 0x0016428C
		private void ExcuteBullet(UnitModel target)
		{
			if (!(target is WorkerModel))
			{
				return;
			}
			WorkerModel workerModel = target as WorkerModel;
			try
			{
				if (workerModel != null && !workerModel.HasUnitBuf(UnitBufType.DEATH_ANGEL_BETRAYER))
				{
					if (workerModel.HasUnitBuf(UnitBufType.QUEENBEE_SPORE))
					{
						workerModel.RemoveUnitBuf(workerModel.GetUnitBufByType(UnitBufType.QUEENBEE_SPORE));
					}
					workerModel.SetDeadType(DeadType.EXECUTION);
					workerModel.TakeDamageWithoutEffect(null, new DamageInfo(RwbpType.R, 10000f));
					workerModel.invincible = false;
					workerModel.Die();
				}
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
		}

		private void StimBullet(UnitModel target)
		{
			//
		}

		private void TranqBullet(UnitModel target)
		{
			if (!(target is CreatureModel))
			{
				return;
			}
			//
		}

		// Token: 0x040032D1 RID: 13009
		private static GlobalBulletManager _instance;

		// Token: 0x040032D2 RID: 13010
		private Dictionary<GlobalBulletType, GlobalBulletManager.BulletFunc> _funcs;

		// Token: 0x040032D3 RID: 13011
		public float elapsedCoolTime;

		// Token: 0x040032D4 RID: 13012
		public float coolTime = 10f;

		// Token: 0x040032D5 RID: 13013
		public int initialMaxBullet = 5;

		// Token: 0x040032D6 RID: 13014
		public int maxBullet = 5;

		// Token: 0x040032D7 RID: 13015
		public int currentBullet;

		// Token: 0x020006AE RID: 1710
		// (Invoke) Token: 0x0600376B RID: 14187
		private delegate void BulletFunc(UnitModel target);

		// <Mod>
		private float targetedBulletMult = 1f;

		// <Mod>
		public int refillsAvailable;
	}
}
