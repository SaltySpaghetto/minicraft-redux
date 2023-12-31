namespace MiniCraftRedux.Entities;

public class Oven : CraftingStation
{
    public Oven()
        : base("Oven", 2, Color.Get(-1, 000, 332, 442), horizontalRadius: 3, verticalRadius: 2)
    {
    }

    public override IEnumerable<Recipe> GetRecipes(GamePlayer player)
    {
        yield return new ResourceRecipe(Resource.Bread, new ResourceItem(Resource.Wheat, 4));
        yield return new ResourceRecipe(Resource.CookedBeef, new ResourceItem(Resource.RawBeef, 1));
    }
}