import { Content } from '../../../_metronic/layout/components/content';
import { PageLink, PageTitle } from '../../../_metronic/layout/core';
import { PageTitleWrapper } from '../../../_metronic/layout/components/toolbar/page-title';
import { KTCard, KTCardBody } from '../../../_metronic/helpers';
import { useEffect, useState } from 'react';
import { Tab, Tabs } from 'react-bootstrap';
import LoanSchedule from '../farmers/partials/LoanSchedule';
import { useParams } from 'react-router-dom';
import LoanApplicationService from '../../../services/LoanApplicationService';

import LoanRepaymentHistory from './partials/LoanRepaymentHistory';
import LoanSummary from './LoanSummary';
import LoanInterestSimulator from './LoanInterestSimulator';
import { FarmerLoanSchedule } from '../farmers/partials/FarmerLoanSchedule';

const loanApplicationService = new LoanApplicationService();

const breadCrumbs: Array<PageLink> = [
  {
    title: "Loans",
    path: "/loans",
    isSeparator: false,
    isActive: true,
  },
];

const LoanManager = () => {
  const { loanApplicationId } = useParams();
  const [loanData, setLoanData] = useState<any>(null);
  const [activeTab, setActiveTab] = useState<string>("_schedule");
 

  const showDetails = (id: any) => {
    setTimeout(async () => {
      const response = await loanApplicationService.getEmiSchedule(id);
      setLoanData(response);
    }, 500);
  }

  useEffect(() => {
    showDetails(loanApplicationId);
  }, [loanApplicationId]);
 

  return (
    <div>
      <Content>
        <PageTitle breadcrumbs={breadCrumbs}>Loan Details</PageTitle>
        <PageTitleWrapper />
        <KTCard className='shadow mt-10'>
          <KTCardBody>
            <LoanSummary loanData={loanData} />
          </KTCardBody>
        </KTCard>

        <KTCard className='shadow mt-10'>
          <KTCardBody>
            <Tabs
              id="tab_loans"
              activeKey={activeTab}
              onSelect={(k: string | null) => k && setActiveTab(k)}
              className="mb-5 custom-tabs"
            >
              {/* Tab 1: Schedule */}
              <Tab eventKey="_schedule"
                title={<span className="custom-tab-title">EMI Schedule</span>}>
               <FarmerLoanSchedule loanApplicationId={loanApplicationId}  />
              </Tab>

              {/* Tab 2: Schedule */}
              <Tab eventKey="_transactions"
                title={<span className="custom-tab-title">Transactions</span>}>
                <LoanInterestSimulator loanApplicationId={loanApplicationId} />
              </Tab>

              {/* Tab 3: Repayments */}
              <Tab eventKey="_repayments"
                title={<span className="custom-tab-title">Repayments</span>}>
                <LoanRepaymentHistory loanApplicationId={loanApplicationId} />
              </Tab>
            </Tabs>
          </KTCardBody>
        </KTCard>

      </Content>
    </div>
  )
}

export default LoanManager
