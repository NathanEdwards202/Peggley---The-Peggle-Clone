using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Misc;
using Peggley.Code.Scenes.Objects.UI.Interactable.Buttons.SceneSwap;
using Scenes;
using Scenes.SceneTypes;
using System;
using System.Collections.Generic;


namespace Peggley
{
    internal class PeggleyGame
    {
        public PeggleyGame()
        {
            _currentScene = new MainMenuScene();
            SetupDelegates();
            OnSceneSwapButtonCreated(this, null); // This has to be called manually on first entering a scene, probably a better way to handle this
        }

        void SetupDelegates()
        {
            SetupSceneSwapDelegates();
        }

        void RemoveDelegates()
        {
            RemoveSceneSwapDelegates();
        }

        Scene _currentScene;
        Scene _nextScene;
        public void Update(GameTime gameTime)
        {
            _currentScene.Update(gameTime);

            if (_currentScene._sceneEnding)
            {
                if (_nextScene == null)
                {
                    Logger.Log("Scene not set before changing scene", Logger.LogLevel.Error);
                    return;
                }
                SwitchScene();
            }
        }

        public void Render(ref SpriteBatch sb, GameWindow window)
        {
            _currentScene.Render(ref sb, window);
        }

        void SwitchScene()
        {
            RemoveSceneSwapDelegates();
            _currentScene?.OnSceneExit();
            _currentScene = _nextScene;
            _currentScene.OnSceneEnter();
            SetupSceneSwapDelegates();
            _nextScene = null;

            OnSceneSwapButtonCreated(this, null); // This has to be called manually on first entering a scene, probably a better way to handle this
        }

        void GenerateNewScene(object sender, SceneSwapButtonEventArgs args)
        {
            // Map types to Scene Creation functions
            Dictionary<Type, Func<Scene>> sceneCreators = new()
            {
                { typeof(MainMenuScene), () => new MainMenuScene() },
                { typeof(MainGameScene), () => new MainGameScene() }
            };

            // Find scene creator, make scene
            if (sceneCreators.TryGetValue(args.sceneType, out Func<Scene> createScene))
            {
                _nextScene = createScene();
                _currentScene.EndScene();
            }
            else
            {
                throw new ArgumentException("Unsupported scene type.", nameof(args.sceneType));
            }
        }
        
        void OnSceneSwapButtonCreated(object obj, EventArgs args)
        {
            foreach (SceneSwapButton btn in _currentScene._sceneSwapButtons)
            {
                btn.buttonClicked -= GenerateNewScene;
            }

            foreach (SceneSwapButton btn in _currentScene._sceneSwapButtons)
            {
                btn.buttonClicked += GenerateNewScene;
            }
        }

        void OnSceneSwapButtonRemoved(object obj, EventArgs args)
        {
            foreach (SceneSwapButton btn in _currentScene._sceneSwapButtons)
            {
                btn.buttonClicked -= GenerateNewScene;
            }
        }

        void SetupSceneSwapDelegates()
        {
            _currentScene.onSceneSwapButtonAdded += OnSceneSwapButtonCreated;
            _currentScene.onSceneSwapButtonRemoved += OnSceneSwapButtonRemoved;
        }

        void RemoveSceneSwapDelegates()
        {
            _currentScene.onSceneSwapButtonAdded -= OnSceneSwapButtonCreated;
            _currentScene.onSceneSwapButtonRemoved -= OnSceneSwapButtonRemoved;
        }
    }
}