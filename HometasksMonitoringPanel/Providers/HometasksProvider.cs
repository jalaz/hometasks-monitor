using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HometasksMonitoringPanel.Providers
{
    public class HometasksProvider : IHometasksProvider
    {
        public string[] GetHometaskTitles()
        {
            return new[]
            {
                "Домашка #0",
                "Домашка #1",
                "Домашка #2",
                "Домашка №3 (ООП)",
                "Домашка №4 (Battleship)",
                "Домашка №5 (LINQ)"
            };
        }
    }

    public interface IHometasksProvider
    {
        string[] GetHometaskTitles();
    }
}