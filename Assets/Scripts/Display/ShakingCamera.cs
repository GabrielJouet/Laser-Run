using System.Collections;
using UnityEngine;

/// <summary>
/// Class used to display a camera shake.
/// </summary>
public class ShakingCamera : MonoBehaviour
{
    /// <summary>
    /// The camera that will be shaken.
    /// </summary>
    private Camera _camera;


    /// <summary>
    /// Does the camera is already shaking?
    /// </summary>
    private bool _isShaking = false;

    /// <summary>
    /// Initial position of this camera.
    /// </summary>
    private Vector3 _initialPosition;



    /// <summary>
    /// Awake method, called at initialization before Start.
    /// </summary>
    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _initialPosition = transform.position;
    }


    /// <summary>
    /// Method called to shake the camera.
    /// </summary>
    /// <param name="amount">The amount shaked</param>
    public void ShakeCamera(float amount)
    {
        if (!_isShaking)
            StartCoroutine(DelayShake(amount));
    }


    /// <summary>
    /// Coroutine used to delay the camera shakes.
    /// </summary>
    /// <param name="amount">How much the camera will be shaken</param>
    private IEnumerator DelayShake(float amount)
    {
        _isShaking = true;
        Vector3 initialPosition = _camera.transform.position;

        for (int i = 0; i < Random.Range(4, 8); i ++)
        {
            _camera.transform.position = new Vector3(initialPosition.x + Random.Range(-amount, amount), initialPosition.y + Random.Range(-amount, amount), initialPosition.z);

            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
        }

        _camera.transform.position = _initialPosition;
        _isShaking = false;
    }
}