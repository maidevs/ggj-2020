using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{

    [Header("Match Settings")]
    [SerializeField]
    private bool MatchStatus = false;
    [SerializeField]
    public float InstaLossLevel;



    [Header("Player Settings")]
    [SerializeField]
    public float bulletDamage;
    [SerializeField]
    public float bulletStunTime;
    [SerializeField]
    public float gunMaxCharge;
    [SerializeField]
    public float gunDepleteRate;
    [SerializeField]
    public float gunRechargeRate;
    [SerializeField]
    private List<Transform> players;

    [Header("Water Settings")]
    public float waterBaseSpeed;
    public float holeIncreaseSpeed;


    [Header("Holes Settings")]
    public float holeSpawnTime;
    public float holeMaxHealth;
    public Hole HolePrefab;
    
    [Header("Runtime Properties")]
    [SerializeField]
    private Room[] Rooms;

    private void Start() {
        InitializeRooms();

        StartCoroutine(StartMatchCountDown());
    }

    private void Update() {
        if(!MatchStatus)
            return;

        MoveWaters();
    }

    private IEnumerator SpawnHoles() {
        while(MatchStatus) {
            yield return new WaitForSecondsRealtime(holeSpawnTime);

            GenerateHoles();
        }
    }

    private void InitializeRooms() {
        foreach(Room room in Rooms) {
            room.SetReady();
        }
    }

    private void MoveWaters() {
        foreach(Room room in Rooms)
            room.MoveWater();
    }


    public IEnumerator StartMatchCountDown() {
        int value = 3;

        HUDController.SetText("Get Ready...");

        yield return new WaitForSeconds(2);


        while(value > 0) {
            HUDController.SetCountdown(value);

            yield return new WaitForSeconds(1);
            value--;
        }

        HUDController.SetText("Go!");

        yield return new WaitForSeconds(1);

        HUDController.Clear();

        StartMatch();
    }

    public Vector3 GetPlayerLocation(int playerNumber) {
        return Vector3.zero;
    }

    private void StartMatch() {
        MatchStatus = true;

        foreach(Room room in Rooms) {
            room.EnablePlayer();
        }

        StartCoroutine(SpawnHoles());
    }

    [Button("Generate Holes")]
    private void GenerateHoles() {
        Hole newHole;

        foreach(Room room in Rooms) {
            newHole = Instantiate(HolePrefab, transform.position, Quaternion.identity);
            newHole.Initialize(holeMaxHealth, room);

            room.PositionHole(newHole);
        }
    }

    public Transform GetEnemy(int index)
    {
        if (index == 1)
            return players[0];
        else 
            return players[1];
    }

    public void EndMatch(PlayerController loserPlayer) {
        Debug.Log("Game Ended");
        MatchStatus = false;

        foreach(Room room in Rooms) {
            room.DisablePlayer();
        }
    }

    public static HUDController HUDController { get { return HUDController.Instance; } }
}

