using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RookieDrivers
{
    class PowerUps
    {
        // Textures representing powerUp
        public Animation powerUpAnimation;

        // Position of the powerUp
        public Vector2 Position;

        public float powerUpSpeed;
        public bool activeOnScreen;

        //Properties specific to Powerups
        public float oppSpeedMultiplier;
        public float playerSpeedMultiplier;
        public float playerHealthMultiplier;
        public float oppHealthMultiplier;
        public int count;
        public Boolean isActive;

        //Default Constructor
        public PowerUps()
        {
            this.oppHealthMultiplier    = 1;
            this.oppSpeedMultiplier     = 1;
            this.playerHealthMultiplier = 1;
            this.playerSpeedMultiplier  = 1;
            this.count                  = 0;
            this.isActive               = true;
        }

        //Parameterized Constructor
        public PowerUps(float oppHealthMultiplier, float oppSpeedMultiplier, float playerHealthMultiplier, float playerSpeedMultiplier, Boolean isActive)
        {
            this.oppHealthMultiplier    = oppHealthMultiplier;
            this.oppSpeedMultiplier     = oppSpeedMultiplier;
            this.playerHealthMultiplier = playerHealthMultiplier;
            this.playerSpeedMultiplier  = playerSpeedMultiplier;
            this.count                  = 0;
            this.isActive               = isActive;
        }

        public void setCount(int count)
        {
            this.count = count;
        }

        public void setOpponentSpeed(float oppSpeedMultiplier)
        {
            this.oppSpeedMultiplier = oppSpeedMultiplier;
        }

        public void setPlayerSpeed(float playerSpeedMultiplier)
        {
            this.playerSpeedMultiplier = playerSpeedMultiplier;
        }

        public void setOpponentHealth(float oppHealthMultiplier)
        {
            this.oppHealthMultiplier = oppHealthMultiplier;
        }
        
        public void setPlayerHealth(float playerHealthMultiplier)
        {
            this.playerHealthMultiplier = playerHealthMultiplier;
        }
        
        public void setActive(Boolean active)
        {
            this.isActive = active;
        }

        public float getOpponentSpeed()
        {
            return oppSpeedMultiplier;
        }

        public float getPlayerSpeed()
        {
            return playerSpeedMultiplier;
        }

        public float getOpponentHealth()
        {
            return oppHealthMultiplier;
        }

        public float getPlayerHealth()
        {
            return playerHealthMultiplier;
        }

        public int getCount()
        {
            return count;
        }

        public Boolean getActive()
        {
            return isActive;
        }

        // Power-up width
        public int Width
        {
            get { return powerUpAnimation.FrameWidth; }
        }

        // Power-up height
        public int Height
        {
            get { return powerUpAnimation.FrameHeight; }
        }

        //Power-up Generation
        public void Intialize(Animation powerUpTexture, Vector2 position)
        {
            powerUpAnimation = powerUpTexture;
            Position = position;
            powerUpSpeed = 5f;                                              // RAD: Should be same as map updation speed
            activeOnScreen = true;
        }


        public void Update(GameTime gameTime)
        {
            // powerUp will flow from top to bottom
            Position.Y += powerUpSpeed;

            powerUpAnimation.Position = Position;
            powerUpAnimation.Update(gameTime);

            // Deactivating powerUp if it crosses screen or health reaches 0
            if (Position.Y < -Height)
            {
                // Deactivating powerUp will remove powerUp from the game
                activeOnScreen = false;
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            powerUpAnimation.Draw(spriteBatch);
        }
    }
}
