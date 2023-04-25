public class OnPlayerStatUpdate : EventParam
{
    public Player player;

    public OnPlayerStatUpdate(Player player)
    {
        this.player = player;
    }
}