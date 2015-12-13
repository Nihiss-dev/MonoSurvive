using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoSurvive
{
    class Player
    {
        public Texture2D PlayerTexture;
        public Vector2 Position;

        public bool Active;

        public int Width
        {
            get { return PlayerTexture.Width; }
        }
        public int Height
        {
            get { return PlayerTexture.Height; }
        }

        public void Initialize(Texture2D texture, Vector2 position)
        {
            PlayerTexture = texture;
            Position = position;
            Active = true;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
                spriteBatch.Draw(PlayerTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
