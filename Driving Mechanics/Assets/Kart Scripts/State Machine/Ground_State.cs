using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Kart States/Ground", fileName = "Ground State")]
public class Ground_State : State_Base
{
    [SerializeField] private float rayDistance = 1f;
    [SerializeField] private float applyGravityRayDistance;
    [SerializeField] private float wallRayDistance;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private State_Base onTouchAir;
    [SerializeField] private AnimationCurve turnCurve;
    private float currentSpeed;
    private float rotate;
    private float currentRotate;
    [SerializeField] private GameObject particleEffectPrefab;
    private GameObject particleEffect;

    public override void OnEnter(Rigidbody passedRB, GameObject pKartModel, GameObject pKartNormal, Kart_Input pInput, Kart_Stats pStats, Player_Stats pPlayerStats)
    {
        base.OnEnter(passedRB, pKartModel, pKartNormal, pInput, pStats, pPlayerStats);

        kartModel.transform.eulerAngles = new Vector3(0,kartModel.transform.eulerAngles.y,kartModel.transform.eulerAngles.z);
        currentSpeed= 0f;
        if (particleEffect == null)
        {
            particleEffect = Instantiate(particleEffectPrefab, kartNormal.transform.position, Quaternion.identity);
            particleEffect.transform.parent = kartNormal.transform;
            particleEffect.SetActive(false);
        }
        Debug.Log("Ground State");
    }

    public override void OnExit(State_Base passIn)
    {
        base.OnExit(passIn);
        particleEffect.SetActive(false);
    }

    public void AirCheck()
    {
        Debug.DrawRay(kartNormal.transform.position, Vector3.down * rayDistance, Color.red);
        if (!Physics.Raycast(kartNormal.transform.position, Vector3.down, rayDistance, groundLayer))
        {
            //Debug.Log("Exiting Ground");
            OnExit(onTouchAir);
        }
    }

    public override void LeftStick()
    {
        Vector2 move = input.Kart_Controls.Move.ReadValue<Vector2>();
        rotate = move.x * (kart_stats.turnSpeed * player_stats.turn);
        currentRotate = Mathf.Lerp(currentRotate, rotate, Time.deltaTime * 4f);
        float turnLimiter = Mathf.Abs(currentSpeed / (kart_stats.topSpeed * player_stats.topSpeed));
        currentRotate = currentRotate * turnCurve.Evaluate(turnLimiter);
        rotate = 0f;

        kartModel.transform.localEulerAngles = Vector3.Lerp(kartModel.transform.localEulerAngles, new Vector3(0f, kartModel.transform.localEulerAngles.y + currentRotate, 0), Time.deltaTime * 4f);
        if(Mathf.Abs(move.x) > 0)
        {
            particleEffect.SetActive(true);
            particleEffect.transform.rotation = kartModel.transform.localRotation;
        }
        else
        {
            particleEffect.SetActive(false);
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

    public void MatchNormal()
    {
        RaycastHit hitNear;
        Physics.Raycast(kartNormal.transform.position, Vector3.down, out hitNear, rayDistance);
        kartNormal.transform.up = Vector3.Lerp(kartNormal.transform.up, hitNear.normal, Time.deltaTime * 8.0f);
        kartNormal.transform.Rotate(0, kartNormal.transform.parent.transform.eulerAngles.y, 0);
    }

    public override void AButton()
    {
        float button = input.Kart_Controls.ActionButton.ReadValue<float>();

        if (button == 1)
        {
            //currentSpeed -= kart_stats.decelerateRate * player_stats.weight * Time.deltaTime;
            currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime * (kart_stats.decelerateRate * player_stats.weight) * currentSpeed);
            if (currentSpeed < 0) { currentSpeed = 0; }
        }
    }

    public override void Move()
    {
        float button = input.Kart_Controls.ActionButton.ReadValue<float>();

        if (button == 0)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, kart_stats.topSpeed * player_stats.topSpeed, Time.deltaTime * kart_stats.accelrateRate);
        }
        float configureSpeed = (kart_stats.boostValue.DataValue * kart_stats.boostMultiplier) + 1;
        rb.AddForce(kartModel.transform.forward * ((currentSpeed * configureSpeed * kart_stats.forceMultiplier.DataValue)));
    }

    public void WallCheck()
    {
        RaycastHit hit;

        Debug.DrawRay(kartNormal.transform.position, kartModel.transform.forward * wallRayDistance, Color.green);
        if (Physics.Raycast(kartNormal.transform.position, kartModel.transform.forward, out hit, wallRayDistance, groundLayer))
        {
            currentSpeed = 0;
            Debug.Log("Wall Hit");
        }
    }
}
