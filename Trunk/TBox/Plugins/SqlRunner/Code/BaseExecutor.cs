using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Windows;
using SqlRunner.Code.Settings;
using Common.Tools;
using WPFControls.Dialogs;

namespace SqlRunner.Code
{
	public class BaseExecutor
	{
		public void Execute(Window owner, Op operation, string connectionString, Action<DatabaseInfo> onEnd)
		{
			DialogsCache.ShowProgress(
				u => onEnd(GetResult(connectionString, operation)), 
				"Make request to: " + operation.Key, owner);
		}

		public static DatabaseInfo GetResult(string connectionString, Op operation)
		{
			var sw = new Stopwatch();
			sw.Start();
			try
			{
				var table = new List<IList<string>>(); 
				using (var connection = new SqlConnection(connectionString))
				{
					connection.Open();
					using (var tr = connection.BeginTransaction())
					{
						using (var cmd = new SqlCommand(operation.Command, connection))
						{
							cmd.CommandTimeout = operation.Timeout;
							cmd.Transaction = tr;
							var sb = new StringBuilder();
							using (var r = cmd.ExecuteReader())
							{
								sb.AppendLine("Records affected: " + r.RecordsAffected);
								if (r.VisibleFieldCount > 0)
								{
									table.Add(new List<string>());
									for (var i = 0; i < r.VisibleFieldCount; ++i)
									{
										table[0].Add(r.GetName(i));
									}
									while (r.Read())
									{
										var list = new List<string>();
										table.Add(list);
										for (var i = 0; i < r.VisibleFieldCount; ++i)
										{
											list.Add(r[i].ToString());
										}
									}
									sb.FormatTable(table);
								}
							}
							tr.Commit();
							
							return new DatabaseInfo
							{
								Status = "OK",
								Body = sb.ToString(),
								Time = sw.ElapsedMilliseconds
							};
						}
					}
				}
			}
			catch (Exception ex)
			{
				return new DatabaseInfo {Status = "Exception", Body = ex.Message, Time = sw.ElapsedMilliseconds};
			}
		}
	}
}
