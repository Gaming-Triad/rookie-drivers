using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace RookieDrivers
{
    class Traffic
    {
        public Texture2D TrafficAnimation;
        public Vector2 Position;

        public int Width
        {
            get { return TrafficAnimation.Bounds.Width; }
        }

        public int Height
        {
            get { return TrafficAnimation.Bounds.Height; }
        }

        float TrafficSpeed;
        public bool Active;
        public bool ShowExplosion;
        public int Health;
        public int Damage;
        public int TrafficToPlayerScore;
        private CollisionDetection collisionDetection;
        Color[,] playerColorArray;
        Color[,] trafficColorArray;
        SoundEffect CarCarshSound;
        Texture2D PlayerTexture;
        Rectangle playerRectangle;
        Rectangle trafficRectangle;
        Player player = new Player();
        float elapsed;
        public bool TrafficCollidedPlayer;
        
        public void Intialize(Texture2D trafficTexture, Vector2 position, Texture2D playerTexture, SoundEffect carCrashSound, Difficulty level)
        {
            TrafficAnimation = trafficTexture;
            Position = position;
            TrafficSpeed = 2f;  //ToDo: set speed based on Difficulty
            Active = true;
            ShowExplosion = false;
            TrafficCollidedPlayer = false;
            Health = 10;
            Damage = (int)level.getPlayerDamage();
            TrafficToPlayerScore = 50;
            collisionDetection = new CollisionDetection();
            PlayerTexture = playerTexture;
            CarCarshSound = carCrashSound;
        }

        public void Update(GameTime gameTime, Vector2 playerPosition, Player player, int screenHeight, float pavementLeftBoundary, float pavementRightBoundary, KeyboardState currentKeyboardState)
        {
            TrafficCollidedPlayer = false;

            // Traffic will flow from top to bottom
            if (currentKeyboardState.IsKeyDown(Keys.Up))
            {
                Position.Y += TrafficSpeed;
            }
            else
            {
                Position.Y -= TrafficSpeed;
            }

            CheckIntersection(playerPosition);

            // Checking Traffic if it hits Map Pavement
            if (Position.X <= pavementLeftBoundary)
            {
                ShowExplosion = true;
                Health = 0;
                Active = false;
            }

            if (Position.X >= pavementRightBoundary - Width)
            {
                ShowExplosion = true;
                Health = 0;
                Active = false;
            }

            // Deactivating Traffic if it crosses SCREEN HEIGHT or HEALTH reaches 0
            if (Active)
            {
                if(Position.Y > screenHeight || Health <= 0)
                {
                    Active = false;
                }
            }
        }

        private void CheckIntersection(Vector2 playerPosition)
        {
            playerRectangle = new Rectangle((int)playerPosition.X, (int)playerPosition.Y, PlayerTexture.Width, PlayerTexture.Height);
            trafficRectangle = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            if (playerRectangle.Intersects(trafficRectangle))
            {
                CheckPlayersCollision(playerPosition);
            }
        }

        private Vector2 CheckPlayersCollision(Vector2 playerPosition)
        {
            playerColorArray = collisionDetection.TextureTo2DArray(PlayerTexture);
            trafficColorArray = collisionDetection.TextureTo2DArray(TrafficAnimation);
            Matrix playerMatrix = Matrix.CreateTranslation(-(PlayerTexture.Bounds.Width), -(PlayerTexture.Bounds.Height), 0) * 1 * 1 * Matrix.CreateTranslation(playerPosition.X, playerPosition.Y, 0);
            Matrix trafficMatrix = Matrix.CreateTranslation(-(TrafficAnimation.Bounds.Width), -(TrafficAnimation.Bounds.Height), 0) * 1 * Matrix.CreateTranslation(Position.X, Position.Y, 0);
            Vector2 playerCollisionPoint = collisionDetection.TexturesCollide(trafficColorArray, trafficMatrix, playerColorArray, playerMatrix);
            if (playerCollisionPoint.X > -1)
            {
                CarCarshSound.Play();
                player.setPlayerHealth(player.getPlayerHealth() - Damage);
                CollisionImpact(playerPosition);
            }
            return new Vector2(-1, -1);
        }

        private void CollisionImpact(Vector2 playerPosition)
        {
            CalculatePlayerTrafficCurrentPosition(playerPosition);
        }

        private void CalculatePlayerTrafficCurrentPosition(Vector2 playerPosition)
        {
            if (Position.X != playerPosition.X)
            {
                // TRAFFIC is on LEFT side of PLAYER
                if (Position.X < playerPosition.X)
                {
                    Position.X -= 70;
                }

                // TRAFFIC is on RIGHT side of PLAYER
                else if (Position.X > playerPosition.X)
                {
                    Position.X += 70;
                }

                // TRAFFIC is in X-Cordinate as PLAYER
                else
                {
                    Position.X -= 70;
                }
                TrafficCollidedPlayer = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TrafficAnimation, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

    }
}
