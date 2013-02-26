using System;
using System.Windows.Forms;
using Power_Options.Classes;
using Power_Options.Properties;

namespace Power_Options.Form
{
    public class TrayIconContextMenu
    {
        private static Form1 form;

        private readonly MenuItem[] menuItems;

        private readonly String[] menuItemsText = new[]
            {
                Resources.Advanced_power_settings,
                Resources.Power_options,
                Resources._,
                Resources.Turn_system_icon_off
            };

        public ContextMenu ContextMenu { get; set; }

        public TrayIconContextMenu(ref Form1 form)
        {
            TrayIconContextMenu.form = form;

            // Create new menuItem.
            menuItems = new MenuItem[4];

            // Initialize menuItem1.
            for (var i = 0; i < menuItems.Length; i++)
            {
                menuItems[i] = new MenuItem { Index = i, Text = menuItemsText[i] };
                menuItems[i].Click += MenuItem_Click;
            }

            // Create new contextMenu1.
            ContextMenu = new ContextMenu();

            // Initialize contextMenu1.
            ContextMenu.MenuItems.AddRange(menuItems);
        }

        private static void MenuItem_Click(object sender, EventArgs e)
        {
            form.Hide();
            
            var item = (MenuItem) sender;

            switch (item.Index)
            {
                case 0:
                    new ProcessManager().Start("control.exe", " powercfg.cpl,,1");
                    break;
                case 1:
                    new ProcessManager().Start("powercfg.cpl", "");
                    break;
                case 3:
                    Application.Exit();
                    break;
            }
        }
    }
}
