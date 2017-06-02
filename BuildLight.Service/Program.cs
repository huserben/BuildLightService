using System;
using BuildLight.ClewareControl;
using BuildLight.Controller;
using BuildLight.Source.Tfs;

namespace BuildLight.Service
{
   class Program
   {
      static void Main(string[] args)
      {
         var tfsBuildSource = new TfsBuildSource(
              "https://benjsawesometfstest.visualstudio.com/DefaultCollection", "Build Test", "PAT", "sww3otrtvfaqi4sqcqqjceq23lxgvlyjfoftqox7272qc3vxyi2q");

         using (var buildLight = new ClewareControlBuildLamp())
         {
            //buildLight.DiscoverConnectedDevices();
            buildLight.AddDeviceToBuildMapping(1011, "CI Test");

            var buildLightController = new BuildLightController(buildLight, tfsBuildSource, "CI Test");
            buildLightController.Start(30000);

            Console.ReadKey();
         }
      }
   }
}
