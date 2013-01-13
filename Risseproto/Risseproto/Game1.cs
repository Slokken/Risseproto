using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Risseproto
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ContentHolder contentHolder;
        Gameworld gameWorld;
        Input input;
        Button startButton;
        int width = 1280;
        int height = 720;
        String gameName = "Risse prototype";
        Controller controller;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = height;
            graphics.PreferredBackBufferWidth = width;

            Window.Title = gameName;
        }

        
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            contentHolder = new ContentHolder(Content);
            gameWorld = new Gameworld(contentHolder);
            controller = new Controller();
            base.Initialize();
        }

        
        protected override void LoadContent()
        {

            // Sets the mouse position in our window.
            Mouse.WindowHandle = this.Window.Handle;

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            input = new Input();
            startButton = new Button(new Rectangle(0,0,400,75), "Start Spill", ref input, new List<Texture2D>() {
                contentHolder.menuStart,
                contentHolder.menuStartHover,
                contentHolder.menuStartClicked
            });

            input.jump += new Input.EventHandler(jump);
            input.duck += new Input.EventHandler(duck);
            startButton.clicked += new Button.EventHandler(buttonClicked);
            // TODO: use this.Content to load your game content here
        }
        public void jump()
        {
            System.Diagnostics.Debug.WriteLine("jump");
        }
        public void duck()
        {
            System.Diagnostics.Debug.WriteLine("duck");
        }
        public void buttonClicked(string action)
        {
            System.Diagnostics.Debug.WriteLine(action);
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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
                this.Exit();

            //controller.update(gameTime);

            // TODO: Add your update logic here
            input.Update();
            startButton.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            gameWorld.Draw(spriteBatch);
            startButton.Draw(spriteBatch);
            // TODO: Add your drawing code here

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
