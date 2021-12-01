using System.Collections;
using UnityEngine;

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
    /// <param name="angle">The new laser angle</param>
    /// <param name="newPosition">The new position of this laser</param>
    /// <param name="renderTime">The render time of this laser</param>
    public void Initialize(float angle, Vector2 newPosition, float renderTime)
    {
        transform.localRotation = Quaternion.Euler(0, 0, angle);
        transform.position = newPosition;

        StartCoroutine(StartCasting(renderTime));
        StartCoroutine(FadeOut(renderTime));
    }


    /// <summary>
    /// Coroutine used to cast a laser and checks collision with player.
    /// </summary>
    /// <param name="renderTime">The render time of this laser</param>
    private IEnumerator StartCasting(float renderTime)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.up, 10);

        foreach (RaycastHit2D hit in hits)
        {
            if (!_fake && hit.collider.TryGetComponent(out Player player))
            {
                _lineRenderer.SetPosition(0, transform.position);
                _lineRenderer.SetPosition(1, player.transform.position);
                player.GetHit();
            }
            else
            {
                _lineRenderer.SetPosition(0, transform.position);
                _lineRenderer.SetPosition(1, transform.position + (transform.up * (!_fake ? hit.distance : hit.distance / 2f)));
            }
        }

        for (int i = 0; i < 5; i ++)
        {
            hits = Physics2D.RaycastAll(transform.position, transform.up, 10);

            foreach (RaycastHit2D hit in hits)
                if (!_fake && hit.collider.TryGetComponent(out Player player))
                    player.GetHit();

            yield return new WaitForSeconds(renderTime / 5);
        }
    }


    /// <summary>
    /// Coroutine method used to fade the laser out.
    /// </summary>
    /// <param name="renderTime">The render time max of this laser</param>
    private IEnumerator FadeOut(float renderTime)
    {
        yield return new WaitForSeconds(renderTime);
        Controller.Instance.PoolController.RetrieveObject(gameObject);
    }
}