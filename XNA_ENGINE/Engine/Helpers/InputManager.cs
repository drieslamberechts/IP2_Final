using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XNA_ENGINE.Engine.Helpers
{

    public enum MouseButtons
    {
        LeftButton,
        MiddleButton,
        RightButton,
        XButton1,
        XButton2
    }

    public enum TriggerState
    {
        Pressed,
        Released,
        Down,
        Up
    }

    static class MouseStateEx
    {
        public static bool IsButtonDown(this MouseState mouseState, MouseButtons button)
        {
            return GetButtonState(mouseState, button) == ButtonState.Pressed;
        }

        public static bool IsButtonUp(this MouseState mouseState, MouseButtons button)
        {
            return GetButtonState(mouseState, button) == ButtonState.Released;
        }

        private static ButtonState GetButtonState(MouseState mouseState, MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.LeftButton:
                    return mouseState.LeftButton;
                case MouseButtons.RightButton:
                    return mouseState.RightButton;
                case MouseButtons.MiddleButton:
                    return mouseState.MiddleButton;
                case MouseButtons.XButton1:
                    return mouseState.XButton1;
                case MouseButtons.XButton2:
                    return mouseState.XButton2;
            }

            return ButtonState.Released;
        }
    }

    public class InputAction
    {
        public int Id { get; private set; }
        public Buttons? GamePadButton { get; set; }
        public MouseButtons? MouseButton { get; set; }
        public Keys? KeyButton { get; set; }
        public TriggerState TriggerButtonState { get; set; }
        public bool IsTriggered { get; set; }

        public InputAction(int id, TriggerState triggerButtonState)
        {
            Id = id;
            TriggerButtonState = triggerButtonState;
        }
    }

    public class InputManager
    {
        private readonly Dictionary<int, InputAction> _actions = new Dictionary<int, InputAction>();

        public GamePadState CurrentGamepadState { get; private set; }
        public GamePadState OldGamepadState { get; private set; }

        public MouseState CurrentMouseState { get; private set; }
        public MouseState OldMouseState { get; private set; }

        public KeyboardState CurrentKeyboardState { get; private set; }
        public KeyboardState OldKeyboardState { get; private set; }

        public void MapAction(InputAction action)
        {
            _actions[action.Id] = action;
        }

        public InputAction GetAction(int id)
        {
            return _actions.ContainsKey(id) ? _actions[id] : null;
        }

        public bool IsActionTriggered(int id)
        {
            return _actions.ContainsKey(id) && _actions[id].IsTriggered;
        }

        public void Update()
        {
            OldGamepadState = CurrentGamepadState;
            OldKeyboardState = CurrentKeyboardState;
            OldMouseState = CurrentMouseState;

            CurrentGamepadState = GamePad.GetState(PlayerIndex.One);
            CurrentKeyboardState = Keyboard.GetState();
            CurrentMouseState = Mouse.GetState();

            foreach (var inputAction in _actions.Values)
            {
                inputAction.IsTriggered = false;

                if (inputAction.TriggerButtonState == TriggerState.Down)
                {
                    //Check Gamepad
                    if (inputAction.GamePadButton.HasValue &&
                        CurrentGamepadState.IsButtonDown(inputAction.GamePadButton.Value))
                    {
                        inputAction.IsTriggered = true;
                    }

                        //Check Keyboard
                    else if (inputAction.KeyButton.HasValue &&
                             CurrentKeyboardState.IsKeyDown(inputAction.KeyButton.Value))
                    {
                        inputAction.IsTriggered = true;
                    }

                        //Check Mouse
                    else if (inputAction.MouseButton.HasValue &&
                             CurrentMouseState.IsButtonDown(inputAction.MouseButton.Value))
                    {
                        inputAction.IsTriggered = true;
                    }
                }
                else if (inputAction.TriggerButtonState == TriggerState.Pressed)
                {
                    //Check Gamepad
                    if (inputAction.GamePadButton.HasValue &&
                        CurrentGamepadState.IsButtonDown(inputAction.GamePadButton.Value) &&
                         OldGamepadState.IsButtonUp(inputAction.GamePadButton.Value))
                    {
                        inputAction.IsTriggered = true;
                    }

                        //Check Keyboard
                    else if (inputAction.KeyButton.HasValue &&
                             CurrentKeyboardState.IsKeyDown(inputAction.KeyButton.Value) &&
                         OldKeyboardState.IsKeyUp(inputAction.KeyButton.Value))
                    {
                        inputAction.IsTriggered = true;
                    }

                        //Check Mouse
                    else if (inputAction.MouseButton.HasValue &&
                             CurrentMouseState.IsButtonDown(inputAction.MouseButton.Value) &&
                         OldMouseState.IsButtonUp(inputAction.MouseButton.Value))
                    {
                        inputAction.IsTriggered = true;
                    }
                }
                else if (inputAction.TriggerButtonState == TriggerState.Released)
                {
                    //Check Gamepad
                    if (inputAction.GamePadButton.HasValue &&
                        (CurrentGamepadState.IsButtonUp(inputAction.GamePadButton.Value) &&
                         OldGamepadState.IsButtonDown(inputAction.GamePadButton.Value)))
                    {
                        inputAction.IsTriggered = true;
                    }

                        //Check Keyboard
                    else if (inputAction.KeyButton.HasValue &&
                             (CurrentKeyboardState.IsKeyUp(inputAction.KeyButton.Value) &&
                              OldKeyboardState.IsKeyDown(inputAction.KeyButton.Value)))
                    {
                        inputAction.IsTriggered = true;
                    }

                        //Check Mouse
                    else if (inputAction.MouseButton.HasValue &&
                             (CurrentMouseState.IsButtonUp(inputAction.MouseButton.Value) &&
                              OldMouseState.IsButtonDown(inputAction.MouseButton.Value)))
                    {
                        inputAction.IsTriggered = true;
                    }
                }
                else inputAction.IsTriggered = true;
            }
        }
    }
}
