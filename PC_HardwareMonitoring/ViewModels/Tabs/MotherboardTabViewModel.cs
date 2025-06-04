/// <summary>
/// ViewModel for the Motherboard tab, displaying motherboard-related information.
/// </summary>
using CommunityToolkit.Mvvm.ComponentModel;
using PC_HardwareMonitoring.Tools.HW;

namespace PC_HardwareMonitoring.ViewModels.Tabs
{
	public partial class MotherboardTabViewModel : ViewModelBase
	{
		/// <summary>
		/// Gets or sets the motherboard information.
		/// </summary>
		[ObservableProperty]
		private string motherboardInfos;

		/// <summary>
		/// Initializes a new instance of the <see cref="MotherboardTabViewModel"/> class.
		/// </summary>
		public MotherboardTabViewModel()
		{
			MotherboardInfos = Monitor.Instance.GetMotherboardInfos();
		}
	}
}
