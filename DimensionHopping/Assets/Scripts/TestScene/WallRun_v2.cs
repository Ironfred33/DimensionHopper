using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun_v2 : MonoBehaviour
{
    public Transform orientation;
    public Transform cam;
    public float wallDistance;

    private bool _wallLeft = false;
    private bool _wallRight = false;
    public bool isWallRunning = false;

    public float upForce;
    public float sideForce;

    private RaycastHit _leftWallHit;
    private RaycastHit _rightWallHit;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        WallCheck();
    }

    public void WallCheck()
    {
        _wallLeft = Physics.Raycast(transform.position, -orientation.right, out _leftWallHit, wallDistance);
        _wallRight = Physics.Raycast(transform.position, orientation.right, out _rightWallHit, wallDistance);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("RunnableWall"))
        {
            rb.useGravity = false;

            if(_wallRight)
            {
                cam.localEulerAngles = new Vector3(0f, 0f, 10f);
            }

            else if (_wallLeft)
            {
                cam.localEulerAngles = new Vector3(0f, 0f, -10f);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.transform.CompareTag("RunnableWall"))
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(_wallLeft)
                {
                    rb.AddForce(Vector3.up * upForce * Time.deltaTime);
                    rb.AddForce(orientation.right * sideForce * Time.deltaTime);
                }

                else if (_wallLeft)
                {
                    rb.AddForce(Vector3.up * upForce * Time.deltaTime);
                    rb.AddForce(-orientation.right * sideForce * Time.deltaTime);
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        rb.useGravity = true;
        cam.localEulerAngles = new Vector3(0f, 0f, 0f);
    }
}
