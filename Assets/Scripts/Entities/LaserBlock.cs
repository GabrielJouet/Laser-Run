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
    private int _facing;

    [SerializeField]
    private List<Transform> _canonPositions;

    private float _timeBetweenShots;
    private float _reactionTime;

    private float _dispersion;

    private int _numberOfShots;

    private bool _randomShots;



    public void WarmUp(float timeBetweenShots, float dispersion, float reactionTime, int numberOfShots, bool randomShots)
    {
        _numberOfShots = numberOfShots;
        _timeBetweenShots = timeBetweenShots;
        _reactionTime = reactionTime;
        _dispersion = dispersion;
        _randomShots = randomShots;

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
            for (int i = 0; i < _clockLeds.Count; i++)
            {
                yield return new WaitForSeconds(_timeBetweenShots / _clockLeds.Count);
                _clockLeds[i].SetActive(true);
            }
        }

        for (int i = 0; i < _clockLeds.Count; i++)
            _clockLeds[i].SetActive(false);

        for (int i = 0; i < _numberOfShots; i ++)
        {
            float angle = Random.Range(-_dispersion, _dispersion);

            if (_randomShots)
                angle += _facing * 90;
            else
            {
                Transform buffer = FindObjectOfType<Player>().transform;
                Vector3 vectorToTarget = new Vector3(transform.position.x - buffer.position.x, transform.position.y - buffer.position.y, 0);
                angle += Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90;
            }

            Shot(_semiLaser, angle);
            yield return new WaitForSeconds(_reactionTime);
            Shot(_laser, angle);
            yield return new WaitForSeconds(_reactionTime);
        }
    }


    private void Shot(GameObject laser, float angle)
    {
        Controller.Instance.PoolController.GiveObject(laser).GetComponent<Laser>().Initialize(angle, _canonPositions[_facing].position);
    }
}