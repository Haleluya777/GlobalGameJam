using UnityEngine;

public class MaskManager : MonoBehaviour
{
    [SerializeField] private CutOutMaskUI maskUI;
    public Sprite holeSprite; // 구멍 형태
    public Sprite[] crackSprite = new Sprite[2]; // 금 간 형태.

    void Start()
    {
        if (maskUI == null)
        {
            return;
        }
        if (holeSprite == null)
        {
            return;
        }
    }

    public void SetMask()
    {
        maskUI.SetTextures(crackSprite[(GameManager.instance.stageManager.chance - 1)].texture, holeSprite.texture);
    }
}
