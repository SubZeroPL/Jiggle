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

        private const int IconInactive = 0;
        private const int IconActive = 1;

        public MainForm()
        {
            InitializeComponent();
        }

        private void cbEnabled_Click(object sender, EventArgs e)
        {
            jiggleTimer.Enabled = cbEnabled.Checked;

            notifyIcon.Icon = Icon.FromHandle(cbEnabled.Checked
                ? new Bitmap(icons.Images[IconActive]).GetHicon()
                : new Bitmap(icons.Images[IconInactive]).GetHicon());
        }

        private static void Jiggle()
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
    }
}