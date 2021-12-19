using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

/// <summary>
/// Class used to emulates a laser behavior.
/// </summary>
/// <remarks>Needs a line renderer component</remarks>
[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    /// <summary>
    /// Does the laser is a fake one?
    /// </summary>
    [SerializeField]
    private bool _fake;

    /// <summary>
    /// Particle component used when the laser hits the wall.
    /// </summary>
    [SerializeField]
    private Transform _particleSystem;

    /// <summary>
    /// Light component used when the laser hits the wall.
    /// </summary>
    [SerializeField]
    private Light2D _hitLight;


    /// <summary>
    /// Line renderer component of the laser.
    /// </summary>
    private LineRenderer _lineRenderer;

    private Transform _parentLaserBlock;



    /// <summary>
    /// Awake method called at first.
    /// </summary>
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }


    /// <summary>
    /// Initialize method, called when restarting the entity.
    /// </summary>
    /// <param name="angle">The new laser angle</param>
    /// <param name="newPosition">The new position of this laser</param>
    /// <param name="renderTime">The render time of this laser</param>
    public void Initialize(float angle, Vector2 newPosition, float renderTime, Transform parentLaserBlock)
    {
        _parentLaserBlock = parentLaserBlock;
        _lineRenderer.enabled = false;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
        transform.position = newPosition;

        StartCoroutine(ShootLaser(renderTime));
    }


    private void Update()
    {
        _lineRenderer.SetPosition(0, _parentLaserBlock.position);
        _lineRenderer.SetPosition(1, _parentLaserBlock.position + (transform.up * CheckDistance()));
    }


    /// <summary>
    /// Coroutine used to cast a laser and checks collision with player.
    /// </summary>
    /// <param name="renderTime">The render time of this laser</param>
    private IEnumerator ShootLaser(float renderTime)
    {
        yield return new WaitForEndOfFrame();

        _lineRenderer.enabled = true;

        if (!_fake)
            StartCoroutine(CheckObjectCollision(renderTime));

        yield return new WaitForSeconds(renderTime);

        Controller.Instance.PoolController.RetrieveObject(gameObject);
    }


    private IEnumerator CheckObjectCollision(float renderTime)
    {
        _hitLight.transform.position = _parentLaserBlock.position + (transform.up * CheckDistance());
        _hitLight.enabled = true;

        _particleSystem.localRotation = Quaternion.Euler(new Vector3(0, 0, transform.localRotation.z + 180));
        _particleSystem.GetComponent<ParticleSystem>().Play();

        for (int i = 0; i < 5; i++)
        {
            if (Physics2D.RaycastAll(_parentLaserBlock.position, transform.up, 10)[0].collider.TryGetComponent(out Player player))
                player.GetHit();

            yield return new WaitForSeconds(renderTime / 5);
        }

        _hitLight.enabled = false;
    }


    private float CheckDistance()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(_parentLaserBlock.position, transform.up, 10);

        return hits[hits.Length - 1].distance * (!_fake ? 1 : 0.5f);
    }
}