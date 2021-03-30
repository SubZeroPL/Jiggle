using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Jiggle
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Synthesizes keystrokes, mouse motions, and button clicks.
        /// </summary>
        [DllImport("user32.dll")]
        private static extern uint SendInput(uint nInputs,
            [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs,
            int cbSize);

        /// <summary>
        /// Enables an application to inform the system that it is in use, thereby preventing the system from entering sleep or turning off the display while the application is running.
        /// </summary>
        /// <param name="esFlags">The thread's execution requirements.</param>
        /// <returns>If the function succeeds, the return value is the previous thread execution state. If the function fails, the return value is NULL.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        private enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_SYSTEM_REQUIRED = 0x00000001,
            ES_DISPLAY_REQUIRED = 0x00000002,
        }

        private const int IconInactive = 0;
        private const int IconActive = 1;

        private EXECUTION_STATE previousState;

        public MainForm()
        {
            InitializeComponent();
        }

        private void Jiggle()
        {
            var input = new INPUT {U = new InputUnion {mi = new MOUSEINPUT()}, type = 0};
            input.U.mi.dx = 0;
            input.U.mi.dy = 0;
            input.U.mi.mouseData = 0;
            input.U.mi.dwFlags = 1;
            input.U.mi.time = 0;
            input.U.mi.dwExtraInfo = (UIntPtr) 0;
            var pInputs = new[] {input};
            SendInput((uint) pInputs.Length, pInputs, INPUT.Size);
        }

        private void jiggleTimer_Tick(object sender, EventArgs e)
        {
            Jiggle();
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            if (Visible)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        private void cbEnabled_CheckedChanged(object sender, EventArgs e)
        {
            jiggleTimer.Enabled = cbEnabled.Checked;

            notifyIcon.Icon = Icon.FromHandle(cbEnabled.Checked
                ? new Bitmap(icons.Images[IconActive]).GetHicon()
                : new Bitmap(icons.Images[IconInactive]).GetHicon());

            if (cbEnabled.Checked)
            {
                previousState = SetThreadExecutionState(EXECUTION_STATE.ES_SYSTEM_REQUIRED |
                                                        EXECUTION_STATE.ES_DISPLAY_REQUIRED |
                                                        EXECUTION_STATE.ES_CONTINUOUS);
            }
            else
            {
                SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
            }
        }
    }
}