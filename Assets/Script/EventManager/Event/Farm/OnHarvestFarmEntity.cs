public class OnHarvestFarmEntity : EventParam
{
    public FarmEntityData data;
    public IFarmEntity farmEntity;
    public float liveTime;
    public float harvestTime;

    public OnHarvestFarmEntity(FarmEntityData data, IFarmEntity farmEntity, float liveTime, float harvestTime)
    {
        this.data = data;
        this.farmEntity = farmEntity;
        this.liveTime = liveTime;
        this.harvestTime = harvestTime;
    }
}