import { useEffect, useState } from "react";
import { KTCard, KTCardBody } from "../../../../_metronic/helpers";
import { Content } from "../../../../_metronic/layout/components/content";
import CustomTable from "../../../../_shared/CustomTable/Index";
import { useNavigate, useParams } from "react-router-dom";
import ExportService from "../../../../services/ExportService";
import { toast } from "react-toastify";
import PaymentBatchService from "../../../../services/PaymentBatchService";
import { Tab, Tabs } from "react-bootstrap";

const exportService = new ExportService();
const paymentBatchService = new PaymentBatchService();

const KycResult = () => {
  const navigate = useNavigate();

  const [rowData, setRowData] = useState<any[]>([]);
  const { id, fileName } = useParams();
  const [activeTab, setActiveTab] = useState<string>("Pending");
  const [completedPayments, setCompletedPayments] = useState<any>([]);
  const [pendingPayments, setPendingPayments] = useState<any>([]);
  const [loading, setLoading] = useState(false)


  const StatusComponent = (props: any) => {
    return (
      <div className="py-1">
        {props.data.farmer.isFarmerVerified === true && (
          <div className="badge fs-7 badge-light-success  fw-normal">Yes</div>
        )}
        {props.data.farmer.isFarmerVerified === false && (
          <div className="badge fs-7 badge-light-danger  fw-normal">No</div>
        )}
      </div>
    );
  };

  const farmerComponent = (props: any) => {
    const farmer = props.data.farmer;
    const fields = props.fields || []; // Fields to display

    if (!farmer) {
      return <div>No Farmer Data</div>;
    }

    return (
      <div className="d-flex flex-column">
        {fields.map((field: string) => (
          <div key={field}>{farmer[field] || "Not Available"}</div>
        ))}
      </div>
    );
  };

  const [colDefs, setColDefs] = useState<any>([
    {
      field: "fullName",
      flex: 1,
      cellRenderer: farmerComponent,
      cellRendererParams: {
        fields: ["fullName"],
      },
    },
    {
      field: "mobile",
      flex: 1,

      cellRenderer: farmerComponent,
      cellRendererParams: {
        fields: ["mobile"],
      },
    },
    { field: "systemId", flex: 1, headerName: "System Id" },
    {
      field: "farmerPayableEarningsLc", headerName: "Farmer Payable Earnings(LC)",
      valueFormatter: (params: any) => {
        return parseFloat(params.value).toFixed(2);
      }, flex: 1
    },
    {
      field: "isFarmerVerified",
      headerName: "Is Mobile Verified",
      flex: 1,
      cellRenderer: StatusComponent,
    },
  ]);

  const bindImportDetail = async () => {
    const model = { statusId: 1, batchId: id };
    const response = await exportService.getDeductiblePayments(model);
    const completed = response.filter((item) => item.isPaymentComplete);
    const pending = response.filter((item) => !item.isPaymentComplete);

    setCompletedPayments(completed);
    setPendingPayments(pending);

    setRowData(response);

  };

  const handleConfirmPaymentPartner = async () => {
    setLoading(true);
    // Logic to confirm payment with Payment Partner
    const searchParams = {
      batchId: id,
      statusId: 1,
      isFarmerValid: true
    };

    const response = await paymentBatchService.payAllBatchAsync(searchParams);
    setLoading(false);
    toast.info("Payment processing is in progress");
    navigate(`/payment-processing/transactions`);
  };

  useEffect(() => {
    bindImportDetail();
  }, []);

  const [isButtonEnabled, setIsButtonEnabled] = useState(false);

  useEffect(() => {
    setIsButtonEnabled(rowData.some(item => item.farmer.isFarmerVerified));
  }, [rowData]);

  return (
    <Content>
      {/* <div className="rounded  border border-dashed px-6 bg-light-warning border-warning mb-3">
        <span className="text-gray-700 fs-4 d-block">Showing mobile verfication results</span>
      </div> */}
      <KTCard className='shadow mt-10'>
        <KTCardBody>
          <Tabs
            id="Payments tab"
            activeKey={activeTab}
            onSelect={(k: string | null) => k && setActiveTab(k)}
            className="mb-5 custom-tabs"
          >
            {/* Tab 1: Table View */}
            <Tab eventKey="Pending" title={<span className="custom-tab-title">Pending Payments</span>}>
              <div className="mb-5">
                <div className="card-header d-flex justify-content-end align-items-end border-0">
                  <div className="d-flex flex-column align-items-end">
                    <button
                      className="btn btn-theme w-120px"
                      onClick={handleConfirmPaymentPartner}
                      disabled={!isButtonEnabled}
                    >

                      {!loading && <span className='indicator-label'>Confirm and Pay</span>}
                      {loading && (
                        <span className='indicator-progress' style={{ display: 'block' }}>
                          Please wait...
                          <span className='spinner-border spinner-border-sm align-middle ms-2'></span>
                        </span>
                      )}
                    </button>
                    <div className="my-3">
                      <div className="d-flex align-items-top my-3 text-gray-700">
                        <i className="fas fa-info-circle m-2"></i>
                        {
                          isButtonEnabled ? "Batch Processing Enabled"
                            : "Payment can be sent for processing is there is atleast one verified farmer"
                        }
                      </div>
                    </div>
                  </div>

                </div>
                <CustomTable
                  rowData={pendingPayments}
                  colDefs={colDefs}
                  header="Pay"
                  addBtnText=""
                  addBtnLink=""
                  className="mt-0"
                />
              </div>
            </Tab>

            {/* Tab 2: Date-Based Data */}
            <Tab eventKey="Completed" title={<span className="custom-tab-title">Completed Payments</span>}
              className="mb-10">
              <div className="mb-5">
                <CustomTable
                  rowData={completedPayments}
                  colDefs={colDefs}
                  header="Employee Incomes"
                  addBtnText=""
                  addBtnLink=""
                />
              </div>
            </Tab>
          </Tabs>
        </KTCardBody>
      </KTCard>
    </Content>
  );
};

export default KycResult;
