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

namespace Mr_Potato_Adventure
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        MrPotatoHead myPotato;
        Texture2D TxMrPotato;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            TxMrPotato = Content.Load<Texture2D>("_SS_PotatoHead");
            myPotato = new MrPotatoHead(TxMrPotato, new Vector2(100,100)); 

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
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

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                myPotato.moveRight();
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                myPotato.moveLeft();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            if (myPotato.direction == 2)
            spriteBatch.Draw(myPotato.spriteTexture, myPotato.screenpos,
                             new Rectangle(myPotato.currentFrameX * myPotato.spriteWidth,
                                           myPotato.currentFrameY * myPotato.spriteHeight, 
                                           myPotato.spriteWidth, myPotato.spriteHeight), Color.White,
                                           0f, myPotato.origin, 1.0f, SpriteEffects.FlipHorizontally, 0);
            if (myPotato.direction == 0)
                spriteBatch.Draw(myPotato.spriteTexture, myPotato.screenpos,
                                 new Rectangle(myPotato.currentFrameX * myPotato.spriteWidth,
                                               myPotato.currentFrameY * myPotato.spriteHeight,
                                               myPotato.spriteWidth, myPotato.spriteHeight), Color.White,
                                               0f, myPotato.origin, 1.0f, SpriteEffects.None, 0);


            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
