using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField] Slider slider;
    private Kart_Input input;
    [SerializeField] float boostDuration = 0.01f;
    [Tooltip("Value of 1 = 1 second. Value of 2 = .5 second. Value of .5 = 2 seconds")]
    [SerializeField] float slowDuration = 0.01f;
    [SerializeField] DataFloat sliderValue;
    [SerializeField] DataFloat speedMultiplier;
    [SerializeField] DataFloat kartVelocity;
    //[SerializeField] float accelerationTime;
    [SerializeField] float decelerationTime;
    private void Awake()
    {
        input = new Kart_Input();
        slider.value = slider.minValue;
    }
    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
    }

    private void Update()
    {
        ChargeMeter();
        StopKart();
    }

    public void ChargeMeter()
    {
        float button = input.Kart_Controls.ActionButton.ReadValue<float>(); //read value of A button
        if (button == 1) //if 'A' button held down execute below
        {
            sliderValue.DataValue -= slowDuration * Time.deltaTime;
            if (sliderValue.DataValue <= slider.minValue)
            {
                sliderValue.DataValue = slider.minValue;
            }
        }
        else
        {
            sliderValue.DataValue += boostDuration * Time.deltaTime;
            if (sliderValue.DataValue >= slider.maxValue)
            {
                sliderValue.DataValue = slider.maxValue;
            }
        }

        slider.value = sliderValue.DataValue;
    }

    public void StopKart()
    {
        //could use the speed multiplier here to slow down the kart
        float button = input.Kart_Controls.ActionButton.ReadValue<float>(); //read value of A button
        if (button == 1) //if 'A' button held down execute below
        {
            speedMultiplier.DataValue -= (kartVelocity.DataValue * decelerationTime) * Time.deltaTime;
            if(speedMultiplier.DataValue <= 0)
            {
                speedMultiplier.DataValue = 0;
            }
        }
        else
        {
            speedMultiplier.DataValue = 1;
            /*
            speedMultiplier.DataValue += accelerationTime * Time.deltaTime;
            if (speedMultiplier.DataValue >= 1)
            {
                speedMultiplier.DataValue = 1;
            }
            */
        }
    }
}
