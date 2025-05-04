using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SalirPartida : MonoBehaviour
{
    public GameObject _child;
    private Slider _slider;
    private float _timeToExit = 3f;
    private float _time = 0f;

    private void Start()
    {
        _slider = GetComponentInChildren<Slider>();
    }

    void Update()
    {
        _child.gameObject.SetActive(false);

        if (Input.GetKeyUp(KeyCode.Escape)) { _time = 0f; }
        if (Input.GetKey(KeyCode.Escape))
        {
            _child.gameObject.SetActive(true);
            if (_time >= _timeToExit)
                SceneManager.LoadScene(0); 
            _time += Time.deltaTime;
            _slider.value = _time / _timeToExit;
        }
    }
}
