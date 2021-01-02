using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    
    public float speed2D = 5.0f;

    public float speedFP = 10.0f;
    public float jumpForce = 1.0f;
    public float rotationSpeed = 60;

    public CameraController cameraControl;
    public Transform groundCheck;
    public float groundRadius;
    public LayerMask groundMask;
    bool isOnGround;

    public GameObject player;

    [SerializeField]
    //private bool _is2D = true;

    public Rigidbody rb;

    
        void Start()
    {
        rb = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (cameraControl._is2DView)
        {
            //Debug.Log("_is2DView is true.");
            Controller2DPerspective();
        }
        else
        {
            //Debug.Log("_is2DView is false");
            ControllerFPPerspective();
        }

        isOnGround = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);
      
    }


    void Controller2DPerspective() 
    {

        float horizontalMovement = Input.GetAxis("Horizontal");
    
        if (Input.GetKey(KeyCode.D))
        {

            player.transform.Translate(0, 0, horizontalMovement * speed2D *  Time.deltaTime);

        }
        else if (Input.GetKey(KeyCode.A))
        {

            player.transform.Translate(0, 0, horizontalMovement * speed2D * Time.deltaTime);
        }

   
        if (Input.GetKey(KeyCode.Space) && isOnGround)
        {
            rb.AddForce(transform.up * jumpForce);
            Debug.Log("Addforce!");
 
        }


    }

    void ControllerFPPerspective()
    {

        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");


        if (Input.GetKey(KeyCode.W))
        {
            player.transform.Translate(0, 0, verticalMovement * speedFP * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            player.transform.Translate(0, 0, verticalMovement * speedFP * Time.deltaTime);
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            player.transform.Translate(horizontalMovement * speedFP * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            player.transform.Translate(horizontalMovement * speedFP * Time.deltaTime, 0, 0);
        }
        
        if (Input.GetKey(KeyCode.Space) && isOnGround)
        {
            rb.AddForce(transform.up * jumpForce);
            Debug.Log("Addforce!");
 
        }


    }

}
