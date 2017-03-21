using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using AJTaskManagerMobile.Common;
using AJTaskManagerMobile.DataServices;
using AJTaskManagerMobile.Helpers;
using AJTaskManagerMobile.Model;
using AJTaskManagerMobile.Model.DTO;
using AJTaskManagerMobile.Views.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.WindowsAzure.MobileServices;
using Syncfusion.Data.Extensions;
using Syncfusion.UI.Xaml.Converters;

namespace AJTaskManagerMobile.ViewModel
{
    public class ToDoItemsViewModel : ViewModelBase, INavigable
    {
        private ObservableCollection<TodoItem> _todoItems;
        private ObservableCollection<User> _users;
        private ObservableCollection<Group> _groups;
        private string _newItemText;
        private string _listTitle;
        private bool _isBusy;
        private bool _isAddEnabled;
        private string _userInternalId;
        private IMobileServiceTable<TodoItem> _todoTable = DataServiceBase.MobileService.GetTable<TodoItem>();
        //private IMobileServiceTable<User> _userTable = DataServiceBase.MobileService.GetTable<User>();
        private RelayCommand<object> _addNewItemCommand;
        private RelayCommand _openAddItemPopupCommand;
        private RelayCommand _deleteCompletedCommand;
        private RelayCommand<object> _checkboxCheckedCommand;
        private RelayCommand _refreshCommand;
        private DataServices.DataService _dataService;
        private ITodoItemsDataService _todoItemsDataService;
        private IUserDataService _userDataService;
        private IRoleTypeDataService _roleTypeDataService;
        private TodoList _associatedTodoList;

        public ObservableCollection<TodoItem> TodoItems
        {
            get
            {
                return _todoItems;
            }
            set
            {
                if (_todoItems == value)
                    return;
                _todoItems = value;
                RaisePropertyChanged(() => TodoItems);
            }
        }

        public ObservableCollection<User> Users
        {
            get
            {
                return _users;
            }
            set
            {
                if (_users == value)
                    return;
                _users = value;
                RaisePropertyChanged(() => Users);
            }
        }

        public ObservableCollection<Group> Groups
        {
            get
            {
                return _groups;
            }
            set
            {
                if (_groups == value)
                    return;
                _groups = value;
                RaisePropertyChanged(() => Groups);
            }
        }

        public string NewItemText
        {
            get
            {
                return _newItemText;
            }
            set
            {
                if (_newItemText == value)
                    return;
                _newItemText = value;
                RaisePropertyChanged(() => NewItemText);
            }
        }

        public string ListTitle
        {
            get { return _listTitle; }
            set
            {
                if (_listTitle == value)
                    return;
                _listTitle = value;
                RaisePropertyChanged(() => ListTitle);
            }
        }


        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value)
                    return;
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public bool IsAddEnabled
        {
            get { return _isAddEnabled; }
            set
            {
                if (_isAddEnabled == value)
                    return;
                _isAddEnabled = value;
                RaisePropertyChanged(() => IsAddEnabled);
            }
        }

        public RelayCommand<object> AddNewItemCommand
        {
            get
            {
                return _addNewItemCommand ??
                       (_addNewItemCommand = new RelayCommand<object>(
                           async item =>
                           {
                               TextBox txtBox = item as TextBox;
                               if (txtBox != null)
                               {
                                   IsBusy = true;
                                   //var userInternalId = await GetUserInternalId();
                                   //bool canUserAdd =
                                   //    await _roleTypeDataService.CanUserAddOrDeleteItem(userInternalId, _associatedTodoList.GroupId);
                                   //if (canUserAdd)
                                   //{
                                   await
                                       _dataService.Insert(new TodoItem()
                                       {
                                           Id = Guid.NewGuid().ToString(),
                                           IsCompleted = false,
                                           Text = txtBox.Text,
                                           TodoListId = _associatedTodoList.Id,
                                           CreatedDateTime = DateTime.Now,
                                           IsDeleted = false,
                                           Deadline = DateTime.Now.AddYears(100),
                                       });
                                   Refresh();
                                   //}
                                   //else
                                   //{
                                   //    IsBusy = false;
                                   //    new MessageDialog(Constants.UserCantAddOrDelete).ShowAsync();
                                   //}
                               }
                           }));
            }
        }

