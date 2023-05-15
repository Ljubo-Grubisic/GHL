using SFML.Graphics;
using SFML.System;

namespace GHL.Helper.SFML
{
    public static class ImageHelper
    {
        private static readonly string ImgsPath = "Resources/Imgs/";

        public static Texture LoadImgNoBackground(string filename, byte rangeOffElimination)
        {
            Image image = new Image(ImgsPath + filename);
            Color[] colorFilter;

            for (byte i = 255; i > rangeOffElimination; i--)
            {
                image.CreateMaskFromColor(new Color(i, i, i));
            }

            Texture texture = new Texture(image);
            image.Dispose();

            return texture;
        }
        public static Image CreateImage(Vector2u imageSize, Color fillColor, Color outlineColor)
        {
            Color[,] pixelData2DColor = new Color[imageSize.X, imageSize.Y];
            for (int row = 0; row < imageSize.Y; row++)
            {
                for (int column = 0; column < imageSize.X; column++)
                {
                    if (row == 0 || row == imageSize.Y || column == 0 || column == imageSize.X)
                    {
                        pixelData2DColor[row, column] = outlineColor;
                    }
                    else
                    {
                        pixelData2DColor[row, column] = fillColor;
                    }
                }
            }

            return new Image(pixelData2DColor);
        }
        public static Image CreateImage(Vector2u imageSize, Color fillColor)
        {
            Color[,] pixelData2DColor = new Color[imageSize.X, imageSize.Y];
            for (int row = 0; row < imageSize.Y; row++)
            {
                for (int column = 0; column < imageSize.X; column++)
                {
                    if (row == 0 || row == imageSize.Y || column == 0 || column == imageSize.X)
                    {
                        pixelData2DColor[row, column] = Color.Black;
                    }
                    else
                    {
                        pixelData2DColor[row, column] = fillColor;
                    }
                }
            }

            return new Image(pixelData2DColor);
        }

        public static Color[,] Convert2dArrayRGBToColor(byte[,] byteArray)
        {
            int height = byteArray.GetLength(0);
            int width = byteArray.GetLength(1);

            Color[,] PixelData = new Color[height, width];

            for (int i = 0; i < width; i += 3)
            {
                for (int j = 0; j < height; j++)
                {
                    PixelData[i, j] = new Color(byteArray[i, j], byteArray[i + 1, j], byteArray[i + 2, j]);
                }
            }

            return PixelData;
        }

        public static byte[] FlattenByteArray(byte[,] byteArray)
        {
            int height = byteArray.GetLength(0);
            int width = byteArray.GetLength(1);

            byte[] flattenedArray = new byte[height * width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    flattenedArray[i * width + j] = byteArray[i, j];
                }
            }

            return flattenedArray;
        }

        public static byte[,] UnflattenByteArray(byte[] flattenedArray, int height, int width)
        {
            byte[,] byteArray = new byte[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    byteArray[i, j] = flattenedArray[i * width + j];
                }
            }

            return byteArray;
        }
    }
}
