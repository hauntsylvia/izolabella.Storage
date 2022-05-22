using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
