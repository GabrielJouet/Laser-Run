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
    [SerializeField]
    [Range(0.5f, 10f)]
    private float _activationTime;
    public float ActivationTime { get => _activationTime; }
    
    /// <summary>
    /// How many traps activated at once.
    /// </summary>
    [SerializeField]
    [Range(1, 5)]
    private int _activationNumber;
    public int ActivationCount { get => _activationNumber; }

    /// <summary>
    /// Warm up time for block.
    /// </summary>
    [SerializeField]
    [Range(0.1f, 5f)]
    private float _loadTime;
    public float LoadTime { get => _loadTime; }


    [Header("Laser")]

    /// <summary>
    /// Time before and after each laser is fired.
    /// </summary>
    [SerializeField]
    [Range(0.05f, 0.75f)]
    private float _reactionTime;
    public float ReactionTime { get => _reactionTime; }

    /// <summary>
    /// Amount of time the laser goes.
    /// </summary>
    [SerializeField]
    [Range(0.05f, 3f)]
    private float _displayTime;
    public float DisplayTime { get => _displayTime; }


    [Header("Acurracy")]

    /// <summary>
    /// Minimum dispersion of laser.
    /// </summary>
    [SerializeField]
    [Range(0f, 90f)]
    private float _dispersionMin;
    public float MinDispersion { get => _dispersionMin; }

    /// <summary>
    /// Maximum dispersion of laser.
    /// </summary>
    [SerializeField]
    [Range(0f, 90f)]
    private float _dispersionMax;
    public float MaxDispersion { get => _dispersionMax; }


    [Header("Difficulty")]

    /// <summary>
    /// How many shots per activation.
    /// </summary>
    [SerializeField]
    [Range(1, 10)]
    private int _shots;
    public int NumberOfShots { get => _shots; }

    /// <summary>
    /// Does the shots are random or homes at you?
    /// </summary>
    [SerializeField]
    private bool _randomShots;
    public bool RandomShots { get => _randomShots; }


    [Header("Display")]

    /// <summary>
    /// Did we need to show a warning for players?
    /// </summary>
    [SerializeField]
    private bool _warning;
    public bool Warning { get => _warning; }

    /// <summary>
    /// What threat comes next?
    /// </summary>
    [SerializeField]
    private string _threatDescription;
    public string ThreatDescription { get => _threatDescription; }
}