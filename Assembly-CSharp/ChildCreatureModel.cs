using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Spine.Unity;
using UnityEngine;

// Token: 0x020005F6 RID: 1526
public class ChildCreatureModel : CreatureModel, IObserver
{
	// Token: 0x0600343C RID: 13372 RVA: 0x0002FDCD File Offset: 0x0002DFCD
	public ChildCreatureModel(long instanceId) : base(instanceId)
	{
		Debug.Log("Child Generated");
	}

	// Token: 0x170004F6 RID: 1270
	// (get) Token: 0x0600343D RID: 13373 RVA: 0x0002FE00 File Offset: 0x0002E000
	public CreatureModel parent
	{
		get
		{
			return this._parent;
		}
	}

	// Token: 0x170004F7 RID: 1271
	// (get) Token: 0x0600343E RID: 13374 RVA: 0x0002FE08 File Offset: 0x0002E008
	public ChildCreatureTypeInfo childMetaInfo
	{
		get
		{
			return this.parent.metaInfo.childTypeInfo;
		}
	}

	// Token: 0x170004F8 RID: 1272
	// (get) Token: 0x0600343F RID: 13375 RVA: 0x0002FE1A File Offset: 0x0002E01A
	public new ChildCreatureUnit Unit
	{
		get
		{
			return this._unit;
		}
	}

	// Token: 0x06003440 RID: 13376 RVA: 0x0016048C File Offset: 0x0015E68C
	public void SetParent(CreatureModel creature)
	{ // <Patch>
		this._parent = creature;
		this.metaInfo = creature.metaInfo.childTypeInfo;
		this.metadataId = creature.metaInfo.childTypeInfo.id;
		if (this.childMetaInfo.isHasBaseMeta)
		{
			LobotomyBaseMod.LcIdLong lcid = new LobotomyBaseMod.LcIdLong(CreatureTypeList.instance.GetModId(creature.metaInfo), this.childMetaInfo.id);
			this.metaInfo = CreatureTypeList.instance.GetData_Mod(lcid);
			if ((this.observeInfo = CreatureManager.instance.GetObserveInfo_Mod(lcid)) == null)
			{
				this.observeInfo = new CreatureObserveInfoModel(this.childMetaInfo.id);
                this.observeInfo.Init_Mod(lcid);
			}
			if (CreatureTypeList.instance.GetSkillTipData_Mod(lcid) != null)
			{
				this.metaInfo.specialSkillTable = CreatureTypeList.instance.GetSkillTipData_Mod(lcid).GetCopy();
			}
			CreatureManager.instance.AddChildObserveInfo(this.observeInfo);
		}
		this._unit = this.GenCreatureUnit(null);
		this.LoadScript(this.childMetaInfo.script);
		Notice.instance.Observe(NoticeName.FixedUpdate, this);
		this.sefiraNum = this.parent.sefiraNum;
		this.sefira = this.parent.sefira;
	}

	// Token: 0x06003441 RID: 13377 RVA: 0x001605CC File Offset: 0x0015E7CC
	public void SetParent(CreatureModel creature, string childScriptSrc, string childPrefab)
	{ // <Patch>
		this._parent = creature;
		this.metaInfo = creature.metaInfo.childTypeInfo;
		if (this.childMetaInfo.isHasBaseMeta)
		{
			LobotomyBaseMod.LcIdLong lcid = new LobotomyBaseMod.LcIdLong(CreatureTypeList.instance.GetModId(creature.metaInfo), this.childMetaInfo.id);
			this.metaInfo = CreatureTypeList.instance.GetData_Mod(lcid);
			this.observeInfo = new CreatureObserveInfoModel(this.childMetaInfo.id);
			if (CreatureTypeList.instance.GetSkillTipData_Mod(lcid) != null)
			{
				this.metaInfo.specialSkillTable = CreatureTypeList.instance.GetSkillTipData_Mod(lcid).GetCopy();
			}
		}
		this._unit = this.GenCreatureUnit(childPrefab);
		this.LoadScript(childScriptSrc);
		Notice.instance.Observe(NoticeName.FixedUpdate, this);
		this.sefiraNum = this.parent.sefiraNum;
		this.sefira = this.parent.sefira;
	}

