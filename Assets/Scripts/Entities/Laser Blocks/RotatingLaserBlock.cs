using UnityEngine;

/// <summary>
/// Class used to handle rotating laser block.
/// </summary>
public class RotatingLaserBlock : LaserBlock
{
    [Header("Rotating parameters")]

    /// <summary>
    /// Which way is this block?
    /// </summary>
    [SerializeField]
    private bool _turnWay;



    /// <summary>
    /// Update method, called each frame.
    /// </summary>
    private void Update()
    {
        transform.Rotate(Vector3.forward * (_turnWay ? 1 : -1) * _difficulty.BlockRotationSpeed);
    }
}