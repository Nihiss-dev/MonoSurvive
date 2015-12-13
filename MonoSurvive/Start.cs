using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MonoSurvive
{
    class Start : Game
    {
        public Texture2D texture;
        public Vector2 position;
        public bool active;
        public int width
        {
            get { return texture.Width; }
        }
        public int height
        {
            get { return texture.Height; }
        }

        public Start()
        {
            Content.RootDirectory = "Content";
        }

        public void Initialize(Texture2D Ntexture, Vector2 position)
        {
            texture = Ntexture;
            active = true;
        }

        public void Update()
        {
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
