using LibreHardwareMonitor.Hardware;

namespace PC_HardwareMonitoring.Tools.HW
{
	public class UpdateVisitor : IVisitor
	{
		public void VisitComputer(IComputer computer)
		{
			computer.Traverse(this);
		}

		public void VisitHardware(IHardware hardware)
		{
			hardware.Update();
			foreach (IHardware subHardware in hardware.SubHardware)
				subHardware.Accept(this);
		}

		public void VisitParameter(IParameter parameter)
		{ }

		public void VisitSensor(ISensor sensor)
		{ }
	}
}