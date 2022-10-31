using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squeezable : MonoBehaviour
{

    public float maximumSqueeze;
    public float squeezeSpeed;
    public float squeezeAmount;

    private float _elapsed;
    private bool _isSqueezing;

    public ChangePosition changePosScript;

    private Vector3 _secondPosition;

    private Rigidbody _rb;

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
         if(changePosScript != null)
        {
            if(!changePosScript.transitionInProgress)
            {
                if(!CheckForMovement(_rb) && _isParented) 
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

                // GetSecondPosition();

                // _elapsed = changePosScript.elapsed;

                

                // Debug.Log("SQUEEEZE");

            }

        }
    }





    IEnumerator Squeeze()
    {

        _isSqueezing = true;


        if(_isSqueezing)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x * -0.1f * Time.deltaTime, this.transform.localScale.y * -0.1f * Time.deltaTime,  this.transform.localScale.z * -0.1f * Time.deltaTime ); 
        }
        
        //_elapsed = 0f;

        while (_elapsed <= changePosScript.transitionDuration)
        {
            _elapsed = _elapsed + Time.deltaTime;

            this.transform.position = Vector3.Lerp(this.transform.position, _secondPosition, _elapsed / changePosScript.transitionDuration);

            // _transitionInProgress = true;

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

        if(rb.velocity.magnitude > 0.1) return true;
        else return false;

    }




    // void GetSecondPosition()
    // {

    //     // 0 - xPos, 1 - xNeg
    //     // 2 - zPos, 3 - zNeg
    //     // 4 - yPos, 5 - yNeg


    //     switch (changePosScript.transitionDirIndex)
    //     {
    //         case 0:

    //             _secondPosition = new Vector3(changePosScript.secondPosition.x + this.transform.localScale.x, this.transform.position.y, this.transform.position.z);

    //             break;

    //         case 1:

    //             _secondPosition = new Vector3(changePosScript.secondPosition.x - this.transform.localScale.x, this.transform.position.y, this.transform.position.z);

    //             break;


    //         case 2:

    //             _secondPosition = new Vector3(changePosScript.secondPosition.x, this.transform.position.y, this.transform.position.z + this.transform.localScale.z);

    //             break;


    //         case 3:

    //             _secondPosition = new Vector3(changePosScript.secondPosition.x, this.transform.position.y, this.transform.position.z - this.transform.localScale.z);

    //             break;


    //         case 4:

    //             _secondPosition = new Vector3(changePosScript.secondPosition.x, this.transform.position.y + this.transform.localScale.y, this.transform.position.z);

    //             break;


    //         case 5:

    //             _secondPosition = new Vector3(changePosScript.secondPosition.x, this.transform.position.y - this.transform.localScale.y, this.transform.position.z);

    //             break;


    //     }
    // }

  

}

