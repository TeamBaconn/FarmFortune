using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

class AssignFarmState : BaseState
{
    private UserInput input;
    private Camera camera;
    public AssignFarmState(UserInput input) : base("AssignFarmState")
    {
        this.input = input;
        this.camera = Camera.main;
        modules.Add(new CameraControlModule(input));
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        //Check for mouse click
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
            Land land = LandManager.GetLand(mousePos, Global.LAND_DETECTION_SIZE);
            if (land != null)
            {
                OnLandClick harvestEvent = new OnLandClick(land, LandAction.HARVEST, null, input.targetPlayer); 
                EventManager.TriggerEvent(harvestEvent);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            OnPlayerClick clickEvent = new OnPlayerClick(input.targetPlayer);
            EventManager.TriggerEvent(clickEvent);
        }
    }
}
