using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoSurvive
{
    class Enemy
    {
        public Texture2D EnemyTexture;
        public Vector2 EnemyPosition;
        public bool Active;
        public int Value;

        public int Width
        {
            get { return EnemyTexture.Width; }
        }
        public int Height
        {
            get { return EnemyTexture.Height; }
        }

        public float EnemySpeed { get; set; }

        public void Initialize(Texture2D texture, Vector2 position)
        {
            EnemyTexture = texture;
            EnemyPosition = position;
            Active = true;
            EnemySpeed = 4.0f;
        }

        public void Update(GameTime gameTime)
        {
            EnemyPosition.X -= EnemySpeed;
            if (EnemyPosition.X < -10)
                Active = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(EnemyTexture, EnemyPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
