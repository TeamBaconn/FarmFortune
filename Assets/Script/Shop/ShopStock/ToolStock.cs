using UnityEngine;

[CreateAssetMenu(fileName = "Land Stock", menuName = "Shop/Create Tool Item", order = 1)]
public class ToolStock : ShopStock
{
    public Sprite toolSprite;
    public int buyAmount = 1;

    public override string GetStockName()
    {
        return "Upgrade tool";
    }

    public override string GetDescription()
    {
        string res = $"Price: {buyPrice} coins / {buyAmount} upgrade";

        res += $"\n\nIncrease {Global.TOOL_BUFF*100}% harvest amount for each tool level";
        return res;
    }

    public override Sprite GetStockIcon()
    {
        return toolSprite;
    }

    public override void AddStock(Player player)
    {
        player.AddToolLevel(1);
    }
}