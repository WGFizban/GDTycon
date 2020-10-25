using GDTycon.Game.NPC;
using System;
using System.Collections.Generic;

namespace GDTycon.Game.Engine
{
    internal class GameProject
    {
        public string projectName;

        public GameEnum.ProjectComplexity complexity;

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
            //technologie są niezmienne i przechowywane razem z enumami w klasie GameEnum
            for (int i = 0; i < GameEnum.technology.Length; i++)
            {
                daysForTechnology.Add(GameEnum.technology[i], techDuration[i]);
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

        private string TranslateReady()
        {
            return (ready) ? "Gotowy" : "Nie gotowy";
        }

        private string DecodeDayForTechnology()
        {
            string afterDecoding = "";
            foreach (var dfr in daysForTechnology)
            {
                afterDecoding += $"        {dfr.Key}: {dfr.Value}\n";
            }
            return afterDecoding;
        }

        public override string ToString()
        {
            return $"Projekt {projectName} o złożonośći {complexity}. Właściciel { owner.firstName} { owner.lastName} \n" + $"Termin Wykonania: {deadLine:D}. Stan gotowości: { TranslateReady()} \n" + $"Technologie i czas:\n{DecodeDayForTechnology()}" + $"Spodziewana nagroda {reward} po {timeOfReward} dniach roboczych od ukończenia. Kara: {penalty} \n" + $"";
        }

        public string ToString(DateTime currentDate)
        {
            return $"Projekt {projectName} o złożonośći {complexity}. Właściciel { owner.firstName} { owner.lastName} \n" + $"Termin Wykonania: {deadLine:D} (pozostało {deadLine.DayOfYear - currentDate.DayOfYear} dni). Stan gotowości: { TranslateReady()} \n" + $"Technologie i czas:\n{DecodeDayForTechnology()}" + $"Spodziewana nagroda {reward} po {timeOfReward} dniach roboczych od ukończenia. Kara: {penalty} \n" + $"";
        }
    }
}