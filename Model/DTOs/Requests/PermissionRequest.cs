namespace Model.DTOs.Requests
{
    public class PermissionRequest
    {
        public int UserGroupId { get; set; }
        public List<RoleDTO> Roles{ get; set; }
    }
}
