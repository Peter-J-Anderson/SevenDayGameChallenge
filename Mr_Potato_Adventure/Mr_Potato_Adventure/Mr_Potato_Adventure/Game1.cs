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

        bool hasToggled = false;

        MrPotatoHead myPotato;

        List<Texture2D> PotatoTransform;

        Texture2D TxMrPotato;
        Texture2D TxInstantMash;
        Texture2D TxSmoke; 
        Texture2D txMash;
   
        Animation AnimSmoke;
        List<Animation> AnimationList;
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
            TxSmoke = Content.Load<Texture2D>("smoke");
            TxMrPotato = Content.Load<Texture2D>("_SS_PotatoHead");
            TxInstantMash = Content.Load<Texture2D>("InstantMash");
            PotatoTransform = new List<Texture2D>();
            PotatoTransform.Add(TxMrPotato);
            PotatoTransform.Add(TxInstantMash);
            
            myPotato = new MrPotatoHead(PotatoTransform, new Vector2(100,400));
            AnimationList = new List<Animation>();
            
            


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
            foreach(Animation e in AnimationList)
                e.update();

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && hasToggled == false)
            {
                hasToggled = true;
                AnimationList.Add(new Animation(TxSmoke, 5, new Vector2(myPotato.screenpos.X, myPotato.screenpos.Y - 40)));
                myPotato.Transform();
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Space) && hasToggled == true)
            {
                hasToggled = false;

            }

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

            foreach (Animation e in AnimationList)
            {
                
                spriteBatch.Draw(e.myTexture, e.screenpos, e.sourceRect, Color.White,
                                   0.0f, e.origin, 1.0f, SpriteEffects.None, 0);
                if (e.currentFrame == e.noFrame)
                {
                    AnimationList.Remove(e);
                    break;
                }
            }



            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
