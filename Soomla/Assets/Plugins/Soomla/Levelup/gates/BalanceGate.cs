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
/// limitations under the License.using System;

using Soomla.Store;

namespace Soomla.Levelup
{
	public class BalanceGate : Gate
	{
		private const string TAG = "SOOMLA BalanceGate";

		public string AssociatedItemId;
		public int DesiredBalance;

		public BalanceGate(string gateId, string associatedItemId, int desiredBalance)
			: base(gateId)
		{
			AssociatedItemId = associatedItemId;
			DesiredBalance = desiredBalance;
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		public BalanceGate(JSONObject jsonGate)
			: base(jsonGate)
		{
			this.AssociatedItemId = jsonGate[JSONConsts.SOOM_ASSOCITEMID].str;
			this.DesiredBalance = jsonGate[JSONConsts.SOOM_DESIRED_BALANCE].n;
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <returns>see parent</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(JSONConsts.SOOM_ASSOCITEMID, this.AssociatedItemId);
			obj.AddField(JSONConsts.SOOM_DESIRED_BALANCE, this.DesiredBalance);

			return obj;
		}

		// TODO: register for events and handle them

		public override bool CanOpen() {
			// check in gate storage if the gate is open
			if (GateStorage.IsOpen(this)) {
				return true;
			}

			try {
				if (StoreInventory.GetItemBalance(AssociatedItemId) < DesiredBalance) {
					return false;
				}
			} catch (VirtualItemNotFoundException e) {
				SoomlaUtils.LogError(TAG, "(canPass) Couldn't find itemId. itemId: " + AssociatedItemId);
				return false;
			}
			return true;
		}

		protected override bool TryOpenInner() {
			if (CanOpen()) {

				try {
					StoreInventory.TakeItem(AssociatedItemId, DesiredBalance);
				} catch (VirtualItemNotFoundException e) {
					SoomlaUtils.LogError(TAG, "(open) Couldn't find itemId. itemId: " + AssociatedItemId);
					return false;
				}
				
				ForceOpen(true);
				return true;
			}
			
			return false;
		}
	}
}

