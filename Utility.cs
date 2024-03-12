using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace XmlSearchReader
{
    public class Utility
    {
        /// <summary>
        /// Checks if the input string contains any invalid characters.
        /// </summary>
        /// <param name="input">The string to be checked for invalid characters.</param>
        /// <returns>True if the input contains invalid characters, otherwise false.</returns>
        static bool ContainsInvalidCharacters(string input)
        {
            // Define the set of valid characters
            HashSet<char> validChars = new HashSet<char>("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.");
            // Check if the input contains any character that is not in the valid set
            foreach (char c in input)
            {
                if (!validChars.Contains(c))
                {
                    return true; // Invalid character found
                }
            }
            return false; // No invalid characters found
        }

        /// <summary>
        /// Checks if the entire string consists of valid alphanumeric characters.
        /// </summary>
        /// <param name="input">The string to be checked for invalid characters.</param>
        /// <returns>True if the input consists of valid characters, otherwise false.</returns>
        static bool InvalideCharacter(string input)
        {
            var pattern = @"^[a-zA-Z0-9]";
            return Regex.IsMatch(input, pattern);

        }

        /// <summary>
        /// Validates the format of a port number.
        /// </summary>
        /// <param name="input">The string representing the port number to be validated.</param>
        /// <returns>True if the port number format is valid, otherwise false.</returns>
        static bool ValidatePortNumberFormat(string input)
        {
            // Port number format: digits (0-9)
            return System.Text.RegularExpressions.Regex.IsMatch(input, "^[0-9]*$");
        }

        /// <summary>
        /// Validates the format of a device type.
        /// </summary>
        /// <param name="input">The string representing the device type to be validated.</param>
        /// <returns>True if the device type format is valid (A3 or A4), otherwise false.</returns>
        static bool ValidateTypeFormat(string input)
        {
            // Type format: A3 or A4
            return (input == "A3" || input == "A4");
        }

        // <summary>
        /// Validates the format of a device address.
        /// </summary>
        /// <param name="input">The string representing the device address to be validated.</param>
        /// <returns>
        /// True if the device address format is valid (alphanumeric characters and dots),
        /// and the address is not empty; otherwise, false.
        /// </returns>
        static bool ValidateAddressFormat(string input)
        {
            // Address format: alphanumeric (A-Z, 0-9)
            return !string.IsNullOrEmpty(input) && System.Text.RegularExpressions.Regex.IsMatch(input, "^[A-Za-z0-9.]*$");
        }

        /// <summary>
        /// Validates the format of the Use SSL setting.
        /// </summary>
        /// <param name="input">The string representing the Use SSL setting to be validated.</param>
        /// <returns>
        /// True if the Use SSL setting format is valid (either "true" or "false"),
        /// and the input is not empty; otherwise, false.
        /// </returns>
        static bool ValidateUseSSLFormat(string input)
        {
            // Use SSL format: true or false
            return !string.IsNullOrEmpty(input) && (input.ToLower() == "true" || input.ToLower() == "false");
        }

        /// <summary>
        /// Checks whether the input string is empty.
        /// </summary>
        /// <param name="input">The string to be checked for emptiness.</param>
        /// <returns>True if the input string is empty; otherwise, false.</returns>
        static bool IsEmpty(string input)
        {
            if (input.Length == 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks whether the length of the input string is invalid.
        /// </summary>
        /// <param name="input">The string to be checked for length.</param>
        /// <param name="maxLength">The maximum allowed length.</param>
        /// <returns>True if the length is valid; otherwise, false.</returns>
        static bool InvalideLength(string input, int maxLength)
        {
            if (input.Length > maxLength)
                return false;
            return true;
        }

        static bool IsValidDevice(Device device)
        {
            if (string.IsNullOrWhiteSpace(device.SrNo) ||
                string.IsNullOrWhiteSpace(device.Address) ||
                string.IsNullOrWhiteSpace(device.DevName) ||
                string.IsNullOrWhiteSpace(device.Type) ||
                device.CommSetting == null
              || string.IsNullOrWhiteSpace(device.CommSetting.Password))
            {
                return false;
            }
            if ((!InvalideLength(device.SrNo, 16)))
            {
                return false;
            }
            if ((!InvalideLength(device.Address, 15)))
            {
                return false;
            }
            if ((!InvalideLength(device.DevName, 24)))
            {
                return false;
            }

            if ((!InvalideLength(device.ModelName, 24)))
            {
                return false;
            }

            if ((!InvalideLength(device.CommSetting.Password, 64)))
            {
                return false;
            }
            if (!IsEmpty(device.CommSetting.Password))
            {
                return false;
            }
            if (!ValidateTypeFormat(device.Type))
            {
                return false;
            }
            if (!ValidatePortNumberFormat(device.CommSetting.PortNo))
            {
                return false;
            }
            if (!InvalideCharacter(device.CommSetting.Password))
            {
                return false;
            }
            if (!InvalideCharacter(device.ModelName))
            {
                return false;
            }
            if (!InvalideCharacter(device.DevName))
            {
                return false;
            }
            if (!InvalideCharacter(device.Address))
            {
                return false;
            }
            if (!InvalideCharacter(device.SrNo))
            {
                return false;
            }
            return true;
        }
    }
}
