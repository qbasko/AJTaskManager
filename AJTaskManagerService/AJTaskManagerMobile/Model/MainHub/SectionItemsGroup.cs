using System;
using System.Collections.ObjectModel;

namespace AJTaskManagerMobile.Model.MainHub
{
   public class SectionItemsGroup
    {
        public SectionItemsGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            UniqueId = uniqueId;
            Title = title;
            Subtitle = subtitle;
            Description = description;
            ImagePath = imagePath;
            Items = new ObservableCollection<SectionItem>();
        }

        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public string Subtitle { get; private set; }
        public string Description { get; private set; }
        public string ImagePath { get; private set; }
        public ObservableCollection<SectionItem> Items { get; private set; }

        public override string ToString()
        {
            return this.Title;
        }
    }
}
