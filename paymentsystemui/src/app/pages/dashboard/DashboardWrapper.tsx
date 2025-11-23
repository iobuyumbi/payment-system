import { FC, useEffect, useState } from "react";
import { useIntl } from "react-intl";
import { PageTitle } from "../../../_metronic/layout/core";
import { Content } from "../../../_metronic/layout/components/content";
import GetStarted from "../../components/GetStarted";
import KeyMetrics from "../../modules/reports/_shared/KeyMetrics";
import ReportService from "../../../services/ReportService";
import { getRoles } from "../../../_metronic/helpers/AppUtil";
import InitiatorDashboard from "./initiator/InitiatorDashboard";
import ReviewerDashboard from "./reviewer/ReviewerDashboard";
import ApproverDashboard from "./approver/ApproverDashboard";
import { ActivityLog } from "../../../_shared/Widgets/ActivityLog";
import PaymentBatchPieChart from "../../modules/reports/PaymentBatchPieChart";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";
import ModalChangePassword from "../../modules/auth/components/ModalChangePassword";
import { AUTH_LOCAL_STORAGE_KEY } from "../../modules/auth";
import PendingActions from "../../components/PendingActions";

const reportService = new ReportService();

const DashboardPage: FC = () => {
  const [pageData, setPageData] = useState<any>();
  const [showForcePassword, setShowForcePassword] = useState(true);

  useEffect(() => {
    document.title = `Dashboard - SD Pay`;
    const roles = getRoles();

    const bindReport = async () => {
      const response = await reportService.getStats(new Date().getFullYear());
      if (response) setPageData(response);
    };

    bindReport();
  }, []);

  useEffect(() => {
    const storedUser = localStorage.getItem(AUTH_LOCAL_STORAGE_KEY);
    const userData = JSON.parse(storedUser!);

    if (userData) {
      setShowForcePassword(userData.requiresPasswordChange);
    }
  }, []);

  return (
    <Content>
      <div>
        <div className="row g-5 g-xl-10 mb-5 mb-xl-10">
          {/* Counters */}
          {isAllowed("dashboard.viewcounters") && (
            <div className="col-md-12 col-lg-12 col-xl-12 col-xxl-12">
              <KeyMetrics keyMetrics={pageData} className="mb-3" />
            </div>
          )}

          {/* Getting Started */}
          {isAllowed("dashboard.viewgettingstarted") && (
            <>
              <div className="col-md-8 col-lg-8 col-xl-8 col-xxl-8">
                <GetStarted
                  icon="media/svg/brand-logos/plurk.svg"
                  badgeColor="primary"
                  className="shadow"
                />
              </div>

              <div className="col-md-4 col-lg-4 col-xl-4 col-xxl-4"
              >
                <PendingActions />
              </div>
            </>
          )}

          {/* Pie Chart - Payments */}
          {isAllowed("dashboard.viewpaymentbatchesgraph") && (
            <div className="col-md-4 col-lg-4 col-xl-4 col-xxl-4">
              <PaymentBatchPieChart />
            </div>
          )}

          {/* Country Widget */}
          <div className="col-md-12 col-lg-12 col-xl-12 col-xxl-12">
            {/* <CountryWidget className='card border my-3' /> */}
          </div>
        </div>

        {/* Activity Log */}
        {/* {isAllowed('dashboard.viewactivitylog') &&
          <div className="row">
            <div className="col-md-12">
              <ActivityLog />
            </div>
          </div>
        } */}
      </div>
      {showForcePassword == true && (
        <ModalChangePassword
          open={showForcePassword}
          onClose={() => setShowForcePassword(false)}
        />
      )}
    </Content>
  );
};

const DashboardWrapper: FC = () => {
  const intl = useIntl();

  return (
    <>
      <PageTitle breadcrumbs={[]}>
        {intl.formatMessage({ id: "MENU.DASHBOARD" })}
      </PageTitle>
      <DashboardPage />
    </>
  );
};

export { DashboardWrapper };
