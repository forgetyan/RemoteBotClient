using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RemoteBotService
{
    public interface IBatteryControlService : IRoverElement
    {
    }

    public class BatteryControlService : IBatteryControlService
    {
        private Texture2D _batteryTexture;
        private Texture2D _gaugeTexture;
        private Texture2D _arrowDownTexture;
        readonly Vector2 _batteryPosition = new Vector2(770, 100);
        readonly Vector2 _gaugePosition = new Vector2(770, 140);
        readonly Vector2 _arrowDownOriginalPosition = new Vector2(785, 100);
        private Vector2 _arrowDownPosition = new Vector2(785, 100);

        private double _millisecondsPassed = 0;
        private int _arrowState = 0;

        public void Initialize(string roverIp)
        {
            
        }

        public void LoadContent(ContentManager content)
        {
            _batteryTexture = content.Load<Texture2D>("Battery");
            _gaugeTexture = content.Load<Texture2D>("Gauge");
            _arrowDownTexture = content.Load<Texture2D>("ArrowDown");
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Update(GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            _millisecondsPassed += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_millisecondsPassed > 300)
            {
                _arrowState ++;
                if (_arrowState > 2)
                {
                    _arrowState = 0;
                }
                _millisecondsPassed = _millisecondsPassed - 300;

            }
            switch (_arrowState)
            {
                case 0:
                    _arrowDownPosition.Y = _arrowDownOriginalPosition.Y;
                    break;
                case 1:
                    _arrowDownPosition.Y = _arrowDownOriginalPosition.Y + 7;
                    break;
                case 2:
                    _arrowDownPosition.Y = _arrowDownOriginalPosition.Y + 14;
                    break;
            }
            
        }

        public void UnloadContent()
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_batteryTexture, _batteryPosition, Color.LightGreen * 0.8f);
            spriteBatch.Draw(_gaugeTexture, _gaugePosition, Color.LightGreen * 0.8f);
            spriteBatch.Draw(_arrowDownTexture, _arrowDownPosition, Color.Red * 0.8f);
        }

        public void Dispose()
        {
            
        }
    }
}
