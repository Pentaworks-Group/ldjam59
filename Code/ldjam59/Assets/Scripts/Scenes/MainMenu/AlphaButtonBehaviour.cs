using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Scenes.MainMenu
{
    [RequireComponent(typeof(Image))]
    public class AlphaButtonBehaviour : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Image image = GetComponent<Image>();
            RectTransform rectTransform = GetComponent<RectTransform>();
            Vector2 localPoint;

            // Convert screen point to local point in the RectTransform
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform, eventData.position, eventData.pressEventCamera, out localPoint))
            {
                // Convert local point to texture coordinates
                Rect rect = rectTransform.rect;
                Vector2 normalizedPoint = new Vector2(
                    (localPoint.x - rect.xMin) / rect.width,
                    (localPoint.y - rect.yMin) / rect.height
                );

                // Get the pixel color at the click position
                Sprite sprite = image.overrideSprite ?? image.sprite;
                if (sprite != null)
                {
                    Texture2D texture = sprite.texture;
                    int x = Mathf.FloorToInt(normalizedPoint.x * texture.width);
                    int y = Mathf.FloorToInt(normalizedPoint.y * texture.height);

                    // Check if the pixel is transparent (alpha < threshold)
                    Color pixelColor = texture.GetPixel(x, y);
                    if (pixelColor.a < 0.5f) // Adjust threshold as needed
                    {
                        return; // Ignore click on transparent area
                    }
                }
            }

            // Proceed with the click if the pixel is not transparent
            Debug.Log("Button clicked on visible area!");
            // Add your button logic here
        }
	}
}
