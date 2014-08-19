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

namespace Soomla.Levelup
{
	/// <summary>
	/// A utility class for persisting and querying the state of missions.
	/// Use this class to check if a certain mission is complete, or to
	/// set its completion status.
	/// </summary>
	public class MissionStorage
	{

		protected const string TAG = "SOOMLA MissionStorage"; // used for Log error messages

		static MissionStorage _instance = null;
		static MissionStorage instance {
			get {
				if(_instance == null) {
					#if UNITY_ANDROID && !UNITY_EDITOR
					_instance = new MissionStorageAndroid();
					#elif UNITY_IOS && !UNITY_EDITOR
					_instance = new MissionStorageIOS();
					#else
					_instance = new MissionStorage();
					#endif
				}
				return _instance;
			}
		}
			

		/** Mission Completion **/

		/// <summary>
		/// Sets the given mission as completed if <c>completed</c> is <c>true</c>.
		/// </summary>
		/// <param name="mission">Mission to set as completed.</param>
		/// <param name="completed">If set to <c>true</c> mission will be set 
		/// as completed.</param>
		public static void SetCompleted(Mission mission, bool completed) {
			SetCompleted (mission, completed, true);
		}
		public static void SetCompleted(Mission mission, bool completed, bool notify) {
			instance._setTimesCompleted(mission, completed, notify);
		}

		/// <summary>
		/// Determines if the given mission is complete.
		/// </summary>
		/// <returns>If the given mission is completed returns <c>true</c>;
		/// otherwise <c>false</c>.</returns>
		/// <param name="mission">Mission to determine if complete.</param>
		public static bool IsCompleted(Mission mission) {
			return GetTimesCompleted(mission) > 0;
		}

		/// <summary>
		/// Retrieves the number of times the given mission has been completed.
		/// </summary>
		/// <returns>The number of times the given mission has been completed.</returns>
		/// <param name="mission">Mission.</param>
		public static int GetTimesCompleted(Mission mission) {
			return instance._getTimesCompleted(mission);
		}


		/** Mission Completion Helpers **/

		protected virtual void _setTimesCompleted(Mission mission, bool up, bool notify) {
#if UNITY_EDITOR
			int total = _getTimesCompleted(mission) + (up ? 1 : -1);
			if(total<0) total = 0;

			string key = keyMissionTimesCompleted(mission.ID);
			PlayerPrefs.SetString(key, total.ToString());
			
			if (notify) {
				if (up) {
					LevelUpEvents.OnMissionCompleted(mission);
				} else {
					LevelUpEvents.OnMissionCompletionRevoked(mission);
				}
			}
#endif
		}


		protected virtual int _getTimesCompleted(Mission mission) {
#if UNITY_EDITOR
			string key = keyMissionTimesCompleted(mission.ID);
			string val = PlayerPrefs.GetString (key);
			if (string.IsNullOrEmpty(val)) {
				return 0;
			}
			return int.Parse(val);
#else
			return 0;
#endif
		}



		/** Keys **/

#if UNITY_EDITOR
		private static string keyMissions(string missionId, string postfix) {
			return LevelUp.DB_KEY_PREFIX + "missions." + missionId + "." + postfix;
		}
		
		private static string keyMissionTimesCompleted(string missionId) {
			return keyMissions(missionId, "timesCompleted");
		}
#endif

	}
}

