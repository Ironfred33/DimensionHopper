using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    public GameObject cam;
    public Transform position2D;
    public Transform positionFPP;

    public animCurve curve2DToFPP;
    public animCurve curveFPPTo2D;

    // Gibt an, wie weit die Kurve ausholt
    public float weight;

    public bool switchingFrom2DtoFPP = false;
    public bool switchingFromFPPto2D = false;
    public bool inFPP = false;

    public float duration;
    private float elapsed;
    private float dt;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            if(inFPP)
            {
                switchingFromFPPto2D = true;
            }

            else
            {
                switchingFrom2DtoFPP = true;
            }
            StartCoroutine(CamTransition());
        }
        
    }

    private IEnumerator CamTransition()
    {
        elapsed = 0f;

        // Wechselt von 2D zur FPP

        if(switchingFrom2DtoFPP)
        {
            while (elapsed <= duration)
            {
                dt = Time.deltaTime;
                elapsed = elapsed + dt;
                Debug.Log("Running");

                // Interpoliert Position und Rotation
                
                cam.transform.position = Vector3.Lerp(position2D.position, positionFPP.position, elapsed / duration) + (new Vector3(curve2DToFPP.curve.Evaluate(elapsed), 0f, 0f) * weight);
                cam.transform.rotation = Quaternion.Lerp(position2D.rotation, positionFPP.rotation, elapsed / duration);

                switchingFrom2DtoFPP = false;
                inFPP = true;
                yield return null;
            }
        }

        // Wechselt von FPP zu 2D

        else if (switchingFromFPPto2D)
        {
            while (elapsed <= duration)
            {
                dt = Time.deltaTime;
                elapsed = elapsed + dt;
                Debug.Log("Running");

                // Interpoliert Position und Rotation

                cam.transform.position = Vector3.Lerp(positionFPP.position, position2D.position, elapsed / duration) + (new Vector3(curveFPPTo2D.curve.Evaluate(elapsed), 0f, 0f) * weight);
                cam.transform.rotation = Quaternion.Lerp(positionFPP.rotation, position2D.rotation, elapsed / duration);

                switchingFromFPPto2D = false;
                inFPP = false;
                yield return null;
            }
        }


    }


}
