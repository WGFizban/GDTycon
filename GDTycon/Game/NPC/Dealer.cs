using GDTycon.Game.Engine;
using System;

namespace GDTycon.Game.NPC
{
    internal class Dealer : Employee, ISearchingProjectInterface
    {
        private int searchingDayes = 0;

        public Dealer(string name, string surname, double salary, double employmentCost, double costOfDismissal) : base(name, surname, salary, employmentCost, costOfDismissal)
        {
            mainOccupation = GameEnum.Occupation.Sprzedawca;
        }

        public override bool DoYourWorkForPlayer(Player player)
        {
            return SearchProject();
        }

        public bool SearchProject()
        {
            if (searchingDayes < GameEnum.MIN_SEARCHING_DAYS)
            {
                searchingDayes++;
                Console.WriteLine("Sprzedawca szuka klientów dla Ciebie");
                if (searchingDayes == GameEnum.MIN_SEARCHING_DAYS)
                {
                    searchingDayes = 0;
                    return true;
                }
                else return false;
            }
            return false;
        }
    }
}