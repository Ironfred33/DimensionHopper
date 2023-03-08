using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Regelt die Interpolation des Perspektivwechsels
public class CameraLerp : MonoBehaviour
{

    private AnimationCurve _mCurve;
    [SerializeField] private Transform _positionFPP;
    [SerializeField] private Transform _position2D;

    
    [SerializeField] private float _duration = 3f;
    private float _timer = 0f;

    private void Update()
    {
        _timer += Time.deltaTime;

        if(_timer > _duration)
        {
            _timer = _duration;
        }

        float lerpRatio = _timer / _duration;

        transform.position = Vector3.Lerp(_positionFPP.position, _position2D.position, lerpRatio) * _mCurve.Evaluate(lerpRatio);


    }
}
