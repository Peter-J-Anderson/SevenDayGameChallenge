using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ThinkGearNET;

namespace Mr_Potato_Adventure
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        bool hasToggledSpace = false;
        bool hasToggledT = false;
        bool hasToggledAttent = false;

        MrPotatoHead myPotato;
        MrPotatoHead little_alien; 

        List<Texture2D> PotatoTransform;
        List<Texture2D> Little_Alien_Alien;

        Texture2D TxMrPotato;
        Texture2D TxInstantMash;
        Texture2D TxSmoke;
        Texture2D TxFloor;

        List<Animation> AnimationList;

        ThinkGearWrapper myThinkGear;
        float myAttention;

        TimeSpan timer;
        // game states 
        int gameState = 0;

        //Home Screen   
        Texture2D homeScreen;
        Texture2D homeStart;
        Texture2D homeInfo;
        Texture2D homeExit;
        Texture2D infoScreen;
        Texture2D alien; 
        int selectedItem = 0;

        // menu selection 
        TimeSpan lastChecked;
        TimeSpan menuCooldown = new TimeSpan(0, 0, 0, 0, 100);
        List<MrPotatoHead> alienList = new List<MrPotatoHead>();


        bool isFalling = false; 
        //sound
        Song music;


        int alienCurrentStep = 0; 
        int[] alien_movement = new int[48] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                                            -2, -2, -2, -2, -2, -2,-2, -2, -2, -2, -2, -2,-2, -2, -2, -2, -2, -2,-2, -2, -2, -2, -2, -2 };

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 420;
            graphics.PreferredBackBufferWidth = 640;

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


            myThinkGear = new ThinkGearWrapper();
            TxSmoke = Content.Load<Texture2D>("smoke");
            TxMrPotato = Content.Load<Texture2D>("_SS_PotatoHead");
            TxInstantMash = Content.Load<Texture2D>("InstantMash");
            TxFloor = Content.Load<Texture2D>("Floor");
            PotatoTransform = new List<Texture2D>();
            Little_Alien_Alien = new List<Texture2D>();

            PotatoTransform.Add(TxMrPotato);
            PotatoTransform.Add(TxInstantMash);
            alien = Content.Load<Texture2D>("Aliens");
            Little_Alien_Alien.Add(alien);

            //homescreen 
            homeScreen = Content.Load<Texture2D>("Home_Screen");
            homeStart = Content.Load<Texture2D>("Start");
            homeInfo = Content.Load<Texture2D>("Info");
            homeExit = Content.Load<Texture2D>("Exit");
            infoScreen = Content.Load<Texture2D>("infoScreen");
            // music 
            music = Content.Load<Song>("music");

            // menu selection 
            lastChecked = new TimeSpan(0, 0, 0, 0, 0);

            myPotato = new MrPotatoHead(PotatoTransform, new Vector2(0, 0),200);

            
            
            alienList.Add(new MrPotatoHead(Little_Alien_Alien, new Vector2(-300 + (TxFloor.Width * 1), 0),100));
            alienList.Add(new MrPotatoHead(Little_Alien_Alien, new Vector2(-300 + (TxFloor.Width * 2), 0), 100));
            alienList.Add(new MrPotatoHead(Little_Alien_Alien, new Vector2(-300 + (TxFloor.Width * 3), 0), 100));


            AnimationList = new List<Animation>();

            if (!myThinkGear.Connect("COM3", 57600, true))
            {
                this.Exit();
            }
            myAttention = myThinkGear.ThinkGearState.Attention;


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
            MediaPlayer.Volume = 0.1f;
            MediaPlayer.Play(music);

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
            timer += gameTime.ElapsedGameTime;
            switch (gameState)
            {
                case 0:
                    if ((timer - lastChecked) >= menuCooldown)
                    {
                        lastChecked = timer;
                    if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    {
                        selectedItem--;
                        if (selectedItem == -1)
                            selectedItem = 2; 
                        selectedItem %= 3;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    {
                        selectedItem++;
                        selectedItem %= 3;
                    }
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        switch (selectedItem)
                        {
                            case 0:
                                gameState = 1;
                                break;
                            case 1:
                                gameState = 2; 
                                break;

                            case 2:
                                Exit(); 
                                break;
                        }
                    }

                    break;
                case 2:
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                        gameState = 0; 
                    break; 
                    
                case 1:
                    

                    //for each 
                    alienCurrentStep++;
                    alienCurrentStep %= 48;
                    foreach (MrPotatoHead e in alienList)
                    {
                        e.screenpos.X += alien_movement[alienCurrentStep] * e.speedMod;
                        if (e.health <= 0)
                        {
                            alienList.Remove(e);
                            break;
                        }
                    }



                    if (myPotato.health <= 0)
                    {
                        Exit();
                    }

                    Window.Title = "canTransform: " +  myPotato.canTransform.ToString() + "     myAttention: " + myAttention.ToString() + "     Mr Potato's HP: " + myPotato.health.ToString();
                    // check collision
                    foreach (MrPotatoHead e in alienList)
                    {

                      
                        if (myPotato.type == 0)
                        {
                            // take damage
                            if (e.screenpos.X >= myPotato.screenpos.X - 10 && e.screenpos.X <= myPotato.screenpos.X + 10)
                            {

                                myPotato.health -= 10;

                                break;
                            }

                        }
                        else
                        {
                            // give damage
                            if (e.screenpos.X >= myPotato.screenpos.X - 10 && e.screenpos.X <= myPotato.screenpos.X + 10)
                            {
                                e.health -= 10;

                            }


                        }
                    }
                    if (myPotato.screenpos.X < -300 || myPotato.screenpos.X > (-300 + TxFloor.Width* 5))
                    {
                        isFalling = true;
                    }

                    // Allows the game to exit
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                        this.Exit();
                    if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    {
                        myPotato.moveRight();
                     
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    {
                        myPotato.moveLeft();

                    }
                    foreach (Animation e in AnimationList)
                        e.update();
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                        gameState = 0; 

                    //***************************************************************************************************************
                    // Key Toggles
                    //***************************************************************************************************************
                    // Toggle 'T'
                    if (Keyboard.GetState().IsKeyDown(Keys.T) && hasToggledT == false)
                    {
                        hasToggledT = true;
                        AnimationList.Add(new Animation(TxSmoke, 5, new Vector2(300, 345 + myPotato.screenpos.Y - 40)));
                        myPotato.Transform();
                    }

                    if (Keyboard.GetState().IsKeyUp(Keys.T) && hasToggledT == true)
                    {
                        hasToggledT = false;

                    }

                    // Toggle 'Space'
                    if (Keyboard.GetState().IsKeyDown(Keys.Space) && hasToggledSpace == false)
                    {
                        hasToggledSpace = true;
                        myPotato.isJumping = true;


                    }

                    if (Keyboard.GetState().IsKeyUp(Keys.Space) && hasToggledSpace == true)
                    {
                        hasToggledSpace = false;
                    }

                    //***************************************************************************************************************
                    // Mindwave Toggles
                    //***************************************************************************************************************
                    if (myAttention > 75 && hasToggledAttent == false)
                    {
                        myPotato.canTransform = true;
                        hasToggledAttent = true;
                        myPotato.startTime = timer;
                        // set time stamp for being able to transform.


                    }

                    if (myAttention < 75 && hasToggledAttent == true)
                    {
                        hasToggledAttent = false;

                    }
                    myPotato.update(timer);



                    
                    break;
                    
            }

            if (myPotato.smoke)
            {
                AnimationList.Add(new Animation(TxSmoke, 5, new Vector2(300, 345 + myPotato.screenpos.Y - 40)));
                myPotato.smoke = false;
            }
                if (isFalling)
                myPotato.screenpos.Y++;
          
            myAttention = myThinkGear.ThinkGearState.Attention;
            //Window.Title = myAttention.ToString() ;

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

            switch (gameState)
            {
                case 0:
                    //home screen 

                    spriteBatch.Draw(homeScreen, new Rectangle(0, 0, homeScreen.Width, homeScreen.Height), Color.White);

                    spriteBatch.Draw(homeStart, new Rectangle(150, 150, homeExit.Width / 3, homeExit.Height), new Rectangle(0, 0, homeStart.Width / 3, homeStart.Height), Color.White);

                    spriteBatch.Draw(homeInfo, new Rectangle(150, 220, homeExit.Width / 3, homeExit.Height), new Rectangle(0, 0, homeInfo.Width / 3, homeInfo.Height), Color.White);

                    spriteBatch.Draw(homeExit, new Rectangle(150, 290, homeExit.Width / 3, homeExit.Height), new Rectangle(0, 0, homeExit.Width / 3, homeExit.Height), Color.White);
                    // draw buttons 
                    switch (selectedItem)
                    {
                        case 0:
                            spriteBatch.Draw(homeStart, new Rectangle(150, 150, homeExit.Width / 3, homeExit.Height), new Rectangle(homeStart.Width / 3 * 2, 0, homeStart.Width / 3, homeStart.Height), Color.White);

                            break;
                        case 1:
                            spriteBatch.Draw(homeInfo, new Rectangle(150, 220, homeExit.Width / 3, homeExit.Height), new Rectangle(homeInfo.Width / 3*2, 0, homeInfo.Width / 3, homeInfo.Height), Color.White);
                            break;
                        case 2:
                            spriteBatch.Draw(homeExit, new Rectangle(150, 290, homeExit.Width / 3, homeExit.Height), new Rectangle(homeExit.Width / 3*2, 0, homeExit.Width / 3, homeExit.Height), Color.White);
                            break;
                    }

                    break;

                case 2:
                    spriteBatch.Draw(infoScreen, new Rectangle(0, 0, homeScreen.Width, homeScreen.Height), new Rectangle(0, 0, infoScreen.Width, infoScreen.Height), Color.White);
                    break; 
                case 1:
                    //start game 


                   




                    //draw floor
                    for (int i = 0; i < 5; i++)
                    {
                        spriteBatch.Draw(TxFloor, new Vector2((i * TxFloor.Width) - myPotato.screenpos.X, 420 - TxFloor.Height),
                                        new Rectangle(0, 0, TxFloor.Width, TxFloor.Height), Color.White);
                    }

                    if (myPotato.direction == 2)
                        spriteBatch.Draw(myPotato.spriteTexture, new Vector2(300, 345 + myPotato.screenpos.Y),
                                         new Rectangle(myPotato.currentFrameX * myPotato.spriteWidth,
                                                       0,
                                                       myPotato.spriteWidth, myPotato.spriteHeight), Color.White,
                                                       0f, myPotato.origin, 1.0f, SpriteEffects.FlipHorizontally, 0);
                    if (myPotato.direction == 0)
                        spriteBatch.Draw(myPotato.spriteTexture, new Vector2(300, 345 + myPotato.screenpos.Y),
                                         new Rectangle(myPotato.currentFrameX * myPotato.spriteWidth,
                                                       0,
                                                       myPotato.spriteWidth, myPotato.spriteHeight), Color.White,
                                                       0f, myPotato.origin, 1.0f, SpriteEffects.None, 0);

                    // draw badguys 

                    foreach (MrPotatoHead e in alienList)
                    spriteBatch.Draw(e.spriteTexture, new Vector2(300 + e.screenpos.X - myPotato.screenpos.X, 345 + e.screenpos.Y),
                                         new Rectangle(e.currentFrameX * e.spriteWidth,
                                                       0,
                                                       e.spriteWidth, e.spriteHeight), Color.White,
                                                       0f, e.origin, 1.0f, SpriteEffects.None, 0);


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

                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
