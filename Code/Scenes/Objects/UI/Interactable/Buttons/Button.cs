using Controllers.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Misc;
using Scenes.Objects.Hitboxes;
using System;


namespace Scenes.Objects.UI.Interactable.Buttons
{
    internal enum ButtonState
    {
        Interactable,
        Hovering,
        Held,
        Pressed,
        Deactivated
    }

    internal class Button : ClickableUIElement
    {
        public Button(Builder builder) : base(builder)
        {
            _interactableStateRenderingHolder = builder.InteractableHolder;
            _hoveringStateRenderingHolder = builder.HoveringHolder ?? builder.InteractableHolder;
            _heldStateRenderingHolder = builder.HeldHolder ?? builder.InteractableHolder;
            _pressedStateRenderingHolder = builder.PressedHolder ?? builder.InteractableHolder;
            _deactivatedStateRenderingHolder = builder.DeactivatedHolder ?? builder.InteractableHolder;

            if (builder.ParentObject != null) _linkedTo = builder.ParentObject;

            ChangeState(builder.DefaultState);

            if (this.GetType() == typeof(Button)) Logger.Log("Button initialized.");
        }

        #region Builder
        public new class Builder : ClickableUIElement.Builder
        {
            public ButtonStateRenderingHolder InteractableHolder { get; private set; }
            public ButtonStateRenderingHolder? HoveringHolder { get; private set; }
            public ButtonStateRenderingHolder? HeldHolder { get; private set; }
            public ButtonStateRenderingHolder? PressedHolder { get; private set; }
            public ButtonStateRenderingHolder? DeactivatedHolder { get; private set; }
            public ButtonState DefaultState { get; private set; }
            public GameObject ParentObject { get; private set; }

            public Builder(ButtonStateRenderingHolder interactableHolder, Hitbox hitbox) : base(hitbox)
            {
                RequiresDisposal = true;
                InteractableHolder = interactableHolder;
                HoveringHolder = null;
                HeldHolder = null;
                PressedHolder = null;
                DeactivatedHolder = null;
                DefaultState = ButtonState.Interactable;
                ParentObject = null;
            }

            public Builder SetHoveringHolder(ButtonStateRenderingHolder holder)
            {
                HoveringHolder = holder;
                return this;
            }

            public Builder SetHeldHolder(ButtonStateRenderingHolder holder)
            {
                HeldHolder = holder;
                return this;
            }

            public Builder SetPressedHolder(ButtonStateRenderingHolder holder)
            {
                PressedHolder = holder;
                return this;
            }

            public Builder SetDeactivatedHolder(ButtonStateRenderingHolder holder)
            {
                DeactivatedHolder = holder;
                return this;
            }

            public Builder SetDefaultState(ButtonState state)
            {
                DefaultState = state;
                return this;
            }

            public Builder SetParentObject(GameObject obj)
            {
                ParentObject = obj;
                return this;
            }

            public Button Build()
            {
                return new Button(this);
            }
        }
        #endregion

        public override void Dispose()
        {
            buttonClicked = null;
            GC.SuppressFinalize(this);
        }

        protected ButtonStateRenderingHolder _interactableStateRenderingHolder;
        protected ButtonStateRenderingHolder _hoveringStateRenderingHolder;
        protected ButtonStateRenderingHolder _heldStateRenderingHolder;
        protected ButtonStateRenderingHolder _pressedStateRenderingHolder;
        protected ButtonStateRenderingHolder _deactivatedStateRenderingHolder;

        public ButtonState _currentState { get; protected set; }
        readonly GameObject? _linkedTo;

        public event EventHandler buttonClicked;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_linkedTo != null) _position = _linkedTo._position;

            DoStateBehaviour();
        }

        protected virtual void DoStateBehaviour() // WHO NEEDS A STATEMACHINE ANYWAY? YIPPEEEEE
        {
            ButtonState? newState;

            newState = _currentState switch
            {
                ButtonState.Interactable => InteractableBehaviour(),
                ButtonState.Hovering => HoveringBehaviour(),
                ButtonState.Held => HeldBehaviour(),
                ButtonState.Pressed => PressedBehaviour(),
                ButtonState.Deactivated => DeactivatedBehaviour(),
                _ => throw new System.Exception("This should not be possible")
            };

            if (newState != null)
            {
                ChangeState(newState.Value);
            }
        }

        // If overlap, goto hovering or held
        protected virtual ButtonState? InteractableBehaviour()
        {
            if (!MouseController.GetMouseOverlap(this)) return null;

            return MouseController.GetButtonPressed(MouseButton.Left) ? ButtonState.Held : ButtonState.Hovering;
        }

        // If no overlap, back to interactable
        // If overlap and press, goto held
        protected virtual ButtonState? HoveringBehaviour()
        {
            if (!MouseController.GetMouseOverlap(this)) return ButtonState.Interactable;

            return MouseController.GetButtonPressed(MouseButton.Left) ? ButtonState.Held : null;
        }

        // Wait until left-mouse-button is no longer held
        protected virtual ButtonState? HeldBehaviour()
        {
            if (MouseController.GetButtonPressed(MouseButton.Left)) return null;

            return ButtonState.Pressed;
        }

        // Do click, return to interactable
        protected virtual ButtonState? PressedBehaviour()
        {
            OnClick();

            return _currentState == ButtonState.Deactivated ? null : ButtonState.Interactable;
        }

        // Do nothing
        protected virtual ButtonState? DeactivatedBehaviour()
        {
            return null;
        }

        protected virtual void ChangeState(ButtonState _newState)
        {
            _currentState = _newState;

            ButtonStateRenderingHolder newRenderingHolder = _currentState switch
            {
                ButtonState.Interactable => _interactableStateRenderingHolder,
                ButtonState.Hovering => _hoveringStateRenderingHolder,
                ButtonState.Held => _heldStateRenderingHolder,
                ButtonState.Pressed => _pressedStateRenderingHolder,
                ButtonState.Deactivated => _deactivatedStateRenderingHolder,
                _ => throw new System.Exception("This should not be possible")
            };

            _texture = newRenderingHolder._texture;
            _alpha = newRenderingHolder._alpha;
        }

        // These next two functions could be made virtual to allow for different default states I guess
        // Could be good in some edge-cases
        public void ActivateButton()
        {
            if (_currentState != ButtonState.Deactivated) return;
            ChangeState(ButtonState.Interactable);
        }

        public void DeactivateButton()
        {
            if (_currentState == ButtonState.Deactivated) return;
            ChangeState(ButtonState.Deactivated);
        }

        public override void OnClick()
        {
            buttonClicked?.Invoke(this, EventArgs.Empty);
        }

        public override void Render(ref SpriteBatch sb, GameWindow window)
        {
            base.Render(ref sb, window);
        }

        public override void RenderRelative(ref SpriteBatch sb, GameWindow window, Vector2 relativeTo)
        {
            base.RenderRelative(ref sb, window, relativeTo);
        }
    }

    public struct ButtonStateRenderingHolder
    {
        public ButtonStateRenderingHolder(Texture2D texture, float alpha = 1)
        {
            _texture = texture;
            _alpha = alpha;
        }

        public Texture2D _texture { get; private set; }
        public float _alpha { get; private set; }
    }
}