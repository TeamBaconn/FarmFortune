public class OnInteractableHighlight : EventParam
{
    public Interactable interactable;
    public bool enable;

    public OnInteractableHighlight(Interactable interactable, bool enable)
    {
        this.interactable = interactable;
        this.enable = enable;
    }
}