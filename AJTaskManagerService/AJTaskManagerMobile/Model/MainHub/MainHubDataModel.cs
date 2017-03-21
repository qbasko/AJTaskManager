using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Syncfusion.UI.Xaml.Controls;

namespace AJTaskManagerMobile.Model.MainHub
{
    class MainHubDataModel
    {
        private static MainHubDataModel _mainHubDataModel = new MainHubDataModel();
        private List<SectionItemsGroup> _groups = new List<SectionItemsGroup>();

        public List<SectionItemsGroup> Groups
        {
            get { return _groups; }
        }

        public static async Task<IEnumerable<SectionItemsGroup>> GetGroupsAsync()
        {
            await _mainHubDataModel.GetGroupsDataAsync();
            return _mainHubDataModel.Groups;
        }

        public static async Task<SectionItemsGroup> GetGroupAsync(string uniqueId)
        {
            await _mainHubDataModel.GetGroupsDataAsync();
            var matches = _mainHubDataModel.Groups.Where(group => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static async Task<SectionItem> GetItemAsync(string uniqueId)
        {
            await _mainHubDataModel.GetGroupsDataAsync();
            var matches =
                _mainHubDataModel.Groups.SelectMany(group => group.Items).Where(item => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        private async Task GetGroupsDataAsync()
        {
            if (this._groups.Count != 0)
                return;
            string jsonText = MainHubData.Data;
            var jsonObj = JObject.Parse(jsonText);
            _groups = jsonObj["Groups"].Children().Select((child => child.ToObject<SectionItemsGroup>())).ToList();
        }
    }
}
