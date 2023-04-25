using UnityEngine;
class PickItemState : BaseState
{
    private UserInput input;
    private Camera camera;
    public PickItemState(UserInput input) : base("PickItemState")
    {
        this.input = input;
        this.camera = Camera.main;
        modules.Add(new CameraControlModule(input));
    }
    public override void Enter()
    {
        base.Enter();
        EventManager.StartListening<OnInventoryUpdate>(OnInventoryUpdate);
    }

    public override void Exit()
    {
        base.Exit();
        input.holdItem = null;
        EventManager.StopListening<OnInventoryUpdate>(OnInventoryUpdate);
    }

    private void OnInventoryUpdate(EventParam param)
    {
        if (input.holdItem == null) return;
        OnInventoryUpdate eventParam = param as OnInventoryUpdate;
        if (eventParam.player.Equals(input.targetPlayer))
        {
            //Check for main player
            if (!eventParam.updatedInventory.TryGetValue(input.holdItem, out int value) || value == 0)
            {
                OnItemDrop dropEvent = new OnItemDrop(null, input.targetPlayer, camera.ScreenToWorldPoint(Input.mousePosition));
                EventManager.TriggerEvent(dropEvent);
                input.ResetState();
                return;
            }
        }

    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (input.holdItem == null) input.ResetState();
        if (!Input.GetMouseButton(0))
        {
            DropItem();
        }
    }
    private void DropItem()
    {
        OnItemDrop dropEvent = new OnItemDrop(input.holdItem, input.targetPlayer, camera.ScreenToWorldPoint(Input.mousePosition));
        EventManager.TriggerEvent(dropEvent);

        if (input.holdItem == null || !(input.holdItem is SeedItem)) return;

        Vector2 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        Land land = LandManager.GetLand(mousePos, Global.LAND_SIZE / 2 * 0.6f);
        if (land != null)
        {
            OnLandClick clickEvent = new OnLandClick(land, LandAction.PLANT, ((SeedItem)input.holdItem), input.targetPlayer);
            EventManager.TriggerEvent(clickEvent);
        }
    }
}