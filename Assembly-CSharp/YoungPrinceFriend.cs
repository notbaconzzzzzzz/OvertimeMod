using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004B6 RID: 1206
public class YoungPrinceFriend : CreatureBase
{
	// Token: 0x06002C42 RID: 11330 RVA: 0x0012EDE8 File Offset: 0x0012CFE8
	public YoungPrinceFriend()
	{
	}

	// Token: 0x1700041A RID: 1050
	// (get) Token: 0x06002C43 RID: 11331 RVA: 0x000211E4 File Offset: 0x0001F3E4
	private static float moveFreq
	{
		get
		{
			return UnityEngine.Random.Range(1f, 2f);
		}
	}

	// Token: 0x1700041B RID: 1051
	// (get) Token: 0x06002C44 RID: 11332 RVA: 0x0001FDE5 File Offset: 0x0001DFE5
	private static float motionDelay
	{
		get
		{
			return UnityEngine.Random.Range(0.8f, 1.2f);
		}
	}

	// Token: 0x1700041C RID: 1052
	// (get) Token: 0x06002C45 RID: 11333 RVA: 0x0002B31C File Offset: 0x0002951C
	private static int atkDmg
	{
		get
		{
			return UnityEngine.Random.Range(6, 5);
		}
	}

	// Token: 0x1700041D RID: 1053
	// (get) Token: 0x06002C46 RID: 11334 RVA: 0x0002B325 File Offset: 0x00029525
	private static int smashDmg
	{
		get
		{
			return UnityEngine.Random.Range(10, 8);
		}
	}

	// Token: 0x1700041E RID: 1054
	// (get) Token: 0x06002C47 RID: 11335 RVA: 0x0002B32F File Offset: 0x0002952F
	public YoungPrinceFriendAnim animScript
	{
		get
		{
			if (this._animScript == null)
			{
				this._animScript = (base.Unit.animTarget as YoungPrinceFriendAnim);
			}
			return this._animScript;
		}
	}

	// Token: 0x06002C48 RID: 11336 RVA: 0x0002B35E File Offset: 0x0002955E
	public override void SetModel(CreatureModel model)
	{
		this.model = model;
		this.SetPrince(((ChildCreatureModel)model).parent.script as YoungPrince);
	}

	// Token: 0x06002C49 RID: 11337 RVA: 0x0002B382 File Offset: 0x00029582
	public void SetPrince(YoungPrince prince)
	{
		this.prince = prince;
	}

	// Token: 0x06002C4A RID: 11338 RVA: 0x0012EE58 File Offset: 0x0012D058
	public override void OnViewInit(CreatureUnit unit)
	{
		this.animScript.SetScript(this);
		this.name = this.prince.GetFriendName();
		this.passage = this.prince.model.GetEntryNode().GetAttachedPassage();
		this.MakeSound("Default");
		this.isMovable = false;
		this.motionDelayTimer.StartTimer(2.5f);
		foreach (MovableObjectNode movableObjectNode in this.passage.GetEnteredTargets())
		{
			CreatureModel creatureModel = movableObjectNode.GetUnit() as CreatureModel;
			if (creatureModel != null)
			{
				if (creatureModel.script is YoungPrinceFriend)
				{
					this.isRoamer = true;
				}
			}
		}
	}

	// Token: 0x06002C4B RID: 11339 RVA: 0x0012EF38 File Offset: 0x0012D138
	public override void UniqueEscape()
	{
		if (this.passage == null)
		{
			return;
		}
		if (this.aggro != null && !this.IsHostile(this.aggro.GetMovableNode()))
		{
			this.StopMovement();
			this.aggro = null;
		}
		if (!this.escapeInit && this.currentPassage == this.passage)
		{
			this.escapeInit = true;
			this.spreadTimer.StartTimer(0.2f);
		}
		if (this.spreadTimer.RunTimer() && this.SpreadSpore())
		{
			this.spreadTimer.StartTimer(0.2f);
		}
		if (this.motionDelayTimer.started)
		{
			this.StopMovement();
			this.motionDelayTimer.RunTimer();
			return;
		}
		if (this.IsAttacking())
		{
			this.StopMovement();
			return;
		}
		if (this.aggro != null)
		{
			if (this.IsInRange(this.aggro, 3f))
			{
				this.StopMovement();
				this.movable.SetDirection(this.GetTargetDirection(this.aggro));
				this.AttackStart();
			}
			else
			{
				this.Chase();
			}
		}
		else
		{
			UnitModel unitModel = null;
			if (this.isRoamer)
			{
				this.GetNearest(float.MaxValue, false);
			}
			else
			{
				this.GetNearestByDoor();
			}
			if (unitModel != null)
			{
				this.aggro = unitModel;
			}
			this.MakeMovement();
		}
	}

