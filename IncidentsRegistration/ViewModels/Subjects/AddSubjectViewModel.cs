using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;

namespace IncidentsRegistration.ViewModels
{
    public partial class AddSubjectViewModel : ObservableValidator
    {
        private readonly ISubjectService _subjectService;
        private int _incidentId;
        private bool _isEditMode;

        public Action? OnSuccess;

        [ObservableProperty] 
        private Subject _currentSubject = new();

        [ObservableProperty] 
        private string? _selectedRole;

        [ObservableProperty] 
        private string? _errorMessage;

        [ObservableProperty]
        private int? _convictionsCount;

        public List<string> Roles { get; } = new() 
        { 
            "Потерпевший", 
            "Свидетель", 
            "Заявитель", 
            "Подозреваемый" };
        public string ButtonText => _isEditMode ? "СОХРАНИТЬ ИЗМЕНЕНИЯ" : "ЗАРЕГИСТРИРОВАТЬ";

        public AddSubjectViewModel(ISubjectService service)
        {
            _subjectService = service;
        }

        public void Initialize(int incidentId, Subject? subjectToEdit = null)
        {
            _incidentId = incidentId;
            ErrorMessage = string.Empty;

            if (subjectToEdit != null)
            {
                CurrentSubject = subjectToEdit;
                _isEditMode = true;
                SelectedRole = "Подозреваемый";
                ConvictionsCount = CurrentSubject.NumberOfConvictions;
            }
            else
            {
                CurrentSubject = new Subject();
                _isEditMode = false;
                ConvictionsCount = 0;
            }
        }

        [RelayCommand]
        private void Save()
        {
            ErrorMessage = string.Empty;

            if (ConvictionsCount < 0)
            {
                ErrorMessage = "Количество судимостей не может быть отрицательным";
                return;
            }

            if (string.IsNullOrWhiteSpace(CurrentSubject.LastName))
            {
                ErrorMessage = "Укажите фамилию участника";
                return;
            }

            if (string.IsNullOrWhiteSpace(CurrentSubject.FirstName))
            {
                ErrorMessage = "Укажите имя участника";
                return;
            }

            if (string.IsNullOrWhiteSpace(SelectedRole))
            {
                ErrorMessage = "Выберите роль (Потерпевший, Свидетель и т.д.)";
                return;
            }

            try
            {
                CurrentSubject.NumberOfConvictions = ConvictionsCount;

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

                OnSuccess?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerException?.Message ?? ex.Message;
            }
        }
    }
}