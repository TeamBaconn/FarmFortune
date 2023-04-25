using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmEntityData : ScriptableObject
{
    public GameObject entityPrefab;
    
    public Item harvestItem;
    public int harvestItemAmount = 1;

    public int timeToHarvest;
    public int totalHarvest = 5;
    public int timeToDecompose = 3600;
    public virtual GameObject Spawn(Land land)
    {
        GameObject newEntityPrefab = Instantiate(entityPrefab, land.transform.position, Quaternion.identity, land.transform);
        return newEntityPrefab;
    }
}
