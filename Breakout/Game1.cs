#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

#endregion

namespace Breakout
{

	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D theBall;

        Texture2D brickTexture1;
        Texture2D brickTexture2;
        Texture2D brickTexture3;
        Texture2D brickTexture4;
        Texture2D brickTexture5;

        Texture2D thePaddle;

        Vector2 theBallPosition;
        Vector2 thePaddlePosition;

        int playerScore = 0;
        SpriteFont font;

        int consecutiveBricksHit = 0;
        int highestCombo = 0;
        int nextNewBallAt = 5;

        int activeBrickCount = 360; //my initialized brick count is 360, I figure it is cheaper to decrement this by 1 every time a brick is destroyed than to constantly recalculate how many bricks remain
        int activeBallCount = 1; //Initially, there is one ball, and if a ball is changed to inactive (ball death) this gets decremented

        List<Brick> theBricks;
        List<Ball> theBalls;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            font = Content.Load<SpriteFont>("SpriteFont1");

            theBallPosition = new Vector2();
            theBallPosition.X = GraphicsDevice.Viewport.Width/2 - theBall.Width / 2;
            theBallPosition.Y = ((GraphicsDevice.Viewport.Height - theBall.Height) / 2) + 15;

            thePaddlePosition = new Vector2();
            thePaddlePosition.X = GraphicsDevice.Viewport.Width / 2 - thePaddle.Width / 2;
            thePaddlePosition.Y = GraphicsDevice.Viewport.Height - 15 - thePaddle.Height;

            theBalls = new List<Ball>();

            Ball firstBall = new Ball(theBall, theBallPosition, 1, 3);
            theBalls.Add(firstBall);

            theBricks = new List<Brick>();
            Vector2 brickPosition = new Vector2();

            brickPosition.X = (((GraphicsDevice.Viewport.Width - (24 * brickTexture1.Width)) / 2)); //right x coordinate of first ball durign draw process
            brickPosition.Y = 5;

            for (int row = 0; row < 3; row++)//This for loop prints a brick1, brick2, (...), 3 times to get color pattern on screen, each of the 5 bricks has a different worth value
            {
                for (int col1 = 0; col1 < 24; col1++)
                {
                    Brick newBrick = new Brick(brickTexture1, brickPosition, 10);
                    theBricks.Add(newBrick);

                    brickPosition.X += brickTexture1.Width;
                }

                brickPosition.X = (((GraphicsDevice.Viewport.Width - (24 * brickTexture1.Width)) / 2));
                brickPosition.Y += brickTexture1.Height;

                for (int col2 = 0; col2 < 24; col2++)
                {
                    Brick newBrick = new Brick(brickTexture2, brickPosition, 20);
                    theBricks.Add(newBrick);

                    brickPosition.X += brickTexture2.Width;
                }

                brickPosition.X = (((GraphicsDevice.Viewport.Width - (24 * brickTexture1.Width)) / 2));
                brickPosition.Y += brickTexture1.Height;

                for (int col3 = 0; col3 < 24; col3++)
                {
                    Brick newBrick = new Brick(brickTexture3, brickPosition, 30);
                    theBricks.Add(newBrick);

                    brickPosition.X += brickTexture3.Width;
                }

                brickPosition.X = (((GraphicsDevice.Viewport.Width - (24 * brickTexture1.Width)) / 2));
                brickPosition.Y += brickTexture3.Height;

                for (int col4 = 0; col4 < 24; col4++)
                {
                    Brick newBrick = new Brick(brickTexture4, brickPosition, 40);
                    theBricks.Add(newBrick);

                    brickPosition.X += brickTexture4.Width;
                }

                brickPosition.X = (((GraphicsDevice.Viewport.Width - (24 * brickTexture1.Width)) / 2));
                brickPosition.Y += brickTexture4.Height;

                for (int col5 = 0; col5 < 24; col5++)
                {
                    Brick newBrick = new Brick(brickTexture5, brickPosition, 50);
                    theBricks.Add(newBrick);

                    brickPosition.X += brickTexture5.Width;
                }

                brickPosition.X = (((GraphicsDevice.Viewport.Width - (24 * brickTexture1.Width)) / 2));
                brickPosition.Y += brickTexture5.Height;
            }
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            theBall = Content.Load<Texture2D>("ball");

            brickTexture1 = Content.Load<Texture2D>("brick1");
            brickTexture2 = Content.Load<Texture2D>("brick2");
            brickTexture3 = Content.Load<Texture2D>("brick3");
            brickTexture4 = Content.Load<Texture2D>("brick4");
            brickTexture5 = Content.Load<Texture2D>("brick5");

