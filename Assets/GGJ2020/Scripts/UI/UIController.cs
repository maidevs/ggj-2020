using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    private static UIController _instance;

    [SerializeField]
    private GameObject itemSpawnWarning;
 
    public static UIController Instance
    {
        get { return _instance; }
    }
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    public void WarnAboutItemSpawn()
    {
        StartCoroutine(TemporaryWarning(itemSpawnWarning, 5f));
    }

    IEnumerator TemporaryWarning(GameObject uiElement, float time)
    {
        uiElement.SetActive(true);

        yield return new WaitForSeconds(time);

        uiElement.SetActive(false);
    }
}
