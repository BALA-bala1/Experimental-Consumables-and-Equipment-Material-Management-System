using System;
using System.Windows.Forms;
using ExperimentalManagementSystem.Models;
using ExperimentalManagementSystem.Services;

namespace ExperimentalManagementSystem
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            // 绑定登录按钮点击事件
            this.btlogin.Click += new EventHandler(this.btlogin_Click);
            // 绑定密码框回车键事件
            this.tbpassword.KeyDown += new KeyEventHandler(this.tbpassword_KeyDown);
        }

        /// <summary>
        /// 登录按钮点击事件
        /// </summary>
        private void btlogin_Click(object sender, EventArgs e)
        {
            PerformLogin();
        }

        /// <summary>
        /// 密码框回车键事件
        /// </summary>
        private void tbpassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PerformLogin();
                e.SuppressKeyPress = true; // 防止发出"叮"声
            }
        }

        /// <summary>
        /// 执行登录操作
        /// </summary>
        private void PerformLogin()
        {
            string username = tbusername.Text.Trim();
            string password = tbpassword.Text;

            // 输入验证
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("请输入用户名！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbusername.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("请输入密码！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbpassword.Focus();
                return;
            }

            try
            {
                // 显示加载状态
                btlogin.Enabled = false;
                btlogin.Text = "登录中...";

                // 验证用户登录
                User user = UserService.ValidateLogin(username, password);

                if (user != null)
                {
                    // 登录成功，保存用户信息到全局变量
                    Global.CurrentUser = user;

                    // 检查用户角色
                    CheckUserRoleAndRedirect(user);
                }
                else
                {
                    MessageBox.Show("用户名或密码错误！", "登录失败",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tbpassword.SelectAll();
                    tbpassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"登录失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 恢复按钮状态
                btlogin.Enabled = true;
                btlogin.Text = "登录";
            }
        }

        /// <summary>
        /// 检查用户角色并跳转到相应页面
        /// </summary>
        private void CheckUserRoleAndRedirect(User user)
        {
            try
            {
                // 检查用户角色
                bool isSuperAdmin = UserService.HasRole(user.Id, "SUPER_ADMIN");
                bool isAdmin = UserService.HasRole(user.Id, "ADMIN");
                bool isLabManager = UserService.HasRole(user.Id, "LAB_MANAGER");
                bool isUser = UserService.HasRole(user.Id, "USER");

                string roleInfo = "";
                if (isSuperAdmin) roleInfo += "超级管理员 ";
                if (isAdmin) roleInfo += "系统管理员 ";
                if (isLabManager) roleInfo += "实验室管理员 ";
                if (isUser) roleInfo += "普通用户 ";

                if (string.IsNullOrEmpty(roleInfo))
                {
                    roleInfo = "无角色";
                }

                // 显示登录成功信息
                MessageBox.Show($"登录成功！\n欢迎回来，{user.FullName}！\n角色：{roleInfo.Trim()}",
                    "登录成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 暂时所有角色都跳转到admin页面，后续可以根据角色跳转到不同页面
                // 例如：
                // if (isSuperAdmin || isAdmin) { 跳转到管理页面 }
                // else if (isLabManager) { 跳转到实验室管理页面 }
                // else { 跳转到用户页面 }

                // 目前全部跳转到admin页面
                AdminForm adminForm = new AdminForm();
                adminForm.Show();
                this.Hide(); // 隐藏登录窗体
            }
            catch (Exception ex)
            {
                MessageBox.Show($"角色检查失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 用户名文本框变化事件（可选，用于实时验证等）
        /// </summary>
        private void tbusername_TextChanged(object sender, EventArgs e)
        {
            // 可以在这里添加用户名实时验证逻辑
            // 例如：检查用户名格式、长度等
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void Login_Load(object sender, EventArgs e)
        {
            // 测试数据库连接
            try
            {
                // 这里可以添加数据库连接测试
                // 如果数据库连接失败，可以在这里提示用户
            }
            catch (Exception ex)
            {
                MessageBox.Show($"数据库连接失败：{ex.Message}\n请检查数据库配置！",
                    "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
    }
}