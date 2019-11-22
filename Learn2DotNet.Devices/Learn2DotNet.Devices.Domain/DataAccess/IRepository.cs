using System.Collections.Generic;

namespace Learn2DotNet.Devices.Domain.DataAccess
{
    public interface IRepository<TEntity> where TEntity : class
    {
        List<TEntity> GetAll();
        void Add(TEntity entity);
    }
}