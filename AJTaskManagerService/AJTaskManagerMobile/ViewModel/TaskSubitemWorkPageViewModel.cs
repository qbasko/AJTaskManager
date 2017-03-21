using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.Common;
using AJTaskManagerMobile.DataServices;
using AJTaskManagerMobile.Model.DTO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using Syncfusion.Data.Extensions;
using Syncfusion.UI.Xaml.Controls;

namespace AJTaskManagerMobile.ViewModel
{
    public class TaskSubitemWorkPageViewModel : ViewModelBase, INavigable
    {
        private readonly INavigationService _navigationService;
        private readonly ITaskSubitemWorkDataService _taskSubitemWorkDataService;
        private readonly IUserDataService _userDataService;
        private readonly ITaskSubitemDataService _taskSubitemDataService;
        private string _taskSubitemTitle;
        private bool _runClock;
        private TaskSubitemWork _currentTaskSubitemWork;
        private TaskSubitem _associatedTaskSubitem;
        private RelayCommand _startButtonCommand;
        private RelayCommand _stopButtonCommand;

        private bool _isBusy;
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

        public string TaskSubitemTitle
        {
            get { return _taskSubitemTitle; }
            set
            {
                if (_taskSubitemTitle == value)
                    return;
                _taskSubitemTitle = value;
                RaisePropertyChanged(() => TaskSubitemTitle);
            }
        }

        private string _elapsedTime;
        public string ElapsedTime
        {
            get { return _elapsedTime; }
            set
            {
                Set(ref _elapsedTime, value);

                //if (_elapsedTime == value)
                //    return;
                //_elapsedTime = value;
                //RaisePropertyChanged(() => ElapsedTime);
            }
        }

        private bool _isStartButtonEnabled;
        public bool IsStartButtonEnabled
        {
            get { return _isStartButtonEnabled; }
            set
            {
                if (_isStartButtonEnabled == value)
                    return;
                _isStartButtonEnabled = value;
                RaisePropertyChanged(() => IsStartButtonEnabled);
            }
        }
        private bool _isStopButtonEnabled;
        public bool IsStopButtonEnabled
        {
            get { return _isStopButtonEnabled; }
            set
            {
                if (_isStopButtonEnabled == value)
                    return;
                _isStopButtonEnabled = value;
                RaisePropertyChanged(() => IsStopButtonEnabled);
            }
        }

        private string _totalElapsedTime;
        public string TotalElapsedTime
        {
            get { return _totalElapsedTime; }
            set
            {
                if (_totalElapsedTime == value)
                    return;
                _totalElapsedTime = value;
                RaisePropertyChanged(() => TotalElapsedTime);
            }
        }

        private ObservableCollection<TaskSubitemWork> _taskSubitemWorks;
        public ObservableCollection<TaskSubitemWork> TaskSubitemWorks
        {
            get { return _taskSubitemWorks; }
            set
            {
                if (_taskSubitemWorks == value)
                    return;
                _taskSubitemWorks = value;
                RaisePropertyChanged(() => TaskSubitemWorks);
            }
        }

        public RelayCommand StartButtonCommand
        {
            get
            {
                return _startButtonCommand ?? (_startButtonCommand = new RelayCommand(async () =>
                {
                    _runClock = true;
                    IsStartButtonEnabled = false;
                    IsStopButtonEnabled = true;
                    await InsertEntry();
                    await Task.Run(async () =>
                    {
                        while (_runClock)
                        {
                            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            {
                                ElapsedTime = (DateTime.Now - _currentTaskSubitemWork.StartDateTime).ToString("hh\\:mm\\:ss");
                            });

                            await Task.Delay(1000);
                        }
                    });
                }));
            }
        }

        public RelayCommand StopButtonCommand
        {
            get
            {
                return _stopButtonCommand ?? (_stopButtonCommand = new RelayCommand(async () =>
                {
                    _runClock = false;
                    _currentTaskSubitemWork.EndDateTime = DateTime.Now;
                    await _taskSubitemWorkDataService.Update(_currentTaskSubitemWork);
                    await Refresh();
                    IsStartButtonEnabled = true;
                    IsStopButtonEnabled = false;
                }));
            }
        }

        private async Task InsertEntry()
        {
            _currentTaskSubitemWork = new TaskSubitemWork()
            {
                Id = Guid.NewGuid().ToString(),
                StartDateTime = DateTime.Now,
                EndDateTime = null,
                TaskSubitemId = _associatedTaskSubitem.Id,
                UserId = _associatedTaskSubitem.ExecutorId
            };
            await _taskSubitemWorkDataService.Insert(_currentTaskSubitemWork);
            await Refresh();
            if (_associatedTaskSubitem.TaskStatusId == ((int) TaskStatusEnum.NotStarted).ToString())
            {
                _associatedTaskSubitem.TaskStatusId = ((int)TaskStatusEnum.InProgress).ToString();
                await _taskSubitemDataService.UpdateTaskSubitem(_associatedTaskSubitem);
            }
        }

        public TaskSubitemWorkPageViewModel(INavigationService navigationService, ITaskSubitemWorkDataService taskSubitemWorkDataService, IUserDataService userDataService, ITaskSubitemDataService taskSubitemDataService)
        {
            _navigationService = navigationService;
            _taskSubitemWorkDataService = taskSubitemWorkDataService;
            _userDataService = userDataService;
            _taskSubitemDataService = taskSubitemDataService;
            IsStartButtonEnabled = IsStopButtonEnabled = false;
        }

        private async Task Refresh()
        {
            IsBusy = true;
            if (_associatedTaskSubitem != null)
            {
                TaskSubitemWorks = await _taskSubitemWorkDataService.GetTaskSubitemWorksWithUsers(_associatedTaskSubitem.Id);

                TaskSubitemTitle = _associatedTaskSubitem.Name;
                TimeSpan totalTimeSpan = new TimeSpan();
                TaskSubitemWorks.Where(w => w.EndDateTime.HasValue).ForEach(w =>
                {
                    totalTimeSpan += w.EndDateTime.Value - w.StartDateTime;
                });
                TotalElapsedTime = totalTimeSpan.ToString("hh\\:mm\\:ss");
            }
            IsBusy = false;
        }

        public async void Activate(object parameter)
        {
            IsStartButtonEnabled = IsStopButtonEnabled = false;
            _associatedTaskSubitem = parameter as TaskSubitem;
            Refresh();
            if (_associatedTaskSubitem != null)
            {
                string userExtId = Helpers.AccountHelper.GetCurrentUserId();
                string userInternalId =
                    await _userDataService.GetUserInternalId(userExtId, Constants.MainAuthenticationDomain);
                if (_associatedTaskSubitem.ExecutorId == userInternalId)
                    IsStartButtonEnabled = true;
                else
                    IsStartButtonEnabled = false;
            }
        }

        public void Deactivate(object parameter)
        {
            if (_runClock)
                StopButtonCommand.Execute(null);
        }
    }
}
