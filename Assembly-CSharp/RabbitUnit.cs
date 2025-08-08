using System;
using System.Collections.Generic;
using InGameUI;
using Rabbit;
using UnityEngine;

// Token: 0x02000B20 RID: 2848
public class RabbitUnit : MonoBehaviour, IMouseOnSelectListener, IMouseCommandTarget
{
	// Token: 0x06005606 RID: 22022 RVA: 0x00045592 File Offset: 0x00043792
	public RabbitUnit()
	{
	}

	// Token: 0x170007FC RID: 2044
	// (get) Token: 0x06005607 RID: 22023 RVA: 0x000455A2 File Offset: 0x000437A2
	private RabbitLayer Layer
	{
		get
		{
			return base.transform.parent.GetComponent<RabbitLayer>();
		}
	}

	// Token: 0x06005608 RID: 22024 RVA: 0x001ECD58 File Offset: 0x001EAF58
	private void Start()
	{
		this.animEventHandler.SetDamageEvent(new AnimatorEventHandler.EventDelegate(this.model.OnGiveDamage));
		this.animEventHandler.SetAttackEndEvent(new AnimatorEventHandler.EventDelegate(this.model.OnEndCycle));
		this.animEventHandler.SetAnimEvent(new AnimatorEventHandler.AnimEventDelegate(this.model.OnAnimEventCalled));
		if (this.model != null)
		{
			this.animController.SetMovementAnimSpeed(this.model.baseMovement / 3f);
		}
		this.ui.activateUI(this.model);
		this.agentUI.Init(this.model);
		int num = UnityEngine.Random.Range(0, this.faceSprites.Length);
		for (int i = 0; i < this.faceSprites.Length; i++)
		{
			bool active = false;
			if (i == num)
			{
				active = true;
			}
			this.faceSprites[i].gameObject.SetActive(active);
		}
		float x = UnityEngine.Random.Range(-0.2f, 0.2f);
		float y = UnityEngine.Random.Range(0f, -0.2f);
		this.animRoot.localPosition = new Vector3(x, y, 0f);
		this.animController.anim.speed = UnityEngine.Random.Range(0.95f, 1.05f);
		this.speech.Init(this);
		this.Speech(RabbitConversationType.START, 1f, 3f);
	}

	// Token: 0x06005609 RID: 22025 RVA: 0x001ECEC0 File Offset: 0x001EB0C0
	public void SetModel(RabbitModel m)
	{
		this.model = m;
		this.animController.SetMovementAnimSpeed(m.baseMovement / 3f);
		this.rwbpType = m.rwbpType;
		this.rwbpSet = this.Layer.GetRwbpSet(this.rwbpType);
		this.spriteSetter.Apply(this.rwbpSet.sword, this.rwbpSet.gauge);
		foreach (ParticleSystem particleSystem in this.particle)
		{
			ParticleSystem.MainModule main = particleSystem.main;
			ParticleSystem.MinMaxGradient startColor = main.startColor;
			startColor.color = UIColorManager.instance.GetRWBPTypeColor(this.rwbpType);
			main.startColor = startColor;
		}
		float currentScale = this.model.GetMovableNode().currentScale;
		Vector3 localScale = new Vector3(1f, 1f, 1f) * currentScale;
		Vector3 euler = new Vector3(0f, 0f, 0f);
		GameObject gameObject = Prefab.LoadPrefab("Effect/Rabbit/RabbitAppear");
		gameObject.transform.SetParent(EffectLayer.currentLayer.transform);
		gameObject.transform.position = this.model.GetMovableNode().GetCurrentViewPosition();
		gameObject.transform.localRotation = Quaternion.Euler(euler);
		gameObject.transform.localScale = localScale;
		this.model.MakeSound("Rabbit/RabbitTeam_Teleport");
	}

	// Token: 0x0600560A RID: 22026 RVA: 0x000455B4 File Offset: 0x000437B4
	public void SetDefaultZValue(float value)
	{
		this.zValue = value;
	}

	// Token: 0x0600560B RID: 22027 RVA: 0x00013D75 File Offset: 0x00011F75
	public bool IsSelectable()
	{
		return true;
	}

	// Token: 0x0600560C RID: 22028 RVA: 0x000040A1 File Offset: 0x000022A1
	public void OnSelect()
	{
	}

	// Token: 0x0600560D RID: 22029 RVA: 0x000040A1 File Offset: 0x000022A1
	public void OnUnselect()
	{
	}

	// Token: 0x0600560E RID: 22030 RVA: 0x000455BD File Offset: 0x000437BD
	public IMouseCommandTargetModel GetCommandTargetModel()
	{
		return this.model;
	}

	// Token: 0x0600560F RID: 22031 RVA: 0x000455C5 File Offset: 0x000437C5
	private void Update()
	{
		this.UpdateViewPosition();
		this.UpdateDirection();
	}

	// Token: 0x06005610 RID: 22032 RVA: 0x000455D3 File Offset: 0x000437D3
	public void FixedUpdate()
	{
		this.speech.Execute();
		if (this.model.GetMovableNode().IsMoving())
		{
			this.animController.SetMove(true);
		}
		else
		{
			this.animController.SetMove(false);
		}
	}

