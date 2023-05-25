using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KartController : MonoBehaviour
{
    //---Input and Rigidbody---//
    [SerializeField] private Rigidbody rb;
    private Kart_Input input;
    //---Kart Objects---//
    [SerializeField] private GameObject kartNormal;
    [SerializeField] private GameObject kartModel;
    //---State Machine---//
    [SerializeField] public State_Base currentState;
    //---Kart Stats---//
    [SerializeField] Kart_Stats kart_stats;
    [SerializeField] Player_Stats player_stats;
    //---Events---//
    public UnityEvent onAwake;

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
        onAwake.Invoke();
        player_stats.ResetStats();
        currentState.OnEnter(rb, kartModel, kartNormal, input, kart_stats, player_stats);
    }
    public void CallOnEnterState(State_Base passIn)
    {
        currentState = passIn;
        currentState.OnEnter(rb, kartModel, kartNormal, input, kart_stats, player_stats);
    }

    // Update is called once per frame
    void Update() { currentState.OnUpdate(); }
    void FixedUpdate() { currentState.OnFixedUpdate(); }

    private void LateUpdate()
    {
        transform.position = rb.transform.position; //keep this        
    }
}
