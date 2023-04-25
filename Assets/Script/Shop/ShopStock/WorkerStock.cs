using UnityEngine;

[CreateAssetMenu(fileName = "Worker Stock", menuName = "Shop/Create Worker Item", order = 1)]
public class WorkerStock : ShopStock
{
    public Sprite workerSprite;
    public int buyAmount = 10;

    public override string GetStockName()
    {
        return "Worker";
    }

    public override string GetDescription()
    {
        string res = $"Price: {buyPrice} coins / {buyAmount} worker";

        res += "\n\nThese workers will plant & harvest for you\n\n2 min per action";
        return res;
    }

    public override Sprite GetStockIcon()
    {
        return workerSprite;
    }

    public override void AddStock(Player player)
    {
        player.AddWorker(1);
    }
}