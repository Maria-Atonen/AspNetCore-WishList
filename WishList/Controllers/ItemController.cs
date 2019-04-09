using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WishList.Data;
using WishList.Models;
using WishList.Services;

namespace WishList.Controllers
{
    public class ItemController : Controller
    {
        private ItemService _itemService;

        public ItemController(ItemService itemService)
        {
            _itemService = itemService;

        }

        public IActionResult Index()
        {
            var items = _itemService.GetItems();

            return View("Index", items);
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public IActionResult Create(Models.Item item)
        {
            _itemService.SaveItem(item);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            _itemService.DeleteItem(id);
            return RedirectToAction("Index");
        }

    }
}
