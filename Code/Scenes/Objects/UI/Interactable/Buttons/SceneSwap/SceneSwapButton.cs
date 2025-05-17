using Misc;
using Scenes.Objects.UI.Interactable.Buttons;
using Scenes.SceneTypes;
using System;

namespace Peggley.Code.Scenes.Objects.UI.Interactable.Buttons.SceneSwap
{
    internal class SceneSwapButton : Button
    {
        public SceneSwapButton(Builder builder) : base(builder)
        {}

        public override void Dispose()
        {
            base.Dispose();
            buttonClicked = null;
            GC.SuppressFinalize(this);
        }

        public new event EventHandler<SceneSwapButtonEventArgs> buttonClicked;

        public Type _sceneToSwapToType { get; private set; }
        public MainGameGenerationArgs? _mainGameGenerationArgs { get; private set; }

        public override void OnClick()
        {
            Logger.Log("Scene Swap Button Clicked");

            if (_sceneToSwapToType == null)
            {
                Logger.Log("No scene set to change to on scene swap button click", Logger.LogLevel.Error);
                return;
            }

            buttonClicked?.Invoke(this, new(_sceneToSwapToType, _mainGameGenerationArgs ?? null));
        }

        public void SetData(SceneSwapButtonEventArgs args)
        {
            _sceneToSwapToType = args.sceneType;
            if (args.sceneSwapArgs != null) _mainGameGenerationArgs = (MainGameGenerationArgs)args.sceneSwapArgs;
        }
    }

    internal class SceneSwapButtonEventArgs : EventArgs
    {
        public Type sceneType { get; private set; }

        public MainGameGenerationArgs? sceneSwapArgs { get; private set; }

        public SceneSwapButtonEventArgs(Type t, MainGameGenerationArgs? swapArgs = null)
        {
            sceneType = t;
            sceneSwapArgs = swapArgs;
        }
    }
}
