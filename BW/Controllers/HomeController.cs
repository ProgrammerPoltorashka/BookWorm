﻿using BW.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BW.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult Index( string searchString)
        {

            var clubs = from m in context.Clubs
                        select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                clubs = clubs.Where(s => s.Name.Contains(searchString));    
            } 
           
            return View(clubs.ToList());
        }

        public ActionResult Clubpage(int? id)
        {
            Clubs clubs = context.Clubs.Find(id);
            if (id == null) return RedirectToAction("Index");
            if (clubs == null)
            {
                return HttpNotFound();
            }
            ViewBag.Books = context.Books.ToList();
            ViewBag.Users = context.Users.ToList();
            return View(clubs);
        }

        [HttpPost]
        public ActionResult Clubpage(Clubs clubs, int[] selectedBooks, string[] selectedUsers)
        {
            Clubs newclub = context.Clubs.Find(clubs.Id);
            newclub.Books.Clear();
            newclub.ApplicationUser.Clear();
            if (selectedBooks != null)
            {
                //получаем выбранные книги
                foreach (var c in context.Books.Where(co => selectedBooks.Contains(co.Id)))
                {
                    newclub.Books.Add(c);
                }
            }
            if (selectedUsers != null)
            {
                foreach (var c in context.Users.Where(co => selectedUsers.Contains(co.Id)))
                {
                    newclub.ApplicationUser.Add(c);
                }
            }

            context.Entry(newclub).State = EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("ClubPage");
        }
        [HttpGet]
        public ActionResult DeleteClub(int? id)
        {
            if (id == null)
            { return HttpNotFound(); }
            Clubs b = context.Clubs.Find(id);
            if (b == null) 
            { return HttpNotFound();}
            return View(b);
        }
        [HttpPost, ActionName("DeleteClub")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            { return HttpNotFound(); }
            Clubs b = context.Clubs.Find(id);
            if (b == null)
            { return HttpNotFound(); }

            context.Clubs.Remove(b);
            context.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Edit(int? id)
        {
            Clubs clubs = context.Clubs.Find(id);
            if (id == null) return RedirectToAction("Clubpage");
            if (clubs == null)
            {
                return HttpNotFound();
            }
            ViewBag.Tags = context.Tags.ToList();
            return View(clubs);
        } 
        

        [HttpPost]
        public ActionResult Edit(Clubs clubs, int[] selectedTags, int[] selectedBooks)
        {
            Clubs newclub = context.Clubs.Find(clubs.Id);
            newclub.Name = clubs.Name;
            newclub.Discription = clubs.Discription;
            //менять фотрграфия и иконку

            newclub.Tags.Clear();
            if (selectedTags != null)
            {
                //получаем выбранные тэги
                foreach (var c in context.Tags.Where(co => selectedTags.Contains(co.Id)))
                {
                    newclub.Tags.Add(c);
                }
            }

            context.Entry(newclub).State = EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("ClubPage");
        }
        
       
        public ActionResult Bookadd(string searchString)
        {
            var books = from m in context.Books
                        select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Name.Contains(searchString));
            }

            return View(books.ToList());
        }


        [HttpPost]
        public ActionResult Bookadd(Clubs clubs,int[] selectedBook)
        {
            Clubs newclub = context.Clubs.Find(clubs.Id);

            newclub.Books.Clear();
            if (selectedBook != null)
            {
                //получаем выбранные книги
                foreach (var c in context.Books.Where(co => selectedBook.Contains(co.Id)))
                {
                    newclub.Books.Add(c);
                }
            }
            context.Entry(newclub).State = EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("ClubPage");
        }

    }
}