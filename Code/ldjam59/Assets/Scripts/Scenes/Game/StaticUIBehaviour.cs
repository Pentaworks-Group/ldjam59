using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Scenes.Game
{
	public class StaticUIBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField] private GameMainBehaviour gameMainBehaviour;

        public void OnPointerEnter(PointerEventData eventData)
        {
            gameMainBehaviour.DisableClick();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            gameMainBehaviour.EnableClick();
        }
    }
}
