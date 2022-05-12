using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace DTSaveManager.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
		public static DependencyProperty ApplicationActiveProperty =
		DependencyProperty.RegisterAttached(
			"ApplicationActive",
			typeof(bool),
			typeof(MainWindow));

		public bool ApplicationActive
		{
			get { return (bool)GetValue(ApplicationActiveProperty); }
			set { SetValue(ApplicationActiveProperty, value); }
		}

		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);
			((HwndSource)PresentationSource.FromVisual(this)).AddHook(HookProc);
		}

		public static IntPtr HookProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg == WM_GETMINMAXINFO)
			{
				MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

				IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

				if (monitor != IntPtr.Zero)
				{
					MONITORINFO monitorInfo = new MONITORINFO();
					monitorInfo.cbSize = Marshal.SizeOf(typeof(MONITORINFO));
					GetMonitorInfo(monitor, ref monitorInfo);
					RECT rcWorkArea = monitorInfo.rcWork;
					RECT rcMonitorArea = monitorInfo.rcMonitor;
					mmi.ptMaxPosition.X = Math.Abs(rcWorkArea.Left - rcMonitorArea.Left);
					mmi.ptMaxPosition.Y = Math.Abs(rcWorkArea.Top - rcMonitorArea.Top);
					mmi.ptMaxSize.X = Math.Abs(rcWorkArea.Right - rcWorkArea.Left);
					mmi.ptMaxSize.Y = Math.Abs(rcWorkArea.Bottom - rcWorkArea.Top);
				}

				Marshal.StructureToPtr(mmi, lParam, true);
			}

			return IntPtr.Zero;
		}

		private const int WM_GETMINMAXINFO = 0x0024;

		private const uint MONITOR_DEFAULTTONEAREST = 0x00000002;

		[DllImport("user32.dll")]
		private static extern IntPtr MonitorFromWindow(IntPtr handle, uint flags);

		[DllImport("user32.dll")]
		private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

		[Serializable]
		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;

			public RECT(int left, int top, int right, int bottom)
			{
				this.Left = left;
				this.Top = top;
				this.Right = right;
				this.Bottom = bottom;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MONITORINFO
		{
			public int cbSize;
			public RECT rcMonitor;
			public RECT rcWork;
			public uint dwFlags;
		}

		[Serializable]
		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int X;
			public int Y;

			public POINT(int x, int y)
			{
				this.X = x;
				this.Y = y;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MINMAXINFO
		{
			public POINT ptReserved;
			public POINT ptMaxSize;
			public POINT ptMaxPosition;
			public POINT ptMinTrackSize;
			public POINT ptMaxTrackSize;
		}

		public MainWindow()
        {
            InitializeComponent();
			RefreshMaximizeRestoreButton();
			StateChanged += Window_StateChanged;
		}

		private void OnMinimizeButtonClick(object sender, RoutedEventArgs e)
		{
			this.WindowState = WindowState.Minimized;
		}

		private void OnMaximizeRestoreButtonClick(object sender, RoutedEventArgs e)
		{
			if (this.WindowState == WindowState.Maximized)
			{
				this.WindowState = WindowState.Normal;
			}
			else
			{
				this.WindowState = WindowState.Maximized;
			}
		}

		private void OnCloseButtonClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void RefreshMaximizeRestoreButton()
		{
			if (this.WindowState == WindowState.Maximized)
			{
				this.maximizeButton.Visibility = Visibility.Collapsed;
				this.restoreButton.Visibility = Visibility.Visible;
			}
			else
			{
				this.maximizeButton.Visibility = Visibility.Visible;
				this.restoreButton.Visibility = Visibility.Collapsed;
			}
		}

		private void Window_StateChanged(object sender, EventArgs e)
		{
			this.RefreshMaximizeRestoreButton();
		}

		private void MainWindow_Activated(object sender, EventArgs e)
		{
			ApplicationActive = true;
		}

		private void MainWindow_Deactivated(object sender, EventArgs e)
		{
			ApplicationActive = false;
		}
	}
}