            thePaddle = Content.Load<Texture2D>("paddle");
        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState currentKeyboardState = Keyboard.GetState();
            if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                thePaddlePosition.X -= 7;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                thePaddlePosition.X += 7;
            }
            if (currentKeyboardState.IsKeyDown(Keys.A))//Add new ball by pressing A button
            {
                Ball newBall = new Ball(theBall, theBallPosition, 1, 2);
                theBalls.Add(newBall);

                activeBallCount++;
            }

            if (consecutiveBricksHit > highestCombo)
            {
                highestCombo = consecutiveBricksHit;
            }

            for (int i = 0; i < theBricks.Count; i++) //turns a brick that fell off screen "off" (sets visible to false) so Draw doesn't draw it and waste ressources
            {
                if (theBricks[i].isHit())
                {
                    if (theBricks[i].thePosition.Y > GraphicsDevice.Viewport.Height)
                    {
                        theBricks[i].brickHit();
                    }
                }
            }

            for (int i = 0; i < theBricks.Count; i++)
            {
                if (theBricks[i].isHit() && theBricks[i].isVisible())
                {
                    theBricks[i].thePosition.Y = theBricks[i].thePosition.Y + theBricks[i].dY;
                    theBricks[i].dY = theBricks[i].dY * 1.1f;
                }
            }

            thePaddlePosition.X = MathHelper.Clamp(thePaddlePosition.X, 0, GraphicsDevice.Viewport.Width - thePaddle.Width);

            if (activeBallCount == 0 || activeBrickCount == 0)//End Game if there are no more balls OR there are no more bricks
            {
                Exit();
            }

            UpdateBall();

            base.Update(gameTime);
        }

        private void UpdateBall()
        {
            for (int i = 0; i < theBalls.Count; i++)// Update all the balls
            {
                if (theBalls[i].thePosition.Y + theBalls[i].getHeight() > GraphicsDevice.Viewport.Height) //Ball hits bottom of screen
                {
                    if (theBalls[i].isActive())
                    {
                        theBalls[i].setInactive();

                        activeBallCount--;
                    }
                }

                if ((theBalls[i].thePosition.X + theBalls[i].getWidth() > GraphicsDevice.Viewport.Width) || theBalls[i].thePosition.X < 0)
                {
                    theBalls[i].dX = -theBalls[i].dX;
                }

                if (theBalls[i].thePosition.Y < 0)
                {
                    theBalls[i].dY = -theBalls[i].dY;
                }

                theBalls[i].Update();
            }

            Rectangle paddleRectL = new Rectangle((int)thePaddlePosition.X, (int)thePaddlePosition.Y, 2, thePaddle.Height);
            Rectangle paddleRectR = new Rectangle((int)thePaddlePosition.X + thePaddle.Width- 2, (int)thePaddlePosition.Y, 2, thePaddle.Height);
            
            Rectangle paddleRectT1 = new Rectangle((int)thePaddlePosition.X, (int)thePaddlePosition.Y, thePaddle.Width / 4, 2);
            Rectangle paddleRectT2 = new Rectangle((int)thePaddlePosition.X + thePaddle.Width / 4, (int)thePaddlePosition.Y, thePaddle.Width / 4, 2);
            Rectangle paddleRectT3 = new Rectangle((int)thePaddlePosition.X + (2 * (thePaddle.Width / 4)), (int)thePaddlePosition.Y, thePaddle.Width / 4, 2);
            Rectangle paddleRectT4 = new Rectangle((int)thePaddlePosition.X + (3 * (thePaddle.Width / 4)), (int)thePaddlePosition.Y, thePaddle.Width / 4, 2);
            //going to make 4 rectangles on the top of the paddle so I can change the ball's dX accordingly


            ///The ball can never hit the bottom of the paddle therefore a bottom collision rectangle is not necessary
            ///
            for (int i = 0; i < theBalls.Count; i++)
            {
                if (theBalls[i].isActive())//we only care about balls that are active on screen
                {
                    //each ball gets a more detailed collision rectangle, instead of just one. each active ball gets a top, bottom, left, and right rectangle of width 1 ( T B L R )
                    //now if we just check when the right side of the ball hit sthe left side of the brick, we can do more detail collision detection resulting in true breakout style physics
                    //this is just a try

                    //for example the top of the ball can only collide with the bottom of a brick, so we just have to check a few conditions for collision detection

                    //Rectangle(xpos, ypos, width, height);

                    Rectangle ballRectL = new Rectangle((int)theBalls[i].thePosition.X, (int)theBalls[i].thePosition.Y, 3, theBalls[i].getHeight());
                    Rectangle ballRectR = new Rectangle((int)theBalls[i].thePosition.X + theBalls[i].getWidth() - 3, (int)theBalls[i].thePosition.Y, 3, theBalls[i].getHeight());
                    Rectangle ballRectT = new Rectangle((int)theBalls[i].thePosition.X, (int)theBalls[i].thePosition.Y, theBalls[i].getWidth(), 3);
                    Rectangle ballRectB = new Rectangle((int)theBalls[i].thePosition.X, (int)theBalls[i].thePosition.Y - 3+ theBalls[i].getHeight(), theBalls[i].getWidth(), 3);

                    for (int j = 0; j < theBricks.Count; j++)
                    {
                        if (theBricks[j].isVisible() && !theBricks[j].isHit()) //only do collision detection for bricks that are visible and are not falling (is hit)
                        {
                            Rectangle brickRectL = new Rectangle((int)theBricks[j].thePosition.X, (int)theBricks[j].thePosition.Y, 2, theBricks[j].theTexture.Height);
                            Rectangle brickRectR = new Rectangle((int)theBricks[j].thePosition.X + theBricks[j].theTexture.Width - 2, (int)theBricks[j].thePosition.Y, 2, theBricks[j].theTexture.Height);
                            Rectangle brickRectT = new Rectangle((int)theBricks[j].thePosition.X, (int)theBricks[j].thePosition.Y, theBricks[j].theTexture.Width, 2);
                            Rectangle brickRectB = new Rectangle((int)theBricks[j].thePosition.X, (int)theBricks[j].thePosition.Y - 2 + theBricks[j].theTexture.Height, theBricks[j].theTexture.Width, 2);

                            if (ballRectL.Intersects(brickRectR) || ballRectR.Intersects(brickRectL))
                            {
                                theBalls[i].dX = -theBalls[i].dX;
                                theBricks[j].brickWasHit();

                                playerScore += theBricks[j].worth;

                                consecutiveBricksHit++;
                                activeBrickCount--;

                                break;
                            }
                            if (ballRectT.Intersects(brickRectB) || ballRectB.Intersects(brickRectT))
                            {
                                theBalls[i].dY = -theBalls[i].dY;
                                theBricks[j].brickWasHit();

                                playerScore += theBricks[j].worth;

                                consecutiveBricksHit++;
                                activeBrickCount--;

                                break;
                            }
                            if (ballRectB.Intersects(paddleRectT1))//Could be used to make the ball not move predictably despite where on the paddle it collided
                            {
                                theBalls[i].dX = -2;
                                theBalls[i].dY = -2.5f;
                                consecutiveBricksHit = 0;
                                nextNewBallAt = 5;
                                break;
                            }
                            if (ballRectB.Intersects(paddleRectT2))//Could be used to make the ball not move predictably despite where on the paddle it collided
                            {
                                theBalls[i].dX = -1;
                                theBalls[i].dY = -2.2f; //default new ball 1, 2.5f
                                consecutiveBricksHit = 0;
                                nextNewBallAt = 5;
                                break;
                            }
                            if (ballRectB.Intersects(paddleRectT3))//Could be used to make the ball not move predictably despite where on the paddle it collided
                            {
                                theBalls[i].dX = 1;
                                theBalls[i].dY = -2.2f;
                                consecutiveBricksHit = 0;
                                nextNewBallAt = 5;
                                break;
                            }
                            if (ballRectB.Intersects(paddleRectT4))//Could be used to make the ball not move predictably despite where on the paddle it collided
                            {
                                theBalls[i].dX = 2;
                                theBalls[i].dY = -2.5f;
                                consecutiveBricksHit = 0;
                                nextNewBallAt = 5;
                                break;
                            }
                            if (ballRectR.Intersects(paddleRectL))
                            {
                                theBalls[i].dX = -theBalls[i].dX;
                                consecutiveBricksHit = 0;
                                nextNewBallAt = 5;

                                break;
                            }
                            if (ballRectL.Intersects(paddleRectR))
                            {
                                theBalls[i].dX = -theBalls[i].dX;
                                consecutiveBricksHit = 0;
                                nextNewBallAt = 5;

                                break;
                            }

                            if (consecutiveBricksHit == nextNewBallAt)
                            {
                                Ball newBall = new Ball(theBall, theBalls[i].thePosition, 1, 1);
                                theBalls.Add(newBall);

                                activeBallCount++;

                                nextNewBallAt = nextNewBallAt + 5;
                            }
                        }
                    }
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            for (int i = 0; i < theBricks.Count; i++)
            {
                if (theBricks[i].isVisible())
                {
                    theBricks[i].Draw(spriteBatch);
                }
            }

            for (int i = 0; i < theBalls.Count; i++)
            {
                if (theBalls[i].isActive())
                {
                    theBalls[i].Draw(spriteBatch);
                }
            }

            spriteBatch.Begin();

            spriteBatch.DrawString(font, "Score: " + playerScore, new Vector2(0, GraphicsDevice.Viewport.Height - 22), Color.Black);
            spriteBatch.DrawString(font, "Brick Combo: " + consecutiveBricksHit, new Vector2(GraphicsDevice.Viewport.Width / 2 - 290, GraphicsDevice.Viewport.Height - 22), Color.Black);
            spriteBatch.DrawString(font, "Highest Combo: " + highestCombo, new Vector2(GraphicsDevice.Viewport.Width / 2 - 130, GraphicsDevice.Viewport.Height - 22), Color.Black);
            spriteBatch.DrawString(font, "Balls on-screen: " + activeBallCount, new Vector2(GraphicsDevice.Viewport.Width /2 + 40, GraphicsDevice.Viewport.Height - 22), Color.Black);
            spriteBatch.DrawString(font, "Bricks remaining: " + activeBrickCount, new Vector2(GraphicsDevice.Viewport.Width - 180, GraphicsDevice.Viewport.Height - 22), Color.Black);
            
            spriteBatch.Draw(thePaddle, thePaddlePosition, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
	}
}