	// Token: 0x06003442 RID: 13378 RVA: 0x0002FE22 File Offset: 0x0002E022
	public CreatureTypeInfo GetBaseMetaInfo()
	{
		return this.metaInfo;
	}

	// Token: 0x06003443 RID: 13379 RVA: 0x001606B8 File Offset: 0x0015E8B8
	private void LoadScript(string src)
	{
		object obj = null;
		foreach (Assembly assembly in Add_On.instance.AssemList)
		{
			foreach (Type type in assembly.GetTypes())
			{
				if (type.Name == src)
				{
					obj = Activator.CreateInstance(type);
				}
			}
		}
		if (obj == null)
		{
			obj = Activator.CreateInstance(Type.GetType(src));
		}
		this.script = (CreatureBase)obj;
		this.script.SetModel(this);
		this.script.OnInit();
		this.Speed = this.childMetaInfo.speed;
	}

	// Token: 0x06003444 RID: 13380 RVA: 0x000043CD File Offset: 0x000025CD
	public override void OnStageRelease()
	{
	}

	// Token: 0x06003445 RID: 13381 RVA: 0x0016077C File Offset: 0x0015E97C
	public override void OnFixedUpdate()
	{
		if (this.remainMoveDelay > 0f)
		{
			this.remainMoveDelay -= Time.deltaTime;
		}
		if (this.remainAttackDelay > 0f)
		{
			this.remainAttackDelay -= Time.deltaTime;
		}
		this.commandQueue.Execute(this);
		if (this.animAutoSet)
		{
			if (this.GetMovableNode().IsMoving())
			{
				this.SetMoveAnimState(true);
			}
			else
			{
				this.SetMoveAnimState(false);
			}
		}
		if (this._equipment.weapon != null)
		{
			this._equipment.weapon.OnFixedUpdate();
		}
		if (GameManager.currentGameManager.ManageStarted)
		{
			this.script.OnFixedUpdate(this);
		}
		if (base.state == CreatureState.ESCAPE)
		{
			this.OnEscapeUpdate();
		}
		else if (base.state == CreatureState.SUPPRESSED)
		{
		}
		if (this.remainMoveDelay > 0f)
		{
			this.movableNode.ProcessMoveNode(0f);
		}
		else
		{
			this.movableNode.ProcessMoveNode(this.Speed);
		}
	}

	// Token: 0x06003446 RID: 13382 RVA: 0x0016089C File Offset: 0x0015EA9C
	public override void SendAnimMessage(string name)
	{
		if (this.activateState && this.Unit != null)
		{
			CreatureAnimScript animTarget = this.Unit.animTarget;
			if (animTarget != null)
			{
				animTarget.SendMessage(name);
			}
		}
	}

	// Token: 0x06003447 RID: 13383 RVA: 0x001608E4 File Offset: 0x0015EAE4
	public override void SetMoveAnimState(bool b)
	{
		if (this.Unit == null)
		{
			return;
		}
		if (this.activateState)
		{
			CreatureAnimScript animTarget = this.Unit.animTarget;
			if (animTarget != null && animTarget.gameObject.activeInHierarchy)
			{
				if (b)
				{
					animTarget.Move();
				}
				else
				{
					animTarget.Stop();
				}
			}
		}
	}

	// Token: 0x06003448 RID: 13384 RVA: 0x0002FE2A File Offset: 0x0002E02A
	public override void OnEscapeUpdate()
	{
		this.script.UniqueEscape();
	}

	// Token: 0x06003449 RID: 13385 RVA: 0x0002FE37 File Offset: 0x0002E037
	public override void PursueWorker(WorkerModel target)
	{
		this.commandQueue.SetAgentCommand(CreatureCommand.MakePursue(target));
	}

	// Token: 0x0600344A RID: 13386 RVA: 0x00160950 File Offset: 0x0015EB50
	public new List<WorkerModel> CheckNearWorkerEncounting()
	{
		List<WorkerModel> list = new List<WorkerModel>();
		PassageObjectModel passage = this.movableNode.GetPassage();
		if (passage == null)
		{
			return list;
		}
		foreach (MovableObjectNode movableObjectNode in passage.GetEnteredTargets())
		{
			UnitModel unit = movableObjectNode.GetUnit();
			if (unit is WorkerModel)
			{
				WorkerModel workerModel = unit as WorkerModel;
				if (!workerModel.IsDead())
				{
					if (!(workerModel is AgentModel) || (workerModel as AgentModel).activated)
					{
						if (!list.Contains(workerModel))
						{
							list.Add(workerModel);
						}
						if (!this.encounteredWorker.Contains(workerModel))
						{
							this.encounteredWorker.Add(workerModel);
							this.ChildInitialEncounter(workerModel);
							if (!workerModel.returnPanic)
							{
								workerModel.EncounterCreature(this);
							}
						}
					}
				}
			}
		}
		return list;
	}

