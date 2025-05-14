using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Misc;

#nullable enable

namespace Scenes.Objects.UI.Displays
{
    // I'd like to think this class is self-explanatory enough
    internal class TextDisplay : GameObject
    {
        public TextDisplay(Builder builder) : base(builder)
        {
            _font = builder.Font;
            _textValue = builder.DefaultText;

            if (this.GetType() == typeof(TextDisplay)) Logger.Log("TextDisplay initialized.");
        }

        #region Builder
        public new class Builder : GameObject.Builder
        {
            public SpriteFont Font { get; private set; }
            public string DefaultText { get; private set; }

            public Builder(SpriteFont font) : base()
            {
                Font = font;
                DefaultText = "";
            }

            public Builder SetDefaultText(string defaultText)
            {
                DefaultText = defaultText;
                return this;
            }

            public TextDisplay Build()
            {
                return new TextDisplay(this);
            }
        }
        #endregion

        protected SpriteFont _font;
        public string _textValue { get; protected set; }


        public virtual void UpdateText(string text)
        {
            _textValue = text;
        }

        public override void Render(ref SpriteBatch sb, GameWindow window)
        {
            Vector2 textSize = _font.MeasureString(_textValue);
            Vector2 scale = new(_size.X / textSize.X, _size.Y / textSize.Y);
            Vector2 origin = new(textSize.X / 2f, textSize.Y / 2f);

            sb.DrawString(
                spriteFont: _font,
                text: _textValue,
                position: new(
                    (int)_position.X + (int)(origin.X * scale.X),
                    (int)_position.Y + (int)(origin.Y * scale.Y)
                    ),
                color: Color.Black,
                rotation: 0f,
                origin: origin,
                scale: scale,
                effects: SpriteEffects.None,
                layerDepth: 0
            );
        }
    }
}
