using System;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public class FsAclEntry
    {
        public FsAclType Type;
        public string Name;
        public FsPermission? Permission;

        public override string ToString()
        {
            var rwx = this.Permission.Value.ToRwxString();
            var aclType = AclTypeToString(this.Type);

            string s = String.Format("{0}:{1}:{2}", aclType, this.Name, rwx);
            return s;
        }

        private FsAclEntry()
        {
            
        }

        public FsAclEntry(FsAclType type, string name, FsPermission permission)
        {
            this.Type = type;

            if (name == null)
            {
                name = "";
            }
            this.Name = name;
            this.Permission = permission;
        }

        public FsAclEntry(string entry)
        {
            var tokens = entry.Split(':');

            string type_str = tokens[0].ToLowerInvariant();
            string user = tokens[1];
            if ((type_str == "user") && (user.Length == 0))
            {
                this.Type = FsAclType.OwningUser;
            }
            else if ((type_str == "user") && (user.Length > 0))
            {
                this.Type = FsAclType.NamedUser;
            }
            else if ((type_str == "group") && (user.Length == 0))
            {
                this.Type = FsAclType.OwningGroup;
            }
            else if ((type_str == "group") && (user.Length > 0))
            {
                this.Type = FsAclType.NamedGroup;
            }
            else if (type_str == "mask")
            {
                this.Type = FsAclType.Mask;
            }
            else if (type_str == "other")
            {
                this.Type = FsAclType.Other;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }

            this.Name = user;
            this.Permission = new FsPermission(tokens[2]);
        }

        public static string AclTypeToString(FsAclType type)
        {
            string aclType = "ERROR";
            if (type == FsAclType.Mask)
            {
                aclType = "mask";
            }
            else if (type == FsAclType.NamedGroup)
            {
                aclType = "group";
            }
            else if (type == FsAclType.NamedUser)
            {
                aclType = "user";
            }
            else if (type == FsAclType.Other)
            {
                aclType = "other";
            }
            else if (type == FsAclType.OwningGroup)
            {
                aclType = "group";
            }
            else if (type == FsAclType.OwningUser)
            {
                aclType = "user";
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
            return aclType;
        }

        public FsAclEntry AndWith(FsPermission mask)
        {
            var new_perm = this.Permission.Value.AndWith(mask);
            return this.CloneWithPermission(new_perm);
        }

        public FsAclEntry OrWith(FsPermission mask)
        {
            var new_perm = this.Permission.Value.OrWith(mask);
            return this.CloneWithPermission(new_perm);
        }

        public FsAclEntry CloneWithPermission(FsPermission permission)
        {
            var new_entry = new FsAclEntry(this.Type, this.Name, permission);
            return new_entry;
        }

        public static string EntriesToString(IEnumerable<FsAclEntry> entries)
        {
            var strings = entries.Select(e => e.ToString());
            var s = String.Join(",", strings);
            return s;
        }
    }
}