	// Token: 0x0600344B RID: 13387 RVA: 0x0002FE4A File Offset: 0x0002E04A
	public void ChildInitialEncounter(WorkerModel worker)
	{
		if (worker.IsDead())
		{
			return;
		}
		if (worker.invincible)
		{
			return;
		}
		worker.InitialEncounteredCreature(this);
	}

	// Token: 0x0600344C RID: 13388 RVA: 0x0002FE6B File Offset: 0x0002E06B
	public override string GetUnitName()
	{
		return this.childMetaInfo.data.name;
	}

	// Token: 0x0600344D RID: 13389 RVA: 0x00160A64 File Offset: 0x0015EC64
	public override void Escape()
	{
		base.ClearCommand();
		base.state = CreatureState.ESCAPE;
		this.baseMaxHp = this.childMetaInfo.maxHp;
		this.hp = (float)this.childMetaInfo.maxHp;
		this.SetFaction(FactionTypeList.StandardFaction.EscapedCreature);
		Notice.instance.Send(NoticeName.OnEscape, new object[]
		{
			this
		});
	}

	// Token: 0x0600344E RID: 13390 RVA: 0x0002FE7D File Offset: 0x0002E07D
	public override bool IsAttackTargetable()
	{
		return !this.destroied && base.IsAttackTargetable();
	}

	// Token: 0x0600344F RID: 13391 RVA: 0x00160AC8 File Offset: 0x0015ECC8
	public override void Suppressed()
	{
		base.Suppressed();
		this.destroied = true;
		this.parent.script.OnChildSuppressed(this);
		if (this.movableNode != null)
		{
			this.movableNode.SetActive(false);
		}
		if (this.factionTypeInfo.code != FactionTypeList.StandardFaction.IdleCreature)
		{
			this.SetFaction(FactionTypeList.StandardFaction.IdleCreature);
		}
	}

	// Token: 0x06003450 RID: 13392 RVA: 0x0002FE92 File Offset: 0x0002E092
	public void OnDeleted()
	{
		Notice.instance.Remove(NoticeName.FixedUpdate, this);
	}

	// Token: 0x06003451 RID: 13393 RVA: 0x00160B30 File Offset: 0x0015ED30
	public ChildCreatureUnit GenCreatureUnit(string prefabSrc = null)
	{
		ChildCreatureUnit component = ResourceCache.instance.LoadPrefab("Unit/ChildCreatureBase").GetComponent<ChildCreatureUnit>();
		component.transform.position = new Vector3(0f, 0f, -1000f);
		component.transform.SetParent(CreatureLayer.currentLayer.transform, false);
		component.model = this;
		base.SetUnit(component);
		if (prefabSrc == null)
		{
			if (this.childMetaInfo.animSrc != string.Empty)
			{
				this.LoadCustom(component, this.childMetaInfo.animSrc);
			}
		}
		else
		{
			this.LoadCustom(component, prefabSrc);
		}
		component.returnObject = component.returnSpriteRenderer.gameObject;
		component.returnObject.SetActive(false);
		component.Init();
		return component;
	}

	// Token: 0x06003452 RID: 13394 RVA: 0x0002FEA4 File Offset: 0x0002E0A4
	public new void OnNotice(string notice, params object[] param)
	{
		if (this.destroied)
		{
			return;
		}
		if (notice == NoticeName.FixedUpdate)
		{
			this.OnFixedUpdate();
		}
	}

	// Token: 0x06003453 RID: 13395 RVA: 0x0002FEC8 File Offset: 0x0002E0C8
	public override int GetRiskLevel()
	{
		return (int)(this.RiskLevel + 1);
	}

