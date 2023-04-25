using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LandInteract))]
public class Land : StateMachine
{
    public IFarmEntity farmEntity;
    public FarmEntityData entityData;

    public float liveTime = 0;
    public float harvestTime = 0;

    public Player owner;
    public WorkerEntity currentWorker;

    private void Awake()
    {
        defaultState = "LandFreeState";
        stateList.Add(new LandFreeState(this));
        stateList.Add(new LandGrowState(this));
        stateList.Add(new LandDecomposeState(this));
    }

    public void Init(Player owner)
    {
        this.owner = owner;
    }

    public bool IsLandEmpty()
    {
        return IsStateName("LandFreeState");
    }

    public bool ReadyToHarvest()
    {
        return (IsStateName("LandGrowState") && GetProgress() >= 1);
    }
    
    public bool IsLandDecompose()
    {
        return IsStateName("LandDecomposeState");
    }

    public float GetProgress()
    {
        return (float)liveTime / entityData.timeToHarvest;
    }

    public void Reset()
    {
        foreach (Transform child in transform) Destroy(child.gameObject);
        farmEntity = null;
        entityData = null;
    }

    public void GrowPlant(FarmEntityData data)
    {
        farmEntity = data.Spawn(this).GetComponent<IFarmEntity>();
        entityData = data;
        ChangeState("LandGrowState");
    }
}
