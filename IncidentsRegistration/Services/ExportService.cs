using ClosedXML.Excel;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;

namespace IncidentsRegistration.Services
{
    public class ExportService : IExportService
    {
        public void ExportAllIncidentsToExcel(List<Incident> incidents, string filePath)
        {

            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("Отчет по инцидентам");

                string[] headers = { "ID", "Дата/Время", "Тип", "Группа", "Адреса", "Участники (ФИО - Роль)", "Решение и УД" };
                for (int i = 0; i < headers.Length; i++)
                {
                    var cell = ws.Cell(1, i + 1);
                    cell.Value = headers[i];
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#2F5597");
                    cell.Style.Font.FontColor = XLColor.White;
                }

                int row = 2;
                foreach (var incident in incidents)
                {
                    ws.Cell(row, 1).Value = incident.IdIncident;
                    ws.Cell(row, 2).Value = $"{incident.RegistrationDate} {incident.RegistrationTime}";
                    ws.Cell(row, 3).Value = incident.TypeOfIncident;
                    ws.Cell(row, 4).Value = incident.IdResponseTeamNavigation?.NameTeam ?? "—";

                    var addressStrings = incident.IncidentLocations.Select(il =>
                    {
                        var l = il.IdLocationNavigation;
                        if (l == null) return "[Адрес не указан]";
                        return $"{l.Region}, {l.Settlement}, {l.Street} {l.House} {l.Room}".Trim();
                    });

                    ws.Cell(row, 5).Value = string.Join(Environment.NewLine, addressStrings);

                    var subjectStrings = incident.SubjectRoles.Select(sr =>
                    {
                        var s = sr.IdSubjectNavigation;

                        if (s == null) return $"[Данные отсутствуют] — ({sr.RoleName})";

                        string fio = $"{s.LastName} {s.FirstName} {s.Patronymic}".Trim();
                        return $"{fio} — ({sr.RoleName})";
                    });

                    ws.Cell(row, 6).Value = string.Join(Environment.NewLine, subjectStrings);

                    string decisionDetails = "Нет решения";
                    if (incident.Decision != null)
                    {
                        decisionDetails = $"Решение: {incident.Decision.DecisionType}";
                        if (incident.Decision.CriminalCase != null)
                            decisionDetails += $"{Environment.NewLine}УД №{incident.Decision.CriminalCase.СaseNumber} ({incident.Decision.CriminalCase.Qualification})";
                        if (incident.Decision.TerritorialTransfer != null)
                            decisionDetails += $"{Environment.NewLine}Передано в: {incident.Decision.TerritorialTransfer.Division}";
                    }
                    ws.Cell(row, 7).Value = decisionDetails;

                    var range = ws.Range(row, 1, row, 7);
                    range.Style.Alignment.WrapText = true;
                    range.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    row++;
                }

                ws.Columns().AdjustToContents();
                ws.Column(5).Width = 35;
                ws.Column(6).Width = 40;
                ws.Column(7).Width = 45;
                ws.SheetView.FreezeRows(1);

                workbook.SaveAs(filePath);
            }
        }
    }
}
