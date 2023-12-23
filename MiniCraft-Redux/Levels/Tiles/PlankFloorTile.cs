using MiniCraftRedux.Items.Tools;

namespace MiniCraftRedux.Levels.Tiles;

public record class PlankFloorTile : Tile
{
    public PlankFloorTile(byte id)
        : base(id)
    {
    }

    protected virtual int MainColor => 350;
    protected virtual int DarkColor => 550;

    public override void Render(Screen screen, Level level, int x, int y)
    {
        var Color1 = Color.Rgb(83, 59, 37);
        var Color2 = Color.Rgb(202, 156, 110);
        var Color3 = Color.Rgb(158, 112, 89);

        int color = Color.Get(-1, Color1, Color2, Color3);
        int sprite = 6;
        screen.Render(x * 16 + 0, y * 16 + 0, sprite * 2 + 8 * 32, color, 0);
        screen.Render(x * 16 + 8, y * 16 + 0, sprite * 2 + 8 * 32 + 1, color, 0);
        screen.Render(x * 16 + 0, y * 16 + 8, sprite * 2 + 8 * 32 + 32, color, 0);
        screen.Render(x * 16 + 8, y * 16 + 8, sprite * 2 + 8 * 32 + 33, color, 0);
    }

    public override void Hurt(Level level, int x, int y, Mob source, int dmg, Direction attackDir)
    {
       // Hurt(level, x, y, dmg);
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
            level.Add(new ItemEntity(new ResourceItem(Resource.Planks), x * 16 + random.NextInt(10) + 3, y * 16 + random.NextInt(10) + 3));
            level.SetTile(x, y, Hole, 0);
        }
        else
        {
            level.SetData(x, y, damage);
        }
    }


    public override bool MayPass(Level level, int x, int y, Entity e)
    {
        return true;
    }

    public override void Update(Level level, int xt, int yt)
    {
        byte damage = level.GetData(xt, yt);

        if (damage > 0)
        {
            level.SetData(xt, yt, (byte)(damage - 1));
        }
    }
}
