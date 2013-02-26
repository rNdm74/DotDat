using Power_Options.Classes;

namespace Power_Options.Form
{
    public class FormLoad
    {
        public FormLoad(ref Form1 form)
        {
            // Refresh form.
            form.Refresh();

            // Clear form controls.
            form.Controls.Clear();

            // Access active schemes.
            new ProcessManager();

            // Add controls to form.
            form.FormControls = new FormControls(ref form);

            // Set form location.
            form.Location = SystemTray.Position(form, WinAPI.GetTrayRectangle());

            if (form.Opacity <= 0)
                form.Opacity = 100;

            form.TopMost = true;
            
            form.Show();

            form.Activate();
        }
    }
}