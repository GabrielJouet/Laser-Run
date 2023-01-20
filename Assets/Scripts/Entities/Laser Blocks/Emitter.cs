using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Class that will handle emitter behavior.
/// </summary>
/// <remarks>Needs to have an audio source component attached</remarks>
[RequireComponent(typeof(AudioSource))]
public abstract class Emitter : MonoBehaviour
{
    [Header("Laser")]

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


    [Header("Display")]

    /// <summary>
    /// Clock leds, used to announce next shot.
    /// </summary>
    [SerializeField]
    protected List<GameObject> _clockLeds;

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


    [Header("Sound")]

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
    /// All difficulties loaded in this laser.
    /// </summary>
    protected List<LevelDifficulty> _difficulties;

    /// <summary>
    /// Difficulty loaded in the laser.
    /// </summary>
    protected LevelDifficulty _difficulty;


    /// <summary>
    /// Shaking camera component shortcut, used at every emition.
    /// </summary>
    protected ShakingCamera _shakingCamera;


    /// <summary>
    /// Current shot laser.
    /// </summary>
    protected Laser _currentLaser;



    /// <summary>
    /// Method called to initialize an emitter.
    /// </summary>
    /// <param name="difficulties">All level difficulties</param>
    public virtual void Initialize(List<LevelDifficulty> difficulties)
    {
        _shakingCamera = FindObjectOfType<ShakingCamera>();
        _light = _canon.GetComponent<Light2D>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = !Controller.Instance.SaveController.SaveFile.SoundMuted ? Controller.Instance.SaveController.SaveFile.Sound : 0;

        switch (_facing)
        {
            case 0:
                _canon.localPosition = new Vector3(0, 0.09f);
                break;

            case 1:
                _canon.localPosition = new Vector3(-0.09f, 0);
                break;

            case 2:
                _canon.localPosition = new Vector3(0, -0.09f);
                break;

            case 3:
                _canon.localPosition = new Vector3(0.09f, 0);
                break;
        }

        _difficulties = new List<LevelDifficulty>(difficulties);
        ResetObject();
        StopAllCoroutines();

        UpdateDifficulty(0);
    }



    /// <summary>
    /// Method called to initialize an emitter.
    /// </summary>
    /// <param name="difficulty">One level difficulty</param>
    public void Initialize(LevelDifficulty difficulty)
    {
        _difficulties = new List<LevelDifficulty>
        {
            difficulty
        };

        Initialize(_difficulties);
    }


    /// <summary>
    /// Method called to update the difficulty of this laser block.
    /// </summary>
    /// <param name="index">New index loaded</param>
    public void UpdateDifficulty(int index)
    {
        _difficulty = _difficulties[index];
    }


    /// <summary>
    /// Method called to update the difficulty of this laser block.
    /// </summary>
    /// <param name="difficulty">New difficulty loaded</param>
    public void UpdateDifficulty(LevelDifficulty difficulty)
    {
        _difficulty = difficulty;
    }


    /// <summary>
    /// Coroutine used to charge up visually the laser block.
    /// </summary>
    protected IEnumerator ChargeUpLaser()
    {
        _light.enabled = true;
        _light.intensity = 0.25f;

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
    protected virtual IEnumerator Shot()
    {
        for (int i = 0; i < _difficulty.NumberOfShots; i++)
        {
            _canon.localRotation = Quaternion.Euler(new Vector3(0, 0, ComputeAngle()));

            ShotProjectile(_semiLaser, _difficulty.ReactionTime);
            yield return new WaitForSeconds(_difficulty.ReactionTime);

            StartEmitting(_difficulty.DisplayTime, 0.01f);

            yield return new WaitForSeconds(_difficulty.DisplayTime);
            _particleSystem.Stop();
        }

        ResetObject();
    }


    /// <summary>
    /// Method called to start the emition of a laser or projectile.
    /// </summary>
    /// <param name="displayTime">The time the projectile will be displayed</param>
    /// <param name="screenShakeAmount">The amount of screen shake</param>
    protected void StartEmitting(float displayTime, float screenShakeAmount)
    {
        ShotProjectile(_laser, displayTime);
        _shakingCamera.ShakeCamera(screenShakeAmount);

        _audioSource.clip = _laserSounds[Random.Range(0, _laserSounds.Count)];
        _audioSource.Play();
        _particleSystem.Play();
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
    protected void ShotProjectile(GameObject laser, float displayTime)
    {
        _currentLaser = Instantiate(laser).GetComponent<Laser>();

        _currentLaser.transform.SetParent(_canon);
        _currentLaser.Initialize(displayTime, _laserColor);
    }


    /// <summary>
    /// Method called to reset the object back to its original state.
    /// </summary>
    public virtual void ResetObject()
    {
        _light.enabled = false;

        for (int i = 0; i < _clockLeds.Count; i++)
            _clockLeds[i].SetActive(false);

        _particleSystem.Stop();

        if (_currentLaser)
            Destroy(_currentLaser.gameObject);
    }
}