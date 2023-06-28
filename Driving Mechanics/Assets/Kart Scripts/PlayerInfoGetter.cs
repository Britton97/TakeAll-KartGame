using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInfoGetter : MonoBehaviour
{
    [SerializeField] public PlayerController playerController;
    [SerializeField] private Rigidbody colliderBall;
    [SerializeField] public KartController kartController;
    [SerializeField] private UnityEvent onEnterKart;
    [SerializeField] private UnityEvent onExitKart;

    public void PlayerExitKart()
    {
        Debug.Log("EXIT");
        onExitKart.Invoke();
    }

    public void PlayerEnterKart()
    {
        Debug.Log("ENTER");
        onEnterKart.Invoke();
    }

    public Rigidbody GetColliderBall()
    {
        return colliderBall;
    }
}
