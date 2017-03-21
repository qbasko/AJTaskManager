using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.Common;
using AJTaskManagerMobile.Model.DTO;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.WindowsAzure.MobileServices;

namespace AJTaskManagerMobile.DataServices
{
    public class TaskSubitemWorkDataService : DataService, ITaskSubitemWorkDataService
    {
        public async Task<System.Collections.ObjectModel.ObservableCollection<Model.DTO.TaskSubitemWork>> GetTaskSubitemWorks(string taskSubitemId)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var workItems =
                    await MobileService.GetTable<TaskSubitemWork>()
                        .Where(w => w.TaskSubitemId == taskSubitemId)
                        .ToCollectionAsync();
                return workItems;
            });
        }

        public async Task<System.Collections.ObjectModel.ObservableCollection<TaskSubitemWork>> GetTaskSubitemWorksWithUsers(string taskSubitemId)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var workItems =
                    await MobileService.GetTable<TaskSubitemWork>()
                        .Where(w => w.TaskSubitemId == taskSubitemId)
                        .ToCollectionAsync();
                var userDataService = SimpleIoc.Default.GetInstance<IUserDataService>();
                var userLoginDict = new Dictionary<string, User>();
                foreach (var item in workItems)
                {
                    if (!userLoginDict.ContainsKey(item.UserId))
                        userLoginDict.Add(item.UserId, await userDataService.GetUserById(item.UserId));
                    item.User = userLoginDict[item.UserId];
                }
                return workItems;
            });
        }

        public async Task<System.Collections.ObjectModel.ObservableCollection<TaskSubitemWork>> GetTaskSubitemWorksInProgress(List<string> groupTaskSubitemIds)
        {
            DateTime dt = DateTime.Now.AddDays(-2);
            var works =
                await MobileService.GetTable<TaskSubitemWork>()
                    .Where(
                        w =>
                            groupTaskSubitemIds.Contains(w.TaskSubitemId) && w.EndDateTime == null &&
                            w.StartDateTime > dt).ToCollectionAsync();

            return works;
        }

        public async Task<bool> Insert(Model.DTO.TaskSubitemWork taskSubitemWork)
        {
            return await ExecuteAuthenticated(async () =>
            {
                await MobileService.GetTable<TaskSubitemWork>().InsertAsync(taskSubitemWork);
                return true;
            });
        }

        public async Task<bool> Update(Model.DTO.TaskSubitemWork taskSubitemWork)
        {
            return await ExecuteAuthenticated(async () =>
            {
                await MobileService.GetTable<TaskSubitemWork>().UpdateAsync(taskSubitemWork);
                return true;
            });
        }

        public async Task<bool> Delete(Model.DTO.TaskSubitemWork taskSubitemWork)
        {
            return await ExecuteAuthenticated(async () =>
            {
                await MobileService.GetTable<TaskSubitemWork>().DeleteAsync(taskSubitemWork);
                return true;
            });
        }


    }
}
