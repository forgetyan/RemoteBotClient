using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteBotService
{
    public interface IAimService : IRoverElement
    {

    }
    public class AimService : IAimService
    {
        public Texture2D _aimTexture { get; set; }
        readonly Vector2 _aimPosition = new Vector2(247, 214);
        public void Initialize(string roverIp)
        {

        }

        public void LoadContent(ContentManager content)
        {
            _aimTexture = content.Load<Texture2D>("Aim");
        }

        public void Update(GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice, GameTime gameTime)
        {

        }

        public void UnloadContent()
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_aimTexture, _aimPosition, Color.LightGreen * 0.8f);
        }

        public void Dispose()
        {

        }
    }
}
