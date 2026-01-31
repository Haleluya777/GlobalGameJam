using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [Header("Object Links")]
    [SerializeField] private GameObject mask;
    [SerializeField] private RectTransform rectTransform;

    [Header("Movement Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float gageDecreaseRate = 5f;
    private Vector2 input;

    private void Awake()
    {

    }

    private void Update()
    {
        if (rectTransform is null) return;

        Vector2 moveDelta = Vector2.zero;
        bool isTryingToMove = input.sqrMagnitude > 0;

        if (isTryingToMove)
        {
            bool isStuckAtBoundary = (rectTransform.anchoredPosition.x <= -700 && input.x < 0) || (rectTransform.anchoredPosition.x >= 700 && input.x > 0) || (rectTransform.anchoredPosition.y <= -700 && input.y < 0) || (rectTransform.anchoredPosition.y >= 300 && input.y > 0);

            if (!GameManager.instance.soundManager.audioSources[0].isPlaying) GameManager.instance.soundManager.audioSources[0].Play();

            if (GameManager.instance.MovingGage > 0 && !isStuckAtBoundary)
            {
                gageDecreaseRate = 10f;
                moveDelta = input * speed * Time.deltaTime;
            }
            else if (GameManager.instance.MovingGage <= 0 && !GameManager.instance.gameOver)
            {
                GameManager.instance.gameOver = true;
                Debug.Log("넌 게임 종료다 이자식아");
                GameManager.instance.stageManager.StageFailed();
            }
        }
        else
        {
            if (GameManager.instance.soundManager.audioSources[0].isPlaying) GameManager.instance.soundManager.audioSources[0].Stop();
            gageDecreaseRate = 5f;
        }

        GameManager.instance.MovingGage -= gageDecreaseRate * Time.deltaTime;

        GameManager.instance.MovingGage = Mathf.Clamp(GameManager.instance.MovingGage, 0, 100);
        GameManager.instance.canvasManager.SetMovementGage(GameManager.instance.MovingGage);

        Vector2 nextPosition = rectTransform.anchoredPosition + moveDelta;

        float clampedX = Mathf.Clamp(nextPosition.x, -700, 700);
        float clampedY = Mathf.Clamp(nextPosition.y, -700, 300);

        rectTransform.anchoredPosition = new Vector2(clampedX, clampedY);
    }

    private void OnMovement(InputValue value)
    {
        if (GameManager.instance.canControl) input = value.Get<Vector2>();
    }
}
