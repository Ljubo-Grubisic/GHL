using SFML.Graphics;
using SFML.System;

namespace GHL.Helper
{
    public static class MessegeManager
    {
        private const string Arial_FONT_PATH = "Resources/Fonts/arial.ttf";
        private const string Courier_FONT_PATH = "Resources/Fonts/courier.ttf";
        public static Font Arial;
        public static Font Courier;

        public static void LoadContent()
        {
            Arial = new Font(Arial_FONT_PATH);
            Courier = new Font(Courier_FONT_PATH);
        }

        public static void Message(RenderWindow window, string textStr, Font font, Vector2f position, Color fontColor, uint fontSize = 14)
        {
            if (Arial == null)
                return;

            Text text = new Text(textStr, font, fontSize);
            text.Position = new Vector2f(position.X, position.Y);
            text.Color = fontColor;

            window.Draw(text);
        }
        public static void Message(RenderWindow window, string textStr, Font font, Vector2f position, uint fontSize = 14)
        {
            if (Arial == null)
                return;

            Text text = new Text(textStr, font, fontSize);
            text.Position = new Vector2f(position.X, position.Y);
            text.Color = Color.Black;

            window.Draw(text);
        }
        public static void Message(RenderWindow window, string textStr, Vector2f position, uint fontSize = 14)
        {
            if (Arial == null)
                return;

            Text text = new Text(textStr, Arial, fontSize);
            text.Position = new Vector2f(position.X, position.Y);
            text.Color = Color.Black;

            window.Draw(text);
        }

        public static FloatRect GetTextRect(string textStr, Font font, uint fontSize)
        {
            return new Text(textStr, font, fontSize).GetLocalBounds();
        }
    }
}
