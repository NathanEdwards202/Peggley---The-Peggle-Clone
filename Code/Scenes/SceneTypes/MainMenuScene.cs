using Microsoft.Xna.Framework;
using Misc;
using Scenes.Objects;
using Scenes.Objects.UI.MainMenu;

namespace Scenes.SceneTypes
{
    internal class MainMenuScene : Scene
    {
        public MainMenuScene() : base()
        {
            _sceneName = "Main Menu";

            foreach(GameObject g in MainMenuUICreator.GenerateSceneUIElements(_sceneName))
            {
                AddToScene(g);
            }

            if (GetType() == typeof(MainMenuScene)) Logger.Log($"{_sceneName} initialized.");
        }

        public override void OnSceneEnter()
        {
            base.OnSceneEnter();
        }

        public override void OnSceneExit()
        {
            base.OnSceneExit();
        }

        public override void Update(GameTime gameTime)
        {
            base.GetInputs(gameTime);

            base.Update(gameTime);
        }
    }
}
