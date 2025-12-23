namespace Solidaridad.DataAccess.Persistence.Seeding.Permission;

public static class Permissions
{
    public static List<string> GenerateAllPermissions()
    {
        return new List<string>
        {
            // Dashboard
            "dashboard.view",
            "dashboard.viewactivitylog",
            "dashboard.viewcounters",
            "dashboard.viewgettingstarted",
            "dashboard.viewpaymentbatchesgraph",
            
            // Farmers
            "farmers.view",
            "farmers.create",
            "farmers.edit",
            "farmers.delete",
            "farmers.import",
            "farmers.kyc.view",
            
            // Loans
            "loans.batch.view",
            "loans.applications.view",
            "loans.products.view",
            "loans.terms.view",
            
            // Payments
            "payments.batch.history",
            "payments.batch.create",
            "payments.batch.process",
            
            // Reports
            "reports.general.loan-accounts",
            "reports.loan.portfolio",
            "reports.payment.summary",
            
            // Settings - Loans
            "settings.loans.categories.view",
            "settings.loans.items.view",
            "settings.loans.masterfee.view",
            "settings.loans.terms.view",
            
            // Settings - System
            "settings.system.users.view",
            "settings.system.users.groups.view",
            
            // Settings - Communication
            "settings.communication.email.setup",
            "settings.communication.sms.setup",
            "settings.communication.email.template.view",
            
            // Settings - Other
            "settings.projects.view",
            "settings.cooperatives.view",
            "settings.administrative.view",
            
            // Logs
            "logs.email",
            "logs.sms",
            "logs.audit",
            "logs.contacts-api",
            "logs.payments-api",
            "logs.payments-callbacks"
        };
    }
    
    public static class Farmers
    {
        public const string View = "farmers.view";
        public const string Create = "farmers.create";
        public const string Edit = "farmers.edit";
        public const string Delete = "farmers.delete";
        public const string Import = "farmers.import";
        public const string KycView = "farmers.kyc.view";
    }
    
    public static class Loans
    {
        public const string BatchView = "loans.batch.view";
        public const string ApplicationsView = "loans.applications.view";
        public const string ProductsView = "loans.products.view";
        public const string TermsView = "loans.terms.view";
    }
    
    public static class Payments
    {
        public const string BatchHistory = "payments.batch.history";
        public const string BatchCreate = "payments.batch.create";
        public const string BatchProcess = "payments.batch.process";
    }
    
    public static class Reports
    {
        public const string GeneralLoanAccounts = "reports.general.loan-accounts";
        public const string LoanPortfolio = "reports.loan.portfolio";
        public const string PaymentSummary = "reports.payment.summary";
    }
    
    public static class Settings
    {
        public static class Loans
        {
            public const string CategoriesView = "settings.loans.categories.view";
            public const string ItemsView = "settings.loans.items.view";
            public const string MasterFeeView = "settings.loans.masterfee.view";
            public const string TermsView = "settings.loans.terms.view";
        }
        
        public static class System
        {
            public const string UsersView = "settings.system.users.view";
            public const string UsersGroupsView = "settings.system.users.groups.view";
        }
        
        public static class Communication
        {
            public const string EmailSetup = "settings.communication.email.setup";
            public const string SmsSetup = "settings.communication.sms.setup";
            public const string EmailTemplateView = "settings.communication.email.template.view";
        }
        
        public const string ProjectsView = "settings.projects.view";
        public const string CooperativesView = "settings.cooperatives.view";
        public const string AdministrativeView = "settings.administrative.view";
    }
    
    public static class Logs
    {
        public const string Email = "logs.email";
        public const string Sms = "logs.sms";
        public const string Audit = "logs.audit";
        public const string ContactsApi = "logs.contacts-api";
        public const string PaymentsApi = "logs.payments-api";
        public const string PaymentsCallbacks = "logs.payments-callbacks";
    }
    
    public static class Dashboard
    {
        public const string View = "dashboard.view";
        public const string ViewActivityLog = "dashboard.viewactivitylog";
    }
}
