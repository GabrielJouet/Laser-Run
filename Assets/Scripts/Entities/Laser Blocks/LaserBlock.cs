/// <summary>
/// Class used to handle every laser block behavior.
/// </summary>
public class LaserBlock : Emitter
{
    /// <summary>
    /// Does the laser is used?
    /// </summary>
    public bool Used { get; protected set; }



    /// <summary>
    /// Method used to load a new difficulty and start the laser.
    /// </summary>
    public void WarmUp()
    {
        Used = true;

        StartCoroutine(ChargeUpLaser());
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