namespace izolabella.Storage.Objects.Structures
{
    public interface IDataStoreEntity
    {
        /// <summary>
        /// The unique identifier of this <see cref="IDataStoreEntity"/>.
        /// </summary>
        ulong Id { get; }
    }
}
