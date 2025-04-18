using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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
    /// Color used when in normal difficulty.
    /// </summary>
    [SerializeField]
    private Color _normalColor;

    /// <summary>
    /// Color used when in hard difficulty.
    /// </summary>
    [SerializeField]
    private Color _hardColor;


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
    /// Does the player is invicible?
    /// </summary>
    public bool Invicible { get; private set; } = false;


    /// <summary>
    /// Total distance runned this time.
    /// </summary>
    private float _distanceRunned = 0;

    /// <summary>
    /// Total time runned this time.
    /// </summary>
    private float _timeRunned = 0;



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
        GetComponent<Light2D>().color = Controller.Instance.SaveController.Hard ? _hardColor : _normalColor;

        transform.position = newPosition;

        _speed = (hard ? 0.65f : 1) * _speedMax;
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
        if (_inputs.magnitude > 1)
            _inputs = _inputs.normalized;

        _animator.SetBool("move", _inputs.x != 0 || _inputs.y != 0);

        if (_inputs.y != 0)
            _animator.SetBool("back", _inputs.y > 0);

        if (_inputs.x != 0)
            _spriteRenderer.flipX = _inputs.x < 0;

        if (_inputs.x != 0 || _inputs.y != 0)
        {
            _timeRunned += Time.fixedDeltaTime;
            _distanceRunned += _inputs.magnitude * Time.fixedDeltaTime;
        }

        _rigidBody.MovePosition(_rigidBody.position + _inputs * Time.fixedDeltaTime * _speed);
        _spriteRenderer.sortingOrder = (int)Camera.main.WorldToScreenPoint(transform.position).y * -1;
        _shadowSpriteRenderer.sortingOrder = _spriteRenderer.sortingOrder - 1;
    }


    /// <summary>
    /// Method called when an entity gets hit.
    /// </summary>
    public void GetHit()
    {
        FindObjectOfType<ShakingCamera>().ShakeCamera(0.05f);

        EndlessController endlessController = FindObjectOfType<EndlessController>();
        if (endlessController)
            endlessController.FinishLevel();
        else
            Controller.Instance.LevelController.FinishLevel(false);

        enabled = false;
        _spriteRenderer.enabled = false;
        _shadowSpriteRenderer.enabled = false;

        StartCoroutine(Clean());
    }


    /// <summary>
    /// 
    /// </summary>
    private IEnumerator Clean()
    {
        List<GameObject> detritus = new List<GameObject>();
        for (int i = 0; i < Random.Range(3, 8); i++)
        {
            GameObject buffer = Instantiate(_destroyedPartPrefab);
            buffer.transform.position = transform.position;
            detritus.Add(buffer);

            buffer.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 25);
            buffer.GetComponent<SpriteRenderer>().sprite = _destroyedParts[Random.Range(0, _destroyedParts.Count)];
        }

        yield return new WaitForSeconds(0.5f);

        foreach (GameObject detritut in detritus)
            Destroy(detritut);

        Destroy(gameObject);
    }


    /// <summary>
    /// Method called when the level is finished.
    /// </summary>
    public void BecameInvicible()
    {
        Invicible = true;

        Controller.Instance.SaveController.SaveAchievementProgress("A-11", Mathf.FloorToInt(_timeRunned), true);
        Controller.Instance.SaveController.SaveAchievementProgress("A-10", Mathf.FloorToInt(_distanceRunned) * 10, true);
    }
}