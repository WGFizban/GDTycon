using GDTycon.Game.Engine;
using System;
using System.Collections.Generic;

namespace GDTycon.Game.NPC

{
    /// <summary>
    /// klasa statyczna do automatycznego generowania obiektów w grze
    /// </summary>
    internal static class Generator
    {
        private static readonly string[] NAMES = { "Jan", "Marek", "Anna", "Krzysztof", "Karol", "Tomasz", "Paweł", "Piotr", "Wojciech", "Zofia", "Magda", "Jakub", "Alicja", "Weronika", "Kamil", "Kamila" };
        private static readonly string[] SURNAMES = { "Poważny", "Luzacki", "Wyrozumiały", "Java", "Press", "Kosa" };
        private static readonly string[] WORKER_SURNAMES = { "Dokładny", "Wymyślny", "Pospieszny", "Coder", "Pospolity", "Dron" };
        private static readonly string[] CLIENT_CHARACTER = { "LUZAK", "WYMAGAJACY", "SKRWL" };
        private static readonly string[] PROJECT_COMPLEXITY = { "EASY", "MIDDLE", "HARD" };

        private static readonly string[] PROJECT_FIRST_SEGMENT_NAME = { "Rozszerzenie ", "Nowy ", "Wydajniejszy ", "Sequel " };
        private static readonly string[] PROJECT_LAST_SEGMENT_NAME = { "Allegro", "Sklep", "Matrix", "Fallout", "Simis City", "CITY SKYLINES" };

        private static readonly double TESTER_MAIN_COST = 2000.00;
        private static readonly double PROGRAMMER_MAIN_COST = 2400.00;
        private static readonly double DEALER_MAIN_COST = 1200.00;

        //technologie są przechowywane obecnie w klasie enum GameEnum.technology
        //static final String [] TECHNOLOGY = {"front-end","backend","baza danych","mobile","wordpress","prestashop"};

        //przygotowanie klasy randomowej c# do pracy
        private static readonly Random rand = new Random();

        public static void NumberGenerator()
        {
            Console.WriteLine("100 random integers between 0 and 100:");
            for (int ctr = 0; ctr <= 100; ctr++)
                Console.Write("{0,8:N0}", rand.Next(0, 3));
            Console.WriteLine();
        }

        //Ganerator projektów
        public static GameProject GetRandomGameProject(DateTime now)
        {
            int randFirstSegmentName = rand.Next(0, PROJECT_FIRST_SEGMENT_NAME.Length);
            int randLastSegmentName = rand.Next(0, PROJECT_LAST_SEGMENT_NAME.Length);
            //int randProjectComplexity = rand.Next(0, PROJECT_COMPLEXITY.Length);

            GameEnum.ProjectComplexity randProjectComplexity = (GameEnum.ProjectComplexity)rand.Next(0, PROJECT_COMPLEXITY.Length);

            int minDay = 1, maxDay = 2;
            List<int> randTechTime = new List<int>();
            int[,] percentagesForTechnologyTime = { { 50, 100, 50, 0, 40, 40 }, { 100, 100, 100, 40, 70, 100 }, { 100, 100, 100, 50, 100, 100 }, };
            //{timeModyficatorTechnology,dayModyficatorDeadline} w 3 odmianach dla konkretcnych poziomów złożoności projektu}
            int[,] tableOfIntModyficator = { { 0, 16 }, { 2, 8 }, { 4, 4 }, };
            // { penaltyModyficator, rewardModyficator }
            double[,] tableOfDoubleModyficator = { { 50.00, 200.00 }, { 100.00, 400.00 }, { 150.00, 600.00 }, };

            int rowModyficator = 0; //podstawowy modyfikator dla projektów łatwych
            switch (randProjectComplexity)
            {
                case GameEnum.ProjectComplexity.Średni:
                    rowModyficator = 1;
                    break;

                case GameEnum.ProjectComplexity.Trudny:
                    rowModyficator = 2;
                    break;
            }

            //generacja czasów poszczególnych technologii procentowo z modyfikatorem
            for (int i = 0; i < 6; i++)
            {
                int randTime = (rand.Next(minDay, maxDay + 1)) + tableOfIntModyficator[rowModyficator, 0];
                int randTimePercent = (rand.Next(1, 101));
                if (randTimePercent <= percentagesForTechnologyTime[rowModyficator, i]) randTechTime.Add(randTime);
                else randTechTime.Add(0);
            }
            //ilość dni na projekt od dnia generacji = min czas projektu + modyfikator
            int dayDeadline = 0;
            foreach (var time in randTechTime)
            {
                dayDeadline += time;
            }
            dayDeadline += tableOfIntModyficator[rowModyficator, 1];
            int forMoney = rand.Next(6, 13);

            int[] randTechDuration = new int[randTechTime.Count];
            for (int i = 0; i < randTechTime.Count; i++)
            {
                int time = randTechTime[i];
                randTechDuration[i] = time;
            }

            return new GameProject(PROJECT_FIRST_SEGMENT_NAME[randFirstSegmentName] + PROJECT_LAST_SEGMENT_NAME[randLastSegmentName],
                    randProjectComplexity,
                    now.AddDays(dayDeadline),
                    (tableOfDoubleModyficator[rowModyficator, 0] * forMoney),
                    (tableOfDoubleModyficator[rowModyficator, 1] * forMoney),
                    (rand.Next(minDay, maxDay + 1)) + tableOfIntModyficator[rowModyficator, 0],
                    randTechDuration
            );
        }

        public static Client GetRandomClient()
        {
            int randCharacter = rand.Next(0, CLIENT_CHARACTER.Length);
            int randName = rand.Next(0, NAMES.Length);
            int randSurname = rand.Next(0, SURNAMES.Length);

            return new Client(NAMES[randName], SURNAMES[randSurname], (GameEnum.ClientCharacter)randCharacter);
        }

        public static Employee GetRandomEmployee(GameEnum.Occupation occupation)
        {
            int randName = rand.Next(0, NAMES.Length);
            int randSurname = rand.Next(0, WORKER_SURNAMES.Length);
            double cost;

            switch (occupation)
            {
                case GameEnum.Occupation.Sprzedawca:
                    cost = DEALER_MAIN_COST;
                    return new Dealer(NAMES[randName], WORKER_SURNAMES[randSurname], cost, cost / 4, cost / 2);

                case GameEnum.Occupation.Tester:
                    cost = TESTER_MAIN_COST;
                    return new Tester(NAMES[randName], WORKER_SURNAMES[randSurname], cost, cost / 4, cost / 2);

                default:
                    cost = PROGRAMMER_MAIN_COST; //w późniejszej fazie koszty programistów zostaną zmienione (brakuje tu jeszcze parametrów losowych technologii które programisci umieją(będą to 3 losowe technologie))
                    return new Programmer(NAMES[randName], WORKER_SURNAMES[randSurname], cost, cost / 4, cost / 2);
            }
        }

        //szansa w procentach że coś się uda która po wylosowaniu zwraca true lub false
        public static bool CheckPercentegesChance(int chance)
        {
            int randPercent = (rand.Next(1, 101));
            return chance >= randPercent;
        }

        public static void RandomAddProjectToClient(List<GameProject> projects, List<Client> clients)
        {
            foreach (var project in projects)
            {
                int ranIndexClient = rand.Next(0, clients.Count);
                clients[ranIndexClient].AddProject(project);
            }
        }

        public static void RandomAddProjectToClient(GameProject project, List<Client> clients)
        {
            int ranIndexClient = rand.Next(0, clients.Count);
            clients[ranIndexClient].AddProject(project);
        }
    }
}