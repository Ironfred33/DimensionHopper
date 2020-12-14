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
            Debug.Log("_is2DView is true.");
            controller2DPerspective();
        }
        else
        {
            Debug.Log("_is2DView is false");
            controllerFPPerspective();
        }
      
    }


    void controller2DPerspective() 
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


        // noch checken, ob grounded, und übernehmen zu FPPperspectiveController

        else if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(transform.up * jumpForce);
            Debug.Log("Addforce!");
 
        }


    }

    void controllerFPPerspective()
    {

        if (Input.GetKey(KeyCode.W))
        {
            player.transform.Translate(Vector3.forward * speedFP * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            player.transform.Translate(Vector3.back * speedFP * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            player.transform.Translate(Vector3.right * speedFP * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            player.transform.Translate(Vector3.left * speedFP * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(transform.up * jumpForce);
            Debug.Log("Addforce!");
 
        }


    }

}
