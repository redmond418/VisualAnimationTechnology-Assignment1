using Alchemy.Inspector;
using UnityEngine;

namespace VAT
{
    public class SpriteCornerUVTranscripter : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        private Material material;

        private void Reset()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Awake()
        {
            material = spriteRenderer.material;
            TranscriptUV();
        }

        [Button, DisableInEditMode]
        public void TranscriptUV()
        {
            Sprite sprite = spriteRenderer.sprite;
            Vector2 rectSize = sprite.rect.size / 2 / sprite.pixelsPerUnit;
            Vector3 leftBottom = transform.TransformPoint(new Vector2(-1, -1) * rectSize);
            Vector3 rightBottom = transform.TransformPoint(new Vector2(1, -1) * rectSize);
            Vector3 leftTop = transform.TransformPoint(new Vector2(-1, 1) * rectSize);
            Vector3 rightTop = transform.TransformPoint(new Vector2(1, 1) * rectSize);

            Camera mainCam = Camera.main;
            Vector3 screenLeftBottom = mainCam.WorldToScreenPoint(leftBottom);
            Vector3 screenRightBottom = mainCam.WorldToScreenPoint(rightBottom);
            Vector3 screenLeftTop = mainCam.WorldToScreenPoint(leftTop);
            Vector3 screenRightTop = mainCam.WorldToScreenPoint(rightTop);

            material.SetVector("_LeftBottom", screenLeftBottom);
            material.SetVector("_RightBottom", screenRightBottom);
            material.SetVector("_LeftTop", screenLeftTop);
            material.SetVector("_RightTop", screenRightTop);
            material.SetVector("_Size", mainCam.pixelRect.size);
        }
    }
}
