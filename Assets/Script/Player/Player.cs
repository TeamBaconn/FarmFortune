using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(LandManager))]
[RequireComponent(typeof(WorkerManager))]
public class Player : MonoBehaviour
{
    [HideInInspector]
    public Inventory inventory;
    [HideInInspector]
    public LandManager landManager;

    public int totalMoney = 0;
    public int totalWorker = 1;
    public int totalLand = 3;
    public int toolLevel = 1;

    public int totalWorkingWorker = 0;

    public bool isMainPlayer;
    public static Player main;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        landManager = GetComponent<LandManager>();
        if (isMainPlayer)
        {
            if(main != null)
            {
                Debug.LogError("There are 2 main players");
                Destroy(gameObject);
                return;
            }
            main = this;
        }
    }

    private void Start()
    {
        UpdateStat();
    }

    public void UpdateStat()
    {
        OnPlayerStatUpdate updateEvent = new OnPlayerStatUpdate(this);
        EventManager.TriggerEvent(updateEvent);
    }

    public bool HasMoney(int amount)
    {
        return totalMoney >= amount;
    }

    public int AddMoney(int amount)
    {
        totalMoney += amount;
        totalMoney = Mathf.Max(0, totalMoney);
        UpdateStat();
        return totalMoney;
    }

    public int AddToolLevel(int amount)
    {
        toolLevel += amount;
        toolLevel = Mathf.Max(0, toolLevel);
        UpdateStat();
        return toolLevel;
    }

    public int AddWorker(int amount)
    {
        totalWorker += amount;
        totalWorker = Mathf.Max(0, totalWorker);
        UpdateStat();
        return totalWorker;
    }
    public int AddWorkingWorker(int amount)
    {
        totalWorkingWorker += amount;
        totalWorkingWorker = Mathf.Max(0, totalWorkingWorker);
        UpdateStat();
        return totalWorkingWorker;
    }

    public int AddLand(int amount)
    {
        totalLand += amount;
        totalLand = Mathf.Max(0, totalLand);
        UpdateStat();
        return totalLand;
    }

    private void OnEnable()
    {
        EventManager.StartListening<OnPlayerSellItem>(OnPlayerSellItem);
        EventManager.StartListening<OnHarvestFarmEntity>(OnHarvestFarmEntity);
        EventManager.StartListening<OnLandClick>(OnLandClick);
        EventManager.StartListening<OnGameStateChange>(OnGameStateChange);
    }

    private void OnDisable()
    {
        EventManager.StopListening<OnPlayerSellItem>(OnPlayerSellItem);
        EventManager.StopListening<OnHarvestFarmEntity>(OnHarvestFarmEntity);
        EventManager.StopListening<OnLandClick>(OnLandClick);
        EventManager.StopListening<OnGameStateChange>(OnGameStateChange);
    }

    private void OnGameStateChange(EventParam param)
    {
        OnGameStateChange eventParam = param as OnGameStateChange;
        if (eventParam.state != GameState.NEW_GAME) return;
        inventory.ClearItems();
        foreach(ItemInfo item in eventParam.manager.starterItems)
        {
            inventory.AddItem(item.item, item.amount);
        }
        totalMoney = eventParam.manager.startMoney;
        toolLevel = eventParam.manager.startToolLevel;
        totalLand = eventParam.manager.startLand;
        totalWorker = eventParam.manager.startWorker;
        UpdateStat();
    }

    private void OnPlayerSellItem(EventParam param)
    {
        OnPlayerSellItem eventParam = param as OnPlayerSellItem;
        if (!eventParam.player.Equals(this)) return;
        AddMoney(eventParam.item.sellPrice * eventParam.amount);
    }

    private void OnHarvestFarmEntity(EventParam param)
    {
        OnHarvestFarmEntity eventParam = param as OnHarvestFarmEntity;
        int amount = eventParam.data.harvestItemAmount;
        amount += (int)(amount * toolLevel * Global.TOOL_BUFF);
        inventory.AddItem(eventParam.data.harvestItem, amount);
    }

    private void OnLandClick(EventParam param)
    {
        OnLandClick eventParam = param as OnLandClick;
        if (!eventParam.player.Equals(this)) return;
        SeedItem data = eventParam.argument as SeedItem;
        if (data == null || eventParam.action != LandAction.PLANT) return;
        //Plant
        inventory.AddItem(data, -1);
    }
}
