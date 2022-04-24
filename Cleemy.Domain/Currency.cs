using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cleemy.Domain
{
    public class Currency : IEqualityComparer<Currency>
    {
        public Currency()
        {
        }
        public int Id { get; set; }
        /// <summary>
        /// the Symbol from currency
        /// example 
        /// </summary>
        public string Symbol { get; set; } 

        /// <summary>
        /// the ISO code from currency
        /// example USD for US dollar
        /// </summary>
        public string ISOCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        public bool Equals(Currency x, Currency y) => x.Symbol.Equals(y.Symbol);

        public int GetHashCode(Currency obj) => obj.Symbol.GetHashCode() ;
       
    }
}
