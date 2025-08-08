using System;
using UnityEngine;
using UnityEngine.UI;

namespace CommandWindow
{
	// Token: 0x02000967 RID: 2407
	[Serializable]
	public class WorkData
	{
		// Token: 0x06004872 RID: 18546 RVA: 0x00004378 File Offset: 0x00002578
		public WorkData()
		{
		}

		// Token: 0x06004873 RID: 18547 RVA: 0x0003CB5D File Offset: 0x0003AD5D
		public void SetCurrentWork(SkillTypeInfo current)
		{
			this._current = current;
			if (this._currentAgent == null)
			{
				return;
			}
			this.CheckCurrentSkill();
		}

		// Token: 0x06004874 RID: 18548 RVA: 0x001B1408 File Offset: 0x001AF608
		public void SetData(AgentModel model)
		{
			this._currentAgent = model;
			this.RLevel.sprite = DeployUI.GetGradeSprite(model.fortitudeLevel);
			this.WLevel.sprite = DeployUI.GetGradeSprite(model.prudenceLevel);
			this.BLevel.sprite = DeployUI.GetGradeSprite(model.temperanceLevel);
			this.PLevel.sprite = DeployUI.GetGradeSprite(model.justiceLevel);
			this.CheckCurrentSkill();
		}

		// Token: 0x06004875 RID: 18549 RVA: 0x0003CB78 File Offset: 0x0003AD78
		public void SetCreature(CreatureModel creature)
		{
			this._currentCreature = creature;
		}

