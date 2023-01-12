using UnityEngine;

/// <summary>
/// Class used to handle every laser block behavior.
/// </summary>
/// <remarks>Needs to have an audio source component attached</remarks>
[RequireComponent(typeof(AudioSource))]
public class LaserBlock : Emitter
{
    /// <summary>
    /// Does the laser is used?
    /// </summary>
    public bool Used { get; protected set; }



    /// <summary>
    /// Method used to load a new difficulty and start the laser.
    /// </summary>
    /// <param name="difficulty">The difficulty wanted</param>
    public void WarmUp(LevelDifficulty difficulty)
    {
        _difficulty = difficulty;

        Used = true;

        StartCoroutine(ChargeUpLaser(_difficulty.LoadTime));
    }


    /// <summary>
    /// Method called to reset the object back to its original state.
    /// </summary>
    public override void ResetObject()
    {
        Used = false;

        base.ResetObject();
    }
}