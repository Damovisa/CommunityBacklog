using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace CommunityBacklogWebRole.Storage
{
    public abstract class TableStorageServiceBase
    {

        protected readonly string TableStorageConnStr = ConfigurationManager.AppSettings.Get("AzureTableStorageConnectionString");
        protected readonly string TableName;

        public TableStorageServiceBase(string tableName)
        {
            TableName = tableName;
        }

        protected CloudTableClient GetTableClient()
        {
            var storageAccount = CloudStorageAccount.Parse(TableStorageConnStr);

            return storageAccount.CreateCloudTableClient();
        }

    }
}