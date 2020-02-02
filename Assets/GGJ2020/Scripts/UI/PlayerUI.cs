using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerUI : MonoBehaviour
{
    [Header("Item Interaction")]
    [SerializeField]
    private GameObject interactItemUI;
    [SerializeField]
    private Image interactFillCircle;

    [Space]

    [Header("Item Effect")]
    [SerializeField]
    private Text effectText;

    [Space]

    [Header("Ammo")]
    [SerializeField]
    private Slider ammoSlider;

    [Space]

    [Header("Toasty")]
    [SerializeField]
    private GameObject toasty;
    [SerializeField]
    private AudioClip toastySound;
    [SerializeField]
    private Transform toastyActivePos;
    private Vector3 toastyOrigin;
    [SerializeField]
    private AnimationCurve toastyCurve;

    [Header("Stun Effect")]
    [SerializeField]
    private GameObject stunOverlay;


    private PlayerWeapon weapon;
    private PlayerController playerController;

    void Start()
    {
        weapon = GetComponent<PlayerWeapon>();
        playerController = GetComponent<PlayerController>();
        toastyOrigin = toasty.transform.position;
    }

    private void Update()
    {
        UpdateAmmoSlider();

        stunOverlay.SetActive(playerController.IsStunned);
    }

    private void UpdateAmmoSlider()
    {
        ammoSlider.value = Mathf.InverseLerp(0, weapon.maxCharge, weapon.currentCharge);
    }

    public void ActivateItemInteractUI()
    {
        interactItemUI.SetActive(true);
    }

    public void UpdateInteractionFillCircle(float progress)
    {
        interactFillCircle.fillAmount = progress;
    }

    public void DeactivateItemInteractUI()
    {
        interactItemUI.SetActive(false);
    }

    public void TriggerToasty()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(toasty.transform.DOMoveX(toastyActivePos.position.x, 0.15f).SetEase(toastyCurve));
        seq.AppendCallback(() => AudioSource.PlayClipAtPoint(toastySound, transform.position, 1f));
        seq.AppendInterval(2f);
        seq.Append(toasty.transform.DOMoveX(toastyOrigin.x, 0.15f));
    }

    public void DisplayItemEffect(string text)
    {
        effectText.text = text;

        Sequence seq = DOTween.Sequence();
        seq.Append(effectText.DOFade(1, 0.3f));
        seq.AppendInterval(2f);
        seq.Append(effectText.DOFade(0, 1f));
    }

}
