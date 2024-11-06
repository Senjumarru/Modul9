using System;

internal class LogisticsSystem
{
    public interface IInternalDeliveryService
    {
        void DeliverOrder(string orderId);
        string GetDeliveryStatus(string orderId);
    }

    public class InternalDeliveryService : IInternalDeliveryService
    {
        public void DeliverOrder(string orderId)
        {
            Console.WriteLine($"Заказ {orderId} доставлен внутренней службой.");
        }

        public string GetDeliveryStatus(string orderId)
        {
            return "Доставлено внутренней службой";
        }
    }

    public class ExternalLogisticsServiceA
    {
        public void ShipItem(int itemId)
        {
            Console.WriteLine($"Отправка товара {itemId} через внешнюю службу A.");
        }

        public string TrackShipment(int shipmentId)
        {
            return $"Отслеживание отправления {shipmentId} через внешнюю службу A";
        }
    }

    public class ExternalLogisticsServiceB
    {
        public void SendPackage(string packageInfo)
        {
            Console.WriteLine($"Отправка посылки {packageInfo} через внешнюю службу B.");
        }

        public string CheckPackageStatus(string trackingCode)
        {
            return $"Отслеживание посылки {trackingCode} через внешнюю службу B";
        }
    }

    public class LogisticsAdapterA : IInternalDeliveryService
    {
        private readonly ExternalLogisticsServiceA externalService;

        public LogisticsAdapterA(ExternalLogisticsServiceA service)
        {
            externalService = service;
        }

        public void DeliverOrder(string orderId)
        {
            int itemId = int.Parse(orderId);
            externalService.ShipItem(itemId);
        }

        public string GetDeliveryStatus(string orderId)
        {
            int shipmentId = int.Parse(orderId);
            return externalService.TrackShipment(shipmentId);
        }
    }

    public class LogisticsAdapterB : IInternalDeliveryService
    {
        private readonly ExternalLogisticsServiceB externalService;

        public LogisticsAdapterB(ExternalLogisticsServiceB service)
        {
            externalService = service;
        }

        public void DeliverOrder(string orderId)
        {
            externalService.SendPackage(orderId);
        }

        public string GetDeliveryStatus(string orderId)
        {
            return externalService.CheckPackageStatus(orderId);
        }
    }

    public class DeliveryServiceFactory
    {
        public static IInternalDeliveryService GetDeliveryService(string serviceType)
        {
            return serviceType switch
            {
                "Internal" => new InternalDeliveryService(),
                "ExternalA" => new LogisticsAdapterA(new ExternalLogisticsServiceA()),
                "ExternalB" => new LogisticsAdapterB(new ExternalLogisticsServiceB()),
                _ => null
            };
        }
    }

    public static void Main()
    {
        IInternalDeliveryService service = DeliveryServiceFactory.GetDeliveryService("ExternalA");
        service.DeliverOrder("12345");
        Console.WriteLine(service.GetDeliveryStatus("12345"));
    }
}
