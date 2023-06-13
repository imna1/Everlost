using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    [HideInInspector] public List<WayPoint> Neighbours = new List<WayPoint>();
    [HideInInspector] public WayPoint prevPoint = null;
    [HideInInspector] public float Distance = 999999;
    [HideInInspector] public bool IsAnalysed = false;
    public bool IsStatic;
}
