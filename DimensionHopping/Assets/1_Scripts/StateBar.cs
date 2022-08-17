using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Wechselt UI des Perspektivwechsels
public class StateBar : MonoBehaviour
{
    [SerializeField] private CameraController _camControl;
    [SerializeField] private Image _square;
    [SerializeField] private Image _cube;

    [SerializeField] private Color _activatedColor;
    [SerializeField] private Color _deactivedColor;

    private void Start()
    {
        _activatedColor.a = 1f;
        _deactivedColor.a = 0.5f;

        _camControl = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
    }
    
    void Update()
    {
        if(_camControl.is2DView)
        {
            _square.color = _activatedColor;
            _cube.color = _deactivedColor;
        }
        else
        {
            _square.color = _deactivedColor;
            _cube.color = _activatedColor;
        }

    }
    
    
}
