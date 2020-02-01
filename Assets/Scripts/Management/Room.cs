using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Room
{
    public IPlayer player;
    public GameObject water;
    public List<GameObject> holes;


    private float waterSpeedMultiplier = 1;

    public void SetReady() {
        //reset player position
        //reset water position
    }

    public void MoveWater() {
        Vector3 waterPosition = water.transform.position;

        waterPosition.y += GameController.waterBaseSpeed + (GetHoleNumber() * GameController.holeIncreaseSpeed) * waterSpeedMultiplier;

        water.transform.position = waterPosition;

        CheckWaterLevel();
    }

    public void CheckWaterLevel() {
        Vector3 waterPosition = water.transform.position;

        if(waterPosition.y >= GameController.InstaLossLevel)
            GameController.EndMatch(player);
    }

    public void CreateHole() {
        //spawn hole
    }

    public int GetHoleNumber() {
        if(holes == null)
            holes = new List<GameObject>();

        return holes.Count;
    }

    public void OnWaterReachLossLevel() {

    }

    public static GameController GameController { get { return GameController.Instance; } }
}
