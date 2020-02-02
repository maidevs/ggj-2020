using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : Singleton<HUDController> {

    public Text Label;
    public Slider[] playersCharge;


    public void SetCountdown(int value) {
        Label.text = value.ToString();
    }

    public void SetText(string value) {
        Label.text = value;
    }

    public void Clear() {
        Label.text = "";
    }

    public void SetPlayerStun(int playerNumber) {
        Debug.Log("TODO DIS");
    }

    public void SetPlayerCharge(int playerNumber, float chargePercentage) {

        playersCharge[playerNumber].value = chargePercentage;
    }

}
