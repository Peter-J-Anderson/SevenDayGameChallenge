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


        //constructor 
        public MrPotatoHead(Texture2D _myTexture, Vector2 _startLocation)
        {
            speedMod = 3;
            direction = 2; 
            type = 0;
            spriteWidth = 48;
            spriteHeight = 56;

            origin.X = spriteWidth / 2;
            origin.Y = spriteHeight / 2;
            screenpos = _startLocation;
            
            spriteTexture = _myTexture;

        }

        public void moveLeft()
        {
            direction = 0;
            screenpos.X -= 1.0f * speedMod;
            if (currentFrameX == 0 && currentFrameY == 0)
            {
                currentFrameX = 9;
                currentFrameY = 1;
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
            if (currentFrameX == 9 && currentFrameY == 0)
            {
                currentFrameX = 0;
                currentFrameY = 1; 
            }
            if (currentFrameX == 9 && currentFrameY == 1)
            {
                
                currentFrameX = 0;
                currentFrameY = 0;
            }
            currentFrameX++;
        }

        
    }
}
