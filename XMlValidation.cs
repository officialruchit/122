using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace XmlSearchReader
{
    public class XMlValidation
    {
        public static string ValidateAndGetXmlFilePath(string[] args)
        {
            if (args.Length == 1)
            {
                return args[0];
            }
            else
            {
                return null; // or throw an exception based on your preference
            }
        }
        public static bool CheckFileExistence(string filePath)
        {
            if (File.Exists(filePath) == false)
            {
                return false;
            }
            return true;
        }
        public static bool ValidateXmlFileExtension(string xmlFilePath)
        {
            if (Path.GetExtension(xmlFilePath).ToLower() != ".xml")
            {
                return false;
            }
            return true;
        }
        public static string ErrorForXmlException(string xmlFilePath, out Dictionary<string, Device> devicesDictionary)
        {
            devicesDictionary = null;

            try
            {
                devicesDictionary = ParseXml(xmlFilePath);
                if (devicesDictionary.Count == 0)
                {
                    return "Error: No devices found in the XML file.\nTerminate program.";
                }

                return null; // No error
            }
            catch (XmlException ex)
            {
                return $"Error: Invalid XML format. {ex.Message}\nPlease check the XML file and fix the formatting issues.\nTerminate program.";
            }
            catch (Exception ex)
            {
                return $"Error: An unexpected error occurred while parsing XML. {ex.Message}\nTerminate program.";
            }
        }
        /*public static Dictionary<string, Device> ParseXml(string filePath)
        {
            // Initialize XML serializer for DeviceList type
            XmlSerializer serializer = new XmlSerializer(typeof(DeviceList));

            // Read XML file and deserialize its content into a DeviceList object
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                DeviceList deviceList = (DeviceList)serializer.Deserialize(fileStream);
                Dictionary<string, Device> devicesDictionary = new Dictionary<string, Device>();

                         // Validate and process each device in the list
                int deviceIndex = 1;
                foreach (var device in deviceList.Devices)
                {
                    *//*  ValidateAndProcessDevice(device, devicesDictionary, deviceIndex);*//*
                    deviceIndex++;
                }

                // Return the dictionary containing valid devices
                return devicesDictionary;
            }
        }*/




        static Dictionary<string, Device> ParseXml(string filePath)
        {
            // Initialize XML serializer for DeviceList type
            XmlSerializer serializer = new XmlSerializer(typeof(DeviceList));

            // Read XML file and deserialize its content into a DeviceList object
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                DeviceList deviceList = (DeviceList)serializer.Deserialize(fileStream);
                Dictionary<string, Device> devicesDictionary = new Dictionary<string, Device>();

                // Validate and process each device in the list
                int deviceIndex = 1;
                /* foreach (var device in deviceList.Devices)
                 {
                     // Check if the device is valid
                     if (!IsValidDevice(device))
                     {
                         // Output error details for invalid devices
                         Console.WriteLine("Error: Invalid device information. Please refer below details.");
                         Console.WriteLine($"Device index: {deviceIndex}");

                         Console.WriteLine($"Serial Number: {(InvalideLength(device.SrNo, 16) ? "" : "(invalid length)")} {(IsEmpty(device.SrNo) ? "" : "(Empty)")} {(ContainsInvalidCharacters(device.SrNo) ? "(not supported character)" : "")}");
                         Console.WriteLine($"IP Address: {(InvalideLength(device.Address, 15) ? "" : "(invalid length)")} {(IsEmpty(device.Address) ? "" : "(Empty)")} {(ContainsInvalidCharacters(device.Address) ? "(not supported character)" : "")} {(ValidateAddressFormat(device.Address) ? "" : "(Not Supported format)")}");
                         Console.WriteLine($"Device Name:{(InvalideLength(device.DevName, 24) ? "" : "(invalid length)")} {(ContainsInvalidCharacters(device.DevName) ? "(not supported character)" : "")}");
                         Console.WriteLine($"Model Name: {(InvalideLength(device.ModelName, 24) ? "" : "(invalid length)")}  {(ContainsInvalidCharacters(device.ModelName) ? "(not supported character)" : "")}");
                         Console.WriteLine($"Type: {(IsEmpty(device.Type) ? "" : "(Empty)")} {(ValidateTypeFormat(device.Type) ? "" : "(Not Supported format)")}");
                         Console.WriteLine($"Port Number: {(IsEmpty(device.CommSetting.PortNo) ? "" : "(Empty)")} {(ValidatePortNumberFormat(device.CommSetting.PortNo) ? "" : "(Not Supported format)")}"); // Assuming port number cannot be 0 if provided
                         Console.WriteLine($"Use SSL: {(IsEmpty(device.CommSetting.UseSSL) ? "" : "(Empty)")} {(ValidateUseSSLFormat(device.CommSetting.UseSSL) ? "" : "(Not Supported format)")}"); // Assuming UseSSL is a boolean
                         Console.WriteLine($"Password: {(InvalideLength(device.CommSetting.Password, 64) ? "" : "(invalid length)")} {(IsEmpty(device.CommSetting.Password) ? "" : "(Empty)")} {(ContainsInvalidCharacters(device.CommSetting.Password) ? " (invalid Character)" : "")}");
                         Console.WriteLine();
                     }
                     else
                     {
                         // Check for duplicate SrNo or Address
                         if (devicesDictionary.ContainsKey(device.SrNo))
                         {
                             Console.WriteLine($"Error: Duplicate Serial Number detected: {device.SrNo}");
                         }
                         else if (devicesDictionary.Values.Any(d => d.Address == device.Address))
                         {
                             Console.WriteLine($"Error: Duplicate Address detected: {device.Address}");
                         }
                         else
                         {
                             // Add the device to the dictionary
                             devicesDictionary.Add(device.SrNo, device);
                         }
                     }

                     // Move to the next device
                     deviceIndex++;
                 }*/


                foreach (var device in deviceList.Devices)
                {
                    // Check if the device is valid
                    if (ValidateDevice(device))
                    {
                        // Output error details for invalid devices
                        Console.WriteLine("Error: Invalid device information. Please refer below details.");
                        Console.WriteLine($"Device index: {deviceIndex}");
                        Console.WriteLine($"Serial Number: {ValidateSrNo(device.SrNo)}");
                        Console.WriteLine($"IP Address: {ValidateAddress(device.Address)}");
                        Console.WriteLine($"Device Name: {ValidateDevName(device.DevName)}");
                        Console.WriteLine($"Model Name: {ValidateModelName(device.ModelName)}");
                        Console.WriteLine($"Type: {ValidateType(device.Type)}");
                        Console.WriteLine($"Port Number: {ValidatePortNumber(device.CommSetting.PortNo)}");
                        Console.WriteLine($"Use SSL: {ValidateUseSSL(device.CommSetting.UseSSL)}");
                        Console.WriteLine($"Password: {ValidatePassword(device.CommSetting.Password)}");
                        Console.WriteLine();
                    }
                    else
                    {
                        // Check for duplicate SrNo or Address
                        if (devicesDictionary.ContainsKey(device.SrNo))
                        {
                            Console.WriteLine($"Error: Duplicate Serial Number detected: {device.SrNo}");
                        }
                        else if (devicesDictionary.Values.Any(d => d.Address == device.Address))
                        {
                            Console.WriteLine($"Error: Duplicate Address detected: {device.Address}");
                        }
                        else
                        {
                            // Add the device to the dictionary
                            devicesDictionary.Add(device.SrNo, device);
                        }
                    }

                    // Move to the next device
                    deviceIndex++;
                }


                // Return the dictionary containing valid devices
                return devicesDictionary;
            }
        }




































        static bool ValidateSrNo(string srNo)
        {
            return InvalideLength(srNo, 16) || IsEmpty(srNo) || ContainsInvalidCharacters(srNo);
        }

        static bool ValidateAddress(string address)
        {
            return InvalideLength(address, 15) || IsEmpty(address) || ContainsInvalidCharacters(address) || !ValidateAddressFormat(address);
        }

        static bool ValidateDevName(string devName)
        {
            return InvalideLength(devName, 24) || ContainsInvalidCharacters(devName);
        }

        static bool ValidateModelName(string modelName)
        {
            return InvalideLength(modelName, 24) || ContainsInvalidCharacters(modelName);
        }

        static bool ValidateType(string type)
        {
            return IsEmpty(type) || !ValidateTypeFormat(type);
        }

      /*  static bool ValidateDevice(Device device)
        {
            return ValidateSrNo(device.SrNo) ||
                   ValidateAddress(device.Address) ||
                   ValidateDevName(device.DevName) ||
                   ValidateModelName(device.ModelName) ||
                   ValidateType(device.Type) ||
                   ValidatePortNumber(device.CommSetting.PortNo) ||
                   ValidateUseSSL(device.CommSetting.UseSSL) ||
                   ValidatePassword(device.CommSetting.Password);
        }
*/
        static bool ValidatePortNumber(string portNumber)
        {
            return IsEmpty(portNumber) || !ValidatePortNumberFormat(portNumber);
        }

        static bool ValidateUseSSL(string useSSL)
        {
            return IsEmpty(useSSL) || !ValidateUseSSLFormat(useSSL);
        }

        static bool ValidatePassword(string password)
        {
            return InvalideLength(password, 64) || IsEmpty(password) || ContainsInvalidCharacters(password);
        }

        static bool ValidateDevice(Device device)
        {
            // Implement your specific validations here based on the device properties
            // For example, you can use the functions defined above for individual validations
            // and combine the results based on your requirements.
            return ValidateSrNo(device.SrNo) ||
                   ValidateAddress(device.Address) ||
                   ValidateDevName(device.DevName) ||
                   ValidateModelName(device.ModelName) ||
                   ValidateType(device.Type) ||
                   ValidatePortNumber(device.CommSetting.PortNo) ||
                   ValidateUseSSL(device.CommSetting.UseSSL) ||
                   ValidatePassword(device.CommSetting.Password);
        }


        /*  public static Dictionary<string, Device> ParseXml(string filePath)
          {
              // Initialize XML serializer for DeviceList type
              XmlSerializer serializer = new XmlSerializer(typeof(DeviceList));
              Dictionary<string, Device> devicesDictionary = new Dictionary<string, Device>();

              try
              {
                  // Read XML file and deserialize its content into a DeviceList object
                  using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                  {
                      DeviceList deviceList = (DeviceList)serializer.Deserialize(fileStream);

                  }

                  // You can optionally perform additional post-processing here if needed

                  return devicesDictionary;
              }
              catch (Exception ex)   
              {
                  // Handle exceptions if needed
                  Console.WriteLine($"Error: An unexpected error occurred while processing XML. {ex.Message}");
                  return null;
              }
          }*/

        /* static Dictionary<string, Device> ParseXml(string filePath)
         {
             // Initialize XML serializer for DeviceList type
             XmlSerializer serializer = new XmlSerializer(typeof(DeviceList));

             // Read XML file and deserialize its content into a DeviceList object
             using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
             {
                 DeviceList deviceList = (DeviceList)serializer.Deserialize(fileStream);
                 Dictionary<string, Device> devicesDictionary = ValidateAndProcessDevices(deviceList.Devices);

                 return devicesDictionary;
             }
         }*/

        //**
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
                if (validChars.Contains(c) == false && string.IsNullOrEmpty(input))
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

        /// <summary>
        /// Checks whether a device has valid information.
        /// </summary>
        /// <param name="device">The device to be validated.</param>
        /// <returns>True if the device is valid; otherwise, false.</returns>
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

        /*static Dictionary<string, Device> ParseXml(string filePath)
        {
            // Initialize XML serializer for DeviceList type
            XmlSerializer serializer = new XmlSerializer(typeof(DeviceList));

            // Read XML file and deserialize its content into a DeviceList object
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                DeviceList deviceList = (DeviceList)serializer.Deserialize(fileStream);
                Dictionary<string, Device> devicesDictionary = ValidateAndProcessDevices(deviceList.Devices);

                return devicesDictionary;
            }
        }*/





        /*  private static Dictionary<string, Device> ValidateAndProcessDevices(List<Device> devices)
          {
              Dictionary<string, Device> devicesDictionary = new Dictionary<string, Device>();
              StringBuilder errorMessages = new StringBuilder();
              StringBuilder successfull = new StringBuilder();
              int deviceIndex = 1;

              foreach (var device in devices)
              {
                  errorMessages.AppendLine($"Device index: {deviceIndex}");

                  // Check and show error if the "SrNo" node is not present
                  if (string.IsNullOrEmpty(device.SrNo))
                  {
                      errorMessages.AppendLine($"Serial Number: (not present)");
                  }
                  else
                  {
                      successfull.AppendLine($"Serial Number: {device.SrNo}");
                  }
                  if (string.IsNullOrEmpty(device.Address))
                  {
                      errorMessages.AppendLine($"Address: (not present)");
                  }
                  else
                  {
                      successfull.AppendLine($"Address: {device.Address}");
                  }

                  if (string.IsNullOrEmpty(device.DevName))
                  {
                      errorMessages.AppendLine($"DevName: (not present)");
                  }
                  else
                  {
                      successfull.AppendLine($"DevName: {device.DevName}");
                  }

                  if (string.IsNullOrEmpty(device.ModelName))
                  {
                      errorMessages.AppendLine($"ModelName: (not present)");
                  }
                  else
                  {
                      successfull.AppendLine($"ModelName: {device.ModelName}");
                  }
                  if (string.IsNullOrEmpty(device.Type))
                  {
                      errorMessages.AppendLine($"Type: (not present)");
                  }
                  else
                  {
                      successfull.AppendLine($"Type: {device.Type}");
                  }

                  // Check and show error if the "Address" node is not present
                  if (string.IsNullOrEmpty(device.Address))
                  {
                      errorMessages.AppendLine("IP Address: (not present)");
                  }
                  else
                  {
                      successfull.AppendLine($"IP Address: {device.Address}");
                  }

                  // Repeat similar checks for other nodes

                  // If there are errors, print them and return an empty dictionary
                  if (errorMessages.Length > 1)
                  {
                      Console.WriteLine(errorMessages.ToString());
                      return new Dictionary<string, Device>();
                  }

                  // Check for duplicate SrNo or Address
                  if (devicesDictionary.ContainsKey(device.SrNo))
                  {
                      Console.WriteLine($"Error: Duplicate Serial Number detected: {device.SrNo}");
                      return new Dictionary<string, Device>();
                  }
                  else if (devicesDictionary.Values.Any(d => d.Address == device.Address))
                  {
                      Console.WriteLine($"Error: Duplicate Address detected: {device.Address}");
                      return new Dictionary<string, Device>();
                  }

                  // Add the device to the dictionary
                  devicesDictionary.Add(device.SrNo, device);

                  // Move to the next device
                  deviceIndex++;
              }

              return devicesDictionary;
          }*/








        /* public static string CheckXmlData(string filePath)
         {
             StringBuilder errorMessages = new StringBuilder();
             int deviceIndex = 1;

             try
             {
                 // Load XML document
                 XmlDocument xmlDoc = new XmlDocument();
                 xmlDoc.Load(filePath);

                 // Get all "Dev" nodes
                 XmlNodeList deviceNodes = xmlDoc.SelectNodes("/Devices/Dev");

                 foreach (XmlNode deviceNode in deviceNodes)
                 {
                     errorMessages.AppendLine($"Device index: {deviceIndex}");

                     AppendNodeValueIfEmpty(errorMessages, deviceNode, "SrNo", "Serial Number");
                     AppendNodeValueIfEmpty(errorMessages, deviceNode, "Address", "IP Address");
                     AppendNodeValueIfEmpty(errorMessages, deviceNode, "DevName", "Device Name");
                     AppendNodeValueIfEmpty(errorMessages, deviceNode, "ModelName", "Model Name");
                     AppendNodeValueIfEmpty(errorMessages, deviceNode, "Type", "Type");
                     AppendNodeValueIfEmpty(errorMessages, deviceNode, "PortNo", "PortNo");
                     AppendNodeValueIfEmpty(errorMessages, deviceNode, "UseSSL", "UseSSL");
                     AppendNodeValueIfEmpty(errorMessages, deviceNode, "Password", "Password");



                     errorMessages.AppendLine();
                     deviceIndex++;
                 }
             }
             catch (XmlException ex)
             {
                 errorMessages.AppendLine($"Error: Invalid XML format. {ex.Message}");
             }
             catch (Exception ex)
             {
                 errorMessages.AppendLine($"Error: An unexpected error occurred while processing XML. {ex.Message}");
             }

             return errorMessages.ToString();
         }

         private static void AppendNodeValueIfEmpty(StringBuilder errorMessages, XmlNode parentNode, string nodeName, string propertyName)
         {
             string nodeValue = GetNodeValue(parentNode, nodeName);
             AppendIfEmpty(errorMessages, $"{propertyName}: {nodeValue}", $"{propertyName}: (empty)");
         }

         private static string GetNodeValue(XmlNode parentNode, string nodeName)
         {
             XmlNode node = parentNode.SelectSingleNode(nodeName);
             return node != null ? node.InnerText : "(not present)";
         }

         private static void AppendIfEmpty(StringBuilder errorMessages, string propertyWithContent, string propertyEmpty)
         {
             errorMessages.AppendLine(IsEmpty(propertyWithContent) ? propertyEmpty : propertyWithContent);
         }

         static bool IsEmpty(string value)
         {
             return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
         }*/


    }
}
