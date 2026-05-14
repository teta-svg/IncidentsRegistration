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

                string[] headers =
                {
                    "Номер",
                    "Дата/Время",
                    "Тип",
                    "Группа",
                    "Адреса",
                    "Участники",
                    "Решение и УД"
                };

                for (int i = 0; i < headers.Length; i++)
                {
                    var cell = ws.Cell(1, i + 1);

                    cell.Value = headers[i];

                    cell.Style.Font.Bold = true;
                    cell.Style.Font.FontColor = XLColor.White;
                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#B5D5CA");

                    cell.Style.Alignment.Horizontal =
                        XLAlignmentHorizontalValues.Center;

                    cell.Style.Alignment.Vertical =
                        XLAlignmentVerticalValues.Center;

                    cell.Style.Alignment.WrapText = true;
                }

                int row = 2;

                foreach (var incident in incidents)
                {
                    ws.Cell(row, 1).Value = incident.IdIncident;

                    ws.Cell(row, 2).Value =
                        $"{incident.RegistrationDate} {incident.RegistrationTime}";

                    ws.Cell(row, 3).Value =
                        incident.TypeOfIncident;

                    ws.Cell(row, 4).Value =
                        incident.IdResponseTeamNavigation?.NameTeam ?? "—";

                    var addressStrings = incident.IncidentLocations.Select(il =>
                    {
                        var l = il.IdLocationNavigation;

                        if (l == null)
                            return "[Адрес не указан]";

                        return
                            $"{l.Region}, {l.Settlement}, {l.Street} {l.House} {l.Room}"
                            .Trim();
                    });

                    ws.Cell(row, 5).Value =
                        string.Join(Environment.NewLine, addressStrings);

                    var subjectStrings = incident.SubjectRoles
                        .GroupBy(sr => sr.IdSubject)
                        .Select(g => g
                            .OrderByDescending(sr => sr.DateOfInvolvement)
                            .ThenByDescending(sr => sr.IdSubjectRole)
                            .First())
                        .Select(sr =>
                        {
                            var s = sr.IdSubjectNavigation;

                            if (s == null)
                                return $"[Данные отсутствуют] — ({sr.RoleName})";

                            string fio =
                                $"{s.LastName} {s.FirstName} {s.Patronymic}"
                                .Trim();

                            return $"{fio} — ({sr.RoleName})";
                        });

                    ws.Cell(row, 6).Value =
                        string.Join(Environment.NewLine, subjectStrings);

                    string decisionDetails = "Нет решения";

                    if (incident.Decision != null)
                    {
                        decisionDetails =
                            $"Решение: {incident.Decision.DecisionType}";

                        if (incident.Decision.CriminalCase != null)
                        {
                            decisionDetails +=
                                $"{Environment.NewLine}УД №{incident.Decision.CriminalCase.СaseNumber} " +
                                $"({incident.Decision.CriminalCase.Qualification})";
                        }

                        if (incident.Decision.TerritorialTransfer != null)
                        {
                            decisionDetails +=
                                $"{Environment.NewLine}Передано в: " +
                                $"{incident.Decision.TerritorialTransfer.Division}";
                        }
                    }

                    ws.Cell(row, 7).Value = decisionDetails;

                    row++;
                }

                var fullRange = ws.Range(1, 1, row - 1, 7);

                fullRange.Style.Border.TopBorder =
                    XLBorderStyleValues.Thin;

                fullRange.Style.Border.BottomBorder =
                    XLBorderStyleValues.Thin;

                fullRange.Style.Border.LeftBorder =
                    XLBorderStyleValues.Thin;

                fullRange.Style.Border.RightBorder =
                    XLBorderStyleValues.Thin;

                fullRange.Style.Border.InsideBorder =
                    XLBorderStyleValues.Thin;

                fullRange.Style.Border.OutsideBorder =
                    XLBorderStyleValues.Thin;

                fullRange.Style.Alignment.Vertical =
                    XLAlignmentVerticalValues.Top;

                fullRange.Style.Alignment.WrapText = true;

                ws.Column(1).Width = 10;
                ws.Column(2).Width = 24;
                ws.Column(3).Width = 28;
                ws.Column(4).Width = 24;
                ws.Column(5).Width = 40;
                ws.Column(6).Width = 45;
                ws.Column(7).Width = 50;

                ws.Rows().AdjustToContents();

                ws.SheetView.FreezeRows(1);

                fullRange.SetAutoFilter();

                ws.Column(1).Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Center;

                ws.Column(2).Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Center;

                workbook.SaveAs(filePath);
            }
        }
    }
}