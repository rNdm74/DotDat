using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Power_Options.Classes;
using Power_Options.Properties;
using ToolTip = System.Windows.Forms.ToolTip;

namespace Power_Options.Form
{
    /// <summary>
    /// Creates radiobuttons
    /// </summary>
    public class FormRadioButtons
    {
        /// <summary>
        /// Private data members 
        /// </summary>
        private static Form1 form;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="form"></param>
        public FormRadioButtons(ref Form1 form)
        {
            // Populate pdm
            FormRadioButtons.form = form;

            // Create form labels
            form.FormLabels = new FormLabels(ref form, new FormRectangles().BottomRect());

            // Create control size
            var button = new Control[Power.Schemes.Count];

            // Initialize controls
            for (var i = 0; i < button.Length; i++)
            {
                button[i] = new RadioButton
                {
                    Location = new Point(Convert.ToInt16(Resources.RADIOBUTTON_LEFT), (new FormRectangles().CenterRect().Top + 8) + 25 * (i + 1)),
                    Size = new Size(Convert.ToInt16(Resources.RADIOBUTTON_WIDTH), Convert.ToInt16(Resources.RADIOBUTTON_HEIGHT)),
                    Font = SystemFonts.MenuFont,
                    Text = Power.Schemes[i].Name,
                    Checked = ProcessManager.GetActiveScheme() == Power.Schemes[i].Name,
                };

                new ToolTip
                {
                    AutoPopDelay = 5000,
                    InitialDelay = 1000,
                    ReshowDelay = 500,
                    ShowAlways = true
                }.SetToolTip(button[i], Power.Schemes[i].Tooltip);

                button[i].MouseClick += RadioButtons_MouseClick;
            }

            // Add controls to form
            form.Controls.AddRange(button);

            // Set label (Active Scheme)
            form.FormLabels.Scheme.Text = ProcessManager.GetActiveScheme() + Resources.Active;
        }

        /// <summary>
        /// Generic event handler for radiobuttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void RadioButtons_MouseClick(object sender, MouseEventArgs e)
        {
            // Populate clicked button into variable
            var button = (RadioButton) sender;

            // Set label text
            form.FormLabels.Scheme.Text = button.Text + Resources.Active;

            // Start process to change power scheme
            foreach (var plan in PowerSchemeHelper.GetAllPowerSchemas().Where(plan => button.Text == plan.Value.FriendlyName))
            {
                PowerSchemeHelper.SetPowerScheme(plan.Value.SchemeGuid);
            }
        }
    }
}
