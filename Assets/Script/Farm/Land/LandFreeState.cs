using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandFreeState : BaseState
{
    private Land land;
    public LandFreeState(Land land) : base("LandFreeState")
    {
        this.land = land;
    }
    public override void Enter()
    {
        base.Enter();
        land.Reset();
        EventManager.StartListening<OnItemDrop>(OnItemDrop);
    }
    public override void Exit()
    {
        base.Exit();
        EventManager.StopListening<OnItemDrop>(OnItemDrop);
    }

    private void OnItemDrop(EventParam param)
    {
        OnItemDrop eventParam = param as OnItemDrop;
        if (!eventParam.player.Equals(land.owner)) return;

        if (Vector2.Distance(eventParam.dropPosition, land.transform.position) > Global.LAND_DETECTION_SIZE) return;
        if (eventParam.item == null || !(eventParam.item is SeedItem data)) return;
        if (eventParam.IsEventCanceled()) return;

        if (eventParam.player.inventory.GetAmount(eventParam.item) == 0) return;
        eventParam.player.inventory.AddItem(eventParam.item, -1);

        eventParam.CancelEvent();
        land.GrowPlant(data.seedData);
    }
}
