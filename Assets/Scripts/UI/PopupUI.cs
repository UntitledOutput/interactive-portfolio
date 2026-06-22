using System;
using DG.Tweening;
using External;
using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PopupUI : MonoBehaviour
    {
        [Serializable]
        public class PopupData
        {
            public string Title;
            public Sprite TitleImage;
            public string Description;
            public Sprite Thumbnail;
            
            [Separator]
            public bool ShowButton;

            public string URL;
        }
        
        [SerializeField]
        private PopupData _data = new PopupData();
        
        public string Title => _data.Title;
        public string Description => _data.Description;
        public Sprite Thumbnail => _data.Thumbnail;

        public Vector3 WorldPosition;
        
        private TMP_Text _titleText;
        private Image _titleImage;
        private TMP_Text _descriptionText;
        private Image _thumbnailImage;
        private CanvasGroup _canvasGroup;
        private Camera _mainCamera;
        private RectTransform _rectTransform;
        
        private bool _isVisible = false;
        
        private static PopupUI _instance;
        
        public static PopupUI ShowNewPopup(PopupData data)
        {
            var canvas = GameObject.Find("Canvas");
            
            PopupUI popup = Instantiate(Resources.Load<PopupUI>("PopupUI"),canvas.transform);
            popup._data = data;
            
            popup.Hide();
            
            return popup;
        }
        
        public void Start()
        {
            _titleText = transform.RecursiveFind("TitleText").GetComponent<TMP_Text>();
            _titleImage = transform.RecursiveFind("TitleImage").GetComponent<Image>();
            _descriptionText = transform.Find("Description").GetComponent<TMP_Text>();
            _thumbnailImage = transform.RecursiveFind("Thumbnail",false).GetComponent<Image>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
            _rectTransform = GetComponent<RectTransform>();
            
            _mainCamera = Camera.main;
            
            if (_data.TitleImage != null)
            {
                _titleImage.sprite = _data.TitleImage;
                _titleImage.gameObject.SetActive(true);
                _titleText.gameObject.SetActive(true);
            } else if (!string.IsNullOrEmpty(_data.Title))
            {
                _titleText.text = _data.Title;
                _titleImage.gameObject.SetActive(false);
                _titleText.gameObject.SetActive(true);
            } else
            {
                _titleImage.gameObject.SetActive(false);
                _titleText.gameObject.SetActive(false);
            }

            var button = transform.RecursiveFind("Button").GetComponent<Button>();
            button.gameObject.SetActive(_data.ShowButton);
            button.onClick.AddListener(OnButtonClick);
            if (_data.ShowButton)
            {
                
            }
        }
        
        public void Show()
        {
            if (_isVisible) return;
            if (_instance != null) return;
            _isVisible = true;
            _instance = this;
            
            _canvasGroup.DOKill();
            var sequence = DOTween.Sequence();

            float duration = 0.25f;
            
            sequence.Append(_canvasGroup.DOFade(1, duration));
            sequence.Join(transform.DOScale(Vector3.one, duration).From(Vector3.zero).SetEase(Ease.InQuint));
            
            sequence.Play();
        }
        
        public void Hide()
        {
            if (!_isVisible) return;
            if (_instance != this) return;
            _isVisible = false;
            _instance = null;
            
            _canvasGroup.DOKill();
            var sequence = DOTween.Sequence();
            
            float duration = 0.25f;

            sequence.Append(_canvasGroup.DOFade(0,duration));
            sequence.Join(transform.DOScale(Vector3.zero, duration).From(Vector3.one).SetEase(Ease.OutQuint));
            
            sequence.Play();
        }
        
        public void OnButtonClick()
        {
            if (!string.IsNullOrEmpty(_data.URL))
            {
                Application.OpenURL(_data.URL);
            }
        }

        private void Update()
        {
            _thumbnailImage.sprite = Thumbnail;
            _titleText.text = Title;
            _descriptionText.text = Description;
            if (UIController.Instance.IsMobile)
            {
                // for mobile, the popup is going to be at the top of the screen, so we don't need to update its position based on the world position
                
                // step 1: set the pivot to the top center
                _rectTransform.pivot = new Vector2(0.5f, 1f);
                _rectTransform.anchorMin = new Vector2(0.5f, 1f);
                _rectTransform.anchorMax = new Vector2(0.5f, 1f);
                
                // step 2: set the anchored position to be at the top center of the screen
                _rectTransform.anchoredPosition = new Vector2(0, -(Screen.safeArea.position.y*2) - 50f);
                _rectTransform.SetWidth(UIController.Instance.CanvasScaler.referenceResolution.x-400f);
                
                
            }
            else
            {
                _rectTransform.pivot = new Vector2(0.5f, 0.5f);
                _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                _rectTransform.anchorMax = new Vector2(0.5f, 0.5f); 
                _rectTransform.SetWidth(400);
                
                Vector3 screenPos = _mainCamera.WorldToScreenPoint(WorldPosition);
                transform.position = screenPos;
            }
        }
    }
}