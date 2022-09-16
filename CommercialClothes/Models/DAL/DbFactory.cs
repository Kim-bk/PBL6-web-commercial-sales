using System;

namespace ComercialClothes.Models.DAL
{
    public class DbFactory : IDisposable
    {
        private bool _disposed;
        private Func<ECommerceSellingClothesContext> _instanceFunc;
        private ECommerceSellingClothesContext _dbContext;
        public ECommerceSellingClothesContext DbContext => _dbContext ?? (_dbContext = _instanceFunc.Invoke());

        public DbFactory(Func<ECommerceSellingClothesContext> dbContextFactory)
        {
            _instanceFunc = dbContextFactory;
        }

        public void Dispose()
        {
            if (!_disposed && _dbContext != null)
            {
                _disposed = true;
                _dbContext.Dispose();
            }
        }
    }
}
