using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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
    private ParticleSystem _particleSystem;

    /// <summary>
    /// Light component used when the laser hits the wall.
    /// </summary>
    [SerializeField]
    private Light2D _hitLight;


    /// <summary>
    /// Line renderer component of the laser.
    /// </summary>
    private LineRenderer _lineRenderer;


    /// <summary>
    /// Nearest hit stored.
    /// </summary>
    private RaycastHit2D _nearestHit;



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
    /// <param name="renderTime">The render time of this laser</param>
    /// <param name="newColor">The new color of this laser</param>
    public void Initialize(float renderTime, Color newColor)
    {
        if (!_fake)
        {
            _lineRenderer.startColor = newColor;
            _lineRenderer.endColor = newColor;

            _particleSystem.startColor = newColor;
            _hitLight.color = newColor;
        }

        StartCoroutine(ShootLaser(renderTime));
    }


    /// <summary>
    /// Update method called every frame.
    /// </summary>
    private void Update()
    {
        if (_lineRenderer.enabled)
        {
            _nearestHit = NearestHit();

            _lineRenderer.SetPosition(0, transform.parent.position);
            _lineRenderer.SetPosition(1, transform.parent.position + (transform.parent.up * _nearestHit.distance * (!_fake ? 1 : 0.5f)));

            if (!_fake)
            {
                _hitLight.transform.position = _lineRenderer.GetPosition(1);

                if (_nearestHit.collider.TryGetComponent(out Player player) && !player.Invicible)
                    player.GetHit();
            }
        }
    }


    /// <summary>
    /// Coroutine used to cast a laser and checks collision with player.
    /// </summary>
    /// <param name="renderTime">The render time of this laser</param>
    private IEnumerator ShootLaser(float renderTime)
    {
        _lineRenderer.enabled = true;

        if (!_fake)
        {
            _hitLight.enabled = true;

            _hitLight.transform.localEulerAngles = Vector3.forward * (transform.parent.localEulerAngles.z + 180);
            _particleSystem.Play();
        }

        yield return new WaitForSeconds(renderTime);
        StopLaser();
    }


    /// <summary>
    /// Method called to find the nearest hittable object.
    /// </summary>
    /// <returns>The nearest hittable object</returns>
    private RaycastHit2D NearestHit()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.parent.position, transform.parent.up, 10);
        float nearestDistance = Mathf.Infinity;
        RaycastHit2D nearestHit = new RaycastHit2D();

        foreach(RaycastHit2D hit in hits)
        {
            if (hit.distance < nearestDistance)
            {
                nearestDistance = hit.distance;
                nearestHit = hit;
            }
        }

        return nearestHit;
    }


    /// <summary>
    /// Method called when we need to stop the laser earlier.
    /// </summary>
    public void StopLaser()
    {
        Destroy(gameObject);
    }
}