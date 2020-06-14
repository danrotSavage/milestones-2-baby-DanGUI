using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF2SOL_final;
using WPF2SOL_final.Model;

namespace WpfApp1.Model
{
    public class BoardModel:NotifiableModelObject
    {
        private int pos = 0;
        private string columnTitle;
    
        public string Columntitle
        {
            get => columnTitle;
            set
            {
                this.columnTitle = value;
                RaisePropertyChanged("ColumnTitle");

            }
        }

        private string user;
        public ObservableCollection<ColumnModel> ColumnNames { get; set; }

        public BoardModel(BackendController controller, string user) : base(controller)
        {
            
            this.user = user;
            List<ColumnModel> colMod = new List<ColumnModel>();
            
             
            foreach (string m in controller.GetBoard(user))
            {
                colMod.Add(new ColumnModel(controller,m,8));
                pos++;
            }


           

            ColumnNames = new ObservableCollection<ColumnModel>(colMod);
            ColumnNames.CollectionChanged += HandleChange;
               }
        public void RemoveColumn(ColumnModel SelectedColumn)
        {
            
            ColumnNames.Remove(SelectedColumn);            
           

        }
       
        public void AddColumn(string newTitle)
        {
            ColumnNames.Add(new ColumnModel (Controller,newTitle,8));
            pos++;
        }
        public void HandleChange(object sender, NotifyCollectionChangedEventArgs e)

        {
            if(e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (ColumnModel m in e.OldItems)
                    Controller.RemoveColumn(user,8);

            }
            


        }

    }
}
