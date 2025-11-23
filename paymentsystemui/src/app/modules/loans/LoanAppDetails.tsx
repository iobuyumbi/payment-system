import { useEffect, useState } from "react";
import { Content } from "../../../_metronic/layout/components/content";
import { KTIcon, toAbsoluteUrl } from "../../../_metronic/helpers";
import { PageLink, PageTitle } from "../../../_metronic/layout/core";
import { PageTitleWrapper } from "../../../_metronic/layout/components/toolbar/page-title";
import { useSelector } from "react-redux";
import { LoanItem } from "./partials/LoanItem";
import LoanApplicationsModal from "../loanbatches/partials/LoanApplicationsModal";
import { Tab, Tabs } from "react-bootstrap";
import { round } from "lodash";
import { getAPIBaseUrl, isAllowed } from "../../../_metronic/helpers/ApiUtil";
import LoanApplicationService from "../../../services/LoanApplicationService";
import StatusModal from "./partials/StatusModal";
import moment from "moment";
import LoanSummary from "./LoanSummary";
import AuditListModal from "../../../_shared/Modals/AuditListModal";
import { IConfirmModel } from "../../../_models/confirm-model";
import { ConfirmBox } from "../../../_shared/Modals/ConfirmBox";
import { FarmerLoanSchedule } from "../farmers/partials/FarmerLoanSchedule";
import { AppStatusBadge } from "../../../_shared/Status/AppStatusBadge";
import LoanStatement from "./partials/LoanStatement";
import { ApplicationHistory } from "./partials/ApplicationHistory";
import { useNavigate } from "react-router-dom";
import LoanRepaymentHistory from "./partials/LoanRepaymentHistory";

const loanApplicationService = new LoanApplicationService();
const API_URL = getAPIBaseUrl();

const breadCrumbs: Array<PageLink> = [
  {
    title: "Dashboard",
    path: "/dashboard",
    isSeparator: false,
    isActive: true,
  },
  {
    title: "",
    path: "",
    isSeparator: true,
    isActive: false,
  },
  {
    title: "Loans",
    path: "/loans",
    isSeparator: false,
    isActive: true,
  },
  {
    title: "",
    path: "",
    isSeparator: true,
    isActive: false,
  },
  {
    title: "Loan Applications",
    path: "/loan-applications",
    isSeparator: false,
    isActive: true,
  },
];

