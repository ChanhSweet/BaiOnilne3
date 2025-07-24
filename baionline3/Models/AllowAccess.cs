namespace baionline3.Models
{
    public class AllowAccess
    {
        public int Id { get; set; }

        public int RoleId { get; set; }
        public Role? Role { get; set; }

        public string TableName { get; set; } = string.Empty;
        public string AccessProperties { get; set; } = string.Empty; // CSV: "InternName,DateOfBirth,University"
        public int UserId { get; internal set; }
        public object ColumnName { get; internal set; }
    }
}
