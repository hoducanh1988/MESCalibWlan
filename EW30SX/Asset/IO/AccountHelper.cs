using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EW30SX.Asset.IO {

    public class AccountHelper {

        string file_id = AppDomain.CurrentDomain.BaseDirectory + "id.dll";
        string file_log = AppDomain.CurrentDomain.BaseDirectory + "login.log";

        public class Account {
            public string User { get; set; }
            public string Password { get; set; }
        }

        public AccountHelper() {
            if (File.Exists(file_id) == false) File.WriteAllText(file_id, "admin,vnpt");
        }

        public List<Account> Get() {
            var buffer = File.ReadAllLines(file_id);
            List<Account> list_acc = new List<Account>();
            foreach (var b in buffer) {
                if (string.IsNullOrEmpty(b) || string.IsNullOrWhiteSpace(b)) continue;
                if (b.Contains(",") == false) continue;
                Account acc = new Account() {
                    User = b.Split(',')[0].Replace("\n", "").Replace("\r", "").Trim(),
                    Password = b.Split(',')[1].Replace("\n", "").Replace("\r", "").Trim()
                };
                list_acc.Add(acc);
            }
            return list_acc;
        }

        public void saveLog(string user, string pass, string result) {
            using (var f = new StreamWriter(file_log, true, Encoding.Unicode)) {
                f.WriteLine($"{DateTime.Now},account({user},{pass}) logged into settings with result = {result}");
            }
        }

    }
}
