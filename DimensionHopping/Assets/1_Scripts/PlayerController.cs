using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Steuerung des Spieler-Charakters
public enum PlayerState
{
    Idle,
    Running,
    Jumping
}
public class PlayerController : MonoBehaviour
{

    public PlayerState state;
    public EVPlayer extVars;
    public CameraController cameraControl;
    public CameraTransition cameraTransition;
    public Transform groundCheck;
    public float groundRadius;
    public LayerMask groundMask;
    public bool isOnGround;
    public GameObject player;
    private Rigidbody _rb;
    public PlayerHealth health;
    public Animator anim;
    public bool playerIsFlipped;
    public SFX soundEffects;
    public bool parented;
    public GameObject canvas;
    private bool _invincible;

    
    void Start()
    {
        _rb = player.GetComponent<Rigidbody>();

        canvas = GameObject.Find("Canvas");


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

        if (cameraControl.is2DView)
        {
            Controller2DPerspective();
        }
        else
        {
            ControllerFPPerspective();
        }


    CheckForHearts();

        

    }
    void FixedUpdate()
    {

        isOnGround = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);

        float globalGravity = -9.81f;
        Vector3 gravity = globalGravity * extVars.gravityScale * Vector3.up;
        _rb.AddForce(gravity, ForceMode.Acceleration);


      
    }


    // Playerrespawn wenn alle Herzen verbraucht 
    void CheckForHearts()
    {

          if(health.currentHearts <= 0)
            {
                GameOver();
            }

    }


    // Steuert den Charakter in der 2D-Seitenansicht
    void Controller2DPerspective() 
    {

        float horizontalMovement = Input.GetAxis("Horizontal");
    
        if (Input.GetKey(KeyCode.D))
        {

            player.transform.Translate(0, 0, horizontalMovement * extVars.speed2D *  Time.deltaTime);
            player.transform.localScale = new Vector3(1, 1, 1);
          

            playerIsFlipped = false;

            state = PlayerState.Running;

        }
        else if (Input.GetKey(KeyCode.A))
        {

            player.transform.Translate(0, 0, horizontalMovement * extVars.speed2D * Time.deltaTime);
            player.transform.localScale = new Vector3(1, 1, -1);
            

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
            _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
            _rb.drag = extVars.linearDrag * 0.15f;
            Debug.Log("Jumped" + Vector3.up.normalized * extVars.jumpForce2D);
            _rb.AddForce(Vector3.up.normalized * extVars.jumpForce2D, ForceMode.Impulse);
            state = PlayerState.Jumping;
        }

        
        if(!isOnGround)
        {
            extVars.gravityScale = 1;
            if(_rb.velocity.y < 0)
            {
                extVars.gravityScale = extVars.gravity * extVars.fallMultiplier;
            }
            else if(_rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            {
                extVars.gravityScale = extVars.gravity * (extVars.fallMultiplier / 2);
            }
        }
        
        else
        {
            extVars.gravityScale = 0;
            _rb.drag = extVars.linearDrag;
        }

        


    }

    // Steuert den Charakter in der Egoperspektive
    void ControllerFPPerspective()
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
        }

        
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !player.GetComponent<WallRun_v2>().isWallRunning)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
            _rb.drag = extVars.linearDrag * 0.15f;
            soundEffects.PlayJumpSound();
            _rb.AddForce(Vector3.up.normalized * extVars.jumpForceFP, ForceMode.Impulse);
            Debug.Log("Addforce!");
            anim.SetBool("isJumping", true);
            

        }

        else if(player.GetComponent<WallRun_v2>().isWallRunning && Input.GetKey(KeyCode.W))
        {
            extVars.gravityScale = 0.5f;
        }



        else if(!isOnGround)
        {
            extVars.gravityScale = 1;
            if(_rb.velocity.y < 0)
            {
                extVars.gravityScale = extVars.gravity * extVars.fallMultiplier;
            }
            else if(_rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            {
                extVars.gravityScale = extVars.gravity * (extVars.fallMultiplier / 2);
            }
        }
        
        else
        {
            extVars.gravityScale = 0;
            _rb.drag = extVars.linearDrag;
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
            if (!_invincible) health.currentHearts -= 1;
            Debug.Log("Lost a heart");
            StartCoroutine(InvincibleTime());
            PlayerKnockBack();

        }

    }

    void PlayerKnockBack()
    {
        Debug.Log("KNOCKBACK!");
        _rb.AddForce(extVars.knockBackForce * Vector3.left, ForceMode.Impulse);
        _rb.AddForce(extVars.knockBackForce * Vector3.up, ForceMode.Impulse);
    }

    public IEnumerator InvincibleTime()
    {
        _invincible = true;

        yield return new WaitForSeconds(extVars.invincibleTime);

        _invincible = false;
    }

    // Aktiviert GameOverScreen und respawnt Spieler
    public void GameOver()
    {
        canvas.GetComponent<UIManager>().state = UIState.GameOver;
        player.transform.position = extVars.spawnPoint;
        health.currentHearts = extVars.maxHearts;
        
    }

    // Lets character stay on moving platform
    
    private void OnCollisionStay(Collision collisionInfo)
    {
        Debug.Log("Tag =" + collisionInfo.gameObject.tag);
        if(collisionInfo.collider.CompareTag("PGOzNegative") || collisionInfo.collider.CompareTag("PGOzPositive") || collisionInfo.collider.CompareTag("PGOxPositive") || collisionInfo.collider.CompareTag("PGOxNegative") || collisionInfo.collider.CompareTag("MovingPlatform"))
        {
            Debug.Log("Parenting");
            this.transform.SetParent(collisionInfo.collider.transform);
            parented = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        this.transform.SetParent(null);
        parented = false;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("OutOfBounds"))
        {
            GameOver();
        }

        // if (other.gameObject.CompareTag("Deadly"))
        // {
        //     soundEffects.PlayDamageSound();
        //     health.currentHearts -= 1;

        //     if(health.currentHearts <= 0)
        //     {
        //         player.transform.position = extVars.spawnPoint;
        //         health.currentHearts = extVars.maxHearts;
        //     }
        // }


    }

        
}



