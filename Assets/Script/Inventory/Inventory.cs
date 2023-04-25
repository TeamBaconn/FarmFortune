using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<Item, int> items { private set; get; }
    private Player targetPlayer;

    private void Awake()
    {
        items = new Dictionary<Item, int>();
        targetPlayer = GetComponent<Player>();
    }

    private void OnEnable()
    {
        EventManager.StartListening<OnPlayerSellItem>(OnPlayerSellItem);
    }

    private void OnDisable()
    {
        EventManager.StopListening<OnPlayerSellItem>(OnPlayerSellItem);
    }

    public Type GetItemByType<Type>()
    {
        foreach (Item item in items.Keys) if (item is Type itemType) return itemType;
        return default;
    }

    private void OnPlayerSellItem(EventParam param)
    {
        OnPlayerSellItem eventParam = param as OnPlayerSellItem;
        if (!eventParam.player.Equals(targetPlayer)) return;
        AddItem(eventParam.item, -eventParam.amount);
    }

    public bool AddItem(int id, int amount)
    {
        Item item = ItemManager.GetItem(id);
        return AddItem(item, amount);
    }

    public bool AddItem(Item item, int amount)
    {
        if (item == null || items == null) return false;
        if (!ItemManager.ContainsItem(item))
        {
            Debug.LogWarning("Item did not register");
            return false;
        }

        if (items.TryGetValue(item, out int value))
        {
            value += amount;
            if (value <= 0)
            {
                items.Remove(item);
            }
            else
            {
                items[item] = value;
            }
            UpdateInventory();
            return true;
        }
        else if (amount > 0)
        {
            items[item] = amount;
            UpdateInventory();
            return true;
        }
        return false;
    }

    public int ClearItems()
    {
        items.Clear();
        UpdateInventory();
        return 0;
    }

    public int GetAmount(Item item)
    {
        if (item == null || items == null) return 0;
        if (!ItemManager.ContainsItem(item))
        {
            Debug.LogWarning("Item did not register");
            return 0;
        }
        if (items.TryGetValue(item, out int value))
        {
            return value;
        }
        return 0;
    }

    private void UpdateInventory()
    {
        OnInventoryUpdate updateEvent = new OnInventoryUpdate(items, targetPlayer);
        EventManager.TriggerEvent(updateEvent);
    }
}
