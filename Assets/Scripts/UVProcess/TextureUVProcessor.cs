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
        [SerializeField] private DeltaBufferContainer256 deltaBuffer;
        [SerializeField, InlineEditor] private ProcessElementsGroup processElementsGroup;

        private Material rendererMaterial;
        private Texture2D processedTexture;
        private Texture2D sourceCache;
        private Color32[] savedUVPixels;
        private Color32[] pixelsCache;
        private Color32[] processedUVPixels;

        private int textureWidth;
        private int textureHeight;

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
            processedUVPixels = new Color32[textureWidth * textureHeight];
            rendererMaterial = processedRenderer.material;

            for (int j = 0; j < textureHeight; j++)
            {
                for (int i = 0; i < textureWidth; i++)
                {
                    savedUVPixels[i + j * textureWidth] = new((byte)i, (byte)j, 0, 255);
                }
            }
            savedUVPixels.CopyTo(processedUVPixels, 0);
            float currentTime = Time.time;

            ChangeElements(processElementsGroup);
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
            deltaBuffer.DeltaBuffer.Update(currentTime);

            savedUVPixels.CopyTo(pixelsCache, 0);

            ProcessContext context = new ProcessContext()
            {
                textureWidth = textureWidth,
                textureHeight = textureHeight,
                sourcePixels = sourcePixels,
                savedUVPixels = savedUVPixels,
                pixelsCache = pixelsCache,
                processedUVPixels = processedUVPixels,
                currentTime = currentTime,
            };

            processElementsGroup.ProcessAll(context);

            // 変更したUV情報をTextureに保存(Applyを忘れずに!)
            processedTexture.SetPixels32(processedUVPixels);
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

        public void ChangeElements(ProcessElementsGroup processElementsGroup)
        {
            this.processElementsGroup = processElementsGroup;
            processElementsGroup.ResetTimeAll(Time.time);
        }
    }
}
