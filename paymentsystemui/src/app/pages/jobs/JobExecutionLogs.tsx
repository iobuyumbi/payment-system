import { useEffect, useState } from 'react';
import { KTCard, KTCardBody, toAbsoluteUrl } from '../../../_metronic/helpers'
import CustomTable from '../../../_shared/CustomTable/Index'
import ReportService from '../../../services/ReportService';
import { Content } from '../../../_metronic/layout/components/content';
import { PageLink, PageTitle } from '../../../_metronic/layout/core';
import { PageTitleWrapper } from '../../../_metronic/layout/components/toolbar/page-title';

const reportService = new ReportService();
const breadCrumbs: Array<PageLink> = [
  {
    title: 'Dashboard',
    path: '/dashboard',
    isSeparator: false,
    isActive: true,
  },
  {
    title: '',
    path: '',
    isSeparator: true,
    isActive: true,
  },
];

const JobExecutionLogs = () => {
  const [rowData, setRowData] = useState<any>();

  const [colDefs, setColDefs] = useState<any>([
    { field: "jobName", flex: 1 },
    { field: "startTime", flex: 1 },
    { field: "endTime", flex: 1 },
    { field: "isSuccess", flex: 1 },
    { field: "errorMessage", flex: 1 },
  ]);

  const bindLogs = async () => {
    const response = await reportService.getJobExecutionLog();
    setRowData(response);
  }

  useEffect(() => {
    bindLogs();
  }, []);

  return (<Content>
    <PageTitleWrapper />
    <PageTitle breadcrumbs={breadCrumbs}>Scheduled Jobs</PageTitle>
    {/* <KTCard className="shadow mt-10">
      <KTCardBody>
        <div className="d-flex align-items-center justify-content-between">
          <div className='d-flex flex-center  '>
            <div className='px-5'>
              <h1 className='fs-1 text-gray-900 fw-normal'>Interest Calculation</h1>
              <span className='fs-4 text-gray-600'>
                Calculates annual interest and effective principal for loans
              </span>
            </div>
          </div>
          <div>
            <div className='fs-3 text-gray-700'>Runs on 1st Jan every year</div>
          </div>
        </div>
      </KTCardBody>
    </KTCard> */}
    <KTCard className="shadow mt-10">
      <CustomTable
        rowData={rowData}
        colDefs={colDefs}
        header=""
      />
    </KTCard>
  </Content>
  )
}

export default JobExecutionLogs
