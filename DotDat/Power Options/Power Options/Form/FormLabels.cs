using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Power_Options.Classes;
using Power_Options.Properties;

namespace Power_Options.Form
{
    public class FormLabels
    {
        internal Label Scheme { get; set; }
        internal static LinkLabel PowerOptions { get; set; }

        public FormLabels(ref Form1 form, Rectangle rect)
        {
            //FormLabels.form = form;
            Scheme = new Label
            {
                Font = SystemFonts.MenuFont,
                AutoSize = true,
                ForeColor = SystemColors.Desktop,
                Location = new Point(60, 15)
            };

            PowerOptions = new LinkLabel
                {
                    Font = SystemFonts.MenuFont,
                    AutoSize = true,
                    BackColor = Color.WhiteSmoke,
                    ActiveLinkColor = SystemColors.HotTrack,
                    LinkColor = SystemColors.HotTrack,
                    Text = Resources.More_power_options,
                };

            PowerOptions.Location = new Point((rect.Width / 2) - (PowerOptions.Width / 2) - 10,
                                              (rect.Y + (rect.Height / 2) - (PowerOptions.Height / 2) + 4));

            PowerOptions.LinkBehavior = LinkBehavior.HoverUnderline;

            form.Controls.Add(Scheme);
            form.Controls.Add(PowerOptions);

            PowerOptions.MouseMove += PowerOptions_MouseMove;
            PowerOptions.MouseDown += PowerOptions_MouseDown;
        }

        private static void PowerOptions_MouseDown(object sender, MouseEventArgs e)
        {
            Cursor.Current = CursorHand();

            new ProcessManager().Start("powercfg.cpl", "");
        }

        private static void PowerOptions_MouseMove(object sender, EventArgs e)
        {
            Cursor.Current = CursorHand();
        }

        private static Cursor CursorHand()
        {
            return new Cursor(WinAPI.LoadCursor(IntPtr.Zero, WinAPI.IDC.HAND));
        }
    }
}
