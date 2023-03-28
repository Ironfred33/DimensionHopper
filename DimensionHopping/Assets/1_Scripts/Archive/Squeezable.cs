using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squeezable : MonoBehaviour
{

    public ChangePosition changePosScript;
    
    private Vector3 _secondPosition;
    private Rigidbody _rb;

    public float maximumSqueeze;
    public float squeezeSpeed;
    public float squeezeAmount;
    private float _elapsed;
    private bool _isSqueezing;
    private bool _isParented;


    void Start()
    {
        Setup();

    }

    // Update is called once per frame
    void Update()
    {

        CheckForSqueeze();



    }

    void Setup()
    {
        _rb = this.GetComponent<Rigidbody>();

    }

    void CheckForSqueeze()
    {
        if (changePosScript != null)
        {
            if (!changePosScript.transitionInProgress)
            {
                if (!CheckForMovement(_rb) && _isParented)
                {
                    UnparentThis();
                    changePosScript = null;
                }

            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {

        if (other.transform.tag == "Squeezer")
        {

            ParentThis(other.gameObject);

            changePosScript = other.transform.GetComponent<ChangePosition>();

            _isSqueezing = true;

            if (!_isSqueezing)
            {
                StartCoroutine(Squeeze());

            }

        }
    }



    IEnumerator Squeeze()
    {

        _isSqueezing = true;


        if (_isSqueezing)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x * -0.1f * Time.deltaTime, this.transform.localScale.y * -0.1f * Time.deltaTime, this.transform.localScale.z * -0.1f * Time.deltaTime);
        }


        while (_elapsed <= changePosScript.transitionDuration)
        {
            _elapsed = _elapsed + Time.deltaTime;

            this.transform.position = Vector3.Lerp(this.transform.position, _secondPosition, _elapsed / changePosScript.transitionDuration);


            yield return null;



        }


        _isSqueezing = false;


    }

    void ParentThis(GameObject parent)
    {
        this.transform.parent = parent.transform;

        _isParented = true;


    }

    void UnparentThis()
    {

        if (this.transform.parent != null) this.transform.parent = null;

    }

    bool CheckForMovement(Rigidbody rb)
    {

        if (rb.velocity.magnitude > 0.1) return true;
        else return false;

    }



}

