namespace PC_HardwareMonitoring.Models.Hardware
{
	public class SensorValue
	{
		public string Name { get; set; }
		public float? Value { get; set; }

		public SensorValue(string name, float? value)
		{
			Name = name;
			Value = value;
		}
	}
}