	// Token: 0x06002C4C RID: 11340 RVA: 0x0012F0A0 File Offset: 0x0012D2A0
	public override bool CanTakeDamage(UnitModel attacker, DamageInfo dmg)
	{
		if (attacker == null || dmg == null)
		{
			return base.CanTakeDamage(attacker, dmg);
		}
		if (this.aggro == null && this.IsHostile(attacker.GetMovableNode()))
		{
			this.aggro = attacker;
		}
		return base.CanTakeDamage(attacker, dmg);
	}

	// Token: 0x06002C4D RID: 11341 RVA: 0x0012F0F0 File Offset: 0x0012D2F0
	private void MakeMovement()
	{
		if (this.aggro != null)
		{
			this.Chase();
		}
		else
		{
			UnitModel nearestByDoor = this.GetNearestByDoor();
			if (nearestByDoor == null)
			{
				this.Roam();
			}
			else
			{
				this.aggro = nearestByDoor;
				this.Chase();
			}
		}
		this.animScript.OnMove();
	}

	// Token: 0x06002C4E RID: 11342 RVA: 0x0002B38B File Offset: 0x0002958B
	private void StopMovement()
	{
		this.movable.StopMoving();
		this.animScript.OnStop();
	}

	// Token: 0x06002C4F RID: 11343 RVA: 0x0012F144 File Offset: 0x0012D344
	private void Chase()
	{
		if (this.IsHostile(this.aggro.GetMovableNode()))
		{
			this.model.MoveToMovable(this.aggro.GetMovableNode());
		}
		else
		{
			this.StopMovement();
			this.aggro = this.GetNearestByDoor();
		}
	}

	// Token: 0x06002C50 RID: 11344 RVA: 0x0012F194 File Offset: 0x0012D394
	private void Roam()
	{
		if (this.movable.IsMoving() || this.movable.InElevator())
		{
			return;
		}
		MapNode mapNode;
		if (this.isRoamer)
		{
			mapNode = MapGraph.instance.GetCreatureRoamingPoint();
		}
		else
		{
			List<MapNode> list = new List<MapNode>(this.passage.GetNodeList());
			mapNode = list[UnityEngine.Random.Range(0, list.Count)];
		}
		this.model.MoveToNode(mapNode);
	}

	// Token: 0x06002C51 RID: 11345 RVA: 0x0012F210 File Offset: 0x0012D410
	private void AttackStart()
	{
		int attackType = 0;
		if (this.Prob(20))
		{
			attackType = 1;
			this.MakeSound("Atk2_base");
		}
		else
		{
			this.MakeSound("Atk1");
		}
		this.animScript.OnAttackStart(attackType);
	}

	// Token: 0x06002C52 RID: 11346 RVA: 0x0002B3A3 File Offset: 0x000295A3
	public void OnAttackEnd()
	{
		this.motionDelayTimer.StartTimer(YoungPrinceFriend.motionDelay);
	}

	// Token: 0x06002C53 RID: 11347 RVA: 0x0012F258 File Offset: 0x0012D458
	public void OnAttackDamageTimeCalled()
	{
		if (this.aggro != null && this.IsInRange(this.aggro, 3.5f) && this.IsInView(this.aggro))
		{
			this.aggro.TakeDamage(this.model, new DamageInfo(this.atkType, (float)YoungPrinceFriend.atkDmg));
			DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(this.aggro, this.atkType, this.model);
		}
	}

	// Token: 0x06002C54 RID: 11348 RVA: 0x0012F2D4 File Offset: 0x0012D4D4
	public void OnSmashDamageTimeCalled()
	{
		List<UnitModel> list = new List<UnitModel>(this.GetTargets(4f, true));
		foreach (UnitModel unitModel in list)
		{
			WorkerModel workerModel = unitModel as WorkerModel;
			unitModel.TakeDamage(this.model, new DamageInfo(this.smashType, (float)YoungPrinceFriend.smashDmg));
			DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(unitModel, this.smashType, this.model);
			if (workerModel != null && !workerModel.IsDead())
			{
				workerModel.AddUnitBuf(new YoungPrinceSlowBuf());
			}
		}
		this.MakeSound("Atk2_atk");
	}

	// Token: 0x06002C55 RID: 11349 RVA: 0x0002B3B5 File Offset: 0x000295B5
	public override bool OnAfterSuppressed()
	{
		this.MakeSound("Dead");
		this.Infest();
		this.sporeList = new List<YoungPrinceFriend.Spore>();
		return false;
	}

