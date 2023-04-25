public class CursorMessage
{
    public string title;
    public string description;
    public int priority;

    public CursorMessage(string title, string description, int priority = 0)
    {
        this.title = title;
        this.description = description;
        this.priority = priority;
    }
}
public class OnCursorMessageRequest : EventParam
{
    public CursorMessage message;
    public bool show;

    public OnCursorMessageRequest(CursorMessage message, bool show)
    {
        this.message = message;
        this.show = show;
    }
}