using Microsoft.Xna.Framework;
using Scenes.Objects;
using Scenes.Objects.Hitboxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Misc
{
    public static class ObjectManager
    {
        private static List<GameObject> objects = new();
        public static IReadOnlyList<GameObject> _objects => objects;

        public static bool _dirtyLayer { get; private set; } = true;

        public static EventHandler<OnGameObjectCreatedEventArgs> onGameObjectCreated;
        public class OnGameObjectCreatedEventArgs : EventArgs
        {
            public GameObject obj;
        }

        public static void OnDirtyLayer()
        {
            objects = objects.OrderBy(o => o._isUI ? 1 : 0)
                    .ThenBy(o => o._layer)
                    .ToList();

            _dirtyLayer = false;
        }

        public static bool ValidateObjectIsPopulated(object obj)
        {
            // Get both fields and properties of the object (public and private)
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            // Check fields
            foreach (var field in fields)
            {
                if (field.GetValue(obj) == null)
                {
                    return false;
                }
            }

            // Check properties
            foreach (var property in properties)
            {
                if (property.GetValue(obj) == null)
                {
                    return false;
                }
            }

            return true;
        }

        // AABB moment
        public static bool GetObjectOverlap(GameObject obj1, GameObject obj2)
        {
            return obj1._position.X < obj2._position.X + obj2._size.X &&
                   obj1._position.X + obj1._size.X > obj2._position.X &&

                   obj2._position.Y < obj1._position.Y + obj1._size.Y &&
                   obj2._position.Y + obj2._size.Y > obj1._position.Y;
        }

        #region CollisionManager

        public static class CollisionManager
        {
            // Static method to check collision between two GameObjects
            public static bool CheckCollision(GameObject thisGameObject, GameObject otherGameObject)
            {
                // Pass the GameObject's position and size to the static check methods
                return Hitbox.CheckCollision(thisGameObject, otherGameObject);
            }
        }

        #endregion


        public static float GetDistance(GameObject obj1, GameObject obj2)
        {
            Vector2 pos1 = obj1._position;
            Vector2 pos2 = obj2._position;

            return MathF.Sqrt(MathF.Pow(pos1.X - pos2.X, 2) + MathF.Pow(pos1.Y - pos2.Y, 2));
        }

        public static float GetDistanceFromOrigin(GameObject obj1, GameObject obj2)
        {
            Vector2 pos1 = obj1._position + obj1._size / 2;
            Vector2 pos2 = obj2._position + obj2._size / 2;

            return MathF.Sqrt(MathF.Pow(pos1.X - pos2.X, 2) + MathF.Pow(pos1.Y - pos2.Y, 2));
        }

        public static void SetDirtyLayer()
        {
            _dirtyLayer = true;
        }

        public static void AddObject(GameObject obj)
        {
            objects.Add(obj);
            obj.OnAddedToScene();
        }

        public static void RemoveObject(GameObject obj)
        {
            objects.Remove(obj);
            obj.OnRemovedFromScene();
        }
    }
}
