using NaughtyAttributes;
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
    private IPlayer PlayerPrefab;


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
    }

    private void Update() {
        if(!MatchStatus)
            return;

        MoveWaters();
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

    public void OnReadyPlayer() {
        CheckAllPlayersReady();
    }

    public void CheckAllPlayersReady() {
        foreach(Room room in Rooms)
            if(!room.player.IsPlayerReady())
                return;


        StartMatch();
    }

    public Vector3 GetPlayerLocation(int playerNumber) {
        return Vector3.zero;
    }

    private void StartMatch() {
        MatchStatus = true;
    }

    [Button("Generate Holes")]
    private void GenerateHoles() {
        Hole newHole;

        foreach(Room room in Rooms) {
            newHole = Instantiate(HolePrefab, transform.position, Quaternion.identity);

            room.PositionHole(newHole);
        }
    }

    public void EndMatch(IPlayer loserPlayer) {
        Debug.Log("Game Ended");
        MatchStatus = false;
    }
}
