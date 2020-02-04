public enum HexTileDirection : int{
    North,
    NorthEast,
    SouthEast,
    South,
    SouthWest,
    NorthWest
}

static class HexTileDirectionMethods
{

    public static HexTileDirection GetInverse(this HexTileDirection p_hexTileDirection)
    {
        switch (p_hexTileDirection)
        {
            case HexTileDirection.North:
                return HexTileDirection.South;
            case HexTileDirection.NorthEast:
                return HexTileDirection.SouthWest;
            case HexTileDirection.SouthEast:
                return HexTileDirection.NorthWest;
            case HexTileDirection.South:
                return HexTileDirection.North;
            case HexTileDirection.SouthWest:
                return HexTileDirection.NorthEast;
            case HexTileDirection.NorthWest:
                return HexTileDirection.SouthEast;
            default:
                return HexTileDirection.North;
        }
    }
}