	// Token: 0x06002C56 RID: 11350 RVA: 0x0012F398 File Offset: 0x0012D598
	private void Infest()
	{
		List<UnitModel> targets = this.GetTargets(float.MaxValue, false);
		foreach (UnitModel unitModel in targets)
		{
			WorkerModel workerModel = unitModel as WorkerModel;
			if (workerModel != null)
			{
				if (!workerModel.IsDead())
				{
					if (!this.Prob(60))
					{
						workerModel.AddUnitBuf(new YoungPrinceSporeBuf(this.prince));
					}
				}
			}
		}
	}

	// Token: 0x06002C57 RID: 11351 RVA: 0x0012F43C File Offset: 0x0012D63C
	private bool SpreadSpore()
	{
		if (this.leftSpore == null)
		{
			long instanceId;
			this.nextSporeId = (instanceId = this.nextSporeId) + 1L;
			YoungPrinceFriend.Spore spore = new YoungPrinceFriend.Spore(instanceId, this.model.GetMovableNode());
			this.leftSpore = spore;
			this.rightSpore = spore;
			this.sporeList.Add(spore);
			Notice.instance.Send(NoticeName.AddEtcUnit, new object[]
			{
				spore
			});
			return true;
		}
		bool flag = true;
		MovableObjectNode sideMovableNode = this.leftSpore.GetMovableNode().GetSideMovableNode(UnitDirection.LEFT, 0.75f);
		MovableObjectNode sideMovableNode2 = this.rightSpore.GetMovableNode().GetSideMovableNode(UnitDirection.RIGHT, 0.75f);
		if (sideMovableNode.currentPassage == this.currentPassage && MovableObjectNode.GetDistance(sideMovableNode, this.leftSpore.GetMovableNode()) > 0.4f)
		{
			long instanceId;
			this.nextSporeId = (instanceId = this.nextSporeId) + 1L;
			YoungPrinceFriend.Spore spore2 = new YoungPrinceFriend.Spore(instanceId, sideMovableNode);
			this.leftSpore = spore2;
			this.sporeList.Add(spore2);
			Notice.instance.Send(NoticeName.AddEtcUnit, new object[]
			{
				spore2
			});
			flag = false;
		}
		if (sideMovableNode2.currentPassage == this.currentPassage && MovableObjectNode.GetDistance(sideMovableNode2, this.rightSpore.GetMovableNode()) > 0.4f)
		{
			long instanceId;
			this.nextSporeId = (instanceId = this.nextSporeId) + 1L;
			YoungPrinceFriend.Spore spore3 = new YoungPrinceFriend.Spore(instanceId, sideMovableNode2);
			this.rightSpore = spore3;
			this.sporeList.Add(spore3);
			Notice.instance.Send(NoticeName.AddEtcUnit, new object[]
			{
				spore3
			});
			flag = false;
		}
		return !flag;
	}

	// Token: 0x06002C58 RID: 11352 RVA: 0x0002B3D5 File Offset: 0x000295D5
	public string GetSoundSrc(string key)
	{
		return "creature/YoungPrince/Friend/Prince_Child_" + key;
	}

	// Token: 0x06002C59 RID: 11353 RVA: 0x0012F5D8 File Offset: 0x0012D7D8
	public override SoundEffectPlayer MakeSound(string src)
	{
		string soundSrc = this.GetSoundSrc(src);
		if (soundSrc == string.Empty)
		{
			return null;
		}
		if (src != null)
		{
			if (src == "Dead" || src == "Atk1" || src == "Atk2_base" || src == "Atk2_atk")
			{
				return SoundEffectPlayer.PlayOnce(soundSrc, this.model.Unit.transform.position, 1f);
			}
			if (src == "Default")
			{
				this.effects[0] = SoundEffectPlayer.Play(soundSrc, this.model.Unit.transform, 1f);
				return this.effects[0];
			}
			if (src == "Walk")
			{
				this.effects[1] = SoundEffectPlayer.Play(soundSrc, this.model.Unit.transform, 1f);
				return this.effects[1];
			}
		}
		return null;
	}

	// Token: 0x06002C5A RID: 11354 RVA: 0x0012F6EC File Offset: 0x0012D8EC
	private UnitModel GetNearestByDoor()
	{
		List<UnitModel> targets = this.GetTargets(float.MaxValue, false);
		UnitModel result = null;
		float num = 10000f;
		foreach (UnitModel unitModel in targets)
		{
			float x = unitModel.GetCurrentViewPosition().x;
			float x2 = this.prince.model.GetEntryNode().GetPosition().x;
			float num2 = Math.Abs(x - x2);
			if (num2 < num)
			{
				result = unitModel;
				num = num2;
			}
		}
		return result;
	}

