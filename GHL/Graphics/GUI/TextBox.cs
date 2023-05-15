using SFML.System;
using SFML.Graphics;
using SFML.Window;
using GHL.Helper;
using System;


namespace GHL.Graphics
{
    public class TextBox : Rectangle
    {
        public string DisplayedString
        {
            get { return Text.DisplayedString; }
            set { Text.DisplayedString = value; }
        }
        public Text Text;

        #region Getters and Setters
        /// <summary>
        /// Character size of the text
        /// </summary>
        public uint CharacterSize { get => Text.CharacterSize; set => Text.CharacterSize = value; }
        /// <summary>
        /// Font of the text
        /// </summary>
        public Font TextFont { get => Text.Font; set => Text.Font = value; }
        #endregion

        public int Index { get; set; }
        private float DeleteDelay { get; set; } = 1;

        private Cursor EditTextCursor;

        public delegate void TextBoxEventHandler(TextBox source, EventArgs args);
        public event TextBoxEventHandler TextBoxEnterPressed;

        private bool Clicked = false;

        public string AcceptableCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.BackSpaceReturn";

        public TextBox(Vector2f position, Vector2f size) : base(position, size)
        {
            OutlineThickness = 2f;
            OutlineColor = Color.Black;
            Text = new Text("", MessegeManager.Courier, 14);
            Text.Color = Color.Black;
            Text.Position = new Vector2f(PositionX, PositionY + (SizeY - MessegeManager.GetTextRect("A", MessegeManager.Courier, 14).Height) / 2);
            EditTextCursor = new Cursor(position, Color.Black, 0.5f, 14 + 2f, 12f);
        }
        public TextBox(Vector2f position, Vector2f size, Font font, uint fontSize) : base(position, size)
        {
            OutlineThickness = 2f;
            OutlineColor = Color.Black;
            Text = new Text("", font, fontSize);
            Text.Color = Color.Black;
            Text.Position = new Vector2f(PositionX, PositionY + (SizeY - MessegeManager.GetTextRect("A", font, fontSize).Height) / 2);
            EditTextCursor = new Cursor(position, Color.Black, 0.5f, fontSize + 2f, 12f);
        }

        public new void Draw(RenderWindow window)
        {
            base.Draw(window);
            window.Draw(Text);
            if (Clicked)
            {
                EditTextCursor.Draw(window);
            }
        }

        public void Update(Vector2i mousePos, float totalTimeElapsed)
        {
            Text.Position = new Vector2f(PositionX, PositionY + (SizeY - Text.GetLocalBounds().Height) / 2 - Text.GetLocalBounds().Top);
            EditTextCursor.SizeCenter = Text.CharacterSize + 2f;
            EditTextCursor.Update(new Vector2f(MessegeManager.GetTextRect("", Text.Font, Text.CharacterSize).Width + PositionX + 1, SizeY / 2 + PositionY));

            UpdateIndexBounds();

            if (Index == DisplayedString.Length && DisplayedString.Length != 0)
            {
                EditTextCursor.Update(new Vector2f(MessegeManager.GetTextRect(DisplayedString, Text.Font, Text.CharacterSize).Width + PositionX + 1, SizeY / 2 + PositionY));
            }
            else if (DisplayedString != "")
            {
                EditTextCursor.Update(new Vector2f(MessegeManager.GetTextRect(DisplayedString.Remove(Index), Text.Font, Text.CharacterSize).Width + PositionX + 1, SizeY / 2 + PositionY));
            }
            else if (Index == 0)
            {
                EditTextCursor.Update(new Vector2f(MessegeManager.GetTextRect("", Text.Font, Text.CharacterSize).Width + PositionX + 3, SizeY / 2 + PositionY));
            }

            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                if (MathHelper.IsMouseInRectangle(this, mousePos))
                {
                    Clicked = true;
                }
                else
                {
                    Clicked = false;
                }
            }