	// Token: 0x06005611 RID: 22033 RVA: 0x00045612 File Offset: 0x00043812
	public void Speech(RabbitConversationType type, float speechProb = 0.6f, float speechTime = 3f)
	{
		if (type < this.currentSpeechType)
		{
			this.currentSpeechType = type;
			this.speech.SetSpeechProb(speechProb);
			this.speech.Speech(RabbitConversationText.GetText(type), speechTime);
		}
	}

	// Token: 0x06005612 RID: 22034 RVA: 0x00045645 File Offset: 0x00043845
	public void OnSpeechEnd()
	{
		this.currentSpeechType = RabbitConversationType.NONE;
	}

	// Token: 0x06005613 RID: 22035 RVA: 0x001ED05C File Offset: 0x001EB25C
	protected void UpdateDirection()
	{
		MovableObjectNode movableNode = this.model.GetMovableNode();
		UnitDirection direction = movableNode.GetDirection();
		Vector3 localScale = this.animRoot.localScale;
		if (direction == UnitDirection.RIGHT)
		{
			if (localScale.x > 0f)
			{
				localScale.x = -localScale.x;
			}
		}
		else if (localScale.x < 0f)
		{
			localScale.x = -localScale.x;
		}
		this.animRoot.localScale = localScale;
	}

	// Token: 0x06005614 RID: 22036 RVA: 0x001ED0E0 File Offset: 0x001EB2E0
	protected void UpdateViewPosition()
	{
		base.transform.localScale = new Vector3(this.model.GetMovableNode().currentScale, this.model.GetMovableNode().currentScale, base.transform.localScale.z);
		MapEdge currentEdge = this.model.GetMovableNode().GetCurrentEdge();
		if (currentEdge != null && currentEdge.type == "door")
		{
			Vector3 currentViewPosition = this.model.GetCurrentViewPosition();
			currentViewPosition.z = 100000f;
			base.transform.localPosition = currentViewPosition;
		}
		else
		{
			Vector3 currentViewPosition2 = this.model.GetCurrentViewPosition();
			if (!this.model.GetMovableNode().isIgnoreZValue)
			{
				currentViewPosition2.z += this.zValue;
			}
			base.transform.localPosition = currentViewPosition2;
		}
	}

	// Token: 0x06005615 RID: 22037 RVA: 0x0004564F File Offset: 0x0004384F
	public void OnDie()
	{
		this.animController.Dead();
	}

	// Token: 0x06005616 RID: 22038 RVA: 0x0004565C File Offset: 0x0004385C
	public void OnDieByMental()
	{
		this.animController.DeadByMental();
	}

	// Token: 0x06005617 RID: 22039 RVA: 0x00045669 File Offset: 0x00043869
	public void Attack(int index)
	{
		this.animController.StartAttack(index);
	}

	// Token: 0x06005618 RID: 22040 RVA: 0x00045677 File Offset: 0x00043877
	public void OnClear()
	{
		this.animController.Clear();
	}

	// Token: 0x06005619 RID: 22041 RVA: 0x00045684 File Offset: 0x00043884
	public void Fire()
	{
		this.animController.Fire();
	}

	// Token: 0x0600561A RID: 22042 RVA: 0x00045691 File Offset: 0x00043891
	public void StopFire()
	{
		this.animController.StopFire();
	}

	// Token: 0x04004F88 RID: 20360
	private const string appearEffect = "Effect/Rabbit/RabbitAppear";

	// Token: 0x04004F89 RID: 20361
	public const string _Execute_src = "Effect/Bullet/RabbitFallback";

	// Token: 0x04004F8A RID: 20362
	public RabbitModel model;

	// Token: 0x04004F8B RID: 20363
	public RabbitAnimatorController animController;

	// Token: 0x04004F8C RID: 20364
	public AnimatorEventHandler animEventHandler;

	// Token: 0x04004F8D RID: 20365
	public RabbitSpriteSetter spriteSetter;

	// Token: 0x04004F8E RID: 20366
	public RabbitUnitUI ui;

	// Token: 0x04004F8F RID: 20367
	public AgentUI agentUI;

	// Token: 0x04004F90 RID: 20368
	public Transform animRoot;

	// Token: 0x04004F91 RID: 20369
	public Transform centerRoot;

	// Token: 0x04004F92 RID: 20370
	public Transform executionCenter;

	// Token: 0x04004F93 RID: 20371
	public Transform faceFollower;

	// Token: 0x04004F94 RID: 20372
	public SpriteRenderer[] faceSprites;

	// Token: 0x04004F95 RID: 20373
	public RabbitSpeech speech;

	// Token: 0x04004F96 RID: 20374
	[Header("RWBP Set")]
	public RabbitRwbpSet rwbpSet;

	// Token: 0x04004F97 RID: 20375
	public RwbpType rwbpType;

	// Token: 0x04004F98 RID: 20376
	public List<ParticleSystem> particle;

	// Token: 0x04004F99 RID: 20377
	public RabbitConversationType currentSpeechType = RabbitConversationType.NONE;

	// Token: 0x04004F9A RID: 20378
	[NonSerialized]
	public float zValue;
}
