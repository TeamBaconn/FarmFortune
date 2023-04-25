public class WorkerFreeState : BaseState
{
    private WorkerEntity entity;
    public WorkerFreeState(WorkerEntity entity) : base("WorkerFreeState")
    {
        this.entity = entity;
    }

    public override void Enter()
    {
        base.Enter();
        entity.target = GameplayManager.Instance.GetRandomSpawn();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (!entity.isMoving)
        {
            entity.LookAt(GameplayManager.Instance.GetRandomSpawn(false));
        }
        if (entity.owner == null) return;
        Land targetLand = null;
        bool playerHasSeed = entity.owner.inventory.GetItemByType<SeedItem>() != null;
        for(int i = entity.owner.landManager.availableLands.Count-1; i >= 0; i--)
        {
            Land land = entity.owner.landManager.availableLands[i];
            if (land.currentWorker != null) continue;
            if ((land.IsLandEmpty() && playerHasSeed) || land.IsLandDecompose())
            {
                targetLand = land;
                continue;
            }
            if (land.ReadyToHarvest())
            {
                targetLand = land;
                break;
            }
        }
        if (targetLand == null) return;
        entity.currentLand = targetLand;
        entity.ChangeState("WorkerWorkState");
    }
}