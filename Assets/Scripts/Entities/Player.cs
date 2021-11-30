using UnityEngine;

/// <summary>
/// Class used to handle player behavior.
/// </summary>
public class Player : MonoBehaviour
{

    /// <summary>
    /// Speed factor while moving.
    /// </summary>
    [SerializeField]
    [Range(0f, 1.5f)]
    protected float _speed;


    /// <summary>
    /// The sprite renderer component that will be updated.
    /// </summary>
    protected Animator _animator;

    /// <summary>
    /// Rigidbody component.
    /// </summary>
    protected Rigidbody2D _rigidBody;

    /// <summary>
    /// The sprite renderer component that will be updated.
    /// </summary>
    protected SpriteRenderer _spriteRenderer;

    protected Vector2 _inputs;


    private void Awake()
    {
        Initialize();
    }


    /// <summary>
    /// Initialize method used when object is created.
    /// </summary>
    public void Initialize()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        FlipSprite(Random.Range(0, 2) == 0);
    }


    /// <summary>
    /// Method called when we need to flip the sprite vertically.
    /// </summary>
    /// <param name="side">The side we want to flip, true is flipped</param>
    protected void FlipSprite(bool side)
    {
        _spriteRenderer.flipX = side;
    }


    /// <summary>
    /// Update method, called every frame.
    /// </summary>
    protected void Update()
    {
        _inputs = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }


    protected void FixedUpdate()
    {
        Move(_inputs);
    }


    /// <summary>
    /// Method used to move the entity.
    /// </summary>
    /// <param name="horizontal">Horizontal force</param>
    /// <param name="vertical">Vertical force</param>
    protected void Move(Vector2 inputs)
    {
        if (inputs.magnitude > 1)
            inputs = inputs.normalized;

        ChangeAnimation("move", inputs.x != 0 || inputs.y != 0);

        if (inputs.y != 0)
            ChangeAnimation("back", inputs.y > 0);
            
        if (inputs.x != 0)
            FlipSprite(inputs.x < 0);

        _rigidBody.MovePosition(_rigidBody.position + inputs * Time.deltaTime * _speed);
    }


    protected void ChangeAnimation(string name, bool status)
    {
        _animator.SetBool(name, status);
    }


    /// <summary>
    /// Method called when an entity gets hit.
    /// </summary>
    public void GetHit()
    {
        Debug.Log("hit");
    }
}