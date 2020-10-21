using System;
using System.Collections.Generic;
using System.Text;

namespace GDTycon.Game.Engine
{
    class Menu
    {
        private string[] menuOptions;

        public Menu(string[] menuOptions)
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











    }
}
