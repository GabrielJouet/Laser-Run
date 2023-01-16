using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used to handle moving laser block behavior in top of classic block.
/// </summary>
public class MovingLaserBlock : LaserBlock
{
    [Header("Moving parameters")]

    /// <summary>
    /// Minimal position of this block.
    /// </summary>
    [SerializeField]
    private Vector2 _minPosition;

    /// <summary>
    /// Maximal position of this block.
    /// </summary>
    [SerializeField]
    private Vector2 _maxPosition;


    /// <summary>
    /// Next goal of this block.
    /// </summary>
    private Vector2 _goal;


    /// <summary>
    /// Does this laser block is processing a new direction?
    /// </summary>
    private bool _processingNewDirection = false;



    /// <summary>
    /// Method called to initialize an emitter.
    /// </summary>
    /// <param name="difficulties">All level difficulties</param>
    public override void Initialize(List<LevelDifficulty> difficulties)
    {
        base.Initialize(difficulties);

        StartCoroutine(ChangeDirection());
    }


    /// <summary>
    /// Awake method, called at initialization.
    /// </summary>
    protected void Awake()
    {
        _goal = Random.Range(0, 2) == 1 ? _minPosition : _maxPosition;
    }


    /// <summary>
    /// Update method, called each frame.
    /// </summary>
    private void Update()
    {
        if (!_processingNewDirection)
        {
            transform.position = Vector3.MoveTowards(transform.position, _goal, Time.deltaTime * _difficulty.Speed);

            if ((Vector2)transform.position == _goal)
                StartCoroutine(ChangeDirection());
        }
    }


    /// <summary>
    /// Coroutine used to delay the direction change.
    /// </summary>
    private IEnumerator ChangeDirection()
    {
        _processingNewDirection = true;

        yield return new WaitForSeconds(_difficulty.TimeBeforeDirectionChange);

        _goal = _goal == _minPosition ? _maxPosition : _minPosition;
        _processingNewDirection = false;
    }
}