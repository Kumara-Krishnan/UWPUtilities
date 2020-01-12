using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPUtilities.Model
{
    public abstract class SettingsBase
    {
        [PrimaryKey]
        public virtual string Id { get; set; }

        public string UserId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public string GroupId { get; set; }

        public SettingsBase() { }

        public SettingsBase(string userId, string key, string value)
        {
            Id = $"{userId}_{key}";
            UserId = userId;
            Key = key;
            Value = value;
        }

        public SettingsBase(string userId, string key, string value, string groupId) : this(userId, key, value)
        {
            GroupId = groupId;
        }
    }
}
