/*  
 EXEC GetPermissions  
 @OrgId = '7D33224B-2445-4348-8307-08DC9E313551',  
 @RoleId= '229a0f1f-528a-44d1-b9aa-6b9b4526ee30'  
*/  
CREATE   PROC GetPermissions    
(    
 @OrgId UNIQUEIDENTIFIER,  
 @RoleId NVARCHAR(50)  
)    
AS    
BEGIN    
 SELECT p.Id,     
   m.ModuleName AS ModuleName,    
   p.PermissionName AS PermissionName,    
   p.Description AS PermissionDescription,    
   CASE WHEN rp.Id IS NOT NULL THEN 1 ELSE 0 END AS Selected    
FROM     
   [Permission] p    
   INNER JOIN [Module] m ON p.ModuleId = m.Id    
   LEFT JOIN [RolePermission] rp ON p.Id = rp.PermissionId AND rp.RoleId = @RoleId    
WHERE p.OrganizationId = @OrgId    
ORDER BY     
   m.ModuleName, p.PermissionName;  
END  
  
--select * from Permission  
--select * from RolePermission  
--select * from AspNetRoles where Id='229a0f1f-528a-44d1-b9aa-6b9b4526ee30'  
  
--insert into RolePermission (Id, RoleId, PermissionId,  CreatedBy, CreatedOn)  
--select NEWID(), '229a0f1f-528a-44d1-b9aa-6b9b4526ee30', 'A282887F-E3AE-4957-A70B-6C9F8EB6F622',  'admin', GETUTCDATE()