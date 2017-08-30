using Microsoft.Xna.Framework;

namespace RookieDrivers.Screens
{
    class MainMenuScreen : MenuScreen
    {
        #region Initialization
        public MainMenuScreen()
            : base(GameConstants.MENU_GAME_MENU)
        {
            // Create our menu entries.
            MenuEntry playGameMenuEntry = new MenuEntry(GameConstants.MENU_PLAY_GAME);
            MenuEntry optionsMenuEntry = new MenuEntry(GameConstants.MENU_OPTIONS);
            MenuEntry exitMenuEntry = new MenuEntry(GameConstants.MENU_EXIT);

            // Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }
        #endregion

        #region Handle Input
        void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new GameplayScreen(), e.PlayerIndex);
        }

        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }


        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = GameConstants.GAME_EXIT_CONFIRMATION;

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }


        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }
        #endregion
    }
}
