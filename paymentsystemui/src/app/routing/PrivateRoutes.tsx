import { lazy, FC, Suspense } from "react";
import { Route, Routes, Navigate } from "react-router-dom";
import { MasterLayout } from "../../_metronic/layout/MasterLayout";
import TopBarProgress from "react-topbar-progress-indicator";
import { DashboardWrapper } from "../pages/dashboard/DashboardWrapper";
import { getCSSVariableValue } from "../../_metronic/assets/ts/_utils";
import { WithChildren } from "../../_metronic/helpers";
import ProtectedRoute from "./ProtectedRoute";
import AddProcessingFee from "../modules/processingFee/AddProcessingFee";
import { ListProcessingFee } from "../modules/processingFee/ListProcessingFee";
import { ListMasterLoanTerms } from "../modules/masterloanterms/ListMasterLoanTerms";
import { AddMasterLoanTerms } from "../modules/masterloanterms/AddMasterLoanTerms";
import InitiatorLandingPage from "../modules/landing-pages/InitiatorLandingPage";
import ApproverLandingPage from "../modules/landing-pages/ApproverLandingPage";
import ReviewerLandingPage from "../modules/landing-pages/ReviewerLandingPage";
import KycStatus from "../modules/kyc/KycStatus";
import JobExecutionLogs from "../pages/jobs/JobExecutionLogs";
import PrePaymentKyc from "../modules/kyc/PrePaymentKyc";
import ListAuditLog from "../pages/audit-log/ListAuditLog";
import { ActivityLog } from "../../_shared/Widgets/ActivityLog";
import ListLocations from "../modules/locations/ListLocations";
import AddLocation from "../modules/locations/AddLocation";
import ListLoanApplications from "../modules/loan-applications/ListLoanApplications";
import ChangePassword from "../pages/ChangePassword";
import TransactionsPage from "../modules/payment-processing/TransactionsPage";
import { AccessLog } from "../pages/access-log/AccessLog";

