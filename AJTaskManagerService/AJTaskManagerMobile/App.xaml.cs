﻿using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using AJTaskManagerMobile.DataServices;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using AJTaskManagerMobile.ViewModel;
using AJTaskManagerMobile.Views;
using AJTaskManagerMobile.Views.Controls;
using Microsoft.VisualBasic;
using Microsoft.WindowsAzure.MobileServices;

namespace AJTaskManagerMobile
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App
    {
        private TransitionCollection _transitions;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            // Configure and register the MVVM Light NavigationService
            var nav = new NavigationService();
            SimpleIoc.Default.Register<INavigationService>(() => nav);
            //nav.Configure(ViewModelLocator.SecondPageKey, typeof(SecondPage));
            nav.Configure(Common.Constants.ToDoListsPageKey, typeof(ToDoListsPage));
            nav.Configure(Common.Constants.ToDoItemsPageKey, typeof(ToDoItemsPage));
            nav.Configure(Common.Constants.GroupsPageKey, typeof(GroupsPage));
            nav.Configure(Common.Constants.GroupUsersPageKey, typeof(GroupUsersPage));
            nav.Configure(Common.Constants.UsersPermissionsPageKey, typeof(UsersPermissionsPage));
            nav.Configure(Common.Constants.TasksPageKey, typeof(TasksListPage));
            nav.Configure(Common.Constants.TaskItemFormKey, typeof(TaskItemForm));
            nav.Configure(Common.Constants.TaskSubitemsPageKey, typeof(TaskSubItemsListPage));
            nav.Configure(Common.Constants.TaskSubitemFormKey, typeof(TaskSubitemForm));
            nav.Configure(Common.Constants.TaskSubitemWorkPageKey, typeof(TaskSubitemWorkPage));
            nav.Configure(Common.Constants.SettingsPageKey, typeof(SettingsPage));
            nav.Configure(Common.Constants.PersonalSettingsPageKey, typeof(PersonalSettingsPage));
            nav.Configure(Common.Constants.GroupsMembershipPageKey, typeof(GroupsMembershipPage));
            nav.Configure(Common.Constants.TasksInProgressPageKey, typeof(TasksInProgressPage));
            nav.Configure(Common.Constants.CalendarWithTaskItemsPage, typeof(CalendarWithTaskItemsPage));
            //nav.Configure(Common.Constants.SchedulerPageKey, typeof(SchedulerPage));
            nav.Configure(Common.Constants.TasksOverduePageKey, typeof(TasksOverdueViewModel));

            // Register the MVVM Light DialogService

            SimpleIoc.Default.Register<IDialogService, DialogService>();

            //var todoListDataService = new TodoListDataService();
            //SimpleIoc.Default.Register<ITodoListDataService>(() => todoListDataService);
            //var todoItemsDataService = new TodoItemsDataService();
            //SimpleIoc.Default.Register<ITodoItemsDataService>(() => todoItemsDataService);

            InitializeComponent();
            Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            var rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    _transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        _transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += RootFrame_FirstNavigated;

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();

          
            

            // Illustrates how to use the Messenger by receiving a message
            // and sending a message back.
            Messenger.Default.Register<NotificationMessageAction<string>>(
                this,
                HandleNotificationMessage);

            // MVVM Light's DispatcherHelper for cross-thread handling.
            DispatcherHelper.Initialize();
        }

        private void HandleNotificationMessage(NotificationMessageAction<string> message)
        {
            // Execute a callback to send a reply to the sender.
            message.Execute("Success! (from App.xaml.cs)");
        }

        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = _transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= RootFrame_FirstNavigated;

            // Enable back button
            HardwareButtons.BackPressed += (s, args) =>
            {
                if (!rootFrame.CanGoBack)
                {
                    return;
                }

                // Allow back navigation using Back button
                args.Handled = true;
                rootFrame.GoBack();
            };
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);

#if WINDOWS_PHONE_APP
            if (args.Kind == ActivationKind.WebAuthenticationBrokerContinuation)
            {

                DataServiceBase.MobileService.LoginComplete(args as WebAuthenticationBrokerContinuationEventArgs);
            }
#endif
        }
    }
}