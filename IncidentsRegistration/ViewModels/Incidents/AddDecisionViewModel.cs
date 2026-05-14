using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using System.Windows;

namespace IncidentsRegistration.ViewModels
{
    public partial class AddDecisionViewModel : ObservableObject
    {
        private readonly IDecisionService _decisionService;
        private readonly IContentNavigationService _nav;

        private int _incidentId;

        [ObservableProperty]
        private Decision _newDecision = new();

        [ObservableProperty]
        private CriminalCase _newCriminalCase = new();

        [ObservableProperty]
        private TerritorialTransfer _newTransfer = new();

        [ObservableProperty]
        private SystemUser _currentUser;

        [ObservableProperty]
        private string? _errorMessage;

        [ObservableProperty]
        private string? _selectedType;

        public AddDecisionViewModel(IDecisionService service, IContentNavigationService nav)
        {
            _decisionService = service;
            _nav = nav;
        }

        public void Initialize(DecisionInitDTO model)
        {
            _incidentId = model.IncidentId;
            CurrentUser = model.User;

            NewDecision = new Decision
            {
                IdIncident = _incidentId,
                DateOfAdoption = DateOnly.FromDateTime(DateTime.Today)
            };

            NewCriminalCase = new CriminalCase 
            { 
                DateOfInitiation = DateOnly.FromDateTime(DateTime.Today) 
            };
            NewTransfer = new TerritorialTransfer 
            { 
                DateOfTransfer = DateOnly.FromDateTime(DateTime.Today) 
            };

            ErrorMessage = string.Empty;
        }

        public DateTime UI_DecisionDate 
        { 
            get => NewDecision.DateOfAdoption.ToDateTime(TimeOnly.MinValue); 
            set => NewDecision.DateOfAdoption = DateOnly.FromDateTime(value); 
        }
        public DateTime UI_CriminalCaseDate 
        { 
            get => NewCriminalCase.DateOfInitiation.ToDateTime(TimeOnly.MinValue); 
            set => NewCriminalCase.DateOfInitiation = DateOnly.FromDateTime(value); 
        }
        public DateTime UI_TransferDate 
        { 
            get => NewTransfer.DateOfTransfer.ToDateTime(TimeOnly.MinValue); 
            set => NewTransfer.DateOfTransfer = DateOnly.FromDateTime(value); 
        }

        public List<string> DecisionTypes { get; } = new() 
        { 
            "отказано", 
            "возбуждено уголовное дело", 
            "передано по территориальному признаку" 
        };

        public Visibility CriminalCaseVisibility 
            => SelectedType == "возбуждено уголовное дело" ? Visibility.Visible : Visibility.Collapsed;
        public Visibility TransferVisibility 
            => SelectedType == "передано по территориальному признаку" ? Visibility.Visible : Visibility.Collapsed;

        partial void OnSelectedTypeChanged(string? value)
        {
            NewDecision.DecisionType = value ?? "";
            OnPropertyChanged(nameof(CriminalCaseVisibility));
            OnPropertyChanged(nameof(TransferVisibility));
        }
        [RelayCommand]
        private void Save()
        {
            ErrorMessage = string.Empty;

            try
            {
                var teamLink = CurrentUser?.SystemUserResponseTeams.FirstOrDefault();
                if (teamLink == null)
                {
                    ErrorMessage = "Ошибка: Вы не привязаны к группе реагирования!";
                    return;
                }

                if (string.IsNullOrWhiteSpace(SelectedType))
                {
                    ErrorMessage = "Выберите тип решения из списка";
                    return;
                }

                if (string.IsNullOrWhiteSpace(NewDecision.LegalBasis))
                {
                    ErrorMessage = "Опишите принятое решение (Основание)";
                    return;
                }

                if (SelectedType == "возбуждено уголовное дело")
                {
                    if (string.IsNullOrWhiteSpace(NewCriminalCase.Qualification))
                    {
                        ErrorMessage = "Укажите квалификацию";
                        return;
                    }
                }
                else if (SelectedType == "передано по территориальному признаку")
                {
                    if (string.IsNullOrWhiteSpace(NewTransfer.Division))
                    {
                        ErrorMessage = "Введите подразделение";
                        return;
                    }
                }

                _decisionService.SaveDecision(
                    NewDecision,
                    NewCriminalCase,
                    NewTransfer,
                    teamLink.IdResponseTeam);

                ErrorMessage = "Решение успешно сохранено!";

                _nav.GoBack();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Ошибка БД: " + (ex.InnerException?.Message ?? ex.Message);
            }
        }

        [RelayCommand]
        private void Back()
        {
            _nav.GoBack();
        }
    }
}