namespace BuildLight.Controller.Interfaces
{
   public interface IBuildSource
   {
      BuildStatus GetBuildStatus(string buildName);
   }
}
