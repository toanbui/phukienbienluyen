using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace Utilities
{
    public class GeneratorCode
    {
        protected string _tableName = "";
        protected List<string> Excludes = new List<string>() { "CreatedBy", "ModifiedBy", "Id", "Created", "Modified" };
        private string StringFormat = @"<div class=""form-group row""> <label class=""col-sm-2 col-form-label"">{{Name}}</label> <div class=""col-sm-10 ""> @Html.TextBoxFor(x => x.{{Table}}.{{Name}}, new { @class = ""col-xs-12 col-sm-12 form-control"", @placeholder = """" }) </div> </div>";
        private string IntFormat = @"<div class=""form-group row""> <label class=""col-sm-2 col-form-label"">{{Name}}</label> <div class=""col-sm-10 ""> @Html.TextBoxFor(x => x.{{Table}}.{{Name}}, new { @class = ""col-xs-12 col-sm-12 form-control autonumber int"", @placeholder = """" }) </div> </div>";
        private string BooleanFormat = @"<div class=""form-group row""> <label class=""col-sm-2 col-form-label"">{{Name}}</label> <div class=""col-sm-10 border-checkbox-section""> <div class=""border-checkbox-group border-checkbox-group-primary""> @Html.CheckBox(""{{Table}}.{{Name}}"", Model.{{Table}}.{{Name}} == true, new { @class= ""border-checkbox""}) <label class=""border-checkbox-label"" for=""{{Table}}_{{Name}}""></label> </div> </div> </div>";
        private string DateTimeFormat = @"<div class=""form-group row""> <label class=""col-sm-2 col-form-label"">{{Name}}</label><div class=""col-sm-10""> <div class=""input-group""> @Html.TextBoxFor(x => x.{{Table}}.{{Name}}, new {@class =""form-control date-picker"", @placeholder = """" , @data_date_format = ""dd/mm/yyyy"" }) <span class=""input-group-addon""> <i class=""fa fa-calendar bigger-110""></i> </span> </div> </div> </div>";
        private string StatusFormat = @"<div class=""form-group row""> <label class=""col-sm-2 col-form-label"">Trạng thái</label> <div class=""col-sm-10""> @Html.DropDownListFor(x => x.{{Table}}.{{Name}}, ViewBag.Status as IEnumerable<SelectListItem>, new { @class = ""form-control"", @placeholder = """" , init_select2 = ""true"", data_selected = Model.{{Table}}.{{Name}}, data_placeholder = ""Chọn trạng thái"" }) </div> </div>";
        private string DescriptionFormat = @"<div class=""form-group row""> <label class=""col-sm-2 col-form-label"">{{Name}}</label> <div class=""col-sm-10 ""> @Html.TextAreaFor(x => x.{{Table}}.{{Name}}, new { @class = ""col-xs-12 col-sm-12 form-control ckeditor"", @placeholder = """" }) </div> </div>";
        private string ImageFormat = @"<div class=""form-group row""> <label class=""col-sm-2 col-form-label"">{{Name}}</label> <div class=""col-sm-10 ""> <div class=""row""> <div class=""col-sm-12""> <input type=""file"" name=""{{Name}}Input"" class=""actFile form-control"" /> </div> <div class=""col-sm-12""> @if (!string.IsNullOrEmpty(Model.{{Table}}.{{Name}})) { <img src=""@Model.{{Table}}.{{Name}}"" style=""height: 100px; width: 100px;"" /> } @Html.HiddenFor(x => x.{{Table}}.{{Name}}) </div> </div> </div> </div>";
        StringBuilder sbView = new StringBuilder();
        StringBuilder sbGet = new StringBuilder();
        StringBuilder sbSet = new StringBuilder();
        private string GetFormat = @"{{Name}} = n.{{Name}},";
        private string SetFormat = @"dbItem.{{Name}} = item.{{Name}};";
        public GeneratorCode(string tableName)
        {
            _tableName = tableName;
            if (string.IsNullOrEmpty(tableName))
                return;
            GenIndex();
            GenCreate();
            GenDelete();
            GenController();
            GenCode();
        }
        public void GenIndex()
        {
            string Index_Path = HostingEnvironment.MapPath("/configs/IndexFormat.txt");
            string INDEX_FORMAT = "";
            using (StreamReader sr = new StreamReader(Index_Path))
            {
                String line = sr.ReadToEnd();
                INDEX_FORMAT = line;
            }
            if (!string.IsNullOrEmpty(INDEX_FORMAT))
            {
                var content = INDEX_FORMAT.Replace("[TABLE_NAME]", _tableName);
                string outputFile = HostingEnvironment.MapPath(string.Format("/Views/Admin/{0}/Index.cshtml", _tableName));
                string outputPath = HostingEnvironment.MapPath(string.Format("/Views/Admin/{0}", _tableName));
                new CreateLogFiles().WriteFile(outputPath, outputFile, content);
            }
        }
        public void GenCreate()
        {
            string Index_Path = HostingEnvironment.MapPath("/configs/CreateFormat.txt");
            string INDEX_FORMAT = "";
            using (StreamReader sr = new StreamReader(Index_Path))
            {
                String line = sr.ReadToEnd();
                INDEX_FORMAT = line;
            }
            if (!string.IsNullOrEmpty(INDEX_FORMAT))
            {
                var content = INDEX_FORMAT.Replace("[TABLE_NAME]", _tableName);
                string outputFile = HostingEnvironment.MapPath(string.Format("/Views/Admin/{0}/Create.cshtml", _tableName));
                string outputPath = HostingEnvironment.MapPath(string.Format("/Views/Admin/{0}", _tableName));

                //Tạo input 
                var dt = GetData(string.Format("Select * from {0}", _tableName));


                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    var name = dt.Columns[i].ColumnName;
                    var type = dt.Columns[i].DataType.ToString();

                    sbGet.AppendLine(GetFormat.Replace("{{Name}}", name));
                    //Set
                    sbSet.AppendLine(SetFormat.Replace("{{Name}}", name));

                    if (name.ToLower() == "id" || Excludes.Contains(name))
                        continue;

                    if (type == "System.String")
                    {
                        if (name == "Image" || name == "FullImage")
                            sbView.AppendLine(ImageFormat.Replace("{{Table}}", _tableName).Replace("{{Name}}", name));
                        else if (name == "Description")
                            sbView.AppendLine(DescriptionFormat.Replace("{{Table}}", _tableName).Replace("{{Name}}", name));
                        else
                            sbView.AppendLine(StringFormat.Replace("{{Table}}", _tableName).Replace("{{Name}}", name));

                    }
                    else if (type == "System.Int32" || type == "System.Double")
                    {
                        if (name == "Status")
                        {
                            sbView.AppendLine(StatusFormat.Replace("{{Table}}", _tableName).Replace("{{Name}}", name));
                        }
                        else
                            sbView.AppendLine(IntFormat.Replace("{{Table}}", _tableName).Replace("{{Name}}", name));
                    }
                    else if (type == "System.Boolean")
                    {
                        sbView.AppendLine(BooleanFormat.Replace("{{Table}}", _tableName).Replace("{{Name}}", name));
                    }
                    else if (type == "System.DateTime")
                    {
                        sbView.AppendLine(DateTimeFormat.Replace("{{Table}}", _tableName).Replace("{{Name}}", name));
                    }

                }
                content = content.Replace("[COLLUMN]", sbView.ToString());
                new CreateLogFiles().WriteFile(outputPath, outputFile, content);
            }
        }
        public void GenDelete()
        {
            string Index_Path = HostingEnvironment.MapPath("/configs/DeleteFormat.txt");
            string INDEX_FORMAT = "";
            using (StreamReader sr = new StreamReader(Index_Path))
            {
                String line = sr.ReadToEnd();
                INDEX_FORMAT = line;
            }
            if (!string.IsNullOrEmpty(INDEX_FORMAT))
            {
                var content = INDEX_FORMAT.Replace("[TABLE_NAME]", _tableName);
                string outputFile = HostingEnvironment.MapPath(string.Format("/Views/Admin/{0}/Delete.cshtml", _tableName));
                string outputPath = HostingEnvironment.MapPath(string.Format("/Views/Admin/{0}", _tableName));

                new CreateLogFiles().WriteFile(outputPath, outputFile, content);
            }
        }
        public void GenController()
        {
            string Index_Path = HostingEnvironment.MapPath("/configs/ControllerFormat.txt");
            string INDEX_FORMAT = "";
            using (StreamReader sr = new StreamReader(Index_Path))
            {
                String line = sr.ReadToEnd();
                INDEX_FORMAT = line;
            }
            if (!string.IsNullOrEmpty(INDEX_FORMAT))
            {
                var content = INDEX_FORMAT.Replace("[TABLE_NAME]", _tableName);
                string outputFile = HostingEnvironment.MapPath(string.Format("/Controllers/Admin/{0}Controller.cs", _tableName));
                string outputPath = HostingEnvironment.MapPath("/Controllers/Admin");

                new CreateLogFiles().WriteFile(outputPath, outputFile, content);
            }
        }
        public void GenCode()
        {
            var list = new List<string>() { "Bo", "Dao", "Param", "Info" , "Filter" };
            for (int i = 0; i < list.Count; i++)
            {
                string Index_Path = HostingEnvironment.MapPath(string.Format("/configs/{0}Format.txt", list[i]));
                string INDEX_FORMAT = "";
                using (StreamReader sr = new StreamReader(Index_Path))
                {
                    String line = sr.ReadToEnd();
                    INDEX_FORMAT = line;
                }
                if (!string.IsNullOrEmpty(INDEX_FORMAT))
                {
                    var content = INDEX_FORMAT.Replace("[TABLE_NAME]", _tableName);

                    if (list[i] == "Dao")
                        content = content.Replace("[DB_UPDATE]", sbSet.ToString()).Replace("[DB_GET]", sbGet.ToString());
                    var path = HostingEnvironment.MapPath("/MvcProject");
                    var outputFile = "";
                    var outputPath = "";
                    switch (list[i])
                    {
                        case "Bo":
                            {
                                outputFile = string.Format("{0}\\BO\\{1}Bo.cs", path.Replace("\\MvcProject\\MvcProject\\MvcProject", "\\MvcProject"), _tableName);
                                outputPath = string.Format("{0}\\BO\\", path.Replace("\\MvcProject\\MvcProject\\MvcProject", "\\MvcProject"));
                                //new CreateLogFiles().WriteFile(outputPath, outputFile, content);
                                break;
                            }
                        case "Dao":
                            {
                                outputFile = string.Format("{0}\\DAO\\{1}Dao.cs", path.Replace("\\MvcProject\\MvcProject\\MvcProject", "\\MvcProject"), _tableName);
                                outputPath = string.Format("{0}\\DAO\\", path.Replace("\\MvcProject\\MvcProject\\MvcProject", "\\MvcProject"));
                                break;
                            }
                        case "Param":
                            {
                                outputFile = string.Format("{0}\\Entities\\Param\\{1}Param.cs", path.Replace("\\MvcProject\\MvcProject\\MvcProject", "\\MvcProject"), _tableName);
                                outputPath = string.Format("{0}\\Entities\\Param", path.Replace("\\MvcProject\\MvcProject\\MvcProject", "\\MvcProject"));
                                break;
                            }
                        case "Info":
                            {
                                outputFile = string.Format("{0}\\Entities\\Entities\\{1}Entity.cs", path.Replace("\\MvcProject\\MvcProject\\MvcProject", "\\MvcProject"), _tableName);
                                outputPath = string.Format("{0}\\Entities\\Entities", path.Replace("\\MvcProject\\MvcProject\\MvcProject", "\\MvcProject"));
                                break;
                            }
                        case "Filter":
                            {
                                outputFile = string.Format("{0}\\Entities\\Filter\\{1}Filter.cs", path.Replace("\\MvcProject\\MvcProject\\MvcProject", "\\MvcProject"), _tableName);
                                outputPath = string.Format("{0}\\Entities\\Filter", path.Replace("\\MvcProject\\MvcProject\\MvcProject", "\\MvcProject"));
                                break;
                            }
                        default:
                            break;
                    }
                    //var outputFile = HostingEnvironment.MapPath(string.Format("/Source/{0}/Code/{0}{1}.cs", _tableName, list[i]));
                    //string outputPath = HostingEnvironment.MapPath(string.Format("/Source/{0}/Code", _tableName));
                    
                    new CreateLogFiles().WriteFile(outputPath, outputFile, content);
                }
            }

        }
        public static DataTable GetData(string command)
        {
            var sqlconn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["KeyDbContext"].ConnectionString ?? "");
            if (sqlconn.State == ConnectionState.Closed)
                sqlconn.Open();

            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand(command,sqlconn);
            cmd.CommandType = CommandType.Text;
            new SqlDataAdapter(cmd).Fill(dataTable);
            sqlconn.Close();
            return dataTable;
        }
    }
}
