using System;
namespace XmlSearchReader
{
    public class Constant
    {
/*        public static Constant InvalidMessageForArg= $"Error: Invalid input. Program usage is as below.{Environment.NewLine}[DeviceUtil.exe] [XML file path]{Environment.NewLine}DeviceUtil.exe : Name of the executable file{Environment.NewLine}If anyone changes the name of the EXE, then the executable file name in usage should change accordingly.{Environment.NewLine}Terminate program.\";*/
        public static string MessageForMenu()
        {
            return $"\nPlease select an option:{Environment.NewLine}[1] Show all devices{Environment.NewLine}[2] Search devices by serial number{Environment.NewLine}[3] Exit";
        }
    }
}
