using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Class that handle rotation and movement UI element.
/// </summary>
public class UIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// Base angle.
    /// </summary>
    private float _angle;

    /// <summary>
    /// Alternative angle.
    /// </summary>
    private float _altAngle;



    /// <summary>
    /// Method called at initialization.
    /// </summary>
    /// <param name="angle">The angle randomly chosen</param>
    public void Initialize(float angle)
    {
        _angle = angle;
        _altAngle = -angle;

        ChangeRotation(_angle);
    }


    /// <summary>
    /// Method called when the mouse hovers the object.
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        ChangeRotation(_altAngle);
    }


    /// <summary>
    /// Method called when the mouse exits the object.
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        ChangeRotation(_angle);
    }


    /// <summary>
    /// Method called when we need to change the rotation object.
    /// </summary>
    private void ChangeRotation(float angle)
    {
        GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, angle);
    }
}