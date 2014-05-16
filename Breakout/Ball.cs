using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout
{
	public class Ball
	{

		public Texture2D theTexture;
        public Vector2 thePosition;
        Boolean active;
        public float dX, dY;

        public Ball(Texture2D theTexture, Vector2 thePosition, float dX, float dY) //This is our Initialize() for balls
        {
            this.theTexture = theTexture;
            this.thePosition = thePosition;
            this.active = true;
            this.dX = dX;
            this.dY = dY;
        }

        public void Update()
        {
            thePosition.X += dX;
            thePosition.Y += dY;
        }

        public int getWidth()
        {
            return theTexture.Width;
        }

        public int getHeight()
        {
             return theTexture.Height;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(theTexture, thePosition, Color.White);
            spriteBatch.End();
        }

        public void setInactive()
        {
            this.active = false;
        }

        public Boolean isActive()
        {
            return this.active;
        }
	}
}

