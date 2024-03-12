using System;
using System.Collections.Generic;

namespace XmlSearchReader
{
    public class Program
    {
        static void Main(string[] args)
        {
            string xmlFilePath = XMlValidation.ValidateAndGetXmlFilePath(args);
            if (xmlFilePath == null)
            {
                Console.WriteLine($"Error: Invalid input. Program usage is as below.{Environment.NewLine}[DeviceUtil.exe] [XML file path]{Environment.NewLine}DeviceUtil.exe : Name of the executable file{Environment.NewLine}If anyone changes the name of the EXE, then the executable file name in usage should change accordingly.{Environment.NewLine}Terminate program.");
            }

            bool CheckFileExistence = XMlValidation.CheckFileExistence(xmlFilePath);
            
            if (CheckFileExistence == false)
            {
                Console.WriteLine("Error: File does not exist. Please provide a valid file path.{Environment.NewLine}Terminate program.");
            }
            
            bool filexmlFileValidation = XMlValidation.ValidateXmlFileExtension(xmlFilePath);
            
            if (filexmlFileValidation == false)
            {
                Console.WriteLine($"Error: Given file is not an XML file. The file extension is wrong.{Environment.NewLine}Terminate program.");
            }
            string errorMessage = XMlValidation.ErrorForXmlException(xmlFilePath, out Dictionary<string, Device> devicesDictionary);

            if (string.IsNullOrEmpty(errorMessage) == false)
            {
                Console.WriteLine(errorMessage);
            }
            Console.ReadKey();

           Console.WriteLine(Constant.MessageForMenu());

            while (true)
            {
                string choice = Console.ReadLine().Trim();
                switch (choice)
                {
                    case "1":
                        showAndSearch.ShowDevices(devicesDictionary);
                        break;
                    case "2":
                        Console.Write("Enter serial number of the device: ");
                        string serialNumber = Console.ReadLine().Trim();
                        showAndSearch.SearchDevice(devicesDictionary, serialNumber);
                        break;
                    case "3":
                        Console.WriteLine("Program terminated.");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Error: Invalid input. Please choose from the above options.");
                        break;
                }
            }


            Console.ReadKey();
        }
    }
}
