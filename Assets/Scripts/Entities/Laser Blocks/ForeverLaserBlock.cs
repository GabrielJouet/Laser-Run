using System.Collections;
using UnityEngine;

/// <summary>
/// Class used to handle lengthy shooting laser block.
/// </summary>
public class ForeverLaserBlock : Emitter
{
    /// <summary>
    /// Which side is picked?
    /// </summary>
    private bool _side;

    /// <summary>
    /// Does this laser block is emitting?
    /// </summary>
    private bool _isEmitting;

    /// <summary>
    /// Angle goal.
    /// </summary>
    private float _angleGoal;



    /// <summary>
    /// Start method, called after Awake, overriden from Emitter.
    /// </summary>
    protected override void Start()
    {
        base.Start();

        _isEmitting = false;
        StartCoroutine(ChargeUpLaser());
    }


    /// <summary>
    /// Coroutine used to delay next shot.
    /// </summary>
    protected override IEnumerator Shot()
    {
        _side = Random.Range(0, 2) == 1;
        _angleGoal = _facing * 90 + (_side ? _difficulty.MinusAngle : _difficulty.PositiveAngle);

        _canon.localRotation = Quaternion.Euler(Vector3.forward * (_facing * 90 + (_side ? _difficulty.PositiveAngle : _difficulty.MinusAngle)));

        ShotProjectile(_semiLaser, _difficulty.ReactionTime);

        yield return new WaitForSeconds(_difficulty.ReactionTime);

        _isEmitting = true;
        StartEmitting(999, 0.01f);
    }


    /// <summary>
    /// Update method, called every frame.
    /// </summary>
    private void Update()
    {
        if (_isEmitting)
        {
            _shakingCamera.ShakeCamera(0.002f);

            _canon.Rotate(Vector3.forward * ((_side ? -1 : 1) * _difficulty.RotationSpeed) * Time.deltaTime);

            if (_side && _canon.localEulerAngles.z <= _angleGoal || !_side && _canon.localEulerAngles.z >= _angleGoal)
                StartCoroutine(ChangeDirection());
        }
    }


    /// <summary>
    /// Coroutine used to put a little delay before a direction change.
    /// </summary>
    private IEnumerator ChangeDirection()
    {
        _isEmitting = false;
        yield return new WaitForSeconds(_difficulty.TimeBeforeRotationChange);

        _side = !_side;
        _angleGoal = _facing * 90 + (_side ? _difficulty.MinusAngle : _difficulty.PositiveAngle);
        _isEmitting = true;
    }


    /// <summary>
    /// Method called to reset the object back to its original state.
    /// </summary>
    public override void ResetObject()
    {
        base.ResetObject();

        StartCoroutine(ChargeUpLaser());
    }
}