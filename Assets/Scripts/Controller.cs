using UnityEngine;

/// <summary>
/// Class that will handle every other controllers.
/// </summary>
[RequireComponent(typeof(PoolController))]
public class Controller : MonoBehaviour
{
    /// <summary>
    /// Instance of itself.
    /// </summary>
    public static Controller Instance { get; private set; } = null;

    /// <summary>
    /// Pool Controller component.
    /// </summary>
    public PoolController PoolController { get; private set; }

    /// <summary>
    /// Choice Controller component.
    /// </summary>
    public ChoiceController ChoiceController { get; private set; }

    /// <summary>
    /// UI Controller component.
    /// </summary>
    public UIController UIController { get; private set; }

    /// <summary>
    /// Level Controller component.
    /// </summary>
    public LevelController LevelController { get; private set; }

    /// <summary>
    /// Level Controller component.
    /// </summary>
    public SaveController SaveController { get; private set; }



    /// <summary>
    /// Awake method called before start.
    /// </summary>
    private void Awake()
    {
        if (FindObjectsOfType<Controller>().Length > 1)
            Destroy(gameObject);
    }


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;
        Instance = this;

        PoolController = GetComponent<PoolController>();
        ChoiceController = GetComponent<ChoiceController>();
        SaveController = GetComponent<SaveController>();
    }


    /// <summary>
    /// Method called when we load a scene, it allows the UI Controller to update.
    /// </summary>
    /// <param name="newController">New level controller component</param>
    public void LoadScene(LevelController newController)
    {
        LevelController = newController;
        UIController = FindObjectOfType<UIController>();
    }
}