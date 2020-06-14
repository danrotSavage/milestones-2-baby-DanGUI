using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DAL.DTO
{
    class DTObj
    {
        public const string IDColumnName = "Id";
        protected DalController Controller;
        public int Id { get; set; } = -1;
        protected DTObj(DalController controller)
        {
            Controller = controller;
        }
    }
}
