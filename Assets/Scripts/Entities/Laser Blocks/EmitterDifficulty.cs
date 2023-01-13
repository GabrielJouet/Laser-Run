using UnityEngine;

/// <summary>
/// Class used to handle block based difficulty.
/// </summary>
[CreateAssetMenu(menuName = "Difficulty / Classic")]
public class EmitterDifficulty : ScriptableObject
{
    /// <summary>
    /// Warm up time for block.
    /// </summary>
    [SerializeField, Range(0.5f, 5), Tooltip("Block loading time")]
    protected float _loadTime;

    /// <summary>
    /// Warm up time for block.
    /// </summary>
    public float LoadTime { get => _loadTime; }


    /// <summary>
    /// How many shots per activation.
    /// </summary>
    [SerializeField, Range(1, 10), Tooltip("Numbers of shots")]
    protected int _shots;

    /// <summary>
    /// How many shots per activation.
    /// </summary>
    public int NumberOfShots { get => _shots; }



    [Header("Laser")]

    /// <summary>
    /// Time before and after each laser is fired.
    /// </summary>
    [SerializeField, Range(0.05f, 0.75f), Tooltip("Time between showing and firing")]
    private float _reactionTime;

    /// <summary>
    /// Time before and after each laser is fired.
    /// </summary>
    public float ReactionTime { get => _reactionTime; }


    /// <summary>
    /// Amount of time the laser goes.
    /// </summary>
    [SerializeField, Range(0.05f, 3f), Tooltip("Laser display time")]
    private float _displayTime;

    /// <summary>
    /// Amount of time the laser goes.
    /// </summary>
    public float DisplayTime { get => _displayTime; }


    [Header("Acurracy")]

    /// <summary>
    /// Minimum dispersion of laser.
    /// </summary>
    [SerializeField, Range(0f, 45f), Tooltip("Minimum dispersion angle")]
    private float _dispersionMin;

    /// <summary>
    /// Minimum dispersion of laser.
    /// </summary>
    public float MinDispersion { get => _dispersionMin; }


    /// <summary>
    /// Maximum dispersion of laser.
    /// </summary>
    [SerializeField, Range(0f, 45f), Tooltip("Maximum dispersion angle")]
    private float _dispersionMax;

    /// <summary>
    /// Maximum dispersion of laser.
    /// </summary>
    public float MaxDispersion { get => _dispersionMax; }
}