using SFML.Graphics;
using SFML.System;

namespace GHL.Graphics
{
    public class Rectangle : RectangleShape
    {
        private Vector2f Buffer = new Vector2f(0, 0);
        public float PositionX
        {
            get { return Position.X; }
            set
            {
                Buffer.X = value;
                Buffer.Y = PositionY;
                Position = Buffer;
            }
        }
        public float PositionY
        {
            get { return Position.Y; }
            set
            {
                Buffer.X = PositionX;
                Buffer.Y = value;
                Position = Buffer;
            }
        }

        public float SizeX
        {
            get { return Size.X; }
            set
            {
                Buffer.X = value;
                Buffer.Y = SizeY;
                Size = Buffer;
            }
        }
        public float SizeY
        {
            get { return Size.Y; }
            set
            {
                Buffer.X = SizeX;
                Buffer.Y = value;
                Size = Buffer;
            }
        }

        public Rectangle(Vector2f position, Vector2f size) : base()
        {
            Position = position;
            Size = size;
        }
        public Rectangle(Vector2f position, Vector2f size, Texture texture) : base()
        {
            Position = position;
            Size = size;
            Texture = texture;
        }
        public Rectangle(Vector2f position, float sizeX, float sizeY) : base()
        {
            Position = position;
            Size = new Vector2f(sizeX, sizeY);
        }
        public Rectangle(float positionX, float positionY, Vector2f size) : base()
        {
            Position = new Vector2f(positionX, positionY);
            Size = size;
        }
        public Rectangle(float positionX, float positionY, float sizeX, float sizeY) : base()
        {
            Position = new Vector2f(positionX, positionY);
            Size = new Vector2f(sizeX, sizeY);
        }

        public void Draw(RenderWindow window)
        {
            Draw(window, RenderStates.Default);
        }
    }
}
