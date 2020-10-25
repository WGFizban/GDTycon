using GDTycon.Game.NPC;
using System;
using System.Collections.Generic;

namespace GDTycon.Game.Engine
{
    internal class Player : ISearchingProjectInterface, IProgrammingInterface
    {
        public static readonly double defaultStartingCash = 10000.00;

        public string nickName;
        private double cash;
        public List<Employee> myEmployee = new List<Employee>();
        public List<GameProject> myProjects = new List<GameProject>();

        //listy pamiętające daty i wielkości wypłat za gotowe projekty
        public List<DateTime> dateOfPrizeForProject = new List<DateTime>();

        public List<GameProject> finishedProjects = new List<GameProject>();
        public int countFeePerMonth = 0;

        //ilość dni wykorzystanych na szukanie projektu od klientów
        private int dayForLookingClient = 0;

        public Player(string nickName)
        {
            this.nickName = nickName;
            this.cash = defaultStartingCash;
        }

        public int GetDayForLookingClient()
        {
            return dayForLookingClient;
        }

        //sprawdzić czy da się zamienić poniższe funkcje z java na właściwość
        //public double Cash { get => Math.Round(cash,2); set => cash = value; }

        public double GetCash()
        {
            //dodanie zaokrąglania do pola cash - pomocne przy pracy z procentami

            return Math.Round(cash, 2);
        }

        public void SetCash(double cash)
        {
            this.cash = cash;
        }

        public void CheckAndShowProject(DateTime curDay)
        {
            if (myProjects.Count == 0) Console.WriteLine("Aktualnie nie masz żadnych projektów \n");
            else ShowMyProjects(curDay);
        }

        public void ShowMyProjects(DateTime curDay)
        {
            Console.Clear();
            Console.WriteLine("Twoje projekty: \n");
            for (int i = 0; i < myProjects.Count; i++)
            {
                GameProject project = myProjects[i];
                Console.WriteLine($"{i} {project.ToString(curDay)}");
            }
        }

        public bool AnyoneProjectIsReady()
        {
            bool isReady = true;
            foreach (var project in myProjects)
            {
                if (project.ready) break;
                else
                {
                    isReady = false;
                    break;
                }
            }
            return isReady;
        }

        public bool AllProjectAreReady()
        {
            bool isReady = true;
            foreach (var project in myProjects)
            {
                if (!project.ready)
                {
                    isReady = false;
                    break;
                }
            }
            return isReady;
        }

        public bool AreProjectsToTest()
        {
            int isTested = 0;
            if (HasProject())
            {
                foreach (var project in myProjects)
                {
                    if (project.coderError == 0) isTested = 1;
                    else
                    {
                        isTested = 0;
                        break;
                    }
                }
            }
            return isTested == 1;
        }

        public bool HasProject()
        {
            return myProjects.Count > 0;
        }

        public void AddProject(GameProject project)
        {
            this.myProjects.Add(project);
            Console.WriteLine("\nPomyślnie dodałeś projekt " + project.projectName + " do swoich zleceń");
        }

        /// <summary>
        /// Sprawdza czy gracz posiada pracownika o konkretnym rodzaju
        /// </summary>
        /// <param Rodzaj zawodu ="occupation"></param>
        /// <returns></returns>
        public bool HasEmployee(GameEnum.Occupation occupation)
        {
            foreach (var worker in myEmployee)
            {
                if (worker.mainOccupation.Equals(occupation)) return true;
            }
            return false;
        }

        public void ShowMyEmployee()
        {
            for (int i = 0; i < myEmployee.Count; i++)
            {
                Employee employee = myEmployee[i];
                Console.WriteLine("\n" + i + " " + employee);
            }
        }

        public bool IsOnlyMobile(GameProject project)
        {
            int mobile = 0;
            int otherTech = 0;
            foreach (var technology in GameEnum.technology)
            {
                if (!technology.Equals("mobile"))
                {
                    if (otherTech < project.daysForTechnology[technology])
                        otherTech = project.daysForTechnology[technology];
                }
                else
                {
                    if (mobile < project.daysForTechnology["mobile"]) mobile = project.daysForTechnology["mobile"];
                }
            }
            return otherTech == 0 && !(mobile == 0);
        }

        public void ProgrammingDay(GameProject selectedProject)
        {
            Console.WriteLine("Spędzasz dzień na programowaniu");
            int time;

            foreach (var technology in GameEnum.technology)
            {
                if (selectedProject.daysForTechnology[technology] > 0)
                {
                    time = selectedProject.daysForTechnology[technology];
                    //gracz nie umie prgramować mobilnie
                    if (!technology.Equals("mobile"))
                    {
                        selectedProject.daysForTechnology[technology] = time - 1;
                        //gracz ma 10% szans na wygenerowanie błedu w projekcie - błedy się sumują i mają wplyw przy oddaniu projektu

                        if (Generator.CheckPercentegesChance(10)) selectedProject.coderError++;
                        break;
                    }
                }
            }
            selectedProject.IsReady();
        }

        public bool SearchProject()
        {
            if (dayForLookingClient < GameEnum.MIN_SEARCHING_DAYS)
            {
                dayForLookingClient++;
                if (dayForLookingClient == GameEnum.MIN_SEARCHING_DAYS)
                {
                    dayForLookingClient = 0;
                    return true;
                }
                else return false;
            }
            return false;
        }
    }
}