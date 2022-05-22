using izolabella.Storage.Objects.Structures;
using Newtonsoft.Json;

namespace izolabella.Storage.Objects.DataStores
{
    /// <summary>
    /// A class containing simple methods of data storage.
    /// </summary>
    public class DataStore
    {
        internal static DirectoryInfo AppData => new(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        /// <summary>
        /// Initializes an instance of the <see cref="DataStore"/> class.
        /// </summary>
        /// <param name="AppDirectoryName">AppData/<paramref name="AppDirectoryName"/></param>
        /// <param name="DataStoreName">AppData/<paramref name="AppDirectoryName"/>/<paramref name="DataStoreName"/></param>
        public DataStore(string AppDirectoryName, string DataStoreName)
        {
            DirectoryInfo AppDirectory = new(Path.Combine(AppData.FullName, $".{AppDirectoryName}"));
            if (!AppDirectory.Exists)
            {
                AppDirectory.Create();
            }

            this.Location = new(Path.Combine(AppDirectory.FullName, DataStoreName));
            if (!this.Location.Exists)
            {
                this.Location.Create();
            }
        }

        /// <summary>
        /// The location of the directory this <see cref="DataStore"/> is operating in.
        /// </summary>
        public DirectoryInfo Location { get; }

        private FileInfo GetFileInfoFromKey(object Key)
        {
            return new(Path.Combine(this.Location.FullName, Key + ".json"));
        }

        /// <summary>
        /// Deletes the <see cref="IDataStoreEntity"/> relevant to this <see cref="Location"/> and id provided.
        /// </summary>
        /// <param name="Id">The id of the <see cref="IDataStoreEntity"/> to delete.</param>
        /// <returns></returns>
        public Task DeleteAsync(ulong Id)
        {
            FileInfo FileInfo = this.GetFileInfoFromKey(Id);
            if (FileInfo.Exists)
            {
                FileInfo.Delete();
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Saves the <see cref="IDataStoreEntity"/> to a file using <see cref="IDataStoreEntity.Id"/> as the key.
        /// </summary>
        /// <param name="Entity">The <see cref="IDataStoreEntity"/> to save.</param>
        /// <returns></returns>
        public async Task SaveAsync(IDataStoreEntity Entity)
        {
            FileInfo FileInfo = this.GetFileInfoFromKey(Entity.Id);
            using StreamWriter Writer = new(FileInfo.FullName);
            await Writer.WriteAsync(JsonConvert.SerializeObject(Entity, Formatting.Indented));
        }

        /// <summary>
        /// Reads the contents of the file of the given id and returns them, or default if no file exists.
        /// </summary>
        /// <typeparam name="T">The <see cref="IDataStoreEntity"/> derivation to return.</typeparam>
        /// <param name="Id">The id of the <see cref="IDataStoreEntity"/>.</param>
        /// <returns></returns>
        public async Task<T?> ReadAsync<T>(ulong Id) where T : IDataStoreEntity
        {
            FileInfo FileInfo = this.GetFileInfoFromKey(Id);
            if (FileInfo.Exists)
            {
                try
                {
                    using StreamReader Reader = new(FileInfo.FullName);
                    return JsonConvert.DeserializeObject<T>(await Reader.ReadToEndAsync());
                }
                catch(Exception Ex)
                {
                    Console.WriteLine(Ex);
                    throw;
                }
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// Reads the contents of all files within the operating directory, and casts them to the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<List<T>> ReadAllAsync<T>() where T : IDataStoreEntity
        {
            List<T> Entities = new();
            foreach (FileInfo File in this.Location.GetFiles())
            {
                if (File.Exists)
                {
                    using StreamReader Reader = new(File.FullName);
                    T? Entity = JsonConvert.DeserializeObject<T>(await Reader.ReadToEndAsync());
                    if (Entity != null)
                    {
                        Entities.Add(Entity);
                    }
                }
            }
            return Entities;
        }
    }
}