	// Token: 0x06003454 RID: 13396 RVA: 0x00160BF0 File Offset: 0x0015EDF0
	public void SelfDestroy()
	{
		try
		{
			if (this.movableNode != null)
			{
				this.movableNode.SetActive(false);
			}
			Notice.instance.Remove(NoticeName.FixedUpdate, this);
			this.commandQueue.Clear();
			this.movableNode = null;
			UnityEngine.Object.Destroy(this.Unit);
			this.destroied = true;
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			if (this.Unit != null)
			{
				UnityEngine.Object.Destroy(this.Unit);
			}
		}
	}

	// Token: 0x06003455 RID: 13397 RVA: 0x0002FED2 File Offset: 0x0002E0D2
	public void SetSpeed(float speed = -1f)
	{
		if (speed == -1f)
		{
			this.Speed = this.childMetaInfo.speed;
		}
		else
		{
			this.Speed = speed;
		}
	}

	// Token: 0x06003456 RID: 13398 RVA: 0x00160C88 File Offset: 0x0015EE88
	public void LoadCustom(ChildCreatureUnit component, string Src)
	{
		string[] array = Src.Split(new char[]
		{
			'/'
		});
		if (array[0] == "Custom")
		{
			DirectoryInfo directoryInfo = null;
			foreach (DirectoryInfo directoryInfo2 in Add_On.instance.DirList)
			{
				if (Directory.Exists(directoryInfo2.FullName + "/CreatureAnimation/" + array[1]))
				{
					directoryInfo = new DirectoryInfo(directoryInfo2.FullName + "/CreatureAnimation/" + array[1]);
					break;
				}
			}
			if (directoryInfo != null)
			{
				List<Texture2D> list = new List<Texture2D>();
				foreach (FileInfo fileInfo in directoryInfo.GetFiles())
				{
					if (fileInfo.FullName.Contains(".png"))
					{
						byte[] data = File.ReadAllBytes(fileInfo.FullName);
						Texture2D texture2D = new Texture2D(2, 2);
						texture2D.LoadImage(data);
						texture2D.name = Path.GetFileNameWithoutExtension(fileInfo.Name);
						list.Add(texture2D);
					}
				}
				string atlasText = File.ReadAllText(directoryInfo.FullName + "/atlas.txt");
				Shader shader = null;
				AtlasAsset atlasAsset = AtlasAsset.CreateRuntimeInstance(atlasText, list.ToArray(), shader, true);
				GameObject gameObject;
				if (File.Exists(directoryInfo.FullName + "/json.txt"))
				{
					gameObject = SkeletonAnimation.NewSkeletonAnimationGameObject(SkeletonDataAsset.CreateRuntimeInstance(File.ReadAllText(directoryInfo.FullName + "/json.txt"), atlasAsset, true, 0.01f)).gameObject;
				}
				else
				{
					gameObject = SkeletonAnimation.NewSkeletonAnimationGameObject(SkeletonDataAsset.CreateRuntimeInstance(File.ReadAllBytes(directoryInfo.FullName + "/skeleton.skel"), atlasAsset, true, 0.01f)).gameObject;
				}
				Type type = null;
				foreach (Assembly assembly in Add_On.instance.AssemList)
				{
					foreach (Type type2 in assembly.GetTypes())
					{
						if (type2.Name == array[1])
						{
							type = type2;
							break;
						}
					}
					if (type != null)
					{
						break;
					}
				}
				gameObject.AddComponent(type);
				component.animTarget = gameObject.GetComponent<CreatureAnimScript>();
				gameObject.transform.SetParent(component.transform, false);
				return;
			}
		}
		else
		{
			GameObject gameObject2 = Prefab.LoadPrefab(Src);
			component.animTarget = gameObject2.GetComponent<CreatureAnimScript>();
			gameObject2.transform.SetParent(component.transform, false);
		}
	}

	// Token: 0x040030EA RID: 12522
	private CreatureModel _parent;

	// Token: 0x040030EB RID: 12523
	private new ChildCreatureUnit _unit;

	// Token: 0x040030EC RID: 12524
	public bool destroied;

	// Token: 0x040030ED RID: 12525
	public bool activateState = true;

	// Token: 0x040030EE RID: 12526
	public bool animAutoSet = true;

	// Token: 0x040030EF RID: 12527
	public RiskLevel RiskLevel = RiskLevel.HE;

	// Token: 0x040030F0 RID: 12528
	public string PortraitSrc = string.Empty;

	// Token: 0x040030F1 RID: 12529
	private float Speed;
}
