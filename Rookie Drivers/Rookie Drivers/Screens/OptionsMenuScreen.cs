using Microsoft.Xna.Framework;

namespace RookieDrivers.Screens
{
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        MenuEntry Difficulty;
        MenuEntry Video;
        MenuEntry Audio;
        MenuEntry MusicVolume;

        enum GameHardness
        {
            Amature,
            Professional,
            Legendary
        }

        static GameHardness currentDifficulty = GameHardness.Amature;

        static string[] videoResolutions = { GameConstants.VIDEO_RESOLUTION_1024_768, GameConstants.VIDEO_RESOLUTION_1200_720, GameConstants.VIDEO_RESOLUTION_1800_600 };
        static int currentResolution = 0;

        static bool CurrentAudioStatus = true;

        static int currentMusicVolume = 24;

        #endregion

        #region Initialization


        public OptionsMenuScreen()
            : base(GameConstants.MENU_OPTIONS)
        {
            Difficulty = new MenuEntry(string.Empty);
            Video = new MenuEntry(string.Empty);
            Audio = new MenuEntry(string.Empty);
            MusicVolume = new MenuEntry(string.Empty);

            SetMenuEntryText();

            MenuEntry back = new MenuEntry(GameConstants.MENU_BACK);

            Difficulty.Selected += DifficultyEntrySelected;
            Video.Selected += VideoMenuEntrySelected;
            Audio.Selected += AudioMenuEntrySelected;
            MusicVolume.Selected += MusicVolumeMenuEntrySelected;
            back.Selected += OnCancel;
            MenuEntries.Add(Difficulty);
            MenuEntries.Add(Video);
            MenuEntries.Add(Audio);
            MenuEntries.Add(MusicVolume);
            MenuEntries.Add(back);
        }

        void SetMenuEntryText()
        {
            Difficulty.Text = GameConstants.MENU_CURRENT_DIFFICULTY + currentDifficulty;
            Video.Text = GameConstants.MENU_VIDEO_RESOLUTION + videoResolutions[currentResolution];
            Audio.Text = GameConstants.MENU_AUDIO_STATUS + (CurrentAudioStatus ? GameConstants.MENU_ON : GameConstants.MENU_OFF);
            MusicVolume.Text = GameConstants.MENU_MUSIC_VOLUME + currentMusicVolume;
        }


        #endregion

        #region Handle Input

        void DifficultyEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentDifficulty++;

            if (currentDifficulty > GameHardness.Legendary)
                currentDifficulty = 0;

            SetMenuEntryText();
        }


        void VideoMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentResolution = (currentResolution + 1) % videoResolutions.Length;

            SetMenuEntryText();
        }


        void AudioMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            CurrentAudioStatus = !CurrentAudioStatus;

            SetMenuEntryText();
        }


        void MusicVolumeMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentMusicVolume++;

            SetMenuEntryText();
        }
        #endregion
    }
}
