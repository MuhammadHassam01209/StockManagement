using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Trade
    {
        public int Id { get; set; }
        public int StockId { get; set; }
        public int UserId { get; set; }
        public int Quantity { get; set; }
        public DateTime TradeDate { get; set; }
        public Stock Stock { get; set; }
        public User User { get; set; }
    }
}
