using System;
using UI;
using UnityEngine;

public class PopupObject : MonoBehaviour
{
    public float PopupDistance;
    
    private PopupUI _popupUI;
    
    public PopupUI.PopupData Data;
    public Vector3 PositionOffset;
    
    void Start()
    {
        _popupUI = PopupUI.ShowNewPopup(Data);
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) < PopupDistance)
        {
            _popupUI.WorldPosition = transform.position+PositionOffset;
            _popupUI.Show();
        }
        else
        {
            _popupUI.Hide();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, PopupDistance);
    }
#endif

}
