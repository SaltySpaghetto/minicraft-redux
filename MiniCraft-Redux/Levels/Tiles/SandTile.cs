using MiniCraftRedux.Items.Tools;

namespace MiniCraftRedux.Levels.Tiles;

public record class SandTile : Tile
{
    public SandTile(byte id)
        : base(id, connectsToSand: true)
    {
    }

    public override void Render(Screen screen, Level level, int x, int y)
    {
        int col = Color.Get(level.SandColor + 2, level.SandColor, level.SandColor - 110, level.SandColor - 110);
        int transitionColor = Color.Get(level.SandColor - 110, level.SandColor, level.SandColor - 110, level.DirtColor);

        bool u = !level.GetTile(x, y - 1).ConnectsToSand;
        bool d = !level.GetTile(x, y + 1).ConnectsToSand;
        bool l = !level.GetTile(x - 1, y).ConnectsToSand;
        bool r = !level.GetTile(x + 1, y).ConnectsToSand;

        bool steppedOn = level.GetData(x, y) > 0;

        if (!u && !l)
        {
            if (!steppedOn)
            {
                screen.Render(x * 16 + 0, y * 16 + 0, 0, col, 0);
            }
            else
            {
                screen.Render(x * 16 + 0, y * 16 + 0, 3 + 1 * 32, col, 0);
            }
        }
        else
        {
            screen.Render(x * 16 + 0, y * 16 + 0, (l ? 11 : 12) + (u ? 0 : 1) * 32, transitionColor, 0);
        }

        if (!u && !r)
        {
            screen.Render(x * 16 + 8, y * 16 + 0, 1, col, 0);
        }
        else
        {
            screen.Render(x * 16 + 8, y * 16 + 0, (r ? 13 : 12) + (u ? 0 : 1) * 32, transitionColor, 0);
        }

        if (!d && !l)
        {
            screen.Render(x * 16 + 0, y * 16 + 8, 2, col, 0);
        }
        else
        {
            screen.Render(x * 16 + 0, y * 16 + 8, (l ? 11 : 12) + (d ? 2 : 1) * 32, transitionColor, 0);
        }

        if (!d && !r)
        {
            if (!steppedOn)
            {
                screen.Render(x * 16 + 8, y * 16 + 8, 3, col, 0);
            }
            else
            {
                screen.Render(x * 16 + 8, y * 16 + 8, 3 + 1 * 32, col, 0);
            }
        }
        else
        {
            screen.Render(x * 16 + 8, y * 16 + 8, (r ? 13 : 12) + (d ? 2 : 1) * 32, transitionColor, 0);
        }
    }

    public override void Update(Level level, int x, int y)
    {
        int d = level.GetData(x, y);

        if (d > 0)
        {
            level.SetData(x, y, (byte)(d - 1));
        }
    }

    public override void SteppedOn(Level level, int x, int y, Entity entity)
    {
        if (entity is Mob)
        {
            level.SetData(x, y, 10);
        }
    }

    public override bool Interact(Level level, int xt, int yt, GamePlayer player, Item item, Direction attackDir)
    {
        if (item is ToolItem tool)
        {
            if (tool.Type == ToolType.Shovel)
            {
                if (player.PayStamina(4 - (int)tool.Level))
                {
                    level.SetTile(xt, yt, Dirt, 0);
                    level.Add(new ItemEntity(new ResourceItem(Resource.Sand), xt * 16 + random.NextInt(10) + 3, yt * 16 + random.NextInt(10) + 3));

                    return true;
                }
            }
        }

        return false;
    }
}
