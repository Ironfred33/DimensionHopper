using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Steuerung des Spieler-Charakters
public enum PlayerState
{
    Idle,
    Running,
    Jumping,
    Landing
}
public class PlayerController : MonoBehaviour
{

    public PlayerState state;

    [SerializeField] private Material standardMaterial;
    [SerializeField] private Material transparencyMaterial;
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

    private bool _flickering;
    private bool _transparentMaterial;

    private float invincibleFlickerSpeed = 0.2f;
    private float _elapsed;
    private float _dt;


    void Start()
    {
        _rb = player.GetComponent<Rigidbody>();

        standardMaterial = this.transform.Find("Body").GetComponent<SkinnedMeshRenderer>().materials[0];

        transparencyMaterial.color = new Color(1.0f, 1.0f, 1.0f, extVars.invincibleTransparency);

        canvas = GameObject.Find("Canvas");


    }


    private void Update()
    {
        // animations
        if (state == PlayerState.Idle)
        {
            anim.SetBool("isRunning", false);
            anim.SetBool("isJumping", false);
            anim.SetBool("isLanding", false);
        }

        else if (state == PlayerState.Running)
        {
            anim.SetBool("isRunning", true);
            anim.SetBool("isJumping", false);
        }

        else if (state == PlayerState.Jumping)
        {
            anim.SetBool("isJumping", true);
            anim.SetBool("isRunning", false);
        }

        else if(state == PlayerState.Landing)
        {
            anim.SetBool("isLanding", true);
            anim.SetBool("isJumping", false);
        }

        Debug.Log("Current state: " + state);

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

        if (health.currentHearts <= 0)
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

            player.transform.Translate(0, 0, horizontalMovement * extVars.speed2D * Time.deltaTime);
            player.transform.localScale = new Vector3(player.transform.localScale.x, player.transform.localScale.y, Mathf.Abs(player.transform.localScale.z));


            playerIsFlipped = false;

            state = PlayerState.Running;

        }
        else if (Input.GetKey(KeyCode.A))
        {

            player.transform.Translate(0, 0, horizontalMovement * extVars.speed2D * Time.deltaTime);
            player.transform.localScale = new Vector3(player.transform.localScale.x, player.transform.localScale.y, -Mathf.Abs(player.transform.localScale.z));


            playerIsFlipped = true;

            state = PlayerState.Running;

        }

