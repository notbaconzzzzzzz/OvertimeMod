using System;
using System.Collections.Generic;
using UnityEngine;
using WorkerSpine;
using WorkerSprite;

// Token: 0x0200048E RID: 1166
public class SnowQueen : CreatureBase
{
	// Token: 0x06002A7F RID: 10879 RVA: 0x00122B94 File Offset: 0x00120D94
	public SnowQueen()
	{
	}

	// Token: 0x170003F2 RID: 1010
	// (get) Token: 0x06002A80 RID: 10880 RVA: 0x00029656 File Offset: 0x00027856
	private SnowQueenAnim _animScript
	{
		get
		{
			return base.Unit.animTarget as SnowQueenAnim;
		}
	}

	// Token: 0x06002A81 RID: 10881 RVA: 0x00122BEC File Offset: 0x00120DEC
	public override void OnInit()
	{
		base.OnInit();
		this.icePiece = EGOgiftModel.MakeGift(EquipmentTypeList.instance.GetData(1021));
		this.dualWinGift = EGOgiftModel.MakeGift(EquipmentTypeList.instance.GetData(1023));
		this._agStackList = new List<AgentModel>();
		this._sounds = new Dictionary<string, SnowQueen.SnowQueenSound>();
		this._sounds.Add("Default", new SnowQueen.SnowQueenSound(this.SOUND_SRC + "Default", null));
		this._sounds.Add("Fight", new SnowQueen.SnowQueenSound(this.SOUND_SRC + "Fight", null));
		this._sounds.Add("Filter_Off", new SnowQueen.SnowQueenSound(this.SOUND_SRC + "Filter_Off", null));
		this._sounds.Add("Filter_On", new SnowQueen.SnowQueenSound(this.SOUND_SRC + "Filter_On", null));
		this._sounds.Add("Kiss1", new SnowQueen.SnowQueenSound(this.SOUND_SRC + "Kiss1", null));
		this._sounds.Add("Kiss2", new SnowQueen.SnowQueenSound(this.SOUND_SRC + "Kiss2", null));
		this._sounds.Add("Kiss3", new SnowQueen.SnowQueenSound(this.SOUND_SRC + "Kiss3", null));
		this._sounds.Add("Win", new SnowQueen.SnowQueenSound(this.SOUND_SRC + "Win", null));
		this._sounds.Add("Win_Sword", new SnowQueen.SnowQueenSound(this.SOUND_SRC + "Win_Sword", null));
		this._sounds.Add("Lose", new SnowQueen.SnowQueenSound(this.SOUND_SRC + "Lose", null));
		this._sounds.Add("Lose_Sword", new SnowQueen.SnowQueenSound(this.SOUND_SRC + "Lose_Sword", null));
		this._sounds.Add("EnergyFreeze", new SnowQueen.SnowQueenSound(this.SOUND_SRC + "EnergyFreeze", null));
		this._sounds.Add("EnergyMelt", new SnowQueen.SnowQueenSound(this.SOUND_SRC + "EnergyMelt", null));
	}

	// Token: 0x06002A82 RID: 10882 RVA: 0x00029668 File Offset: 0x00027868
	public override void OnViewInit(CreatureUnit unit)
	{
		this.Initialize();
		base.OnViewInit(unit);
	}

