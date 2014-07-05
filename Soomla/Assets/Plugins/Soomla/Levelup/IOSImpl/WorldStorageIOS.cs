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
/// See the License for the specific language governing perworlds and
/// limitations under the License.

using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace Soomla.Levelup
{
	public class WorldStorageIOS : WorldStorage {
#if UNITY_IOS && !UNITY_EDITOR
	
		[DllImport ("__Internal")]
		private static extern void worldStorage_SetCompleted(string worldJson,
		                                                       [MarshalAs(UnmanagedType.Bool)] bool completed,
		                                                       [MarshalAs(UnmanagedType.Bool)] bool notify);
		[DllImport ("__Internal")]
		private static extern void worldStorage_SetBadge(string worldJson, string badgeRewardId);

		[DllImport ("__Internal")]
		[return:MarshalAs(UnmanagedType.I1)]
		private static extern bool worldStorage_IsCompleted(string worldJson);

		[DllImport ("__Internal")]
		private static extern void worldStorage_GetAssignedBadge(string worldJson, out IntPtr json);
		
		
		override protected void _setCompleted(World world, bool completed, bool notify) {
			string worldJson = world.toJSONObject().ToString();
			worldStorage_SetCompleted(worldJson, completed, notify);
		}
		
		override protected bool _isCompleted(World world) {
			string worldJson = world.toJSONObject().ToString();
			return worldStorage_IsCompleted(worldJson);
		}

		override protected void _setBadge(World world, string badgeRewardId) {
			string worldJson = world.toJSONObject().ToString();
			worldStorage_SetBadge(worldJson, badgeRewardId);
		}
		
		override protected string _getAssignedBadge(World world) {
			string worldJson = world.toJSONObject().ToString();

			IntPtr p = IntPtr.Zero;
			worldStorage_GetAssignedBadge(worldJson, out p);
//			IOS_ErrorCodes.CheckAndThrowException(err);
			
			string rewardBadgeId = Marshal.PtrToStringAnsi(p);
			Marshal.FreeHGlobal(p);

			return rewardBadgeId;
		}
#endif
	}
}

