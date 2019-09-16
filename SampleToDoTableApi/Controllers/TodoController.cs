using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SampleToDoTableApi.Models;
using SampleToDoTableApi.Repositories;

namespace SampleToDoTableApi.Controllers
{
    public class TodoController : Controller
    {
        public ActionResult Index()
        {
            var repository = new ToDoRepository();
            var entities = repository.All();

            var models = entities.Select(x => new TodoModel
            {
                Id=x.RowKey,
                Group=x.PartitionKey,
                Content=x.Content,
                Due=x.Due,
                Completed=x.Completed,
                TimeStamp=x.Timestamp
            });
            return View(models);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(TodoModel model)
        {
            var repository = new ToDoRepository();

            repository.Create(new TodoEntity
            {
                PartitionKey=model.Group,
                RowKey=Guid.NewGuid().ToString(),
                Content=model.Content,
                Due=model.Due
            });

            return RedirectToAction("Index");
        }

        public ActionResult ConfirmDelete(string id, string group)
        {
            var repository = new ToDoRepository();
            var item = repository.Get(group, id);

            return View("Delete",new TodoModel
            {
                Id = item.RowKey,
                Group = item.PartitionKey,
                Content = item.Content,
                Due = item.Due,
                Completed = item.Completed,
                TimeStamp = item.Timestamp
            });
        }

        [HttpPost]
        public ActionResult Delete(string Id, string group)
        {
            var repository = new ToDoRepository();
            var item = repository.Get(group, Id);

            repository.Delete(item);

            return RedirectToAction("Index");
        }
    }
}