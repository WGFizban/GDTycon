using GDTycon.Game.NPC;
using System;
using System.Collections.Generic;

namespace GDTycon
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //GameCore myGame = new GameCore();

            /*Menu dayMenu = new Menu("Programuj", "Zobacz dostępne projekty", "Zobacz moje projekty", "Szukaj klientów/projektów", "Testuj kody", "Oddaj projekt", "Zarządzaj pracownikami", "Rozlicz urzędy", "Wyjdź z programu");
            Menu testMenu = new Menu("Testuj swój kod", "Testuj kod pracowników", "testuj kod współpracowników", "Wróć do menu");
            Menu hireMenu = new Menu("Zatrudnij pracownika", "Zwolnij pracownika", "Zainwestuj w reklamy by szukać pracowników (300.00)", "Wróć do menu");
            Menu projMenu = new Menu("Wybierz projekt", "Wróć do menu");
*/
            DateTime startDate = DateTime.Parse("2020, 1, 1");
            Console.WriteLine(startDate + "Czas obecny \n i czas po 20 dniach ");
            startDate = startDate.AddDays(20);

            Console.WriteLine(startDate.ToString("D"));

            Dictionary<string, string> test = new Dictionary<string, string>();
            test.Add("coś1", "coś1");
            test.Add("coś2,", "coś2");
            Console.WriteLine("słownik przed zmianą \n");
            foreach (var item in test)
            {
                Console.WriteLine("klucz {0} i wartość {1}", item.Key, item.Value);
            }
            Console.WriteLine("i po zmianie \n");
            // zmiana elementu w słowniku test["coś1"] = "nowy coś";

            foreach (var item in test)
            {
                Console.WriteLine("klucz {0} i wartość {1}", item.Key, item.Value);
            }

            Generator.NumberGenerator();

            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine(Generator.getRandomClient() + "\n");
            }

            Console.ReadKey();
        }
    }
}