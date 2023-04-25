using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Shop : Interactable
{
    [Header("Shop config")]
    public string shopName = "Shop";
    public List<ShopStock> buyableItems;
    [Header("Render Setting")]
    public ShopRenderer shopRenderer;
    [Header("Outline Setting")]
    public Color sellColor;
    public Color buyColor;
    public Color cannotBuyColor;

    //For main player
    private CursorMessage itemMessage;
    private Item itemPicked;
    private int pickedAmount;

    private void Start()
    {
        if (shopRenderer == null) Debug.LogError("Shop renderer cannot be null");
    }

#if UNITY_EDITOR
    [ContextMenu("Load Data")]
    public void LoadData()
    {
        buyableItems = new List<ShopStock>();
        string[] guids = AssetDatabase.FindAssets("t:ShopStock", new[] { Global.SHOP_CONFIG_PATH });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            ShopStock item = AssetDatabase.LoadAssetAtPath<ShopStock>(assetPath);

            if (item != null)
            {
                buyableItems.Add(item);
            }
        }
        Debug.Log($"Loaded {buyableItems.Count} buyable item");
    }
#endif


    private void OnEnable()
    { 
        EventManager.StartListening<OnItemDrop>(OnItemDrop);
        EventManager.StartListening<OnItemPickUp>(OnItemPickUp);
        EventManager.StartListening<OnInventoryUpdate>(OnInventoryUpdate);
        EventManager.StartListening<OnPlayerClick>(OnPlayerClick);
    }

    private void OnDisable()
    { 
        EventManager.StopListening<OnItemDrop>(OnItemDrop);
        EventManager.StopListening<OnItemPickUp>(OnItemPickUp);
        EventManager.StopListening<OnInventoryUpdate>(OnInventoryUpdate);
        EventManager.StartListening<OnPlayerClick>(OnPlayerClick);
    }

    protected override void OnMouseEnter()
    {
        string title, description;
        if (itemPicked == null)
        {
            SetColor(sellColor);
            title = "Shop";
            description = "Click to open shop or drag item in here to sell";
        }
        else
        {
            SetColor(itemPicked.sellPrice <= 0 ? cannotBuyColor : buyColor);
            if (itemPicked.sellPrice > 0)
            {
                title = "Sell";
                description = $"{itemPicked.name} for {itemPicked.sellPrice * pickedAmount} coins";
            }
            else
            {
                title = "Cannot sell";
                description = "He doesn't want to buy this";
            }
        }
        itemMessage = new CursorMessage(title, description);
        EventManager.TriggerEvent(new OnCursorMessageRequest(itemMessage, true));
        base.OnMouseEnter();
    }

    protected override void OnMouseExit()
    {
        if (itemMessage != null)
        {
            EventManager.TriggerEvent(new OnCursorMessageRequest(itemMessage, false));
            itemMessage = null;
        }
        base.OnMouseExit();
    }

    private void OnPlayerClick(EventParam param)
    {
        OnPlayerClick eventParam = param as OnPlayerClick;
        if (!eventParam.player.Equals(Player.main)) return;
        //This ensure the click only run once
        if (eventParam.IsEventCanceled()) return;
        if (!isInteractable) return;
        eventParam.CancelEvent();

        shopRenderer.RenderShop(this, eventParam.player);
    }

    private void OnItemDrop(EventParam param)
    {
        OnItemDrop eventParam = param as OnItemDrop;
        //Check render for main player
        if (eventParam.player.Equals(Player.main))
        {
            itemPicked = null;
            //Check for dropping inside the shop for main player if not return
            if (!isInteractable) return;
        }

        //Check the item is on sell
        if (eventParam.item == null || eventParam.item.sellPrice <= 0) return;
        TrySell(eventParam.item, eventParam.player);

        OnMouseEnter();
        if (itemMessage != null)
        {
            EventManager.TriggerEvent(new OnCursorMessageRequest(itemMessage, false));
            itemMessage = null;
        }
    }

    private void OnInventoryUpdate(EventParam param)
    {
        if (itemPicked == null) return;
        OnInventoryUpdate eventParam = param as OnInventoryUpdate;
        if (eventParam.player.Equals(Player.main))
        {
            //Check for main player
            if(eventParam.updatedInventory.TryGetValue(itemPicked, out int value) && value > 0)
            {
                pickedAmount = value;
            }
            else
            {
                itemPicked = null;
                pickedAmount = 0;
            }
            OnMouseEnter();
        }

    }

    private void TrySell(Item item, Player player)
    {
        int amount = player.inventory.GetAmount(item);
        OnPlayerSellItem sellEvent = new OnPlayerSellItem(player, item, amount);
        EventManager.TriggerEvent(sellEvent);
    }

    private void OnItemPickUp(EventParam param)
    {
        OnItemPickUp eventParam = param as OnItemPickUp;
        //Check render for main player
        if (eventParam.player.Equals(Player.main))
        {
            itemPicked = eventParam.item;
            pickedAmount = eventParam.amount;
        }
    }
}
