using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scriptable Object für Animationskurven

[CreateAssetMenu(fileName = "New Curve", menuName = "Curve")] 
public class AnimCurve : ScriptableObject
{
    public AnimationCurve curve;

}
