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

    /// <summary>
    /// Parent laser block.
    /// </summary>
    private LaserBlock _laserBlockParent;



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
    /// <param name="parent">The parent laser block used</param>
    public void Initialize(float renderTime, LaserBlock parent)
    {
        _laserBlockParent = parent;

        StartCoroutine(ShootLaser(renderTime));
    }


    /// <summary>
    /// Update method called every frame.
    /// </summary>
    private void Update()
    {
        _lineRenderer.SetPosition(0, transform.parent.position);
        _lineRenderer.SetPosition(1, transform.parent.position + (transform.parent.up * CheckDistance()));
        _hitLight.transform.position = _lineRenderer.GetPosition(1);

        if (!_fake && Physics2D.RaycastAll(transform.parent.position, transform.parent.up, 10)[0].collider.TryGetComponent(out Player player))
            player.GetHit();
    }


    /// <summary>
    /// Coroutine used to cast a laser and checks collision with player.
    /// </summary>
    /// <param name="renderTime">The render time of this laser</param>
    private IEnumerator ShootLaser(float renderTime)
    {
        yield return new WaitForFixedUpdate();

        _lineRenderer.enabled = true;

        if (!_fake)
        {
            _hitLight.enabled = true;

            _hitLight.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, transform.parent.localRotation.eulerAngles.z + 180));
            _particleSystem.GetComponent<ParticleSystem>().Play();

            yield return new WaitForSeconds(renderTime);

            _particleSystem.GetComponent<ParticleSystem>().Stop();
            _hitLight.enabled = false;

            _laserBlockParent.ActiveLaser = false;
        }
        else 
            yield return new WaitForSeconds(renderTime);

        _lineRenderer.enabled = false;

        Controller.Instance.PoolController.RetrieveObject(gameObject);
    }


    /// <summary>
    /// Method used to compute the distance from the impact.
    /// </summary>
    /// <returns>The distance from the object</returns>
    private float CheckDistance()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.parent.position, transform.parent.up, 10);

        return hits[hits.Length - 1].distance * (!_fake ? 1 : 0.5f);
    }
}