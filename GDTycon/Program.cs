using System;
using GDTycon.Game.Engine;

namespace GDTycon
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //GameCore myGame = new GameCore();

            /*Menu dayMenu = new Menu("Programuj", "Zobacz dostępne projekty", "Zobacz moje projekty", "Szukaj klientów/projektów", "Testuj kody", "Oddaj projekt", "Zarządzaj pracownikami", "Rozlicz urzędy", "Wyjdź z programu");
            Menu testMenu = new Menu("Testuj swój kod", "Testuj kod pracowników", "testuj kod współpracowników", "Wróć do menu");
            Menu hireMenu = new Menu("Zatrudnij pracownika", "Zwolnij pracownika", "Zainwestuj w reklamy by szukać pracowników (300.00)", "Wróć do menu");
            Menu projMenu = new Menu("Wybierz projekt", "Wróć do menu");
*/
            DateTime startDate = DateTime.Parse("2020, 1, 29");
            Console.WriteLine(startDate + "Czas obecny \n i czas po 50 dniach ");
            startDate=startDate.AddDays(20);
            Console.WriteLine(startDate);
        }
    }
}