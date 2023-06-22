using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInstance : MonoBehaviour
{
    [SerializeField] Ground_State state;
    Ground_State stateInstance;
    // Start is called before the first frame update
    void Start()
    {
        //create a new instance of the scriptable object
        //with all the settings from the original
        stateInstance = Instantiate(state);
        //stateInstance.TestEvent.Invoke();
        //state.TestEvent.Invoke();
        Debug.Log(state.onFixedUpdateEvent.GetPersistentEventCount());
        Debug.Log(stateInstance.onFixedUpdateEvent.GetPersistentEventCount());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
