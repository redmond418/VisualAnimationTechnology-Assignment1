using Alchemy.Inspector;
using UnityEngine;

namespace VAT
{
    [ExecuteAlways]
    public class TextureUVProcessor : MonoBehaviour
    {
        [SerializeField] private RenderTexture sourceRenderTexture;
        [SerializeField] private Renderer processedRenderer;
        [SerializeField, ValidateInput(nameof(ValidateTexturePropertyName), "The property is invalid!")] private string texturePropertyName = "_Texture2D";

        private Material rendererMaterial;
        private Texture2D processedTexture;
        private Texture2D savedUVTexture;
        private Texture2D sourceCache;

        private bool ValidateTexturePropertyName(string texturePropertyName)
            => processedRenderer != null && processedRenderer.sharedMaterial.HasTexture(texturePropertyName);

        private void Awake()
        {
            processedTexture = new Texture2D(sourceRenderTexture.width, sourceRenderTexture.height, TextureFormat.RG16, false);
            processedTexture.filterMode = FilterMode.Point;
            savedUVTexture = new Texture2D(sourceRenderTexture.width, sourceRenderTexture.height, TextureFormat.ARGB32, false);
            savedUVTexture.filterMode = FilterMode.Point;
            sourceCache = new Texture2D(sourceRenderTexture.width, sourceRenderTexture.height, TextureFormat.ARGB32, false);
            sourceCache.filterMode = FilterMode.Point;
            rendererMaterial = processedRenderer.material;
        }

        private void Update()
        {
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = sourceRenderTexture;
            sourceCache.ReadPixels(new Rect(0, 0, sourceCache.width, sourceCache.height), 0, 0);
            sourceCache.Apply();
            RenderTexture.active = currentRT;

            Color32[] sourcePixels = sourceCache.GetPixels32();

            rendererMaterial.SetTexture(texturePropertyName, sourceCache);
        }
    }
}