const PrivateRoutes = () => {
  const ProfilePage = lazy(() => import("../modules/profile/ProfilePage"));
  const WizardsPage = lazy(() => import("../modules/wizards/WizardsPage"));
  const AccountPage = lazy(() => import("../modules/accounts/AccountPage"));
  const WidgetsPage = lazy(() => import("../modules/widgets/WidgetsPage"));
  const ChatPage = lazy(() => import("../modules/apps/chat/ChatPage"));
  const ImportPage = lazy(() => import("../modules/import/ImportPage"));
  const ProjectsPage = lazy(() => import("../modules/projects/ProjectsPage"));
  const UsersPage = lazy(() => import("../modules/users/UsersPage"));
  const LoanItemsPage = lazy(() => import("../modules/loanitems/LoanItemsPage"));
  const FarmerPage = lazy(() => import("../modules/farmers/FarmersPage"));
  const LoansPage = lazy(() => import('../modules/loans/LoansPage'));
  const LoanBatchesPage = lazy(() => import('../modules/loanbatches/LoanBatchesPage'))
  const CooperativePage = lazy(() => import('../modules/cooperative/CooperativePage'))
  const ImportsPage = lazy(() => import('../modules/imports/ImportsPage'))
  const FarmerDetail = lazy(() => import('../modules/farmers/FarmerDetail'))
  //const PaymentBatchDetail = lazy(() => import('../modules/payments/PaymentBatchDetails_not_in_use'))
  const LoanBatchDetail = lazy(() => import('../modules/loanbatches/LoanBatchDetails'))
  const AdminLevelPage = lazy(() => import('../modules/adminlevel/AdminLevelPage'))
  const CategoryPage = lazy(() => import('../modules/category/CategoryPage'))
  const PaymentBatchesPage = lazy(() => import('../modules/payments/PaymentBatchPage'))
  const FacilitationPage = lazy(() => import('../modules/payments/FacilitationPage'))
  const ReportsPage = lazy(() => import("../modules/reports/ReportsPage"));
  const EmailPage = lazy(() => import("../modules/email/EmailPage"));
  const KycPage = lazy(() => import("../modules/kyc/KycPage"));
  const PaymentProcessingPage = lazy(() => import("../modules/payment-processing/PaymentProcessingPage"));

  return (
    <Routes>
      <Route element={<MasterLayout />}>
        {/* Redirect to Dashboard after success login/registartion */}
        <Route path="auth/*" element={<Navigate to="/dashboard" />} />
        {/* Pages */}
        <Route path="dashboard" element={<DashboardWrapper />} />
        <Route path="change-password" element={<ChangePassword />} />

        <Route path="payment-api" element={<TransactionsPage />} />
        <Route path="access-log" element={<AccessLog />} />
        <Route path="audit-log" element={<ListAuditLog />} />
        <Route path="audit-log" element={<ListAuditLog />} />

        <Route path="processing-fee" element={<ListProcessingFee />} />
        <Route path="processing-fee/add" element={<AddProcessingFee />} />
        <Route path="processing-fee/edit/:id" element={<AddProcessingFee />} />
        <Route path="master-loan-terms" element={<ListMasterLoanTerms />} />
        <Route path="master-loan-terms/add" element={<AddMasterLoanTerms />} />
        <Route path="master-loan-terms/edit/:id" element={<AddMasterLoanTerms />} />

        <Route path="payments/initiate" element={<InitiatorLandingPage />} />
        <Route path="payments/approve" element={<ApproverLandingPage />} />
        <Route path="payments/review" element={<ReviewerLandingPage />} />
        <Route path="kyc/kyc-status" element={<KycStatus />} />
        {/* <Route path="kyc/pre-payment-kyc/:batchId" element={<PrePaymentKyc />} /> */}
        <Route path="activity-log" element={<ActivityLog />} />

        <Route path="jobs" element={<JobExecutionLogs />} />
        <Route path="audit-log" element={<ListAuditLog />} />
        <Route path="locations" element={<ListLocations />} />
        <Route path="locations/add" element={<AddLocation />} />
        <Route path="locations/edit/:id" element={<AddLocation />} />

        <Route path="loan-applications" element={<ListLoanApplications />} />

        {/* Lazy Modules */}
        <Route
          path="farmer-detail/*"
          element={
            <SuspensedView>
              <FarmerDetail />
            </SuspensedView>
          }
        />
        <Route
          path="email/*"
          element={
            <SuspensedView>
              <EmailPage />
            </SuspensedView>
          }
        />
        <Route
          path="loan-batch-details/:batchId/*"
          element={
            <SuspensedView>
              <LoanBatchDetail />
            </SuspensedView>
          }
        />
        <Route
          path="import/*"
          element={
            <SuspensedView>
              <ImportPage />
            </SuspensedView>
          }
        />

        <Route
          path="projects/*"
          element={
            <SuspensedView>
              <ProjectsPage />
            </SuspensedView>
          }
        />

        <Route
          path="farmers/*"
          element={
            <SuspensedView>
              <FarmerPage />
            </SuspensedView>
          }
        />
        <Route
          path="items/*"
          element={
            <SuspensedView>
              <LoanItemsPage />
            </SuspensedView>
          }
        />
        <Route
          path="categories/*"
          element={
            <SuspensedView>
              <CategoryPage />
            </SuspensedView>
          }
        />
        <Route
          path="batches/*"
          element={
            <SuspensedView>
              <LoanBatchesPage />
            </SuspensedView>
          }
        />
        <Route
          path="payment-batch/*"
          element={
            <SuspensedView>
              <PaymentBatchesPage />
            </SuspensedView>
          }
        />
        <Route
          path="facilitation/*"
          element={
            <SuspensedView>
              <FacilitationPage />
            </SuspensedView>
          }
        />
        {/* <Route
          path="users/*"
          element={
            <SuspensedView>
              <UserPage />
            </SuspensedView>
          }
        /> */}
        <Route
          path='account-settings/*'
          element={
            <SuspensedView>
              <UsersPage />
            </SuspensedView>
          }
        />
        <Route
          path='adminlevel/*'
          element={
            <SuspensedView>
              <AdminLevelPage />
            </SuspensedView>
          }
        />
        <Route
          path='loans/*'
          element={
            <SuspensedView>
              <LoansPage />
            </SuspensedView>
          }
        />
        <Route
          path='cooperatives/*'
          element={
            <SuspensedView>
              <CooperativePage />
            </SuspensedView>
          }
        />

        <Route
          path="user/profile/*"
          element={
            <SuspensedView>
              <ProfilePage />
            </SuspensedView>
          }
        />
        <Route
          path="crafted/pages/wizards/*"
          element={
            <SuspensedView>
              <WizardsPage />
            </SuspensedView>
          }
        />
        <Route
          path="crafted/widgets/*"
          element={
            <SuspensedView>
              <WidgetsPage />
            </SuspensedView>
          }
        />
        <Route
          path="user/account/*"
          element={
            <SuspensedView>
              <AccountPage />
            </SuspensedView>
          }
        />
        <Route
          path="apps/chat/*"
          element={
            <SuspensedView>
              <ChatPage />
            </SuspensedView>
          }
        />
        <Route
          path="apps/user-management/*"
          element={
            <SuspensedView>
              <UsersPage />
            </SuspensedView>
          }
        />
        <Route
          path='imports/*'
          element={
            <SuspensedView>
              <ImportsPage />
            </SuspensedView>
          }
        />

        <Route element={<ProtectedRoute permission="farmers.add" redirectPath={""} />}>
          <Route
            path="reports/*"
            element={
              <SuspensedView>
                <ReportsPage />
              </SuspensedView>
            }
          />
        </Route>

        <Route element={<ProtectedRoute permission="payment.read" redirectPath={""} />}>
          <Route
            path="kyc/*"
            element={
              <SuspensedView>
                <KycPage />
              </SuspensedView>
            }
          />
        </Route>

        <Route>
          <Route
            path="payment-processing/*"
            element={
              <SuspensedView>
                <PaymentProcessingPage />
              </SuspensedView>
            }
          />
        </Route>

        {/* Page Not Found */}
        <Route path="*" element={<Navigate to="/error/404" />} />
      </Route>
    </Routes>
  );
};

const SuspensedView: FC<WithChildren> = ({ children }) => {
  const baseColor = getCSSVariableValue("--bs-primary");
  TopBarProgress.config({
    barColors: {
      "0": baseColor,
    },
    barThickness: 1,
    shadowBlur: 5,
  });
  return <Suspense fallback={<TopBarProgress />}>{children}</Suspense>;
};

export { PrivateRoutes };
