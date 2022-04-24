using Cleemy.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cleemy.Application
{
    public interface IUserService
    {
        Task<List<User>> GetAllUserAsync();
    }
}
