using System;
using System.Windows.Forms;
using Power_Options.Classes;
using Power_Options.Properties;

namespace Power_Options.Form
{
    public class TrayIcon
    {
        private static Form1 form;

        public TrayIcon(Form1 form)
        {
            TrayIcon.form = form;

            // Create the notifyIcon1. 
            form.NotifyIcon = new NotifyIcon(form.Components)
                {
                    Icon = Resources.power,
                    ContextMenu = new TrayIconContextMenu(ref form).ContextMenu,
                    Visible = true,
                    BalloonTipIcon = ToolTipIcon.Info,
                    BalloonTipTitle = Resources.Message,
                    BalloonTipText = Resources.Power_options_is_now_running,
                    Text = "",
                };

            // Show notifyIcon.
            form.NotifyIcon.ShowBalloonTip(10000);

            form.NotifyIcon.MouseClick += notifyIcon_MouseClick;

            form.NotifyIcon.MouseMove += notifyIcon_MouseMove;

            form.Paint += FormLayout.DrawRectangles;
        }

        private static void notifyIcon_MouseMove(object sender, MouseEventArgs e)
        {
            form.NotifyIcon.Text = form.FormLoad == null ? ProcessManager.GetActiveScheme() + Resources.Active : form.FormLabels.Scheme.Text;
        }

        private static void notifyIcon_MouseClick(object sender, EventArgs e)
        {
            form.FormLoad = new FormLoad(ref form);
        }
    }
}