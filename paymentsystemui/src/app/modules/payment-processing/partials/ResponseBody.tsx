import React, { useEffect, useState } from 'react'
import { KTCard, KTIcon } from '../../../../_metronic/helpers'
import { Content } from '../../../../_metronic/layout/components/content'
import CustomTable from '../../../../_shared/CustomTable/Index'
import PaymentBatchService from '../../../../services/PaymentBatchService'
import { PageLink } from '../../../../_metronic/layout/core'

import saveAs from 'file-saver'
import { parse } from 'path'
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

const ReponseBodyPage = (props: any) => {
    const { transactionId} = props;
  const [rowData, setRowData] = useState<any>();
const [parsedData, setParsedData] = useState<any>();
  const bindData = async () => {
      const response = await paymentService.getSingleResponseBody(transactionId);
     
      setRowData(response);
    }
  
    useEffect(() => {
        bindData();
    }, []);
    
   

  
      useEffect(() => {
        try {
          setParsedData (typeof rowData === "string" ? JSON.parse(rowData) : rowData);
        } catch (error) {
          console.error("JSON Parsing Error:");
        
        }
    }, [rowData]);

  
  
  
  return (
    <Content>
      <KTCard>
       
        <div>
          {rowData ? (
            <>
          {/* <JsonViewer
                       value={
                         typeof rowData === "string" ? JSON.parse(rowData) : rowData
                       }
                       rootName={false} // hides the root key name
                       defaultInspectDepth={Infinity} // fully expanded
                       displayDataTypes={false}
                     /> */}
                     
            </>
          ) : (
            <p>Loading...</p>
          )}
        </div>
      </KTCard>
    </Content>
  )
}

export default ReponseBodyPage