	// Token: 0x06002A83 RID: 10883 RVA: 0x00122E38 File Offset: 0x00121038
	public override void RoomSpriteInit()
	{
		base.RoomSpriteInit();
		this._kissSprite = Resources.Load<Sprite>("Sprites/CreatureSprite/Isolate/Skill/" + this.model.metadataId + "_1");
		this._kissFilter = base.Unit.room.AddFilter(base.Unit.room.StateFilter);
		this._kissFilter.renderSprite = this._kissSprite;
		this._kissFilter.hasSpecialAnimKey = true;
		this._kissFilter.specialAnimKey = "PlayFade";
		this._kissFilter.renderAnim.enabled = true;
		this._kissFilter.GetComponent<SpriteRenderer>().sortingOrder = 9;
		this._winDualFilter = new IsolateFilter[2];
		this._winDualSprite = Resources.LoadAll<Sprite>("Sprites/CreatureSprite/Isolate/Skill/" + this.model.metadataId + "_3_3");
		this._winDualFilter[0] = base.Unit.room.AddFilter(base.Unit.room.StateFilter);
		this._winDualFilter[0].renderSprite = this._winDualSprite[0];
		this._winDualFilter[0].hasSpecialAnimKey = true;
		this._winDualFilter[0].specialAnimKey = "Play";
		this._winDualFilter[0].renderAnim.enabled = true;
		this._winDualFilter[0].GetComponent<SpriteRenderer>().sortingOrder = 9;
		this._winDualFilter[1] = base.Unit.room.AddFilter(base.Unit.room.StateFilter);
		this._winDualFilter[1].renderSprite = this._winDualSprite[1];
		this._winDualFilter[1].hasSpecialAnimKey = true;
		this._winDualFilter[1].specialAnimKey = "PlayFade";
		this._winDualFilter[1].renderAnim.enabled = true;
		this._winDualFilter[1].GetComponent<SpriteRenderer>().sortingOrder = 9;
	}

	// Token: 0x06002A84 RID: 10884 RVA: 0x0012302C File Offset: 0x0012122C
	public override void OnStageStart()
	{
		this._sounds["Default"].sound = base.MakeSoundLoop(this._sounds["Default"].src);
		this._sounds["Default"].sound.src.minDistance = 3f;
		this._sounds["Default"].sound.src.maxDistance = 100f;
	}

	// Token: 0x06002A85 RID: 10885 RVA: 0x001230B4 File Offset: 0x001212B4
	public override void OnStageRelease()
	{
		base.OnStageRelease();
		foreach (AgentModel agentModel in this._agStackList)
		{
			if (agentModel.HasEquipment(1021))
			{
				agentModel.ReleaseEGOgift(this.icePiece);
			}
		}
		this._agStackList.Clear();
	}

	// Token: 0x06002A86 RID: 10886 RVA: 0x00123138 File Offset: 0x00121338
	public override void OnStageEnd()
	{
		base.OnStageEnd();
		this._rescueAgent = null;
		if (this._frozenAgent != null)
		{
			this._frozenAgent.SetInvincible(false);
			this._frozenAgent.Die();
			this._frozenAgent = null;
		}
		foreach (AgentModel agentModel in this._agStackList)
		{
			if (agentModel.HasEquipment(1021))
			{
				agentModel.ReleaseEGOgift(this.icePiece);
			}
		}
	}

	// Token: 0x06002A87 RID: 10887 RVA: 0x001231E0 File Offset: 0x001213E0
	public override void OnFixedUpdate(CreatureModel creature)
	{
		base.OnFixedUpdate(creature);
		if (this._filterTimer.RunTimer())
		{
			this._kissFilter.Activated = false;
			this._winDualFilter[0].Activated = false;
			this._winDualFilter[1].Activated = false;
		}
		if (this._swordSoundTimer.RunTimer())
		{
			this._animScript.SetDualFilerAnim("DualEnd");
			this._sounds["Filter_Off"].sound = base.MakeSound(this._sounds["Filter_Off"].src, Time.timeScale);
			this._sounds["Filter_Off"].sound.src.minDistance = 3f;
			this._sounds["Filter_Off"].sound.src.maxDistance = 100f;
		}
		if (this._rescueAgent != null && this._rescueAgent.target == null)
		{
			SpriteRenderer component = this._rescueAgent.GetUnit().animRoot.GetChild(0).GetChild(0).Find("RightHandFollower").GetChild(0).Find("RoseSword").GetComponent<SpriteRenderer>();
			component.enabled = false;
		}
	}

	// Token: 0x06002A88 RID: 10888 RVA: 0x00123328 File Offset: 0x00121528
	public override void OnAgentAllocateWork(AgentModel actor)
	{
		base.OnAgentAllocateWork(actor);
		if (this._isBlockWork)
		{
			this._rescueAgent = actor;
			SpriteRenderer component = this._rescueAgent.GetUnit().animRoot.GetChild(0).GetChild(0).Find("RightHandFollower").GetChild(0).Find("RoseSword").GetComponent<SpriteRenderer>();
			component.enabled = true;
		}
	}

