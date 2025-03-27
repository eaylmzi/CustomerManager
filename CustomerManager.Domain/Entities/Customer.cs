using CustomerManager.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManager.Domain.Entities
{
    public class Customer : BaseEntity, IAggregateRoot
    {
        [Column("name")]
        public string Name { get; private set; }
        [Column("surname")]
        public string Surname { get; private set; }
        [Column("balance")]
        public decimal Balance { get; private set; }
        [Column("status")]
        public bool Status { get; private set; }
        [Column("city_id")]
        public int? CityId { get; private set; }
        //if efcore is used
        public Customer()
        {

        }
        public Customer(string name, string surname, decimal balance, bool status, int cityId)
        {
            if (balance < 0)
                throw new ArgumentException("Balance cannot be negative", nameof(balance));
            if (cityId < 0)
                throw new ArgumentException("City id cannot be negative", nameof(balance));

            Name = name;
            Surname = surname;
            Balance = balance;
            Status = status;
            CityId = cityId;
        }
        public void UpdateBalance(decimal amount)
        {
            if (Balance + amount < 0)
                throw new InvalidOperationException("Insufficient balance");

            Balance += amount;
        }
    }
}
