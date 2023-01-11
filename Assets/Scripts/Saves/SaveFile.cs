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
    /// Level progression.
    /// </summary>
    public List<LevelSave> LevelsProgression { get; private set; }


    /// <summary>
    /// All achievements unlocked.
    /// </summary>
    public List<AchievementProgress> Achievements { get; private set; }


    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="soundLevel">New sound level</param>
    /// <param name="musicLevel">New music level</param>
    /// <param name="numberOfLevels">How much levels in the game</param>
    public SaveFile(float soundLevel, float musicLevel, int numberOfLevels)
    {
        Achievements = new List<AchievementProgress>();

        LevelsProgression = new List<LevelSave> { new LevelSave(false) };

        for (int i = 0; i < numberOfLevels - 1; i++)
            LevelsProgression.Add(new LevelSave(true));

        Sound = soundLevel;
        SoundMuted = false;

        Music = musicLevel;
        MusicMuted = false;

        FilmGrain = true;
        Bloom = true;
        ChromaticAberration = true;

        FullScreen = true;
    }
}
