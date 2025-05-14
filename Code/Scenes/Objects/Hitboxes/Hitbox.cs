using Microsoft.Xna.Framework;
using System;
using System.Collections.Frozen;

namespace Scenes.Objects.Hitboxes
{
    public enum HitboxType
    {
        RECTANGLE,
        CIRCLE
    }

    public enum CollisionFlag
    {
        MOUSE,
        ENVIRONMENT,
        ACTORS,
        UI
        // Add others later
    }

    public abstract class Hitbox
    {
        public HitboxType _type { get; protected set; }
        public CollisionFlag _collisionFlag { get; protected set; }
        public FrozenDictionary<CollisionFlag, bool> _collisionFlags { get; protected set; }

        // Method to check if the current hitbox can collide with another one based on flags
        public bool CanCollideWith(Hitbox other)
        {
            return _collisionFlags.ContainsKey(other._collisionFlag) && _collisionFlags[other._collisionFlag];
        }

        // Static method to check collision between two GameObjects
        public static bool CheckCollision(GameObject thisGameObject, GameObject otherGameObject)
        {
            // If either GameObject has no hitbox, no collision can occur
            if (thisGameObject._hitbox == null || otherGameObject._hitbox == null)
                return false;

            // Check if the hitboxes can collide based on their flags
            if (!thisGameObject._hitbox.CanCollideWith(otherGameObject._hitbox))
                return false;

            // Perform the collision check based on the hitbox types
            return thisGameObject._hitbox.PerformCollisionChecks(thisGameObject, otherGameObject);
        }

        // Abstract method to be implemented by subclasses to check collision between two hitboxes
        public abstract bool PerformCollisionChecks(GameObject thisGameObject, GameObject otherGameObject);
    }

    public class RectangleHitbox : Hitbox
    {
        public RectangleHitbox(FrozenDictionary<CollisionFlag, bool> collisionFlags)
        {
            _type = HitboxType.RECTANGLE;
            _collisionFlags = collisionFlags;
        }

        // Rectangle-specific collision check
        public override bool PerformCollisionChecks(GameObject thisObject, GameObject otherObject)
        {
            switch (otherObject._hitbox._type)
            {
                case HitboxType.RECTANGLE:
                    return CheckRectangleCollision(thisObject._position, thisObject._size, otherObject._position, otherObject._size);

                case HitboxType.CIRCLE:
                    return CheckRectangleCircleCollision(thisObject._position, thisObject._size, otherObject._position, otherObject._size.X);

                default:
                    throw new ArgumentException("Unsupported hitbox type.");
            }
        }

        // Method to check if two rectangles are colliding
        public bool CheckRectangleCollision(Vector2 thisPos, Vector2 thisSize, Vector2 otherPos, Vector2 otherSize)
        {
            return thisPos.X < otherPos.X + otherSize.X &&
                   thisPos.X + thisSize.X > otherPos.X &&
                   thisPos.Y < otherPos.Y + otherSize.Y &&
                   thisPos.Y + thisSize.Y > otherPos.Y;
        }

        // Method to check if a rectangle and a circle are colliding
        public bool CheckRectangleCircleCollision(Vector2 thisPos, Vector2 thisSize, Vector2 otherPos, float otherRadius)
        {
            float closestX = Math.Clamp(otherPos.X, thisPos.X, thisPos.X + thisSize.X);
            float closestY = Math.Clamp(otherPos.Y, thisPos.Y, thisPos.Y + thisSize.Y);

            float distanceX = otherPos.X - closestX;
            float distanceY = otherPos.Y - closestY;

            return (distanceX * distanceX + distanceY * distanceY) <= (otherRadius * otherRadius);
        }
    }

    public class CircleHitbox : Hitbox
    {
        public CircleHitbox(FrozenDictionary<CollisionFlag, bool> collisionFlags)
        {
            _type = HitboxType.CIRCLE;
            _collisionFlags = collisionFlags;
        }

        public override bool PerformCollisionChecks(GameObject thisObject, GameObject otherObject)
        {
            switch (otherObject._hitbox._type)
            {
                case HitboxType.RECTANGLE:
                    return CheckCirlceRectangleCollision(thisObject._position, thisObject._size.X, otherObject._position, otherObject._size);

                case HitboxType.CIRCLE:
                    return CheckCircleCollision(thisObject._position, thisObject._size, otherObject._position, otherObject._size.X);

                default:
                    throw new ArgumentException("Unsupported hitbox type.");
            }
        }

        // Method to check if two rectangles are colliding
        public bool CheckCirlceRectangleCollision(Vector2 thisPos, float thisRadius, Vector2 otherPos, Vector2 otherSize)
        {
            float closestX = Math.Clamp(otherPos.X, thisPos.X, thisPos.X + otherSize.X);
            float closestY = Math.Clamp(otherPos.Y, thisPos.Y, thisPos.Y + otherSize.Y);

            float distanceX = otherPos.X - closestX;
            float distanceY = otherPos.Y - closestY;

            return (distanceX * distanceX + distanceY * distanceY) <= (thisRadius * thisRadius);
        }

        // Method to check if a rectangle and a circle are colliding
        public bool CheckCircleCollision(Vector2 thisPos, Vector2 thisSize, Vector2 otherPosition, float otherRadius)
        {
            // Calculate the distance between the two circle centers
            float distance = Vector2.Distance(thisPos + thisSize / 2, otherPosition + thisSize);

            // Check if the distance is less than or equal to the sum of the radii
            return distance <= (thisSize.X + otherRadius); // Assuming Size.X is the radius
        }
    }
}
