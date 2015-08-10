using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using JP.Utils.Data;
using JP.Utils.Framework;
using MyerList.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace MyerListUWP.ViewModel
{
    public class DeletedItemViewModel : ViewModelBase, INavigable
    {

        /// <summary>
        /// 没有已经删除的内容
        /// </summary>
        private Visibility _noDeletedItemsVisibility;
        public Visibility NoDeletedItemsVisibility
        {
            get
            {
                return _noDeletedItemsVisibility;
            }
            set
            {
                if (_noDeletedItemsVisibility != value)
                {
                    _noDeletedItemsVisibility = value;
                    RaisePropertyChanged(() => NoDeletedItemsVisibility);
                }
            }
        }



        /// <summary>
        /// 已经删除了的待办事项
        /// </summary>
        private ObservableCollection<ToDo> _deletedToDos;
        public ObservableCollection<ToDo> DeletedToDos
        {
            get
            {
                if (_deletedToDos != null)
                {
                    if (_deletedToDos.Count == 0) NoDeletedItemsVisibility = Visibility.Visible;
                    else NoDeletedItemsVisibility = Visibility.Collapsed;
                    return _deletedToDos;
                }
                return _deletedToDos = new ObservableCollection<ToDo>();
            }
            set
            {
                if (_deletedToDos != value)
                {
                    _deletedToDos = value;
                    RaisePropertyChanged(() => DeletedToDos);
                }
            }
        }

        #region Deleted involved
        /// <summary>
        /// Readd the item into the list
        /// </summary>
        private RelayCommand<string> _redoCommand;
        public RelayCommand<string> RedoCommand
        {
            get
            {
                if (_redoCommand != null) return _redoCommand;
                return _redoCommand = new RelayCommand<string>(async (id) =>
                {
                    var scheToAdd = DeletedToDos.ToList().Find(s =>
                    {
                        if (s.ID == id) return true;
                        else return false;
                    });

                    Messenger.Default.Send(new GenericMessage<ToDo>(scheToAdd), "Redo");
                    
                    DeletedToDos.Remove(scheToAdd);
                    await SerializerHelper.SerializerToJson<ObservableCollection<ToDo>>(DeletedToDos, "deleteditems.sch", true);

                });
            }
            set
            {
                if (_redoCommand != value)
                {
                    _redoCommand = value;
                    RaisePropertyChanged(() => RedoCommand);
                }
            }
        }

        /// <summary>
        /// Delete the items permanently
        /// </summary>
        private RelayCommand<string> _permanentDeleteCommand;
        public RelayCommand<string> PermanentDeleteCommand
        {
            get
            {
                if (_permanentDeleteCommand != null) return _permanentDeleteCommand;
                return _permanentDeleteCommand = new RelayCommand<string>(async (id) =>
                {
                    DeletedToDos.Remove(DeletedToDos.ToList().Find(s =>
                    {
                        if (s.ID == id) return true;
                        else return false;
                    }));
                    await SerializerHelper.SerializerToJson<ObservableCollection<ToDo>>(DeletedToDos, "deleteditems.sch", true);

                });
            }
            set
            {
                if (_permanentDeleteCommand != value)
                {
                    _permanentDeleteCommand = value;
                    RaisePropertyChanged(() => PermanentDeleteCommand);
                }
            }
        }
        #endregion

        public DeletedItemViewModel()
        {
            DeletedToDos = new ObservableCollection<ToDo>();
            NoDeletedItemsVisibility = Visibility.Collapsed;
            Messenger.Default.Register<GenericMessage<ToDo>>(this, "Delete",async msg =>
              {
                  DeletedToDos.Add(msg.Content);
                  await SerializerHelper.SerializerToJson<ObservableCollection<ToDo>>(DeletedToDos, "deleteditems.sch", true);

              });
            Messenger.Default.Register<GenericMessage<ToDo>>(this, "Redo", act =>
            {
                this.NewToDo = act.Content;
                OkCommand.Execute(false);
            });
        }
        public void Activate(object param)
        {

        }

        public void Deactivate(object param)
        {

        }
    }
}
