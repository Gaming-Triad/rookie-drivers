using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using RookieDrivers;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.Linq;
using RookieDrivers.Manager;

namespace RookieDrivers.Screens
{
    class GameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteFont gameFont;
        SpriteFont font;                                // The font used to display UI elements

        Vector2 playerPosition = new Vector2(100, 100);
        Vector2 enemyPosition = new Vector2(100, 100);

        float pauseAlpha;
        int counter;

        Player player;
        Texture2D playerTexture;
        int playerTotalScore;                           // Number that holds the PLAYER score
        float playerMoveSpeed;                          // A movement playerSpeed for the player
        float playerMaxSpeed;

        static int ClippedRoad_X;                       // To restrict the Objects to Road dimensions
        static int LaneWidth;                           // Width of the Lane
        Texture2D mainBackground;
        Maps Map;

        Difficulty level1;

        Traffic traffic;
        Texture2D trafficTextures;
        Texture2D traffic1Textures;
        Texture2D traffic2Textures;
        Texture2D traffic3Textures;
        Texture2D traffic4Textures;
        Texture2D traffic5Textures;
        TimeSpan trafficSpawnTime;
        TimeSpan previousSpawnTime;
        List<Traffic> trafficList;
        List<Texture2D> trafficSelectorList;
        int iTraffic;
        bool spawnTraffic = true;
        float pavementLeftBoundary;
        float pavementRightBoundary;

        Texture2D explosionTexture;

        Texture2D powerUpTextures;
        Texture2D PowerUp1Textures;
        Texture2D PowerUp2Textures;
        TimeSpan powerUpSpawnTime;
        TimeSpan previousPowerUpSpawnTime;
        List<PowerUps> powerUpList;
        List<Texture2D> powerUpSelectorList;
        int iPowerUps;

        List<Animation> explosions;

        Texture2D bossTexture;
        Boss boss;
        public bool isBossActive = false;
        public bool flagInitBoss = false;

        Random randomNumber;

        Song gamePlayMusic;
        Song bossIntroMusic;
        SoundEffect carCarshSound;
        SoundEffect carSkid;

        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        private int screenHeight;
        private int roadLeftExtremePoint;
        private int roadRightExtremePoint;

        CollisionDetection collisionDetection;
        GraphicsDevice graphics;
        SpriteBatch spriteBatch;

        #endregion

        #region Initialization


        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            player = new Player();
            level1 = new Difficulty(1);
            playerMaxSpeed = level1.getPlayerSpeed(); // Set a constant player move playerSpeed         
            Map = new Maps();

            traffic = new Traffic();
            trafficList = new List<Traffic>();
            trafficSelectorList = new List<Texture2D>();

            explosions = new List<Animation>();

            powerUpList = new List<PowerUps>();
            powerUpSelectorList = new List<Texture2D>();

