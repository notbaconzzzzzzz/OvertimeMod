using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003D4 RID: 980
public class CircusDawn : CircusOrdealCreature
{
	// Token: 0x06001FD4 RID: 8148 RVA: 0x00021A9D File Offset: 0x0001FC9D
	public CircusDawn()
	{
	}

	// Token: 0x170002B1 RID: 689
	// (get) Token: 0x06001FD5 RID: 8149 RVA: 0x00021ABB File Offset: 0x0001FCBB
	private float TeleportDelay
	{
		get
		{
			return UnityEngine.Random.Range(13f, 15f);
		}
	}

	// Token: 0x06001FD6 RID: 8150 RVA: 0x000204D9 File Offset: 0x0001E6D9
	public override void OnInit()
	{
		base.OnInit();
	}

	// Token: 0x06001FD7 RID: 8151 RVA: 0x000FA29C File Offset: 0x000F849C
	public override void OnFixedUpdate(CreatureModel creature)
	{
		if (this.model.hp <= 0f)
		{
			return;
		}
		if (this._isSuppressed)
		{
			return;
		}
		if (!this._readyToMove)
		{
			return;
		}
		if (this._isMoving)
		{
			return;
		}
		if (this._trickCastTimer.started)
		{
			if (this._trickCastTimer.RunTimer())
			{
				this.TrickSuccess();
				this.animScript.StopTrick();
			}
		}
		else if (this._readyForTeleport && !this._isTeleporting)
		{
			this.ReadyForTeleport();
		}
		if (!this._readyForTeleport)
		{
			if (!this._teleportDelayTimer.started)
			{
				this._teleportDelayTimer.StartTimer(this.TeleportDelay);
			}
			else if (this._teleportDelayTimer.RunTimer())
			{
				this._readyForTeleport = true;
			}
		}
	}

