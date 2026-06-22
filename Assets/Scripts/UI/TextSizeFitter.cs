using TMPro;
using UnityEngine;

[ExecuteAlways]
public class TextSizeFitter : MonoBehaviour
{
    public float StartFontSize = 14f;
    public float StartWidthSize = 100f;
    
    private RectTransform _rectTransform;
    private TMP_Text _text;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_rectTransform == null)
        {
            _rectTransform = transform as RectTransform;
        }
        
        if (_text == null)
        {
            _text = GetComponent<TMP_Text>();
        }
        
        float scaleFactor = _rectTransform.rect.width / StartWidthSize;
        float newFontSize = StartFontSize * scaleFactor;
        
        _text.fontSize = newFontSize;
    }
}
