using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using AJTaskManagerMobile.Common;
using AJTaskManagerMobile.DataServices;
using AJTaskManagerMobile.Helpers;
using AJTaskManagerMobile.Model;
using AJTaskManagerMobile.Model.DTO;
using AJTaskManagerMobile.Views.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Syncfusion.Data.Extensions;
using Syncfusion.Pdf.IO;
using Syncfusion.UI.Xaml.Schedule;

namespace AJTaskManagerMobile.ViewModel
{
    public class ToDoListsViewModel : ViewModelBase, INavigable
    {
        private readonly ITodoListDataService _todoListDataService;
        private readonly IUserDataService _userDataService;
        private readonly IGroupDataService _groupDataService;
        private readonly INavigationService _navigationService;
        private readonly IRoleTypeDataService _roleTypeDataService;
        private ObservableCollection<TodoList> _todoLists;
        private ObservableCollection<Group> _availableGroups;
        private Group _selectedGroup;
        private string _listTitle;
        private bool _isBusy;
        private string _userInternalId;
        private TodoList _editedList;
        private RelayCommand _addNewItemCommand;
        private RelayCommand _saveListCommand;
        private RelayCommand _openAddItemPopupCommand;
        private RelayCommand _deleteCompletedCommand;
        private RelayCommand<object> _navigateToListItemsCommand;
        private RelayCommand<object> _checkboxCheckedCommand;
        private RelayCommand<object> _editTodoListCommand;
        private RelayCommand _refreshCommand;
        private RelayCommand<object> _deleteListCommand;


        public ObservableCollection<TodoList> TodoLists
        {
            get { return _todoLists; }
            set
            {
                if (_todoLists == value)
                    return;
                _todoLists = value;
                RaisePropertyChanged(() => TodoLists);
            }
        }

        public ObservableCollection<Group> AvailableGroups
        {
            get { return _availableGroups; }
            set
            {
                if (_availableGroups == value)
                    return;
                _availableGroups = value;
                RaisePropertyChanged(() => AvailableGroups);
            }
        }

