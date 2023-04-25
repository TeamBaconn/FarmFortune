using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Plant Entity", menuName = "Farm/Create Plant Entity", order = 2)]
public class PlantEntity : FarmEntityData
{
    public List<Sprite> plantSprites;
    public Sprite plantDieSprite;

    public override GameObject Spawn(Land land)
    {
        GameObject entity = base.Spawn(land);

        entity.GetComponent<PlantRenderer>().plantSprites = plantSprites;
        entity.GetComponent<PlantRenderer>().plantDieSprite = plantDieSprite;

        return entity;
    }
}
