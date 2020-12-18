using Entities.Base;
using Entities.Entities;
using Entities.Filter;
using System.Collections.Generic;

namespace Entities.Param
{
    public class LiveChatHistoryParam : BaseParam
    {
        public LiveChatHistory LiveChatHistory { get; set; }
        public List<LiveChatHistory> LiveChatHistorys { get; set; }
        public LiveChatHistoryEntity LiveChatHistoryEntity { get; set; }
        public List<LiveChatHistoryEntity> LiveChatHistoryEntitys { get; set; }
        public LiveChatHistoryFilter LiveChatHistoryFilter { get; set; }
    }
}
