using System;

namespace TestRiverWatchApi.Helpers
{
    public class LogErrror
    {
        public void LogError(string msg, string fromPage, string stackTrace, string loggedInUser, string comment)
        {
            RiverWatchEntities entities = new RiverWatchEntities();
            ErrorLog EL = new ErrorLog();
            EL.Date = DateTime.Now;
            EL.Message = msg;
            EL.Comment = comment;
            EL.StackTrace = stackTrace;
            EL.FromPage = fromPage;
            EL.LoggedInUser = loggedInUser ?? "Random acts of kindness";
            entities.ErrorLogs.Add(EL);
            int result = entities.SaveChanges();  // for debugging
        }
    }
}
