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
    /// Awake method called before start.
    /// </summary>
    private void Awake()
    {
        if (FindObjectsOfType<Controller>().Length > 1)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;
        Instance = this;

        PoolController = GetComponent<PoolController>();
        ChoiceController = GetComponent<ChoiceController>();
    }


    public void LoadScene()
    {
        UIController = FindObjectOfType<UIController>();
    }
}