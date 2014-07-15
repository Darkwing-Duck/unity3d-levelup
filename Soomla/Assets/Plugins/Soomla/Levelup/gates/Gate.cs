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
using System.Collections.Generic;

namespace Soomla.Levelup {
	
	public abstract class Gate {

		private const string TAG = "SOOMLA Gate";

		public string GateId;

		protected Gate (string gateId)
		{
			this.GateId = gateId;

			registerEvents();
		}
		
//#if UNITY_ANDROID && !UNITY_EDITOR
//		protected Mission(AndroidJavaObject jniVirtualItem) {
//			this.Name = jniVirtualItem.Call<string>("getName");
//			this.Description = jniVirtualItem.Call<string>("getDescription");
//			this.ItemId = jniVirtualItem.Call<string>("getItemId");
//		}
//#endif

		public Gate(JSONObject jsonObj) {
			this.GateId = jsonObj[LUJSONConsts.LU_GATE_GATEID].str;

			registerEvents();
		}

		public virtual JSONObject toJSONObject() {
			JSONObject obj = new JSONObject(JSONObject.Type.OBJECT);
			obj.AddField(LUJSONConsts.LU_GATE_GATEID, this.GateId);
			obj.AddField(JSONConsts.SOOM_CLASSNAME, GetType().Name);
			
			return obj;
		}

		public static Gate fromJSONObject(JSONObject gateObj) {
			string className = gateObj[JSONConsts.SOOM_CLASSNAME].str;
			
			Gate gate = (Gate) Activator.CreateInstance(Type.GetType("Soomla.Levelup." + className), new object[] { gateObj });
			
			return gate;
		}

		// Equality
		
		public override bool Equals(System.Object obj)
		{
			// If parameter is null return false.
			if (obj == null)
			{
				return false;
			}
			
			// If parameter cannot be cast to Point return false.
			Gate g = obj as Gate;
			if ((System.Object)g == null)
			{
				return false;
			}
			
			// Return true if the fields match:
			return (GateId == g.GateId);
		}
		
		public bool Equals(Gate g)
		{
			// If parameter is null return false:
			if ((object)g == null)
			{
				return false;
			}
			
			// Return true if the fields match:
			return (GateId == g.GateId);
		}
		
		public override int GetHashCode()
		{
			return GateId.GetHashCode();
		}

		public static bool operator ==(Gate a, Gate b)
		{
			// If both are null, or both are same instance, return true.
			if (System.Object.ReferenceEquals(a, b))
			{
				return true;
			}
			
			// If one is null, but not both, return false.
			if (((object)a == null) || ((object)b == null))
			{
				return false;
			}
			
			// Return true if the fields match:
			return a.GateId == b.GateId;
		}
		
		public static bool operator !=(Gate a, Gate b)
		{
			return !(a == b);
		}

#if UNITY_ANDROID && !UNITY_EDITOR
		public AndroidJavaObject toJNIObject() {
			using(AndroidJavaClass jniGateClass = new AndroidJavaClass("com.soomla.levelup.gates.Gate")) {
				return jniGateClass.CallStatic<AndroidJavaObject>("fromJSONString", toJSONObject().print());
			}
		}
#endif

		public bool Open() {
			//  check in gate storage if it's already open
			if (GateStorage.IsOpen(this)) {
				return true;
			}
			return openInner();
		}

		public void ForceOpen(bool open) {
			bool isOpen = IsOpen();
			if (isOpen == open) {
				// if it's already open why open it again?
				return;
			}

			GateStorage.SetOpen(this, open);
			if (open) {
				unregisterEvents();
			} else {
				// we can do this here ONLY becasue we check 'isOpen == open' a few lines above.
				registerEvents();
			}
		}

		public bool IsOpen() {
			return GateStorage.IsOpen(this);
		}

		public bool CanOpen() {
			// check in gate storage if the gate is open
			if (GateStorage.IsOpen(this)) {
				return true;
			}

			return canOpenInner();
		}

		protected abstract void registerEvents();
		protected abstract void unregisterEvents();

		protected abstract bool canOpenInner();
		protected abstract bool openInner();
	}
}

