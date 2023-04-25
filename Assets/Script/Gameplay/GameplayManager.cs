using System;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;
    [Header("Game goal")]
    public int moneyGoal = 1000000;
    [Header("Level settings")]
    public List<ItemInfo> starterItems;
    public GameObject workerRestPlace;
    public int startWorker = 1;
    public int startLand = 3;
    public int startToolLevel = 1;
    public int startMoney = 0;

    private float saveTime = 0;

    public GameState currentState;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        LoadGame();
    }

    private void Update()
    {
        saveTime += Time.deltaTime;
        if(saveTime >= Global.DEFAULT_SAVE_TIME)
        {
            Debug.Log("Saving game");
            SaveGame();
            saveTime = 0;
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void OnEnable()
    {
        EventManager.StartListening<OnPlayerStatUpdate>(OnPlayerStatUpdate);
    }

    private void OnDisable()
    {
        EventManager.StopListening<OnPlayerStatUpdate>(OnPlayerStatUpdate);
        SaveGame();
    }

    private void OnPlayerStatUpdate(EventParam param)
    {
        OnPlayerStatUpdate eventParam = param as OnPlayerStatUpdate;
        if (!eventParam.player.Equals(Player.main)) return;
        if(eventParam.player.totalMoney >= moneyGoal && currentState != GameState.GAME_WIN_END)
        {
            SetState(GameState.GAME_WIN_END);
            SaveGame();
            return;
        }

    }

    public Vector2 GetRandomSpawn(bool random = true)
    {
        Vector2 direction = UnityEngine.Random.insideUnitSphere.normalized;
        return (Vector2)workerRestPlace.transform.position + (random ? direction * 3.5f + direction * UnityEngine.Random.Range(0, 1) : Vector2.zero);
    }

    public void SaveGame()
    {
        //Will have to implement encrypt IO system but I dont have time lol
        PlayerPrefs.SetInt("Init", 1);
        PlayerPrefs.SetInt("state", (int)currentState);
        PlayerPrefs.SetInt("money", Player.main.totalMoney);
        PlayerPrefs.SetInt("land", Player.main.landManager.availableLands.Count);
        PlayerPrefs.SetInt("worker", Player.main.totalWorker);
        PlayerPrefs.SetInt("tool", Player.main.toolLevel);
        PlayerPrefs.SetString("time", DateTime.Now.Ticks.ToString());
        for (int i = 0; i < Player.main.landManager.availableLands.Count; i++)
        {
            Land land = Player.main.landManager.availableLands[i];
            Item seedItem = ItemManager.GetSeedFromPlant(land.entityData);
            if (land.IsLandEmpty() || seedItem == null)
            {
                PlayerPrefs.SetInt($"land_{i}_flag", 0);
                continue;
            }
            PlayerPrefs.SetInt($"land_{i}_flag", 1);
            PlayerPrefs.SetInt($"land_{i}_seed", seedItem.id);
            PlayerPrefs.SetFloat($"land_{i}_liveTime", land.liveTime);
            PlayerPrefs.SetFloat($"land_{i}_harvestTime", land.harvestTime);
        }

        PlayerPrefs.SetInt($"item_count", Player.main.inventory.items.Keys.Count);
        int index = 0;
        foreach (Item item in Player.main.inventory.items.Keys)
        {
            PlayerPrefs.SetInt($"item_{index}_id", item.id);
            PlayerPrefs.SetInt($"item_{index}_amount", Player.main.inventory.items[item]);
            index++;
        }
    }

    public void LoadGame()
    {
        int init = PlayerPrefs.GetInt("Init");
        if (init < 1) {
            Debug.Log("No save file, loading new game..");
            SetState(GameState.NEW_GAME);
            return;
        }

        int state = PlayerPrefs.GetInt("state");
        currentState = (GameState)state == GameState.NEW_GAME ? GameState.LOAD_GAME : (GameState)state; 

        int money = PlayerPrefs.GetInt("money");
        Player.main.totalMoney = money;
        int land = PlayerPrefs.GetInt("land");
        Player.main.totalLand = land;
        int worker = PlayerPrefs.GetInt("worker");
        Player.main.totalWorker = worker;
        int toolLevel = PlayerPrefs.GetInt("tool");
        Player.main.toolLevel = toolLevel;

        long savedTime = DateTime.Now.Ticks - long.Parse(PlayerPrefs.GetString("time"));
        int secondsPassed = (int)Mathf.Abs(savedTime/TimeSpan.TicksPerSecond > int.MaxValue ? int.MaxValue : savedTime/TimeSpan.TicksPerSecond);
        
        Player.main.UpdateStat();
        
        //Plant
        for (int i = 0; i < Player.main.landManager.availableLands.Count; i++)
        {
            int flag = PlayerPrefs.GetInt($"land_{i}_flag");
            if (flag <= 0) continue;
            int seedID = PlayerPrefs.GetInt($"land_{i}_seed"); 
            float liveTime = PlayerPrefs.GetFloat($"land_{i}_liveTime") + secondsPassed;
            float harvestTime = PlayerPrefs.GetFloat($"land_{i}_harvestTime");

            Land targetLand = Player.main.landManager.availableLands[i];
            SeedItem seedItem = ItemManager.GetItem(seedID) as SeedItem;
            if (seedItem == null) continue;

            targetLand.GrowPlant(seedItem.seedData);
            targetLand.liveTime = liveTime;
            targetLand.harvestTime = harvestTime;
        }
        //Item
        int itemCount = PlayerPrefs.GetInt("item_count");
        for(int i = 0; i < itemCount; i++)
        {
            int id = PlayerPrefs.GetInt($"item_{i}_id");
            int amount = PlayerPrefs.GetInt($"item_{i}_amount");
            Item item = ItemManager.GetItem(id);
            if (item == null) continue;
            Player.main.inventory.AddItem(id, amount);
        }
    }

    private void SetState(GameState state)
    {
        currentState = state;
        OnGameStateChange stateEvent = new OnGameStateChange(this, state);
        EventManager.TriggerEvent(stateEvent);
    }
#if UNITY_EDITOR
    [ContextMenu("Clear Data")]
    public void ClearData()
    {
        PlayerPrefs.SetInt("Init", 0);
    }
#endif
}
