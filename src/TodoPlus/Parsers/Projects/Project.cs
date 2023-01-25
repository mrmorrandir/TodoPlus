using System.Collections.Generic;
using TodoPlus.Parsers.Todos;

namespace TodoPlus.Parsers.Projects
{
    public class Project
    {
        public Project Parent { get; set; }
        public string Name { get; set; }
        
        public int Indentation { get; set;}
        public List<Project> Children { get; } 
        public List<Todo> Todos { get; }
        
        public Project()
        {
            Children = new List<Project>();
            Todos = new List<Todo>();
        }

        public override string ToString()
        {
            return Parent != null ? Parent + " > " + Name : Name;
        }
    }
}