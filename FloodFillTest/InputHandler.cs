using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FloodFillTest
{
    public static class InputHandler
    {
        private static KeyboardState keyboardState;
        private static KeyboardState old_keyboardState;
        public static MouseState mouseState;
        public static MouseState old_mouseState;
        
      

        public static Point MousePosition { get { return mouseState.Position; } }
        public static void Update()
        {
            old_keyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
            old_mouseState = mouseState;
            mouseState = Mouse.GetState();
        }

        public static bool WasKeyPressed(Keys key)
        {
            return old_keyboardState.IsKeyDown(key) && keyboardState.IsKeyUp(key);
        }

        public static  bool IsKeyPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        public static bool IsMouseLButtonPressed()
        {
            return mouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool IsMouseRButtonPressed()
        {
            return mouseState.RightButton == ButtonState.Pressed;
        }
    }
}
