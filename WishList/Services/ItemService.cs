using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WishList.Data;
using WishList.Models;

namespace WishList.Services
{
    public class ItemService 
    {
        private readonly ApplicationDbContext _context;

        public ItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Item> GetItems()
        {
            var items = _context.Items.ToList();
            items.Sort((r1, r2) => r1.Deadline.CompareTo(r2.Deadline));
          
            SetOverdues(items);
            return items;
        }

        public void SaveItem(Item item)
        {
            item.Created = DateTime.Now;
            _context.Items.Add(item);
            _context.SaveChanges();
        }

        public Item GetItemByDescription(string description)
        {
            var item = _context.Items.FirstOrDefault(e => e.Description == description);
            var oneHourForward = DateTime.Now.AddHours(1);
            item = SetOverdue(item, oneHourForward);

            return item;
        }

        public void DeleteItem(int id)
        {
            var item = _context.Items.FirstOrDefault(e => e.Id == id);
            _context.Items.Remove(item);
            _context.SaveChanges();
        }

        private List<Item> SetOverdues(List<Item> items)
        {
            var oneHourForward = DateTime.Now.AddHours(1);
            foreach (var item in items)
            {
                SetOverdue(item, oneHourForward);
            }
            return items;
        }

        private Item SetOverdue (Item item, DateTime oneHourForward)
        {
            if (item.Deadline.CompareTo(oneHourForward) < 0)
            {
                item.Overdue = true;
            }

            return item;
        }


    }
}
