using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SQL_Injection.Database;
using SQL_Injection.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SQL_Injection.Controllers
{
    public class HomeController : Controller
    {
        private readonly Context _context;
        private readonly string InternallErrorMessage = "internalErrorMessage";
        private readonly string NoDataErrorMessage = "noDataErrorMessage";

        public HomeController(Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (_context.Category.ToList().Count() == 0)
                Add();
            return View();
        }

        public IActionResult Add()
        {
            _context.Add<Category>(new Category() { Name = "Drink" });
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult SetMode(string mode, string data)
        {
            // need to modify partail view @model -> category easy hard expert

            if (mode == "easy")
                return GetPartialCategoryEasy(data);
            else if (mode == "hard")
                return GetPartialCategoryHard(data);
            else
                return GetPartialCategoryExpert(data);
        }

        // Easy
        public IActionResult GetPartialCategoryEasy(string data)
        {
            List<CategoryViewModel> list = new List<CategoryViewModel>();

            if (data == null)
            {
                string errorMessage = NoDataErrorMessage;
                return Content(errorMessage);
            }
            else
            {
                try
                {
                    var conn = _context.Database.GetDbConnection().ConnectionString;
                    using (SqliteConnection sqlConnection = new SqliteConnection(conn))
                    {
                        SqliteCommand sqlCommand = new SqliteCommand
                            (
                                "SELECT CategoryId, Name FROM Category WHERE Name = '" + data + "'"
                                , sqlConnection
                            );
                        sqlConnection.Open();

                        SqliteDataReader reader = sqlCommand.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                list.Add(new CategoryViewModel()
                                {
                                    CategoryID = Int32.Parse(reader[0].ToString()),
                                    Name = reader[1].ToString()
                                });
                            }
                        }
                        reader.Close();
                        sqlConnection.Close();
                    }

                    if (list.Capacity == 0)
                        return Content(NoDataErrorMessage);
                    else
                        return PartialView("CategoryPartialView", list);

                }
                catch (Exception)
                {
                    return Content(InternallErrorMessage);
                }
            }


        }

        // Hard with parameters
        public IActionResult GetPartialCategoryHard(string data)
        {
            List<CategoryViewModel> list = new List<CategoryViewModel>();

            if (data == null)
            {
                string errorMessage = NoDataErrorMessage;
                return Content(errorMessage);
            }
            else
            {
                try
                {
                    var conn = _context.Database.GetDbConnection().ConnectionString;
                    SqliteParameter sqlParameter = new SqliteParameter("@Name", data);
                    var query = @"SELECT CategoryId, Name FROM Category WHERE Name = @Name";
                    using (SqliteConnection sqlConnection = new SqliteConnection(conn))
                    {
                        SqliteCommand sqlCommand = new SqliteCommand(query, sqlConnection);
                        sqlCommand.Parameters.Add(sqlParameter);

                        sqlConnection.Open();

                        SqliteDataReader reader = sqlCommand.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                list.Add(new CategoryViewModel()
                                {
                                    CategoryID = Int32.Parse(reader[0].ToString()),
                                    Name = reader[1].ToString()
                                });
                            }
                        }
                        reader.Close();
                        sqlConnection.Close();
                    }

                    if (list.Capacity == 0)
                        return Content(NoDataErrorMessage);
                    else
                        return PartialView("CategoryPartialView", list);


                }
                catch (Exception)
                {
                    return Content(InternallErrorMessage);
                }
            }

        }

        // Expert, just with entity framework
        public IActionResult GetPartialCategoryExpert(string data)
        {
            if (data == null)
            {
                string errorMessage = NoDataErrorMessage;
                return Content(errorMessage);
            }
            else
            {
                try
                {
                    var categoryList = _context.Category.Where(a => a.Name == data).ToList();
                    List<CategoryViewModel> list = new List<CategoryViewModel>();
                    if (categoryList.Capacity == 0)
                    {
                        return Content(NoDataErrorMessage);
                    }

                    foreach (var item in categoryList)
                    {
                        list.Add(new CategoryViewModel()
                        {
                            CategoryID = item.CategoryId,
                            Name = item.Name
                        });
                    }
                    return PartialView("CategoryPartialView", list);
                }
                catch (Exception)
                {
                    return Content(InternallErrorMessage);
                }

            }
        }
    }
}
