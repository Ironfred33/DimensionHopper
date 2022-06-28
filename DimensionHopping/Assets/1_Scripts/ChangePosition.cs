using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePosition : MonoBehaviour
{
    public float transitionDuration;
    public float transitionLength;

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

    [SerializeField] private Vector3 _firstPosition;
    [SerializeField] private Vector3 _secondPosition;
    private float _elapsed;
    [SerializeField] private bool _transitioned;
    [SerializeField] private bool _transitionInProgress;


    // Start is called before the first frame update
    void Start()
    {


        Setup();

    }

    private void Setup()
    {

        _firstPosition = this.transform.position;

        GetSecondPosition();

    }

    private void Update()
    {

        if (!_transitionInProgress)
        {

            if (Input.GetKeyDown(transitionButton) && !_transitioned)
            {
                StartCoroutine(Transition(_firstPosition, _secondPosition, transitionDuration));
            }

            if (Input.GetKeyDown(transitionButton) && _transitioned)
            {
                StartCoroutine(Transition(_secondPosition, _firstPosition, transitionDuration));
            }


        }


    }






    public IEnumerator Transition(Vector3 currentPosition, Vector3 newPosition, float transitionDuration)
    {
        _elapsed = 0f;


        while (_elapsed <= transitionDuration)
        {
            _elapsed = _elapsed + Time.deltaTime;

            this.transform.position = Vector3.Lerp(currentPosition, newPosition, _elapsed / transitionDuration);

            _transitionInProgress = true;

            yield return null;



        }

        _transitionInProgress = false;
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

                _secondPosition = new Vector3(_firstPosition.x + transitionLength, _firstPosition.y, _firstPosition.z);

                break;

            case (TransitionDirection.xNegative):

                _secondPosition = new Vector3(_firstPosition.x - transitionLength, _firstPosition.y, _firstPosition.z);

                break;

            case (TransitionDirection.zPositive):

                _secondPosition = new Vector3(_firstPosition.x, _firstPosition.y, _firstPosition.z + transitionLength);

                break;

            case (TransitionDirection.zNegative):

                _secondPosition = new Vector3(_firstPosition.x, _firstPosition.y, _firstPosition.z - transitionLength);

                break;

            case (TransitionDirection.yPositive):

                _secondPosition = new Vector3(_firstPosition.x, _firstPosition.y + transitionLength, _firstPosition.z);

                break;

            case (TransitionDirection.yNegative):

                _secondPosition = new Vector3(_firstPosition.x, _firstPosition.y - transitionLength, _firstPosition.z);

                break;
        }
    }

}