	// Token: 0x06001FD8 RID: 8152 RVA: 0x000FA380 File Offset: 0x000F8580
	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
		this.animScript = (unit.animTarget as CircusDawnAnim);
		this.animScript.SetScript(this);
		if (this._ordealScript is CircusDawnOrdeal)
		{
			if (this.currentPassage != null)
			{
				List<MapNode> list = new List<MapNode>(this.currentPassage.GetNodeList());
				for (int i = list.Count - 1; i >= 0; i--)
				{
					if (list[i].connectedCreature == null)
					{
						list.RemoveAt(i);
					}
				}
				if (list.Count > 0)
				{
					this._currentWaitingCreature = list[UnityEngine.Random.Range(0, list.Count)].connectedCreature;
					this.EndTelport();
				}
				else
				{
					this.ReadyForTeleport();
				}
			}
		}
		else
		{
			this.ReadyForTeleport();
		}
		unit.escapeRisk.text = this._ordealScript.GetRiskLevel(this.model as OrdealCreatureModel).ToString().ToUpper();
		unit.escapeRisk.color = UIColorManager.instance.GetRiskColor(this._ordealScript.GetRiskLevel(this.model as OrdealCreatureModel));
	}

	// Token: 0x06001FD9 RID: 8153 RVA: 0x000FA4B4 File Offset: 0x000F86B4
	public void ReadyForTeleport()
	{
		if (!this.CanReadyForTeleport())
		{
			MapNode teleportNode = this.GetTeleportNode();
			this._currentTeleportNode = teleportNode;
		}
		this._isTeleporting = true;
		this.animScript.StartTeleport();
	}

	// Token: 0x06001FDA RID: 8154 RVA: 0x000FA4F0 File Offset: 0x000F86F0
	public void Teleport()
	{
		Debug.Log("TELEPORT");
		this.model.GetMovableNode().SetCurrentNode(this._currentTeleportNode);
		Sefira sefira = SefiraManager.instance.GetSefira(this._currentTeleportNode.GetAttachedPassage().GetSefiraName());
		this.model.sefira = sefira;
		this.model.sefiraNum = sefira.indexString;
	}

	// Token: 0x06001FDB RID: 8155 RVA: 0x000FA558 File Offset: 0x000F8758
	public void EndTelport()
	{
		CreatureModel currentWaitingCreature = this._currentWaitingCreature;
		if (currentWaitingCreature == null)
		{
			return;
		}
		this.model.MoveToNode(currentWaitingCreature.GetEntryNode());
		this._currentTrickCreature = currentWaitingCreature;
		CreatureCommand currentCommand = this.model.GetCurrentCommand();
		if (currentCommand is MoveCreatureCommand)
		{
			(currentCommand as MoveCreatureCommand).SetEndCommand(new CreatureCommand.OnCommandEnd(this.OnArriveIsolateRoom));
			this._isMoving = true;
		}
		this._readyForTeleport = false;
		if (!this._readyToMove)
		{
			this._readyToMove = true;
		}
		this._isTeleporting = false;
	}

	// Token: 0x06001FDC RID: 8156 RVA: 0x00021ACC File Offset: 0x0001FCCC
	private void OnArriveIsolateRoom()
	{
		Debug.Log("ARRIVED");
		this._trickCastTimer.StartTimer(20f);
		this.animScript.StartTrick();
		this._isMoving = false;
		this.MakeTrickEffect();
	}

	// Token: 0x06001FDD RID: 8157 RVA: 0x000FA5E0 File Offset: 0x000F87E0
	private void TrickSuccess()
	{
		Debug.Log("TRICKSUCCESS");
		this.MakeTrickSuccessEffect();
		this.RemoveTrickEffect();
		if (!this._currentTrickCreature.IsEscaped() && this._currentTrickCreature.script.HasRoomCounter())
		{
			this._currentTrickCreature.SubQliphothCounter();
		}
		int count = (int)((float)this._currentTrickCreature.observeInfo.cubeNum * 0.3f);
		this._currentTrickCreature.SubCreatureSuccessCube(count);
	}

	// Token: 0x06001FDE RID: 8158 RVA: 0x000FA658 File Offset: 0x000F8858
	public void Explosion()
	{
		PassageObjectModel passage = this.model.GetMovableNode().GetPassage();
		if (passage == null)
		{
			return;
		}
		List<UnitModel> list = new List<UnitModel>();
		foreach (MovableObjectNode movableObjectNode in passage.GetEnteredTargets())
		{
			UnitModel unit = movableObjectNode.GetUnit();
			if (unit != this.model)
			{
				if (unit.IsAttackTargetable())
				{
					if (!list.Contains(unit))
					{
						if (movableObjectNode.GetDistanceDouble(this.model.GetMovableNode()) <= 16f)
						{
							list.Add(unit);
						}
					}
				}
			}
		}
		DamageInfo dmg = new DamageInfo(RwbpType.R, 10, 15);
		foreach (UnitModel unitModel in list)
		{
			unitModel.TakeDamage(this.model, dmg);
			DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(unitModel, RwbpType.R, this.model);
		}
		this.model.Unit.gameObject.SetActive(false);
		ExplodeGutEffect explodeGutEffect = null;
		if (ExplodeGutManager.instance.MakeEffects(this.model.GetMovableNode().GetCurrentViewPosition(), ref explodeGutEffect))
		{
			explodeGutEffect.particleCount = UnityEngine.Random.Range(5, 9);
			explodeGutEffect.ground = this.model.GetMovableNode().GetCurrentViewPosition().y;
			float num = 0f;
			float num2 = 0f;
			explodeGutEffect.SetEffectSize(1f);
			explodeGutEffect.Shoot(ExplodeGutEffect.Directional.CENTRAL, null);
			if (this.model.GetMovableNode().GetPassage() != null)
			{
				this.model.GetMovableNode().GetPassage().GetVerticalRange(ref num, ref num2);
				explodeGutEffect.SetCurrentPassage(this.model.GetMovableNode().GetPassage());
			}
		}
	}

	// Token: 0x06001FDF RID: 8159 RVA: 0x000FA86C File Offset: 0x000F8A6C
	public override bool OnAfterSuppressed()
	{
		this._isSuppressed = true;
		try
		{
			this.OnDie();
			this.RemoveTrickEffect();
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
		return base.OnAfterSuppressed();
	}

	// Token: 0x06001FE0 RID: 8160 RVA: 0x000FA8B4 File Offset: 0x000F8AB4
	private void MakeTrickSuccessEffect()
	{
		Debug.Log("try to make successEffect");
		GameObject gameObject = Prefab.LoadPrefab("Effect/Creature/CircusDawn/PEBoxDownSuccess");
		gameObject.transform.SetParent(this._currentTrickCreature.Unit.room.CumlativeCubeCount.transform);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.rotation = Quaternion.identity;
	}

	// Token: 0x06001FE1 RID: 8161 RVA: 0x000FA92C File Offset: 0x000F8B2C
	private void MakeTrickEffect()
	{
		if (this._peBoxDownEffect == null)
		{
			this._peBoxDownEffect = Prefab.LoadPrefab("Effect/Creature/CircusDawn/PEBoxDown");
			this._peBoxDownEffect.transform.SetParent(this._currentTrickCreature.Unit.room.CumlativeCubeCount.transform);
			this._peBoxDownEffect.transform.localPosition = Vector3.zero;
			this._peBoxDownEffect.transform.localScale = Vector3.one;
			this._peBoxDownEffect.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
		}
	}

	// Token: 0x06001FE2 RID: 8162 RVA: 0x000FA9D4 File Offset: 0x000F8BD4
	public void RemoveTrickEffect()
	{
		Debug.Log("try to remove Effect");
		if (this._peBoxDownEffect != null && this._peBoxDownEffect.activeInHierarchy)
		{
			Debug.Log("remove Effect");
			try
			{
				UnityEngine.Object.Destroy(this._peBoxDownEffect.gameObject);
				this._peBoxDownEffect = null;
			}
			catch (MissingReferenceException message)
			{
				Debug.Log(message);
			}
		}
	}

	// Token: 0x06001FE3 RID: 8163 RVA: 0x00021B00 File Offset: 0x0001FD00
	private void AddWaitingTarget(CreatureModel target)
	{
		this._currentWaitingCreature = target;
	}

	// Token: 0x06001FE4 RID: 8164 RVA: 0x000FAA50 File Offset: 0x000F8C50
	public MapNode GetTeleportNode()
	{
		MapNode result = null;
		if (this._currentWaitingCreature != null)
		{
			MapNode entryNode = this._currentWaitingCreature.GetEntryNode();
			PassageObjectModel attachedPassage = entryNode.GetAttachedPassage();
			if (attachedPassage != null)
			{
				MapNode[] nodeList = attachedPassage.GetNodeList();
				result = nodeList[UnityEngine.Random.Range(0, nodeList.Length)];
			}
		}
		return result;
	}

	// Token: 0x06001FE5 RID: 8165 RVA: 0x000FAA98 File Offset: 0x000F8C98
	public CreatureModel GetTarget(out bool hasError)
	{
		List<CreatureModel> list = new List<CreatureModel>(CreatureManager.instance.GetCreatureList());
		List<CreatureModel> list2 = new List<CreatureModel>();
		hasError = false;
		if (list.Count > 1 && this._currentWaitingCreature != null && list.Contains(this._currentWaitingCreature))
		{
			list.Remove(this._currentWaitingCreature);
		}
		foreach (CreatureModel creatureModel in list)
		{
			if (!creatureModel.IsEscaped() && creatureModel.script.HasRoomCounter() && creatureModel.qliphothCounter > 0)
			{
				list2.Add(creatureModel);
			}
		}
		if (list2.Count == 0)
		{
			foreach (CreatureModel creatureModel2 in list)
			{
				if (!creatureModel2.IsEscaped())
				{
					list2.Add(creatureModel2);
				}
			}
		}
		if (list2.Count == 0)
		{
			hasError = true;
			list2 = new List<CreatureModel>(list);
		}
		try
		{
			return list2[UnityEngine.Random.Range(0, list2.Count)];
		}
		catch (IndexOutOfRangeException ex)
		{
		}
		return null;
	}

	// Token: 0x06001FE6 RID: 8166 RVA: 0x000FAC0C File Offset: 0x000F8E0C
	public bool CanReadyForTeleport()
	{
		bool result = false;
		CreatureModel target = this.GetTarget(out result);
		if (target != null)
		{
			this.AddWaitingTarget(target);
		}
		return result;
	}

	// Token: 0x0400201B RID: 8219
	private const float _reduceRate = 0.3f;

	// Token: 0x0400201C RID: 8220
	private const float _teleportDelayMin = 13f;

	// Token: 0x0400201D RID: 8221
	private const float _teleportDelayMax = 15f;

	// Token: 0x0400201E RID: 8222
	private const float _trickTime = 20f;

	// Token: 0x0400201F RID: 8223
	private const string _peBoxDownEffectSrc = "Effect/Creature/CircusDawn/PEBoxDown";

	// Token: 0x04002020 RID: 8224
	private const string _peBoxDownSuccessEffectSrc = "Effect/Creature/CircusDawn/PEBoxDownSuccess";

	// Token: 0x04002021 RID: 8225
	private Timer _teleportDelayTimer = new Timer();

	// Token: 0x04002022 RID: 8226
	private bool _readyForTeleport;

	// Token: 0x04002023 RID: 8227
	private GameObject _peBoxDownEffect;

	// Token: 0x04002024 RID: 8228
	private CreatureModel _currentTrickCreature;

	// Token: 0x04002025 RID: 8229
	private MapNode _currentTeleportNode;

	// Token: 0x04002026 RID: 8230
	private Timer _trickCastTimer = new Timer();

	// Token: 0x04002027 RID: 8231
	private CreatureModel _currentWaitingCreature;

	// Token: 0x04002028 RID: 8232
	private CircusDawnAnim animScript;

	// Token: 0x04002029 RID: 8233
	private CircusDawn.State _state;

	// Token: 0x0400202A RID: 8234
	private bool _readyToMove;

	// Token: 0x0400202B RID: 8235
	private bool _isTeleporting;

	// Token: 0x0400202C RID: 8236
	private bool _isMoving;

	// Token: 0x0400202D RID: 8237
	private bool _isSuppressed;

	// Token: 0x0400202E RID: 8238
	private const int _explosionDamageMin = 10;

	// Token: 0x0400202F RID: 8239
	private const int _explosionDamageMax = 15;

	// Token: 0x04002030 RID: 8240
	private const float _explosionRange = 4f;

	// Token: 0x020003D5 RID: 981
	public enum State
	{
		// Token: 0x04002032 RID: 8242
		IDLE,
		// Token: 0x04002033 RID: 8243
		MOVE,
		// Token: 0x04002034 RID: 8244
		TRICK,
		// Token: 0x04002035 RID: 8245
		TELEPORT
	}
}
