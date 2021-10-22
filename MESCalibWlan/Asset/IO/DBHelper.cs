using ADODB;
using ADOX;
using EW30SX.Asset.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Access = Microsoft.Office.Interop.Access;

namespace EW30SX.Asset.IO {
    public class DBHelper {


        //--------------------------------------------//
        //1. Create Access DB and table
        //2. Open connection
        //3. Insert data to table
        //4. Insert list data to table
        //5. Read data from table
        //6. Update data to table
        //7. Delete data
        //8. Select data
        //9. Close connection
        //--------------------------------------------//


        const int adStateClosed = 0; //Indicates that the object is closed.
        const int adStateOpen = 1; //Indicates that the object is open.
        const int adStateConnecting = 2; //Indicates that the object is connecting.
        const int adStateExecuting = 4; //Indicates that the object is executing a command.
        const int adStateFetching = 8; //Indicates that the rows of the object are being retrieved.


        private string provider = "Microsoft.ACE.OLEDB.12.0"; //Access 2003 = Microsoft.Jet.OLEDB.4.0, Access 2007,10,13,16 = Microsoft.ACE.OLEDB.12.0
        private string conString = "";
        private string dbString = "";
        private string msDatabasePath = "";

        ADODB.Connection con; //connection
        ADODB.Command cmd; //command
        ADODB.Recordset rs; //recordset

        public bool IsConnected = false;

        public DBHelper(string dbFullName) {
            try {
                this.msDatabasePath = dbFullName;
                this.dbString = string.Format("Provider={0};Data Source={1};Jet OLEDB:Engine Type=5", provider, this.msDatabasePath);
                this.conString = string.Format("Provider={0};Data Source={1};Persist Security Info=False", provider, this.msDatabasePath);
            }
            catch (Exception ex) {
                string data = "Function: MSAccessDB(string dbFullName)\n";
                data += ex.ToString();
                mySupport.writeToDebugFile(data);
            }
        }

        /// <summary>
        /// Open connection to access database; True = Success, False = Fail
        /// </summary>
        /// <returns></returns>
        public bool OpenConnection() {
            bool r = true;
            if (con != null && con.State == 1) goto END;

            con = new ADODB.ConnectionClass();
            con.ConnectionString = this.conString;

            try {
                con.Open();
                r = con.State == 1;
            }
            catch (Exception ex) {
                string data = "Function: OpenConnection\n";
                data += ex.ToString();
                mySupport.writeToDebugFile(data);
                r = false;
            }
            goto END;

        END:
            IsConnected = r;
            return r;
        }


        /// <summary>
        /// Close connection to access database; True = Success, False = Fail
        /// </summary>
        /// <returns></returns>
        public bool Close() {
            bool r = true;
            if (con == null || con.State == 0) goto END;

            try {
                con.Close();
                r = con.State == 0;
            }
            catch (Exception ex) {
                string data = "Function: Close\n";
                data += ex.ToString();
                mySupport.writeToDebugFile(data);
                r = false;
            }
            goto END;

        END:
            IsConnected = false;
            return r;
        }

        public T QueryDataReturnObject<T>(string queryString) where T : class, new() {
            try {
                if (con == null || con.State != 1) this.OpenConnection();
                if (con.State != 1) return null;

                //Get properties of T
                Type itemType = typeof(T);
                var properties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                //open recordset
                rs = new ADODB.Recordset();
                rs.Open(queryString, con);

                //get data
                T t = new T();

                object[,] dataRows = (object[,])rs.GetRows(1, 0);
                object[] p = Enumerable.Range(0, dataRows.GetLength(0)).Select(x => dataRows[x, 0]).ToArray();

                for (int i = 0; i < properties.Length; i++) {
                    properties[i].SetValue(t, Convert.ChangeType(p[i], properties[i].PropertyType));
                }

                //close recordset
                rs.Close();
                rs = null;

                //return data
                return t;
            } catch { return null; }
        }

