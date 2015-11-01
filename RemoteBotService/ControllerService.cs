using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Soopah.Xna.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nuclex.Input;

namespace RemoteBotService
{
    public interface IControllerService
    {
        void Init(GameServiceContainer serviceContainer, IntPtr windowsHandle);
        bool HasGamePad();
        Vector2 GetJoystickLeftStickPosition();
        Vector2 GetJoystickRightStickPosition();
        InputManager InputManager { get; set; }
        bool IsButtonDown(Buttons button);
    }

    public class ControllerService : IControllerService
    {
        private bool _firstRequest = true;
        public InputManager InputManager
        {
            get;
            set;
        }

        public bool IsButtonDown(Buttons button)
        {
            var state = GamePad.GetState(PlayerIndex.One);
            //var state = InputManager.GamePads[4].GetState();
            return state.IsButtonDown(button);
        }

        public ControllerService()
        {

        }

        public void Init(GameServiceContainer serviceContainer, IntPtr windowsHandle)
        {
            InputManager = new InputManager(serviceContainer, windowsHandle);
        }

        public bool HasGamePad()
        {
            return InputManager.GamePads.Count > 0;
            //return DirectInputGamepad.Gamepads.Count > 0;
        }

        public Vector2 GetJoystickLeftStickPosition()
        {
            if (_firstRequest)
            {
                _firstRequest = false;
                return new Vector2(0, 0);
            }
            //DirectInputThumbSticks thumbSticks = DirectInputGamepad.Gamepads[0].ThumbSticks;
            var state = GamePad.GetState(PlayerIndex.One);
            //var state = InputManager.GamePads[0].GetState();

            return state.ThumbSticks.Left;
        }

        public Vector2 GetJoystickRightStickPosition()
        {
            if (_firstRequest)
            {
                _firstRequest = false;
                return new Vector2(0, 0);
            }
            var state = GamePad.GetState(PlayerIndex.One);
            //Microsoft.Xna.Framework.Input. DirectInputThumbSticks thumbSticks = DirectInputGamepad.Gamepads[0].ThumbSticks;
            //var state = InputManager.GamePads[0].GetState();

            return state.ThumbSticks.Right;
        } 
    }
}
