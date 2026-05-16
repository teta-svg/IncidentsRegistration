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
    private int? _currentRoleId;
    private bool _isEditMode;
    private bool _isAddMode;

    [ObservableProperty]
    private Subject _currentSubject = new();

    [ObservableProperty] 
    private string? _selectedRole;

    [ObservableProperty]
    private string? errorMessage;

    public List<string> Roles { get; } = new() { "Потерпевший", "Свидетель", "Заявитель", "Подозреваемый" };

    public Visibility RoleSelectionVisibility => Visibility.Visible;

    public bool IsEditMode => _isEditMode;

    public bool IsAddMode => _isAddMode;

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
        _isAddMode = payload.Subject == null;
        CurrentSubject = payload.Subject ?? new Subject();

        if (_isEditMode && payload.CurrentRole != null)
        {
            SelectedRole = payload.CurrentRole.RoleName;
            _currentRoleId = payload.CurrentRole.IdSubjectRole;
        }
        else
        {
            SelectedRole = null;
        }
    }

    [RelayCommand]
    private void Save()
    {
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(CurrentSubject?.LastName))
        {
            ErrorMessage = "Заполните фамилию!";
            return;
        }

        if (string.IsNullOrWhiteSpace(SelectedRole))
        {
            ErrorMessage = "Выберите роль для участника!";
            return;
        }

        if (CurrentSubject?.NumberOfConvictions <= 0)
        {
            ErrorMessage = "Количество судимостей должно быть положительным числом";
            return;
        }

        if (CurrentSubject?.Inn.Length != 12)
        {
            ErrorMessage = "ИНН должен содержать 12 цифр";
            return;
        }

        try
        {
            if (_isEditMode)
            {
                _subjectService.UpdateSubject(CurrentSubject);
            }
            else
            {
                var existing = _subjectService.FindExistingSubjectByInn(CurrentSubject.Inn);

                if (existing != null)
                {
                    CurrentSubject.IdSubject = existing.IdSubject;
                }
                else
                {
                    _subjectService.AddSubject(CurrentSubject);
                }
            }

            var lastRole = _subjectService.GetLastRole(CurrentSubject.IdSubject, _incidentId);

            if (lastRole == null || lastRole.RoleName != SelectedRole)
            {
                _subjectService.AddSubjectRole(
                    CurrentSubject.IdSubject,
                    _incidentId,
                    SelectedRole);
            }

            _nav.GoBack();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Ошибка: {ex.Message}";
        }
    }

    [RelayCommand] private void Back() => _nav.GoBack();

    public void TryLoadByInn()
    {
        if (string.IsNullOrWhiteSpace(CurrentSubject.Inn))
            return;

        var existing = _subjectService.FindExistingSubjectByInn(CurrentSubject.Inn);

        if (existing != null)
        {
            FillSubject(existing);
        }
    }

    private void FillSubject(Subject src)
    {
        CurrentSubject = new Subject
        {
            IdSubject = src.IdSubject,
            LastName = src.LastName,
            FirstName = src.FirstName,
            Patronymic = src.Patronymic,
            NumberOfConvictions = src.NumberOfConvictions,
            Settlement = src.Settlement,
            Street = src.Street,
            House = src.House,
            Room = src.Room,
            Inn = src.Inn
        };
    }
}