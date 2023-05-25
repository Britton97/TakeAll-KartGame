using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class State_Base : ScriptableObject
{
    //---Events---//
    [SerializeField] public UnityEvent onEnterEvent;
    [SerializeField] public UnityEvent onUpdateEvent;
    [SerializeField] public UnityEvent onFixedUpdateEvent;
    [SerializeField] public UnityEvent onExitEvent;
    //--Kart Controller and Input ---//
    [HideInInspector] public CharacterController controller;
    [HideInInspector] public Kart_Input input;
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

    public virtual void OnEnter(Rigidbody passedRB, GameObject pKartModel, GameObject pKartNormal, Kart_Input pInput, Kart_Stats pStats, Player_Stats pPlayerStats)
    {
        rb = passedRB;
        kartModel = pKartModel;
        tiltObject = pKartModel.transform.GetChild(0).gameObject;
        kartNormal = pKartNormal;
        input = pInput;
        kart_stats = pStats;
        player_stats = pPlayerStats;

        onEnterEvent.Invoke();
    }
    public virtual void OnUpdate() { onUpdateEvent.Invoke(); }
    public virtual void OnFixedUpdate() { onFixedUpdateEvent.Invoke(); }

    public virtual void OnExit(State_Base passIn)
    {
        onExitEvent.Invoke();

        KartController player = kartNormal.transform.parent.GetComponent<KartController>(); //get KartController
        player.CallOnEnterState(passIn); //call OnEnterState() on the new state
    }

    public virtual void AButton() { }
    public virtual void LeftStick() { }

    public virtual void Move() { }

    public virtual void Tilt()
    {
        Vector2 move = input.Kart_Controls.Move.ReadValue<Vector2>();
        float tilt = move.x * 10;
        tiltObject.transform.localEulerAngles = new Vector3(tiltObject.transform.localEulerAngles.x, tiltObject.transform.localEulerAngles.y, -tilt);
    }
}