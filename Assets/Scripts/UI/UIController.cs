using System;
using External;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        public static UIController Instance { get; private set; }
        
        public enum InputMethod { Mouse, Touch }
        public InputMethod currentInputMethod { get; private set; } = InputMethod.Mouse;

        
        public bool IsMobile => currentInputMethod == InputMethod.Touch;
        public bool IsKeyboard => currentInputMethod == InputMethod.Mouse;
    
        private float lastInputTime;
        public float switchCooldown = 0.2f;

        private RectTransform _touchScreenContainer;
        
        private CanvasScaler _canvasScaler;
        
        public CanvasScaler CanvasScaler => _canvasScaler;
        
        void Awake()
        {
            Instance = this;
            Joystick = GetComponentInChildren<JoystickController>();
            _canvasScaler = GetComponent<CanvasScaler>();
            
            _touchScreenContainer = Joystick.transform.parent as RectTransform;
        }

        private void Update()
        {
            Instance = this;
            
            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            {
                if (currentInputMethod != InputMethod.Touch && Time.time - lastInputTime > switchCooldown)
                {
                    currentInputMethod = InputMethod.Touch;
                    lastInputTime = Time.time;
                }
            }
        
            if ((Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) || (Keyboard.current != null && Keyboard.current.wasUpdatedThisFrame))
            {
                if (currentInputMethod != InputMethod.Mouse && Time.time - lastInputTime > switchCooldown)
                {
                    currentInputMethod = InputMethod.Mouse;
                    lastInputTime = Time.time;
                }
            }
            
            _touchScreenContainer.gameObject.SetActiveFast(currentInputMethod == InputMethod.Touch);
        }
        
        public JoystickController Joystick;
    }
}