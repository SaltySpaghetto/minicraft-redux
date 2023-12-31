using Vildmark.Serialization;

namespace MiniCraftRedux.Items;

public class ResourceItem : Item
{
    public Resource Resource { get; set; }
    public int Count { get; set; } = 1;

    public ResourceItem(Resource resource)
    {
        Resource = resource;
    }

    public ResourceItem(Resource resource, int count)
    {
        Resource = resource;
        Count = count;
    }

    private ResourceItem()
    {
    }

    public override int GetColor()
    {
        return Resource.Color;
    }

    public override int GetSprite()
    {
        return Resource.Sprite;
    }

    public override void RenderIcon(Screen screen, int x, int y)
    {
        screen.Render(x, y, Resource.Sprite, Resource.Color, 0);
    }

    public override void RenderInventory(Screen screen, int x, int y)
    {
        screen.Render(x, y, Resource.Sprite, Resource.Color, 0);
        Font.Draw(Resource.Name, screen, x + 32, y, Color.Get(-1, 555, 555, 555));
        int cc = Count;
        if (cc > 999)
        {
            cc = 999;
        }

        Font.Draw("" + cc, screen, x + 8, y, Color.Get(-1, 444, 444, 444));
    }

    public override string GetName()
    {
        return Resource.Name;
    }

    public override void OnTake(ItemEntity itemEntity)
    {
    }

    public override bool InteractOn(Tile tile, Level level, int xt, int yt, GamePlayer player, Direction attackDir)
    {
        if (Resource.InteractOn(tile, level, xt, yt, player, attackDir))
        {
            Count--;
            return true;
        }

        return false;
    }

    public override bool IsDepleted()
    {
        return Count <= 0;
    }

    public override void Serialize(IWriter writer)
    {
        base.Serialize(writer);

        writer.WriteString(Resource.Name);
        writer.WriteValue(Count);
    }

    public override void Deserialize(IReader reader)
    {
        base.Deserialize(reader);

        Resource = Resource.All.GetValueOrDefault(reader.ReadString());
        Count = reader.ReadValue<int>();
    }
}