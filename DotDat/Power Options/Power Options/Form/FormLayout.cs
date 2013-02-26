using System.Drawing;
using System.Windows.Forms;

namespace Power_Options.Form
{
    public class FormLayout
    {
        private static void TopRectangle(ref PaintEventArgs e)
        {
            // Draw rectangle to screen.
            e.Graphics.DrawRectangle(new Pen(Color.Gainsboro, 1), new FormRectangles().TopRect());

            //Draw icon to the screen
            e.Graphics.DrawIcon(Properties.Resources.poweroptions, 22, 7);
        }

        private static void CenterRectangle(ref PaintEventArgs e)
        {
            e.Graphics.DrawString("Select a power plan:", SystemFonts.MenuFont, new SolidBrush(SystemColors.ControlDark), new PointF(20, 58));
        }

        private static void BottomRectangle(ref PaintEventArgs e)
        {
            // Draw rectangle to screen.
            e.Graphics.DrawRectangle(new Pen(Color.Gainsboro, 2), new FormRectangles().BottomRect());

            // Fill rectangle to screen.
            e.Graphics.FillRectangle(new SolidBrush(Color.WhiteSmoke), new FormRectangles().BottomRect());
        }

        public static void DrawRectangles(object sender, PaintEventArgs e)
        {
            TopRectangle(ref e);
            CenterRectangle(ref e);
            BottomRectangle(ref e);

            e.Dispose();
        }
    }
}