        public Group SelectedGroup
        {
            get { return _selectedGroup; }
            set
            {
                if (_selectedGroup == value)
                    return;
                _selectedGroup = value;
                RaisePropertyChanged(() => SelectedGroup);
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

        private bool _canUserEdit;
        public bool CanUserEdit
        {
            get { return _canUserEdit; }
            set
            {
                if (_canUserEdit == value)
                    return;
                _canUserEdit = value;
                RaisePropertyChanged(() => CanUserEdit);
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

        public RelayCommand AddNewItemCommand
        {
            get
            {
                return _addNewItemCommand ??
                       (_addNewItemCommand = new RelayCommand(async () =>
                       {
                           IsBusy = true;
                       
                           //var userId = AccountHelper.GetCurrentUserId();
                           var internalUserId = await GetUserInternalId(); //await _userDataService.GetUserInternalId(userId, Constants.MainAuthenticationDomain);
                           TodoList todoList = new TodoList()
                           {
                               Id = Guid.NewGuid().ToString(),
                               IsCompleted = false,
                               IsDeleted = false,
                               ListName = ListTitle,
                               GroupId = SelectedGroup != null ? SelectedGroup.Id : ""
                           };
                           bool canUserAdd = await _roleTypeDataService.CanUserAddOrDeleteItem(internalUserId, todoList.GroupId);
                           if (canUserAdd)
                           {
                               await _todoListDataService.InsertTodoList(todoList, internalUserId);
                               Refresh();
                           }
                           else
                           {
                               IsBusy = false;
                               new MessageDialog(Constants.UserCantAddOrDelete).ShowAsync();
                           }
                       }));
            }
        }

        public RelayCommand SaveListCommand
        {
            get
            {
                return _saveListCommand ??
                       (_saveListCommand = new RelayCommand(async () =>
                       {
                           var internalUserId = await GetUserInternalId();
                           bool canUserEdit =
                               await _roleTypeDataService.CanUserEditItem(internalUserId, _editedList.GroupId);
                           if (canUserEdit)
                           {
                               _editedList.ListName = ListTitle;
                               await _todoListDataService.UpdateTodoList(_editedList);
                               Refresh();
                           }
                       }));
            }
        }

        public RelayCommand OpenAddItemPopupCommand
        {
            get
            {
                return _openAddItemPopupCommand ??
                       (_openAddItemPopupCommand = new RelayCommand(() =>
                       {
                           SelectedGroup =
                           AvailableGroups.SingleOrDefault(
                               g => g.GroupName.Contains(Constants.DefaultGroupForUserNamePrefix));

                           Popup popup = new Popup();
                           var addList = new AddTodoList
                           {
                               DataContext = this,
                               MinWidth = 400,
                               Height = 240,
                               HorizontalAlignment = HorizontalAlignment.Stretch,
                               VerticalAlignment = VerticalAlignment.Center,
                               Margin = new Thickness() { Top = 30 }
                           };
                           popup.Child = addList;
                           popup.IsLightDismissEnabled = true;
                           popup.IsOpen = true;
                       }));
            }
        }

        public RelayCommand<object> DeleteListCommand
        {
            get
            {
                return _deleteListCommand ??
                       (_deleteListCommand = new RelayCommand<object>(async (param) =>
                       {
                           IsBusy = true;
                           TodoList list = param as TodoList;
                           if (list != null)
                           {
                               string internalUserId = await GetUserInternalId();
                               bool canUserDelete = await _roleTypeDataService.CanUserAddOrDeleteItem(internalUserId,
                                   list.GroupId);
                               if (canUserDelete)
                               {
                                   list.IsDeleted = true;
                                   await _todoListDataService.UpdateTodoList(list);
                                   Refresh();
                               }
                               else
                               {
                                   IsBusy = false;
                                   new MessageDialog(Constants.UserCantAddOrDelete).ShowAsync();
                               }
                           }
                           IsBusy = false;
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
                    IsBusy = true;
                    //var userId = AccountHelper.GetCurrentUserId();
                    var internalUserId = await GetUserInternalId();//await _userDataService.GetUserInternalId(userId, Constants.MainAuthenticationDomain);
                    var completedLists = TodoLists.Where(l => l.IsCompleted);
                    bool canUserDelete = false;
                    foreach (var completedList in completedLists)
                    {
                        bool result = await _roleTypeDataService.CanUserAddOrDeleteItem(internalUserId, completedList.GroupId);
                        if (!result)
                            return;
                        canUserDelete = true;
                    }
                    completedLists.ForEach(async l =>
                    {
                        if (canUserDelete)
                        {
                            l.IsDeleted = true;
                            await _todoListDataService.UpdateTodoList(l);
                        }
                        else
                        {
                            IsBusy = false;
                            new MessageDialog(Constants.UserCantAddOrDelete).ShowAsync();
                        }
                    });
                    Refresh();
                }));
            }
        }

        public RelayCommand<object> EditTodoListCommand
        {
            get
            {
                return _editTodoListCommand ??
                       (_editTodoListCommand = new RelayCommand<object>(async (obj) =>
                       {
                           _editedList = obj as TodoList;
                           if (_editedList == null) return;
                           var internalUserId = await GetUserInternalId();

                           ListTitle = _editedList.ListName;
                           SelectedGroup = AvailableGroups.Single(g => g.Id == _editedList.GroupId);
                           CanUserEdit =  await _roleTypeDataService.CanUserEditItem(internalUserId, SelectedGroup.Id);
                           Popup popup = new Popup();
                           var addList = new EditTodoList()
                           {
                               DataContext = this,
                               MinWidth = 400,
                               Height = 240,
                               HorizontalAlignment = HorizontalAlignment.Center,
                               VerticalAlignment = VerticalAlignment.Center,
                               Margin = new Thickness() { Top = 30 }
                           };
                           popup.Child = addList;
                           popup.IsLightDismissEnabled = true;
                           popup.IsOpen = true;
                       }));
            }
        }

        public RelayCommand<object> NavigateToListItemsCommand
        {
            get
            {
                return _navigateToListItemsCommand ??
                       (_navigateToListItemsCommand = new RelayCommand<object>((param) =>
                       {
                           _navigationService.NavigateTo(Constants.ToDoItemsPageKey, param);
                       }));
            }
        }

        public RelayCommand<object> CheckboxCheckedCommand
        {
            get
            {
                return _checkboxCheckedCommand ??
                       (_checkboxCheckedCommand = new RelayCommand<object>(async (obj) =>
                       {
                           var userInternalId = await GetUserInternalId();
                           TodoList todoList = obj as TodoList;
                           if (todoList != null)
                           {
                               //IsBusy = true;
                               bool canUserUpdate = await _roleTypeDataService.CanUserEditItem(userInternalId,
                                   todoList.GroupId);
                               if (canUserUpdate)
                                   await _todoListDataService.UpdateTodoList(todoList);
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

        public ToDoListsViewModel(ITodoListDataService todoListDataService, IUserDataService userDataService,
            IGroupDataService groupDataService, INavigationService navigationService, IRoleTypeDataService roleTypeDataService)
        {
            _todoListDataService = todoListDataService;
            _userDataService = userDataService;
            _groupDataService = groupDataService;
            _navigationService = navigationService;
            _roleTypeDataService = roleTypeDataService;

            Refresh();
            Initialize();
        }

        private async void Initialize()
        {
            AvailableGroups = await _groupDataService.GetGroupsAvailableForUser(AccountHelper.GetCurrentUserId());
        }

        private async void Refresh()
        {
            try
            {
                IsBusy = true;
                //var userId = AccountHelper.GetCurrentUserId();
                string userInternalId = await GetUserInternalId(); //await _userDataService.GetUserInternalId(userId, Constants.MainAuthenticationDomain);
                TodoLists = await _todoListDataService.GetTodoListsTable(userInternalId);
                TodoLists = TodoLists.OrderBy(t => t.ListName).ToObservableCollection();
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
            Refresh();
        }

        public void Deactivate(object parameter)
        {

        }
    }
}
