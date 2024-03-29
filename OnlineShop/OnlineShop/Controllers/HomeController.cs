﻿using Microsoft.AspNetCore.Mvc;
using OnlineShop.DataAccess;
using OnlineShop.DataAccess.DAO;
using OnlineShop.Models;
using System.Diagnostics;

namespace OnlineShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserDAO userDAO;
        private readonly ProductDAO productDAO;
        private readonly CategoryDAO categoryDAO;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            userDAO = new UserDAO();
            productDAO = new ProductDAO();
            categoryDAO = new CategoryDAO();
            _logger = logger;
        }

        public IActionResult Index()
        {
            User user = GetCurrentLoggedInUser();
            bool isLoggedIn = (user != null);
            ViewBag.IsLoggedIn = isLoggedIn;
            if (isLoggedIn)
            {
                ProfileModel model = new ProfileModel
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    Avatar = user.Avatar,
                    Address = user.Address,
                    Birthday = user.Dob,
                    Gender = user.Gender,
                    PhoneNumber = user.Phone,
                    RoleName = user.RoleNavigation?.RoleName
                };
                ViewData["Products"] = productDAO.GetProductList();
                ViewData["Categories"] = categoryDAO.GetCategoryList();
                return View(model);
            }
            ViewData["Products"] = productDAO.GetProductList();
            ViewData["Categories"] = categoryDAO.GetCategoryList();
            return View();
        }

        private User GetCurrentLoggedInUser()
        {
            string email = HttpContext.Session.GetString("Email");
            if (!string.IsNullOrEmpty(email))
            {
                return userDAO.GetUser(email);
            }
            return null;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}