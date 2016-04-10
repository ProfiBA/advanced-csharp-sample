namespace ConsoleApp.code
{
   public interface ILogger
   {
       void Print(string val);
       void PrintError(string val);
       void PrintSuccess(string val);
   }
}
