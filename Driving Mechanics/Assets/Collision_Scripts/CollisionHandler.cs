using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] public InterfaceChecker interfaceChecker;
    [SerializeField] public UnityEvent<GameObject, GameObject> CollisionHandlerEvent;
    [SerializeField] public UnityEvent onPassedInterfaceCheck;


    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log($"Collision with {collision.gameObject.name}");
        if(interfaceChecker.CheckInterface(collision.gameObject) != null)
        {
            onPassedInterfaceCheck.Invoke();
            CollisionHandlerEvent.Invoke(this.gameObject ,collision.gameObject);
        }
    }
}

public interface ICollisionHandlerable
{
    void CollisionHandler(GameObject hitObject, GameObject hittingObject);
}