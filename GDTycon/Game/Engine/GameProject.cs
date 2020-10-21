using GDTycon.Game.NPC;
using System;
using System.Collections.Generic;

namespace GDTycon.Game.Engine
{
    internal class GameProject
    {
        public string projectName;

        public GameEnum.ProjectComplexity complexity;
        private static string[] technology = { "front-end", "backend", "baza danych", "mobile", "wordpress", "prestashop" };

        //słowniik z c# zamiast mapy z Java
        // zmiana elementu w słowniku -> słownik["klucz"] = "nowa wartość";
        public Dictionary<string, int> daysForTechnology = new Dictionary<string, int>();

        //użycie metody ToString dla daty by ją zformatować do pożądanego wyglądu dla gry
        //Console.WriteLine(startDate.ToString("D"));

        public void SetOwner(Client owner)
        {
            this.owner = owner;
        }

        public Client owner;
        public DateTime deadLine;
        public Double penalty;
        public Double reward;
        public int timeOfReward;
        public bool ready;
        public int coderError = 0;

        public GameProject(String projectName, GameEnum.ProjectComplexity complexity, DateTime deadLine, Double penalty, Double reward, int timeOfReward, int[] techDuration)
        {
            this.projectName = projectName;
            this.complexity = complexity;
            this.deadLine = deadLine;
            this.penalty = penalty;
            this.reward = reward;
            this.timeOfReward = timeOfReward;
            for (int i = 0; i < technology.Length; i++)
            {
                daysForTechnology.Add(technology[i], techDuration[i]);
            }
        }

        public void IsReady()
        {
            foreach (var item in daysForTechnology)
            {
                if (item.Value > 0)
                {
                    ready = false;
                    break;
                }
                else ready = true;
            }
        }

        public override string ToString()
        {
            return "Projekt " + projectName + " o złożoności  " + complexity + ". Właściciel " + owner.firstName + " " + owner.lastName + "\n" +
                    "Termin wykonania: " + deadLine.ToString("D") + " " + " Stan gotowości: " + ready + "\n" +
                    "Technologie i czas " + daysForTechnology + "\n" +
                    "Spodziewana nagroda " + reward + " po " + timeOfReward + " dniach roboczych od ukończenia. Kara: " + penalty + "\n";
        }
    }
}