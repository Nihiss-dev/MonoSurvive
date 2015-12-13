using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System;
using System.IO;

namespace MonoSurvive
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D mainBackground;
        Rectangle rectBackground;

        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        Texture2D enemyTexture;
        List<Enemy> enemies;
        float EnemyAddSpeed;

        Player player;
        float playerSpeed;

        TimeSpan enemySpawnTime;
        TimeSpan previousSpawnTime;

        SoundEffect song;
        SoundEffectInstance instance;

        SpriteFont FontTimer;
        Vector2 TimerPos;

        int score;
        bool isPlaying;
        string output;

        Random random;
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
            // TODO: Add your initialization logic here
            rectBackground = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            player = new Player();
            playerSpeed = 8.0f;
            EnemyAddSpeed = 0.2f;

            score = 0;
            isPlaying = true;

            enemies = new List<Enemy>();

            previousSpawnTime = TimeSpan.Zero;
            enemySpawnTime = TimeSpan.FromSeconds(1.0f);
            random = new Random();
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

            Texture2D playerTexture = Content.Load<Texture2D>("Graphics\\vaisseau.png");
            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y
                                                + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(playerTexture, playerPosition);

            mainBackground = Content.Load<Texture2D>("Graphics\\espace.png");

            enemyTexture = Content.Load<Texture2D>("Graphics\\tie_fighter.png");

            song = Content.Load<SoundEffect>("Audio\\Waves\\main_menu");
            instance = song.CreateInstance();
            instance.IsLooped = true;
            instance.Play();
            instance.Volume = 0.5f;

            try
            {
                FontTimer = Content.Load<SpriteFont>(@"Font");
            }
            catch (Exception e)
            {
                Console.WriteLine("OK TAMER");
            }

            TimerPos = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            previousKeyboardState = currentKeyboardState;

            currentKeyboardState = Keyboard.GetState();

            UpdatePlayer(gameTime);

            if (isPlaying)
                UpdateEnemies(gameTime);

            base.Update(gameTime);
        }

        private void AddEnemy()
        {
            Vector2 Position = new Vector2(GraphicsDevice.Viewport.Width + enemyTexture.Width / 2,
                                            random.Next(15, GraphicsDevice.Viewport.Height - 75));
            Enemy enemy = new Enemy();
            enemy.Initialize(enemyTexture, Position);
            if ((enemy.EnemySpeed + EnemyAddSpeed) < 13.0f)
            {
                EnemyAddSpeed += 0.1f;
                Console.WriteLine("< 13.0f");
            }
            else
            {
                EnemyAddSpeed += 0.05f;
                Console.WriteLine("> 13.0f");
            }
            enemy.EnemySpeed += EnemyAddSpeed;
            Console.WriteLine(enemy.EnemySpeed);
            enemies.Add(enemy);
        }

        private void UpdatePlayer(GameTime gameTime)
        {
            // use keyboard
            if (currentKeyboardState.IsKeyDown(Keys.Up))
                player.Position.Y -= playerSpeed;
            if (currentKeyboardState.IsKeyDown(Keys.Down))
                player.Position.Y += playerSpeed;

            //make sure player is not out of bounds
            player.Position.X = MathHelper.Clamp(player.Position.X, 0, GraphicsDevice.Viewport.Width - player.Width);
            player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, GraphicsDevice.Viewport.Height - player.Height);
            player.Update(gameTime);
            UpdateCollision(gameTime);
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            if ((gameTime.TotalGameTime - previousSpawnTime) >= enemySpawnTime)
            {
                previousSpawnTime = gameTime.TotalGameTime;
                AddEnemy();
                if (enemySpawnTime > TimeSpan.FromSeconds(0.15f))
                {
                    enemySpawnTime -= TimeSpan.FromSeconds(0.01f);
                }
            }

            for (int i = (enemies.Count) - 1; i >= 0; i--)
            {
                enemies[i].Update(gameTime);
                if (enemies[i].Active == false)
                {
                    enemies.RemoveAt(i);
                    score += 1;
                }
            }
        }

        private void UpdateCollision(GameTime gameTime)
        {
            Rectangle rectangle1;
            Rectangle rectangle2;

            rectangle1 = new Rectangle((int)player.Position.X, (int)player.Position.Y,
                                        player.Width, player.Height);

            for (int i = 0; i < enemies.Count; i++)
            {
                rectangle2 = new Rectangle((int)enemies[i].EnemyPosition.X, (int)enemies[i].EnemyPosition.Y,
                                            enemies[i].Width, enemies[i].Height);
                if (rectangle1.Intersects(rectangle2))
                {
                    isPlaying = false;
                    output = gameTime.TotalGameTime.ToString();
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            if (isPlaying)
            {
                spriteBatch.Draw(mainBackground, rectBackground, Color.White);
                player.Draw(spriteBatch);

                for (int i = 0; i < enemies.Count; i++)
                    enemies[i].Draw(spriteBatch);
                try
                {
                    output = gameTime.TotalGameTime.ToString();
                    Vector2 FontOrigin = FontTimer.MeasureString(output) / 2;
                    spriteBatch.DrawString(FontTimer, "Time\n" + output, new Vector2(GraphicsDevice.Viewport.Width / 3, 15), Color.White);
                    spriteBatch.DrawString(FontTimer, "Score: " + score, new Vector2(GraphicsDevice.Viewport.Width - 100, 15), Color.White);
                }
                catch (Exception e)
                {
                    Console.WriteLine("OK TAMER");
                }
            }
            else
            {
                enemies.Clear();
                player.Active = false;
                instance.Pause();
                spriteBatch.Draw(mainBackground, rectBackground, Color.White);
                spriteBatch.DrawString(FontTimer, "Time\n" + output, new Vector2(GraphicsDevice.Viewport.Width / 3, 15), Color.White);
                using (FileStream stream = File.Open("highscore", FileMode.OpenOrCreate))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        int highscore;
                        try
                        {
                            highscore = reader.ReadInt32();
                        }
                        catch (Exception e)
                        { highscore = 0; }
                        Console.WriteLine(score);
                        if (score > highscore)
                            using (BinaryWriter writer = new BinaryWriter(stream))
                            {
                                writer.Write(score);
                                spriteBatch.DrawString(FontTimer, "New highscore\n" + score, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 3), Color.White, 0, new Vector2(20, 20), 3.0f, SpriteEffects.None, 0);
                            }
                        else
                        {
                            spriteBatch.DrawString(FontTimer, "Your score: " + score, new Vector2(GraphicsDevice.Viewport.Width / 3, GraphicsDevice.Viewport.Height / 3), Color.White, 0, new Vector2(20, 20), 3.0f, SpriteEffects.None, 0);
                            spriteBatch.DrawString(FontTimer, "Highscore: " + highscore, new Vector2(GraphicsDevice.Viewport.Width / 3, GraphicsDevice.Viewport.Height / 3), Color.White, 0, new Vector2(20, 20), 3.0f, SpriteEffects.None, 0);

                        }

                    }
                }

            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
