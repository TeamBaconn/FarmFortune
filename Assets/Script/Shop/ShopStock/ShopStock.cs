using UnityEngine;

public class ShopStock : ScriptableObject
{
    public int buyPrice = 0;
    
    public virtual string GetStockName()
    {
        return "Stock";
    }

    public virtual string GetDescription()
    {
        return "";
    }

    public virtual Sprite GetStockIcon()
    {
        return null;
    }

    public virtual void AddStock(Player player)
    {

    }
}