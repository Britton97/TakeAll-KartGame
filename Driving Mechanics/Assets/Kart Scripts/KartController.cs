using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(KartTag))]
public class KartController : MonoBehaviour
{
    //---Input and Rigidbody---//
    [SerializeField] private Rigidbody rb;
    private Kart_Input input;
    //---Kart Objects---//
    [SerializeField] private GameObject kartNormal;
    [SerializeField] private GameObject kartModel;
    [SerializeField] private GameObject tiltObject;
    [SerializeField] private GameObject lookAtObject;
    //---State Machine---//
    //[SerializeField] private KartState startingState;
    [SerializeField] private State_Base currentState;
    [SerializeField] private List<State_Base> dataStates;
    [SerializeField] private Dictionary<KartState, State_Base> stateDictionary = new Dictionary<KartState, State_Base>();
    //---Kart Stats---//
    [SerializeField] Kart_Stats kart_stats;
    [SerializeField] Player_Stats player_stats;
    //---Events---//
    public UnityEvent onAwake;
    [SerializeField] DataGameObject cinemachineCam;

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
        CreateStateSOs();
        onAwake.Invoke();
        player_stats.ResetStats();

        currentState.OnEnter(rb, kartModel, kartNormal, tiltObject, input, kart_stats, player_stats);
    }
    public void CreateStateSOs()
    {
        foreach (State_Base state in dataStates)
        {
            //create instance of state
            State_Base instance = Instantiate(state);
            KartState s = instance.stateType;
            //add instance and s to dictionary
            stateDictionary.Add(s, instance);
        }

        KartState currentS = currentState.stateType;
        currentState = stateDictionary[currentS];

    } 
    public void CallOnEnterState(KartState passIn)
    {
        //currentState equals the state that matches the enum
        currentState = stateDictionary[passIn];
        currentState.OnEnter(rb, kartModel, kartNormal, tiltObject, input, kart_stats, player_stats);
    }

    // Update is called once per frame
    void Update() { currentState.OnUpdate(); }
    void FixedUpdate() { currentState.OnFixedUpdate(); }

    private void LateUpdate()
    {
        transform.position = rb.transform.position; //keep this        
    }

    public void SetCameraFollow()
    {
        CinemachineFreeLook cinemachineFreeLook = cinemachineCam.DataValue.gameObject.GetComponent<CinemachineFreeLook>();
        cinemachineFreeLook.Follow = kartModel.transform;
        cinemachineFreeLook.LookAt = lookAtObject.transform;
    }

    public void FindPlayerRigidbody()
    {
        StartCoroutine(WaitFrame());
    }

    private IEnumerator WaitFrame()
    {
        yield return new WaitForEndOfFrame();
        Transform parent = transform.parent;
        //Debug.Log("Parent is " + parent.name);

        foreach(Transform child in parent)
        {
            if (child.gameObject.tag == "Player Collider")
            {
                Debug.Log("Found the player collider");
                rb = child.gameObject.GetComponent<Rigidbody>();
            }
        }
    }
}
