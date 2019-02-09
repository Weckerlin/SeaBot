namespace SeaBotCore.Utils
{
    using System;
    using System.IO;
    using System.Net;
    using System.Runtime.Serialization.Formatters.Binary;

    public static class Cookies
    {
        private static string sentryfile = "sentry.dat";
        public static void WriteCookiesToDisk(CookieContainer cookieJar)
        {
            using(Stream stream = File.Create(sentryfile))
            {
                try {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, cookieJar);
                } catch(Exception e) { 
                }
            }
        }   

        public static CookieContainer ReadCookiesFromDisk()
        {

            try {
                using(Stream stream = File.Open(sentryfile, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();;
                    return (CookieContainer)formatter.Deserialize(stream);
                }
            } catch(Exception) { 
                return new CookieContainer(); 
            }
        }
    }
}
