using GDTycon.Game.Engine;
using System.Collections.Generic;

namespace GDTycon.Game.NPC
{
    internal class Client
    {
        public string firstName;
        public string lastName;
        public GameEnum.ClientCharacter character;
        public List<GameProject> projects;

        public Client(string firstName, string lastName, GameEnum.ClientCharacter character)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.character = character;
        }

        public void AddProject(GameProject project)
        {
            if (project != null) project.SetOwner(this);
            this.projects.Add(project);
        }

        public override string ToString()
        {
            return "Klient " + firstName + " " + lastName /*+ " o charakterze " + character*/;
        }
    }
}