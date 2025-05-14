using Controllers.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Misc;
using Scenes.Objects;
using System;
using System.Collections.Generic;

namespace Scenes
{
    internal abstract class Scene
    {
        public Scene()
        {
            _toAddNextFrame = new(); // Do not edit main list of objects mid-loop

            if (this.GetType() == typeof(Scene)) Logger.Log("Scene initialized.");
        }

        protected Queue<GameObject> _toAddNextFrame;

        int _frameCounterForGC = 0; // Tacky but used for forced garbage collection

        public bool _isPaused { get; protected set; } = false;


        #region Initialization
        public virtual void OnSceneEnter()
        {

        }

        public virtual void OnSceneExit()
        {

        }
        #endregion

        // This should be in every scene no matter what
        public virtual void GetInputs(GameTime gameTime)
        {
            MouseController.Update(deltaTime: gameTime.ElapsedGameTime.TotalSeconds);
            KeyboardController.Update(deltaTime: gameTime.ElapsedGameTime.TotalSeconds);
        }


        public virtual void Update(GameTime gameTime)
        {
            if (_isPaused)
            {
                PausedUpdate();
                return;
            }

            // Update Objects
            foreach (GameObject obj in ObjectManager._objects)
            {
                obj.Update(gameTime);
            }

            // Add new objects to scene
            while (_toAddNextFrame.Count > 0)
            {
                AddToScene(_toAddNextFrame.Dequeue());
            }
            _toAddNextFrame.Clear();
            _frameCounterForGC++;
            if (_frameCounterForGC % 3600 == 0)
            {
                GC.Collect(); // Hack as fuck
                _frameCounterForGC = 0;
            }

            // Reorder objects for rendering
            if (ObjectManager._dirtyLayer) ObjectManager.OnDirtyLayer();
        }

        public virtual void PausedUpdate()
        {
            throw new NotImplementedException();
        }

        // Add / remove Gameobjects from the scene
        public static void AddToScene(GameObject g)
        {
            ObjectManager.AddObject(g);
            ObjectManager.SetDirtyLayer();
        }
        public static void RemoveFromScene(GameObject g)
        {
            ObjectManager.RemoveObject(g);
        }

        public virtual void Render(ref SpriteBatch sb, GameWindow window)
        {
            if (_isPaused)
            {
                PausedRender();
                return;
            }


            foreach (GameObject obj in ObjectManager._objects)
            {
                obj.Render(ref sb, window);
            }
        }

        public virtual void PausedRender()
        {
            throw new NotImplementedException();
        }


        public virtual void OnStartPause()
        {
            _isPaused = true;
        }

        public virtual void OnEndPause()
        {
            _isPaused = false;
        }
    }
}
