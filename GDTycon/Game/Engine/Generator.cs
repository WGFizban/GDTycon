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

        private static int min;
        private static int max;

        public static void NumberGenerator()
        {
            Console.WriteLine("100 random integers between 0 and 100:");
            for (int ctr = 0; ctr <= 100; ctr++)
                Console.Write("{0,8:N0}", rand.Next(0, 3));
            Console.WriteLine();
        }

        /*

       //Ganerator projektów
       public static GameProject getRandomGameProject(LocalDate now) {
           int randFirstSegmentName = ThreadLocalRandom.current().nextInt(0, PROJECT_FIRST_SEGMENT_NAME.length);
           int randLastSegmentName = ThreadLocalRandom.current().nextInt(0, PROJECT_LAST_SEGMENT_NAME.length);
           int randProjectComplexity = ThreadLocalRandom.current().nextInt(0, PROJECT_COMPLEXITY.length);

           int minDay = 1, maxDay = 2, rowModyficator;
           List<Integer> randTechTime = new ArrayList<>();
           int[][] percentagesForTechnologyTime = {{50, 100, 50, 0, 40, 40}, {100, 100, 100, 40, 70, 100}, {100, 100, 100, 50, 100, 100},};
           //{timeModyficatorTechnology,dayModyficatorDeadline} w 3 odmianach dla konkretcnych poziomów złożoności projektu}
           int[][] tableOfIntModyficator = {{0, 16}, {2, 8}, {4, 4},};
           // { penaltyModyficator, rewardModyficator }
           Double[][] tableOfDoubleModyficator = {{50.00, 200.00}, {100.00, 400.00}, {150.00, 600.00},};

           switch (PROJECT_COMPLEXITY[randProjectComplexity]) {
               case "MIDDLE":
                   rowModyficator = 1;
                   break;

               case "HARD":
                   rowModyficator = 2;
                   break;

               default:
                   rowModyficator = 0;
           }

           //generacja czasów poszczególnych technologii procentowo z modyfikatorem
           for (int i = 0; i < 6; i++) {
               int randTime = (ThreadLocalRandom.current().nextInt(minDay, maxDay + 1)) + tableOfIntModyficator[rowModyficator][0];
               int randTimePercent = (ThreadLocalRandom.current().nextInt(1, 101));
               if (randTimePercent <= percentagesForTechnologyTime[rowModyficator][i]) randTechTime.add(randTime);
               else randTechTime.add(0);
           }
           //ilość dni na projekt od dnia generacji = min czas projektu + modyfikator
           int dayDeadline = 0;
           for (Integer time : randTechTime) dayDeadline += time;
           dayDeadline += tableOfIntModyficator[rowModyficator][1];
           int forMoney = ThreadLocalRandom.current().nextInt(6, 13);

           Integer[] randTechDuration = new Integer[randTechTime.size()];
           for (int i = 0; i < randTechTime.size(); i++) {
               Integer time = randTechTime.get(i);
               randTechDuration[i] = time;
           }

           return new GameProject(PROJECT_FIRST_SEGMENT_NAME[randFirstSegmentName] + PROJECT_LAST_SEGMENT_NAME[randLastSegmentName],
                   ProjectComplexity.valueOf(PROJECT_COMPLEXITY[randProjectComplexity]),
                   now.plusDays(dayDeadline),
                   (tableOfDoubleModyficator[rowModyficator][0] * forMoney),
                   (tableOfDoubleModyficator[rowModyficator][1] * forMoney),
                   (ThreadLocalRandom.current().nextInt(minDay, maxDay + 1)) + tableOfIntModyficator[rowModyficator][0],
                   randTechDuration
           );
       }
        */

        public static Client getRandomClient()
        {
            int randCharacter = rand.Next(0, CLIENT_CHARACTER.Length);
            int randName = rand.Next(0, NAMES.Length);
            int randSurname = rand.Next(0, SURNAMES.Length);

            return new Client(NAMES[randName], SURNAMES[randSurname], (GameEnum.ClientCharacter)randCharacter);
        }

        /*
       public static Employee getRandomEmployee(Occupation occupation) {
           int randName = ThreadLocalRandom.current().nextInt(0, NAMES.length);
           int randSurname = ThreadLocalRandom.current().nextInt(0, WORKER_SURNAMES.length);
           double cost;

           switch (occupation) {
               case DEALER:
                   cost = DEALER_MAIN_COST;
                   return new Dealer(NAMES[randName], WORKER_SURNAMES[randSurname], cost, cost / 4, cost / 2);

               case TESTER:
                   cost = TESTER_MAIN_COST;
                   return new Tester(NAMES[randName], WORKER_SURNAMES[randSurname], cost, cost / 4, cost / 2);

               default:
                   cost = PROGRAMMER_MAIN_COST; //w późniejszej fazie koszty programistów zostaną zmienione (brakuje tu jeszcze parametrów losowych technologii które programisci umieją(będą to 3 losowe technologie))
                   return new Programmer(NAMES[randName], WORKER_SURNAMES[randSurname], cost, cost / 4, cost / 2);
           }
       }
        */

        //szansa w procentach że coś się uda
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
    }
}