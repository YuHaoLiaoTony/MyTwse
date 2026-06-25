using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace MyTwse.Infrastructure
{
    public static class SqliteHelper
    {
        public static List<T> ExecSQL<T>(DbContext db, string query, object parameters = null) where T : class
        {
            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                if (parameters != null)
                {
                    foreach (var prop in parameters.GetType().GetProperties())
                    {
                        var value = prop.GetValue(parameters);
                        var param = command.CreateParameter();
                        param.ParameterName = $"@{prop.Name}";
                        param.Value = value ?? DBNull.Value;
                        command.Parameters.Add(param);
                    }
                }

                db.Database.OpenConnection();

                var list = new List<T>();
                using (var result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        var obj = Activator.CreateInstance<T>();
                        foreach (PropertyInfo prop in typeof(T).GetProperties())
                        {
                            if (!object.Equals(result[prop.Name], DBNull.Value))
                            {
                                prop.SetValue(obj, result[prop.Name], null);
                            }
                        }
                        list.Add(obj);
                    }
                }

                db.Database.CloseConnection();
                return list;
            }
        }
    }
}
