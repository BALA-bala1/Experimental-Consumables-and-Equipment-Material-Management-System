using System;
using System.Data;
using System.Windows.Forms;

namespace ExperimentalManagementSystem
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
            // 在构造函数中绑定按钮点击事件
            this.test.Click += new EventHandler(this.test_Click);
        }

        // 按钮点击事件处理方法
        private void test_Click(object sender, EventArgs e)
        {
            try
            {
                // 计算密码"admin"的SHA256哈希值
                string testPassword = "admin";
                string calculatedHash = CalculateSHA256Hash(testPassword);
                string expectedHash = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918";

                // 比较计算出的哈希值与预期值
                bool isHashCorrect = calculatedHash.Equals(expectedHash, StringComparison.OrdinalIgnoreCase);

                string resultMessage = $"测试密码: {testPassword}\n" +
                                      $"计算出的哈希: {calculatedHash}\n" +
                                      $"预期哈希: {expectedHash}\n" +
                                      $"哈希验证: {(isHashCorrect ? "✓ 成功" : "✗ 失败")}";

                MessageBox.Show(resultMessage, "密码哈希验证");

                // 同时测试数据库中的用户数据（可选）
                if (isHashCorrect)
                {
                    DataTable dt = DatabaseHelper.GetAllUsers();
                    if (dt.Rows.Count > 0)
                    {
                        string userList = "数据库中的用户:\n\n";
                        foreach (DataRow row in dt.Rows)
                        {
                            userList += $"用户名: {row["username"]}, 姓名: {row["full_name"]}, 状态: {row["status"]}\n";
                        }
                        MessageBox.Show(userList, "用户列表");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"测试失败: {ex.Message}", "错误");
            }
        }

        // SHA256哈希计算方法
        private string CalculateSHA256Hash(string input)
        {
            using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}