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


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Setup()
    {

    }



    private void OnTriggerEnter(Collider other)
    {

        if (other.transform.tag == "Squeezer")
        {

            //if (!_isSqueezing)
            //{
                changePosScript = other.transform.GetComponent<ChangePosition>();

                GetSecondPosition();

                _elapsed = changePosScript.elapsed;

                StartCoroutine(Squeeze());

                Debug.Log("SQUEEEZE");

            //}

        }
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.transform.tag == "Squeezer")
        {


        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Squeezer")
        {
            //changePosScript = null;

            Debug.Log("NON SQUEEZE");
        }
    }


    IEnumerator Squeeze()
    {

        _isSqueezing = true;

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




    void GetSecondPosition()
    {

        // 0 - xPos, 1 - xNeg
        // 2 - zPos, 3 - zNeg
        // 4 - yPos, 5 - yNeg


        switch (changePosScript.transitionDirIndex)
        {
            case 0:

                _secondPosition = new Vector3(changePosScript.secondPosition.x + this.transform.localScale.x, this.transform.position.y, this.transform.position.z);

                break;

            case 1:

                _secondPosition = new Vector3(changePosScript.secondPosition.x - this.transform.localScale.x, this.transform.position.y, this.transform.position.z);

                break;


            case 2:

                _secondPosition = new Vector3(changePosScript.secondPosition.x, this.transform.position.y, this.transform.position.z + this.transform.localScale.z);

                break;


            case 3:

                _secondPosition = new Vector3(changePosScript.secondPosition.x, this.transform.position.y, this.transform.position.z - this.transform.localScale.z);

                break;


            case 4:

                _secondPosition = new Vector3(changePosScript.secondPosition.x, this.transform.position.y + this.transform.localScale.y, this.transform.position.z);

                break;


            case 5:

                _secondPosition = new Vector3(changePosScript.secondPosition.x, this.transform.position.y - this.transform.localScale.y, this.transform.position.z);

                break;


        }
    }

}

