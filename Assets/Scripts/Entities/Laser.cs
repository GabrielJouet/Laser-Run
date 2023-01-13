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
        _lineRenderer.SetPosition(0, transform.parent.position);
        _lineRenderer.SetPosition(1, transform.parent.position + (transform.parent.up * CheckDistance()));

        if (!_fake && _lineRenderer.enabled)
        {
            _hitLight.transform.position = _lineRenderer.GetPosition(1);

            if (Physics2D.RaycastAll(transform.parent.position, transform.parent.up, 10)[0].collider.TryGetComponent(out Player player) && !player.Invicible)
                player.GetHit();
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

            _hitLight.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, transform.parent.localRotation.eulerAngles.z + 180));
            _particleSystem.Play();

            yield return new WaitForSeconds(renderTime);

            _particleSystem.Stop();
        }
        else 
            yield return new WaitForSeconds(renderTime);

        _lineRenderer.SetPosition(0, Vector3.zero);
        _lineRenderer.SetPosition(1, Vector3.zero);
        _hitLight.transform.position = Vector3.one * 10;

        Controller.Instance.PoolController.In(gameObject);
    }


    /// <summary>
    /// Method used to compute the distance from the impact.
    /// </summary>
    /// <returns>The distance from the object</returns>
    private float CheckDistance()
    {
        return Physics2D.RaycastAll(transform.parent.position, transform.parent.up, 10)[^1].distance * (!_fake ? 1 : 0.5f);
    }
}