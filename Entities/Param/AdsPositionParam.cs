using Entities.Base;
using Entities.Entities;
using Entities.Filter;
using System.Collections.Generic;

namespace Entities.Param
{
    public class AdsPositionParam : BaseParam
    {
        public AdsPosition AdsPosition { get; set; }
        public List<AdsPosition> AdsPositions { get; set; }
        public AdsPositionEntity AdsPositionEntity { get; set; }
        public List<AdsPositionEntity> AdsPositionEntitys { get; set; }
        public AdsPositionFilter AdsPositionFilter { get; set; }
    }
}
