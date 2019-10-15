using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todo_api.DTOs;
using todo_api.Models;
using todo_api.ViewModels;

namespace todo_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTaskController : ControllerBase
    {

        private readonly TodoDbContext _context;

        public UserTaskController(TodoDbContext context)
        {
            _context = context;
        }

        // GET: api/UserTask
        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<UserTaskViewModel>>> GetUserTasks()
        // {
        //     var userTasks = await _context.UserTasks.ToListAsync();
        //     Console.WriteLine(userTasks.First().TaskGroup);
        //     return userTasks.Select(ut => new UserTaskViewModel
        //     {
        //         Id = ut.Id,
        //         Name = ut.Name ?? "Task Name",
        //         Deadline = ut.Deadline ?? DateTime.Now,
        //         UserId = ut?.User?.Id,
        //         Status = ut?.Status.ToString() ?? Models.TaskStatus.New.ToString(),
        //         GroupId = ut?.TaskGroup.Id
        //     }).ToList();
        // }

        // GET: api/UserTask?groupId=2
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserTaskViewModel>>> GetTaskGroupUserTasks(
            [FromQuery] int taskGroupId
        )
        {
            var taskGroup = await _context.TaskGroups.Include(tg => tg.UserTasks).Where(tg => tg.Id == taskGroupId).FirstAsync();
            return taskGroup.UserTasks.Select(ut => new UserTaskViewModel
            {
                Id = ut.Id,
                Name = ut.Name ?? "Task Name",
                Deadline = ut.Deadline ?? DateTime.Now,
                UserId = ut?.User?.Id,
                Status = ut?.Status.ToString() ?? Models.TaskStatus.New.ToString(),
                GroupId = ut?.TaskGroup.Id
            }).ToList();
        }

        // GET: api/UserTask/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserTaskViewModel>> GetUserTask(int id)
        {
            var ut = await _context.UserTasks.FindAsync(id);

            if (ut == null)
            {
                return NotFound();
            }

            // return ut;

            return new UserTaskViewModel
            {
                Id = ut.Id,
                Name = ut.Name ?? "No-name",
                Deadline = ut.Deadline ?? DateTime.Now,
                UserId = ut?.User?.Id,
                Status = ut?.Status.ToString() ?? Models.TaskStatus.New.ToString(),
                GroupId = ut?.TaskGroup?.Id
            };
        }

        // PATCH: api/UserTask/5
        [HttpPatch("{id}")]
        public async Task<ActionResult<UserTaskViewModel>> PatchUserTask(int id, UserTaskDTO userTaskUpdate)
        {
            var userTask = await _context.UserTasks.FindAsync(id);
            if (userTask == null)
            {
                return NotFound();
            }

            userTask.Name = userTaskUpdate.Name ?? userTask.Name;
            userTask.Status = userTaskUpdate.Status;
            userTask.User = await _context.Users.FindAsync(userTaskUpdate.UserId);
            userTask.Deadline = userTaskUpdate.Deadline;

            _context.Entry(userTask).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return new UserTaskViewModel
            {
                Id = userTask.Id,
                Name = userTask.Name ?? "No-name",
                Deadline = userTask.Deadline ?? DateTime.Now,
                UserId = userTask?.User.Id,
                Status = userTask?.Status.ToString() ?? Models.TaskStatus.New.ToString(),
                GroupId = userTask?.TaskGroup?.Id
            };
        }

        // POST: api/UserTask
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<UserTaskViewModel>> PostUserTask(UserTaskDTO userTask)
        {
            var taskGroup = await _context.TaskGroups.FindAsync(userTask.GroupId);
            if (taskGroup == null)
            {
                return BadRequest("Invalid task group specified");
            }

            var newTask = new UserTask();
            newTask.Name = userTask.Name;
            newTask.Deadline = userTask.Deadline;
            newTask.Status = userTask.Status;
            newTask.User = _context.Users.Find(userTask.UserId);
            newTask.TaskGroup = taskGroup;

            taskGroup?.UserTasks.Add(newTask);

            _context.UserTasks.Add(newTask);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserTask), new { id = newTask.Id }, new UserTaskViewModel
            {
                Id = newTask.Id,
                Name = newTask.Name ?? "New Task",
                Deadline = newTask.Deadline ?? DateTime.Now,
                UserId = newTask?.User?.Id,
                Status = newTask?.Status.ToString() ?? Models.TaskStatus.New.ToString(),
                GroupId = newTask?.TaskGroup.Id
            });
        }

        // DELETE: api/UserTask/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserTask(int id)
        {
            var taskGroup = await _context.UserTasks.FindAsync(id);
            if (taskGroup == null)
            {
                return NotFound();
            }

            _context.UserTasks.Remove(taskGroup);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
