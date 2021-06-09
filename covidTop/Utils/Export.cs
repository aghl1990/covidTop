using covidTop.Models;
using covidTop.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;

namespace covidTop.Utils
{
    public class Export
    {
        public static string ToCsv(HttpResponseBase resp)
        {
            DataTable dtReport = ToDataTableReport();

            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = dtReport.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dtReport.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field =>
                  string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                sb.AppendLine(string.Join(",", fields));
            }
            resp.Clear();
            resp.Buffer = true;
            resp.AddHeader("content-disposition", "attachment;filename=CovidReport.csv");
            resp.Charset = "";
            resp.ContentType = "application/text";
            resp.Output.Write(sb);
            resp.Flush();
            resp.End();

            return "Successfully generated CSV file";
        }

        public static string ToJson(HttpResponseBase resp)
        {
            DataTable dtReport = ToDataTableReport();
            var listProduct = (
                from DataRow row in dtReport.Rows
                select new Report(
                   Convert.ToString(row["Iso"]),
                     Convert.ToString(row["Name"]),
                     row["Cases"] != null ? Convert.ToInt32(row["Cases"]) : 0,
                     row["Death"] != null ? Convert.ToInt32(row["Death"]) : 0
                )

            ).ToList();
            string jsonProductList = new JavaScriptSerializer().Serialize(listProduct);

            resp.ClearContent();
            resp.ClearHeaders();
            resp.Buffer = true;
            resp.ContentType = "application/json";
            resp.AddHeader("Content-Length", jsonProductList.Length.ToString());
            resp.AddHeader("Content-Disposition", "attachment; filename=CovidReport.json;");
            resp.Output.Write(jsonProductList);
            resp.Flush();
            resp.End();

            return "Successfully generated JSON file";
        }

        public static string ToXML(HttpResponseBase resp)
        {
            DataTable dtReport = ToDataTableReport();
            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("Report");
            xml.AppendChild(root);

            var reports = (
                 from DataRow row in dtReport.Rows
                 select new Report(
                               Convert.ToString(row["Iso"]),
                                 Convert.ToString(row["Name"]),
                                 row["Cases"] != null ? Convert.ToInt32(row["Cases"]) : 0,
                                 row["Death"] != null ? Convert.ToInt32(row["Death"]) : 0
                            )

             ).ToList();

            foreach (var item in reports)
            {
                XmlElement child = xml.CreateElement(Global.typeList);

                XmlElement Iso = xml.CreateElement(string.Empty, "Iso", string.Empty);
                XmlText textIso = xml.CreateTextNode(item.Iso);
                Iso.AppendChild(textIso);
                child.AppendChild(Iso);

                XmlElement Name = xml.CreateElement(string.Empty, "Name", string.Empty);
                XmlText textName = xml.CreateTextNode(item.Name);
                Name.AppendChild(textName);
                child.AppendChild(Name);

                XmlElement Cases = xml.CreateElement(string.Empty, "Cases", string.Empty);
                XmlText textCases = xml.CreateTextNode(item.Cases.ToString());
                Cases.AppendChild(textCases);
                child.AppendChild(Cases);

                XmlElement Deaths = xml.CreateElement(string.Empty, "Deaths", string.Empty);
                XmlText textDeaths = xml.CreateTextNode(item.Deaths.ToString());
                Deaths.AppendChild(textDeaths);
                child.AppendChild(Deaths);

                 
                root.AppendChild(child);
            }
            resp.ClearContent();
            resp.ClearHeaders();
            resp.Buffer = true;
            resp.ContentType = "application/xml";
            resp.AddHeader("Content-Disposition", "attachment; filename=CovidReport.xml;");
            resp.Output.Write(xml.OuterXml.ToString());
            resp.Flush();
            resp.End();

            return "Successfully generated XML file";
        } 
        public static DataTable ToDataTableReport()
        {
            DataTable dtReport = new DataTable("Report");
            dtReport.Columns.AddRange(new DataColumn[4] { new DataColumn("Iso"),
                                        new DataColumn("Name"),
                                        new DataColumn("Cases"),
                                        new DataColumn("Death") });

            foreach (var product in Global.lstReport.ToList())
            {
                dtReport.Rows.Add(product.Iso, product.Name, product.Cases, product.Deaths);
            }

            return dtReport;
        }
    }
}