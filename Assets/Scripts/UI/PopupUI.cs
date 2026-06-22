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
        private bool _isVisible = false;
        
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
            _isVisible = true;
            
            _canvasGroup.DOKill();
            var sequence = DOTween.Sequence();

            float duration = 0.25f;
            
            sequence.Append(_canvasGroup.DOFade(1, duration));
            sequence.Join(transform.DOScale(Vector3.one, duration).From(Vector3.zero).SetEase(Ease.InElastic));
            
            sequence.Play();
        }
        
        public void Hide()
        {
            if (!_isVisible) return;
            _isVisible = false;
            
            _canvasGroup.DOKill();
            var sequence = DOTween.Sequence();
            
            float duration = 0.25f;

            sequence.Append(_canvasGroup.DOFade(0,duration));
            sequence.Join(transform.DOScale(Vector3.zero, duration).From(Vector3.one).SetEase(Ease.InBack));
            
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
            Vector3 screenPos = _mainCamera.WorldToScreenPoint(WorldPosition);
            transform.position = screenPos;
        }
    }
}