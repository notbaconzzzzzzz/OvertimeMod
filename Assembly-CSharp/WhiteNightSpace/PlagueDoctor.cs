using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using CommandWindow;
using UnityEngine;
using WorkerSpine;

namespace WhiteNightSpace
{
	// Token: 0x02000451 RID: 1105
	public class PlagueDoctor : CreatureBase, IObserver
	{
		// Token: 0x06002734 RID: 10036 RVA: 0x00026FC7 File Offset: 0x000251C7
		public PlagueDoctor()
		{
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06002735 RID: 10037 RVA: 0x00027006 File Offset: 0x00025206
		private static float AttractFreq
		{
			get
			{
				return PlagueDoctor._attractFreq.GetRandomFloat();
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06002736 RID: 10038 RVA: 0x00027012 File Offset: 0x00025212
		public PlagueDoctorAnim AnimScript
		{
			get
			{
				return this._animScript;
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06002737 RID: 10039 RVA: 0x0002701A File Offset: 0x0002521A
		public int ApostleLevel
		{
			get
			{
				return this.apostleData.Count;
			}
		}

		// Token: 0x06002738 RID: 10040 RVA: 0x00003FDD File Offset: 0x000021DD
		public static void Log(string log, bool isError = false)
		{
		}

		// Token: 0x06002739 RID: 10041 RVA: 0x00027027 File Offset: 0x00025227
		public override void OnInit()
		{
			this.LoadScriptData();
			if (this.ApostleLevel >= 12)
			{
				this.AdventWhiteNight();
			}
		}

		// Token: 0x0600273A RID: 10042 RVA: 0x00027042 File Offset: 0x00025242
		public void AdventWhiteNight()
		{
			PlagueDoctor.Log("Save : Load WhiteNight", true);
			this.GenDeathAngel();
		}

		// Token: 0x0600273B RID: 10043 RVA: 0x00027056 File Offset: 0x00025256
		public void OnAdventEnd()
		{
			PlagueDoctor.Log("Save : Load WhiteNight Advented", true);
			this._angel.Escape();
		}

		// Token: 0x0600273C RID: 10044 RVA: 0x00115424 File Offset: 0x00113624
		public CreatureModel GenDeathAngel()
		{
			CreatureModel creatureModel = CreatureManager.instance.ReplaceCreature(PlagueDoctor.WhiteNightID, this.model);
			DeathAngel deathAngel = creatureModel.script as DeathAngel;
			creatureModel.SetCurrentNode(this.model.GetCurrentNode());
			try
			{
				if (GameManager.currentGameManager != null)
				{
					creatureModel.Unit.transform.position = base.Unit.transform.position;
					creatureModel.Unit.transform.localScale = base.Unit.transform.localScale;
					if (GameManager.currentGameManager.state != GameState.STOP)
					{
						deathAngel.OnStageStart();
					}
				}
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
			deathAngel.SetApostleData(this.apostleData);
			this._angel = deathAngel;
			return creatureModel;
		}

		// Token: 0x0600273D RID: 10045 RVA: 0x00003FDD File Offset: 0x000021DD
		public override void ParamInit()
		{
		}

		// Token: 0x0600273E RID: 10046 RVA: 0x00115500 File Offset: 0x00113700
		public void OnPlagueDoctorAdventEnd()
		{
			this.AnimScript.AdventClockUI.AdventTimerStart(this._genData);
			this.AnimScript.transform.parent.gameObject.SetActive(false);
			this._angel.Unit.gameObject.SetActive(true);
		}

		// Token: 0x0600273F RID: 10047 RVA: 0x00115554 File Offset: 0x00113754
		public override void OnViewInit(CreatureUnit unit)
		{
			PlagueDoctor.Log("View Init", true);
			this._animScript = (unit.animTarget as PlagueDoctorAnim);
			this.AnimScript.SetScript(this);
			this.AnimScript.SkeletonAnim.SetState(this.ApostleLevel);
			this.AnimScript.AdventClockUI.SetNameEffectEndEvent(new AdventClockUI.EndEvent(this.OnClockUIEnd));
			this.AnimScript.AdventClockUI.SetAdventEffectEndEvent(new AdventClockUI.EndEvent(this.OnAdventEnd));
		}

		// Token: 0x06002740 RID: 10048 RVA: 0x001155D8 File Offset: 0x001137D8
		public override void OnStageStart()
		{
			foreach (AgentModel agentModel in AgentManager.instance.GetAgentList())
			{
				if (this.CheckApostle(agentModel))
				{
					agentModel.SetWorkCommandCheckEvnet(new AgentModel.CheckCommandState(this.BlessedAgentWorkCheck));
				}
			}
		}

		// Token: 0x06002741 RID: 10049 RVA: 0x00026EE1 File Offset: 0x000250E1
		public override void OnWorkCoolTimeEnd(CreatureFeelingState oldState)
		{
			if (oldState == CreatureFeelingState.BAD)
			{
				this.model.SubQliphothCounter();
			}
		}

		// Token: 0x06002742 RID: 10050 RVA: 0x0002706E File Offset: 0x0002526E
		public override void OnGamemanagerInit()
		{
			if (this._genDay < PlayerModel.instance.GetDay())
			{
				this.ChangeIsolateRoom();
			}
		}

		// Token: 0x06002743 RID: 10051 RVA: 0x00013CB1 File Offset: 0x00011EB1
		public override bool HasRoomCounter()
		{
			return true;
		}

		// Token: 0x06002744 RID: 10052 RVA: 0x0011564C File Offset: 0x0011384C
		public override void ActivateQliphothCounter()
		{
			List<AgentModel> list = new List<AgentModel>();
			foreach (AgentModel agentModel in AgentManager.instance.GetAgentList())
			{
				if (!agentModel.IsDead())
				{
					if (!agentModel.IsPanic() && !agentModel.IsCrazy())
					{
						if (!agentModel.CannotControll())
						{
							if (!this.CheckApostle(agentModel))
							{
								list.Add(agentModel);
							}
						}
					}
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			AgentModel agentModel2 = list[UnityEngine.Random.Range(0, list.Count)];
			agentModel2.LoseControl();
			agentModel2.SetUncontrollableAction(new Uncontorllable_PlagueDoctor(agentModel2, this));
			this.RoomSkillSpriteOn();
		}

		// Token: 0x06002745 RID: 10053 RVA: 0x00003FDD File Offset: 0x000021DD
		public void OnNotice(string notice, params object[] param)
		{
		}

		// Token: 0x06002746 RID: 10054 RVA: 0x00115734 File Offset: 0x00113934
		public override void OnSkillGoalComplete(UseSkill skill)
		{
			if (skill.GetCurrentFeelingState() == CreatureFeelingState.GOOD)
			{
				if (skill.agent.hp <= 0f || skill.agent.maxHp <= 0)
				{
					return;
				}
				if (this.CheckApostle(skill.agent))
				{
					return;
				}
				skill.PauseWorking();
				this._currentWork = skill;
				base.Unit.room.StopWorkDesc();
				this.StartKiss(skill.agent);
				skill.agent.RecoverHP((float)skill.agent.maxHp);
				skill.agent.RecoverMental((float)skill.agent.maxMental);
			}
		}

		// Token: 0x06002747 RID: 10055 RVA: 0x001157E0 File Offset: 0x001139E0
		public void StartKiss(AgentModel target)
		{
			this.MakeSound(PlagueDoctor.KissSoundSrc);
			this._currentKissTarget = target;
			this._isKiss = true;
			this.AnimScript.OnSetKiss();
			this._kissTargetInitialScale = target.GetWorkerUnit().animChanger.transform.localScale;
			target.GetWorkerUnit().animChanger.ChangeAnimator(WorkerSpine.AnimatorName.PlagueDoctorAgentCTRL, false);
			target.GetWorkerUnit().animChanger.transform.localScale = this._kissTargetInitialScale * PlagueDoctor._workerAnimatorScale;
			this._currentKissAnim = target.GetWorkerUnit().animChanger.GetComponent<Animator>();
			this._kissDurationTimer.StartTimer(PlagueDoctor._kissTime.GetRandomFloat());
			this.RoomSkillSpriteOn();
			base.Unit.room.TurnOnRoomLight();
			PlagueDoctor.Log("Start Kiss" + target.GetUnitName(), false);
		}

		// Token: 0x06002748 RID: 10056 RVA: 0x0002708B File Offset: 0x0002528B
		public bool IsKissing()
		{
			return this._isKiss;
		}

		// Token: 0x06002749 RID: 10057 RVA: 0x00027093 File Offset: 0x00025293
		private void MoveWorkerToCustomNode(WorkerModel target)
		{
			target.MoveToNode(this.model.GetCustomNode(), false);
		}

		// Token: 0x0600274A RID: 10058 RVA: 0x000270A7 File Offset: 0x000252A7
		public void OnEndKiss()
		{
			this.ClearKissSound();
			this.AnimScript.OnSetKissEnd();
			this._currentKissAnim.SetTrigger("KissEnd");
			this._kissEndTimer.StartTimer(PlagueDoctor._kissEnd_1);
		}

		// Token: 0x0600274B RID: 10059 RVA: 0x001158C0 File Offset: 0x00113AC0
		public void ClearKiss()
		{
			this.ClearKissSound();
			this.RoomSkillSpriteOff();
			this.AnimScript.OnEndKiss();
			if (this._currentWork != null)
			{
				this._currentWork.ResumeWorking();
				this._currentWork = null;
			}
			if (this._currentKissTarget.unconAction is Uncontorllable_PlagueDoctor)
			{
				this._currentKissTarget.ClearUnconCommand();
				this._currentKissTarget.GetControl();
				base.Unit.room.TurnOffRoomLight();
			}
			this._currentKissTarget.GetWorkerUnit().animChanger.ChangeAnimator();
			this._currentKissTarget.GetWorkerUnit().animChanger.transform.localScale = this._kissTargetInitialScale;
			this.AddApostle(this._currentKissTarget);
			this.AnimScript.SkeletonAnim.SetState(this.ApostleLevel);
			this._currentKissTarget = null;
			this._currentKissAnim = null;
			this._isKiss = false;
			this.model.ResetQliphothCounter();
		}

		// Token: 0x0600274C RID: 10060 RVA: 0x000270DA File Offset: 0x000252DA
		public void CancelAttract()
		{
			this.model.ResetQliphothCounter();
			this.RoomSkillSpriteOff();
		}

		// Token: 0x0600274D RID: 10061 RVA: 0x001159B4 File Offset: 0x00113BB4
		public bool CheckApostle(AgentModel target)
		{
			foreach (ApostleData apostleData in this.apostleData)
			{
				if (apostleData.instId == target.instanceId && apostleData.NameId == target._agentName.id)
				{
					return true;
				}
			}
			return target.Equipment.HasEquipment(PlagueDoctor.giftId);
		}

		// Token: 0x0600274E RID: 10062 RVA: 0x00115A54 File Offset: 0x00113C54
		public void OnClockUIEnd()
		{
			if (this.ApostleLevel < 12)
			{
				GameManager.currentGameManager.Resume(PAUSECALL.INGAMEEFFECT);
			}
			else
			{
				PlaySpeedSettingUI.instance.SetNormalSpeedForcely();
				this.GenDeathAngel();
				this.AnimScript.transform.parent.gameObject.SetActive(true);
				this._angel.Unit.Start();
				this._angel.Unit.gameObject.SetActive(false);
				this._angel.movable.SetDirection(UnitDirection.RIGHT);
				List<ApostleGenData> adeventTargets = DeathAngel.GetAdeventTargets(this.apostleData);
				this._genData = adeventTargets;
				this._angel.GenApostle(this._genData);
				this.AnimScript.AdventClockUI.StartAdventEvent();
				base.Unit.room.StopWorkDesc();
				GameManager.currentGameManager.Pause(PAUSECALL.INGAMEEFFECT);
				CameraMover.instance.SetEndCall(new CameraMover.OnCameraMoveEndEvent(this.AnimScript.OnStartAdvent));
				Vector3 position = this.AnimScript._eye1.transform.position;
				position.x -= 0.2f;
				position.y -= 0.9f;
				CameraMover.instance.CameraMoveEvent(position, 4f, 1f);
				this.MakeSound("creature/deathangel/Lucifer_Bell0");
			}
		}

		// Token: 0x0600274F RID: 10063 RVA: 0x00115BA8 File Offset: 0x00113DA8
		public void AddApostle(AgentModel target)
		{
			if (this.CheckApostle(target))
			{
				return;
			}
			this.ClearKissSound();
			GameManager.currentGameManager.Pause(PAUSECALL.INGAMEEFFECT);
			this.AnimScript.StartAdventNameEffect(this.ApostleLevel, target.GetUnitName());
			ApostleData item = new ApostleData(target);
			this.apostleData.Add(item);
			this.AnimScript.SetAdventDesc(this.GetApostleDescRefined(this.ApostleLevel));
			EGOgiftModel gift = InventoryModel.Instance.CreateEquipmentForcely(PlagueDoctor.giftId) as EGOgiftModel;
			target.AttachEGOgift(gift);
			target.SetWorkCommandCheckEvnet(new AgentModel.CheckCommandState(this.BlessedAgentWorkCheck));
			this.SaveScriptData();
		}

		// Token: 0x06002750 RID: 10064 RVA: 0x0001A52D File Offset: 0x0001872D
		public CreatureStaticData.ParameterData GetApostleDesc(int targetindex)
		{
			return this.model.metaInfo.creatureStaticData.GetParam(targetindex);
		}

		// Token: 0x06002751 RID: 10065 RVA: 0x00115C48 File Offset: 0x00113E48
		public string GetApostleDescRefined(int index)
		{
			CreatureStaticData.ParameterData param = this.model.metaInfo.creatureStaticData.GetParam(index);
			string text = param.desc;
			for (int i = this.apostleData.Count - 1; i >= 0; i--)
			{
				try
				{
					AgentName name = AgentNameList.instance.GetName(this.apostleData[i].NameId);
					text = text.Replace("#" + (i + 1), name.GetName());
				}
				catch (NullReferenceException ex)
				{
					text = text.Replace("#" + (i + 1), "Error");
				}
			}
			return text;
		}

		// Token: 0x06002752 RID: 10066 RVA: 0x00115D08 File Offset: 0x00113F08
		public static string GetApostleDescRefined(int index, List<ApostleGenData> data)
		{
			CreatureStaticData creatureStaticData = CreatureTypeList.instance.GetData(PlagueDoctor.ID).creatureStaticData;
			CreatureStaticData.ParameterData param = creatureStaticData.GetParam(index);
			string text = param.desc;
			for (int i = data.Count - 1; i >= 0; i--)
			{
				try
				{
					AgentName name = AgentNameList.instance.GetName(data[i].data.NameId);
					text = text.Replace("#" + (i + 1), name.GetName());
				}
				catch (NullReferenceException ex)
				{
					text = text.Replace("#" + (i + 1), "Error");
				}
			}
			return text;
		}

		// Token: 0x06002753 RID: 10067 RVA: 0x00013CB1 File Offset: 0x00011EB1
		public override bool HasScriptSaveData()
		{
			return true;
		}

		// Token: 0x06002754 RID: 10068 RVA: 0x00115DCC File Offset: 0x00113FCC
		public void ChangeIsolateRoom()
		{
			Sefira sefira = this.model.sefira;
			CreatureModel creatureModel = CreatureManager.instance.PickOtherSefiraCreatureByRandom(this.model);
			if (creatureModel != null)
			{
				Sefira sefira2 = creatureModel.sefira;
				CreatureManager.instance.ChangeCreaturePos(this.model, creatureModel);
				Sefira sefira3 = sefira2;
				creatureModel.sefira = sefira;
				this.model.sefira = sefira3;
				base.Unit.room.TurnOffRoomLight();
				creatureModel.Unit.room.TurnOffRoomLight();
				PlagueDoctor.Log("Change Room With " + creatureModel.metaInfo.name, false);
			}
			else
			{
				PlagueDoctor.Log("Error To Pick Creature", true);
			}
		}

		// Token: 0x06002755 RID: 10069 RVA: 0x000270ED File Offset: 0x000252ED
		public override void OnInitialBuild()
		{
			this._genDay = PlayerModel.instance.GetDay();
			this.SaveScriptData();
		}

		// Token: 0x06002756 RID: 10070 RVA: 0x00115E78 File Offset: 0x00114078
		public override void OnFixedUpdate(CreatureModel creature)
		{
			if (this._kissEndTimer.started && this._kissEndTimer.RunTimer())
			{
				this.ClearKiss();
			}
			if (this._kissDurationTimer.started && this._kissDurationTimer.RunTimer())
			{
				this.OnEndKiss();
			}
		}

		// Token: 0x06002757 RID: 10071 RVA: 0x00027105 File Offset: 0x00025305
		public override bool OnOpenWorkWindow()
		{
			return base.OnOpenWorkWindow() && !this._isKiss;
		}

		// Token: 0x06002758 RID: 10072 RVA: 0x0002711E File Offset: 0x0002531E
		public override bool IsWorkable()
		{
			return !this._isKiss || this._currentWork != null;
		}

		// Token: 0x06002759 RID: 10073 RVA: 0x0002713A File Offset: 0x0002533A
		public override bool HasUniqueCollectionCost(string areaName, out string text)
		{
			text = string.Empty;
			if (areaName == CreatureModel.regionName[1])
			{
				text = "?";
				return true;
			}
			return false;
		}

		// Token: 0x0600275A RID: 10074 RVA: 0x00115ED4 File Offset: 0x001140D4
		public override Dictionary<string, object> GetSaveData()
		{
			return new Dictionary<string, object>
			{
				{
					PlagueDoctor._save[0],
					this._genDay
				},
				{
					PlagueDoctor._save[1],
					this.apostleData.Count
				},
				{
					PlagueDoctor._save[2],
					this.GetApostleSaveData()
				}
			};
		}

		// Token: 0x0600275B RID: 10075 RVA: 0x00115F34 File Offset: 0x00114134
		private Dictionary<int, Dictionary<string, object>> GetApostleSaveData()
		{
			Dictionary<int, Dictionary<string, object>> dictionary = new Dictionary<int, Dictionary<string, object>>();
			for (int i = 0; i < this.apostleData.Count; i++)
			{
				Dictionary<string, object> saveData = this.apostleData[i].GetSaveData();
				dictionary.Add(i, saveData);
			}
			return dictionary;
		}

		// Token: 0x0600275C RID: 10076 RVA: 0x00115F80 File Offset: 0x00114180
		private void LoadApostleSaveData(int max, Dictionary<int, Dictionary<string, object>> data)
		{
			for (int i = 0; i < max; i++)
			{
				Dictionary<string, object> data2 = null;
				if (data.TryGetValue(i, out data2))
				{
					ApostleData apostleData = new ApostleData(data2);
					this.apostleData.Add(apostleData);
					PlagueDoctor.Log("Load Apostle : " + apostleData.NameId, false);
				}
			}
		}

		// Token: 0x0600275D RID: 10077 RVA: 0x00115FE0 File Offset: 0x001141E0
		public override void LoadData(Dictionary<string, object> dic)
		{
			int num = -1;
			this.apostleData.Clear();
			if (!GameUtil.TryGetValue<int>(dic, PlagueDoctor._save[0], ref this._genDay))
			{
				PlagueDoctor.Log("Failed To Read Gen Day", true);
			}
			if (!GameUtil.TryGetValue<int>(dic, PlagueDoctor._save[1], ref num))
			{
				PlagueDoctor.Log("Failed To ApostleData", true);
			}
			else
			{
				if (num == -1)
				{
					return;
				}
				Dictionary<int, Dictionary<string, object>> data = null;
				if (GameUtil.TryGetValue<Dictionary<int, Dictionary<string, object>>>(dic, PlagueDoctor._save[2], ref data))
				{
					this.LoadApostleSaveData(num, data);
				}
			}
		}

		// Token: 0x0600275E RID: 10078 RVA: 0x00116068 File Offset: 0x00114268
		public List<string> GetApostleNames()
		{
			List<string> list = new List<string>();
			foreach (ApostleData apostleData in this.apostleData)
			{
				AgentName name = AgentNameList.instance.GetName(apostleData.NameId);
				if (name == null)
				{
					Debug.LogError("Error To Find ApostleName : " + apostleData.NameId);
				}
				else
				{
					list.Add(name.GetName());
				}
			}
			return list;
		}

		// Token: 0x0600275F RID: 10079 RVA: 0x0002715F File Offset: 0x0002535F
		public bool BlessedAgentWorkCheck()
		{
			return CommandWindow.CommandWindow.CurrentWindow.CurrentTarget != this.model;
		}

		// Token: 0x06002760 RID: 10080 RVA: 0x00027179 File Offset: 0x00025379
		private void ClearKissSound()
		{
			if (this._kissSound != null)
			{
				if (this._kissSound.gameObject != null)
				{
					UnityEngine.Object.Destroy(this._kissSound.gameObject);
				}
				this._kissSound = null;
			}
		}

		// Token: 0x06002761 RID: 10081 RVA: 0x00116108 File Offset: 0x00114308
		public static bool CheckAdvent()
		{
			string text = Application.persistentDataPath + "/creatureData/100014.dat";
			string name = "apostleListCount";
			DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath + "/creatureData");
			if (!directoryInfo.Exists)
			{
				directoryInfo.Create();
			}
			if (!File.Exists(text))
			{
				Debug.Log(text + " doesn't exist");
				return false;
			}
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream fileStream = File.Open(text, FileMode.Open);
			Dictionary<string, object> dic = (Dictionary<string, object>)binaryFormatter.Deserialize(fileStream);
			fileStream.Close();
			int num = -1;
			return GameUtil.TryGetValue<int>(dic, name, ref num) && num == 12;
		}

		// Token: 0x06002762 RID: 10082 RVA: 0x001161B0 File Offset: 0x001143B0
		// Note: this type is marked as 'beforefieldinit'.
		static PlagueDoctor()
		{
		}

		// Token: 0x0400260A RID: 9738
		private const int QliphothCounterMax = 1;

		// Token: 0x0400260B RID: 9739
		public static long ID = 100014L;

		// Token: 0x0400260C RID: 9740
		public static long WhiteNightID = 100015L;

		// Token: 0x0400260D RID: 9741
		public static int giftId = 400014;

		// Token: 0x0400260E RID: 9742
		private const int AdventLevel = 12;

		// Token: 0x0400260F RID: 9743
		private static MinMax _attractFreq = new MinMax(20f, 30f);

		// Token: 0x04002610 RID: 9744
		private static MinMax _kissTime = new MinMax(15f, 20f);

		// Token: 0x04002611 RID: 9745
		private static float _workerAnimatorScale = 1.25f;

		// Token: 0x04002612 RID: 9746
		private const int _apostleCount = 12;

		// Token: 0x04002613 RID: 9747
		public static string KissSoundSrc = "creature/deathangel/Lucifer_Kiss1";

		// Token: 0x04002614 RID: 9748
		public const string Bell = "creature/deathangel/Lucifer_Bell0";

		// Token: 0x04002615 RID: 9749
		public const string TickSound = "creature/deathangel/Lucifer_Tick1";

		// Token: 0x04002616 RID: 9750
		private static float _kissEnd_1 = 8.667f;

		// Token: 0x04002617 RID: 9751
		private static string[] _save = new string[]
		{
			"genDay",
			"apostleListCount",
			"apostleList"
		};

		// Token: 0x04002618 RID: 9752
		private PlagueDoctorAnim _animScript;

		// Token: 0x04002619 RID: 9753
		private AgentModel _currentKissTarget;

		// Token: 0x0400261A RID: 9754
		private UseSkill _currentWork;

		// Token: 0x0400261B RID: 9755
		private Animator _currentKissAnim;

		// Token: 0x0400261C RID: 9756
		private Timer _kissEndTimer = new Timer();

		// Token: 0x0400261D RID: 9757
		private Timer _kissDurationTimer = new Timer();

		// Token: 0x0400261E RID: 9758
		private Timer _kissMovementTimer = new Timer();

		// Token: 0x0400261F RID: 9759
		private Vector3 _kissTargetInitialScale = Vector3.one;

		// Token: 0x04002620 RID: 9760
		private DeathAngel _angel;

		// Token: 0x04002621 RID: 9761
		private List<ApostleGenData> _genData;

		// Token: 0x04002622 RID: 9762
		private SoundEffectPlayer _kissSound;

		// Token: 0x04002623 RID: 9763
		private int _genDay;

		// Token: 0x04002624 RID: 9764
		private List<ApostleData> apostleData = new List<ApostleData>();

		// Token: 0x04002625 RID: 9765
		private bool _isKiss;
	}
}
