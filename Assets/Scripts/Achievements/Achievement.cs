using UnityEngine;

/// <summary>
/// Class used to handle achievement behavior.
/// </summary>
[CreateAssetMenu(menuName = "Achievements")]
public class Achievement : ScriptableObject
{
    /// <summary>
    /// The name of this achievement.
    /// </summary>
    [SerializeField]
    private string _name;

    /// <summary>
    /// The name of this achievement.
    /// </summary>
    public string Name { get => _name; }


    /// <summary>
    /// The unique id of this achievement.
    /// </summary>
    [SerializeField]
    private string _id;

    /// <summary>
    /// The unique id of this achievement.
    /// </summary>
    public string ID { get => _id; }



    /// <summary>
    /// The description of this achievement.
    /// </summary>
    [SerializeField]
    private string _description;

    /// <summary>
    /// The description of this achievement.
    /// </summary>
    public string Description { get => _description; }


    /// <summary>
    /// Icon used to display this achievement.
    /// </summary>
    [SerializeField]
    private Sprite _icon;

    /// <summary>
    /// Icon used to display this achievement.
    /// </summary>
    public Sprite Icon { get => _icon; }


    /// <summary>
    /// Icon used when this achievement is not yet unlocked.
    /// </summary>
    [SerializeField]
    private Sprite _lockedIcon;

    /// <summary>
    /// Icon used when this achievement is not yet unlocked.
    /// </summary>
    public Sprite LockedIcon { get => _lockedIcon; }


    /// <summary>
    /// Does this achievement is hintted when not found?
    /// </summary>
    [SerializeField]
    private bool _showHints;

    /// <summary>
    /// Does this achievement is hintted when not found?
    /// </summary>
    public bool Hintable { get => _showHints; }
}