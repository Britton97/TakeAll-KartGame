using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Other : MonoBehaviour
{
    private Kart_Input input;
    [SerializeField] private float forceAmount;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private int health = 3;
    [SerializeField] private MeshRenderer myMesh;
    [SerializeField] private Collider myCollider;

    #region OnEnable/OnDisable
    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
    #endregion

    private void Awake()
    {
        input = new Kart_Input();
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.rigidbody.velocity = Vector3.zero;
        Vector3 dir = transform.position - collision.transform.position;
        VelocityGetter otherSpeed = collision.transform.GetComponent<VelocityGetter>();
        if (otherSpeed != null)
        {
            dir.Normalize();
            collision.rigidbody.AddForce(-dir * forceAmount * 1000, ForceMode.Impulse);
            //Debug.Log("Called");
            TakeDamage(1);
            GetComponent<AnimationBehavior>().PlayAnimation();
        }
    }

    private void TakeDamage(int passIn)
    {
        health -= passIn;
        if(health == 0)
        {
            myCollider.enabled = false;
            myMesh.enabled = false;
        }
    }
}
