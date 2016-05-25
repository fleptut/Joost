using Joost.Helpers;

namespace Joost.Models
{
    public class Device : ObservableObject
    {
        private const string CopyingImages = @"Copying new images";
        private const string NoNewImags = @"No new images";
        public string Identifier { get; set; }
        public string Maker { get; set; }
        public string Model { get; set; }
        public string ImageSource { get; set; }

        protected const string ImageStatusProperty = @"HasNewImages";
        private bool _hasNewImages;
        public bool HasNewImages
        {
            get { return _hasNewImages; }
            set
            {
                if (_hasNewImages == value)
                    return;

                _hasNewImages = value;
                RaisePropertyChanged(ImageStatusProperty, ImageStatusTextProperty);
            }
        }

        protected const string ImageStatusTextProperty = @"ImageStatusText";
        public string ImageStatusText => _hasNewImages ? CopyingImages : NoNewImags;

    }
}
