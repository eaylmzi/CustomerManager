using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManager.Domain.SeedWork
{
    public class BaseEntity
    {
        [Column("id")]
        public int Id { get; set; }
    }
}
