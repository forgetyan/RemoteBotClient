using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RemoteBotService
{
    public interface IRoverElement : IDisposable
    {
        void Initialize(string roverIp);
        void LoadContent(ContentManager content);
        void Update(GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice, GameTime gameTime);
        void UnloadContent();
        void Draw(SpriteBatch spriteBatch);
    }
}
