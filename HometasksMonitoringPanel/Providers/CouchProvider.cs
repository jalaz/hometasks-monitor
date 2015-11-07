using System.Collections.Generic;

namespace HometasksMonitoringPanel.Providers
{
    public interface ICouchProvider
    {
        string[] GetAll();
    }

    public class CouchProvider : ICouchProvider
    {
        public string[] GetAll()
        {
            return new[]
            {
                "kottans-net",
                "jalaz",
                "Zaknafeyn",
                "hnatiukdm",
                "programulya"
            };
        }
    }
}