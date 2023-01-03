using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used to handle player behavior.
/// </summary>
/// <remarks>Needs a RigidBody2D, Animator and spriteRenderer components</remarks>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    /// <summary>
    /// Speed factor while moving.
    /// </summary>
    [SerializeField]
    [Range(0f, 1.5f)]
    protected float _speed;
    protected float _speedMax;

    /// <summary>
    /// All sprites used in destroyed parts.
    /// </summary>
    [SerializeField]
    private List<Sprite> _destroyedParts;

    /// <summary>
    /// Destroyed parts prefab, called when destroyed.
    /// </summary>
    [SerializeField]
    private GameObject _destroyedPartPrefab;

    /// <summary>
    /// Shadow sprite renderer component.
    /// </summary>
    [SerializeField]
    private SpriteRenderer _shadowSpriteRenderer;


    /// <summary>
    /// The animator component that will be updated.
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

    /// <summary>
    /// Inuts vector.
    /// </summary>
    protected Vector2 _inputs;

    /// <summary>
    /// Does the player is dead?
    /// </summary>
    public bool Dead { get; private set; }


    /// <summary>
    /// Does the player is invicible?
    /// </summary>
    public bool Invicible { get; private set; }



    /// <summary>
    /// Awake method, called at first.
    /// </summary>
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _speedMax = _speed;
    }


    /// <summary>
    /// Initialize method used when object is created.
    /// </summary>
    /// <param name="newPosition">The new position of the player</param>
    public void Initialize(Vector2 newPosition, bool hard)
    {
        Dead = false;
        Invicible = false;

        transform.position = newPosition;

        _speed = (hard ? 0.55f : 1) * _speedMax;

        SwitchState(true);
    }


    /// <summary>
    /// Update method, called every frame.
    /// </summary>
    protected void Update()
    {
        _inputs = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }


    /// <summary>
    /// Fixed update method, called 50 times a second.
    /// </summary>
    protected void FixedUpdate()
    {
        Move(_inputs);
    }


    /// <summary>
    /// Method used to move the entity.
    /// </summary>
    /// <param name="inputs">Player inputs</param>
    protected void Move(Vector2 inputs)
    {
        if (inputs.magnitude > 1)
            inputs = inputs.normalized;

        ChangeAnimation("move", inputs.x != 0 || inputs.y != 0);

        if (inputs.y != 0)
            ChangeAnimation("back", inputs.y > 0);
            
        if (inputs.x != 0)
            _spriteRenderer.flipX = inputs.x < 0;

        _rigidBody.MovePosition(_rigidBody.position + inputs * Time.deltaTime * _speed);
        _spriteRenderer.sortingOrder = (int)Camera.main.WorldToScreenPoint(transform.position).y * -1;
        _shadowSpriteRenderer.sortingOrder = _spriteRenderer.sortingOrder - 1;
    }


    /// <summary>
    /// Method called when we want to change the state of the player.
    /// </summary>
    /// <param name="activated">Does the player is activated or not?</param>
    private void SwitchState(bool activated)
    {
        enabled = activated;
        _spriteRenderer.enabled = activated;
        _shadowSpriteRenderer.enabled = activated;

        Dead = !activated;
    }


    /// <summary>
    /// Method used to change animation based on status.
    /// </summary>
    /// <param name="name">Name of the animation</param>
    /// <param name="status">The new status of this animation</param>
    protected void ChangeAnimation(string name, bool status)
    {
        _animator.SetBool(name, status);
    }


    /// <summary>
    /// Method called when an entity gets hit.
    /// </summary>
    public void GetHit()
    {
        ExplodeIntoPieces();
        Controller.Instance.LevelController.FinishLevel(false);

        SwitchState(false);
    }


    /// <summary>
    /// Method called when the level is finished.
    /// </summary>
    public void BecameInvicible()
    {
        Invicible = true;
    }


    /// <summary>
    /// Method called when the player dies, it then explodes in a random number of pieces.
    /// </summary>
    private void ExplodeIntoPieces()
    {
        PoolController poolController = Controller.Instance.PoolController;

        for (int i = 0; i < Random.Range(3, 8); i++)
        {
            Vector2 directions = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 25;
            GameObject buffer = poolController.Out(_destroyedPartPrefab);
            buffer.transform.position = transform.position;
            buffer.GetComponent<Rigidbody2D>().AddForce(directions);
            buffer.GetComponent<SpriteRenderer>().sprite = _destroyedParts[Random.Range(0, _destroyedParts.Count)];
        }
    }
}