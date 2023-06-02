using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Kart States/Flying", fileName = "Flying State")]
public class Air_State : State_Base
{
    [Header("Exit State Condition")]
    [SerializeField] private float rayDistance;
    [SerializeField] private float applyGravityRayDistance;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private State_Base onTouchGround;
    [SerializeField] private float xTurnSpeedMultiplier = 2f;
    [SerializeField] private float yTurnSpeedMultiplier = 2f;

    private float currentSpeed;
    private float elaspedTime;


    public override void OnEnter(Rigidbody passedRB, GameObject pKartModel, GameObject pKartNormal, Kart_Input pInput, Kart_Stats pStats, Player_Stats pPlayerStats)
    {
        base.OnEnter(passedRB, pKartModel, pKartNormal, pInput, pStats, pPlayerStats);
        elaspedTime = 0;
        Debug.Log("Flying State");
    }
    public override void AButton()
    {
        float button = input.Kart_Controls.ActionButton.ReadValue<float>(); //read value of A button
        if (button == 1) //if 'A' button held down execute below
        {
            ApplyGravity();
        }
    }

    public void ApplyGravity()
    {
        RaycastHit hit;

        if (!Physics.Raycast(kartNormal.transform.position, Vector3.down, out hit, applyGravityRayDistance, groundLayer))
        {
            rb.AddForce(Vector3.up * gravity * gravityMultiplier * Time.deltaTime, ForceMode.Acceleration);
        }
    }

    public void GroundCheck()
    {
        if (Physics.Raycast(kartNormal.transform.position, Vector3.down, rayDistance, groundLayer))
        {
            //Debug.Log("Exiting Air");
            OnExit(onTouchGround);
        }
    }

    public void GroundPull()
    {
        if (elaspedTime <= kart_stats.flightTime)
        {
            elaspedTime += Time.deltaTime;
        }

        float gravityPercentage = elaspedTime / kart_stats.flightTime;
        float curve = kart_stats.flightCurve.Evaluate(gravityPercentage);
        //Debug.Log($"{gravityPercentage}% = {curve}");
        rb.AddForce(Vector3.up * gravity * (gravityMultiplier * curve));
    }

    public override void LeftStick()
    {
        Vector2 move = input.Kart_Controls.Move.ReadValue<Vector2>();

        float x = kartModel.transform.eulerAngles.x;
        float y = kartModel.transform.eulerAngles.y;
        x = x + (move.y * (kart_stats.turnSpeed * player_stats.turn) * xTurnSpeedMultiplier * Time.deltaTime);
        y = y + (move.x * (kart_stats.turnSpeed * player_stats.turn) * yTurnSpeedMultiplier * Time.deltaTime);
        kartModel.transform.rotation = Quaternion.Euler(x, y, 0);
    }

    public override void Move()
    {
        float button = input.Kart_Controls.ActionButton.ReadValue<float>();

        if (button == 0)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, kart_stats.topSpeed * player_stats.topSpeed, Time.deltaTime * kart_stats.accelrateRate);
        }
        rb.AddForce(kartModel.transform.forward * currentSpeed * kart_stats.forceMultiplier.DataValue);
    }
}
