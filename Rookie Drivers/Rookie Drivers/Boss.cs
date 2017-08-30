using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using RookieDrivers;

namespace RookieDrivers
{
    class Boss
    {
        public Texture2D BossAnimation;
        public Vector2 Position;
        private String InComingBossDirection;
        public bool Active;
        public int Health;
        public int BossToPlayerScore;
        public float BossInitSpeed;
        public int ScreenHeight;
        private int RoadLeftExtremePoint;
        private int RoadRightExtremePoint;
        private bool BossFightStart = false;
        private bool BossLoadSequenceComplete = false;
        private bool moveTowards = true;
        private bool jerk = false;
        private CollisionDetection collisionDetection;
        Color[,] playerColorArray;
        Color[,] bossColorArray;
        SoundEffect CarCarshSound;
        SoundEffect CarSkidSound;
        Texture2D PlayerTexture;
        Texture2D BossTexture;
        
        public int Width
        {
            get { return BossAnimation.Width; }
        }

        public int Height
        {
            get { return BossAnimation.Height; }
        }

        public void Initialize(Texture2D bossTexture, Vector2 position, int screenHeight, Texture2D playerTexture, Texture2D bossTextureOriginal, SoundEffect carCrashSound, SoundEffect carSkid, int roadLeftExtremePoint, int roadRightExtremePoint)
        {
            BossAnimation = bossTexture;
            Position = position;
            Active = true;
            Health = 500;
            BossInitSpeed = 2f;
            ScreenHeight = screenHeight;
            RoadLeftExtremePoint = roadLeftExtremePoint;
            RoadRightExtremePoint = roadRightExtremePoint;
            collisionDetection = new CollisionDetection();
            playerColorArray = collisionDetection.TextureTo2DArray(playerTexture);
            bossColorArray = collisionDetection.TextureTo2DArray(bossTextureOriginal);
            PlayerTexture = playerTexture;
            BossTexture = bossTextureOriginal;
            CarCarshSound = carCrashSound;
            CarSkidSound = carSkid;
        }

        public void Update(GameTime gameTime, Vector2 playerPosition)
        {
            if (!BossLoadSequenceComplete)
            {
                if (Position.Y <= (ScreenHeight / 3))
                {
                    Position.Y += BossInitSpeed;
                    if (Position.Y == (ScreenHeight / 3))
                    {
                        BossFightStart = true;
                    }
                }
            }

            if (BossFightStart)
            {
                BossLoadSequenceComplete = true;
                CheckPlayersCollision(gameTime, playerPosition);
            }
        }

        private void MoveBossTowardsPlayer(Vector2 playerPosition, GameTime gameTime)
        {
            if (Position.X != playerPosition.X)
            {
                // BOSS is on left side of PLAYER
                if (Position.X < playerPosition.X)
                {
                    Position.X += 1;    // Value will depend on Difficulty Level
                    InComingBossDirection = GameConstants.IN_COMING_BOSS_DIRECTION_LEFT;
                }

                // BOSS is on right side of PLAYER
                else if (Position.X > playerPosition.X)
                {
                    Position.X -= 1;
                    InComingBossDirection = GameConstants.IN_COMING_BOSS_DIRECTION_RIGHT;
                }
            }

            if (Position.Y != playerPosition.Y)
            {
                // BOSS is ahead of the PLAYER
                if (Position.Y < playerPosition.Y)
                {
                    Position.Y += 1;
                    InComingBossDirection = GameConstants.IN_COMING_BOSS_DIRECTION_TOP;
                }

                // BOSS is behind the PLAYER
                else if (Position.Y > playerPosition.Y)
                {
                    Position.Y -= 1;
                    InComingBossDirection = GameConstants.IN_COMING_BOSS_DIRECTION_BOTTOM;
                }
            }
        }

        private void MoveBossAwayFromPlayer(Vector2 playerCollisionPoint, GameTime gameTime)
        {
            switch (InComingBossDirection)
            {
                case GameConstants.IN_COMING_BOSS_DIRECTION_LEFT:
                    if (jerk)
                    {
                        CarSkidSound.Play();
                        Position.X -= 50;
                        jerk = false;
                    }
                    if (Position.X >= RoadLeftExtremePoint)
                    {
                        Position.X -= 1;    // Value will depend on Difficulty Level
                    }
                    else
                    {
                        moveTowards = true;
                    }
                    break;

                case GameConstants.IN_COMING_BOSS_DIRECTION_RIGHT:
                    if (jerk)
                    {
                        CarSkidSound.Play();
                        Position.X += 50;
                        jerk = false;
                    }
                    if (Position.X <= RoadRightExtremePoint)
                    {
                        Position.X += 1;
                    }
                    else
                    {
                        moveTowards = true;
                    }
                    break;

                case GameConstants.IN_COMING_BOSS_DIRECTION_TOP:
                    if (jerk)
                    {
                        CarSkidSound.Play();
                        Position.Y -= 50;
                        jerk = false;
                    }
                    if (Position.Y >= (0 - BossAnimation.Height))
                    {
                        Position.Y -= 1;
                    }
                    else
                    {
                        moveTowards = true;
                    }
                    break;

                case GameConstants.IN_COMING_BOSS_DIRECTION_BOTTOM:
                    if (jerk)
                    {
                        CarSkidSound.Play();
                        Position.Y += 50;
                        jerk = false;
                    }
                    if (Position.Y <= ScreenHeight)
                    {
                        Position.Y += 1;
                    }
                    else
                    {
                        moveTowards = true;
                    }
                    break;
            }
        }

        private void CheckPlayersCollision(GameTime gameTime, Vector2 playerPosition)
        {
            Matrix playerMatrix = Matrix.CreateTranslation(-(PlayerTexture.Bounds.Width), -(PlayerTexture.Bounds.Height), 0) * 1 * 1 * Matrix.CreateTranslation(playerPosition.X, playerPosition.Y, 0);
            Matrix bossMatrix = Matrix.CreateTranslation(-(BossTexture.Bounds.Width), -(BossTexture.Bounds.Height), 0) * 1 * Matrix.CreateTranslation(Position.X, Position.Y, 0);
            Vector2 playerCollisionPoint = collisionDetection.TexturesCollide(bossColorArray, bossMatrix, playerColorArray, playerMatrix);
            if (playerCollisionPoint.X > -1)
            {
                moveTowards = false;
                jerk = true;
                CarCarshSound.Play();
                BossToPlayerScore = -10; //This has to be linked to PlayerScore
            }
            if (!moveTowards)
            {
                MoveBossAwayFromPlayer(playerCollisionPoint, gameTime);
            }
            else
            {
                MoveBossTowardsPlayer(playerPosition, gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BossTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
