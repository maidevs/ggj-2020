using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerController : MonoBehaviour {

    public int CurrentPlayerNumber;
    public LayerMask waterItemLayer;
    public LayerMask deathLayer;
    public Animator[] animators;

    PlayerUI playerUI;
    CharacterController characterController;
    FirstPersonController fpsController;
    PlayerWeapon playerWeapon;
    Room parentRoom;

    public float stunMultiplier = 1f;

    private bool isUnderWater;
    public bool IsStunned;


    private float stunLevel;
    private float maxStun = 3;
    private float stunDeplete = 0.1f;
    private float stunIncrease = 0.5f;

    public void Awake() {
        playerUI = GetComponent<PlayerUI>();
        characterController = GetComponent<CharacterController>();
        fpsController = GetComponent<FirstPersonController>();
        playerWeapon = GetComponent<PlayerWeapon>();

        Disable();
    }

    public void SetRoom(Room room) {
        parentRoom = room;
    }

    public void Enable() {
        fpsController.enabled = true;
        characterController.enabled = true;
        playerWeapon.enabled = true;
    }

    public void Disable() {
        fpsController.enabled = false;
        playerWeapon.enabled = false;
        characterController.enabled = false;
    }

    public void Update() {
        playerWeapon.SetUnderWater(isUnderWater);

        stunLevel -= stunDeplete * Time.deltaTime;
        if(stunLevel <= 0)
            stunLevel = 0;
    }

    public void SetStun(bool playAnimation = true, bool showOverlay = true) {
        if(IsStunned)
            StopAllCoroutines();

        IsStunned = true;

        if(playAnimation)
            SetAnimatorTrigger("hitted");

        PlayerUI playerUI = transform.GetComponent<PlayerUI>();

        playerUI.TriggerToasty();

        if(showOverlay)
            playerUI.stunOverlay.SetActive(true);

        StartCoroutine(UnStun());
    }

    public void AddStun() {
        if(IsStunned)
            return;

        stunLevel += stunIncrease;

        if(stunLevel >= maxStun) {
            SetStun();

            stunLevel = 0;
        }
    }

    public void SetColor(Color color) {
        playerWeapon.SetColor(color);
    }

    IEnumerator UnStun() {
        fpsController.enabled = false;
        HUDController.SetPlayerStun(CurrentPlayerNumber);

        yield return new WaitForSeconds(GameController.bulletStunTime * stunMultiplier);

        IsStunned = false;

        fpsController.enabled = true;

        playerUI.stunOverlay.SetActive(false);
    }


    private void OnTriggerEnter(Collider other) {
        if((deathLayer.value & 1 << other.gameObject.layer) != 0) {
            SetAnimatorTrigger("hitted");
            parentRoom.RespawnPlayer();
        }
    }

    private void OnTriggerStay(Collider other) {
        if((waterItemLayer.value & 1 << other.gameObject.layer) != 0) {
            if(other.transform.position.y > transform.position.y + characterController.height * 0.3f) {
                SetUnderWateR(true);
            } else {
                SetUnderWateR(false);
            }
        }
    }


    public void SetUnderWateR(bool status) {
        if(status) {
            isUnderWater = true;
        } else {
            isUnderWater = false;
        }
    }

    private void OnTriggerExit(Collider other) {
        if((waterItemLayer.value & 1 << other.gameObject.layer) != 0) {
            SetUnderWateR(false);
        }
    }


    public void SetAnimatorFloat(string name, float value) {
        foreach(Animator animator in animators) {
            animator.SetFloat(name, value);
        }
    }

    public void SetAnimatorBool(string name, bool value) {
        foreach(Animator animator in animators) {
            animator.SetBool(name, value);
        }
    }

    public void SetAnimatorTrigger(string name) {
        foreach(Animator animator in animators) {
            animator.SetTrigger(name);
        }
    }

    public static GameController GameController { get { return GameController.Instance; } }
    public static HUDController HUDController { get { return HUDController.Instance; } }
}
