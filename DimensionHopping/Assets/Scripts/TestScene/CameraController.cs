using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  
public Transform trackingTarget;

public GameObject FPPposition;

public GameObject player;



//bool für die späteren Player Movement Scripts
public bool _is2DView;

public Vector3 camera2DDefaultPosition;

[SerializeField]
private Vector3 _camera2DPosition;

[SerializeField]
private Vector3 _cameraFirstPersonPosition;



public float sensitivity = 5.0f;


private void Start()
{
    transform.position = camera2DDefaultPosition;
    _is2DView = true;


}


void Update()
{

    Debug.Log("Position des Empty Childs: " + FPPposition.transform.position);


    _camera2DPosition = new Vector3(trackingTarget.position.x, trackingTarget.position.y, trackingTarget.position.z - 10);

    _cameraFirstPersonPosition = FPPposition.transform.position;

    Debug.Log(FPPposition);

    
    // Debug.Log("Camera 2D Position: " + _camera2DPosition);
    // Debug.Log("Camera First Person Position: " + _cameraFirstPersonPosition);


    if (Input.GetKeyDown(KeyCode.H))
    {
        if (_is2DView)
        {
            _is2DView = false;
            //transform.eulerAngles = new Vector3( 0, 90, 0);			transform.eulerAngles = new Vector3(0, FPPposition.transform.rotation.eulerAngles.y, 0);        }
        else
        {
            _is2DView = true;

            //set2DCameraAngle();

            
            transform.eulerAngles = new Vector3( 0, 0, 0);
        }

    }

    // das iTween ist noch buggy, wird sich aber vielleicht eh erledigen wenn wir das selbst coden

    if (_is2DView)
    {
        
        //iTween.MoveTo(this.gameObject, _camera2DPosition, 2);
        trackingIn2d();
        //set2DCameraAngle();
        
    
    }
    else
    {

        //iTween.MoveTo(this.gameObject, _cameraFirstPersonPosition, 2);
        trackingInFP();
        
        
        
    }
    
}


void trackingIn2d()
{
    // _camera2DPosition ist der Vector3 der sich immer parallel zum Player mitbewegt und die festgelegte Position der 2D-Kamera speichert

    transform.position = _camera2DPosition;

}


void trackingInFP()
{

    // Der Code hier steuert die Maus in der FPP. Das mit dem Pitch und Yaw hab ich aus dem Internet, soll verhindern dass sich die
    // z-Achse auch verschiebt. Frag mich nicht wie das funktioniert, aber davor hat sich auch öfter mal die z-Achse verschoben
    // dafür gibts eventuell auch noch eine bessere Lösung mit gimbal lock (heißt doch so oder?) oder sowas.


    float pitch = Input.GetAxis ("Mouse Y") * -1f * sensitivity;

    float yaw = Input.GetAxis("Mouse X") * sensitivity;

    //float yaw = Input.GetAxis("Mouse X") * sensitivity;



    transform.Rotate (pitch * Vector3.right, Space.Self); 

    player.transform.Rotate(yaw * Vector3.up, Space.World);
    


    transform.Rotate (yaw * Vector3.up, Space.World);


    // Kamera transformt die Position und bleibt immer an der Position des Empty Gameobjects "FPPPosition", das ein Child des Players ist 

    transform.position = _cameraFirstPersonPosition;

}



void set2DCameraAngle() {

    if (player.transform.rotation.eulerAngles.y > 45.1 && player.transform.rotation.eulerAngles.y <= 135.0) 
    {
        Debug.Log("transform Cameraposition z - 10");

        transform.eulerAngles = new Vector3( 0, 0, 0);


    }
    else if(player.transform.rotation.eulerAngles.y > 135.1 && player.transform.rotation.eulerAngles.y <= 225)
    {
        Debug.Log("transform Cameraposition x - 10");

        transform.eulerAngles = new Vector3(0 , 90, 0);


    }
    else if((player.transform.rotation.eulerAngles.y > 225.1 && player.transform.rotation.eulerAngles.y <= 269.9) || (player.transform.rotation.eulerAngles.y > -90 && player.transform.rotation.eulerAngles.y < -45))
    {
        Debug.Log("transform Camera z +10");

        transform.eulerAngles = new Vector3(0 , 180, 0);



    }
    else if(player.transform.rotation.eulerAngles.y >= -45 && player.transform.rotation.eulerAngles.y < 45)
    {
        Debug.Log("transform Camera x + 10");

        transform.eulerAngles = new Vector3(0 , -90, 0);

    }

}




// If- Abfrage Funktion in Update: 

/*


 Wenn _is2D == true & Wenn Spieler Y-Rotation zwischen  45.01 und 135, 
 
 dann Kamera: 
 
 transform.eulerAngles = new Vector3( 0, 0, 0);

 und _camera2DPosition = new Vector3(trackingTarget.position.x, trackingTarget.position.y, trackingTarget.position.z - 10)


Wenn _is2D == true & Wenn Spieler Y-Rotation zwischen 135.01 und 225

 dann Kamera: transform.eulerAngles = new Vector3(0 , 90, 0);

 und _camera2DPosition = new Vector3(trackingTarget.position.x -10, trackingTarget.position.y, trackingTarget.position.z )



 Wenn _is2D == true & Wenn Spieler Y-Rotation zwischen 225.1 und 269,9 ODER zwischen -90 und -45 

 dann Kamera: transform.eulerAngles = new Vector3(0 , 180, 0);

 und _camera2DPosition = new Vector3(trackingTarget.position.x, trackingTarget.position.y, trackingTarget.position.z + 10 )


else:

 dann Kamera: transform.eulerAngles = new Vector3(0 , -90, 0)

 und _camera2DPosition = new Vector3(trackingTarget.position.x +10, trackingTarget.position.y, trackingTarget.position.z)

*/










}
