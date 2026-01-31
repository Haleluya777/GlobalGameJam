using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Character : PoolAble
{
    [Header("Sprite When Character is Dead")]
    [SerializeField] private Sprite tomb;

    [SerializeField] private EventTrigger eventTrigger;
    private EventTrigger.Entry click;
    private Animator anim;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        anim.SetFloat("Speed", Random.Range(0.7f, 1f));
    }

    public void InitClickEvent(bool target = false)
    {
        eventTrigger.triggers.Clear();
        click = new EventTrigger.Entry();

        click.eventID = EventTriggerType.PointerClick;
        eventTrigger.triggers.Add(click);

        if (GameManager.instance is not null)
        {
            GameManager.instance.eventManager.ReleaseAllCharacter -= InitCharacter;
            GameManager.instance.eventManager.ReleaseAllCharacter += InitCharacter;
        }

        if (target)
        {
            click.callback.AddListener((data) => { GameManager.instance.stageManager.StageClear(); });
            click.callback.AddListener((data) => { GameManager.instance.canControl = false; });
        }

        else
        {
            click.callback.AddListener((data) => { GameManager.instance.stageManager.chance--; });
        }

        click.callback.AddListener((data) => { this.GetComponent<Image>().sprite = tomb; });
        click.callback.AddListener((data) => ChildrenSetActiveFalse());
        click.callback.AddListener((data) => GameManager.instance.soundManager.audioSources[1].PlayOneShot(GameManager.instance.soundManager.killSound));
    }

    public void ChildrenSetActiveFalse()
    {
        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        var img = this.gameObject.GetComponent<Image>();

        img.raycastTarget = false;
        img.SetNativeSize();
    }

    public void InitCharacter()
    {
        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        var img = this.GetComponent<Image>();

        img.sprite = null;
        img.raycastTarget = true;

        this.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 80);
        ReleaseObject();
    }

    void OnDisable()
    {
        if (GameManager.instance is not null)
        {
            GameManager.instance.eventManager.ReleaseAllCharacter -= InitCharacter;
        }
    }
}
