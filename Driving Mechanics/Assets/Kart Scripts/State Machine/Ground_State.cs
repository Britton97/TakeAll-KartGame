using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Kart States/Ground", fileName = "Ground State")]
public class Ground_State : State_Base
{
    [Header("Game Actions")]
    [SerializeField] private GameAction leaveKartAction;
    [Header("Ground State Variables")]
    [SerializeField] private float rayDistance = 1f;
    [SerializeField] private float applyGravityRayDistance;
    [SerializeField] private float wallRayDistance;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private State_Base onTouchAir;
    [SerializeField] private AnimationCurve turnCurve;
    private float currentSpeed;

    [Header("Wave Effect Settings")]
    [SerializeField] private GameObject waveEffectPrefab;
    private GameObject waveEffect;
    [SerializeField] private Vector3 waveOffset = new Vector3(0,0,1.8f);
    private WaveController waveController;

    public override void OnEnter(Rigidbody passedRB, GameObject pKartModel, GameObject pKartNormal, GameObject pTiltObject, Kart_Input pInput, Kart_Stats pStats, Player_Stats pPlayerStats)
    {
        base.OnEnter(passedRB, pKartModel, pKartNormal, pTiltObject, pInput, pStats, pPlayerStats);


        if(passedRB == null)
        {
            Debug.LogError($"nothing was passed");
        }

        kartModel.transform.localEulerAngles = new Vector3(0, kartModel.transform.localEulerAngles.y, kartModel.transform.localEulerAngles.z);
        currentSpeed = 0f;
        if (waveEffect == null)
        {
            waveEffect = Instantiate(waveEffectPrefab, kartModel.transform.position, Quaternion.identity);
            waveEffect.transform.parent = kartModel.transform;
            waveEffect.transform.localPosition = waveOffset;
            waveEffect.transform.localRotation = Quaternion.Euler(new Vector3(0,0,0));
            waveController = waveEffect.GetComponent<WaveController>();
            waveEffect.SetActive(true);
        }

        waveEffect.SetActive(true);
        kart_stats.canAffectCharge = true;
        //Debug.Log("Ground State");
    }

    public override void OnExit(KartState passIn)
    {
        base.OnExit(passIn);
        waveEffect.SetActive(false);
    }

    #region Idle Sine Wave
    public float frequency = 1f;
    public float amplitude = 1f;
    public float offset = 0f;
    private float time = 0f;
    public bool thing = false;
    public void BoardIdleSine()
    {
        time += Time.deltaTime;

        float y = amplitude * Mathf.Sin(2f * Mathf.PI * frequency * time);

        y = Mathf.Clamp(y, -amplitude, amplitude);

        tiltObject.transform.localPosition = new Vector3(tiltObject.transform.localPosition.x, y + amplitude + offset, tiltObject.transform.localPosition.z);
    }
    #endregion

    public void AirCheck()
    {
        Debug.DrawRay(kartNormal.transform.position, Vector3.down * rayDistance, Color.red);
        if (!Physics.Raycast(kartNormal.transform.position, Vector3.down, rayDistance, groundLayer))
        {
            Debug.Log("Exiting Ground");
            OnExit(KartState.Flying);
        }
    }

    private float rotate;
    private float currentRotate;
    public override void LeftStick()
    {
        Vector2 move = input.Kart_Controls.Move.ReadValue<Vector2>();
        rotate = move.x * (kart_stats.turnSpeed * player_stats.turn);
        currentRotate = Mathf.Lerp(currentRotate, rotate, Time.deltaTime * 4f);
        float turnLimiter = Mathf.Abs(currentSpeed / (kart_stats.topSpeed * player_stats.topSpeed));
        currentRotate = currentRotate * turnCurve.Evaluate(turnLimiter);
        rotate = 0f;

        kartModel.transform.localEulerAngles = Vector3.Lerp(kartModel.transform.localEulerAngles, new Vector3(0f, kartModel.transform.localEulerAngles.y + currentRotate, 0), Time.deltaTime * 4f);

        waveController.ReceiveVector2Input(move);
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
            Vector2 move = input.Kart_Controls.Move.ReadValue<Vector2>();

            currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime * (kart_stats.decelerateRate * player_stats.weight));

            if (currentSpeed < 0) { currentSpeed = 0; }

            if (move.y < -.95)
            {
                OnExit(KartState.Ground);
                kartController.OnLeaveKartEvent();
            }
            else if (move.y > 0.25f)
            {
                currentSpeed = kart_stats.scootSpeed.DataValue;
            }
        }
    }

    public override void Move()
    {
        float button = input.Kart_Controls.ActionButton.ReadValue<float>();
        float configureSpeed = (kart_stats.boostValue.DataValue * kart_stats.boostMultiplier) + 1;

        if (button == 0)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, kart_stats.topSpeed * player_stats.topSpeed, Time.deltaTime * kart_stats.accelrateRate);
        }
        else
        {
            configureSpeed = 1;
        }

        //test
        waveController.WaveLength = ((currentSpeed / kart_stats.topSpeed * player_stats.topSpeed) + 1) * 4;
        //test
        rb.AddForce(kartModel.transform.forward * ((currentSpeed * configureSpeed * kart_stats.forceMultiplier.DataValue)));
    }

    public void WallCheck()
    {
        RaycastHit hit;

        Debug.DrawRay(kartNormal.transform.position, kartModel.transform.forward * wallRayDistance, Color.green);
        if (Physics.Raycast(kartNormal.transform.position, kartModel.transform.forward, out hit, wallRayDistance, groundLayer))
        {
            currentSpeed = 0;
        }
    }
}
