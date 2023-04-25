using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandInteract : Interactable
{
    private Land land;

    private CursorMessage currentMessage;

    protected override void Awake()
    {
        base.Awake();
        land = GetComponent<Land>();
    }


    private void OnEnable()
    { 
    }

    private void OnDisable()
    {
        ClearMessage();
    }

    protected override void OnMouseEnter()
    {
        string title = null, description = null;
        if (land == null) return;
        if (land.IsLandEmpty())
        {
            title = "Empty land";
            description = "Try to plant some seed";
        }
        else if (land.ReadyToHarvest())
        {
            title = $"<color=\"green\">Harvest {land.entityData.harvestItem.itemName}</color>";
            description = "Click to harvest" +
                "\n" +
                $"Decompose time: {GetTime((int)land.liveTime - land.entityData.timeToHarvest)} / {GetTime(land.entityData.timeToDecompose)}";
        }
        else if (land.IsLandDecompose())
        {
            title = $"<color=\"red\">Decomposed</color>";
            description = "Click to clean it up";
        }
        else if (land.entityData != null)
        {
            title = land.entityData.harvestItem.itemName;
            description = $"Grow time: {GetTime((int)land.liveTime)} / {GetTime(land.entityData.timeToHarvest)}" +
                $"\n" +
                $"Harvest amount: {NumberExtensions.FormatNumber(land.entityData.harvestItemAmount)}";
        }
        if (title == null || description == null) return;
        currentMessage = new CursorMessage(title, description);
        EventManager.TriggerEvent(new OnCursorMessageRequest(currentMessage, true));
        base.OnMouseEnter();
    }
    private void Update()
    {
        if(currentMessage != null)
        {
            OnMouseEnter();
        }
    }
    private string GetTime(int seconds)
    {
        return $"{(seconds / 60 > 0 ? (seconds / 60).ToString() + "m " : "")}{(seconds % 60 > 0 ? (seconds % 60).ToString() + "s " : "")}";
    }
    protected override void OnMouseExit()
    {
        ClearMessage();
        base.OnMouseExit();
    }

    private void ClearMessage()
    {
        if (currentMessage != null)
        {
            EventManager.TriggerEvent(new OnCursorMessageRequest(currentMessage, false));
            currentMessage = null;
        }
    }
}
