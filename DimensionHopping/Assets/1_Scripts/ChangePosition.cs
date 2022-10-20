using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePosition : MonoBehaviour
{
    public float transitionDuration;
    public float transitionLength;

    public int transitionDirIndex;

    public enum TransitionDirection
    {
        xPositive,
        xNegative,
        zPositive,
        zNegative,
        yPositive,
        yNegative
    }

    public TransitionDirection transitionDir;

    public KeyCode transitionButton;

    [HideInInspector] public Vector3 firstPosition;
    [HideInInspector] public Vector3 secondPosition;
    public float elapsed;
    [SerializeField] private bool _transitioned;
    public bool transitionInProgress;


    // Start is called before the first frame update
    void Start()
    {


        Setup();

    }

    private void Setup()
    {

        firstPosition = this.transform.position;

        GetSecondPosition();

    }

    private void Update()
    {

        if (!transitionInProgress)
        {

            if (Input.GetKeyDown(transitionButton) && !_transitioned)
            {
                StartCoroutine(Transition(firstPosition, secondPosition, transitionDuration));
            }

            if (Input.GetKeyDown(transitionButton) && _transitioned)
            {
                StartCoroutine(Transition(secondPosition, firstPosition, transitionDuration));
            }


        }


    }






    public IEnumerator Transition(Vector3 currentPosition, Vector3 newPosition, float transitionDuration)
    {
        elapsed = 0f;


        while (elapsed <= transitionDuration)
        {
            elapsed = elapsed + Time.deltaTime;

            this.transform.position = Vector3.Lerp(currentPosition, newPosition, elapsed / transitionDuration);

            transitionInProgress = true;

            yield return null;



        }

        transitionInProgress = false;
        if (!_transitioned) _transitioned = true;
        else if (_transitioned) _transitioned = false;





    }


    public IEnumerator ReverseSqueezing()
    {


        _transitioned = false;

        return null;
    }





    void GetSecondPosition()
    {
        switch (transitionDir)
        {
            case (TransitionDirection.xPositive):

                secondPosition = new Vector3(firstPosition.x + transitionLength, firstPosition.y, firstPosition.z);

                transitionDirIndex = 0;

                break;

            case (TransitionDirection.xNegative):

                secondPosition = new Vector3(firstPosition.x - transitionLength, firstPosition.y, firstPosition.z);

                transitionDirIndex = 1;

                break;

            case (TransitionDirection.zPositive):

                secondPosition = new Vector3(firstPosition.x, firstPosition.y, firstPosition.z + transitionLength);

                transitionDirIndex = 2;

                break;

            case (TransitionDirection.zNegative):

                secondPosition = new Vector3(firstPosition.x, firstPosition.y, firstPosition.z - transitionLength);

                transitionDirIndex = 3;

                break;

            case (TransitionDirection.yPositive):

                secondPosition = new Vector3(firstPosition.x, firstPosition.y + transitionLength, firstPosition.z);

                transitionDirIndex = 4;

                break;

            case (TransitionDirection.yNegative):

                secondPosition = new Vector3(firstPosition.x, firstPosition.y - transitionLength, firstPosition.z);

                transitionDirIndex = 5;

                break;
        }
    }

}



