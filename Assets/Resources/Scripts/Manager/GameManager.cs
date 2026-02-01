using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public StageManager stageManager;
    public ObjectPooler objectPoolManager;
    public CharacterManager characterManager;
    public CanvasManager canvasManager;
    public EventManager eventManager;
    public MaskManager maskManager;
    public SoundManager soundManager;

    public bool canControl;
    public float MovingGage = 100f;
    public bool gameOver = false;
    public bool stageClear = false;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        //Debug.Log("스테이지 시작");
        stageManager.StageStart();
    }

    public void RefillGage()
    {
        MovingGage = 100f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GameClear()
    {
        Debug.Log("게임을 클리어 하였습니다.");
        canvasManager.ActiveStaffRoll();
    }
}
