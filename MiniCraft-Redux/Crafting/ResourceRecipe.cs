namespace MiniCraftRedux.Crafting;

public class ResourceRecipe : Recipe
{
    public Resource Resource { get; }

    public ResourceRecipe(Resource resource, params ResourceItem[] costs)
        : base(new ResourceItem(resource, 1), costs)
    {
        Resource = resource;
    }

    public override Item CreateItem()
    {
        return new ResourceItem(Resource, 1);
    }
}

public class ResourceRecipeMultiple : Recipe
{
    public Resource Resource { get; }
    public int count = 0;

    public ResourceRecipeMultiple(Resource resource, int count, params ResourceItem[] costs)
        : base(new ResourceItem(resource, 1), costs)
    {
        this.count = count;
        Resource = resource;
    }

    public override Item CreateItem()
    {
        return new ResourceItem(Resource, count);
    }
}
