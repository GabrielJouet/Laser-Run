using UnityEngine;

public class MovingLaserBlock : LaserBlock
{
    [SerializeField]
    private Vector2 _minPosition;

    [SerializeField]
    private Vector2 _maxPosition;

    [SerializeField]
    [Range(0.01f, 0.35f)]
    private float _speed;

    private Vector2 _goal;



    protected override void Awake()
    {
        base.Awake();
        _goal = _minPosition;
    }


    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _goal, Time.deltaTime * _speed);

        if ((Vector2)transform.position == _goal)
            _goal = _goal == _minPosition ? _maxPosition : _minPosition;
    }
}