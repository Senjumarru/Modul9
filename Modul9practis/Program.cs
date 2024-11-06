using System;
internal class ReportSystem
{
    public interface IReport
    {
        string Generate();
    }

    public class SalesReport : IReport
    {
        public string Generate()
        {
            return "Данные отчета по продажам";
        }
    }

    public class UserReport : IReport
    {
        public string Generate()
        {
            return "Данные отчета по пользователям";
        }
    }

    public abstract class ReportDecorator : IReport
    {
        protected IReport report;

        protected ReportDecorator(IReport report)
        {
            this.report = report;
        }

        public virtual string Generate()
        {
            return report.Generate();
        }
    }

    public class DateFilterDecorator : ReportDecorator
    {
        private readonly string startDate;
        private readonly string endDate;

        public DateFilterDecorator(IReport report, string startDate, string endDate) : base(report)
        {
            this.startDate = startDate;
            this.endDate = endDate;
        }

        public override string Generate()
        {
            return $"{report.Generate()} | Фильтр по дате: {startDate} до {endDate}";
        }
    }

    public class SortingDecorator : ReportDecorator
    {
        private readonly string criteria;

        public SortingDecorator(IReport report, string criteria) : base(report)
        {
            this.criteria = criteria;
        }

        public override string Generate()
        {
            return $"{report.Generate()} | Сортировка по: {criteria}";
        }
    }

    public class CsvExportDecorator : ReportDecorator
    {
        public CsvExportDecorator(IReport report) : base(report) { }

        public override string Generate()
        {
            return $"{report.Generate()} | Экспорт в CSV";
        }
    }

    public class PdfExportDecorator : ReportDecorator
    {
        public PdfExportDecorator(IReport report) : base(report) { }

        public override string Generate()
        {
            return $"{report.Generate()} | Экспорт в PDF";
        }
    }

    public static void Main()
    {
        IReport report = new SalesReport();
        report = new DateFilterDecorator(report, "2024-01-01", "2024-12-31");
        report = new SortingDecorator(report, "дата");
        report = new PdfExportDecorator(report);
        Console.WriteLine(report.Generate());
    }
}
