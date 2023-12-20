#nullable enable
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Trinet.Core.IO.Ntfs;

namespace Jiggle
{
    public partial class MainForm : Form
    {
        private const string StreamName = "Jiggle.Config";

        private const int IconInactive = 0;
        private const int IconActive = 1;

        private Config _config = new()
        {
            Enabled = false, MinimizeAfterEnabling = false, CloseButtonMinimizesWindow = false,
            RightClickOnIconClosesApplication = false
        };

        private bool _fromIcon;

        private readonly ResourceManager _rm = new(typeof(MainForm));

        public MainForm()
        {
            InitializeComponent();
            ReadConfig();
            Application.ApplicationExit += (_, _) => { WriteConfig(); };
        }

        /// <summary>
        ///     Synthesizes keystrokes, mouse motions, and button clicks.
        /// </summary>
        [DllImport("user32.dll")]
        private static extern uint SendInput(uint nInputs,
            [MarshalAs(UnmanagedType.LPArray)] [In]
            INPUT[] pInputs,
            int cbSize);

        /// <summary>
        ///     Enables an application to inform the system that it is in use, thereby preventing the system from entering sleep or
        ///     turning off the display while the application is running.
        /// </summary>
        /// <param name="esFlags">The thread's execution requirements.</param>
        /// <returns>
        ///     If the function succeeds, the return value is the previous thread execution state. If the function fails, the
        ///     return value is NULL.
        /// </returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern ExecutionState SetThreadExecutionState(ExecutionState esFlags);

        /// <summary>
        /// Get path of launcher executable.
        /// </summary>
        /// <returns>Path of launcher executable</returns>
        private static string? GetPath()
        {
            return Process.GetCurrentProcess().MainModule?.FileName;
        }

        private void ReadConfig()
        {
            var adsi = FileSystem.GetAlternateDataStream(GetPath(), StreamName);
            try
            {
                var stream = adsi.OpenRead();
                if (!stream.CanRead) return;
                using (var sr = new StreamReader(stream))
                {
                    var config = sr.ReadToEnd();
                    var localConfig = JsonSerializer.Deserialize<Config>(config);
                    if (localConfig != null)
                    {
                        _config = localConfig;
                    }

                    cbMinimize.Checked = _config.MinimizeAfterEnabling;
                    cbCloseMinimize.Checked = _config.CloseButtonMinimizesWindow;
                    cbRightClick.Checked = _config.RightClickOnIconClosesApplication;
                }

                void Handler(object? sender, EventArgs args)
                {
                    cbEnabled.Checked = _config.Enabled;
                    Shown -= Handler;
                }

                Shown += Handler;
            }
            catch (FileNotFoundException fnf)
            {
                notifyIcon.BalloonTipIcon = ToolTipIcon.Error;
                notifyIcon.BalloonTipTitle = _rm.GetString("MainForm_WriteConfig_Error") ?? "Error";
                var message = fnf.Message;
                if (fnf.FileName != null && fnf.FileName.Contains(StreamName))
                {
                    message = string.Empty;
                }

                notifyIcon.BalloonTipText =
                    string.Format(_rm.GetString("MainForm_ReadConfig_CannotLoadConfiguration") ?? string.Empty,
                        message);
                notifyIcon.ShowBalloonTip(5000);
            }
            catch (Exception e)
            {
                notifyIcon.BalloonTipIcon = ToolTipIcon.Error;
                notifyIcon.BalloonTipTitle = _rm.GetString("MainForm_WriteConfig_Error") ?? "Error";
                notifyIcon.BalloonTipText =
                    string.Format(
                        _rm.GetString("MainForm_ReadConfig_CannotLoadConfiguration") ??
                        "Cannot load configuration, using default values.\n{0}",
                        e.Message);
                notifyIcon.ShowBalloonTip(5000);
            }
        }

        private void WriteConfig()
        {
            var adsi = FileSystem.GetAlternateDataStream(GetPath(), StreamName);
            try
            {
                adsi.Delete();
                var stream = adsi.OpenWrite();
                if (!stream.CanWrite) return;
                using var sw = new StreamWriter(stream);
                _config.Enabled = cbEnabled.Checked;
                _config.MinimizeAfterEnabling = cbMinimize.Checked;
                _config.CloseButtonMinimizesWindow = cbCloseMinimize.Checked;
                _config.RightClickOnIconClosesApplication = cbRightClick.Checked;
                var config = JsonSerializer.Serialize(_config);
                sw.Write(config);
            }
            catch (Exception e)
            {
                notifyIcon.BalloonTipIcon = ToolTipIcon.Error;
                notifyIcon.BalloonTipTitle = _rm.GetString("MainForm_WriteConfig_Error") ?? "Error";
                notifyIcon.BalloonTipText =
                    string.Format(
                        _rm.GetString("MainForm_WriteConfig_CannotSaveConfiguration") ??
                        "Cannot save configuration.\n{0}",
                        e.Message);
                notifyIcon.ShowBalloonTip(5000);
            }
        }

        private static void Jiggle()
        {
            var input = new INPUT { U = new InputUnion { mi = new MOUSEINPUT() }, type = 0 };
            input.U.mi.dx = 0;
            input.U.mi.dy = 0;
            input.U.mi.mouseData = 0;
            input.U.mi.dwFlags = 1;
            input.U.mi.time = 0;
            // ReSharper disable once RedundantCast - needed for .NET 6
            input.U.mi.dwExtraInfo = (UIntPtr)0;
            var pInputs = new[] { input };
            SendInput((uint)pInputs.Length, pInputs, INPUT.Size);
        }

        private void jiggleTimer_Tick(object sender, EventArgs e)
        {
            Jiggle();
        }

        private void cbEnabled_CheckedChanged(object sender, EventArgs e)
        {
            jiggleTimer.Enabled = cbEnabled.Checked;

            notifyIcon.Icon = Icon.FromHandle(cbEnabled.Checked
                ? new Bitmap(icons.Images[IconActive]).GetHicon()
                : new Bitmap(icons.Images[IconInactive]).GetHicon());

            if (cbEnabled.Checked)
            {
                SetThreadExecutionState(ExecutionState.EsSystemRequired |
                                        ExecutionState.EsDisplayRequired |
                                        ExecutionState.EsContinuous);
                if (cbMinimize.Checked) Hide();
            }
            else
            {
                SetThreadExecutionState(ExecutionState.EsContinuous);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing || !cbCloseMinimize.Checked || _fromIcon) return;
            e.Cancel = true;
            Hide();
        }

        [Flags]
        private enum ExecutionState : uint
        {
            EsContinuous = 0x80000000,
            EsSystemRequired = 0x00000001,
            EsDisplayRequired = 0x00000002
        }

        private bool _isSingleClick;

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            _isSingleClick = false;
            Visible = e.Button switch
            {
                MouseButtons.Left => !Visible,
                _ => Visible
            };
        }

        private async void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            _isSingleClick = true;
            await Task.Delay(SystemInformation.DoubleClickTime);
            if (!_isSingleClick) return;

            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (e.Button)
            {
                case MouseButtons.Left:
                    cbEnabled.Checked = !cbEnabled.Checked;
                    break;
                case MouseButtons.Right when cbRightClick.Checked:
                    _fromIcon = true;
                    Close();
                    break;
            }

            _isSingleClick = false;
        }
    }
}