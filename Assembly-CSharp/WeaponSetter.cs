using System;
using UnityEngine;
using WorkerSpine;
using WorkerSprite;

// Token: 0x02000025 RID: 37
public class WeaponSetter : MonoBehaviour
{
	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000077 RID: 119 RVA: 0x000047B5 File Offset: 0x000029B5
	private Animator animator
	{
		get
		{
			return base.GetComponent<Animator>();
		}
	}

	// Token: 0x06000078 RID: 120 RVA: 0x000047BD File Offset: 0x000029BD
	private void Awake()
	{
		this.setter = base.gameObject.GetComponent<WorkerSpriteSetter>();
	}

	// Token: 0x06000079 RID: 121 RVA: 0x00051B24 File Offset: 0x0004FD24
	public static bool IsTwohanded(WeaponModel weapon)
	{
		WeaponClassType weaponClassType = weapon.metaInfo.weaponClassType;
		int num = (int)weaponClassType;
		return weaponClassType == WeaponClassType.SPECIAL || num > 3;
	}

	// Token: 0x0600007A RID: 122 RVA: 0x00051B54 File Offset: 0x0004FD54
	public void SetAnimatorInfo(WeaponModel weapon)
	{
		WeaponClassType weaponClassType = weapon.metaInfo.weaponClassType;
		int num = (int)weaponClassType;
		if (weaponClassType != WeaponClassType.SPECIAL)
		{
			if (weaponClassType == WeaponClassType.OFFICER)
			{
				this.animator.SetBool("UniqueBattleMove", false);
				this.animator.SetInteger("WeaponId", num);
				this.animator.SetBool("TwoHanded", false);
			}
			else if (weaponClassType == WeaponClassType.FIST)
			{
				num -= 4;
				this.animator.SetBool("UniqueBattleMove", true);
				this.animator.SetInteger("WeaponId", num);
				this.animator.SetBool("TwoHanded", true);
			}
			else
			{
				if (num >= 4)
				{
					num -= 4;
					if (num >= 3)
					{
						this.animator.SetBool("UniqueBattleMove", true);
					}
					else
					{
						this.animator.SetBool("UniqueBattleMove", false);
					}
					this.animator.SetBool("TwoHanded", true);
				}
				else
				{
					this.animator.SetBool("TwoHanded", false);
				}
				this.animator.SetInteger("WeaponId", num);
			}
		}
	}

	// Token: 0x0600007B RID: 123 RVA: 0x000047D0 File Offset: 0x000029D0
	public static void SetWeaponAnimParam(WorkerModel worker)
	{
		worker.GetWorkerUnit().weaponSetter.SetAnimatorInfo(worker.Equipment.weapon);
	}

	// Token: 0x0600007C RID: 124 RVA: 0x00051C70 File Offset: 0x0004FE70
	public void SetWeapon(WeaponModel weapon)
	{ // <Patch> <Mod>
		LobotomyBaseMod.KeyValuePairSS ss = new LobotomyBaseMod.KeyValuePairSS(EquipmentTypeInfo.GetLcId(weapon.metaInfo).packageId, weapon.metaInfo.sprite);
		WeaponClassType weaponClassType = weapon.metaInfo.weaponClassType;
		int num = (int)weaponClassType;
		if (weaponClassType != WeaponClassType.SPECIAL)
		{
			if (weaponClassType == WeaponClassType.OFFICER)
			{
				this.animator.SetBool("UniqueBattleMove", false);
				this.animator.SetInteger("WeaponId", num);
				this.animator.SetBool("TwoHanded", false);
				this.setter.SetRightWeapon(WeaponClassType.OFFICER, null);
				if (weapon.metaInfo.sprite != string.Empty)
				{
					Sprite weaponSprite = WorkerSpriteManager.instance.GetWeaponSprite_Mod(weaponClassType, ss);
					this.setter.SetRightWeapon(weaponClassType, weaponSprite);
				}
			}
			else if (weaponClassType == WeaponClassType.FIST)
			{
				Sprite[] fistSprite = null;
				if (ss.key != "")
				{
					fistSprite = WorkerSpriteManager.instance.GetFistSprite(ss);
					if (fistSprite[0] == null || fistSprite[1] == null)
					{
						return;
					}
				}
				else
				{
					int id = (int)float.Parse(weapon.metaInfo.sprite);
					fistSprite = WorkerSprite_WorkerSpriteManager.instance.GetFistSprite(id);
					if (fistSprite[0] == null || fistSprite[1] == null)
					{
						return;
					}
				}
				this.isTwohanded = true;
				num -= 4;
				this.animator.SetBool("UniqueBattleMove", true);
				this.setter.SetLeftWeapon(WeaponClassType.FIST, fistSprite[0]);
				this.setter.SetRightWeapon(WeaponClassType.FIST, fistSprite[1]);
				this.weaponId = num;
				this.uniqueMovement = (num >= 3);
				this.animator.SetInteger("WeaponId", this.weaponId);
				this.animator.SetBool("TwoHanded", this.isTwohanded);
				return;
			}
			else
			{
				if (num >= 4)
				{
					num -= 4;
					this.isTwohanded = true;
					this.uniqueMovement = (num >= 3);
					if (num >= 3)
					{
						this.animator.SetBool("UniqueBattleMove", true);
					}
					else
					{
						this.animator.SetBool("UniqueBattleMove", false);
					}
				}
				else
				{
					this.isTwohanded = false;
				}
				this.weaponId = num;
				this.animator.SetInteger("WeaponId", this.weaponId);
				this.animator.SetBool("TwoHanded", this.isTwohanded);
				if (weapon.metaInfo.sprite != string.Empty)
				{
					Sprite weaponSprite2 = WorkerSpriteManager.instance.GetWeaponSprite_Mod(weaponClassType, ss);
					this.setter.SetRightWeapon(weaponClassType, weaponSprite2);
				}
			}
		}
	}

