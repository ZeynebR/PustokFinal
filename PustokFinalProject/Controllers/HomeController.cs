﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace PustokFinalProject.Controllers
{
    public class HomeController : Controller
    {
       

        public IActionResult Index()
        {
            return View();
        }

        

       
    }
}