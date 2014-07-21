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
	public class ActionMission : Mission
	{
		public ActionMission(string id, string name)
			: base(id, name)
		{
		}

		public ActionMission(string id, string name, List<Reward> rewards)
			: base(id, name, rewards)
		{
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		public ActionMission(JSONObject jsonMission)
			: base(jsonMission)
		{
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <returns>see parent</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();

			return obj;
		}

		protected override void registerEvents() {
		}
		
		protected override void unregisterEvents() {

		}
	}
}

