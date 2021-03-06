﻿using GDTycon.Game.Engine;

namespace GDTycon.Game.NPC
{
    internal class Programmer : Employee
    {
        public Programmer(string name, string surname, double salary, double employmentCost, double costOfDismissal) : base(name, surname, salary, employmentCost, costOfDismissal)
        {
            mainOccupation = GameEnum.Occupation.Programista;
        }

        public override bool DoYourWorkForPlayer(Player player)
        {
            return true;
        }
    }
}