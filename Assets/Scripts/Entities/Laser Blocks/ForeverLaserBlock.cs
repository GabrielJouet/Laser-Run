using System.Collections;
using System.Collections.Generic;
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
    /// Method called to initialize an emitter.
    /// </summary>
    /// <param name="difficulties">All level difficulties</param>
    public override void Initialize(List<LevelDifficulty> difficulties)
    {
        base.Initialize(difficulties);

        _isEmitting = false;
        _side = true;
        _audioSource.volume = (!Controller.Instance.SaveController.SaveFile.SoundMuted ? Controller.Instance.SaveController.SaveFile.Sound : 0) * 0.5f;

        StartCoroutine(ChargeUpLaser());
    }


    /// <summary>
    /// Coroutine used to delay next shot.
    /// </summary>
    protected override IEnumerator Shot()
    {
        _angleGoal = Quaternion.Euler(Vector3.forward * (_facing * 90 + (_side ? _difficulty.MinusAngle : _difficulty.PositiveAngle))).eulerAngles.z;

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

            if (_side && _canon.localEulerAngles.z <= _angleGoal && Mathf.Abs(_canon.localEulerAngles.z - _angleGoal) < 180)
                StartCoroutine(ChangeDirection());
            else if (!_side && _canon.localEulerAngles.z >= _angleGoal && Mathf.Abs(_canon.localEulerAngles.z - _angleGoal) < 180)
                StartCoroutine(ChangeDirection());
            else if (_angleGoal == 0 && (_side && _canon.localEulerAngles.z > 355 || !_side && _canon.localEulerAngles.z < 5))
                StartCoroutine(ChangeDirection());
        }
    }


    /// <summary>
    /// Coroutine used to put a little delay before a direction change.
    /// </summary>
    private IEnumerator ChangeDirection()
    {
        _isEmitting = false;

        if (_difficulty.StopWhenNotMoving)
        {
            _currentLaser.StopLaser();
            ResetObject();
        }

        yield return new WaitForSeconds(_difficulty.TimeBeforeRotationChange);

        if (_difficulty.StopWhenNotMoving)
            StartCoroutine(ChargeUpLaser());

        _side = !_side;
        _angleGoal = Quaternion.Euler(Vector3.forward * (_facing * 90 + (_side ? _difficulty.MinusAngle : _difficulty.PositiveAngle))).eulerAngles.z;
        _isEmitting = true;
    }
}