using Microsoft.Xna.Framework.Graphics;
using Misc;
using System;


// Wow what an amazing, totally necessary file

namespace Scenes.Objects.MainGame.Background
{
    internal class BackgroundImage : GameObject
    {
        public BackgroundImage(Builder builder) : base(builder)
        {
            if (_texture == null) throw new ArgumentNullException(nameof(Texture), "Texture cannot be null for a BackgroundImage.");
        }

        #region Builder

        public new class Builder : GameObject.Builder
        {
            public Builder(Texture2D texture) : base()
            {
                IsUI = true;
                Texture = texture;
            }

            public BackgroundImage Build()
            {
                return new BackgroundImage(this);
            }
        }

        #endregion
    }
}