using System;
using System.Threading.Tasks;

namespace Yoyo.Pro.DataFileObjects
{
    public interface IDataFileObjectManager
    {
        Task<DataFileObject> GetOrNullAsync(Guid id);

        Task SaveAsync(DataFileObject file);

        Task DeleteAsync(Guid id);
    }
}
