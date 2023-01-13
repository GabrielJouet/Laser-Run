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
    public float ActivationTime { get => _activationTime; }
    

    /// <summary>
    /// How many traps activated at once.
    /// </summary>
    [SerializeField, Range(1, 5), Tooltip("How many traps activated in one activation")]
    private int _activationNumber;

    /// <summary>
    /// How many traps activated at once.
    /// </summary>
    public int ActivationCount { get => _activationNumber; }


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
}