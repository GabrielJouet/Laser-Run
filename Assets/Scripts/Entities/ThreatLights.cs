using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ThreatLights : MonoBehaviour
{
    [SerializeField]
    private float _speed;


    private Light2D _light;



    private void Awake()
    {
        _light = GetComponent<Light2D>();
    }


    private void Update()
    {
        transform.localRotation = Quaternion.Euler(0, 0, transform.localRotation.eulerAngles.z + _speed * Time.deltaTime);
    }


    public void LightUp()
    {
        enabled = true;
        _light.enabled = true;
    }


    public void CutOff()
    {
        enabled = false;
        _light.enabled = false;
    }
}