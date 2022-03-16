using GeneralStore.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GeneralStore.MVC.Controllers
{
    public class TransactionController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Transaction
        public ActionResult Index()
        {
            var context = new ApplicationDbContext();
            var query = context.Transactions.ToArray();
            return View(query);
        }
        // GET: Transaction/Create
        public ActionResult Create()
        {
            var context = new ApplicationDbContext();
            ViewData["Products"] = context.Products.Select(product => new SelectListItem
            {
                Text = product.Name,
                Value = product.ProductId.ToString()
            }).ToArray();
            ViewData["Customers"] = context.Customers.AsEnumerable().Select(customer => new SelectListItem
            {
                Text = customer.FullName,
                Value = customer.CustomerID.ToString()
            });
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Transaction model)
        {
            var context = new ApplicationDbContext();
            ViewData["Products"] = context.Products.Select(product => new SelectListItem
            {
                Text = product.Name,
                Value = product.ProductId.ToString()
            });
            ViewData["Customers"] = context.Customers.AsEnumerable().Select(customer => new SelectListItem
            {
                Text = customer.FullName,
                Value = customer.CustomerID.ToString()
            });
            if (context.Products.Find(model.ProductID) == null)
            {
                ViewData["Error"] = "Invalid Product Id";
                return View(model);
            }
            else if (context.Customers.Find(model.CustomerID) == null)
            {
                ViewData["Error"] = "Invalid Customer Id";
                return View(model);
            }
            model.CreatedAt = DateTime.Now;
            context.Transactions.Add(model);
            if (context.SaveChanges() == 1)
            {
                return Redirect("/Transaction");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var context = new ApplicationDbContext();
            // Validate the id, put entity in a variable
            var entity = context.Transactions.Find(id);
            if (entity == null)
            {
                ViewData["Error"] = "Invalid Transaction Id";
                return Redirect("/transaction");
            }
            // Populate the drop downs
            var customers = context.Customers.AsEnumerable();
            var products = context.Products.AsEnumerable();
            ViewData["Customers"] = customers.Select(c => new SelectListItem
            {
                Text = c.FullName,
                Value = c.CustomerID.ToString()
            });
            ViewData["Products"] = products.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.ProductId.ToString()
            });
            // Supply the entity to the view
            return View(entity);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Transaction model)
        {
            var context = new ApplicationDbContext();
            var entity = context.Transactions.Find(id);
            if (entity == null)
            {
                return Redirect("/transaction");
            }
            entity.CustomerID = model.CustomerID;
            entity.ProductID = model.ProductID;
            if (context.SaveChanges() == 1)
            {
                return Redirect("/transaction");
            }
            ViewData["Error"] = "Couldn't update your transaction, my bad bro.";
            return View(model);
        }

        // GET : Delete
        // Transaction /Delete/{id}
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Transaction transaction = _db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        //POST: Transaction /Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Transaction transaction = _db.Transactions.Find(id);
            _db.Transactions.Remove(transaction);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET : Details(int? id)
        // Transaction /Details/{id}
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = _db.Transactions.Find(id);

            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }
    }
}