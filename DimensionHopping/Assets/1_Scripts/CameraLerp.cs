using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Regelt die Interpolation des Perspektivwechsels
public class CameraLerp : MonoBehaviour
{
    public Transform positionFPP;
    public Transform position2D;

    public AnimationCurve mCurve;

    public float duration = 3f;

    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer > duration)
        {
            timer = duration;
        }

        float lerpRatio = timer / duration;

        transform.position = Vector3.Lerp(positionFPP.position, position2D.position, lerpRatio) * mCurve.Evaluate(lerpRatio);


    }
}
