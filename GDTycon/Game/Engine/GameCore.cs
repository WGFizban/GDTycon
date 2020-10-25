using GDTycon.Game.NPC;
using System;
using System.Collections.Generic;

namespace GDTycon.Game.Engine
{
    public class GameCore
    {
        // data rozpoczęcia gry i ilość dostępnych zasobów
        private static readonly DateTime startDate = DateTime.Parse("2020, 1, 20");

        private const int startAvailableProject = 3;
        private const int startAvailableTesters = 1;
        private const int startAvailableDealer = 1;
        private const int startAvailableProgrammers = 2;

        //Pamięć dnia, aktualnego gracza, i stanu dla gry
        public DateTime currentDay;

        private Player playerOnTurn;
        private bool gameIsOn;

        //listy potrzebne na bierząco
        private readonly List<Player> players = new List<Player>();

        private readonly List<GameProject> availableProject = new List<GameProject>();
        private readonly List<Employee> availableEmployee = new List<Employee>(); //lista do przechowywania dostępnych pracowników
        private readonly List<Client> allClient = new List<Client>();

        //Przygotowanie menu pojawiających się w grze
        private readonly Menu dayMenu = new Menu("Programuj", "Zobacz dostępne projekty", "Zobacz moje projekty", "Szukaj klientów/projektów", "Testuj kody", "Oddaj projekt", "Zarządzaj pracownikami", "Rozlicz urzędy", "Wyjdź z programu");

        private readonly Menu testMenu = new Menu("Testuj swój kod", "Testuj kod pracowników", "testuj kod współpracowników", "Wróć do menu");
        private readonly Menu hireMenu = new Menu("Zatrudnij pracownika", "Zwolnij pracownika", "Zainwestuj w reklamy by szukać pracowników (300.00)", "Wróć do menu");
        private readonly Menu projMenu = new Menu("Wybierz projekt", "Wróć do menu");

        public GameCore()
        {
            StartNewGame();
        }

        public void StartNewGame()
        {
            //ekran powitalny
            Console.WriteLine($"Witaj w Game dev Tycon symulatorze firmy IT. Przygoda rozpocznie się w {startDate:D}\nPodejmuj mądre decyzje i nie zbankrutuj. Powodzenia!\n");

            //wprowadzenie graczy
            int isMorePlayer;

            do
            {
                Console.WriteLine("Podaj Nick dla gracza: ");
                Player player = new Player(Console.ReadLine());
                Console.Clear();
                isMorePlayer = dayMenu.SelectOptions(2, "Czy będzie więcej graczy?  0 - nie   1 - tak    wybierasz: ");
                players.Add(player);
            } while (isMorePlayer == 1);

            //poczatkowe ustawienia
            SetStartProporties();

            //start gry dla kazdego z graczy
            while (gameIsOn)
            {
                foreach (var player in players)
                {
                    playerOnTurn = player;
                    Console.Clear();
                    StartDay(playerOnTurn); //rozpoczynanie nowego dnia
                }
            }
        }

        private void SetStartProporties()
        {
            //przygotowanie puli początkowych projektów dostępnych dla gracza
            for (int i = 0; i < startAvailableProject; i++)
            {
                availableProject.Add(Generator.GetRandomGameProject(startDate));
            }
            // przygotowanie puli klientów jako 2x ilość początkowych projektów
            for (int i = 0; i < startAvailableProject * 2; i++)
            {
                allClient.Add(Generator.GetRandomClient());
            }
            //lososwe przypisanie klientów do wygenerowanych projektów
            Generator.RandomAddProjectToClient(availableProject, allClient);

            //ustawienie początkowych pracowników
            for (int i = 0; i < startAvailableDealer; i++)
            {
                availableEmployee.Add(Generator.GetRandomEmployee(GameEnum.Occupation.Sprzedawca));
            }
            for (int i = 0; i < startAvailableTesters; i++)
            {
                availableEmployee.Add(Generator.GetRandomEmployee(GameEnum.Occupation.Tester));
            }
            for (int i = 0; i < startAvailableProgrammers; i++)
            {
                availableEmployee.Add(Generator.GetRandomEmployee(GameEnum.Occupation.Programista));
            }
            gameIsOn = true;
            currentDay = startDate;

            // część testowa
            /*
                        players.get(0).myEmployee.add(Generator.getRandomEmployee(Occupation.PROGRAMMER));
                        players.get(0).myEmployee.add(Generator.getRandomEmployee(Occupation.PROGRAMMER));
                        players.get(0).myEmployee.add(Generator.getRandomEmployee(Occupation.TESTER));
                        players.get(0).myEmployee.add(Generator.getRandomEmployee(Occupation.DEALER));*/
        }

