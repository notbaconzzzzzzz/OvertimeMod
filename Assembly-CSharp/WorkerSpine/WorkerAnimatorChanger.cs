using System;
using Spine;
using Spine.Unity;
using UnityEngine;
using WorkerSprite;

namespace WorkerSpine
{
	// Token: 0x020008B4 RID: 2228
	[RequireComponent(typeof(SkeletonAnimator), typeof(Animator), typeof(WorkerSpriteSetter))]
	public class WorkerAnimatorChanger : MonoBehaviour
	{
		// Token: 0x0600448C RID: 17548 RVA: 0x0000462C File Offset: 0x0000282C
		public WorkerAnimatorChanger()
		{
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x0600448D RID: 17549 RVA: 0x0003A306 File Offset: 0x00038506
		// (set) Token: 0x0600448E RID: 17550 RVA: 0x0003A30E File Offset: 0x0003850E
		private SkeletonAnimator skeletonAnimator
		{
			get
			{
				return this._skeletonAnimator;
			}
			set
			{
				this._skeletonAnimator = value;
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x0600448F RID: 17551 RVA: 0x0003A317 File Offset: 0x00038517
		// (set) Token: 0x06004490 RID: 17552 RVA: 0x0003A31F File Offset: 0x0003851F
		private Animator animator
		{
			get
			{
				return this._animator;
			}
			set
			{
				this._animator = value;
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06004491 RID: 17553 RVA: 0x0003A328 File Offset: 0x00038528
		// (set) Token: 0x06004492 RID: 17554 RVA: 0x0003A330 File Offset: 0x00038530
		public WorkerSpriteSetter setter
		{
			get
			{
				return this._setter;
			}
			set
			{
				this._setter = value;
			}
		}

		// Token: 0x06004493 RID: 17555 RVA: 0x001A5DAC File Offset: 0x001A3FAC
		private void Awake()
		{
			this._skeletonAnimator = base.gameObject.GetComponent<SkeletonAnimator>();
			this._animator = base.gameObject.GetComponent<Animator>();
			this._setter = base.gameObject.GetComponent<WorkerSpriteSetter>();
			this.DefaultData = WorkerSpineAnimatorData.MakeDefault(this.animator.runtimeAnimatorController, this.skeletonAnimator.SkeletonDataAsset);
		}

		// Token: 0x06004494 RID: 17556 RVA: 0x000043CD File Offset: 0x000025CD
		private void Start()
		{
		}

		// Token: 0x06004495 RID: 17557 RVA: 0x000043CD File Offset: 0x000025CD
		private void Update()
		{
		}

		// Token: 0x06004496 RID: 17558 RVA: 0x0003A339 File Offset: 0x00038539
		public void ChangeAnimator()
		{
			this.SetAnimator(this.DefaultData);
			this._setter.SetPanicShadow(false, RwbpType.N);
			this._setter.EnableSeparator();
		}

		// Token: 0x06004497 RID: 17559 RVA: 0x001A5E10 File Offset: 0x001A4010
		public void ChangeAnimator(string name)
		{
			WorkerSpineAnimatorData animator = null;
			if (WorkerSpineAnimatorManager.instance.GetDataWithCheck(name, out animator))
			{
				this.SetAnimator(animator);
			}
			else
			{
				Debug.Log("Error in founding spine animator data : " + name);
			}
			this._setter.BaiscRendererInit();
		}

		// Token: 0x06004498 RID: 17560 RVA: 0x001A5E54 File Offset: 0x001A4054
		public void ChangeAnimator(string name, bool separator)
		{
			WorkerSpineAnimatorData data = null;
			if (WorkerSpineAnimatorManager.instance.GetDataWithCheck(name, out data))
			{
				this.SetAnimator(data, separator);
			}
			else
			{
				Debug.Log("Error in founding spine animator data : " + name);
			}
			this._setter.BaiscRendererInit();
		}

		// Token: 0x06004499 RID: 17561 RVA: 0x001A5E98 File Offset: 0x001A4098
		public void ChangeAnimatorWithUniqueFace(string name, bool separator)
		{
			WorkerSpineAnimatorData data = null;
			if (WorkerSpineAnimatorManager.instance.GetDataWithCheck(name, out data))
			{
				this.SetAnimatorWithUniqueFace(data, separator);
				return;
			}
			Debug.Log("Error in founding spine animator data : " + name);
		}

		// Token: 0x0600449A RID: 17562 RVA: 0x001A5ED0 File Offset: 0x001A40D0
		public void ChangeAnimator(int id)
		{
			WorkerSpineAnimatorData animator = null;
			if (WorkerSpineAnimatorManager.instance.GetDataWithCheck(id, out animator))
			{
				this.SetAnimator(animator);
			}
			else
			{
				Debug.Log("Error in founding spine animator data : " + id);
			}
			this._setter.BaiscRendererInit();
		}

		// Token: 0x0600449B RID: 17563 RVA: 0x001A5F18 File Offset: 0x001A4118
		private void SetAnimator(WorkerSpineAnimatorData data)
		{
			if (!data.IsLoaded)
			{
				data.LoadData();
			}
			this.skeletonAnimator.skeletonDataAsset = data.skeletonData;
			this.animator.runtimeAnimatorController = data.animator;
			this.skeletonAnimator.Initialize(true);
			if (data.name.Contains("_"))
			{
				this.skeletonAnimator.state = new Spine.AnimationState(data.skeletonData.GetAnimationStateData());
				this.state = this.skeletonAnimator.state;
				this.skeleton = this.skeletonAnimator.skeleton;
			}
			else
			{
				this.skeletonAnimator.state = null;
				this.state = null;
				this.skeleton = null;
				this.skeletonAnimator.UseState = false;
			}
			this._setter.Reskin();
			this.CurrentData = data;
		}

		// Token: 0x0600449C RID: 17564 RVA: 0x001A5FEC File Offset: 0x001A41EC
		private void SetAnimator(WorkerSpineAnimatorData data, bool separator)
		{
			if (!data.IsLoaded)
			{
				data.LoadData();
			}
			this.skeletonAnimator.skeletonDataAsset = data.skeletonData;
			this.animator.runtimeAnimatorController = data.animator;
			this.skeletonAnimator.Initialize(true);
			if (data.name.Contains("_"))
			{
				this.skeletonAnimator.state = new Spine.AnimationState(data.skeletonData.GetAnimationStateData());
				this.state = this.skeletonAnimator.state;
				this.skeleton = this.skeletonAnimator.skeleton;
			}
			else
			{
				this.skeletonAnimator.state = null;
				this.state = null;
				this.skeleton = null;
			}
			this._setter.Reskin();
			if (!separator)
			{
				this._setter.DisableSeparator();
			}
			this.CurrentData = data;
		}

		// Token: 0x0600449D RID: 17565 RVA: 0x001A60C0 File Offset: 0x001A42C0
		private void SetAnimatorWithUniqueFace(WorkerSpineAnimatorData data, bool separator)
		{
			if (!data.IsLoaded)
			{
				data.LoadData();
			}
			this.skeletonAnimator.skeletonDataAsset = data.skeletonData;
			this.animator.runtimeAnimatorController = data.animator;
			this.skeletonAnimator.Initialize(true);
			if (data.name.Contains("_"))
			{
				this.skeletonAnimator.state = new Spine.AnimationState(data.skeletonData.GetAnimationStateData());
				this.state = this.skeletonAnimator.state;
				this.skeleton = this.skeletonAnimator.skeleton;
				if (data.name.Split(new char[]
				{
					'_'
				})[1].ToLower() == "custom")
				{
					this.skeletonAnimator.UseState = true;
				}
			}
			else
			{
				this.skeletonAnimator.state = null;
				this.state = null;
				this.skeleton = null;
			}
			this._setter.UniqueFaceReskin();
			if (!separator)
			{
				this._setter.DisableSeparatorForUnique();
			}
			this.CurrentData = data;
		}

		// Token: 0x0600449E RID: 17566 RVA: 0x0003A35F File Offset: 0x0003855F
		public void SetState(bool state)
		{
			this.skeletonAnimator.UseState = state;
		}

		// Token: 0x0600449F RID: 17567 RVA: 0x0003A306 File Offset: 0x00038506
		public SkeletonAnimator GetAnimator()
		{
			return this._skeletonAnimator;
		}

		// Token: 0x04003F42 RID: 16194
		private SkeletonAnimator _skeletonAnimator;

		// Token: 0x04003F43 RID: 16195
		private Animator _animator;

		// Token: 0x04003F44 RID: 16196
		private WorkerSpriteSetter _setter;

		// Token: 0x04003F45 RID: 16197
		public WorkerSpineAnimatorData DefaultData;

		// Token: 0x04003F46 RID: 16198
		public WorkerSpineAnimatorData CurrentData;

		// Token: 0x04003F47 RID: 16199
		public Skeleton skeleton;

		// Token: 0x04003F48 RID: 16200
		public Spine.AnimationState state;
	}
}
