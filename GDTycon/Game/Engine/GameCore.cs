using System;

namespace GDTycon.Game.Engine
{
    public class GameCore
    {
        // data rozpoczęcia gry i ilość dostępnych zasobów
        private static DateTime startDate = DateTime.Parse("2020, 1, 29");

        private const int startAvailableProject = 3;
        private const int startAvailableTesters = 1;
        private const int startAvailableDealer = 1;
        private const int startAvailableProgrammers = 2;

        //Pamięć dnia, aktualnego gracza, i stanu dla gry
        private DateTime currentDay;

        private Player playerOnTurn;
        private bool gameIsOn;

        //Przygotowanie menu pojawiających się w grze
        private Menu dayMenu = new Menu("Programuj", "Zobacz dostępne projekty", "Zobacz moje projekty", "Szukaj klientów/projektów", "Testuj kody", "Oddaj projekt", "Zarządzaj pracownikami", "Rozlicz urzędy", "Wyjdź z programu");

        private Menu testMenu = new Menu("Testuj swój kod", "Testuj kod pracowników", "testuj kod współpracowników", "Wróć do menu");
        private Menu hireMenu = new Menu("Zatrudnij pracownika", "Zwolnij pracownika", "Zainwestuj w reklamy by szukać pracowników (300.00)", "Wróć do menu");
        private Menu projMenu = new Menu("Wybierz projekt", "Wróć do menu");

        public GameCore()
        {
        }
    }
}