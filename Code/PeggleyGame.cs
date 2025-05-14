using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Misc;
using Scenes;
using Scenes.SceneTypes;


namespace Peggley
{
    internal class PeggleyGame
    {
        public PeggleyGame()
        {
            // If this was a proper game, current scene would be set to a title screen lol
            // TODO: Implement Switch Scene method
            _currentScene = new MainMenuScene();

            Logger.Log("Game initialized.");
        }

        Scene _currentScene; // Visual studio wants me to make this readonly... But like... If this was made properly, with multiple scene types it wouldn't be

        public void Update(GameTime gameTime)
        {
            _currentScene.Update(gameTime);
        }

        public void Render(ref SpriteBatch sb, GameWindow window)
        {
            _currentScene.Render(ref sb, window);
        }


        public void SwitchScene(Scene newScene)
        {
            _currentScene?.OnSceneExit();
            _currentScene = newScene;
            _currentScene.OnSceneEnter();
        }
    }
}