            if (Clicked)
            {
                UpdateIndexKeys(totalTimeElapsed);
                UpdateIndexBounds();

                string input = KeyboardManager.ReadInput("", AcceptableCharacters);

                if (input != "")
                {
                    if (input.Contains("BackSpace"))
                    {
                        input = input.Replace("BackSpace", "");
                        try
                        {
                            DisplayedString = DisplayedString.Remove(Index - 1, 1);
                            Index--;
                        }
                        catch { }
                    }
                    if (input.Contains("Space"))
                    {
                        input = input.Replace("Space", " ");
                    }
                    if (input.Contains("Return"))
                    {
                        input = input.Replace("Return", "");
                        OnTextBoxEnterPressed();
                    }

                    DisplayedString = DisplayedString.Insert(Index, input);
                    Index += input.Length;
                }
            }
        }

        private void UpdateIndexKeys(float totalTimeElapsed)
        {
            if (KeyboardManager.OnKeyPressed(Keyboard.Key.Left))
            {
                Index--;
            }
            if (KeyboardManager.OnKeyPressed(Keyboard.Key.Right))
            {
                Index++;
            }
            if (KeyboardManager.OnKeyDownForTime(Keyboard.Key.BackSpace, 0.7f, totalTimeElapsed))
            {
                DeleteDelay -= 0.02f * totalTimeElapsed;
                if (DeleteDelay < 0)
                {
                    try
                    {
                        DisplayedString = DisplayedString.Remove(Index - 1, 1);
                        Index--;
                    }
                    catch { }
                    DeleteDelay = 1;
                }
            }
        }
        private void UpdateIndexBounds()
        {
            if (Index < 0)
            {
                Index = 0;
            }
            if (Index > DisplayedString.Length && DisplayedString.Length != 0)
            {
                Index = DisplayedString.Length;
            }
        }

        protected virtual void OnTextBoxEnterPressed()
        {
            if (TextBoxEnterPressed != null)
                TextBoxEnterPressed(this, EventArgs.Empty);
        }

        private class Cursor
        {
            public Vector2f Position;

            private Line[] Lines = new Line[3];
            public float SizeCenter;
            public float SizeSides;

            public Cursor(Vector2f position, Color color, float outlineThickness, float sizeCenter, float sizeSides)
            {
                Position = position;
                SizeCenter = sizeCenter;
                SizeSides = sizeSides;

                for (int i = 0; i < 3; i++)
                {
                    Lines[i] = new Line();
                    Lines[i].OutlineColor = color;
                    Lines[i].OutlineThickness = outlineThickness;
                }
                Lines[0].Position0 = new Vector2f(Position.X, Position.Y - SizeCenter / 2);
                Lines[0].Position1 = new Vector2f(Position.X, Position.Y + SizeCenter / 2);

                Lines[1].Position0 = new Vector2f(Position.X - SizeSides / 2, Position.Y + SizeCenter / 2);
                Lines[1].Position1 = new Vector2f(Position.X + SizeSides / 2, Position.Y + SizeCenter / 2);

                Lines[2].Position0 = new Vector2f(Position.X - SizeSides / 2, Position.Y - SizeCenter / 2);
                Lines[2].Position1 = new Vector2f(Position.X + SizeSides / 2, Position.Y - SizeCenter / 2);
            }

            public void Update(Vector2f positon)
            {
                Position = positon;
                Lines[0].Position0X = Position.X;
                Lines[0].Position0Y = Position.Y - SizeCenter / 2;
                Lines[0].Position1X = Position.X;
                Lines[0].Position1Y = Position.Y + SizeCenter / 2;

                Lines[1].Position0X = Position.X - SizeSides / 2;
                Lines[1].Position0Y = Position.Y + SizeCenter / 2;
                Lines[1].Position1X = Position.X + SizeSides / 2;
                Lines[1].Position1Y = Position.Y + SizeCenter / 2;

                Lines[2].Position0X = Position.X - SizeSides / 2;
                Lines[2].Position0Y = Position.Y - SizeCenter / 2;
                Lines[2].Position1X = Position.X + SizeSides / 2;
                Lines[2].Position1Y = Position.Y - SizeCenter / 2;
            }

            public void Draw(RenderWindow window)
            {
                for (int i = 0; i < 3; i++)
                {
                    Lines[i].Draw(window);
                }
            }
        }
    }
}
