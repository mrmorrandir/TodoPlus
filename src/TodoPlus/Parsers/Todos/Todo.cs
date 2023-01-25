using System;
using System.Collections.Generic;
using TodoPlus.Parsers.Projects;

namespace TodoPlus.Parsers.Todos
{
    public class Todo
    {
        public State State { get; set; }
        public string Description { get; set; }
        public DateTime? Started { get; set; }
        public DateTime? Done { get; set; }
        public DateTime? Cancelled { get; set; }
        public TimeRange? Lasted { get; set; }
        public TimeRange? Wasted { get; set; }
        public TimeRange? Estimate { get; set; }
        public List<string> Tags { get; set; }
        public List<Property> Properies { get; set; }
        
        public int Indentation { get; set; }
        public Project Project { get; set; }
    }
}