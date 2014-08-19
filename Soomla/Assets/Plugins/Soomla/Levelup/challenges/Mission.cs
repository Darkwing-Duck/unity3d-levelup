/// Copyright (C) 2012-2014 Soomla Inc.
///
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///      http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Soomla.Levelup {
	
	public abstract class Mission : SoomlaEntity<Mission> {

		private const string TAG = "SOOMLA Mission";

		public List<Reward> Rewards;
		public Schedule Schedule;
		protected Gate Gate;

		public string AutoGateId {
			get { return "gate_" + this._id; }
		}

		protected Mission (String id, String name) 
			: this(id, name, null, null)
		{
		}

		protected Mission (String id, String name, List<Reward> rewards) 
			: this(id, name, rewards, null, null)
		{
		}

		protected Mission (String id, String name, Type gateType, object[] gateInitParams)
			: this(id, name, new List<Reward>(), gateType, gateInitParams)
		{
		}

		protected Mission (String id, String name, List<Reward> rewards, Type gateType, object[] gateInitParams)
			: base(id, name, "")
		{
			this.Rewards = rewards;
			if (gateType != null) {
				this.Gate = (Soomla.Levelup.Gate) Activator.CreateInstance(gateType, new object[] { AutoGateId }.Concat(gateInitParams).ToArray());
			}

			Schedule = Schedule.AnyTimeOnce();

			registerEvents();
		}

		public Mission(JSONObject jsonObj)
			: base(jsonObj)
		{
			this.Rewards = new List<Reward>();
			List<JSONObject> jsonRewardList = jsonObj [JSONConsts.SOOM_REWARDS].list;
			foreach (JSONObject jsonRewardObj in jsonRewardList) {
				this.Rewards.Add(Reward.fromJSONObject(jsonRewardObj));
			}

			this.Gate = Gate.fromJSONObject(jsonObj[LUJSONConsts.LU_GATE]);
			if (jsonObj[JSONConsts.SOOM_SCHEDULE]) {
				this.Schedule = new Schedule(jsonObj[JSONConsts.SOOM_SCHEDULE]);
			}
			registerEvents();
		}

		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();

			JSONObject rewardsArr = new JSONObject(JSONObject.Type.ARRAY);
			foreach(Reward reward in this.Rewards) {
				rewardsArr.Add(reward.toJSONObject());
			}
			obj.AddField(JSONConsts.SOOM_REWARDS, rewardsArr);

			obj.AddField(LUJSONConsts.LU_GATE, Gate.toJSONObject());
			obj.AddField(JSONConsts.SOOM_SCHEDULE, Schedule.toJSONObject());

			return obj;
		}

		public static Mission fromJSONObject(JSONObject missionObj) {
			string className = missionObj[JSONConsts.SOOM_CLASSNAME].str;
			
			Mission mission = (Mission) Activator.CreateInstance(Type.GetType("Soomla.Levelup." + className), new object[] { missionObj });
			
			return mission;
		}

#if UNITY_ANDROID && !UNITY_EDITOR
		public AndroidJavaObject toJNIObject() {
			using(AndroidJavaClass jniClass = new AndroidJavaClass("com.soomla.levelup.challenges.Mission")) {
				string json = toJSONObject().print();
				SoomlaUtils.LogError(TAG, "json:"+json);
				return jniClass.CallStatic<AndroidJavaObject>("fromJSONString", json);
			}
		}
#endif

		protected virtual void registerEvents() {
			if (!IsCompleted() && this.Gate != null) {
				LevelUpEvents.OnGateOpened += onGateOpened;
			}
		}
		
		private void onGateOpened(Gate gate) {
			if(this.Gate == gate) {
				Gate.ForceOpen(false);
				setCompletedInner(true);
			}
		}

		public virtual bool IsAvailable() {
			return Gate.CanOpen() && Schedule.Approve(MissionStorage.GetTimesCompleted(this));
		}

		public virtual bool IsCompleted() {
			// check if completed in Mission Storage
			// this checks if the mission was ever completed... no matter how many times.
			return MissionStorage.IsCompleted (this);
		}

		public bool Complete() {
			if (!Schedule.Approve(MissionStorage.GetTimesCompleted(this))) {
				SoomlaUtils.LogDebug(TAG, "missions cannot be completed b/c Schedule doesn't approve.");
				return false;
			}
			SoomlaUtils.LogDebug(TAG, "trying opening gate to complete mission: " + ID);
			return Gate.Open();
		}

		// this function ignores Schedule, it's not supposed to be used in standard scenarios.
		public void ForceComplete() {
			Gate.ForceOpen(true);
		}

		// NOTE: We are allowing missions to be completed multiple times.
		protected void setCompletedInner(bool completed) {
			// set completed in Mission Storage
			MissionStorage.SetCompleted (this, completed);

			if (completed) {

				giveRewards();
			} else {
				takeRewards();
			}
		}

		private void takeRewards() {
			foreach (Reward reward in Rewards) {
				reward.Take();
			}
		}
		
		private void giveRewards() {
			// The mission is completed, giving the rewards.
			foreach (Reward reward in Rewards) {
				reward.Give();
			}
		}

		public override Mission Clone(string newMissionId) {
			return (Mission) base.Clone(newMissionId);
		}
	}
}

