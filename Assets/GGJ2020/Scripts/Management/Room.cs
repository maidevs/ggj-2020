using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Room : MonoBehaviour
{
    public PlayerController player;
    public float CurrentWaterSpeed;
    public Renderer water;
    public Renderer[] walls;


    private List<Hole> holes;
    private float waterSpeedMultiplier = 1;

    public void Start() {
        holes = new List<Hole>();
    }

    public void SetReady() {
        //reset player position
        //reset water position
    }

    public void MoveWater() {
        Vector3 waterPosition = water.transform.position;

        CurrentWaterSpeed = GameController.waterBaseSpeed + (GetHoleNumber() * GameController.holeIncreaseSpeed) * waterSpeedMultiplier;

        waterPosition.y += CurrentWaterSpeed;

        water.transform.position = waterPosition;

        CheckWaterLevel();
    }

    public void CheckWaterLevel() {
        Vector3 waterPosition = water.transform.position;

        if(waterPosition.y >= GameController.InstaLossLevel)
            GameController.EndMatch(player);
    }

    public void PositionHole(Hole hole) {
        bool foundPlace = false;
        int tries = 0;

        do {
            int randomWallIndex = UnityEngine.Random.Range(0, walls.Length);
            Renderer wall = walls[randomWallIndex];

            Vector3 wallCenter = wall.bounds.center;
            Vector3 wallExtends = wall.bounds.extents;

            Bounds holeBounds = hole.GetBonds();


            Vector3 randomPosition = Vector3.zero;
            randomPosition.y = UnityEngine.Random.Range(-wall.bounds.extents.y + holeBounds.size.y/2, wall.bounds.extents.y - holeBounds.size.y/2);
            randomPosition.x = UnityEngine.Random.Range(-wall.bounds.extents.x + holeBounds.size.x/2, wall.bounds.extents.x - holeBounds.size.x/2);
            randomPosition.z += (wall.bounds.extents.z * 1.05f * -wall.transform.forward.z);

            randomPosition += wallCenter;

            Collider[] foundHoles= Physics.OverlapBox(randomPosition, holeBounds.extents / 2);

            if(foundHoles == null || foundHoles.Length > 0) {
                //place already occuped
                foundPlace = false;
                tries++;

                if(tries >= 10) {
                    Debug.LogWarning("Did not found spot to instantiate hole. Skipping");
                }
                continue;
            }

            foundPlace = true;

            hole.transform.SetParent(wall.transform, true);
            hole.transform.position = randomPosition;
            hole.transform.rotation = Quaternion.Inverse(wall.transform.rotation);

            holes.Add(hole);
        } while(!foundPlace && tries < 10);

    }

    public int GetHoleNumber() {
        return holes.Count;
    }

    public void WarnDeadHole(Hole hole) {
        holes.RemoveAll(a => a == hole);
    }

    public void EnablePlayer() {
        player.Enable();
    }

    public void DisablePlayer() {
        player.Disable();
    }

    public static GameController GameController { get { return GameController.Instance; } }
}
