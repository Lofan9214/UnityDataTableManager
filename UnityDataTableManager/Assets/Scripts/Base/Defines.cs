public enum Languages
{
    Korean,
    English,
    Japanese,
}

public class DataTableIds
{
    public static readonly string[] String =
    {
        "StringTableKr",
        //"StringTableEn",
        //"StringTableJp",
    };

    public const string Resource = "ResourceTable";
    public const string Event = "EventTable";
    public const string EventType = "EventTypeTable";
    public const string Signal = "SignalTable";
}

public class Variables
{
    public static Languages currentLanguage = Languages.Korean;
}

public static class GameInfos
{
    public static readonly int initialTurn = 1;
    public static readonly int lastTurn = 22;


}
public enum ResourceType
{
    Food,
    Energy,
    Oxygen,
}
