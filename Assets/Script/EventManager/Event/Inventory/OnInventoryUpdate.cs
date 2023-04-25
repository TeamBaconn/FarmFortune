using System.Collections.Generic;

public class OnInventoryUpdate : EventParam
{
    public Dictionary<Item,int> updatedInventory;
    public Player player;

    public OnInventoryUpdate(Dictionary<Item, int> updatedInventory, Player player)
    {
        this.updatedInventory = updatedInventory;
        this.player = player;
    }
}