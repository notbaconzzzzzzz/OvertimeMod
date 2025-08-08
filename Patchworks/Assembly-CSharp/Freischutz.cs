/*
private float RequestCost // 
*/
using System;
using System.Collections.Generic;
using Assets.Scripts.UI.Utils;
using CommandWindow;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000404 RID: 1028
public class Freischutz : CreatureBase
{
	// Token: 0x06002327 RID: 8999 RVA: 0x00024011 File Offset: 0x00022211
	public Freischutz()
	{
	}

	// Token: 0x1700031A RID: 794
	// (get) Token: 0x06002328 RID: 9000 RVA: 0x00105BC0 File Offset: 0x00103DC0
	private float RequestCost
	{ // <Mod>
		get
		{
			float num = EnergyModel.instance.GetEnergy() * this._COST_REQUEST;
            float num2 = (num >= this._COST_REQUEST_MIN) ? num : this._COST_REQUEST_MIN;
            if (ResearchDataModel.instance.IsUpgradedAbility("energy_discount"))
			{
				num2 *= 0.9f;
			}
			return num2;
		}
	}

	// Token: 0x06002329 RID: 9001 RVA: 0x0002404F File Offset: 0x0002224F
	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
		this._animScript = (FreischutzAnim)this.model.Unit.animTarget;
		this._animScript.SetScript(this);
	}

	// Token: 0x0600232A RID: 9002 RVA: 0x0002407F File Offset: 0x0002227F
	public override void OnStageStart()
	{
		this.isDefaultState = true;
		this._shootCount = 0;
	}

	// Token: 0x0600232B RID: 9003 RVA: 0x00014021 File Offset: 0x00012221
	public override bool HasRoomCounter()
	{
		return true;
	}

	// Token: 0x0600232C RID: 9004 RVA: 0x00105BF8 File Offset: 0x00103DF8
	public override void OnReleaseWork(UseSkill skill)
	{
		CreatureFeelingState feelingState = this.model.GetFeelingState();
		if (feelingState != CreatureFeelingState.BAD)
		{
			if (feelingState == CreatureFeelingState.NORM)
			{
				this.model.SubQliphothCounter();
			}
		}
		else
		{
			this.model.SubQliphothCounter();
		}
	}

	// Token: 0x0600232D RID: 9005 RVA: 0x0002408F File Offset: 0x0002228F
	public override void OnFinishWork(UseSkill skill)
	{
		if (skill.agent.justiceLevel < this._CONDITION_JUSTICE_LEVEL)
		{
			this.model.SubQliphothCounter();
		}
	}

	// Token: 0x0600232E RID: 9006 RVA: 0x000240B2 File Offset: 0x000222B2
	public override void ActivateQliphothCounter()
	{
		this.ShootBullet();
		this.isDefaultState = false;
	}

	// Token: 0x0600232F RID: 9007 RVA: 0x00105C44 File Offset: 0x00103E44
	public bool ActiveShootRequest()
	{
		float energy = EnergyModel.instance.GetEnergy();
		if (energy >= this.RequestCost)
		{
			this._shootCount++;
			if (this._shootCount >= this._CONDITION_SHOOT_COUNT)
			{
				this._shootCount = 0;
				this.model.SubQliphothCounter();
			}
			if (this.model.qliphothCounter > 0)
			{
				this.isDefaultState = false;
				EnergyModel.instance.SubEnergy(this.RequestCost);
				this._animScript.ShowSnipintUI();
			}
			return true;
		}
		return false;
	}

	// Token: 0x06002330 RID: 9008 RVA: 0x00105CD0 File Offset: 0x00103ED0
	private void ShootBullet()
	{
		Sefira[] opendSefiraList = SefiraManager.instance.GetOpendSefiraList();
		Sefira sefira = opendSefiraList[UnityEngine.Random.Range(0, opendSefiraList.Length)];
		List<PassageObjectModel> passageList = sefira.passageList;
		List<PassageObjectModel> list = new List<PassageObjectModel>();
		foreach (PassageObjectModel passageObjectModel in passageList)
		{
			if (passageObjectModel.isActivate)
			{
				list.Add(passageObjectModel);
			}
		}
		PassageObjectModel[] array = list.ToArray();
		PassageObjectModel passageObjectModel2 = array[UnityEngine.Random.Range(0, array.Length)];
		MapNode[] nodeList = passageObjectModel2.GetNodeList();
		MapNode mapNode = nodeList[UnityEngine.Random.Range(0, nodeList.Length)];
		this._animScript.ReadyToShoot(mapNode.GetPosition());
	}

	// Token: 0x06002331 RID: 9009 RVA: 0x000240C1 File Offset: 0x000222C1
	public override bool IsWorkable()
	{
		return this.model.state == CreatureState.WORKING || (this.model.qliphothCounter > 0 && this.isDefaultState);
	}

	// Token: 0x06002332 RID: 9010 RVA: 0x000240F0 File Offset: 0x000222F0
	public override bool OnOpenWorkWindow()
	{
		return this.model.qliphothCounter > 0 && this.isDefaultState;
	}

	// Token: 0x06002333 RID: 9011 RVA: 0x00105D9C File Offset: 0x00103F9C
	public override void OnOpenCommandWindow(Button[] buttons)
	{
		Image component = buttons[2].transform.Find("Icon").GetComponent<Image>();
		component.sprite = CommandWindow.CommandWindow.CurrentWindow.Work_S;
		LocalizeTextLoadScript component2 = buttons[2].transform.Find("WorkName").GetComponent<LocalizeTextLoadScript>();
		component2.SetText("Swork");
	}

	// Token: 0x06002334 RID: 9012 RVA: 0x0002410C File Offset: 0x0002230C
	public override bool HasUniqueCommandAction(int workType)
	{
		if (workType == 3)
		{
			this.ActiveShootRequest();
			return true;
		}
		return false;
	}

	// Token: 0x0400228B RID: 8843
	private readonly int _MAX_QLI = 3;

	// Token: 0x0400228C RID: 8844
	private readonly int _CONDITION_JUSTICE_LEVEL = 3;

	// Token: 0x0400228D RID: 8845
	private readonly int _CONDITION_SHOOT_COUNT = 7;

	// Token: 0x0400228E RID: 8846
	private readonly float _RANDOM_SHOOT_FREQ = 3f;

	// Token: 0x0400228F RID: 8847
	private readonly float _COST_REQUEST = 0.1f;

	// Token: 0x04002290 RID: 8848
	private readonly float _COST_REQUEST_MIN = 10f;

	// Token: 0x04002291 RID: 8849
	private int _shootCount;

	// Token: 0x04002292 RID: 8850
	private FreischutzAnim _animScript;

	// Token: 0x04002293 RID: 8851
	public bool isDefaultState;
}
