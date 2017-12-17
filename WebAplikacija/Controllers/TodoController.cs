
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Identity;

using Microsoft.Extensions.Logging;
using WebAplikacija.core;
using WebAplikacija.Models;
using WebAplikacija.Data;
namespace WebAplikacija.Controllers

{
    [Authorize]
    [Route("[controller]")]
    public class TodoController :Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITodoRepository _repository;
        private readonly ILogger<TodoController> _errorLogs;
        public TodoController(UserManager<ApplicationUser> userManager, ITodoRepository repository, ILogger<TodoController> errorLogs)
        {
            _userManager = userManager;
            _repository = repository;
            _errorLogs = errorLogs;
        }
        [Route("")]
        [Route("[action]")]
        public async Task<IActionResult> Index()
        {
    
                Guid userId = new Guid(_userManager.GetUserId(User));
                IndexViewModel model = new IndexViewModel();
                List<TodoItem> items = await _repository.GetActiveAsync(userId);
                foreach (var item in items)
                {
                    TodoViewModel view = new TodoViewModel(item.Id, item.Text, item.DateDue, item.IsCompleted);
                    model.Add(view);
                }
                return View(model);


        }
        [HttpGet]
        public async Task<IActionResult> Mark(Guid? id)
        {
            if (id.HasValue)
            {
                try
                {
                    Guid ide = (Guid)id;
                    Guid userId = new Guid(_userManager.GetUserId(User));
                    await _repository.MarkAsCompletedAsync(ide, userId);
                    return RedirectToAction("Completed");
                }
                catch (Exception ex)
                {
                    _errorLogs.LogWarning(ex, ex.Message);
                    return Unauthorized();
                }
            }
            return NotFound();
        }
        public async Task<IActionResult> Remove(Guid? id)
        {
            if (id.HasValue)
            {
                try
                {
                    Guid ide = (Guid)id;
                    Guid userId = new Guid(_userManager.GetUserId(User));
                    await _repository.RemoveAsync(ide, userId);
                    return RedirectToAction("Completed");
                }
                catch (Exception ex)
                {
                    _errorLogs.LogWarning(ex, ex.Message, new object[] { });
                    return Unauthorized();
                }
            }
            return NotFound();
        }
        [Route("[action]")]
        public async Task<IActionResult> Completed()
        {
            Guid userId = new Guid(_userManager.GetUserId(User));
            CompletedViewModel completed = new CompletedViewModel();
            List<TodoItem> items = await _repository.GetCompletedAsync(userId);
            foreach (var item in items)
            {
                TodoViewModel model = new TodoViewModel(item.Id, item.Text, item.DateCompleted, item.IsCompleted);
                completed.Add(model);
            }
            return View(completed);
        }

        [Route("[action]")]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Text == null) model.Text = "";
                Guid userId = new Guid(_userManager.GetUserId(User));
                TodoItem item = new TodoItem(model.Text, userId);
                _repository.AddAsync(item);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [Route("[action]")]
        public IActionResult Todo()
        {
            return NotFound();
        }

    }
}
