using Cleemy.Domain;
using Cleemy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cleemy.Application
{
    public  class UserService : IUserService
    {
        private readonly CleemyDbContext context;

        public UserService(CleemyDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<User>> GetAllUserAsync()
        {
            return await context.User.ToListAsync();
        }


    }
}
