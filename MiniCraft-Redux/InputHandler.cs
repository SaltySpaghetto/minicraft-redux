using OpenTK.Graphics.ES11;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Vildmark.Input;

namespace MiniCraftRedux;

public class InputHandler
{
    private readonly InputKey[] keys;

    public InputKey Up { get; } = new(Keys.Up);
    public InputKey Down { get; } = new(Keys.Down);
    public InputKey Left { get; } = new(Keys.Left);
    public InputKey Right { get; } = new(Keys.Right);
    public InputKey Attack { get; } = new(Keys.C);
    public InputKey Pause { get; } = new(Keys.Escape);
    public InputKey Menu { get; } = new(Keys.X);
    public InputKey Debug { get; } = new(Keys.F3);
    public InputKey Throw { get; } = new(Keys.Z);
    public InputKey DebugAdd { get; } = new(Keys.F1);
    public InputKey Craft { get; } = new(Keys.Z);
    public InputKey Backspace { get; } = new(Keys.Backspace);

    private bool hasWritingInput = false;
    public string writingInput = "";
    public string enabledWritingKeys = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.,!?'\"-+=/\\%()<>:;";
    public static int writingLimit = 10;

    public InputHandler()
    {
        keys = new[] { Up, Down, Left, Right, Attack, Menu, Debug, Pause, Throw, DebugAdd, Craft, Backspace };

        Keyboard.OnTextInput += GetWritingInput;
        Keyboard.OnKeyPressed += Keyboard_OnKeyPressed;
        Keyboard.OnKeyReleased += Keyboard_OnKeyReleased;
    }

    public void DoWritingInput(bool getInput = true)
    {
        hasWritingInput = getInput;
    }

    public void GetWritingInput(char key)
    {
        if (!hasWritingInput)
            return;
        if (enabledWritingKeys.Contains(char.ToUpper(key)) && writingInput.Length < writingLimit)
            writingInput += key;
        if (Keyboard.IsKeyPressed(Keys.Backspace) && writingInput.Length > 0)
        {
            writingInput = writingInput.Remove(writingInput.Length - 1, 1);
        }

    }

    private void Keyboard_OnKeyReleased(Keys key)
    {
        foreach (var inputKey in keys)
        {
            if (inputKey.Key == key)
            {
                inputKey.Toggle(false);
            }
        }
    }

    private void Keyboard_OnKeyPressed(Keys key)
    {
        foreach (var inputKey in keys)
        {
            if (inputKey.Key == key)
            {
                inputKey.Toggle(true);
            }
        }
    }

    public void ReleaseAll()
    {
        foreach (var key in keys)
        {
            key.Down = false;
        }
    }

    public void Update()
    {
        foreach (var key in keys)
        {
            key.Update();
        }
    }

    public class InputKey
    {
        private int presses, absorbs;

        public bool Down { get; set; }
        public bool Clicked { get; set; }
        public Keys Key { get; }

        public InputKey(Keys key)
        {
            Key = key;
        }

        public virtual void Update()
        {
            if (absorbs < presses)
            {
                absorbs++;
                Clicked = true;
            }
            else
            {
                Clicked = false;
            }
        }

        public void Toggle(bool pressed)
        {
            if (pressed != Down)
            {
                Down = pressed;
            }

            if (pressed)
            {
                presses++;
            }
        }
    }
}
