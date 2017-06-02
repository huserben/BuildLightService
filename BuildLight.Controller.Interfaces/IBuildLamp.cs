namespace BuildLight.Controller.Interfaces
{
   public interface IBuildLamp
   {
      void UpdateStatus(string buildName, BuildStatus status);
   }
}
