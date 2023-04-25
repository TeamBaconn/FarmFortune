public class OnLandNumberChange : EventParam
{
    public int newLandNumber;
    public int prevLandNumber;

    public OnLandNumberChange(int newLandNumber, int prevLandNumber)
    {
        this.newLandNumber = newLandNumber;
        this.prevLandNumber = prevLandNumber;
    }
}