        private void StartDay(Player player)
        {
            if (!gameIsOn) return;

            PrepareConsole(player);

            //kod wypłacania pensji pracowniczych
            //CheckIsTimeForReward(currentDay);

            //kod by pracowali pracownicy
            //EmployeeToWork(player);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Twój stan konta: " + player.GetCash());
            Console.ResetColor();

            MainAction(dayMenu.SelectOptions());
        }

        // przygotowywanie konsoli - czyszczenie i info o graczu i dacie
        private void PrepareConsole(Player player)
        {
            Console.Clear();
            if (players.Count > 1) Console.Write("\nTura gracza " + player.nickName);
            Console.WriteLine($"          Jest  {currentDay:D} \n");
        }

        // Główny switch sterujący - Menu główne
        private void MainAction(int action)
        {
            PrepareConsole(playerOnTurn);
            switch (action)
            {
                case 0:
                    //GoProgrammingPlayer();
                    break;

                case 1:
                    PrepareConsole(playerOnTurn);
                    ChooseFromAvailableProject();
                    break;

                case 2:
                    ShowPlayerProject();
                    break;

                case 3:
                    SearchNewProject(playerOnTurn);
                    break;

                case 4:
                    //TestProject();
                    break;

                case 5:
                    //UploadReadyProject(playerOnTurn);
                    break;

                case 6:
                    //ManageEmployee(playerOnTurn);
                    break;

                case 7:
                    PayOfficialFees();
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("Zamykam grę GameDevTycon. Do widzeenia :) ");
                    gameIsOn = false;
                    break;
            }
        }

        //funkcja zakończenia dnia -> odpalenie funkcji sprawdzajacej czy gra trwa
        private void EndDay(Player actuallyPlayer)
        {
            CheckGameDuration(currentDay.AddDays(1));
            if (actuallyPlayer.Equals(players[^1]))
                currentDay = currentDay.AddDays(1);
        }

        // czy gra trwa -> opłacenie zus przy zmianie miesiąca i sprawdzenie warunku przegrania
        // dla określonej wartości pieniężnej (bankrut)
        private void CheckGameDuration(DateTime nextDate)
        {
            CheckZusAndPaySalary(nextDate.Month);
            CheckCash();
        }

