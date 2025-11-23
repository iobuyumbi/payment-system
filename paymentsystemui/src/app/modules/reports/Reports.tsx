import { Fragment } from "react";
import { Content } from "../../../_metronic/layout/components/content";
import { PageTitleWrapper } from "../../../_metronic/layout/components/toolbar/page-title";
import { PageLink, PageTitle } from "../../../_metronic/layout/core";
import { Link } from "react-router-dom";
import { reportSetupData } from "./data/ReportsData";

const profileBreadCrumbs: Array<PageLink> = [
  {
    title: "Reports",
    path: "/reports",
    isSeparator: false,
    isActive: true,
  },
];
const Reports = () => {
  return (
    <Content>
      <PageTitleWrapper />
      <PageTitle breadcrumbs={profileBreadCrumbs}>Reports</PageTitle>

      <div className="row">
        {reportSetupData &&
          reportSetupData.map((item) => (
            <div className="col-md-6">
              <div key={item.id} className="card card-xl-stretch mb-5 mb-xl-8">
                <div className="card-header border">
                  <h3 className="card-title fw-bold text-gray-900">
                    {item.name}
                  </h3>
                  <div className="toolbar mt-5">
                    <Link  to={item.link} className="btn btn-sm btn-primary">View Report</Link>
                  </div>
                </div>
                <div className="card-body">
                  {item.reports.map((report, index) => (
                    <Fragment key={`lw26-rows-${index}`}>
                      <ul className="d-flex flex-stack">
                        <li
                         
                          className="fw-semibold fs-5 me-2"
                        >
                          {report.name}
                        </li>
                      </ul>
                      {item.reports.length - 1 > index && (
                        <div className="separator separator-dashed my-3" />
                      )}
                    </Fragment>
                  ))}
                </div>
              </div>
            </div>
          ))}
      </div>
    </Content>
  );
};

export { Reports };
