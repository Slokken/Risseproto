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
        enum GameState { Menu, InGame, Grandma, Outro }
        GameState gameState = GameState.Menu;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ContentHolder contentHolder;
        SoundManager soundManager;
        Gameworld gameWorld;
        Input input;
        Button startButton;
        Button restartButton;
        Button menuButton;
        Gameobject menuBackground;
        Gameobject grandma;
        Gameobject outroBackground;
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
            input = new Input();
            contentHolder = new ContentHolder(Content);
            soundManager = new SoundManager(contentHolder);
            gameWorld = new Gameworld(contentHolder);
            controller = new Controller(input, soundManager, contentHolder);
            this.IsMouseVisible = true;
            base.Initialize();
        }

        
        protected override void LoadContent()
        {

            // Sets the mouse position in our window.
            Mouse.WindowHandle = this.Window.Handle;

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
           
            startButton = new Button(new Rectangle((width/3)*2,550,400,75), "Start Spill", ref input, new List<Texture2D>() {
                contentHolder.menuStart,
                contentHolder.menuStartHover,
                contentHolder.menuStartClicked
            });
            menuBackground = new Gameobject(contentHolder.menuBackground, Vector2.Zero, new Vector2(0, 0));
            grandma = new Gameobject(contentHolder.grandma, Vector2.Zero, new Vector2(0, 0));
            outroBackground = new Gameobject(contentHolder.texture_outro, Vector2.Zero, new Vector2(0, 0));

            restartButton = new Button(new Rectangle(50, height - 125, 400, 75), "Start paa nytt", ref input, new List<Texture2D>() {
                contentHolder.menuRestart,
                contentHolder.menuRestartHover,
                contentHolder.menuStartClicked
            });

            menuButton = new Button(new Rectangle(width - 450, height - 125, 400, 75), "Meny", ref input, new List<Texture2D>() {
                contentHolder.menuMenu,
                contentHolder.menuMenuHover,
                contentHolder.menuMenuClicked
            });

            startButton.clicked += new Button.EventHandler(buttonClicked);
            restartButton.clicked += new Button.EventHandler(buttonClicked);
            menuButton.clicked += new Button.EventHandler(buttonClicked);
            controller.goToOutro += new Controller.EventHandler(outro);
            // TODO: use this.Content to load your game content here
        }

        public void outro()
        {
            gameState = GameState.Outro;
        }

        public void buttonClicked(string action)
        {
            switch (action)
            {
                case "Start Spill":
                    gameWorld = new Gameworld(contentHolder);
                    gameState = GameState.Grandma;
                    break;
                case "Start paa nytt":
                    //FUCKING UPDATE THE FUCKING FUCK, FUCK
                    gameWorld.resetToCheckpoint();
                    gameState = GameState.InGame;
                    break;
                case "Meny":
                    gameState = GameState.Menu;
                    break;
            }
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
            input.Update();

            if (gameState == GameState.Menu)
            {
                // Allows the game to exit
                if (input.WasKeyClicked(Keys.Escape))
                    this.Exit();
                if (input.WasKeyClicked(Keys.Enter))
                {
                    gameWorld = new Gameworld(contentHolder);
                    gameState = GameState.Grandma;
                }
                startButton.Update();
            }
            else if (gameState == GameState.Grandma)
            {
                if (input.WasKeyClicked(Keys.Escape))
                    gameState = GameState.Menu;
                if (input.WasKeyClicked(Keys.Enter))
                    gameState = GameState.InGame;
                if (input.WasMouseClicked())
                    gameState = GameState.InGame;
            }
            else if (gameState == GameState.Outro)
            {
                if (input.WasKeyClicked(Keys.Escape))
                    gameState = GameState.Menu;
                if (input.WasKeyClicked(Keys.Enter))
                {
                    gameWorld.resetToCheckpoint();
                    gameState = GameState.InGame;
                }
                restartButton.Update();
                menuButton.Update();

            }
            else if (gameState == GameState.InGame)
            {
                // Puts you back into the menu
                if (input.WasKeyClicked(Keys.Escape))
                    gameState = GameState.Menu;

                controller.update(gameWorld, gameTime);
                input.Swipe();
                input.Click();
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            if (gameState == GameState.Menu)
            {
                menuBackground.Draw(spriteBatch);
                startButton.Draw(spriteBatch);
            }
            else if (gameState == GameState.Grandma)
            {
                grandma.Draw(spriteBatch);
            }
            else if(gameState == GameState.InGame)
            {
                gameWorld.Draw(spriteBatch);
            }
            else if (gameState == GameState.Outro)
            {
                outroBackground.Draw(spriteBatch);
                restartButton.Draw(spriteBatch);
                menuButton.Draw(spriteBatch);
            }
            // TODO: Add your drawing code here

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
