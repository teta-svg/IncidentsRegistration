using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using System.Windows;

public partial class AddSubjectViewModel : ObservableObject
{
    private readonly ISubjectService _subjectService;
    private readonly IContentNavigationService _nav;
    private int _incidentId;
    private bool _isEditMode;

    [ObservableProperty]
    private Subject _currentSubject = new();

    [ObservableProperty] 
    private string? _selectedRole;

    [ObservableProperty]
    private string? errorMessage;

    public List<string> Roles { get; } = new() { "Потерпевший", "Свидетель", "Заявитель", "Подозреваемый" };

    public Visibility RoleSelectionVisibility => Visibility.Visible;

    private bool IsEditMode => CurrentSubject.IdSubject > 0;

    public string ButtonText => IsEditMode ? "СОХРАНИТЬ ИЗМЕНЕНИЯ" : "ЗАРЕГИСТРИРОВАТЬ";

    public AddSubjectViewModel(
     ISubjectService service,
     IContentNavigationService nav)
    {
        _subjectService = service;
        _nav = nav;
    }

    public void Initialize(SubjectEditPayloadDTO payload)
    {
        _incidentId = payload.IncidentId;

        _isEditMode = payload.Subject != null;

        CurrentSubject = payload.Subject ?? new Subject();
    }

    [RelayCommand]
    private void Save()
    {
        ErrorMessage = string.Empty;

        if (CurrentSubject == null)
            CurrentSubject = new Subject();

        if (string.IsNullOrWhiteSpace(CurrentSubject.LastName))
        {
            ErrorMessage = "Заполните фамилию!";
            return;
        }

        if (string.IsNullOrWhiteSpace(SelectedRole))
        {
            ErrorMessage = "Выберите роль для участника!";
            return;
        }

        try
        {
            if (_isEditMode)
            {
                _subjectService.UpdateSubjectAndRole(CurrentSubject, _incidentId, SelectedRole);
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
            }

            _nav.GoBack();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Ошибка: {ex.Message}";
        }
    }

    [RelayCommand]
    private void Back()
    {
        _nav.GoBack();
    }
}