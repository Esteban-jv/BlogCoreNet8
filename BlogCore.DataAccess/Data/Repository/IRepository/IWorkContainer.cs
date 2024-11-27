using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.DataAccess.Data.Repository.IRepository
{
    public interface IWorkContainer : IDisposable // Unit of work class
    {
        // Aqui se deben ir agregando los diferentes repositorios que se vayan creando
        ICategoryRepository Category { get; }

        void Save();
    }
}
