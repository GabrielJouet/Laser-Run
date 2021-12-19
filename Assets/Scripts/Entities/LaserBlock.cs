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

    [SerializeField]
    private ParticleSystem _particleSystem;

    [SerializeField]
    private List<AudioClip> _laserSounds;

    private AudioSource _audioSource;


    /// <summary>
    /// Does the laser is used?
    /// </summary>
    public bool Used;

    /// <summary>
    /// Difficulty loaded in the laser.
    /// </summary>
    private LevelDifficulty _difficulty;

    private Light2D _light;


    /// <summary>
    /// Awake method, used at first.
    /// </summary>
    protected virtual void Awake()
    {
        _light = _canon.GetComponent<Light2D>();
        _audioSource = GetComponent<AudioSource>();

        switch (_facing)
        {
            case 0:
                _canon.position = transform.position + new Vector3(0, 0.09f);
                _particleSystem.transform.position = transform.position + new Vector3(0, 0.09f);
                break;

            case 1:
                _canon.position = transform.position + new Vector3(-0.09f, 0);
                _particleSystem.transform.position = transform.position + new Vector3(-0.09f, 0);
                break;

            case 2:
                _canon.position = transform.position + new Vector3(0, -0.09f);
                _particleSystem.transform.position = transform.position + new Vector3(0, -0.09f);
                break;

            case 3:
                _canon.position = transform.position + new Vector3(0.09f, 0);
                _particleSystem.transform.position = transform.position + new Vector3(0.09f, 0);
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
            yield return new WaitForSeconds(_difficulty.LoadTime / _clockLeds.Count);
            _light.intensity = 0.25f + i * 0.1f;
            _clockLeds[i].SetActive(true);
        }

        for (int i = 0; i < _difficulty.NumberOfShots; i ++)
        {
            float angle = Random.Range(_difficulty.MinDispersion, _difficulty.MaxDispersion) * (Random.Range(0, 2) == 1 ? -1 : 1);

            if (_difficulty.RandomShots)
                angle += _facing * 90;
            else
            {
                Transform buffer = FindObjectOfType<Player>().transform;
                Vector3 vectorToTarget = new Vector3(transform.position.x - buffer.position.x, transform.position.y - buffer.position.y, 0);
                angle += Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90;
            }

            _particleSystem.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            Shot(_semiLaser, angle);
            yield return new WaitForSeconds(_difficulty.ReactionTime);
            Shot(_laser, angle);

            _audioSource.clip = _laserSounds[Random.Range(0, _laserSounds.Count)];
            _audioSource.Play();
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
        GameObject buffer = Controller.Instance.PoolController.GiveObject(laser);
        buffer.GetComponent<Laser>().Initialize(angle, _canon.position, _difficulty.ReactionTime, _canon);
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