using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace IncidentsRegistration.ViewModels;

public partial class UsersViewModel : ObservableObject
{
    private readonly IUserService _userService;
    public ObservableCollection<SystemUser> Users { get; } = new();

    public Action? OnAddRequested;
    public Action<SystemUser>? OnUpdateRequested;

    [ObservableProperty]
    private SystemUser? selectedUser;

    public UsersViewModel(IUserService userService) => _userService = userService;

    [RelayCommand]
    public void LoadData()
    {
        Users.Clear();
        foreach (var user in _userService.GetAllUsers()) Users.Add(user);
    }

    [RelayCommand]
    private void Add() => OnAddRequested?.Invoke();

    [RelayCommand]
    private void Update()
    {
        if (SelectedUser != null) OnUpdateRequested?.Invoke(SelectedUser);
    }

    [RelayCommand]
    private void Delete()
    {
        if (SelectedUser == null) return;
        if (MessageBox.Show("Удалить пользователя?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
            _userService.DeleteUser(SelectedUser.IdUser);
            LoadData();
        }
    }
}