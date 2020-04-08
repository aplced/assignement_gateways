using System;
using System.ComponentModel.DataAnnotations;

namespace Gateways.Models
{
    public class Device
    {
        public enum DeviceStatus
        {
            Offline,
            Online
        }

        [Key]
        public long UUID { get; set; }
        public string Vendor { get; set; }
        public DateTime Created { get; set; }
        public DeviceStatus Status { get; set; }
    }
}