using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Ninject;

namespace RemoteBotService
{
    public class CompassService : ICompassService
    {
        private readonly IFontService _fontService;
        private Texture2D _compassTexture;
        private Texture2D _compassHeading;
        [Inject]
        public INetworkService NetworkService { get; set; }
        private readonly Vector2 _compassHeadingPosition = new Vector2(400, 50);
        public double AccelerometerX { get; set; }
        public double AccelerometerY { get; set; }
        public double AccelerometerZ { get; set; }

        public double MagnetometerX { get; set; }
        public double MagnetometerY { get; set; }
        public double MagnetometerZ { get; set; }
        public double MagnetometerOrientation { get; set; }

        private double HUDOrientation { get; set; }

        public bool WaitingForAnswer { get; set; }

        public CompassService(IFontService fontService)
        {
            _fontService = fontService;
            HUDOrientation = 0;
        }

        public void Initialize(string roverIp)
        {

        }

        public void LoadContent(ContentManager content)
        {
            _compassTexture = content.Load<Texture2D>("Compass");
            _compassHeading = content.Load<Texture2D>("CompassHeading");
        }

        public void Update(GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            if (!WaitingForAnswer && NetworkService.IsConnected)
            {
                WaitingForAnswer = true;
                NetworkService.SendMessage("ASK_COMPASS");
            }
            // Calculate distance between HUD and Compass Orientation
            double distance = HUDOrientation - MagnetometerOrientation;
            double distance2 = HUDOrientation - MagnetometerOrientation + 360;
            double distance3 = HUDOrientation - MagnetometerOrientation - 360;

            double shortestDistance = distance;
            if (Math.Abs(shortestDistance) > Math.Abs(distance2))
            {
                shortestDistance = distance2;
            }
            if (Math.Abs(shortestDistance) > Math.Abs(distance3))
            {
                shortestDistance = distance3;
            }

            HUDOrientation = HUDOrientation - (shortestDistance * gameTime.ElapsedGameTime.TotalMilliseconds / 300);
            if (HUDOrientation > 360)
            {
                HUDOrientation -= 360;
            }
            else if (HUDOrientation < 0)
            {
                HUDOrientation += 360;
            }
        }

        public void UnloadContent()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            const int compassWidth = 865;
            float compassX = (float)(-HUDOrientation * compassWidth / 365) - 28;
            Vector2 compassPosition = new Vector2(compassX, 0);
            Vector2 secondCompassPosition;
            if (compassX > 0)
            {
                secondCompassPosition = new Vector2(compassX - compassWidth, 0);
            }
            else
            {
                secondCompassPosition = new Vector2(compassX + compassWidth, 0);
            }

            spriteBatch.Draw(_compassTexture, compassPosition, Color.LightGreen);
            spriteBatch.Draw(_compassTexture, secondCompassPosition, Color.LightGreen);
            spriteBatch.Draw(_compassHeading, new Rectangle(397, 50, 20, 11), Color.LightGreen);
            //string orientationString = MagnetometerX.ToString("0") + " <-> " + MagnetometerY.ToString("0") + " <-> " + MagnetometerZ.ToString("0") + " <-> " + MagnetometerOrientation.ToString("0");
            string orientationString = MagnetometerOrientation.ToString("0");
            var font = _fontService.GetDefaultFont();
            Vector2 size = font.MeasureString(orientationString);
            Vector2 pos = new Vector2(406f - (size.X / 2), 60f);
            Vector2 origin = size * 0.5f;
            spriteBatch.DrawString(font, orientationString, pos, Color.LightGreen);
        }

        public void ReceiveNetworkMessage(string responseData)
        {
            if (responseData.StartsWith("COMPASS:"))
            {
                WaitingForAnswer = false;
                string parameterResult = responseData.Substring(8);
                var compassResult = (Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(parameterResult);
                AccelerometerX = double.Parse(compassResult[0][0].ToString());
                AccelerometerY = double.Parse(compassResult[0][1].ToString());
                AccelerometerZ = double.Parse(compassResult[0][2].ToString());

                MagnetometerX = double.Parse(compassResult[1][0].ToString());
                MagnetometerY = double.Parse(compassResult[1][1].ToString());
                MagnetometerZ = double.Parse(compassResult[1][2].ToString());
                MagnetometerOrientation = double.Parse(compassResult[1][3].ToString());
                //MagnetometerOrientation = double.Parse(parameterResult, CultureInfo.InvariantCulture);
            }
        }

        public void Dispose()
        {

        }
    }

    public interface ICompassService : IRoverElement, INetworkListener
    {
    }
}
