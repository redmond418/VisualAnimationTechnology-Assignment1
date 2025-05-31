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
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = sourceRenderTexture;
            sourceCache.ReadPixels(new Rect(0, 0, sourceCache.width, sourceCache.height), 0, 0);
            sourceCache.Apply();
            RenderTexture.active = currentRT;

            Color32[] sourcePixels = sourceCache.GetPixels32();
            for (int j = 0; j < textureHeight; j++)
            {
                for (int i = 0; i < textureWidth; i++)
                {
                    int originIndex = i + j * textureWidth;
                    int indexOffset = Mathf.FloorToInt(sourcePixels[originIndex].r / 255 * Time.time * 20);
                    int uvIndex = (i + 0) % textureWidth + ((j + indexOffset) % textureHeight) * textureWidth;
                    pixelsCache[originIndex] = savedUVPixels[uvIndex];
                }
            }

            processedTexture.SetPixels32(pixelsCache);
            processedTexture.Apply();
            print(savedUVPixels[0] + ", " + savedUVPixels[textureWidth - 1] + ", \n" +
                savedUVPixels[(textureHeight - 1) * textureWidth] + ", " + savedUVPixels[textureWidth * textureHeight - 1]);

            rendererMaterial.SetTexture(texturePropertyName, processedTexture);
        }

        void OnDestroy()
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
    }
}
