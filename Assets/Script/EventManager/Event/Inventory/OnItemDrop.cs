using UnityEngine;

public class OnItemDrop : EventParam
{
    public Item item;
    public Player player;
    public Vector2 dropPosition;

    public OnItemDrop(Item item, Player player, Vector2 dropPosition)
    {
        this.item = item;
        this.player = player;
        this.dropPosition = dropPosition;
    }
}