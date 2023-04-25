public class OnPlayerClick : EventParam
{
    public Player player;

    public OnPlayerClick(Player player)
    {
        this.player = player;
    }
}