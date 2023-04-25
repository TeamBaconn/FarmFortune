using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/Create Item", order = 1)]
public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public Sprite itemIcon;
    public int sellPrice = -1;

    public virtual string GetDescription()
    {
        if (sellPrice > 0)
            return $"Sell: {sellPrice} coins / each\n\nDrag this item to any stores to sell";
        return "";
    }
}

[Serializable]
public class ItemInfo
{
    public Item item;
    public int amount;
}