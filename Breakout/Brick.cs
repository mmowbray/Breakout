using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout
{
	public class Brick
	{

		public Texture2D theTexture;
        public Vector2 thePosition;
        Boolean visible;
        Boolean hit;
        public int worth;

        public float dY;//or fallin bricks

        public Brick(Texture2D theTexture, Vector2 thePosition, int worth)
        {
            this.theTexture = theTexture;
            this.thePosition = thePosition;
            this.visible = true;
            this.worth = worth;
            this.hit = false;
            this.dY = 0.1f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(theTexture, thePosition, Color.White);
            spriteBatch.End();
        }

        public void brickHit()
        {
            this.visible = false;
        }
        
        public void brickWasHit()
        {
            this.hit = true;
        }
        public Boolean isVisible()
        {
            return this.visible;
        }

        public Boolean isHit()
        {
            return this.hit;
        }
	}
}