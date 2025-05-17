using Controllers.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Misc;
using Scenes.Objects.Hitboxes;
using System;

namespace Scenes.Objects.UI.Interactable
{
    internal abstract class ClickableUIElement : GameObject
    {
        protected ClickableUIElement(Builder builder) : base(builder)
        {
        }

        #region Builder

        public new class Builder : GameObject.Builder
        {
            public Builder(Hitbox hitbox) : base()
            {
                IsUI = true;
                Hitbox = hitbox; // Hitbox must not be null in this class
            }

            public new T Build<T>() where T : ClickableUIElement
            {
                return (T)Activator.CreateInstance(typeof(T), this);
            }
        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected bool GetMouseOverlap()
        {
            return MouseController.GetMouseOverlap(this);
        }



        public virtual void OnClick()
        {

        }

        // I noticed I put these overrides and just left them base a lot huh
        public override void Render(ref SpriteBatch sb, GameWindow window)
        {
            base.Render(ref sb, window);
        }

        public override void RenderRelative(ref SpriteBatch sb, GameWindow window, Vector2 relativeTo)
        {
            base.RenderRelative(ref sb, window, relativeTo);
        }
    }
}