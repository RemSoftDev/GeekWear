using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GeekWear.Models;
using Microsoft.AspNet.Identity;
using System.Web;

namespace GeekWear.Controllers
{
	public class ProjectsController : ApiController
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: api/Projects
		public IQueryable<Project> GetProjects()
		{
			var userId = User.Identity.GetUserId();
			return db.Projects.Where(p => p.UserId == userId);
		}

		// GET: api/Projects/5
		[ResponseType(typeof(Project))]
		public IHttpActionResult GetProject(int id)
		{
			Project project = db.Projects.Find(id);
			var userId = User.Identity.GetUserId();

			if (project == null)
			{
				return NotFound();
			}
			else if (string.IsNullOrWhiteSpace(userId))
			{
				project.Id = 0;
				project.UserId = "";
			}
			else if (project.UserId != userId)
			{
				project.Id = 0;
				project.UserId = userId;
			}
			return Ok(project);
		}

		//not used because server doesn't allow this method
		// PUT: api/Projects/5
		public IHttpActionResult PutProject(int id)
		{
			Project oldProject = db.Projects.Find(id);
			var userId = User.Identity.GetUserId();

			if (string.IsNullOrWhiteSpace(userId))
			{
				return NotFound();
			}

			var project = new Project
			{
				UserId = userId,
				ShirtColor = oldProject.ShirtColor,
				TextInput = oldProject.TextInput,
				Transform = oldProject.Transform
			};

			db.Projects.Add(project);
			db.SaveChanges();

			return Ok(new { ok = 1 });
		}

		// POST: api/Projects
		[ResponseType(typeof(Project))]
		public IHttpActionResult PostProject(Project project)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (project.Id > 0)
			{
				var userId = User.Identity.GetUserId();
				if (project.UserId != userId)
				{
					return NotFound();
				}
				if (project.Transform == "deleteme")
				{
					project = db.Projects.Find(project.Id);
					db.Projects.Remove(project);
				}
				else
				{
					db.Entry(project).State = EntityState.Modified;
				}
			}
			else
			{
				db.Projects.Add(project);
			}

			db.SaveChanges();

			var cart = HttpContext.Current.Session["Cart"] == null
				? new CartViewModel()
				: (CartViewModel)HttpContext.Current.Session["Cart"];

			cart.Items.Add(new CartViewModelItem { Count = 1, Project = project });
			HttpContext.Current.Session["Cart"] = cart;

			return CreatedAtRoute("DefaultApi", new { id = project.Id }, project);
		}

		//not used because server doesn't allow this method
		// DELETE: api/Projects/5
		public IHttpActionResult DeleteProject(int id)
		{
			Project project = db.Projects.Find(id);
			var userId = User.Identity.GetUserId();
			if (project == null || project.UserId != userId)
			{
				return NotFound();
			}

			db.Projects.Remove(project);
			db.SaveChanges();

			return Ok(new { ok = 1 });
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}

		private bool ProjectExists(int id)
		{
			return db.Projects.Count(e => e.Id == id) > 0;
		}
	}
}
