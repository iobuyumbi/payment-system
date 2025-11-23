import React, { useEffect, useState } from "react";
import { KTCard, KTIcon } from "../../../_metronic/helpers";
import { Content } from "../../../_metronic/layout/components/content";
import CustomTable from "../../../_shared/CustomTable/Index";
import PaymentBatchService from "../../../services/PaymentBatchService";
import { PageLink, PageTitle } from "../../../_metronic/layout/core";
import RequestBodyModal from "../payment-processing/partials/RequestBodyModal";
import ResponseBodyModal from "../payment-processing/partials/ResponseBodyModal";
import { PageTitleWrapper } from "../../../_metronic/layout/components/toolbar/page-title";
import moment from "moment";
import saveAs from "file-saver";
import KeyMetrics from "../reports/_shared/KeyMetrics";
import { stat } from "fs";

const paymentService = new PaymentBatchService();

const usersBreadcrumbs: Array<PageLink> = [
  {
    title: "Trasactions",
    path: "/transactions",
    isSeparator: false,
    isActive: false,
  },
  {
    title: "",
    path: "",
    isSeparator: true,
    isActive: false,
  },
];

const TransactionReport = () => {
  const [rowData, setRowData] = useState<any>();
  const [showRequestBody, setShowRequestBody] = useState<any>(false);
  const [showResponseBody, setShowResponseBody] = useState<any>(false);
  const [transactionId, setTransactionId] = useState<any>();
  const [stats, setStats] = useState<any>();
  const bindItems = async () => {
    const data: any = {
      pageNumber: 1,
      pageSize: 10,
    };
    const response = await paymentService.getApiRequestData(data);
    if (response !== null) {
      setRowData(response.apiRequestLogResponseModel);
      setStats(response.logCounterResponseModel)
    }
  };

  const afterConfirm = (res: any) => {
    setShowResponseBody(false);
    setShowRequestBody(false);
  };

  const [pageData, setPageData] = useState<{ title: string; value: number }[]>(
    []
  );

  useEffect(() => {
    if (rowData) {
      setPageData([
        { title: "Total", value: stats.totalTransactions },
        { title: "Failed", value: stats.successful },
        { title: "Completed", value: stats.unsuccessful },
      ]);
    }
  }, [rowData]);
  useEffect(() => {
    bindItems();
  }, []);

  const RequestBodyComponent = (props: any) => {
    if (props.data.apiName === "Payment API" ||props.data.apiName === "Create Contacts API"  ) {
      return (
        <div className="d-flex align-items-center justify-content-center">
          <button
            className="btn btn-sm btn-light my-2 mx-2"
            onClick={() => {
              setShowRequestBody(true);
              setTransactionId(props.data.id);
            }}
          >
            View
          </button>
        </div>
      );
    }
  };

  const downloadData = async (props: any) => {
    try {
      const response = await paymentService.getSingleResponseBody(
        props.transactionId
      );

      // Ensure `response` contains valid data
      if (response) {
        const dataStr =
          typeof response === "string"
            ? response
            : JSON.stringify(response, null, 2);
        const blob = new Blob([dataStr], { type: "application/json" });
        saveAs(blob, "ResponseBody.json");
      } else {
        console.error("No data received from the service.");
      }
    } catch (error) {
      console.error("Failed to download data:", error);
    }
  };

  const ResponseBodyComponent = (props: any) => {
    return (
      <div className="d-flex ">
        <button
          className="btn btn-sm btn-secondary my-2 mx-2"
          onClick={() => {
            setShowResponseBody(true);
            setTransactionId(props.data.id);
          }}
        >
          View
        </button>
        <button
          className="btn btn-sm btn-theme my-2 mx-2"
          onClick={() => {
            downloadData({ transactionId: props.data.id });
          }}
        >
          Download
        </button>
      </div>
    );
  };

  const StatusComponent = (props: any) => {
    return (
      <div className="py-1">
        {props.data.isSuccessful === true && (
          <div className="badge fs-7 badge-light-success  fw-normal">Yes</div>
        )}
        {props.data.isSuccessful === false && (
          <div className="badge fs-7 badge-light-danger  fw-normal">No</div>
        )}
      </div>
    );
  };

  const [colDefs, setColDefs] = useState<any>([
    { field: "apiName", flex: 1 },
    { field: "isSuccessful", flex: 1, cellRenderer: StatusComponent },
    //{ field: "requestTimestamp", flex: 1 },
    {
      field: "requestTimestamp",
      flex: 1,
      valueFormatter: function (params: any) {
        return moment(params.value).format("DD-MM-yyyy hh:mm");
      },
    },
    //{ field: "responseTimestamp", flex: 1 },
    {
      field: "responseTimestamp",
      flex: 1,
      valueFormatter: function (params: any) {
        return moment(params.value).format("DD-MM-yyyy hh:mm");
      },
    },
    { field: "errorMessage", flex: 1 },
    { field: "requestBody", flex: 1, cellRenderer: RequestBodyComponent },
    { field: "responseBody", flex: 2, cellRenderer: ResponseBodyComponent },
  ]);

  return (
    <Content>
      <PageTitleWrapper />
      <PageTitle breadcrumbs={usersBreadcrumbs}>Transactions</PageTitle>
      <div className="pt-2"></div>
      <KeyMetrics keyMetrics={pageData} className="shadow my-3" />
      <KTCard>
        <CustomTable
          rowData={rowData}
          colDefs={colDefs}
          header=""
          addBtnText={""}
          importBtnText={""}
          addBtnLink={""}
          showImportBtn={false}
        />
      </KTCard>

      {showRequestBody && (
        <RequestBodyModal
          transactionId={transactionId}
          afterConfirm={afterConfirm}
        />
      )}
      {showResponseBody && (
        <ResponseBodyModal
          transactionId={transactionId}
          afterConfirm={afterConfirm}
        />
      )}
    </Content>
  );
};

export default TransactionReport;
