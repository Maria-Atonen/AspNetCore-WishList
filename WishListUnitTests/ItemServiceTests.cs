using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using WishList.Data;
using WishList.Models;
using WishList.Services;
using Xunit;



namespace WishListUnitTests
{


    public class ItemSeviceTests
    {

        private ItemService _itemService;

        

        public ItemSeviceTests()
        {

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase("WishList");
            var context = new ApplicationDbContext(optionsBuilder.Options);             
            _itemService = new ItemService(context);

        }
        [Fact]
        public void SaveItemTest()
        {
            Item item = MakeTestRequest();
            DateTime created = DateTime.Now;
            created = SetMillisecondsToNull(created);
            String description = item.Description;
            DateTime deadline = item.Deadline;          

            _itemService.SaveItem(item);
            var itemFromContext = _itemService.getItemByDescription(description);
            Assert.Equal(created, SetMillisecondsToNull(itemFromContext.Created));
            Assert.Equal(description, itemFromContext.Description);
            Assert.Equal(deadline, SetMillisecondsToNull(itemFromContext.Deadline));

        }

        [Fact]
        public void DeleteItemTest()
        {
            Item item = MakeTestRequest();
            String description = item.Description;
            _itemService.SaveItem(item);
            int theId = item.Id;
            _itemService.DeleteItem(theId);
            Assert.Throws<NullReferenceException>(()=> _itemService.getItemByDescription(description));         

        }

        [Fact]
        public void TestForOverdue()
        {
            Item itemWithOverdue = MakeTestRequest(true);
            _itemService.SaveItem(itemWithOverdue);
            String description = itemWithOverdue.Description;
            Item itemFromContext = _itemService.getItemByDescription(description);
            Assert.True(itemFromContext.Overdue);
        }

        [Fact]
        public void TestForNoOverdue()
        {
            Item itemWithNoOverdue = MakeTestRequest(false);
            _itemService.SaveItem(itemWithNoOverdue);
            String description = itemWithNoOverdue.Description;
            Item itemFromContext = _itemService.getItemByDescription(description);
            Assert.False(itemFromContext.Overdue);
        }

        [Fact]
        public void TestSorting()
        {
            Item item1 = MakeTestRequest();
            _itemService.SaveItem(item1);
            Item item2 = MakeTestRequest(true);
            _itemService.SaveItem(item2);
            var items = _itemService.GetItems();
            DateTime firtsItemDeadline = items[0].Deadline;
            DateTime secondItemDeadline = items[1].Deadline;
            Assert.True(DateTime.Compare(firtsItemDeadline, secondItemDeadline) < 0);

        }

        private Item MakeTestRequest(Boolean isOverdue)
        {
            Item item = new Item();
            Random random = new Random();
            String description = "test. " + random.Next();
            item.Description = description;
            DateTime deadline;
            if (isOverdue)
            {
                deadline = DateTime.Now;
          
            }
            else
            {
                deadline = DateTime.Now.AddHours(3);
            }
            deadline = SetMillisecondsToNull(deadline);
            item.Deadline = deadline;
            return item;

        }

        private Item MakeTestRequest()
        {
            return MakeTestRequest(false);
        }

        private DateTime SetMillisecondsToNull(DateTime dateTime)
        {
            dateTime = new DateTime(
                    dateTime.Ticks - (dateTime.Ticks % TimeSpan.TicksPerSecond),
                    dateTime.Kind
                );
            return dateTime;
        }


    }
}
