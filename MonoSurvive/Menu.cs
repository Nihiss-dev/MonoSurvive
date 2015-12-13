using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoSurvive
{
    public class Menu : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D mainBackground;

        Rectangle rectBackGround;

        enum BState
        {
            HOVER,
            UP,
            JUST_RELEASED,
            DOWN
        }
        const int NUMBER_OF_BUTTONS = 1,
            START_BUTTON_INDEX = 0,
            BUTTON_HEIGHT = 29*2,
            BUTTON_WIDTH = 91*2;

        Color[] button_color = new Color[NUMBER_OF_BUTTONS];
        Rectangle[] button_rectangle = new Rectangle[NUMBER_OF_BUTTONS];
        BState[] button_state = new BState[NUMBER_OF_BUTTONS];
        Texture2D[] button_texture = new Texture2D[NUMBER_OF_BUTTONS];
        double[] button_timer = new double[NUMBER_OF_BUTTONS];
        bool mpressed, prev_mpressed;
        int mx, my;
        double frameTime;

        public Menu()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            rectBackGround = new Rectangle(0, 0, GraphicsDevice.Viewport.Width,
                                            GraphicsDevice.Viewport.Height);
            int x = Window.ClientBounds.Width / 2 - BUTTON_WIDTH / 2;
            int y = Window.ClientBounds.Height / 2 - NUMBER_OF_BUTTONS / 2 *
                BUTTON_HEIGHT - (NUMBER_OF_BUTTONS % 2) * BUTTON_HEIGHT / 2;
            for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
            {
                button_state[i] = BState.UP;
                button_color[i] = Color.Transparent;
                button_timer[i] = 0.0;
                button_rectangle[i] = new Rectangle(x, y, BUTTON_WIDTH, BUTTON_HEIGHT);
                y += BUTTON_HEIGHT;
            }
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //base.LoadContent();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mainBackground = Content.Load<Texture2D>("Graphics\\espace.png");
            button_texture[START_BUTTON_INDEX] = Content.Load<Texture2D>("Graphics\\start.png");
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // get elapsed frame time in seconds
            frameTime = gameTime.ElapsedGameTime.Milliseconds / 1000.0;

            // update mouse variables
            MouseState mouse_state = Mouse.GetState();
            mx = mouse_state.X;
            my = mouse_state.Y;
            prev_mpressed = mpressed;
            mpressed = mouse_state.LeftButton == ButtonState.Pressed;

            update_buttons();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(mainBackground, rectBackGround, Color.White);
            spriteBatch.DrawString(Content.Load<SpriteFont>(@"Font"), "MonoSurvive", new Vector2(GraphicsDevice.Viewport.Height / 2 - 45, 100), Color.Yellow, 0, new Vector2(7, 7), 4.0f, SpriteEffects.None, 0);
            for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
                spriteBatch.Draw(button_texture[i], button_rectangle[i], button_color[i]);
            spriteBatch.End();
            base.Draw(gameTime);
        }



        // wrapper for hit_image_alpha taking Rectangle and Texture
        Boolean hit_image_alpha(Rectangle rect, Texture2D tex, int x, int y)
        {
            return hit_image_alpha(0, 0, tex, tex.Width * (x - rect.X) /
                rect.Width, tex.Height * (y - rect.Y) / rect.Height);
        }

        // wraps hit_image then determines if hit a transparent part of image 
        Boolean hit_image_alpha(float tx, float ty, Texture2D tex, int x, int y)
        {
            if (hit_image(tx, ty, tex, x, y))
            {
                uint[] data = new uint[tex.Width * tex.Height];
                tex.GetData<uint>(data);
                if ((x - (int)tx) + (y - (int)ty) *
                    tex.Width < tex.Width * tex.Height)
                {
                    return ((data[
                        (x - (int)tx) + (y - (int)ty) * tex.Width
                        ] &
                                0xFF000000) >> 24) > 20;
                }
            }
            return false;
        }

        // determine if x,y is within rectangle formed by texture located at tx,ty
        Boolean hit_image(float tx, float ty, Texture2D tex, int x, int y)
        {
            return (x >= tx &&
                x <= tx + tex.Width &&
                y >= ty &&
                y <= ty + tex.Height);
        }
        // determine state and color of button
        void update_buttons()
        {
            for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
            {

                if (hit_image_alpha(
                    button_rectangle[i], button_texture[i], mx, my))
                {
                    button_timer[i] = 0.0;
                    if (mpressed)
                    {
                        // mouse is currently down
                        button_state[i] = BState.DOWN;
                        button_color[i] = Color.Blue;
                    }
                    else if (!mpressed && prev_mpressed)
                    {
                        // mouse was just released
                        if (button_state[i] == BState.DOWN)
                        {
                            // button i was just down
                            button_state[i] = BState.JUST_RELEASED;
                        }
                    }
                    else
                    {
                        button_state[i] = BState.HOVER;
                        button_color[i] = Color.LightBlue;
                    }
                }
                else
                {
                    button_state[i] = BState.UP;
                    if (button_timer[i] > 0)
                    {
                        button_timer[i] = button_timer[i] - frameTime;
                    }
                    else
                    {
                        button_color[i] = Color.White;
                    }
                }

                if (button_state[i] == BState.JUST_RELEASED)
                {
                    take_action_on_button(i);
                }
            }
        }
        // Logic for each button click goes here
        void take_action_on_button(int i)
        {
            //take action corresponding to which button was clicked
            switch (i)
            {
                case START_BUTTON_INDEX:
                    Exit();
                    break;
                default:
                    break;
            }
        }
    }
}
