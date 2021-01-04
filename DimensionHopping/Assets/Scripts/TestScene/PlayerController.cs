using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject externalVariables;
    EVPlayer extVars;
    public CameraController cameraControl;
    public Transform groundCheck;
    public float groundRadius;
    public LayerMask groundMask;
    bool isOnGround;
    public GameObject player;

    public Rigidbody rb;

    
        void Start()
    {
        rb = player.GetComponent<Rigidbody>();

        extVars = externalVariables.GetComponent<EVPlayer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (cameraControl._is2DView)
        {
            controller2DPerspective();
        }
        else
        {
            controllerFPPerspective();
        }

        isOnGround = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);
      
    }


    void controller2DPerspective() 
    {

        float horizontalMovement = Input.GetAxis("Horizontal");
    
        if (Input.GetKey(KeyCode.D))
        {

            player.transform.Translate(0, 0, horizontalMovement * extVars.speed2D *  Time.deltaTime);

        }
        else if (Input.GetKey(KeyCode.A))
        {

            player.transform.Translate(0, 0, horizontalMovement * extVars.speed2D * Time.deltaTime);
        }

   
        if (Input.GetKey(KeyCode.Space) && isOnGround)
        {
            rb.AddForce(transform.up * extVars.jumpForce2D);
 
        }


    }

    void controllerFPPerspective()
    {

        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");


        if (Input.GetKey(KeyCode.W))
        {
            player.transform.Translate(0, 0, verticalMovement * extVars.speedFP * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            player.transform.Translate(0, 0, verticalMovement * extVars.speedFP * Time.deltaTime);
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            player.transform.Translate(horizontalMovement * extVars.speedFP * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            player.transform.Translate(horizontalMovement * extVars.speedFP * Time.deltaTime, 0, 0);
        }
        
        if (Input.GetKey(KeyCode.Space) && isOnGround)
        {
            rb.AddForce(transform.up * extVars.jumpForceFP);
            Debug.Log("Addforce!");
 
        }


    }

}
