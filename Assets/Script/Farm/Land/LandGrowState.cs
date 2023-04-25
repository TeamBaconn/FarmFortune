using UnityEngine;

public class LandGrowState : BaseState
{
    private Land land; 
    public LandGrowState(Land land) : base("LandGrowState")
    {
        this.land = land;
    }
    public override void Enter()
    {
        base.Enter();
        land.liveTime = 0;
        land.harvestTime = 0;
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
        if (eventParam.action != LandAction.HARVEST) return;
        if (land.GetProgress() >= 1)
        {
            OnHarvest();
        }
    }
    private void OnHarvest()
    {
        //Harvest farm entity
        land.harvestTime++;

        OnHarvestFarmEntity harvestEvent = new OnHarvestFarmEntity(land.entityData, land.farmEntity, land.liveTime, land.harvestTime);
        EventManager.TriggerEvent(harvestEvent);

        if (land.harvestTime >= land.entityData.totalHarvest)
        {
            land.ChangeState("LandFreeState");
            return;
        }
        land.liveTime = 0;
    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        //TODO: CHECK TOOL
        land.liveTime += Time.deltaTime;
        float progress = land.GetProgress();
        land.farmEntity.UpdateStage(progress);
        //Check die
        if (land.liveTime - land.entityData.timeToHarvest >= land.entityData.timeToDecompose)
        {
            land.ChangeState("LandDecomposeState");
        }
    }
}
