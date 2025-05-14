using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Misc;
using Scenes.Objects.MainGame.Background;
using System.Collections.Generic;

namespace Scenes.Objects.UI.MainMenu
{
    internal class MainMenuUICreator
    {
        public static List<GameObject> GenerateSceneUIElements(string sceneName)
        {
            return new List<GameObject>()
            {
                GenerateBackgroundImage(sceneName)
            };
        }


        /// Each UI object will follow the same loading procedure
        // Required constants defined before the function
        // Attempt to load in required assets, this will skip if the asset is loaded so don't worry about copying
        // Create the object
        // Update parameters
        // Return the created object


        const int DEFAULT_BG_WIDTH = 1280;
        const int DEFAULT_BG_HEIGHT = 720;
        const string MAIN_MENU_BACKGROUND_PATH = "MainMenu/Background/";
        const string MAIN_MENU_BACKGROUND_TEXTURE_NAME = "TestBackgroundOne";
        static BackgroundImage GenerateBackgroundImage(string sceneName)
        {
            AssetManager.LoadAsset<Texture2D>(MAIN_MENU_BACKGROUND_PATH + MAIN_MENU_BACKGROUND_TEXTURE_NAME, sceneName);

            return new BackgroundImage.Builder(AssetManager._textures[MAIN_MENU_BACKGROUND_TEXTURE_NAME])
                .SetPosition(new Vector2(0, 0))
                .SetSize(new Vector2(DEFAULT_BG_WIDTH, DEFAULT_BG_HEIGHT))
                .Build<BackgroundImage>();
        }
    }
}
