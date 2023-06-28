using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(KartTag))]
public class KartController : MonoBehaviour, ICollisionHandlerable
{
    [SerializeField] PlayerInfoGetter playerInfoGetter;
    //---Input and Rigidbody---//
    [SerializeField] private Rigidbody colliderBall;
    private Kart_Input input;
    //---Kart Objects---//
    [SerializeField] private GameObject kartNormal;
    [SerializeField] private GameObject modelHolder;
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
    public UnityEvent onLeaveKart;
    public UnityEvent onEnterKart;
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

        if (playerInfoGetter != null)
        {
            colliderBall = playerInfoGetter.GetColliderBall();
        }
        else
        {
            onLeaveKart.Invoke();
        }

        currentState.OnEnter(colliderBall, modelHolder, kartNormal, tiltObject, input, kart_stats, player_stats);
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
        currentState.OnEnter(colliderBall, modelHolder, kartNormal, tiltObject, input, kart_stats, player_stats);
    }


    Vector2 move;
    void Update()
    {
        move = input.Kart_Controls.Move.ReadValue<Vector2>();
        currentState.OnUpdate();
    }

    void FixedUpdate() { currentState.OnFixedUpdate(); }

    private void LateUpdate()
    {
        transform.position = colliderBall.transform.position; //keep this        
    }

    public void SetCameraFollow()
    {
        CinemachineFreeLook cinemachineFreeLook = cinemachineCam.DataValue.gameObject.GetComponent<CinemachineFreeLook>();
        cinemachineFreeLook.Follow = modelHolder.transform;
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
                colliderBall = child.gameObject.GetComponent<Rigidbody>();
            }
        }
    }

    public void OnLeaveKartEvent()
    {
        onLeaveKart.Invoke();
        if(playerInfoGetter != null)
        {
            playerInfoGetter.PlayerExitKart();
        }
    }

    public void CollisionHandler(GameObject hitObject, GameObject hittingObject)
    {
        this.enabled = true;
        onEnterKart.Invoke();
        //CallOnEnterState(KartState.Ground);
    }
}