        public RelayCommand OpenNewItemPopupCommand
        {
            get
            {
                return _openAddItemPopupCommand ??
                       (_openAddItemPopupCommand = new RelayCommand(async () =>
                       {
                           var userInternalId = await GetUserInternalId();
                           IsAddEnabled =
                               await _roleTypeDataService.CanUserAddOrDeleteItem(userInternalId, _associatedTodoList.GroupId);
                           if (IsAddEnabled)
                           {
                               Popup popup = new Popup();
                               var addItem = new AddItemControl
                               {
                                   DataContext = this,
                                   MinWidth = 400,
                                   Height = 100,
                                   HorizontalAlignment = HorizontalAlignment.Stretch,
                                   VerticalAlignment = VerticalAlignment.Center,
                                   Margin = new Thickness() { Top = 30 }
                               };
                               popup.Child = addItem;
                               popup.IsLightDismissEnabled = true;
                               popup.IsOpen = true;
                           }
                           else
                           {
                               new MessageDialog(Constants.UserCantAddOrDelete).ShowAsync();
                           }
                       }));
            }
        }

        public RelayCommand DeleteCompletedCommand
        {
            get
            {
                return _deleteCompletedCommand ??
                       (_deleteCompletedCommand = new RelayCommand(async () =>
                       {
                           var completedItems = TodoItems.Where(i => i.IsCompleted);
                           var userInternalId = await GetUserInternalId();
                           bool canUserAdd =
                               await _roleTypeDataService.CanUserAddOrDeleteItem(userInternalId, _associatedTodoList.GroupId);
                           if (canUserAdd)
                           {
                               IsBusy = true;
                               completedItems.ForEach(async i =>
                               {
                                   i.IsDeleted = true;
                                   await _todoItemsDataService.UpdateTodoItem(i);
                                   Refresh();
                               });
                           }
                           else
                           {
                               new MessageDialog(Constants.UserCantAddOrDelete).ShowAsync();
                           }
                       }));
            }
        }

        public RelayCommand<object> CheckboxCheckedCommand
        {
            get
            {
                return _checkboxCheckedCommand ??
                       (_checkboxCheckedCommand = new RelayCommand<object>(async (param) =>
                       {
                           var userInternalId = await GetUserInternalId();

                           TodoItem todoItem = param as TodoItem;
                           if (todoItem != null)
                           {
                               bool canUserUpdate =
                                   await
                                       _roleTypeDataService.CanUserEditItem(userInternalId, _associatedTodoList.GroupId);
                               if (canUserUpdate)
                                   await _todoItemsDataService.UpdateTodoItem(todoItem);
                               else
                                   Refresh();
                           }
                       }));
            }
        }

        public RelayCommand RefreshCommand
        {
            get
            {
                return _refreshCommand ??
                       (_refreshCommand = new RelayCommand(Refresh));
            }
        }

        public ToDoItemsViewModel(ITodoItemsDataService todoItemsDataService, IUserDataService userDataService, IRoleTypeDataService roleTypeDataService)
        {
            _dataService = new DataServices.DataService();
            _todoItemsDataService = todoItemsDataService;
            _userDataService = userDataService;
            _roleTypeDataService = roleTypeDataService;
            Refresh();
        }

        private async void Refresh()
        {
            try
            {
                IsBusy = true;
                if (_associatedTodoList != null)
                {
                    TodoItems = await _todoItemsDataService.GetTodoListsItems(_associatedTodoList.Id);
                    TodoItems = TodoItems.OrderBy(t => t.Text).ToObservableCollection();
                }
            }
            catch (Exception ex)
            {
                new MessageDialog(ex.Message).ShowAsync();
            }
            IsBusy = false;
        }

        private async Task<string> GetUserInternalId()
        {
            if (!String.IsNullOrWhiteSpace(_userInternalId))
                return _userInternalId;
            _userInternalId = await _userDataService.GetUserInternalId(AccountHelper.GetCurrentUserId(), Constants.MainAuthenticationDomain);
            return _userInternalId;
        }


        public void Activate(object parameter)
        {
            _associatedTodoList = parameter as TodoList;
            ListTitle = _associatedTodoList != null ? _associatedTodoList.ListName : String.Empty;
            Refresh();
        }

        public void Deactivate(object parameter)
        {
            if (TodoItems != null)
                TodoItems.Clear();
        }
    }
}
