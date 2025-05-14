using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Misc;
using Scenes.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scenes.SceneTypes
{
    internal class MainGameScene : Scene
    {
        public MainGameScene() : base()
        {
            ObjectManager.SetDirtyLayer();

            if (this.GetType() == typeof(MainGameScene)) Logger.Log($"{_sceneName} initialized.");
        }

        public string _sceneName { get; private set; } = "MainGameScene";


        public override void Update(GameTime gameTime)
        {
            base.GetInputs(gameTime); // Of course

            base.Update(gameTime);

            DoObjectRemoval();
        }

        static void DoObjectRemoval()
        {
            List<GameObject> removalObjects = ObjectManager._objects.Where(o => o._forDeletion).ToList();

            foreach (GameObject obj in removalObjects)
            {
                RemoveFromScene(obj);
            }

            if (removalObjects.Count > 100) // I don't think I've ever managed to have this condition get called lol
            {
                GC.Collect();
            }
        }

        public override void Render(ref SpriteBatch sb, GameWindow window)
        {
            base.Render(ref sb, window);
        }
    }
}
