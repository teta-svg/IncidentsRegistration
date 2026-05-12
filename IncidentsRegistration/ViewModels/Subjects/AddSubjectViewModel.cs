using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using System.Windows;

public partial class AddSubjectViewModel : ObservableObject
{
    private readonly ISubjectService _subjectService;
    private readonly int _incidentId;
    private readonly bool _isEditMode;

    public Action? OnSuccess;

    [ObservableProperty] private Subject _currentSubject;
    [ObservableProperty] private string? _selectedRole;

    public List<string> Roles { get; } = new() { "Потерпевший", "Свидетель", "Заявитель", "Подозреваемый" };

    public Visibility RoleSelectionVisibility => Visibility.Visible;

    public string ButtonText => _isEditMode ? "СОХРАНИТЬ ИЗМЕНЕНИЯ" : "ЗАРЕГИСТРИРОВАТЬ";

    public AddSubjectViewModel(ISubjectService service, int incidentId, Subject? subjectToEdit = null)
    {
        _subjectService = service;
        _incidentId = incidentId;

        if (subjectToEdit != null)
        {
            _currentSubject = subjectToEdit;
            _isEditMode = true;
            SelectedRole = "Подозреваемый";
        }
        else
        {
            _currentSubject = new Subject();
            _isEditMode = false;
        }
    }

    [RelayCommand]
    private void Save()
    {
        if (string.IsNullOrWhiteSpace(CurrentSubject.LastName))
        {
            MessageBox.Show("Заполните фамилию!");
            return;
        }

        if (string.IsNullOrWhiteSpace(SelectedRole))
        {
            MessageBox.Show("Выберите роль для участника!");
            return;
        }

        try
        {
            if (_isEditMode)
            {
                _subjectService.UpdateSubjectAndRole(CurrentSubject, _incidentId, SelectedRole);
                MessageBox.Show("Данные обновлены, роль успешно добавлена/проверена.");
            }
            else
            {
                var roleLink = new SubjectRole
                {
                    IdIncident = _incidentId,
                    RoleName = SelectedRole,
                    DateOfInvolvement = DateOnly.FromDateTime(DateTime.Today)
                };
                _subjectService.AddSubjectAndLinkToIncident(CurrentSubject, roleLink);
                MessageBox.Show("Участник успешно зарегистрирован.");
            }

            OnSuccess?.Invoke();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}");
        }
    }
}