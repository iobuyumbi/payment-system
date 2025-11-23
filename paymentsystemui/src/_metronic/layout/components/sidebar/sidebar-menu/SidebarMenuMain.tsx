import { useIntl } from "react-intl";
import { SidebarMenuItemWithSub } from "./SidebarMenuItemWithSub";
import { SidebarMenuItem } from "./SidebarMenuItem";
import { isAllowed } from "../../../../helpers/ApiUtil";

const SidebarMenuMain = () => {
  const intl = useIntl();
  const hasVisibleLoanItems =
    isAllowed('settings.loans.categories.view') ||
    isAllowed('settings.loans.items.view') ||
    isAllowed('settings.loans.masterfee.view') ||
    isAllowed('settings.loans.terms.view');

  const hasVisibleUsersItems =
    isAllowed('settings.system.users.groups.view') ||
    isAllowed('settings.system.users.view');

  const hasVisibleComm =
    isAllowed('settings.communication.email.setup') ||
    isAllowed('settings.communication.sms.setup');

  const hasVisibleSettings =
    hasVisibleLoanItems ||
    hasVisibleUsersItems ||
    hasVisibleComm;

  return (
    <>
      <SidebarMenuItem to="/dashboard" icon="element-11" title={intl.formatMessage({ id: "MENU.DASHBOARD" })} fontIcon="bi-app-indicator" visible={true} />

      {/* Farmers */}
      <SidebarMenuItemWithSub to="/farmers" title="Farmers" fontIcon="bi-people" icon="people" visible={isAllowed("farmers.view")}>
        <SidebarMenuItem
          to="/farmers"
          title="List Farmers"
          hasBullet={true}
          visible={isAllowed("farmers.view")}
        />
        <SidebarMenuItem
          to="/kyc/kyc-status"
          title="KYC Status"
          hasBullet={true}
          visible={isAllowed("farmers.kyc.view")}
        />
      </SidebarMenuItemWithSub>

      {/* Loans */}
      <SidebarMenuItem to="/loans" icon="cheque" title="Loan Products" fontIcon="bi-people" visible={isAllowed('loans.batch.view')} />
{/* Loans */}
      <SidebarMenuItem to="/loan-applications" icon="cheque" title="Loan Applications" fontIcon="bi-people" visible={isAllowed('loans.applications.view')} />
      {/* Payment Batches */}
      <SidebarMenuItemWithSub to="/payment-batch" title="Payments" fontIcon="bi-sliders" icon="tag" visible={isAllowed('payments.batch.history')}>
        <SidebarMenuItem
          to="/payment-batch"
          title="Payment Batches"
          hasBullet={true}
          visible={isAllowed('payments.batch.history')}
        />
      </SidebarMenuItemWithSub>

      {/* Reports */}
  <SidebarMenuItemWithSub
  to="/reports"
  title="Reports & Analytics"
  fontIcon="bi-sliders"
  icon="chart-pie-4"
  visible={isAllowed('reports.general.loan-accounts')}
>
  {/* Loan Reports */}
  <SidebarMenuItemWithSub
    to=""
    title="Loan Reports"
    fontIcon="bi-sliders"
    icon="chart-pie-4"
    visible={isAllowed('reports.general.loan-accounts')}
  >
     

    <SidebarMenuItem
      to="/reports/loan-portfolio-summary"
      title="Loan Portfolio Summary"
      hasBullet={true}
      visible={isAllowed('reports.general.loan-accounts')}
    />

    <SidebarMenuItem
      to="/reports/country-loan-portfolio-summary"
      title="Country Loan Portfolio Summary"
      hasBullet={true}
      visible={isAllowed('reports.general.loan-accounts')}
    />
    <SidebarMenuItem
      to="/reports/global-loan-portfolio-summary"
      title="Global Loan Portfolio Summary"
      hasBullet={true}
      visible={isAllowed('reports.general.loan-accounts')}
    />
     <SidebarMenuItem
      to="/reports/global-loan-application-summary"
      title="Global Loan Application Summary"
      hasBullet={true}
      visible={isAllowed('reports.general.loan-accounts')}
    />


    <SidebarMenuItem
      to="/reports/disbursed-loan-report"
      title="Loan Disbursement Report"
      hasBullet={true}
      visible={isAllowed('reports.general.loan-accounts')}
    />
    
    {/* <SidebarMenuItem
      to="/reports/loan-performance-summary"
      title="Loan Account Reports"
      hasBullet={true}
      visible={isAllowed('reports.general.loan-accounts')}
    /> */}
  </SidebarMenuItemWithSub>

  {/* Payment Reports */}
  <SidebarMenuItemWithSub
    to=""
    title="Payment Reports"
    fontIcon="bi-sliders"
    icon="chart-pie-4"
    visible={isAllowed('reports.general.loan-accounts')}
  >
    <SidebarMenuItem
      to="/reports/payment-batch-summary"
      title="Payment Batch Reports"
      hasBullet={true}
      visible={isAllowed('reports.general.loan-accounts')}
    />
  </SidebarMenuItemWithSub>
</SidebarMenuItemWithSub>

      

      {/* Imports */}
      <SidebarMenuItem to="/imports" icon="exit-up" title="Imports" fontIcon="bi-layers" visible={isAllowed('farmers.import')} />

      {/* Logs */}
      <SidebarMenuItemWithSub to="/" title="Logs" fontIcon="bi-sliders" icon="gear" visible={isAllowed('logs.email')} >
        {/* <SidebarMenuItem to="/#email" title="Email" hasBullet={true} visible={isAllowed('logs.email')} />
        <SidebarMenuItem to="/sms" title="SMS" hasBullet={true} visible={isAllowed('logs.sms')} />
        <SidebarMenuItem to="/audit" title="Audit" hasBullet={true} visible={isAllowed('logs.audit')} /> */}
        <SidebarMenuItem to="/access-log" title="System access Logs" hasBullet={true} visible={isAllowed('logs.contacts-api')} />
        <SidebarMenuItem to="/audit-log?q=LoanApplications" title="Loans Logs" hasBullet={true} visible={isAllowed('logs.payments-api')} />
        <SidebarMenuItem to="/audit-log?q=PaymentBatch" title="Payment logs" hasBullet={true} visible={isAllowed('logs.payments-callbacks')} />
        <SidebarMenuItem to="/payment-api" title="API calls logs" hasBullet={true} visible={isAllowed('logs.payments-api')} />
      </SidebarMenuItemWithSub>

      {/* Settings */}
      <SidebarMenuItemWithSub to="/settings" title="Settings" fontIcon="bi-sliders" icon="gear" visible={hasVisibleSettings}>

        <SidebarMenuItemWithSub to="/categories" title="Loan Settings" hasBullet={true} visible={hasVisibleLoanItems}>
          <SidebarMenuItem to="/categories" title="Item Categories" hasBullet={true} visible={isAllowed('settings.loans.categories.view')} />
          <SidebarMenuItem to="/loans/items" title="Items" hasBullet={true} visible={isAllowed('settings.loans.items.view')} />
          <SidebarMenuItem to="/processing-fee" title="Master Additional Fee" hasBullet={true} visible={isAllowed('settings.loans.masterfee.view')} />
          <SidebarMenuItem to="/master-loan-terms" title="Master Loan Terms" hasBullet={true} visible={isAllowed('settings.loans.terms.view')} />
        </SidebarMenuItemWithSub>

        {/* Projects */}
        <SidebarMenuItem to="/projects" title="Projects" hasBullet={true} visible={isAllowed('settings.projects.view')} />

        {/* Locations */}
        {/* <SidebarMenuItem to="/locations" title="Locations" hasBullet={true} visible={isAllowed('settings.projects.view')} /> */}

        {/* Users & Roles */}
        <SidebarMenuItemWithSub to="/account-settings" title="Users & Roles" hasBullet={true} visible={hasVisibleUsersItems}>
          <SidebarMenuItem to="/account-settings/roles" title="User Groups" hasBullet={true} visible={isAllowed('settings.system.users.groups.view')} />
          <SidebarMenuItem to="/account-settings/users" title="Users" hasBullet={true} visible={isAllowed('settings.system.users.view')} />
        </SidebarMenuItemWithSub>

        {/* Cooperatives */}
        <SidebarMenuItem to="/cooperatives" title="Cooperatives" hasBullet={true} visible={isAllowed('settings.cooperatives.view')} />

        {/* Administrative Setup */}
        <SidebarMenuItem to="/adminlevel" title="Administrative Setup" hasBullet={true} visible={isAllowed('settings.administrative.view')} />

        {/* Communication */}
        <SidebarMenuItemWithSub to="/communication" title="Communication" hasBullet={true} visible={hasVisibleComm}>
          <SidebarMenuItem to="/#smtp-setup" title="SMTP Setup" hasBullet={true} visible={isAllowed('settings.communication.email.setup')} />
          <SidebarMenuItem to="/#sms-setup" title="SMS Setup" hasBullet={true} visible={isAllowed('settings.communication.sms.setup')} />
        </SidebarMenuItemWithSub>

        <SidebarMenuItem to="/adminlevel" title="Payment Setup (API Key/Token)" hasBullet={true} visible={false} />
        <SidebarMenuItem to="/email/templates" title="Email Templates" hasBullet={true} visible={isAllowed('settings.communication.email.template.view')} />
        <SidebarMenuItem to="/activity-log" title="Activity Log" hasBullet={true} visible={isAllowed('dashboard.viewactivitylog')} />

      </SidebarMenuItemWithSub>
    </>
  );
};

export { SidebarMenuMain };
