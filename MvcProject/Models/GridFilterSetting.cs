using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcProject.Models
{
    public class GridFilterSetting<TEntity> where TEntity : class
    {

        public int iColumns { get; set; }

        public int iDisplayLength { get; set; }

        public int iDisplayStart { get; set; }

        public string sColumns { get; set; }

        public string sEcho { get; set; }
        public string sSearch { get; set; }
    }
}