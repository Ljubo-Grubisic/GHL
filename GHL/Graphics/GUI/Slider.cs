using SFML.Graphics;
using SFML.System;
using SFML.Window;
using GHL.Helper;
using System;


namespace GHL.Graphics
{
    public class Slider
    {
        private Vector2f position;

        public float Value;
        /// <summary>
        /// From X to Y
        /// </summary>
        public Vector2f Scale;
        public Vector2f Size;

        public Rectangle Rectangle;
        public Circle Circle;

        public delegate void SliderEventHandler(Slider sender, EventArgs args);

        // Animation events
        public event SliderEventHandler SliderDefaultAnimation;
        public event SliderEventHandler SliderPressedAnimation;
        public event SliderEventHandler SliderHoveringAnimation;

        public Vector2f Position
        {
            get { return position; }
            set
            {
                position = value;
            }
        }
        public float PositionX
        {
            get { return position.X; }
            set
            {
                position.X = value;
            }
        }
        public float PositionY
        {
            get { return position.Y; }
            set
            {
                position.Y = value;
            }
        }

        private bool Clicked;

        public Slider(Vector2f position, Vector2f size, Vector2f scale, float defaultValue, float radius)
        {
            this.position = position;
            Scale = scale;
            Size = size;
            Value = defaultValue;

            Rectangle = new Rectangle(Position, Size) { OutlineThickness = 2f, OutlineColor = Color.Black };

            float x = MathHelper.Map(Scale, new Vector2f(PositionX, PositionX + Size.X), Value);
            float y = Size.Y / 2 + Position.Y;

            Circle = new Circle(MathHelper.CenterRectangle(x, y, radius * 2, radius * 2), radius)
            { OutlineThickness = 2f, OutlineColor = Color.Black };
            SetCircleToValue(Value);
        }

        private bool StartUp = true;
        private Vector2f Buffer = new Vector2f();
        public void Update(Vector2i mousePos)
        {
            if (StartUp)
            {
                SetCircleToValue(Value);
                StartUp = false;
            }
            bool mouseState = MouseManager.IsButtonDown(Mouse.Button.Left);
            Rectangle.Position = Position;
            Rectangle.Size = Size;
            Circle.PositionY = PositionY + Size.Y / 2 - Circle.Radius;
            Value = MathHelper.Map(new Vector2f(0, Size.X), Scale, Circle.Position.X + Circle.Radius - PositionX);
            CircleOutOfBounds();
            if (MathHelper.IsMouseInCircle(Circle, mousePos) && mouseState)
            {
                Clicked = true;
            }
            if (!mouseState)
            {
                Clicked = false;
            }

            if (MathHelper.IsMouseInCircle(Circle, mousePos))
            {
                OnSliderHoveringAnimation();
            }
            else
            {
                OnSliderDefaultAnimation();
            }
            if (Clicked)
            {
                Buffer.X = mousePos.X - Circle.Radius;
                Buffer.Y = Circle.Position.Y;
                Circle.Position = Buffer;
                OnSliderPressedAnimation();
            }
            CircleOutOfBounds();
        }
        public void Update(Vector2i mousePos, bool mouseState)
        {
            if (StartUp)
            {
                SetCircleToValue(Value);
                StartUp = false;
            }
            Rectangle.Position = Position;
            Rectangle.Size = Size;
            Circle.PositionY = PositionY + Size.Y / 2 - Circle.Radius;
            Value = MathHelper.Map(new Vector2f(0, Size.X), Scale, Circle.Position.X + Circle.Radius - PositionX);
            CircleOutOfBounds();
            if (MathHelper.IsMouseInCircle(Circle, mousePos) && mouseState)
            {
                Clicked = true;
            }
            if (!mouseState)
            {
                Clicked = false;
            }

            if (MathHelper.IsMouseInCircle(Circle, mousePos))
            {
                OnSliderHoveringAnimation();
            }
            else
            {
                OnSliderDefaultAnimation();
            }
            if (Clicked)
            {
                Buffer.X = mousePos.X - Circle.Radius;
                Buffer.Y = Circle.Position.Y;
                Circle.Position = Buffer;
                OnSliderPressedAnimation();
            }
            CircleOutOfBounds();
        }

        public void Draw(RenderWindow window)
        {
            Rectangle.Draw(window);
            Circle.Draw(window);
        }

        public void SetCircleToValue(float value)
        {
            if (value < Scale.X || value > Scale.Y)
            {
                return;
            }
            Circle.PositionX = MathHelper.Map(Scale, new Vector2f(0, Size.X), value) + PositionX - Circle.Radius;
        }

        private void CircleOutOfBounds()
        {
            if (Circle.Position.X + Circle.Radius > Position.X + Size.X)
            {
                Buffer.X = Position.X + Size.X - Circle.Radius;
                Buffer.Y = Circle.Position.Y;
                Circle.Position = Buffer;
            }
            if (Circle.Position.X + Circle.Radius < Position.X)
            {
                Buffer.X = PositionX - Circle.Radius;
                Buffer.Y = Circle.Position.Y;
                Circle.Position = Buffer;
            }
        }

        protected virtual void OnSliderDefaultAnimation()
        {
            if (SliderDefaultAnimation != null)
                SliderDefaultAnimation(this, EventArgs.Empty);
            else
                DefaultStateAnimation();
        }
        protected virtual void OnSliderHoveringAnimation()
        {
            if (SliderHoveringAnimation != null)
                SliderHoveringAnimation(this, EventArgs.Empty);
            else
                OnMouseHoverAnimation();
        }
        protected virtual void OnSliderPressedAnimation()
        {
            if (SliderPressedAnimation != null)
                SliderPressedAnimation(this, EventArgs.Empty);
            else
                OnMousePressedAnimation();
        }

        private void DefaultStateAnimation()
        {
            Circle.FillColor = Color.White;
            Circle.OutlineColor = Color.Black;
        }
        private void OnMouseHoverAnimation()
        {
            Circle.FillColor = Color.Black;
            Circle.OutlineColor = Color.White;
        }
        private void OnMousePressedAnimation()
        {
            Circle.FillColor = Color.White;
            Circle.OutlineColor = Color.Blue;
        }
    }
}
