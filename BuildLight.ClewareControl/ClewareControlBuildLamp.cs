using System;
using System.Collections.Generic;
using BuildLight.Controller.Interfaces;

namespace BuildLight.ClewareControl
{
   public class ClewareControlBuildLamp : IClewareControlLamp
   {
      private readonly int deviceCount;
      private readonly IntPtr clewareObject;

      private readonly Dictionary<string, int> deviceBuildMapping = new Dictionary<string, int>();

      public ClewareControlBuildLamp()
      {
         // Please note that OpenCleware should be called only once in the initialisation of your programm, 
         // not every time a cleware function is called
         clewareObject = LampAccess.FCWInitObject();
         deviceCount = LampAccess.FCWOpenCleware(clewareObject);
      }

      public void AddDeviceToBuildMapping(int deviceNumber, string buildName)
      {
         deviceBuildMapping.Add(buildName, deviceNumber);
      }

      public void DiscoverConnectedDevices()
      {
         for (var i = 0; i < deviceCount; i++)
         {
            var devType = LampAccess.FCWGetUSBType(clewareObject, i);
            Console.WriteLine("Device " + i + ": Type = " + devType);
            if (devType == (int)LampAccess.USBtype_enum.TEMPERATURE_DEVICE)
            {
               var temperature = LampAccess.FCWDGetTemperature(clewareObject, i);
               Console.WriteLine("Temperature at " + i + " is = " + temperature + "°C");
            }
            if (devType == (int)LampAccess.USBtype_enum.SWITCH1_DEVICE)
            {
               var state = LampAccess.FCWGetSwitch(clewareObject, i, (int)LampAccess.SWITCH_IDs.SWITCH_0);
               Console.WriteLine("Switch state is = " + state);
               LampAccess.FCWSetSwitch(clewareObject, i, (int)LampAccess.SWITCH_IDs.SWITCH_0, 1);
               System.Threading.Thread.Sleep(1000);     // wait one second
               LampAccess.FCWSetSwitch(clewareObject, i, (int)LampAccess.SWITCH_IDs.SWITCH_0, 0);
            }
         }
      }

      public void Dispose()
      {
         if (deviceCount > 0)
         {
            LampAccess.FCWCloseCleware(clewareObject);
         }

         LampAccess.FCWUnInitObject(clewareObject);   // free the allocated object
      }

      public void UpdateStatus(string buildName, BuildStatus status)
      {
         var deviceNumber = deviceBuildMapping[buildName];

         TurnOffLights(deviceNumber);

         switch (status)
         {
            case BuildStatus.Successful:
               SwitchLight(deviceNumber, 1, LampAccess.SWITCH_IDs.SWITCH_0);
               break;
            case BuildStatus.PartiallySuccessful:
               SwitchLight(deviceNumber, 1, LampAccess.SWITCH_IDs.SWITCH_1);
               break;
            case BuildStatus.Failed:
               SwitchLight(deviceNumber, 1, LampAccess.SWITCH_IDs.SWITCH_2);
               break;
            case BuildStatus.Inconclusive:
               TurnOnAllLights(deviceNumber);
               TurnOffLights(deviceNumber);
               break;
         }
      }

      private void TurnOffLights(int deviceNr)
      {
         SwitchAllLights(deviceNr, 0);
      }

      private void TurnOnAllLights(int deviceNr)
      {
         SwitchAllLights(deviceNr, 1);
      }

      private void SwitchAllLights(int deviceNr, int state)
      {
         SwitchLight(deviceNr, state, LampAccess.SWITCH_IDs.SWITCH_0);
         System.Threading.Thread.Sleep(500);
         SwitchLight(deviceNr, state, LampAccess.SWITCH_IDs.SWITCH_1);
         System.Threading.Thread.Sleep(500);
         SwitchLight(deviceNr, state, LampAccess.SWITCH_IDs.SWITCH_2);
      }

      private void SwitchLight(int deviceNr, int state, LampAccess.SWITCH_IDs switchID)
      {
         LampAccess.FCWSetSwitch(clewareObject, deviceNr, (int)switchID, state);
      }
   }
}
