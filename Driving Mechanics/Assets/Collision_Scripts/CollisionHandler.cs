using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] List<LayerMask> interactableLayers = new List<LayerMask>();
    //[SerializeField] DataReference onCollisionData;

    private void OnCollisionEnter(Collision collision)
    {
        if (interactableLayers.Contains(collision.gameObject.layer))
        {
            //onCollisionData.dataEvent.Invoke(onCollisionData);
        }
    }
}
