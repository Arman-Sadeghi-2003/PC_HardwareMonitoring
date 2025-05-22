using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using PC_HardwareMonitoring.Infrastructure.GlobalEnums;

namespace PC_HardwareMonitoring.Infrastructure.NotificationManager
{
	/// <summary>
	/// Provides functionality to generate and display different types of notifications.
	/// </summary>
	public class NotificationGenerator
	{
		/// <summary>
		/// Private constructor to prevent external instantiation.
		/// </summary>
		private NotificationGenerator() { }
		private static NotificationGenerator? instance;
		/// <summary>
		/// Gets the singleton instance of the <see cref="NotificationGenerator"/> class.
		/// </summary>
		public static NotificationGenerator Instance => instance ?? (instance = new NotificationGenerator());

		// ----> Methods

		/// <summary>
		/// Displays a greeting notification with a predefined message.
		/// </summary>
		/// <param name="topLevel">The top-level window to associate the notification with (can be null).</param>
		/// <param name="duration">The duration for which the notification is shown. Defaults to <see cref="NotificationDurations.Medium"/>.</param>
		public void ShowGreetingNotification(TopLevel? topLevel, NotificationDurations duration = NotificationDurations.Medium) 
			=> generateNotification("Greeting", "Welcome to PC hardware monitoring app. Hopefully, this app will be useful to you!"
				, (byte)duration, NotificationType.Information, topLevel);

		public void ShowCPUTempWarning(TopLevel? topLevel, float temperature, NotificationDurations duration = NotificationDurations.Medium)
			=> generateNotification("CPU Temperature Warning!", " The CPU temperature has reached a critical level.\nTemperature: " + temperature.ToString()
				, (byte)duration, NotificationType.Information, topLevel);

		/// <summary>
		/// Generates and displays a notification with the specified parameters.
		/// </summary>
		/// <param name="title">The title of the notification.</param>
		/// <param name="message">The message to display in the notification.</param>
		/// <param name="seconds">The duration in seconds the notification should be displayed.</param>
		/// <param name="type">The type of the notification (e.g., Information, Warning, Error).</param>
		/// <param name="topLevel">The top-level window to associate the notification with (can be null).</param>
		private void generateNotification(string title, string message, byte seconds, NotificationType type, TopLevel? topLevel)
		{
			var notification = new Notification(title, message, type, TimeSpan.FromSeconds(seconds));

			NotificationWindowGenerator.Instance.ShowNotification(notification, topLevel);
		}

	}
}
