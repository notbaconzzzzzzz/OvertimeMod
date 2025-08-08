/*
PassageDrugged public void Process() // Fixed hallway buf
*/
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000471 RID: 1137
public class Shark : CreatureBase
{
	// Token: 0x06002910 RID: 10512 RVA: 0x0002822F File Offset: 0x0002642F
	public Shark()
	{
	}

	// Token: 0x170003B9 RID: 953
	// (get) Token: 0x06002911 RID: 10513 RVA: 0x00028263 File Offset: 0x00026463
	private static int chargeDmg
	{
		get
		{
			return UnityEngine.Random.Range(150, 151);
		}
	}

	// Token: 0x170003BA RID: 954
	// (get) Token: 0x06002912 RID: 10514 RVA: 0x00028274 File Offset: 0x00026474
	public SharkAnim animScript
	{
		get
		{
			if (this._animScript == null)
			{
				this._animScript = (base.Unit.animTarget as SharkAnim);
			}
			return this._animScript;
		}
	}

	// Token: 0x06002913 RID: 10515 RVA: 0x000282A3 File Offset: 0x000264A3
	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
		this.animScript.SetScript(this);
		this.ParamInit();
	}

	// Token: 0x06002914 RID: 10516 RVA: 0x0011D718 File Offset: 0x0011B918
	public override void ParamInit()
	{
		base.ParamInit();
		this.passagesDrugged = new List<Shark.PassageDrugged>();
		this.attacked = new List<UnitModel>();
		this.entranceNode = null;
		this.exitNode = null;
		this.model.ResetQliphothCounter();
		this.delayTimer.StopTimer();
		this.castingTimer.StopTimer();
		this.animScript.Init();
		this.model.movementScale = 0f;
		if (this.dashSound != null)
		{
			this.dashSound.Stop();
			this.dashSound = null;
		}
		if (this.delaySound != null)
		{
			this.delaySound.Stop();
			this.delaySound = null;
		}
		if (this.defaultSound != null)
		{
			this.defaultSound.Stop();
			this.defaultSound = null;
		}
		if (this.defaultSound == null)
		{
			this.defaultSound = this.MakeSoundLoop("default");
		}
	}

	// Token: 0x06002915 RID: 10517 RVA: 0x00020453 File Offset: 0x0001E653
	public override void OnStageStart()
	{
		base.OnStageStart();
		this.ParamInit();
	}

	// Token: 0x06002916 RID: 10518 RVA: 0x000282BE File Offset: 0x000264BE
	public override void OnStageRelease()
	{
		base.OnStageRelease();
		this.ParamInit();
		if (this.defaultSound != null)
		{
			this.defaultSound.Stop();
			this.defaultSound = null;
		}
	}

	// Token: 0x06002917 RID: 10519 RVA: 0x0011D818 File Offset: 0x0011BA18
	public override void OnReleaseWork(UseSkill skill)
	{
		base.OnReleaseWork(skill);
		if (skill.agent.temperanceLevel == 1)
		{
			this.model.SubQliphothCounter();
		}
		if ((skill.agent.mental <= 0f && !skill.agent.invincible) || skill.agent.IsPanic())
		{
			this.model.SubQliphothCounter();
		}
	}

	// Token: 0x06002918 RID: 10520 RVA: 0x0001DFE3 File Offset: 0x0001C1E3
	public override void ActivateQliphothCounter()
	{
		base.ActivateQliphothCounter();
		this.Escape();
	}

	// Token: 0x06002919 RID: 10521 RVA: 0x0011D888 File Offset: 0x0011BA88
	public override void OnFixedUpdate(CreatureModel creature)
	{
		base.OnFixedUpdate(creature);
		foreach (Shark.PassageDrugged passageDrugged in this.passagesDrugged)
		{
			passageDrugged.Process();
		}
	}

	// Token: 0x0600291A RID: 10522 RVA: 0x0011D8EC File Offset: 0x0011BAEC
	public override void UniqueEscape()
	{
		base.UniqueEscape();
		if (this.model.hp <= 0f)
		{
			return;
		}
		this.model.CheckNearWorkerEncounting();
		if (this.delayTimer.started)
		{
			this.movable.StopMoving();
			if (this.delayTimer.RunTimer())
			{
				if (this.movable.GetDirection() == UnitDirection.RIGHT)
				{
					this.movable.SetDirection(UnitDirection.LEFT);
				}
				else
				{
					this.movable.SetDirection(UnitDirection.RIGHT);
				}
				foreach (Shark.PassageDrugged passageDrugged in this.passagesDrugged)
				{
					if (passageDrugged.GetPassage() != this.currentPassage)
					{
						passageDrugged.IsPassed = false;
					}
					else
					{
						passageDrugged.IsPassed = true;
					}
				}
				this.StartCasting();
				this.animScript.EndDelay();
			}
			return;
		}
		if (this.castingTimer.started)
		{
			this.movable.StopMoving();
			if (this.castingTimer.RunTimer())
			{
				this.EndCasting();
			}
			return;
		}
		if (this.currentPassage != null)
		{
			List<UnitModel> list = new List<UnitModel>();
			foreach (MovableObjectNode movableObjectNode in this.currentPassage.GetEnteredTargets(this.movable))
			{
				UnitModel unit = movableObjectNode.GetUnit();
				if (unit.hp > 0f)
				{
					if (!this.attacked.Contains(unit))
					{
						if (this.IsInRange(movableObjectNode))
						{
							list.Add(unit);
							this.attacked.Add(unit);
						}
					}
				}
			}
			foreach (UnitModel unitModel in list)
			{
				unitModel.TakeDamage(this.model, new DamageInfo(RwbpType.R, (float)Shark.chargeDmg));
				DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(unitModel, RwbpType.R, this.model);
				if (unitModel is WorkerModel && (unitModel as WorkerModel).IsDead())
				{
					this.MakeSound("attack");
					this.MakeExplodeEffect(this.movable.GetDirection(), unitModel as WorkerModel, 1f);
				}
			}
		}
	}

	// Token: 0x0600291B RID: 10523 RVA: 0x0011DB74 File Offset: 0x0011BD74
	public override void OnReturn()
	{
		base.OnReturn();
		foreach (Shark.PassageDrugged passageDrugged in this.passagesDrugged)
		{
			passageDrugged.Destroy();
		}
		this.ParamInit();
	}

	// Token: 0x0600291C RID: 10524 RVA: 0x0011DBDC File Offset: 0x0011BDDC
	public override void Escape()
	{
		base.Escape();
		this.PrevEscape();
		this.model.Escape();
		this.animScript.OnEscape();
		if (this.Prob(50))
		{
			this.movable.SetDirection(UnitDirection.RIGHT);
		}
		else
		{
			this.movable.SetDirection(UnitDirection.LEFT);
		}
		this.StartCasting();
		this.Teleport();
	}

	// Token: 0x0600291D RID: 10525 RVA: 0x000282EF File Offset: 0x000264EF
	private void PrevEscape()
	{
		if (this.defaultSound != null)
		{
			this.defaultSound.Stop();
			this.defaultSound = null;
		}
		this.PassageSelection();
	}

	// Token: 0x0600291E RID: 10526 RVA: 0x0011DC44 File Offset: 0x0011BE44
	private void Teleport()
	{
		if (!this.NodeSelection())
		{
			this.model.movementScale = 0f;
			this.animScript.OnDelay();
			this.delayTimer.StartTimer(10f);
			if (this.dashSound != null)
			{
				this.dashSound.Stop();
				this.dashSound = null;
			}
			if (this.delaySound == null)
			{
				this.delaySound = this.MakeSound("delay");
			}
		}
		else
		{
			this.movable.SetCurrentNode(this.exitNode);
			Sefira sefira = SefiraManager.instance.GetSefira(this.exitNode.GetAttachedPassage().GetSefiraName());
			this.model.sefira = sefira;
			this.model.sefiraNum = sefira.indexString;
			if (this.castingTimer.started)
			{
				this.MakeSound("cast");
			}
			else
			{
				this.Charge();
			}
		}
	}

	// Token: 0x0600291F RID: 10527 RVA: 0x0011DD44 File Offset: 0x0011BF44
	private void PassageSelection()
	{
		Sefira[] openedAreaList = PlayerModel.instance.GetOpenedAreaList();
		List<PassageObjectModel> list = new List<PassageObjectModel>();
		for (int i = 0; i < openedAreaList.Length; i++)
		{
			List<PassageObjectModel> list2 = new List<PassageObjectModel>();
			int num = 1;
			foreach (PassageObjectModel passageObjectModel in openedAreaList[i].passageList)
			{
				if (passageObjectModel.isActivate)
				{
					if (passageObjectModel.GetPassageType() == PassageType.HORIZONTAL)
					{
						if (!(SefiraMapLayer.instance.GetPassageObject(passageObjectModel) == null))
						{
							if (SefiraMapLayer.instance.GetPassageObject(passageObjectModel).type != SpaceObjectType.HUBSHORT)
							{
								list2.Add(passageObjectModel);
							}
						}
					}
				}
			}
			while (list2.Count > 0 && num > 0)
			{
				PassageObjectModel item = list2[UnityEngine.Random.Range(0, list2.Count)];
				list.Add(item);
				list2.Remove(item);
				num--;
			}
		}
		foreach (PassageObjectModel passageObjectModel2 in list)
		{
			Texture2D sp = Resources.Load("Sprites/CreatureSprite/Shark/Filter") as Texture2D;
			PassageObject passageObject = SefiraMapLayer.instance.GetPassageObject(passageObjectModel2);
			if (passageObject != null)
			{
				GameObject filter = passageObject.SetPassageFilter(sp, "SharkDrugFilter", "Normal", 2);
				Shark.PassageDrugged item2 = new Shark.PassageDrugged(passageObjectModel2, filter);
				this.passagesDrugged.Add(item2);
			}
		}
	}

	// Token: 0x06002920 RID: 10528 RVA: 0x0011DF10 File Offset: 0x0011C110
	private bool NodeSelection()
	{
		List<Shark.PassageDrugged> list = new List<Shark.PassageDrugged>();
		foreach (Shark.PassageDrugged passageDrugged in this.passagesDrugged)
		{
			if (!passageDrugged.IsPassed)
			{
				list.Add(passageDrugged);
			}
		}
		if (list.Count <= 0)
		{
			Debug.Log("All passed : shark");
			MapNode mapNode = this.entranceNode;
			this.entranceNode = this.exitNode;
			this.exitNode = mapNode;
			return false;
		}
		Shark.PassageDrugged passageDrugged2 = list[UnityEngine.Random.Range(0, list.Count - 1)];
		passageDrugged2.IsPassed = true;
		List<MapNode> list2 = new List<MapNode>(passageDrugged2.GetPassage().GetNodeList());
		List<MapNode> list3 = list2;
		if (cache0 == null)
		{
			cache0 = new Comparison<MapNode>(MapNode.CompareByX);
		}
		list3.Sort(cache0);
		if (this.movable.GetDirection() == UnitDirection.RIGHT)
		{
			this.entranceNode = list2[list2.Count - 1];
			this.exitNode = list2[0];
		}
		else
		{
			this.entranceNode = list2[0];
			this.exitNode = list2[list2.Count - 1];
		}
		return true;
	}

	// Token: 0x06002921 RID: 10529 RVA: 0x0011E068 File Offset: 0x0011C268
	private void Charge()
	{
		this.attacked = new List<UnitModel>();
		this.model.movementScale = 24f;
		this.model.MoveToNode(this.entranceNode);
		CreatureCommand currentCommand = this.model.GetCurrentCommand();
		currentCommand.SetEndCommand(new CreatureCommand.OnCommandEnd(this.Teleport));
	}

	// Token: 0x06002922 RID: 10530 RVA: 0x0002831A File Offset: 0x0002651A
	private void StartCasting()
	{
		this.castingTimer.StartTimer(8f);
		this.animScript.OnCasting();
		this.MakeSound("cast");
	}

	// Token: 0x06002923 RID: 10531 RVA: 0x00028343 File Offset: 0x00026543
	private void EndCasting()
	{
		this.animScript.EndCasting();
		this.Charge();
		this.MakeSound("dash_start");
	}

	// Token: 0x06002924 RID: 10532 RVA: 0x00028362 File Offset: 0x00026562
	public override bool OnAfterSuppressed()
	{
		this.animScript.PlayDeadMotion();
		if (this.dashSound != null)
		{
			this.dashSound.Stop();
			this.dashSound = null;
		}
		this.MakeSound("dead");
		return true;
	}

	// Token: 0x06002925 RID: 10533 RVA: 0x0011E0C0 File Offset: 0x0011C2C0
	private bool IsInRange(MovableObjectNode mov)
	{
		float num = 1f;
		if (this.currentPassage != null)
		{
			num = this.currentPassage.scaleFactor;
		}
		return Math.Abs(mov.GetCurrentViewPosition().x - this.movable.GetCurrentViewPosition().x) <= 3f * num;
	}

	// Token: 0x06002926 RID: 10534 RVA: 0x00105F74 File Offset: 0x00104174
	public void MakeExplodeEffect(UnitDirection dir, WorkerModel target, float size)
	{
		ExplodeGutEffect explodeGutEffect = null;
		WorkerUnit workerUnit = target.GetWorkerUnit();
		if (ExplodeGutManager.instance.MakeEffects(workerUnit.gameObject.transform.position, ref explodeGutEffect))
		{
			explodeGutEffect.particleCount = UnityEngine.Random.Range(3, 9);
			explodeGutEffect.ground = target.GetMovableNode().GetCurrentViewPosition().y;
			float num = 0f;
			float num2 = 0f;
			explodeGutEffect.SetEffectSize(size);
			explodeGutEffect.Shoot(ExplodeGutEffect.Directional.DIRECTION, dir);
			if (target.GetMovableNode().GetPassage() != null)
			{
				target.GetMovableNode().GetPassage().GetVerticalRange(ref num, ref num2);
				explodeGutEffect.SetCurrentPassage(target.GetMovableNode().GetPassage());
			}
		}
		workerUnit.gameObject.SetActive(false);
	}

	// Token: 0x06002927 RID: 10535 RVA: 0x000EF070 File Offset: 0x000ED270
	public string GetSoundSrc(string key)
	{
		string empty = string.Empty;
		if (this.model.metaInfo.soundTable.TryGetValue(key, out empty))
		{
		}
		return empty;
	}

	// Token: 0x06002928 RID: 10536 RVA: 0x0011E120 File Offset: 0x0011C320
	public override SoundEffectPlayer MakeSound(string src)
	{
		string soundSrc = this.GetSoundSrc(src);
		if (soundSrc == string.Empty)
		{
			return null;
		}
		return SoundEffectPlayer.PlayOnce(soundSrc, this.model.Unit.transform.position);
	}

	// Token: 0x06002929 RID: 10537 RVA: 0x0011E168 File Offset: 0x0011C368
	public override SoundEffectPlayer MakeSoundLoop(string src)
	{
		string soundSrc = this.GetSoundSrc(src);
		if (soundSrc == string.Empty)
		{
			return null;
		}
		return SoundEffectPlayer.Play(soundSrc, this.model.Unit.transform);
	}

	// Token: 0x04002763 RID: 10083
	private Timer castingTimer = new Timer();

	// Token: 0x04002764 RID: 10084
	private const float castingTime = 8f;

	// Token: 0x04002765 RID: 10085
	private Timer delayTimer = new Timer();

	// Token: 0x04002766 RID: 10086
	private const float delayTime = 10f;

	// Token: 0x04002767 RID: 10087
	private const int chargeDmgMin = 150;

	// Token: 0x04002768 RID: 10088
	private const int chargeDmgMax = 150;

	// Token: 0x04002769 RID: 10089
	private const float attackRange = 3f;

	// Token: 0x0400276A RID: 10090
	private const float movement = 24f;

	// Token: 0x0400276B RID: 10091
	private const int _qliphothCounterMax = 2;

	// Token: 0x0400276C RID: 10092
	private const string _filterName = "SharkDrugFilter";

	// Token: 0x0400276D RID: 10093
	private const string sound_cast = "cast";

	// Token: 0x0400276E RID: 10094
	private const string sound_dashStart = "dash_start";

	// Token: 0x0400276F RID: 10095
	public const string sound_dash = "dash";

	// Token: 0x04002770 RID: 10096
	public const string sound_delay = "delay";

	// Token: 0x04002771 RID: 10097
	private const string sound_attack = "attack";

	// Token: 0x04002772 RID: 10098
	private const string sound_dead = "dead";

	// Token: 0x04002773 RID: 10099
	private const string sound_default = "default";

	// Token: 0x04002774 RID: 10100
	private List<UnitModel> attacked = new List<UnitModel>();

	// Token: 0x04002775 RID: 10101
	private List<Shark.PassageDrugged> passagesDrugged = new List<Shark.PassageDrugged>();

	// Token: 0x04002776 RID: 10102
	private MapNode entranceNode;

	// Token: 0x04002777 RID: 10103
	private MapNode exitNode;

	// Token: 0x04002778 RID: 10104
	private SoundEffectPlayer defaultSound;

	// Token: 0x04002779 RID: 10105
	public SoundEffectPlayer dashSound;

	// Token: 0x0400277A RID: 10106
	public SoundEffectPlayer delaySound;

	// Token: 0x0400277B RID: 10107
	private SharkAnim _animScript;

	// Token: 0x0400277C RID: 10108
	[CompilerGenerated]
	private static Comparison<MapNode> cache0;

	// Token: 0x02000472 RID: 1138
	public class PassageDrugged
	{
		// Token: 0x0600292A RID: 10538 RVA: 0x0002839F File Offset: 0x0002659F
		public PassageDrugged(PassageObjectModel passage, GameObject filter)
		{
			this._passage = passage;
			this._filter = filter;
			this._passed = false;
		}

		// Token: 0x0600292B RID: 10539 RVA: 0x0011E1A8 File Offset: 0x0011C3A8
		public void Process()
		{ // <Mod> fixed hallway buf being overriden every frame
			if (this._passage != null)
			{
				foreach (MovableObjectNode movableObjectNode in this._passage.GetEnteredTargets())
				{
					UnitModel unit = movableObjectNode.GetUnit();
					if (unit is WorkerModel)
					{
						WorkerModel workerModel = unit as WorkerModel;
						UnitBuf buf = workerModel.GetUnitBufByType(UnitBufType.SHARK_DRUGGED);
						if (buf != null) {
							buf.remainTime = 1f;
						} else {
							workerModel.AddUnitBuf(new SharkDruggedBuf());
						}
					}
				}
			}
		}

		// Token: 0x0600292C RID: 10540 RVA: 0x000283BC File Offset: 0x000265BC
		public PassageObjectModel GetPassage()
		{
			return this._passage;
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x0600292D RID: 10541 RVA: 0x000283C4 File Offset: 0x000265C4
		// (set) Token: 0x0600292E RID: 10542 RVA: 0x000283CC File Offset: 0x000265CC
		public bool IsPassed
		{
			get
			{
				return this._passed;
			}
			set
			{
				this._passed = value;
			}
		}

		// Token: 0x0600292F RID: 10543 RVA: 0x000283D5 File Offset: 0x000265D5
		public void Destroy()
		{
			UnityEngine.Object.Destroy(this._filter);
		}

		// Token: 0x0400277D RID: 10109
		private PassageObjectModel _passage;

		// Token: 0x0400277E RID: 10110
		private GameObject _filter;

		// Token: 0x0400277F RID: 10111
		private bool _passed;
	}
}
