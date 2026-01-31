using UnityEngine;
using UnityEngine.UI;

// ICanvasRaycastFilter 인터페이스를 구현하여 클릭을 직접 제어합니다.
public class CutOutMaskUI : Image, ICanvasRaycastFilter
{
    [Header("Click Filtering")]
    [Range(0.01f, 1f)]
    [SerializeField] private float alphaHitThreshold = 0.1f;

    private Material currentMaterial;

    protected override void Start()
    {
        base.Start();
        currentMaterial = new Material(material);
        material = currentMaterial;
    }

    public void SetTextures(Texture2D crackTexture, Texture2D maskTexture)
    {
        if (currentMaterial == null) return;

        currentMaterial.SetTexture("_CrackTex", crackTexture);
        currentMaterial.SetTexture("_MaskTex", maskTexture);
    }

    public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        if (currentMaterial == null) return false;

        Texture2D maskTexture = currentMaterial.GetTexture("_MaskTex") as Texture2D;
        if (maskTexture == null) return true;

        try
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out Vector2 localPoint);

            Vector2 uv = new Vector2(
                (localPoint.x + rectTransform.pivot.x * rectTransform.rect.width) / rectTransform.rect.width,
                (localPoint.y + rectTransform.pivot.y * rectTransform.rect.height) / rectTransform.rect.height
            );

            Color color = maskTexture.GetPixelBilinear(uv.x, uv.y);
            return color.a > alphaHitThreshold;
        }
        catch (UnityException e)
        {
            return true;
        }
    }
}