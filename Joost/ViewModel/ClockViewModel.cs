using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Search;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Joost.Helpers;
using Joost.Models;

namespace Joost.ViewModel
{
    public class ClockViewModel : ObservableObject
    {
        private readonly DispatcherTimer _timer;
        private readonly Random _rnd;

        public ClockViewModel()
        {
            _rnd = new Random(Guid.NewGuid().GetHashCode());
            GetDevices();
            GetImages();

            _timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(1)};
            _timer.Tick += (sender, args) =>
            {
                CurrentDateTime = DateTime.Now;
                Counter++;
                var rndPosition = _rnd.Next(0, Images.Count);
                if (rndPosition > 0)
                    ActiveImage = Images.ElementAt(rndPosition);
            };
            _timer.Start();
        }

        protected string DevicesProperty = @"Devices";
        private ObservableCollection<Device> _devices;
        public ObservableCollection<Device> Devices
        {
            get { return _devices; }
            set
            {
                _devices = value;
                RaisePropertyChanged(DevicesProperty);
            }
        }

        protected const string CurrentDateTimeProperty = @"CurrentDateTime";
        private DateTime _currentDateTime;
        public DateTime CurrentDateTime
        {
            get { return _currentDateTime; }
            set
            {
                if (_currentDateTime == value)
                    return;

                _currentDateTime = value;
                RaisePropertyChanged(CurrentDateTimeProperty, @"CurrentTime", @"CurrentDate");
            }
        }

        protected const string CounterProperty = @"Counter";
        private int _counter;
        public int Counter
        {
            get { return _counter; }
            set
            {
                if (_counter == value)
                    return;

                _counter = value;
                RaisePropertyChanged(CounterProperty);
            }
        }

        protected const string ImagesProperty = @"Images";
        private ObservableCollection<BitmapImage> _images;
        public ObservableCollection<BitmapImage> Images
        {
            get { return _images; }
            set
            {
                _images = value;
                RaisePropertyChanged(ImagesProperty);
            }
        }

        protected const string ActiveImageProperty = @"ActiveImage";
        private BitmapImage _activeImage;
        public BitmapImage ActiveImage
        {
            get { return _activeImage; }
            set
            {
                if (_activeImage == value)
                    return;

                _activeImage = value;
                RaisePropertyChanged(ActiveImageProperty);
            }
        }

        public string IsEnabled => _timer.IsEnabled ? @"Timer is runnig" : @"Timer is disabled";
        public string CurrentDate => _currentDateTime.ToString(@"yyyy-MM-dd");
        public string CurrentTime => _currentDateTime.ToString(@"HH:mm:ss");

        private void GetDevices()
        {
            Devices = new ObservableCollection<Device>
            {
                new Device {ImageSource = "/Assets/apple-logo.png", Maker = @"Apple", Model = @"IPhone 6S", HasNewImages = true},
                new Device {ImageSource = "/Assets/apple-logo.png", Maker = @"Apple", Model = @"IPhone 6SE", HasNewImages = false},
                new Device {ImageSource = "/Assets/microsoft-logo.jpg", Maker = @"Microsoft", Model = @"Limua 950XL", HasNewImages = false},
                new Device {ImageSource = "/Assets/android-logo.jpg", Maker = @"Samsung", Model = @"Galaxy S7", HasNewImages = true}
            };
        }

        private async void GetImages()
        {
            Images = new ObservableCollection<BitmapImage>();
            var picker = new FolderPicker() { SuggestedStartLocation = PickerLocationId.PicturesLibrary };

            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            //StorageFolder folder = await picker.PickSingleFolderAsync();
            var picturesFolder = KnownFolders.PicturesLibrary;
            IReadOnlyList<StorageFile> fileList = await picturesFolder.GetFilesAsync();
            var i = new List<BitmapImage>();
            i.AddRange(fileList.Select(file => new BitmapImage(new Uri(file.Path))));
            
            Images = new ObservableCollection<BitmapImage>(i);


            //if (folder != null)
            //{
            //    var files = await folder.GetFileAsync();
            //    //var files = Directory.GetFiles(@"C:\Users\tomas\Pictures\Party\", "*.*", SearchOption.AllDirectories).ToList();
            //    //Images = new ObservableCollection<Image>(files.Select(f => new Image { DeviceIdentifier = f, ImageSource = string.Format(@"/" + f) }));
            //}

        }
    }
}