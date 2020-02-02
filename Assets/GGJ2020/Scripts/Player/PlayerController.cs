using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerController : MonoBehaviour {

    public int CurrentPlayerNumber;
    public FirstPersonController fpsController;

    bool IsStunned;

    public void Awake() {
        fpsController = GetComponent<FirstPersonController>();

        fpsController.enabled = false;
    }

    public void Enable() {
        fpsController.enabled = true;
    }

    public void Disable() {
        fpsController.enabled = false;
    }


    public void SetStun() {
        if(IsStunned)
            StopAllCoroutines();

        IsStunned = true;

        StartCoroutine(UnStun());
    }
    
    IEnumerator UnStun() {
        fpsController.enabled = false;
        HUDController.SetPlayerStun(CurrentPlayerNumber);

        yield return new WaitForSeconds(GameController.bulletStunTime);

        IsStunned = false;

        fpsController.enabled = true;
    }


    public static GameController GameController { get { return GameController.Instance; } }
    public static HUDController HUDController { get { return HUDController.Instance; } }
}
