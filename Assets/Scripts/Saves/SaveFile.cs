using System;
using System.Collections.Generic;

/// <summary>
/// Class used to handles every save objects.
/// </summary>
[Serializable]
public class SaveFile
{
    /// <summary>
    /// Sound level saved.
    /// </summary>
    public float Sound { get; set; }

    /// <summary>
    /// Does the sound is muted?
    /// </summary>
    public bool SoundMuted { get; set; }


    /// <summary>
    /// Music level saved.
    /// </summary>
    public float Music { get; set; }

    /// <summary>
    /// Does the music is muted?
    /// </summary>
    public bool MusicMuted { get; set; }


    /// <summary>
    /// Does the bloom is active?
    /// </summary>
    public bool Bloom { get; set; }

    /// <summary>
    /// Does the chromatic aberration is active?
    /// </summary>
    public bool ChromaticAberration { get; set; }

    /// <summary>
    /// Does the film grain is active?
    /// </summary>
    public bool FilmGrain { get; set; }

    /// <summary>
    /// Does the full screen is active?
    /// </summary>
    public bool FullScreen { get; set; }

    /// <summary>
    /// Does the tutorial has been completed?
    /// </summary>
    public bool Tutorial { get; set; }

    /// <summary>
    /// Does the endless mode is unlocked?
    /// </summary>
    public bool EndlessUnlocked { get; set; }

    /// <summary>
    /// Maximum endless mode score.
    /// </summary>
    public int EndlessScore { get; set; }


    /// <summary>
    /// Level progression.
    /// </summary>
    public List<LevelSave> LevelsProgression { get; private set; }


    /// <summary>
    /// All achievements in progress.
    /// </summary>
    public List<AchievementProgress> AchievementsProgress { get; private set; }

    /// <summary>
    /// All achievements unlocked.
    /// </summary>
    public List<string> AchievementsUnlocked { get; private set; }



    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="soundLevel">New sound level</param>
    /// <param name="musicLevel">New music level</param>
    /// <param name="numberOfLevels">How much levels in the game</param>
    public SaveFile(float soundLevel, float musicLevel, int numberOfLevels)
    {
        AchievementsProgress = new List<AchievementProgress>();
        AchievementsUnlocked = new List<string>();

        LevelsProgression = new List<LevelSave> { new LevelSave(false) };

        for (int i = 0; i < numberOfLevels - 1; i++)
            LevelsProgression.Add(new LevelSave(true));

        Sound = soundLevel;
        SoundMuted = false;

        Tutorial = false;

        Music = musicLevel;
        MusicMuted = false;

        FilmGrain = true;
        Bloom = true;
        ChromaticAberration = true;

        EndlessUnlocked = false;
        FullScreen = true;
        EndlessScore = 0;
    }



    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="numberOfLevels">How much levels in the game</param>
    public SaveFile(int numberOfLevels)
    {
        AchievementsProgress = new List<AchievementProgress>();
        AchievementsUnlocked = new List<string>();

        LevelsProgression = new List<LevelSave> { new LevelSave(false) };

        for (int i = 0; i < numberOfLevels - 1; i++)
            LevelsProgression.Add(new LevelSave(true));

        Sound = 1;
        SoundMuted = false;

        Tutorial = false;

        Music = 1;
        MusicMuted = false;

        FilmGrain = true;
        Bloom = true;
        ChromaticAberration = true;

        EndlessUnlocked = false;
        FullScreen = true;
        EndlessScore = 0;
    }
}