	// Token: 0x06002A89 RID: 10889 RVA: 0x00123394 File Offset: 0x00121594
	public override void OnEnterRoom(UseSkill skill)
	{
		base.OnEnterRoom(skill);
		AgentModel agent = skill.agent;
		if (agent.IsDead())
		{
			return;
		}/*
		if (agent.HasEquipment(this._FIRE_BIRD_ARMOR) && !agent.invincible)
		{
			this._rescueAgent = null;
			this._killedByArmor = true;
			SpriteRenderer component = agent.GetUnit().animRoot.GetChild(0).GetChild(0).Find("RightHandFollower").GetChild(0).Find("RoseSword").GetComponent<SpriteRenderer>();
			component.enabled = false;
			agent.GetWorkerUnit().animChanger.ChangeAnimatorWithUniqueFace(WorkerSpine.AnimatorName.FireBirdAgentDead, false);
			agent.Die();
			return;
		}*/
		if (this._isBlockWork)
		{
			this.model.ShowNarrationText("special_ability_1", true, new string[0]);
			this._animScript.SetDualFilerAnim("DualStart");
			this._sounds["Filter_On"].sound = base.MakeSound(this._sounds["Filter_On"].src, Time.timeScale);
			this._sounds["Filter_On"].sound.src.minDistance = 3f;
			this._sounds["Filter_On"].sound.src.maxDistance = 100f;
			this._rescueAgent = skill.agent;
			this._rescueAgent.specialDeadScene = true;
			this._rescueAgent.WorkEndReaction();
			this._rescueAgent.animationMessageRecevied = this;
			skill.animRemoveOnDestroy = false;
		}
	}

