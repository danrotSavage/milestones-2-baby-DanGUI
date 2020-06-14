using System;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Task
    {
        public readonly int Id;
        public readonly DateTime CreationTime;
        public readonly DateTime dueDate;
        public readonly string Title;
        public readonly string Description;
		    public readonly string emailAssignee;
        internal Task(int id, DateTime creationTime, string title, string description, string emailAssignee,DateTime dueDate)

        {
            this.Id = id;
            this.CreationTime = creationTime;
            this.Title = title;
            this.Description = description;
			      this.emailAssignee = emailAssignee;
            this.dueDate = dueDate;

        }
        public override bool Equals(object obj)
        {
            if (!(obj is Task))
                return false;
            else
            {
                Task tas = (Task)obj;
                if (tas.Id == this.Id & tas.Title == this.Title & tas.Description == this.Description &-1<(tas.CreationTime-this.CreationTime).TotalMinutes&& (tas.CreationTime - this.CreationTime).TotalMinutes < 1 & tas.emailAssignee==this.emailAssignee&tas.dueDate==this.dueDate)

                    return true;
                else
                    return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }  
    }
}