            previousSpawnTime = TimeSpan.Zero;
            previousPowerUpSpawnTime = TimeSpan.Zero;
            // Used to determine how fast Traffic respawns
            trafficSpawnTime = TimeSpan.FromMilliseconds(level1.getTrafficDensity()); // Get traffic density factor from Difficulty
            powerUpSpawnTime = TimeSpan.FromMilliseconds(level1.getPowerUpDensity());
            randomNumber = new Random();
            playerTotalScore = 0;
            iTraffic = 0;
            iPowerUps = 0;
            collisionDetection = new CollisionDetection();
            counter = 0;
        }

        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, GameConstants.CONTENT_ROOT_DIRECTORY);
            }

            gameFont = content.Load<SpriteFont>(GameConstants.GAME_FONTS);

            spriteBatch = this.ScreenManager.SpriteBatch;
            graphics = this.ScreenManager.GraphicsDevice;
            screenHeight = graphics.Viewport.Height;

            Map.Initialize(content, GameConstants.TEXTURE_PAVEMENTS, graphics.Viewport.Width, graphics.Viewport.Height, -10);
            mainBackground = content.Load<Texture2D>(GameConstants.TEXTURE_PAVEMENTS);
            ClippedRoad_X = (graphics.Viewport.Width - mainBackground.Width) / 2 + 100; // Setting the road Width, Adding 100(pavement Width)
            LaneWidth = 102;

            Vector2 playerPosition = new Vector2(ClippedRoad_X + (randomNumber.Next(0, 3) * LaneWidth) + 50, screenHeight - 150); // DAMODAR ToDo: Value of X has to be center of Road
            playerTexture = content.Load<Texture2D>(GameConstants.TEXTURE_PLAYER);
            player.Initialize(playerTexture, playerPosition);

            traffic1Textures = content.Load<Texture2D>(GameConstants.TEXTURE_TRAFFIC_1);
            traffic2Textures = content.Load<Texture2D>(GameConstants.TEXTURE_TRAFFIC_2);
            traffic3Textures = content.Load<Texture2D>(GameConstants.TEXTURE_TRAFFIC_3);
            traffic4Textures = content.Load<Texture2D>(GameConstants.TEXTURE_TRAFFIC_4);
            traffic5Textures = content.Load<Texture2D>(GameConstants.TEXTURE_TRAFFIC_5);

            explosionTexture = content.Load<Texture2D>(GameConstants.TEXTURE_EXPLOSION);

            PowerUp1Textures = content.Load<Texture2D>(GameConstants.TEXTURE_SPIKE_STRIP);
            PowerUp2Textures = content.Load<Texture2D>(GameConstants.TEXTURE_MINE);

            bossTexture = content.Load<Texture2D>(GameConstants.TEXTURE_BOSS);

            gamePlayMusic = content.Load<Song>(GameConstants.SOUNDS_HOW_WE_ROLL);
            bossIntroMusic = content.Load<Song>(GameConstants.SOUNDS_AKATSUKI);
            carCarshSound = content.Load<SoundEffect>(GameConstants.SOUNDS_EXPLOSION);
            carSkid = content.Load<SoundEffect>(GameConstants.SOUNDS_SKID);
            PlayMusic(gamePlayMusic);   // ToDo: Pass in a LIST of music

            ScreenManager.Game.ResetElapsedTime();
        }


        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            if (IsActive)
            {
                // Save the previous state of the keyboard so we can determine single key presses
                previousKeyboardState = currentKeyboardState;

                // Read the current state of the keyboard and store it
                currentKeyboardState = Keyboard.GetState();

                UpdatePlayer(gameTime);
                UpdateExplosion(gameTime);

                if (!isBossActive)
                {
                    UpdateMap(gameTime);
                    UpdateTraffic(gameTime);
                    UpdatePowerUp(gameTime);
                }
                else
                {
                    Map.Update(playerMoveSpeed);
                }

                // Boss spawns after 8 minutes of gameplay
                if (gameTime.TotalGameTime.Minutes >= 3)    // Has to be based on Player Score instead of GameTime
                {
                    if (!flagInitBoss)
                    {
                        // Removing Traffic  and PowerUps from the Map
                        trafficList = new List<Traffic>();
                        trafficSelectorList = new List<Texture2D>();

                        powerUpList = new List<PowerUps>();
                        powerUpSelectorList = new List<Texture2D>();

                        isBossActive = true;
                        boss = new Boss();
                    }
                    UpdateBoss(gameTime);
                }
            }
        }

        private void UpdatePlayer(GameTime gameTime)
        {
            // KeyBoard events
            if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                player.Position.X -= playerMoveSpeed / 3;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                player.Position.X += playerMoveSpeed / 3;
            }

            if (isBossActive)
            {
                if (currentKeyboardState.IsKeyDown(Keys.Up))
                {
                    player.Position.Y -= playerMoveSpeed / 3;
                }
                if (currentKeyboardState.IsKeyDown(Keys.Down))
                {
                    player.Position.Y += playerMoveSpeed / 3;
                }
            }

            // Make sure that the player does not go out of bounds
            player.Position.X = MathHelper.Clamp(player.Position.X, ClippedRoad_X, (graphics.Viewport.Width - ClippedRoad_X) - player.Width);
            player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, graphics.Viewport.Height - player.Height);
        }

        private void UpdateMap(GameTime gameTime)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Up))
            {
                counter += 1;
                playerTotalScore += 1;
                playerMoveSpeed = 1 + (int)(counter / playerMaxSpeed);
                if (playerMoveSpeed >= playerMaxSpeed)
                    playerMoveSpeed = playerMaxSpeed;
                Map.Update(-playerMoveSpeed);
            }
            else
            {
                counter -= 1;
                if (playerMoveSpeed > 0)
                    playerMoveSpeed -= 1 + ((float)Math.Floor(playerMoveSpeed / playerMaxSpeed));
                else
                    counter = 0;
                Map.Update(-(playerMoveSpeed));
            }
        }

        private void UpdateTraffic(GameTime gameTime)
        {
            trafficSelectorList.Add(traffic1Textures);
            trafficSelectorList.Add(traffic2Textures);
            trafficSelectorList.Add(traffic3Textures);
            trafficSelectorList.Add(traffic4Textures);
            trafficSelectorList.Add(traffic5Textures);

            // Spawn a new Traffic car
            if (spawnTraffic)
            {
                spawnTraffic = false;
                AddTraffic();
            }

            pavementLeftBoundary = ClippedRoad_X;
            pavementRightBoundary = graphics.Viewport.Width - ClippedRoad_X;

            for (int i = trafficList.Count - 1; i >= 0; i--)
            {
                trafficList[i].Update(gameTime, this.player.Position, player, screenHeight, pavementLeftBoundary, pavementRightBoundary, currentKeyboardState);

                if (trafficList[i].TrafficCollidedPlayer)
                {
                    player.Health -= 10;
                }

                if (trafficList[i].ShowExplosion)
                {
                    AddExplosion(trafficList[i].Position);
                }

                if (trafficList[i].Active == false)
                {
                    trafficList.RemoveAt(i);
                    spawnTraffic = true;
                }

            }
        }

        private void AddTraffic()
        {
            iTraffic = randomNumber.Next(0, 5);
            trafficTextures = trafficSelectorList.ElementAt<Texture2D>(iTraffic);
            int trafficWidth = trafficSelectorList.ElementAt<Texture2D>(iTraffic).Bounds.Width;
            int trafficHeight = trafficSelectorList.ElementAt<Texture2D>(iTraffic).Bounds.Height;
            Vector2 position = new Vector2(ClippedRoad_X + (randomNumber.Next(0, 3) * LaneWidth) + (50 - (trafficWidth / 2)), 0 - trafficHeight);
            traffic.Intialize(trafficTextures, position, playerTexture, carCarshSound, level1);
            trafficList.Add(traffic);
        }

        private void UpdatePowerUp(GameTime gameTime)
        {
            powerUpSelectorList.Add(PowerUp1Textures);
            powerUpSelectorList.Add(PowerUp2Textures);

            // Spawn new Weapon on screen
            if (gameTime.TotalGameTime - previousPowerUpSpawnTime > powerUpSpawnTime)
            {
                previousPowerUpSpawnTime = gameTime.TotalGameTime;

                AddPowerUp();
            }

            // Update PowerUp
            for (int i = powerUpList.Count - 1; i >= 0; i--)
            {
                powerUpList[i].Update(gameTime);

                if (powerUpList[i].activeOnScreen == false)
                {
                    powerUpList.RemoveAt(i);
                }
            }
        }

        private void AddPowerUp()
        {
            Animation powerUpAnimation = new Animation();

            iPowerUps = randomNumber.Next(0, 2);

            powerUpTextures = powerUpSelectorList.ElementAt<Texture2D>(iPowerUps);
            int powerUpWidth = powerUpSelectorList.ElementAt<Texture2D>(iPowerUps).Bounds.Width;
            int powerUpHeight = powerUpSelectorList.ElementAt<Texture2D>(iPowerUps).Bounds.Height;

            powerUpAnimation.Initialize(powerUpTextures, Vector2.Zero, powerUpWidth, powerUpHeight, 1, 0, Color.White, 1f, true);

            Vector2 position = new Vector2(ClippedRoad_X + (randomNumber.Next(0, 3) * LaneWidth) + 50, 0);

            PowerUps powerUp = new PowerUps();
            powerUp.Intialize(powerUpAnimation, position);

            powerUpList.Add(powerUp);
        }

        private void UpdateBoss(GameTime gameTime)
        {
            if (!flagInitBoss)
            {
                MediaPlayer.Stop();
                AddBoss(gameTime);
            }

            boss.Update(gameTime, this.player.Position);


            if (boss.Health <= 0)
            {
                boss.Active = false;
            }
        }

        private void AddBoss(GameTime gameTime)
        {
            //PlayMusic(bossIntroMusic);
            Vector2 position = new Vector2(ClippedRoad_X + (randomNumber.Next(0, 3) * LaneWidth) + 50, 0);

            player.Position.X = MathHelper.Clamp(player.Position.X, ClippedRoad_X, (graphics.Viewport.Width - ClippedRoad_X) - player.Width);
            player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, graphics.Viewport.Height - player.Height);

            roadLeftExtremePoint = ClippedRoad_X;
            roadRightExtremePoint = (graphics.Viewport.Width - ClippedRoad_X) - bossTexture.Width;
            boss.Initialize(bossTexture, position, screenHeight, playerTexture, bossTexture, carCarshSound, carSkid, roadLeftExtremePoint, roadRightExtremePoint);
            flagInitBoss = true;

        }

        private void UpdateExplosion(GameTime gameTime)
        {
            for (int i = explosions.Count - 1; i >= 0; i--)
            {
                explosions[i].Update(gameTime);
                if (explosions[i].Active == false)
                {
                    explosions.RemoveAt(i);
                }
            }
        }

        private void AddExplosion(Vector2 position)
        {
            Animation explosion = new Animation();
            explosion.Initialize(explosionTexture, position, 134, 134, 12, 45, Color.White, 1f, false);
            explosions.Add(explosion);
        }

        private void PlayMusic(Song song)
        {
            try
            {
                MediaPlayer.Play(song);
                MediaPlayer.IsRepeating = true;
            }
            catch { }
        }

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);

            // Our player and enemy are both actually just text strings.
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            spriteBatch.DrawString(gameFont, GameConstants.SCORE + playerTotalScore, new Vector2(100, 50), Color.White);
            // Draw the player health
            spriteBatch.DrawString(gameFont, GameConstants.HEALTH + player.Health, new Vector2(100, 80), Color.White);

            Map.Draw(spriteBatch);
            player.Draw(spriteBatch);

            for (int i = 0; i < explosions.Count; i++)
            {
                explosions[i].Draw(spriteBatch);
            }

            if (!isBossActive)
            {
                for (int i = 0; i < trafficList.Count; i++)
                {
                    trafficList[i].Draw(spriteBatch);
                }
                for (int i = 0; i < powerUpList.Count; i++)
                {
                    powerUpList[i].Draw(spriteBatch);
                }
            }

            if (isBossActive)
            {
                boss.Draw(spriteBatch);
            }


            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }


        #endregion
    }
}
