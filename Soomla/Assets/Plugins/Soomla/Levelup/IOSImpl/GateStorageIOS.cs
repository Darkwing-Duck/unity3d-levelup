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
using System.Runtime.InteropServices;

namespace Soomla.Levelup
{
	public class GateStorageIOS : GateStorage {
		
#if UNITY_IOS && !UNITY_EDITOR

		[DllImport ("__Internal")]
		private static extern void gateStorage_SetOpen(string gateJson,
		                                               [MarshalAs(UnmanagedType.Bool)] bool open,
		                                               [MarshalAs(UnmanagedType.Bool)] bool notify);
		[DllImport ("__Internal")]
		[return:MarshalAs(UnmanagedType.I1)]
		private static extern bool gateStorage_IsOpen(string gateJson);


		override protected void _setOpen(Gate gate, bool open, bool notify) {
			string gateJson = gate.toJSONObject().ToString();
			gateStorage_SetOpen(gateJson, open, notify);
		}
		
		override protected bool _isOpen(Gate gate) {
			string gateJson = gate.toJSONObject().ToString();
			return gateStorage_IsOpen(gateJson);
		}

#endif
	}
}

