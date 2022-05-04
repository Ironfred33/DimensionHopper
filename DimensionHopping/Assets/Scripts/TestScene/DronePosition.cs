using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Updated die Position der Drohne
public class DronePosition : MonoBehaviour
{
    public Vector3 clippingVector;
    public float clippingValue;
    public EVCamera cameraEV;

    public CameraController camControl;
    private Vector3 _newDronePosition;
    private Quaternion _newDroneRotation;

    // Start is called before the first frame update
    void Start()
    {
        clippingVector = new Vector3 (0, 0, clippingValue);
        transform.position = camControl.current2DPosition - clippingVector;
    }

    private void Update()
    {
        
        
        _newDroneRotation = Quaternion.Euler(camControl.current2DEulerAngles);


        if(transform.localEulerAngles.y >= 175.0 && transform.localEulerAngles.y <= 185)
        {
            clippingVector = new Vector3(0, 0, -clippingValue);
        }
        else if(transform.localEulerAngles.y >= 85 && transform.localEulerAngles.y <= 95)
        {
            clippingVector = new Vector3(clippingValue, 0, 0);
        }

        else if(transform.localEulerAngles.y >= 265 && transform.localEulerAngles.y <= 275)
        {
            clippingVector = new Vector3(-clippingValue, 0, 0);
        }

        else
        {
            clippingVector = new Vector3(0, 0, clippingValue);
        }

        _newDronePosition = camControl.current2DPosition - clippingVector;

        


    }
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _newDronePosition, cameraEV.smoothing);
        transform.rotation = Quaternion.Lerp(transform.rotation, _newDroneRotation, cameraEV.smoothing);
    }
}
