/*
+public virtual void OnDestroy() // 
+public virtual void OnTakeDamage_After(UnitModel actor, DamageInfo dmg) // 
+public virtual bool IsTranqable() // 
+public virtual void OnTranquilized() // 
+public virtual void OnTranqEnd() // 
+public virtual int SuppressionEnergy // 
+public virtual void ForceSpawnWithoutRoom() // 
+public virtual void TryForceAggro(UnitModel unit) // 
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003DD RID: 989
public class CreatureBase
{
	// Token: 0x0600203F RID: 8255 RVA: 0x0002198D File Offset: 0x0001FB8D
	public CreatureBase()
	{
	}

	// Token: 0x170002BF RID: 703
	// (get) Token: 0x06002040 RID: 8256 RVA: 0x000219BD File Offset: 0x0001FBBD
	public AgentModel AllocatedAgent
	{
		get
		{
			return this._allocatedAgent;
		}
	}

	// Token: 0x170002C0 RID: 704
	// (get) Token: 0x06002041 RID: 8257 RVA: 0x000219C5 File Offset: 0x0001FBC5
	public SkillTriggerCheck skillTriggerCheck
	{
		get
		{
			return this._check;
		}
	}

	// Token: 0x170002C1 RID: 705
	// (get) Token: 0x06002042 RID: 8258 RVA: 0x000219CD File Offset: 0x0001FBCD
	public CreatureUnit Unit
	{
		get
		{
			return this.model.Unit;
		}
	}

	// Token: 0x170002C2 RID: 706
	// (get) Token: 0x06002043 RID: 8259 RVA: 0x000219DA File Offset: 0x0001FBDA
	public string GetSaveSrc
	{
		get
		{
			return string.Concat(new object[]
			{
				Application.persistentDataPath,
				"/creatureData/",
				this.model.metadataId,
				".dat"
			});
		}
	}

	// Token: 0x170002C3 RID: 707
	// (get) Token: 0x06002044 RID: 8260 RVA: 0x00021A12 File Offset: 0x0001FC12
	public virtual MovableObjectNode movable
	{
		get
		{
			return this.model.GetMovableNode();
		}
	}

	// Token: 0x170002C4 RID: 708
	// (get) Token: 0x06002045 RID: 8261 RVA: 0x00021A1F File Offset: 0x0001FC1F
	public virtual PassageObjectModel currentPassage
	{
		get
		{
			return this.movable.GetPassage();
		}
	}

	// Token: 0x06002046 RID: 8262 RVA: 0x000FACBC File Offset: 0x000F8EBC
	public virtual void SetModel(CreatureModel model)
	{
		this.model = model;
		CreatureSpecialSkillTipTable specialSkillTable = model.metaInfo.specialSkillTable;
		if (specialSkillTable != null)
		{
			foreach (CreatureSpecialSkillDesc desc in specialSkillTable.descList)
			{
				CreatureBase.SpecialSkillTipParameter item = new CreatureBase.SpecialSkillTipParameter(this, desc);
				this.specialSkillTipParamList.Add(item);
			}
		}
		this._check = model.metaInfo.skillTriggerCheck;
		this._check.SetScript(this);
	}

	// Token: 0x06002047 RID: 8263 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnInit()
	{
	}

	// Token: 0x06002048 RID: 8264 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnStageStart()
	{
	}

	// Token: 0x06002049 RID: 8265 RVA: 0x000FAD5C File Offset: 0x000F8F5C
	public void OnViewInitPrev(CreatureUnit unit)
	{
		if (this.isWorkAllocated)
		{
			this.isWorkAllocated = false;
		}
		if (unit.room != null)
		{
			this.RoomCounterInit();
		}
		foreach (CreatureBase.SpecialSkillTipParameter specialSkillTipParameter in this.specialSkillTipParamList)
		{
			if (specialSkillTipParameter.openLevel <= this.model.observeInfo.observeProgress && !specialSkillTipParameter.IsRevealed())
			{
				specialSkillTipParameter.OnObserveLevelUpdated(this.model.observeInfo.observeProgress);
			}
		}
	}

	// Token: 0x0600204A RID: 8266 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnViewInit(CreatureUnit unit)
	{
	}

	// Token: 0x0600204B RID: 8267 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnFixedUpdate(CreatureModel creature)
	{
	}

	// Token: 0x0600204C RID: 8268 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnFixedUpdateInSkill(UseSkill skill)
	{
	}

	// Token: 0x0600204D RID: 8269 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnSkillFailWorkTick(UseSkill skill)
	{
	}

	// Token: 0x0600204E RID: 8270 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnSkillSuccessWorkTick(UseSkill skill)
	{
	}

	// Token: 0x0600204F RID: 8271 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnSkillTickUpdate(UseSkill skill)
	{
	}

	// Token: 0x06002050 RID: 8272 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnSkillGoalComplete(UseSkill skill)
	{
	}

	// Token: 0x06002051 RID: 8273 RVA: 0x00013B09 File Offset: 0x00011D09
	public virtual bool PermitCancelCurrentWork()
	{
		return true;
	}

	// Token: 0x06002052 RID: 8274 RVA: 0x00021A2C File Offset: 0x0001FC2C
	public virtual float GetKitCreatureProcessTime()
	{
		return 1.5f;
	}

	// Token: 0x06002053 RID: 8275 RVA: 0x00003F3F File Offset: 0x0000213F
	public virtual int OnBonusWorkProb()
	{
		return 0;
	}

	// Token: 0x06002054 RID: 8276 RVA: 0x00021A33 File Offset: 0x0001FC33
	public virtual float GetDamageMultiplierInWork(UseSkill skill)
	{
		return 1f;
	}

	// Token: 0x06002055 RID: 8277 RVA: 0x00021A3A File Offset: 0x0001FC3A
	public virtual float TranformWorkProb(float originWorkProb)
	{
		return originWorkProb;
	}

	// Token: 0x06002056 RID: 8278 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnEnterRoom(UseSkill skill)
	{
	}

	// Token: 0x06002057 RID: 8279 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnReleaseWork(UseSkill skill)
	{
	}

	// Token: 0x06002058 RID: 8280 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnFinishWork(UseSkill skill)
	{
	}

	// Token: 0x06002059 RID: 8281 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnWorkCoolTimeEnd(CreatureFeelingState oldState)
	{
	}

	// Token: 0x0600205A RID: 8282 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnReturn()
	{
	}

	// Token: 0x0600205B RID: 8283 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void UniqueEscape()
	{
	}

	// Token: 0x0600205C RID: 8284 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void Escape()
	{
	}

	// Token: 0x0600205D RID: 8285 RVA: 0x00003E72 File Offset: 0x00002072
	public virtual SkillTypeInfo GetSpecialSkill()
	{
		return null;
	}

	// Token: 0x0600205E RID: 8286 RVA: 0x00021A3D File Offset: 0x0001FC3D
	public virtual string GetDebugText()
	{
		return string.Empty;
	}

	// Token: 0x0600205F RID: 8287 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnTimerEnd()
	{
	}

	// Token: 0x06002060 RID: 8288 RVA: 0x000FAE18 File Offset: 0x000F9018
	public virtual void MakeEffect(IsolateRoom room, int currentSkillResult)
	{
		GameObject gameObject;
		switch (currentSkillResult)
		{
		case 1:
			gameObject = Prefab.LoadPrefab("Effect/Isolate/VeryBad");
			goto IL_62;
		case 2:
			gameObject = Prefab.LoadPrefab("Effect/Isolate/Bad");
			goto IL_62;
		case 4:
			gameObject = Prefab.LoadPrefab("Effect/Isolate/Good");
			goto IL_62;
		case 5:
			gameObject = Prefab.LoadPrefab("Effect/Isolate/VeryGood");
			goto IL_62;
		}
		return;
		IL_62:
		gameObject.transform.SetParent(room.transform);
		gameObject.transform.position = CreatureLayer.currentLayer.GetCreature(this.model.instanceId).animTarget.head.transform.position;
		gameObject.transform.localScale = Vector3.one;
	}

	// Token: 0x06002061 RID: 8289 RVA: 0x000FAEDC File Offset: 0x000F90DC
	public void MakeEffectAlter(IsolateRoom room, int result)
	{
		GameObject gameObject;
		GameObject gameObject2;
		if (result != 0)
		{
			if (result != 2)
			{
				return;
			}
			gameObject = Prefab.LoadPrefab("Effect/Isolate/BadWork");
			gameObject2 = Prefab.LoadPrefab("Effect/Isolate/EnergyDown");
		}
		else
		{
			gameObject = Prefab.LoadPrefab("Effect/Isolate/GoodWork");
			gameObject2 = Prefab.LoadPrefab("Effect/Isolate/EnergyUp");
		}
		gameObject.transform.SetParent(room.transform);
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject2.transform.SetParent(room.transform);
		gameObject2.transform.localScale = Vector3.one;
		gameObject2.transform.localPosition = Vector3.zero;
		ParticleDestroy component = gameObject.GetComponent<ParticleDestroy>();
		ParticleDestroy component2 = gameObject2.GetComponent<ParticleDestroy>();
		component2.DelayedDestroy(10f);
		component.DelayedDestroy(5f);
	}

	// Token: 0x06002062 RID: 8290 RVA: 0x000FAFB8 File Offset: 0x000F91B8
	public virtual bool Prob(int probability)
	{
		int num = UnityEngine.Random.Range(0, 100);
		return num < probability;
	}

	// Token: 0x06002063 RID: 8291 RVA: 0x00013B09 File Offset: 0x00011D09
	public virtual bool isAttackInWorkProcess()
	{
		return true;
	}

	// Token: 0x06002064 RID: 8292 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnAttackInWorkProcess(UseSkill skill)
	{
	}

	// Token: 0x06002065 RID: 8293 RVA: 0x000FAFD8 File Offset: 0x000F91D8
	public virtual bool AttackProcess(UnitModel target)
	{
		if (this.model.IsAttackState())
		{
			this.movable.StopMoving();
			return true;
		}
		if (this.model.InWeaponRange(target))
		{
			this.movable.StopMoving();
			this.model.Attack(target);
			return true;
		}
		return false;
	}

	// Token: 0x06002066 RID: 8294 RVA: 0x00013B09 File Offset: 0x00011D09
	public virtual bool AutoFeelingDown()
	{
		return true;
	}

	// Token: 0x06002067 RID: 8295 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void AgentAnimCalled(int i, WorkerModel actor)
	{
	}

	// Token: 0x06002068 RID: 8296 RVA: 0x000FB030 File Offset: 0x000F9230
	public virtual void MakingEffect(string effect, float effectLength, string sound, Transform parent, int recoil)
	{
		Transform parent2 = parent;
		CreatureUnit creature = CreatureLayer.currentLayer.GetCreature(this.model.instanceId);
		if (parent == null)
		{
			parent = creature.gameObject.transform;
		}
		GameObject gameObject = Prefab.LoadPrefab(effect);
		gameObject.transform.SetParent(parent2);
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		ParticleDestroy component = gameObject.GetComponent<ParticleDestroy>();
		component.DelayedDestroy(effectLength);
		creature.PlaySound(sound);
		if (recoil > 0)
		{
			CameraMover.instance.Recoil(recoil);
		}
	}

	// Token: 0x06002069 RID: 8297 RVA: 0x000FB0E0 File Offset: 0x000F92E0
	public virtual void MakingEffect(string effect, float effectLength)
	{
		CreatureUnit creature = CreatureLayer.currentLayer.GetCreature(this.model.instanceId);
		GameObject gameObject = Prefab.LoadPrefab(effect);
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = creature.model.GetMovableNode().GetCurrentViewPosition();
		gameObject.transform.localRotation = Quaternion.identity;
		ParticleDestroy component = gameObject.GetComponent<ParticleDestroy>();
		component.DelayedDestroy(effectLength);
	}

	// Token: 0x0600206A RID: 8298 RVA: 0x000FB154 File Offset: 0x000F9354
	public virtual GameObject MakeEffectAttachedToHead(string effectSrc)
	{
		GameObject gameObject = Prefab.LoadPrefab(effectSrc);
		GameObject head = this.Unit.animTarget.head;
		gameObject.transform.SetParent(head.transform);
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		return gameObject;
	}

	// Token: 0x0600206B RID: 8299 RVA: 0x000FB1BC File Offset: 0x000F93BC
	public virtual void MakingEffect(string effect, float effectLength, string sound, Vector3 pos, int recoil)
	{
		CreatureUnit creature = CreatureLayer.currentLayer.GetCreature(this.model.instanceId);
		GameObject gameObject = Prefab.LoadPrefab(effect);
		gameObject.transform.position = pos;
		ParticleDestroy component = gameObject.GetComponent<ParticleDestroy>();
		component.DelayedDestroy(effectLength);
		creature.PlaySound(sound);
		if (recoil > 0)
		{
			CameraMover.instance.Recoil(recoil);
		}
	}

	// Token: 0x0600206C RID: 8300 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnAgentWorkEndAnimationPlayed(UseSkill skill)
	{
	}

	// Token: 0x0600206D RID: 8301 RVA: 0x00021A44 File Offset: 0x0001FC44
	public virtual void OnAgentAllocateWork(AgentModel actor)
	{
		actor.ShowCreatureActionSpeech(this.model.metadataId, "workAllocate");
	}

	// Token: 0x0600206E RID: 8302 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnSuppressed()
	{
	}

	// Token: 0x0600206F RID: 8303 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnStageRelease()
	{
	}

	// Token: 0x06002070 RID: 8304 RVA: 0x00021A5C File Offset: 0x0001FC5C
	public virtual void OnAllocatedWork(AgentModel agent)
	{
		this.isWorkAllocated = true;
		this._allocatedAgent = agent;
	}

	// Token: 0x06002071 RID: 8305 RVA: 0x00021A6C File Offset: 0x0001FC6C
	public virtual void OnReleaseWorkAllocated()
	{
		this.isWorkAllocated = false;
	}

	// Token: 0x06002072 RID: 8306 RVA: 0x000FB220 File Offset: 0x000F9420
	public void ObserveLevelChangeForSpecialSkillTip()
	{
		foreach (CreatureBase.SpecialSkillTipParameter specialSkillTipParameter in this.specialSkillTipParamList)
		{
			specialSkillTipParameter.OnObserveLevelUpdated(this.model.observeInfo.observeProgress);
		}
	}

	// Token: 0x06002073 RID: 8307 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnObserveLevelChanged()
	{
	}

	// Token: 0x06002074 RID: 8308 RVA: 0x00003F3F File Offset: 0x0000213F
	public virtual bool HasUniqueProcessWorkNarration()
	{
		return false;
	}

	// Token: 0x06002075 RID: 8309 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void UniqueProcessWorkNarration(UseSkill skill)
	{
	}

	// Token: 0x06002076 RID: 8310 RVA: 0x000FB28C File Offset: 0x000F948C
	public virtual void RoomSpriteInit()
	{
		if (this.Unit.room == null)
		{
			return;
		}
		Sprite sprite = Resources.Load<Sprite>("Sprites/CreatureSprite/Isolate/Escape/" + this.model.metadataId);
		Sprite sprite2 = Resources.Load<Sprite>("Sprites/CreatureSprite/Isolate/Skill/" + this.model.metadataId);
		Sprite sprite3 = Resources.Load<Sprite>(string.Concat(new object[]
		{
			"Sprites/CreatureSprite/Isolate/State/",
			this.model.metadataId,
			"_",
			GlobalGameManager.instance.GetCurrentLanguage()
		}));
		try
		{
			if (sprite != null)
			{
				this.Unit.room.EscapeFilter.renderSprite = sprite;
			}
			if (sprite2 != null)
			{
				this.Unit.room.SkillFilter.renderSprite = sprite2;
			}
			if (sprite3 != null)
			{
				this.Unit.room.StateFilter.renderSprite = sprite3;
			}
		}
		catch (Exception ex)
		{
		}
	}

	// Token: 0x06002077 RID: 8311 RVA: 0x00021A75 File Offset: 0x0001FC75
	public virtual void RoomEscapeSpriteOn()
	{
		this.Unit.room.EscapeFilter.Activated = true;
	}

	// Token: 0x06002078 RID: 8312 RVA: 0x00021A8D File Offset: 0x0001FC8D
	public virtual void RoomEscapeSpriteOff()
	{
		this.Unit.room.EscapeFilter.Activated = false;
	}

	// Token: 0x06002079 RID: 8313 RVA: 0x00021AA5 File Offset: 0x0001FCA5
	public virtual void RoomSkillSpriteOn()
	{
		this.Unit.room.SkillFilter.Activated = true;
	}

	// Token: 0x0600207A RID: 8314 RVA: 0x00021ABD File Offset: 0x0001FCBD
	public virtual void RoomSkillSpriteOff()
	{
		this.Unit.room.SkillFilter.Activated = false;
	}

	// Token: 0x0600207B RID: 8315 RVA: 0x00021AD5 File Offset: 0x0001FCD5
	public virtual void RoomStateSpriteOn()
	{
		this.Unit.room.StateFilter.Activated = true;
	}

	// Token: 0x0600207C RID: 8316 RVA: 0x00021AED File Offset: 0x0001FCED
	public virtual void RoomStateSpriteOff()
	{
		if (this.Unit.room.StateFilter != null)
		{
			this.Unit.room.StateFilter.Activated = false;
		}
	}

	// Token: 0x0600207D RID: 8317 RVA: 0x00021B20 File Offset: 0x0001FD20
	public virtual void OnForceSpecialSkillTipReveal(string key, params object[] param)
	{
		this.model.OnRevealSpecialSkillTip(key, param);
	}

	// Token: 0x0600207E RID: 8318 RVA: 0x00021B2F File Offset: 0x0001FD2F
	public virtual void OnWorkReleaseSpeicalSkillTipReveal(string key)
	{
		this.OnReleaseSpecialTip.Add(key);
	}

	// Token: 0x0600207F RID: 8319 RVA: 0x000FB3B4 File Offset: 0x000F95B4
	public virtual void OnWorkReleaseTipUpdate(params object[] param)
	{
		foreach (string key in this.OnReleaseSpecialTip)
		{
			this.model.OnRevealSpecialSkillTip(key, param);
		}
		this.OnReleaseSpecialTip.Clear();
	}

	// Token: 0x06002080 RID: 8320 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnAgentAnimatorReseted()
	{
	}

	// Token: 0x06002081 RID: 8321 RVA: 0x00021B3D File Offset: 0x0001FD3D
	public virtual float SpecialEnergyTick()
	{
		return 0f;
	}

	// Token: 0x06002082 RID: 8322 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnStageEnd()
	{
	}

	// Token: 0x06002083 RID: 8323 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnFeverTimeOver()
	{
	}

	// Token: 0x06002084 RID: 8324 RVA: 0x00013B09 File Offset: 0x00011D09
	public virtual bool OnOverlayIsolateWork()
	{
		return true;
	}

	// Token: 0x06002085 RID: 8325 RVA: 0x00013B09 File Offset: 0x00011D09
	public virtual bool OnOverlayIsolateObserve()
	{
		return true;
	}

	// Token: 0x06002086 RID: 8326 RVA: 0x00013B09 File Offset: 0x00011D09
	public virtual bool OnOpenObserveWindow()
	{
		return true;
	}

	// Token: 0x06002087 RID: 8327 RVA: 0x00013B09 File Offset: 0x00011D09
	public virtual bool OnOpenWorkWindow()
	{
		return true;
	}

	// Token: 0x06002088 RID: 8328 RVA: 0x00013B09 File Offset: 0x00011D09
	public virtual bool OnOpenCollectionWindow()
	{
		return true;
	}

	// Token: 0x06002089 RID: 8329 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void DelayAttackMotion(float value)
	{
	}

	// Token: 0x0600208A RID: 8330 RVA: 0x00021B44 File Offset: 0x0001FD44
	public virtual bool IsAttackTargetable()
	{
		return this.model.state == CreatureState.ESCAPE;
	}

	// Token: 0x0600208B RID: 8331 RVA: 0x00021B5A File Offset: 0x0001FD5A
	public virtual UnitModel[] GetRealTargets()
	{
		return new UnitModel[]
		{
			this.model
		};
	}

	// Token: 0x0600208C RID: 8332 RVA: 0x00003F3F File Offset: 0x0000213F
	public virtual bool HasScriptSaveData()
	{
		return false;
	}

	// Token: 0x0600208D RID: 8333 RVA: 0x00003E72 File Offset: 0x00002072
	public virtual Dictionary<string, object> GetSaveData()
	{
		return null;
	}

	// Token: 0x0600208E RID: 8334 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void LoadData(Dictionary<string, object> dic)
	{
	}

	// Token: 0x0600208F RID: 8335 RVA: 0x000FB424 File Offset: 0x000F9624
	public virtual void LoadScriptData()
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath + "/creatureData");
		if (!directoryInfo.Exists)
		{
			directoryInfo.Create();
		}
		if (!File.Exists(this.GetSaveSrc))
		{
			Debug.Log(this.GetSaveSrc + " doesn't exist");
			return;
		}
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileStream = File.Open(this.GetSaveSrc, FileMode.Open);
		Dictionary<string, object> dic = (Dictionary<string, object>)binaryFormatter.Deserialize(fileStream);
		fileStream.Close();
		this.LoadData(dic);
	}

	// Token: 0x06002090 RID: 8336 RVA: 0x000FB4AC File Offset: 0x000F96AC
	public virtual void SaveScriptData()
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath + "/creatureData");
		if (!directoryInfo.Exists)
		{
			directoryInfo.Create();
		}
		Debug.Log(this.model.metaInfo.name + "save dat");
		Dictionary<string, object> saveData = this.GetSaveData();
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileStream = File.Create(this.GetSaveSrc);
		binaryFormatter.Serialize(fileStream, saveData);
		fileStream.Close();
	}

	// Token: 0x06002091 RID: 8337 RVA: 0x00021B6B File Offset: 0x0001FD6B
	public bool ExistSaveData()
	{
		return File.Exists(this.GetSaveSrc);
	}

	// Token: 0x06002092 RID: 8338 RVA: 0x000FB528 File Offset: 0x000F9728
	public void RemoveSaveData()
	{
		Debug.Log(this.model.metaInfo.name + "remove dat");
		if (this.ExistSaveData())
		{
			Debug.Log("exist");
			File.Delete(this.GetSaveSrc);
		}
	}

	// Token: 0x06002093 RID: 8339 RVA: 0x00021B78 File Offset: 0x0001FD78
	public void ReplaceCommand(CreatureModel replaced)
	{
		CreatureManager.instance.ReplaceCommand(this.model, replaced);
		CreatureManager.instance.UnRegisterCreature(this.model);
	}

	// Token: 0x06002094 RID: 8340 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnGamemanagerInit()
	{
	}

	// Token: 0x06002095 RID: 8341 RVA: 0x000FB574 File Offset: 0x000F9774
	public virtual GameObject MakeEffectGlobalPos(string src, Vector3 pos)
	{
		GameObject gameObject = Prefab.LoadPrefab(src);
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.position = pos;
		return gameObject;
	}

	// Token: 0x06002096 RID: 8342 RVA: 0x000FB5B8 File Offset: 0x000F97B8
	public virtual GameObject MakeEffectGlobalPosNonTrans(string src, Vector3 pos)
	{
		GameObject gameObject = Prefab.LoadPrefab(src);
		gameObject.transform.position = pos;
		return gameObject;
	}

	// Token: 0x06002097 RID: 8343 RVA: 0x00013B09 File Offset: 0x00011D09
	public virtual bool CanObservedByAgent(AgentModel agent)
	{
		return true;
	}

	// Token: 0x06002098 RID: 8344 RVA: 0x00021B9B File Offset: 0x0001FD9B
	public virtual SoundEffectPlayer MakeSound(string src)
	{
		return SoundEffectPlayer.PlayOnce(src, this.model.GetCurrentViewPosition());
	}

	// Token: 0x06002099 RID: 8345 RVA: 0x00021BB3 File Offset: 0x0001FDB3
	public virtual SoundEffectPlayer MakeSound(string src, float pitch)
	{
		return SoundEffectPlayer.PlayOnce(src, pitch, this.model.GetCurrentViewPosition());
	}

	// Token: 0x0600209A RID: 8346 RVA: 0x000FB5DC File Offset: 0x000F97DC
	public virtual SoundEffectPlayer MakeSound(string src, AudioRolloffMode mode)
	{
		SoundEffectPlayer soundEffectPlayer = SoundEffectPlayer.PlayOnce(src, this.model.GetCurrentViewPosition());
		soundEffectPlayer.src.rolloffMode = mode;
		return soundEffectPlayer;
	}

	// Token: 0x0600209B RID: 8347 RVA: 0x00021BCC File Offset: 0x0001FDCC
	public virtual SoundEffectPlayer MakeSoundQueue(params string[] fileName)
	{
		return SoundEffectPlayer.PlaySequence(this.model.GetCurrentViewPosition(), fileName);
	}

	// Token: 0x0600209C RID: 8348 RVA: 0x00021BE4 File Offset: 0x0001FDE4
	public virtual SoundEffectPlayer MakeSoundLoop(string src)
	{
		return SoundEffectPlayer.Play(src, this.model.GetAnimScript().animator.transform);
	}

	// Token: 0x0600209D RID: 8349 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnViewDestroy()
	{
	}

	// Token: 0x0600209E RID: 8350 RVA: 0x00013B09 File Offset: 0x00011D09
	public virtual bool IsWorkable()
	{
		return true;
	}

	// Token: 0x0600209F RID: 8351 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void ParamInit()
	{
	}

	// Token: 0x060020A0 RID: 8352 RVA: 0x00003F3F File Offset: 0x0000213F
	public virtual bool HasUniqueMaxObservationFinish()
	{
		return false;
	}

	// Token: 0x060020A1 RID: 8353 RVA: 0x00003F3F File Offset: 0x0000213F
	public virtual bool UniqueMaxObservationFinish(CreatureMaxObserve.Desc desc)
	{
		return false;
	}

	// Token: 0x060020A2 RID: 8354 RVA: 0x00021C01 File Offset: 0x0001FE01
	public virtual void OnChildSuppressed(ChildCreatureModel child)
	{
		this.model.DeleteChildCreatureModel(child.instanceId);
	}

	// Token: 0x060020A3 RID: 8355 RVA: 0x00003E72 File Offset: 0x00002072
	public virtual ChildCreatureModel MakeChildCreature(UnitModel origin)
	{
		return null;
	}

	// Token: 0x060020A4 RID: 8356 RVA: 0x00013B09 File Offset: 0x00011D09
	public virtual bool OnKillWorker(WorkerModel target)
	{
		return true;
	}

	// Token: 0x060020A5 RID: 8357 RVA: 0x00003F3F File Offset: 0x0000213F
	public virtual bool GenPursueCommandAlter(WorkerModel target)
	{
		return false;
	}

	// Token: 0x060020A6 RID: 8358 RVA: 0x00003F3F File Offset: 0x0000213F
	public virtual bool OnAfterSuppressed()
	{
		return false;
	}

	// Token: 0x060020A7 RID: 8359 RVA: 0x00003F3F File Offset: 0x0000213F
	public virtual bool hasUniqueDeadScene()
	{
		return false;
	}

	// Token: 0x060020A8 RID: 8360 RVA: 0x000FB610 File Offset: 0x000F9810
	public virtual bool SetHpSlider(Slider slider)
	{
		if (this.model.state != CreatureState.ESCAPE)
		{
			return false;
		}
		if (!ResearchDataModel.instance.IsUpgradedAbility("show_agent_ui") && (GlobalGameManager.instance.gameMode != GameMode.TUTORIAL || GlobalGameManager.instance.tutorialStep <= 1))
		{
			return false;
		}
		slider.maxValue = (float)this.model.baseMaxHp;
		slider.value = this.model.hp;
		return true;
	}

	// Token: 0x060020A9 RID: 8361 RVA: 0x00003F3F File Offset: 0x0000213F
	public virtual bool SetCastingSlider(Slider castingSlider)
	{
		return false;
	}

	// Token: 0x060020AA RID: 8362 RVA: 0x00003F3F File Offset: 0x0000213F
	public virtual bool HasUniqueAttackDealy()
	{
		return false;
	}

	// Token: 0x060020AB RID: 8363 RVA: 0x00013B09 File Offset: 0x00011D09
	public virtual bool CanTakeDamage(UnitModel attacker, DamageInfo dmg)
	{
		return true;
	}

	// Token: 0x060020AC RID: 8364 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnTakePhysicalDamage(UnitModel attacker, float damage)
	{
	}

	// Token: 0x060020AD RID: 8365 RVA: 0x00003F3F File Offset: 0x0000213F
	public virtual bool HasUniqueFaction()
	{
		return false;
	}

	// Token: 0x060020AE RID: 8366 RVA: 0x00003F3F File Offset: 0x0000213F
	public virtual bool OnWorkerPanic(WorkerModel worker)
	{
		return false;
	}

	// Token: 0x060020AF RID: 8367 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnSelectMaxObservation(int index)
	{
	}

	// Token: 0x060020B0 RID: 8368 RVA: 0x00003F3F File Offset: 0x0000213F
	public virtual bool OnLoadCreatureName(ref string nameOut)
	{
		return false;
	}

	// Token: 0x060020B1 RID: 8369 RVA: 0x00013B09 File Offset: 0x00011D09
	public virtual bool IsSuppressable()
	{
		return true;
	}

	// Token: 0x060020B2 RID: 8370 RVA: 0x00013B09 File Offset: 0x00011D09
	public virtual bool IsSuppressableByRoom()
	{
		return true;
	}

	// Token: 0x060020B3 RID: 8371 RVA: 0x00013B09 File Offset: 0x00011D09
	public virtual bool IsAutoSuppressable()
	{
		return true;
	}

	// Token: 0x060020B4 RID: 8372 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnWorkWindowSkillClicked(long id)
	{
	}

	// Token: 0x060020B5 RID: 8373 RVA: 0x00013B09 File Offset: 0x00011D09
	public virtual bool UseDefaultDamageIgnoreMessage(DamageTextEffect damageScript)
	{
		return true;
	}

	// Token: 0x060020B6 RID: 8374 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnWorkAllocated(SkillTypeInfo skill, AgentModel agent)
	{
	}

	// Token: 0x060020B7 RID: 8375 RVA: 0x00021C14 File Offset: 0x0001FE14
	public virtual bool GetPhysicalDamage(out float dmg)
	{
		dmg = 0f;
		return false;
	}

	// Token: 0x060020B8 RID: 8376 RVA: 0x00021C14 File Offset: 0x0001FE14
	public virtual bool GetMentalDamage(out float dmg)
	{
		dmg = 0f;
		return false;
	}

	// Token: 0x060020B9 RID: 8377 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnSuperArmorBreak()
	{
	}

	// Token: 0x060020BA RID: 8378 RVA: 0x00003F3F File Offset: 0x0000213F
	public virtual bool UniqueStunEffect()
	{
		return false;
	}

	// Token: 0x060020BB RID: 8379 RVA: 0x00013B09 File Offset: 0x00011D09
	public virtual bool HasEscapeUI()
	{
		return true;
	}

	// Token: 0x060020BC RID: 8380 RVA: 0x00021C1E File Offset: 0x0001FE1E
	public virtual bool HasRoomCounter()
	{
		return this.Unit.model.metaInfo.qliphothMax > 0;
	}

	// Token: 0x060020BD RID: 8381 RVA: 0x00021C38 File Offset: 0x0001FE38
	public virtual void RoomCounterInit()
	{
		this.Unit.room.SetCounterEnable(this.HasRoomCounter());
	}

	// Token: 0x060020BE RID: 8382 RVA: 0x00021C50 File Offset: 0x0001FE50
	public virtual void ResetQliphothCounter()
	{
		this.model.SetQliphothCounter(this.GetQliphothCounterMax());
	}

	// Token: 0x060020BF RID: 8383 RVA: 0x00021C63 File Offset: 0x0001FE63
	public virtual int GetMaxWorkCountView()
	{
		return this.model.metaInfo.maxWorkCount;
	}

	// Token: 0x060020C0 RID: 8384 RVA: 0x00021C75 File Offset: 0x0001FE75
	public virtual int GetQliphothCounterMax()
	{
		return this.model.metaInfo.qliphothMax;
	}

	// Token: 0x060020C1 RID: 8385 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void ActivateQliphothCounter()
	{
	}

	// Token: 0x060020C2 RID: 8386 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void ReducedQliphothCounter()
	{
	}

	// Token: 0x060020C3 RID: 8387 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void AddedQliphothCounter()
	{
	}

	// Token: 0x060020C4 RID: 8388 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnTakeDamage(UnitModel actor, DamageInfo dmg, float value)
	{
	}

	// Token: 0x060020C5 RID: 8389 RVA: 0x00021B3D File Offset: 0x0001FD3D
	public virtual float GetRadius()
	{
		return 0f;
	}

	// Token: 0x060020C6 RID: 8390 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnElevatorStuck()
	{
	}

	// Token: 0x060020C7 RID: 8391 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnWorkClosed(UseSkill skill, int successCount)
	{
	}

	// Token: 0x060020C8 RID: 8392 RVA: 0x00013B09 File Offset: 0x00011D09
	public virtual bool IsActivatedWorkDesc()
	{
		return true;
	}

	// Token: 0x060020C9 RID: 8393 RVA: 0x00013B09 File Offset: 0x00011D09
	public virtual bool IsSensoredInPassage()
	{
		return true;
	}

	// Token: 0x060020CA RID: 8394 RVA: 0x00013B09 File Offset: 0x00011D09
	public virtual bool IsIndirectSuppressable()
	{
		return true;
	}

	// Token: 0x060020CB RID: 8395 RVA: 0x00021C87 File Offset: 0x0001FE87
	public virtual string GetRiskLevel()
	{
		return this.model.metaInfo.riskLevel;
	}

	// Token: 0x060020CC RID: 8396 RVA: 0x000FB68C File Offset: 0x000F988C
	public virtual string GetName()
	{
		if (this.model is ChildCreatureModel)
		{
			return (this.model as ChildCreatureModel).parent.metaInfo.childTypeInfo.data.name;
		}
		return this.model.metaInfo.collectionName;
	}

	// Token: 0x060020CD RID: 8397 RVA: 0x00003F3F File Offset: 0x0000213F
	public virtual bool UniqueMoveControl()
	{
		return false;
	}

	// Token: 0x060020CE RID: 8398 RVA: 0x00003F3F File Offset: 0x0000213F
	public virtual bool HasUniqueName()
	{
		return false;
	}

	// Token: 0x060020CF RID: 8399 RVA: 0x00021A33 File Offset: 0x0001FC33
	public virtual float GetDamageFactor(UnitModel target, DamageInfo damage)
	{
		return 1f;
	}

	// Token: 0x060020D0 RID: 8400 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnInitialBuild()
	{
	}

	// Token: 0x060020D1 RID: 8401 RVA: 0x00021C99 File Offset: 0x0001FE99
	public virtual bool HasUniqueCollectionCost(string areaName, out string text)
	{
		text = string.Empty;
		return false;
	}

	// Token: 0x060020D2 RID: 8402 RVA: 0x00003E35 File Offset: 0x00002035
	public virtual void OnOpenCommandWindow(Button[] buttons)
	{
	}

	// Token: 0x060020D3 RID: 8403 RVA: 0x00003F3F File Offset: 0x0000213F
	public virtual bool HasUniqueCommandAction(int workType)
	{
		return false;
	}

	// Token: 0x060020D4 RID: 8404 RVA: 0x00021A3A File Offset: 0x0001FC3A
	public virtual int HasUniqueWorkSelect(int workId)
	{
		return workId;
	}

	// Token: 0x060020D5 RID: 8405 RVA: 0x00003F3F File Offset: 0x0000213F
	public virtual bool ForcelySuccess(UseSkill skill)
	{
		return false;
	}

	// Token: 0x060020D6 RID: 8406 RVA: 0x00003F3F File Offset: 0x0000213F
	public virtual bool ForcelyFail(UseSkill skill)
	{
		return false;
	}

	// Token: 0x060020D7 RID: 8407 RVA: 0x00013B09 File Offset: 0x00011D09
	public virtual bool TryRabbitTeleport(MapNode node)
	{
		return true;
	}

	// Token: 0x060020D8 RID: 8408 RVA: 0x00013B09 File Offset: 0x00011D09
	public virtual bool CanEnterRoom()
	{
		return true;
	}

    //> <Mod>
    public virtual void OnDestroy()
    {
    }

	public virtual void OnTakeDamage_After(UnitModel actor, DamageInfo dmg)
	{

	}

	public virtual bool IsTranqable()
	{
		if (model.IsEscaped() || model.IsWorkingState() || (model.metaInfo.creatureKitType == CreatureKitType.EQUIP && model.kitEquipOwner != null)) return false;
		return true;
	}

	public virtual void OnTranquilized()
	{
		
	}

	public virtual void OnTranqEnd()
	{
		
	}

	public virtual int SuppressionEnergy
	{
		get
		{
			if (model is OrdealCreatureModel || model is EventCreatureModel || model is SefiraBossCreatureModel) return 0;
			switch (model.GetRiskLevel())
			{
				case 1:
				case 2:
					return 5;
				case 3:
					return 15;
				case 4:
					return 30;
				case 5:
					return 50;
			}
			return 0;
		}
	}

	public virtual void ForceSpawnWithoutRoom()
	{
		
	}

	public virtual void TryForceAggro(UnitModel unit)
	{
		
	}

	public virtual void TryAttractAttention(PassageObjectModel passage, bool tryCancelAggro)
	{
		
	}
	//<

	// Token: 0x04002086 RID: 8326
	public const string isolateSpriteSrc = "Sprites/CreatureSprite/Isolate/";

	// Token: 0x04002087 RID: 8327
	public CreatureModel model;

	// Token: 0x04002088 RID: 8328
	public CreatureSpecialSkill skill;

	// Token: 0x04002089 RID: 8329
	public CreatureBase.KitEquipEventListener kitEvent = new CreatureBase.KitEquipEventListener();

	// Token: 0x0400208A RID: 8330
	public bool hasUniqueEscapeLogic;

	// Token: 0x0400208B RID: 8331
	public bool isWorkAllocated;

	// Token: 0x0400208C RID: 8332
	private AgentModel _allocatedAgent;

	// Token: 0x0400208D RID: 8333
	public int damage = 1;

	// Token: 0x0400208E RID: 8334
	public List<string> OnReleaseSpecialTip = new List<string>();

	// Token: 0x0400208F RID: 8335
	public List<CreatureBase.SpecialSkillTipParameter> specialSkillTipParamList = new List<CreatureBase.SpecialSkillTipParameter>();

	// Token: 0x04002090 RID: 8336
	private SkillTriggerCheck _check;

	// Token: 0x020003DE RID: 990
	public class CreatureTimer
	{
		// Token: 0x060020D9 RID: 8409 RVA: 0x00021CA3 File Offset: 0x0001FEA3
		public CreatureTimer(float Time)
		{
			this.maxTime = Time;
		}

		// Token: 0x060020DA RID: 8410 RVA: 0x00021CB9 File Offset: 0x0001FEB9
		public CreatureTimer()
		{
		}

		// Token: 0x060020DB RID: 8411 RVA: 0x00021CC8 File Offset: 0x0001FEC8
		public void TimerStart(bool oneShot)
		{
			this.started = true;
			this.oneShot = oneShot;
			this.elapsed = 0f;
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x00021CE3 File Offset: 0x0001FEE3
		public void TimerStart(float Time, bool oneShot)
		{
			this.started = true;
			this.oneShot = oneShot;
			this.maxTime = Time;
			this.elapsed = 0f;
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x00021D05 File Offset: 0x0001FF05
		public bool isStarted()
		{
			return this.started;
		}

		// Token: 0x060020DE RID: 8414 RVA: 0x000FB6E0 File Offset: 0x000F98E0
		public bool TimerRun()
		{
			if (!this.started)
			{
				return false;
			}
			this.elapsed += Time.deltaTime;
			if (this.elapsed > this.maxTime)
			{
				this.elapsed = 0f;
				if (this.oneShot)
				{
					this.started = false;
				}
				return true;
			}
			return false;
		}

		// Token: 0x060020DF RID: 8415 RVA: 0x00021D0D File Offset: 0x0001FF0D
		public void TimerStop()
		{
			this.oneShot = false;
			this.started = false;
			this.elapsed = 0f;
		}

		// Token: 0x04002091 RID: 8337
		public float elapsed;

		// Token: 0x04002092 RID: 8338
		public float maxTime;

		// Token: 0x04002093 RID: 8339
		public bool started;

		// Token: 0x04002094 RID: 8340
		public bool oneShot;

		// Token: 0x04002095 RID: 8341
		public bool enabled = true;
	}

	// Token: 0x020003DF RID: 991
	public class SensingModule
	{
		// Token: 0x060020E0 RID: 8416 RVA: 0x00021D28 File Offset: 0x0001FF28
		public SensingModule()
		{
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x00021D37 File Offset: 0x0001FF37
		public void Set(float x1, float x2, float y1, float y2)
		{
			this.leftX = x1;
			this.rightX = x2;
			this.downY = y1;
			this.upY = y2;
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x000FB740 File Offset: 0x000F9940
		public void Set(RectTransform rect, Vector3 scale)
		{
			Vector2 vector = rect.position;
			float width = rect.rect.width;
			float height = rect.rect.height;
			this.Set(vector.x - width / 2f * scale.x, vector.x + width / 2f * scale.x, vector.y - height / 2f * scale.y, vector.y + height / 2f * scale.y);
		}

		// Token: 0x060020E3 RID: 8419 RVA: 0x00021D56 File Offset: 0x0001FF56
		public void SetEnabled(bool b)
		{
			this.enabled = b;
		}

		// Token: 0x060020E4 RID: 8420 RVA: 0x00021D5F File Offset: 0x0001FF5F
		public bool GetEnabled()
		{
			return this.enabled;
		}

		// Token: 0x060020E5 RID: 8421 RVA: 0x000FB7DC File Offset: 0x000F99DC
		public bool Check(Vector3 pos)
		{
			return this.enabled && (pos.x > this.leftX && pos.x < this.rightX && pos.y > this.downY && pos.y < this.upY);
		}

		// Token: 0x060020E6 RID: 8422 RVA: 0x000FB844 File Offset: 0x000F9A44
		public void Print()
		{
			Debug.Log(string.Concat(new object[]
			{
				this.leftX,
				" ",
				this.rightX,
				" ",
				this.downY,
				" ",
				this.upY
			}));
		}

		// Token: 0x04002096 RID: 8342
		private float leftX;

		// Token: 0x04002097 RID: 8343
		private float rightX;

		// Token: 0x04002098 RID: 8344
		private float downY;

		// Token: 0x04002099 RID: 8345
		private float upY;

		// Token: 0x0400209A RID: 8346
		private bool enabled = true;
	}

	// Token: 0x020003E0 RID: 992
	public class SpecialSkillTipParameter
	{
		// Token: 0x060020E7 RID: 8423 RVA: 0x000FB8B4 File Offset: 0x000F9AB4
		public SpecialSkillTipParameter(CreatureBase script, string key, int maxCount)
		{
			this.desc = script.model.metaInfo.specialSkillTable.GetDesc(key);
			this.maxCount = maxCount;
			this.script = script;
			this.key = key;
			this.openLevel = this.desc.openLevel;
		}

		// Token: 0x060020E8 RID: 8424 RVA: 0x000FB928 File Offset: 0x000F9B28
		public SpecialSkillTipParameter(CreatureBase script, CreatureSpecialSkillDesc desc)
		{
			this.script = script;
			this.desc = desc;
			this.maxCount = 0;
			this.key = desc.key;
			this.openLevel = desc.openLevel;
			this.isRelease = false;
		}

		// Token: 0x060020E9 RID: 8425 RVA: 0x000FB98C File Offset: 0x000F9B8C
		public SpecialSkillTipParameter(CreatureBase script, string key, int maxCount, bool isRelease)
		{
			this.desc = script.model.metaInfo.specialSkillTable.GetDesc(key);
			this.maxCount = maxCount;
			this.script = script;
			this.key = key;
			this.isRelease = isRelease;
			this.openLevel = this.desc.openLevel;
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x00021D67 File Offset: 0x0001FF67
		public bool IsActivated()
		{
			return this.isActivated;
		}

		// Token: 0x060020EB RID: 8427 RVA: 0x000FBA08 File Offset: 0x000F9C08
		public void ParamSave(params object[] param)
		{
			this.paramSave.Clear();
			foreach (object item in param)
			{
				this.paramSave.Add(item);
			}
		}

		// Token: 0x060020EC RID: 8428 RVA: 0x00021D6F File Offset: 0x0001FF6F
		public void Clear()
		{
			if (this.isActivated)
			{
				return;
			}
			this.desc.currentTipRevealCount = 0;
			this.paramSave.Clear();
		}

		// Token: 0x060020ED RID: 8429 RVA: 0x00021D94 File Offset: 0x0001FF94
		public void SetCount(int cnt)
		{
			if (this.isActivated)
			{
				return;
			}
			this.desc.currentTipRevealCount = cnt;
			if (this.desc.currentTipRevealCount >= this.maxCount)
			{
				this.OnActivate();
			}
		}

		// Token: 0x060020EE RID: 8430 RVA: 0x00021DCA File Offset: 0x0001FFCA
		public void AddCount()
		{
			if (this.isActivated)
			{
				return;
			}
			this.desc.currentTipRevealCount++;
			if (this.desc.currentTipRevealCount >= this.maxCount)
			{
				this.OnActivate();
			}
		}

		// Token: 0x060020EF RID: 8431 RVA: 0x00021E07 File Offset: 0x00020007
		public void OnObserveLevelUpdated(int currentObserveLevel)
		{
			if (this.isActivated)
			{
				return;
			}
			if (this.openLevel <= currentObserveLevel)
			{
				this.OnActivate();
			}
		}

		// Token: 0x060020F0 RID: 8432 RVA: 0x000FBA48 File Offset: 0x000F9C48
		public void OnActivate()
		{
			this.isActivated = true;
			if (this.isRelease)
			{
				this.script.OnWorkReleaseSpeicalSkillTipReveal(this.key);
			}
			else
			{
				this.script.OnForceSpecialSkillTipReveal(this.key, this.paramSave.ToArray());
			}
		}

		// Token: 0x060020F1 RID: 8433 RVA: 0x00021E27 File Offset: 0x00020027
		public bool IsRevealed()
		{
			return this.desc.isRevealed;
		}

		// Token: 0x0400209B RID: 8347
		private CreatureSpecialSkillDesc desc;

		// Token: 0x0400209C RID: 8348
		private int maxCount;

		// Token: 0x0400209D RID: 8349
		private bool isRelease = true;

		// Token: 0x0400209E RID: 8350
		private bool isActivated;

		// Token: 0x0400209F RID: 8351
		private string key = string.Empty;

		// Token: 0x040020A0 RID: 8352
		private CreatureBase script;

		// Token: 0x040020A1 RID: 8353
		private List<object> paramSave = new List<object>();

		// Token: 0x040020A2 RID: 8354
		public int openLevel;
	}

	// Token: 0x020003E1 RID: 993
	public class KitEquipEventListener
	{
		// Token: 0x060020F2 RID: 8434 RVA: 0x00003E08 File Offset: 0x00002008
		public KitEquipEventListener()
		{
		}

		// Token: 0x060020F3 RID: 8435 RVA: 0x00003E35 File Offset: 0x00002035
		public virtual void OnViewInit(CreatureUnit unit)
		{
		}

		// Token: 0x060020F4 RID: 8436 RVA: 0x00003E35 File Offset: 0x00002035
		public virtual void OnUseKit(AgentModel actor)
		{
		}

		// Token: 0x060020F5 RID: 8437 RVA: 0x00003E35 File Offset: 0x00002035
		public virtual void OnEnterRoom(AgentModel actor, UseSkill skill)
		{
		}

		// Token: 0x060020F6 RID: 8438 RVA: 0x00003E35 File Offset: 0x00002035
		public virtual void OnFixedUpdateInKitEquip(AgentModel actor)
		{
		}

		// Token: 0x060020F7 RID: 8439 RVA: 0x00003E35 File Offset: 0x00002035
		public virtual void OnAttack(AgentModel actor, UnitModel target)
		{
		}

		// Token: 0x060020F8 RID: 8440 RVA: 0x00003E35 File Offset: 0x00002035
		public virtual void OnTakeDamagePhysical(WorkerModel actor, float damage)
		{
		}

		// Token: 0x060020F9 RID: 8441 RVA: 0x00003E35 File Offset: 0x00002035
		public virtual void OnTakeDamageMental(WorkerModel actor, float damage)
		{
		}

		// Token: 0x060020FA RID: 8442 RVA: 0x00003E35 File Offset: 0x00002035
		public virtual void OnCommandReleaseKitEquip(AgentModel actor)
		{
		}

		// Token: 0x060020FB RID: 8443 RVA: 0x00003E35 File Offset: 0x00002035
		public virtual void OnReleaseKitEquip(AgentModel actor, bool stageEnd)
		{
		}
	}
}