	// Token: 0x0600007D RID: 125 RVA: 0x00051EB4 File Offset: 0x000500B4
	public void SetWeapon(Sprite weaponSprite, WeaponClassType weaponType)
	{
		int num = (int)weaponType;
		this.changer = base.GetComponent<AgentSpriteChanger>();
		this.MoveWeapon.gameObject.SetActive(false);
		if (weaponType != WeaponClassType.FIST)
		{
			if (num >= 2)
			{
				num -= 2;
				this.isTwohanded = true;
				this.uniqueMovement = (num >= 3);
				if (num >= 3)
				{
					this.animator.SetBool("UniqueBattleMove", true);
				}
				else
				{
					this.animator.SetBool("UniqueBattleMove", false);
				}
			}
			else
			{
				this.isTwohanded = false;
			}
			this.weaponId = num;
			this.animator.SetInteger("WeaponId", this.weaponId);
			this.animator.SetBool("TwoHanded", this.isTwohanded);
			string empty = string.Empty;
			string empty2 = string.Empty;
			WeaponRegionKey.GetKey(weaponType, out empty, out empty2);
			this.changer.WeaponSetting(weaponSprite, empty, empty2);
			this.MoveWeapon.sprite = weaponSprite;
			this.MoveWeapon.transform.localPosition = new Vector3(this.changer.positionFix.x, this.changer.positionFix.y, this.MoveWeapon.transform.localPosition.z);
			this.MoveWeapon.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, this.changer.rotationFix));
			return;
		}
		Sprite[] fistSprite = WorkerSprite_WorkerSpriteManager.instance.GetFistSprite();
		if (fistSprite[0] == null || fistSprite[1] == null)
		{
			return;
		}
		this.isTwohanded = true;
		num -= 2;
		this.animator.SetBool("UniqueBattleMove", true);
		string left = WeaponRegionKey.Left;
		string right = WeaponRegionKey.Right;
		string fistLeft = WeaponRegionKey.FistLeft;
		string fistRight = WeaponRegionKey.FistRight;
		this.setter.SetLeftWeapon(WeaponClassType.FIST, fistSprite[0]);
		this.setter.SetRightWeapon(WeaponClassType.FIST, fistSprite[1]);
		this.weaponId = num;
		this.uniqueMovement = (num >= 3);
		this.animator.SetInteger("WeaponId", this.weaponId);
		this.animator.SetBool("TwoHanded", this.isTwohanded);
	}

	// Token: 0x040000BB RID: 187
	[NonSerialized]
	public int weaponId;

	// Token: 0x040000BC RID: 188
	[NonSerialized]
	public bool isTwohanded;

	// Token: 0x040000BD RID: 189
	public SpriteRenderer MoveWeapon;

	// Token: 0x040000BE RID: 190
	private AgentSpriteChanger changer;

	// Token: 0x040000BF RID: 191
	private WorkerSpriteSetter setter;

	// Token: 0x040000C0 RID: 192
	private bool uniqueMovement;
}
