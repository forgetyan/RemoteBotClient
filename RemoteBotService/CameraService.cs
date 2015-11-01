using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteBotService
{
    public interface ICameraService : IRoverElement
    {


    }
    public class CameraService : ICameraService
    {
        private const int MAX_CAMERA_POSITION = 100;
        private const int MIN_CAMERA_POSITION = -100;
        private float _cameraXAxis = 0;
        private float _cameraYAxis = 0;
        private readonly IFontService _fontService;
        private readonly INetworkService _networkService;
        private readonly IControllerService _controllerService;
        private Vector2 lastPosition;
        private double lastElapsedGameTime = 0;

        public CameraService(IFontService fontService, INetworkService networkService, IControllerService controllerService)
        {
            _fontService = fontService;
            _networkService = networkService;
            _controllerService = controllerService;
            lastPosition = new Vector2();
        }

        public void Initialize(string roverIp)
        {
            
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            
        }

        public void Update(Microsoft.Xna.Framework.GraphicsDeviceManager graphics, Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (lastElapsedGameTime + 100 < gameTime.TotalGameTime.TotalMilliseconds)
            {
                lastElapsedGameTime = gameTime.TotalGameTime.TotalMilliseconds;
                int elapsedGameTime = gameTime.ElapsedGameTime.Milliseconds;

                bool cameraPositionChanged = false;

                Vector2 position = _controllerService.GetJoystickRightStickPosition();

                if (position.X != lastPosition.X || position.Y != lastPosition.Y)
                {
                    cameraPositionChanged = true;
                    // Changement total en 2 secondes
                    //_cameraXAxis += position.X / 600 * elapsedGameTime * MAX_CAMERA_POSITION;
                    //if(_cameraXAxis > MAX_CAMERA_POSITION)
                    //{
                    //    _cameraXAxis = MAX_CAMERA_POSITION;
                    //}
                    //if (_cameraXAxis < MIN_CAMERA_POSITION)
                    //{
                    //    _cameraXAxis = MIN_CAMERA_POSITION;
                    //}
                    //_cameraYAxis += position.Y / 600 * elapsedGameTime * MAX_CAMERA_POSITION;
                    //if (_cameraYAxis > MAX_CAMERA_POSITION)
                    //{
                    //    _cameraYAxis = MAX_CAMERA_POSITION;
                    //}
                    //if (_cameraYAxis < MIN_CAMERA_POSITION)
                    //{
                    //    _cameraYAxis = MIN_CAMERA_POSITION;
                    //}
                }
                if (position.X != lastPosition.X || position.Y != lastPosition.Y)
                {
                    lastPosition.X = position.X;
                    lastPosition.Y = position.Y;
                    _networkService.SendMessage("CAMERA_POS" + "|" + Math.Round(position.X * 100, 0).ToString() + "|" + Math.Round(position.Y * 100, 0).ToString());
                    //_networkService.SendMessage("CAMERA_POS" + "|" + Math.Round(_cameraXAxis, 0).ToString() + "|" + Math.Round(_cameraYAxis, 0).ToString());
                }
            }
        }

        public void UnloadContent()
        {
            
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            string cameraPositionString = "CAM: " + _cameraXAxis.ToString("0") + " - " + _cameraYAxis.ToString("0");
            var font = _fontService.GetDefaultFont();
            Vector2 size = font.MeasureString(cameraPositionString);
            Vector2 pos = new Vector2(120f , 500f);
            Vector2 origin = size * 0.5f;
            spriteBatch.DrawString(font, cameraPositionString, pos, Color.LightGreen);
        }

        public void Dispose()
        {
            
        }
    }
}