	// Token: 0x06002A8A RID: 10890 RVA: 0x00123528 File Offset: 0x00121728
	public override void OnReleaseWork(UseSkill skill)
	{ // <Mod> reset _isBlockWork in many edge cases
		if (this._isBlockWork)
		{
			SpriteRenderer component = skill.agent.GetUnit().animRoot.GetChild(0).GetChild(0).Find("RightHandFollower").GetChild(0).Find("RoseSword").GetComponent<SpriteRenderer>();
			component.enabled = false;
		}
		if (this._killedByArmor)
		{
            if (this._isBlockWork)
			{
				this._isBlockWork = false;
				this._soundTimer = new AutoTimer();
				this._soundTimer.Init();
				this._soundTimer.StartTimer(0.2f, new AutoTimer.TargetMethod(this.FadeOutSound), false, AutoTimer.UpdateMode.FIXEDUPDATE);
			}
			return;
		}
		if (skill.skillTypeInfo.id == 3L && (skill.agent.HasEquipment(4000371) || skill.agent.HasEquipment(4000372) || skill.agent.HasEquipment(4000373) || skill.agent.HasEquipment(4000374)))
		{
			foreach (CreatureModel creatureModel in CreatureManager.instance.GetCreatureList())
			{
				if (creatureModel.script is ArmorCreature)
				{
					this._rescueAgent = null;
                    if (this._isBlockWork)
					{
						this._isBlockWork = false;
						this._soundTimer = new AutoTimer();
						this._soundTimer.Init();
						this._soundTimer.StartTimer(0.2f, new AutoTimer.TargetMethod(this.FadeOutSound), false, AutoTimer.UpdateMode.FIXEDUPDATE);
					}
					return;
				}
			}
		}
		if (this._isBlockWork && this._rescueAgent != null)
		{
			SpriteRenderer component2 = this._rescueAgent.GetUnit().animRoot.GetChild(0).GetChild(0).Find("RightHandFollower").GetChild(0).Find("RoseSword").GetComponent<SpriteRenderer>();
			component2.enabled = false;
			int fortitudeLevel = skill.agent.fortitudeLevel;
			int probability = 0;
			switch (fortitudeLevel)
			{
			case 1:
				probability = 10;
				break;
			case 2:
				probability = 30;
				break;
			case 3:
				probability = 50;
				break;
			case 4:
				probability = 80;
				break;
			case 5:
				probability = 100;
				break;
			}
			bool state = base.Prob(probability);
			if (skill.agent.mental <= 0f || skill.agent.hp <= 0f)
			{
				state = false;
			}
			this.DecideDualResult(state);
			this._soundTimer = new AutoTimer();
			this._soundTimer.Init();
			this._soundTimer.StartTimer(0.2f, new AutoTimer.TargetMethod(this.FadeOutSound), false, AutoTimer.UpdateMode.FIXEDUPDATE);
			this._rescueAgent.specialDeadScene = false;
			return;
		}
        else
        {
            if (this._isBlockWork)
			{
				this._isBlockWork = false;
				this._soundTimer = new AutoTimer();
				this._soundTimer.Init();
				this._soundTimer.StartTimer(0.2f, new AutoTimer.TargetMethod(this.FadeOutSound), false, AutoTimer.UpdateMode.FIXEDUPDATE);
			}
        }
		CreatureFeelingState feelingState = this.model.feelingState;
		bool flag = false;
		if (feelingState != CreatureFeelingState.BAD)
		{
			if (feelingState != CreatureFeelingState.NORM)
			{
				if (feelingState != CreatureFeelingState.GOOD)
				{
				}
			}
			else
			{
				flag = base.Prob(50);
			}
		}
		else
		{
			flag = base.Prob(100);
		}
		if (!flag || skill.agent.mental <= 0f || skill.agent.hp <= 0f)
		{
			return;
		}
		if (this._isBlockWork)
		{
			return;
		}
		if (this._agStackList.Find((AgentModel am) => skill.agent == am) != null)
		{
			this._kissFilter.Activated = true;
			this._animScript.SetAnim("Kiss2");
			this._sounds["Kiss2"].sound = base.MakeSound(this._sounds["Kiss2"].src, Time.timeScale);
			this._sounds["Kiss2"].sound.src.minDistance = 3f;
			this._sounds["Kiss2"].sound.src.maxDistance = 100f;
			skill.agent.LoseControl();
			this._frozenAgent = skill.agent;
			this._frozenAgent.SetInvincible(true);
			this._isBlockWork = true;
			if (this._frozenAgent.HasUnitBuf(UnitBufType.OTHER_WORLD_PORTRAIT_VICTIM))
			{
				CreatureModel creatureModel2 = CreatureManager.instance.FindCreature(300104L);
				if (creatureModel2 != null)
				{
					(creatureModel2.script as OtherWorldPortrait).ReleaseVictim(this._frozenAgent);
				}
			}
			this._filterTimer.StartTimer(this._filterTime);
		}
		else
		{
			this._kissFilter.Activated = true;
			this._animScript.SetAnim("Kiss1");
			this._sounds["Kiss1"].sound = base.MakeSound(this._sounds["Kiss1"].src, Time.timeScale);
			this._sounds["Kiss1"].sound.src.minDistance = 3f;
			this._sounds["Kiss1"].sound.src.maxDistance = 100f;
			this._bufAgent = skill.agent;
			this._bufAgent.LoseControl();
			this._filterTimer.StartTimer(this._filterTime);
		}
	}

	// Token: 0x06002A8B RID: 10891 RVA: 0x00003FDD File Offset: 0x000021DD
	public override void OnFinishWork(UseSkill skill)
	{
	}

	// Token: 0x06002A8C RID: 10892 RVA: 0x00029677 File Offset: 0x00027877
	public override void OnWorkCoolTimeEnd(CreatureFeelingState oldState)
	{
		base.OnWorkCoolTimeEnd(oldState);
		this._killedByArmor = false;
	}

	// Token: 0x06002A8D RID: 10893 RVA: 0x00123A4C File Offset: 0x00121C4C
	public void BufAgent()
	{
		EGOgiftModel gift = this.icePiece;
		this._bufAgent.AttachEGOgift(gift);
		this._agStackList.Add(this._bufAgent);
		this._bufAgent.GetControl();
		this._bufAgent = null;
	}

