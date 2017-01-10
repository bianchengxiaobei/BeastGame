using UnityEngine;
using System.Timers;
using Utility;
using Utility.Export;
using System;
using System.Runtime.InteropServices;
using GameClient;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：CWindowHandle
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.4
// 模块描述：Window窗体
//----------------------------------------------------------------*/
#endregion
public class CWindowHandle : Singleton<CWindowHandle>
{
    public enum SWP
    {
        SWP_NOSIZE = 1,
        SWP_NOMOVE,
        SWP_NOZORDER = 4,
        SWP_NOREDRAW = 8,
        SWP_NOACTIVATE = 16,
        SWP_DRAWFRAME = 32,
        SWP_FRAMECHANGED = 32,
        SWP_SHOWWINDOW = 64,
        SWP_HIDEWINDOW = 128,
        SWP_NOCOPYBITS = 256,
        SWP_NOOWNERZORDER = 512,
        SWP_NOREPOSITION = 512,
        SWP_NOSENDCHANGING = 1024,
        SWP_DEFERERASE = 8192,
        SWP_ASYNCWINDOWPOS = 16384
    }
    private const int m_nMaxFlashCount = 5;
    private const int GWL_STYLE = -16;
    private const int WS_CAPTION = 12582912;
    private const float m_nDuraTime = 0.1f;
    private int m_nFlashCount = 0;
    private bool m_bInit = false;
    private bool m_bCorrect = false;
    private bool m_bChangeStatus = false;
    private float m_fStartTime = 0f;
    private Timer m_oWindowTimer = null;
    private static IntPtr m_Handle;
    private IXLog m_log = XLog.GetLog<CWindowHandle>();
    public bool ChangeFullWindowStatus
    {
        set
        {
            if (!value)
            {
                this.SetWindowStatus(true);
            }
            else
            {
                this.m_bChangeStatus = true;
                this.m_fStartTime = Time.time;
            }
        }
    }
    public IntPtr Handle
    {
        get
        {
            return CWindowHandle.m_Handle;
        }
    }
    [DllImport("user32.dll")]
    public static extern bool FlashWindow(IntPtr hWnd, bool bInvert);
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();
    [DllImport("user32.dll")]
    public static extern IntPtr GetActiveWindow();
    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hWnd, int nShow);
    [DllImport("user32.dll", SetLastError = true)]
    public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
    [DllImport("user32.dll")]
    public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int W, int H, uint uFlags);
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern bool SetWindowText(IntPtr hWnd, string windowText);
    public void Init()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            if (!this.m_bInit)
            {
                CWindowHandle.m_Handle = CWindowHandle.GetForegroundWindow();
                this.m_bInit = true;
            }
        }
    }
    public void Update()
    {
        if (this.m_bChangeStatus)
        {
            if (Time.time - this.m_fStartTime >= 0.1f)
            {
                this.SetWindowStatus(false);
                this.m_bChangeStatus = false;
            }
        }
    }
    public void Correct()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            if (!this.m_bCorrect)
            {
                IntPtr foregroundWindow = CWindowHandle.GetForegroundWindow();
                if (CWindowHandle.m_Handle != foregroundWindow)
                {
                    this.Reset();
                    CWindowHandle.m_Handle = foregroundWindow;
                    if (UserOptions.Singleton.DisplayMode == 2)
                    {
                        this.ChangeFullWindowStatus = false;
                    }
                }
                this.m_bCorrect = true;
            }
        }
    }
    public void SetWindowFlash()
    {
        if (null == this.m_oWindowTimer)
        {
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                this.m_nFlashCount = 0;
                this.FlashWindow(true);
                this.m_oWindowTimer = new Timer(500.0);
                this.m_oWindowTimer.Enabled = true;
                this.m_oWindowTimer.AutoReset = true;
                this.m_oWindowTimer.Elapsed += new ElapsedEventHandler(this.WindowElapsed);
            }
        }
    }
    public void SetWindowOpen()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            IntPtr foregroundWindow = CWindowHandle.GetForegroundWindow();
            if (CWindowHandle.m_Handle != foregroundWindow)
            {
                CWindowHandle.ShowWindow(CWindowHandle.m_Handle, 4);
            }
        }
    }
    public void SetWindowStatus(bool bShow)
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            if (bShow)
            {
                CWindowHandle.SetWindowLong(CWindowHandle.m_Handle, -16, CWindowHandle.GetWindowLong(CWindowHandle.m_Handle, -16) | 12582912);
                CWindowHandle.SetWindowPos(CWindowHandle.m_Handle, IntPtr.Zero, 0, 0, 0, 0, 39u);
            }
            else
            {
                if (UserOptions.Singleton.DisplayMode == 2)
                {
                    CWindowHandle.SetWindowLong(CWindowHandle.m_Handle, -16, CWindowHandle.GetWindowLong(CWindowHandle.m_Handle, -16) & -12582913);
                    int width = UserOptions.Singleton.Resolution.Width;
                    int height = UserOptions.Singleton.Resolution.Height;
                    CWindowHandle.SetWindowPos(CWindowHandle.m_Handle, IntPtr.Zero, 0, 0, width, height, 36u);
                }
            }
        }
    }
    private void WindowElapsed(object sender, ElapsedEventArgs e)
    {
        if (this.m_nFlashCount < 5)
        {
            this.FlashWindow(true);
        }
        else
        {
            this.FlashWindow(false);
            this.TimerDispose();
        }
    }
    private void FlashWindow(bool bFlash)
    {
        IntPtr foregroundWindow = CWindowHandle.GetForegroundWindow();
        if (CWindowHandle.m_Handle != foregroundWindow)
        {
            CWindowHandle.FlashWindow(CWindowHandle.m_Handle, bFlash);
            this.m_nFlashCount++;
        }
        else
        {
            this.TimerDispose();
        }
    }
    private void TimerDispose()
    {
        if (null != this.m_oWindowTimer)
        {
            this.m_oWindowTimer.Close();
            this.m_oWindowTimer.Dispose();
            this.m_oWindowTimer = null;
        }
    }
    private void Reset()
    {
        if (UserOptions.Singleton.DisplayMode == 2)
        {
            this.ChangeFullWindowStatus = false;
        }
    }
}
