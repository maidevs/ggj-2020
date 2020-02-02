using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerController : MonoBehaviour {

    public int CurrentPlayerNumber;
    FirstPersonController fpsController;
    PlayerWeapon playerWeapon;


    bool IsStunned;

    public void Awake() {
        fpsController = GetComponent<FirstPersonController>();
        playerWeapon = GetComponent<PlayerWeapon>();

        Disable();
    }

    public void Enable() {
        fpsController.enabled = true;
        playerWeapon.enabled = true;
    }

    public void Disable() {
        fpsController.enabled = false;
        playerWeapon.enabled = false;
    }


    public void SetStun() {
        if(IsStunned)
            StopAllCoroutines();

        IsStunned = true;

        StartCoroutine(UnStun());
    }

    public void SetColor(Color color) {
        playerWeapon.SetColor(color);
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
