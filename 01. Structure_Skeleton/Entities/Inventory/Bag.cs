using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarCroft.Entities.Items;
using WarCroft.Constants;

namespace WarCroft.Entities.Inventory
{
    public abstract class Bag : IBag
    {
        private readonly List<Item> items;

        public Bag(int capacity = 100)
        {
            this.items = new List<Item>();
            this.Capacity = capacity;
        }

        public int Capacity { get; set; }

        

        public int Load =>
            this.Items.Sum(x => x.Weight);

        public IReadOnlyCollection<Item> Items
            => this.items;

        

        public void AddItem(Item item)
        {
            if (this.Load + item.Weight > this.Capacity)
            {
                throw new InvalidOperationException(ExceptionMessages.ExceedMaximumBagCapacity);
            }
            this.items.Add(item);
        }

        public Item GetItem(string name)
        {
            if (!this.Items.Any())
            {
                throw new InvalidOperationException(ExceptionMessages.EmptyBag);

            }
            Item item = this.Items.FirstOrDefault(x => x.GetType().Name == name);

            if (item == null)
            {
                throw new InvalidOperationException(string.Format(ExceptionMessages.ItemNotFoundInBag, name));
            }
            items.Remove(item);
            return item;

        }
    }
}
