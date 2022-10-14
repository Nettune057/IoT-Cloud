using System;
using Microsoft.Azure.Devices;
using Newtonsoft.Json;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static RegistryManager registryManager;
        static string connectionString = "HostName=nha-kinh-iot-hub.azure-devices.net;SharedAccessKeyName=registryWriteAndServiceConnect;SharedAccessKey=LBEhfGJJ7SxMmCK09qmGUm7cw5LezgK8nM5Pq7uXgSQ=";
        public class DeviceTwinProperties
        {
            public DeviceTwinDesired properties { get; set; }
        }
        public class DeviceTwinDesired
        {
            public DesiredData desired { get; set; }
        }
        public static async Task AddTagsAndQuery(string patch)
        {
            var twin = await registryManager.GetTwinAsync("demorasberry");
            await registryManager.UpdateTwinAsync(twin.DeviceId, patch, twin.ETag);
        }

        static void Main(string[] args)
        {    
            if (args.Length > 0)
            {
                DesiredData desiredData = new DesiredData();
                DeviceTwinProperties prop = new DeviceTwinProperties();
                DeviceTwinDesired deviceTwinDesired = new DeviceTwinDesired();
                deviceTwinDesired.desired = desiredData;
                prop.properties = deviceTwinDesired;
                desiredData.DeviceId = (args[0] != null) ? args[0] : "";
                desiredData.Temperature = (args[1] != null) ? Convert.ToInt32(args[1]) : null;
                desiredData.MinCO2 = (args[2] != null) ? Convert.ToInt32(args[2]) : null;
                desiredData.MaxCO2 = (args[3] != null) ? Convert.ToInt32(args[3]) : null;
                desiredData.MinHumidity = (args[4] != null) ? Convert.ToInt32(args[4]) : null;
                desiredData.MaxHumidity = (args[5] != null) ? Convert.ToInt32(args[5]) : null;
                desiredData.LedState = (args[6] != null) ? Convert.ToInt32(args[6]) : 0;
                desiredData.Disable = (args[7] != null) ? Convert.ToInt32(args[7]) : 0;
                Console.WriteLine(desiredData.DeviceId);
                Console.WriteLine(desiredData.Temperature);
                string desiredJson = JsonConvert.SerializeObject(prop);
                Console.WriteLine("Json serialize: " + desiredJson);

                registryManager = RegistryManager.CreateFromConnectionString(connectionString);
                AddTagsAndQuery(desiredJson).Wait();
                Console.WriteLine("Update desired data complete!");
                
            }

            Console.WriteLine("\nPress any key to exit");
            Console.ReadLine();
        }
    }
}