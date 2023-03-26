using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Updated die Position der Drohne
public class DronePosition : MonoBehaviour
{
    public EVCamera cameraEV;
    public CameraController camControl;

    private Vector3 _clippingVector;
    private Vector3 _newDronePosition;
    private Quaternion _newDroneRotation;

    [SerializeField] private float _clippingValue;

    // Start is called before the first frame update
    void Start()
    {
        _clippingVector = new Vector3 (0, 0, _clippingValue);
        transform.position = camControl.current2DPosition - _clippingVector;
    }

    private void Update()
    {
        
        
        _newDroneRotation = Quaternion.Euler(camControl.current2DEulerAngles);


        if(transform.localEulerAngles.y >= 175.0 && transform.localEulerAngles.y <= 185)
        {
            _clippingVector = new Vector3(0, 0, -_clippingValue);
        }
        else if(transform.localEulerAngles.y >= 85 && transform.localEulerAngles.y <= 95)
        {
            _clippingVector = new Vector3(_clippingValue, 0, 0);
        }

        else if(transform.localEulerAngles.y >= 265 && transform.localEulerAngles.y <= 275)
        {
            _clippingVector = new Vector3(-_clippingValue, 0, 0);
        }

        else
        {
            _clippingVector = new Vector3(0, 0, _clippingValue);
        }

        _newDronePosition = camControl.current2DPosition - _clippingVector;

        


    }
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _newDronePosition, cameraEV.smoothing);
        transform.rotation = Quaternion.Lerp(transform.rotation, _newDroneRotation, cameraEV.smoothing);
    }
}
