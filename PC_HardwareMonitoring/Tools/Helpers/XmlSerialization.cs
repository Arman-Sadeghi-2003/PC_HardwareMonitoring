using System;
using System.IO;
using System.Xml.Serialization;

namespace PC_HardwareMonitoring.Tools.Helpers
{
	/// <summary>
	/// Provides functions for performing common XML Serialization operations.
	/// </summary>
	/// <remarks>
	/// Only public properties and variables will be serialized.
	/// Use the [XmlIgnore] attribute to prevent a property/variable from being serialized.
	/// Object to be serialized must have a parameterless constructor.
	/// </remarks>
	public static class XmlSerialization
	{
		/// <summary>
		/// Writes the given object instance to an XML file.
		/// </summary>
		/// <typeparam name="T">The type of object being written to the file.</typeparam>
		/// <param name="filePath">The file path to write the object instance to.</param>
		/// <param name="objectToWrite">The object instance to write to the file.</param>
		/// <param name="append">If false, the file will be overwritten if it already exists. If true, the contents will be appended to the file.</param>
		public static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
		{
			TextWriter writer = null;
			try
			{
				var serializer = new XmlSerializer(typeof(T));
				writer = new StreamWriter(filePath, append);
				serializer.Serialize(writer, objectToWrite);
			}
			catch
			{
				// Handle serialization exceptions here. Log the exception or take appropriate action.
			}
			finally
			{
				if (writer != null)
				{
					writer.Close();
				}
			}
		}

		/// <summary>
		/// Reads an object instance from an XML file.
		/// </summary>
		/// <typeparam name="T">The type of object to read from the file.</typeparam>
		/// <param name="filePath">The file path to read the object instance from.</param>
		/// <returns>Returns a new instance of the object read from the XML file.</returns>
		public static T ReadFromXmlFile<T>(string filePath) where T : new()
		{
			TextReader reader = null;
			try
			{
				var serializer = new XmlSerializer(typeof(T));
				reader = new StreamReader(filePath);
				return (T)serializer.Deserialize(reader);
			}
			catch (Exception)
			{
				// Consider re-throwing the exception or handling it more specifically.
				throw;
			}
			finally
			{
				if (reader != null)
				{
					reader.Close();
				}
			}
		}
	}
}