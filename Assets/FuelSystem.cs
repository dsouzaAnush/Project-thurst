using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelSystem : MonoBehaviour {
    public float startFuel=50f;
    public float maxFuel = 100f;
    public float fuelConsumptionRate=0.2f;
    public Slider fuelIndicatorSlid;
    public Text fuelIndicatorTxt;



	// Use this for initialization
	void Start () {
        if (startFuel > maxFuel)
        {
            startFuel = maxFuel;
          
        }
        startFuel = 50f;
        fuelIndicatorSlid.maxValue = maxFuel;
        UpdateUI();
    }

    public void ReduceFuel()
    {

        startFuel = Time.deltaTime * fuelConsumptionRate;
        Debug.Log(startFuel);
        UpdateUI();
    }
	
	// Update is called once per frame
	void UpdateUI () {
        fuelIndicatorSlid.value = startFuel;
        fuelIndicatorTxt.text = "fuel Left: " + startFuel.ToString("0") + "%";
        if(startFuel <= 0)
        {
            startFuel = 0;
            fuelIndicatorTxt.text = "Out of fuel";
        }

		
	}
}
