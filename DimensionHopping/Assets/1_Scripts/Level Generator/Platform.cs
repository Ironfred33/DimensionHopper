using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{


    public enum CurrentAxis
    {

        noAxis,
        xPositive,
        zPositive,
        xNegative,
        zNegative
    }

    public bool firstPlatform;

    public CurrentAxis currentAxis;
}
