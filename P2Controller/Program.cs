using System;
using System.Net;
using System.Windows.Forms;

namespace P2Controller;

internal static class Program
{
	[STAThread]
	private static void Main(string[] args)
	{
		if (args.Length != 1)
		{
			MessageBox.Show("Must provide IP address", "IP Error");
			return;
		}
		string iP = IPAddress.Parse(args[0]).ToString().Trim();
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		Application.Run(new controller(iP));
	}
}
