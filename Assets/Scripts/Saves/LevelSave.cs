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
    /// Does this level is locked?
    /// </summary>
    public bool Locked;

    /// <summary>
    /// Does this level is finished in hard mode?
    /// </summary>
    public bool Hard;

    /// <summary>
    /// Does this level is finished?
    /// </summary>
    public bool Win;



    /// <summary>
    /// Constructor of the class.
    /// </summary>
    /// <param name="locked">Does the new object is locked?</param>
    public LevelSave(bool locked)
    {
        Time = 0;
        Locked = locked;
        Hard = false;
        Win = false;
    }
}