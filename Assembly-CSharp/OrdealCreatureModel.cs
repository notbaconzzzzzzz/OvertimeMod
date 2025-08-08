using System;
using UnityEngine;

// Token: 0x020006CE RID: 1742
public class OrdealCreatureModel : CreatureModel
{
	// Token: 0x0600381B RID: 14363 RVA: 0x00032536 File Offset: 0x00030736
	public OrdealCreatureModel(long instanceId) : base(instanceId)
	{
		base.state = CreatureState.ESCAPE;
		this.SetFaction(FactionTypeList.StandardFaction.EscapedCreature);
	}

	// Token: 0x1700052D RID: 1325
	// (get) Token: 0x0600381C RID: 14364 RVA: 0x00032551 File Offset: 0x00030751
	// (set) Token: 0x0600381D RID: 14365 RVA: 0x00032559 File Offset: 0x00030759
	public OrdealBase OrdealBase
	{
		get
		{
			return this._ordealBase;
		}
		private set
		{
			this._ordealBase = value;
		}
	}

	// Token: 0x0600381E RID: 14366 RVA: 0x00032562 File Offset: 0x00030762
	public void SetOrdealBase(OrdealBase ordeal)
	{
		this.OrdealBase = ordeal;
	}

	// Token: 0x0600381F RID: 14367 RVA: 0x0003256B File Offset: 0x0003076B
	public override void Escape()
	{
		Debug.LogError("OrdealCreatureModel don't use Escape()");
	}

	// Token: 0x06003820 RID: 14368 RVA: 0x00032577 File Offset: 0x00030777
	public override void OnStageStart()
	{
		Debug.LogError("OrdealCreatureModel don't use OnStageStart()");
	}

	// Token: 0x06003821 RID: 14369 RVA: 0x00032583 File Offset: 0x00030783
	public override void OnStageRelease()
	{
		Debug.LogError("OrdealCreatureModel don't use OnStageRelease()");
	}

	// Token: 0x06003822 RID: 14370 RVA: 0x0003258F File Offset: 0x0003078F
	public void OnDestroy()
	{
		base.ClearCommand();
		this.GetMovableNode().SetActive(false);
	}

	// Token: 0x06003823 RID: 14371 RVA: 0x0016AFB0 File Offset: 0x001691B0
	public override void OnFixedUpdate()
	{
		this.UpdateBufState();
		this.tempAnim.OnFixedUpdate();
		if (this._equipment.weapon != null)
		{
			this._equipment.weapon.OnFixedUpdate();
		}
		this.commandQueue.Execute(this);
		if (!this.script.UniqueMoveControl())
		{
			if (this.GetMovableNode().IsMoving())
			{
				this.SetMoveAnimState(true);
			}
			else if (this._unit != null && this._unit.animTarget != null)
			{
				this._unit.animTarget.StopMoving();
			}
		}
		if (GameManager.currentGameManager.ManageStarted)
		{
			this.script.OnFixedUpdate(this);
		}
		this.movableNode.ProcessMoveNode(this.metaInfo.speed * this.movementScale);
	}

	// Token: 0x06003824 RID: 14372 RVA: 0x000325A3 File Offset: 0x000307A3
	public override bool IsHostile(UnitModel target)
	{
		return target is WorkerModel || (!(target is OrdealCreatureModel) && target is CreatureModel && (target as CreatureModel).script.IsAutoSuppressable());
	}

	// Token: 0x04003360 RID: 13152
	private OrdealBase _ordealBase;
}
