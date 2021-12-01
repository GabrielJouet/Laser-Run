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

    /// <summary>
    /// Destroyed parts prefab, called when destroyed.
    /// </summary>
    [SerializeField]
    private List<GameObject> _destroyedParts;


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
    protected bool _dead = false;


    /// <summary>
    /// Initialize method used when object is created.
    /// </summary>
    public void Initialize()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _dead = false;

        enabled = true;
        _spriteRenderer.enabled = true;

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
            FlipSprite(inputs.x < 0);

        _rigidBody.MovePosition(_rigidBody.position + inputs * Time.deltaTime * _speed);
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
        if (!_dead)
        {
            PoolController poolController = Controller.Instance.PoolController;

            foreach (GameObject part in _destroyedParts)
            {
                Vector2 directions = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized * 25;
                GameObject buffer = poolController.GiveObject(part);
                buffer.transform.position = transform.position;
                buffer.GetComponent<Rigidbody2D>().AddForce(directions);
            }

            enabled = false;
            _spriteRenderer.enabled = false;
            Controller.Instance.UIController.DisplayGameOverScreen();

            _dead = true;
        }
    }
}