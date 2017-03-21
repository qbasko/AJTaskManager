using System;

namespace AJTaskManagerMobile.Model
{
    public interface IDataService
    {
        void GetData(Action<DataItem, Exception> callback);
    }
}