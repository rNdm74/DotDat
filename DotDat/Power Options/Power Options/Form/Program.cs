using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Power_Options.Classes;

namespace Power_Options.Form
{
    public sealed class Form1 : System.Windows.Forms.Form
    {
        /// <summary>
        /// CONSTANTS
        /// </summary>
        private const int WIDTH = 250;

        //private Dictionary<Guid, PowerPlanInfo> schemes;

        /// <summary>
        /// System tray icon
        /// </summary>
        internal TrayIcon TrayIcon;
        internal NotifyIcon NotifyIcon;

        /// <summary>
        /// Form Data Members
        /// </summary>
        internal FormLoad FormLoad;
        internal FormLabels FormLabels;
        internal FormLayout FormLayout;
        internal FormControls FormControls;
        internal FormRadioButtons RadioButtons;
        
        /// <summary>
        /// Properties
        /// </summary>
        internal IContainer Components { get; set; }

        /// <summary>
        /// Runtime
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1
            {
                StartPosition = FormStartPosition.Manual,
                FormBorderStyle = FormBorderStyle.Sizable,
                BackColor = SystemColors.ControlLightLight,
                ControlBox = false,
                MinimizeBox = false,
                MaximizeBox = false,
                ShowInTaskbar = false,
                ShowIcon = false,
            });
        }

        /// <summary>
        /// Contructor
        /// </summary>
        public Form1()
        {
            // Access form components.
            Components = new Container();

            // Set Form width.
            Width = WIDTH;
            
            // Form system tray icon.
            TrayIcon = new TrayIcon(this);
            
            // Hide form.
            Deactivate += Form1_Deactivate;
        }
        
        /// <summary>
        /// Clear members when deactivated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Deactivate(object sender, EventArgs e)
        {
            // Hide form
            Hide();

            // Clear members
            FormLoad = null;
            FormLabels = null;
            FormControls = null;
            RadioButtons = null;
            Power.Schemes = null;
            
            // Garbage Collection
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        
        /// <summary>
        /// Form Overrides
        /// </summary>
        /// <param name="message"></param>
        #region Form Overrides
        protected override void WndProc(ref Message message)
        {
            const int wmNchittest = 0x0084;

            switch (message.Msg)
            {
                case wmNchittest:
                    return;
            }

            base.WndProc(ref message);
        }
        protected override void Dispose(bool disposing)
        {
            // Clean up any components being used. 
            if (disposing)
                if (Components != null)
                    Components.Dispose();

            base.Dispose(disposing);
        }
        protected override void OnLoad(EventArgs e)
        {
            Location = SystemTray.Position(this, WinAPI.GetTrayRectangle());

            Opacity = 0;

            Hide();

            base.OnLoad(e);
        }
        #endregion
    }
}
