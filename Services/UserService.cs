using System;
using System.Data;
using MySql.Data.MySqlClient;
using ExperimentalManagementSystem.Models;

namespace ExperimentalManagementSystem.Services
{
    public class UserService
    {
        // 验证用户登录
        public static User ValidateLogin(string username, string plainPassword)
        {
            string query = @"
                SELECT id, username, password_hash, full_name, email, phone, 
                       status, last_login_at, created_at, updated_at 
                FROM users 
                WHERE username = @username AND status = 'active'";

            MySqlParameter[] parameters = {
                new MySqlParameter("@username", username)
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count == 0)
                return null;

            DataRow row = dt.Rows[0];
            string storedPasswordHash = row["password_hash"].ToString();

            // 验证密码
            if (VerifyPassword(plainPassword, storedPasswordHash))
            {
                // 更新最后登录时间
                UpdateLastLoginTime(row["id"].ToString());

                return new User
                {
                    Id = row["id"].ToString(),
                    Username = row["username"].ToString(),
                    PasswordHash = storedPasswordHash,
                    FullName = row["full_name"].ToString(),
                    Email = row["email"]?.ToString(),
                    Phone = row["phone"]?.ToString(),
                    Status = row["status"].ToString(),
                    LastLoginAt = row["last_login_at"] == DBNull.Value ? null : (DateTime?)row["last_login_at"],
                    CreatedAt = (DateTime)row["created_at"],
                    UpdatedAt = (DateTime)row["updated_at"]
                };
            }

            return null;
        }

        // 加密密码（SHA256）
        public static string HashPassword(string password)
        {
            using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // 验证密码
        private static bool VerifyPassword(string inputPassword, string storedPasswordHash)
        {
            string inputHash = HashPassword(inputPassword);
            return inputHash.Equals(storedPasswordHash, StringComparison.OrdinalIgnoreCase);
        }

        // 更新最后登录时间
        private static void UpdateLastLoginTime(string userId)
        {
            string query = "UPDATE users SET last_login_at = NOW() WHERE id = @id";
            MySqlParameter[] parameters = {
                new MySqlParameter("@id", userId)
            };
            DatabaseHelper.ExecuteNonQuery(query, parameters);
        }

        // 检查用户是否有特定角色
        public static bool HasRole(string userId, string roleCode)
        {
            string query = @"
                SELECT COUNT(*) 
                FROM user_roles ur 
                JOIN roles r ON ur.role_id = r.id 
                WHERE ur.user_id = @userId AND r.code = @roleCode";

            MySqlParameter[] parameters = {
                new MySqlParameter("@userId", userId),
                new MySqlParameter("@roleCode", roleCode)
            };

            object result = DatabaseHelper.ExecuteScalar(query, parameters);
            return Convert.ToInt32(result) > 0;
        }
    }
}