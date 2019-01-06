using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SeaBotGUI.TelegramBot
{
  public  static class TeleUtils
  {
      private static string _macaddr;
   

      public static string MacAdressCode
      {
          get
          {
              if (_macaddr == "")
              {
                  _macaddr = GetDefMac();
              }

              return _macaddr;
          }
         
      }

      public static string GetDefMac()
        {
            var computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            var nics = NetworkInterface.GetAllNetworkInterfaces();
            Console.WriteLine("Interface information for {0}.{1}     ",
                computerProperties.HostName, computerProperties.DomainName);

            if (nics == null || nics.Length < 1)
            {
                Console.WriteLine("  No network interfaces found.");
                return "DEFCODE";
            }

            foreach (var adapter in nics)
            {
                var address = adapter.GetPhysicalAddress();
                var bytes = address.GetAddressBytes();
                StringBuilder addr = new StringBuilder();
                for (var i = 0; i < bytes.Length; i++)
                {
                    // Display the physical address in hexadecimal.
                    addr.Append(bytes[i].ToString("X2"));
                    // Insert a hyphen after each byte, unless we are at the end of the
                }

                if (addr.ToString() == "")
                {
                }
                else
                {
                    return addr.ToString();
                }
            }

            return "DEFCODE";
        }
    }
}
