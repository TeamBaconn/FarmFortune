using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantRenderer : MonoBehaviour, IFarmEntity
{
    [HideInInspector]
    public List<Sprite> plantSprites;

    [HideInInspector]
    public Sprite plantDieSprite;

    private List<SpriteRenderer> rendererList;

    private void Awake()
    {
        rendererList = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
        UpdateStage(0);
    }

    public void UpdateStage(float ratio)
    {
        if(plantSprites.Count == 0 || plantDieSprite == null)
        {
            Debug.LogWarning("Plant doesn't have sprite");
            return;
        }

        Sprite sprite;
        if (ratio < 0) sprite = plantDieSprite;
        else
        {
            ratio = Mathf.Clamp(ratio, 0, 1);
            int index = (int)(ratio * (plantSprites.Count - 1));
            sprite = plantSprites[index];
        }

        foreach(SpriteRenderer renderer in rendererList)
        {
            renderer.sprite = sprite;
        }
    }
}