        public List<T> QueryDataReturnListObject<T>(string queryString) where T : new() {
            try {
                if (con == null || con.State != 1) this.OpenConnection();
                if (con.State != 1) return null;

                //Get properties of T
                Type itemType = typeof(T);
                var properties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                //open recordset
                rs = new ADODB.Recordset();
                rs.Open(queryString, con);

                //get data
                object[,] dataRows = (object[,])rs.GetRows();
                List<T> ts = new List<T>();

                for (int i = 0; i < dataRows.GetLength(1); i++) {
                    object[] p = Enumerable.Range(0, dataRows.GetLength(0)).Select(x => dataRows[x, i]).ToArray();

                    T t = new T();
                    for (int k = 0; k < properties.Length; k++) {
                        properties[k].SetValue(t, Convert.ChangeType(p[k], properties[k].PropertyType));
                    }
                    ts.Add(t);
                }

                //close recordset
                rs.Close();
                rs = null;

                //return data
                return ts;
            } catch { return null; }
        }

        public bool InsertDataToTable<T>(T t, string table_name) {
            try {
                if (con == null || con.State != 1) this.OpenConnection();
                if (con.State != 1) return false;

                //Get properties of T
                Type itemType = typeof(T);
                var properties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                //get field and value
                string s1 = "", s2 = "";
                foreach (var p in properties) {
                    s1 += string.Format("[{0}],", p.Name);
                    s2 += string.Format("'{0}',", p.GetValue(t, null));
                }
                s1 = s1.Substring(0, s1.Length - 1);
                s2 = s2.Substring(0, s2.Length - 1);

                //Assign the connection to the command 
                cmd = new CommandClass();
                cmd.ActiveConnection = con;
                cmd.CommandType = CommandTypeEnum.adCmdText;

                cmd.CommandText = string.Format("INSERT INTO {0}({1}) VALUES({2})", table_name, s1, s2);

                //Execute the command 
                object nRecordsAffected = Type.Missing;
                object oParams = Type.Missing;
                cmd.Execute(out nRecordsAffected, ref oParams, (int)ADODB.ExecuteOptionEnum.adExecuteNoRecords);

                cmd = null;
                return true;
            }
            catch (Exception ex) {
                string data = "Function: InsertDataToTable<T>(T t, string table_name)\n";
                data += ex.ToString();
                mySupport.writeToDebugFile(data);
                return false;
            }
        }

        public int CountAllDataRowInTable(string table_name) {
            try {
                if (con == null || con.State != 1) this.OpenConnection();
                if (con.State != 1) return -1;

                string strSQL = $"SELECT * FROM {table_name}";
                rs = new ADODB.Recordset();
                rs.Open(strSQL, con);
                int row = 0;
                try {
                    object[,] dataRows = (object[,])rs.GetRows();
                    row = dataRows.GetLength(1);
                }
                catch { row = 0; }
                rs.Close();
                rs = null;

                return row;
            }
            catch (Exception ex) {
                string data = "Function: CountAllDataRowInTable(string table_name)\n";
                data += ex.ToString();
                mySupport.writeToDebugFile(data);
                return -1;
            }
        }

        public bool InsertIEnumerableDataToTable<T>(IEnumerable<T> ts, string table_name) {
            try {
                if (con == null || con.State != 1) this.OpenConnection();
                if (con.State != 1) return false;

                //Get properties of T
                Type itemType = typeof(T);
                var properties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                //Assign the connection to the command 
                cmd = new CommandClass();
                cmd.ActiveConnection = con;
                cmd.CommandType = CommandTypeEnum.adCmdText;

                foreach (var t in ts) {

                    //get field and value
                    string s1 = "", s2 = "";
                    foreach (var p in properties) {
                        s1 += string.Format("[{0}],", p.Name);
                        s2 += string.Format("'{0}',", p.GetValue(t, null));
                    }
                    s1 = s1.Substring(0, s1.Length - 1);
                    s2 = s2.Substring(0, s2.Length - 1);

                    //
                    cmd.CommandText = string.Format("INSERT INTO {0} VALUES({1})", table_name, s2);

                    //Execute the command 
                    object nRecordsAffected = Type.Missing;
                    object oParams = Type.Missing;
                    cmd.Execute(out nRecordsAffected, ref oParams, (int)ADODB.ExecuteOptionEnum.adExecuteNoRecords);
                }

                cmd = null;
                return true;
            }
            catch (Exception ex) {
                string data = "Function: InsertIEnumerableDataToTable<T>(IEnumerable<T> ts, string table_name)\n";
                data += ex.ToString();
                mySupport.writeToDebugFile(data);
                return false;
            }
        }

