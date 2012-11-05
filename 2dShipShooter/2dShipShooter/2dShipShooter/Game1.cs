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
using ThinkGearNET;

namespace _2dShipShooter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 
    class ship
    {
        public Vector2 origin;
        public Vector2 screenpos;
        public float RotationAngle { get; set; }
        public float speedMod {get;set;}

        public int type{get;set;} 

        public ship(Texture2D _myTexture, Viewport _viewport)
        {
            RotationAngle = 1.57f;
            speedMod = 3;
            type = 1;
            origin.X = _myTexture.Width / 2;
            origin.Y = _myTexture.Height / 2;
            screenpos.X = _viewport.Width / 2;
            screenpos.Y = _viewport.Height / 2;

            RotationAngle = 1.57f ;
            float circle = MathHelper.Pi * 2;
            RotationAngle = RotationAngle % circle;

        }
    }

    class bullet
    {
        
        public Vector2 origin;
        public Vector2 screenpos;
        public float RotationAngle { get; set; }
        public float speedMod {get;set;}
        public int f;
        public int type{get;set;}

        public bullet(Texture2D _myTexture, Vector2 _startLocation)
        {
            RotationAngle = 1.57f;
            speedMod = 10;
            type = 1;
            origin.X = _myTexture.Width / 2;
            origin.Y = _myTexture.Height / 2;
            screenpos.X = _startLocation.X - (_myTexture.Width / 2.0f);
            screenpos.Y = _startLocation.Y - (_myTexture.Height / 2.0f);
            f = 180; 
            RotationAngle = 1.57f ;
            float circle = MathHelper.Pi * 2;
            RotationAngle = RotationAngle % circle;

        }

        public void update()
        {
            if (f >= 360)
            {
                f = 0;
            }
            else
            {
                f++;
            }

        }

    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {

        ThinkGearWrapper myThinkGear;
        
        
        bool canShoot = true; 
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Viewport viewport;


        Texture2D txShip;
        Texture2D txbullet; 
       List<bullet> bullets;

       float value = 0.0f;
       ship myShip; 

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 420;
            graphics.PreferredBackBufferWidth = 640;

            myThinkGear = new ThinkGearWrapper(); 
            
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
            // setup the event
            
            myThinkGear.ThinkGearChanged += _thinkGearWrapper_ThinkGearChanged;
            myThinkGear.Connect("COM27", 57600, true);


            base.Initialize();
            viewport = graphics.GraphicsDevice.Viewport;
            bullets = new List<bullet>();
        }

        void _thinkGearWrapper_ThinkGearChanged(object sender, ThinkGearChangedEventArgs e)
        {
            value = myThinkGear.ThinkGearState.Attention;

            System.Threading.Thread.Sleep(10);


        }
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            

            // load ship image
            
            // load bullet image 
            txbullet = Content.Load<Texture2D>("laser");
            txShip = Content.Load<Texture2D>("ship");

            myShip = new ship(txShip, viewport);
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

            //check for movement 
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                myShip.screenpos.Y = myShip.screenpos.Y + (1 * myShip.speedMod);
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                myShip.screenpos.Y = myShip.screenpos.Y - (1 * myShip.speedMod);
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                myShip.screenpos.X = myShip.screenpos.X + (1 * myShip.speedMod);
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                myShip.screenpos.X = myShip.screenpos.X - (1 * myShip.speedMod);


            if (Keyboard.GetState().IsKeyUp(Keys.Space) && canShoot == false)
            {
                canShoot = true;
            }
            // check tov see if the ship is shooting
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && canShoot == true)
            {
                canShoot = false; 
                bullets.Add(new bullet(txbullet, myShip.screenpos));
            }

            foreach (bullet e in bullets)
            {
                e.update();
                e.screenpos.X +=  0.8f * (1*e.speedMod);
                e.screenpos.Y += (1 * e.speedMod) * (float)Math.Cos((value/100.0f)*e.f);
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

            Window.Title = myThinkGear.ThinkGearState.Attention.ToString();

            spriteBatch.Begin();

            spriteBatch.Draw(txShip, myShip.screenpos, null, Color.White, myShip.RotationAngle,
                            myShip.origin, 0.5f, SpriteEffects.None, 0f);

            foreach (bullet e in bullets)
            { 
                spriteBatch.Draw(txbullet,e.screenpos,null, Color.White, e.RotationAngle, 
                                e.origin, 1.0f,SpriteEffects.None, 0f);
            }

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
