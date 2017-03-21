using System;

namespace AJTaskManagerMobile.Model.MainHub
{
    public class SectionItem
    {
        public SectionItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content)
        {
            UniqueId = uniqueId;
            Title = title;
            Subtitle = subtitle;
            Description = description;
            ImagePath = imagePath;
            Content = content;
        }

        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public string Subtitle { get; private set; }
        public string Description { get; private set; }
        public string ImagePath { get; private set; }
        public string Content { get; private set; }
    }
}
