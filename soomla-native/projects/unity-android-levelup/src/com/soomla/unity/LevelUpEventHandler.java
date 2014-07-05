package com.soomla.unity;

import com.soomla.BusProvider;
import com.soomla.levelup.events.GateOpenedEvent;
import com.soomla.levelup.events.LevelEndedEvent;
import com.soomla.levelup.events.LevelStartedEvent;
import com.soomla.levelup.events.MissionCompletedEvent;
import com.soomla.levelup.events.MissionCompletionRevokedEvent;
import com.soomla.levelup.events.ScoreRecordChangedEvent;
import com.soomla.levelup.events.WorldAssignedRewardEvent;
import com.soomla.levelup.events.WorldCompletedEvent;
import com.squareup.otto.Subscribe;
import com.unity3d.player.UnityPlayer;

public class LevelUpEventHandler {
    private static LevelUpEventHandler mLocalEventHandler;

    public static void initialize() {
        mLocalEventHandler = new LevelUpEventHandler();

    }

    public LevelUpEventHandler() {
        BusProvider.getInstance().register(this);
    }

    @Subscribe
    public void onLevelStartedEvent(LevelStartedEvent levelStartedEvent) {
        UnityPlayer.UnitySendMessage("LevelUpEvents", "onLevelStarted", levelStartedEvent.Level.toJSONObject().toString());
    }

    @Subscribe
    public void onLevelEndedEvent(LevelEndedEvent levelEndedEvent) {
        UnityPlayer.UnitySendMessage("LevelUpEvents", "onLevelEnded", levelEndedEvent.Level.toJSONObject().toString());
    }

    @Subscribe
    public void onWorldCompletedEvent(WorldCompletedEvent worldCompletedEvent) {
        UnityPlayer.UnitySendMessage("LevelUpEvents", "onWorldCompleted", worldCompletedEvent.World.toJSONObject().toString());
    }

    @Subscribe
    public void onGateOpenedEvent(GateOpenedEvent gateOpenedEvent) {
        UnityPlayer.UnitySendMessage("LevelUpEvents", "onGateOpened", gateOpenedEvent.Gate.toJSONObject().toString());
    }

    @Subscribe
    public void onMissionCompletedEvent(MissionCompletedEvent missionCompletedEvent) {
        UnityPlayer.UnitySendMessage("LevelUpEvents", "onMissionCompleted", missionCompletedEvent.Mission.toJSONObject().toString());
    }

    @Subscribe
    public void onMissionCompletionRevokedEvent(MissionCompletionRevokedEvent missionCompletionRevokedEvent) {
        UnityPlayer.UnitySendMessage("LevelUpEvents", "onMissionCompletionRevoked", missionCompletionRevokedEvent.Mission.toJSONObject().toString());
    }

    @Subscribe
    public void onScoreRecordChangedEvent(ScoreRecordChangedEvent scoreRecordChangedEvent) {
        UnityPlayer.UnitySendMessage("LevelUpEvents", "onScoreRecordChanged", scoreRecordChangedEvent.Score.toJSONObject().toString());
    }

    @Subscribe
    public void onWorldAssignedRewardEvent(WorldAssignedRewardEvent worldAssignedRewardEvent) {
        UnityPlayer.UnitySendMessage("LevelUpEvents", "onWorldAssignedReward", worldAssignedRewardEvent.World.toJSONObject().toString());
    }
}
