using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WishList.Data;
using WishList.Models;

namespace WishList.Controllers
{
    public class ItemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Item> requests = _context.Items.ToList();
            requests.Sort((r1, r2) => r1.Deadline.CompareTo(r2.Deadline));
            SetOverdues(requests);

            return View("Index", requests);
        }

        private List<Item> SetOverdues(List<Item> requests)
        {
            DateTime oneHourForward = DateTime.Now.AddHours(1);
            foreach(Item request in requests)
            {
                if (request.Deadline.CompareTo(oneHourForward) < 0)
                {
                    request.Overdue = true;
                }
            }
            return requests;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public IActionResult Create(Models.Item item)
        {
            item.Created = DateTime.Now;
            _context.Items.Add(item);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var item = _context.Items.FirstOrDefault(e => e.Id == id);
            _context.Items.Remove(item);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
