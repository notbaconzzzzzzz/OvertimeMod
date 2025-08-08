using System;
using System.Collections.Generic;
using UnityEngine;

namespace BinahBoss
{
	// Token: 0x0200083C RID: 2108
	public class BinahOverload : IObserver
	{
		// Token: 0x060040EB RID: 16619 RVA: 0x0018FCF0 File Offset: 0x0018DEF0
		public BinahOverload(BinahCoreScript binah, OverloadType type)
		{
			this.binah = binah;
			this.overloadType = type;
			Notice.instance.Observe(NoticeName.OnIsolateOverloaded, this);
			Notice.instance.Observe(NoticeName.OnIsolateOverloadCanceled, this);
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x060040EC RID: 16620 RVA: 0x00037CFF File Offset: 0x00035EFF
		public MovableObjectNode Movable
		{
			get
			{
				return this.binah.movable;
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x060040ED RID: 16621 RVA: 0x00037D0C File Offset: 0x00035F0C
		public CreatureModel Model
		{
			get
			{
				return this.binah.model;
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x060040EE RID: 16622 RVA: 0x00037D19 File Offset: 0x00035F19
		public BinahCoreAnim AnimScript
		{
			get
			{
				return this.binah.AnimScript;
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x060040EF RID: 16623 RVA: 0x00037D26 File Offset: 0x00035F26
		public Animator Animator
		{
			get
			{
				return this.AnimScript.animator;
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x060040F0 RID: 16624 RVA: 0x00037D33 File Offset: 0x00035F33
		public DefenseInfo DefenseInfo
		{
			get
			{
				return this._defenseInfo;
			}
		}

		// Token: 0x060040F1 RID: 16625 RVA: 0x0018FD48 File Offset: 0x0018DF48
		public static int GetOverloadTargetCount(BinahOverload overload)
		{
			int creatureCount = CreatureManager.instance.GetCreatureCount();
			return (int)((float)creatureCount * overload.IsolatePercent);
		}

		// Token: 0x060040F2 RID: 16626 RVA: 0x0018FD6C File Offset: 0x0018DF6C
		public virtual GameObject LoadBinahAttachedEffect(string src)
		{
			if (!string.IsNullOrEmpty(src))
			{
				try
				{
					GameObject gameObject = Prefab.LoadPrefab(src);
					this.BinahAttachedEffect = gameObject;
					return gameObject;
				}
				catch (Exception ex)
				{
					BinahCoreScript.Log(ex.ToString(), true);
				}
			}
			return null;
		}

		// Token: 0x060040F3 RID: 16627 RVA: 0x00037D3B File Offset: 0x00035F3B
		public virtual void OnDestroy()
		{
			Notice.instance.Remove(NoticeName.OnIsolateOverloaded, this);
			Notice.instance.Remove(NoticeName.OnIsolateOverloadCanceled, this);
		}

		// Token: 0x060040F4 RID: 16628 RVA: 0x0018FDC0 File Offset: 0x0018DFC0
		public virtual void CastOverload()
		{
			int overloadTargetCount = BinahOverload.GetOverloadTargetCount(this);
			this.overloadedCreatures.AddRange(CreatureOverloadManager.instance.ActivateOverload(overloadTargetCount, this.overloadType, this.TimeLimit, false, false, false, new long[0]));
			foreach (CreatureModel creatureModel in this.overloadedCreatures)
			{
				creatureModel.ProbReductionValue = this.ProbReductionValue;
			}
			if (this.binah.IsInvincible)
			{
				this.binah.IsInvincible = false;
			}
		}

		// Token: 0x060040F5 RID: 16629 RVA: 0x00037D5D File Offset: 0x00035F5D
		public int GetCreatureCount()
		{
			return this.overloadedCreatures.Count;
		}

		// Token: 0x060040F6 RID: 16630 RVA: 0x0018FE70 File Offset: 0x0018E070
		public virtual void OnSuccess()
		{
			BinahCoreScript.Log("Success", false);
			this.binah.OnOverloadSuccessParticle(this.overloadType);
			if (this.SuccessAction != null)
			{
				this.binah.EnqueueAction(this.SuccessAction);
			}
			this.binah.ClearOverload(this.overloadType);
			this.AnimScript.TurnOffBodyEffect(this.overloadType);
		}

		// Token: 0x060040F7 RID: 16631 RVA: 0x000040A1 File Offset: 0x000022A1
		public virtual void Interrupt()
		{
		}

		// Token: 0x060040F8 RID: 16632 RVA: 0x00037D6A File Offset: 0x00035F6A
		public virtual void OnFail()
		{
			BinahCoreScript.Log("Failed", false);
			if (this.FailureAction != null)
			{
				this.binah.EnqueueAction(this.FailureAction);
			}
			this.binah.ClearOverload(this.overloadType);
		}

		// Token: 0x060040F9 RID: 16633 RVA: 0x0018FED8 File Offset: 0x0018E0D8
		public virtual void OnReducedCreature(CreatureModel creature)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.AnimScript.BinahOverloadClear);
			BinahOverloadClearParticle component = gameObject.GetComponent<BinahOverloadClearParticle>();
			ParticleSystem component2 = gameObject.GetComponent<ParticleSystem>();
			ParticleSystem componentInChildren = gameObject.GetComponentInChildren<ParticleSystem>();
			Vector3 position = creature.Unit.room.transform.position;
			component.Init(this.binah, position, this.overloadType);
			component.invokedCreature = creature;
			ParticleSystem.MainModule main = component2.main;
			ParticleSystem.MinMaxGradient startColor = main.startColor;
			ParticleSystem.MainModule main2 = componentInChildren.main;
			ParticleSystem.MinMaxGradient startColor2 = main2.startColor;
			Color overloadTypeColor = this.AnimScript.GetOverloadTypeColor(this.overloadType);
			startColor.colorMin = overloadTypeColor;
			startColor.colorMax = new Color(overloadTypeColor.r * 0.6f, overloadTypeColor.g * 0.6f, overloadTypeColor.b * 0.6f);
			startColor2.colorMin = overloadTypeColor;
			startColor2.colorMax = this.AnimScript.defaultColorMax;
			main.startColor = startColor;
			main2.startColor = startColor2;
			SoundEffectPlayer soundEffectPlayer = this.binah.MakeSound(BinahSoundInfo.OverloadClear);
			soundEffectPlayer.transform.SetParent(component.transform);
			soundEffectPlayer.transform.localPosition = new Vector3(0f, 0f, soundEffectPlayer.transform.localPosition.z);
			this.OnParticleArrived(creature);
		}

		// Token: 0x060040FA RID: 16634 RVA: 0x00190038 File Offset: 0x0018E238
		public void OnNotice(string notice, params object[] param)
		{
			if (notice == NoticeName.OnIsolateOverloaded)
			{
				CreatureModel item = param[0] as CreatureModel;
				OverloadType overloadType = (OverloadType)param[1];
				if (this.overloadType == overloadType && this.overloadedCreatures.Contains(item))
				{
					this.OnFail();
				}
			}
			else if (notice == NoticeName.OnIsolateOverloadCanceled)
			{
				CreatureModel creatureModel = param[0] as CreatureModel;
				OverloadType overloadType2 = (OverloadType)param[1];
				if (this.overloadType == overloadType2 && this.overloadedCreatures.Contains(creatureModel))
				{
					this.OnReducedCreature(creatureModel);
				}
			}
		}

		// Token: 0x060040FB RID: 16635 RVA: 0x00037DA4 File Offset: 0x00035FA4
		public virtual void OnParticleArrived(CreatureModel creature)
		{
			if (this.overloadedCreatures.Contains(creature))
			{
				this.overloadedCreatures.Remove(creature);
				if (this.overloadedCreatures.Count == 0)
				{
					this.OnSuccess();
				}
			}
		}

		// Token: 0x060040FC RID: 16636 RVA: 0x000040A1 File Offset: 0x000022A1
		public virtual void OnExecute()
		{
		}

		// Token: 0x04003C11 RID: 15377
		public int ProbReductionValue;

		// Token: 0x04003C12 RID: 15378
		public float TimeLimit = 60f;

		// Token: 0x04003C13 RID: 15379
		public GameObject BinahAttachedEffect;

		// Token: 0x04003C14 RID: 15380
		public float IsolatePercent;

		// Token: 0x04003C15 RID: 15381
		public BinahAction SuccessAction;

		// Token: 0x04003C16 RID: 15382
		public BinahAction FailureAction;

		// Token: 0x04003C17 RID: 15383
		public OverloadType overloadType;

		// Token: 0x04003C18 RID: 15384
		public List<CreatureModel> overloadedCreatures = new List<CreatureModel>();

		// Token: 0x04003C19 RID: 15385
		public BinahCoreScript binah;

		// Token: 0x04003C1A RID: 15386
		private DefenseInfo _defenseInfo;
	}
}
