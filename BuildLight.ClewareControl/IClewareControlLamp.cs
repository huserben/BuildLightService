using System;
using BuildLight.Controller.Interfaces;

namespace BuildLight.ClewareControl
{
   public interface IClewareControlLamp : IBuildLamp, IDisposable
   {
      /// <summary>
      /// Prints all the available devices found.
      /// </summary>
      void DiscoverConnectedDevices();

      void AddDeviceToBuildMapping(int deviceNumber, string buildName);
   }
}
