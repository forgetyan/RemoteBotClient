using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MjpegProcessor;
using Color = Microsoft.Xna.Framework.Color;

namespace RemoteBotService
{
    public interface IMjpegService : IRoverElement
    {
        decimal NbPerSecond { get; set; }
        void RemoteBotService();
        void StartDecoder(string roverIp);
        void StopDecoder();
        System.Drawing.Bitmap GetCurrentImage();
    }

    public class MjpegService : IMjpegService
    {
        Vector2 _spritePosition = Vector2.Zero;
        private SpriteFont _font;
        MjpegDecoder _mjpegDecoder;
        System.Drawing.Bitmap _lastBitmap = null;
        Stopwatch sw = new Stopwatch();
        private Texture2D _cameraTexture;
        private IFontService _fontService;
        private readonly IControllerService _controllerService;
        private bool _zoomEnabled = false;

        public MjpegService(IFontService fontService, IControllerService controllerService)
        {
            _fontService = fontService;
            _controllerService = controllerService;
        }

        public decimal NbPerSecond { get; set; }

        public void RemoteBotService()
        {

        }

        public void StartDecoder(string roverIp)
        {
            _mjpegDecoder = new MjpegDecoder();
            _mjpegDecoder.FrameReady += mjpeg_FrameReady;
            _mjpegDecoder.ParseStream(new Uri("http://" + roverIp + ":8080/?action=stream"), "", "");
        }

        public void StopDecoder()
        {
            if (_mjpegDecoder != null)
                _mjpegDecoder.StopStream();
        }

        // Private method
        private void mjpeg_FrameReady(object sender, FrameReadyEventArgs e)
        {
            sw.Stop();
            NbPerSecond = sw.ElapsedMilliseconds;
            if (sw.ElapsedMilliseconds == 0)
            {
                NbPerSecond = 9999;
            }
            else
            {
                NbPerSecond = 1000M / sw.ElapsedMilliseconds;
            }
            sw = new Stopwatch();
            sw.Start();
            _lastBitmap = e.Bitmap;
        }

        public System.Drawing.Bitmap GetCurrentImage()
        {
            var image = _lastBitmap;
            _lastBitmap = null;
            return image;
        }

        public void Initialize(string roverIp)
        {
            StartDecoder(roverIp);
        }

        public void LoadContent(ContentManager content)
        {
            //_cameraTexture = content.Load<Texture2D>("Megaman16");
            _font = content.Load<SpriteFont>("DefaultFont");
        }

        public void Update(GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            var state = Mouse.GetState();
            _zoomEnabled = state.RightButton == ButtonState.Pressed || _controllerService.IsButtonDown(Buttons.RightShoulder);
            var videoBitmap = GetCurrentImage();
            if (videoBitmap != null)
            {
                MemoryStream memoryStream = new MemoryStream();
                videoBitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                _cameraTexture = Texture2D.FromStream(graphicsDevice, memoryStream);
            }
        }

        public Matrix SpriteScale { get; set; }

        public void UnloadContent()
        {
            StopDecoder();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            string zoomText = "1x";
            Rectangle position;
            if (_zoomEnabled)
            {
                int zoomLevel = 2;
                int width = zoomLevel * 800;
                int height = zoomLevel * 600;
                zoomText = zoomLevel + "x";
                position = new Rectangle(-width / 4, -height / 4, width, height);
                //spriteBatch.Draw(_cameraTexture, new Rectangle(-400, -300, 1600, 1200), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            }
            else
            {
                position = new Rectangle(0, 0, 800, 600);
                //spriteBatch.Draw(_cameraTexture, _spritePosition, null, Color.White, 0f, Vector2.Zero, new Vector2(2.5f), SpriteEffects.None, 1f);
            }
            if (_cameraTexture != null)
                spriteBatch.Draw(_cameraTexture, position, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            var font = _fontService.GetDefaultFont();
            Vector2 size = font.MeasureString(zoomText);
            spriteBatch.DrawString(_font, zoomText, new Vector2(700 - size.X, 200), Color.LightGreen);
        }

        public void Dispose()
        {
            StopDecoder();
        }
    }
}
