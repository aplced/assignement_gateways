using Gateways.Models;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
 
namespace Gateways.Converters
{
  public class DateTimeConverter : JsonConverter<DateTime>
  {
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
      return DateTime.Parse(reader.GetString());
    }
 
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
      writer.WriteStringValue(value.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:ss"));
    }
  }

  public class DeviceStatusConverter: JsonConverter<Device.DeviceStatus>
  {
    public override Device.DeviceStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
      return (Device.DeviceStatus)Enum.Parse(typeof(Device.DeviceStatus), reader.GetString());
    }
 
    public override void Write(Utf8JsonWriter writer, Device.DeviceStatus value, JsonSerializerOptions options)
    {
      writer.WriteStringValue(value.ToString());
    }
  }
}