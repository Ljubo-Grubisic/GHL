using SFML.Graphics;
using SFML.System;

namespace GHL.Graphics
{
    public class Line : ConvexShape
    {
        private Vector2f[] position = new Vector2f[2];

        public new Vector2f[] Position
        {
            get { return position; }
            set
            {
                position = value;
                DataChanged();
            }
        }
        public Vector2f Position0
        {
            get { return position[0]; }
            set
            {
                position[0] = value;
                DataChanged();
            }
        }
        public float Position0X
        {
            get { return position[0].X; }
            set
            {
                position[0].X = value;
                DataChanged();
            }
        }
        public float Position0Y
        {
            get { return position[0].Y; }
            set
            {
                position[0].Y = value;
                DataChanged();
            }
        }

        public Vector2f Position1
        {
            get { return position[1]; }
            set
            {
                position[1] = value;
                DataChanged();
            }
        }
        public float Position1X
        {
            get { return position[1].X; }
            set
            {
                position[1].X = value;
                DataChanged();
            }
        }
        public float Position1Y
        {
            get { return position[1].Y; }
            set
            {
                position[1].Y = value;
                DataChanged();
            }
        }

        public Line(Vector2f positionDot1, Vector2f positionDot2) : base(4)
        {
            Position[0] = positionDot1;
            Position[1] = positionDot2;
            OutlineThickness = 1f;
            DataChanged();
        }
        public Line() : base(4)
        {
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(this);
        }

        private void DataChanged()
        {
            SetPoint(0, position[0]);
            SetPoint(1, position[0]);
            SetPoint(2, position[1]);
            SetPoint(3, position[1]);
        }
    }
}
