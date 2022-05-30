using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Steuerung der Kamera
public class CameraController : MonoBehaviour
{

    public Transform trackingTarget;

    public GameObject externalVariables;
    public EVCamera extVars;
    public GameObject fpPosition;
    public GameObject player;
    public Vector3 current2DPosition;
    public Vector3 current2DEulerAngles;

    float xRotationFPP = 0f;
    float yRotationFPP = 0f;

    public bool is2DView;
    private Vector3 _cameraFirstPersonPosition;

    public float smoothing = 0.05f;

    public enum PlayerOrientation
    {

        xPositive,
        xNegative,
        zPositive,
        zNegative


    }

    public PlayerOrientation playerOrientation;


    private void Start()
    {
        extVars = externalVariables.GetComponent<EVCamera>();

        current2DPosition = transform.position;
        is2DView = true;


    }


    void Update()
    {

        // Debug.Log("CAMERACONTROLLER UPDATE TRIGGERED");

        //Debug.Log("Tracking Target: " + trackingTarget.position);

        //Debug.Log("FPP Position: " + FPPposition.position.x, FPPposition.y)


        if(GameObject.FindGameObjectWithTag("Player") != null) _cameraFirstPersonPosition = fpPosition.transform.position;



        if (!is2DView)
        {

            TrackingInFP();


        }

        else if (is2DView)
        {

            TrackingIn2d();

        }

        if (player.transform.rotation.eulerAngles.y > 45.1 && player.transform.rotation.eulerAngles.y <= 135.0)
        {


            current2DPosition = new Vector3(trackingTarget.position.x, trackingTarget.position.y + extVars.cameraHeight2DP, trackingTarget.position.z - extVars.cameraDistance2DP);



        }
        // Weltachsen Ausrichtung des Spielers:   Z Negative 
        else if (player.transform.rotation.eulerAngles.y > 135.1 && player.transform.rotation.eulerAngles.y <= 225)
        {

            current2DPosition = new Vector3(trackingTarget.position.x - extVars.cameraDistance2DP, trackingTarget.position.y + extVars.cameraHeight2DP, trackingTarget.position.z);
            

        }


        // Weltachsen Ausrichtung des Spielers:   X Negative 
        else if ((player.transform.rotation.eulerAngles.y > 225.1 && player.transform.rotation.eulerAngles.y <= 315))
        {
            current2DPosition = new Vector3(trackingTarget.position.x, trackingTarget.position.y + extVars.cameraHeight2DP, trackingTarget.position.z + extVars.cameraDistance2DP);

        }


        // Weltachsen Ausrichtung des Spielers:    Z Positive
        else if (player.transform.rotation.eulerAngles.y >= 315.1 && player.transform.rotation.eulerAngles.y <= 359.9 || player.transform.rotation.eulerAngles.y >= 0 && player.transform.rotation.eulerAngles.y <= 45)
        {

            current2DPosition = new Vector3(trackingTarget.position.x + extVars.cameraDistance2DP, trackingTarget.position.y + extVars.cameraHeight2DP, trackingTarget.position.z);

        }

    }


    // Hier wird auch der Player in die entsprechende Richtung gedreht, das vielleicht noch irgendwo reinhauen, wo es nicht in Update ist, weil es sonst evtl. zu Performanceeinbußen kommt

