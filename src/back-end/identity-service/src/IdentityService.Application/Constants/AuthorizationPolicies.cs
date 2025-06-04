namespace IdentityService.Application.Constants;

public static class AuthorizationPolicies
{
    public const string CanViewGroup = nameof(CanViewGroup);
    public const string CanApproveGroup = nameof(CanApproveGroup);
    public const string CanEditGroup = nameof(CanEditGroup);
    public const string CanDeleteGroup = nameof(CanDeleteGroup);
    public const string CanViewPermission = nameof(CanViewPermission);
    public const string CanViewRole = nameof(CanViewRole);
    public const string CanEditRole = nameof(CanEditRole);
    public const string CanDeleteRole = nameof(CanDeleteRole);
    public const string CanAssignRolePermission = nameof(CanAssignRolePermission);

    public static readonly Dictionary<string, string> Map = new()
    {
        [CanViewGroup] = "CAN_VIEW_GROUP",
        [CanApproveGroup] = "CAN_APPROVE_GROUP",
        [CanEditGroup] = "CAN_EDIT_GROUP",
        [CanDeleteGroup] = "CAN_DELETE_GROUP",
        [CanViewPermission] = "CAN_VIEW_PERMISSION",
        [CanViewRole] = "CAN_VIEW_ROLE",
        [CanEditRole] = "CAN_EDIT_ROLE",
        [CanDeleteRole] = "CAN_DELETE_ROLE",
        [CanAssignRolePermission] = "CAN_ASSIGN_ROLE_PERMISSION",
    };
}
