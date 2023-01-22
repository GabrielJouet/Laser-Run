using System;

/// <summary>
/// Class used to hold every save data of a level.
/// </summary>
[Serializable]
public class LevelSave
{
    /// <summary>
    /// Reached time of this level.
    /// </summary>
    public float Time;

    /// <summary>
    /// Reached time of this level in hard mode.
    /// </summary>
    public float HardTime;

    /// <summary>
    /// State of this level.
    /// </summary>
    public LevelState State;



    /// <summary>
    /// Constructor of the class.
    /// </summary>
    /// <param name="locked">Does the new object is locked?</param>
    public LevelSave(bool locked)
    {
        Time = 0;
        HardTime = 0;
        State = locked ? LevelState.LOCKED : LevelState.OPENED;
    }
}