		// Token: 0x06004876 RID: 18550 RVA: 0x001B147C File Offset: 0x001AF67C
		public void CheckCurrentSkill()
		{
			if (this._current == null)
			{
				return;
			}
			if (this._currentAgent == null)
			{
				return;
			}
			if (this._currentCreature == null)
			{
				return;
			}
			int num = (int)this._current.id;
			switch (num)
			{
			case 1:
				this.WorkLevel.text = AgentModel.GetLevelGradeText(this._currentAgent.Rstat);
				break;
			case 2:
				this.WorkLevel.text = AgentModel.GetLevelGradeText(this._currentAgent.Wstat);
				break;
			case 3:
				this.WorkLevel.text = AgentModel.GetLevelGradeText(this._currentAgent.Bstat);
				break;
			case 4:
				this.WorkLevel.text = AgentModel.GetLevelGradeText(this._currentAgent.Pstat);
				break;
			case 6:
				this.WorkLevel.text = 0.ToString();
				break;
			case 7:
				this.WorkLevel.text = AgentModel.GetLevelGradeText(this._currentAgent.Wstat);
				break;
			}
			if (num != 6 && num != 7 && (num < 1 || num > 4))
			{
				return;
			}
			if (this.WorkSuccess.GetComponent<FontLoadScript>() == null)
			{
				this.WorkSuccess.gameObject.AddComponent<FontLoadScript>();
			}
			float num2;
			if (this._currentCreature.ProbReductionValue > 0)
			{
				num2 = (float)this._currentCreature.ProbReductionValue;
			}
			else
			{
				num2 = (float)this._currentCreature.GetRedusedWorkProbByCounter();
			}
			if (num != 6 && num != 7)
			{
				if (this._currentCreature.observeInfo.GetObserveState(WorkData.prefix + WorkData.region[num - 1]))
				{
					float num3 = this._currentCreature.GetWorkSuccessProb(this._currentAgent, this._current) * 100f + (float)this._currentAgent.workProb / 5f;
					if (num3 > 95f)
					{
						num3 = 95f;
					}
					num3 -= num2;
					if (num3 < 0f)
					{
						num3 = 0f;
					}
					float rate = num3 / 100f;
					string percentText = UICommonTextConverter.GetPercentText(rate);
					if (num2 > 0f)
					{
						this.WorkSuccess.text = string.Format("<color=red>{0}</color>", percentText);
					}
					else
					{
						this.WorkSuccess.text = percentText;
					}
					this.WorkSpeed.text = string.Format("{0}", Math.Ceiling((double)((float)this._currentCreature.metaInfo.feelingStateCubeBounds.GetLastBound() / (this._currentCreature.GetCubeSpeed() * (1f + (float)(this._currentCreature.GetObserveBonusSpeed() + this._currentAgent.workSpeed) / 100f)))));
				}
				else
				{
					string text = LocalizeTextDataModel.instance.GetText("CannotCalculate");
					if (num2 > 0f)
					{
						this.WorkSuccess.text = string.Format("<color=red>{0}</color>", text);
					}
					else
					{
						this.WorkSuccess.text = text;
					}
					this.WorkSpeed.text = LocalizeTextDataModel.instance.GetText("CannotCalculate");
				}
			}
			else if (num == 7)
			{
				if (this._currentCreature.observeInfo.GetObserveState(WorkData.prefix + WorkData.region[1]))
				{
					float num4 = this._currentCreature.GetWorkSuccessProb(this._currentAgent, this._current) * 100f + (float)this._currentAgent.workProb / 5f;
					if (num4 > 95f)
					{
						num4 = 95f;
					}
					num4 -= num2;
					if (num4 < 0f)
					{
						num4 = 0f;
					}
					float rate2 = num4 / 100f;
					string percentText2 = UICommonTextConverter.GetPercentText(rate2);
					if (num2 > 0f)
					{
						this.WorkSuccess.text = string.Format("<color=red>{0}</color>", percentText2);
					}
					else
					{
						this.WorkSuccess.text = percentText2;
					}
					this.WorkSpeed.text = string.Format("{0}", Math.Ceiling((double)((float)this._currentCreature.metaInfo.feelingStateCubeBounds.GetLastBound() / (this._currentCreature.GetCubeSpeed() * (1f + (float)(this._currentCreature.GetObserveBonusSpeed() + this._currentAgent.workSpeed) / 100f)))));
				}
				else
				{
					string text2 = LocalizeTextDataModel.instance.GetText("CannotCalculate");
					if (num2 > 0f)
					{
						this.WorkSuccess.text = string.Format("<color=red>{0}</color>", text2);
					}
					else
					{
						this.WorkSuccess.text = text2;
					}
					this.WorkSpeed.text = LocalizeTextDataModel.instance.GetText("CannotCalculate");
				}
			}
			else
			{
				string text3 = LocalizeTextDataModel.instance.GetText("CannotCalculate");
				if (num2 > 0f)
				{
					this.WorkSuccess.text = string.Format("<color=red>{0}</color>", text3);
				}
				else
				{
					this.WorkSuccess.text = text3;
				}
				this.WorkSpeed.text = LocalizeTextDataModel.instance.GetText("CannotCalculate");
			}
			this.WorkIcon.sprite = CommandWindow.CurrentWindow.GetWorkSprite((RwbpType)this._current.id);
		}

		// Token: 0x06004877 RID: 18551 RVA: 0x0003CB81 File Offset: 0x0003AD81
		// Note: this type is marked as 'beforefieldinit'.
		static WorkData()
		{
		}

		// Token: 0x040042ED RID: 17133
		private static string prefix = "work_";

		// Token: 0x040042EE RID: 17134
		private static string[] region = new string[]
		{
			"r",
			"w",
			"b",
			"p"
		};

		// Token: 0x040042EF RID: 17135
		public Image WorkIcon;

		// Token: 0x040042F0 RID: 17136
		public Text WorkLevel;

		// Token: 0x040042F1 RID: 17137
		public Text WorkSpeed;

		// Token: 0x040042F2 RID: 17138
		public Text WorkSuccess;

		// Token: 0x040042F3 RID: 17139
		[Header("Stat Level")]
		public Image RLevel;

		// Token: 0x040042F4 RID: 17140
		public Image WLevel;

		// Token: 0x040042F5 RID: 17141
		public Image BLevel;

		// Token: 0x040042F6 RID: 17142
		public Image PLevel;

		// Token: 0x040042F7 RID: 17143
		private SkillTypeInfo _current;

		// Token: 0x040042F8 RID: 17144
		[NonSerialized]
		private AgentModel _currentAgent;

		// Token: 0x040042F9 RID: 17145
		[NonSerialized]
		private CreatureModel _currentCreature;
	}
}
