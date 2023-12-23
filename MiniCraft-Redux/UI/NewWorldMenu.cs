using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vildmark.Input;

namespace MiniCraftRedux.UI
{
    public class NewWorldMenu : Menu
    {
        private int selected = 0;

        private readonly Menu parent;
        public string worldName = "";
        private static readonly string[] options = { "PLAY", "EXIT" };


        public NewWorldMenu(Menu parent)
        {
            this.parent = parent;
        }

        public override void Update()
        {
            input.DoWritingInput();

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

            if (Keyboard.IsKeyPressed(Keys.Enter))
            {
                if (selected == 1)
                {
                    game.Menu = parent;
                    input.DoWritingInput(false);
                    input.writingInput = "";
                }
                else if (selected == 0) 
                {
                    Game.worldName = worldName;
                    AudioTracks.Test.Play();
                    game.NewGame();
                    game.Menu = null;

                }
            }

            worldName = input.writingInput;
        }

        public override void Render(Screen screen)
        {
            screen.Clear(0);

            for (int i = 0; i < options.Length; i++)
            {
                string msg = options[i];
                int col = Color.Get(0, 222, 222, 222);
                if (i == selected)
                {
                    msg = "> " + msg + " <";
                    col = Color.Get(0, 555, 555, 555);
                }
                Font.Draw(msg, screen, (screen.Width - msg.Length * 8) / 2, (10 + i) * 8, col);
            }


            var msg2 = "NAME YOUR WORLD";
            Font.Draw(msg2, screen, (screen.Width - msg2.Length * 8) / 2, 1 * 8, Color.Get(0, 555, 555, 555));
            Font.Draw(worldName, screen, (screen.Width - worldName.Length * 8) / 2, 5 * 8, Color.Get(0, 555, 555, 555));
            Font.Draw("(ENTER TO SELECT)", screen, 0, screen.Height - 8, Color.Get(0, 111, 111, 111));

        }

    }
}
