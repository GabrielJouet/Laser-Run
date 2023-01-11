using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Class used to handle every laser block behavior.
/// </summary>
/// <remarks>Needs to have an audio source component attached</remarks>
[RequireComponent(typeof(AudioSource))]
public class LaserBlock : MonoBehaviour
{
    /// <summary>
    /// Laser prefab, used in shooting laser.
    /// </summary>
    [SerializeField]
    protected GameObject _laser;

    /// <summary>
    /// Fake laser, used to show where the laser will strike.
    /// </summary>
    [SerializeField]
    protected GameObject _semiLaser;

    /// <summary>
    /// Clock leds, used to announce next shot.
    /// </summary>
    [SerializeField]
    protected List<GameObject> _clockLeds;

    /// <summary>
    /// Which way the block is facing?
    /// </summary>
    /// 0 => Top
    /// 1 => Left
    /// 2 => Bottom
    /// 3 => Right
    [SerializeField]
    protected int _facing;

    /// <summary>
    /// Canon component.
    /// </summary>
    [SerializeField]
    protected Transform _canon;

    /// <summary>
    /// Particle system used in display.
    /// </summary>
    [SerializeField]
    protected ParticleSystem _particleSystem;

    /// <summary>
    /// Color of the laser shot (and particles).
    /// </summary>
    [SerializeField]
    protected Color _laserColor;

    /// <summary>
    /// Every sound available for this block.
    /// </summary>
    [SerializeField]
    protected List<AudioClip> _laserSounds;


    /// <summary>
    /// Audio source component.
    /// </summary>
    protected AudioSource _audioSource;

    /// <summary>
    /// Light component of this block.
    /// </summary>
    protected Light2D _light;


    /// <summary>
    /// Does the laser is used?
    /// </summary>
    public bool Used { get; protected set; }


    /// <summary>
    /// Difficulty loaded in the laser.
    /// </summary>
    protected LevelDifficulty _difficulty;



    /// <summary>
    /// Awake method, used at first.
    /// </summary>
    protected virtual void Awake()
    {
        _light = _canon.GetComponent<Light2D>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = !Controller.Instance.SaveController.SaveFile.SoundMuted ? Controller.Instance.SaveController.SaveFile.Sound : 0;

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

        _particleSystem.startColor = _laserColor;
        _light.color = _laserColor;
    }


    /// <summary>
    /// Method used to load a new difficulty and start the laser.
    /// </summary>
    /// <param name="difficulty">The difficulty wanted</param>
    public void WarmUp(LevelDifficulty difficulty)
    {
        _difficulty = difficulty;

        Used = true;
        _light.enabled = true;
        _light.intensity = 0.25f;

        StartCoroutine(ChargeUpLaser());
    }


    /// <summary>
    /// Coroutine used to charge up visually the laser block.
    /// </summary>
    protected IEnumerator ChargeUpLaser()
    {
        for (int i = 0; i < _clockLeds.Count; i++)
        {
            yield return new WaitForSeconds(_difficulty.LoadTime / _clockLeds.Count);
            _light.intensity = 0.25f + i * 0.1f;
            _clockLeds[i].SetActive(true);
        }

        StartCoroutine(Shot());
    }


    /// <summary>
    /// Coroutine used to delay next shot.
    /// </summary>
    protected IEnumerator Shot()
    {
        for (int i = 0; i < _difficulty.NumberOfShots; i ++)
        {
            _canon.localRotation = Quaternion.Euler(new Vector3(0, 0, ComputeAngle()));

            Shot(_semiLaser, _difficulty.ReactionTime);
            yield return new WaitForSeconds(_difficulty.ReactionTime);
            Shot(_laser, _difficulty.DisplayTime);
            FindObjectOfType<ShakingCamera>().ShakeCamera(0.01f);

            _audioSource.clip = _laserSounds[Random.Range(0, _laserSounds.Count)];
            _audioSource.Play();
            _particleSystem.Play();

            yield return new WaitForSeconds(_difficulty.DisplayTime);
            _particleSystem.Stop();
        }

        ResetObject();
    }


    /// <summary>
    /// Method used to compute the next shot angle.
    /// </summary>
    /// <returns>The angle found</returns>
    protected float ComputeAngle()
    {
        float angle = (Controller.Instance.SaveController.Hard ? 0 : Random.Range(_difficulty.MinDispersion, _difficulty.MaxDispersion)) * (Random.Range(0, 2) == 1 ? -1 : 1);

        Player player = FindObjectOfType<Player>();

        if (player != null)
        {
            Transform buffer = player.transform;
            Vector3 vectorToTarget = new Vector3(transform.position.x - buffer.position.x, transform.position.y - buffer.position.y, 0);
            angle += Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90;
        }

        return angle;
    }


    /// <summary>
    /// Method called when the block shoots a laser.
    /// </summary>
    /// <param name="laser">What laser will be fired?</param>
    /// <param name="displayTime">How much time the laser will be rendered?</param>
    protected void Shot(GameObject laser, float displayTime)
    {
        GameObject buffer = Controller.Instance.PoolController.Out(laser);
        buffer.GetComponent<Laser>().Initialize(displayTime, _laserColor);
        buffer.transform.SetParent(_canon);
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

        _particleSystem.Stop();
    }
}