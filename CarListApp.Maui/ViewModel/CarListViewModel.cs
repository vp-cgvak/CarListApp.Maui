﻿using CarListApp.Maui.Models;
using CarListApp.Maui.Services;
using CarListApp.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarListApp.Maui.ViewModel
{
    public partial class CarListViewModel : BaseViewModel
    {
        const string editButtonText = "Update Car";
        const string createButtonText = "Add Car";
        private readonly CarService carService;
        public ObservableCollection<Car> Cars { get; private set; } = new();

        public CarListViewModel(CarService carService)
        {
            Title = "Car List";
            AddEditButtonText = createButtonText;
            GetCarList().Wait();
        }
        [ObservableProperty]
        bool isRefreshing;
        [ObservableProperty]
        string make;
        [ObservableProperty]
        string model;
        [ObservableProperty]
        string vin;
        [ObservableProperty]
        string addEditButtonText;
        [ObservableProperty]
        int carId;


        [ICommand]

        async Task GetCarList()
        {
            if (IsLoading) return;
            try
            {
                IsLoading = true;
                if (Cars.Any()) Cars.Clear();

                var cars = App.CarService.GetCars();

                foreach (var car in cars) Cars.Add(car);

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get cars :{ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Failed to retrive the list of cars.", "Ok");
            }
            finally
            {
                IsLoading = false;
                IsRefreshing = false;
            }
        }
        [ICommand]
        async Task GetCarDetails(int id)
        {
            if (id == 0) return;
            await Shell.Current.GoToAsync($"{nameof(CarDetailsPage)}?Id={id},true");
        }
        [ICommand]

        async Task SaveCar()
        {
            if (string.IsNullOrEmpty(make) || string.IsNullOrEmpty(model) || string.IsNullOrEmpty(vin))
            {
                await Shell.Current.DisplayAlert("Invalid Data", "Please insert valid data", "Ok");
            }
            var car = new Car
            {
                Make = Make,
                Model = Model,
                Vin = Vin,
            };
            if (CarId != 0)
            {
                car.Id = CarId;
                App.CarService.UpdateCar(car);
                await Shell.Current.DisplayAlert("Info", App.CarService.StatusMessage, "Ok");
            }
            else
            {
                App.CarService.AddCar(car);
                await Shell.Current.DisplayAlert("Info", App.CarService.StatusMessage, "Ok");
            }
            await GetCarList();
            await ClearForm();
        }

        [ICommand]
        async Task DeleteCar(int id)
        {
            if (id == 0)
            {
                await Shell.Current.DisplayAlert("Invalid Record", "Please try again", "Ok");
            }
            var result = App.CarService.DeleteCar(id);
            if (result == 0) await Shell.Current.DisplayAlert("Failed", "Failed to delete", "Ok");
            else
            {
                await Shell.Current.DisplayAlert("Deletion Successful", "Record removed successfully", "Ok");
                await GetCarList();
            }

        }

        [ICommand]
        async Task UpdateCar(int id)
        {
            AddEditButtonText = editButtonText;
            return;
        }

        [ICommand]
        async Task SetEditMode(int id)
        {
            AddEditButtonText = editButtonText;
            CarId = id;
            var car = App.CarService.GetCar(id);
            Make = car.Make;
            Model = car.Model;
            Vin = car.Vin;
        }

        [ICommand]
        async Task ClearForm()
        {
            AddEditButtonText = createButtonText;
            CarId = 0;
            Make = string.Empty;
            Model = string.Empty;
            Vin = string.Empty;
        }
    }
}
