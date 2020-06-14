using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Board
    {
        public readonly IReadOnlyCollection<string> ColumnsNames;
		public readonly string emailCreator;
        internal Board(IReadOnlyCollection<string> columnsNames, string emailCreator) 
        {
            this.ColumnsNames = columnsNames;
			this.emailCreator = emailCreator;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Board))
                return false;

            else
            {
                Board board = (Board)obj;
                if (this.ColumnsNames.SequenceEqual(board.ColumnsNames)&&this.emailCreator==board.emailCreator)
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
