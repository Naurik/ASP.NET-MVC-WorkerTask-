using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskUser.Models;
using TaskUser.ViewModels;

namespace TaskUser.Controllers
{
    public class WorkersController : Controller
    {
        private readonly ContextUser _context;

        public WorkersController(ContextUser context)
        {
            _context = context;
        }

        // GET: Workers
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["PositionSortParam"] = String.IsNullOrEmpty(sortOrder) ? "position_desc" : "";
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["CurrentFilter"] = searchString;

            var doc = from s in _context.Workers
                      select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                doc = doc.Where(s => s.FIO.Contains(searchString)
                                       || s.FIO.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    doc = doc.OrderByDescending(s => s.FIO);
                    break;
                case "position_desc":
                    doc = doc.OrderByDescending(s => s.Position);
                    break;
            }
            return View(await doc.AsNoTracking().ToListAsync());
        }

        // GET: Workers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worker = await _context.Workers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (worker == null)
            {
                return NotFound();
            }

            return View(worker);
        }

        // GET: Workers/Create
        public IActionResult Create()
        {
            List<WorkersTaskList> Models = _context.Tasks
               .Select(c => new WorkersTaskList { Id = c.Id, Name = c.NameTask })
               .ToList();

            ViewModelWorkersTask ivm = new ViewModelWorkersTask { WorkersTasks = Models };
            ViewBag.List = Models;
            return View();
        }

        // POST: Workers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkerDTO workerDTO)
        {
            if (ModelState.IsValid)
            {
                var getID = await (from task in _context.Tasks
                             where task.NameTask == workerDTO.TaskName
                             select task.Id).SingleOrDefaultAsync();
                var worker = new Worker()
                {
                    Id = Guid.NewGuid(),
                    FIO = workerDTO.FIO,
                    Position = workerDTO.Position,
                    State = workerDTO.State,
                    TaskId = getID,
                    TaskName = workerDTO.TaskName
                };
               
                _context.Add(worker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(workerDTO);
        }

        // GET: Workers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worker = await _context.Workers.FindAsync(id);
            if (worker == null)
            {
                return NotFound();
            }
            return View(worker);
        }

        // POST: Workers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,FIO,Position,State,TaskId")] Worker worker)
        {
            if (id != worker.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(worker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkerExists(worker.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(worker);
        }

        // GET: Workers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worker = await _context.Workers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (worker == null)
            {
                return NotFound();
            }

            return View(worker);
        }

        // POST: Workers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var worker = await _context.Workers.FindAsync(id);
            _context.Workers.Remove(worker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkerExists(Guid id)
        {
            return _context.Workers.Any(e => e.Id == id);
        }
    }
}
