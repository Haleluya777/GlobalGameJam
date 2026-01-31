using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject targetObj;
    [SerializeField] private GameObject targetFullObj;
    [SerializeField] private GameObject StageClearPanel;
    [SerializeField] private GameObject StageFailedPanel;
    [SerializeField] private GameObject ReGameObj;
    [SerializeField] private Slider movementGage;

    public List<Sprite> targetSprites = new List<Sprite>();
    public GameObject witness;

    public void SetMovementGage(float movement)
    {
        movementGage.value = movement / 100f;
    }

    public void SetTargetImg()
    {
        var face = targetObj.transform.GetChild(0).GetComponent<Image>();
        var hair = targetObj.transform.GetChild(1).GetComponent<Image>();

        face.sprite = targetSprites[0];
        hair.sprite = targetSprites[3];
    }

    public void ActiveStageClearPanel()
    {
        //var face = targetFullObj.transform.GetChild(0).GetComponent<Image>();
        //var pants = targetFullObj.transform.GetChild(1).GetComponent<Image>();
        //var body = targetFullObj.transform.GetChild(2).GetComponent<Image>();
        //var hair = targetFullObj.transform.GetChild(3).GetComponent<Image>();
        //
        //face.sprite = targetSprites[0];
        //pants.sprite = targetSprites[1];
        //body.sprite = targetSprites[2];
        //hair.sprite = targetSprites[3];

        StageClearPanel.SetActive(true);
    }

    public void ActiveStageFailedrPanel(bool busted = false)
    {
        StageFailedPanel.SetActive(true);
        if (busted) StageFailedPanel.transform.GetChild(0).gameObject.SetActive(true);
        else StageFailedPanel.transform.GetChild(1).gameObject.SetActive(true);

        Invoke("ActiveReGameObj", 1.5f);
    }

    private void ActiveReGameObj()
    {
        ReGameObj.SetActive(true);
    }
}
