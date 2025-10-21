using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace ExperimentalManagementSystem
{
    public class DatabaseHelper
    {
        private static string connectionString = "Server=127.0.0.1;Database=experimentalmanagementsystem;Uid=root;Pwd=123456;";

        #region 通用数据库操作

        public static DataTable ExecuteQuery(string sql, params MySqlParameter[] parameters)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters);

                        DataTable dt = new DataTable();
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                        return dt;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"数据库查询失败: {ex.Message}");
                }
            }
        }

        public static int ExecuteNonQuery(string sql, params MySqlParameter[] parameters)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters);
                        return cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"数据库操作失败: {ex.Message}");
                }
            }
        }

        public static object ExecuteScalar(string sql, params MySqlParameter[] parameters)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters);
                        return cmd.ExecuteScalar();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"数据库查询失败: {ex.Message}");
                }
            }
        }

        #endregion

        #region 用户管理相关方法

        public static DataTable GetAllUsers()
        {
            string sql = @"SELECT id, username, full_name, email, phone, status, 
                                  last_login_at, created_at, updated_at 
                           FROM users 
                           ORDER BY created_at DESC";
            return ExecuteQuery(sql);
        }

        public static DataTable GetUserById(string userId)
        {
            string sql = "SELECT * FROM users WHERE id = @id";
            MySqlParameter[] parameters = {
                new MySqlParameter("@id", userId)
            };
            return ExecuteQuery(sql, parameters);
        }

        public static int CreateUser(string username, string passwordHash, string fullName,
                                   string email = null, string phone = null)
        {
            string sql = @"INSERT INTO users (username, password_hash, full_name, email, phone, status) 
                           VALUES (@username, @passwordHash, @fullName, @email, @phone, 'active')";

            MySqlParameter[] parameters = {
                new MySqlParameter("@username", username),
                new MySqlParameter("@passwordHash", passwordHash),
                new MySqlParameter("@fullName", fullName),
                new MySqlParameter("@email", email ?? (object)DBNull.Value),
                new MySqlParameter("@phone", phone ?? (object)DBNull.Value)
            };

            return ExecuteNonQuery(sql, parameters);
        }

        public static int UpdateUserStatus(string userId, string status)
        {
            string sql = "UPDATE users SET status = @status WHERE id = @id";
            MySqlParameter[] parameters = {
                new MySqlParameter("@status", status),
                new MySqlParameter("@id", userId)
            };
            return ExecuteNonQuery(sql, parameters);
        }

        public static bool IsUsernameExists(string username)
        {
            string sql = "SELECT COUNT(*) FROM users WHERE username = @username";
            MySqlParameter[] parameters = {
                new MySqlParameter("@username", username)
            };
            object result = ExecuteScalar(sql, parameters);
            return Convert.ToInt32(result) > 0;
        }

        #endregion

        #region 角色管理相关方法

        public static DataTable GetAllRoles()
        {
            string sql = "SELECT * FROM roles ORDER BY created_at";
            return ExecuteQuery(sql);
        }

        public static DataTable GetUserRoles(string userId)
        {
            string sql = @"SELECT r.*, ur.assigned_at, ur.assigned_by 
                           FROM user_roles ur 
                           JOIN roles r ON ur.role_id = r.id 
                           WHERE ur.user_id = @userId";
            MySqlParameter[] parameters = {
                new MySqlParameter("@userId", userId)
            };
            return ExecuteQuery(sql, parameters);
        }

        public static int AssignRoleToUser(string userId, string roleId, string assignedBy)
        {
            string sql = @"INSERT INTO user_roles (user_id, role_id, assigned_by) 
                           VALUES (@userId, @roleId, @assignedBy) 
                           ON DUPLICATE KEY UPDATE assigned_by = @assignedBy, assigned_at = NOW()";

            MySqlParameter[] parameters = {
                new MySqlParameter("@userId", userId),
                new MySqlParameter("@roleId", roleId),
                new MySqlParameter("@assignedBy", assignedBy)
            };

            return ExecuteNonQuery(sql, parameters);
        }

        public static int RemoveRoleFromUser(string userId, string roleId)
        {
            string sql = "DELETE FROM user_roles WHERE user_id = @userId AND role_id = @roleId";
            MySqlParameter[] parameters = {
                new MySqlParameter("@userId", userId),
                new MySqlParameter("@roleId", roleId)
            };
            return ExecuteNonQuery(sql, parameters);
        }

        #endregion

        #region 实验室成员管理相关方法

        public static DataTable GetAllLabs()
        {
            string sql = @"SELECT l.*, d.name as department_name, u.full_name as manager_name 
                           FROM labs l 
                           LEFT JOIN departments d ON l.department_id = d.id 
                           LEFT JOIN users u ON l.manager_id = u.id";
            return ExecuteQuery(sql);
        }

        public static DataTable GetLabMembers(string labId)
        {
            string sql = @"SELECT u.id, u.username, u.full_name, u.email, lm.role_in_lab, lm.active 
                           FROM lab_memberships lm 
                           JOIN users u ON lm.user_id = u.id 
                           WHERE lm.lab_id = @labId AND lm.active = TRUE";
            MySqlParameter[] parameters = {
                new MySqlParameter("@labId", labId)
            };
            return ExecuteQuery(sql, parameters);
        }

        public static int AddUserToLab(string userId, string labId, string roleInLab)
        {
            string sql = @"INSERT INTO lab_memberships (lab_id, user_id, role_in_lab, active) 
                           VALUES (@labId, @userId, @roleInLab, TRUE) 
                           ON DUPLICATE KEY UPDATE role_in_lab = @roleInLab, active = TRUE";

            MySqlParameter[] parameters = {
                new MySqlParameter("@labId", labId),
                new MySqlParameter("@userId", userId),
                new MySqlParameter("@roleInLab", roleInLab)
            };

            return ExecuteNonQuery(sql, parameters);
        }

        public static int RemoveUserFromLab(string userId, string labId)
        {
            string sql = "UPDATE lab_memberships SET active = FALSE WHERE user_id = @userId AND lab_id = @labId";
            MySqlParameter[] parameters = {
                new MySqlParameter("@userId", userId),
                new MySqlParameter("@labId", labId)
            };
            return ExecuteNonQuery(sql, parameters);
        }

        #endregion

        #region 审计日志

        public static int LogAudit(string actorId, string action, string objectType,
                                  string objectId, string beforeJson = null, string afterJson = null, string ip = null)
        {
            string sql = @"INSERT INTO audit_logs (actor_id, action, object_type, object_id, before_json, after_json, ip) 
                           VALUES (@actorId, @action, @objectType, @objectId, @beforeJson, @afterJson, @ip)";

            MySqlParameter[] parameters = {
                new MySqlParameter("@actorId", actorId),
                new MySqlParameter("@action", action),
                new MySqlParameter("@objectType", objectType),
                new MySqlParameter("@objectId", objectId),
                new MySqlParameter("@beforeJson", beforeJson ?? (object)DBNull.Value),
                new MySqlParameter("@afterJson", afterJson ?? (object)DBNull.Value),
                new MySqlParameter("@ip", ip ?? (object)DBNull.Value)
            };

            return ExecuteNonQuery(sql, parameters);
        }

        #endregion

        #region 其他常用方法

        public static DataTable GetAllDepartments()
        {
            string sql = @"SELECT d.*, p.name as parent_name, u.full_name as manager_name 
                           FROM departments d 
                           LEFT JOIN departments p ON d.parent_id = p.id 
                           LEFT JOIN users u ON d.manager_id = u.id 
                           ORDER BY d.name";
            return ExecuteQuery(sql);
        }

        public static DataTable GetUserLabMemberships(string userId)
        {
            string sql = @"SELECT lm.*, l.name as lab_name, l.code as lab_code, d.name as department_name 
                           FROM lab_memberships lm 
                           JOIN labs l ON lm.lab_id = l.id 
                           JOIN departments d ON l.department_id = d.id 
                           WHERE lm.user_id = @userId AND lm.active = TRUE";
            MySqlParameter[] parameters = {
                new MySqlParameter("@userId", userId)
            };
            return ExecuteQuery(sql, parameters);
        }

        public static bool IsUserInLab(string userId, string labId)
        {
            string sql = "SELECT COUNT(*) FROM lab_memberships WHERE user_id = @userId AND lab_id = @labId AND active = TRUE";
            MySqlParameter[] parameters = {
                new MySqlParameter("@userId", userId),
                new MySqlParameter("@labId", labId)
            };
            object result = ExecuteScalar(sql, parameters);
            return Convert.ToInt32(result) > 0;
        }

        #endregion
    }
}