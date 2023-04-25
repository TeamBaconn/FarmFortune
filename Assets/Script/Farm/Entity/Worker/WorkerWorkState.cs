using UnityEngine;

public class WorkerWorkState : BaseState
{
    private WorkerEntity entity;
    private float currentWorkTime;
    public WorkerWorkState(WorkerEntity entity) : base("WorkerWorkState")
    {
        this.entity = entity;
    }

    public override void Enter()
    {
        base.Enter();
        entity.owner.AddWorkingWorker(1);
        if (entity.currentLand == null || entity.owner == null)
        {
            entity.ResetState();
            return;
        }
        entity.currentLand.currentWorker = entity;
        entity.target = entity.currentLand.transform.position;
        currentWorkTime = 0;
    }

    public override void Exit()
    {
        base.Exit();
        entity.owner.AddWorkingWorker(-1);
        if (entity.currentLand.currentWorker.Equals(entity)) entity.currentLand.currentWorker = null;
        entity.animator.SetBool("isWorking", false);
        entity.currentLand = null;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (entity.owner == null) return;
        entity.animator.SetBool("isWorking", !entity.isMoving);
        if(entity.currentLand == null)
        {
            entity.ResetState();
            return;
        }
        SeedItem seedItem = entity.owner.inventory.GetItemByType<SeedItem>();
        if (!(entity.currentLand.IsLandEmpty() && seedItem != null) && !entity.currentLand.ReadyToHarvest() && !entity.currentLand.IsLandDecompose())
        {
            entity.ResetState();
            return;
        }
        if (entity.isMoving) return;
        currentWorkTime += Time.deltaTime;
        if(currentWorkTime >= entity.workTime)
        {
            //Finish working
            Work();
            entity.ResetState();
            return;
        }
    }

    private void Work()
    {
        if(entity.currentLand.IsLandEmpty())
        {
            SeedItem seedItem = entity.owner.inventory.GetItemByType<SeedItem>();
            if (seedItem == null) return;
            OnItemDrop drop = new OnItemDrop(seedItem, entity.owner, entity.currentLand.transform.position);
            EventManager.TriggerEvent(drop);
        }
        else if(entity.currentLand.ReadyToHarvest() || entity.currentLand.IsLandDecompose())
        {
            OnLandClick clickEvent = new OnLandClick(entity.currentLand, LandAction.HARVEST, null, entity.owner);
            EventManager.TriggerEvent(clickEvent);
        }
    }
}