using System.Drawing;

namespace Power_Options.Form
{
    public class FormControls
    {
        public FormControls(ref Form1 form)
        {
            // RadioButtons on form.
            form.RadioButtons = new FormRadioButtons(ref form);

            // Set form size.
            form.ClientSize = new Size(250, new FormRectangles().TotalRectHeight());
        }
    }
}