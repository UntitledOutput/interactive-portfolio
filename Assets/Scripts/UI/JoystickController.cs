using MyBox;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class JoystickController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        private RectTransform _rectTransform;

        public bool IsDown = false;
        public Vector2 MoveDirection = Vector2.zero;
    
        public float Radius = 25f;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _rectTransform = transform as RectTransform;
        }

        // Update is called once per frame
        void Update()
        {
            MoveDirection = MoveDirection.ClampX(-1, 1).ClampY(-1, 1);
            _rectTransform.anchoredPosition = Vector2.Lerp(
                _rectTransform.anchoredPosition, MoveDirection * Radius, Time.deltaTime * 5f);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            IsDown = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta;
            _rectTransform.anchoredPosition = _rectTransform.anchoredPosition.ClampX(-Radius, Radius).ClampY(-Radius, Radius);

            MoveDirection = _rectTransform.anchoredPosition / new Vector2(Radius, Radius);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            IsDown = false;
            MoveDirection = Vector2.zero;
        }
    }

}