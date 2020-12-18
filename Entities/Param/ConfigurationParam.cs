using Entities.Base;
using Entities.Entities;
using Entities.Filter;
using System.Collections.Generic;

namespace Entities.Param
{
    public class ConfigurationParam : BaseParam
    {
        public Configuration Configuration { get; set; }
        public List<Configuration> Configurations { get; set; }
        public ConfigurationEntity ConfigurationEntity { get; set; }
        public List<ConfigurationEntity> ConfigurationEntitys { get; set; }
        public ConfigurationFilter ConfigurationFilter { get; set; }
    }
}
