using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    /// <summary>
    /// Movement speed of the projectile.
    /// </summary>
    [SerializeField]
    [Range(0.1f, 4f)]
    protected float _movementSpeed;

    /// <summary>
    /// How much time (in seconds) does the projectile will move?
    /// </summary>
    [SerializeField]
    [Range(0.15f, 1f)]
    protected float _liveTime;


    /// <summary>
    /// Pool controller component.
    /// </summary>
    protected PoolController _pool;


    /// <summary>
    /// Pool controller component.
    /// </summary>
    protected Rigidbody2D _rigidBody;



    /// <summary>
    /// Method used at initialization when instantiated.
    /// </summary>
    /// <param name="angle">The angle of the projectile</param>
    /// <param name="newPosition">New Position of the projectile </param>
    public void ChangeDefaultParameters(float angle, Vector2 newPosition)
    {
        transform.position = newPosition;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        _pool = Controller.Instance.PoolController;
        _rigidBody = GetComponent<Rigidbody2D>();

        StartCoroutine(Initialize());
    }


    /// <summary>
    /// Coroutine used at start to desactivate Projectile once it reached max time.
    /// </summary>
    protected IEnumerator Initialize()
    {
        yield return new WaitForSeconds(_liveTime);
        _pool.RetrieveObject(gameObject);
    }


    /// <summary>
    /// Update method is called each frame.
    /// </summary>
    protected void Update()
    {
        _rigidBody.MovePosition(_rigidBody.position + (Vector2)transform.up * Time.deltaTime * _movementSpeed);
    }


    /// <summary>
    /// Method called when a projectile hits another object.
    /// </summary>
    /// <param name="collision">The other object</param>
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("Button") && collision.transform.parent.TryGetComponent(out LaserBlock laserBlock))
        {
            laserBlock.ActivateEarly();
            _pool.RetrieveObject(gameObject);
        }
    }
}