using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using System.Windows;

public partial class AddSubjectViewModel : ObservableObject
{
    private readonly ISubjectService _subjectService;
    private readonly int _incidentId;

    public Action? OnSuccess;

    [ObservableProperty] private Subject _newSubject;
    [ObservableProperty] private string? _selectedRole;

    public List<string> Roles { get; } = new() {"Потерпевший", "Свидетель", "Заявитель" };

    public AddSubjectViewModel(ISubjectService service, int incidentId)
    {
        _subjectService = service;
        _incidentId = incidentId;

        NewSubject = new Subject();
    }

    [RelayCommand]
    private void Save()
    {
        if (string.IsNullOrWhiteSpace(NewSubject.LastName) || string.IsNullOrWhiteSpace(SelectedRole))
        {
            MessageBox.Show("Заполните фамилию и выберите роль!");
            return;
        }

        try
        {
            var roleLink = new SubjectRole
            {
                IdIncident = _incidentId,
                RoleName = SelectedRole,
                DateOfInvolvement = DateOnly.FromDateTime(DateTime.Today)
            };

            _subjectService.AddSubjectAndLinkToIncident(NewSubject, roleLink);

            MessageBox.Show("Участник успешно зарегистрирован и добавлен к делу.");
            OnSuccess?.Invoke();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка сохранения: {ex.Message}");
        }
    }
}
