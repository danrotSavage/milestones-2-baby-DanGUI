using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("KanbanUnitTest")]

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Column
    {
        public readonly IReadOnlyCollection<Task> Tasks;
        public readonly string Name;
        public readonly int Limit;
        internal Column(IReadOnlyCollection<Task> tasks, string name, int limit)
        {
            this.Tasks = tasks;
            this.Name = name;
            this.Limit = limit;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Column))
                return false;

            else
            {
                Column col = (Column)obj;
                if (col.Limit != this.Limit | col.Name != this.Name)
                    return false;
                else
                {
                    if (this.Tasks == null & col.Tasks == null)
                        return true;
                    IEnumerable <Task> tas1 = this.Tasks.Except<Task>(col.Tasks);
                    IEnumerable<Task> tas2 = col.Tasks.Except(this.Tasks);
                    if (tas1.Count<Task>()==0 & tas2.Count<Task>() == 0)
                        return true;
                    else
                        return false;
                }
            }
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
