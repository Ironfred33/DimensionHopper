using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateBar : MonoBehaviour
{
    public CameraController camControl;
    public Image square;
    public Image cube;

    public Color _activatedColor;
    public Color _deactivedColor;

    private void Start()
    {
        _activatedColor.a = 1f;
        _deactivedColor.a = 0.5f;
        
    }
    void Update()
    {
        if(camControl._is2DView)
        {
            square.color = _activatedColor;
            cube.color = _deactivedColor;
        }
        else
        {
            square.color = _deactivedColor;
            cube.color = _activatedColor;
        }

    }
    
}
