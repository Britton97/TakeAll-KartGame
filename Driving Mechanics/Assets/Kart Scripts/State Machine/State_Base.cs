using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class State_Base : ScriptableObject
{
    //---State---//
    [Header("State Type")]
    public KartState stateType;
    //---Events---//
    [SerializeField] public UnityEvent onEnterEvent;
    [SerializeField] public UnityEvent onUpdateEvent;
    [SerializeField] public UnityEvent onFixedUpdateEvent;
    [SerializeField] public UnityEvent onExitEvent;
    //--Kart Controller and Input ---//

    [HideInInspector] public Kart_Input input;
    [HideInInspector] public KartController kartController;
    //---Kart GameObjects---//
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public GameObject kartNormal;
    [HideInInspector] public GameObject kartModel;
    [HideInInspector] public GameObject tiltObject;
    [HideInInspector] public Player_Stats player_stats;
    //---Kart Stats---//
    [SerializeField] public Kart_Stats kart_stats;
    [SerializeField] public float gravity = -9.81f;
    [SerializeField] public float gravityMultiplier = 1000f;

    public virtual void OnEnter(Rigidbody passedRB, GameObject pKartModel, GameObject pKartNormal, GameObject pTiltObject, Kart_Input pInput, Kart_Stats pStats, Player_Stats pPlayerStats)
    {
        rb = passedRB;
        kartModel = pKartModel;
        tiltObject = pTiltObject;
        kartNormal = pKartNormal;
        kartController = kartNormal.transform.parent.GetComponent<KartController>(); //get KartController
        input = pInput;
        kart_stats = pStats;
        player_stats = pPlayerStats;

        onEnterEvent.Invoke();
    }
    public virtual void OnUpdate() { onUpdateEvent.Invoke(); }
    public virtual void OnFixedUpdate() { onFixedUpdateEvent.Invoke(); }

    public virtual void OnExit(KartState passIn)
    {
        onExitEvent.Invoke();

        kartController.CallOnEnterState(passIn); //call OnEnterState() on the new state
    }

    public virtual void AButton() { }
    public virtual void LeftStick() { }

    public virtual void Move() { }

    private float currentTiltX;
    private float currentTiltY;
    public virtual void Tilt()
    {
        Vector2 move = input.Kart_Controls.Move.ReadValue<Vector2>();
        float tiltX = move.x * 20; //20
        float tiltY = move.y * 20; //10
        currentTiltX = Mathf.Lerp(currentTiltX, tiltX, Time.deltaTime * 8f);
        currentTiltY = Mathf.Lerp(currentTiltY, tiltY, Time.deltaTime * 8f);
        tiltObject.transform.localEulerAngles = new Vector3(currentTiltY, tiltObject.transform.localEulerAngles.y, -currentTiltX);
    }
}