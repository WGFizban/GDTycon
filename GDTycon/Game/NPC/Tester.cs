using GDTycon.Game.Engine;
using System;

namespace GDTycon.Game.NPC
{
    internal class Tester : Employee
    {
        public Tester(string name, string surname, double salary, double employmentCost, double costOfDismissal) : base(name, surname, salary, employmentCost, costOfDismissal)
        {
            mainOccupation = GameEnum.Occupation.Tester;
        }

        public override bool DoYourWorkForPlayer(Player player)
        {
            if ((CountOfTesters(player) * 3) <= CountOfProgrammers(player) && player.myProjects.Count > 0)
            {
                foreach (var project in player.myProjects)
                {
                    project.coderError = 0;
                }
                Console.WriteLine("Tester poprawił wszystkie Twoje projekty");
                return true;
            }
            return false;
        }

        private int CountOfProgrammers(Player player)
        {
            int programmersCount = 0;
            foreach (var employee in player.myEmployee)
            {
                if (employee.mainOccupation.Equals(GameEnum.Occupation.Programista))
                {
                    programmersCount++;
                }
            }
            return programmersCount + 1;
        }

        private int CountOfTesters(Player player)
        {
            int testersCount = 0;
            foreach (var employee in player.myEmployee)
            {
                if (employee.mainOccupation.Equals(GameEnum.Occupation.Tester))
                {
                    testersCount++;
                }
            }
            return testersCount;
        }
    }
}