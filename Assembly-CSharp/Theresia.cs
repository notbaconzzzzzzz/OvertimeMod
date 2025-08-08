using System;

// Token: 0x0200049B RID: 1179
public class Theresia : CreatureBase
{
	// Token: 0x06002B10 RID: 11024 RVA: 0x0002A36C File Offset: 0x0002856C
	public Theresia()
	{
	}

	// Token: 0x06002B11 RID: 11025 RVA: 0x00128B74 File Offset: 0x00126D74
	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
		this._anim = (TheresiaAnim)unit.animTarget;
		this._anim.SetModel(this.model);
		if (this._soundPlayer != null)
		{
			this._soundPlayer.Stop();
		}
		this._soundPlayer = null;
	}

	// Token: 0x06002B12 RID: 11026 RVA: 0x00128BD0 File Offset: 0x00126DD0
	public override void OnEnterRoom(UseSkill skill)
	{
		base.OnEnterRoom(skill);
		this.mentalRecoverTimer.StartTimer(this.mentalRecoverTime);
		this._anim.Cast();
		if (this._soundPlayer != null)
		{
			this._soundPlayer.Stop();
		}
		this._soundPlayer = skill.targetCreatureView.PlaySoundLoop("listen");
	}

	// Token: 0x06002B13 RID: 11027 RVA: 0x00128C34 File Offset: 0x00126E34
	public override void OnFixedUpdateInSkill(UseSkill skill)
	{
		base.OnFixedUpdateInSkill(skill);
		if (this.mentalRecoverTimer.RunTimer())
		{
			this.mentalRecoverTimer.StartTimer(this.mentalRecoverTime);
			foreach (AgentModel agentModel in this.model.sefira.agentList)
			{
				if (!agentModel.IsDead())
				{
					agentModel.RecoverMental(this.mentalRecovery);
				}
			}
		}
		if (skill.elapsedTime > 30f)
		{
			skill.agent.mental = 0f;
		}
	}

	// Token: 0x06002B14 RID: 11028 RVA: 0x00128CF8 File Offset: 0x00126EF8
	public override void OnReleaseWork(UseSkill skill)
	{
		base.OnReleaseWork(skill);
		this.mentalRecoverTimer.StopTimer();
		this._anim.CancelCast();
		if (this._soundPlayer != null)
		{
			this._soundPlayer.Stop();
		}
		this._soundPlayer = null;
	}

	// Token: 0x0400290B RID: 10507
	private float mentalRecovery = 10f;

	// Token: 0x0400290C RID: 10508
	private float mentalRecoverTime = 5f;

	// Token: 0x0400290D RID: 10509
	private Timer mentalRecoverTimer = new Timer();

	// Token: 0x0400290E RID: 10510
	private TheresiaAnim _anim;

	// Token: 0x0400290F RID: 10511
	private SoundEffectPlayer _soundPlayer;
}
