using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityGetter : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] public DataFloat speed;

    private void Update()
    {
        speed.DataValue = rb.velocity.magnitude;
    }
}
