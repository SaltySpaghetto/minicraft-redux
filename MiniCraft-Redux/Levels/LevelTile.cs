namespace MiniCraftRedux.Levels;

public record struct LevelTile(byte ID, byte Data = 0, byte ExtraData = 0)
{
    public static implicit operator LevelTile(Tile tile) => new LevelTile(tile.ID);
}