	// Token: 0x06002C5B RID: 11355 RVA: 0x0012F7A0 File Offset: 0x0012D9A0
	private UnitModel GetNearest(float range, bool needDir = true)
	{
		List<UnitModel> targets = this.GetTargets(range, needDir);
		UnitModel result = null;
		float num = 10000f;
		foreach (UnitModel unitModel in targets)
		{
			float distance = this.GetDistance(unitModel);
			if (distance < num)
			{
				result = unitModel;
				num = distance;
			}
		}
		return result;
	}

	// Token: 0x06002C5C RID: 11356 RVA: 0x0012F81C File Offset: 0x0012DA1C
	private List<UnitModel> GetTargets(float range, bool needDir = true)
	{
		List<UnitModel> list = new List<UnitModel>();
		if (this.currentPassage == null)
		{
			return list;
		}
		foreach (MovableObjectNode movableObjectNode in this.currentPassage.GetEnteredTargets(this.movable))
		{
			UnitModel unit = movableObjectNode.GetUnit();
			if (this.IsHostile(movableObjectNode))
			{
				if (this.IsInRange(unit, range))
				{
					if (!needDir || this.IsInView(unit))
					{
						list.Add(unit);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06002C5D RID: 11357 RVA: 0x0012F8B4 File Offset: 0x0012DAB4
	private bool IsInRange(UnitModel target, float range)
	{
		float distance = this.GetDistance(target);
		return range >= distance;
	}

	// Token: 0x06002C5E RID: 11358 RVA: 0x000FCC44 File Offset: 0x000FAE44
	private bool IsInView(UnitModel target)
	{
		float x = this.movable.GetCurrentViewPosition().x;
		float x2 = target.GetCurrentViewPosition().x;
		if (this.model.GetDirection() == UnitDirection.LEFT)
		{
			if (x < x2)
			{
				return false;
			}
		}
		else if (this.model.GetDirection() == UnitDirection.RIGHT && x > x2)
		{
			return false;
		}
		return true;
	}

	// Token: 0x06002C5F RID: 11359 RVA: 0x0012F8D4 File Offset: 0x0012DAD4
	private float GetDistance(UnitModel target)
	{
		float result;
		try
		{
			float currentScale = this.movable.currentScale;
			float x = this.movable.GetCurrentViewPosition().x;
			float x2 = target.GetCurrentViewPosition().x;
			float num = Math.Abs(x - x2);
			result = num / currentScale;
		}
		catch (Exception arg)
		{
			Debug.LogError("friend get distance error " + arg);
			result = float.MaxValue;
		}
		return result;
	}

	// Token: 0x06002C60 RID: 11360 RVA: 0x0012F958 File Offset: 0x0012DB58
	private bool IsHostile(MovableObjectNode mov)
	{
		UnitModel unit = mov.GetUnit();
		CreatureModel creatureModel = unit as CreatureModel;
		WorkerModel workerModel = unit as WorkerModel;
		return this.IsAttackable(unit) && unit.IsAttackTargetable() && (creatureModel == null || !(creatureModel.script is YoungPrinceFriend)) && (workerModel == null || workerModel.unconAction == null || !(workerModel.unconAction is Uncontrollable_YoungPrince));
	}

	// Token: 0x06002C61 RID: 11361 RVA: 0x0012F9D4 File Offset: 0x0012DBD4
	private bool IsAttackable(UnitModel unit)
	{
		try
		{
			if (unit == null)
			{
				return false;
			}
			if (unit.hp <= 0f)
			{
				return false;
			}
			if (unit.GetMovableNode().currentPassage == null)
			{
				return false;
			}
			if (unit.GetMovableNode().currentPassage != this.passage)
			{
				return false;
			}
		}
		catch (Exception ex)
		{
			return false;
		}
		return true;
	}

	// Token: 0x06002C62 RID: 11362 RVA: 0x0012FA5C File Offset: 0x0012DC5C
	private UnitDirection GetTargetDirection(UnitModel target)
	{
		UnitDirection unitDirection = this.model.GetDirection();
		if (!this.IsInView(target))
		{
			unitDirection = ((unitDirection != UnitDirection.LEFT) ? UnitDirection.LEFT : UnitDirection.RIGHT);
		}
		return unitDirection;
	}

	// Token: 0x06002C63 RID: 11363 RVA: 0x0002B3E2 File Offset: 0x000295E2
	public override string GetName()
	{
		return this.name;
	}

	// Token: 0x06002C64 RID: 11364 RVA: 0x0002B3EA File Offset: 0x000295EA
	private bool IsAttacking()
	{
		return this.animScript.IsAttacking();
	}

	// Token: 0x040029E9 RID: 10729
	private Timer moveTimer = new Timer();

	// Token: 0x040029EA RID: 10730
	private const float _moveFreqMax = 2f;

	// Token: 0x040029EB RID: 10731
	private const float _moveFreqMin = 1f;

	// Token: 0x040029EC RID: 10732
	private const float escapeDelay = 2.5f;

	// Token: 0x040029ED RID: 10733
	private Timer spreadTimer = new Timer();

	// Token: 0x040029EE RID: 10734
	private const float spreadFreq = 0.2f;

	// Token: 0x040029EF RID: 10735
	private Timer motionDelayTimer = new Timer();

	// Token: 0x040029F0 RID: 10736
	private const float _motionDelayMax = 1.2f;

	// Token: 0x040029F1 RID: 10737
	private const float _motionDelayMin = 0.8f;

	// Token: 0x040029F2 RID: 10738
	private const int _atkDmgMax = 4;

	// Token: 0x040029F3 RID: 10739
	private const int _atkDmgMin = 6;

	// Token: 0x040029F4 RID: 10740
	private RwbpType atkType = RwbpType.B;

	// Token: 0x040029F5 RID: 10741
	private const int _smashDmgMax = 7;

	// Token: 0x040029F6 RID: 10742
	private const int _smashDmgMin = 10;

	// Token: 0x040029F7 RID: 10743
	private RwbpType smashType = RwbpType.B;

	// Token: 0x040029F8 RID: 10744
	private const int _smashProb = 20;

	// Token: 0x040029F9 RID: 10745
	private const int _infestProb = 40;

	// Token: 0x040029FA RID: 10746
	private const float _attackRange = 3f;

	// Token: 0x040029FB RID: 10747
	private const float _atkDmgRange = 3.5f;

	// Token: 0x040029FC RID: 10748
	private const float _smashDmgRange = 4f;

	// Token: 0x040029FD RID: 10749
	private const float volume = 1f;

	// Token: 0x040029FE RID: 10750
	public const string soundPath = "creature/YoungPrince/Friend/Prince_Child_";

	// Token: 0x040029FF RID: 10751
	public const string sound_Default = "Default";

	// Token: 0x04002A00 RID: 10752
	public const string sound_Walk = "Walk";

	// Token: 0x04002A01 RID: 10753
	public const string sound_Atk1 = "Atk1";

	// Token: 0x04002A02 RID: 10754
	public const string sound_Atk2_Base = "Atk2_base";

	// Token: 0x04002A03 RID: 10755
	public const string sound_Atk2_Hit = "Atk2_atk";

	// Token: 0x04002A04 RID: 10756
	public const string sound_Dead = "Dead";

	// Token: 0x04002A05 RID: 10757
	public YoungPrince prince;

	// Token: 0x04002A06 RID: 10758
	private PassageObjectModel passage;

	// Token: 0x04002A07 RID: 10759
	public SoundEffectPlayer[] effects = new SoundEffectPlayer[2];

	// Token: 0x04002A08 RID: 10760
	private List<YoungPrinceFriend.Spore> sporeList = new List<YoungPrinceFriend.Spore>();

	// Token: 0x04002A09 RID: 10761
	public long nextSporeId = 10000L;

	// Token: 0x04002A0A RID: 10762
	public bool isMovable;

	// Token: 0x04002A0B RID: 10763
	private bool isRoamer;

	// Token: 0x04002A0C RID: 10764
	private bool escapeInit;

	// Token: 0x04002A0D RID: 10765
	public string name = string.Empty;

	// Token: 0x04002A0E RID: 10766
	private YoungPrinceFriend.Spore leftSpore;

	// Token: 0x04002A0F RID: 10767
	private YoungPrinceFriend.Spore rightSpore;

	// Token: 0x04002A10 RID: 10768
	private UnitModel aggro;

	// Token: 0x04002A11 RID: 10769
	private YoungPrinceFriendAnim _animScript;

	// Token: 0x020004B7 RID: 1207
	public class Spore : UnitModel
	{
		// Token: 0x06002C65 RID: 11365 RVA: 0x00029FDB File Offset: 0x000281DB
		public Spore(long instanceId, MovableObjectNode m)
		{
			this.instanceId = instanceId;
			this.movableNode = new MovableObjectNode(false);
			this.movableNode.Assign(m);
			this.movableNode.StopMoving();
		}

		// <Mod>
		public override bool IsEtcUnit()
		{
			return true;
		}
	}
}
