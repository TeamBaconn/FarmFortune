public class EventParam
{
    private int cancel = 1;

    public bool IsEventCanceled()
    {
        return cancel < 0;
    }
    public void CancelEvent()
    {
        cancel *= -1;
    }
    public void ProtectFromCancel()
    {
        cancel = 0;
    }
}
