using UnityEngine;

[CreateAssetMenu(fileName = "Shop Stock", menuName = "Shop/Create Shop Item", order = 1)]
public class ItemStock : ShopStock
{
    public Item item;
    public int buyAmount = 10;

    public override string GetStockName()
    {
        return item.itemName;
    }

    public override string GetDescription()
    {
        string res = $"Price: {buyPrice} coins / {buyAmount} units";
        if(item is SeedItem)
        {
            res += "\n\nDrag to soil to plant these seeds";
        }
        return res;
    }

    public override Sprite GetStockIcon()
    {
        return item.itemIcon;
    }

    public override void AddStock(Player player)
    {
        player.inventory.AddItem(item, buyAmount);
    }
}