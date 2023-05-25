using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    [SerializeField] FloatReference onCollisionData;
    [SerializeField] StatType statType;
    [SerializeField] C_CollectItemBeh c_CollectItemBeh;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.gameObject.name} hit me");
        if (c_CollectItemBeh.CheckInterface(other.gameObject) != null)
        {
            Debug.Log("hit");
            CollectItemBeh itemCollector = c_CollectItemBeh.CheckInterface(other.gameObject) as CollectItemBeh;
            itemCollector.PickUpItem(statType, onCollisionData.Value);
            Destroy(gameObject);
        }
    }
}
