using Avalonia;
using PC_HardwareMonitoring.Infrastructure.URLs;
using System;
using System.IO;

namespace PC_HardwareMonitoring
{
	internal sealed class Program
	{
		// Initialization code. Don't use any Avalonia, third-party APIs or any
		// SynchronizationContext-reliant code before AppMain is called: things aren't initialized
		// yet and stuff might break.
		[STAThread]
		public static void Main(string[] args)
		{
			if (!Directory.Exists(FileNames.HWLocalPath))
				Directory.CreateDirectory(FileNames.HWLocalPath);

			BuildAvaloniaApp()
			.StartWithClassicDesktopLifetime(args);
		}

		// Avalonia configuration, don't remove; also used by visual designer.
		public static AppBuilder BuildAvaloniaApp()
			=> AppBuilder.Configure<App>()
				.UsePlatformDetect()
				.WithInterFont()
				.LogToTrace();
	}
}
