using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public partial class AppConfig
{
    public static string eventHubConnectionString = "{event hub connection string}";
    public static string eventHubName = "{event hub name}";
    public static string storageAccountName = "{storage account name}";
    public static string storageAccountKey = "{storage account key}";
    public static string consumerGroupName = "{consumer group name}";
    public static string storageConnectionString = "{connection string to storage}";
}


// You can update the config values above, or you can create a new file called AppConfig.local.cs
// this local file will be ignored by GIT and not be uploaded to the repository or overwritten when you pull updates

//Examples of value formats expected

//public partial class AppConfig
//{
//    static AppConfig()
//    {
//        AppConfig.eventHubConnectionString = "Endpoint=sb://smartcitysimulatornamespace.servicebus.windows.net/;SharedAccessKeyName=ReceiverRule;SharedAccessKey=O1c1yoNGFzyPxS7IXEEdLEkYG2TdNDpfRdp7iuKnvrE=";
//        AppConfig.eventHubConnectionString = "Endpoint=sb://smartcitysimulatornamespace.servicebus.windows.net/;SharedAccessKeyName=ReceiverRule;SharedAccessKey=O1c1yoNGFzyPxS7IXEEdLEkYG2TdNDpfRdp7iuKnvrE=";
//        AppConfig.eventHubName = "smartcitysimulatoreventhub";
//        AppConfig.storageAccountName = "smartcitysimulatorstorageaccount";
//        AppConfig.storageAccountKey = "PsMiEbkQVachawqGSSiimYfuTjKXabBlVjmtWfSayMyMqHjQNOaERU7kDpj21X7ndIEOlFmJ4cgaCmg5pOrTvSg==";
//        AppConfig.consumerGroupName = "smartcitygroup";
//        AppConfig.storageConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}",
//                storageAccountName, storageAccountKey);
//    }
//}