	// Token: 0x06002A8E RID: 10894 RVA: 0x00123A90 File Offset: 0x00121C90
	public void FreezingAgent()
	{
		this._frozenAgent.GetUnit().gameObject.SetActive(false);
		Sprite frontHair = this._frozenAgent.spriteData.FrontHair;
		Sprite rearHair = this._frozenAgent.spriteData.RearHair;
		this._animScript.ActiveIce(3, true, frontHair, rearHair);
	}

	// Token: 0x06002A8F RID: 10895 RVA: 0x00123AE4 File Offset: 0x00121CE4
	public void KillTwoAgents()
	{
		if (this._frozenAgent.invincible)
		{
			this._frozenAgent.SetInvincible(false);
		}
		this._frozenAgent.Die();
		this._rescueAgent.Die();
		this._frozenAgent.GetUnit().gameObject.SetActive(false);
		this._rescueAgent.GetUnit().gameObject.SetActive(false);
		this._frozenAgent.GetUnit().animChanger.ChangeAnimator();
		this._rescueAgent.GetUnit().animChanger.ChangeAnimator();
		this._animScript.ActiveIce(3, false, null, null);
		this._animScript.ActiveIce(UnityEngine.Random.Range(0, 3), true, null, null);
		this._frozenAgent = null;
		this._rescueAgent = null;
	}

	// Token: 0x06002A90 RID: 10896 RVA: 0x00123BAC File Offset: 0x00121DAC
	public void SetDualSound(bool state)
	{
		if (state)
		{
			this._sounds["Fight"].sound = base.MakeSoundLoop(this._sounds["Fight"].src);
			this._sounds["Fight"].sound.src.minDistance = 3f;
			this._sounds["Fight"].sound.src.maxDistance = 100f;
		}
	}

	// Token: 0x06002A91 RID: 10897 RVA: 0x00123C38 File Offset: 0x00121E38
	public void AgentAnim(int i)
	{ // <Mod> EGO gift reward has been moved to a different function
		AgentUnit unit = this._rescueAgent.GetUnit();
		AgentUnit unit2 = this._frozenAgent.GetUnit();
		if (i != 0)
		{
			if (i != 1)
			{
			}
		}
		else
		{
			this._frozenAgent.GetControl();
			this._frozenAgent.SetInvincible(false);
			this._rescueAgent.GetControl();
			unit.animChanger.ChangeAnimator();
			unit2.animChanger.ChangeAnimator();
			/*Vector3 localPosition = new Vector3(0f, 0f, 0f);
			this._rescueAgent.AttachEGOgift(this.dualWinGift);
			EGOGiftRenderData egogiftRenderData;
			this._rescueAgent.GetUnit().spriteSetter.TryGetGift(this.dualWinGift, out egogiftRenderData);
			egogiftRenderData.renderer.transform.localPosition = localPosition;
			this._frozenAgent.AttachEGOgift(this.dualWinGift);
			EGOGiftRenderData egogiftRenderData2;
			this._frozenAgent.GetUnit().spriteSetter.TryGetGift(this.dualWinGift, out egogiftRenderData2);
			egogiftRenderData2.renderer.transform.localPosition = localPosition;*/
			this._frozenAgent = null;
			this._rescueAgent = null;
		}
	}

