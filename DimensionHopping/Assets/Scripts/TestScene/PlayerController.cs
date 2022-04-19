using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Running,
    Jumping
}
public class PlayerController : MonoBehaviour
{
    public PlayerState state;
    //public GameObject externalVariables;
    public EVPlayer extVars;
    public CameraController cameraControl;
    public CameraTransition cameraTransition;
    public Transform groundCheck;
    public float groundRadius;
    public LayerMask groundMask;
    bool isOnGround;
    private float movement = 0f;
    public bool lookRight = true;
    public GameObject player;

    public Rigidbody rb;
    public PlayerHealth health;

    public Animator anim;

    public bool playerIsFlipped;
    private bool _gamePaused;

    public SFX soundEffects;

    
    void Start()
    {
        rb = player.GetComponent<Rigidbody>();

        //extVars = externalVariables.GetComponent<EVPlayer>();


    }


    private void Update()
    {
        // animations
        if(state == PlayerState.Idle)
        {
            anim.SetBool("isRunning", false);
            anim.SetBool("isJumping", false);
        }

        else if (state == PlayerState.Running)
        {
            anim.SetBool("isRunning", true);
            anim.SetBool("isJumping", false);
        }

        else if(state == PlayerState.Jumping)
        {
            anim.SetBool("isJumping", true);
            anim.SetBool("isRunning", false);
        }

        if (cameraControl._is2DView)
        {
            controller2DPerspective();
        }
        else
        {
            controllerFPPerspective();
        }

        

    }
    void FixedUpdate()
    {

        isOnGround = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);

