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
        private readonly int _incidentId;

        public Action? OnSuccess;

        [ObservableProperty]
        private Decision _newDecision;

        [ObservableProperty]
        private CriminalCase _newCriminalCase;

        [ObservableProperty]
        private TerritorialTransfer _newTransfer;

        [ObservableProperty]
        private SystemUser _currentUser;

        [ObservableProperty]
        private string? _selectedType;
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

        public Visibility CriminalCaseVisibility => SelectedType == "возбуждено уголовное дело" ? Visibility.Visible : Visibility.Collapsed;
        public Visibility TransferVisibility => SelectedType == "передано по территориальному признаку" ? Visibility.Visible : Visibility.Collapsed;

        public AddDecisionViewModel(IDecisionService service, int incidentId, SystemUser currentUser)
        {
            _decisionService = service;
            _incidentId = incidentId;
            _currentUser = currentUser;

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
        }

        partial void OnSelectedTypeChanged(string? value)
        {
            NewDecision.DecisionType = value ?? "";
            OnPropertyChanged(nameof(CriminalCaseVisibility));
            OnPropertyChanged(nameof(TransferVisibility));
        }

        [RelayCommand]
        private void Save()
        {
            if (string.IsNullOrWhiteSpace(SelectedType))
            {
                MessageBox.Show("Выберите тип решения!");
                return;
            }

            if (string.IsNullOrWhiteSpace(NewDecision.LegalBasis))
            {
                MessageBox.Show("Укажите правовое основание!");
                return;
            }

            try
            {
                var teamLink = _currentUser.SystemUserResponseTeams.FirstOrDefault();

                if (teamLink == null)
                {
                    MessageBox.Show("Ошибка: Пользователь не привязан ни к одной группе реагирования!");
                    return;
                }

                int teamId = teamLink.IdResponseTeam;

                _decisionService.SaveDecision(NewDecision, NewCriminalCase, NewTransfer, teamId);
                MessageBox.Show("Решение успешно сохранено.");

                OnSuccess?.Invoke();
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message ?? ex.Message;
                MessageBox.Show($"Ошибка при сохранении: {inner}");
            }
        }
    }
}
