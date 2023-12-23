using MiniCraftRedux.Items.Tools;
using System.Diagnostics.SymbolStore;

namespace MiniCraftRedux.Levels.Tiles;

public record class DoorTile : Tile
{

    public bool open;

    public DoorTile(byte id)
        : base(id)
    {
        this.open = false;
    }

    public override void Render(Screen screen, Level level, int x, int y)
    {
        var Color1 = Color.Rgb(83, 59, 37);
        var Color2 = Color.Rgb(202, 156, 110);
        var Color3 = Color.Rgb(158, 112, 89);

        int color = Color.Get(-1, Color1, Color2, Color3);

        int col = Color.Get(level.DirtColor, level.DirtColor, level.DirtColor - 111, level.DirtColor - 111);
        screen.Render(x * 16 + 0, y * 16 + 0, 0, col, 0);
        screen.Render(x * 16 + 8, y * 16 + 0, 1, col, 0);
        screen.Render(x * 16 + 0, y * 16 + 8, 2, col, 0);
        screen.Render(x * 16 + 8, y * 16 + 8, 3, col, 0);

        int openTile = 28;
        int closedTile = 26;

        byte openByte = level.GetData(x, y, true);
        open = openByte == 1;

        if (open)
        {
            screen.Render(x * 16 + 0, y * 16 + 0, openTile + 1 * 32, color, 0);
            screen.Render(x * 16 + 8, y * 16 + 0, openTile + 1 + 1 * 32, color, 0);
            screen.Render(x * 16 + 0, y * 16 + 8, openTile + 2 * 32, color, 0);
            screen.Render(x * 16 + 8, y * 16 + 8, openTile + 1 + 2 * 32, color, 0);
        }
        else
        {
            screen.Render(x * 16 + 0, y * 16 + 0, closedTile + 1 * 32, color, 0);
            screen.Render(x * 16 + 8, y * 16 + 0, closedTile + 1 + 1 * 32, color, 0);
            screen.Render(x * 16 + 0, y * 16 + 8, closedTile + 2 * 32, color, 0);
            screen.Render(x * 16 + 8, y * 16 + 8, closedTile + 1 + 2 * 32, color, 0);
        }

    }
    public override bool MayPass(Level level, int x, int y, Entity e)
    {
        byte openByte = level.GetData(x, y, true);
        return openByte == 1;
    }

    public override void Hurt(Level level, int x, int y, Mob source, int dmg, Direction attackDir)
    {
        byte damage = (byte)(level.GetData(x, y));
        level.SetData(x, y, damage, 1);
        if (open)
            level.SetData(x, y, damage, 0);
    }

    public override bool Interact(Level level, int xt, int yt, GamePlayer player, Item item, Direction attackDir)
    {
        if (item is ToolItem tool)
        {
            if (tool.Type == ToolType.Axe)
            {
                if (player.PayStamina(4 - (int)tool.Level))
                {
                    Hurt(level, xt, yt, random.NextInt(10) + (int)tool.Level * 5 + 10);
                    return true;
                }
            }
        }
        return false;
    }

    public void Hurt(Level level, int x, int y, int dmg)
    {
        byte damage = (byte)(level.GetData(x, y) + dmg);

        level.Add(new SmashParticle(x * 16 + 8, y * 16 + 8));
        level.Add(new TextParticle("" + dmg, x * 16 + 8, y * 16 + 8, Color.Get(-1, 500, 500, 500)));

        if (damage >= 30)
        {
            level.Add(new ItemEntity(new ResourceItem(Resource.Doors), x * 16 + random.NextInt(10) + 3, y * 16 + random.NextInt(10) + 3));
            level.SetTile(x, y, Dirt, 0);
        }
        else
        {
            level.SetData(x, y, damage, 0);
        }
    }
}
