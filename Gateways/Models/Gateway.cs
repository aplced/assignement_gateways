using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Gateways.Models
{
    public class Gateway
    {
        [Key]
        public string SerialNumber { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [CustomValidation(typeof(Gateway), "ValidateIPv4")]
        public string IPv4 { get; set; }
        [MaxLength(10)]
        public ICollection<Device> Devices { get; set; }

        public static ValidationResult ValidateIPv4(string x, ValidationContext context)
        {
            ValidationResult validationResult;

            try
            {
                validationResult = IPAddress.TryParse(x, out IPAddress addr) ? ValidationResult.Success : new ValidationResult($"{x} is not a valid IPv4 address");
            }
            catch(ArgumentNullException ex)
            {
                validationResult = new ValidationResult(ex.Message);
            }
            return validationResult;
        }
    }
}