using Controllers.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Misc;
using System;
using System.IO;

namespace Peggley
{
    public class Game1 : Game
    {
        // Template code
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Should automatically rename to Game Name
        PeggleyGame _thisGame;

        // Textures
        // This has been migrated to AssetManager.cs
        //public static Dictionary<string, Texture2D> _textures { get; private set; }

        // Template code
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        // Early Initialize, anything not requiring textures
        protected override void Initialize()
        {
            // Default Monitor Settings
            // TODO: Add monitor size switcherooing
            _graphics.HardwareModeSwitch = false;
            _graphics.IsFullScreen = false;
            WindowManager.SetDefaultDimensions();
            _graphics.PreferredBackBufferWidth = WindowManager.WindowXDimension;
            _graphics.PreferredBackBufferHeight = WindowManager.WindowYDimension;
            _graphics.ApplyChanges();

            // Initialize mouse inputs
            // Keyboard doesn't need initialization because monogame is inconsistant
            MouseController.Initialize();

            // Initialize TextureManager
            AssetManager.SetContentManger(Content, GraphicsDevice);

            // DEFAULT MONOGAME CODE -- Keep last
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Template code
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load textures
            try
            {
                // Load Single Asset Example
                //AssetManager.LoadAsset<Texture2D>("Overworld/BackgroundMaps/InfoBG/InfoBG");

                // Load Assets From File Example
                //AssetManager.LoadAssetsFromFile<SpriteFont>("Fonts");
            }
            catch (DirectoryNotFoundException e)
            {
                Logger.Log(e.Message);
                Exit();
            }
            catch (Exception e)
            {
                Logger.Log(e.Message);
                Exit();
            }

            // Keep last
            LateInitialize();
        }



        // Late Initialize (Anything that requires textures)
        void LateInitialize()
        {
            _thisGame = new();
        }

        protected override void Update(GameTime gameTime)
        {
            // Exit input logic
            // TODO: IMPLEMENT PROPER GAME EXIT FUNCTIONALITY
            if (KeyboardController.GetKeyPressed(Keys.Escape))
                Exit();


            // Game update
            _thisGame.Update(gameTime);


            // DEFAULT MONOGAME CODE -- Keep last
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Clear Screen with default colour
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Render Game
            _spriteBatch.Begin();
            _thisGame.Render(ref _spriteBatch, Window);
            _spriteBatch.End();

            // DEFAULT MONOGAME CODE -- Keep last
            base.Draw(gameTime);
        }
    }
}