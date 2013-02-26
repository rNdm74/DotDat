using System.Drawing;
using Power_Options.Form;

namespace Power_Options.Classes
{
    class SystemTray
    {
        internal enum TaskbarPosition
        {
            Top = 0,
            Bottom = 1,
            Left = 2,
            Right = 3,
        }

        private static TaskbarPosition Location(ref Rectangle trayRectangle)
        {
            // Get location of the system tray on taskbar.
            if (trayRectangle.Y <= 0)
                return TaskbarPosition.Top;

            if (trayRectangle.X < -100 && trayRectangle.Width < 100)
                return TaskbarPosition.Left;

            if (trayRectangle.X == 0 && trayRectangle.Width < 100)
                return TaskbarPosition.Left;

            if (trayRectangle.X > 0 && trayRectangle.Width < 100)
                return TaskbarPosition.Right;

            if (trayRectangle.X == -62 && trayRectangle.Width < 100)
                return TaskbarPosition.Right;

            return TaskbarPosition.Bottom;
        }

        public static Point Position(Form1 form, Rectangle trayRectangle)
        {
            // Return form position location.
            switch (Location(ref trayRectangle))
            {
                case TaskbarPosition.Top:
                    return new Point((trayRectangle.Left + trayRectangle.Width - form.Width - 12), 
                                     (trayRectangle.Top + 48));
                case TaskbarPosition.Bottom:
                    return new Point((trayRectangle.Left + trayRectangle.Width - form.Width - 12), 
                                     (trayRectangle.Top - form.Height - 8));
                case TaskbarPosition.Left:
                    return new Point((trayRectangle.Left + trayRectangle.Width + 7), 
                                     (trayRectangle.Top + trayRectangle.Height) - form.Height - 12);
                case TaskbarPosition.Right:
                    return new Point((trayRectangle.Left - trayRectangle.Width - form.Width + 55),
                                     (trayRectangle.Top + trayRectangle.Height) - form.Height - 12);
                default:
                    return new Point(0, 0);
            }
        }
    }
}