        public bool CheckDupplicate(string table_name, string field_name, string field_value, string ref_field_name, string ref_field_value) {
            try {
                if (con == null || con.State != 1) this.OpenConnection();
                if (con.State != 1) return false;

                string strSQL = $"SELECT * FROM {table_name} WHERE {field_name}='{field_value}' AND {ref_field_name}='{ref_field_value}' AND result='Passed'";
                rs = new ADODB.Recordset();
                rs.Open(strSQL, con);
                int row = 0;
                try {
                    object[,] dataRows = (object[,])rs.GetRows();
                    row = dataRows.GetLength(1);
                } catch { row = 0; }
                rs.Close();
                rs = null;

                return row > 0;
            } catch (Exception ex) {
                string data = "Function: CheckDupplicate(string table_name, string field_name, string field_value, string ref_field_name, string ref_field_value)\n";
                data += ex.ToString();
                mySupport.writeToDebugFile(data);
                return false;
            }
        }

        public bool QueryDeleteOrUpdate(string queryString) {
            try {
                if (con == null || con.State != 1) this.OpenConnection();
                if (con.State != 1) return false;

                //Assign the connection to the command 
                cmd = new CommandClass();
                cmd.ActiveConnection = con;
                cmd.CommandType = CommandTypeEnum.adCmdText;

                //delete row
                cmd.CommandText = queryString;

                //Execute the command 
                object nRecordsAffected = Type.Missing;
                object oParams = Type.Missing;
                cmd.Execute(out nRecordsAffected, ref oParams, (int)ADODB.ExecuteOptionEnum.adExecuteNoRecords);

                cmd = null;
                return true;
            }
            catch (Exception ex) {
                string data = "Function: QueryDeleteOrUpdate(string queryString)\n";
                data += ex.ToString();
                mySupport.writeToDebugFile(data);
                return false;
            }
        }

        public bool ExportQuery(string tableName, string export_file) {
            try {
                //init access file
                Access.Application oAccess = null;

                // Start a new instance of Access for Automation:
                oAccess = new Access.Application();
                oAccess.Visible = false;

                // Open a database in exclusive mode:
                oAccess.OpenCurrentDatabase(this.msDatabasePath);

                //transfer access data to excel file
                oAccess.DoCmd.TransferSpreadsheet(Access.AcDataTransferType.acExport, Access.AcSpreadSheetType.acSpreadsheetTypeExcel12, tableName, export_file, true);

                //close database
                oAccess.CloseCurrentDatabase();
                oAccess.Quit();

                Marshal.ReleaseComObject(oAccess);

                return true;
            }
            catch (Exception ex) {
                System.Windows.MessageBox.Show(ex.ToString());
                return false;
            }
        }

        #region support function

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        CatalogClass openDatabase() {
            CatalogClass catalog = new CatalogClass();
            con = new Connection();
            try {
                con.Open(this.conString);
                catalog.ActiveConnection = con;
            }
            catch (Exception ex) {
                catalog.Create(this.conString);
                string data = "Function: openDatabase\n";
                data += ex.ToString();
                mySupport.writeToDebugFile(data);
            }
            return catalog;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="catalog"></param>
        /// <returns></returns>
        ADOX.ColumnClass tableIDField(string colName, ADOX.CatalogClass catalog) {
            ADOX.ColumnClass column = new ADOX.ColumnClass();
            column.Name = colName; // The name of the column
            column.ParentCatalog = catalog;
            column.Type = ADOX.DataTypeEnum.adInteger; //Indicates a four byte signed integer.
            column.Properties["AutoIncrement"].Value = true; //Enable the auto increment property for this column.
            return column;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="catalog"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        ADOX.ColumnClass tableField(string colName, ADOX.CatalogClass catalog, ADOX.DataTypeEnum dataType) {
            ADOX.ColumnClass column = new ADOX.ColumnClass();
            column.Name = colName; // The name of the column
            column.ParentCatalog = catalog;
            column.Type = dataType;
            return column;
        }

        #endregion

    }
}
