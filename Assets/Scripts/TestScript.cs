using System;
using Runtime.Utility;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private void OnDrawGizmos() {
        HandlesProxy.DrawCapsule(transform.position, 2, 0.5f, 3, Color.green);
    }
}
