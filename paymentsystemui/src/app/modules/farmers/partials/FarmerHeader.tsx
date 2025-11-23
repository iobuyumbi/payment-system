import { Link, useLocation } from "react-router-dom";
import { Content } from "../../../../_metronic/layout/components/content";
import { KTIcon } from "../../../../_metronic/helpers";
import { ToolbarWrapper } from "../../../../_metronic/layout/components/toolbar";
import { isAllowed } from "../../../../_metronic/helpers/ApiUtil";
import { Error401 } from "../../errors/components/Error401";
import moment from "moment";
import config from "../../../../environments/config";
import { useState } from "react";
import AuditListModal from "../../../../_shared/Modals/AuditListModal";
import FarmerLoans from "./FarmerLoans";
import { FarmerLoanSchedule } from "./FarmerLoanSchedule";

const FarmerHeader = (props: any) => {
  const { farmer, farmerProjects, coperatives } = props;
  const [showAuditModal, setShowAuditModal] = useState(false);
  const [currentId, setCurrentId] = useState("");
  const location = useLocation();
  const afterConfirm = (res: any) => {
    setShowAuditModal(false);
  };
  return (
    <>
      <ToolbarWrapper />
      <Content>
        {isAllowed("farmers.view") ? (
          <>
            {" "}
            <div className="row">
              <div className="col-md-9">
                <div className="card">
                  <div className="card-header">
                    <div className="d-flex flex-row">
                      <h3 className="card-title align-items-start flex-column">
                        <span className="card-label fs-1 mb-1">
                          {" "}
                          {`${farmer?.firstName} ${farmer?.otherNames}`}
                        </span>
                        {/* <span className='text-muted mt-1 fw-semibold fs-7'>{loanApplication?.beneficiaryId}</span> */}
                      </h3>
                    </div>

                    <div className="card-toolbar">
                      {isAllowed("farmers.edit") ? (
                        <Link
                          to={`/farmers/edit/${farmer?.id}`}
                          className="btn btn-sm btn-secondary w-150px me-3"
                        >
                          <KTIcon
                            iconName="pencil"
                            className="fs-6"
                            iconType="outline"
                          />{" "}
                          Edit
                        </Link>
                      ) : (
                        ""
                      )}

                      {/* <Link
                      to={`/farmers/loans/${farmer.id}`}
                      className="btn btn-sm btn-outline btn-primary"
                    >
                      View Loans
                    </Link> */}
                      {isAllowed("farmers.view-audit") && (
                        <button
                          onClick={() => {
                            setShowAuditModal(true);
                            setCurrentId(props.data.id);
                          }}
                          className="btn btn-sm btn-outline btn-primary"
                        >
                          Audit Log
                        </button>
                      )}
                    </div>
                  </div>

                  <div className="card-body py-9 pb-0">
                    <div className="row mb-4">
                      <div className="col-md-12 mb-5">
                        <p className="fw-semibold text-gray-600 text-uppercase fs-6 ls-1 mb-3">
                          Basic Information
                        </p>
                        <div className="d-flex flex-row justify-content-justify">
                          <div className="me-3 w-25">
                            <div className="d-flex flex-column">
                              <label className="text-gray-600">Name</label>
                              <h6 className="fw-normal">
                                {farmer?.firstName} {farmer?.otherNames}
                              </h6>
                            </div>
                          </div>
                          <div className="me-3 w-25">
                            <div className="d-flex flex-column">
                              <label className="text-gray-600">
                                National ID
                              </label>
                              <h6 className="fw-normal">
                                {farmer?.beneficiaryId}
                              </h6>
                            </div>
                          </div>
                          <div className="me-3" style={{ width: "15%" }}>
                            <div className="d-flex flex-column">
                              <label className="text-gray-600">Gender</label>
                              <h6 className="fw-normal">{farmer?.gender}</h6>
                            </div>
                          </div>
                          <div className="me-3 w-15" style={{ width: "15%" }}>
                            <div className="d-flex flex-column">
                              <label className="text-gray-600">
                                Month/Year of Birth
                              </label>
                              <h6 className="fw-normal">
                                {farmer?.birthMonth}/{farmer?.birthYear}
                              </h6>
                            </div>
                          </div>
                          <div className="me-3">
                            <div className="d-flex flex-column">
                              <label className="text-gray-600">
                                Has Disability
                              </label>
                              <h6 className="fw-normal">
                                {farmer?.hasDisability}
                              </h6>
                            </div>
                          </div>
                        </div>
                        <div className="d-flex flex-row justify-content-justify mt-3">
                          <div className="me-3" style={{ width: "25%" }}>
                            <div className="d-flex flex-column">
                              <label className="text-gray-600">Country</label>
                              <h6 className="fw-normal">
                                {" "}
                                {farmer?.country?.countryName}
                              </h6>
                            </div>
                          </div>
                          <div className="me-3">
                            <div className="d-flex flex-column">
                              <label className="text-gray-600">
                                Project(s)
                              </label>
                              <h6 className="fw-normal"> {farmerProjects}</h6>
                            </div>
                          </div>
                        </div>
                      </div>

                      <div className="separator mb-5"></div>

                      <div className="col-md-12 mb-5">
                        <p className="fw-semibold text-gray-600 text-uppercase fs-6 ls-1 mb-3">
                          Contact Information
                        </p>
                        <div className="d-flex flex-row">
                          <div className="me-3 w-25">
                            <div className="d-flex flex-column">
                              <label className="text-gray-600">Mobile</label>
                              <h6 className="fw-normal">{farmer?.mobile}</h6>
                            </div>
                          </div>

                          <div className="me-3 w-25">
                            <div className="d-flex flex-column">
                              <label className="text-gray-600">
                                Alternate Number
                              </label>
                              <h6 className="fw-normal">
                                {farmer?.alternateContactNumber}
                              </h6>
                            </div>
                          </div>

                          <div className="me-3">
                            <div className="me-3">
                              <div className="d-flex flex-column">
                                <label className="text-gray-600">Email</label>
                                <h6 className="fw-normal">{farmer?.email}</h6>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>

                      <div className="separator mb-5"></div>

                      <div className="col-md-12 mb-5">
                        <p className="fw-semibold text-gray-600 text-uppercase fs-6 ls-1 mb-3">
                          Location
                        </p>
                        <div className="d-flex flex-row ">
                          <div className="me-3 w-25">
                            <div className="d-flex flex-column">
                              <label className="text-gray-600">County</label>
                              <h6 className="fw-normal">
                                {" "}
                                {farmer?.adminLevel1?.countyName}
                              </h6>
                            </div>
                          </div>

                          <div className="me-3 w-25">
                            <div className="d-flex flex-column">
                              <label className="text-gray-600">
                                Sub-County
                              </label>
                              <h6 className="fw-normal">
                                {" "}
                                {farmer?.adminLevel2?.subCountyName}
                              </h6>
                            </div>
                          </div>
                          <div className="me-3 w-25">
                            <div className="d-flex flex-column">
                              <label className="text-gray-600">Ward</label>
                              <h6 className="fw-normal">
                                {" "}
                                {farmer?.adminLevel3?.wardName}
                              </h6>
                            </div>
                          </div>

                          <div className="me-3">
                            <div className="d-flex flex-column">
                              <label className="text-gray-600">Village</label>
                              <h6 className="fw-normal"> {farmer?.village}</h6>
                            </div>
                          </div>
                        </div>
                      </div>

                      <div className="separator mb-5"></div>

                      <div className="col-md-12 mb-5">
                        <p className="fw-semibold text-gray-600 text-uppercase fs-6 ls-1 mb-3">
                          Other
                        </p>
                        <div className="d-flex flex-row">
                          <div className="me-3 w-25">
                            <div className="d-flex flex-column">
                              <label className="text-gray-600">
                                Co-operative(s)
                              </label>
                              <h6 className="fw-normal">
                                {" "}
                                {coperatives}
                              </h6>
                            </div>
                          </div>

                          <div className="me-3 w-25">
                            <div className="d-flex flex-column">
                              <label className="text-gray-600">
                                Uwanjani System ID
                              </label>
                              <h6 className="fw-normal"> {farmer?.systemId}</h6>
                            </div>
                          </div>

                          <div className="me-3 w-25">
                            <div className="d-flex flex-column">
                              <label className="text-gray-600">
                                Solidaridad Participant ID
                              </label>
                              <h6 className="fw-normal">
                                {" "}
                                {farmer?.participantId}
                              </h6>
                            </div>
                          </div>

                          <div className="me-3">
                            <div className="d-flex flex-column">
                              <label className="text-gray-600">
                                Enumeration date
                              </label>
                              <h6 className="fw-normal">
                                {" "}
                                {moment(farmer?.enumerationDate).format(
                                  config.dateOnlyFormat
                                )}
                              </h6>
                            </div>
                          </div>
                        </div>
                      </div>

                      <div className="separator mb-5"></div>
                      <div className="col-md-12 mb-5">
                        <p className="fw-semibold text-gray-600 text-uppercase fs-6 ls-1 mb-3">
                          Payment Information
                        </p>
                        <div className="d-flex flex-row">
                          <div className="me-3 w-25">
                            <div className="d-flex flex-column">
                              <label className="text-gray-600">
                                Payment Phone Number
                              </label>
                              <h6 className="fw-normal">
                                {" "}
                                {farmer.paymentPhoneNumber}
                              </h6>
                            </div>
                          </div>

                          <div className="me-3">
                            <div className="d-flex flex-column">
                              <label className="text-gray-600">
                                Is the farmer the phone owner? *
                              </label>
                              <h6 className="fw-normal">
                                {" "}
                                {farmer?.isFarmerPhoneOwner ? "Yes" : "No"}
                              </h6>
                            </div>
                          </div>
                        </div>
                      </div>
                      <div className="separator mb-5"></div>
                    </div>

                    <div className="d-flex">
                      <ul className="nav nav-stretch nav-line-tabs border-transparent fs-5 fw-bolder flex-nowrap">
                        {/* <li className="nav-item">
                          <Link
                            className={`nav-link text-gray-800 me-6 ${
                              location.pathname === "/farmer-detail/loans"
                                ? "active"
                                : ""
                            }`}
                            to="/farmer-detail/loans"
                          >
                            Loans
                          </Link>
                        </li>
                        <li className="nav-item">
                          <Link
                            className={`nav-link text-gray-800 me-6 ${
                              location.pathname ===
                              "/farmer-detail/loan-schedule"
                                ? "active"
                                : ""
                            }`}
                            to="/farmer-detail/loan-schedule"
                          >
                            Loan Schedule
                          </Link>
                        </li> */}

                        {/* Uncomment below if needed */}
                        {/*
        <li className="nav-item">
          <Link
            className={`nav-link text-gray-800 me-6 ${location.pathname === "/farmer-detail/documents" ? "active" : ""}`}
            to="/farmer-detail/documents"
          >
            Documents
          </Link>
        </li>
        <li className="nav-item">
          <Link
            className={`nav-link text-gray-800 me-6 ${location.pathname === "/farmer-detail/location" ? "active" : ""}`}
            to="/farmer-detail/location"
          >
            Location
          </Link>
        </li>
        */}
                      </ul>
                    </div>
                  </div>
                </div>
              </div>
              <div className="col-md-3">
                <div className="bg-white border rounded p-9 m-2">
                  <h2>Wallet balance:{farmer.walletBalance} KES</h2>
                  {/* <p >Last updated: 22 Sept, 2024</p> */}
                  <div className="py-5"></div>
                </div>
              </div>
            </div>
            <div className="row my-3">
             
                <FarmerLoans farmer={farmer} />
            </div>
          </>
        ) : (
          <Error401 />
        )}
        {showAuditModal && (
          <AuditListModal
            exModule={"Farmer"}
            componentId={currentId}
            onClose={afterConfirm}
          />
        )}
      </Content>
    </>
  );
};

export default FarmerHeader;
