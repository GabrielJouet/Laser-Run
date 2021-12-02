using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LaserBlock : MonoBehaviour
{
    /// <summary>
    /// Laser prefab, used in shooting laser.
    /// </summary>
    [SerializeField]
    private GameObject _laser;

    /// <summary>
    /// Fake laser, used to show where the laser will strike.
    /// </summary>
    [SerializeField]
    private GameObject _semiLaser;

    /// <summary>
    /// Clock leds, used to announce next shot.
    /// </summary>
    [SerializeField]
    private List<GameObject> _clockLeds;

    /// <summary>
    /// Which way the block is facing?
    /// </summary>
    /// 0 => Top
    /// 1 => Left
    /// 2 => Bottom
    /// 3 => Right
    [SerializeField]
    private int _facing;

    /// <summary>
    /// Canon component.
    /// </summary>
    [SerializeField]
    private Transform _canon;


    /// <summary>
    /// Does the laser is used?
    /// </summary>
    public bool Used;

    /// <summary>
    /// Difficulty loaded in the laser.
    /// </summary>
    private LevelDifficulty _difficulty;

    private ParticleSystem _particleSystem;

    private Light2D _light;


    /// <summary>
    /// Awake method, used at first.
    /// </summary>
    private void Awake()
    {
        _particleSystem = _canon.GetComponent<ParticleSystem>();
        _light = _canon.GetComponent<Light2D>();

        switch (_facing)
        {
            case 0:
                _canon.position = transform.position + new Vector3(0, 0.09f);
                break;

            case 1:
                _canon.position = transform.position + new Vector3(-0.09f, 0);
                break;

            case 2:
                _canon.position = transform.position + new Vector3(0, -0.09f);
                break;

            case 3:
                _canon.position = transform.position + new Vector3(0.09f, 0);
                break;
        }
    }


    /// <summary>
    /// Method used to load a new difficulty and start the laser.
    /// </summary>
    /// <param name="difficulty">The difficulty wanted</param>
    public void WarmUp(LevelDifficulty difficulty)
    {
        _difficulty = difficulty;

        StartCoroutine(DelayShot());
    }


    /// <summary>
    /// Coroutine used to delay next shot.
    /// </summary>
    private IEnumerator DelayShot()
    {
        Used = true;
        _light.enabled = true;
        _light.intensity = 0.25f;

        for (int i = 0; i < _clockLeds.Count; i++)
        {
            yield return new WaitForSeconds(_difficulty.ShotsTime / _clockLeds.Count);
            _light.intensity = 0.25f + i * 0.1f;
            _clockLeds[i].SetActive(true);
        }

        for (int i = 0; i < _difficulty.NumberOfShots; i ++)
        {
            float angle = Random.Range(-_difficulty.Dispersion, _difficulty.Dispersion);
            _canon.rotation = Quaternion.Euler(new Vector3(0, 0, _facing * 90 + angle));

            if (_difficulty.RandomShots)
                angle += _facing * 90;
            else
            {
                Transform buffer = FindObjectOfType<Player>().transform;
                Vector3 vectorToTarget = new Vector3(transform.position.x - buffer.position.x, transform.position.y - buffer.position.y, 0);
                angle += Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90;
            }

            Shot(_semiLaser, angle);
            yield return new WaitForSeconds(_difficulty.ReactionTime);
            Shot(_laser, angle);
            _particleSystem.Play();

            yield return new WaitForSeconds(_difficulty.ReactionTime);
        }

        for (int i = 0; i < _clockLeds.Count; i++)
            _clockLeds[i].SetActive(false);

        Used = false;
        _light.enabled = false;
    }


    /// <summary>
    /// Method called when the block shoots a laser.
    /// </summary>
    /// <param name="laser">What laser will be fired?</param>
    /// <param name="angle">The new angle for this laser</param>
    private void Shot(GameObject laser, float angle)
    {
        Controller.Instance.PoolController.GiveObject(laser).GetComponent<Laser>().Initialize(angle, _canon.transform.position, _difficulty.ReactionTime);
    }


    /// <summary>
    /// Method called to reset the object back to its original state.
    /// </summary>
    public void ResetObject()
    {
        StopAllCoroutines();
        Used = false;
        _light.enabled = false;

        for (int i = 0; i < _clockLeds.Count; i++)
            _clockLeds[i].SetActive(false);
    }
}