namespace PC_HardwareMonitoring.Infrastructure.URLs
{
	public abstract class BaseUrls
	{
		private static string localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

		public static string HWLocalPath = Path.Combine(localPath, "PC_HW_Monitoring (AILC)");
	}
}
