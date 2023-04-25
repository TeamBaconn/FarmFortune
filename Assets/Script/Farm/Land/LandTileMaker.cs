using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandTileMaker : MonoBehaviour
{
    public GameObject topLand;
    public GameObject midLand;
    public GameObject botLand;

    private List<GameObject> spawnLand;

    private void OnEnable()
    {
        EventManager.StartListening<OnLandNumberChange>(OnLandNumberChange);
    }

    private void OnDisable()
    {
        EventManager.StopListening<OnLandNumberChange>(OnLandNumberChange);
    }

    private void OnLandNumberChange(EventParam param)
    { 
        OnLandNumberChange eventParam = param as OnLandNumberChange;
        CreateLand(Mathf.CeilToInt((eventParam.newLandNumber-1) / Global.MAX_LAND_PER_SQUARE) + 1, transform.position);
    }

    public void CreateLand(int landNumber, Vector2 initPosition)
    {
        if(spawnLand != null)
        {
            foreach (GameObject land in spawnLand) Destroy(land);
            spawnLand.Clear();
        }else spawnLand = new List<GameObject>();
        if(landNumber <= 0)
        {
            landNumber = 1;
            Debug.LogWarning("Create land must have landNumber greater than 0");
        }
        initPosition -= new Vector2(0, Global.SQUARE_OFFSET_Y);
        GameObject newTopLand = Instantiate(topLand, initPosition, Quaternion.identity, transform);
        newTopLand.SetActive(true);
        spawnLand.Add(newTopLand);

        for(int i = 0; i < landNumber; i++)
        {
            GameObject land;
            initPosition += new Vector2(0, Global.SQUARE_OFFSET_Y);

            if(i == landNumber - 1)
            {
                land = Instantiate(botLand, initPosition, Quaternion.identity, transform);
            }
            else
            {
                land = Instantiate(midLand, initPosition, Quaternion.identity, transform);
            }

            land.SetActive(true);
            spawnLand.Add(land);
        }
    }
}
