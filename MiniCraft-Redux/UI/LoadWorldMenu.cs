namespace MiniCraftRedux.UI;

public class LoadWorldMenu : Menu
{
    private int selected = 0;

    private static string[] options = { "Exit" };
    private int debugColor = Color.Get(0, 0, 0, 0);
    private int debugColorInc = 0;
    private Menu par;


    public LoadWorldMenu(Menu parent)
    {
        par = parent;
        options = [ "Exit" ];
        worldFiles = new List<string>();
        worldFiles.Add("Exit");
        ListWorlds();
    }

    public static List<string> worldFiles = new List<string>();
    public static bool hasAWorld = false;

    public static void ListWorlds()
    {
        var worldDir = "worlds/";
        var worlds = Directory.GetFiles(worldDir);
        int worldNumber = 0;
        foreach (string world in worlds)
        {
            var worldFile = Path.GetFileNameWithoutExtension(world);
            worldFiles.Add(worldFile);
            worldNumber++;
        }
        if (worldNumber < 1)
            hasAWorld = false;
        options = worldFiles.ToArray();
    }

    public override void Update()
    {

        int len = options.Length;
        if (selected < 0)
        {
            selected += len;
        }

        if (selected >= len)
        {
            selected -= len;
        }

        if (input.Up.Clicked)
        {
            selected--;
        }

        if (input.Down.Clicked)
        {
            selected++;
        }


        if (input.Attack.Clicked || input.Menu.Clicked)
        {
            if (selected == 0)
            {
                game.Menu = par;
            }
            else
            {
                Game.worldName = options[selected];
                AudioTracks.Test.Play();
                game.NewGame();
                game.Menu = null;
            }
        }
    }

    public override void Render(Screen screen)
    {
        screen.Clear(0);

        int h = 2;
        int w = 13;
        int titleColor = Color.Get(0, 010, 131, 551);
        int xo = (screen.Width - w * 8) / 2;
        int yo = 24;

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                screen.Render(xo + x * 8, yo + y * 8, x + (y + 6) * 32, titleColor, 0);
            }
        }

        for (int i = 0; i < options.Length; i++)
        {
            string msg = options[i];
            int col = Color.Get(0, 222, 222, 222);
            if (i == selected)
            {
                msg = "> " + msg + " <";
                col = Color.Get(0, 555, 555, 555);
            }
            Font.Draw(msg, screen, (screen.Width - msg.Length * 8) / 2, (8 + i) * 8, col);
        }

        Font.Draw("(Arrow keys,X and C)", screen, 0, screen.Height - 8, Color.Get(0, 111, 111, 111));
    }
}