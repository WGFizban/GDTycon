using GDTycon.Game.Engine;
using System;

namespace GDTycon.Game.NPC
{
    internal abstract class Employee
    {
        private String name;
        private string surname;
        private double salary;
        private double employmentCost;
        private double costOfDismissal;
        public GameEnum.Occupation mainOccupation;

        public Employee(string name, string surname, double salary, double employmentCost, double costOfDismissal)
        {
            this.name = name;
            this.surname = surname;
            this.salary = salary;
            this.employmentCost = employmentCost;
            this.costOfDismissal = costOfDismissal;
        }

        public void Hire(Player player)
        {
            if (player.getCash() < employmentCost) Console.WriteLine("\nMasz za mało pieniędzy by zatrudnić tą osobe!");
            else
            {
                Console.WriteLine(this.mainOccupation + " " + this.name + " " + this.surname + " został pomyślnie zatrudniony.");
                player.myEmployee.Add(this);
                player.setCash(player.getCash() - employmentCost);
            }
        }

        public void Dismiss(Player player)
        {
            Console.WriteLine("Zwolniłeś pracownika " + this.name + " " + this.surname + " pracującego jako " + this.mainOccupation);
            player.myEmployee.Remove(this);
            player.setCash(player.getCash() - costOfDismissal);
        }

        public void GetSalaryFromPlayer(Player player)
        {
            if (player.getCash() < salary)
            {
                Console.WriteLine("Nie masz wystarczającej ilości gotówki by zapłacić pracownikowi " + this.name + " " + this.surname + " Twój " + this.mainOccupation + " odchodzi z Twojej firmy :( ");
                player.myEmployee.Remove(this);
            }
            else
            {
                Console.WriteLine("Wypłacono pensje miesięczna dla pracownika " + this.mainOccupation + " " + this.name + " " + this.surname);
                player.setCash(player.getCash() - salary);
            }
        }

        abstract public bool DoYourWorkForPlayer(Player player);

        public override string ToString()
        {
            return this.mainOccupation + " " + this.name + " " + this.surname + "\n" +
                        "Koszt zatrudnienia: " + this.employmentCost + " Pensja: " + this.salary + " Koszt przy zwolnieniu: " + this.costOfDismissal + "\n";
        }
    }
}