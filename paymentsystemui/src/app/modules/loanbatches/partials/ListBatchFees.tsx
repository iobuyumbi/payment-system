import React, { useEffect, useState } from "react";
import { KTCard, toAbsoluteUrl } from "../../../../_metronic/helpers"; // Adjust the path as needed
import { getAPIBaseUrl } from "../../../../_metronic/helpers/ApiUtil";
import { round } from "lodash";
import { Content } from "../../../../_metronic/layout/components/content";
import BatchFileUploadModal from "./BatchFileUploadModal";
import LoanBatchService from "../../../../services/LoanBatchService";
import { useParams } from "react-router-dom";
import CustomTable from "../../../../_shared/CustomTable/Index";
import { Error401 } from "../../errors/components/Error401";

const loanBatchService = new LoanBatchService(); // Ensure this service is imported correctly
const ListBatchFees = (props: any) => {
   const { loanBatch } = props;
  const { id } = useParams();
  const API_URL = getAPIBaseUrl();

  const [showItemBox, setShowItemBox] = useState<boolean>(false);
  const [rowData, setRowData] = useState<any[]>([]);
  const [attachments, setAttachments] = useState<any>({});

  const afterConfirm = (value: any) => {
    setShowItemBox(value);
  };

  const [colDefs] = useState<any[]>([
    { field: "feeName", flex: 1, sortable: false, filter: true },
    { field: "feeType", flex: 1, filter: true },
    { field: "value", flex: 1, filter: true },
  ]);

  const bindDetails = async () => {
    const response = await loanBatchService.getSingle(loanBatch.id);
    if (response && response.processingFees?.length > 0) {
      console.log("Loan Product details response", response);
      setRowData(response.processingFees);
    }
  };

  useEffect(() => {
    if (loanBatch?.id) {
      bindDetails();
    }
  }, [loanBatch]);

  return (
    <Content>
      {true ? (
        <KTCard className="shadow">
          {/* begin::Header */}
          <div className="card-header border-0 py-5">
            <h3 className="card-title align-items-start flex-column">
              <span className="card-label fw-bold fs-4 mb-1">Loan Product Fees</span>
            </h3>
          </div>
          {/* end::Header */}
          <CustomTable
            rowData={rowData}
            colDefs={colDefs}
            header=""
          />
        </KTCard>
      ) : (
        <Error401 />
      )}
    </Content>
  );
};


export default ListBatchFees;
