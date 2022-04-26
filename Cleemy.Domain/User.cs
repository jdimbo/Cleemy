using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cleemy.Domain
{
    public class User
    {
        public User()
        {
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CurrencyId { get; set; }

        public virtual Currency Currency { get; set; }

    }
}
