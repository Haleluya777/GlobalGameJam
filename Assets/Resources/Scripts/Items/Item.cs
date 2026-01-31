using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : PoolAble
{
    [SerializeField] private EventTrigger eventTrigger;

    private bool isReleased = false;

    public void InitClickEvent()
    {
        isReleased = false;

        if (eventTrigger != null)
        {
            eventTrigger.triggers.Clear();
        }

        if (GameManager.instance != null)
        {
            GameManager.instance.eventManager.ReleaseAllCharacter -= ReleaseThisItem;
            GameManager.instance.eventManager.ReleaseAllCharacter += ReleaseThisItem;
        }

        EventTrigger.Entry click = new EventTrigger.Entry();
        click.eventID = EventTriggerType.PointerClick;

        click.callback.AddListener((data) => { GameManager.instance.MovingGage += 20; });
        click.callback.AddListener((data) => { ReleaseThisItem(); });

        eventTrigger.triggers.Add(click);
    }

    private void ReleaseThisItem()
    {
        if (isReleased)
        {
            return;
        }

        isReleased = true;
        ReleaseObject();
    }

    void OnDisable()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.eventManager.ReleaseAllCharacter -= ReleaseThisItem;
        }
    }
}