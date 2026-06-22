using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace GameInputSystem
{
    /// <summary>
    /// Static utility class for centralized input handling across the game.
    /// Provides simplified access to player controls and device detection.
    /// </summary>
    public static class InputSystem
    {
        #region Public Fields
        /// <summary>
        /// Global player controls instance.
        /// </summary>
        public static PlayerControls Controls;

        /// <summary>
        /// Currently detected input device type.
        /// </summary>
        public static DeviceType CurrentDeviceType
        {
            get
            {
                CheckInputDevice();
                
                return _lastDeviceType;
                
            }
            private set
            {
                _lastDeviceType = value;
            }
        }
        
        /// <summary>
        /// Event triggered when input device changes.
        /// </summary>
        public static UnityEvent OnDeviceChanged;
        #endregion

        #region Nested Types
        /// <summary>
        /// Types of input devices supported by the game.
        /// </summary>
        public enum DeviceType
        {
            KeyboardMouse,
            Xbox,
            PlayStation,
            Switch,
            GenericGamepad,
            Unknown
        }
        #endregion

        #region Private Fields
        private static readonly Dictionary<Type, DeviceType> _baseDeviceMap = new()
        {
            { typeof(Keyboard), DeviceType.KeyboardMouse },
            { typeof(Mouse), DeviceType.KeyboardMouse },
            { typeof(Gamepad), DeviceType.GenericGamepad }
        };

        private static InputUser _user;
        private static InputDevice _lastDevice;
        private static DeviceType _lastDeviceType = DeviceType.KeyboardMouse;
        #endregion

        #region Initialization
        /// <summary>
        /// Static constructor initializes the input system.
        /// </summary>
        static InputSystem()
        {
            OnDeviceChanged = new UnityEvent();
            Controls = new PlayerControls();
            Controls.Enable();
            
            
        }



        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Reinitialize()
        {
            if (Controls != null)
            {
                Controls.Player.Disable();
                Controls.UI.Disable();
                Controls.Dispose();
            }

            // Reset static listeners to avoid retaining scene objects across reloads.
            OnDeviceChanged = new UnityEvent();

            Controls = new PlayerControls();
            Controls.Enable();

            _user = default;
            _lastDevice = null;
            _lastDeviceType = DeviceType.KeyboardMouse;
        }
        
        private static void SetDevice(DeviceType type)
        {
            if (_lastDeviceType == type) return;
            _lastDeviceType = type;
            Debug.Log($"Input device switched to: {type}");
            OnDeviceChanged?.Invoke();
        }

        private static void CheckInputDevice()
        {
            // Check gamepad first (highest priority for most games)
            if (Gamepad.current != null)
            {
                var gp = Gamepad.current;
                if (gp.leftStick.ReadValue().sqrMagnitude > 0.01f ||
                    gp.rightStick.ReadValue().sqrMagnitude > 0.01f ||
                    gp.IsPressed())
                {
                    SetDevice(DeviceType.GenericGamepad);
                    return;
                }
            }

            // uncomment if mobile support is needed
            
            // // Check touch
            // if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            // {
            //     SetDevice(DeviceType.Unknown);
            //     return;
            // }

            // Check keyboard/mouse
            if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
            {
                SetDevice(DeviceType.KeyboardMouse);
                return;
            }

            if (Mouse.current != null &&
                (Mouse.current.delta.ReadValue().sqrMagnitude > 0 ||
                 Mouse.current.leftButton.wasPressedThisFrame))
            {
                SetDevice(DeviceType.KeyboardMouse); // treat mouse+KB as the same
                return;
            }
        }
        
        
        #endregion

        #region Input Properties
        /// <summary>
        /// Gets the current movement input vector.
        /// </summary>
        public static Vector2 Move => Controls.Player.Move.ReadValue<Vector2>();
        
        /// <summary>
        /// Gets the current look/camera input vector.
        /// </summary>
        public static Vector2 Look => Controls.Player.Look.ReadValue<Vector2>();

        /// <summary>
        /// Returns true if attack was performed this frame.
        /// </summary>
        public static bool Interact => Controls.Player.Interact.WasPerformedThisFrame();

        
        #endregion
    }
}
