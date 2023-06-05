using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeMeter : MonoBehaviour
{
    [SerializeField] private Slider boostSlider;
    [SerializeField] private Kart_Stats kartStats;
    [SerializeField] private Player_Stats playerStats;
    
    private Kart_Input input;
    private float button;
    private float finishedPercentage = 0;
    private float chargeTime;
    private float dechargeTime;

    #region OnEnable/OnDisable/Awake
    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
    private void Awake()
    {
        input = new Kart_Input();
    }

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        kartStats.boostValue.DataValue = 0f;
        chargeTime = 1 / kartStats.chargeTime;
        dechargeTime = 1 / kartStats.dechargeTime;
        boostSlider.maxValue = 1;
        boostSlider.value = kartStats.boostValue.DataValue;
    }

    // Update is called once per frame
    void Update()
    {
        if(!kartStats.canAffectCharge) { UpdateMeter(); return;}
        button = input.Kart_Controls.ActionButton.ReadValue<float>(); //read value of A button
        if (button == 1) //if 'A' button held down execute below
        {
            if (boostSlider.value < boostSlider.maxValue)
            {
                boostSlider.value += (chargeTime * Time.deltaTime);
            }
            finishedPercentage = boostSlider.value / boostSlider.maxValue;
        }
        else if (button == 0)
        {
            if (boostSlider.value > 0)
            {
                boostSlider.value -= (dechargeTime * Time.deltaTime);
                kartStats.boostValue.DataValue = kartStats.boostCurve.Evaluate(finishedPercentage) * boostSlider.value;
            }
        }
    }

    public void UpdateMeter()
    {
        Debug.Log("called");
        boostSlider.value = kartStats.boostValue.DataValue;
    }
}
