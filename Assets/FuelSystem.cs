using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelSystem : MonoBehaviour
{

    public float startFuel;//start fuel
    public float maxFuel = 100f;//max fuel
    public float fuelConsumptionRate; //fuel drop rate
    public Slider fuelIndicatorSlid; //slider to indicate the fuel level
    public Text fuelIndicatorTxt; //text to indicate the fuel level

    // Use this for initialization
    void Start()
    {

        ///cap the fuel
        if (startFuel > maxFuel)
        {
            startFuel = maxFuel;
        }
        //update ui elements
        fuelIndicatorSlid.maxValue = maxFuel;
        UpdateUI();
    }


    public void ReduceFuel()
    {
        //reduce fuel level and update ui elements
        startFuel -= Time.deltaTime * fuelConsumptionRate;
        UpdateUI();
    }


    //PICK UP JerryCan 
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GasCan"))
        {
            startFuel = 100f;
            ///cap the fuel
            if (startFuel > maxFuel)
            {
                startFuel = maxFuel;
            }
            UpdateUI();

            Destroy(other.gameObject);
        }
    }


    void UpdateUI()
    {
        fuelIndicatorSlid.value = startFuel;
        fuelIndicatorTxt.text = "Fuel left: " + startFuel.ToString("0") + "%";

        //if there is no fuel inform the user
        if (startFuel <= 0)
        {
            startFuel = 0;
            fuelIndicatorTxt.text = "Out of fuel!!!";
        }
    }
}
