using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using AJTaskManagerMobile.DataServices;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using AJTaskManagerMobile.Model;
using AJTaskManagerMobile.Views;

namespace AJTaskManagerMobile.ViewModel
{
    /// <summary>
    /// This class contains static references to the most relevant view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// The key used by the NavigationService to go to the second page.
        /// </summary>
        public const string SecondPageKey = "SecondPage";

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }

        public ToDoItemsViewModel ToDoItemsViewModel
        {
            get { return ServiceLocator.Current.GetInstance<ToDoItemsViewModel>(); }
        }

        public ToDoListsViewModel ToDoListsViewModel
        {
            get { return ServiceLocator.Current.GetInstance<ToDoListsViewModel>(); }
        }

        public GroupsViewModel GroupsViewModel
        {
            get { return ServiceLocator.Current.GetInstance<GroupsViewModel>(); }
        }

        public GroupUsersViewModel GroupUsersViewModel
        {
            get { return ServiceLocator.Current.GetInstance<GroupUsersViewModel>(); }
        }

        public UsersPermissionsViewModel UsersPermissionsViewModel
        {
            get { return ServiceLocator.Current.GetInstance<UsersPermissionsViewModel>(); }
        }

        public TasksListViewModel TasksViewModel
        {
            get { return ServiceLocator.Current.GetInstance<TasksListViewModel>(); }
        }

        public TaskItemViewModel TaskItemViewModel
        {
            get { return ServiceLocator.Current.GetInstance<TaskItemViewModel>(); }
        }

        public TaskSubitemsListViewModel TaskSubitemsListViewModel
        {
            get { return ServiceLocator.Current.GetInstance<TaskSubitemsListViewModel>(); }
        }

        public TaskSubitemViewModel TaskSubitemViewModel
        {
            get { return ServiceLocator.Current.GetInstance<TaskSubitemViewModel>(); }
        }

        public TaskSubitemWorkPageViewModel TaskSubitemWorkPageViewModel
        {
            get { return ServiceLocator.Current.GetInstance<TaskSubitemWorkPageViewModel>(); }
        }

        public SettingsViewModel SettingsViewModel
        {
            get { return ServiceLocator.Current.GetInstance<SettingsViewModel>(); }
        }

        public PersonalSettingsViewModel PersonalSettingsViewModel
        {
            get { return ServiceLocator.Current.GetInstance<PersonalSettingsViewModel>(); }
        }

        public GroupsMembershipViewModel GroupsMembershipViewModel
        {
            get { return ServiceLocator.Current.GetInstance<GroupsMembershipViewModel>(); }
        }

        public ActualTasksViewModel ActualTasksViewModel
        {
            get { return ServiceLocator.Current.GetInstance<ActualTasksViewModel>(); }
        }

        public TasksInProgressViewModel TasksInProgressViewModel
        {
            get { return ServiceLocator.Current.GetInstance<TasksInProgressViewModel>(); }
        }

        public CalendarWithTaskItemsViewModel CalendarWithTaskItemsViewModel
        {
            get { return ServiceLocator.Current.GetInstance<CalendarWithTaskItemsViewModel>(); }
        }

        public SchedulerPageViewModel SchedulerPageViewModel
        {
            get { return ServiceLocator.Current.GetInstance<SchedulerPageViewModel>(); }
        }

        public TasksOverdueViewModel TasksOverdueViewModel
        {
            get { return ServiceLocator.Current.GetInstance<TasksOverdueViewModel>(); }
        }

        /// <summary>
        /// This property can be used to force the application to run with design time data.
        /// </summary>
        public static bool UseDesignTimeData
        {
            get
            {
                return false;
            }
        }

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IUserDataService, UserDataService>();
            SimpleIoc.Default.Register<ITodoListDataService, TodoListDataService>();
            SimpleIoc.Default.Register<ITodoItemsDataService, TodoItemsDataService>();
            SimpleIoc.Default.Register<IGroupDataService, GroupDataService>();
            SimpleIoc.Default.Register<IUserGroupDataService, UserGroupDataService>();
            SimpleIoc.Default.Register<IRoleTypeDataService, RoleTypeDataService>();
            SimpleIoc.Default.Register<ITaskItemDataService, TaskItemDataService>();
            SimpleIoc.Default.Register<ITaskSubitemDataService, TaskSubitemDataService>();
            SimpleIoc.Default.Register<ITaskSubitemWorkDataService, TaskSubitemWorkDataService>();
            SimpleIoc.Default.Register<ISettingsService, SettingsService>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ToDoItemsViewModel>();
            SimpleIoc.Default.Register<ToDoListsViewModel>();
            SimpleIoc.Default.Register<GroupsViewModel>();
            SimpleIoc.Default.Register<GroupUsersViewModel>();
            SimpleIoc.Default.Register<UsersPermissionsViewModel>();
            SimpleIoc.Default.Register<TasksListViewModel>();
            SimpleIoc.Default.Register<TaskItemViewModel>();
            SimpleIoc.Default.Register<TaskSubitemViewModel>();
            SimpleIoc.Default.Register<TaskSubitemsListViewModel>();
            SimpleIoc.Default.Register<TaskSubitemWorkPageViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<PersonalSettingsViewModel>();
            SimpleIoc.Default.Register<GroupsMembershipViewModel>();
            SimpleIoc.Default.Register<ActualTasksViewModel>();
            SimpleIoc.Default.Register<TasksInProgressViewModel>();
            SimpleIoc.Default.Register<CalendarWithTaskItemsViewModel>();
            SimpleIoc.Default.Register<SchedulerPageViewModel>();
            SimpleIoc.Default.Register<TasksOverdueViewModel>();
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}