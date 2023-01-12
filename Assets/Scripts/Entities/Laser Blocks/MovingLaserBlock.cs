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
    /// Speed of the block.
    /// </summary>
    [SerializeField]
    [Range(0.01f, 0.35f)]
    private float _speed;


    /// <summary>
    /// Next goal of this block.
    /// </summary>
    private Vector2 _goal;



    /// <summary>
    /// Awake method, called at initialization.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        _goal = Random.Range(0, 2) == 1 ? _minPosition : _maxPosition;
    }


    /// <summary>
    /// Update method, called each frame.
    /// </summary>
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _goal, Time.deltaTime * _speed);

        if ((Vector2)transform.position == _goal)
            _goal = _goal == _minPosition ? _maxPosition : _minPosition;
    }
}