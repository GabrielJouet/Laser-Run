using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBlock : MonoBehaviour
{
    [SerializeField]
    private GameObject _laser;

    [SerializeField]
    private GameObject _semiLaser;

    [SerializeField]
    private List<GameObject> _clockLeds;

    [SerializeField]
    private GameObject _button;

    [SerializeField]
    private int _facing;

    [SerializeField]
    private List<Transform> _canonPositions;

    private float _timeBetweenShots;
    private float _reactionTime;

    private float _dispersion;


    public void Start()
    {
        _button.transform.localPosition = _canonPositions[_facing].localPosition;
        _button.transform.localRotation = Quaternion.Euler(0, 0, _facing * 90);
    }


    public void WarmUp(float timeBetweenShots, float dispersion, float reactionTime)
    {
        _timeBetweenShots = timeBetweenShots;
        _reactionTime = reactionTime;
        _dispersion = dispersion;

        StartCoroutine(DelayShot(false));
    }


    public void ActivateEarly()
    {
        StopAllCoroutines();
        StartCoroutine(DelayShot(true));
    }


    private IEnumerator DelayShot(bool skipLoad)
    {
        if (!skipLoad)
        {
            ActivateButton();

            for (int i = 0; i < _clockLeds.Count; i++)
            {
                yield return new WaitForSeconds(_timeBetweenShots / _clockLeds.Count);
                _clockLeds[i].SetActive(true);
            }
        }

        float angle = Random.Range(-_dispersion, _dispersion);

        DesactivateButton();
        Shot(_semiLaser, angle);
        yield return new WaitForSeconds(_reactionTime);
        Shot(_laser, angle);
    }


    private void ActivateButton()
    {
        _button.SetActive(true);
    }


    private void DesactivateButton()
    {
        _button.SetActive(false);

        for (int i = 0; i < _clockLeds.Count; i++)
            _clockLeds[i].SetActive(false);
    }


    private void Shot(GameObject laser, float angle)
    {
        Controller.Instance.PoolController.GiveObject(laser).GetComponent<Laser>().Initialize(_facing * 90 + angle, _canonPositions[_facing].position);
    }
}