using System;

namespace GDTycon.Game.Engine
{
    public class Menu
    {
        private readonly string[] menuOptions;

        public Menu(params string[] menuOptions)
        {
            this.menuOptions = menuOptions;
        }

        public void ShowOnly()
        {
            Console.WriteLine();
            for (int i = 0; i < menuOptions.Length; i++)
            {
                Console.WriteLine(i + " " + menuOptions[i]);
            }
        }

        public int SelectOptions()
        {
            ShowOnly();
            Console.Write("Którą opcję wybierasz? Opcja: ");

            int choice = CheckChoose();
            while (choice >= menuOptions.Length || choice < 0)
            {
                Console.Write("Nie ma takiej opcji. Podaj poprawną opcje. Opcja: ");
                choice = CheckChoose();
            }
            return choice;
        }

        //możliwość tworzenia szybkiego menu
        public int SelectOptions(int optionsLimit, string message)
        {
            Console.WriteLine("\n" + message + " ");

            int choice = CheckChoose();
            while (choice >= optionsLimit || choice < 0)
            {
                Console.WriteLine("Niedozwolony numer. Podaj poprawny: ");
                choice = CheckChoose();
            }
            return choice;
        }

        //zapobieganie wprowadzeniu znaków innych niż potrzebna liczba
        private static int CheckChoose()
        {
            try
            {
                return Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.Write("Wykryto niedozwolone znaki. Musisz podać liczbę! Opcja: ");
                return CheckChoose();
            }
        }
    }
}