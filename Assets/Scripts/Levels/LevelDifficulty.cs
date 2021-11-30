using System;
using UnityEngine;

[Serializable]
public class LevelDifficulty
{
    [SerializeField]
    [Range(0.5f, 10f)]
    private float _timeBetweenEachActivation;
    public float ActivationTime { get => _timeBetweenEachActivation; }

    [SerializeField]
    [Range(0.1f, 5f)]
    private float _timeBetweenShots;
    public float ShotsTime { get => _timeBetweenShots; }

    [SerializeField]
    [Range(0.05f, 0.75f)]
    private float _reactionTime;
    public float ReactionTime { get => _reactionTime; }

    [SerializeField]
    [Range(0f, 90f)]
    private float _dispersion;
    public float Dispersion { get => _dispersion; }

    [SerializeField]
    [Range(1, 10)]
    private int _numberOfShots;
    public int NumberOfShots { get => _numberOfShots; }

    [SerializeField]
    private bool _randomShots;
    public bool RandomShots { get => _randomShots; }


    public LevelDifficulty(LevelDifficulty clone)
    {
        _timeBetweenEachActivation = clone.ActivationTime;
        _timeBetweenShots = clone.ShotsTime;
        _reactionTime = clone.ReactionTime;
        _dispersion = clone.Dispersion;
        _numberOfShots = clone.NumberOfShots;
    }
}