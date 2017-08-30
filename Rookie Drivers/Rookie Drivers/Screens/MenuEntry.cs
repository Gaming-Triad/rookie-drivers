using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RookieDrivers.Manager;

namespace RookieDrivers.Screens
{
    class MenuEntry
    {
        #region Fields
        string text;
        float selectionFade;
        Vector2 position;
        #endregion

        #region Properties
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        #endregion

        #region Events

        public event EventHandler<PlayerIndexEventArgs> Selected;

        protected internal virtual void OnSelectEntry(PlayerIndex playerIndex)
        {
            if (Selected != null)
                Selected(this, new PlayerIndexEventArgs(playerIndex));
        }
        #endregion

        #region Initialization
        
        public MenuEntry(string text)
        {
            this.text = text;
        }
        #endregion

        #region Update and Draw

        public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
        
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

            if (isSelected)
                selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
            else
                selectionFade = Math.Max(selectionFade - fadeSpeed, 0);
        }


        public virtual void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
        
            Color color = isSelected ? Color.Red : Color.Black;

            // Pulsate the size of the selected menu entry.
            double time = gameTime.TotalGameTime.TotalSeconds;
            
            float pulsate = (float)Math.Sin(time * 6) + 1;

            float scale = 1 + pulsate * 0.05f * selectionFade;

            // Modify the alpha to fade text out during transitions.
            color *= screen.TransitionAlpha;

            // Draw text, centered on the middle of each line.
            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            SpriteFont font = screenManager.Font;

            Vector2 origin = new Vector2(0, font.LineSpacing / 2);

            spriteBatch.DrawString(font, text, position, color, 0,
                                   origin, scale, SpriteEffects.None, 1);
        }


        public virtual int GetHeight(MenuScreen screen)
        {
            return screen.ScreenManager.Font.LineSpacing;
        }


        public virtual int GetWidth(MenuScreen screen)
        {
            return (int)screen.ScreenManager.Font.MeasureString(Text).X;
        }
        #endregion
    }
}