        float globalGravity = -9.81f;
        Vector3 gravity = globalGravity * extVars.gravityScale * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);


      
    }



    void controller2DPerspective() 
    {

        float horizontalMovement = Input.GetAxis("Horizontal");
    
        if (Input.GetKey(KeyCode.D))
        {

            player.transform.Translate(0, 0, horizontalMovement * extVars.speed2D *  Time.deltaTime);
            player.transform.localScale = new Vector3(1, 1, 1);
            //cameraControl.FPPposition.transform.localScale = new Vector3(1, 1, 1);

            playerIsFlipped = false;

            state = PlayerState.Running;

        }
        else if (Input.GetKey(KeyCode.A))
        {

            player.transform.Translate(0, 0, horizontalMovement * extVars.speed2D * Time.deltaTime);
            player.transform.localScale = new Vector3(1, 1, -1);
            //player.transform.Rotate(0, 180, 0);

            //cameraControl.FPPposition.transform.Rotate(0, 180, 0);
            

            playerIsFlipped = true;

            state = PlayerState.Running;
            
        }
        else
        {
            state = PlayerState.Idle;
        }


   
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            
            soundEffects.PlayJumpSound();
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.drag = extVars.linearDrag * 0.15f;
            Debug.Log("Jumped" + Vector3.up.normalized * extVars.jumpForce2D);
            rb.AddForce(Vector3.up.normalized * extVars.jumpForce2D, ForceMode.Impulse);
            state = PlayerState.Jumping;
        }

        
        if(!isOnGround)
        {
            extVars.gravityScale = 1;
            if(rb.velocity.y < 0)
            {
                extVars.gravityScale = extVars.gravity * extVars.fallMultiplier;
            }
            else if(rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            {
                extVars.gravityScale = extVars.gravity * (extVars.fallMultiplier / 2);
            }
        }
        
        else
        {
            extVars.gravityScale = 0;
            rb.drag = extVars.linearDrag;
        }

        


    }

    void controllerFPPerspective()
    {

        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");


        if (Input.GetKey(KeyCode.W))
        {
            
            if(!playerIsFlipped)
            {
                player.transform.Translate(0, 0, verticalMovement * extVars.speedFP * Time.deltaTime);
                state = PlayerState.Running;
            }
            else if(playerIsFlipped)
            {
                player.transform.Translate(0, 0, - verticalMovement * extVars.speedFP * Time.deltaTime);
                state = PlayerState.Running;
            }
            
            
        }
        else if (Input.GetKey(KeyCode.S))
        {

            if(!playerIsFlipped)
            {
                player.transform.Translate(0, 0, verticalMovement * extVars.speedFP * Time.deltaTime);
                state = PlayerState.Running;
            }
            else if(playerIsFlipped)
            {
                player.transform.Translate(0, 0, - verticalMovement * extVars.speedFP * Time.deltaTime);
                state = PlayerState.Running;
            }
            
        }
        
        if (Input.GetKey(KeyCode.D))
        {

            if(!playerIsFlipped)
            {
                player.transform.Translate(horizontalMovement * extVars.speedFP * Time.deltaTime, 0, 0);
                state = PlayerState.Running;
            }
            else if(playerIsFlipped)
            {
                player.transform.Translate(- horizontalMovement * extVars.speedFP * Time.deltaTime, 0, 0);
                state = PlayerState.Running;
            }



            // player.transform.Translate(horizontalMovement * extVars.speedFP * Time.deltaTime, 0, 0);
            // state = PlayerState.Running;
        }
        else if (Input.GetKey(KeyCode.A))
        {

            if(!playerIsFlipped)
            {
                player.transform.Translate(horizontalMovement * extVars.speedFP * Time.deltaTime, 0, 0);
                state = PlayerState.Running;
            }
            else if(playerIsFlipped)
            {
                player.transform.Translate(- horizontalMovement * extVars.speedFP * Time.deltaTime, 0, 0);
                state = PlayerState.Running;
            }

            
            // player.transform.Translate(horizontalMovement * extVars.speedFP * Time.deltaTime, 0, 0);
            // state = PlayerState.Running;
        }

        
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !player.GetComponent<WallRun_v2>().isWallRunning)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.drag = extVars.linearDrag * 0.15f;
            soundEffects.PlayJumpSound();
            rb.AddForce(Vector3.up.normalized * extVars.jumpForceFP, ForceMode.Impulse);
            Debug.Log("Addforce!");
            anim.SetBool("isJumping", true);
            

        }

        if(!isOnGround)
        {
            extVars.gravityScale = 1;
            if(rb.velocity.y < 0)
            {
                extVars.gravityScale = extVars.gravity * extVars.fallMultiplier;
            }
            else if(rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            {
                extVars.gravityScale = extVars.gravity * (extVars.fallMultiplier / 2);
            }
        }
        
        else
        {
            extVars.gravityScale = 0;
            rb.drag = extVars.linearDrag;
        }

        if(!Input.anyKey)
        {
            state = PlayerState.Idle;
        }


    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.CompareTag("Deadly"))
        {
            soundEffects.PlayDamageSound();
            health.currentHearts -= 1;
            Debug.Log("Lost a heart");

            if(health.currentHearts <= 0)
            {
                Debug.Log("Dead!");
                player.transform.position = extVars.spawnPoint;
                Debug.Log("Respawned.");
                health.currentHearts = extVars.maxHearts;
                Debug.Log("Hearts set back to maxHearts");
            }
        }

    }

    // Lets character stay on moving platform
    private void OnCollisionStay(Collision collisionInfo)
    {
        if(cameraTransition._transitionInProgress && collisionInfo.collider.CompareTag("PGOzNegative") || collisionInfo.collider.CompareTag("PGOzPositive") || collisionInfo.collider.CompareTag("PGOxPositive") || collisionInfo.collider.CompareTag("PGOxNegative") || collisionInfo.collider.CompareTag("MovingPlatform"))
        {
            Debug.Log("Parenting");
            this.transform.SetParent(collisionInfo.collider.transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        this.transform.SetParent(null);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("OutOfBounds"))
        {
            Debug.Log("Out of Bounds");
            player.transform.position = extVars.spawnPoint;
            health.currentHearts = extVars.maxHearts;
        }

        if (other.gameObject.CompareTag("Deadly"))
        {
            soundEffects.PlayDamageSound();
            health.currentHearts -= 1;

            if(health.currentHearts <= 0)
            {
                player.transform.position = extVars.spawnPoint;
                health.currentHearts = extVars.maxHearts;
            }
        }
    }

        
}



