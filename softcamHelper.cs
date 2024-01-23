using System;
using System.Runtime.InteropServices;

public static class softcamHelper
{
	[DllImport("softcam.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern IntPtr scCreateCamera(int width, int height, float framerate);

	[DllImport("softcam.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern void scDeleteCamera(IntPtr camera);

	[DllImport("softcam.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern void scSendFrame(IntPtr camera, byte[] image_bits);

	[DllImport("softcam.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern bool scWaitForConnection(IntPtr camera, float timeout);
}
