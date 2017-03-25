using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Common;

namespace CortanaCanteenChecker
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly ICanteenService canteenService;

        private ObservableCollection<Canteen> canteens;
        private ICommand searchCanteensCommand;
        private string nameFilter;
        private string dishFilter;

        private bool isLoading;

        public ObservableCollection<Canteen> Canteens
        {
            get
            {
                return this.canteens;
            }
            private set
            {
                this.canteens = value;
                this.NotifyPropertyChanged(nameof(this.Canteens));
            }
        }

        public string NameFilter
        {
            get
            {
                return this.nameFilter;
            }
            set
            {
                this.nameFilter = value;
                this.NotifyPropertyChanged(nameof(this.NameFilter));
            }
        }

        public string DishFilter
        {
            get
            {
                return this.dishFilter;
            }
            set
            {
                this.dishFilter = value;
                this.NotifyPropertyChanged(nameof(this.DishFilter));
            }
        }

        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }
            set
            {
                this.isLoading = value;
                this.NotifyPropertyChanged(nameof(this.IsLoading));
                this.NotifyPropertyChanged(nameof(this.IsNotLoading));
            }
        }

        public bool IsNotLoading
        {
            get
            {
                return !this.IsLoading;
            }
        }

        public ICommand SearchCanteensCommand
        {
            get
            {
                return this.searchCanteensCommand;
            }
        }

        public MainPageViewModel(ICanteenService canteenService)
        {
            if(canteenService == null)
            {
                throw new ArgumentException(nameof(canteenService));
            }

            this.canteenService = canteenService;

            this.Canteens = new ObservableCollection<Canteen>();

            this.searchCanteensCommand = new RelayCommand(async () => {
                await LoadDataAsync();
            });
        }

        public async Task LoadDataAsync()
        {
            this.IsLoading = true;

            this.Canteens.Clear();
            foreach (var c in await this.canteenService.GetCanteens(this.NameFilter, this.DishFilter)) {
                this.Canteens.Add(c);
            }

            this.IsLoading = false;
        }
    }
}
