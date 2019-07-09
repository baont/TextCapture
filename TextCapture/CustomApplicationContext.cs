using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using NonInvasiveKeyboardHookLibrary;
using System.Diagnostics;

namespace TextCapture
{

    /// <summary>
    /// Framework for running application as a tray app.
    /// </summary>
    /// <remarks>
    /// Tray app code adapted from "https://www.red-gate.com/simple-talk/dotnet/.net-framework/creating-tray-applications-in-.net-a-practical-guide/", Michael Sorens,
    /// http://windowsclient.net/articles/notifyiconapplications.aspx
    /// </remarks>
    public class CustomApplicationContext : ApplicationContext
    {
        // Icon graphic from http://prothemedesign.com/circular-icons/
        private static readonly string IconFileName = "icon.ico";
        private static readonly string DefaultTooltip = "Extract text from a screen region";
        public static readonly string HOT_KEY = "hotkey";

        /// <summary>
		/// This class should be created and passed into Application.Run( ... )
		/// </summary>
		public CustomApplicationContext() 
		{
			InitializeContext();
            // set the keyhooker
            var keyboardHookManager = new KeyboardHookManager();
            keyboardHookManager.Start();
            keyboardHookManager.RegisterHotkey(0x60, () =>
            {
                Debug.WriteLine("NumPad0 detected");
            });
            keyboardHookManager.RegisterHotkey(NonInvasiveKeyboardHookLibrary.ModifierKeys.Control | NonInvasiveKeyboardHookLibrary.ModifierKeys.Alt, 0x60, () =>
            {
                Debug.WriteLine("Ctrl+Alt+NumPad0 detected");
            });
        }

        private SettingsForm settingsForm;
        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
            notifyIcon.ContextMenuStrip.Items.Clear();
            var settingButton = new ToolStripMenuItem("Settings");
            settingButton.Click += settings_Clicked;
            notifyIcon.ContextMenuStrip.Items.Add(settingButton);
            var exitButton = new ToolStripMenuItem("Exit");
            exitButton.Click += exitItem_Click;
            notifyIcon.ContextMenuStrip.Items.Add(exitButton);
        }

        private void settings_Clicked(object sender, EventArgs e)
        {
            if (settingsForm == null)
            {
                settingsForm = new SettingsForm();
                settingsForm.Closed += settingsFormClosed; // avoid reshowing a disposed form
                settingsForm.StartPosition = FormStartPosition.Manual;

                Rectangle workAreaRectangle = new Rectangle();
                workAreaRectangle = Screen.GetWorkingArea(workAreaRectangle);
                settingsForm.Location = new Point(workAreaRectangle.Width - settingsForm.Width - 10, workAreaRectangle.Height - settingsForm.Height - 10);
                settingsForm.ShowInTaskbar = false;
                settingsForm.Show();
            }
            else { settingsForm.Activate(); }
        }

        // null out the forms so we know to create a new one.
        private void settingsFormClosed(object sender, EventArgs e) {
            settingsForm = null;
        }

        #region the child forms

        // From http://stackoverflow.com/questions/2208690/invoke-notifyicons-context-menu
        private void notifyIcon_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(notifyIcon, null);
            }
        }

        # endregion the child forms

        # region generic code framework

        private System.ComponentModel.IContainer components;	// a list of components to dispose when the context is disposed
        private NotifyIcon notifyIcon;				            // the icon that sits in the system tray

        private void InitializeContext()
        {
            components = new System.ComponentModel.Container();
            notifyIcon = new NotifyIcon(components)
                             {
                                 ContextMenuStrip = new ContextMenuStrip(),
                                 Icon = new Icon(IconFileName),
                                 Text = DefaultTooltip,
                                 Visible = true
                             };
            notifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            notifyIcon.MouseUp += notifyIcon_MouseUp;
        }

        /// <summary>
		/// When the application context is disposed, dispose things like the notify icon.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && components != null) { components.Dispose(); }
		}

		/// <summary>
		/// When the exit menu item is clicked, make a call to terminate the ApplicationContext.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void exitItem_Click(object sender, EventArgs e) 
		{
			ExitThread();
		}

        /// <summary>
        /// If we are presently showing a form, clean it up.
        /// </summary>
        protected override void ExitThreadCore()
        {
            // before we exit, let forms clean themselves up.
            notifyIcon.Visible = false; // should remove lingering tray icon
            base.ExitThreadCore();
        }

        # endregion generic code framework

    }
}
