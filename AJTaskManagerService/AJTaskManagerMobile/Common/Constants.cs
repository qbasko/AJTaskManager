using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AJTaskManagerMobile.Common
{
    internal class Constants
    {
        public const string UserId = "USER_ID";
        public const string UserToken = "USER_TOKEN";
        public const string UserEmail = "USER_EMAIL";
        public const string UserFirstName = "USER_FIRSTNAME";
        public const string UserLastName = "USER_LAST_NAME";

        public const string ToDoItemsPageKey = "ToDoItemsPage";
        public const string ToDoListsPageKey = "ToDoListsPage";
        public const string GroupsPageKey = "GroupsPage";
        public const string GroupUsersPageKey = "GroupUsersPage";
        public const string UsersPermissionsPageKey = "UsersPermissionsPageKey";
        public const string TasksPageKey = "TasksPage";
        public const string TaskSubitemsPageKey = "TaskSubitemsPage";
        public const string TaskItemFormKey = "TaskItemForm";
        public const string TaskSubitemFormKey = "TaskSubitemForm";
        public const string TaskSubitemWorkPageKey = "TaskSubitemWorkPage";
        public const string SettingsPageKey = "SettingsPage";
        public const string GroupsMembershipPageKey = "GroupsMemberhipPage";
        public const string PersonalSettingsPageKey = "PersonalSettingsPage";
        public const string ActualTasksPageKey = "ActualTasksPage";
        public const string TasksInProgressPageKey = "TasksInProgressPage";
        public const string CalendarWithTaskItemsPage = "CalendarWithTaskItemsPage";
        public const string SchedulerPageKey = "SchedulerPage";
        public const string TasksOverduePageKey = "TasksOverduePage";
        public const string LogoutActionKey = "Logout";
        public const string UserIdMsPrefix = "MicrosoftAccount:";
        public const UserDomainsEnum MainAuthenticationDomain = UserDomainsEnum.Microsoft;

        public const string DefaultGroupForUserNamePrefix = "Default group for user:";
        public const string UserWithEmailDoesntExist = "User with this email does not exist.";
        public const string UserWithEmailAlreadyExists= "User with this email already exists.";
        public const string DefaultGroupCantBeDeleted = "Default group cannot be deleted!";
        public const string WelcomeMessage = "Hello {0} {1} ({2})!";

        public const string UserCantAdd = "To add items Admin role is required";
        public const string UserCantAddOrDelete = "To execute this action Admin role is required";
        public const string UserCantEdit = "To execute this action Editor role is required";
        public const string CantDeleteDefaultGroup = "You can't delete your default group";

        public const string TaskSubitemAlreadyStartedTitle = "Task: {0}";
        public const string TaskSubitemAlreadyStartedDesc = "has been started: {0}";

        public const string TaskSubitemNearDeadlineTitle = "Task: {0}";
        public const string TaskSubitemNearDeadlineDesc = "is close to deadline: {0}";
       


        public const string PersonalInformationPageTitle = "personal informations";
        public const string GroupsMembershipsPageTitle = "group memberships";
    }
}
