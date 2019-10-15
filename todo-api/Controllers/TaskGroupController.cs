using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todo_api.DTOs;
using todo_api.Models;

namespace todo_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskGroupController : ControllerBase
    {
        private readonly TodoDbContext _context;

        public TaskGroupController(TodoDbContext context)
        {
            _context = context;
        }

        // GET: api/TaskGroup
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskGroupViewModel>>> GetTaskGroups()
        {
            var taskGroups = await _context.TaskGroups
                .Include(tg => tg.UserTasks)
                .ToListAsync();

            return taskGroups.Select(tg => new TaskGroupViewModel
            (
                tg.Id,
                tg.Name,
                tg.UserTasks?.Count ?? 0
            )).ToList();
        }

        // GET: api/TaskGroup/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskGroupViewModel>> GetTaskGroup(int id)
        {
            var taskGroup = await _context.TaskGroups.FindAsync(id);
            var taskGroupTasks = taskGroup.UserTasks;
            Console.WriteLine(taskGroupTasks);
            if (taskGroup == null)
            {
                return NotFound();
            }

            return new TaskGroupViewModel
            (
                taskGroup.Id,
                taskGroup.Name,
                taskGroup.UserTasks?.Count ?? 0
            );
        }

        // PATCH: api/TaskGroup/5
        [HttpPatch("{id}")]
        public async Task<ActionResult<TaskGroupViewModel>> PatchTaskGroup(int id, TaskGroupDTO taskGroupUpdate)
        {
            var taskGroup = await _context.TaskGroups.FindAsync(id);
            if (taskGroup == null)
            {
                return NotFound();
            }

            taskGroup.Name = taskGroupUpdate.Name ?? "Updated Name";

            _context.Entry(taskGroup).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return new TaskGroupViewModel
            (
                taskGroup.Id,
                taskGroup.Name,
                taskGroup.UserTasks?.Count ?? 0
            );
        }

        // POST: api/TaskGroup
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<TaskGroupViewModel>> PostTaskGroup(TaskGroupDTO taskGroup)
        {
            var newGroup = new TaskGroup(taskGroup.Name ?? "New task group");
            _context.TaskGroups.Add(newGroup);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTaskGroup), new { id = newGroup.Id }, new TaskGroupViewModel(
                newGroup.Id,
                newGroup.Name,
                newGroup.UserTasks?.Count ?? 0
            ));
        }

        // DELETE: api/TaskGroup/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTaskGroup(int id)
        {
            var taskGroup = await _context.TaskGroups.FindAsync(id);
            if (taskGroup == null)
            {
                return NotFound();
            }

            _context.TaskGroups.Remove(taskGroup);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
