using demo.Models;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

public class AccountController : Controller
{
    // Không khai báo private connectionString ở đây nữa
    private string GetConnectionString()
    {
        var connStr = ConfigurationManager.ConnectionStrings["AnPhatDB"]?.ConnectionString;
        if (string.IsNullOrEmpty(connStr))
        {
            throw new Exception("Không tìm thấy connection string 'AnPhatDB' trong Web.config");
        }
        return connStr;
    }

    // GET: /Account/Login
    [HttpGet]
    public ActionResult Login()
    {
        if (Session["UserID"] != null)
        {
            string role = Session["RoleName"]?.ToString();
            return RedirectToAction("Index", role == "Admin" ? "Admin" : "Home");
        }
        return View(new LoginViewModel());
    }

    // POST: /Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            string connStr = ConfigurationManager.ConnectionStrings["AnPhatDB"]?.ConnectionString;

            if (string.IsNullOrEmpty(connStr))
            {
                model.ErrorMessage = "Không tìm thấy chuỗi kết nối database!";
                return View(model);
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Login", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Username", model.Username);
                    cmd.Parameters.AddWithValue("@PasswordHash", HashPassword(model.Password));   // Tạm không hash

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Lưu thông tin đăng nhập vào Session
                            Session["UserID"] = reader["user_id"];
                            Session["Username"] = reader["username"];
                            Session["FullName"] = reader["full_name"];
                            Session["RoleID"] = reader["role_id"];
                            Session["RoleName"] = reader["role_name"];

                            // Phân quyền redirect
                            if (reader["role_name"].ToString().Equals("Admin", StringComparison.OrdinalIgnoreCase))
                            {
                                return RedirectToAction("Index", "Admin");     // Trang Admin
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");      // Trang Nhân viên
                            }
                        }
                        else
                        {
                            model.ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng!";
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            model.ErrorMessage = "Lỗi hệ thống: " + ex.Message;
        }

        return View(model);
    }

    // Hàm hash mật khẩu (giữ nguyên như cũ)
    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
                builder.Append(b.ToString("x2"));
            return builder.ToString();
        }
    }
}