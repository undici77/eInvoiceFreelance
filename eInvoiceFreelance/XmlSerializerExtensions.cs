using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace eInvoiceFreelance.Serializer
{
	public static class XmlSerializerExtensions
	{
		private const string Decimal_Formatter = "0.00";

		public static void SerializeWithDecimalFormatting(this XmlSerializer serializer, StreamWriter stream, object obj, XmlSerializerNamespaces name_space)
		{
			IteratePropertiesRecursively(obj);

			serializer.Serialize(stream, obj, name_space);
		}

		private static void IteratePropertiesRecursively(object obj)
		{
			Type           type;
			PropertyInfo[] properties;
			Type           property_type;
			object         val;
			IList          elements;

			if (obj == null)
			{
				return;
			}

			type = obj.GetType();

			properties = type.GetProperties();

			foreach (var p in properties)
			{
				property_type = p.PropertyType;

				// if property is a generic list
				if (property_type.IsArray)
				{
					val      = p.GetValue(obj, null);
					elements = val as IList;

					if (elements != null)
					{
						foreach (var item in elements)
						{
							IteratePropertiesRecursively(item);
						}
					}
				}
				else if (property_type == typeof(decimal))
				{
					// check if there is a property with name XXXSpecified, this is the case if we have a type of decimal?
					var specified_property_name = string.Format("{0}Specified", p.Name);
					var is_specified_property = type.GetProperty(specified_property_name);

					if (is_specified_property != null)
					{
						var is_specified_property_value = is_specified_property.GetValue(obj, null) as bool?;
						if (is_specified_property_value == true)
						{
							FormatDecimal(p, obj);
						}
					}
					else
					{
						FormatDecimal(p, obj);
					}
				}
				else
				{
					if (property_type.IsClass)
					{
						IteratePropertiesRecursively(p.GetValue(obj));
					}
				}
			}
		}

		private static void FormatDecimal(PropertyInfo property_info, object obj)
		{
			var value = (decimal)property_info.GetValue(obj, null);
			var formattedString = value.ToString(Decimal_Formatter, CultureInfo.InvariantCulture);
			property_info.SetValue(obj, decimal.Parse(formattedString), null);
		}
	}
}
