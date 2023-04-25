using UnityEngine;

[CreateAssetMenu(fileName = "Land Stock", menuName = "Shop/Create Land Item", order = 1)]
public class LandStock : ShopStock
{
    public Sprite landSprite;
    public int buyAmount = 1;

    public override string GetStockName()
    {
        return "Extend land";
    }

    public override string GetDescription()
    {
        string res = $"Price: {buyPrice} coins / {buyAmount} land";

        res += "\n\nExtending your land to grow more crops";
        return res;
    }

    public override Sprite GetStockIcon()
    {
        return landSprite;
    }

    public override void AddStock(Player player)
    {
        player.AddLand(1);
    }
}