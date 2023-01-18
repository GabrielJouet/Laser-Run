using System;
using UnityEngine;

/// <summary>
/// Class used to handle the level difficulty.
/// </summary>
[Serializable]
public class LevelDifficulty
{
    [Header("Activation")]
    /// <summary>
    /// Time between each laser block activation.
    /// </summary>
    [SerializeField, Range(0.5f, 10f), Tooltip("Time between each block activation")]
    private float _activationTime;

    /// <summary>
    /// Time between each laser block activation.
    /// </summary>
    public float ActivationTime { get => _activationTime; set => _activationTime = value; }


    /// <summary>
    /// How many traps activated at once.
    /// </summary>
    [SerializeField, Range(1, 5), Tooltip("How many traps activated in one activation")]
    private int _activationNumber;

    /// <summary>
    /// How many traps activated at once.
    /// </summary>
    public int ActivationCount { get => _activationNumber; set => _activationNumber = value; }


    [Header("Laser block")]

    /// <summary>
    /// Warm up time for block.
    /// </summary>
    [SerializeField, Range(0.5f, 5), Tooltip("Block loading time")]
    protected float _loadTime;

    /// <summary>
    /// Warm up time for block.
    /// </summary>
    public float LoadTime { get => _loadTime; set => _loadTime = value; }


    /// <summary>
    /// How many shots per activation.
    /// </summary>
    [SerializeField, Range(1, 10), Tooltip("Numbers of shots")]
    protected int _shots;

    /// <summary>
    /// How many shots per activation.
    /// </summary>
    public int NumberOfShots { get => _shots; set => _shots = value; }



    [Header("Laser")]

    /// <summary>
    /// Time before and after each laser is fired.
    /// </summary>
    [SerializeField, Range(0.05f, 0.75f), Tooltip("Time between showing and firing")]
    private float _reactionTime;

    /// <summary>
    /// Time before and after each laser is fired.
    /// </summary>
    public float ReactionTime { get => _reactionTime; set => _reactionTime = value; }


    /// <summary>
    /// Amount of time the laser goes.
    /// </summary>
    [SerializeField, Range(0.05f, 3f), Tooltip("Laser display time")]
    private float _displayTime;

    /// <summary>
    /// Amount of time the laser goes.
    /// </summary>
    public float DisplayTime { get => _displayTime; set => _displayTime = value; }


    [Header("Acurracy")]

    /// <summary>
    /// Minimum dispersion of laser.
    /// </summary>
    [SerializeField, Range(0f, 45f), Tooltip("Minimum dispersion angle")]
    private float _dispersionMin;

    /// <summary>
    /// Minimum dispersion of laser.
    /// </summary>
    public float MinDispersion { get => _dispersionMin; set => _dispersionMin = value; }


    /// <summary>
    /// Maximum dispersion of laser.
    /// </summary>
    [SerializeField, Range(0f, 45f), Tooltip("Maximum dispersion angle")]
    private float _dispersionMax;

    /// <summary>
    /// Maximum dispersion of laser.
    /// </summary>
    public float MaxDispersion { get => _dispersionMax; set => _dispersionMax = value; }


    [Header("Moving related")]

    /// <summary>
    /// Speed of the block.
    /// </summary>
    [SerializeField, Range(0.01f, 0.35f), Tooltip("Movement speed of moving laser blocks")]
    private float _speed;

    /// <summary>
    /// Speed of the block.
    /// </summary>
    public float Speed { get => _speed; set => _speed = value; }


    /// <summary>
    /// How much time before direction change?
    /// </summary>
    [SerializeField, Range(0f, 0.5f), Tooltip("How much time before direction change of moving laser blocks")]
    private float _timeBeforeDirectionChange;

    /// <summary>
    /// How much time before direction change?
    /// </summary>
    public float TimeBeforeDirectionChange { get => _timeBeforeDirectionChange; set => _timeBeforeDirectionChange = value; }


    [Header("Forever related")]

    /// <summary>
    /// Rotation angle speed of this laser.
    /// </summary>
    [SerializeField, Range(2f, 15f)]
    private float _rotationSpeed;

    /// <summary>
    /// Rotation angle speed of this laser.
    /// </summary>
    public float RotationSpeed { get => _rotationSpeed; set => _rotationSpeed = value; }


    /// <summary>
    /// Min angle reached in rotation.
    /// </summary>
    [SerializeField]
    private float _minusShiftAngle;

    /// <summary>
    /// Min angle reached in rotation.
    /// </summary>
    public float MinusAngle { get => _minusShiftAngle; set => _minusShiftAngle = value; }


    /// <summary>
    /// Max angle reached in rotation.
    /// </summary>
    [SerializeField]
    private float _positiveShiftAngle;

    /// <summary>
    /// Max angle reached in rotation.
    /// </summary>
    public float PositiveAngle { get => _positiveShiftAngle; set => _positiveShiftAngle = value; }


    /// <summary>
    /// How much time before rotation change?
    /// </summary>
    [SerializeField, Range(0f, 5f), Tooltip("How much time before rotation change of rotating laser blocks")]
    private float _timeBeforeRotationChange;

    /// <summary>
    /// How much time before rotation change?
    /// </summary>
    public float TimeBeforeRotationChange { get => _timeBeforeRotationChange; set => _timeBeforeRotationChange = value; }


    /// <summary>
    /// Does the emitter stops when not moving?
    /// </summary>
    [SerializeField, Tooltip("Does the emitter stops when not moving?")]
    private bool _stopWhenNotMoving;

    /// <summary>
    /// Does the emitter stops when not moving?
    /// </summary>
    public bool StopWhenNotMoving { get => _stopWhenNotMoving; set => _stopWhenNotMoving = value; }


    [Header("Rotater related")]

    /// <summary>
    /// Rotation speed of the laser block.
    /// </summary>
    [SerializeField]
    private float _blockRotationSpeed;

    /// <summary>
    /// Rotation speed of the laser block.
    /// </summary>
    public float BlockRotationSpeed { get => _blockRotationSpeed; set => _blockRotationSpeed = value; }


    [Header("Display")]

    /// <summary>
    /// Did we need to show a warning for players?
    /// </summary>
    [SerializeField, Tooltip("Show a warning before level starts?")]
    private bool _warning;

    /// <summary>
    /// Did we need to show a warning for players?
    /// </summary>
    public bool Warning { get => _warning; }


    /// <summary>
    /// What threat comes next?
    /// </summary>
    [SerializeField, Tooltip("Description of level")]
    private string _threatDescription;

    /// <summary>
    /// What threat comes next?
    /// </summary>
    public string ThreatDescription { get => _threatDescription; }



    /// <summary>
    /// Constructor of this class.
    /// </summary>
    public LevelDifficulty()
    {
        _activationTime = 1.5f;
        _activationNumber = 1;

        _loadTime = 1;
        _shots = 1;

        _reactionTime = 0.65f;
        _displayTime = 0.85f;

        _dispersionMin = 0;
        _dispersionMax = 15;

        _speed = 0.25f;
        _timeBeforeDirectionChange = 0.25f;

        _rotationSpeed = 10;
        _minusShiftAngle = -80;
        _positiveShiftAngle = -45;
        _timeBeforeRotationChange = 0.75f;
        _stopWhenNotMoving = false;

        _blockRotationSpeed = 0.25f;
    }
}