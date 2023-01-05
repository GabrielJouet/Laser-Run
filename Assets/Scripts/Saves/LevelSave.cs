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
    /// State of this level.
    /// </summary>
    public LevelState State { get; set; }



    /// <summary>
    /// Constructor of the class.
    /// </summary>
    /// <param name="locked">Does the new object is locked?</param>
    public LevelSave(bool locked)
    {
        Time = 0;
        State = locked ? LevelState.LOCKED : LevelState.OPENED;
    }
}