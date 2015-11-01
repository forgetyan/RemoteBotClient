using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Speech.Synthesis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Ninject;
using RemoteBotService;

//using RemoteBotService;

namespace XnaRemoteBot
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        private List<IRoverElement> _roverElementList;
        //INetworkService _networkService;
        IControllerService _controllerService;

        public MainGame()
        {
            //var input = new InputManager(Services, Window.Handle);
            IKernel kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            var elementList = kernel.GetAll<IRoverElement>();
            _controllerService = kernel.Get<IControllerService>();
            _controllerService.Init(Services, Window.Handle);
            Components.Add(_controllerService.InputManager);
            _roverElementList = elementList.ToList();
            _graphics = new GraphicsDeviceManager(this);
            //_mjpegService = kernel.Get<IMjpegService>();
            //_networkService = kernel.Get<INetworkService>();
            //List<INetworkListener> networkListenerList = kernel.Get<List<INetworkListener>>();
            //_networkService.NetworkListenerList = networkListenerList;
            //_batteryControlService = kernel.Get<IBatteryControlService>();
            
            //_headlightService = kernel.Get<IHeadlightService>();// new HeadlightService(_networkService);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            string roverIp = Microsoft.VisualBasic.Interaction.InputBox("Please enter IP of the Rover", "IP of the Rover", "192.168.0.113", -1, -1);
            if (!String.IsNullOrWhiteSpace(roverIp))
            {
                _roverElementList.ForEach(e => e.Initialize(roverIp));
                _graphics.PreferredBackBufferHeight = 768;
                _graphics.PreferredBackBufferWidth = 1024;
                _graphics.ApplyChanges();
            }
            else
            {
                ExitGame();
            }
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _roverElementList.ForEach(e => e.LoadContent(Content));

            //using (SpeechSynthesizer synth = new SpeechSynthesizer())
            //{
            //    synth.SelectVoice("Microsoft Zira Desktop");
            //    synth.Speak("Remote bot activated");
            //}
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            _roverElementList.ForEach(e => e.UnloadContent());
        }
        
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                ExitGame();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                ExitGame();
            }
                

            // TODO: Add your update logic here
            _roverElementList.ForEach(e => e.Update(_graphics, GraphicsDevice, gameTime));
            //UpdateSprite(gameTime);
            // Change the resolution dynamically based on input
            if (GamePad.GetState(PlayerIndex.One).Buttons.A ==
                ButtonState.Pressed)
            {
                _graphics.PreferredBackBufferHeight = 768;
                _graphics.PreferredBackBufferWidth = 1024;
                _graphics.ApplyChanges();
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.B ==
                ButtonState.Pressed)
            {
                _graphics.PreferredBackBufferHeight = 600;
                _graphics.PreferredBackBufferWidth = 800;
                _graphics.ApplyChanges();
            }

            //if (Keyboard.GetState().IsKeyDown(Keys.A))
            //{
            //    _graphics.PreferredBackBufferHeight = 1080;
            //    _graphics.PreferredBackBufferWidth = 1920;
            //    _graphics.IsFullScreen = true;
            //    _graphics.ApplyChanges();
            //}

            //if (Keyboard.GetState().IsKeyDown(Keys.B))
            //{
            //    _graphics.PreferredBackBufferHeight = 768;
            //    _graphics.PreferredBackBufferWidth = 1024;
            //    _graphics.IsFullScreen = false;
            //    _graphics.ApplyChanges();
            //}
            base.Update(gameTime);
        }

        private void ExitGame()
        {
            this.Exit();
        }

        protected override void OnExiting(Object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
            // Stop the threads
            _roverElementList.ForEach(e => e.Dispose());
        }

        //void UpdateSprite(GameTime gameTime)
        //{
        //    Vector2 spriteSpeed = new Vector2(100,10);
        //    //const float spriteSpeed = 1;
        //    // Move the sprite by speed, scaled by elapsed time.
        //    _spritePosition +=
        //        spriteSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        //    int MaxX =
        //        _graphics.GraphicsDevice.Viewport.Width - _cameraTexture.Width;
        //    int MinX = 0;
        //    int MaxY =
        //        _graphics.GraphicsDevice.Viewport.Height - _cameraTexture.Height;
        //    int MinY = 0;

        //    // Check for bounce.
        //    if (_spritePosition.X > MaxX)
        //    {
        //        spriteSpeed.X *= -1;
        //        _spritePosition.X = MaxX;
        //    }

        //    else if (_spritePosition.X < MinX)
        //    {
        //        spriteSpeed.X *= -1;
        //        _spritePosition.X = MinX;
        //    }

        //    if (_spritePosition.Y > MaxY)
        //    {
        //        spriteSpeed.Y *= -1;
        //        _spritePosition.Y = MaxY;
        //    }

        //    else if (_spritePosition.Y < MinY)
        //    {
        //        spriteSpeed.Y *= -1;
        //        _spritePosition.Y = MinY;
        //    }
        //}

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Matrix spriteScale;

            float yScreenScale = _graphics.GraphicsDevice.Viewport.Height / 600f;
            float xScreenScale = _graphics.GraphicsDevice.Viewport.Width / 800f;
            spriteScale = Matrix.CreateScale(xScreenScale, yScreenScale, 1);
            // Draw the sprite.
            //_spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, spriteScale);
            _roverElementList.ForEach(e => e.Draw(_spriteBatch));
            //_spriteBatch.Draw(_cameraTexture, _spritePosition, null, Color.White, 0f, Vector2.Zero, new Vector2(2.5f), SpriteEffects.None, 1f);
            //_spriteBatch.DrawString(_font, _mjpegService.NbPerSecond.ToString("#.##"), new Vector2(0, 0), Color.White);
            //_spriteBatch.DrawString(_font, _cameraTexture.Width.ToString(), new Vector2(0, 0), Color.White);
            _spriteBatch.End();



            base.Draw(gameTime);
        }
    }
}
