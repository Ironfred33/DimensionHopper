using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{


    public enum CurrentAxis
    {

        NoAxis,
        XPositive,
        ZPositive,
        XNegative,
        ZNegative
    }

    public bool firstPlatform;

    public CurrentAxis currentAxis;
}
