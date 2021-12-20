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
    /// Does you need to render laser?
    /// </summary>
    private bool _renderer = false;



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
    public void Initialize(float renderTime)
    {
        _lineRenderer.enabled = false;
        _renderer = false;

        StartCoroutine(ShootLaser(renderTime));
    }


    /// <summary>
    /// Update method called every frame.
    /// </summary>
    private void Update()
    {
        if (_renderer)
        {
            _lineRenderer.SetPosition(0, transform.parent.position);
            _lineRenderer.SetPosition(1, transform.parent.position + (transform.parent.up * CheckDistance()));
            _hitLight.transform.position = _lineRenderer.GetPosition(1);
        }
    }


    /// <summary>
    /// Coroutine used to cast a laser and checks collision with player.
    /// </summary>
    /// <param name="renderTime">The render time of this laser</param>
    private IEnumerator ShootLaser(float renderTime)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        _renderer = true;
        _lineRenderer.enabled = true;

        if (!_fake)
            StartCoroutine(CheckObjectCollision(renderTime));

        yield return new WaitForSeconds(renderTime);

        Controller.Instance.PoolController.RetrieveObject(gameObject);
    }


    /// <summary>
    /// Coroutine used to check the collision with player.
    /// </summary>
    /// <param name="renderTime">The render time of the laser</param>
    private IEnumerator CheckObjectCollision(float renderTime)
    {
        _hitLight.enabled = true;

        _hitLight.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, transform.parent.localRotation.eulerAngles.z + 180));
        _particleSystem.GetComponent<ParticleSystem>().Play();

        float timeLeft = renderTime;
        while (timeLeft > 0)
        {
            if (Physics2D.RaycastAll(transform.parent.position, transform.parent.up, 10)[0].collider.TryGetComponent(out Player player))
                player.GetHit();

            yield return new WaitForFixedUpdate();
            timeLeft -= Time.fixedDeltaTime;
        }

        _hitLight.enabled = false;
        _particleSystem.GetComponent<ParticleSystem>().Stop();
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