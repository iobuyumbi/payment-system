namespace Solidaridad.DataAccess.Persistence.Seeding.Permission;

public static class Permissions
{
    public static List<string> GeneratePermissionsForModule(string module)
    {
        return new List<string>()
            {
                $"Permissions.{module}.Create",
                $"Permissions.{module}.View",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete",
                $"Permissions.{module}.Import",
            };
    }
    public static class Farmers
    {
        public const string View = "Permissions.Farmers.View";
        public const string Create = "Permissions.Farmers.Create";
        public const string Edit = "Permissions.Farmers.Edit";
        public const string Delete = "Permissions.Farmers.Delete";
        public const string Import = "Permissions.Farmers.Import";
    }
}
