namespace Misc
{
    internal class WindowManager
    {
        const int WINDOW_X_DEFAULT = 1280;
        const int WINDOW_Y_DEFAULT = 720;

        public static int WindowXDimension;
        public static int WindowYDimension;

        public static void SetDefaultDimensions()
        {
            WindowXDimension = WINDOW_X_DEFAULT;
            WindowYDimension = WINDOW_Y_DEFAULT;
        }
    }
}
