using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RookieDrivers
{
    class CollisionDetection
    {
        
        public CollisionDetection()
        {

        }

        // This method will check if Non-Transparent pixels of 2 images intersect each other or not. On intersection screenPosition is returned else keeps returning (-1, -1)
        public Vector2 TexturesCollide(Color[,] textureA, Matrix matrixA, Color[,] textureB, Matrix matrixB)
        {
            Matrix matrixAtoB = matrixA * Matrix.Invert(matrixB);
            int widthA = textureA.GetLength(0);
            int heightA = textureA.GetLength(1);
            int widthB = textureB.GetLength(0);
            int heightB = textureB.GetLength(1);

            for (int x1 = 0; x1 < widthA; x1++)
            {
                for (int y1 = 0; y1 < heightA; y1++)
                {
                    Vector2 positionA = new Vector2(x1, y1);
                    Vector2 positionB = Vector2.Transform(positionA, matrixAtoB);

                    int x2 = (int)positionB.X;
                    int y2 = (int)positionB.Y;
                    if ((x2 >= 0) && (x2 < widthB))
                    {
                        if ((y2 >= 0) && (y2 < heightB))
                        {
                            if (textureA[x1, y1].A > 0)
                            {
                                if (textureB[x2, y2].A > 0)
                                {
                                    Vector2 screenPosition = Vector2.Transform(positionA, matrixA);
                                    return screenPosition;
                                }
                            }
                        }
                    }
                }
            }

            return new Vector2(-1, -1);
        }

        // This method will extract the 2D color arrays of our images
        public Color[,] TextureTo2DArray(Texture2D texture)
        {
            Color[] colors1D = new Color[texture.Width * texture.Height];
            texture.GetData(colors1D);

            Color[,] colors2D = new Color[texture.Width, texture.Height];
            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    colors2D[x, y] = colors1D[x + y * texture.Width];
                }
            }
            return colors2D;
        }

    }
}
