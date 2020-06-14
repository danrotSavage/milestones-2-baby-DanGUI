using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF2SOL_final;
using WPF2SOL_final.Model;

namespace WpfApp1.Model
{
    public class ColumnModel : NotifiableModelObject
    {
        public ObservableCollection<MTask> Tasks { get; set; }
        
        private string name;
        private int limit;
        public string Name
        {
            get => name;
            set
            {
                this.name = value;
                RaisePropertyChanged("Name");
                MessageBox.Show(value);
            }
        }
        public int Limit
        {
            get => limit;
            set
            {
                this. limit= value;
                RaisePropertyChanged("Limit");
            }
        }
        public ColumnModel(BackendController cont, string name,int limit):base (cont)
        {
            this.name = name;
            this.limit = limit;

            

        }
        public void HandleChange(object sender, NotifyCollectionChangedEventArgs e)

        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (ColumnModel m in e.OldItems)
                    Controller.RemoveColumn(name, 8);

            }
            

        }


    }
}