    public void TrackingIn2d()
    {

        // Weltachsen Ausrichtung des Spielers:   X Positive
        if (player.transform.rotation.eulerAngles.y > 45.1 && player.transform.rotation.eulerAngles.y <= 135.0)
        {


            //transform.position = current2DPosition;

            transform.position = Vector3.Lerp(transform.position, current2DPosition , smoothing);

            player.transform.rotation = Quaternion.Euler(0, 90, 0);

            //  if (!playerIsFlipped)
            // {
            //     yRotationFPP = 0;
            //     playerOrientation = PlayerOrientation.xPositive;
            // }
            // else if (playerIsFlipped)
            // {
            //     yRotationFPP = 180;
            //     playerOrientation = PlayerOrientation.xNegative;
            // }


            yRotationFPP = 0;
            playerOrientation = PlayerOrientation.xPositive;



        }
        // Weltachsen Ausrichtung des Spielers:   Z Negative 
        else if (player.transform.rotation.eulerAngles.y > 135.1 && player.transform.rotation.eulerAngles.y <= 225)
        {

            
            //transform.position = current2DPosition;

            transform.position = Vector3.Lerp(transform.position, current2DPosition , smoothing);

            player.transform.rotation = Quaternion.Euler(0, 180, 0);
            yRotationFPP = 270;
            playerOrientation = PlayerOrientation.zNegative;

        }


        // Weltachsen Ausrichtung des Spielers:   X Negative 
        else if ((player.transform.rotation.eulerAngles.y > 225.1 && player.transform.rotation.eulerAngles.y <= 315))
        {

            //transform.position = current2DPosition;
            transform.position = Vector3.Lerp(transform.position, current2DPosition , smoothing);

            player.transform.rotation = Quaternion.Euler(0, 270, 0);

            yRotationFPP = 180;
            playerOrientation = PlayerOrientation.xNegative;

        }


        // Weltachsen Ausrichtung des Spielers:    Z Positive
        else if (player.transform.rotation.eulerAngles.y >= 315.1 && player.transform.rotation.eulerAngles.y <= 359.9 || player.transform.rotation.eulerAngles.y >= 0 && player.transform.rotation.eulerAngles.y <= 45)
        {

            //transform.position = current2DPosition;
            transform.position = Vector3.Lerp(transform.position, current2DPosition , smoothing);
            player.transform.rotation = Quaternion.Euler(0, 0, 0);
            yRotationFPP = 90;
            playerOrientation = PlayerOrientation.zPositive;
        }


    }


    void TrackingInFP()
    {

        // Der Code hier steuert die Maus in der FPP. Das mit dem Pitch und Yaw hab ich aus dem Internet, soll verhindern dass sich die
        // z-Achse auch verschiebt. Frag mich nicht wie das funktioniert, aber davor hat sich auch öfter mal die z-Achse verschoben
        // dafür gibts eventuell auch noch eine bessere Lösung mit gimbal lock.


        float pitch = Input.GetAxis("Mouse Y") * extVars.mouseSensitivityFP * Time.deltaTime;

        float yaw = Input.GetAxis("Mouse X") * extVars.mouseSensitivityFP * Time.deltaTime;



        // Clampt die vertikale Rotation der Kamera
        yRotationFPP -= yaw;
        xRotationFPP -= pitch;
        xRotationFPP = Mathf.Clamp(xRotationFPP, -60f, 60f);

        //Debug.Log("XRotation = " + pitch);

        //float yaw = Input.GetAxis("Mouse X") * extVars.mouseSensitivityFP;


        // ------------------------------------ HIER BUG ------------------------------

        player.transform.Rotate(yaw * Vector3.up);

        transform.rotation = Quaternion.Euler(xRotationFPP, (-yRotationFPP + 90), 0f);

        // Kamera transformt die Position und bleibt immer an der Position des Empty Gameobjects "FPPPosition", das ein Child des Players ist 

        transform.position = _cameraFirstPersonPosition;

    }



    public void Set2DCameraAngle()
    {

        // AUSRICHTUNG: FRONT ZUR WORLD X - ACHSE
        if (player.transform.rotation.eulerAngles.y > 45.1 && player.transform.rotation.eulerAngles.y <= 135.0)
        {

            current2DEulerAngles = new Vector3(0, 0, 0);



        }
        else if (player.transform.rotation.eulerAngles.y > 135.1 && player.transform.rotation.eulerAngles.y <= 225)
        {

            current2DEulerAngles = new Vector3(0, 90, 0);


        }
        else if ((player.transform.rotation.eulerAngles.y > 225.1 && player.transform.rotation.eulerAngles.y <= 315))
        {

            current2DEulerAngles = new Vector3(0, 180, 0);



        }
        else if (player.transform.rotation.eulerAngles.y >= 315.1 && player.transform.rotation.eulerAngles.y <= 359.9 || player.transform.rotation.eulerAngles.y >= 0 && player.transform.rotation.eulerAngles.y <= 45)
        {

            current2DEulerAngles = new Vector3(0, -90, 0);

        }

    }



}
