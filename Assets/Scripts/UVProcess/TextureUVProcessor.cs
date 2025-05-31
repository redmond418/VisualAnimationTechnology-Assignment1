using Alchemy.Inspector;
using UnityEngine;

namespace VAT
{
    // [ExecuteAlways]
    public class TextureUVProcessor : MonoBehaviour
    {
        [SerializeField] private RenderTexture sourceRenderTexture;
        [SerializeField] private Renderer processedRenderer;
        [SerializeField, ValidateInput(nameof(ValidateTexturePropertyName), "The property is invalid!")] private string texturePropertyName = "_Texture2D";

        private Material rendererMaterial;
        private Texture2D processedTexture;
        private Texture2D sourceCache;
        private Color32[] savedUVPixels;
        private Color32[] pixelsCache;

        private int textureWidth;
        private int textureHeight;

        private float previousTime;

        private bool ValidateTexturePropertyName(string texturePropertyName)
            => processedRenderer != null && processedRenderer.sharedMaterial.HasTexture(texturePropertyName);

        private void Awake()
        {
            textureWidth = sourceRenderTexture.width;
            textureHeight = sourceRenderTexture.height;

            processedTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RG16, false);
            processedTexture.filterMode = FilterMode.Point;
            sourceCache = new Texture2D(textureWidth, textureHeight, TextureFormat.ARGB32, false);
            sourceCache.filterMode = FilterMode.Point;
            savedUVPixels = new Color32[textureWidth * textureHeight];
            pixelsCache = new Color32[textureWidth * textureHeight];
            rendererMaterial = processedRenderer.material;

            for (int j = 0; j < textureHeight; j++)
            {
                for (int i = 0; i < textureWidth; i++)
                {
                    savedUVPixels[i + j * textureWidth] = new((byte)i, (byte)j, 0, 255);
                }
            }
        }

        private void Update()
        {
            // RenderTextureのピクセル情報をTextureに転写する
            // 現在アクティブなRenderTextureを読み取りたいものに変更する
            RenderTexture currentRTCache = RenderTexture.active;
            RenderTexture.active = sourceRenderTexture;
            // ReadPixelsでアクティブなRenderTextureのピクセル情報をTextureに転写(Applyを忘れずに!)
            sourceCache.ReadPixels(new Rect(0, 0, sourceCache.width, sourceCache.height), 0, 0);
            sourceCache.Apply();
            RenderTexture.active = currentRTCache;

            // Textureのピクセル情報を1列の配列に移す
            Color32[] sourcePixels = sourceCache.GetPixels32();

            // ピクセル情報を元にUVをぐりぐりする
            float currentTime = Time.time;
            int delta = Mathf.FloorToInt((currentTime - previousTime) * 20);
            if (delta > 0) previousTime = currentTime;

            savedUVPixels.CopyTo(pixelsCache, 0);
            for (int j = 0; j < textureHeight; j++)
            {
                for (int i = 0; i < textureWidth; i++)
                {
                    int originIndex = i + j * textureWidth;
                    int indexOffset = Mathf.FloorToInt(sourcePixels[originIndex].g / 255f * delta);
                    int uvIndex = ModLoop(i + 0, textureWidth) + ModLoop(j + indexOffset, textureHeight) * textureWidth;
                    if (sourcePixels[uvIndex].g != sourcePixels[originIndex].g) uvIndex = Random.Range(0, textureWidth * textureHeight);
                    pixelsCache[originIndex] = pixelsCache[uvIndex];
                }
            }

            for (int j = 0; j < textureHeight; j++)
            {
                for (int i = 0; i < textureWidth; i++)
                {
                    int originIndex = i + j * textureWidth;
                    int uvIndex = originIndex;
                    if (Random.Range(0, 256) < sourcePixels[originIndex].r) uvIndex = Random.Range(0, textureWidth * textureHeight);
                    pixelsCache[originIndex] = pixelsCache[uvIndex];
                }
            }
            pixelsCache.CopyTo(savedUVPixels, 0);

            // 変更したUV情報をTextureに保存(Applyを忘れずに!)
            processedTexture.SetPixels32(pixelsCache);
            processedTexture.Apply();

            rendererMaterial.SetTexture(texturePropertyName, processedTexture);
        }

        private void OnDestroy()
        {
            if (Application.isPlaying)
            {
                Destroy(processedTexture);
                Destroy(sourceCache);
                Destroy(rendererMaterial);
            }
            else
            {
                DestroyImmediate(processedTexture);
                DestroyImmediate(sourceCache);
                DestroyImmediate(rendererMaterial);
            }
        }

        private static int ModLoop(int a, int b)
        {
            int mod = a % b;
            if (mod < 0) mod += b;
            return mod;
        }
    }
}
