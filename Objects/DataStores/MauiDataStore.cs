using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace izolabella.Storage.Objects.DataStores
{
    public class MauiDataStore : DataStore
    {
        public MauiDataStore(string AppDirectoryName, string DataStoreName, JsonSerializerSettings? Settings = null) : base(AppDirectoryName, DataStoreName, Settings, new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)))
        {
        }
    }
}