        //sprawdzenie czy gracz zapłacił Zus i wypłacenie pensji pracownikom
        private void CheckZusAndPaySalary(int nextMonth)
        {
            // ostatni termin = długosc miesiąca obecnego - dzien miesiaca w obecnej dacie
            int lastTermin = DateTime.DaysInMonth(currentDay.Year, currentDay.Month) - Convert.ToInt32(currentDay.ToString("dd"));

            //Awaryjna przypominajka dla gracza
            if (lastTermin == 2 && !(playerOnTurn.countFeePerMonth == 2))
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\n\nOSTRZEŻENIE! Dwa dni do końca miesiąca. \nJeśli nie poświęcisz wymaganych dni: " + (2 - playerOnTurn.countFeePerMonth) + " na rozliczenia z urzędami przegrasz!");
                Console.ResetColor();
                Console.ReadKey();
            }
            if (!(nextMonth == currentDay.Month) && playerOnTurn.countFeePerMonth < 2)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nW tym miesiącu zapomniałeś dwukrotnie rozliczyś się z urzędami. Wpada kontrola i zamykasz firmę z długami.");
                playerOnTurn.SetCash(0.0);
            }
            else if ((!(nextMonth == currentDay.Month)) && playerOnTurn.countFeePerMonth == 2)
            {
                playerOnTurn.countFeePerMonth = 0;
                Console.WriteLine("\nRozpoczyna się nowy miesiąc. Dobrze że pamiętałeś o Zus :) ");
                //kod do wypłacania pensji pracownikom
                if (playerOnTurn.myEmployee.Count > 0)
                {
                    for (int i = 0; i < playerOnTurn.myEmployee.Count; i++)
                    {
                        int memorySize = playerOnTurn.myEmployee.Count;
                        playerOnTurn.myEmployee[i].GetSalaryFromPlayer(playerOnTurn);
                        if (memorySize > playerOnTurn.myEmployee.Count) i--;
                    }
                }
            }
        }

        // warunek przegrana/wygrania gry
        public void CheckCash()
        {
            if (playerOnTurn.GetCash() <= 0)
            {
                gameIsOn = false;
                Console.WriteLine("Gracz " + playerOnTurn.nickName + " właśnie zbankrutował. Koniec Gry!!!");
            }
            else if (playerOnTurn.GetCash() >= (10 * Player.defaultStartingCash))
            {
                gameIsOn = false;
                Console.WriteLine("Gracz " + playerOnTurn + " zarobił 10x więcej pieniędzy niż miał na początku gry. WYGRYWA!!! GRATULUJĘ!");
            }
        }

        // wybieranie projektu z dostępnych projektów
        private void ChooseFromAvailableProject()
        {
            if (availableProject.Count == 0)
            {
                Console.WriteLine("Na razie nie ma tu żadnych nowych zleceń którymi mógłbyś się zająć");
                Console.ReadKey();
                PrepareConsole(playerOnTurn);
                MainAction(dayMenu.SelectOptions());
            }
            else
            {
                for (int i = 0; i < availableProject.Count; i++)
                {
                    GameProject project = availableProject[i];
                    Console.WriteLine(i + " " + project);
                }
                if (projMenu.SelectOptions() == 0)
                {
                    int choice = dayMenu.SelectOptions(availableProject.Count, "Podaj numer projektu który chcesz wybrać: ");
                    Console.Clear();

                    if (CanAddProject(availableProject[choice], playerOnTurn))
                    {
                        playerOnTurn.AddProject(availableProject[choice]);
                        //availableProject.remove(choice);
                        availableProject.RemoveAt(choice);
                        Console.WriteLine("Do końca dnia cieszysz się z podpisanej umowy i nic nie robisz!");
                        Console.ReadKey();
                        EndDay(playerOnTurn);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n Nie możesz podjąć się tego projektu ze względu na jego złożoność. Spróbuj wybrać inny projekt! \n");
                        Console.ResetColor();
                        ChooseFromAvailableProject();
                    }
                }
                else
                {
                    PrepareConsole(playerOnTurn);
                    MainAction(dayMenu.SelectOptions());
                }
            }
        }

        //logika czy można dodać projekt - odrzucenie skomplikowanych projektów przy braku pracowników
        private bool CanAddProject(GameProject project, Player player)
        {
            return !project.complexity.Equals(GameEnum.ProjectComplexity.Trudny) || player.myEmployee.Count != 0;
        }

        // wyswietlanie projektów gracza - podjętych przez niego
        private void ShowPlayerProject()
        {
            playerOnTurn.CheckAndShowProject(currentDay);
            Console.ReadKey();
            PrepareConsole(playerOnTurn);
            MainAction(dayMenu.SelectOptions());
        }

        // dokonanie niezbędnych płatnosci zusu
        private void PayOfficialFees()
        {
            if (playerOnTurn.countFeePerMonth == 2)
            {
                Console.WriteLine("W tym miesiącu opłaciłeś już urzędy wymaganą ilość razy. ZUS jest Ci wdzięczny.\nKliknij dowolny przycisk by wrócić do Menu");
                Console.ReadKey();
                PrepareConsole(playerOnTurn);
                MainAction(dayMenu.SelectOptions());
            }
            else
            {
                //opłata urzędów jako 5 % dwa razy w msc
                playerOnTurn.SetCash(playerOnTurn.GetCash() - (playerOnTurn.GetCash() * 0.05));
                CheckCash();
                playerOnTurn.countFeePerMonth++;
                Console.WriteLine("Dzień papierkowej roboty. Dokonujesz opłat " + playerOnTurn.countFeePerMonth + " raz w tym miesiącu. Pobraliśmy wymagane opłaty.\nKliknij dowolny przycisk by wrócić do Menu");
                Console.ReadKey();
                EndDay(playerOnTurn);
            }
        }

        //kod załączający wyszukiwanie nowych projektów
        private void SearchNewProject(Player player)
        {
            if (!player.SearchProject())
            {
                Console.WriteLine("Dzisiaj cały dzień poszukujesz nowych zleceń w internecie. Do znalezienia projektu potrzeba jeszcze " + (5 - player.GetDayForLookingClient()) + " wywołania.");
            }
            else
            {
                Console.WriteLine("\nZnalezłeś nowy projekt. Sprawdź go jutro w dostępnych zleceniach!");
                AddNewAvailableProjeckt();
            }
            Console.ReadKey();
            EndDay(playerOnTurn);
        }

        private void AddNewAvailableProjeckt()
        {
            GameProject anotherOne = Generator.GetRandomGameProject(currentDay);
            allClient.Add(Generator.GetRandomClient());
            Generator.RandomAddProjectToClient(anotherOne, allClient);
            availableProject.Add(anotherOne);
        }

        /*

private void EmployeeToWork(Player player) {
    if (!(currentDay.getDayOfWeek().toString().equals("SUNDAY")) && player.myEmployee.size() > 0) {
        System.out.println("Twoi pracownicy pracują");
        for (Employee employee : player.myEmployee) {
            if (employee.mainOccupation.equals(Occupation.DEALER)) {
                if (employee.doYourWorkForPlayer(player)) {
                    GameProject project = Generator.getRandomGameProject(currentDay);
                    project.owner = Generator.getRandomClient();
                    availableProject.add(project);
                    System.out.println("Sprzedawca znalazł nowy projekt! sprawdź go w dostępnych zleceniach.");
                }
            } else employee.doYourWorkForPlayer(player);
        }
    }
}

private void manageEmployee(Player player) throws InterruptedException {
    switch (hireMenu.selectOptions()) {
        case 0:
            //zatrudnianie pracowników
            showAvailableEmployee();
            if (dayMenu.selectOptions(2, "Czy chcesz teraz zatrudnić pracowników?\n0 - Tak lub 1 - Inna opcja     Twój wybór:") == 1)
                manageEmployee(player);
            else {
                int toHire = dayMenu.selectOptions(availableEmployee.size(), "Wybierz pracownika którego chcesz zatrudnić: ");
                availableEmployee.get(toHire).hire(player);
            }
            endDay(playerOnTurn);
            break;

        case 1: //zwalnianie pracowników
            if (player.myEmployee.size() == 0) {
                System.out.println("Nie masz jeszcze żadnych pracowników by ich zwalniać! Wybierz inną opcję");
                manageEmployee(player);
            } else {
                player.showMyEmployee();
                int toDismiss = dayMenu.selectOptions(player.myEmployee.size(), "Wybierz pracownika którego chcesz zwolnić");
                player.myEmployee.get(toDismiss).dismiss(player);
            }
            endDay(playerOnTurn);
            break;

        case 2:
            //kampania reklamowa;
            break;

        case 3:
            mainAction(dayMenu.selectOptions());
            break;
    }
}

private void showAvailableEmployee() {
    System.out.println();
    for (int i = 0; i < availableEmployee.size(); i++) {
        Employee emp = availableEmployee.get(i);
        System.out.println(i + " " + emp);
    }
}

private void uploadReadyProject(Player player) throws InterruptedException {
    //sprawdzanie posiadania projektów
    if (player.myProjects.size() == 0) {
        System.out.println("Musisz POSIADAĆ jakikolwiek projekt");
        mainAction(dayMenu.selectOptions());
        //sprawdzenie posiadania gotowego projektu
    } else if (!player.anyoneProjectIsReady()) {
        System.out.println("Musisz posiadać gotowy projekt żeby go oddać");
        mainAction(dayMenu.selectOptions());
    } else {
        player.showMyProjects();
        int choice = dayMenu.selectOptions(player.myProjects.size(), "Możesz oddać tylko jeden projekt dziennie, który wybierasz: ");
        if (!player.myProjects.get(choice).ready) {
            System.out.println("Musisz wybrać gotowy projekt!\n");
            uploadReadyProject(player);
        } else {
            //sprawdzenie przedawnienia
            Double penalty = checkPeanalty(player, player.myProjects.get(choice), currentDay);
            //błedy w projekcie a kontract
            boolean isContract = impactErrorOnContract(player.myProjects.get(choice));
            System.out.println("\nPomyślnie oddałeś projekt " + player.myProjects.get(choice).projectName + " do klienta " + player.myProjects.get(choice).owner.firstName + " " + player.myProjects.get(choice).owner.lastName +
                    "\nJeśli wszystko dobrze pójdzie zapłatę dostaniesz w " + formatDate.format(currentDay.plusDays(player.myProjects.get(choice).timeOfReward)));

            if (penalty > 0) {
                System.out.println("Nie wykonałeś projektu w terminie i zapłacisz karę: " + penalty + " która w tym momencie jest odejmowana z Twojego konta.");
                player.setCash(player.getCash() - penalty);
            }
            if (!isContract) {
                System.out.println("Straciłeś kontrakt z klientem przez oddanie projektu z błędami! Nie otrzymasz żadnej zapłaty za projekt.");
            } else {
                int daysForClient = additionalTimeForClient(player.myProjects.get(choice));

                if ("SKRWL".equals(player.myProjects.get(choice).owner.character.toString())) {
                    if (!Generator.checkPercentegesChance(1)) {
                        addDateAndFinishedProject(currentDay.plusDays(daysForClient + player.myProjects.get(choice).timeOfReward), player.myProjects.get(choice));
                    }
                } else {
                    addDateAndFinishedProject(currentDay.plusDays(daysForClient + player.myProjects.get(choice).timeOfReward), player.myProjects.get(choice));
                }
                player.myProjects.remove(choice);
                endDay(playerOnTurn);
            }
        }
    }
}

private void addDateAndFinishedProject(LocalDate dateReward, GameProject project) {
    playerOnTurn.dateOfPrizeForProject.add(dateReward);
    playerOnTurn.finishedProjects.add(project);
}

private int additionalTimeForClient(GameProject project) {
    switch (project.owner.character.toString()) {
        case "LUZAK":
            if (Generator.checkPercentegesChance(30)) return 7;
            else return 0;
        case "SKRWL":
            if (Generator.checkPercentegesChance(30)) return 7;
            else if (Generator.checkPercentegesChance(5)) return 31;
            else return 0;
        default:
            return 0;
    }
}

private boolean impactErrorOnContract(GameProject project) {
    if (project.coderError > 0) {
        switch (project.owner.character.toString()) {
            case "WYMAGAJACY":
                return Generator.checkPercentegesChance(50);

            case "SKRWL":
                return false;

            default:
                return true;
        }
    }
    return true;
}

private Double checkPeanalty(Player player, GameProject project, LocalDate currentDay) {
    double penalty = 0.0;
    if (currentDay.isAfter(project.deadLine)) {
        if ("LUZAK".equals(project.owner.character.toString())) {
            //poprawka w porównywania dat (opóznienie nie większe niż tydzień czyli równe mniejsze tydzień)
            if (project.deadLine.plusDays(7).isAfter(currentDay) || project.deadLine.plusDays(7).isEqual(currentDay)) {
                if (!Generator.checkPercentegesChance(20)) penalty = project.penalty;
            } else penalty = project.penalty;
        } else penalty = project.penalty;
        //naliczenie kar za przedawnienie projektu w zależności od charakteru kleinta
    }
    return penalty;
}

//poświęcenie dnia na testowanie daje gwarancję oddania sprawnego kodu
private void testProject() throws InterruptedException {
    switch (testMenu.selectOptions()) {
        case 0:
            testPlayerProject(playerOnTurn);
            break;

        case 1:
            System.out.println("Opcja nie została zaimplementowana");
            break;

        case 2:
            System.out.println("Opcja nie została zaimplementowana");
            break;

        default:
            mainAction(dayMenu.selectOptions());
        break;
    }
}

private void testPlayerProject(Player player) throws InterruptedException {
    if (player.myProjects.size() == 0 || player.areProjectsToTest()) {
        System.out.println("\nNie masz projektów lub nie wymagają one na razie testowania");
        testProject();
    } else {
        player.showMyProjects();
        int choice = dayMenu.selectOptions(player.myProjects.size(), "Wybierz projekt który chcesz testować");

        if (player.myProjects.get(choice).coderError == 0) {
            System.out.println("Ten projekt nie miał błędów. wybierz inny projekt!\n");
            testPlayerProject(player);
        } else {
            player.myProjects.get(choice).coderError = 0;
            System.out.println("Cały dzień testujesz ale masz pewność że już napisany kod jest 100% poprawny");
            endDay(playerOnTurn);
        }
    }
}

private void goProgrammingPlayer() throws InterruptedException {
    if (playerOnTurn.hasProject() && playerOnTurn.allProjectAreReady()) {
        System.out.println("Wszystkie Twoje projekty są gotowe. Pomyśl nad inną opcją.");
        mainAction(dayMenu.selectOptions());
    } else if (playerOnTurn.hasProject()) {
        playerOnTurn.checkAndShowProject();

        int choice = dayMenu.selectOptions(playerOnTurn.myProjects.size(), "Wybierz numer projektu nad którym chcesz pracować: ");

        if (!playerOnTurn.myProjects.get(choice).ready && !(playerOnTurn.isOnlyMobile(playerOnTurn.myProjects.get(choice)))) {
            playerOnTurn.programmingDay(playerOnTurn.myProjects.get(choice));
            endDay(playerOnTurn);
        } else if (playerOnTurn.isOnlyMobile(playerOnTurn.myProjects.get(choice))) {
            System.out.println("Ten projekt wymaga jeszcze technologii mobilnej której nie potrafisz. Wracasz do menu. \nPomyśl nad zleceniem tej częsci komuś lub zatrudnij odpowiedniego pracownika.");
            mainAction(dayMenu.selectOptions());
        } else {
            System.out.println("Ten projekt jest już gotowy. Wracasz do menu. Następnym razem wybierz niedokończony projekt.");
            mainAction(dayMenu.selectOptions());
        }
    } else {
        System.out.println("Nie masz projektów nad którymi możesz pracować. Zacznij pracę nad nowym projektem.");
        mainAction(dayMenu.selectOptions());
    }
}

private void checkIsTimeForReward(LocalDate currentDay) {
    for (int i = 0; i < playerOnTurn.dateOfPrizeForProject.size(); i++) {
        LocalDate dateNow = playerOnTurn.dateOfPrizeForProject.get(i);
        //poprawka w porównywaniu dat (wartości a nie obiekty )
        if (dateNow.isEqual(currentDay)) {
            System.out.println("Dostałeś przelew na konto w wysokości " + playerOnTurn.finishedProjects.get(i).reward + " za zakończony projekt " + playerOnTurn.finishedProjects.get(i).projectName);
            playerOnTurn.setCash(playerOnTurn.getCash() + playerOnTurn.finishedProjects.get(i).reward);
        }
    }
    //zwalnianie pamięci w zmiennych listowych
    for (int i = 0; i < playerOnTurn.dateOfPrizeForProject.size(); i++) {
        LocalDate oldDate = playerOnTurn.dateOfPrizeForProject.get(i);
        if (currentDay.isAfter(oldDate)) {
            playerOnTurn.finishedProjects.remove(i);
            playerOnTurn.dateOfPrizeForProject.remove(i);
            break;
        }
    }
}
         */
    }
}