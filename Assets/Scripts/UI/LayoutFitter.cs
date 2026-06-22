using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class LayoutFitter : MonoBehaviour
{
    private LayoutGroup _layoutGroup;
    private RectTransform _rectTransform;
    
    
    public RectOffset _ogPadding;
    public float StartWidthSize = 100f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (_layoutGroup == null)
        {
            _layoutGroup = GetComponent<LayoutGroup>();
        }
        
        if (_rectTransform == null)
        {
            _rectTransform = transform as RectTransform;
        }
        
        float scaleFactor = _rectTransform.rect.width / StartWidthSize;
        
        _layoutGroup.padding.left = Mathf.RoundToInt(_ogPadding.left * scaleFactor);
        _layoutGroup.padding.right = Mathf.RoundToInt(_ogPadding.right * scaleFactor);
        _layoutGroup.padding.top = Mathf.RoundToInt(_ogPadding.top * scaleFactor);
        _layoutGroup.padding.bottom = Mathf.RoundToInt(_ogPadding.bottom * scaleFactor);
        
        // gap
        if (_layoutGroup is HorizontalOrVerticalLayoutGroup horizontalOrVerticalLayoutGroup)
        {
            horizontalOrVerticalLayoutGroup.spacing = Mathf.RoundToInt((_ogPadding.left/2f) * scaleFactor);
        }
    }
}
