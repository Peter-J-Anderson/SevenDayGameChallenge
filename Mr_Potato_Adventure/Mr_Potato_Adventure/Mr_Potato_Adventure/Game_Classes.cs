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
    class MrPotatoHead
    {
        // this class is everything needed for mr potatohead
        public Vector2 origin;
        public Vector2 screenpos;
        public float speedMod { get; set; }
        public int type { get; set; }

        public Texture2D spriteTexture;
        float timer = 0f;
        float interval = 200f;
        public int currentFrameX = 0;
        public int currentFrameY = 0;
        public int spriteWidth { get; set; }
        public int spriteHeight { get; set; }
        public int direction { get; set; }
        Rectangle sourceRect;
        List<Texture2D> textureList;


        //constructor 
        public MrPotatoHead(List<Texture2D> _myTexture, Vector2 _startLocation)
        {
            speedMod = 3;
            direction = 2; 
            type = 0;
            spriteWidth = 47;
            spriteHeight = 56;
            textureList = _myTexture;
            origin.X = spriteWidth / 2;
            origin.Y = spriteHeight / 2;
            screenpos = _startLocation;
            
            spriteTexture = _myTexture[type];

        }

        public void moveLeft()
        {
            direction = 0;
            screenpos.X -= 1.0f * speedMod;
            if (currentFrameX == 0 && currentFrameY == 0)
            {
                currentFrameX = 9;
                currentFrameY = 0;
            }
            if (currentFrameX == 0 && currentFrameY == 1)
            {
                direction = 2;
                currentFrameX = 9;
                currentFrameY = 0;
            }
            currentFrameX--;
        }

        public void moveRight()
        {
            direction = 2;
            screenpos.X += 1.0f * speedMod;
            if (currentFrameX == 19 && currentFrameY == 0)
            {
                currentFrameX = 0;
                currentFrameY = 0; 
            }
            if (currentFrameX == 19 && currentFrameY == 1)
            {
                
                currentFrameX = 0;
                currentFrameY = 0;
            }
            currentFrameX++;
        }

        public void Transform()
        {
            type++;
            type %= 2;
            spriteTexture = textureList[type];
        }
    }

    class Animation
    {

        public int noFrame;
        public Texture2D myTexture;

        public int totalWidth;
        public int spriteWidth;
        public int spriteHeight;
        public int currentFrame;

        public Vector2 origin;
        public Vector2 screenpos;

        public Rectangle sourceRect;

        public Animation(Texture2D _myTexture, int _noFrames, Vector2 _screenpos)
        { 
            myTexture = _myTexture;
            totalWidth = myTexture.Width; 
            noFrame = _noFrames; 

            spriteWidth = totalWidth / noFrame; 
            spriteHeight = myTexture.Height;    
            
            origin.X = spriteWidth/2;
            origin.Y = spriteHeight/2;

            screenpos = _screenpos;

            sourceRect = new Rectangle(0, 0, spriteWidth, spriteHeight);
        }

        public void update()
        {

            sourceRect.X = 0 + (spriteWidth * currentFrame) ;
            if (currentFrame != noFrame)
                currentFrame++;
            else
                currentFrame = 0;
        }

    }
}
