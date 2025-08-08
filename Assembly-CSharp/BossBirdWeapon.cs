using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000652 RID: 1618
public class BossBirdWeapon : EquipmentScriptBase
{
	// Token: 0x060035D2 RID: 13778 RVA: 0x00030AF2 File Offset: 0x0002ECF2
	public BossBirdWeapon()
	{
	}

	// Token: 0x060035D3 RID: 13779 RVA: 0x0015FD50 File Offset: 0x0015DF50
	private void Init()
	{
		this.worker = (base.model.owner as WorkerModel);
		this.defaultInfo.type = SplashType.SPLASH;
		this.defaultInfo.range = 1f;
		this.skillInfo.type = SplashType.PENETRATION;
		this.skillInfo.iff = false;
		this._isSpecialAttack = false;
		this.LoadEffect();
	}

	// Token: 0x060035D4 RID: 13780 RVA: 0x0015FDB4 File Offset: 0x0015DFB4
	private void LoadEffect()
	{
		this._skillEffect = Prefab.LoadPrefab(effectSrc);
		Transform giftPos = this.worker.GetWorkerUnit().spriteSetter.GetGiftPos(EGOgiftAttachRegion.EYE);
		this._skillEffect.transform.SetParent(giftPos);
		this._skillEffect.transform.localPosition = new Vector3(-0.585f, 0f, 0f);
		this._skillEffect.transform.localScale = Vector3.one;
		this._skillEffect.transform.localRotation = Quaternion.Euler(Vector3.zero);
	}

	// Token: 0x060035D5 RID: 13781 RVA: 0x00030B22 File Offset: 0x0002ED22
	private void SetEffectActive(bool state)
	{
		this._skillEffect.SetActive(state);
	}

	// Token: 0x060035D6 RID: 13782 RVA: 0x00030B30 File Offset: 0x0002ED30
	private void ClearEffect()
	{
		UnityEngine.Object.Destroy(this._skillEffect);
		this._skillEffect = null;
	}

	// Token: 0x060035D7 RID: 13783 RVA: 0x00030B44 File Offset: 0x0002ED44
	public override void OnStageStart()
	{
		base.OnStageStart();
		this.Init();
		this.SetEffectActive(false);
	}

	// Token: 0x060035D8 RID: 13784 RVA: 0x00030B59 File Offset: 0x0002ED59
	public override void OnStageRelease()
	{
		base.OnStageRelease();
		this.ClearEffect();
	}

	// Token: 0x060035D9 RID: 13785 RVA: 0x00030B67 File Offset: 0x0002ED67
	public override void OnPrepareWeapon(UnitModel actor)
	{
		this._isBattle = true;
	}

	// Token: 0x060035DA RID: 13786 RVA: 0x00030B70 File Offset: 0x0002ED70
	public override void OnCancelWeapon(UnitModel actor)
	{
		this._isBattle = false;
		this.SetEffectActive(false);
	}

	// Token: 0x060035DB RID: 13787 RVA: 0x00030B80 File Offset: 0x0002ED80
	public void CloseSkill()
	{
		if (this._isSpecialAttack)
		{
			this._skillCoolTimer.StartTimer(_skillCoolTime);
			this._isSpecialAttack = false;
		}
	}

	// Token: 0x060035DC RID: 13788 RVA: 0x0015FE4C File Offset: 0x0015E04C
	public override EquipmentScriptBase.WeaponDamageInfo OnAttackStart(UnitModel actor, UnitModel target)
	{
		List<DamageInfo> list = new List<DamageInfo>();
		string animationName = string.Empty;
		List<int> list2 = new List<int>();
		if (!this._skillCoolTimer.started)
		{
			list2.AddRange(BossBirdWeapon.specialDamageAry);
			animationName = base.model.metaInfo.animationNames[1];
			this._isSpecialAttack = true;
		}
		else
		{
			list2.AddRange(BossBirdWeapon.normalDamageAry);
			animationName = base.model.metaInfo.animationNames[0];
		}
		foreach (int index in list2)
		{
			list.Add(this.GetDamage(index));
		}
		AutoTimer autoTimer = new AutoTimer();
		autoTimer.Init();
		autoTimer.StartTimer(5f, new AutoTimer.TargetMethod(this.CloseSkill), AutoTimer.UpdateMode.FIXEDUPDATE);
		this._closeTimer = autoTimer;
		return new EquipmentScriptBase.WeaponDamageInfo(animationName, list.ToArray());
	}

	// Token: 0x060035DD RID: 13789 RVA: 0x0015FF50 File Offset: 0x0015E150
	private DamageInfo GetDamage(int index)
	{
		return base.model.metaInfo.damageInfos[index].Copy();
	}

	// Token: 0x060035DE RID: 13790 RVA: 0x00030BA4 File Offset: 0x0002EDA4
	public override void OnAttackEnd(UnitModel actor, UnitModel target)
	{
		base.OnAttackEnd(actor, target);
		this.CloseSkill();
		if (this._closeTimer != null)
		{
			AutoTimer.Destroy(this._closeTimer);
		}
	}

	// Token: 0x060035DF RID: 13791 RVA: 0x0015FF78 File Offset: 0x0015E178
	public override void OnFixedUpdate()
	{
		base.OnFixedUpdate();
		if (GameManager.currentGameManager.state != GameState.PLAYING)
		{
			return;
		}
		if (this._skillCoolTimer.started)
		{
			if (this._isBattle && this._skillEffect.activeInHierarchy)
			{
				this.SetEffectActive(false);
			}
			if (this._skillCoolTimer.RunTimer())
			{
			}
		}
		else if (this._isBattle && !this._skillEffect.activeInHierarchy)
		{
			this.SetEffectActive(true);
		}
	}

	// Token: 0x060035E0 RID: 13792 RVA: 0x00030BCA File Offset: 0x0002EDCA
	// Note: this type is marked as 'beforefieldinit'.
	static BossBirdWeapon()
	{
	}

	// Token: 0x040031D7 RID: 12759
	private const string effectSrc = "Effect/Creature/BigBird/BossBirdWeaponEffect";

	// Token: 0x040031D8 RID: 12760
	private const float _skillCoolTime = 60f;

	// Token: 0x040031D9 RID: 12761
	private static int[] normalDamageAry = new int[]
	{
		0,
		1,
		2,
		3
	};

	// Token: 0x040031DA RID: 12762
	private static int[] specialDamageAry = new int[]
	{
		4,
		5,
		6,
		7,
		8,
		9,
		10,
		11
	};

	// Token: 0x040031DB RID: 12763
	private SplashInfo defaultInfo = new SplashInfo();

	// Token: 0x040031DC RID: 12764
	private SplashInfo skillInfo = new SplashInfo();

	// Token: 0x040031DD RID: 12765
	private Timer _skillCoolTimer = new Timer();

	// Token: 0x040031DE RID: 12766
	private AutoTimer _closeTimer;

	// Token: 0x040031DF RID: 12767
	private WorkerModel worker;

	// Token: 0x040031E0 RID: 12768
	private bool _isSpecialAttack;

	// Token: 0x040031E1 RID: 12769
	private GameObject _skillEffect;

	// Token: 0x040031E2 RID: 12770
	private bool _isBattle = true;
}
