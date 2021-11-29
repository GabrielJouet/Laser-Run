using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    [Range(0.05f, 0.5f)]
    private float _renderTime;

    private LineRenderer _lineRenderer;


    public void Initialize(float angle, Vector2 newPosition)
    {
        transform.localRotation = Quaternion.Euler(0, 0, angle);
        transform.position = newPosition;

        _lineRenderer = GetComponent<LineRenderer>();

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.up, 10);

        foreach(RaycastHit2D hit in hits)
        {
            if (hit.collider.TryGetComponent(out Player player))
            {
                _lineRenderer.SetPosition(0, transform.position);
                _lineRenderer.SetPosition(1, player.transform.position);
                player.GetHit();
            }
            else
            {
                _lineRenderer.SetPosition(0, transform.position);
                _lineRenderer.SetPosition(1, transform.position + (transform.up * hit.distance));
            }
        }

        StartCoroutine(FadeOut());
    }


    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(_renderTime);
        Controller.Instance.PoolController.RetrieveObject(gameObject);
    }
}