	// Token: 0x06002A92 RID: 10898 RVA: 0x00123D58 File Offset: 0x00121F58
	public void DualResult()
	{
		this._rescueAgent.GetUnit().animChanger.ChangeAnimator();
		AgentUnit unit = this._rescueAgent.GetUnit();
		unit.animChanger.ChangeAnimatorWithUniqueFace(WorkerSpine.AnimatorName.SnowQueenAgentCTRL, false);
		unit.animEventHandler.SetAnimEvent(new AnimatorEventHandler.AnimEventDelegate(this.AgentAnim));
		if (this._isDualWin)
		{
			this._frozenAgent.GetUnit().gameObject.SetActive(true);
			this._frozenAgent.SetCurrentNode(this.model.GetCustomNode());
			this._animScript.ActiveIce(3, false, null, null);
			AgentUnit unit2 = this._frozenAgent.GetUnit();
			unit2.animChanger.ChangeAnimatorWithUniqueFace(WorkerSpine.AnimatorName.SnowQueenAgentCTRL, false);
			unit2.model.workerAnimator.SetBool("Win2", true);
			this._winDualFilter[0].Activated = true;
			this._winDualFilter[1].Activated = true;
			this._filterTimer.StartTimer(this._filterTime);
			unit.model.workerAnimator.SetBool("Win", true);
			this._sounds["Win"].sound = base.MakeSound(this._sounds["Win"].src, Time.timeScale);
			this._sounds["Win"].sound.src.minDistance = 3f;
			this._sounds["Win"].sound.src.maxDistance = 100f;
			this.model.ShowNarrationText("win", true, new string[0]);
		}
		else
		{
			this._animScript.SetAnim("Kiss3");
			this._sounds["Kiss3"].sound = base.MakeSound(this._sounds["Kiss3"].src, Time.timeScale);
			unit.model.workerAnimator.SetBool("Lose", true);
			this._sounds["Lose"].sound = base.MakeSound(this._sounds["Lose"].src, Time.timeScale);
			this._sounds["Lose"].sound.src.minDistance = 3f;
			this._sounds["Lose"].sound.src.maxDistance = 100f;
			this.model.ShowNarrationText("lose", true, new string[0]);
		}
	}

	// Token: 0x06002A93 RID: 10899 RVA: 0x00123FF4 File Offset: 0x001221F4
	private void Initialize()
	{
		this._isBlockWork = false;
		this._killedByArmor = false;
		this._animScript.SetScript(this);
		foreach (SpriteRenderer spriteRenderer in this._animScript.ices)
		{
			spriteRenderer.gameObject.SetActive(false);
		}
		PlaySpeedSettingUI.instance.AddSpaceEvent(new PlaySpeedSettingUI.SpaceEvent(this.SetSoundState));
	}

	// Token: 0x06002A94 RID: 10900 RVA: 0x00124064 File Offset: 0x00122264
	private void SetSoundState(bool state)
	{
		List<string> list = new List<string>(this._sounds.Keys);
		foreach (string key in list)
		{
			if (this._sounds[key].sound != null)
			{
				if (state)
				{
					this._sounds[key].sound.src.UnPause();
				}
				else
				{
					this._sounds[key].sound.src.Pause();
				}
			}
		}
	}

	// Token: 0x06002A95 RID: 10901 RVA: 0x00124124 File Offset: 0x00122324
	private bool DecideDualResult(bool state)
	{ // <Mod> EGO gift reward has been move to this function
		this._isBlockWork = false;
		this._isDualWin = state;
        if (this._isDualWin)
		{
			Vector3 localPosition = new Vector3(0f, 0f, 0f);
			EGOgiftModel giftmodel = InventoryModel.Instance.CreateEquipmentForcely(1023) as EGOgiftModel;
			this._rescueAgent.AttachEGOgift(giftmodel);
			EGOGiftRenderData egogiftRenderData;
			this._rescueAgent.GetUnit().spriteSetter.TryGetGift(giftmodel, out egogiftRenderData);
			if (egogiftRenderData != null)
			{
				egogiftRenderData.renderer.transform.localPosition = localPosition;
			}
			EGOgiftModel giftmodel2 = InventoryModel.Instance.CreateEquipmentForcely(1023) as EGOgiftModel;
			this._frozenAgent.AttachEGOgift(giftmodel2);
			EGOGiftRenderData egogiftRenderData2;
			this._frozenAgent.GetUnit().spriteSetter.TryGetGift(this.dualWinGift, out egogiftRenderData2);
			if (egogiftRenderData2 != null)
			{
				egogiftRenderData2.renderer.transform.localPosition = localPosition;
			}
		}
		this._animScript.SetDualFilerAnim("DualEnd");
		this._sounds["Filter_Off"].sound = base.MakeSound(this._sounds["Filter_Off"].src, Time.timeScale);
		this._rescueAgent.LoseControl();
		foreach (AgentModel agentModel in this._agStackList)
		{
			if (agentModel.HasEquipment(1021))
			{
				agentModel.ReleaseEGOgift(this.icePiece);
			}
		}
		this._agStackList.Clear();
		return true;
	}

