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



            // Die nächste Zeile sorgt dafür, dass sich die Kamera um 90 Grad dreht, wenn FPP eingenommen wird.
            // klappt aber nur einmal, ist also auch buggy :D 

            transform.eulerAngles = new Vector3( 0, 90, 0);
        }
        else
        {
            _is2DView = true;

            // Ich denke hier liegt das Problem, das "zurückswitchen" muss man irgendwie anders bewerkstelligen
            
            transform.eulerAngles = new Vector3( 0, 0, 0);
        }

    }

    // das iTween ist noch buggy, wird sich aber vielleicht eh erledigen wenn wir das selbst coden

    if (_is2DView)
    {
        
        iTween.MoveTo(this.gameObject, _camera2DPosition, 2);
        trackingIn2d();
        
        
    }
    else
    {

        iTween.MoveTo(this.gameObject, _cameraFirstPersonPosition, 2);
        trackingInFP();
        
        
        
    }
    
}

/*
IEnumerator waitForSeconds(int seconds)
    {

        yield return new WaitForSeconds(seconds);
        


    }
*/

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

    float yaw = Input.GetAxis ("Mouse X") * sensitivity;



    transform.Rotate (pitch * Vector3.right, Space.Self); 

    player.transform.Rotate(yaw * Vector3.up, Space.World);
    


    transform.Rotate (yaw * Vector3.up, Space.World);


    // Kamera transformt die Position und bleibt immer an der Position des Empty Gameobjects "FPPPosition", das ein Child des Players ist 

    transform.position = _cameraFirstPersonPosition;

   

}


}
