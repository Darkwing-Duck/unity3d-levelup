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


using System.Collections;
using System.Collections.Generic;

namespace Soomla.Levelup
{
	public class GatesListOR : GatesList
	{

		public GatesListOR(string gateId)
			: base(gateId)
		{
			Gates = new List<Gate>();
		}

		public GatesListOR(string gateId, Gate singleGate)
			: base(gateId, singleGate)
		{
		}

		public GatesListOR(string gateId, List<Gate> gates)
			: base(gateId, gates)
		{
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		public GatesListOR(JSONObject jsonGate)
			: base(jsonGate)
		{
		}

		protected override bool canOpenInner() {
			foreach (Gate gate in Gates) {
				if (!gate.IsOpen()) {
					return true;
				}
			}
			return false;
		}

	}
}

