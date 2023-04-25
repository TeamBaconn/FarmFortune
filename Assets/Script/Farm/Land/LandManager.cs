using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class LandManager : MonoBehaviour
{
    public static LandManager Instance;
    public List<Land> availableLands;
    public GameObject landPrefab;

    private Player targetPlayer;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        targetPlayer = GetComponent<Player>();
    }

    private void OnEnable()
    {
        EventManager.StartListening<OnPlayerStatUpdate>(OnPlayerStatUpdate);
    }

    private void OnDisable()
    {
        EventManager.StopListening<OnPlayerStatUpdate>(OnPlayerStatUpdate);
    }

    private void OnPlayerStatUpdate(EventParam param)
    {
        OnPlayerStatUpdate eventParam = param as OnPlayerStatUpdate;
        if (!eventParam.player.Equals(targetPlayer)) return;
        CreateLand(eventParam.player.totalLand - availableLands.Count);
    }

    public static Land GetLand(Vector3 location, float minDist = 0f)
    {
        Land res = null;
        float min = -1;
        foreach(Land land in Instance.availableLands)
        {
            float dis = Vector2.Distance(land.gameObject.transform.position, location);
            if (dis > minDist) continue;
            if(min < 0 || min > dis)
            {
                min = dis;
                res = land;
            }
        }

        return res;
    }

    public static void CreateLand(int landNumber)
    {
        int prevLand = Instance.availableLands.Count;
        if (landNumber > 0)
        {
            //Adding land
            for (int i = 0; i < landNumber; i++)
            {
                Land newLand = Instantiate(Instance.landPrefab, GetLandPosition(Instance.availableLands.Count), Quaternion.identity, null).GetComponent<Land>();
                newLand.Init(Instance.targetPlayer);
                Instance.availableLands.Add(newLand);
            }
        }
        else
        {
            //Remove land
            for (int i = 0; i < Mathf.Abs(landNumber); i++)
            {
                if (Instance.availableLands.Count == 0) break;
                int index = Instance.availableLands.Count - 1 - i;
                Land land = Instance.availableLands[index];
                Instance.availableLands.RemoveAt(index);
                Destroy(land.gameObject);
            }
        }
        OnLandNumberChange changeEvent = new OnLandNumberChange(Instance.availableLands.Count, prevLand);
        EventManager.TriggerEvent(changeEvent); 
    }

    private static Vector2 GetLandPosition(int index)
    {
        float landWidth = Global.SQUARE_OFFSET_Y*-1;
        int squareIndex = Mathf.CeilToInt((index) / Global.MAX_LAND_PER_SQUARE);
        
        Vector2 squarePos = new Vector2(0, -(squareIndex) * (landWidth));
        squarePos += new Vector2(-landWidth / 2, landWidth / 2);

        squarePos.x += (index % Global.MAX_LAND_PER_ROW) * (Global.LAND_SIZE + Global.LAND_OFFSET) + Global.LAND_SIZE/2 + Global.LAND_OFFSET;
        squarePos.y -= ((index/Global.MAX_LAND_PER_ROW) % Global.MAX_LAND_PER_ROW) * (Global.LAND_SIZE + Global.LAND_OFFSET) + Global.LAND_SIZE / 2 + Global.LAND_OFFSET;

        return squarePos;
    }
}
