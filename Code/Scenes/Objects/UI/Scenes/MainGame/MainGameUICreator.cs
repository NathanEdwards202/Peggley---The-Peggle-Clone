using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Misc;
using Peggley.Code.Scenes.Objects.UI.Interactable.Buttons.SceneSwap;
using Scenes.Objects.Hitboxes;
using Scenes.Objects.MainGame.Background;
using Scenes.Objects.UI.Interactable.Buttons;
using System.Collections.Frozen;
using System.Collections.Generic;

namespace Scenes.Objects.UI.MainGame
{
    internal class MainGameUICreator
    {
        #region No Additional Setup Required
        // Create those with no additional setup
        public static List<GameObject> GenerateSceneUIElements(string sceneName)
        {
            return new List<GameObject>()
            {
                GenerateBackgroundImage(sceneName),
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
        const string MAIN_MENU_BACKGROUND_PATH = "MainGame/Background/";
        const string MAIN_MENU_BACKGROUND_TEXTURE_NAME = "TestBackgroundTwo";
        static BackgroundImage GenerateBackgroundImage(string sceneName)
        {
            AssetManager.LoadAsset<Texture2D>(MAIN_MENU_BACKGROUND_PATH + MAIN_MENU_BACKGROUND_TEXTURE_NAME, sceneName);

            return new BackgroundImage.Builder(AssetManager._textures[MAIN_MENU_BACKGROUND_TEXTURE_NAME])
                .SetPosition(new Vector2(0, 0))
                .SetSize(new Vector2(DEFAULT_BG_WIDTH, DEFAULT_BG_HEIGHT))
                .Build<BackgroundImage>();
        }

        #endregion



        #region Additional Setup Required

        const int DEFAULT_SCENE_SWAP_BUTTON_WIDTH = 200;
        const int DEFAULT_SCENE_SWAP_BUTTON_HEIGHT = 100;
        const string SCENE_SWAP_BUTTON_BACKGROUND_PATH = "Non-Scene-Specific/UI/Buttons/";
        const string SCENE_SWAP_BUTTON_BACKGROUND_TEXTURE_NAME = "TestButton";
        public static SceneSwapButton GenerateSceneSwapButton(string sceneName)
        {
            AssetManager.LoadAsset<Texture2D>(SCENE_SWAP_BUTTON_BACKGROUND_PATH + SCENE_SWAP_BUTTON_BACKGROUND_TEXTURE_NAME, sceneName);

            SceneSwapButton btn = new Button.Builder(
                interactableHolder: new(
                    texture: AssetManager._textures[SCENE_SWAP_BUTTON_BACKGROUND_TEXTURE_NAME],
                    alpha: 1
                ),
                hitbox: new RectangleHitbox(
                    collisionFlags: new Dictionary<CollisionFlag, bool>
                    {
                        { CollisionFlag.MOUSE, true },
                    }.ToFrozenDictionary()
                )
                )
                .SetPosition(new(0, 0))
                .SetSize(new(DEFAULT_SCENE_SWAP_BUTTON_WIDTH, DEFAULT_SCENE_SWAP_BUTTON_HEIGHT))
                .SetLayer(1)
                .Build<SceneSwapButton>();

            return btn;
        }

        #endregion
    }
}
