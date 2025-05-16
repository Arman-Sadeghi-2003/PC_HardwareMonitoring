using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;

namespace PC_HardwareMonitoring.Infrastructure.NotificationManager
{
	/// <summary>
	/// Responsible for displaying notifications in a specified window using a notification manager.
	/// </summary>
	public class NotificationWindowGenerator
	{
		/// <summary>
		/// Private constructor to prevent external instantiation (Singleton pattern).
		/// </summary>
		private NotificationWindowGenerator() { }
		private static NotificationWindowGenerator instance;
		/// <summary>
		/// Gets the singleton instance of the <see cref="NotificationWindowGenerator"/> class.
		/// </summary>
		public static NotificationWindowGenerator Instance => instance ?? (instance = new NotificationWindowGenerator());

		// ----> Methods

		/// <summary>
		/// Displays the specified notification in the provided top-level window.
		/// </summary>
		/// <param name="notification">The notification to display.</param>
		/// <param name="topLevel">The top-level window in which to show the notification.</param>
		public void ShowNotification(Notification notification, TopLevel? topLevel)
		{
			WindowNotificationManager window = new(topLevel);
			window.Position = NotificationPosition.BottomRight;
			window.Show(notification);
		}
	}
}
