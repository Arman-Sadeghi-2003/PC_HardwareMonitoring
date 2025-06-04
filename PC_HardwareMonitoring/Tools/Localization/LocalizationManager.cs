using Avalonia.Controls;
using Avalonia.Markup.Xaml.Styling;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace PC_HardwareMonitoring.Tools.Localization
{
	/// <summary>
	/// Manages localization and resource updates for the application.
	/// </summary>
	public class LocalizationManager
	{
		private LocalizationManager() { }
		private static LocalizationManager instance;

		/// <summary>
		/// Gets the singleton instance of the <see cref="LocalizationManager"/> class.
		/// </summary>
		public static LocalizationManager Instance => instance ?? (instance = new LocalizationManager());

		/// <summary>
		/// Updates the application's resources for a given culture.
		/// </summary>
		/// <param name="culture">The culture to update resources for.</param>
		public async Task UpdateResourcesForCulture(CultureInfo culture)
		{
			// Clear existing localization resources
			var resources = App.Current.Resources.MergedDictionaries.Where(x => x is ResourceInclude resourceInclude &&
				resourceInclude.Source.AbsoluteUri.Contains("/Localization/")).ToList();

			foreach (var resource in resources)
			{
				App.Current.Resources.MergedDictionaries.Remove(resource);
			}

			// Add the resource dictionary for the specified culture
			var resourcePath = $"avares://PC_HardwareMonitoring/Assets/Localization/Strings.{culture.Name}.axaml";

			// Fallback to default language if the specific culture resource doesn't exist
			if (!Uri.TryCreate(resourcePath, UriKind.Absolute, out _) || culture.Name == "en-US")
			{
				resourcePath = "avares://PC_HardwareMonitoring/Assets/Localization/Strings.axaml";
			}

			var uri = new Uri(resourcePath);
			var resourceInclude = new ResourceInclude(uri);
			resourceInclude.Source = uri;  // Set the Source property before adding to MergedDictionaries
			App.Current.Resources.MergedDictionaries.Add(resourceInclude);

			await Task.Delay(50);
		}

		/// <summary>
		/// Changes the application's language at runtime.
		/// </summary>
		/// <param name="languageCode">The language code to change to (e.g., "en-US", "fr-FR").</param>
		public void ChangeLanguage(string languageCode)
		{
			var newCulture = new CultureInfo(languageCode);
			CultureInfo.DefaultThreadCurrentCulture = newCulture;
			CultureInfo.DefaultThreadCurrentUICulture = newCulture;

			UpdateResourcesForCulture(newCulture);
		}

		/// <summary>
		/// Gets a localized string for a given key.
		/// </summary>
		/// <param name="key">The key of the string to retrieve.</param>
		/// <returns>The localized string, or the key if the string is not found.</returns>
		public string GetString(string key)
		{
			try
			{
				object? value = new();
				App.Current?.TryFindResource(key, out value);
				var resource = value?.ToString();
				if (resource != null && !resource.ToString().Equals("Unset"))
					return resource.ToString();
			}
			catch { }

			return key; // Fallback to key
		}
	}
}