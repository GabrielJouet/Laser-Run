using UnityEngine;
using UnityEngine.EventSystems;

public class UIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private float _angle;
    private float _altAngle;



    public void Initialize(float angle, float altAngle)
    {
        _angle = angle;
        _altAngle = altAngle;

        ChangeRotation(_angle);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        ChangeRotation(_altAngle);
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        ChangeRotation(_angle);
    }


    private void ChangeRotation(float angle)
    {
        GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, angle);
    }
}