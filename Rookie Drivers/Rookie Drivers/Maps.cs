using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RookieDrivers
{
    class Maps
    {
        Texture2D texture;
        Vector2[] positions;

        // The playerSpeed which the background is moving
        int speed;

        public void Initialize(ContentManager content, String texturePath, int screenWidth, int screenHeight, int speed)
        {
            texture = content.Load<Texture2D>(texturePath);
            this.speed = speed;

            // If we divide the screen with the texture width then we can determine the number of tiles need.
            // We add 1 to it so that we won't have a gap in the tiling
            positions = new Vector2[screenHeight/texture.Height + 1];
            //positions = new Vector2[3];

            // Set the initial positions of the background
            for (int i = 0; i < positions.Length; i++)
            {
                // We need the tiles to be side by side to create a tiling effect
                positions[i] = new Vector2(((screenWidth - texture.Width) / 2), i * texture.Height);
            }
        }

        public void Update()
        {
            for (int i = 0; i < positions.Length; i++)
            {
                // Update the position of the screen by adding the playerSpeed
                positions[i].Y += -speed;

                if (speed < positions.Length)
                {
                    // Check if the texture is out of view then position it to the start of the screen
                    if (positions[i].Y >= texture.Height * (positions.Length - 1))
                    {
                        positions[i].Y = -(texture.Height - 15);
                    }
                }
            }
        }

        public void Update(float speed)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                // Update the position of the screen by adding the playerSpeed
                positions[i].Y += -speed;

                if (speed < positions.Length)
                {
                    // Check if the texture is out of view then position it to the start of the screen
                    if (positions[i].Y >= texture.Height * (positions.Length -1)-1)
                    {
                        positions[i].Y = -(texture.Height + speed + 1);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                spriteBatch.Draw(texture, positions[i], Color.White);
            }
        }
    }
}
