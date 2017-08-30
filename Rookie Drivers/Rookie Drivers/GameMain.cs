using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RookieDrivers.Screens;
using System;
using RookieDrivers.Manager;

namespace RookieDrivers
{
    public class GameMain : Microsoft.Xna.Framework.Game
    {
        #region Fields
        
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;

        static readonly string[] preloadAssets = {GameConstants.PRE_LOAD_ASSETS_GRADIENT};

        #endregion

        #region Initialization


        public void RenderScreenResolution(object sender, PreparingDeviceSettingsEventArgs e)
        {
            DisplayMode displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferFormat = displayMode.Format;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth = displayMode.Width;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight = displayMode.Height;
        }

        public GameMain()
        {
            Content.RootDirectory = GameConstants.CONTENT_ROOT_DIRECTORY;

            graphics = new GraphicsDeviceManager(this);
            
            screenManager = new ScreenManager(this);

            Components.Add(screenManager);

            // Activate the first screens.
            screenManager.AddScreen(new BackgroundScreen(),null);
            screenManager.AddScreen(new MainMenuScreen(),null);
            graphics.IsFullScreen = true;
            graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(RenderScreenResolution);
            
        }


        protected override void LoadContent()
        {
            foreach (string asset in preloadAssets)
            {
                Content.Load<object>(asset);
            }
        }


        #endregion

        #region Draw


        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }


        #endregion
    }
      
}
