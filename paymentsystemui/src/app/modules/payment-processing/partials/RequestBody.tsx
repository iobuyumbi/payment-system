import React, { useEffect, useState } from "react";
import { KTCard, KTIcon } from "../../../../_metronic/helpers";
import { Content } from "../../../../_metronic/layout/components/content";
import CustomTable from "../../../../_shared/CustomTable/Index";
import PaymentBatchService from "../../../../services/PaymentBatchService";
import { PageLink } from "../../../../_metronic/layout/core";

// import { JsonViewer } from "@textea/json-viewer";
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

const RequestBodyPage = (props: any) => {
  const { transactionId } = props;
  const [rowData, setRowData] = useState<any>();

  const bindItems = async () => {
    const response = await paymentService.getSingleRequestBody(transactionId);

    setRowData(response);
  };

  useEffect(() => {
    bindItems();
  }, []);

  return (
    <Content>
      <KTCard>
        <div>
          {rowData ? (
            // <JsonViewer
            //   value={
            //     typeof rowData === "string" ? JSON.parse(rowData) : rowData
            //   }
            //   rootName={false} // hides the root key name
            //   defaultInspectDepth={Infinity} // fully expanded
            //   displayDataTypes={false}
            // />
            <div className="p-5">
              <h3 className="mb-4">Request Body</h3>
              <pre>
                <code>{JSON.stringify(rowData, null, 2)}</code>
              </pre>
              </div>
          ) : (
            <p>Loading...</p>
          )}
        </div>
      </KTCard>
    </Content>
  );
};

export default RequestBodyPage;
