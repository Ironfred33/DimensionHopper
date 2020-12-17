using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  
    public Transform trackingTarget;

    public GameObject FPPposition;

    public GameObject player;

    public int cameraDistance2DP = 10;

    public int cameraHeight2DP = 2;

    public float cameraRotation2DP;

    float xRotationFPP = 0f;

    float yRotationFPP = 0f;



    //bool für die späteren Player Movement Scripts

    public bool _is2DView;

    private Vector3 _camera2DDefaultPosition;

    private Vector3 _cameraFirstPersonPosition;



    public float sensitivityFPP = 5.0f;


    private void Start()
    {
        transform.position = _camera2DDefaultPosition;
        _is2DView = true;


    }


    void Update()
    {

        _cameraFirstPersonPosition = FPPposition.transform.position;



        if (Input.GetKeyDown(KeyCode.H))
        {
            if (_is2DView)
            {
                _is2DView = false;
            		
            
                transform.eulerAngles = new Vector3(0, FPPposition.transform.rotation.eulerAngles.y, 0);        
                }
            else
            {
                _is2DView = true;

                set2DCameraAngle();

            
          
            }

        }


        if (_is2DView)
        {
    
            trackingIn2d();

        }
        else
        {

            trackingInFP();
     
        }
    
    }


    void trackingIn2d()
    {

        if (player.transform.rotation.eulerAngles.y > 45.1 && player.transform.rotation.eulerAngles.y <= 135.0) 
        {


            transform.position = new Vector3(trackingTarget.position.x, trackingTarget.position.y + cameraHeight2DP, trackingTarget.position.z - cameraDistance2DP);


        }
        else if(player.transform.rotation.eulerAngles.y > 135.1 && player.transform.rotation.eulerAngles.y <= 225)
        {
     
            transform.position = new Vector3(trackingTarget.position.x -cameraDistance2DP, trackingTarget.position.y + cameraHeight2DP, trackingTarget.position.z );

        }
        else if((player.transform.rotation.eulerAngles.y > 225.1 && player.transform.rotation.eulerAngles.y <= 315))
        {
            transform.position = new Vector3(trackingTarget.position.x, trackingTarget.position.y + cameraHeight2DP, trackingTarget.position.z + cameraDistance2DP );


        }
        else if(player.transform.rotation.eulerAngles.y >= 315.1 && player.transform.rotation.eulerAngles.y <= 359.9 || player.transform.rotation.eulerAngles.y >= 0 && player.transform.rotation.eulerAngles.y <= 45)
        {
     
            transform.position = new Vector3(trackingTarget.position.x + cameraDistance2DP, trackingTarget.position.y + cameraHeight2DP, trackingTarget.position.z);

        }


    }


    void trackingInFP()
    {

        // Der Code hier steuert die Maus in der FPP. Das mit dem Pitch und Yaw hab ich aus dem Internet, soll verhindern dass sich die
        // z-Achse auch verschiebt. Frag mich nicht wie das funktioniert, aber davor hat sich auch öfter mal die z-Achse verschoben
        // dafür gibts eventuell auch noch eine bessere Lösung mit gimbal lock (heißt doch so oder?) oder sowas.


     
        
        float pitch = Input.GetAxis ("Mouse Y") * sensitivityFPP * Time.deltaTime;

        float yaw = Input.GetAxis("Mouse X") * sensitivityFPP * Time.deltaTime;



        // Clampt die vertikale Rotation der Kamera
        yRotationFPP -= yaw;
        xRotationFPP -= pitch;
        xRotationFPP = Mathf.Clamp(xRotationFPP, -60f, 60f);

        Debug.Log("XRotation = " + pitch);

        //float yaw = Input.GetAxis("Mouse X") * sensitivityFPP;



    

        player.transform.Rotate(yaw * Vector3.up);
        transform.localRotation = Quaternion.Euler(xRotationFPP, (-yRotationFPP + 90f), 0f);




        // Kamera transformt die Position und bleibt immer an der Position des Empty Gameobjects "FPPPosition", das ein Child des Players ist 

        transform.position = _cameraFirstPersonPosition;

    }



    void set2DCameraAngle() {

        if (player.transform.rotation.eulerAngles.y > 45.1 && player.transform.rotation.eulerAngles.y <= 135.0) 
        {

            transform.eulerAngles = new Vector3( cameraRotation2DP, 0, 0);


        }
        else if(player.transform.rotation.eulerAngles.y > 135.1 && player.transform.rotation.eulerAngles.y <= 225)
        {

            transform.eulerAngles = new Vector3(cameraRotation2DP, 90, 0);


        }
        else if((player.transform.rotation.eulerAngles.y > 225.1 && player.transform.rotation.eulerAngles.y <= 315))
        {

            transform.eulerAngles = new Vector3(cameraRotation2DP, 180, 0);



        }
        else if(player.transform.rotation.eulerAngles.y >= 315.1 && player.transform.rotation.eulerAngles.y <= 359.9 || player.transform.rotation.eulerAngles.y >= 0 && player.transform.rotation.eulerAngles.y <= 45)
        {

            transform.eulerAngles = new Vector3(cameraRotation2DP, -90, 0);

        }

    }



}
