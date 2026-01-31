using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public Action ReleaseAllCharacter;

    public void CallReleaseCharacter()
    {
        ReleaseAllCharacter?.Invoke();
    }
}
