using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // EventTrigger 사용을 위해 추가

public class StageManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> Stages = new List<GameObject>();
    [SerializeField] private List<StagePoints> StagePoints = new List<StagePoints>();
    [SerializeField] private List<Sprite> Masks = new List<Sprite>();

    [Header("Stage Count")]
    [SerializeField] private int Stage = 0;

    private int target; //타겟 수.
    [SerializeField] private int characters; //들러리 수.
    [SerializeField] private int items; //아이템 수.
    private List<GameObject> availablePoints = new List<GameObject>();
    public int chance = 3; //기회 수.
    private int previousChange;

    void Awake()
    {
        previousChange = chance;

        for (int i = 0; i < 5; i++)
        {
            foreach (Transform point in Stages[i].transform.GetChild(1).transform)
            {
                StagePoints[i].points.Add(point.gameObject);
            }
        }
    }

    void Update()
    {
        if (chance != previousChange)
        {
            if (chance <= 0 && !GameManager.instance.gameOver)
            {
                Debug.Log("게임 종료.");
                GameManager.instance.gameOver = true;
                previousChange = chance;
                StageFailed(true);
                return;
            }

            Debug.Log("찬스 소모.");
            GameManager.instance.maskManager.SetMask();
            previousChange = chance;
        }
    }

    public void StageInitialization()
    {
        Stage = 0;
        chance = 3;
        GameManager.instance.MovingGage = 100f;
        StageStart();
    }

    public void StageStart()
    {
        if (Stage >= StagePoints.Count || Stage >= Masks.Count || Stage >= GameManager.instance.characterManager.maker.Count)
        {
            return;
        }

        //target = Random.Range(1, 2);
        //characters = Random.Range(3, 5);

        //테스트 용
        target = 1;
        characters = 48;
        items = 1;

        GameManager.instance.canControl = true;

        SettingMask(Stage);

        availablePoints.Clear();
        availablePoints.AddRange(StagePoints[Stage].points);

        //들러리 및 목표 캐릭터 생성.
        for (int i = 1; i <= target + characters; i++)
        {
            var point = availablePoints[Random.Range(0, availablePoints.Count)];

            var CharacterTemplate = GameManager.instance.objectPoolManager.GetGo("CharacterTemplate");

            if (i > characters)
            {
                GameManager.instance.characterManager.maker[Stage].MakeWitness(CharacterTemplate); //타겟 생성.
                GameManager.instance.canvasManager.witness = CharacterTemplate;
            }
            else
            {
                GameManager.instance.characterManager.maker[Stage].MakeCharacter(CharacterTemplate);
            }

            CharacterTemplate.transform.SetParent(point.transform, false);

            var pos = CharacterTemplate.GetComponent<RectTransform>();
            // pos.anchoredPosition = new Vector3(0, 0, 0);
            // pos.localEulerAngles = Vector3.zero;
            pos.localScale = 2 * CharacterTemplate.transform.parent.localScale;

            availablePoints.Remove(point);
        }

        //아이템 생성.
        for (int i = 0; i < items; i++)
        {
            var point = availablePoints[Random.Range(0, availablePoints.Count)];
            var item = GameManager.instance.objectPoolManager.GetGo("HeartItem");

            item.GetComponent<Item>().InitClickEvent();

            item.transform.SetParent(point.transform, false);
            var pos = item.GetComponent<RectTransform>();
            pos.anchoredPosition = Vector2.zero;
            pos.localScale = item.transform.parent.localScale;
        }
    }

    public void StageFailed(bool busted = false)
    {
        Debug.Log("스테이지 실패.");
        GameManager.instance.canControl = false;
        if (busted)
        {
            GameManager.instance.canvasManager.ActiveStageFailedrPanel(busted);
        }
        else
        {
            GameManager.instance.canvasManager.ActiveStageFailedrPanel();
        }
    }

    public void StageClear()
    {
        Debug.Log("스테이지 클리어");
        GameManager.instance.canControl = false;
        GameManager.instance.canvasManager.ActiveStageClearPanel();
    }

    public void SettingMask(int stage)
    {
        GameManager.instance.maskManager.holeSprite = Masks[stage];
        GameManager.instance.maskManager.SetMask();
    }

    public void DisablePreviousStage()
    {
        Stages[Stage].SetActive(false);
    }

    public void ActiveStage()
    {
        if (Stage + 1 >= Stages.Count)
        {
            return;
        }

        Stage++;
        GameManager.instance.RefillGage();
        Stages[Stage].SetActive(true);
    }
}
