using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    [SerializeField] StatType statType;
    [SerializeField] C_CollectItemBeh c_CollectItemBeh;
    [SerializeField] MeshRenderer itemMesh;
    [SerializeField] ParticleSystem idle;
    [SerializeField] ParticleSystem burst;
    [SerializeField] FloatReference onCollisionData;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.gameObject.name} hit me");
        if (c_CollectItemBeh.CheckInterface(other.gameObject) != null)
        {
            itemMesh.enabled = false;
            idle.Stop();
            burst.Play();
            Debug.Log("hit");
            CollectItemBeh itemCollector = c_CollectItemBeh.CheckInterface(other.gameObject) as CollectItemBeh;
            itemCollector.PickUpItem(statType, onCollisionData.Value);
            burst.transform.parent = other.transform;
            StartCoroutine(DestroyItem());
            //Destroy(gameObject, 2);
        }
    }

    private IEnumerator DestroyItem()
    {
        yield return new WaitForSeconds(2);
        Destroy(burst.gameObject);
        Destroy(gameObject);
    }
}
