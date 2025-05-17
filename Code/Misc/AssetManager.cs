using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Scenes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

#nullable enable

namespace Misc
{
    internal static class AssetManager
    {
        public static Dictionary<string, Texture2D> _textures = new();
        public static string? _currentMapTexture = null; // More stuff should've been handled like this, allows me to unload at the end of a scene / its use

        public static Dictionary<string, SpriteFont> _fonts = new();

        // Mapping of scenes to their loaded assets (textures and fonts)
        private static Dictionary<string, HashSet<string>> _sceneAssets = new();


#pragma warning disable CS8618 // VALUE SET AT THE START OF THE PROGRAM THROUGH SetContentManager(). THIS WILL NEVER BE NULL
        public static ContentManager _contentManager { get; private set; }
        public static GraphicsDevice _graphicsDevice { get; private set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        // To be called once at the initialization of the game
        public static void SetContentManger(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            _contentManager = contentManager;
            _graphicsDevice = graphicsDevice;
        }


        // Load a texture at a given path and add it to _textures, accessable via it's name (not path)
        public static void LoadAsset<T>(string textureName, string sceneName)
        {
            // Find string not including any '/'s
            string key = textureName;
            int finalSlash = textureName.LastIndexOf('/');
            if (finalSlash != -1) key = key[(finalSlash + 1)..];

            try
            {
                if (!_sceneAssets.ContainsKey(sceneName))
                {
                    _sceneAssets[sceneName] = new HashSet<string>();
                }

                if (!_sceneAssets.Values.Any(set => set.Contains(key)))
                {
                    switch (Type.GetTypeCode(typeof(T)))
                    {
                        case TypeCode.Object when typeof(T) == typeof(Texture2D):
                            _textures.TryAdd(key, _contentManager.Load<Texture2D>(textureName));
                            Logger.Log($"Texture {key} loaded");
                            break;
                        case TypeCode.Object when typeof(T) == typeof(SpriteFont):
                            _fonts.TryAdd(key, _contentManager.Load<SpriteFont>(textureName));
                            Logger.Log($"Font {key} loaded");
                            break;
                        default:
                            throw new Exception("Unsupported type.");
                    }
                }

                _sceneAssets[sceneName].Add(key);

                var tst = _sceneAssets;

            }
            catch
            {
                throw new Exception($"Failed to load texture: {key}");
            }
        }

        // Stolen (and then further edited) from Stackoverflow, that's why this is different to LoadAsset<T>
        /*
        public static void LoadAssetsFromFile<T>(string folderName, string sceneName)
        {
            //Load directory info, abort if none
            DirectoryInfo dir = new(_contentManager.RootDirectory + "\\" + folderName);
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException($"Failed to find directory: {folderName}");
            }

            //Load all files that matches the file filter
            FileInfo[] files = dir.GetFiles("*.*");
            foreach (FileInfo file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name);

                if (!_sceneAssets.ContainsKey(key))
                {
                    switch (Type.GetTypeCode(typeof(T)))
                    {
                        case TypeCode.Object when typeof(T) == typeof(Texture2D):
                            _textures.Add(key, _contentManager.Load<Texture2D>(folderName + "/" + key));
                            Logger.Log($"Texture {key} loaded");
                            break;
                        case TypeCode.Object when typeof(T) == typeof(SpriteFont):
                            _fonts.Add(key, _contentManager.Load<SpriteFont>(folderName + "/" + key));
                            Logger.Log($"Font {key} loaded");
                            break;
                        default:
                            throw new Exception("Unsupported type.");
                    }

                    _sceneAssets[sceneName] = new HashSet<string>();
                }

                _sceneAssets[sceneName].Add(key);
            }
        }*/

        public static void UnloadAssetsForScene(string sceneName)
        {
            if (_sceneAssets.ContainsKey(sceneName))
            {
                // Create a list of assets to unload
                var assetsToUnload = new List<string>(_sceneAssets[sceneName]);

                // Iterate through the assets of the scene to check if they're used in other scenes
                foreach (string assetName in assetsToUnload)
                {
                    bool isUsedInOtherScenes = false;

                    // Check if the asset is used in any other scene
                    foreach (var scene in _sceneAssets.Keys)
                    {
                        if (scene != sceneName && _sceneAssets[scene].Contains(assetName))
                        {
                            isUsedInOtherScenes = true;
                            break; // If the asset is used in another scene, we can stop checking
                        }
                    }

                    // If the asset is not used in any other scene, unload it
                    if (!isUsedInOtherScenes)
                    {
                        Logger.Log($"Unloading Asset: {assetName}");
                        UnloadAsset(assetName);
                    }
                }

                // Now remove the scene from the mapping once its assets are unloaded
                _sceneAssets.Remove(sceneName);
                Logger.Log($"All assets unloaded for scene {sceneName}");
            }
            else
            {
                Logger.Log($"No assets found for scene {sceneName}");
            }
        }

        public static void UnloadAsset(string assetName)
        {
            if (_textures.ContainsKey(assetName))
            {
                _textures.Remove(assetName);
            }

            if (_fonts.ContainsKey(assetName))
            {
                _fonts.Remove(assetName);
            }
        }

        public static Texture2D GetTexture(string textureName)
        {
            return _textures[textureName];
        }

        public static SpriteFont GetFont(string fontName)
        {
            return _fonts[fontName];
        }


        // Helper functions
        // THIS FUCKER
        // This uses so much memory
        // Someone tell me how to make this better I swear, the only thing I can think is to make them a bit smaller then stretch them
        // But dear god the memory usage
        // (Disregard the previous comments, they were written before I just uh... Parallelized it. Still not good, but not god-awful)
        public static Texture2D CreateCircleTexture(int radius)
        {
            Texture2D texture = new(_graphicsDevice, radius * 2, radius * 2);
            Color[] colorData = new Color[radius * 2 * radius * 2];

            System.Threading.Tasks.Parallel.For(0, radius * 2, y =>
            {
                for (int x = 0; x < radius * 2; x++)
                {
                    int dx = x - radius;
                    int dy = y - radius;

                    if (dx * dx + dy * dy <= radius * radius)
                    {
                        colorData[y * radius * 2 + x] = Color.White;
                    }
                    else
                    {
                        colorData[y * radius * 2 + x] = Color.Transparent;
                    }
                }
            });

            texture.SetData(colorData);
            return texture;
        }
    }
}