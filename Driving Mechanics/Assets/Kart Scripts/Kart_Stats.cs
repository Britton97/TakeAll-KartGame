using UnityEngine;

[CreateAssetMenu(menuName = "Kart Stats", fileName = "New Kart Stats")]
public class Kart_Stats : ScriptableObject
{
    [Header("Kart Movement Variables")]
    [SerializeField] public float decelerateRate;
    [SerializeField] public float topSpeed;
    [SerializeField] public DataFloat scootSpeed;
    [SerializeField] public float accelrateRate;
    [SerializeField] public DataFloat forceMultiplier;
    [Header("Boost Variables")]
    [SerializeField] public DataFloat boostValue;
    [SerializeField] public float boostMultiplier;
    [Tooltip("this is a function much of how much boost the player will get at the percentage of boost completion")]
    [SerializeField] public AnimationCurve boostCurve;
    [SerializeField] public float chargeTime;
    [SerializeField] public float dechargeTime;
    [Header("Turn Variables")]
    [SerializeField] public float turnSpeed = 1f;
    [SerializeField] public float turnSpeedMultiplier = 1f;
    [Header("Flight Variables")]
    [SerializeField] public float flightTime;
    [SerializeField] public AnimationCurve flightCurve;
}
