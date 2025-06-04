using LibreHardwareMonitor.Hardware;

namespace PC_HardwareMonitoring.Tools.HW
{
	/// <summary>
	/// Implements the <see cref="IVisitor"/> interface to update hardware information.
	/// </summary>
	public class UpdateVisitor : IVisitor
	{
		/// <summary>
		/// Visits a computer and traverses its hardware components to update them.
		/// </summary>
		/// <param name="computer">The computer to visit.</param>
		public void VisitComputer(IComputer computer)
		{
			computer.Traverse(this);
		}

		/// <summary>
		/// Visits a hardware component and updates it, then recursively visits its sub-hardware components.
		/// </summary>
		/// <param name="hardware">The hardware component to visit.</param>
		public void VisitHardware(IHardware hardware)
		{
			hardware.Update();

			foreach (IHardware subHardware in hardware.SubHardware)
			{
				subHardware.Accept(this);
			}
		}

		/// <summary>
		/// Visits a parameter (not implemented).
		/// </summary>
		/// <param name="parameter">The parameter to visit.</param>
		public void VisitParameter(IParameter parameter)
		{ }

		/// <summary>
		/// Visits a sensor (not implemented).
		/// </summary>
		/// <param name="sensor">The sensor to visit.</param>
		public void VisitSensor(ISensor sensor)
		{ }
	}
}