        else
        {
            state = PlayerState.Idle;
        }


        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            state = PlayerState.Jumping;
            soundEffects.PlayJumpSound();
            _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
            _rb.drag = extVars.linearDrag * 0.15f;
            Debug.Log("Jumped" + Vector3.up.normalized * extVars.jumpForce2D);
            _rb.AddForce(Vector3.up.normalized * extVars.jumpForce2D, ForceMode.Impulse);
            
        }


        if (!isOnGround)
        {
            extVars.gravityScale = 1;
            if (_rb.velocity.y < 0)
            {
                extVars.gravityScale = extVars.gravity * extVars.fallMultiplier;
            }
            else if (_rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
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

            if (!playerIsFlipped)
            {
                player.transform.Translate(0, 0, verticalMovement * extVars.speedFP * Time.deltaTime);
                state = PlayerState.Running;
            }
            else if (playerIsFlipped)
            {
                player.transform.Translate(0, 0, -verticalMovement * extVars.speedFP * Time.deltaTime);
                state = PlayerState.Running;
            }


        }
        else if (Input.GetKey(KeyCode.S))
        {

            if (!playerIsFlipped)
            {
                player.transform.Translate(0, 0, verticalMovement * extVars.speedFP * Time.deltaTime);
                state = PlayerState.Running;
            }
            else if (playerIsFlipped)
            {
                player.transform.Translate(0, 0, -verticalMovement * extVars.speedFP * Time.deltaTime);
                state = PlayerState.Running;
            }

        }

        if (Input.GetKey(KeyCode.D))
        {

            if (!playerIsFlipped)
            {
                player.transform.Translate(horizontalMovement * extVars.speedFP * Time.deltaTime, 0, 0);
                state = PlayerState.Running;
            }
            else if (playerIsFlipped)
            {
                player.transform.Translate(-horizontalMovement * extVars.speedFP * Time.deltaTime, 0, 0);
                state = PlayerState.Running;
            }

        }
        else if (Input.GetKey(KeyCode.A))
        {

            if (!playerIsFlipped)
            {
                player.transform.Translate(horizontalMovement * extVars.speedFP * Time.deltaTime, 0, 0);
                state = PlayerState.Running;
            }
            else if (playerIsFlipped)
            {
                player.transform.Translate(-horizontalMovement * extVars.speedFP * Time.deltaTime, 0, 0);
                state = PlayerState.Running;
            }
        }


        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !player.GetComponent<WallRun_v2>().isWallRunning)
        {
            anim.SetBool("isJumping", true);
            _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
            _rb.drag = extVars.linearDrag * 0.15f;
            soundEffects.PlayJumpSound();
            _rb.AddForce(Vector3.up.normalized * extVars.jumpForceFP, ForceMode.Impulse);
            Debug.Log("Addforce!");


        }

        else if (player.GetComponent<WallRun_v2>().isWallRunning && Input.GetKey(KeyCode.W))
        {
            extVars.gravityScale = 0.5f;
        }



        else if (!isOnGround)
        {
            extVars.gravityScale = 1;
            if (_rb.velocity.y < 0)
            {
                extVars.gravityScale = extVars.gravity * extVars.fallMultiplier;
            }
            else if (_rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            {
                extVars.gravityScale = extVars.gravity * (extVars.fallMultiplier / 2);
            }
        }

        else
        {
            extVars.gravityScale = 0;
            _rb.drag = extVars.linearDrag;
        }

        if (!Input.anyKey)
        {
            state = PlayerState.Idle;
        }


    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.CompareTag("Deadly"))
        {
            if (!_invincible)
            {
                soundEffects.PlayDamageSound();
                health.currentHearts -= 1;

            }

            Debug.Log("Lost a heart");
            StartCoroutine(InvincibleTime());
            PlayerKnockBack();

        }

        else if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            state = PlayerState.Landing;
        }

    }

    void PlayerKnockBack()
    {


        Debug.Log("KNOCKBACK!");

        //Richtung abhängig von Spielerausrichtung machen

        _rb.AddForce(extVars.knockBackForce * Vector3.left, ForceMode.Impulse);
        _rb.AddForce(extVars.knockBackForce * Vector3.up, ForceMode.Impulse);
    }

    public IEnumerator InvincibleTime()
    {
        _invincible = true;

        Debug.Log("INVINCIBLE TRIGGERED");

        ChangeMaterial(transparencyMaterial);
        _transparentMaterial = true;


        StartCoroutine(Flicker());

        Debug.Log("MATERIAL: " + this.transform.Find("Body").GetComponent<SkinnedMeshRenderer>().materials[0]);

        //yield return new WaitForSeconds(extVars.invincibleTime);

        yield return Flicker();

        // HIER COROUTINE HIN -> Flackern

        _transparentMaterial = false;
        _invincible = false;

        ChangeMaterial(standardMaterial);

    }

    // antagonist  -> hier weiterarbeiten

    private IEnumerator Flicker()
    {

        _flickering = true;

        bool switcher = false;

        float flickerFrequency = extVars.invincibleTime / extVars.flickerSpeed;

        for (int i = 0; i < flickerFrequency; i ++)
        {

            if (switcher == false)
            {
                
                transparencyMaterial.color = new Color(1.0f, 1.0f, 1.0f, extVars.invincibleTransparency);

                ChangeMaterial(transparencyMaterial);

                Debug.Log("1 Triggered");

            }

            else if (switcher == true)
            {
                transparencyMaterial.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

                ChangeMaterial(transparencyMaterial);

                Debug.Log("2 Triggered");

            }

            if (switcher == true) switcher = false;
            else if (switcher == false) switcher = true;

            yield return new WaitForSeconds(extVars.flickerSpeed);
            

            Debug.Log(switcher);


        }

        _flickering = false;

        yield return null;

    }


    public void ChangeMaterial(Material mat)
    {
        var materials = this.transform.Find("Body").GetComponent<SkinnedMeshRenderer>().materials;

        materials[0] = mat;

        this.transform.Find("Body").GetComponent<SkinnedMeshRenderer>().materials = materials;

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
        // Debug.Log("Tag =" + collisionInfo.gameObject.tag);
        if (collisionInfo.collider.CompareTag("PGOzNegative") || collisionInfo.collider.CompareTag("PGOzPositive") || collisionInfo.collider.CompareTag("PGOxPositive") || collisionInfo.collider.CompareTag("PGOxNegative") || collisionInfo.collider.CompareTag("MovingPlatform"))
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
        if (other.gameObject.CompareTag("OutOfBounds"))
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



