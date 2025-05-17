using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Misc;
using Scenes.Objects.Hitboxes;
using System;

#nullable enable

namespace Scenes.Objects
{
    public abstract class GameObject
    {
        // Constructor and Initializer
        protected GameObject(Builder builder)
        {
            _requiresDisposal = builder.RequiresDisposal;
            _position = builder.Position;
            _rotation = builder.Rotation;
            _layer = builder.Layer;
            _isUI = builder.IsUI;
            _size = builder.Size;
            _hitbox = builder.Hitbox;
            _alpha = builder.Alpha;

            _texture = builder.Texture;

            _forDeletion = false;
        }

        #region BUILDER
        public class Builder
        {
            public bool RequiresDisposal { get; protected set; }
            public Vector2 Position { get; protected set; }
            public int Layer { get; protected set; }
            public bool IsUI { get; protected set; }
            public float Rotation { get; protected set; }
            public Vector2 Size { get; protected set; }
            public Hitbox? Hitbox { get; protected set; }
            public float Alpha { get; protected set; }
            public Texture2D? Texture { get; protected set; }

            // Default constructor with default values
            public Builder()
            {
                RequiresDisposal = false;
                Position = new Vector2();
                Layer = 0;
                IsUI = false;
                Rotation = 0;
                Size = new Vector2();
                Hitbox = null;
                Alpha = 1;
                Texture = null;
            }

            // Fluent methods to set values
            public Builder SetPosition(Vector2 position)
            {
                Position = position;
                return this;
            }

            public Builder SetLayer(int layer)
            {
                Layer = layer;
                return this;
            }

            public Builder SetIsUI(bool isUI)
            {
                IsUI = isUI;
                return this;
            }

            public Builder SetRotation(float rotation)
            {
                Rotation = rotation;
                return this;
            }

            public Builder SetSize(Vector2 size)
            {
                Size = size;
                return this;
            }
            public Builder SetHitBox(Hitbox hitbox)
            {
                Hitbox = hitbox;
                return this;
            }

            public Builder SetAlpha(float alpha)
            {
                Alpha = alpha;
                return this;
            }

            public Builder SetTexture(Texture2D texture)
            {
                Texture = texture;
                return this;
            }

            // Method to create the concrete GameObject instance
            public T Build<T>() where T : GameObject
            {
                return (T)Activator.CreateInstance(typeof(T), this);
            }
        }
        #endregion

        #region Initialization
        public virtual void OnAddedToScene() { }
        #endregion

        #region Deletion
        public virtual void OnRemovedFromScene()
        {
            if (_requiresDisposal && !_disposed)
            {
                Dispose();
                _disposed = true;
            }
        }

        public virtual void Dispose() { }
        #endregion



        public Vector2 _position { get; protected set; }
        public float _rotation { get; protected set; }
        public int _layer { get; protected set; }
        public bool _isUI { get; protected set; }

        public Vector2 _size { get; protected set; }

        public Hitbox? _hitbox { get; protected set; }

        public bool _forDeletion { get; protected set; } // Used to destroy the fucker at the end of the frame
        readonly protected bool _requiresDisposal;
        protected bool _disposed = false;

        public Texture2D? _texture { get; protected set; }
        public float _alpha { get; protected set; } // Transparency


        public virtual void Update(GameTime gameTime)
        {

        }

        // I physically hate the sheer amount of casts to int here, but ehhhhh, it's not been a problem
        public virtual void Render(ref SpriteBatch sb, GameWindow window)
        {
            if (_texture == null) return; // And this is why texture is nullable

            Vector2 origin = new(_size.X / 2f, _size.Y / 2f);

            sb.Draw(
                texture: _texture,
                destinationRectangle: new Rectangle(
                        (int)_position.X + (int)origin.X,
                        (int)_position.Y + (int)origin.Y,
                        (int)_size.X,
                        (int)_size.Y
                        ),
                sourceRectangle: null,
                color: Color.White * _alpha,
                rotation: _rotation,
                origin: origin,
                effects: SpriteEffects.None,
                layerDepth: 0
                );
        }

        public virtual void RenderRelative(ref SpriteBatch sb, GameWindow window, Vector2 relativeTo)
        {
            if (_texture == null) return;

            Vector2 origin = new(_size.X / 2f, _size.Y / 2f);

            sb.Draw(
                texture: _texture,
                destinationRectangle: new Rectangle(
                        (int)relativeTo.X + (int)_position.X + (int)origin.X,
                        (int)relativeTo.Y + (int)_position.Y + (int)origin.Y,
                        (int)_size.X,
                        (int)_size.Y
                        ),
                sourceRectangle: null,
                color: Color.White * _alpha,
                rotation: _rotation,
                origin: origin,
                effects: SpriteEffects.None,
                layerDepth: 0
                );
        }


        // Helper function
        protected static float GetRadiansFromNormalizedVector(Vector2 normalizedVector)
        {
            float radians = MathF.Atan2(normalizedVector.Y, normalizedVector.X);

            // Normalize angle to [0, 2pi]
            if (radians < 0) radians += MathF.PI * 2;

            return radians;
        }
    }
}
