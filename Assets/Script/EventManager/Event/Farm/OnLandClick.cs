public enum LandAction
{
    PLANT, HARVEST, REMOVE
}
public class OnLandClick : EventParam
{
    public Land land;
    public LandAction action;
    public object argument;
    public Player player;

    public OnLandClick(Land land, LandAction action, object argument, Player player)
    {
        this.land = land;
        this.action = action;
        this.argument = argument;
        this.player = player;
    }
}
