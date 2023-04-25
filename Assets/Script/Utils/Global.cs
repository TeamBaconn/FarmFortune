
public static class Global
{
    //STATIC
    public static int MAX_SLOT = 12;

    //CONST
    public const int DEFAULT_SAVE_TIME = 300;

    public const float TOOL_BUFF = 0.1f;

    public const int MONEY_GOAL = 1000000;

    public const string ITEM_CONFIG_PATH = "Assets/Config/Item";
    public const string SHOP_CONFIG_PATH = "Assets/Config/Item";

    public const float LAND_SIZE = 6;
    public const float LAND_DETECTION_SIZE = LAND_SIZE / 2 * 0.6f;

    public const int MAX_LAND_PER_ROW = 3;

    public const int MAX_LAND_PER_SQUARE = MAX_LAND_PER_ROW * MAX_LAND_PER_ROW;

    public const float SQUARE_OFFSET_Y = -16;

    public const float LAND_OFFSET = ((SQUARE_OFFSET_Y*-1) - MAX_LAND_PER_ROW*LAND_SIZE)/(MAX_LAND_PER_ROW+1);
}