	// Token: 0x06002A96 RID: 10902 RVA: 0x00124200 File Offset: 0x00122400
	private void FadeOutSound()
	{
		if (this._sounds["Fight"].sound != null)
		{
			this._sounds["Fight"].sound.src.volume -= 0.2f;
			if (this._sounds["Fight"].sound.src.volume <= 0.1f)
			{
				this._sounds["Fight"].sound.Stop();
				AutoTimer.Destroy(this._soundTimer);
				this._swordSoundTimer.StartTimer(this._swordSoundTime / Time.timeScale);
				if (this._isDualWin)
				{
					this._sounds["Win_Sword"].sound = base.MakeSound(this._sounds["Win_Sword"].src, Time.timeScale);
					this._sounds["Win_Sword"].sound.src.minDistance = 3f;
					this._sounds["Win_Sword"].sound.src.maxDistance = 100f;
				}
				else
				{
					this._sounds["Lose_Sword"].sound = base.MakeSound(this._sounds["Lose_Sword"].src, Time.timeScale);
					this._sounds["Lose_Sword"].sound.src.minDistance = 3f;
					this._sounds["Lose_Sword"].sound.src.maxDistance = 100f;
				}
			}
			else
			{
				this._soundTimer.StartTimer(0.3f);
			}
		}
	}

	// Token: 0x04002893 RID: 10387
	public static bool IsFreezeEnergy;

	// Token: 0x04002894 RID: 10388
	private readonly int _FIRE_BIRD_ARMOR = 300061;

	// Token: 0x04002895 RID: 10389
	private readonly string SOUND_SRC = "creature/SnowQueen/SnowQueen_";

	// Token: 0x04002896 RID: 10390
	private readonly float _filterTime = 2f;

	// Token: 0x04002897 RID: 10391
	private readonly Timer _filterTimer = new Timer();

	// Token: 0x04002898 RID: 10392
	private readonly float _swordSoundTime = 1.7f;

	// Token: 0x04002899 RID: 10393
	private readonly Timer _swordSoundTimer = new Timer();

	// Token: 0x0400289A RID: 10394
	private bool _isBlockWork;

	// Token: 0x0400289B RID: 10395
	private bool _isDualWin;

	// Token: 0x0400289C RID: 10396
	private bool _killedByArmor;

	// Token: 0x0400289D RID: 10397
	private List<AgentModel> _agStackList;

	// Token: 0x0400289E RID: 10398
	private AgentModel _frozenAgent;

	// Token: 0x0400289F RID: 10399
	private AgentModel _rescueAgent;

	// Token: 0x040028A0 RID: 10400
	private AgentModel _bufAgent;

	// Token: 0x040028A1 RID: 10401
	private IsolateFilter _kissFilter;

	// Token: 0x040028A2 RID: 10402
	private Sprite _kissSprite;

	// Token: 0x040028A3 RID: 10403
	private IsolateFilter[] _winDualFilter;

	// Token: 0x040028A4 RID: 10404
	private Sprite[] _winDualSprite;

	// Token: 0x040028A5 RID: 10405
	private Dictionary<string, SnowQueen.SnowQueenSound> _sounds;

	// Token: 0x040028A6 RID: 10406
	private EGOgiftModel icePiece;

	// Token: 0x040028A7 RID: 10407
	private EGOgiftModel dualWinGift;

	// Token: 0x040028A8 RID: 10408
	private AutoTimer _soundTimer;

	// Token: 0x0200048F RID: 1167
	private class SnowQueenSound
	{
		// Token: 0x06002A97 RID: 10903 RVA: 0x00029687 File Offset: 0x00027887
		public SnowQueenSound(string p, SoundEffectPlayer s)
		{
			this.src = p;
			this.sound = s;
		}

		// Token: 0x040028A9 RID: 10409
		public string src;

		// Token: 0x040028AA RID: 10410
		public SoundEffectPlayer sound;
	}
}
