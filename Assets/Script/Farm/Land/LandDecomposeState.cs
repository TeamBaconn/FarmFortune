using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandDecomposeState : BaseState
{
    private Land land;
    public LandDecomposeState(Land land) : base("LandDecomposeState")
    {
        this.land = land;
    }
    public override void Enter()
    {
        base.Enter();
        land.farmEntity.UpdateStage(-1);
        EventManager.StartListening<OnLandClick>(OnLandClick);
    }
    public override void Exit()
    {
        base.Exit();
        EventManager.StopListening<OnLandClick>(OnLandClick);
    }

    private void OnLandClick(EventParam param)
    {
        OnLandClick eventParam = param as OnLandClick;
        if (!eventParam.player.Equals(land.owner)) return;
        if (eventParam.land != land) return;
        if ((eventParam.action != LandAction.REMOVE && eventParam.action != LandAction.HARVEST)) return;
        
        land.ChangeState("LandFreeState");
    }
}
