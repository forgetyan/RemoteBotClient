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

    public interface IMotorService : IRoverElement
    {

    }

    public class MotorService : IMotorService
    {
        private const int MAX_THROTTLE_POSITION = 100;
        private Texture2D _roverTopTexture { get; set; }
        readonly Vector2 _roverTopPosition = new Vector2(25, 500);
        private readonly IFontService _fontService;
        private readonly INetworkService _networkService;
        private readonly IControllerService _controllerService;
        private Texture2D _motorDotTexture;
        private int _keyboardLastLeftTrack = 0;
        private int _keyboardLastRightTrack = 0;
        private int _joystickLastLeftTrack = 0;
        private int _joystickLastRightTrack = 0;
        private int _usedRightTrack = 0;
        private int _usedLeftTrack = 0;
        private float _speedMultiplier = 0.4f;
        private double lastElapsedGameTime = 0;

        public MotorService(IFontService fontService, INetworkService networkService, IControllerService controllerService)
        {
            _fontService = fontService;
            _networkService = networkService;
            _controllerService = controllerService;
        }

        public void Initialize(string roverIp)
        {

        }

        public void LoadContent(ContentManager content)
        {
            _roverTopTexture = content.Load<Texture2D>("RoverTop");
            _motorDotTexture = content.Load<Texture2D>("MotorDot");
        }

        public void Update(GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            if (lastElapsedGameTime + 100 < gameTime.TotalGameTime.TotalMilliseconds)
            {
                lastElapsedGameTime = gameTime.TotalGameTime.TotalMilliseconds;

                int rightTrack = 0;
                int leftTrack = 0;

                GetKeyboardTrackPosition(ref leftTrack, ref rightTrack);


                leftTrack = (int)((float)leftTrack * _speedMultiplier);
                rightTrack = (int)((float)rightTrack * _speedMultiplier);
                bool usedTrackChanged = false;
                if (_keyboardLastLeftTrack != leftTrack || _keyboardLastRightTrack != rightTrack)
                {
                    usedTrackChanged = true;
                    _keyboardLastLeftTrack = leftTrack;
                    _keyboardLastRightTrack = rightTrack;
                    _usedLeftTrack = leftTrack;
                    _usedRightTrack = rightTrack;
                }

                GetJoystickTrackPosition(ref leftTrack, ref rightTrack);
                leftTrack = (int)((float)leftTrack * _speedMultiplier);
                rightTrack = (int)((float)rightTrack * _speedMultiplier);
                if (_joystickLastLeftTrack != leftTrack || _joystickLastRightTrack != rightTrack)
                {
                    usedTrackChanged = true;
                    _joystickLastLeftTrack = leftTrack;
                    _joystickLastRightTrack = rightTrack;
                    _usedLeftTrack = leftTrack;
                    _usedRightTrack = rightTrack;
                }
                if (usedTrackChanged)
                {
                    _networkService.SendMessage("MOTOR" + (_usedLeftTrack >= 0 ? "F" : "R") + Math.Abs(_usedLeftTrack).ToString() + "|" + (_usedRightTrack >= 0 ? "F" : "R") + Math.Abs(_usedRightTrack).ToString());
                }
            }
        }

        private void GetJoystickTrackPosition(ref int leftTrack, ref int rightTrack)
        {
            var position = _controllerService.GetJoystickLeftStickPosition();
            float turning = position.X * MAX_THROTTLE_POSITION;
            float forward = position.Y * MAX_THROTTLE_POSITION;
            leftTrack = (int)Math.Round(forward + turning, 0);
            rightTrack = (int)Math.Round(forward - turning, 0);
            if (leftTrack > MAX_THROTTLE_POSITION)
                leftTrack = MAX_THROTTLE_POSITION;
            if (leftTrack < -MAX_THROTTLE_POSITION)
                leftTrack = -MAX_THROTTLE_POSITION;
            if (rightTrack > MAX_THROTTLE_POSITION)
                rightTrack = MAX_THROTTLE_POSITION;
            if (rightTrack < -MAX_THROTTLE_POSITION)
                rightTrack = -MAX_THROTTLE_POSITION;
        }

        private void GetKeyboardTrackPosition(ref int leftTrack, ref int rightTrack)
        {
            int forward = 0;
            int turning = 0;
            // Verify WASD key press
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                // Going forward
                forward += MAX_THROTTLE_POSITION;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                // Going backward
                forward -= MAX_THROTTLE_POSITION;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                // Going left
                turning -= MAX_THROTTLE_POSITION;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                // Going right
                turning += MAX_THROTTLE_POSITION;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                _speedMultiplier = 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                _speedMultiplier = 0.2f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                _speedMultiplier = 0.3f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D4))
            {
                _speedMultiplier = 0.4f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D5))
            {
                _speedMultiplier = 0.5f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D6))
            {
                _speedMultiplier = 0.6f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D7))
            {
                _speedMultiplier = 0.7f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D8))
            {
                _speedMultiplier = 0.8f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D9))
            {
                _speedMultiplier = 0.9f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D0))
            {
                _speedMultiplier = 1;
            }
            leftTrack = forward + turning;
            rightTrack = forward - turning;
            if (leftTrack > MAX_THROTTLE_POSITION)
                leftTrack = MAX_THROTTLE_POSITION;
            if (leftTrack < -MAX_THROTTLE_POSITION)
                leftTrack = -MAX_THROTTLE_POSITION;
            if (rightTrack > MAX_THROTTLE_POSITION)
                rightTrack = MAX_THROTTLE_POSITION;
            if (rightTrack < -MAX_THROTTLE_POSITION)
                rightTrack = -MAX_THROTTLE_POSITION;
        }


        public void UnloadContent()
        {

        }

        private void DrawTrackDots(SpriteBatch spriteBatch, int trackValue, float startX, float startY)
        {
            Vector2 dotPosition = new Vector2(startX, startY);
            // For each track Dots
            for (int i = 0; i < 11; i++)
            {
                Color dotColor;
                if (i < 5)
                {
                    // Backward
                    if (Math.Round(trackValue * 10 / _speedMultiplier / MAX_THROTTLE_POSITION / 2, 0) <= i - 5)
                    {
                        dotColor = Color.LightGreen;
                    }
                    else
                    {
                        dotColor = Color.DarkGreen;
                    }
                }
                else if (i > 5)
                {
                    // Forward
                    if (Math.Round(trackValue * 10 / _speedMultiplier / MAX_THROTTLE_POSITION / 2, 0) >= i - 5)
                    {
                        dotColor = Color.LightGreen;
                    }
                    else
                    {
                        dotColor = Color.DarkGreen;
                    }
                }
                else // Center Dot
                {
                    dotColor = Color.Yellow;
                }
                spriteBatch.Draw(_motorDotTexture, dotPosition, dotColor * 0.9f);
                dotPosition.Y = dotPosition.Y - 6;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_roverTopTexture, new Rectangle((int)_roverTopPosition.X, (int)_roverTopPosition.Y, 63, 73), Color.White * 0.8f);
            DrawTrackDots(spriteBatch, _usedRightTrack, _roverTopPosition.X + 65, _roverTopPosition.Y + 64);
            DrawTrackDots(spriteBatch, _usedLeftTrack, _roverTopPosition.X - 10, _roverTopPosition.Y + 64);

            var font = _fontService.GetDefaultFont();
            string kphString = "0 KPH";
            Vector2 size = font.MeasureString(kphString);
            spriteBatch.DrawString(_fontService.GetDefaultFont(), kphString, new Vector2(700 - size.X, 160), Color.LightGreen);
        }

        public void Dispose()
        {

        }


    }
}
