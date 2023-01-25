using System.Collections.Generic;
using TodoPlus.Parsers.Projects;
using TodoPlus.Parsers.Todos;

namespace TodoPlus.Parsers.Files
{
    public class TodoFile
    {
        public List<Project> Projects { get; }
        public List<Todo> Todos { get; }
        
        public TodoFile(IEnumerable<Project> projects, IEnumerable<Todo> todos)
        {
            Projects = new List<Project>(projects);
            Todos = new List<Todo>(todos);
        }
    }
}