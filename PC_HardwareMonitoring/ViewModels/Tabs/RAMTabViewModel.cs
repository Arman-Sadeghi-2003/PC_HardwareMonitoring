/// <summary>
/// ViewModel for the RAM tab, displaying RAM-related information.
/// </summary>
using CommunityToolkit.Mvvm.ComponentModel;
using PC_HardwareMonitoring.Tools.HW;

namespace PC_HardwareMonitoring.ViewModels.Tabs
{
	public partial class RAMTabViewModel : ViewModelBase
	{
		/// <summary>
		/// Gets or sets the RAM information.
		/// </summary>
		[ObservableProperty]
		private string ramInfos;

		/// <summary>
		/// Initializes a new instance of the <see cref="RAMTabViewModel"/> class.
		/// </summary>
		public RAMTabViewModel()
		{
			RamInfos = Monitor.Instance.GetMemoryInfos();
		}
	}
}
