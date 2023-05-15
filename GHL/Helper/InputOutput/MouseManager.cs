using SFML.System;
using SFML.Window;

namespace GHL.Helper
{
    public static class MouseManager
    {
        public static Vector2i MouseOffSet = new Vector2i(8, 30);
        private static bool[] KeyHandlers = new bool[(int)Mouse.Button.ButtonCount];

        public static bool OnButtonPressed(Mouse.Button button)
        {
            if (!IsButtonDown(button))
            {
                KeyHandlers[(int)button] = true;
                return false;
            }
            else if (IsButtonDown(button))
            {
                if (KeyHandlers[(int)button])
                {
                    KeyHandlers[(int)button] = false;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        public static bool IsButtonDown(Mouse.Button button)
        {
            return Mouse.IsButtonPressed(button);
        }
    }
}
