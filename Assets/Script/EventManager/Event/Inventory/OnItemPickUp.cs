public class OnItemPickUp : EventParam
{
    public Item item;
    public Player player;
    public int amount;

    public OnItemPickUp(Item item, Player player, int amount)
    {
        this.item = item;
        this.player = player;
        this.amount = amount;
    }
}