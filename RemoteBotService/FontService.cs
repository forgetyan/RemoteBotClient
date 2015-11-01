using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RemoteBotService
{
    public class FontService :IFontService
    {
        private SpriteFont _font;

        public void Initialize(string roverIp)
        {
            
        }

        public void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("DefaultFont");
        }

        public void Update(GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice, GameTime gameTime)
        {
        }

        public void UnloadContent()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
        }

        public SpriteFont GetDefaultFont()
        {
            return _font;
        }

        public void Dispose()
        {
            
        }
    }

    public interface IFontService : IRoverElement
    {
        SpriteFont GetDefaultFont();
    }
}
