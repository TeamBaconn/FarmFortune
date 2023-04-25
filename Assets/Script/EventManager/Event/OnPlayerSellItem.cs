public class OnPlayerSellItem : EventParam
{
    public Player player;
    public Item item;
    public int amount;

    public OnPlayerSellItem(Player player, Item item, int amount)
    {
        this.player = player;
        this.item = item;
        this.amount = amount;
    }
}