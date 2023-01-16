using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used to handle rotating laser block.
/// </summary>
public class RotatingLaserBlock : Emitter
{
    [Header("Rotating parameters")]

    /// <summary>
    /// Which way is this block?
    /// </summary>
    [SerializeField]
    private bool _turnWay;



    /// <summary>
    /// Method called to initialize an emitter.
    /// </summary>
    /// <param name="difficulties">All level difficulties</param>
    public override void Initialize(List<LevelDifficulty> difficulties)
    {
        base.Initialize(difficulties);

        StartCoroutine(ChargeUpLaser());
    }


    /// <summary>
    /// Coroutine used to delay next shot.
    /// </summary>
    protected override IEnumerator Shot()
    {
        _canon.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
        ShotProjectile(_semiLaser, _difficulty.ReactionTime);

        yield return new WaitForSeconds(_difficulty.ReactionTime);

        StartEmitting(999, 0.01f);
    }


    /// <summary>
    /// Update method, called each frame.
    /// </summary>
    private void Update()
    {
        transform.Rotate(Vector3.forward * (_turnWay ? 1 : -1) * _difficulty.BlockRotationSpeed);
    }
}