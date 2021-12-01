using System;
using UnityEngine;

/// <summary>
/// Class used to handle the level difficulty.
/// </summary>
[Serializable]
public class LevelDifficulty
{
    /// <summary>
    /// Time between each laser block activation.
    /// </summary>
    [SerializeField]
    [Range(0.5f, 10f)]
    private float _timeBetweenEachActivation;
    public float ActivationTime { get => _timeBetweenEachActivation; }

    /// <summary>
    /// Warm up time for block.
    /// </summary>
    [SerializeField]
    [Range(0.1f, 5f)]
    private float _timeBetweenShots;
    public float ShotsTime { get => _timeBetweenShots; }

    /// <summary>
    /// Time before and after each laser is fired.
    /// </summary>
    [SerializeField]
    [Range(0.05f, 0.75f)]
    private float _reactionTime;
    public float ReactionTime { get => _reactionTime; }

    /// <summary>
    /// Maximum dispersion of laser.
    /// </summary>
    [SerializeField]
    [Range(0f, 90f)]
    private float _dispersion;
    public float Dispersion { get => _dispersion; }

    /// <summary>
    /// How many shots per activation.
    /// </summary>
    [SerializeField]
    [Range(1, 10)]
    private int _numberOfShots;
    public int NumberOfShots { get => _numberOfShots; }

    /// <summary>
    /// Does the shots are random or homes at you?
    /// </summary>
    [SerializeField]
    private bool _randomShots;
    public bool RandomShots { get => _randomShots; }
}