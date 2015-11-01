using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RemoteBotService
{
    public interface IHeadlightService : IRoverElement
    {
    }

    public class HeadlightService : IHeadlightService
    {
        private readonly INetworkService _networkService;
        private Texture2D _headlightTexture;
        readonly Vector2 _headlightPosition = new Vector2(750, 500);
        public bool _wasPressed = false;

        public HeadlightService(INetworkService networkService)
        {
            _networkService = networkService;
        }

        public void Initialize(string roverIp)
        {

        }

        public void LoadContent(ContentManager content)
        {
            _headlightTexture = content.Load<Texture2D>("Headlight");
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Update(GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && !_wasPressed)
            {
                _wasPressed = true;
                _networkService.SendMessage("ON");
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && _wasPressed)
            {
                _wasPressed = false;
                _networkService.SendMessage("OFF");
            }
        }

        public void UnloadContent()
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_headlightTexture, _headlightPosition, (_wasPressed ? Color.Blue : Color.Black));
        }

        public void Dispose()
        {
            
        }
    }
}
