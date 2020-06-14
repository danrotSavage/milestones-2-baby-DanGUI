using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presentation;
using Presentation.Model;

namespace Presentation.Model
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
        public ObservableCollection<ColumnModel> columnNames;
        public ObservableCollection<ColumnModel> ColumnNames
        {
            get => columnNames;
            set
            {
                columnNames = value;
                RaisePropertyChanged("ColumnNames");
            }
        }

        public BoardModel(BackendController controller, string user,IReadOnlyCollection<string> columns) : base(controller)
        {
            
            this.user = user;
            LinkedList<ColumnModel> colMod = new LinkedList<ColumnModel>();
            foreach (string m in columns)
            {
                colMod.AddLast(new ColumnModel(controller,m));
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
            ColumnNames.Add(new ColumnModel (Controller,newTitle));
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
