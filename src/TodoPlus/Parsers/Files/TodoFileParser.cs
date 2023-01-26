using System.Collections.Generic;
using System.Linq;
using TodoPlus.Parsers.Projects;
using TodoPlus.Parsers.Todos;

namespace TodoPlus.Parsers.Files
{
    public class TodoFileParser
    {
        private readonly IProjectParser _projectParser;
        private readonly ITodoParser _todoParser;
        
        public TodoFileParser(IProjectParser projectParser, ITodoParser todoParser)
        {
            _projectParser = projectParser;
            _todoParser = todoParser;
        }

        public TodoFile Parse(string text)
        {
            var lines = text.Split('\n');
            var projects = new List<Project>();
            var todos = new List<Todo>();
            Project currentProject = null;
            var currentIndentation = 0;
            foreach (var line in lines)
            {
                if (_projectParser.TryParse(line, out var project)) {

                    // Gleiche Ebene - Gleiches Parent-Projekt
                    if (project.Indentation == currentIndentation)
                    {
                        project.Parent = currentProject?.Parent;
                    }
                    // Tiefere Ebene - Parent-Projekt ist das aktuelle Projekt
                    else if (project.Indentation > currentIndentation)
                    {
                        project.Parent = currentProject;
                        project.Parent?.Children.Add(project);
                    }
                    // Höhere Ebene - Parent-Projekt ist das letzte Projekt mit niedriger Indentation
                    else
                    {
                        var parent = projects.LastOrDefault(p => p.Indentation < project.Indentation);
                        project.Parent = parent;
                        project.Parent?.Children.Add(project);
                    }
                    
                    projects.Add(project);
                    currentIndentation = project.Indentation;
                    currentProject = project;
                }
                else
                {
                    if (!_todoParser.TryParse(line, out var todo)) continue;
                    if (currentProject != null)
                    {
                        todo.Project = currentProject;
                        currentProject.Todos.Add(todo);
                    }
                    // Todos werden alle auch auf der Hauptebene gesammelt
                    todos.Add(todo);
                }
            }
            return new TodoFile(projects, todos);
        }
    }
}