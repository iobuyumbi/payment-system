CREATE   PROCEDURE usp_MergeRolePermissions  
    @roleId nvarchar(100),  
    @json nvarchar(max)  
AS  
BEGIN  
    -- Delete permissions that are no longer present for that role  
    DELETE rp  
    FROM [RolePermission] rp  
    WHERE rp.RoleId = @roleId;  
  
 -- Insert permissions  
 INSERT INTO [RolePermission](Id, RoleId, PermissionId, CreatedOn, CreatedBy)  
 SELECT   
            NEWID(),  
   @roleId AS RoleId,  
            p.Id AS PermissionId,  
   GETUTCDATE(),  
   'Admin'  
        FROM   
            OPENJSON(@json)  
            WITH (  
                PermissionName nvarchar(150),  
    ModuleName nvarchar(50)  
            ) AS j  
        INNER JOIN [Permission] p ON j.PermissionName = p.PermissionName  
  INNER JOIN [Module] m ON j.ModuleName = m.moduleName  
  
    SELECT 'Permissions updated successfully' AS [Message];  
END;  