const LoanAppDetails = ({ loanBatch }: any) => {
  const navigate = useNavigate();
  const loanApplication: any = useSelector(
    (state: any) => state?.loanApplications
  );
  console.log("loanApplication", loanApplication);
  const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
  const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
  const [showItemBox, setShowItemBox] = useState<boolean>(false);
  const [showStatusBox, setShowStatusBox] = useState<boolean>(false);
  const [attachments, setAttachments] = useState<any>([]);
  const [loanData, setLoanData] = useState<any>(null);
  const [activeTab, setActiveTab] = useState<string>("_schedule");
  const [showAuditModal, setShowAuditModal] = useState(false);
  const [loading, setLoading] = useState(false);
  const [info, setInfo] = useState<any>(null);

  const afterConfirm = (value: any) => {
    navigate("/loan-applications");
    window.location.reload();
    // setShowStatusBox(false);
    // setShowItemBox(false);
    // setShowConfirmBox(false);
    // setShowAuditModal(false);
  };

  const [fullImage, setFullImage] = useState(null);
  const handleImageClick = (imagePath: any) => {
    setFullImage(imagePath);
  };

  const downloadHandler = async (filePath: any) => {
    try {
      const baseUrl = getAPIBaseUrl();
      const completeUrl = baseUrl + "wwwroot/" + filePath;
      const response = await fetch(completeUrl);

      // Check if the request was successful
      if (!response.ok) {
        throw new Error("Failed to fetch file");
      }

      // Convert the response to a Blob
      const blob = await response.blob();

      // Create a URL for the Blob
      const url = window.URL.createObjectURL(blob);

      // Create a temporary anchor element
      const a = document.createElement("a");
      a.href = url;
      a.download = filePath.split("/").pop();
      document.body.appendChild(a);
      a.click();

      // Clean up
      document.body.removeChild(a);
      window.URL.revokeObjectURL(url);
    } catch (error) {}
  };

  const bindAttachments = async () => {
    const response = await loanApplicationService.getApplicationDocumentsData(
      loanApplication.id
    );

    if (response) {
      setAttachments(response);
    }
  };

  useEffect(() => {
    bindAttachments();
  }, []);

  useEffect(() => {
    const bindLoanSchedule = async () => {
      const response = await loanApplicationService.getEmiSchedule(
        loanApplication.id
      );
      console.log("Loan Schedule Response", response);
      setLoanData(response);
    };

    bindLoanSchedule();
  }, [loanApplication.id]);

  useEffect(() => {}, [info]);

  const openDeleteModal = (id: any) => {
    if (id) {
      const confirmModel: IConfirmModel = {
        title: "Delete loan application",
        btnText: "Delete this application?",
        deleteUrl: `api/LoanApplication/${id}`,
        message: "delete-application",
      };

      setConfirmModel(confirmModel);
      setTimeout(() => {
        setShowConfirmBox(true);
      }, 500);
    }
  };

  return (
    <div>
      <Content>
        <PageTitle breadcrumbs={breadCrumbs}>Loan Application</PageTitle>
        <PageTitleWrapper />

        <div className="accordion mt-4" id="kt_accordion_1">
          <div className="accordion-item">
            <h2 className="accordion-header" id="kt_accordion_1_header_1">
              <button
                className="accordion-button fs-4 fw-bold"
                type="button"
                data-bs-toggle="collapse"
                data-bs-target="#kt_accordion_1_body_1"
                aria-expanded="false"
                aria-controls="kt_accordion_1_body_1"
              >
                {loanApplication?.farmer?.fullName} :{" "}
                {loanApplication.loanNumber}
                <span className="ms-2">
                  <AppStatusBadge value={loanApplication?.statusText} />
                </span>
              </button>
            </h2>
            <div
              id="kt_accordion_1_body_1"
              className="accordion-collapse"
              aria-labelledby="kt_accordion_1_header_1"
              data-bs-parent="#kt_accordion_1"
            >
              <div className="accordion-body">
                <div className="card-body position-relative">
                  <div className=" d-flex flex-row pt-3 pb-5 justify-content-between">
                    <div className="fw-semibold text-gray-600 text-uppercase fs-6 ls-1">
                      general Information
                    </div>

                    <div className="card-toolbar">
                      {isAllowed("loan.edit") &&
                      loanApplication.inUse !== true ? (
                        <button
                          onClick={() => setShowStatusBox(true)}
                          className="btn btn-sm btn-primary w-150px me-3"
                        >
                          Update Status
                        </button>
                      ) : (
                        ""
                      )}

                      {/* Edit */}
                      {isAllowed("loans.applications.edit") &&
                        loanApplication.inUse !== true &&
                        loanApplication?.statusText !== "Disbursed" && (
                          <button
                            onClick={() => setShowItemBox(true)}
                            className="btn btn-sm btn-secondary w-150px me-3"
                          >
                            Edit
                          </button>
                        )}

                      {/* Audit log */}
                      {isAllowed("loans.applications.view-audit") && (
                        <button
                          onClick={() => {
                            setShowAuditModal(true);
                            //setCurrentId(props.data.id);
                          }}
                          className="btn btn-sm btn-secondary w-150px me-3"
                        >
                          Audit Log
                        </button>
                      )}

                      {/* Delete */}
                      {isAllowed("loans.applications.delete") &&
                        loanApplication.inUse !== true &&
                        loanApplication?.statusText !== "Disbursed" && (
                          <button
                            className="btn btn-sm btn-secondary w-150px me-3"
                            onClick={() => openDeleteModal(loanApplication.id)}
                          >
                            <KTIcon
                              iconName="trash"
                              iconType="outline"
                              className="text-danger fs-4"
                            />
                          </button>
                        )}
                    </div>
                  </div>
                  <div className="separator mt-2 mb-3"></div>
                  <div className="row mb-7">
                    <div className="col-lg-4">
                      <label className="text-gray-600">Full Name</label>
                      <h6 className="fw-normal">
                        {loanApplication?.farmer?.fullName}
                      </h6>
                    </div>
                    <div className="col-lg-4">
                      <label className="text-gray-600">Mobile</label>
                      <h6 className="fw-normal">
                        {loanApplication?.farmer?.mobile}
                      </h6>
                    </div>
                    <div className="col-lg-4">
                      <label className="text-gray-600">Loan Product</label>
                      <h6 className="fw-normal">
                        {loanApplication?.loanBatch?.name}
                      </h6>
                    </div>
                  </div>

                  <div className="row mb-7">
                    <div className="col-lg-4">
                      <label className="text-gray-600">Uwanjani ID</label>
                      <h6 className="fw-normal">
                        {loanApplication?.farmer?.systemId}
                      </h6>
                    </div>
                    <div className="col-lg-4">
                      <label className="text-gray-600">Fee Charged</label>
                      <h6 className="fw-normal">
                        {loanData?.application?.feeApplied?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
                      </h6>
                    </div>
                    <div className="col-lg-4">
                      <label className="text-gray-600">Total Item Count</label>
                      <h6 className="fw-normal">
                        {loanApplication.loanItems.length}
                      </h6>
                    </div>
                  </div>

                  <div className="row mb-7">
                    <div className="col-lg-4">
                      <label className="text-gray-600">Remaining Balance</label>
                      <h6 className="fw-normal">
                        {loanData?.application?.principalAmount -
                          loanData?.application?.feeApplied -
                          info?.principalAmount >=
                        0
                          ? info?.balance?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 })
                          : info?.principalAmount?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 })
                          ? info?.principalAmount?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 })
                          : 0}
                      </h6>
                    </div>
                    <div className="col-lg-4">
                      <label className="text-gray-600">Total Repayments</label>
                      <h6 className="fw-normal">
                        {Math.max(
                          Number(
                            (loanData?.application?.principalReceived ?? 0) +
                              (loanData?.application?.interestReceived ?? 0) 
                          ),
                          0
                        )?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
                      </h6>
                    </div>
                    <div className="col-lg-4">
                      <label className="text-gray-600">Total Interest</label>
                      <h6 className="fw-normal">
                        {(
                          loanData?.schedule[0]?.interestAmount *
                          loanApplication?.loanBatch?.tenure
                        )?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 }) ?? 0}
                      </h6>
                    </div>
                  </div>

                  <div className="row mb-7">
                    <div className="col-lg-4">
                      <label className="text-gray-600">Rate Type</label>
                      <h6 className="fw-normal">
                        {loanApplication?.loanBatch?.rateType}
                      </h6>
                    </div>
                    <div className="col-lg-4">
                      <label className="text-gray-600">Tenure</label>
                      <h6 className="fw-normal">
                        {loanApplication?.loanBatch?.tenure}
                      </h6>
                    </div>
                    <div className="col-lg-4">
                      <label className="text-gray-600">Interest Rate</label>
                      <h6 className="fw-normal">
                        {loanApplication?.loanBatch?.interestRate} %
                      </h6>
                    </div>
                  </div>

                  <div className="row mb-7">
                    <div className="col-lg-4">
                      <label className="text-gray-600">Grace Period</label>
                      <h6 className="fw-normal">
                        {loanApplication?.loanBatch?.gracePeriod} months
                      </h6>
                    </div>
                  </div>

                  {/* Separator */}
                  <div className="pt-3 pb-5">
                    <h6 className="fw-semibold text-gray-600 text-uppercase fs-6 ls-1">
                      Witness Information
                    </h6>
                    <div className="separator mt-2 mb-3"></div>
                  </div>

                  <div className="row mb-7">
                    <div className="col-lg-4">
                      <label className="text-gray-600">Witness full name</label>
                      <h6 className="fw-normal">
                        {loanApplication?.witnessFullName}
                      </h6>
                    </div>
                    <div className="col-lg-4">
                      <label className="text-gray-600">Witness phone</label>
                      <h6 className="fw-normal">
                        {loanApplication?.witnessPhoneNo}
                      </h6>
                    </div>
                    <div className="col-lg-4">
                      <label className="text-gray-600">Witness relation</label>
                      <h6 className="fw-normal">
                        {loanApplication?.witnessRelation}
                      </h6>
                    </div>
                  </div>

                  <div className="row mb-7">
                    <div className="col-lg-4">
                      <label className="text-gray-600">Date of witness</label>
                      <h6 className="fw-normal">
                        {loanApplication?.dateOfWitness
                          ? moment(loanApplication.dateOfWitness).format(
                              "YYYY-MM-DD HH:mm "
                            )
                          : "N/A"}
                      </h6>
                    </div>
                    <div className="col-lg-4">
                      <label className="text-gray-600">Principal Amount</label>
                      <h6 className="fw-normal">
                        {loanApplication?.principalAmount?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
                      </h6>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div className="accordion-item">
            <h2 className="accordion-header" id="kt_accordion_1_header_2">
              <button
                className="accordion-button fs-4 fw-bold collapsed"
                type="button"
                data-bs-toggle="collapse"
                data-bs-target="#kt_accordion_1_body_2"
                aria-expanded="false"
                aria-controls="kt_accordion_1_body_2"
              >
                Loan Items
              </button>
            </h2>
            <div
              id="kt_accordion_1_body_2"
              className="accordion-collapse collapse"
              aria-labelledby="kt_accordion_1_header_2"
              data-bs-parent="#kt_accordion_1"
            >
              <div className="accordion-body">
                <LoanItem
                  loanItems={loanApplication.loanItems}
                  status={loanApplication.statusText}
                />
              </div>
            </div>
          </div>
          <div className="accordion-item">
            <h2 className="accordion-header" id="kt_accordion_1_header_3">
              <button
                className="accordion-button fs-4 fw-bold collapsed"
                type="button"
                data-bs-toggle="collapse"
                data-bs-target="#kt_accordion_1_body_3"
                aria-expanded="false"
                aria-controls="kt_accordion_1_body_3"
              >
                Documents and files
              </button>
            </h2>
            <div
              id="kt_accordion_1_body_3"
              className="accordion-collapse collapse"
              aria-labelledby="kt_accordion_1_header_3"
              data-bs-parent="#kt_accordion_1"
            >
              <div className="timeline-item">
                <div className="timeline-line w-40px"></div>

                <div className="timeline-content mb-10 mt-n1">
                  <div className="overflow-auto pb-5">
                    <div className="d-flex align-items-center border border-dashed border-gray-300 overflow-auto rounded min-w-700px p-5 m-5">
                      {attachments?.attachmentFiles && (
                        <div className="d-flex flex-aligns-center flex-wrap">
                          {attachments.attachmentFiles &&
                            attachments.attachmentFiles.map((item: any) => (
                              <div
                                className="d-flex flex-aligns-center pe-10 pe-lg-20"
                                key={item.fileName}
                                onClick={() => downloadHandler(item.imagePath)}
                              >
                                <img
                                  alt=""
                                  className="w-30px me-3"
                                  src={toAbsoluteUrl(
                                    item.contentType === "application/pdf"
                                      ? "media/svg/files/pdf.svg"
                                      : "media/svg/files/blank-image.svg"
                                  )}
                                />
                                <a href={`${API_URL}${item.imagePath}`}>
                                  <div className="md-1 fw-bold">
                                    {item.fileName}

                                    <div className="text-gray-500">
                                      Size:{" "}
                                      {round(item.fileSize / (8000 * 1000), 3)}{" "}
                                      MB
                                    </div>
                                  </div>
                                </a>
                              </div>
                            ))}
                        </div>
                      )}
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <div className="accordion-item">
            <h2 className="accordion-header" id="kt_accordion_1_header_4">
              <button
                className="accordion-button fs-4 fw-bold collapsed"
                type="button"
                data-bs-toggle="collapse"
                data-bs-target="#kt_accordion_1_body_4"
                aria-expanded="false"
                aria-controls="kt_accordion_1_body_4"
              >
                Status history
              </button>
            </h2>
            <div
              id="kt_accordion_1_body_4"
              className="accordion-collapse collapse"
              aria-labelledby="kt_accordion_1_header_4"
              data-bs-parent="#kt_accordion_1"
            >
              <div className="accordion-body">
                <div className="card mb-5 mb-xl-10">
                  <div className="card-body">
                    <ApplicationHistory id={loanApplication.id} />
                  </div>
                </div>
              </div>
            </div>
          </div>

          <div className="accordion-item">
            <h2 className="accordion-header" id="kt_accordion_1_header_5">
              <button
                className="accordion-button fs-4 fw-bold collapsed"
                type="button"
                data-bs-toggle="collapse"
                data-bs-target="#kt_accordion_1_body_5"
                aria-expanded="false"
                aria-controls="kt_accordion_1_body_5"
              >
                Loan Schedule & Transactions
              </button>
            </h2>
            <div
              id="kt_accordion_1_body_5"
              className="accordion-collapse collapse"
              aria-labelledby="kt_accordion_1_header_5"
              data-bs-parent="#kt_accordion_1"
            >
              <div className="accordion-body">
                {/* <LoanRepaymentHistory loanApplicationId={loanApplication.id} /> */}
                <div className="my-5">
                  <LoanSummary
                    loanData={loanData}
                    info={info}
                    loanApplication={loanApplication}
                  />
                </div>
                {/* {loanApplication.id && <LoanSchedule loanApplicationId={loanApplication.id} schedule={loanData?.schedule} />} */}
                {loanApplication.id && (
                  <Tabs
                    id="tab_loans"
                    activeKey={activeTab}
                    onSelect={(k: string | null) => k && setActiveTab(k)}
                    className="mb-5 custom-tabs"
                  >
                    {/* Tab 1: Schedule */}
                    <Tab
                      eventKey="_schedule"
                      title={
                        <span className="custom-tab-title">EMI Schedule</span>
                      }
                    >
                      <FarmerLoanSchedule
                        loanApplicationId={loanApplication.id}
                       
                        loanApplication={loanApplication}
                      />
                    </Tab>

                    {/* Tab 2: Schedule */}
                    {/* <Tab
                      eventKey="_transactions"
                      title={
                        <span className="custom-tab-title">Transactions</span>
                      }
                    >
                      <LoanInterestSimulator
                        loanApplicationId={loanApplication.id}
                      />
                    </Tab> */}

                    <Tab
                      eventKey="_repayments"
                      title={
                        <span className="custom-tab-title">Repayments</span>
                      }
                    >
                      <LoanRepaymentHistory
                        loanApplicationId={loanApplication.id}
                      />
                    </Tab>

                    <Tab
                      eventKey="_statements"
                      title={
                        <span className="custom-tab-title">Loan Statement</span>
                      }
                    >
                      <LoanStatement
                        loanApplicationId={loanApplication.id}
                        loanNumber={loanApplication.loanNumber}
                         setInfo={setInfo}
                      />
                    </Tab>
                  </Tabs>
                )}
              </div>
            </div>
          </div>
        </div>
      </Content>
      {showItemBox && (
        <LoanApplicationsModal
          loanBatch={loanBatch ?? loanApplication?.loanBatch}
          afterConfirm={afterConfirm}
          isAdd={false}
          loanApplicationDetails={loanApplication}
          loanApplicationItems={loanApplication.loanItems}
        />
      )}
      {showStatusBox && (
        <StatusModal
          afterConfirm={afterConfirm}
          applicationId={loanApplication.Id}
        />
      )}

      {showAuditModal && (
        <AuditListModal
          exModule={"LoanApplications"}
          componentId={loanApplication.id}
          onClose={afterConfirm}
        />
      )}

      {showConfirmBox && (
        <ConfirmBox
          confirmModel={confirmModel}
          afterConfirm={afterConfirm}
          loading={loading}
        />
      )}
    </div>
  );
};

export default LoanAppDetails;
